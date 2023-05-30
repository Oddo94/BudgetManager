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
        private DateTime creationDate;
        private int accountBalance;
        private int totalInTransfers;
        private int totalOutTransfers;
        private int totalUnpaidReceivables;
        private int totalInterestAmount;

        public ExternalAccountDetailsModel(string accountName, string bankName, DateTime creationDate, int accountBalance, int totalInTransfers, int totalOutTransfers, int totalUnpaidReceivables, int totalInterestAmount)
        {
            this.accountName = accountName;
            this.bankName = bankName;
            this.creationDate = creationDate;
            this.accountBalance = accountBalance;
            this.totalInTransfers = totalInTransfers;
            this.totalOutTransfers = totalOutTransfers;
            this.totalUnpaidReceivables = totalUnpaidReceivables;
            this.totalInterestAmount = totalInterestAmount;
        }

        public string AccountName { get => accountName; set => accountName = value; }
        public string BankName { get => bankName; set => bankName = value; }
        public DateTime CreationDate { get => creationDate; set => creationDate = value; }
        [Required(ErrorMessage = "The account balance cannot be null! It must be greater or equal to 0.")]
        public int AccountBalance { get => accountBalance; set => accountBalance = value; }
        [Required(ErrorMessage = "The total IN transfers value cannot be null! It must be greater or equal to 0.")]
        public int TotalInTransfers { get => totalInTransfers; set => totalInTransfers = value; }
        [Required(ErrorMessage = "The total OUT transfers value cannot be null! It must be greater or equal to 0.")]
        public int TotalOutTransfers { get => totalOutTransfers; set => totalOutTransfers = value; }
        [Required(ErrorMessage = "The total unpaid receivables value cannot be null! It must be greater or equal to 0.")]
        public int TotalUnpaidReceivables { get => totalUnpaidReceivables; set => totalUnpaidReceivables = value; }
        [Required(ErrorMessage = "The total interest amount value cannot be null! It must be greater or equal to 0.")]
        public int TotalInterestAmount { get => totalInterestAmount; set => totalInterestAmount = value; }
    }
}
