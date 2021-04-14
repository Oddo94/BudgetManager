namespace BudgetManager.mvc.views {
    partial class BudgetPlanManagementForm {
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
            this.monthRecordsCheckboxBP = new System.Windows.Forms.CheckBox();
            this.yearRecordsCheckboxBP = new System.Windows.Forms.CheckBox();
            this.dateTimePickerBPManagement = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewBPManagement = new System.Windows.Forms.DataGridView();
            this.submitButtonBPManagement = new System.Windows.Forms.Button();
            this.deleteButtonBPManagement = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewSelectedPlanInfo = new System.Windows.Forms.DataGridView();
            this.ItemTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PercentageFromLimitColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PercentageLimitColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalIncomesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.planManagementCalculatorButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBPManagement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedPlanInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select timerspan";
            // 
            // monthRecordsCheckboxBP
            // 
            this.monthRecordsCheckboxBP.AutoSize = true;
            this.monthRecordsCheckboxBP.Location = new System.Drawing.Point(24, 76);
            this.monthRecordsCheckboxBP.Name = "monthRecordsCheckboxBP";
            this.monthRecordsCheckboxBP.Size = new System.Drawing.Size(94, 17);
            this.monthRecordsCheckboxBP.TabIndex = 1;
            this.monthRecordsCheckboxBP.Text = "Month records";
            this.monthRecordsCheckboxBP.UseVisualStyleBackColor = true;
            this.monthRecordsCheckboxBP.CheckedChanged += new System.EventHandler(this.monthRecordsCheckboxBP_CheckedChanged);
            // 
            // yearRecordsCheckboxBP
            // 
            this.yearRecordsCheckboxBP.AutoSize = true;
            this.yearRecordsCheckboxBP.Location = new System.Drawing.Point(24, 108);
            this.yearRecordsCheckboxBP.Name = "yearRecordsCheckboxBP";
            this.yearRecordsCheckboxBP.Size = new System.Drawing.Size(86, 17);
            this.yearRecordsCheckboxBP.TabIndex = 2;
            this.yearRecordsCheckboxBP.Text = "Year records";
            this.yearRecordsCheckboxBP.UseVisualStyleBackColor = true;
            this.yearRecordsCheckboxBP.CheckedChanged += new System.EventHandler(this.yearRecordsCheckboxBP_CheckedChanged);
            // 
            // dateTimePickerBPManagement
            // 
            this.dateTimePickerBPManagement.CustomFormat = "MM/yyyy";
            this.dateTimePickerBPManagement.Enabled = false;
            this.dateTimePickerBPManagement.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerBPManagement.Location = new System.Drawing.Point(24, 148);
            this.dateTimePickerBPManagement.Name = "dateTimePickerBPManagement";
            this.dateTimePickerBPManagement.ShowUpDown = true;
            this.dateTimePickerBPManagement.Size = new System.Drawing.Size(104, 20);
            this.dateTimePickerBPManagement.TabIndex = 3;
            this.dateTimePickerBPManagement.ValueChanged += new System.EventHandler(this.dateTimePickerBPManagement_ValueChanged);
            // 
            // dataGridViewBPManagement
            // 
            this.dataGridViewBPManagement.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBPManagement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBPManagement.Location = new System.Drawing.Point(24, 198);
            this.dataGridViewBPManagement.Name = "dataGridViewBPManagement";
            this.dataGridViewBPManagement.Size = new System.Drawing.Size(669, 220);
            this.dataGridViewBPManagement.TabIndex = 4;
            this.dataGridViewBPManagement.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewBPManagement_CellMouseClick);
            this.dataGridViewBPManagement.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBPManagement_CellValueChanged);
            this.dataGridViewBPManagement.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewBPManagement_DataError);
            // 
            // submitButtonBPManagement
            // 
            this.submitButtonBPManagement.Enabled = false;
            this.submitButtonBPManagement.Location = new System.Drawing.Point(22, 686);
            this.submitButtonBPManagement.Name = "submitButtonBPManagement";
            this.submitButtonBPManagement.Size = new System.Drawing.Size(106, 23);
            this.submitButtonBPManagement.TabIndex = 5;
            this.submitButtonBPManagement.Text = "Submit changes";
            this.submitButtonBPManagement.UseVisualStyleBackColor = true;
            this.submitButtonBPManagement.Click += new System.EventHandler(this.submitButtonBPManagement_Click);
            // 
            // deleteButtonBPManagement
            // 
            this.deleteButtonBPManagement.Enabled = false;
            this.deleteButtonBPManagement.Location = new System.Drawing.Point(578, 686);
            this.deleteButtonBPManagement.Name = "deleteButtonBPManagement";
            this.deleteButtonBPManagement.Size = new System.Drawing.Size(92, 23);
            this.deleteButtonBPManagement.TabIndex = 6;
            this.deleteButtonBPManagement.Text = "Delete record";
            this.deleteButtonBPManagement.UseVisualStyleBackColor = true;
            this.deleteButtonBPManagement.Click += new System.EventHandler(this.deleteButtonBPManagement_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 461);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(252, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Information about the currently selected budget plan";
            // 
            // dataGridViewSelectedPlanInfo
            // 
            this.dataGridViewSelectedPlanInfo.AllowUserToAddRows = false;
            this.dataGridViewSelectedPlanInfo.AllowUserToDeleteRows = false;
            this.dataGridViewSelectedPlanInfo.AllowUserToOrderColumns = true;
            this.dataGridViewSelectedPlanInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectedPlanInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemTypeColumn,
            this.TotalValueColumn,
            this.PercentageFromLimitColumn,
            this.PercentageLimitColumn,
            this.MaxValueColumn,
            this.TotalIncomesColumn});
            this.dataGridViewSelectedPlanInfo.Location = new System.Drawing.Point(27, 494);
            this.dataGridViewSelectedPlanInfo.Name = "dataGridViewSelectedPlanInfo";
            this.dataGridViewSelectedPlanInfo.Size = new System.Drawing.Size(643, 103);
            this.dataGridViewSelectedPlanInfo.TabIndex = 8;
            // 
            // ItemTypeColumn
            // 
            this.ItemTypeColumn.HeaderText = "Item type";
            this.ItemTypeColumn.Name = "ItemTypeColumn";
            this.ItemTypeColumn.ReadOnly = true;
            // 
            // TotalValueColumn
            // 
            this.TotalValueColumn.HeaderText = "Current total value";
            this.TotalValueColumn.Name = "TotalValueColumn";
            this.TotalValueColumn.ReadOnly = true;
            // 
            // PercentageFromLimitColumn
            // 
            this.PercentageFromLimitColumn.HeaderText = "Percentage from limit value";
            this.PercentageFromLimitColumn.Name = "PercentageFromLimitColumn";
            this.PercentageFromLimitColumn.ReadOnly = true;
            // 
            // PercentageLimitColumn
            // 
            this.PercentageLimitColumn.HeaderText = "Percentage limit";
            this.PercentageLimitColumn.Name = "PercentageLimitColumn";
            this.PercentageLimitColumn.ReadOnly = true;
            // 
            // MaxValueColumn
            // 
            this.MaxValueColumn.HeaderText = "Max value limit";
            this.MaxValueColumn.Name = "MaxValueColumn";
            this.MaxValueColumn.ReadOnly = true;
            // 
            // TotalIncomesColumn
            // 
            this.TotalIncomesColumn.HeaderText = "Total incomes";
            this.TotalIncomesColumn.Name = "TotalIncomesColumn";
            this.TotalIncomesColumn.ReadOnly = true;
            // 
            // planManagementCalculatorButton
            // 
            this.planManagementCalculatorButton.Location = new System.Drawing.Point(281, 686);
            this.planManagementCalculatorButton.Name = "planManagementCalculatorButton";
            this.planManagementCalculatorButton.Size = new System.Drawing.Size(107, 23);
            this.planManagementCalculatorButton.TabIndex = 9;
            this.planManagementCalculatorButton.Text = "Show calculator";
            this.planManagementCalculatorButton.UseVisualStyleBackColor = true;
            this.planManagementCalculatorButton.Click += new System.EventHandler(this.planManagementCalculatorButton_Click);
            // 
            // BudgetPlanManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 777);
            this.Controls.Add(this.planManagementCalculatorButton);
            this.Controls.Add(this.dataGridViewSelectedPlanInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.deleteButtonBPManagement);
            this.Controls.Add(this.submitButtonBPManagement);
            this.Controls.Add(this.dataGridViewBPManagement);
            this.Controls.Add(this.dateTimePickerBPManagement);
            this.Controls.Add(this.yearRecordsCheckboxBP);
            this.Controls.Add(this.monthRecordsCheckboxBP);
            this.Controls.Add(this.label1);
            this.Name = "BudgetPlanManagementForm";
            this.Text = "Budget plan management";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBPManagement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedPlanInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox monthRecordsCheckboxBP;
        private System.Windows.Forms.CheckBox yearRecordsCheckboxBP;
        private System.Windows.Forms.DateTimePicker dateTimePickerBPManagement;
        private System.Windows.Forms.DataGridView dataGridViewBPManagement;
        private System.Windows.Forms.Button submitButtonBPManagement;
        private System.Windows.Forms.Button deleteButtonBPManagement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewSelectedPlanInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercentageFromLimitColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercentageLimitColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalIncomesColumn;
        private System.Windows.Forms.Button planManagementCalculatorButton;
    }
}