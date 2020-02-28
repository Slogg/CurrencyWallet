using CurrencyWallet.Domain.RateAPI;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyWallet.DAL
{
    public sealed class RateStore : IRateStore
    {
        public IReadOnlyList<RateModel> RateDict { get; private set; }

        public RateStore(RateContext rateContext)
        {
            RateDict = rateContext.Update;
        }

        /// <summary>
        /// Получить валюту со значением
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public decimal GetRate(string currency)
        {
            return RateDict.First(x => x.Currency.Equals(currency)).Amount;
        }
    }
}
