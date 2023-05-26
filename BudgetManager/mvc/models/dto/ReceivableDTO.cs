using BudgetManager.utils.enums;
using System;

namespace BudgetManager.mvc.models.dto {
    class ReceivableDTO : IDataInsertionDTO {
        private String name;
        private int value;
        private String debtorName;
        private String accountName;
        private int totalPaidAmount;
        private ReceivableStatus status;
        private String createdDate;
        private String dueDate;
        private int userID;

        public ReceivableDTO() {

        }

        public ReceivableDTO(String name, int value, String debtorName, String accountName, int totalPaidAmount, ReceivableStatus status, String createdDate, String dueDate, int userID) {
            this.name = name;
            this.value = value;
            this.debtorName = debtorName;
            this.accountName = accountName;
            this.totalPaidAmount = totalPaidAmount;
            this.status = status;
            this.createdDate = createdDate;
            this.dueDate = dueDate;
            this.userID = userID;
        }

        public String Name {
            get {
                return this.name;
            }

            set {
                this.name = value;
            }
        }

        public int Value {
            get {
                return this.value;
            }

            set {
                this.value = value;
            }
        }

        public String DebtorName {
            get {
                return this.debtorName;
            }

            set {
                this.debtorName = value;
            }
        }

        public String AccountName {
            get {
                return this.accountName;
            }

            set {
                this.accountName = value;
            }
        }

        public int TotalPaidAmount {
            get {
                return this.totalPaidAmount;
            }

            set {
                this.totalPaidAmount = value;
            }
        }

        public ReceivableStatus ReceivableStatus {
            get {
                return this.status;
            }

            set {
                this.status = value;
            }
        }

        public String CreatedDate {
            get {
                return this.createdDate;
            }

            set {
                this.createdDate = value;
            }
        }

        public String DueDate {
            get {
                return this.dueDate;
            }

            set {
                this.dueDate = value;
            }
        }

        public int UserID {
            get {
                return this.userID;
            }

            set {
                this.userID = value;
            }
        }

        public BudgetItemType getBudgetItemType() {
            return BudgetItemType.RECEIVABLE;
        }
    }
}
