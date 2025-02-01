using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces.IServices
{
    public interface ITransactionProducer
    {
        Task<bool> PushTransaction(PowerRequest powerRequest, PowerProvider powerProvider);
    }
}
