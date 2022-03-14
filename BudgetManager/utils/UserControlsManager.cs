using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.utils {
    class UserControlsManager {


        public static void clearActiveControls(List<Control> activeControls) {
            Guard.notNull(activeControls, "active controls list", "The active controls list cannot be null");

            //Takes each control and checks its type
            //If it is of the specified type it casts it to that type before invoking the specific method needed to clear it
            foreach (Control control in activeControls) {
                if (control is TextBox) {
                    ((TextBox)control).Text = "";
                } else if (control is ComboBox) {
                    //Setting SelectedIndex to -1 when any item other than the first one is selected does not work properly
                    ((ComboBox)control).SelectedIndex = -1;
                    ((ComboBox)control).SelectedIndex = -1;
                } else if (control is DateTimePicker) {
                    ((DateTimePicker)control).Value = DateTime.Now;
                } else if (control is RadioButton) {
                    //Sets the "General incomes" radio button as the default selection
                    RadioButton radioButton = (RadioButton)control;
                    radioButton.Checked = false;
                }
            }
        }


        //It currently works only for text boxes, combo boxes and check boxes
        public static bool hasDataOnRequiredFields(List<Control> activeControls) {
            Guard.notNull(activeControls, "active controls list", "The active controls list cannot be null");

            String content = null;
            int index = 0;
            bool isEmpty = false;

            //Takes each control and checks its type
            //If it is of the specified type it casts it to that type before invoking the specific method needed to clear it
            foreach (Control control in activeControls) {
                if (control is TextBox) {
                    content = ((TextBox) control).Text;
                    isEmpty = "".Equals(content) ? true : false;
                } else if (control is ComboBox) {
                    //Setting SelectedIndex to -1 when any item other than the first one is selected does not work properly
                    index = ((ComboBox) control).SelectedIndex;
                    isEmpty = index == -1 ? true : false;
                } else if (control is CheckBox) {
                    isEmpty = ((CheckBox)control).Checked;
                }

                if (isEmpty) {
                    return false;
                }
            
            }

            return true;
        }

        //Method for setting the state of a button based on the existence/inexistence of data on the specified controls
        public static void setButtonState(Button targetButton, List<Control> activeControls) {
            Guard.notNull(targetButton, "button object");
            Guard.notNull(activeControls, "active controls list");

            if (UserControlsManager.hasDataOnRequiredFields(activeControls)) {
                targetButton.Enabled = true;
                return;
            }

            targetButton.Enabled = false;

        }
    }
}
