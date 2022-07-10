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
        private String sqlStatementInsertReceivable = @"INSERT INTO receivables(name, value, debtorID, user_ID,  totalPaidAmount, dateCreated, dateDue) VALUES(@paramItemName, @paramItemValue, @paramDebtorID, @paramUserID, @paramPaidAmount, @paramStartDate, @paramEndDate)";
        private String sqlStatementGetDebtorID = @"SELECT debtorID FROM debtors WHERE debtorName = @paramTypeName";//Modify the getID method of DataInsertionUtils class to use a more generic record ID retrieval method from the SQLCommandBuilder class

        public int execute(QueryData paramContainer) {
            Guard.notNull(paramContainer, "parameter container");
            int executionResult = -1;

            //Retrieving the ID of the selected debtor
            int debtorID = DataInsertionUtils.getID(sqlStatementGetDebtorID, paramContainer.DebtorName);

            //Creating a new param container object that contains all the values from the param container received as argument plus the debtor ID(which will be necessary for inserting the new receivable record) 
            QueryData updatedParamContainer = new QueryData.Builder(paramContainer.UserID)
                .addItemName(paramContainer.ItemName)
                .addItemValue(paramContainer.ItemValue)
                .addDebtorName(paramContainer.DebtorName)
                .addDebtorID(debtorID)
                .addStartDate(paramContainer.StartDate)
                .addEndDate(paramContainer.EndDate)
                .addIncomeSource(paramContainer.IncomeSource)
                .addPaidAmount(paramContainer.PaidAmount)
                .build();

            MySqlCommand insertReceivableCommand = SQLCommandBuilder.getReceivableInsertionCommand(sqlStatementInsertReceivable, updatedParamContainer);


            executionResult = DBConnectionManager.insertData(insertReceivableCommand);


            return executionResult;

        }

        public int execute(IDataInsertionDTO dataInsertionDTO) {
            throw new NotImplementedException();
        }
    }
}
