using BudgetManager.utils.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    //Clasa utilitara ca ajuta la crearea unui obiect care incapsuleaza datele necesare pt executia frazei SQL(parametri efectivi ce vor fi folositi pt inlocuirea placeholders)
    public class QueryData {
        private int userID;
        private int debtorID;
        private int month;
        private int year;            
        private int expenseLimit;
        private int debtLimit;
        private int savingLimit;
        private int estimatedIncome;
        private int paidAmount;        
        private int planTypeID;
        private int thresholdPercentage;
        private int alarmExistenceValue;
        private int itemValue;
        private int itemTypeID;
        private int bankID;
        private int currencyID;
        private int sourceAccountID;    
        private int destinationAccountID;
        private int sentValue;
        private int receivedValue;
        private double exchangeRate;
        private String itemCreationDate;
        private String startDate;
        private String endDate;
        private String userName;
        private String bankName;
        private String currencyName; 
        private String tableName;
        private String budgetPlanName;
        private String typeName;
        private String itemName;
        private String itemIdentificationNumber;
        private String creditorName;
        private String debtorName;
        private String additionalData;    
        private IncomeSource incomeSource;
        private BudgetItemType budgetItemType;

        private QueryData() {

        }

        public int UserID {
            get {
                return this.userID;
            }
           
        }

        public int DebtorID {
            get {
                return this.debtorID;
            }
        }

        public int Month {
            get {
                return this.month;
            }         
        }
        public int Year {
            get {
                return this.year;
            }         
        }
           
        public int ExpenseLimit {
            get {
                return this.expenseLimit;
            }
        }

        public int DebtLimit {
            get {
                return this.debtLimit;
            }
        }

        public int SavingLimit {
            get {
                return this.savingLimit;
            }
        }

        public int EstimatedIncome {
            get {
                return this.estimatedIncome;
            }
        }

        public int PaidAmount {
            get {
                return this.paidAmount;
            }
        }

        public int PlanTypeID {
            get {
                return this.planTypeID;
            }
        }

        public int ThresholdPercentage {
            get {
                return this.thresholdPercentage;
            }
        }

        public int AlarmExistenceValue {
            get {
                return this.alarmExistenceValue;
            }
        }

        public int ItemValue {
            get {
                return this.itemValue;
            }
        }

        public int ItemTypeID {
            get {
                return this.itemTypeID;
            }
        }

        public int BankID {
            get {
                return this.bankID;
            }
        }

        public int CurrencyID {
            get {
                return this.currencyID;
            }
        }

        public int SourceAccountID {
            get {
                return this.sourceAccountID;
            }
        }

        public int DestinationAccountID {
            get {
                return this.destinationAccountID;
            }
        }

        public int SentValue {
            get {
                return this.sentValue;
            }
        }

        public int ReceivedValue {
            get {
                return this.receivedValue;
            }
        }

        public double ExchangeRate {
            get {
                return this.exchangeRate;
            }
        }

        public String ItemCreationDate {
            get {
                return this.itemCreationDate;
            }
        }

        public String StartDate {
            get {
                return this.startDate;
            }
        }

        public String EndDate {
            get {
                return this.endDate;
            }
        }

        public String UserName {
            get {
                return this.userName;
            }
        }

        public String BankName {
            get {
                return this.bankName;
            }
        }

        public String CurrencyName {
            get {
                return this.currencyName;
            }
        }


        public String TableName {
            get {
                return this.tableName;
            }
        }

        public String BudgetPlanName {
            get {
                return this.budgetPlanName;
            }
        }


        public String TypeName {
            get {
                return this.typeName;
            }
        }

        public String ItemName {
            get {
                return this.itemName;
            }
        }

        public String ItemIdentificationNumber {
            get {
                return this.itemIdentificationNumber;
            }
        }

        public String CreditorName {
            get {
                return this.creditorName;
            }
        }

        public String DebtorName {
            get {
                return this.debtorName;
            }
        }

        public String AdditionalData {
            get {
                return this.additionalData;
            }
        }

        public IncomeSource IncomeSource {
            get {
                return this.incomeSource;
            }
        }

        public BudgetItemType BudgetItemType {
            get {
                return this.budgetItemType;
            }
        }
      
        public class Builder {
            private int userID;
            private int debtorID;
            private int month;
            private int year;            
            private int expenseLimit;
            private int debtLimit;
            private int savingLimit;
            private int estimatedIncome;
            private int paidAmount;
            private int planTypeID;
            private int thresholdPercentage;
            private int alarmExistenceValue;
            private int itemValue;
            private int itemTypeID;
            private int bankID;
            private int currencyID;
            private int sourceAccountID;           
            private int destinationAccountID;
            private int sentValue;
            private int receivedValue;
            private double exchangeRate;
            private String itemCreationDate;
            private String startDate;
            private String endDate;
            private String userName;
            private String bankName;
            private String currencyName;
            private String tableName;
            private String budgetPlanName;
            private String typeName;
            private String itemName;
            private String itemIdentificationNumber;
            private String creditorName;
            private String debtorName;
            private String additionalData;
            private IncomeSource incomeSource;
            private BudgetItemType budgetItemType;

            public Builder() {

            }

            public Builder(int userID) {
                this.userID = userID;
            }

            public Builder addDebtorID(int debtorID) {
                this.debtorID = debtorID;

                return this;
            }

            public Builder addMonth(int month) {
                this.month = month;

                return this;
            }

            public Builder addYear(int year) {
                this.year = year;

                return this;
            }
       
            public Builder addExpenseLimit(int expenseLimit) {
                this.expenseLimit = expenseLimit;

                return this;
            }

            public Builder addDebtLimit(int debtLimit) {
                this.debtLimit = debtLimit;

                return this;
            }

            public Builder addSavingLimit(int savingLimit) {
                this.savingLimit = savingLimit;

                return this;
            }

            public Builder addEstimatedIncome(int estimatedIncome) {
                this.estimatedIncome = estimatedIncome;

                return this;
            }

            public Builder addPaidAmount (int paidAmount) {
                this.paidAmount = paidAmount;

                return this;
            }

            public Builder addPlanTypeID(int planTypeID) {
                this.planTypeID = planTypeID;

                return this;
            }

            public Builder addThresholdPercentage(int thresholdPercentage) {
                this.thresholdPercentage = thresholdPercentage;

                return this;
            }

            public Builder addAlarmExistenceValue(int alarmExistenceValue) {
                this.alarmExistenceValue = alarmExistenceValue;

                return this;
            }

            public Builder addItemValue(int itemValue) {
                this.itemValue = itemValue;

                return this;
            }

            public Builder addItemTypeID(int itemTypeID) {
                this.itemTypeID = itemTypeID;

                return this;
            }

            public Builder addBankID(int bankID) {
                this.bankID = bankID;

                return this;
            }

            public Builder addCurrencyID(int currencyID) {
                this.currencyID = currencyID;

                return this;
            } 

            public Builder addSourceAccountID(int sourceAccountID) {
                this.sourceAccountID = sourceAccountID;

                return this;
            }

            public Builder addDestinationAccountID(int destinationAccountID) {
                this.destinationAccountID = destinationAccountID;

                return this;
            }

            public Builder addExchangeRate(double exchangeRate) {
                this.exchangeRate = exchangeRate;

                return this;
            }

            public Builder addSentValue(int sentValue) {
                this.sentValue = sentValue;

                return this;
            }

            public Builder addReceivedValue(int receivedValue) {
                this.receivedValue = receivedValue;

                return this;
            }

            public Builder addItemCreationDate(String itemCreationDate) {
                this.itemCreationDate = itemCreationDate;

                return this;
            }

            public Builder addStartDate(String startDate) {
                this.startDate = startDate;

                return this;
            }

            public Builder addEndDate(String endDate) {
                this.endDate = endDate;

                return this;
            }

            public Builder addUserName(String userName) {
                this.userName = userName;

                return this;
            }

            public Builder addBankName(String bankName) {
                this.bankName = bankName;

                return this;
            }

            public Builder addCurrencyName(string currencyName) {
                this.currencyName = currencyName;

                return this;
            }

            public Builder addTableName(String tableName) {
                this.tableName = tableName;

                return this;
            }

            public Builder addBudgetPlanName(String budgetPlanName) {
                this.budgetPlanName = budgetPlanName;

                return this;
            }

            public Builder addTypeName(String typeName) {
                this.typeName = typeName;

                return this;
            }

            public Builder addItemName(String itemName) {
                this.itemName = itemName;

                return this;
            }

            public Builder addItemIdentificationNumber(String itemIdentificationNumber) {
                this.itemIdentificationNumber = itemIdentificationNumber;

                return this;
            }

            public Builder addCreditorName(String creditorName) {
                this.creditorName = creditorName;

                return this;
            }

            public Builder addDebtorName(String debtorName) {
                this.debtorName = debtorName;

                return this;
            } 

            public Builder addAdditionalData(String additionalData) {
                this.additionalData = additionalData;

                return this;
            }

            public Builder addIncomeSource(IncomeSource incomeSource) {
                this.incomeSource = incomeSource;

                return this;
            }

            public Builder addBudgetItemType(BudgetItemType budgetItemType) {
                this.budgetItemType = budgetItemType;

                return this;
            }

            public QueryData build() {
                return new QueryData {
                    userID = this.userID,
                    debtorID = this.debtorID,
                    month = this.month,
                    year = this.year,
                    expenseLimit = this.expenseLimit,
                    debtLimit = this.debtLimit,
                    savingLimit = this.savingLimit,
                    paidAmount = this.paidAmount,
                    estimatedIncome = this.estimatedIncome,
                    planTypeID = this.planTypeID,
                    thresholdPercentage = this.thresholdPercentage,
                    alarmExistenceValue = this.alarmExistenceValue,
                    itemValue = this.itemValue,
                    itemTypeID = this.itemTypeID,
                    bankID = this.bankID,
                    currencyID = this.currencyID,
                    sourceAccountID = this.sourceAccountID,
                    sentValue = this.sentValue,
                    receivedValue = this.receivedValue,
                    exchangeRate = this.exchangeRate,
                    destinationAccountID = this.destinationAccountID,
                    itemCreationDate = this.itemCreationDate,
                    startDate = this.startDate,
                    endDate = this.endDate,
                    userName = this.userName,
                    bankName = this.bankName,
                    currencyName = this.currencyName,
                    tableName = this.tableName,
                    budgetPlanName = this.budgetPlanName,
                    typeName = this.typeName,
                    itemName = this.itemName,
                    itemIdentificationNumber = this.itemIdentificationNumber,
                    creditorName = this.creditorName,
                    debtorName = this.debtorName,
                    additionalData = this.additionalData,
                    incomeSource = this.incomeSource,
                    budgetItemType = this.budgetItemType
                };
        }
        }
    }
}
