using BudgetManager.mvp.misc;
using BudgetManager.mvp.models;
using BudgetManager.mvp.views;
using BudgetManager.utils.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BudgetManager.mvp.presenters
{
    internal class ExternalAccountStatisticsPresenter {
        //Views
        private IExternalAccountStatisticsView accountStatisticsView;
        private IExternalAccountStatisticsRepository accountStatisticsRepository;

        //Binding sources
        private BindingSource userAccountsBindingSource;
        private BindingSource accountStatisticsBindingSource;
        private BindingSource accountTransfersOrInterestsBindingSource;
        private BindingSource accountTransfersActivityBindingSource;
        private BindingSource monthlyAccountBalanceBindingSource;

        public ExternalAccountStatisticsPresenter(IExternalAccountStatisticsView accountStatisticsView, IExternalAccountStatisticsRepository accountStatisticsRepository) {
            this.accountStatisticsView = accountStatisticsView;
            this.accountStatisticsRepository = accountStatisticsRepository;

            this.userAccountsBindingSource = new BindingSource();
            this.accountStatisticsBindingSource = new BindingSource();
            this.accountTransfersOrInterestsBindingSource = new BindingSource();
            this.accountTransfersActivityBindingSource = new BindingSource();
            this.monthlyAccountBalanceBindingSource = new BindingSource();

            this.accountStatisticsView.loadUserAccountsEvent += getUserAccounts;
            this.accountStatisticsView.displayAccountStatisticsEvent += getAccountStatistics;
            this.accountStatisticsView.displayAccountTransfersEvent += getCorrectAccountInfoToDisplay;
            this.accountStatisticsView.displayAccountInterestsEvent += getCorrectAccountInfoToDisplay;
            this.accountStatisticsView.displayAccountTransfersActivityEvent += getAccountTransfersActivity;
            this.accountStatisticsView.displayAccountBalanceMonthlyEvolutionEvent += getAccountBalanceMonthlyEvolution;
            this.accountStatisticsView.setControlsBindingSource(userAccountsBindingSource, accountStatisticsBindingSource, accountTransfersOrInterestsBindingSource, accountTransfersActivityBindingSource, monthlyAccountBalanceBindingSource);
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
            accountStatisticsBindingSource.DataSource = externalAccountDetails;
        }

        private void getCorrectAccountInfoToDisplay(object sender, EventArgs e) {
            BudgetItemType budgetItemType = accountStatisticsView.selectedItemType;

            switch (budgetItemType) {
                case BudgetItemType.ACCOUNT_TRANSFER:
                    DataTable accountTransfersDT = getAccountTransfers(sender, e);
                    accountTransfersOrInterestsBindingSource.DataSource = accountTransfersDT;
                    break;

                case BudgetItemType.SAVING_ACCOUNT_INTEREST:
                    DataTable accountInterestsDT = getAccountInterests(sender, e);
                    accountTransfersOrInterestsBindingSource.DataSource = accountInterestsDT;
                    break;

                default:
                    return;
            }
        }

        private DataTable getAccountTransfers(object sender, EventArgs e) {
            DataTable accountTransfersDT = accountStatisticsRepository.getAccountTransfers(accountStatisticsView.accountName, accountStatisticsView.userId, accountStatisticsView.startDate, accountStatisticsView.endDate);
            
            return accountTransfersDT;
        }

        private DataTable getAccountInterests(object sender, EventArgs e) {
            DataTable accountInterestsDT = accountStatisticsRepository.getAccountInterests(accountStatisticsView.accountName, accountStatisticsView.userId, accountStatisticsView.startDate, accountStatisticsView.endDate);
            
            return accountInterestsDT;
        }

        private void getAccountTransfersActivity(object sender, EventArgs e) {
            DataTable accountTransfersActivityDT = accountStatisticsRepository.getAccountTransfersActivity(accountStatisticsView.accountName, accountStatisticsView.userId, accountStatisticsView.transfersActivityYear);
            accountTransfersActivityBindingSource.DataSource = accountTransfersActivityDT;

        }

        private void getAccountBalanceMonthlyEvolution(object sender, EventArgs e) {
            DataTable accountBalanceMonthlyEvolutionDT = accountStatisticsRepository.getAccountMonthlyBalanceEvolution(accountStatisticsView.accountName, accountStatisticsView.userId, accountStatisticsView.monthlyAccountBalanceYear);
            monthlyAccountBalanceBindingSource.DataSource = accountBalanceMonthlyEvolutionDT;
        }

 
    }
}
