using MediatR;
using Microsoft.EntityFrameworkCore;
using PaySys.Server.Data;
using PaySys.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaySys.Server.Application.Wallets.Commands
{
    public class CreateWalletCommand : IRequest<CreateWalletResult>
    {
        public string Currency { get; set; }
        public string UserId { get; set; }
    }

    public class CreateWalletResult
    {
        public bool IsSuccessful { get; set; }

        public static CreateWalletResult ReturnSuccess()
        {
            return new CreateWalletResult { IsSuccessful = true };
        }

        public static CreateWalletResult ReturnFailure()
        {
            return new CreateWalletResult { IsSuccessful = false };
        }
    }


    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateWalletResult>
    {
        private readonly ApplicationDbContext context;

        public CreateWalletCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<CreateWalletResult> Handle(CreateWalletCommand command, CancellationToken cancellationToken)
        {
            if (!CurrencyManager.Currencies.Contains(command.Currency))
            {
                return CreateWalletResult.ReturnFailure();
            }

            var user = await context.Users.Include(x => x.Wallets).FirstOrDefaultAsync(x => x.Id == command.UserId);

            if (user.Wallets.Any(x => x.Currency == command.Currency))
            {
                return CreateWalletResult.ReturnFailure();
            }

            var wallet = new Wallet
            {
                Currency = command.Currency,
                Amount = 0
            };

            if (user.Wallets == null)
            {
                user.Wallets = new List<Wallet>();
            }

            user.Wallets.Add(wallet);

            context.SaveChanges();

            return CreateWalletResult.ReturnSuccess();
        }
    }
}