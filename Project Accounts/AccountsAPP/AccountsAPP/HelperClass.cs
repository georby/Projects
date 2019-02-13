using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountsAPP
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        All = 3,
        Invalid = 4
    }
    public class Response
    {
        public int AccountNumber { get; set; }
        public bool Successful { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class Account
    {
        public int AccountNumber { get; set; }
        public string HolderName { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public decimal ConversionRateToDollar { get; set; }
    }
    public class TransactionLog
    {
        public int TransactionId { get; set; }
        public int AccountNumber { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ConversionRateToDollar { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
    }
}