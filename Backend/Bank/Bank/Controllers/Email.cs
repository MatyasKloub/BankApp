using Bank.Core.Emailer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    public class Email : Controller
    {


        [HttpGet]
        [Route("/sendEmail")]
        public IActionResult SendEmail([FromQuery] string email,[FromQuery] string uniqueKey)
        {
            if (EmailAction.sendEmail(uniqueKey,email))
            {
                return Ok("Ok");
            }
            else
            {
                return BadRequest("Problem sending email");
            }
        }
    }
}
