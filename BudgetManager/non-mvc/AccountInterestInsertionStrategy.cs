using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.mvc.models.dto;
using MySql.Data.MySqlClient;

namespace BudgetManager.non_mvc {
    class AccountInterestInsertionStrategy : DataInsertionStrategy {
        private string sqlStatementInsertSavingAccountInterest = @"INSERT INTO saving_accounts_interest(account_id, interestName, interestType, paymentType, interestRate, value, creationDate, updatedDate)
                                                                   VALUES(
                                                                   (SELECT accountID FROM saving_accounts WHERE accountName = @paramAccountName and user_ID = @paramUserId),
                                                                   @paramInterestName,
                                                                   (SELECT typeID FROM interest_types WHERE typeName = @paramInterestTypeName),
                                                                   (SELECT typeID FROM interest_payment_type WHERE typeName = @paramInterestPaymentTypeName),
                                                                   @paramInterestRate,
                                                                   @paramInterestValue,
                                                                   @paramCreationDate,
                                                                   @paramUpdatedDate)";

        public int execute(IDataInsertionDTO dataInsertionDTO) {
            int executionResult = -1;

            MySqlCommand savingAccountInterestInsertionCommand = SQLCommandBuilder.getSavingAccountInterestInsertionCommand(sqlStatementInsertSavingAccountInterest, dataInsertionDTO);
            executionResult = DBConnectionManager.insertData(savingAccountInterestInsertionCommand);

            return executionResult;
        }

        public int execute(QueryData paramContainer) {
            throw new NotImplementedException();
        }
    }
}
