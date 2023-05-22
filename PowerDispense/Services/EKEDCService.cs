using System;
using PowerDispense.Interfaces;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.DTO;

namespace PowerDispense.Services
{
    public class EKEDCService : IPowerService, ICanBorrowPower
    {
        const int serviceCharge = 200;

        public PowerTransaction Purchase(PowerRequest powerRequest)
        {
            var meterInfo = MeterInfoSampleData.meterInfos.Where(meter => meter.MeterNo == powerRequest.MeterNo).SingleOrDefault();
            return new PowerTransaction()
            {
                AmountPaid = powerRequest.AmountPaid,
                Cost = powerRequest.AmountPaid - serviceCharge,
                MeterNo = powerRequest.MeterNo,
                PurchaseDate = DateTime.Now,
                message = "Processed by EKEDC",
                meterInfo = meterInfo
            };
        }

        public MeterInfo? ValidateMeter(PowerRequest powerRequest)
        {
            var meterInfo = MeterInfoSampleData.meterInfos.Where(meterInfo
                => meterInfo.MeterNo == powerRequest.MeterNo
                && meterInfo.MeterProvider == powerRequest.MeterProvider
                .ToString())
                .SingleOrDefault();
            return meterInfo is not null ? meterInfo : null;
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

