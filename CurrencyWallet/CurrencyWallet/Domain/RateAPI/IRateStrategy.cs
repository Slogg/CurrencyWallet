using CurrencyWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyWallet.Domain.RateAPI
{
    internal interface IRateStrategy
    {
        IReadOnlyList<RateModel> Update();
    }
}
