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
            if (DbAction.UserExistsAndRight(user, null))
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
             if (DbAction.CreateUser(user, null))
             {
                 return Ok();
             }
             else return BadRequest("Email already exists");
            return Ok();
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("/loginAttempt")]
        [Produces("application/json")]
        public IActionResult LoginAttempt([FromBody] User user)
        {
           

            string str = DbAction.ReturnUserData(user, null);

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
