using CurrencyWallet.Models;
using System.Collections.Generic;

namespace CurrencyWallet.DAL
{
    internal interface IRateStore
    {
        IReadOnlyList<RateModel> RateDict { get; }

        decimal GetRate(string currency);
    }
}
