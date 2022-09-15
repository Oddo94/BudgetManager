using BudgetManager.utils.enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils {
    class GeneralInsertionCheckStrategy : DataInsertionCheckStrategy {

        //SQL queries for getting the total value of budget elements for a month in order to allow further checks
        private String sqlStatementSingleMonthIncomes = @"SELECT SUM(value) from incomes WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthExpenses = @"SELECT SUM(value) from expenses WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthDebts = @"SELECT SUM(value) from debts WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementSingleMonthSavings = @"SELECT SUM(value) from savings WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";

        //SQL query to get the saving account current balance value in order to allow further checks when the user selects the saving account as the income source for the inserted expense
        //Currently the check is made to allow the balance retrieval only for the default saving account(typeName = SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT)
        private String sqlStatementGetSavingAccountBalance = @"SELECT SUM(value) FROM saving_accounts_balance sab
                                                               INNER JOIN saving_accounts sa on sab.account_ID = sa.accountID
                                                               INNER JOIN saving_account_types sat on sa.type_ID = sat.typeID
                                                               WHERE sab.user_ID = @paramID AND sat.typeID = 1";


        public int performCheck(QueryData paramContainer, String selectedItemName, int valueToInsert) {
            /****SAVING ACCOUNT SOURCE****/
            if (paramContainer.IncomeSource == IncomeSource.SAVING_ACCOUNT) {
                if (!hasEnoughMoney(IncomeSource.SAVING_ACCOUNT, valueToInsert, paramContainer)) {
                    MessageBox.Show("The inserted value is higher than the money left in the saving account! You cannot exceed the currently available balance of the saving account.", "Data insertion", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    return -1;
                }

                //executionResult = insertSelectedItem(selectedItemIndex);

            } else if (paramContainer.IncomeSource == IncomeSource.GENERAL_INCOMES) {
                /****GENERAL INCOMES SOURCE****/
                //GENERAL CHECK(item value(general expense, debt, saving) > available amount)
                //Checks if the inserted item value is greater than the amount of money left 
                if (!hasEnoughMoney(IncomeSource.GENERAL_INCOMES, valueToInsert, paramContainer)) {
                    MessageBox.Show(String.Format("The inserted value for the current {0} is higher than the money left! You cannot exceed the maximum incomes for the current month.", selectedItemName.ToLower()), "Data insertion", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    return -1;
                }
            }

            return 0;
        }

        private bool hasEnoughMoney(IncomeSource incomeSource, int valueToInsert, QueryData paramContainer) {
            if (incomeSource == IncomeSource.GENERAL_INCOMES) {
                //Getting the total value for each budget element        
                int totalIncomes = getTotalValueForSelectedElement(BudgetItemType.INCOME, sqlStatementSingleMonthIncomes, paramContainer);
                //int totalExpenses = getTotalValueForSelectedElement(BudgetItemType.EXPENSE, sqlStatementSingleMonthExpenses, paramContainer);
                int totalExpenses = getTotalValueForSelectedElement(BudgetItemType.GENERAL_EXPENSE, sqlStatementSingleMonthExpenses, paramContainer);
                int totalDebts = getTotalValueForSelectedElement(BudgetItemType.DEBT, sqlStatementSingleMonthDebts, paramContainer);
                int totalSavings = getTotalValueForSelectedElement(BudgetItemType.SAVING, sqlStatementSingleMonthSavings, paramContainer);

                //Calculating the amount left to spend
                int amountLeft = getAvailableAmount(totalIncomes, totalExpenses, totalDebts, totalSavings);

                if (valueToInsert <= amountLeft) {
                    return true;
                }

            } else if (incomeSource == IncomeSource.SAVING_ACCOUNT) {
                //Getting the current balance of the saving acount
                int currentBalance = getSavingAccountCurrentBalance(sqlStatementGetSavingAccountBalance, paramContainer);

                if (valueToInsert <= currentBalance) {
                    return true;
                }
            }

            return false;
        }

        //Method that gets the total value of the selected element for the specified month
        private int getTotalValueForSelectedElement(BudgetItemType itemType, String sqlStatement, QueryData paramContainer) {
            int totalValue = 0;

            //Getting the correct SQL comand for the selected element
            MySqlCommand command = getCommand(itemType, sqlStatement, paramContainer);

            if (command == null) {
                return -1;
            }

            //Getting the data based on the previously created command
            DataTable resultDataTable = DBConnectionManager.getData(command);

            //Checking if the DataTable contains data and if so converting the value to int
            if (resultDataTable != null && resultDataTable.Rows.Count == 1) {
                Object result = resultDataTable.Rows[0].ItemArray[0];
                totalValue = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                return totalValue;
            }

            return -1;

        }

        //Method for retrieving the total saving amount
        private int getSavingAccountCurrentBalance(String sqlStatement, QueryData paramContainer) {
            //Setting the default value for current balance.If data cannot be retrieved for any reason then 0 will be returned since it is not be allowed for the saving account to have negative balance
            int currentBalance = 0;

            MySqlCommand getCurrentBalanceCommand = SQLCommandBuilder.getRecordSumValueCommand(sqlStatementGetSavingAccountBalance, paramContainer);
            //getCurrentBalanceCommand.Parameters.AddWithValue("@paramID", paramContainer.UserID);
            //getCurrentBalanceCommand.Parameters.AddWithValue("@paramYear", paramContainer.Year);

            DataTable resultDataTable = DBConnectionManager.getData(getCurrentBalanceCommand);

            if (resultDataTable != null && resultDataTable.Rows.Count == 1) {
                Object result = resultDataTable.Rows[0].ItemArray[0];
                currentBalance = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                return currentBalance;
            }

            return currentBalance;
        }

        //Method that returns the correct SQL command according to the type of selected item 
        private MySqlCommand getCommand(BudgetItemType itemType, String sqlStatement, QueryData paramContainer) {
            switch (itemType) {
                case BudgetItemType.INCOME:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                //CHANGE!!!(from EXPENSE TO GENERAL_EXPENSE)
                case BudgetItemType.GENERAL_EXPENSE:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.DEBT:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                case BudgetItemType.SAVING:
                    return SQLCommandBuilder.getSingleMonthCommand(sqlStatement, paramContainer);

                default:
                    return null;
            }
        }

        //Method for calculating the amount left to spend
        private int getAvailableAmount(int totalIncomes, int totalExpenses, int totalDebts, int totalSavings) {

            return totalIncomes - (totalExpenses + totalDebts + totalSavings);

        }

    }
}
