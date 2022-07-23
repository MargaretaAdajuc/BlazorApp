using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace PaySys.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
<<<<<<< HEAD
        public List<Wallet> Wallets { get; set; }
=======
        public List<Transaction> Wallets { get; set; }
>>>>>>> 79f396f2c4e1662797d7c01f24cff62922f803cd
    }
}
