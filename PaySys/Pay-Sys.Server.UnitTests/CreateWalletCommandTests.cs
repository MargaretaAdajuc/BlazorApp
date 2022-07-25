using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PaySys.Server.Application.Promotion;
using PaySys.Server.Application.Wallets.Commands;
using PaySys.Server.Data;
using PaySys.Server.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pay_Sys.Server.UnitTests
{
    public class CreateWalletCommandHandlerTests
    {
        private ApplicationDbContext context;
        private Mock<IPromotionManager> promotionManagerMock;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite("Filename=Test.db")
                    .Options, Microsoft.Extensions.Options.Options.Create(new OperationalStoreOptions()));

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new ApplicationUser
            {
                Id = "test_user_id",
                Wallets = new List<Wallet>
                {
                    new Wallet
                    {
                        Amount = 100,
                        Currency = "EC"
                    }
                }
            };

            context.Add(user);

            context.SaveChanges();

            promotionManagerMock = new Mock<IPromotionManager>();

            promotionManagerMock.Setup(x => x.GetDefaultAmount(It.IsAny<string>())).Returns(500);
        }

        [Test]
        public async Task CreateWalletSuccessful()
        {
            var sut = new CreateWalletCommandHandler(context, promotionManagerMock.Object);

            var command = new CreateWalletCommand
            {
                UserId = "test_user_id",
                Currency = "EUR"
            };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task CreateWalletInvalidCurrency()
        {
            var sut = new CreateWalletCommandHandler(context, promotionManagerMock.Object);

            var command = new CreateWalletCommand
            {
                UserId = "test_user_id",
                Currency = "RUB"
            };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(result.IsSuccessful);
                Assert.That(result.FailureReason, Is.EqualTo("INVALID_CURRENCY"));
            });
        }

        [Test]
        public async Task CreateWalletTestAmount()
        {
            var sut = new CreateWalletCommandHandler(context, promotionManagerMock.Object);

            var command = new CreateWalletCommand
            {
                UserId = "test_user_id",
                Currency = "EUR"
            };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.IsSuccessful);
                Assert.That(result.Amount, Is.EqualTo(500));
            });
        }
    }
}