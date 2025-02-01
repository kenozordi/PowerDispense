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
            if (_backgroundServiceState != null && !_backgroundServiceState.IsCompleted)
            {
                return (false, "Service already running");
            }

            _cancelTokenSource = new CancellationTokenSource();
            ExecuteAsync(_cancelTokenSource.Token);
            return (true, "Service running");
        }

        public (bool, string) Stop()
        {
            if(_backgroundServiceState == null || _backgroundServiceState.IsCompleted || _cancelTokenSource == null)
            {
                return (false, "Worker service is not running");
            }

            _cancelTokenSource.Cancel();
            return (true, "Worker service stopped");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _backgroundServiceState = Task.Run(async() =>
            {
                int delayInSeconds = 5;
                using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _cancelTokenSource.Token))
                {
                    while (!linkedTokenSource.Token.IsCancellationRequested)
                    {
                        // Process transactions
                        var processedTransactions = await _transactionConsumer.ProcessTransactions();

                        // Delay before processing next task
                        await Task.Delay(delayInSeconds * 1000);
                    }
                }
            });
            //_backgroundServiceState = Task.CompletedTask;
        }
    }
}
