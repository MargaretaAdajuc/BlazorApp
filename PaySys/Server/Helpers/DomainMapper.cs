using PaySys.Shared;
using PaySys.Server.Models;

namespace PaySys.Server.Helpers
{
    public static class DomainMapper
    {
        public static TransactionDto ToDto(Transaction transaction)
        {
            return transaction == null 
                ? null 
                : new TransactionDto
                {
                    Id = transaction.Id,
                    Amount = transaction.Amount,
                    SourceWalletId = transaction.SourceWalletId,    
                    Date = transaction.Date,
                    DestinationWalletId = transaction.DestinationWalletId
                };
        }
    }
}
