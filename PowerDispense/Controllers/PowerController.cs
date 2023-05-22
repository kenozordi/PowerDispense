using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PowerDispense.Interfaces;
using PowerDispense.Models;
using PowerDispense.Models.DTO;
using PowerDispense.Services;

namespace PowerDispense.Controllers
{
    [Route("api/[controller]")]
    public class PowerController : Controller
    {
        public IPowerService _powerService;
        public ICanBorrowPower _borrowPowerService;

        public PowerController(IPowerService powerService, ICanBorrowPower canBorrowPower)
        {
            _powerService = powerService;
            _borrowPowerService = canBorrowPower;
        }

        [HttpGet]
        [Route("Meters")]
        public ActionResult<MeterInfo> Meters()
        {
            try
            {
                var meterInfos = _powerService.AllMeterInfo();

                return meterInfos is not null ? Ok(meterInfos) : BadRequest("No meter available");
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }
            
        }
        
        // POST api/power
        [HttpPost]
        public ActionResult<PowerTransaction> PurchasePower([FromBody]PowerRequest powerRequest)
        {
            try
            {
                var meterInfo = _powerService.ValidateMeter(powerRequest);

                if (meterInfo is not null)
                {
                    _powerService = PowerServiceSwitch.GetPowerService(powerRequest.MeterProvider);
                    var powerTransaction = _powerService.Purchase(powerRequest);
                    return Ok(powerTransaction);
                }

                return BadRequest("Unable to validate meter");
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }
            
        }

        [HttpPost]
        [Route("Borrow")]
        public ActionResult<PowerTransaction> Borrow([FromBody] PowerRequest powerRequest)
        {
            try
            {
                var meterInfo = _powerService.ValidateMeter(powerRequest);

                if (meterInfo is not null)
                {
                    _borrowPowerService = PowerServiceSwitch.GetPowerBorrowService(powerRequest.MeterProvider);
                    var powerTransaction = _borrowPowerService.Borrow(powerRequest);
                    return Ok(powerTransaction);
                }

                return BadRequest("Unable to validate meter");
            }
            catch (NotImplementedException message)
            {
                return Problem("This feature is not available for you yet");
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }

        }
    }
}

