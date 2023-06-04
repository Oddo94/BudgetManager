namespace BudgetManager.mvp.views
{
    partial class ExternalAccountStatisticsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.userAccountsComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.accountCreationDateValueLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource7 = new System.Windows.Forms.BindingSource(this.components);
            this.accountCreationDateLabel = new System.Windows.Forms.Label();
            this.bankNameValueLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource6 = new System.Windows.Forms.BindingSource(this.components);
            this.accountNameValueLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource5 = new System.Windows.Forms.BindingSource(this.components);
            this.bankNameLabel = new System.Windows.Forms.Label();
            this.accountNameLabel = new System.Windows.Forms.Label();
            this.totalInterestAmountValueLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource4 = new System.Windows.Forms.BindingSource(this.components);
            this.totalOutTransfersValueLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.totalInTransfersValueLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.totalInterestAmountLabel = new System.Windows.Forms.Label();
            this.totaloutTransfersLabel = new System.Windows.Forms.Label();
            this.totalInTransfersLabel = new System.Windows.Forms.Label();
            this.accountBalanceValueLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountBalanceLabel = new System.Windows.Forms.Label();
            this.externalAccountDetailsModelBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.displayAccountStatisticsButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource3)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select account";
            // 
            // userAccountsComboBox
            // 
            this.userAccountsComboBox.FormattingEnabled = true;
            this.userAccountsComboBox.Location = new System.Drawing.Point(25, 79);
            this.userAccountsComboBox.Name = "userAccountsComboBox";
            this.userAccountsComboBox.Size = new System.Drawing.Size(170, 21);
            this.userAccountsComboBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.accountCreationDateValueLabel);
            this.groupBox1.Controls.Add(this.accountCreationDateLabel);
            this.groupBox1.Controls.Add(this.bankNameValueLabel);
            this.groupBox1.Controls.Add(this.accountNameValueLabel);
            this.groupBox1.Controls.Add(this.bankNameLabel);
            this.groupBox1.Controls.Add(this.accountNameLabel);
            this.groupBox1.Controls.Add(this.totalInterestAmountValueLabel);
            this.groupBox1.Controls.Add(this.totalOutTransfersValueLabel);
            this.groupBox1.Controls.Add(this.totalInTransfersValueLabel);
            this.groupBox1.Controls.Add(this.totalInterestAmountLabel);
            this.groupBox1.Controls.Add(this.totaloutTransfersLabel);
            this.groupBox1.Controls.Add(this.totalInTransfersLabel);
            this.groupBox1.Controls.Add(this.accountBalanceValueLabel);
            this.groupBox1.Controls.Add(this.accountBalanceLabel);
            this.groupBox1.Location = new System.Drawing.Point(25, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 206);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // accountCreationDateValueLabel
            // 
            this.accountCreationDateValueLabel.AutoSize = true;
            this.accountCreationDateValueLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.externalAccountDetailsModelBindingSource7, "CreationDate", true));
            this.accountCreationDateValueLabel.Location = new System.Drawing.Point(79, 91);
            this.accountCreationDateValueLabel.Name = "accountCreationDateValueLabel";
            this.accountCreationDateValueLabel.Size = new System.Drawing.Size(61, 13);
            this.accountCreationDateValueLabel.TabIndex = 19;
            this.accountCreationDateValueLabel.Text = "2023-05-24";
            // 
            // externalAccountDetailsModelBindingSource7
            // 
            this.externalAccountDetailsModelBindingSource7.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            // 
            // accountCreationDateLabel
            // 
            this.accountCreationDateLabel.AutoSize = true;
            this.accountCreationDateLabel.Location = new System.Drawing.Point(6, 91);
            this.accountCreationDateLabel.Name = "accountCreationDateLabel";
            this.accountCreationDateLabel.Size = new System.Drawing.Size(70, 13);
            this.accountCreationDateLabel.TabIndex = 18;
            this.accountCreationDateLabel.Text = "Creation date";
            // 
            // bankNameValueLabel
            // 
            this.bankNameValueLabel.AutoSize = true;
            this.bankNameValueLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.externalAccountDetailsModelBindingSource6, "BankName", true));
            this.bankNameValueLabel.Location = new System.Drawing.Point(76, 63);
            this.bankNameValueLabel.Name = "bankNameValueLabel";
            this.bankNameValueLabel.Size = new System.Drawing.Size(98, 13);
            this.bankNameValueLabel.TabIndex = 17;
            this.bankNameValueLabel.Text = "Banca Transilvania";
            // 
            // externalAccountDetailsModelBindingSource6
            // 
            this.externalAccountDetailsModelBindingSource6.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            // 
            // accountNameValueLabel
            // 
            this.accountNameValueLabel.AutoSize = true;
            this.accountNameValueLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.externalAccountDetailsModelBindingSource5, "AccountName", true));
            this.accountNameValueLabel.Location = new System.Drawing.Point(82, 28);
            this.accountNameValueLabel.Name = "accountNameValueLabel";
            this.accountNameValueLabel.Size = new System.Drawing.Size(70, 13);
            this.accountNameValueLabel.TabIndex = 16;
            this.accountNameValueLabel.Text = "Test account";
            // 
            // externalAccountDetailsModelBindingSource5
            // 
            this.externalAccountDetailsModelBindingSource5.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            // 
            // bankNameLabel
            // 
            this.bankNameLabel.AutoSize = true;
            this.bankNameLabel.Location = new System.Drawing.Point(6, 63);
            this.bankNameLabel.Name = "bankNameLabel";
            this.bankNameLabel.Size = new System.Drawing.Size(64, 13);
            this.bankNameLabel.TabIndex = 15;
            this.bankNameLabel.Text = "Bank name:";
            // 
            // accountNameLabel
            // 
            this.accountNameLabel.AutoSize = true;
            this.accountNameLabel.Location = new System.Drawing.Point(6, 28);
            this.accountNameLabel.Name = "accountNameLabel";
            this.accountNameLabel.Size = new System.Drawing.Size(79, 13);
            this.accountNameLabel.TabIndex = 14;
            this.accountNameLabel.Text = "Account name:";
            // 
            // totalInterestAmountValueLabel
            // 
            this.totalInterestAmountValueLabel.AutoSize = true;
            this.totalInterestAmountValueLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.externalAccountDetailsModelBindingSource4, "TotalInterestAmount", true));
            this.totalInterestAmountValueLabel.Location = new System.Drawing.Point(410, 91);
            this.totalInterestAmountValueLabel.Name = "totalInterestAmountValueLabel";
            this.totalInterestAmountValueLabel.Size = new System.Drawing.Size(31, 13);
            this.totalInterestAmountValueLabel.TabIndex = 11;
            this.totalInterestAmountValueLabel.Text = "5514";
            // 
            // externalAccountDetailsModelBindingSource4
            // 
            this.externalAccountDetailsModelBindingSource4.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            // 
            // totalOutTransfersValueLabel
            // 
            this.totalOutTransfersValueLabel.AutoSize = true;
            this.totalOutTransfersValueLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.externalAccountDetailsModelBindingSource2, "TotalOutTransfers", true));
            this.totalOutTransfersValueLabel.Location = new System.Drawing.Point(395, 63);
            this.totalOutTransfersValueLabel.Name = "totalOutTransfersValueLabel";
            this.totalOutTransfersValueLabel.Size = new System.Drawing.Size(31, 13);
            this.totalOutTransfersValueLabel.TabIndex = 10;
            this.totalOutTransfersValueLabel.Text = "3000";
            // 
            // externalAccountDetailsModelBindingSource2
            // 
            this.externalAccountDetailsModelBindingSource2.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            // 
            // totalInTransfersValueLabel
            // 
            this.totalInTransfersValueLabel.AutoSize = true;
            this.totalInTransfersValueLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.externalAccountDetailsModelBindingSource1, "TotalInTransfers", true));
            this.totalInTransfersValueLabel.Location = new System.Drawing.Point(395, 28);
            this.totalInTransfersValueLabel.Name = "totalInTransfersValueLabel";
            this.totalInTransfersValueLabel.Size = new System.Drawing.Size(31, 13);
            this.totalInTransfersValueLabel.TabIndex = 9;
            this.totalInTransfersValueLabel.Text = "8000";
            // 
            // externalAccountDetailsModelBindingSource1
            // 
            this.externalAccountDetailsModelBindingSource1.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            // 
            // totalInterestAmountLabel
            // 
            this.totalInterestAmountLabel.AutoSize = true;
            this.totalInterestAmountLabel.Location = new System.Drawing.Point(295, 91);
            this.totalInterestAmountLabel.Name = "totalInterestAmountLabel";
            this.totalInterestAmountLabel.Size = new System.Drawing.Size(109, 13);
            this.totalInterestAmountLabel.TabIndex = 7;
            this.totalInterestAmountLabel.Text = "Total interest amount:";
            // 
            // totaloutTransfersLabel
            // 
            this.totaloutTransfersLabel.AutoSize = true;
            this.totaloutTransfersLabel.Location = new System.Drawing.Point(295, 63);
            this.totaloutTransfersLabel.Name = "totaloutTransfersLabel";
            this.totaloutTransfersLabel.Size = new System.Drawing.Size(103, 13);
            this.totaloutTransfersLabel.TabIndex = 5;
            this.totaloutTransfersLabel.Text = "Total OUT transfers:";
            // 
            // totalInTransfersLabel
            // 
            this.totalInTransfersLabel.AutoSize = true;
            this.totalInTransfersLabel.Location = new System.Drawing.Point(298, 28);
            this.totalInTransfersLabel.Name = "totalInTransfersLabel";
            this.totalInTransfersLabel.Size = new System.Drawing.Size(91, 13);
            this.totalInTransfersLabel.TabIndex = 2;
            this.totalInTransfersLabel.Text = "Total IN transfers:";
            // 
            // accountBalanceValueLabel
            // 
            this.accountBalanceValueLabel.AutoSize = true;
            this.accountBalanceValueLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.externalAccountDetailsModelBindingSource, "AccountBalance", true));
            this.accountBalanceValueLabel.Location = new System.Drawing.Point(94, 125);
            this.accountBalanceValueLabel.Name = "accountBalanceValueLabel";
            this.accountBalanceValueLabel.Size = new System.Drawing.Size(37, 13);
            this.accountBalanceValueLabel.TabIndex = 1;
            this.accountBalanceValueLabel.Text = "16514";
            // 
            // externalAccountDetailsModelBindingSource
            // 
            this.externalAccountDetailsModelBindingSource.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            //this.externalAccountDetailsModelBindingSource.CurrentChanged += new System.EventHandler(this.externalAccountDetailsModelBindingSource_CurrentChanged);
            // 
            // accountBalanceLabel
            // 
            this.accountBalanceLabel.AutoSize = true;
            this.accountBalanceLabel.Location = new System.Drawing.Point(6, 125);
            this.accountBalanceLabel.Name = "accountBalanceLabel";
            this.accountBalanceLabel.Size = new System.Drawing.Size(91, 13);
            this.accountBalanceLabel.TabIndex = 0;
            this.accountBalanceLabel.Text = "Account balance:";
            // 
            // externalAccountDetailsModelBindingSource3
            // 
            this.externalAccountDetailsModelBindingSource3.DataSource = typeof(BudgetManager.mvp.models.ExternalAccountDetailsModel);
            // 
            // displayAccountStatisticsButton
            // 
            this.displayAccountStatisticsButton.Location = new System.Drawing.Point(291, 77);
            this.displayAccountStatisticsButton.Name = "displayAccountStatisticsButton";
            this.displayAccountStatisticsButton.Size = new System.Drawing.Size(75, 23);
            this.displayAccountStatisticsButton.TabIndex = 3;
            this.displayAccountStatisticsButton.Text = "Test binding";
            this.displayAccountStatisticsButton.UseVisualStyleBackColor = true;
            // 
            // ExternalAccountStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.displayAccountStatisticsButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.userAccountsComboBox);
            this.Controls.Add(this.label1);
            this.Name = "ExternalAccountStatisticsForm";
            this.Text = "External account statistics";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAccountDetailsModelBindingSource3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox userAccountsComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label totalInTransfersLabel;
        private System.Windows.Forms.Label accountBalanceValueLabel;
        private System.Windows.Forms.Label accountBalanceLabel;
        private System.Windows.Forms.Label totaloutTransfersLabel;
        private System.Windows.Forms.Label totalInterestAmountLabel;
        private System.Windows.Forms.Label totalInterestAmountValueLabel;
        private System.Windows.Forms.Label totalOutTransfersValueLabel;
        private System.Windows.Forms.Label totalInTransfersValueLabel;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource2;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource1;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource3;
        private System.Windows.Forms.Label bankNameLabel;
        private System.Windows.Forms.Label accountNameLabel;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource4;
        private System.Windows.Forms.Label accountCreationDateValueLabel;
        private System.Windows.Forms.Label accountCreationDateLabel;
        private System.Windows.Forms.Label bankNameValueLabel;
        private System.Windows.Forms.Label accountNameValueLabel;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource6;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource5;
        private System.Windows.Forms.BindingSource externalAccountDetailsModelBindingSource7;
        private System.Windows.Forms.Button displayAccountStatisticsButton;
    }
}