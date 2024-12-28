using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models;

namespace PowerDispense.Controllers
{
    [Route("api/[controller]")]
    public class RaffleDrawController : Controller
    {
        public IRaffleDrawService _raffleDrawService;

        public RaffleDrawController(IRaffleDrawService raffleDrawService)
        {
            _raffleDrawService = raffleDrawService;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllEntries()
        {
            try
            {
                var raffleEntries = await _raffleDrawService.GetAllEntry();
                return raffleEntries is not null ? Ok(raffleEntries) : BadRequest("No entry available");
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }
            
        }
        
    }
}

