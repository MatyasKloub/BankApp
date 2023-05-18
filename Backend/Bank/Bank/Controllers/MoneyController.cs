using Bank.Core.Authorization;
using Bank.Core.Database;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    public class MoneyController : Controller
    {

        [HttpGet]
        [Route("/getUcty")]
        public IActionResult getUcty([FromQuery] string email)
        {
            string returnal = DbAction.GetUcty(email, null);

            if (returnal != null)
            {
                return Ok(returnal);
            }
            else
            {
                return BadRequest("neplatny email");
            }
        }


        [HttpGet]
        [Route("/getMeny")]
        public IActionResult getMeny([FromQuery] string key)
        {
            if (key != "HwQ8qQMU$tRSsv")
            {
                return BadRequest("Wrong Key");
            }
            string returnal = DbAction.GetMeny();

            if (returnal != null)
            {
                return Ok(returnal);
            }
            else
            {
                return BadRequest("chyba v menach");
            }
        }

        [HttpGet]
        [Route("/pay")]
        public IActionResult Pay([FromQuery] string key, [FromQuery] string zkratka, [FromQuery] int value, [FromQuery] string type, [FromQuery] string email)
        {
            if (key != "HwQ8qQMU$tRSsv")
            {
                return BadRequest("Wrong key");
            }

            if (DbAction.doPayment(zkratka,value,email,type, null))
            {
                return Ok("Vše ok!");
            }
            else { return BadRequest("Nop"); }
        }

        [HttpGet]
        [Route("/getHistory")]
        public IActionResult GetHistory([FromQuery] string key, [FromQuery] string email)
        {
            if (key != "HwQ8qQMU$tRSsv")
            {
                return BadRequest("Wrong key");
            }
            return Ok(DbAction.getPlatby(email, null));


        }



    }
}
