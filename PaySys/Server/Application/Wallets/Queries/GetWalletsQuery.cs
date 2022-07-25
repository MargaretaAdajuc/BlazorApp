using MediatR;
using Microsoft.EntityFrameworkCore;
using PaySys.Server.Data;
using PaySys.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaySys.Server.Application.Wallets.Queries
{
    public class GetWalletsQuery : IRequest<List<Wallet>>
    {
        public string UserId { get; set; }
        public decimal AmountTo { get; set; }
        public decimal AmountFrom { get; set; }
    }

    public class GetWalletsQueryHandler : IRequestHandler<GetWalletsQuery, List<Wallet>>
    {
        private readonly ApplicationDbContext context;

        public GetWalletsQueryHandler(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Wallet>> Handle(GetWalletsQuery query, CancellationToken cancellationToken)
        {
            var userWithWallets =  await context.Users
                .Include(x => x.Wallets)
                .FirstOrDefaultAsync(x => x.Id == query.UserId);

            return userWithWallets.Wallets;
        }
    }
}