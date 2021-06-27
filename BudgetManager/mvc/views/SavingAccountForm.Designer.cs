namespace BudgetManager.mvc.views {
    partial class SavingAccountForm {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.label2 = new System.Windows.Forms.Label();
            this.startLabelSavingAccount = new System.Windows.Forms.Label();
            this.endLabelSavingAccount = new System.Windows.Forms.Label();
            this.monthPickerPanelSavingAccount = new System.Windows.Forms.FlowLayoutPanel();
            this.dateTimePickerEndSavingAccount = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePickerStartSavingAccount = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewSavingAccount = new System.Windows.Forms.DataGridView();
            this.columnChartMonthlyBalance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerMonthlyBalance = new System.Windows.Forms.DateTimePicker();
            this.savingAccountBalanceLabel = new System.Windows.Forms.Label();
            this.refreshBalanceDataButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.savingAccountComboBox = new System.Windows.Forms.ComboBox();
            this.infoLabelSavingAccount = new System.Windows.Forms.Label();
            this.intervalCheckBoxSavingAccount = new System.Windows.Forms.CheckBox();
            this.monthPickerPanelSavingAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSavingAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnChartMonthlyBalance)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(586, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Current saving account balance";
            // 
            // startLabelSavingAccount
            // 
            this.startLabelSavingAccount.AutoSize = true;
            this.startLabelSavingAccount.Location = new System.Drawing.Point(27, 158);
            this.startLabelSavingAccount.Name = "startLabelSavingAccount";
            this.startLabelSavingAccount.Size = new System.Drawing.Size(37, 13);
            this.startLabelSavingAccount.TabIndex = 2;
            this.startLabelSavingAccount.Text = "Month";
            // 
            // endLabelSavingAccount
            // 
            this.endLabelSavingAccount.AutoSize = true;
            this.endLabelSavingAccount.Location = new System.Drawing.Point(3, 0);
            this.endLabelSavingAccount.Name = "endLabelSavingAccount";
            this.endLabelSavingAccount.Size = new System.Drawing.Size(72, 13);
            this.endLabelSavingAccount.TabIndex = 3;
            this.endLabelSavingAccount.Text = "Ending month";
            // 
            // monthPickerPanelSavingAccount
            // 
            this.monthPickerPanelSavingAccount.Controls.Add(this.endLabelSavingAccount);
            this.monthPickerPanelSavingAccount.Controls.Add(this.dateTimePickerEndSavingAccount);
            this.monthPickerPanelSavingAccount.Location = new System.Drawing.Point(299, 158);
            this.monthPickerPanelSavingAccount.Name = "monthPickerPanelSavingAccount";
            this.monthPickerPanelSavingAccount.Size = new System.Drawing.Size(223, 55);
            this.monthPickerPanelSavingAccount.TabIndex = 4;
            this.monthPickerPanelSavingAccount.Visible = false;
            // 
            // dateTimePickerEndSavingAccount
            // 
            this.dateTimePickerEndSavingAccount.CustomFormat = "MM/yyyy";
            this.dateTimePickerEndSavingAccount.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEndSavingAccount.Location = new System.Drawing.Point(3, 16);
            this.dateTimePickerEndSavingAccount.Name = "dateTimePickerEndSavingAccount";
            this.dateTimePickerEndSavingAccount.ShowUpDown = true;
            this.dateTimePickerEndSavingAccount.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerEndSavingAccount.TabIndex = 4;
            this.dateTimePickerEndSavingAccount.Value = new System.DateTime(2021, 6, 1, 0, 0, 0, 0);
            this.dateTimePickerEndSavingAccount.ValueChanged += new System.EventHandler(this.dateTimePickerEndSavingAccount_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 462);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(238, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Select year for saving account balance evolution";
            // 
            // dateTimePickerStartSavingAccount
            // 
            this.dateTimePickerStartSavingAccount.CustomFormat = "MM/yyyy";
            this.dateTimePickerStartSavingAccount.Enabled = false;
            this.dateTimePickerStartSavingAccount.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStartSavingAccount.Location = new System.Drawing.Point(30, 174);
            this.dateTimePickerStartSavingAccount.Name = "dateTimePickerStartSavingAccount";
            this.dateTimePickerStartSavingAccount.ShowUpDown = true;
            this.dateTimePickerStartSavingAccount.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerStartSavingAccount.TabIndex = 7;
            this.dateTimePickerStartSavingAccount.Value = new System.DateTime(2021, 6, 1, 0, 0, 0, 0);
            this.dateTimePickerStartSavingAccount.ValueChanged += new System.EventHandler(this.dateTimePickerStartSavingAccount_ValueChanged);
            // 
            // dataGridViewSavingAccount
            // 
            this.dataGridViewSavingAccount.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSavingAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSavingAccount.Location = new System.Drawing.Point(30, 244);
            this.dataGridViewSavingAccount.Name = "dataGridViewSavingAccount";
            this.dataGridViewSavingAccount.ReadOnly = true;
            this.dataGridViewSavingAccount.Size = new System.Drawing.Size(524, 180);
            this.dataGridViewSavingAccount.TabIndex = 8;
            // 
            // columnChartMonthlyBalance
            // 
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months;
            chartArea1.AxisX.LabelStyle.Format = "MMM";
            chartArea1.Name = "ChartArea1";
            this.columnChartMonthlyBalance.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.columnChartMonthlyBalance.Legends.Add(legend1);
            this.columnChartMonthlyBalance.Location = new System.Drawing.Point(30, 526);
            this.columnChartMonthlyBalance.Name = "columnChartMonthlyBalance";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Monthly balance value";
            this.columnChartMonthlyBalance.Series.Add(series1);
            this.columnChartMonthlyBalance.Size = new System.Drawing.Size(714, 181);
            this.columnChartMonthlyBalance.TabIndex = 9;
            this.columnChartMonthlyBalance.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "Saving account balance evolution for ";
            this.columnChartMonthlyBalance.Titles.Add(title1);
            this.columnChartMonthlyBalance.MouseHover += new System.EventHandler(this.columnChartMonthlyBalance_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Select month/year";
            // 
            // dateTimePickerMonthlyBalance
            // 
            this.dateTimePickerMonthlyBalance.CustomFormat = "yyyy";
            this.dateTimePickerMonthlyBalance.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMonthlyBalance.Location = new System.Drawing.Point(30, 488);
            this.dateTimePickerMonthlyBalance.Name = "dateTimePickerMonthlyBalance";
            this.dateTimePickerMonthlyBalance.ShowUpDown = true;
            this.dateTimePickerMonthlyBalance.Size = new System.Drawing.Size(78, 20);
            this.dateTimePickerMonthlyBalance.TabIndex = 11;
            this.dateTimePickerMonthlyBalance.ValueChanged += new System.EventHandler(this.dateTimePickerMonthlyBalance_ValueChanged);
            // 
            // savingAccountBalanceLabel
            // 
            this.savingAccountBalanceLabel.AutoSize = true;
            this.savingAccountBalanceLabel.Location = new System.Drawing.Point(642, 107);
            this.savingAccountBalanceLabel.Name = "savingAccountBalanceLabel";
            this.savingAccountBalanceLabel.Size = new System.Drawing.Size(0, 13);
            this.savingAccountBalanceLabel.TabIndex = 12;
            // 
            // refreshBalanceDataButton
            // 
            this.refreshBalanceDataButton.Location = new System.Drawing.Point(612, 148);
            this.refreshBalanceDataButton.Name = "refreshBalanceDataButton";
            this.refreshBalanceDataButton.Size = new System.Drawing.Size(97, 23);
            this.refreshBalanceDataButton.TabIndex = 13;
            this.refreshBalanceDataButton.Text = "Refresh data";
            this.refreshBalanceDataButton.UseVisualStyleBackColor = true;
            this.refreshBalanceDataButton.Click += new System.EventHandler(this.refreshBalanceDataButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Select item";
            // 
            // savingAccountComboBox
            // 
            this.savingAccountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.savingAccountComboBox.FormattingEnabled = true;
            this.savingAccountComboBox.Items.AddRange(new object[] {
            "Saving",
            "Saving account expense"});
            this.savingAccountComboBox.Location = new System.Drawing.Point(30, 63);
            this.savingAccountComboBox.Name = "savingAccountComboBox";
            this.savingAccountComboBox.Size = new System.Drawing.Size(121, 21);
            this.savingAccountComboBox.TabIndex = 15;
            this.savingAccountComboBox.SelectedIndexChanged += new System.EventHandler(this.savingAccountComboBox_SelectedIndexChanged);
            // 
            // infoLabelSavingAccount
            // 
            this.infoLabelSavingAccount.AutoSize = true;
            this.infoLabelSavingAccount.Location = new System.Drawing.Point(30, 217);
            this.infoLabelSavingAccount.Name = "infoLabelSavingAccount";
            this.infoLabelSavingAccount.Size = new System.Drawing.Size(119, 13);
            this.infoLabelSavingAccount.TabIndex = 16;
            this.infoLabelSavingAccount.Text = "Selected item record list";
            // 
            // intervalCheckBoxSavingAccount
            // 
            this.intervalCheckBoxSavingAccount.AutoSize = true;
            this.intervalCheckBoxSavingAccount.Enabled = false;
            this.intervalCheckBoxSavingAccount.Location = new System.Drawing.Point(30, 124);
            this.intervalCheckBoxSavingAccount.Name = "intervalCheckBoxSavingAccount";
            this.intervalCheckBoxSavingAccount.Size = new System.Drawing.Size(93, 17);
            this.intervalCheckBoxSavingAccount.TabIndex = 17;
            this.intervalCheckBoxSavingAccount.Text = "Month interval";
            this.intervalCheckBoxSavingAccount.UseVisualStyleBackColor = true;
            this.intervalCheckBoxSavingAccount.CheckedChanged += new System.EventHandler(this.intervalCheckBoxSavingAccount_CheckedChanged);
            // 
            // SavingAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 719);
            this.Controls.Add(this.intervalCheckBoxSavingAccount);
            this.Controls.Add(this.infoLabelSavingAccount);
            this.Controls.Add(this.savingAccountComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.refreshBalanceDataButton);
            this.Controls.Add(this.savingAccountBalanceLabel);
            this.Controls.Add(this.dateTimePickerMonthlyBalance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.columnChartMonthlyBalance);
            this.Controls.Add(this.dataGridViewSavingAccount);
            this.Controls.Add(this.dateTimePickerStartSavingAccount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.monthPickerPanelSavingAccount);
            this.Controls.Add(this.startLabelSavingAccount);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SavingAccountForm";
            this.Text = "Saving account manager";
            this.monthPickerPanelSavingAccount.ResumeLayout(false);
            this.monthPickerPanelSavingAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSavingAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnChartMonthlyBalance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label startLabelSavingAccount;
        private System.Windows.Forms.Label endLabelSavingAccount;
        private System.Windows.Forms.FlowLayoutPanel monthPickerPanelSavingAccount;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndSavingAccount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartSavingAccount;
        private System.Windows.Forms.DataGridView dataGridViewSavingAccount;
        private System.Windows.Forms.DataVisualization.Charting.Chart columnChartMonthlyBalance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerMonthlyBalance;
        private System.Windows.Forms.Label savingAccountBalanceLabel;
        private System.Windows.Forms.Button refreshBalanceDataButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox savingAccountComboBox;
        private System.Windows.Forms.Label infoLabelSavingAccount;
        private System.Windows.Forms.CheckBox intervalCheckBoxSavingAccount;
    }
}