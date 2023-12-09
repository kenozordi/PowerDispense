using System;
namespace PowerDispense.Models
{
	public class MeterInfo
	{
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? MeterNo { get; set; }
        public string? MeterProvider { get; set; }
        public int Debt { get; set; }

        public enum MeterProviders
        {
            AEDC,
            EKEDC
        }
    }
}

