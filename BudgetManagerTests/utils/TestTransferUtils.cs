using BudgetManager;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BudgetManagerTests.utils {
    internal class TestTransferUtils {
        private int sourceAccountId;
        private int destinationAccountId;
        private String transferName;
        private double sentValue;
        private double receivedValue;
        private double exchangeRate;
        private String transactionId;
        private String transferObservations;
        private String transferDate;
        private int userId;

        public TestTransferUtils(int sourceAccountId, int destinationAccountId, String transferName, double sentValue, double receivedValue, double exchangeRate, String transactionId, String transferObservations, String transferDate, int userId) {
            this.sourceAccountId = sourceAccountId;
            this.destinationAccountId = destinationAccountId;
            this.transferName = transferName;
            this.sentValue = sentValue;
            this.receivedValue = receivedValue;
            this.exchangeRate = exchangeRate;
            this.transactionId = transactionId;
            this.transferObservations = transferObservations;
            this.transferDate = transferDate;
            this.userId = userId;

        }

        private String sqlStatementInsertTestTransfer = @"INSERT INTO saving_accounts_transfers(senderAccountID, receivingAccountID, transferName, sentValue, receivedValue, exchangeRate, transactionID, observations, transferDate) 
                                                                VALUES(@paramSenderAccountId, @paramReceivingAccountId, @paramTransferName, @paramSentValue, @paramReceivedValue, @paramExchangeRate, @paramTransactionID, @paramObservations, @paramTransferDate)";
        private String sqlStatementUpdateTestTransfer = @"UPDATE saving_accounts_transfers SET sentValue = @paramSentValue, receivedValue = @paramReceivedValue, exchangeRate = @paramExchangeRate WHERE transferName = @paramTransferName";
        private String sqlStatementDeleteTestTransfer = @"DELETE FROM saving_accounts_transfers WHERE transferName = @paramTransferName";

    
        public int insertTestTransferIntoDb() {
            QueryData paramContainer = new QueryData.Builder(userId)
                .addSourceAccountID(sourceAccountId)
                .addDestinationAccountID(destinationAccountId)
                .addItemName(transferName)
                .addSentValue(sentValue)
                .addReceivedValue(receivedValue)
                .addExchangeRate(exchangeRate)
                .addAdditionalData(transferObservations)
                .addItemCreationDate(transferDate)
                .addGenericID(transactionId)
                .build();

            MySqlCommand insertTestTransferCommand = SQLCommandBuilder.getTransferInsertionCommand(sqlStatementInsertTestTransfer, paramContainer);
            int executionResult = DBConnectionManager.insertData(insertTestTransferCommand);

            return executionResult;
        }

        public int updateTestTransferFromDb(String transferName, double newSentValue, double newReceivedValue, double newExchangeRate) {
            MySqlCommand updateTestTransferCommand = new MySqlCommand(sqlStatementUpdateTestTransfer);
            updateTestTransferCommand.Parameters.AddWithValue("@paramSentValue", newSentValue);
            updateTestTransferCommand.Parameters.AddWithValue("@paramReceivedValue", newReceivedValue);
            updateTestTransferCommand.Parameters.AddWithValue("@paramExchangeRate", newExchangeRate);
            updateTestTransferCommand.Parameters.AddWithValue("@paramTransferName", transferName);

            int executionResult = DBConnectionManager.updateData(updateTestTransferCommand);

            return executionResult;
        }

        public int deleteTestTransferFromDb(String transferName) {
            MySqlCommand deleteTestTransferCommand = new MySqlCommand(sqlStatementDeleteTestTransfer);
            deleteTestTransferCommand.Parameters.AddWithValue("@paramTransferName", transferName);

            int executionResult = DBConnectionManager.deleteData(deleteTestTransferCommand);

            return executionResult;
        }
    
    }
}
