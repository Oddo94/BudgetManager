using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils.enums {
    public enum BudgetItemType {
        [Description("income")]
        INCOME,
        [Description("expense")]
        GENERAL_EXPENSE,
        [Description("saving account expense")]
        SAVING_ACCOUNT_EXPENSE,
        [Description("debt")]
        DEBT,
        [Description("receivable")]
        RECEIVABLE,
        [Description("saving")]
        SAVING,
        [Description("creditor")]
        CREDITOR,
        [Description("debtor")]
        DEBTOR,
        [Description("saving account interest")]
        SAVING_ACCOUNT_INTEREST,
        [Description("partial payment")]
        PARTIAL_PAYMENT,
        [Description("transfer")]
        ACCOUNT_TRANSFER,
        [Description("external account banking fee")]
        EXTERNAL_ACCOUNT_BANKING_FEE,
        UNDEFINED
    }
}
