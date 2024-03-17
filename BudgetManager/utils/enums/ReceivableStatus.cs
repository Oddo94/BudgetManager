using System;
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
        OVERDUE,
        [Description("Undefined")]
        UNDEFINED
    }
    public static class ReceivableStatusExtension {
        public static ReceivableStatus getReceivableStatusByDescription(String description) {
            switch (description) {
                case "New":
                    return ReceivableStatus.NEW;

                case "Partially paid":
                    return ReceivableStatus.PARTIALLY_PAID;

                case "Paid":
                    return ReceivableStatus.PAID;

                case "Overdue":
                    return ReceivableStatus.OVERDUE;

                default:
                    return ReceivableStatus.UNDEFINED;
            }
        }
    }
}




