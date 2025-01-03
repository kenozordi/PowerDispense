using PowerDispense.Interfaces.IServices;
using PowerDispense.Models.Config;
using PowerDispense.Models.Constants;
using PowerDispense.Models.Enum;
using StackExchange.Redis;
using Microsoft.Extensions.Options;

namespace PowerDispense.Services
{
    public class PowerProviderHealthService : IPowerProviderHealth
    {
        private ConfigService _configService;
        public PowerProviderHealthService(ConfigService configService)
        {
            _configService = configService;
        }
        public async Task<PowerProviderStatus>? GetProviderHealthStatus(PowerProvider powerProvider)
        {
            var redisConnectionString = _configService.ConnectionStrings.Redis;
            var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
            var db = muxer.GetDatabase();

            return db.SetContains(CacheKey.POWER_PROVIDER_UNSTABLE, powerProvider.ToString())
                ? PowerProviderStatus.Unstable
                : PowerProviderStatus.Stable;
        }
        public void SetProviderHealthStatus(PowerProvider powerProvider, PowerProviderStatus providerStatus)
        {
            var redisConnectionString = _configService.ConnectionStrings.Redis;
            var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
            var db = muxer.GetDatabase();

            if (providerStatus == PowerProviderStatus.Stable)
            {
                db.SetRemove(CacheKey.POWER_PROVIDER_UNSTABLE, powerProvider.ToString());
                db.SetAdd(CacheKey.POWER_PROVIDER_STABLE, powerProvider.ToString());
            }
            else
            {
                db.SetRemove(CacheKey.POWER_PROVIDER_STABLE, powerProvider.ToString());
                db.SetAdd(CacheKey.POWER_PROVIDER_UNSTABLE, powerProvider.ToString());
            }
        }
    }
}
