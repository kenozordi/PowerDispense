using System;
using PowerDispense.Interfaces.IFactories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models.Enum;

namespace PowerDispense.Services
{
    public class PowerServiceSwitch : IPowerServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public PowerServiceSwitch(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPowerService GetPowerService(PowerProvider meterProvider)
		{
            using var scope = _serviceProvider.CreateScope();

            switch (meterProvider)
			{
				case PowerProvider.AEDC:
                    return scope.ServiceProvider.GetRequiredService<AEDCService>();
                case PowerProvider.EKEDC:
                    return scope.ServiceProvider.GetRequiredService<EKEDCService>();
                default:
					throw new NotImplementedException();
			}

		}

        public ICanBorrowPower GetBorrowPowerService(PowerProvider meterProvider)
        {
            using var scope = _serviceProvider.CreateScope();

            switch (meterProvider)
            {
                case PowerProvider.EKEDC:
                    return scope.ServiceProvider.GetRequiredService<EKEDCService>();
                default:
                    throw new NotImplementedException();
            }

        }
    }
}

