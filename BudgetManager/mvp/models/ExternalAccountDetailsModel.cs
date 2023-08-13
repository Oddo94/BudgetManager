using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BudgetManager.mvp.models
{
    internal class ExternalAccountDetailsModel {
        private String accountName;
        private String bankName;
        private String accountCurrency;
        private String creationDate;
        private double accountBalance;
        private double totalInTransfers;
        private double totalOutTransfers;
        private double totalUnpaidReceivables;
        private double totalInterestAmount;

        public ExternalAccountDetailsModel(String accountName, String bankName, String accountCurrency, String creationDate, double accountBalance, double totalInTransfers, double totalOutTransfers, double totalUnpaidReceivables, double totalInterestAmount) {
            this.accountName = accountName;
            this.bankName = bankName;
            this.accountCurrency = accountCurrency;
            this.creationDate = creationDate;
            this.accountBalance = accountBalance;
            this.totalInTransfers = totalInTransfers;
            this.totalOutTransfers = totalOutTransfers;
            this.totalUnpaidReceivables = totalUnpaidReceivables;
            this.totalInterestAmount = totalInterestAmount;
        }

        public ExternalAccountDetailsModel() { }

        public String AccountName { get => accountName; set => accountName = value; }
        public String BankName { get => bankName; set => bankName = value; }
        public String AccountCurrency { get => accountCurrency; set => accountCurrency = value; }
        public String CreationDate { get => creationDate; set => creationDate = value; }
        [Required(ErrorMessage = "The account balance cannot be null! It must be greater or equal to 0.")]
        public double AccountBalance { get => accountBalance; set => accountBalance = value; }
        [Required(ErrorMessage = "The total IN transfers value cannot be null! It must be greater or equal to 0.")]
        public double TotalInTransfers { get => totalInTransfers; set => totalInTransfers = value; }
        [Required(ErrorMessage = "The total OUT transfers value cannot be null! It must be greater or equal to 0.")]
        public double TotalOutTransfers { get => totalOutTransfers; set => totalOutTransfers = value; }
        [Required(ErrorMessage = "The total unpaid receivables value cannot be null! It must be greater or equal to 0.")]
        public double TotalUnpaidReceivables { get => totalUnpaidReceivables; set => totalUnpaidReceivables = value; }
        [Required(ErrorMessage = "The total interest amount value cannot be null! It must be greater or equal to 0.")]
        public double TotalInterestAmount { get => totalInterestAmount; set => totalInterestAmount = value; }
    }
}
