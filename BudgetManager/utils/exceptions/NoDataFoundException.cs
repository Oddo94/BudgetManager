using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.utils.exceptions {
    [Serializable]
    public class NoDataFoundException : Exception {

        public string ItemName {
            get;
        }

        public NoDataFoundException() { }

        public NoDataFoundException(string message)
            : base(message) { }

        public NoDataFoundException(string message, Exception inner)
            : base(message, inner) { }

        public NoDataFoundException(string message, string itemName)
            : this(message) {

            ItemName = itemName;
        }
    }
}
