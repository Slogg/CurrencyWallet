using System.Collections.Generic;
using CurrencyWallet.Models;

namespace CurrencyWallet.DAL
{
    /// <summary>
    /// Хранилище кошельков
    /// </summary>
    internal interface IWalletStore
    {

        /// <summary>
        /// Добавить новую валюту
        /// </summary>
        /// <param name="userName">пользователь</param>
        /// <param name="rate">модель валюты</param>
        void AddCurrency(string userName, RateModel rate);

        /// <summary>
        /// Добавить значение валюты
        /// </summary>
        /// <param name="userName">пользователь</param>
        /// <param name="rate">модель валюты</param>
        void UpdateAmount(string userName, RateModel rate);

        /// <summary>
        /// Наполнить пользователями
        /// </summary>
        /// <param name="userName">пользователь</param>
        List<RateModel> GetMoneyByUser(string userName);
        bool IsUserExists(string userName);
    }
}