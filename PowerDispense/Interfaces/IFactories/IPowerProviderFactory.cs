using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces.IFactories
{
    public interface IPowerProviderFactory
    {
        string GetProviderStreamKey(PowerProvider powerProvider);
    }
}
