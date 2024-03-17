using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.utils.enums;

namespace BudgetManager.mvc.models.dto {
    public class PartialPaymentDTO : IDataInsertionDTO {
        private int receivableID;
        private String paymentName;
        private int paymentValue;
        private String paymentDate;

        public PartialPaymentDTO(int receivableID, String paymentName, int paymentValue, String paymentDate) {
            this.receivableID = receivableID;
            this.paymentName = paymentName;
            this.paymentValue = paymentValue;
            this.paymentDate = paymentDate;
        }

        public int ReceivableID {
            get {
                return this.receivableID;
            }

            set {
                this.receivableID = value;
            }
        }

        public String PaymentName {
            get {
                return this.paymentName;
            }

            set {
                this.paymentName = value;
            }
        }

        public int PaymentValue {
            get {
                return this.paymentValue;
            }

            set {
                this.paymentValue = value;
            }
        }

        public String PaymentDate {
            get {
                return this.paymentDate;
            }

            set {
                this.paymentDate = value;
            }
        }

        public BudgetItemType getBudgetItemType() {
            return BudgetItemType.PARTIAL_PAYMENT;
        }
    }
}
