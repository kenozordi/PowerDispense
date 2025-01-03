using PowerDispense.Interfaces;
using PowerDispense.Models.Enum;

namespace PowerDispense.IFactories
{
    public interface IPowerServiceFactory
    {
        /// <summary>
        /// Get the Power Service by Meter Provider
        /// </summary>
        /// <param name="meterProvider"></param>
        /// <returns></returns>
        IPowerService GetPowerService(PowerProvider meterProvider);

        /// <summary>
        /// Get the Service to Borrow Power by Meter Provider
        /// </summary>
        /// <param name="meterProvider"></param>
        /// <returns></returns>
        ICanBorrowPower GetBorrowPowerService(PowerProvider meterProvider);
    }
}
