namespace PowerDispense.Interfaces.IServices
{
    public interface ITransactionWorker
    {
        (bool, string) Start();
        Task<(bool, string)> Stop();
    }
}
