using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.models.dto {
    class SavingAccountInterestDTO : IDataInsertionDTO {
        private String creationDate;
        private String interestName;
        private String accountName;
        private String interestType;
        private String paymentType;
        private double interestRate;
        private int interestValue;
        private int userID;

        public SavingAccountInterestDTO() {

        }

        public SavingAccountInterestDTO(String creationDate, String interestName, String accountName, String interestType, String paymentType, double interestRate, int interestValue, int userID) {
            this.creationDate = creationDate;
            this.interestName = interestName;
            this.accountName = accountName;
            this.interestType = interestType;
            this.paymentType = paymentType;
            this.interestRate = interestRate;
            this.interestValue = interestValue;
            this.userID = userID;
        }

        public String CreationDate {
            get {
                return this.creationDate;
            }

            set {
                this.creationDate = value;
            }
        }

        public String InterestName {
            get {
                return this.interestName;
            }

            set {
                this.interestName = value;
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

        public String InterestType {
            get {
                return this.interestType;
            }

            set {
                this.interestType = value;
            }
        }

        public String PaymentType {
            get {
                return this.paymentType;
            }

            set {
                this.paymentType = value;
            }
        }

        public double InterestRate {
            get {
                return this.interestRate;
            }

            set {
                this.interestRate = value;
            }
        }

        public int InterestValue {
            get {
                return this.interestValue;
            }

            set {
                this.interestValue = value;
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

        public string getName() {
            return this.interestName;
        }
    }
}
