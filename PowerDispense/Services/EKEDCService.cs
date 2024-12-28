using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Options;
using PowerDispense.IFactories.IRepoFactories;
using PowerDispense.Interfaces;
using PowerDispense.Interfaces.IRepositories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.Config;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;
using StackExchange.Redis;

namespace PowerDispense.Services
{
    public class EKEDCService : IPowerService, ICanBorrowPower
    {
        const int serviceCharge = 200;
        private readonly ConnectionStrings _connectionStrings;
        private readonly IPowerRepoFactory _powerRepoFactory;
        private readonly IRaffleDrawService _raffleDrawService;

        public EKEDCService(IOptions<ConnectionStrings> connectionStrings, IPowerRepoFactory powerRepoFactory,
            IRaffleDrawService raffleDrawService)
        {
            _connectionStrings = connectionStrings.Value;
            _powerRepoFactory = powerRepoFactory;
            _raffleDrawService = raffleDrawService;
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
            _raffleDrawService.AddEntry(powerRequest);
            return powerTransaction;
        }

        public async Task<MeterInfo>? ValidateMeter(PowerRequest powerRequest)
        {
            var cachePowerRepo = _powerRepoFactory.GetPowerRepo(DataSource.Cache);
            var meter = await cachePowerRepo.GetMeter(powerRequest);
            if (meter is not null)
            {
                return meter;
            }

            var filePowerRepo = _powerRepoFactory.GetPowerRepo(DataSource.File);
            meter = await filePowerRepo.GetMeter(powerRequest);
            if (meter is not null)
            {
                cachePowerRepo.AddMeter(meter);
                return meter;
            }

            return default;
        }

        public PowerTransaction Borrow(PowerRequest powerRequest)
        {
            var meterInfo = MeterInfoSampleData.meterInfos.Where(meter => meter.MeterNo == powerRequest.MeterNo).Single();
            meterInfo.Debt += powerRequest.AmountPaid;

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
    }
}

