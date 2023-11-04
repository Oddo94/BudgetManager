using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.mvc.models.dto;
using MySql.Data.MySqlClient;
using System.Data;
using BudgetManager.mvc.models;

namespace BudgetManager.utils.data_insertion {
    class PartialPaymentInsertionCheckStrategy : DataInsertionCheckStrategy {
        private String sqlStatementCheckAmountLeftToInsert = @"SELECT (rcs.value - COALESCE(SUM(pps.paymentValue),0)) 
                                                               FROM partial_payments pps
                                                               INNER JOIN receivables rcs on pps.receivable_ID = rcs.receivableID
                                                               WHERE rcs.receivableID = @paramReceivableID";
        private IDataInsertionDTO dataInsertionDTO;

        public PartialPaymentInsertionCheckStrategy(IDataInsertionDTO dataInsertionDTO) {
            this.dataInsertionDTO = dataInsertionDTO;
        }

        public DataCheckResponse performCheck() {
            DataCheckResponse dataCheckResponse = new DataCheckResponse();
            //int executionResult = -1;

            PartialPaymentDTO partialPaymentDTO = (PartialPaymentDTO) dataInsertionDTO;
            int valueToInsert = partialPaymentDTO.PaymentValue;
            int receivableID = partialPaymentDTO.ReceivableID;

            if(canInsertPartialPayment(valueToInsert, sqlStatementCheckAmountLeftToInsert, receivableID)) {
                //executionResult = 0;
                dataCheckResponse.ExecutionResult = 0;
                dataCheckResponse.SuccessMessage = "The partial payment can be inserted.";
            } else {
                dataCheckResponse.ExecutionResult = -1;
                dataCheckResponse.ErrorMessage = "The partial payment value is higher than the amount left to be paid for the currently selected receivable!";
            }

            //return executionResult;
            return dataCheckResponse;
        }

        public DataCheckResponse performCheck(QueryData paramContainer, String selectedItemName, double valueToInsert) {
            throw new NotImplementedException();
        }

        public DataCheckResponse performCheck(QueryData inputData, string selectedItemName, int valueToInsert) {
            throw new NotImplementedException();
        }

        //Method for checking if the value of the partial payment to be inserted is lower than the sum of existing partial payments for a specified receivable
        private bool canInsertPartialPayment(int valueToInsert, String sqlStatement, int receivableID) {
            //The partial payment value cannot be negative
            if (valueToInsert < 0) {
                return false;
            }

            MySqlCommand amountLeftRetrievalCommand = new MySqlCommand(sqlStatement);
            amountLeftRetrievalCommand.Parameters.AddWithValue("@paramReceivableID", receivableID);

            int amountLeftToInsert = 0;
            try {
                DataTable amountLeftDataTable = DBConnectionManager.getData(amountLeftRetrievalCommand);
                amountLeftToInsert = Convert.ToInt32(amountLeftDataTable.Rows[0].ItemArray[0]);
            } catch (MySqlException ex) {
                Console.WriteLine(ex.Message);
                return false;
            } catch(InvalidCastException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }


            return valueToInsert <= amountLeftToInsert;
        }
    }
}
