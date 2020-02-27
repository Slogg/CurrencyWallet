using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using CurrencyWallet.Models;

namespace CurrencyWallet.Domain.RateAPI
{
    internal class Eurofxref : IRateStrategy
    {
        private string _adress = @"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        public IReadOnlyList<Rate> Update()
        {
            List<Rate> rates = new List<Rate>();

            var doc = new XmlDocument();
            doc.Load(_adress);

            XmlNodeList nodes = doc.SelectNodes("//*[@currency]");

            if (nodes != null)
            {
                rates = nodes.Cast<XmlNode>().Select(nd => new Rate
                {
                    Currency = nd.Attributes["currency"].Value,
                    Value = decimal.Parse(nd.Attributes["rate"].Value)
                }).ToList();
            }
            else
            {
                throw new NullReferenceException("Не удалось получить данные по валютам");
            }

            return rates;
        }
    }
}
