using AccountsAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AccountsAPP.Controllers
{
    public class CurrencyController : ApiController
    {
        #region Auto Generated Code by VS
        //// GET: api/Currency
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Currency/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Currency
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Currency/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Currency/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        CurrencyDetailsModel currencyDetails;

        // api/currency/getallcurrencies
        [HttpGet]
        public IEnumerable<Currency> GetAllCurrencies()
        {
            currencyDetails = new CurrencyDetailsModel();
            return currencyDetails.GetAllCurrencies();
        }
    }
}
