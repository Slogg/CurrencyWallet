using CurrencyWallet.Models;
using System.Collections.Generic;

namespace CurrencyWallet.Domain.RateAPI
{
    /// <summary>
    /// Стратегия для реализации парсинга валют
    /// </summary>
    internal interface IRateStrategy
    {
        /// <summary>
        /// Запустить обновление
        /// </summary>
        IReadOnlyList<RateModel> Update();
    }
}
