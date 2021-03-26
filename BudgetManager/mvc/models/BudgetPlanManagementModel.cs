using MySql.Data.MySqlClient;
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

        private String sqlStatementSelectBudgetPlanForASingleMonth = @"SELECT planID AS 'ID', planName AS 'Plan name', expenseLimit AS 'Expense limit', debtLimit  AS 'Debt limit', savingLimit  AS 'Saving limit', (SELECT typeName FROM plan_types WHERE typeID = planType) AS 'Plan type', hasAlarm 'Set alarm', thresholdPercentage AS 'Alarm threshold', startDate AS 'Start date', endDate AS 'End date' FROM budget_plans WHERE user_ID = @paramID AND (MONTH(startDate) = @paramStartMonth AND YEAR(startDate) = @paramYear)";
        private String sqlStatementSelectBudgetPlansForTheWholeYear = @"SELECT planID AS 'ID', planName AS 'Plan name', expenseLimit AS 'Expense limit', debtLimit  AS 'Debt limit', savingLimit  AS 'Saving limit', (SELECT typeName FROM plan_types WHERE typeID = planType) AS 'Plan type', hasAlarm 'Set alarm', thresholdPercentage AS 'Alarm threshold', startDate AS 'Start date', endDate AS 'End date' FROM budget_plans WHERE user_ID = @paramID AND YEAR(startDate) = @paramYear";
        private String sqlStatementDeleteBudgetPlan = @"DELETE FROM budget_plans WHERE planID = @paramItemID";

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

        
        public int deleteData(string tableName, int itemID) {
            MySqlCommand deleteBudgetPlanCommand = new MySqlCommand(sqlStatementDeleteBudgetPlan);
            deleteBudgetPlanCommand.Parameters.AddWithValue("paramItemID", itemID);

            int executionResult = DBConnectionManager.deleteData(deleteBudgetPlanCommand);

            return executionResult;
        }

        public DataTable getNewData(QueryType option, QueryData paramContainer, SelectedDataSource dataSource) {
            MySqlCommand command = null;
            if (option == QueryType.SINGLE_MONTH) {
                switch (dataSource) {                  
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:                      
                        break;

                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:                       
                        break;
                    
                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
                        break;

                    default:
                        break;

                }

            } else if (option == QueryType.FULL_YEAR) {
                switch (dataSource) {
                    
                    case SelectedDataSource.DYNAMIC_DATASOURCE_1:                        
                        break;
                    
                    case SelectedDataSource.DYNAMIC_DATASOURCE_2:                       
                        break;
                    
                    case SelectedDataSource.STATIC_DATASOURCE:
                        command = getCorrectSqlCommandForDataDisplay(option, paramContainer);
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

            } else {
                return null;
            }
        }     
    }
}
