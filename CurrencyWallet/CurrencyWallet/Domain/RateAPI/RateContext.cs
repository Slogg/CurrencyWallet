using CurrencyWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyWallet.Domain.RateAPI
{
    public class RateContext
    {
        private IRateStrategy _rateStrategy;

        public RateContext(IRateStrategy rateStrategy)
        {
            _rateStrategy = rateStrategy;
        }

        /// <summary>
        /// Запустить обновление для получения валют
        /// </summary>
        public IReadOnlyList<RateModel> Update => _rateStrategy.Update();
    }
}
