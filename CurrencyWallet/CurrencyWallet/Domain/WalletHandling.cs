using CurrencyWallet.DAL;
using CurrencyWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyWallet.Domain
{
    internal sealed class WalletHandling
    {
        private IWalletStore _walletStore;
        private IRateValidate _rateValidate;

        public WalletHandling(IWalletStore walletStore, IRateValidate rateValidate)
        {
            _walletStore = walletStore;
        }

        /// <summary>
        /// Пополнить кошелек
        /// </summary>
        public void ReplenishWallet(string userName, RateModel rate)
        {
            _rateValidate.CheckCurrencyInDictionry(rate.Currency);
            _rateValidate.IsUnsigned(rate.Amount);

            decimal? amount = _walletStore.GetMoneyByUser(userName)
                .FirstOrDefault(r => r.Currency.Equals(rate.Currency))?.Amount;

            if (amount == null)
            {
                _walletStore.AddCurrency(userName, rate);
            }
            else
            {
                rate.Amount += amount.Value;
                _walletStore.UpdateAmount(userName, rate);
            }
        }

        /// <summary>
        /// Снять деньги с кошелька
        /// </summary>
        public void WithdrawModey(string userName, RateModel rate)
        {
            _rateValidate.CheckCurrencyInDictionry(rate.Currency);
            _rateValidate.IsUnsigned(rate.Amount);

            decimal? amount = _walletStore.GetMoneyByUser(userName)
                .FirstOrDefault(r => r.Currency.Equals(rate.Currency))?.Amount;

            if (amount == null)
            {
                throw new Exception("У вас нет кошелька с таким типов валюты. Деньги снять невозможно");
            }
            else
            {
                if (rate.Amount < amount.Value)
                {
                    throw new Exception($"Недостаточно средств для снятия. Не хватает: {Math.Abs(rate.Amount - amount.Value)} {rate.Currency}");
                }
                rate.Amount -= amount.Value;
                _walletStore.UpdateAmount(userName, rate);
            }
        }

        public void CurrencyExchange(string userName, RateModel from, RateModel to)
        {

        }

        private void IsUserExists(string userName)
        {
            if (_walletStore.IsUserExists(userName))
            {
                throw new Exception($"Пользователь {userName} не найден");
            }
        }
    }
}
