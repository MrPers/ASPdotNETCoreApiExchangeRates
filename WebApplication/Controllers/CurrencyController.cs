using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication.DTO;
using WebApplication.Entites;
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

            List<CurrencyHistoryVM> currencyHistory = new List<CurrencyHistoryVM>();
            foreach (var item in report)
                currencyHistory.Add(_mapper.Map<CurrencyHistory, CurrencyHistoryVM>(item));

            IActionResult result = report == null ? NotFound() : Ok(new object[] { title, currencyHistory });
            return result;
        }

    }
}