using PowerDispense.DAO;
using PowerDispense.Interfaces.IServices;

namespace PowerDispense.Services.Workers
{
    public class TransactionWorker : BackgroundService, ITransactionWorker
    {
        private CancellationTokenSource _cancelTokenSource = new();
        private readonly ITransactionConsumer _transactionConsumer;
        private Task _backgroundServiceState = Task.CompletedTask;

        public TransactionWorker(ITransactionConsumer transactionConsumer)
        {
            _transactionConsumer = transactionConsumer;
        }
        public (bool, string) Start()
        {
            _cancelTokenSource = new CancellationTokenSource();
            ExecuteAsync(_cancelTokenSource.Token);
            return (true, "Service running");
        }

        public async Task<(bool, string)> Stop()
        {
            await _transactionConsumer.UnSubscribe();
            return (true, "Worker service stopped");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _backgroundServiceState = _transactionConsumer.Subscribe();

        }
    }
}
