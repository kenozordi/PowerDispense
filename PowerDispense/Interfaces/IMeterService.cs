using PowerDispense.Models;

namespace PowerDispense.Interfaces
{
    public interface IMeterService
    {
        /// <summary>
        /// Get All Meters
        /// </summary>
        /// <returns></returns>
        List<MeterInfo> AllMeterInfo();
    }
}
