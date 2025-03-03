using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PowerDispense.Interfaces.IFactories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models;
using PowerDispense.Models.DTO;
using PowerDispense.Services;

namespace PowerDispense.Controllers
{
    [Route("api/[controller]")]
    public class PowerController : Controller
    {
        public IPowerServiceFactory _powerServiceFactory;
        public IMeterService _meterService;
        public ITransactionService _transactionService;

        public PowerController(IPowerServiceFactory powerServiceFactory, IMeterService meterService,
            ITransactionService transactionService)
        {
            _powerServiceFactory = powerServiceFactory;
            _meterService = meterService;
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("Meters")]
        public ActionResult<MeterInfo> Meters()
        {
            try
            {
                var meterInfos = _meterService.AllMeterInfo();
                return meterInfos is not null ? Ok(meterInfos) : BadRequest("No meter available");
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }
            
        }
        
        // POST api/power
        [HttpPost]
        public async Task<ActionResult<PowerTransaction>> PurchasePowerAsync([FromBody]PowerRequest powerRequest)
        {
            try
            {
                var powerTransaction = await _transactionService.PurchasePower(powerRequest);

                return powerTransaction is not null 
                    ? Ok(powerTransaction)
                    : BadRequest("Unable to purchase power");
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
                var powerService = _powerServiceFactory.GetPowerService(powerRequest.MeterProvider);
                var meterInfo = powerService.ValidateMeter(powerRequest);

                if (meterInfo is not null)
                {
                    var borrowPowerService = _powerServiceFactory.GetBorrowPowerService(powerRequest.MeterProvider);
                    var powerTransaction = borrowPowerService.Borrow(powerRequest);
                    return Ok(powerTransaction);
                }

                return BadRequest("Unable to validate meter");
            }
            catch (NotImplementedException ex)
            {
                return Problem("This feature is not available for you yet");
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }

        }

        [HttpPost]
        [Route("Search")]
        public ActionResult<PowerTransaction> Search(
            [FromQuery] string meterNumber, 
            [FromQuery] string meterProvider)
        {
            try
            {
                var metername = _meterService.GetMeterName(meterNumber, meterProvider);

                if (!string.IsNullOrEmpty(metername))
                {
                    return Ok(metername);
                }

                return BadRequest("Unable to get meter name");
            }
            catch (NotImplementedException ex)
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

