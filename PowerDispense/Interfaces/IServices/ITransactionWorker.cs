namespace PowerDispense.Interfaces.IServices
{
    public interface ITransactionWorker
    {
        (bool, string) Start();
        (bool, string) Stop();
    }
}
