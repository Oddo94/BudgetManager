using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils.data_insertion {
    class FormFieldWrapper {
        private Control formField;
        private bool isRequired;

        public FormFieldWrapper(Control formField, bool isRequired) {
            this.formField = formField;
            this.isRequired = isRequired;
        }

        public Control FormField {
            get {
                return this.formField;
            }

            set {
                this.formField = value;
            }
        }

            public bool IsRequired {
            get {
                return this.isRequired;
            }

            set {
                this.isRequired = value;
            }
        }
    }
}
