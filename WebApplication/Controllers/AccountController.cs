using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DTO;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/")]//  /api/Account/login
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //[HttpPost("authenticate")]
        //public IActionResult Authenticate(AuthenticateRequest model)
        //{
        //    var response = _userService.Authenticate(model);

        //    if (response == null)
        //        return BadRequest(new { message = "Username or password is incorrect" });

        //    return Ok(response);
        //}

        [HttpPost("regist")]
        public async Task<IActionResult> Register(UserViewVM userModel)//todo:View model->dto
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new { message = "Didn't register!" });
            }

            var dto = _mapper.Map<UserModelDto>(userModel); //VM->DTO
            var createdUser = await _userService.Register(dto);//DTO

            if (createdUser == null)
            {
                return BadRequest(new { message = "Didn't register!" });
            }
            if (userModel.Authorize)
            {
                //string token = await _userService.Authenticate(createdUser.Name, createdUser.Password);

                //if (string.IsNullOrWhiteSpace(token))
                //{
                //    return BadRequest("somethin went wrong");
                //}
                //return Ok(token);
            }
            return Ok(createdUser);
        }
    }
}
