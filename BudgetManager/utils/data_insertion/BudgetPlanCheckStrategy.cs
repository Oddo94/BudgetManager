using BudgetManager.mvc.models;
using BudgetManager.mvc.models.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils {
    class BudgetPlanCheckStrategy : DataInsertionCheckStrategy {


        public DataCheckResponse performCheck(QueryData inputData, String selectedItemName, int valueToInsert) {
            DataCheckResponse dataCheckResponse = new DataCheckResponse();
            int checkResult = -1;
            //Query data object will have to contain: userID, itemCreationDate
            BudgetPlanChecker planChecker = new BudgetPlanChecker(inputData.UserID, inputData.ItemCreationDate);

            int budgetPlanExistenceCheckResult = checkBudgetPlanExistence(planChecker);
            if (budgetPlanExistenceCheckResult != 0) {
                //return budgetPlanExistenceCheckResult;
                dataCheckResponse.ExecutionResult = budgetPlanExistenceCheckResult;
                dataCheckResponse.SuccessMessage = "No budget plan was found for the month in which the item is about to be inserted. The data insertion can continue.";
                return dataCheckResponse;
            }

            int budgetPlanBoundariesCheckResult = checkBudgetPlanBoundaries(planChecker);
            if (budgetPlanBoundariesCheckResult != 0) {
                //return budgetPlanBoundariesCheckResult;
                dataCheckResponse.ExecutionResult = budgetPlanBoundariesCheckResult;
                dataCheckResponse.SuccessMessage = "No budget plan boundaries were found.The data insertion can continue.";
                return dataCheckResponse;
            }

            int budgetPlanAlarmCheckResult = checkBudgetPlanAlarm(planChecker, inputData, valueToInsert);
            if (budgetPlanAlarmCheckResult != 0) {
                //return budgetPlanAlarmCheckResult;
                dataCheckResponse.ExecutionResult = budgetPlanAlarmCheckResult;
                dataCheckResponse.ErrorMessage = "No budget plan alarm was found.The data insertion can continue.";
                return dataCheckResponse;
            }

            int budgetPlanItemLimitCheckResult = checkInsertedValueAgainstBudgetPlanLimits(planChecker, inputData, selectedItemName, valueToInsert);
            if (budgetPlanItemLimitCheckResult != 0) {
                //return budgetPlanItemLimitCheckResult;
                dataCheckResponse.ExecutionResult = budgetPlanItemLimitCheckResult;
                dataCheckResponse.ErrorMessage = "The inserted value is higher than the value allowed by the currently applicable budget plan. The data insertion will be aborted.";
                return dataCheckResponse;
            }

            //return checkResult;
            dataCheckResponse.ExecutionResult = checkResult;
            dataCheckResponse.ErrorMessage = "Unable to insert the provided data!";
            return dataCheckResponse;


            //Checks if a budget plan exists for the month selected when inserting the item
            /*            if (planChecker.hasBudgetPlanForSelectedMonth()) {
                            //Gets the plan data for the currently applicable budget plan
                            DataTable budgetPlanDataTable = planChecker.getBudgetPlanData();
                            //Extracts the start date and end date of the budget plan into a String array
                            String[] budgetPlanBoundaries = planChecker.getBudgetPlanBoundaries(budgetPlanDataTable);

                            //Checks if the array containing the budget plan start and end date contains data
                            if (budgetPlanBoundaries != null) {
                                //Calculates the total incomes for the selected time interval
                                int totalIncomes = planChecker.getTotalIncomes(budgetPlanBoundaries[0], budgetPlanBoundaries[1]);
                                //Extracts the percentage limit that was set in the budget plan for the currently selected item
                                int percentageLimitForItem = planChecker.getPercentageLimitForItem(getSelectedType(budgetItemComboBox));
                                //Calculates the actual limit value for the currently selected item based on the previously extracted percentage
                                int limitValueForSelectedItem = planChecker.calculateMaxLimitValue(totalIncomes, percentageLimitForItem);

                                //Checks if an alarm was set for the current budget plan
                                if (planChecker.hasBudgetPlanAlarm(budgetPlanDataTable)) {
                                    //Extracts the threshold percentage set in the budget plan for the triggering of the alarm
                                    int thresholdPercentage = planChecker.getThresholdPercentage(budgetPlanDataTable);
                                    //Calculates the sum of the existing database records for the currently selected item(expense, debt, saving) in the specified time interval
                                    int currentItemTotalValue = planChecker.getTotalValueForSelectedItem(getSelectedType(budgetItemComboBox), budgetPlanBoundaries[0], budgetPlanBoundaries[1]);
                                    //Calculates the actual threshold value at which the alarm will be triggered
                                    int thresholdValue = planChecker.calculateValueFromPercentage(limitValueForSelectedItem, thresholdPercentage);

                                    //Calculates the value which will result after adding the current user input value for the selected item to the sum of the existing database records
                                    int futureItemTotalValue = currentItemTotalValue + entryValue;

                                    //Checks if the previously calculated value is between the threshold value and the max limit for the selected item(as calculated based on the percentage set in the budget plan)
                                    if (planChecker.isBetweenThresholdAndMaxLimit(futureItemTotalValue, thresholdValue, limitValueForSelectedItem)) {
                                        //Calculates the percentage of the futureItemTotalValue
                                        int currentItemPercentageValue = planChecker.calculateCurrentItemPercentageValue(futureItemTotalValue, limitValueForSelectedItem);
                                        //Calculates the difference between the previous percentage value and the threshold percentage(in order to show the percentage by which the threshold is exceeded) 
                                        int percentageDifference = currentItemPercentageValue - thresholdPercentage;
                                        DialogResult userOptionExceedThreshold = MessageBox.Show(String.Format("By inserting the current {0} you will exceed the alarm threshold by {1}%. Are you sure that you want to continue?", selectedItem.ToLower(), percentageDifference), "Insert data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                        if (userOptionExceedThreshold == DialogResult.No) {
                                            return;
                                        } else {
                                            //If the user confirms that he agrees to exceed the threshold the value is inserted in the DB
                                            executionResult = insertSelectedItem(selectedItemIndex);
                                        }

                                    } else {
                                        //If the futureItemTotalValue is above the limit set in the budget plan a warning message is shown and no value is inserted
                                        if (planChecker.exceedsItemLimitValue(entryValue, limitValueForSelectedItem, getSelectedType(budgetItemComboBox), budgetPlanBoundaries[0], budgetPlanBoundaries[1])) {
                                            MessageBox.Show(String.Format("Cannot insert the provided {0} since it would exceed the {1}% limit imposed by the currently applicable budget plan! Please revise the plan or insert a lower value.", selectedItem.ToLower(), percentageLimitForItem), "Insert data form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                            //If the futureItemTotalValue is not between threshold limit and limit value and it doesn't exceed the limit value for the selected item it means that it can be inserted in the DB
                                        } else {
                                            executionResult = insertSelectedItem(selectedItemIndex);
                                        }
                                    }
                                } else {
                                    //If the plan doesn't contain an alarm a check is made to see if the future total value for the item (user input value + sum of existing database records for the selected item) is greater than the max limit for the selected item(as calculated based on the percentage set in the budget plan)
                                    if (planChecker.exceedsItemLimitValue(entryValue, limitValueForSelectedItem, getSelectedType(budgetItemComboBox), budgetPlanBoundaries[0], budgetPlanBoundaries[1])) {
                                        MessageBox.Show(String.Format("Cannot insert the provided {0} since it would exceed the {1}% limit imposed by the currently applicable budget plan! Please revise the plan or insert a lower value.", selectedItem.ToLower(), percentageLimitForItem), "Insert data form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                        //If the value is less than the limit then it is inserted in the DB
                                    } else {
                                        executionResult = insertSelectedItem(selectedItemIndex);
                                    }
                                }
                                //If the String array containing the start and end dates for the budget plan is null then no record is inserted in the database since no check can be performed to see if the limits imposed through it are respected
                            } else {
                                MessageBox.Show("Unable to retrieve the start and end dates of the current budget plan! Please revise the plan before trying to insert new data.", "Insert data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            //If there is no budget plan for the selected month/the existing budget plan doesn't have proper start/end dates it means that no additional checks are made and the value can be inserted(the general check is already passed at this point)
                        } else {
                            executionResult = insertSelectedItem(selectedItemIndex);
                        }

                        //If the user wants to insert a creditor or an income in the database no additional checks are made and the value is inserted               

                        executionResult = insertSelectedItem(selectedItemIndex);




                        return executionResult;
            */

        }

        public DataCheckResponse performCheck(QueryData paramContainer, String selectedItemName, double valueToInsert) {
            throw new NotImplementedException();
        }

        public DataCheckResponse performCheck() {
            throw new NotImplementedException();
        }

        private int checkBudgetPlanExistence(BudgetPlanChecker planChecker) {
            int checkResult = -1;

            if (planChecker.hasBudgetPlanForSelectedMonth()) {
                checkResult = 0;
            }

            return checkResult;
        }

        //NOTE the returned values can be the following:
        // -1-> the check failed and the data can be inserted(e.g no budget plan)
        // 0 -> the check was successfull, and the next check can be performed
        // 1 -> the check failed and an error message was shown; no data will be inserted
        private int checkBudgetPlanBoundaries(BudgetPlanChecker planChecker) {
            int checkResult = -1;
            //Gets the plan data for the currently applicable budget plan
            DataTable budgetPlanDataTable = planChecker.getBudgetPlanData();
            //Extracts the start date and end date of the budget plan into a String array
            String[] budgetPlanBoundaries = planChecker.getBudgetPlanBoundaries(budgetPlanDataTable);

            if (budgetPlanBoundaries != null) {
                checkResult = 0;
            } else {
                MessageBox.Show("Unable to retrieve the start and end dates of the current budget plan! Please revise the plan before trying to insert new data.", "Insert data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                checkResult = 1;
            }

            return checkResult;

        }

        private int checkBudgetPlanAlarm(BudgetPlanChecker planChecker, QueryData inputData, int valueToInsert) {
            int checkResult = -1;
            //Gets the plan data for the currently applicable budget plan
            DataTable budgetPlanDataTable = planChecker.getBudgetPlanData();
            //Extracts the start date and end date of the budget plan into a String array
            String[] budgetPlanBoundaries = planChecker.getBudgetPlanBoundaries(budgetPlanDataTable);

            //Calculates the total incomes for the selected time interval
            int totalIncomes = planChecker.getTotalIncomes(budgetPlanBoundaries[0], budgetPlanBoundaries[1]);
            //Extracts the percentage limit that was set in the budget plan for the currently selected item
            int percentageLimitForItem = planChecker.getPercentageLimitForItem(inputData.BudgetItemType);
            //Calculates the actual limit value for the currently selected item based on the previously extracted percentage
            int limitValueForSelectedItem = planChecker.calculateMaxLimitValue(totalIncomes, percentageLimitForItem);

            //Checks if an alarm was set for the current budget plan
            if (planChecker.hasBudgetPlanAlarm(budgetPlanDataTable)) {

                checkResult = 0;
            }

            return checkResult;

        }

        //private void checkInsertedValueAgainstBudgetPlan(BudgetPlanChecker planChecker) {
        //    int checkResult = -1;

        //    //If the plan doesn't contain an alarm a check is made to see if the future total value for the item (user input value + sum of existing database records for the selected item) is greater than the max limit for the selected item(as calculated based on the percentage set in the budget plan)
        //    if (planChecker.exceedsItemLimitValue(entryValue, limitValueForSelectedItem, getSelectedType(budgetItemComboBox), budgetPlanBoundaries[0], budgetPlanBoundaries[1])) {
        //        MessageBox.Show(String.Format("Cannot insert the provided {0} since it would exceed the {1}% limit imposed by the currently applicable budget plan! Please revise the plan or insert a lower value.", selectedItem.ToLower(), percentageLimitForItem), "Insert data form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        checkResult = 1;
        //        //If the value is less than the limit then it is inserted in the DB
        //    } else {
        //        //executionResult = insertSelectedItem(selectedItemIndex);

        //        checkResult = -1;
        //    }

        //}


        private int checkInsertedValueAgainstBudgetPlanLimits(BudgetPlanChecker planChecker, QueryData inputData, String selectedItemName, int valueToInsert) {
            int checkResult = -1;

            //Gets the plan data for the currently applicable budget plan
            DataTable budgetPlanDataTable = planChecker.getBudgetPlanData();
            //Extracts the start date and end date of the budget plan into a String array
            String[] budgetPlanBoundaries = planChecker.getBudgetPlanBoundaries(budgetPlanDataTable);

            //Extracts the threshold percentage set in the budget plan for the triggering of the alarm
            int thresholdPercentage = planChecker.getThresholdPercentage(budgetPlanDataTable);
            //Calculates the sum of the existing database records for the currently selected item(expense, debt, saving) in the specified time interval
            int currentItemTotalValue = planChecker.getTotalValueForSelectedItem(inputData.BudgetItemType, budgetPlanBoundaries[0], budgetPlanBoundaries[1]);

            //Calculates the total incomes for the selected time interval
            int totalIncomes = planChecker.getTotalIncomes(budgetPlanBoundaries[0], budgetPlanBoundaries[1]);
            //Extracts the percentage limit that was set in the budget plan for the currently selected item
            int percentageLimitForItem = planChecker.getPercentageLimitForItem(inputData.BudgetItemType);

            //Calculates the actual limit value for the currently selected item based on the previously extracted percentage
            int limitValueForSelectedItem = planChecker.calculateMaxLimitValue(totalIncomes, percentageLimitForItem);
            //Calculates the actual threshold value at which the alarm will be triggered
            int thresholdValue = planChecker.calculateValueFromPercentage(limitValueForSelectedItem, thresholdPercentage);

            //Calculates the value which will result after adding the current user input value for the selected item to the sum of the existing database records
            int futureItemTotalValue = currentItemTotalValue + valueToInsert;
            //Checks if the previously calculated value is between the threshold value and the max limit for the selected item(as calculated based on the percentage set in the budget plan)

            if (planChecker.isBetweenThresholdAndMaxLimit(futureItemTotalValue, thresholdValue, limitValueForSelectedItem)) {
                //Calculates the percentage of the futureItemTotalValue
                int currentItemPercentageValue = planChecker.calculateCurrentItemPercentageValue(futureItemTotalValue, limitValueForSelectedItem);
                //Calculates the difference between the previous percentage value and the threshold percentage(in order to show the percentage by which the threshold is exceeded) 
                int percentageDifference = currentItemPercentageValue - thresholdPercentage;
                DialogResult userOptionExceedThreshold = MessageBox.Show(String.Format("By inserting the current {0} you will exceed the alarm threshold by {1}%. Are you sure that you want to continue?", selectedItemName, percentageDifference), "Insert data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (userOptionExceedThreshold == DialogResult.No) {
                    checkResult = 1;//Check result is set to 1 -> the value cannot be inserted
                } else {
                    //If the user confirms that he agrees to exceed the threshold the value is inserted in the DB
                    //executionResult = insertSelectedItem(selectedItemIndex);

                    checkResult = -1;//Check result is set to -1 -> a warning message was shown to the user but he decided to continue so the value can be inserted
                }

            } else {
                //If the futureItemTotalValue is above the limit set in the budget plan a warning message is shown and no value is inserted
                if (planChecker.exceedsItemLimitValue(valueToInsert, limitValueForSelectedItem, inputData.BudgetItemType, budgetPlanBoundaries[0], budgetPlanBoundaries[1])) {
                    MessageBox.Show(String.Format("Cannot insert the provided {0} since it would exceed the {1}% limit imposed by the currently applicable budget plan! Please revise the plan or insert a lower value.", selectedItemName, percentageLimitForItem), "Insert data form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    checkResult = 1;//Check result is set to 1 -> the value cannot be inserted
                                    //If the futureItemTotalValue is not between threshold limit and limit value and it doesn't exceed the limit value for the selected item it means that it can be inserted in the DB
                } else {
                    //executionResult = insertSelectedItem(selectedItemIndex);
                    checkResult = -1;//Check result is set to -1 so the element can be inserted because it does not exceed the limits imposed by the applicable budget plan
                }
            }

            return checkResult;
        }
    }
}








