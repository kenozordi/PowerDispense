using System;
namespace PowerDispense.Models
{
	public class PowerTransaction
	{
		public int Id { get; set; }
		public int AmountPaid { get; set; }
		public int Cost { get; set; }
		public string? MeterNo { get; set; }
		public float Unit { get; set; }
		public DateTime PurchaseDate { get; set; }
		public string? message { get; set; }

		public MeterInfo? meterInfo { get; set; }
	}
}

