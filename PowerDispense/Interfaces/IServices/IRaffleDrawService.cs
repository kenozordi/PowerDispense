using PowerDispense.Models.DTO;

namespace PowerDispense.Interfaces.IServices
{
    public interface IRaffleDrawService
    {
        void AddEntry(PowerRequest powerRequest);
        Task<List<string>> GetAllEntry();
    }
}
