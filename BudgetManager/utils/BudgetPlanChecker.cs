﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils {
    class BudgetPlanChecker {
        private int userID;
        //private String planStartDate;
        private String currentDate;

        //SQL statements for checking budget plan existence for the same time interval
        private String sqlStatementCheckBudgetPlanExistence = @"SELECT planName, expenseLimit, debtLimit, savingLimit, hasAlarm, thresholdPercentage, startDate, endDate FROM budget_plans WHERE user_ID = @paramID AND @paramDate BETWEEN startDate AND endDate";
        //private String sqlStatementGetTotalIncomes = @"SELECT SUM(value) AS Total_incomes FROM incomes WHERE user_ID = @paramID AND (MONTH(date) = @paramMonth AND YEAR(date) = @paramYear)";
        private String sqlStatementGetTotalIncomes = @"SELECT SUM(value) AS Total_incomes FROM incomes WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate";
        //SQL statements for retrieving total item value in order to make further checks to see if the specified threshold has been exceeded
        private String sqlStatementGetTotalExpenses = @"SELECT SUM(value) AS Total_expenses FROM expenses WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate";
        private String sqlStatementGetTotalDebts = @"SELECT SUM(value) AS Total_debts FROM debts WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate";
        private String sqlStatementGetTotalSavings = @"SELECT SUM(value) AS Total_savings FROM savings WHERE user_ID = @paramID AND date BETWEEN @paramStartDate AND @paramEndDate";


        public BudgetPlanChecker(int userID, String currentDate) {
            this.userID = userID;
            //this.planStartDate = planStartDate;
            this.currentDate = currentDate;
        }

        public DataTable getBudgetPlanData() {
            QueryData paramContainer = new QueryData.Builder(userID).addStartDate(currentDate).build();
            MySqlCommand budgetPlanExistenceCheckCommand = SQLCommandBuilder.getBudgetPlanCheckCommand(sqlStatementCheckBudgetPlanExistence, paramContainer);

            DataTable budgetPlanDataTable = DBConnectionManager.getData(budgetPlanExistenceCheckCommand);

            if (budgetPlanDataTable != null && budgetPlanDataTable.Rows.Count == 1) {
                return budgetPlanDataTable;
            }

            return null;
        }

        public bool hasBudgetPlanForSelectedMonth() {
            if (getBudgetPlanData() != null) {
                return true;
            }

            return false;
        }

       public int calculateMaxLimitValue(int totalIncomes, int itemPercentage) {
            if (totalIncomes < 0) {
                return -1;
            }

            return (totalIncomes * itemPercentage) / 100;

        }

        //Possible generic method
        public int calculateValueFromPercentage(int totalValue, int percentage) {
            if (totalValue < 0) {
                return -1;
            }

            return (totalValue * percentage) / 100;
        }

        private bool isAboveThresholdValue(int totalItemValue, int userInsertedValue, int thresholdValue) {

            return totalItemValue + userInsertedValue > thresholdValue;
        }

        //Method for retrieving the start date and end of the specified budget plan (the data is returned as String array)
        public String[] getBudgetPlanBoundaries(DataTable budgetPlanDataTable) {
            if (budgetPlanDataTable == null) {
                return null;
            }

            String[] budgetPlanBoundaries = new String[2];

            String budgetPlanStartDate = budgetPlanDataTable.Rows[0].ItemArray[6] != DBNull.Value ? Convert.ToString(budgetPlanDataTable.Rows[0].ItemArray[6]) : null;
            String budgetPlanEndDate = budgetPlanDataTable.Rows[0].ItemArray[7] != DBNull.Value ? Convert.ToString(budgetPlanDataTable.Rows[0].ItemArray[7]) : null;

            String sqlFormatStartDate = DateTime.Parse(budgetPlanStartDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            String sqlFormatEndDate = DateTime.Parse(budgetPlanEndDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            budgetPlanBoundaries[0] = sqlFormatStartDate;
            budgetPlanBoundaries[1] = sqlFormatEndDate;

            return budgetPlanBoundaries;
        }

        //Method for retrieving the total income value for the specified interval
        public int getTotalIncomes(String startDate, String endDate) { 
            String sqlFormatStartDate = DateTime.Parse(startDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            String sqlFormatEndDate = DateTime.Parse(endDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            

            QueryData paramContainer = new QueryData.Builder(userID).addStartDate(sqlFormatStartDate).addEndDate(sqlFormatEndDate).build();
            MySqlCommand getTotalIncomesCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementGetTotalIncomes, paramContainer);

            DataTable totalIncomesDataTable = DBConnectionManager.getData(getTotalIncomesCommand);

            if (totalIncomesDataTable != null && totalIncomesDataTable.Rows.Count == 1) {
                int totalIncomes = totalIncomesDataTable.Rows[0].ItemArray[0] != DBNull.Value ? Convert.ToInt32(totalIncomesDataTable.Rows[0].ItemArray[0]) : 0;

                return totalIncomes;
            }

            return -1;
        }

        //Method for retrieving the total value for the item that the user wants to insert into the DB(for the period between the start and end date of the budget plan)
        public int getTotalValueForSelectedItem(BudgetItemType itemType, String startDate, String endDate) {           
            MySqlCommand getSelectedItemTotalValueCommand = getCorrectSqlCommand(itemType, startDate, endDate);

            if (getSelectedItemTotalValueCommand == null) {
                return -1;
            }

            DataTable itemTotalValueDataTable = DBConnectionManager.getData(getSelectedItemTotalValueCommand);

            if (itemTotalValueDataTable != null && itemTotalValueDataTable.Rows.Count == 1) {
                int totalItemValue = itemTotalValueDataTable.Rows[0].ItemArray[0] != DBNull.Value ? Convert.ToInt32(itemTotalValueDataTable.Rows[0].ItemArray[0]) : 0;

                return totalItemValue;
            }

            return -1;
        }

        public int getPercentageLimitForItem(BudgetItemType itemType) {
            //Gets the budget plan data
            DataTable budgetPlanDataTable = getBudgetPlanData();

            //Sets the default value for the percentage limit
            int percentageLimit = 1;

            //Checks if the specified item type is an expense, debt or saving and sets the percentage limit accordingly (if the percentage limit retrieved from the DB is null the default value of 1 is set)
            switch (itemType) {
                case BudgetItemType.EXPENSE:
                    percentageLimit = budgetPlanDataTable.Rows[0].ItemArray[1] != DBNull.Value ? Convert.ToInt32(budgetPlanDataTable.Rows[0].ItemArray[1]) : 1;
                    break;

                case BudgetItemType.DEBT:
                    percentageLimit = budgetPlanDataTable.Rows[0].ItemArray[2] != DBNull.Value ? Convert.ToInt32(budgetPlanDataTable.Rows[0].ItemArray[2]) : 1;
                    break;

                case BudgetItemType.SAVING:
                    percentageLimit = budgetPlanDataTable.Rows[0].ItemArray[3] != DBNull.Value ? Convert.ToInt32(budgetPlanDataTable.Rows[0].ItemArray[3]) : 1;
                    break;

                default:
                    break;
            }

            return percentageLimit;
        }

        //Method for retrieving the threshold percentage from the budget plan
        public int getThresholdPercentage(DataTable budgetPlanDataTable) {
            if (budgetPlanDataTable != null && budgetPlanDataTable.Rows.Count == 1) {
                int thresholdPercentage = budgetPlanDataTable.Rows[0].ItemArray[5] != DBNull.Value ? Convert.ToInt32(budgetPlanDataTable.Rows[0].ItemArray[5]) : 0;

                return thresholdPercentage;
            }

            return -1;
        }

        //Method for checking if adding the user input value to the total value of the selected item would result in exceeding the imposed limit
        public bool exceedsItemLimitValue(int userInputValue, int limitValue, BudgetItemType itemType, String startDate, String endDate) {
            //Converts the date strings into the format required by the MySqL database
            String sqlFormatStartDate = DateTime.Parse(startDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            String sqlFormatEndDate = DateTime.Parse(endDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            //Creates the command that will retrieve the the total value of the item (expense debt, saving) up to the current date 
            MySqlCommand totalItemValueCommand = getCorrectSqlCommand(itemType, sqlFormatStartDate, sqlFormatEndDate);
            DataTable totalItemValueDataTable = DBConnectionManager.getData(totalItemValueCommand);

            //The default value for the total item value is set to 0 so that it does not affect the calculations in case there are no expense record for the current time interval
            int totalItemValue = 0;
            if (totalItemValueDataTable != null && totalItemValueDataTable.Rows.Count == 1) {
                totalItemValue = totalItemValueDataTable.Rows[0].ItemArray[0] != DBNull.Value ? Convert.ToInt32(totalItemValueDataTable.Rows[0].ItemArray[0]) : 0;
            }
         
            return totalItemValue + userInputValue > limitValue;

        }

        //Method for getting the right Sql command according to the user selected budget item(expense, debt saving)
        private MySqlCommand getCorrectSqlCommand(BudgetItemType itemType, String startDate, String endDate) {
            MySqlCommand getTotalItemValueCommand = null;
            QueryData paramContainer = new QueryData.Builder(userID).addStartDate(startDate).addEndDate(endDate).build();

            switch (itemType) {
                case BudgetItemType.EXPENSE:
                    getTotalItemValueCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementGetTotalExpenses, paramContainer);
                    break;

                case BudgetItemType.DEBT:
                    getTotalItemValueCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementGetTotalDebts, paramContainer);
                    break;

                case BudgetItemType.SAVING:
                    getTotalItemValueCommand = SQLCommandBuilder.getMultipleMonthsCommand(sqlStatementGetTotalSavings, paramContainer);
                    break;

                default:
                    break;
            }

            return getTotalItemValueCommand;

        }

        public bool hasBudgetPlanAlarm(DataTable budgetPlanDataTable) {
            //Checks if the provided DataTable contains data
            if (budgetPlanDataTable != null && budgetPlanDataTable.Rows.Count == 1) {
                //If the value present in the hasAlarm column of the table is not null then the variable is assigned that value else it is assigned 0
                int alarmValue = budgetPlanDataTable.Rows[0].ItemArray[4] != DBNull.Value ? Convert.ToInt32(budgetPlanDataTable.Rows[0].ItemArray[4]) : 0;

                if (alarmValue == 1) {
                    return true;
                }               
            }

            return false;
        }

        //Method for checking if the current item total value is above threshold but is lower than max limit set through the budget plan
        public bool isBetweenThresholdAndMaxLimit(int currentItemTotalValue, int thresholdValue, int maxLimitValue) {
            if (currentItemTotalValue > thresholdValue && currentItemTotalValue <= maxLimitValue) {
                return true;
            }

            return false;
        }

        //Method for calculating the percentage of the current item total value from the imposed limit value
        public int calculateCurrentItemPercentageValue(int currentItemTotalValue, int limitValue) {

            if (currentItemTotalValue > limitValue) {
                return -1;
            }
       
            return (currentItemTotalValue * 100) / limitValue;
        }

    }
       }
    

 
    

