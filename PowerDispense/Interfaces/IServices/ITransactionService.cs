using PowerDispense.Models;
using PowerDispense.Models.DTO;

namespace PowerDispense.Interfaces.IServices
{
    public interface ITransactionService
    {
        Task<bool> AddTransactionLog(PowerTransaction powerTransaction);
        Task<PowerTransaction> PurchasePower(PowerRequest powerRequest);
    }
}
