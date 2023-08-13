using BudgetManager.utils.data_insertion;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                } else if (control is RichTextBox) {
                    ((RichTextBox)control).Text = "";
                } else if(control is NumericUpDown) {
                    NumericUpDown numericUpDown = ((NumericUpDown)control);
                    //Resets the numeric up down controls to its minium specified value 
                    numericUpDown.Value = numericUpDown.Minimum;
                }
            }
        }

        //OVERLOAD(modify code so that it will be the only method used for clearing the controls)
        public static void clearActiveControls(List<FormFieldWrapper> activeControls) {
            Guard.notNull(activeControls, "active controls list", "The active controls list cannot be null");

            //Takes each control and checks its type
            //If it is of the specified type it casts it to that type before invoking the specific method needed to clear it
            foreach (FormFieldWrapper currentItem in activeControls) {
                Control control = currentItem.FormField;
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
                } else if (control is RichTextBox) {
                    ((RichTextBox)control).Text = "";
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
                    content = ((TextBox)control).Text;
                    isEmpty = "".Equals(content) ? true : false;
                } else if (control is ComboBox) {
                    //Setting SelectedIndex to -1 when any item other than the first one is selected does not work properly
                    index = ((ComboBox)control).SelectedIndex;
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


        //It currently works only for text boxes, combo boxes and check boxes(OVERLOAD)
        public static bool hasDataOnRequiredFields(List<FormFieldWrapper> activeControls) {
            Guard.notNull(activeControls, "active controls list", "The active controls list cannot be null");

            String content = null;
            int index = 0;
            bool isEmpty = false;

            //Takes each control and checks its type
            //If it is of the specified type it casts it to that type before invoking the specific method needed to clear it
            foreach (FormFieldWrapper currentItem in activeControls) {
                Control control = currentItem.FormField;
                bool isRequired = currentItem.IsRequired;

                //The controls are checked only if they are marked as required by the boolean flag contained by the FormFieldWrapper object
                if (control is TextBox && isRequired) {
                    content = ((TextBox)control).Text;
                    isEmpty = "".Equals(content) ? true : false;
                } else if (control is ComboBox && isRequired) {
                    //Setting SelectedIndex to -1 when any item other than the first one is selected does not work properly
                    index = ((ComboBox)control).SelectedIndex;
                    isEmpty = index == -1 ? true : false;
                } else if (control is CheckBox && isRequired) {
                    isEmpty = ((CheckBox)control).Checked;
                }

                if (isEmpty) {
                    return false;
                }

            }

            return true;
        }

        //Method for setting the state of a button based on the existence/inexistence of data on the specified controls(OVERLOAD)
        public static void setButtonState(Button targetButton, List<FormFieldWrapper> activeControls) {
            Guard.notNull(targetButton, "button object");
            Guard.notNull(activeControls, "active controls list");

            if (UserControlsManager.hasDataOnRequiredFields(activeControls)) {
                targetButton.Enabled = true;
                return;
            }

            targetButton.Enabled = false;

        }

        public static int fillComboBoxWithData(ComboBox targetComboBox, MySqlCommand dataRetrievalCommand, String displayMember) {
            Guard.notNull(targetComboBox, "The combobox object provided for being populated with data cannot be null.");
            Guard.notNull(displayMember, "column name");

            DataTable sourceDataTable = retrieveData(dataRetrievalCommand);

            //If there are now rows inside the retrieved DataTable object then it means that no data was found
            if(sourceDataTable.Rows.Count == 0) {
                return -1;
            }

            targetComboBox.DataSource = sourceDataTable;
            targetComboBox.DisplayMember = displayMember;

            return 0;
        }

        //Method used to transform a two column DataTable object into a map(for example retrieving account names and their corresponding currencies as key-value pairs)
        public static Dictionary<String, String> getMapFromDataTable(MySqlCommand dataRetrievalCommand) {
            Guard.notNull(dataRetrievalCommand, "data retrieval command");

            DataTable resultDataTable = retrieveData(dataRetrievalCommand);

            int columnCount = resultDataTable.Columns.Count;
            int expectedColumns = 2;
            //Checks to see if the result DataTable can be mapped to a Dictionary object(map)
            //The provided SQL command MUST retrieve a DataTable object with exactly two columns, otherwise this cannot be processed and transformed into a Dictionary object(map)
            if (columnCount !=  expectedColumns) {
                return null;
            }

            Dictionary<String, String> outputMap = new Dictionary<String, String>();

            for(int i = 0; i < resultDataTable.Rows.Count; i++) {
                String key = Convert.ToString(resultDataTable.Rows[i].ItemArray[0]);
                String value = Convert.ToString(resultDataTable.Rows[i].ItemArray[1]);

                outputMap.Add(key, value);
            }

            return outputMap;
        }

        public static void addControlsToContainer(Panel targetContainer, List<Control> controlsList) {
            Guard.notNull(targetContainer, "Controls container");
            Guard.notNull(controlsList, "Controls list");

            if (!controlsList.Any()) {
                return;
            }

            foreach (Control currentControl in controlsList) {
                targetContainer.Controls.Add(currentControl);
            }
        }


        public static void clearActiveControls(ArrayList activeControls) {
            Guard.notNull(activeControls, "The active controls list cannot be null");

            //Takes each control and checks its type
            //If it is of the specified type it casts it to that type before invoking the specific method needed to clear it
            foreach (FormFieldWrapper currentItem in activeControls) {
                Control control = currentItem.FormField;//gets the control object from the FormFieldWrapper object
                if (control is TextBox) {
                    ((TextBox) control).Text = "";
                } else if (control is ComboBox) {
                    //Setting SelectedIndex to -1 when any item other than the first one is selected does not work properly
                    ((ComboBox) control).SelectedIndex = -1;
                    ((ComboBox) control).SelectedIndex = -1;
                } else if (control is DateTimePicker) {
                    ((DateTimePicker) control).Value = DateTime.Now;
                } else if (control is RadioButton) {
                   //Generic reset behavior-sets the radio button 'Checked' property to false
                   ((RadioButton) control).Checked = false;
                }
            }
        }

        private static DataTable retrieveData(MySqlCommand dataRetrievalCommand) {
            Guard.notNull(dataRetrievalCommand, "SQL command");
         
            DataTable comboBoxDataTable = DBConnectionManager.getData(dataRetrievalCommand);

            return comboBoxDataTable;
        }

        public static bool isValidDateSelection(DateTimePicker startPicker, DateTimePicker endPicker) {
            Guard.notNull(startPicker, "start DateTimePicker");
            Guard.notNull(endPicker, "start DateTimePicker");

            DateTime startDate = startPicker.Value.Date;
            DateTime endDate = endPicker.Value.Date;

            return startDate <= endDate;
        }
    }


}
