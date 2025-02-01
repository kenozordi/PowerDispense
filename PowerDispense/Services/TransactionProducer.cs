using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PowerDispense.Interfaces.IFactories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models.Config;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;
using StackExchange.Redis;

namespace PowerDispense.Services
{
    public class TransactionProducer : ITransactionProducer
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly IPowerProviderFactory _powerProviderFactory;
        public TransactionProducer(IOptions<ConnectionStrings> connectionStrings, 
            IPowerProviderFactory powerProviderFactory)
        {
            _connectionStrings = connectionStrings.Value;
            _powerProviderFactory = powerProviderFactory;
        }
        public async Task<bool> PushTransaction(PowerRequest powerRequest, 
            PowerProvider powerProvider)
        {
            var redisConnectionString = _connectionStrings.Redis;
            var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
            var db = muxer.GetDatabase();

            var cacheStreamKey = _powerProviderFactory.GetProviderStreamKey(powerProvider);
            var cacheStreamEntry = new[]
            {
                new NameValueEntry(
                    nameof(powerRequest), 
                    JsonConvert.SerializeObject(powerRequest))
            };
            var streamId = await db.StreamAddAsync(cacheStreamKey, cacheStreamEntry);
            return string.IsNullOrEmpty(streamId.ToString()) ? false : true;
        }
    }
}
