using PowerDispense.Models.DTO;
using PowerDispense.Models;

namespace PowerDispense.Interfaces.IRepositories
{
    public interface IPowerRepo
    {
        Task<bool>? AddMeter(MeterInfo meterInfo);
        Task<MeterInfo>? GetMeter(PowerRequest powerRequest);
    }
}
