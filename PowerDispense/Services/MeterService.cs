using PowerDispense.Interfaces;
using PowerDispense.MockData;
using PowerDispense.Models;

namespace PowerDispense.Services
{
    public class MeterService : IMeterService
    {
        public List<MeterInfo> AllMeterInfo()
        {
            return MeterInfoSampleData.meterInfos;
        }
    }
}
