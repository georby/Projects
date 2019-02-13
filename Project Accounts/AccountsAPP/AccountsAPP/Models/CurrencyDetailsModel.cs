using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountsAPP.Models
{
    public class CurrencyDetailsModel
    {
        AccountAPP_DB_ModelDataContext dbContext;
        public CurrencyDetailsModel()
        {
            dbContext = new AccountAPP_DB_ModelDataContext();
        }
        public List<Currency> GetAllCurrencies()
        {
            List<Currency> currencies = new List<Currency>();
            Currency currency;
            try
            {
                foreach (var item in dbContext.CurrencyDetails.ToList())
                {
                    currency = new Currency();
                    currency.ConversionRateToDollar = item.conversionRateToDollar;
                    currency.CurrencyCode = item.currencyCode.Trim();
                    currency.CurrencyId = item.currencyId;
                    currency.CurrencyName = item.currencyName.Trim();
                    currencies.Add(currency);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return currencies;
        }
        public Currency GetCurrency(Currency currency)
        {
            currency = new Currency();
            try
            {
                var currencyDetail = dbContext.CurrencyDetails.FirstOrDefault(x => x.currencyId == currency.CurrencyId);
                if (currencyDetail != null)
                {
                    currency.ConversionRateToDollar = currencyDetail.conversionRateToDollar;
                    currency.CurrencyCode = currencyDetail.currencyCode.Trim();
                    currency.CurrencyId = currencyDetail.currencyId;
                    currency.CurrencyName = currencyDetail.currencyName.Trim();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return currency;
        }

    }
}