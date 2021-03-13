using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils {
    class BudgetPlanChecker {
        private int userID;
        private String planStartDate;

        //SQL statements for checking budget plan existence for the same time interval
        private String sqlStatementCheckBudgetPlanExistence = @"SELECT planName, expenseLimit, debtLimit, savingLimit, thresholdPercentage startDate, endDate FROM budget_plans WHERE user_ID = @paramID AND @paramDate BETWEEN startDate AND endDate";
        private String sqlStatementGetTotalIncomes = @"SELECT SUM(value) AS Total_incomes FROM incomes WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";

        public BudgetPlanChecker(int userID, String planStartDate) {
            this.userID = userID;
            this.planStartDate = planStartDate;
        }

        private DataTable getBudgetPlanData() {
            QueryData paramContainer = new QueryData.Builder(userID).addStartDate(planStartDate).build();
            MySqlCommand budgetPlanExistenceCheckCommand = SQLCommandBuilder.getBudgetPlanCheckCommand(sqlStatementCheckBudgetPlanExistence, paramContainer);

            DataTable budgetPlanDataTable = DBConnectionManager.getData(budgetPlanExistenceCheckCommand);

            if ( budgetPlanDataTable != null && budgetPlanDataTable.Rows.Count == 1) {
                return budgetPlanDataTable;
            }

            return null;
        }

        private bool hasBudgetPlanForSelectedMonth() {
            if (getBudgetPlanData() != null) {
                return true;
            }

            return false;
        }

        private int calculateMaxLimitValue(int totalIncomes, int itemPercentage) {
            if (totalIncomes < 0) {
                return -1;
            }

            return (totalIncomes * itemPercentage) / 100;

        }

        private bool isAboveThresholdValue(int totalItemValue, int userInsertedValue, int thresholdValue) {

            return totalItemValue + userInsertedValue > thresholdValue;
        }

    }
}
