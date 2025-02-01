using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces.IServices
{
    public interface ITransactionConsumer
    {
        Task<List<PowerRequest>> ProcessTransactions();
    }
}
