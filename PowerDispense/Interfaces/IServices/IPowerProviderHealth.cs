using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces.IServices
{
    public interface IPowerProviderHealth
    {
        Task<PowerProviderStatus>? GetProviderHealthStatus(PowerProvider powerProvider);
        void SetProviderHealthStatus(PowerProvider powerProvider, PowerProviderStatus providerStatus);
    }
}
