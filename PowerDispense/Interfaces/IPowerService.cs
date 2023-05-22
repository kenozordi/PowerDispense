using System;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.DTO;

namespace PowerDispense.Interfaces
{
	public interface IPowerService
	{
        public List<MeterInfo> AllMeterInfo()
        {
            return MeterInfoSampleData.meterInfos;
        }
        public PowerTransaction Purchase(PowerRequest powerRequest);
        public MeterInfo? ValidateMeter(PowerRequest powerRequest);

    }
}

