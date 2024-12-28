using System;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.DTO;

namespace PowerDispense.Interfaces
{
	public interface IPowerService
	{
        PowerTransaction Purchase(PowerRequest powerRequest);
        Task<MeterInfo>? ValidateMeter(PowerRequest powerRequest);

    }
}

