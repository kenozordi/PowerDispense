using Microsoft.Extensions.Options;
using PowerDispense.Interfaces.IServices;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.Config;
using PowerDispense.Models.Constants;
using StackExchange.Redis;
using System.Reflection;

namespace PowerDispense.Services
{
    public class MeterService : IMeterService
    {
        private readonly ConnectionStrings _connectionStrings;

        public MeterService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }
        public List<MeterInfo> AllMeterInfo()
        {
            LoadMetersFromFile();
            return MeterInfoSampleData.meterInfos;
        }

        public bool LoadMetersFromFile()
        {
            var metersFromFile =  MeterInfoSampleData.meterInfos;

            var redisConnectionString = _connectionStrings.Redis;
            var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
            var db = muxer.GetDatabase();
            foreach (var meter in metersFromFile)
            {
                var meterKey = new RedisKey($"{CacheKey.Meter}:{meter.MeterProvider}:{meter.MeterNo}");
                db.HashSet(meterKey, new HashEntry[]
                {
                    new (nameof(meter.Id), meter.Id),
                    new (nameof(meter.CustomerName), meter.CustomerName)
                });
                
            }
            return true;
        }
        public string GetMeterName(string meterNumber, string meterProvider)
        {
            var dummyMeterInfo = new MeterInfo();
            var redisConnectionString = _connectionStrings.Redis;
            var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
            var db = muxer.GetDatabase();

            var meterKey = new RedisKey($"{CacheKey.Meter}:{meterProvider}:{meterNumber}");
            return db.HashGet(meterKey, nameof(dummyMeterInfo.CustomerName));
        }
    }
}
