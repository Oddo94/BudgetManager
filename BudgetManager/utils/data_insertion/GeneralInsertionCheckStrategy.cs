using BudgetManager.mvc.models;
using BudgetManager.mvc.models.dto;
using BudgetManager.utils.enums;
using BudgetManager.utils.exceptions;
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

        //DEPRECATED
        //SQL query to get the saving account current balance value in order to allow further checks when the user selects the saving account as the income source for the inserted expense
        //Currently the check is made to allow the balance retrieval only for the default saving account(typeName = SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT)
        private String sqlStatementGetSavingAccountBalance = @"SELECT SUM(value) FROM saving_accounts_balance sab
                                                               INNER JOIN accounts acc ON sab.account_ID = acc.accountID
                                                               INNER JOIN account_types at ON acc.type_ID = at.typeID
                                                               WHERE sab.user_ID = @paramID 
                                                               AND at.typeName LIKE '%SYSTEM_DEFINED%'";

        //SQL query used for retrieving the account ID for which the balance check is performed
        private String sqlStatementGetAccountID = @"SELECT accountID FROM accounts WHERE accountName LIKE CONCAT('%', @paramRecordName,'%') AND user_ID = @paramID";

        public DataCheckResponse performCheck(QueryData paramContainer, String selectedItemName, int valueToInsert) {
            DataCheckResponse dataCheckResponse = new DataCheckResponse();

            /****SAVING ACCOUNT SOURCE****/
            if (paramContainer.IncomeSource == IncomeSource.SAVING_ACCOUNT) {         
                try {
                    if (!hasEnoughMoney(IncomeSource.SAVING_ACCOUNT, valueToInsert, paramContainer)) {
                        dataCheckResponse.ExecutionResult = -1;
                        dataCheckResponse.ErrorMessage = "The inserted value is higher than the money left in the saving account! You cannot exceed the currently available balance of the saving account.";

                        return dataCheckResponse;
                    }

                } catch (Exception ex) when (ex is MySqlException || ex is NoDataFoundException) {
                    //Handles exceptions occured during the retrieval of data needed to check the account balance

                    dataCheckResponse.ExecutionResult = -1;
                    dataCheckResponse.ErrorMessage = ex.Message;

                    return dataCheckResponse;
                }

            } else if (paramContainer.IncomeSource == IncomeSource.GENERAL_INCOMES) {
                /****GENERAL INCOMES SOURCE****/
                //GENERAL CHECK(item value(general expense, debt, saving) > available amount)
                //Checks if the inserted item value is greater than the amount of money left           
                if (!hasEnoughMoney(IncomeSource.GENERAL_INCOMES, valueToInsert, paramContainer)) {
                    dataCheckResponse.ExecutionResult = -1;
                    dataCheckResponse.ErrorMessage = String.Format("The inserted value for the current {0} is higher than the money left! You cannot exceed the total incomes for the current month!", selectedItemName);

                    return dataCheckResponse;
                }
            }

            dataCheckResponse.ExecutionResult = 0;
            dataCheckResponse.SuccessMessage = "Data check passed!";

            return dataCheckResponse;
        }

        public DataCheckResponse performCheck(QueryData paramContainer, String selectedItemName, double valueToInsert) {
            throw new NotImplementedException();
        }

        public DataCheckResponse performCheck() {
            throw new NotImplementedException();
        }
        private bool hasEnoughMoney(IncomeSource incomeSource, int valueToInsert, QueryData paramContainer) {
            if (incomeSource == IncomeSource.GENERAL_INCOMES) {
                //Getting the total value for each budget element        
                int totalIncomes = getTotalValueForSelectedElement(BudgetItemType.INCOME, sqlStatementSingleMonthIncomes, paramContainer);
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
                double currentBalance = getSavingAccountCurrentBalance(sqlStatementGetSavingAccountBalance, paramContainer);

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

        //Method for retrieving the balance of the saving account which is used as the income source
        private double getSavingAccountCurrentBalance(String sqlStatement, QueryData paramContainer) {
            //Setting the default value for current balance.If data cannot be retrieved for any reason then 0 will be returned since it is not be allowed for the saving account to have negative balance
            double currentBalance = 0;
            int accountID = -1 ;
            DataTable accountIDDataTable = null;

                String accountName = "SYSTEM_DEFINED";//The name of the default account(it uses only 'SYSTEM_DEFINED' as this is the defining element from its name and it's evaluated in a LIKE clause)
                QueryData paramContainerAccountRetrieval = new QueryData.Builder(paramContainer.UserID).addItemName(accountName).build();

                //Retrieves the account ID
                MySqlCommand accountIDRetrievalCommand = SQLCommandBuilder.getRecordIDCommand(sqlStatementGetAccountID, paramContainerAccountRetrieval);
                accountIDDataTable = DBConnectionManager.getData(accountIDRetrievalCommand);

            if (accountIDDataTable != null && accountIDDataTable.Rows.Count > 0) {
                Object result = accountIDDataTable.Rows[0].ItemArray[0];

                if(result == DBNull.Value) {
                    throw new NoDataFoundException("Unable to retrieve the ID of the saving account whose balance needs to be checked!");
                }

                accountID = Convert.ToInt32(result);

            } else {
                throw new NoDataFoundException("Unable to retrieve the ID of the saving account whose balance needs to be checked!");
            }

            //Prepares the inout and output parameters for calling the store procedure that will retrieve the account balance
            MySqlParameter accountIDParam = new MySqlParameter("p_account_ID", accountID);
            MySqlParameter accountBalanceParam = new MySqlParameter("p_account_balance", MySqlDbType.Double);

            List<MySqlParameter> inputParameters = new List<MySqlParameter>();
            inputParameters.Add(accountIDParam);

            List<MySqlParameter> outputParameters = new List<MySqlParameter>();
            outputParameters.Add(accountBalanceParam);


            List<MySqlParameter> procedureExecutionResults = DBConnectionManager.callDatabaseStoredProcedure("get_saving_account_balance", inputParameters, outputParameters);
            
            if(procedureExecutionResults != null && procedureExecutionResults.Count > 0) {
                accountBalanceParam = procedureExecutionResults.ElementAt(0);

                if(accountBalanceParam == null) {
                    throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
                }

                currentBalance = Convert.ToDouble(accountBalanceParam.Value);

            } else {
                throw new NoDataFoundException("Unable to retrieve the balance of the saving account which needs to be checked!");
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
