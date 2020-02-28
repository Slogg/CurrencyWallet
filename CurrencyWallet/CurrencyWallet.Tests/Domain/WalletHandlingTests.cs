using CurrencyWallet.DAL;
using CurrencyWallet.Domain;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using System;
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
                new RateModel(){Currency = "ILS", Amount = 3.7671m}
            });
            _rateValidate = new RateValidate(_rateStore);
        }

        /// <summary>
        /// Проверить добавление новых валют в кошелек пользователю
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        [TestCase("USD", 33)]
        [TestCase("ILS", 3.4)]
        public void ReplenishWallet_AddedNewCurrency_AddRowToListMoney(string currency, decimal amount)
        {
            string user = "пользователь1";
            var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, user);
            var rate = new RateModel() { Currency = currency, Amount = amount };

            wh.ReplenishWallet(rate);

            var amountStore = _walletStore.GetMoneyByUser(user).First(x => x.Currency.Equals(currency)).Amount;

            Assert.AreEqual(amount, amountStore);
        }

        /// <summary>
        /// Несколько операций зачисления на уже имеющийся кошелек
        /// </summary>
        [Test]
        public void ReplenishWallet_MultipleCredits_CorrectValueInStore()
        {
            string currency = "USD";

            string user = "пользователь2";
            var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, user);
            var rate = new RateModel() { Currency = currency, Amount = 12.3m };
            wh.ReplenishWallet(rate);
            rate = new RateModel() { Currency = currency, Amount = 5m };
            wh.ReplenishWallet(rate);


            var amountStore = _walletStore.GetMoneyByUser(user).First(x => x.Currency.Equals(currency)).Amount;

            Assert.AreEqual(37.3m, amountStore);
        }

        /// <summary>
        /// Проверить добавление новых валют в кошелек пользователю
        /// </summary>
        [Test]
        public void WithdrawModey_СashWithdrawal_ReturnValue()
        {
            string currency = "USD";

            string user = "пользователь2";
            var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, user);
            var rate = new RateModel() { Currency = currency, Amount = 12.3m };
            var result = wh.WithdrawModey(rate);

            Assert.AreEqual(7.7m, result);
        }

        /// <summary>
        /// Проверить добавление новых валют в кошелек пользователю
        /// </summary>
        [Test]
        public void WithdrawModey_InvalidWithdrawal_ThrowException()
        {
            string currency = "USD";

            string user = "пользователь2";
            var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, user);
            var rate = new RateModel() { Currency = currency, Amount = 500m };

            var expectedMsg = $"Недостаточно средств для снятия. Не хватает: " +
                $"{480} {rate.Currency}";

            Assert.That(() => wh.WithdrawModey(rate),
                 Throws.TypeOf<Exception>()
                .With.Message.EqualTo(expectedMsg));
        }

        /// <summary>
        /// Проверить добавление новых валют в кошелек пользователю
        /// </summary>
        [Test]
        public void CurrencyExchange_ValueAmountTo_CorrectValue()
        {
            _rateStore.GetRate("USD").Returns(1.0964m);
            _rateStore.GetRate("ILS").Returns(3.7671m);

            var rateFrom = new RateModel() { Currency = "USD", Amount = 10m };
            var rateTo = new RateModel() { Currency = "ILS", Amount = 0m };

            string user = "пользователь2";
            var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, user);
            wh.CurrencyExchange(rateFrom, rateTo);

            var amountStore = _walletStore.GetMoneyByUser(user)
                .First(x => x.Currency.Equals(rateTo.Currency)).Amount;

            Assert.AreEqual(Math.Round(34.35m), Math.Round(amountStore));
        }

    }
}
