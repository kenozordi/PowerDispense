using Microsoft.Extensions.Options;
using PowerDispense.Interfaces.IRepositories;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.Config;
using PowerDispense.Models.DTO;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text;

namespace PowerDispense.Repositories.Power
{
    public class FilePowerRepo : IPowerRepo
    {
        public async Task<bool>? AddMeter(MeterInfo meterInfo)
        {
            throw new NotImplementedException();
        }

        public async Task<MeterInfo>? GetMeter(PowerRequest powerRequest)
        {
            var meterInfo = MeterInfoSampleData.meterInfos.Where(meterInfo
                => meterInfo.MeterNo == powerRequest.MeterNo
                && meterInfo.PowerProvider == powerRequest.MeterProvider
                .ToString())
                .SingleOrDefault();
            return meterInfo;
        }
    }
}
