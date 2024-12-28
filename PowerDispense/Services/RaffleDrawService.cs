using Microsoft.Extensions.Options;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models.Config;
using PowerDispense.Models.Constants;
using PowerDispense.Models.DTO;
using StackExchange.Redis;

namespace PowerDispense.Services
{
    public class RaffleDrawService : IRaffleDrawService
    {
        private readonly ConnectionStrings _connectionStrings;

        public RaffleDrawService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }
        public void AddEntry(PowerRequest powerRequest)
        {
            try
            {
                var redisConnectionString = _connectionStrings.Redis;
                var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
                var db = muxer.GetDatabase();

                var raffleJustStarted = db.StringSet(
                    key: $"{CacheKey.Raffle}:{CacheKey.Status}", 
                    value: "1", 
                    expiry: TimeSpan.FromHours(CacheKey.RaffleExpiryInHours), 
                    when: When.NotExists);

                db.ListRightPush(
                    key: $"{CacheKey.Raffle}",
                    value: $"{CacheKey.Meter}:{powerRequest.MeterProvider}:{powerRequest.MeterNo}");
                if (raffleJustStarted)
                {
                    db.KeyExpireAsync($"{CacheKey.Raffle}", TimeSpan.FromHours(CacheKey.RaffleExpiryInHours));
                }
            }
            catch (Exception ex)
            {

            }
            

        }

        public async Task<List<string>> GetAllEntry()
        {
            try
            {
                var muxer = ConnectionMultiplexer.Connect(_connectionStrings.Redis);
                var db = muxer.GetDatabase();

                var raffleDraw = db.ListRange($"{CacheKey.Raffle}");
                var entries = new List<string>();
                foreach (var raffle in raffleDraw)
                {
                    //entries.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(raffle));
                    entries.Add(raffle);
                }
                return entries;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}
