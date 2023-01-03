using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils.data_insertion {
    class PartialPaymentInsertionStrategy {
        String sqlStatementPartialPaymentInsertion = "INSERT INTO partial_payments(receivable_ID, paymentName, paymentValue, paymentDate) VALUES(@paramReceivableID, @paramPaymentName, @paramPaymentValue, @paramPaymentDate)";
    }
}
