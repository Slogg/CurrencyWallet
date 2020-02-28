using CurrencyWallet.DAL;
using CurrencyWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyWallet.Domain
{
    public sealed class WalletHandling
    {
        private IWalletStore _walletStore;
        private IRateValidate _rateValidate;
        private IRateStore _rateStore;
        private string _userName;

        public WalletHandling(IWalletStore walletStore, IRateStore rateStore, IRateValidate rateValidate, string userName)
        {
            _walletStore = walletStore;
            _rateValidate = rateValidate;
            _rateStore = rateStore;
            _userName = userName;
            IsUserExists();
        }

        /// <summary>
        /// Пополнить кошелек
        /// </summary>
        public void ReplenishWallet(RateModel rate)
        {
            _rateValidate.CheckCurrencyInDictionry(rate.Currency);
            _rateValidate.IsUnsigned(rate.Amount);

            decimal? amount = GetAmount(rate.Currency);
            UpdateOrAddAmount(rate, amount);
        }

        /// <summary>
        /// Снять деньги с кошелька
        /// </summary>
        public void WithdrawModey(RateModel rate)
        {
            _rateValidate.CheckCurrencyInDictionry(rate.Currency);
            _rateValidate.IsUnsigned(rate.Amount);

            decimal amount = TryGetAmount(rate.Currency);

            if (rate.Amount < amount)
            {
                throw new Exception($"Недостаточно средств для снятия. Не хватает: {Math.Abs(rate.Amount - amount)} {rate.Currency}");
            }
            rate.Amount -= amount;
            _walletStore.UpdateAmount(_userName, rate);
        }

        /// <summary>
        /// Выполнить обмен валют
        /// </summary>
        /// <param name="rateFrom">перевос с валюты</param>
        /// <param name="rateTo">перевод на валюту</param>
        public void CurrencyExchange(RateModel rateFrom, RateModel rateTo)
        {
            _rateValidate.CheckCurrencyInDictionry(rateFrom.Currency);
            _rateValidate.CheckCurrencyInDictionry(rateTo.Currency);

            _rateValidate.IsUnsigned(rateFrom.Amount);

            decimal amountFrom = TryGetAmount(rateFrom.Currency);
            if (amountFrom < rateFrom.Amount)
            {
                throw new Exception($"Недостаточно средств для перевода. Не хватает: {Math.Abs(rateFrom.Amount - amountFrom)} {rateFrom.Currency}");
            }
            decimal? amountTo = GetAmount(rateFrom.Currency);

            rateTo.Amount = rateFrom.Amount * _rateStore.GetRate(rateTo.Currency) / _rateStore.GetRate(rateFrom.Currency);

            UpdateOrAddAmount(rateTo, amountTo);
        }

        /// <summary>
        /// Получить все финансы пользователя
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<RateModel> GetAllMoneyUser()
        {
            return _walletStore.GetMoneyByUser(_userName);
        }

        // Обновить значение в кошельке или добавить новую валюту со значением
        private void UpdateOrAddAmount(RateModel rate, decimal? amount)
        {

            if (amount == null)
            {
                _walletStore.AddCurrency(_userName, rate);
            }
            else
            {
                rate.Amount += amount.Value;
                _walletStore.UpdateAmount(_userName, rate);
            }
        }

        // попытаться получить текущий счет по валюте, если валюты нет - вернуть ошибку
        private decimal TryGetAmount(string currency)
        {
            decimal? amount = GetAmount(currency);
            if (amount == null)
            {
                throw new Exception("У вас нет кошелька с таким типов валюты. Деньги снять невозможно");
            }
            return amount.Value;
        }

        // получить текущий счет по валюте
        private decimal? GetAmount(string currency)
        {
            decimal? amount = _walletStore.GetMoneyByUser(_userName)
                ?.FirstOrDefault(r => r.Currency.Equals(currency))?.Amount;
            return amount;
        }

        // проверка существования польователя в БД
        private void IsUserExists()
        {
            if (!_walletStore.IsUserExists(_userName))
            {
                throw new Exception($"Пользователь {_userName} не найден");
            }
        }
    }
}
