using System;
using PowerDispense.Models;

namespace PowerDispense.MockData
{
	public static class MeterInfoSampleData
	{
        public static List<MeterInfo> meterInfos = new List<MeterInfo>()
        {
            new MeterInfo()
            {
                CustomerName = "Ken Ozordi",
                Address = "VGC, Lagos",
                MeterNo = "123456",
                MeterProvider = MeterInfo.MeterProviders.AEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Ruth Agbor",
                Address = "Osapa London, Lagos",
                MeterNo = "654321",
                MeterProvider = MeterInfo.MeterProviders.EKEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Oluwafemi Daramola",
                Address = "Oniru, Lagos",
                MeterNo = "123654",
                MeterProvider = MeterInfo.MeterProviders.EKEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Yusuf Idris",
                Address = "Ketu, Lagos",
                MeterNo = "162534",
                MeterProvider = MeterInfo.MeterProviders.EKEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Esther Efughu",
                Address = "Lakowe, Lagos",
                MeterNo = "126534",
                MeterProvider = MeterInfo.MeterProviders.EKEDC.ToString()
            }
        };
	}
}

