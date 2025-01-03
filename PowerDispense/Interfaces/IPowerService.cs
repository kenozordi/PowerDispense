using System;
using PowerDispense.Models;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces
{
	public interface IPowerService
	{
        PowerTransaction Purchase(PowerRequest powerRequest);
        Task<MeterInquiryResponse>? ValidateMeter(PowerRequest powerRequest);

    }
}

