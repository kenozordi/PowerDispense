using System;
using System.Diagnostics;
using System.Text;
using PowerDispense.IFactories.IRepoFactories;
using PowerDispense.Interfaces;
using PowerDispense.Interfaces.IServices;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;

namespace PowerDispense.Services
{
    public class EKEDCService : IPowerService, ICanBorrowPower
    {
        const int serviceCharge = 200;
        private readonly IPowerRepoFactory _powerRepoFactory;
        private readonly IRaffleDrawService _raffleDrawService;
        private readonly IPowerProviderHealth _powerProviderHealthService;

        public EKEDCService(IPowerRepoFactory powerRepoFactory,
            IRaffleDrawService raffleDrawService, IPowerProviderHealth powerProviderHealth)
        {
            _powerRepoFactory = powerRepoFactory;
            _raffleDrawService = raffleDrawService;
            _powerProviderHealthService = powerProviderHealth;
        }

        public PowerTransaction Purchase(PowerRequest powerRequest)
        {
            var meterInfo = MeterInfoSampleData.meterInfos.Where(meter => meter.MeterNo == powerRequest.MeterNo).SingleOrDefault();
            var powerTransaction = new PowerTransaction()
            {
                AmountPaid = powerRequest.AmountPaid,
                Cost = powerRequest.AmountPaid - serviceCharge,
                MeterNo = powerRequest.MeterNo,
                PurchaseDate = DateTime.Now,
                message = "Processed by EKEDC",
                meterInfo = meterInfo
            };
            if (powerTransaction is not null)
            {
                _raffleDrawService.AddEntry(powerRequest);
                _ = Enum.TryParse(meterInfo.PowerProvider, out PowerProvider powerProvider);
                _powerProviderHealthService.SetProviderHealthStatus(powerProvider, PowerProviderStatus.Stable);
            }
            else
            {
                _powerProviderHealthService.SetProviderHealthStatus(PowerProviderStatus.Unstable);
            }
            return powerTransaction;
        }

        public async Task<MeterInquiryResponse>? ValidateMeter(PowerRequest powerRequest)
        {
            var response = new MeterInquiryResponse();

            var powerProviderStatus = await _powerProviderHealthService.GetProviderHealthStatus(PowerProvider.EKEDC);
            response.PowerProviderInfo = new PowerProviderInfo()
            {
                Status = powerProviderStatus.ToString()
            };
            
            var cachePowerRepo = _powerRepoFactory.GetPowerRepo(DataSource.Cache);
            var meter = await cachePowerRepo.GetMeter(powerRequest);
            if (meter is not null)
            {
                response.MeterInfo = meter;
                return response;
            }

            var filePowerRepo = _powerRepoFactory.GetPowerRepo(DataSource.File);
            meter = await filePowerRepo.GetMeter(powerRequest);
            if (meter is not null)
            {
                cachePowerRepo.AddMeter(meter);
                response.MeterInfo = meter;
                return response;
            }

            return default;
        }

        public PowerTransaction Borrow(PowerRequest powerRequest)
        {
            var meterInfo = MeterInfoSampleData.meterInfos.Where(meter => meter.MeterNo == powerRequest.MeterNo).First();
            if (meterInfo is not null)
            {
                MeterInfoSampleData.meterInfos
                    .Where(meter => meter.MeterNo == powerRequest.MeterNo)
                    .First()
                    .Debt += powerRequest.AmountPaid;

                return new PowerTransaction()
                {
                    AmountPaid = powerRequest.AmountPaid,
                    Cost = powerRequest.AmountPaid - serviceCharge,
                    MeterNo = powerRequest.MeterNo,
                    PurchaseDate = DateTime.Now,
                    message = "Processed by EKEDC",
                    meterInfo = meterInfo,
                };
            }

            return default;
        }

    }
}

