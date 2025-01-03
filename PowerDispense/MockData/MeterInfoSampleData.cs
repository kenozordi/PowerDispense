using System;
using PowerDispense.Models;
using PowerDispense.Models.Enum;

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
                PowerProvider = PowerProvider.AEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Ruth Agbor",
                Address = "Osapa London, Lagos",
                MeterNo = "654321",
                PowerProvider = PowerProvider.EKEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Oluwafemi Daramola",
                Address = "Oniru, Lagos",
                MeterNo = "123654",
                PowerProvider = PowerProvider.EKEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Yusuf Idris",
                Address = "Ketu, Lagos",
                MeterNo = "162534",
                PowerProvider = PowerProvider.EKEDC.ToString()
            },
            new MeterInfo()
            {
                CustomerName = "Esther Efughu",
                Address = "Lakowe, Lagos",
                MeterNo = "126534",
                PowerProvider = PowerProvider.EKEDC.ToString()
            }
        };
	}
}

