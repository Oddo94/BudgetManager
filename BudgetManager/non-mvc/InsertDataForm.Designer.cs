namespace BudgetManager {
    partial class InsertDataForm {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.newEntryDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.budgetItemComboBox = new System.Windows.Forms.ComboBox();
            this.incomeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.expenseTypeComboBox = new System.Windows.Forms.ComboBox();
            this.creditorNameComboBox = new System.Windows.Forms.ComboBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.addEntryButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Budget item";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 254);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 311);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Income type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 418);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Expense type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 470);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Creditor name";
            // 
            // newEntryDateTimePicker
            // 
            this.newEntryDateTimePicker.Location = new System.Drawing.Point(105, 80);
            this.newEntryDateTimePicker.Name = "newEntryDateTimePicker";
            this.newEntryDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.newEntryDateTimePicker.TabIndex = 7;
            // 
            // budgetItemComboBox
            // 
            this.budgetItemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.budgetItemComboBox.FormattingEnabled = true;
            this.budgetItemComboBox.Items.AddRange(new object[] {
            "Income",
            "Expense",
            "Debt",
            "Saving",
            "Creditor"});
            this.budgetItemComboBox.Location = new System.Drawing.Point(105, 140);
            this.budgetItemComboBox.Name = "budgetItemComboBox";
            this.budgetItemComboBox.Size = new System.Drawing.Size(121, 21);
            this.budgetItemComboBox.TabIndex = 8;
            this.budgetItemComboBox.SelectedIndexChanged += new System.EventHandler(this.budgetItemComboBox_SelectedIndexChanged);
            // 
            // incomeTypeComboBox
            // 
            this.incomeTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.incomeTypeComboBox.FormattingEnabled = true;
            this.incomeTypeComboBox.Location = new System.Drawing.Point(105, 359);
            this.incomeTypeComboBox.Name = "incomeTypeComboBox";
            this.incomeTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.incomeTypeComboBox.TabIndex = 9;
            this.incomeTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.incomeTypeComboBox_SelectedIndexChanged);
            // 
            // expenseTypeComboBox
            // 
            this.expenseTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.expenseTypeComboBox.FormattingEnabled = true;
            this.expenseTypeComboBox.Location = new System.Drawing.Point(105, 410);
            this.expenseTypeComboBox.Name = "expenseTypeComboBox";
            this.expenseTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.expenseTypeComboBox.TabIndex = 10;
            this.expenseTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.expenseTypeComboBox_SelectedIndexChanged);
            // 
            // creditorNameComboBox
            // 
            this.creditorNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.creditorNameComboBox.FormattingEnabled = true;
            this.creditorNameComboBox.Location = new System.Drawing.Point(105, 462);
            this.creditorNameComboBox.Name = "creditorNameComboBox";
            this.creditorNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.creditorNameComboBox.TabIndex = 11;
            this.creditorNameComboBox.SelectedIndexChanged += new System.EventHandler(this.creditorNameComboBox_SelectedIndexChanged);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(105, 247);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 20);
            this.nameTextBox.TabIndex = 12;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(105, 311);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(200, 20);
            this.valueTextBox.TabIndex = 13;
            this.valueTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Enabled = false;
            this.addEntryButton.Location = new System.Drawing.Point(31, 546);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(82, 23);
            this.addEntryButton.TabIndex = 14;
            this.addEntryButton.Text = "Add entry";
            this.addEntryButton.UseVisualStyleBackColor = true;
            this.addEntryButton.Click += new System.EventHandler(this.addEntryButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(233, 546);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(82, 23);
            this.resetButton.TabIndex = 15;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(412, 546);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(82, 23);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // InsertDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 613);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.addEntryButton);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.creditorNameComboBox);
            this.Controls.Add(this.expenseTypeComboBox);
            this.Controls.Add(this.incomeTypeComboBox);
            this.Controls.Add(this.budgetItemComboBox);
            this.Controls.Add(this.newEntryDateTimePicker);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "InsertDataForm";
            this.Text = "Insert data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker newEntryDateTimePicker;
        private System.Windows.Forms.ComboBox budgetItemComboBox;
        private System.Windows.Forms.ComboBox incomeTypeComboBox;
        private System.Windows.Forms.ComboBox expenseTypeComboBox;
        private System.Windows.Forms.ComboBox creditorNameComboBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.Button addEntryButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button cancelButton;
    }
}