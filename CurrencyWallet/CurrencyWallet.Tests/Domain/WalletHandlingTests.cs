using CurrencyWallet.DAL;
using CurrencyWallet.Domain;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyWallet.Tests.Domain
{
    [TestFixture]
    class WalletHandlingTests
    {
        private IWalletStore _walletStore;
        private IRateValidate _rateValidate;
        private IRateStore _rateStore;

        [SetUp]
        public void Setup()
        {
            _walletStore = new WalletStore();
            _rateStore = Substitute.For<IRateStore>();
            _rateStore.RateDict.Returns(new List<RateModel>
            {
                new RateModel(){Currency = "USD", Amount = 1.0964m},
                new RateModel(){Currency = "BGN", Amount = 1.9558m}
            });
            _rateValidate = new RateValidate(_rateStore);
        }

        [TestCase("USD", 33)]
        [TestCase("BGN", 3.4)]
        public void ReplenishWallet_AddedNewCurrency_AddRowToListMoney(string currency, decimal amount)
        {
            string user = "пользователь1";
            var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, user);
            var rate = new RateModel() { Currency = currency, Amount = amount };

            wh.ReplenishWallet(rate);

            var amountStore = _walletStore.GetMoneyByUser(user).First(x => x.Currency.Equals(currency)).Amount;

            Assert.AreEqual(amount, amountStore);
        }

    }
}
