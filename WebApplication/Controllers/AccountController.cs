using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DTO;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/")]
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
        
        [HttpPost("regist")]
        public async Task<IActionResult> Register(UserViewVM userModel)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new { message = "Didn't register!" });
            }

            var dto = _mapper.Map<UserDto>(userModel);
            var createdUserId = await _userService.RegisterAsync(dto);

            return Ok(createdUserId);
        }
    }
}
