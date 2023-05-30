using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.models
{
    internal interface IExternalAccountStatisticsRepository
    {
        ExternalAccountDetailsModel getAccountDetails(String accountName, int userId);
        ExternalAccountDetailsModel getAccountDetailsById(int accountId);

    }
}
