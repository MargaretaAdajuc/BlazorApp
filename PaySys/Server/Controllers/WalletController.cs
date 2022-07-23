using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PaySys.Server.Models;
using PaySys.Server.Data;
using System.Linq;

namespace PaySys.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public WalletController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<Wallet> GetWallets()
        {
            var wallets = context.Wallets.ToList();
            return wallets;
        }
    }
}
