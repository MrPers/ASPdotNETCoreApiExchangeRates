using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication.Entities;
using WebApplication.Models;
using WebApplication.Services;

namespace WebAppi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        //[Authorize]
        [HttpGet("{title}")]
        public IActionResult SetFavorite(string title)
        {
            var report = _currencyService.GetWell(title);
            return report == null ? NotFound() : Ok(new object[] { title, report });
        }
    }
}