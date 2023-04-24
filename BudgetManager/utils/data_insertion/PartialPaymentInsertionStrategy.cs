using BudgetManager.non_mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.mvc.models.dto;
using MySql.Data.MySqlClient;

namespace BudgetManager.utils.data_insertion {
    class PartialPaymentInsertionStrategy : DataInsertionStrategy {
        //private IDataInsertionDTO dataInsertionDTO;

  String sqlStatementPartialPaymentInsertion = "INSERT INTO partial_payments(receivable_ID, paymentName, paymentValue, paymentDate) VALUES(@paramReceivableID, @paramPaymentName, @paramPaymentValue, @paramPaymentDate)";

  


        //public PartialPaymentInsertionStrategy(IDataInsertionDTO dataInsertionDTO) {
        //    this.dataInsertionDTO = dataInsertionDTO;
        //}

        public int execute(IDataInsertionDTO dataInsertionDTO) {
            Guard.notNull(dataInsertionDTO, "partial payment DTO");
            int executionResult = -1;

            PartialPaymentDTO partialPaymentDTO = (PartialPaymentDTO)dataInsertionDTO;

            MySqlCommand partialPaymentInsertionCommand = new MySqlCommand(sqlStatementPartialPaymentInsertion);
            partialPaymentInsertionCommand.Parameters.AddWithValue("@paramReceivableID", partialPaymentDTO.ReceivableID);
            partialPaymentInsertionCommand.Parameters.AddWithValue("@paramPaymentName", partialPaymentDTO.PaymentName);
            partialPaymentInsertionCommand.Parameters.AddWithValue("@paramPaymentValue", partialPaymentDTO.PaymentValue);
            partialPaymentInsertionCommand.Parameters.AddWithValue("@paramPaymentDate", partialPaymentDTO.PaymentDate);

            executionResult = DBConnectionManager.insertData(partialPaymentInsertionCommand);

            return executionResult;

        }

        public int execute(QueryData paramContainer) {
            throw new NotImplementedException();
        }
    }
}
