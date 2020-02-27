using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyWallet.Models
{
    /// <summary>
    /// Кошелек пользователя
    /// </summary>
    public sealed class WalletModel
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public IdentityUser User { get; set; }

        /// <summary>
        /// Деньги пользователя в различных валютах
        /// </summary>
        public List<RateModel> Money { get; set; }
    }
}
