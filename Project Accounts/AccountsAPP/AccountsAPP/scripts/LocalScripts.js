var apiList = new Object();
apiList.balance = 'api/account/balance';
apiList.getallaccounts = 'api/account/getallaccounts';
apiList.deposit = 'api/account/deposit';
apiList.withdraw = 'api/account/withdraw';
apiList.getallcurrencies = 'api/currency/getallcurrencies';
apiList.getalltransactionbyaccountnumber = 'api/transaction/getalltransactionbyaccountnumber';

function GetAllAccountsToDDL(accountDdlID) {
    $(accountDdlID).empty();
    $(accountDdlID).append('<option value="0"> ---- Select Account ---- </option>');
    $.ajax({
        url: apiList.getallaccounts,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            var accounts = [];
            $.each(data, function (key, item) {
                $(accountDdlID).append('<option value=' + item.AccountNumber + '>' + item.AccountNumber + ' . ' + item.HolderName + '</option>');
                var accountDetail = new Object();
                accountDetail.AccountBalance = item.AccountBalance;
                accountDetail.AccountNumber = item.AccountNumber;
                accountDetail.HolderName = item.HolderName;
                accountDetail.ModifiedOn = item.ModifiedOn;
                accounts.push(accountDetail);
            });
            $("#hdnAccountDetails").val(JSON.stringify(accounts));
        },
        error: function (request, message, error) {
            alert(message);
        }
    });
}

function GetAllCurranciesToDDL(currencyDdlID) {
    $(currencyDdlID).empty();
    $(currencyDdlID).append('<option value="0"> ---- Select Currency ---- </option>');
    $.ajax({
        url: apiList.getallcurrencies,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var currencies = [];
            $.each(data, function (key, item) {
                $(currencyDdlID).append('<option value=' + item.CurrencyId + '>' + item.CurrencyName + ' (' + item.CurrencyCode + ')</option>');
                var currencyDetail = new Object();
                currencyDetail.CurrencyId = item.CurrencyId;
                currencyDetail.CurrencyName = item.CurrencyName;
                currencyDetail.CurrencyCode = item.CurrencyCode;
                currencyDetail.ConversionRateToDollar = item.ConversionRateToDollar;
                currencies.push(currencyDetail);
            });
            $("#hdnCurrencyDetails").val(JSON.stringify(currencies));
        },
        error: function (request, message, error) {
            alert(message);
        }
    });
}

function GetAllLocalCurranciesToDDL(currencyDDL) {
    $(currencyDDL).empty();
    var currencies = $('#hdnCurrencyDetails').val();
    $(currencyDDL).append('<option value="0"> --- Select --- </option></select>');
    $.each(JSON.parse(currencies), function (key, item) {
        $(currencyDDL).append('<option value="' + item.CurrencyId + '">' + item.CurrencyName + ' (' + item.CurrencyCode + ')</option>');
    });
    return currencyDDL;
}

function CurrencyChange(selectedCurrencyId) {
    var currencies = $('#hdnCurrencyDetails').val();
    var currencyRateText = "";
    $.each(JSON.parse(currencies), function (key, item) {
        if (item.CurrencyId == selectedCurrencyId){
            currencyRateText = "Conversion Rate to Dollar is : " + item.ConversionRateToDollar;
        }
    });
    return currencyRateText;
}

function AccountSelected() {
    if ($('select[id=ddlAccounts]').val() !== '0') {
        $.ajax({
            url: apiList.balance,
            type: 'GET',
            data: {
                accountNumber: $('select[id=ddlAccounts]').val()
            },
            success: function (response) {
                $("#tblAccountInfo").empty();
                $("#tblAccountInfo").append('<tr><td>Current Balance is   : </td><td>$' + response.AccountBalance + '</td></tr>');
                $("#tblAccountInfo").append('<tr><td>Last Modified On   : </td><td>' + response.ModifiedOn + '</td></tr>');
                $("#divAccountActions").css("display", "block");
            },
            error: function (request, message, error) {
                alert(message);
            }
        });
    }
    else {
        $("#tblAccountInfo").empty();
        $("#divAccountBalance").text('');
        $("#divAccountModifiedOn").text('');
        $("#divAccountActions").css("display", "none");
    }
}

function SetConversionRate(selectedCurrencyId) {
    if (selectedCurrencyId != 0) {
        var currencyRateText = CurrencyChange($("#ddlCurrencies").val());
        $("#spanConversionRate").text(currencyRateText);
        $("#btnUpdateAmount").removeAttr("disabled");
    }
    else{
        $("#btnUpdateAmount").attr("disabled", true);
        $("#spanConversionRate").text("");
    }
}

function ModifyBalanceClick() {
    $("#divModifyBalace").css("display", "block");
    $("#divBulkModify").css("display", "none");
    $("#divViewTransactionHistory").css("display", "none");
    FillCurrencyDDLs();
}

function FillCurrencyDDLs() {
    var localCurrencies = $('#hdnCurrencyDetails').val();
    if (localCurrencies == "")
        GetAllCurranciesToDDL('#ddlCurrencies');
    else
        GetAllLocalCurranciesToDDL('#ddlCurrencies');
    if (localCurrencies == "")
        GetAllCurranciesToDDL('#ddlBulkModifyCurrencies');
    else
        GetAllLocalCurranciesToDDL('#ddlBulkModifyCurrencies');
}

function UpdateAmountClick() {
    var AccountNumber = $('select[id=ddlAccounts]').val();
    var currencyId = $('select[id=ddlCurrencies]').val();
    var transactionType = $('#ddlTransactionType option:selected').text();
    var amount = $('#txtDepositAmount').val();
    if (amount && amount !== null && amount !== undefined && $.isNumeric(amount) && currencyId > 0) {
        var amountDetails = new Object();
        amountDetails.AccountNumber = AccountNumber;
        amountDetails.amount = amount;
        amountDetails.currencyId = currencyId;
        var apiPath = '';
        if (transactionType == 'Deposit')
            apiPath = apiList.deposit;
        else
            apiPath = apiList.withdraw;
        UpdateAmount(amountDetails, apiPath);
    }
}

function UpdateAmount(amountDetails, apiPath) {
    $.ajax({
        url: apiPath,
        type: 'POST',
        data: {
            AccountNumber: amountDetails.AccountNumber,
            Amount: amountDetails.amount,
            CurrencyId: amountDetails.currencyId
        },
        success: function (response) {
            if (response.Successful == true) {
                alert(response.Message + '\nNew Balance is : $' + response.Balance);
                $("#tblAccountInfo").empty();
                $("#tblAccountInfo").append('<tr><td>Current Balance is   : </td><td>$' + response.Balance + '</td></tr>');
                $("#tblAccountInfo").append('<tr><td>Last Modified On   : </td><td>' + response.ModifiedOn + '</td></tr>');
                $("#divAccountActions").css("display", "block");
            }
            else {
                alert(response.Message);
            }
        },
        error: function (request, message, error) {
            alert(message);
        }
    });
}

function BulkModifyBalanceClick() {
    $("#divModifyBalace").css("display", "none");
    $("#divBulkModify").css("display", "block");
    $("#divViewTransactionHistory").css("display", "none");
    $("#tbodyBulkModify").empty();
    FillCurrencyDDLs();
}

function AddBulkModifyRow() {
    var tdTransactionType = '<td><select><option value="0">Deposit</option><option value="1">Withdraw</option></select></td>';
    var ddlCurrencies = GetAllLocalCurranciesToDDL($('<select></select>'));
    var tdCurrencyType = $('<td></td>').append(ddlCurrencies);
    var tdAmount = '<td><input type="text" /></td>';
    $("#tbodyBulkModify").append($('<tr></tr>').append(tdTransactionType).append(tdCurrencyType).append(tdAmount));
}

function UpdateBulkModify() {
    var AccountNumber = $('select[id=ddlAccounts]').val();
    var tableRows = $('#tbodyBulkModify').find('tr');
    var amountDetails = new Object();
    $.each(tableRows, function (key, tr) {
        var rowTds = $(tr).find('td');
        amountDetails.AccountNumber = $('select[id=ddlAccounts]').val();
        var transactionsDDL = rowTds[0].children[0];
        amountDetails.transactionType = $(transactionsDDL).find('option:selected').text()
        amountDetails.currencyId = $(rowTds[1].children[0]).val();
        amountDetails.amount = $(rowTds[2].children[0]).val();
        var apiPath = '';
        if (amountDetails.transactionType == 'Deposit')
            apiPath = apiList.deposit;
        else
            apiPath = apiList.withdraw;
        if (amountDetails.amount && amountDetails.amount !== null && amountDetails.amount !== undefined
            && $.isNumeric(amountDetails.amount) && amountDetails.currencyId > 0) {
            UpdateAmount(amountDetails, apiPath);
        }
    });
}

function ViewTransactionHistoryClick() {
    $("#divModifyBalace").css("display", "none");
    $("#divBulkModify").css("display", "none");
    $("#divViewTransactionHistory").css("display", "block");
}

function LoadTransactionHistory() {
    if ($('select[id=ddlAccounts]').val() !== '0') {
        $.ajax({
            url: apiList.getalltransactionbyaccountnumber,
            type: 'GET',
            data: {
                AccountNumber: $('select[id=ddlAccounts]').val()
            },
            success: function (response) {
                $("#tbodyViewTransactionHistory").empty();
                $.each(response, function (key, item) {
                    var rowStyle;
                    if (item.TransactionType == "Deposit")
                        rowStyle = " style='background-color:#6FFD59'; ";
                    else
                        rowStyle = " style='background-color:#FE7575'; ";

                    $("#tbodyViewTransactionHistory").append('<tr><td>' + item.CurrencyCode + '</td>' +
                        '<td ' + rowStyle + '>' + item.TransactionType + '</td>' +
                        '<td>' + item.TransactionAmount + '</td>' +
                        '<td>' + item.ConversionRateToDollar + '</td>' +
                        '<td>' + item.BalanceBefore + '</td>' +
                        '<td>' + item.BalanceAfter + '</td>' +
                        '<td>' + item.AddedOn + '</td>' +
                        '</tr>');
                });
            },
            error: function (request, message, error) {
                alert(message);
            }
        });
    }
}


