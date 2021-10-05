using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("currency/{title}")]
        public async Task<IActionResult> SetFavorite(string title)
        {
            var report = await _currencyService.GetWellAsync(title);

            IEnumerable<CurrencyHistoryVM> currencyHistory = _mapper.Map<IEnumerable<CurrencyHistoryVM>>(report);
            IActionResult result = report == null ? NotFound() : Ok(new object[] { title, currencyHistory });
            return result;
        }

        [HttpGet("currencyhistory")]
        public async Task<IActionResult> TakeAllCurrencies()
        {
            var report = await _currencyService.GetAll();

            IEnumerable<CurrencyVM> currencyHistory = _mapper.Map<IEnumerable<CurrencyVM>>(report);
            IActionResult result = report == null ? NotFound() : Ok(currencyHistory);
            return result;
        }
    }
}