using Bank.Core.Authorization;
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
        public IActionResult LoginAuthorization([FromBody] User user)
        {
            if (DbAction.UserExistsAndRight(user))
            {
                return Ok();
            }
            else return BadRequest("Username or Password not right");
        }


        [HttpPost]
        [Consumes("application/json")]
        [Route("/registerAttempt")]
        public IActionResult RegisterAttempt([FromBody] User user)
        {
            if (DbAction.CreateUser(user))
            {
                return Ok();
            }
            else return BadRequest("Email already exists");
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("/loginAttempt")]
        [Produces("application/json")]
        public IActionResult LoginAttempt([FromBody] User user)
        {
            string str = DbAction.ReturnUserData(user);

            if (str == null)
            {
                return BadRequest("User is null!");
            }
            else
            {
                return Ok(str);
            }
        }
    }
}
