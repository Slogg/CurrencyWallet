using CurrencyWallet.Models;

namespace CurrencyWallet.Domain
{
    public interface IRateValidate
    {
        void CheckCurrencyInDictionry(string currency);
        void CheckRateValueInDictionry(RateModel rate);
        void IsUnsigned(decimal amount);
    }
}