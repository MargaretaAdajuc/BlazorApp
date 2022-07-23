using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PaySys.Server.Models;
using PaySys.Server.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PaySys.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        public WalletController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public List<Wallet> GetWallets()
        {
            var userId = userManager.GetUserId(User);
            var wallets = context.Users.Include(x => x.Wallets).FirstOrDefault(x => x.Id == userId).Wallets;
            return wallets;
        }

        [HttpPost]
        public IActionResult CreateWallet([FromQuery]string currency)
        {
            var userId = userManager.GetUserId(User);

            var wallet = new Wallet 
            { 
                Currency = currency,
                Amount = 0
            };

            var user = context.Users.FirstOrDefault(x => x.Id == userId);

            if (user.Wallets == null)
            {
                user.Wallets = new List<Wallet> ();
            }
            
            user.Wallets.Add(wallet);   

            context.SaveChanges();

            return Ok();
        }
    }
}