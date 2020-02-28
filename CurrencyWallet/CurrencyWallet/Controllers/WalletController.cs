using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyWallet.DAL;
using CurrencyWallet.Domain;
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
        public async Task<IActionResult> GetAllMoneyUser(string userName)
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
    }
}