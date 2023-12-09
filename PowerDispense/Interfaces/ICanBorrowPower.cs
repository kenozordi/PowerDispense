using System;
using PowerDispense.Models;
using PowerDispense.Models.DTO;

namespace PowerDispense.Interfaces
{
	public interface ICanBorrowPower
	{
        public PowerTransaction Borrow(PowerRequest powerRequest);

    }
}

