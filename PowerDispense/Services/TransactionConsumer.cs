using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PowerDispense.Interfaces.IFactories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models.Config;
using PowerDispense.Models.Constants;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;
using StackExchange.Redis;

namespace PowerDispense.Services
{
    public class TransactionConsumer : ITransactionConsumer
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly IPowerProviderFactory _powerProviderFactory;
        private readonly ILogger<TransactionConsumer> _logger;
        private CancellationTokenSource _cancellationTokenSource;
        public TransactionConsumer(IOptions<ConnectionStrings> connectionStrings, IPowerProviderFactory powerProviderFactory,
            ILogger<TransactionConsumer> logger)
        {
            _connectionStrings = connectionStrings.Value;
            _powerProviderFactory = powerProviderFactory;
            _logger = logger;
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

        public async Task<List<PowerRequest>> FetchTransactions(int transPerProviderBatch, 
            CancellationTokenSource cancellationTokenSource)
        {
            int delayInSeconds = 3;
            var powerRequests = new List<PowerRequest>();
            string transConsumerGroup = CacheKey.CONSUMER_GROUP;

            var redisConnectionString = _connectionStrings.Redis;
            var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
            var db = muxer.GetDatabase();
            var providerStreamKeys = new[] { CacheKey.STREAM_AEDC, CacheKey.STREAM_EKEDC };
            var providerStreamsPosition = new Dictionary<string, StreamPosition>();

            // Set Default Stream Positions
            foreach (var streamKey in providerStreamKeys)
            {
                providerStreamsPosition.Add(streamKey, new StreamPosition(streamKey, ">"));
            }

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                foreach (var cacheStreamKey in providerStreamKeys)
                {
                    var readResult = await db.StreamReadGroupAsync([providerStreamsPosition[cacheStreamKey]], transConsumerGroup, cacheStreamKey, countPerStream: transPerProviderBatch);
                    foreach (var stream in readResult)
                    {
                        foreach (var entry in stream.Entries)
                        {
                            var powerRequest = JsonConvert.DeserializeObject<PowerRequest>(entry.Values.First().Value.ToString());
                            providerStreamsPosition[cacheStreamKey] = new StreamPosition(cacheStreamKey, entry.Id);
                            powerRequests.Add(powerRequest);
                            await db.StreamAcknowledgeAsync(cacheStreamKey,transConsumerGroup, entry.Id);
                        }
                    }
                }
                Task.Delay(delayInSeconds * 1000, cancellationTokenSource.Token).Wait();
            }
            
            return powerRequests;
        }

    }
}
