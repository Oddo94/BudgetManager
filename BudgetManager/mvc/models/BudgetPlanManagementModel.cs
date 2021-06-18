﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.models {
    class BudgetPlanManagementModel : IUpdaterModel {
        private ArrayList observerList = new ArrayList();
        private DataTable[] dataSources = new DataTable[10];

        private String sqlStatementSelectBudgetPlanForASingleMonth = @"SELECT planID AS 'ID', planName AS 'Plan name', expenseLimit AS 'Expense limit', debtLimit  AS 'Debt limit', savingLimit  AS 'Saving limit', (SELECT typeName FROM plan_types WHERE typeID = planType) AS 'Plan type', hasAlarm 'Set alarm', thresholdPercentage AS 'Alarm threshold', startDate AS 'Start date', endDate AS 'End date' FROM budget_plans WHERE user_ID = @paramID AND (MONTH(startDate) = @paramMonth AND YEAR(startDate) = @paramYear)";
        private String sqlStatementSelectBudgetPlansForTheWholeYear = @"SELECT planID AS 'ID', planName AS 'Plan name', expenseLimit AS 'Expense limit', debtLimit  AS 'Debt limit', savingLimit  AS 'Saving limit', (SELECT typeName FROM plan_types WHERE typeID = planType) AS 'Plan type', hasAlarm 'Set alarm', thresholdPercentage AS 'Alarm threshold', startDate AS 'Start date', endDate AS 'End date' FROM budget_plans WHERE user_ID = @paramID AND YEAR(startDate) = @paramYear";
        //private String sqlStatementDeleteBudgetPlan = @"DELETE FROM budget_plans WHERE planID = @paramItemID";

        private String sqlStatementGetItemValuesForMultipleMonthsPlan = @"SELECT(SELECT SUM(value) FROM expenses WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate) AS 'Total expenses',
                (SELECT expenseLimit from budget_plans WHERE user_ID = @paramID AND startDate = @paramStartDate AND endDate = @paramEndDate) AS 'Expense percentage limit', 
                (SELECT SUM(value) FROM debts WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate) AS 'Total debts',
                (SELECT debtLimit from budget_plans WHERE user_ID = @paramID AND startDate = @paramStartDate AND endDate = @paramEndDate) AS 'Debt percentage limit', 
                (SELECT SUM(value) FROM savings WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate) AS 'Total savings',
                (SELECT savingLimit from budget_plans WHERE user_ID = @paramID AND startDate = @paramStartDate AND endDate = @paramEndDate) AS 'Saving percentage limit', 
                (SELECT SUM(value) FROM incomes WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate) AS 'Total incomes'";

      
        public BudgetPlanManagementModel() {

        }

        public DataTable[] DataSources {
            get {
                return this.dataSources;
            }

            set {
                this.dataSources = value;
                notifyObservers();
            }
        }

        public void addObserver(IView observer) {
            observerList.Add(observer);
        }

        public void removeObserver(IView observer) {
           observerList.Remove(observer);
        }

        public void notifyObservers() {
            foreach (IView currentObserver in observerList) {
                currentObserver.updateView(this);
            }
        }

        
        //public int deleteData(string tableName, int itemID) {
        //    MySqlCommand deleteBudgetPlanCommand = new MySqlCommand(sqlStatementDeleteBudgetPlan);
        //    deleteBudgetPlanCommand.Parameters.AddWithValue("paramItemID", itemID);

        //    int executionResult = DBConnectionManager.deleteData(deleteBudgetPlanCommand);

        //    return executionResult;
        //}

        public int deleteData(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            //Recreating the command object used to display the data in the DataGridView
            MySqlCommand deleteBudgetPlanCommand = getCorrectSqlCommandForDataDisplay(option, paramContainer);

            int executionResult = DBConnectionManager.deleteData(deleteBudgetPlanCommand, sourceDataTable);

            return executionResult;
        }

        public DataTable getNewData(QueryType option, QueryData paramContainer, SelectedDataSource dataSource) {
            MySqlCommand command = null;
            if (option == QueryType.SINGLE_MONTH) {
                switch (dataSource) {
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        //command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    default:
                        break;

                }

            } else if (option == QueryType.FULL_YEAR) {
                switch (dataSource) {

                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:
                        //command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    default:
                        break;

                }
              //Budget plan option branch
            } else if (option == QueryType.BUDGET_PLAN_INFO) {
                switch (dataSource) {

                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:
                        command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    case SelectedDataSource.STATIC_DATASOURCE:                       
                        break;

                    default:
                        break;
                }
            }

            if (command == null) {
                return null;
            }

            return DBConnectionManager.getData(command);
        }

        public bool hasDBConnection() {
            return DBConnectionManager.hasConnection();
        }


        public int updateData(QueryType option, QueryData paramContainer, DataTable sourceDataTable) {
            //Recreating the command used for displaying the data in the GUI table
            MySqlCommand updateBudgetPlanTableCommand = getCorrectSqlCommandForDataDisplay(option, paramContainer);

            //Calling the method responsible for the actual database table update with the changes made by the user
            int executionResult = DBConnectionManager.updateData(updateBudgetPlanTableCommand, sourceDataTable);

            return executionResult;
        }

        private MySqlCommand getCorrectSqlCommandForDataDisplay(QueryType option, QueryData paramContainer) {
            if (option == QueryType.SINGLE_MONTH) {
                MySqlCommand singleMonthBudgetPlanCommand = SQLCommandBuilder.getSingleMonthCommand(sqlStatementSelectBudgetPlanForASingleMonth, paramContainer);

                return singleMonthBudgetPlanCommand;

            } else if (option == QueryType.FULL_YEAR) {
                MySqlCommand fullYearBudgetPlansCommand = SQLCommandBuilder.getFullYearRecordsCommand(sqlStatementSelectBudgetPlansForTheWholeYear, paramContainer);

                return fullYearBudgetPlansCommand;

            } else if (option == QueryType.BUDGET_PLAN_INFO) {
                                        
                //Gets the budget plan info retrieval command by using the getMultipleMonthsCommand method for both single month and multiple months budget plans because in both cases the startDate and endDate are used to identify the plan so there was no point in creating a different SQL statement for each case. 
                //It was also more efficient to reuse the getMultipleMonthsCommand() method which already could handle the userID, startDate and endDate parameters than to create a different method
                MySqlCommand budgetPlanInfoCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementGetItemValuesForMultipleMonthsPlan, paramContainer);

                return budgetPlanInfoCommand;
            }
      
                return null;           
        }

      
    }
}