﻿using BudgetManager;
using BudgetManager.utils.exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManagerTests.utils {
    internal class TestPartialPaymentUtils {
        public String partialPaymentName;
        public String targetReceivableName;
        private int partialPaymentValue;

        private String sqlStatementGetTestReceivableId = "SELECT receivableID from receivables WHERE name = @paramName";
        private String sqlStatementInsertTestPartialPayment = "INSERT INTO partial_payments(receivable_ID, paymentName, paymentValue, paymentDate) VALUES(@paramId, @paramName, @paramValue, CURDATE())";
        private String sqlStatementUpdateTestPartialPayment = "UPDATE partial_payments SET paymentValue = @paramValue WHERE paymentName = @paramName";
        private String sqlStatementDeleteTestPartialPayment = "DELETE FROM partial_payments WHERE paymentName = @paramName";

        public TestPartialPaymentUtils(string partialPaymentName, string targetReceivableName, int partialPaymentValue) {
            this.partialPaymentName = partialPaymentName;
            this.targetReceivableName = targetReceivableName;
            this.partialPaymentValue = partialPaymentValue;
        }

        public int insertTestPartialPaymentIntoDb() {
            MySqlCommand getReceivableIdCommand = new MySqlCommand(sqlStatementGetTestReceivableId);
            getReceivableIdCommand.Parameters.AddWithValue("@paramName", targetReceivableName);

            DataTable receivableIdDT = DBConnectionManager.getData(getReceivableIdCommand);

            int receivableId;
            if (receivableIdDT != null && receivableIdDT.Rows.Count > 0) {
                receivableId = Convert.ToInt32(receivableIdDT.Rows[0].ItemArray[0].ToString());
            } else {
                return -1;
            }

            MySqlCommand insertPartialPaymentCommand = new MySqlCommand(sqlStatementInsertTestPartialPayment);
            insertPartialPaymentCommand.Parameters.AddWithValue("@paramId", receivableId);
            insertPartialPaymentCommand.Parameters.AddWithValue("@paramName", partialPaymentName);
            insertPartialPaymentCommand.Parameters.AddWithValue("@paramValue", partialPaymentValue);

            int executionResult = DBConnectionManager.insertData(insertPartialPaymentCommand);

            return executionResult;
        }

        public int updateTestPartialPaymentFromDb(int partialPaymentValue, String partialPaymentName) {
            MySqlCommand updatePartialPaymentCommand = new MySqlCommand(sqlStatementUpdateTestPartialPayment);
            updatePartialPaymentCommand.Parameters.AddWithValue("@paramValue", partialPaymentValue);
            updatePartialPaymentCommand.Parameters.AddWithValue("@paramName", partialPaymentName);

            int executionResult = DBConnectionManager.updateData(updatePartialPaymentCommand);

            return executionResult;
        }

        public int deleteTestPartialPaymentFromDb(String partialPaymentName) {
            MySqlCommand deletePartialPaymentCommand = new MySqlCommand(sqlStatementDeleteTestPartialPayment);
            deletePartialPaymentCommand.Parameters.AddWithValue("@paramName", partialPaymentName); 
            
            int executionResult = DBConnectionManager.deleteData(deletePartialPaymentCommand);

            return executionResult;
                
       }
    }
}