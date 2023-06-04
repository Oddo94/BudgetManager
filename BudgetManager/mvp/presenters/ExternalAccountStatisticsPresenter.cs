using BudgetManager.mvp.misc;
using BudgetManager.mvp.models;
using BudgetManager.mvp.views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.mvp.presenters
{
    internal class ExternalAccountStatisticsPresenter {
        private IExternalAccountStatisticsView accountStatisticsView;
        private IExternalAccountStatisticsRepository accountStatisticsRepository;
        private BindingSource userAccountsBindingSource;
        private BindingSource accountStatisticsBindingSource;

        public ExternalAccountStatisticsPresenter(IExternalAccountStatisticsView accountStatisticsView, IExternalAccountStatisticsRepository accountStatisticsRepository) {
            this.accountStatisticsView = accountStatisticsView;
            this.accountStatisticsRepository = accountStatisticsRepository;

            this.userAccountsBindingSource = new BindingSource();
            this.accountStatisticsBindingSource = new BindingSource();

            this.accountStatisticsView.loadUserAccountsEvent += getUserAccounts;
            this.accountStatisticsView.displayAccountStatisticsEvent += getAccountStatistics;
            this.accountStatisticsView.setControlsBindingSource(userAccountsBindingSource, accountStatisticsBindingSource);
        }

        private void getUserAccounts(object sender, EventArgs e) {
            ExternalAccountEventArgs externalAccountEventArgs = (ExternalAccountEventArgs) e;
            DataTable externalAccountsDT = accountStatisticsRepository.getUserAccounts(externalAccountEventArgs.UserId);
            List<string> accountNameList = externalAccountsDT
                .AsEnumerable()
                .Select(row => row.Field<String>("accountName"))
                .ToList();

            userAccountsBindingSource.DataSource = accountNameList;

        }

        private void getAccountStatistics(object sender, EventArgs e) {                 
            ExternalAccountDetailsModel externalAccountDetails = accountStatisticsRepository.getAccountDetails(accountStatisticsView.accountName, accountStatisticsView.userId);
            //ExternalAccountDetailsModel externalAccountDetails = accountStatisticsRepository.getAccountDetails("SYSTEM_DEFINED_SAVING_ACCOUNT", 3);

            accountStatisticsBindingSource.DataSource = externalAccountDetails;
        }

 
    }
}
