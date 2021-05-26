namespace BudgetManager {
    partial class UpdateUserDataForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewTableDisplay = new System.Windows.Forms.DataGridView();
            this.submitButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.monthRecordsCheckBox = new System.Windows.Forms.CheckBox();
            this.yearRecordsCheckBox = new System.Windows.Forms.CheckBox();
            this.dateTimePickerTimeSpanSelection = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // tableSelectionComboBox
            // 
            this.tableSelectionComboBox.Enabled = false;
            this.tableSelectionComboBox.FormattingEnabled = true;
            this.tableSelectionComboBox.Items.AddRange(new object[] {
            "Incomes",
            "General expenses",
            "Saving account expenses",
            "Debts",
            "Savings"});
            this.tableSelectionComboBox.Location = new System.Drawing.Point(46, 100);
            this.tableSelectionComboBox.Name = "tableSelectionComboBox";
            this.tableSelectionComboBox.Size = new System.Drawing.Size(146, 21);
            this.tableSelectionComboBox.TabIndex = 1;
            this.tableSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.tableSelectionComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select table";
            // 
            // dataGridViewTableDisplay
            // 
            this.dataGridViewTableDisplay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTableDisplay.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewTableDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTableDisplay.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTableDisplay.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewTableDisplay.Location = new System.Drawing.Point(28, 180);
            this.dataGridViewTableDisplay.Name = "dataGridViewTableDisplay";
            this.dataGridViewTableDisplay.Size = new System.Drawing.Size(550, 258);
            this.dataGridViewTableDisplay.TabIndex = 3;
            this.dataGridViewTableDisplay.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTableDisplay_CellClick);
            this.dataGridViewTableDisplay.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTableDisplay_CellEnter);
            this.dataGridViewTableDisplay.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTableDisplay_CellMouseClick);
            this.dataGridViewTableDisplay.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTableDisplay_CellValueChanged);
            // 
            // submitButton
            // 
            this.submitButton.Enabled = false;
            this.submitButton.Location = new System.Drawing.Point(49, 458);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(115, 23);
            this.submitButton.TabIndex = 4;
            this.submitButton.Text = "Submit changes";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(46, 142);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(105, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Delete record";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // monthRecordsCheckBox
            // 
            this.monthRecordsCheckBox.AutoSize = true;
            this.monthRecordsCheckBox.Location = new System.Drawing.Point(308, 77);
            this.monthRecordsCheckBox.Name = "monthRecordsCheckBox";
            this.monthRecordsCheckBox.Size = new System.Drawing.Size(94, 17);
            this.monthRecordsCheckBox.TabIndex = 6;
            this.monthRecordsCheckBox.Text = "Month records";
            this.monthRecordsCheckBox.UseVisualStyleBackColor = true;
            this.monthRecordsCheckBox.CheckedChanged += new System.EventHandler(this.monthRecordsCheckBox_CheckedChanged);
            // 
            // yearRecordsCheckBox
            // 
            this.yearRecordsCheckBox.AutoSize = true;
            this.yearRecordsCheckBox.Location = new System.Drawing.Point(308, 100);
            this.yearRecordsCheckBox.Name = "yearRecordsCheckBox";
            this.yearRecordsCheckBox.Size = new System.Drawing.Size(86, 17);
            this.yearRecordsCheckBox.TabIndex = 7;
            this.yearRecordsCheckBox.Text = "Year records";
            this.yearRecordsCheckBox.UseVisualStyleBackColor = true;
            this.yearRecordsCheckBox.CheckedChanged += new System.EventHandler(this.yearRecordsCheckBox_CheckedChanged);
            // 
            // dateTimePickerTimeSpanSelection
            // 
            this.dateTimePickerTimeSpanSelection.CustomFormat = "MM/yyyy";
            this.dateTimePickerTimeSpanSelection.Enabled = false;
            this.dateTimePickerTimeSpanSelection.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTimeSpanSelection.Location = new System.Drawing.Point(308, 126);
            this.dateTimePickerTimeSpanSelection.Name = "dateTimePickerTimeSpanSelection";
            this.dateTimePickerTimeSpanSelection.ShowUpDown = true;
            this.dateTimePickerTimeSpanSelection.Size = new System.Drawing.Size(98, 20);
            this.dateTimePickerTimeSpanSelection.TabIndex = 8;
            this.dateTimePickerTimeSpanSelection.Value = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Select timespan";
            // 
            // UpdateUserDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 559);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerTimeSpanSelection);
            this.Controls.Add(this.yearRecordsCheckBox);
            this.Controls.Add(this.monthRecordsCheckBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.dataGridViewTableDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableSelectionComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UpdateUserDataForm";
            this.Text = "Update data";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox tableSelectionComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewTableDisplay;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.CheckBox monthRecordsCheckBox;
        private System.Windows.Forms.CheckBox yearRecordsCheckBox;
        private System.Windows.Forms.DateTimePicker dateTimePickerTimeSpanSelection;
        private System.Windows.Forms.Label label2;
    }
}