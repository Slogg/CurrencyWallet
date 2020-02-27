﻿using CurrencyWallet.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyWallet.DAL
{
    /// <summary>
    /// <see cref="IWalletStore"/>
    /// </summary>
    public sealed class WalletStore : IWalletStore
    {
        private List<WalletModel> _wallets;

        public WalletStore()
        {
            LazyWallets();
        }

        /// <summary>
        /// <see cref="IWalletStore.UpdateAmount(string, RateModel)"/>
        /// </summary>
        public void UpdateAmount(string userName, RateModel rate)
        {
            var userMoney = GetMoneyByUser(userName);
            userMoney.First(w => w.Currency.Contains(rate.Currency)).Amount = rate.Amount;
        }

        /// <summary>
        /// <see cref="IWalletStore.AddCurrency(string, RateModel)"/>
        /// </summary>
        public void AddCurrency(string userName, RateModel rate)
        {
            var userMoney = GetMoneyByUser(userName);
            userMoney.Add(rate);
        }

        /// <summary>
        /// <see cref="IWalletStore.GetMoneyByUser(string)"/>
        /// </summary>
        public List<RateModel> GetMoneyByUser(string userName)
        {
            return _wallets.First(x => x.User.Equals(userName)).Money;
        }

        public bool IsUserExists(string userName)
        {
            return _wallets.Any(x => x.User.UserName.Equals(userName));
        }

        // Наполнить пользователями
        private void LazyWallets()
        {
            _wallets = new List<WalletModel>()
            {
                new WalletModel() { User = new IdentityUser("пользователь1"), Money = new List<RateModel>() },
                new WalletModel() { User = new IdentityUser("пользователь2"), Money = new List<RateModel>() }
            };
        }
    }
}
