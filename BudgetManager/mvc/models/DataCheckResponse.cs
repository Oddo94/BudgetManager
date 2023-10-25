using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvc.models {
    internal class DataCheckResponse {
        private int executionResult;
        private String successMessage;
        private String errorMessage;

        public int ExecutionResult {
            get {
                return this.executionResult;
            }

            set {
                this.executionResult = value;
            }
        }

        public String SuccessMessage {
            get {
                return this.successMessage;
            }

            set {
                this.successMessage = value;
            }
        }

        public String ErrorMessage {
            get {
                return this.errorMessage;
            }

            set {
                this.errorMessage = value;
            }
        }

    }
}
