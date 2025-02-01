using PowerDispense.Models;

namespace PowerDispense.Interfaces.IServices
{
    public interface IMeterService
    {
        /// <summary>
        /// Get All Meters
        /// </summary>
        /// <returns></returns>
        List<MeterInfo> AllMeterInfo();

        /// <summary>
        /// Load All Meter Info from File
        /// </summary>
        /// <returns></returns>
        bool LoadMetersFromFile();

        /// <summary>
        /// Get the name on a meter
        /// </summary>
        /// <returns></returns>
        string GetMeterName(string meterNumber, string meterProvider);
    }
}
