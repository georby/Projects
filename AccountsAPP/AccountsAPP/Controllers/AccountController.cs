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
    public class AccountController : ApiController
    {
        #region Auto Generated Code by VS
        //// GET: api/Account
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Account/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Account
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Account/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Account/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        Account account;
        Currency currency;
        TransactionLog transactionLog;
        AccountDetailsModel accountDetails;
        CurrencyDetailsModel currencyDetails = new CurrencyDetailsModel();

        // api/account/balance/5
        [HttpGet]
        public Account Balance(int accountNumber)
        {
            account = new Account();
            accountDetails = new AccountDetailsModel();
            account.AccountNumber = accountNumber;
            return accountDetails.GetAccount(account);
        }

        // api/account/getallaccounts
        [HttpPost]
        public IEnumerable<Account> GetAllAccounts()
        {
            accountDetails = new AccountDetailsModel();
            return accountDetails.GetAllAccounts();
        }

        // api/account/deposit
        [HttpPost]
        public Response Deposit()
        {
            return ValidateAndUpdate(TransactionType.Deposit);
        }

        // api/account/withdraw
        [HttpPost]
        public Response Withdraw()
        {
            return ValidateAndUpdate(TransactionType.Withdraw);
        }



        private Response ValidateAndUpdate(TransactionType transactionType)
        {
            var requestParams = HttpContext.Current.Request.Params;
            if (!string.IsNullOrEmpty(requestParams["AccountNumber"].Trim()))
            {
                account = new Account();
                account.AccountNumber = Convert.ToInt32(requestParams["AccountNumber"].Trim());
                if (!string.IsNullOrEmpty(requestParams["Amount"].Trim()))
                {
                    transactionLog = new TransactionLog();
                    decimal amount = transactionLog.TransactionAmount = Convert.ToDecimal(requestParams["Amount"].Trim());
                    if (!string.IsNullOrEmpty(requestParams["CurrencyId"].Trim()))
                    {
                        currency = new Currency();
                        currency.CurrencyId = Convert.ToInt32(requestParams["CurrencyId"].Trim());
                        var currencyInfo = currencyDetails.GetCurrency(currency);
                        account.AccountBalance = Convert.ToDecimal(amount * currencyInfo.ConversionRateToDollar);
                        transactionLog.CurrencyCode = currencyInfo.CurrencyCode;
                        transactionLog.ConversionRateToDollar = currencyInfo.ConversionRateToDollar;
                        return UpdateAmount(account, transactionLog, transactionType);
                    }
                    else
                        return SetFailedResponseMessage("Invalid Currency !!!");
                }
                else
                    return SetFailedResponseMessage("Invalid Amount !!!");
            }
            else
                return SetFailedResponseMessage("Invalid Account Number !!!");
        }

        private static Response SetFailedResponseMessage(string message)
        {
            Response response = new Response();
            response.Successful = false;
            response.Message = message;
            return response;
        }

        private Response UpdateAmount(Account account, TransactionLog transactionLog, TransactionType transactionType)
        {
            accountDetails = new AccountDetailsModel();
            return accountDetails.UpdateAccountBalance(account, transactionLog, transactionType);
        }
    }
}
