using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpGet("currencyhistory/{title}/{scale}/{dtStart}/{dtFinal}")]
        public async Task<IActionResult> SetFavorite(string title, string scale, DateTime dtStart, DateTime dtFinal)
        {
            var currency = await _currencyService.GetByName(title); 
            var report = await _currencyService.GetWellAsync(currency.Id, scale, dtStart, dtFinal);

            IEnumerable<CurrencyHistoryVM> currencyHistory = _mapper.Map<IEnumerable<CurrencyHistoryVM>>(report);
            IActionResult result = report == null ? NotFound() : Ok(new object[] { title, currencyHistory });
            return result;
        }

        [HttpGet("currency")]
        public async Task<IActionResult> TakeAllCurrencies()
        {
            var report = await _currencyService.GetAll();

            IEnumerable<CurrencyVM> currencyHistory = _mapper.Map<IEnumerable<CurrencyVM>>(report);
            IActionResult result = report == null ? NotFound() : Ok(currencyHistory);
            return result;
        }

        [DisableRequestSizeLimit]
        [HttpPost("addcurrencyhistory")]
        public async Task<IActionResult> Experimental(IFormFile file)
        {
            if (file.FileName.EndsWith(".csv"))
            {
                await _currencyService.RegisterAsync(file);
            }
            else
            {
                return BadRequest(new { message = "Bla Bla" });
            }

            return Ok();
        }

        [HttpPost("addcurrency")]
        public async Task<IActionResult> AddCurrency(Currency currency)
        {
            var dto = _mapper.Map<CurrencyDto>(currency);
            await _currencyService.RegisterAsync(dto);

            return Ok();
        }
    }
}