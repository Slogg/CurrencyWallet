using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyWallet.Models
{
    /// <summary>
    /// Модель валюты
    /// </summary>
    internal sealed class RateModel
    {
        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Курс
        /// </summary>
        public decimal Value { get; set; }
    }
}
