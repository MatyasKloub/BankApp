using Bank.Core.Database;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    public class Crons : Controller
    {
        [HttpGet]
        [Route("/updateKurz")]
        public async Task<IActionResult> updateCnb()
        {
            bool success = await CnbKurz.newKurz();

            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
