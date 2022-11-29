using System.ComponentModel;

namespace BudgetManager.utils.enums {
     public enum ReceivableStatus {
        [Description("New")]
        NEW,
        [Description("Partially paid")]
        PARTIALLY_PAID,
        [Description("Paid")]
        PAID,
        [Description("Overdue")]
        OVERDUE
    }
}
