using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace AccountsAPP.Models
{
    public class AccountDetailsModel
    {
        AccountAPP_DB_ModelDataContext dbContext;

        public AccountDetailsModel()
        {
            dbContext = new AccountAPP_DB_ModelDataContext();
        }

        public List<Account> GetAllAccounts()
        {
            try
            {
                List<Account> accounts = new List<Account>();
                Account account;
                var accountDetils = dbContext.AccountDetails.ToList();

                if (accountDetils.Count > 0)
                {
                    foreach (var item in accountDetils)
                    {
                        account = new Account();
                        account.AccountBalance = item.accountBalance;
                        account.AccountNumber = item.accountId;
                        account.HolderName = item.holderName.Trim();
                        account.ModifiedOn = item.modifiedOn;
                        accounts.Add(account);
                    }
                }
                return accounts;

            }
            catch (Exception)
            {
                return new List<Account>();
            }
        }

        public Account GetAccount(Account account)
        {
            try
            {
                var accountDetail = dbContext.AccountDetails.FirstOrDefault(x => x.accountId == account.AccountNumber);
                account = new Account();
                if (accountDetail != null)
                {
                    account.AccountBalance = accountDetail.accountBalance;
                    account.AccountNumber = accountDetail.accountId;
                    account.HolderName = accountDetail.holderName.Trim();
                    account.ModifiedOn = accountDetail.modifiedOn;
                }
                return account;

            }
            catch (Exception)
            {

                return new Account();
            }
        }

        public Response UpdateAccountBalance(Account account, TransactionLog transaction, TransactionType transactionType)
        {
            Response response = new Response();
            try
            {
                var accountDetail = dbContext.AccountDetails.FirstOrDefault(x => x.accountId == account.AccountNumber);
                if (accountDetail != null)
                {
                    transaction.AccountNumber = accountDetail.accountId;
                    transaction.AddedBy = accountDetail.holderName;
                    transaction.BalanceBefore = accountDetail.accountBalance;
                    if (transactionType == TransactionType.Deposit)
                    {
                        accountDetail.accountBalance += account.AccountBalance;
                        transaction.TransactionType = "Deposit";
                    }
                    else if (transactionType == TransactionType.Withdraw)
                    {
                        if (accountDetail.accountBalance - account.AccountBalance > 0)
                        {
                            accountDetail.accountBalance -= account.AccountBalance;
                            transaction.TransactionType = "Withdraw";
                        }
                        else
                        {
                            response.Successful = false;
                            response.Message = "Account Balance Cannot Reach Below 0 !!!";
                            return response;
                        }
                    }
                    transaction.BalanceAfter = response.Balance = accountDetail.accountBalance;
                    accountDetail.modifiedOn = transaction.AddedOn = response.ModifiedOn = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", System.Globalization.CultureInfo.InvariantCulture);
                    try
                    {
                        dbContext.SubmitChanges(ConflictMode.ContinueOnConflict);
                        TransactionLogModel transactionLog = new TransactionLogModel();
                        if (transactionLog.InsertTransactionLog(transaction))
                        {
                            response.Successful = true;
                            response.Message = "Success !!! Account Balance Updated.";
                            return response;
                        }
                        else
                        {
                            response.Successful = true;
                            response.Message = "Success !!! Account Balance Updated." 
                                + Environment.NewLine + "But, Something went wrong while logging the transaction.";
                            return response;
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict item in dbContext.ChangeConflicts)
                        {
                            item.Resolve(RefreshMode.OverwriteCurrentValues);
                        }
                        return UpdateAccountBalance(account, transaction, transactionType);
                    }
                }
                response.Successful = false;
                response.Message = "Oops !!! Something went wrong while updating balance. Please Try Again.";
                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Oops !!! Something went wrong while updating balance. Please Try Again."
                    + Environment.NewLine + ex.Message;
                return response;
            }
        }

    }
}