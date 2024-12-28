using System;
using PowerDispense.IFactories;
using PowerDispense.Interfaces;
using PowerDispense.Models;

namespace PowerDispense.Services
{
	public class PowerServiceSwitch : IPowerServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public PowerServiceSwitch(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPowerService GetPowerService(MeterInfo.MeterProviders meterProvider)
		{
            using var scope = _serviceProvider.CreateScope();

            switch (meterProvider)
			{
				case MeterInfo.MeterProviders.AEDC:
                    return scope.ServiceProvider.GetRequiredService<AEDCService>();
                case MeterInfo.MeterProviders.EKEDC:
                    return scope.ServiceProvider.GetRequiredService<EKEDCService>();
                default:
					throw new NotImplementedException();
			}

		}

        public ICanBorrowPower GetBorrowPowerService(MeterInfo.MeterProviders meterProvider)
        {
            using var scope = _serviceProvider.CreateScope();

            switch (meterProvider)
            {
                case MeterInfo.MeterProviders.EKEDC:
                    return scope.ServiceProvider.GetRequiredService<EKEDCService>();
                default:
                    throw new NotImplementedException();
            }

        }
    }
}

