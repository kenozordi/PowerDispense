using System;
using PowerDispense.Interfaces;
using PowerDispense.Models;

namespace PowerDispense.Services
{
	public static class PowerServiceSwitch
	{
		public static IPowerService GetPowerService(MeterInfo.MeterProviders meterProvider)
		{
            switch (meterProvider)
			{
				case MeterInfo.MeterProviders.AEDC:
					return new AEDCService();
				case MeterInfo.MeterProviders.EKEDC:
                    return new EKEDCService();
                default:
					throw new NotImplementedException();
			}

		}

        public static ICanBorrowPower GetPowerBorrowService(MeterInfo.MeterProviders meterProvider)
        {
            switch (meterProvider)
            {
                case MeterInfo.MeterProviders.EKEDC:
                    return new EKEDCService();
                default:
                    throw new NotImplementedException();
            }

        }
    }
}

