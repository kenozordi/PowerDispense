using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PowerDispense.DAO;
using PowerDispense.Interfaces.IFactories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.Config;
using PowerDispense.Models.Constants;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;
using Redis.OM;
using StackExchange.Redis;
using System.Linq;
using System.Transactions;

namespace PowerDispense.Services
{
    public class TransactionConsumer : ITransactionConsumer
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly ILogger<TransactionConsumer> _logger;
        private readonly IConnectionMultiplexer _redisDao;
        private List<string> subscribedChannels = new();
        public TransactionConsumer(IOptions<ConnectionStrings> connectionStrings,
            ILogger<TransactionConsumer> logger, IConnectionMultiplexer redisDao)
        {
            _connectionStrings = connectionStrings.Value;
            _logger = logger;
            _redisDao = redisDao;
        }

        public async Task<List<PowerRequest>> ProcessTransactions()
        {
            var processedTransactions = new List<PowerRequest>();

            var redisConnectionString = _connectionStrings.Redis;
            var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
            var db = muxer.GetDatabase();

            var serviceProviderStreams = new[] 
            { CacheKey.STREAM_AEDC, CacheKey.STREAM_EKEDC };
            var currentStreamPositions = new Dictionary<string, StreamPosition>();

            // Set stream Positions to begining of the stream (0-0)
            foreach (var servicePoviderStreamKey in serviceProviderStreams)
            {
                currentStreamPositions.Add(
                    servicePoviderStreamKey, 
                    new StreamPosition(servicePoviderStreamKey, "0-0"));
            }

            var readResult = await db.StreamReadAsync(
                currentStreamPositions.Values.ToArray(), 
                countPerStream: 5);

            var streamIdsProcessed = new List<RedisValue>();
            foreach (var stream in readResult)
            {
                foreach (var entry in stream.Entries)
                {
                    //> Process transaction
                    //> Call to service-provider/core-system for value

                    var transaction = entry.Values.First().Value.ToString();
                    processedTransactions.Add(
                        JsonConvert.DeserializeObject<PowerRequest>(transaction));
                    streamIdsProcessed.Add(entry.Id);
                }

                //> Purge transactions from stream
                var streamsDeleted = db.StreamDeleteAsync(
                    stream.Key, 
                    streamIdsProcessed.ToArray());
                streamIdsProcessed = new List<RedisValue>();
            }
            return processedTransactions;
        }

        public async Task SeedTransactionsIndex()
        {
            var redisProvider = new RedisConnectionProvider(_redisDao);
            await redisProvider.Connection.CreateIndexAsync(typeof(PowerTransaction));
        }
        public async Task<bool> UpdateTransactionLog(PowerTransaction powerTransaction)
        {
            var redisProvider = new RedisConnectionProvider(_redisDao);
            var powerTransactions = redisProvider.RedisCollection<PowerTransaction>();
            var key = powerTransactions.Insert(powerTransaction);
            return true;
        }
        public async Task Subscribe()
        {
            await SeedTransactionsIndex();
            var subscriber = _redisDao.GetSubscriber();

            string[] providers = { PowerProvider.EKEDC.ToString(), PowerProvider.AEDC.ToString() };
            foreach (var provider in providers)
            {
                var providerChannelKey = $"{CacheKey.CHANNEL_TRANS}:{provider}";
                if (!subscribedChannels.Contains(providerChannelKey))
                {
                    var channel = await subscriber.SubscribeAsync(providerChannelKey);
                    channel.OnMessage(msg =>
                    {
                        MeterInfoSampleData.processedTransactions.Add(JsonConvert.DeserializeObject<PowerRequest>(msg.Message));
                    });
                    subscribedChannels.Add(providerChannelKey);
                }
            }
        }
        public async Task UnSubscribe()
        {
            var subscriber = _redisDao.GetSubscriber();

            string[] providers = { PowerProvider.EKEDC.ToString(), PowerProvider.AEDC.ToString() };
            foreach (var provider in providers)
            {
                var providerChannelKey = $"{CacheKey.CHANNEL_TRANS}:{provider}";
                if (subscribedChannels.Contains(providerChannelKey))
                {
                    await subscriber.UnsubscribeAsync(providerChannelKey);
                    subscribedChannels.Remove(providerChannelKey);
                }
            }
        }
    }
}
