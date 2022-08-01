using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaySys.Server.Data;
using PaySys.Shared;
using System.Linq;

namespace PaySys.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("{username}/validate")]
        public UserValidationResult ValidateUser(string username)
        {
            var user = context.Users.FirstOrDefault(x => x.UserName == username);

            return new UserValidationResult
            {
                Exists = user != null
            };
        }
    }
}
