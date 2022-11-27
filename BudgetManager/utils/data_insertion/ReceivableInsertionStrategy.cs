using BudgetManager.mvc.models.dto;
using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.non_mvc {
    class ReceivableInsertionStrategy : DataInsertionStrategy {
        private String sqlStatementInsertReceivable = @"INSERT INTO receivables(name, value, debtor_ID, account_ID,  totalPaidAmount, createdDate, dueDate) VALUES(
                                                        @paramName, 
                                                        @paramValue, 
                                                        (SELECT debtorID FROM debtors WHERE debtorName = @paramDebtorName),
                                                        (SELECT accountID FROM saving_accounts WHERE user_ID = @paramUserID AND accountName = @paramAccountName),
                                                        @paramTotalPaidAmount, 
                                                        @paramCreatedDate,
                                                        @paramDueDate)";
        private String sqlStatementGetDebtorID = @"SELECT debtorID FROM debtors WHERE debtorName = @paramTypeName";//Modify the getID method of DataInsertionUtils class to use a more generic record ID retrieval method from the SQLCommandBuilder class


        //DEPRECATED- DO NOT USE!!
        public int execute(QueryData paramContainer) {
            //    Guard.notNull(paramContainer, "parameter container");
            //    int executionResult = -1;

            //    //Retrieving the ID of the selected debtor
            //    int debtorID = DataInsertionUtils.getID(sqlStatementGetDebtorID, paramContainer.DebtorName);
            //    //Retrieving the ID of the account selected as income source for the receivable


            //    //Creating a new param container object that contains all the values from the param container received as argument plus the debtor ID(which will be necessary for inserting the new receivable record) 
            //    QueryData updatedParamContainer = new QueryData.Builder(paramContainer.UserID)
            //        .addItemName(paramContainer.ItemName)
            //        .addItemValue(paramContainer.ItemValue)
            //        .addDebtorName(paramContainer.DebtorName)
            //        .addDebtorID(debtorID)
            //        .addStartDate(paramContainer.StartDate)
            //        .addEndDate(paramContainer.EndDate)
            //        .addIncomeSource(paramContainer.IncomeSource)
            //        .addPaidAmount(paramContainer.PaidAmount)
            //        .build();

            //    MySqlCommand insertReceivableCommand = SQLCommandBuilder.getReceivableInsertionCommand(sqlStatementInsertReceivable, updatedParamContainer);


            //    executionResult = DBConnectionManager.insertData(insertReceivableCommand);


            //    return executionResult;

            throw new NotImplementedException();
        }

        //The new method used for receivables insertion (uses the new ReceivableDTO object for transfering the query data)
        public int execute(IDataInsertionDTO dataInsertionDTO) {
            Guard.notNull(dataInsertionDTO, "receivable DTO");
            int executionResult = -1;

            MySqlCommand insertReceivableCommand = SQLCommandBuilder.getReceivableInsertionCommand(sqlStatementInsertReceivable, dataInsertionDTO);

            executionResult = DBConnectionManager.insertData(insertReceivableCommand);

            return executionResult;
        }
    }
}
