using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaySys.Shared;
using System.Collections.Generic;

namespace PaySys.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        [HttpGet]
        public CurrencyList GetCurrencies()
        {
            return new CurrencyList
            {
                CurrencyNames = CurrencyManager.Currencies
            };
        }   
    }
}
