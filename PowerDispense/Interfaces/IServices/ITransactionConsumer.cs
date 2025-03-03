using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces.IServices
{
    public interface ITransactionConsumer
    {
        /// <summary>
        /// Using Redis Streams
        /// </summary>
        /// <returns></returns>
        Task<List<PowerRequest>> ProcessTransactions();

        /// <summary>
        /// Using Redis Queue Pub/Sub
        /// </summary>
        /// <returns></returns>
        Task Subscribe();

        /// <summary>
        /// Using Redis Queue Pub/Sub
        /// </summary>
        /// <returns></returns>
        Task UnSubscribe();
    }
}
