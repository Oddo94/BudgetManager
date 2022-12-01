using BudgetManager.mvc.models.dto;
using BudgetManager.utils;
using MySql.Data.MySqlClient;
using System;

namespace BudgetManager.non_mvc {
    class ReceivableInsertionStrategy : DataInsertionStrategy {
        private String sqlStatementInsertReceivable = @"INSERT INTO receivables(name, value, debtor_ID, account_ID,  totalPaidAmount, status_ID, createdDate, dueDate) VALUES(
                                                        @paramName, 
                                                        @paramValue, 
                                                        (SELECT debtorID FROM debtors WHERE debtorName = @paramDebtorName),
                                                        (SELECT accountID FROM saving_accounts WHERE user_ID = @paramUserID AND accountName = @paramAccountName),
                                                        @paramTotalPaidAmount,
                                                        (SELECT statusID FROM receivable_status WHERE statusDescription = @paramStatusDescription),
                                                        @paramCreatedDate,
                                                        @paramDueDate)";
        

        public int execute(QueryData paramContainer) {
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
