namespace BudgetManager.non_mvc {
    partial class BudgetPlanManagementCalculator {
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
            this.inputPercentageOrValueLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.percentSymbolLabel2 = new System.Windows.Forms.Label();
            this.calculationModeComboBox = new System.Windows.Forms.ComboBox();
            this.inputValueOrPercentageTextBox = new System.Windows.Forms.TextBox();
            this.totalIncomesTextBox = new System.Windows.Forms.TextBox();
            this.resultTextBox = new System.Windows.Forms.TextBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.calculateButton = new System.Windows.Forms.Button();
            this.percentSymbolLabel1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Budget plan management calculator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Calculation mode";
            // 
            // inputPercentageOrValueLabel
            // 
            this.inputPercentageOrValueLabel.AutoSize = true;
            this.inputPercentageOrValueLabel.Location = new System.Drawing.Point(22, 189);
            this.inputPercentageOrValueLabel.Name = "inputPercentageOrValueLabel";
            this.inputPercentageOrValueLabel.Size = new System.Drawing.Size(88, 13);
            this.inputPercentageOrValueLabel.TabIndex = 2;
            this.inputPercentageOrValueLabel.Text = "Input percentage";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Total incomes";
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(22, 300);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(66, 13);
            this.resultLabel.TabIndex = 5;
            this.resultLabel.Text = "Result value";
            // 
            // percentSymbolLabel2
            // 
            this.percentSymbolLabel2.AutoSize = true;
            this.percentSymbolLabel2.Location = new System.Drawing.Point(238, 300);
            this.percentSymbolLabel2.Name = "percentSymbolLabel2";
            this.percentSymbolLabel2.Size = new System.Drawing.Size(15, 13);
            this.percentSymbolLabel2.TabIndex = 6;
            this.percentSymbolLabel2.Text = "%";
            this.percentSymbolLabel2.Visible = false;
            // 
            // calculationModeComboBox
            // 
            this.calculationModeComboBox.FormattingEnabled = true;
            this.calculationModeComboBox.Items.AddRange(new object[] {
            "---Select---",
            "Input percentage to value",
            "Input value to percentage"});
            this.calculationModeComboBox.Location = new System.Drawing.Point(25, 121);
            this.calculationModeComboBox.Name = "calculationModeComboBox";
            this.calculationModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.calculationModeComboBox.TabIndex = 7;
            this.calculationModeComboBox.SelectedIndexChanged += new System.EventHandler(this.calculationModeComboBox_SelectedIndexChanged);
            // 
            // inputValueOrPercentageTextBox
            // 
            this.inputValueOrPercentageTextBox.Location = new System.Drawing.Point(132, 189);
            this.inputValueOrPercentageTextBox.Name = "inputValueOrPercentageTextBox";
            this.inputValueOrPercentageTextBox.Size = new System.Drawing.Size(100, 20);
            this.inputValueOrPercentageTextBox.TabIndex = 8;
            this.inputValueOrPercentageTextBox.TextChanged += new System.EventHandler(this.inputValueOrPercentageTextBox_TextChanged);
            // 
            // totalIncomesTextBox
            // 
            this.totalIncomesTextBox.Location = new System.Drawing.Point(132, 243);
            this.totalIncomesTextBox.Name = "totalIncomesTextBox";
            this.totalIncomesTextBox.Size = new System.Drawing.Size(100, 20);
            this.totalIncomesTextBox.TabIndex = 9;
            this.totalIncomesTextBox.TextChanged += new System.EventHandler(this.totalIncomesTextBox_TextChanged);
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(132, 293);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.ReadOnly = true;
            this.resultTextBox.Size = new System.Drawing.Size(100, 20);
            this.resultTextBox.TabIndex = 11;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(25, 385);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 12;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // calculateButton
            // 
            this.calculateButton.Enabled = false;
            this.calculateButton.Location = new System.Drawing.Point(221, 385);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(75, 23);
            this.calculateButton.TabIndex = 13;
            this.calculateButton.Text = "Calculate";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // percentSymbolLabel1
            // 
            this.percentSymbolLabel1.AutoSize = true;
            this.percentSymbolLabel1.Location = new System.Drawing.Point(238, 192);
            this.percentSymbolLabel1.Name = "percentSymbolLabel1";
            this.percentSymbolLabel1.Size = new System.Drawing.Size(15, 13);
            this.percentSymbolLabel1.TabIndex = 14;
            this.percentSymbolLabel1.Text = "%";
            // 
            // BudgetPlanManagementCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 451);
            this.Controls.Add(this.percentSymbolLabel1);
            this.Controls.Add(this.calculateButton);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.totalIncomesTextBox);
            this.Controls.Add(this.inputValueOrPercentageTextBox);
            this.Controls.Add(this.calculationModeComboBox);
            this.Controls.Add(this.percentSymbolLabel2);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inputPercentageOrValueLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "BudgetPlanManagementCalculator";
            this.Text = "Budget plan management calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label inputPercentageOrValueLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Label percentSymbolLabel2;
        private System.Windows.Forms.ComboBox calculationModeComboBox;
        private System.Windows.Forms.TextBox inputValueOrPercentageTextBox;
        private System.Windows.Forms.TextBox totalIncomesTextBox;
        private System.Windows.Forms.TextBox resultTextBox;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.Label percentSymbolLabel1;
    }
}