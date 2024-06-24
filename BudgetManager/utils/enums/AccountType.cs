
using System.ComponentModel;

namespace BudgetManager.utils.enums {
    public enum AccountType {
        SOURCE_ACCOUNT,
        DESTINATION_ACCOUNT,
        [Description("SYSTEM_DEFINED-DEFAULT_SAVING_ACCOUNT")]
        DEFAULT_ACCOUNT,
        [Description("USER_DEFINED-CUSTOM_SAVING_ACCOUNT")]
        CUSTOM_ACCOUNT,
        ALL_ACCOUNTS
    }
}
