using Redis.OM.Modeling;
using System;
namespace PowerDispense.Models
{
	[Document(StorageType = StorageType.Hash, Prefixes = new[] {"PowerTransaction"}, IndexName = "transactions")]
	public class PowerTransaction
	{
		[RedisIdField]
		public string Id { get; set; }
		public int AmountPaid { get; set; }
		public int Cost { get; set; }
		[Indexed]
		public string? MeterNo { get; set; }
		public float Unit { get; set; }
		public DateTime PurchaseDate { get; set; }
		[Searchable]
		public string? message { get; set; }

		public MeterInfo? meterInfo { get; set; }
	}
}

