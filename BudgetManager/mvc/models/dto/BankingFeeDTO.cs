using BudgetManager.utils.enums;
using System;

namespace BudgetManager.mvc.models.dto {
    internal class BankingFeeDTO : IDataInsertionDTO {
        private String accountName;
        private String name;
        private double value;
        private String createdDate;
        private int userID;

        public BankingFeeDTO() { }

        public BankingFeeDTO(String accountName, String name, double value, String createdDate, int userID) {
            this.accountName = accountName;
            this.name = name;
            this.value = value;
            this.createdDate = createdDate;
            this.userID = userID;
        }

        public String AccountName {
            get {
                return this.accountName;
            }

            set {
                this.accountName = value;
            }
        }

        public String Name {
            get {
                return this.name;
            }

            set {
                this.name = value;
            }
        }

        public double Value {
            get {
                return this.value;
            }

            set {
                this.value = value;
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

        public int UserID {
            get {
                return this.userID;
            }

            set {
                this.userID = value;
            }
        }

        public BudgetItemType getBudgetItemType() {
            return BudgetItemType.EXTERNAL_ACCOUNT_BANKING_FEE;
        }
    }
}
