using AccountsAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AccountsAPP.Controllers
{
    public class TransactionController : ApiController
    {
        #region Auto Generated Code by VS
        //// GET: api/Transaction
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Transaction/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Transaction
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Transaction/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Transaction/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        TransactionLogModel transactionDetail;
        TransactionLog transaction;

        // api/transaction/getalltransactionbyaccountnumber
        [HttpGet]
        public IEnumerable<TransactionLog> GetAllTransactionByAccountNumber()
        {
            transactionDetail = new TransactionLogModel();
            transaction = new TransactionLog();
            var requestParams = HttpContext.Current.Request.Params;
            if (!string.IsNullOrEmpty(requestParams["AccountNumber"]))
            {
                transaction.AccountNumber = Convert.ToInt32(requestParams["AccountNumber"].Trim());
                return transactionDetail.GetAllTransactionsByAccountID(transaction);
            }
            else
                return new List<TransactionLog>();
        }
    }
}
