namespace BudgetManager.non_mvc {
    partial class BudgetPlanCreator {
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
            this.oneMonthCheckBox = new System.Windows.Forms.CheckBox();
            this.sixMonthsCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.createPlanButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.planNameTextBox = new System.Windows.Forms.TextBox();
            this.startMonthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.expensesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.debtsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.savingsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.alarmCheckBox = new System.Windows.Forms.CheckBox();
            this.thresholdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.startMonthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.expensesNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.debtsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.savingsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Plan type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Select starting month";
            // 
            // oneMonthCheckBox
            // 
            this.oneMonthCheckBox.AutoSize = true;
            this.oneMonthCheckBox.Location = new System.Drawing.Point(15, 53);
            this.oneMonthCheckBox.Name = "oneMonthCheckBox";
            this.oneMonthCheckBox.Size = new System.Drawing.Size(78, 17);
            this.oneMonthCheckBox.TabIndex = 2;
            this.oneMonthCheckBox.Text = "One month";
            this.oneMonthCheckBox.UseVisualStyleBackColor = true;
            this.oneMonthCheckBox.CheckedChanged += new System.EventHandler(this.oneMonthCheckBox_CheckedChanged);
            // 
            // sixMonthsCheckBox
            // 
            this.sixMonthsCheckBox.AutoSize = true;
            this.sixMonthsCheckBox.Location = new System.Drawing.Point(15, 85);
            this.sixMonthsCheckBox.Name = "sixMonthsCheckBox";
            this.sixMonthsCheckBox.Size = new System.Drawing.Size(77, 17);
            this.sixMonthsCheckBox.TabIndex = 3;
            this.sixMonthsCheckBox.Text = "Six months";
            this.sixMonthsCheckBox.UseVisualStyleBackColor = true;
            this.sixMonthsCheckBox.CheckedChanged += new System.EventHandler(this.sixMonthsCheckBox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select percentage limits for each item";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Expenses limit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 377);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Savings limit";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 333);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Debts limit";
            // 
            // createPlanButton
            // 
            this.createPlanButton.Enabled = false;
            this.createPlanButton.Location = new System.Drawing.Point(203, 596);
            this.createPlanButton.Name = "createPlanButton";
            this.createPlanButton.Size = new System.Drawing.Size(75, 23);
            this.createPlanButton.TabIndex = 12;
            this.createPlanButton.Text = "Create plan";
            this.createPlanButton.UseVisualStyleBackColor = true;
            this.createPlanButton.Click += new System.EventHandler(this.createPlanButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Plan name";
            // 
            // planNameTextBox
            // 
            this.planNameTextBox.Location = new System.Drawing.Point(12, 152);
            this.planNameTextBox.Name = "planNameTextBox";
            this.planNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.planNameTextBox.TabIndex = 14;
            this.planNameTextBox.TextChanged += new System.EventHandler(this.planNameTextBox_TextChanged);
            // 
            // startMonthNumericUpDown
            // 
            this.startMonthNumericUpDown.Location = new System.Drawing.Point(13, 218);
            this.startMonthNumericUpDown.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.startMonthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.startMonthNumericUpDown.Name = "startMonthNumericUpDown";
            this.startMonthNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.startMonthNumericUpDown.TabIndex = 16;
            this.startMonthNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // expensesNumericUpDown
            // 
            this.expensesNumericUpDown.Location = new System.Drawing.Point(93, 287);
            this.expensesNumericUpDown.Name = "expensesNumericUpDown";
            this.expensesNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.expensesNumericUpDown.TabIndex = 17;
            this.expensesNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.expensesNumericUpDown.ValueChanged += new System.EventHandler(this.expensesNumericUpDown_ValueChanged);
            // 
            // debtsNumericUpDown
            // 
            this.debtsNumericUpDown.Location = new System.Drawing.Point(93, 326);
            this.debtsNumericUpDown.Name = "debtsNumericUpDown";
            this.debtsNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.debtsNumericUpDown.TabIndex = 18;
            this.debtsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.debtsNumericUpDown.ValueChanged += new System.EventHandler(this.debtsNumericUpDown_ValueChanged);
            // 
            // savingsNumericUpDown
            // 
            this.savingsNumericUpDown.Location = new System.Drawing.Point(93, 370);
            this.savingsNumericUpDown.Name = "savingsNumericUpDown";
            this.savingsNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.savingsNumericUpDown.TabIndex = 19;
            this.savingsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.savingsNumericUpDown.ValueChanged += new System.EventHandler(this.savingsNumericUpDown_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(219, 289);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(219, 326);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(219, 372);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(15, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "%";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 423);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Set alarm";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 500);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Set triggering threshold";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 537);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "Threshold";
            // 
            // alarmCheckBox
            // 
            this.alarmCheckBox.AutoSize = true;
            this.alarmCheckBox.Location = new System.Drawing.Point(13, 456);
            this.alarmCheckBox.Name = "alarmCheckBox";
            this.alarmCheckBox.Size = new System.Drawing.Size(146, 17);
            this.alarmCheckBox.TabIndex = 26;
            this.alarmCheckBox.Text = "Enable budget plan alarm";
            this.alarmCheckBox.UseVisualStyleBackColor = true;
            this.alarmCheckBox.CheckedChanged += new System.EventHandler(this.alarmCheckBox_CheckedChanged);
            // 
            // thresholdNumericUpDown
            // 
            this.thresholdNumericUpDown.Enabled = false;
            this.thresholdNumericUpDown.Location = new System.Drawing.Point(93, 535);
            this.thresholdNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.thresholdNumericUpDown.Name = "thresholdNumericUpDown";
            this.thresholdNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.thresholdNumericUpDown.TabIndex = 27;
            this.thresholdNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(219, 537);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(15, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "%";
            // 
            // BudgetPlanCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 638);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.thresholdNumericUpDown);
            this.Controls.Add(this.alarmCheckBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.savingsNumericUpDown);
            this.Controls.Add(this.debtsNumericUpDown);
            this.Controls.Add(this.expensesNumericUpDown);
            this.Controls.Add(this.startMonthNumericUpDown);
            this.Controls.Add(this.planNameTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.createPlanButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sixMonthsCheckBox);
            this.Controls.Add(this.oneMonthCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BudgetPlanCreator";
            this.Text = "Budget plan creator";
            ((System.ComponentModel.ISupportInitialize)(this.startMonthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.expensesNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.debtsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.savingsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox oneMonthCheckBox;
        private System.Windows.Forms.CheckBox sixMonthsCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button createPlanButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox planNameTextBox;
        private System.Windows.Forms.NumericUpDown startMonthNumericUpDown;
        private System.Windows.Forms.NumericUpDown expensesNumericUpDown;
        private System.Windows.Forms.NumericUpDown debtsNumericUpDown;
        private System.Windows.Forms.NumericUpDown savingsNumericUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox alarmCheckBox;
        private System.Windows.Forms.NumericUpDown thresholdNumericUpDown;
        private System.Windows.Forms.Label label14;
    }
}