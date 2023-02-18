namespace BudgetManager.non_mvc {
    partial class NetInterestCalculator {
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
            this.label4 = new System.Windows.Forms.Label();
            this.grossInterestTextBox = new System.Windows.Forms.TextBox();
            this.taxPercentageNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.calculateInterestButton = new System.Windows.Forms.Button();
            this.resetFieldsButton = new System.Windows.Forms.Button();
            this.copyToFieldButton = new System.Windows.Forms.Button();
            this.copyToClipboardButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.netInterestTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.taxPercentageNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Net interest calculator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Gross interest";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tax percentage";
            // 
            // grossInterestTextBox
            // 
            this.grossInterestTextBox.Location = new System.Drawing.Point(126, 117);
            this.grossInterestTextBox.Name = "grossInterestTextBox";
            this.grossInterestTextBox.Size = new System.Drawing.Size(119, 20);
            this.grossInterestTextBox.TabIndex = 4;
            this.grossInterestTextBox.Validated += new System.EventHandler(this.grossInterestTextBox_Validated);
            // 
            // taxPercentageNumericUpDown
            // 
            this.taxPercentageNumericUpDown.DecimalPlaces = 2;
            this.taxPercentageNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            131072});
            this.taxPercentageNumericUpDown.Location = new System.Drawing.Point(126, 168);
            this.taxPercentageNumericUpDown.Maximum = new decimal(new int[] {
            9900,
            0,
            0,
            131072});
            this.taxPercentageNumericUpDown.Name = "taxPercentageNumericUpDown";
            this.taxPercentageNumericUpDown.ReadOnly = true;
            this.taxPercentageNumericUpDown.Size = new System.Drawing.Size(69, 20);
            this.taxPercentageNumericUpDown.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "%";
            // 
            // calculateInterestButton
            // 
            this.calculateInterestButton.Enabled = false;
            this.calculateInterestButton.Location = new System.Drawing.Point(28, 279);
            this.calculateInterestButton.Name = "calculateInterestButton";
            this.calculateInterestButton.Size = new System.Drawing.Size(75, 23);
            this.calculateInterestButton.TabIndex = 7;
            this.calculateInterestButton.Text = "Calculate";
            this.calculateInterestButton.UseVisualStyleBackColor = true;
            this.calculateInterestButton.Click += new System.EventHandler(this.calculateInterestButton_Click);
            // 
            // resetFieldsButton
            // 
            this.resetFieldsButton.Location = new System.Drawing.Point(80, 328);
            this.resetFieldsButton.Name = "resetFieldsButton";
            this.resetFieldsButton.Size = new System.Drawing.Size(75, 23);
            this.resetFieldsButton.TabIndex = 8;
            this.resetFieldsButton.Text = "Reset";
            this.resetFieldsButton.UseVisualStyleBackColor = true;
            this.resetFieldsButton.Click += new System.EventHandler(this.resetFieldsButton_Click);
            // 
            // copyToFieldButton
            // 
            this.copyToFieldButton.Enabled = false;
            this.copyToFieldButton.Location = new System.Drawing.Point(142, 279);
            this.copyToFieldButton.Name = "copyToFieldButton";
            this.copyToFieldButton.Size = new System.Drawing.Size(75, 23);
            this.copyToFieldButton.TabIndex = 9;
            this.copyToFieldButton.Text = "Copy to field";
            this.copyToFieldButton.UseVisualStyleBackColor = true;
            this.copyToFieldButton.Click += new System.EventHandler(this.copyToFieldButton_Click);
            // 
            // copyToClipboardButton
            // 
            this.copyToClipboardButton.Enabled = false;
            this.copyToClipboardButton.Location = new System.Drawing.Point(257, 279);
            this.copyToClipboardButton.Name = "copyToClipboardButton";
            this.copyToClipboardButton.Size = new System.Drawing.Size(97, 23);
            this.copyToClipboardButton.TabIndex = 10;
            this.copyToClipboardButton.Text = "Copy to clipboard";
            this.copyToClipboardButton.UseVisualStyleBackColor = true;
            this.copyToClipboardButton.Click += new System.EventHandler(this.copyToClipboardButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(205, 328);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 219);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Net interest";
            // 
            // netInterestTextBox
            // 
            this.netInterestTextBox.Location = new System.Drawing.Point(126, 216);
            this.netInterestTextBox.Name = "netInterestTextBox";
            this.netInterestTextBox.ReadOnly = true;
            this.netInterestTextBox.Size = new System.Drawing.Size(90, 20);
            this.netInterestTextBox.TabIndex = 13;
            this.netInterestTextBox.TextChanged += new System.EventHandler(this.netInterestTextBox_TextChanged);
            // 
            // NetInterestCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(375, 386);
            this.Controls.Add(this.netInterestTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.copyToClipboardButton);
            this.Controls.Add(this.copyToFieldButton);
            this.Controls.Add(this.resetFieldsButton);
            this.Controls.Add(this.calculateInterestButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.taxPercentageNumericUpDown);
            this.Controls.Add(this.grossInterestTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NetInterestCalculator";
            this.Text = "Net interest calculator";
            ((System.ComponentModel.ISupportInitialize)(this.taxPercentageNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox grossInterestTextBox;
        private System.Windows.Forms.NumericUpDown taxPercentageNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button calculateInterestButton;
        private System.Windows.Forms.Button resetFieldsButton;
        private System.Windows.Forms.Button copyToFieldButton;
        private System.Windows.Forms.Button copyToClipboardButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox netInterestTextBox;
    }
}