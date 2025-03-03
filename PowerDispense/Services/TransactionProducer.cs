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
    public class TransactionProducer : ITransactionProducer
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly IPowerProviderFactory _powerProviderFactory;
        private readonly IConnectionMultiplexer _redisCache;
        public TransactionProducer(IOptions<ConnectionStrings> connectionStrings, 
            IPowerProviderFactory powerProviderFactory, IConnectionMultiplexer redisCache)
        {
            _connectionStrings = connectionStrings.Value;
            _powerProviderFactory = powerProviderFactory;
            _redisCache = redisCache;
        }
        public async Task<bool> PushTransaction(PowerRequest powerRequest, 
            PowerProvider powerProvider)
        {
            var db = _redisCache.GetDatabase();

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

        public async Task<bool> PublishTransaction(PowerRequest powerRequest,
            PowerProvider powerProvider)
        {
            var db = _redisCache.GetDatabase();

            var clients = await db.PublishAsync(
                $"{CacheKey.CHANNEL_TRANS}:{powerProvider.ToString()}", 
                JsonConvert.SerializeObject(powerRequest));
            return clients < 1 ? false : true;
        }
    }
}
