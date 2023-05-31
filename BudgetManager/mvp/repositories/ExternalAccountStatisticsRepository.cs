using BudgetManager.mvp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.repositories
{
    internal class ExternalAccountStatisticsRepository : IExternalAccountStatisticsRepository
    {
        public ExternalAccountDetailsModel getAccountDetails(string accountName, int userId)
        {
            throw new NotImplementedException();
        }

        public ExternalAccountDetailsModel getAccountDetailsById(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
