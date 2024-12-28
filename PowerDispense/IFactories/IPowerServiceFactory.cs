using PowerDispense.Interfaces;
using PowerDispense.Models;

namespace PowerDispense.IFactories
{
    public interface IPowerServiceFactory
    {
        /// <summary>
        /// Get the Power Service by Meter Provider
        /// </summary>
        /// <param name="meterProvider"></param>
        /// <returns></returns>
        IPowerService GetPowerService(MeterInfo.MeterProviders meterProvider);

        /// <summary>
        /// Get the Service to Borrow Power by Meter Provider
        /// </summary>
        /// <param name="meterProvider"></param>
        /// <returns></returns>
        ICanBorrowPower GetBorrowPowerService(MeterInfo.MeterProviders meterProvider);
    }
}
