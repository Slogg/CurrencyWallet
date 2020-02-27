using CurrencyWallet.Models;
using System.Collections.Generic;

namespace CurrencyWallet.DAL
{
    public interface IRateStore
    {
        IReadOnlyList<RateModel> RateDict { get; }

        decimal GetRate(string currency);
    }
}
