using CurrencyWallet.DAL;
using CurrencyWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyWallet.Domain
{
    public sealed class RateValidate : IRateValidate
    {
        private IReadOnlyList<RateModel> _rates;

        public RateValidate(IRateStore rateStore)
        {
            _rates = rateStore.RateDict;
        }

        public void CheckRateValueInDictionry(RateModel rate)
        {
            HandleCondition(() => _rates.Contains(rate), $"Поданный тип валюты({rate.Currency}, {rate.Amount}) является некорректным или неактуальным");
        }

        public void CheckCurrencyInDictionry(string currency)
        {
            HandleCondition(() => _rates.Select(x => x.Currency).Contains(currency), $"Поданный тип валюты({currency} является некорретным");
        }

        public void IsUnsigned(decimal amount)
        {
            HandleCondition(() => amount >= 0, $"Значение валюты не может быть отрицательным");
        }

        // Обработка условия. При false - выбросить исключение с заданным в аргументах сообщением
        private void HandleCondition(Func<bool> condition, string errorMsg)
        {
            if (!condition())
            {
                throw new Exception(errorMsg);
            };
        }
    }
}
