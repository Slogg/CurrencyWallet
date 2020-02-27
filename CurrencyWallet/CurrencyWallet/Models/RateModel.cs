using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyWallet.Models
{
    /// <summary>
    /// Модель валюты
    /// </summary>
    public sealed class RateModel
    {
        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public decimal Amount { get; set; }
    }
}
