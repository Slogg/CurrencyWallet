using CurrencyWallet.Domain.RateAPI;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyWallet.DAL
{
    internal sealed class RateStore : IRateStore
    {
        public IReadOnlyList<RateModel> RateDict { get; private set; }

        private RateStore(RateContext rateContext)
        {
            RateDict = rateContext.Update;
        }
    }
}
