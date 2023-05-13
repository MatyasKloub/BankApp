using Bank.Core.Database;
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
        public IActionResult Authorization([FromBody] User user)
        {
            return Ok();
        }
    }
}
