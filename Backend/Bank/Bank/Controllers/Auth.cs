using Bank.Core.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    /*
     * Controller taking care of Authentication
     * */
    public class Auth : Controller
    {

        [HttpPost]
        [Consumes("application/json")]
        [Route("/loginAuth")]
        public IActionResult Authorization([FromBody] AuthObject auth)
        {
            return Ok();
        }
    }
}
