using PowerDispense.Models.DTO;

namespace PowerDispense.Interfaces.IServices
{
    public interface IRaffleDrawService
    {
        Task AddEntry(PowerRequest powerRequest);
        Task<List<string>> GetAllEntry();
    }
}
