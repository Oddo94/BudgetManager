using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.models
{
    internal interface IExternalAccountStatisticsRepository {
        DataTable getUserAccounts(int userId);
        ExternalAccountDetailsModel getAccountDetails(String accountName, int userId);
        ExternalAccountDetailsModel getAccountDetailsById(int accountId);
        DataTable getAccountTransfers(String accountName, int userId, String startDate, String endDate);
        DataTable getAccountTransfersActivity(String accountName, int userId, int transfersActivityYear);
        DataTable getAccountMonthlyBalanceEvolution(String accountName, int userId, int monthlyAccountBalanceYear);

    }
}
