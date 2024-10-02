using BudgetManager.mvc.models.dto;
using BudgetManager.non_mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils.data_insertion {
    public class ExternalAccountBankingFeeInsertionStrategy : DataInsertionStrategy {
        private String sqlStatementInsertExternalAccountBankingFee = @"INSERT
	                                                                       INTO
	                                                                       external_accounts_banking_fees (account_ID,
	                                                                       name,
	                                                                       value,
                                                                           description,
	                                                                       createdDate)
                                                                       VALUES (
                                                                       (
                                                                        SELECT
	                                                                         accountID
                                                                        FROM
	                                                                         saving_accounts
                                                                        WHERE
	                                                                         accountName = @paramAccountName
	                                                                         AND user_ID = @paramUserId),
                                                                        @paramBankingFeeName,
                                                                        @paramBankingFeeValue,
                                                                        @paramBankingFeeDescription,
                                                                        @paramBankingFeeCreatedDate
                                                                        )";
        public int execute(QueryData paramContainer) {
          throw new NotImplementedException();
        }

        public int execute(IDataInsertionDTO dataInsertionDTO) { 
            BankingFeeDTO bankingFeeDTO = (BankingFeeDTO) dataInsertionDTO;

            MySqlCommand bankingFeeInsertionCommand = new MySqlCommand(sqlStatementInsertExternalAccountBankingFee);
            bankingFeeInsertionCommand.Parameters.AddWithValue("@paramAccountName", bankingFeeDTO.AccountName);
            bankingFeeInsertionCommand.Parameters.AddWithValue("@paramUserId", bankingFeeDTO.UserID);
            bankingFeeInsertionCommand.Parameters.AddWithValue("@paramBankingFeeName", bankingFeeDTO.Name);
            bankingFeeInsertionCommand.Parameters.AddWithValue("@paramBankingFeeValue", bankingFeeDTO.Value);
            bankingFeeInsertionCommand.Parameters.AddWithValue("@paramBankingFeeDescription", bankingFeeDTO.Description);
            bankingFeeInsertionCommand.Parameters.AddWithValue("@paramBankingFeeCreatedDate", bankingFeeDTO.CreatedDate);

            int executionResult = DBConnectionManager.insertData(bankingFeeInsertionCommand);

            return executionResult;
        }
    }
}
