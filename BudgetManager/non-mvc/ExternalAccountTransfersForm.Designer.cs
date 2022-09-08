namespace BudgetManager.non_mvc {
    partial class ExternalAccountTransfersForm {
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
            this.sourceAccountComboBox = new System.Windows.Forms.ComboBox();
            this.destinationAccountComboBox = new System.Windows.Forms.ComboBox();
            this.amountTransferredTextBox = new System.Windows.Forms.TextBox();
            this.exchangeRateTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.transferDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.transferObservationsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.transferButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.charactersLeftLabel = new System.Windows.Forms.Label();
            this.invalidExchangeRateFormatLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.transferNameTextBox = new System.Windows.Forms.TextBox();
            this.previewTransferButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.transactionIDTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fill in the transfer details";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Source account";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Destination account";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Amount transferred ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 298);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 4;
            // 
            // sourceAccountComboBox
            // 
            this.sourceAccountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceAccountComboBox.FormattingEnabled = true;
            this.sourceAccountComboBox.Location = new System.Drawing.Point(128, 161);
            this.sourceAccountComboBox.Name = "sourceAccountComboBox";
            this.sourceAccountComboBox.Size = new System.Drawing.Size(190, 21);
            this.sourceAccountComboBox.TabIndex = 5;
            this.sourceAccountComboBox.MouseHover += new System.EventHandler(this.sourceAccountComboBox_MouseHover);
            // 
            // destinationAccountComboBox
            // 
            this.destinationAccountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationAccountComboBox.FormattingEnabled = true;
            this.destinationAccountComboBox.Location = new System.Drawing.Point(127, 218);
            this.destinationAccountComboBox.Name = "destinationAccountComboBox";
            this.destinationAccountComboBox.Size = new System.Drawing.Size(190, 21);
            this.destinationAccountComboBox.TabIndex = 6;
            this.destinationAccountComboBox.MouseHover += new System.EventHandler(this.destinationAccountComboBox_MouseHover);
            // 
            // amountTransferredTextBox
            // 
            this.amountTransferredTextBox.Location = new System.Drawing.Point(127, 273);
            this.amountTransferredTextBox.Name = "amountTransferredTextBox";
            this.amountTransferredTextBox.Size = new System.Drawing.Size(191, 20);
            this.amountTransferredTextBox.TabIndex = 7;
            this.amountTransferredTextBox.TextChanged += new System.EventHandler(this.amountTransferredTextBox_TextChanged);
            // 
            // exchangeRateTextBox
            // 
            this.exchangeRateTextBox.Location = new System.Drawing.Point(127, 324);
            this.exchangeRateTextBox.Name = "exchangeRateTextBox";
            this.exchangeRateTextBox.Size = new System.Drawing.Size(190, 20);
            this.exchangeRateTextBox.TabIndex = 8;
            this.exchangeRateTextBox.TextChanged += new System.EventHandler(this.exchangeRateTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 327);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Exchange rate";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 428);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Transfer observations";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Transfer date";
            // 
            // transferDateTimePicker
            // 
            this.transferDateTimePicker.Location = new System.Drawing.Point(127, 59);
            this.transferDateTimePicker.Name = "transferDateTimePicker";
            this.transferDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.transferDateTimePicker.TabIndex = 12;
            // 
            // transferObservationsRichTextBox
            // 
            this.transferObservationsRichTextBox.Location = new System.Drawing.Point(128, 428);
            this.transferObservationsRichTextBox.MaxLength = 150;
            this.transferObservationsRichTextBox.Name = "transferObservationsRichTextBox";
            this.transferObservationsRichTextBox.Size = new System.Drawing.Size(358, 86);
            this.transferObservationsRichTextBox.TabIndex = 13;
            this.transferObservationsRichTextBox.Text = "";
            this.transferObservationsRichTextBox.TextChanged += new System.EventHandler(this.transferObservationsRichTextBox_TextChanged);
            this.transferObservationsRichTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.transferObservationsRichTextBox_KeyPress);
            // 
            // transferButton
            // 
            this.transferButton.Location = new System.Drawing.Point(37, 559);
            this.transferButton.Name = "transferButton";
            this.transferButton.Size = new System.Drawing.Size(95, 23);
            this.transferButton.TabIndex = 14;
            this.transferButton.Text = "Transfer money";
            this.transferButton.UseVisualStyleBackColor = true;
            this.transferButton.Click += new System.EventHandler(this.transferButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(391, 559);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(82, 23);
            this.resetButton.TabIndex = 15;
            this.resetButton.Text = "Reset fields";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(556, 559);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(83, 23);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // charactersLeftLabel
            // 
            this.charactersLeftLabel.AutoSize = true;
            this.charactersLeftLabel.Location = new System.Drawing.Point(125, 517);
            this.charactersLeftLabel.Name = "charactersLeftLabel";
            this.charactersLeftLabel.Size = new System.Drawing.Size(144, 13);
            this.charactersLeftLabel.TabIndex = 17;
            this.charactersLeftLabel.Text = "You have 100 characters left";
            // 
            // invalidExchangeRateFormatLabel
            // 
            this.invalidExchangeRateFormatLabel.AutoSize = true;
            this.invalidExchangeRateFormatLabel.ForeColor = System.Drawing.Color.Red;
            this.invalidExchangeRateFormatLabel.Location = new System.Drawing.Point(255, 297);
            this.invalidExchangeRateFormatLabel.Name = "invalidExchangeRateFormatLabel";
            this.invalidExchangeRateFormatLabel.Size = new System.Drawing.Size(0, 13);
            this.invalidExchangeRateFormatLabel.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Transfer name";
            // 
            // transferNameTextBox
            // 
            this.transferNameTextBox.Location = new System.Drawing.Point(128, 110);
            this.transferNameTextBox.Name = "transferNameTextBox";
            this.transferNameTextBox.Size = new System.Drawing.Size(190, 20);
            this.transferNameTextBox.TabIndex = 20;
            // 
            // previewTransferButton
            // 
            this.previewTransferButton.Location = new System.Drawing.Point(217, 559);
            this.previewTransferButton.Name = "previewTransferButton";
            this.previewTransferButton.Size = new System.Drawing.Size(100, 23);
            this.previewTransferButton.TabIndex = 21;
            this.previewTransferButton.Text = "Preview transfer";
            this.previewTransferButton.UseVisualStyleBackColor = true;
            this.previewTransferButton.Click += new System.EventHandler(this.previewTransferButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 381);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Transaction ID";
            // 
            // transactionIDTextBox
            // 
            this.transactionIDTextBox.Location = new System.Drawing.Point(128, 374);
            this.transactionIDTextBox.MaxLength = 50;
            this.transactionIDTextBox.Name = "transactionIDTextBox";
            this.transactionIDTextBox.Size = new System.Drawing.Size(189, 20);
            this.transactionIDTextBox.TabIndex = 23;
            // 
            // ExternalAccountTransfersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 626);
            this.Controls.Add(this.transactionIDTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.previewTransferButton);
            this.Controls.Add(this.transferNameTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.invalidExchangeRateFormatLabel);
            this.Controls.Add(this.charactersLeftLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.transferButton);
            this.Controls.Add(this.transferObservationsRichTextBox);
            this.Controls.Add(this.transferDateTimePicker);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.exchangeRateTextBox);
            this.Controls.Add(this.amountTransferredTextBox);
            this.Controls.Add(this.destinationAccountComboBox);
            this.Controls.Add(this.sourceAccountComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ExternalAccountTransfersForm";
            this.Text = "External account transfers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox sourceAccountComboBox;
        private System.Windows.Forms.ComboBox destinationAccountComboBox;
        private System.Windows.Forms.TextBox amountTransferredTextBox;
        private System.Windows.Forms.TextBox exchangeRateTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker transferDateTimePicker;
        private System.Windows.Forms.RichTextBox transferObservationsRichTextBox;
        private System.Windows.Forms.Button transferButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label charactersLeftLabel;
        private System.Windows.Forms.Label invalidExchangeRateFormatLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox transferNameTextBox;
        private System.Windows.Forms.Button previewTransferButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox transactionIDTextBox;
    }
}