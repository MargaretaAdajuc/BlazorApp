using MediatR;
using Microsoft.EntityFrameworkCore;
using PaySys.Server.Application.Promotion;
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
        public string FailureReason { get; set; }
        public decimal Amount { get; set; }


        public static CreateWalletResult ReturnSuccess(decimal amount)
        {
            return new CreateWalletResult 
            { 
                IsSuccessful = true,
                Amount = amount
            };
        }

        public static CreateWalletResult ReturnFailure(string failureReason)
        {
            return new CreateWalletResult 
            { 
                IsSuccessful = false,
                FailureReason = failureReason
            };
        }
    }


    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateWalletResult>
    {
        private readonly ApplicationDbContext context;
        private readonly IPromotionManager promotionManager;

        public CreateWalletCommandHandler(ApplicationDbContext context, IPromotionManager promotionManager)
        {
            this.context = context;
            this.promotionManager = promotionManager;
        }

        public async Task<CreateWalletResult> Handle(CreateWalletCommand command, CancellationToken cancellationToken)
        {
            if (!CurrencyManager.Currencies.Contains(command.Currency))
            {
                return CreateWalletResult.ReturnFailure("INVALID_CURRENCY");
            }

            var user = await context.Users.Include(x => x.Wallets).FirstOrDefaultAsync(x => x.Id == command.UserId);

            if (user.Wallets.Any(x => x.Currency == command.Currency))
            {
                return CreateWalletResult.ReturnFailure("WALLET_EXISTS");
            }

            var wallet = new Wallet
            {
                Currency = command.Currency,
                Amount = promotionManager.GetDefaultAmount(command.Currency)
            };

            if (user.Wallets == null)
            {
                user.Wallets = new List<Wallet>();
            }

            user.Wallets.Add(wallet);

            context.SaveChanges();

            return CreateWalletResult.ReturnSuccess(wallet.Amount);
        }
    }
}