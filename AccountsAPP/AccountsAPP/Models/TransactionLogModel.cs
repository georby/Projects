using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountsAPP.Models
{
    public class TransactionLogModel
    {
        AccountAPP_DB_ModelDataContext dbContext;
        public TransactionLogModel()
        {
            dbContext = new AccountAPP_DB_ModelDataContext();
        }
        public bool InsertTransactionLog(TransactionLog transactionLog)
        {
            TransactionLogDetails transaction = new TransactionLogDetails();
            transaction.accountId = transactionLog.AccountNumber;
            transaction.addedBy = transactionLog.AddedBy;
            transaction.addedOn = transactionLog.AddedOn;
            transaction.balanceAfter = transactionLog.BalanceAfter;
            transaction.balanceBefore = transactionLog.BalanceBefore;
            transaction.ConversionRateToDollar = transactionLog.ConversionRateToDollar;
            transaction.currencyCode = transactionLog.CurrencyCode.Trim();
            transaction.transactionAmount = transactionLog.TransactionAmount;
            transaction.transactionType = transactionLog.TransactionType.Trim();
            try
            {
                dbContext.TransactionLogDetails.InsertOnSubmit(transaction);
                dbContext.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<TransactionLog> GetAllTransactions()
        {
            try
            {
                List<TransactionLog> transactions = dbContext.TransactionLogDetails
                    .Select(x => new TransactionLog()
                    {
                        AccountNumber = x.accountId,
                        AddedOn = x.addedOn,
                        AddedBy = x.addedBy,
                        BalanceAfter = x.balanceAfter,
                        BalanceBefore = x.balanceBefore,
                        ConversionRateToDollar = x.ConversionRateToDollar,
                        CurrencyCode = x.currencyCode.Trim(),
                        TransactionAmount = x.transactionAmount,
                        TransactionId = x.transactionId,
                        TransactionType = x.transactionType.Trim()
                    })
                    .ToList();
                return transactions;
            }
            catch (Exception)
            {
                return new List<TransactionLog>();
            }
        }
        public IEnumerable<TransactionLog> GetAllTransactionsByAccountID(TransactionLog transactionLog)
        {
            try
            {
                var transactions = dbContext.TransactionLogDetails
                    .Select(x => new TransactionLog()
                    {
                        AccountNumber = x.accountId,
                        AddedOn = x.addedOn,
                        AddedBy = x.addedBy,
                        BalanceAfter = x.balanceAfter,
                        BalanceBefore = x.balanceBefore,
                        ConversionRateToDollar = x.ConversionRateToDollar,
                        CurrencyCode = x.currencyCode.Trim(),
                        TransactionAmount = x.transactionAmount,
                        TransactionId = x.transactionId,
                        TransactionType = x.transactionType.Trim()
                    })
                    .Where(x => x.AccountNumber == transactionLog.AccountNumber)
                    .AsEnumerable();
                return transactions;
            }
            catch (Exception)
            {
                return new List<TransactionLog>();
            }
        }
        public IEnumerable<TransactionLog> GetAllTransactionsByAccountIDAndTransactionType(TransactionLog transactionLog, TransactionType transactionType)
        {
            try
            {
                string transactionTypeText = string.Empty;
                List<TransactionLog> transactions = dbContext.TransactionLogDetails
                    .Where(x => x.accountId == transactionLog.AccountNumber &&
                        x.transactionType == ((transactionType == TransactionType.Deposit) ? "deposit" : "withdraw"))
                    .Select(x => new TransactionLog()
                    {
                        AccountNumber = x.accountId,
                        AddedOn = x.addedOn,
                        AddedBy = x.addedBy,
                        BalanceAfter = x.balanceAfter,
                        BalanceBefore = x.balanceBefore,
                        ConversionRateToDollar = x.ConversionRateToDollar,
                        CurrencyCode = x.currencyCode,
                        TransactionAmount = x.transactionAmount,
                        TransactionId = x.transactionId,
                        TransactionType = x.transactionType
                    })
                    .ToList();
                return transactions;
            }
            catch (Exception)
            {
                return new List<TransactionLog>();
            }
        }

    }
}