using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApplication.DB.Entites;
using WebApplication.DTO;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
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

            IEnumerable<CurrencyHistoryVM> currencyHistory = _mapper.Map<IEnumerable<CurrencyHistoryVM>>(report);

            IActionResult result = report == null ? NotFound() : Ok(new object[] { title, currencyHistory });
            return result;
        }

    }
}