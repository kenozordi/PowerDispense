using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces.IServices
{
    public interface ITransactionProducer
    {
        /// <summary>
        /// Uses Redis Streams
        /// </summary>
        /// <param name="powerRequest"></param>
        /// <param name="powerProvider"></param>
        /// <returns></returns>
        Task<bool> PushTransaction(PowerRequest powerRequest, PowerProvider powerProvider);

        /// <summary>
        /// Uses Redis Pub/Sub
        /// </summary>
        /// <param name="powerRequest"></param>
        /// <param name="powerProvider"></param>
        /// <returns></returns>
        Task<bool> PublishTransaction(PowerRequest powerRequest, PowerProvider powerProvider);
    }
}
