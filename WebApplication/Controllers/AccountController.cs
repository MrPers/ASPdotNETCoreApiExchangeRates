
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/")]//  /api/Account/login
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost("regist")]
        public async Task<ActionResult<User>> Regist(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(new { user.Id, user.Name });
        }

        //[HttpPost("auth")]
        //public async Task<ActionResult<User>> Auth()
        //{
        //    return Ok();
        //}
    }
}
