using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyWallet.DAL;
using CurrencyWallet.Domain;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyWallet.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private IWalletStore _walletStore;
        private IRateValidate _rateValidate;
        private IRateStore _rateStore;

        public WalletController(IWalletStore walletStore, IRateStore rateStore, IRateValidate rateValidate)
        {
            _walletStore = walletStore;
            _rateValidate = rateValidate;
            _rateStore = rateStore;
        }

        /// <summary>
        /// Получить все данные с кошельков пользователя
        /// </summary>
        /// <param name="userName">пользователь</param>
        [HttpPost]
        public IActionResult GetAllMoneyUser(string userName)
        {
            try
            {
                var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, userName);
                var result = wh.GetAllMoneyUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Снять деньги с кошелька
        /// </summary>
        /// <param name="userName">пользователь</param>
        /// <param name="rate">валюта</param>
        [HttpPost]
        public IActionResult ReplenishWallet(string userName, RateModel rate)
        {
            try
            {
                var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, userName);
                var result = wh.WithdrawModey(rate);
                return Ok($"Сумма успешно зачислена. Остаток: {result} {rate.Currency}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Снять деньги с кошелька
        /// </summary>
        /// <param name="userName">пользователь</param>
        /// <param name="rate">валюта</param>
        [HttpPost]
        public IActionResult WithdrawModey(string userName, RateModel rate)
        {
            try
            {
                var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, userName);
                var result = wh.WithdrawModey(rate);
                return Ok($"Сумма успешно снята. Остаток: {result} {rate.Currency}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Выполнить обмен валют
        /// </summary>
        /// <param name="userName">пользователь</param>
        /// <param name="rateFrom">перевос с валюты</param>
        /// <param name="rateTo">перевод на валюту</param>        
        [HttpPost]
        public IActionResult CurrencyExchange(string userName, RateModel rateFrom, RateModel rateTo)
        {
            try
            {
                var wh = new WalletHandling(_walletStore, _rateStore, _rateValidate, userName);
                return Ok($"Обмен между валютами: {rateFrom.Currency} - {rateTo.Currency} прошёл успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}