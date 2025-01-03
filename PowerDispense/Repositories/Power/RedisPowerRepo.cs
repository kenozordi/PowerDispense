using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PowerDispense.Interfaces.IRepositories;
using PowerDispense.Models;
using PowerDispense.Models.Config;
using PowerDispense.Models.Constants;
using PowerDispense.Models.DTO;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text;

namespace PowerDispense.Repositories.Power
{
    public class RedisPowerRepo : IPowerRepo
    {
        private readonly ConnectionStrings _connectionStrings;
        public RedisPowerRepo(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }
        public async Task<bool>? AddMeter(MeterInfo meterInfo)
        {
            try
            {
                var redisConnectionString = _connectionStrings.Redis;
                var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
                var db = muxer.GetDatabase();

                var meterKey = new RedisKey($"{CacheKey.Meter}:{meterInfo.PowerProvider}:{meterInfo.MeterNo}");
                return db.StringSet(
                    meterKey,
                    JsonConvert.SerializeObject(meterInfo),
                    expiry: TimeSpan.FromSeconds(15));
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<MeterInfo>? GetMeter(PowerRequest powerRequest)
        {
            try
            {
                var redisConnectionString = _connectionStrings.Redis;
                var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
                var db = muxer.GetDatabase();

                var meterKey = new RedisKey($"{CacheKey.Meter}:{powerRequest.MeterProvider}:{powerRequest.MeterNo}");
                var meterInfo = db.StringGet(meterKey);
                return JsonConvert.DeserializeObject<MeterInfo>(meterInfo);

                #region PipelineTest
                var result = new StringBuilder();
                var stopwatch = Stopwatch.StartNew();

                // serially/unpipelined
                for (var i = 0; i < 1000; i++)
                {
                    await db.PingAsync();
                }
                result.AppendLine($"1000 un-pipelined commands took: {stopwatch.ElapsedMilliseconds}ms to execute");

                // implicit pipelined
                var pingTasks = new List<Task<TimeSpan>>();
                stopwatch.Restart();
                for (var i = 0; i < 1000; i++)
                {
                    pingTasks.Add(db.PingAsync());
                }
                await Task.WhenAll(pingTasks);
                result.AppendLine($"1000 implicit pipelined commands took: {stopwatch.ElapsedMilliseconds}ms to execute");

                // explicit pipelined
                pingTasks.Clear();
                var batch = db.CreateBatch();
                stopwatch.Restart();
                for (var i = 0; i < 1000; i++)
                {
                    pingTasks.Add(batch.PingAsync());
                }
                batch.Execute();
                await Task.WhenAll(pingTasks);
                result.AppendLine($"1000 explicit pipelined commands took: {stopwatch.ElapsedMilliseconds}ms to execute");
                var finalResult = result.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
