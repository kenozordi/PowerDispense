using System;
using Microsoft.AspNetCore.Mvc;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models;
using PowerDispense.Services.Workers;

namespace PowerDispense.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        public TransactionWorker _transactionWorker;

        public TransactionsController(TransactionWorker transactionWorker)
        {
            _transactionWorker = transactionWorker;
        }

        [HttpPut]
        [Route("Start")]
        public ActionResult<MeterInfo> StartQueue()
        {
            try
            {
                (bool success, string message) = _transactionWorker.Start();
                if (success)
                {
                    return Ok("transaction queue started");
                }

                return Problem(message);
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }
            
        }
        
        [HttpPut]
        [Route("Stop")]
        public async Task<ActionResult<MeterInfo>> StopQueue()
        {
            try
            {
                (bool success, string message) = await _transactionWorker.Stop();
                if (success)
                {
                    return Ok("transaction queue stopped");
                }

                return Problem(message);
            }
            catch (Exception ex)
            {
                return Problem("Something went wrong, please try again later");
            }
            
        }
        
    }
}

