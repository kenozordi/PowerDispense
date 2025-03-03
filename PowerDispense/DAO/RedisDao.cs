using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PowerDispense.Models.Config;
using PowerDispense.Models.Constants;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;
using Redis.OM;
using StackExchange.Redis;

namespace PowerDispense.DAO
{
    public class RedisDao
    {
        private readonly ConnectionStrings _connectionStrings;

        public ConnectionMultiplexer muxer;
        public IDatabase db;
        public RedisConnectionProvider redisProvider;
        public List<string> transactionChannels;

        public RedisDao(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            muxer = ConnectionMultiplexer.Connect(_connectionStrings.Redis);
            var redisConnection = new RedisConnectionProvider(muxer);
            db = muxer.GetDatabase();
        }

    }
}
