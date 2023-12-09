using System;
namespace PowerDispense.Models.DTO
{
	public class PowerRequest
	{
        public int AmountPaid { get; set; }
        public string? MeterNo { get; set; }
        public MeterInfo.MeterProviders MeterProvider { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

