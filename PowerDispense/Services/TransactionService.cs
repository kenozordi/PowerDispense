using Microsoft.Extensions.Options;
using PowerDispense.DAO;
using PowerDispense.Interfaces.IFactories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models;
using PowerDispense.Models.Config;
using PowerDispense.Models.DTO;
using Redis.OM;
using StackExchange.Redis;

namespace PowerDispense.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly ILogger<TransactionService> _logger;
        private readonly IConnectionMultiplexer _redisDao;
        public IPowerServiceFactory _powerServiceFactory;

        public TransactionService(IOptions<ConnectionStrings> connectionStrings,
            ILogger<TransactionService> logger, IConnectionMultiplexer redisDao,
            IPowerServiceFactory powerServiceFactory)
        {
            _connectionStrings = connectionStrings.Value;
            _logger = logger;
            _redisDao = redisDao;
            _powerServiceFactory = powerServiceFactory;
        }

        public async Task<PowerTransaction> PurchasePower(PowerRequest powerRequest)
        {
            var powerService = _powerServiceFactory.GetPowerService(powerRequest.MeterProvider);
            var meterInfo = await powerService.ValidateMeter(powerRequest);

            if (meterInfo is not null)
            {
                var powerTransaction = await powerService.Purchase(powerRequest);
                await AddTransactionLog(powerTransaction);

                return powerTransaction;
            }

            return default;
        }
        public async Task<bool> AddTransactionLog(PowerTransaction powerTransaction)
        {
            var redisProvider = new RedisConnectionProvider(_redisDao);
            var powerTransactions = redisProvider.RedisCollection<PowerTransaction>();
            var key = powerTransactions.Insert(powerTransaction);
            return key is not null ? true : false;
        }
    }
}
