using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Models;
using WebApplication.Services;

namespace WebAppi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;

        public CurrencyController(ICurrencyService currencyService, IMapper mapper)
        {
            _currencyService = currencyService;
            _mapper = mapper;
        }

        //[Authorize]
        [HttpGet("{title}")]
        public IActionResult SetFavorite(string title)
        {
            var report = _currencyService.GetWellAsync(title);
            return report == null ? NotFound() : Ok(new object[] { title, report });
            //return Ok();
        }

    }
}