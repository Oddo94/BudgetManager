using BudgetManager.mvp.models;
using BudgetManager.mvp.views;
using System;
using System.Collections.Generic;
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

        public ExternalAccountStatisticsPresenter(IExternalAccountStatisticsView accountStatisticsView, IExternalAccountStatisticsRepository accountStatisticsRepository)
        {
            this.accountStatisticsView = accountStatisticsView;
            this.accountStatisticsRepository = accountStatisticsRepository;

            this.userAccountsBindingSource = new BindingSource();
            this.accountStatisticsBindingSource = new BindingSource();

            this.accountStatisticsView.loadUserAccountsEvent += getUserAccounts;
            this.accountStatisticsView.displayAccountStatisticsEvent += getAccountStatistics;
            this.accountStatisticsView.setControlsBindingSource(userAccountsBindingSource, accountStatisticsBindingSource);
        }

        private void getUserAccounts(object sender, EventArgs e)
        {
            userAccountsBindingSource.DataSource = new List<string>() { "Test account 1", "Test account 2", "Test account 3", "Test account 4" };
        }

        private void getAccountStatistics(object sender, EventArgs e)
        {
            accountStatisticsBindingSource.DataSource = new ExternalAccountDetailsModel("Test Razvan", "Banca Romaneasca", DateTime.Now, 10000, 4000, 3000, 0, 999);
        }

 
    }
}
