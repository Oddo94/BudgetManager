namespace BudgetManager.mvc.views {
    partial class ReceivableManagementForm {
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
            this.components = new System.ComponentModel.Container();
            this.receivableManagemenDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.updateReceivableCtxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.markAsPaidItem = new System.Windows.Forms.ToolStripMenuItem();
            this.partialPaymentItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateDetailsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayReceivablesButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.receivablesManagementPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.monthRecordsRadioButton = new System.Windows.Forms.RadioButton();
            this.yearRecordsRadioButton = new System.Windows.Forms.RadioButton();
            this.receivableManagementDgv = new System.Windows.Forms.DataGridView();
            this.saveReceivableChangesButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.pendingChangesLabel = new System.Windows.Forms.Label();
            this.discardChangesButton = new System.Windows.Forms.Button();
            this.updateReceivableCtxMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.receivableManagementDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // receivableManagemenDatePicker
            // 
            this.receivableManagemenDatePicker.CustomFormat = "MM/yyyy";
            this.receivableManagemenDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.receivableManagemenDatePicker.Location = new System.Drawing.Point(16, 112);
            this.receivableManagemenDatePicker.Name = "receivableManagemenDatePicker";
            this.receivableManagemenDatePicker.ShowUpDown = true;
            this.receivableManagemenDatePicker.Size = new System.Drawing.Size(112, 20);
            this.receivableManagemenDatePicker.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please select the timespan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Displaying receivables for...";
            // 
            // updateReceivableCtxMenu
            // 
            this.updateReceivableCtxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markAsPaidItem,
            this.partialPaymentItem,
            this.updateDetailsItem});
            this.updateReceivableCtxMenu.Name = "updateReceivableCtxMenu";
            this.updateReceivableCtxMenu.Size = new System.Drawing.Size(183, 70);
            this.updateReceivableCtxMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.updateReceivableCtxMenu_ItemClicked);
            // 
            // markAsPaidItem
            // 
            this.markAsPaidItem.Name = "markAsPaidItem";
            this.markAsPaidItem.Size = new System.Drawing.Size(182, 22);
            this.markAsPaidItem.Text = "Mark as paid";
            // 
            // partialPaymentItem
            // 
            this.partialPaymentItem.Name = "partialPaymentItem";
            this.partialPaymentItem.Size = new System.Drawing.Size(182, 22);
            this.partialPaymentItem.Text = "Add partial payment";
            // 
            // updateDetailsItem
            // 
            this.updateDetailsItem.Name = "updateDetailsItem";
            this.updateDetailsItem.Size = new System.Drawing.Size(182, 22);
            this.updateDetailsItem.Text = "Update details";
            // 
            // displayReceivablesButton
            // 
            this.displayReceivablesButton.Location = new System.Drawing.Point(16, 153);
            this.displayReceivablesButton.Name = "displayReceivablesButton";
            this.displayReceivablesButton.Size = new System.Drawing.Size(117, 28);
            this.displayReceivablesButton.TabIndex = 4;
            this.displayReceivablesButton.Text = "Display receivables";
            this.displayReceivablesButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.receivablesManagementPanel);
            this.groupBox1.Location = new System.Drawing.Point(16, 491);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(772, 347);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // receivablesManagementPanel
            // 
            this.receivablesManagementPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.receivablesManagementPanel.Location = new System.Drawing.Point(16, 19);
            this.receivablesManagementPanel.Name = "receivablesManagementPanel";
            this.receivablesManagementPanel.Size = new System.Drawing.Size(738, 313);
            this.receivablesManagementPanel.TabIndex = 0;
            // 
            // monthRecordsRadioButton
            // 
            this.monthRecordsRadioButton.AutoSize = true;
            this.monthRecordsRadioButton.Checked = true;
            this.monthRecordsRadioButton.Location = new System.Drawing.Point(15, 47);
            this.monthRecordsRadioButton.Name = "monthRecordsRadioButton";
            this.monthRecordsRadioButton.Size = new System.Drawing.Size(55, 17);
            this.monthRecordsRadioButton.TabIndex = 7;
            this.monthRecordsRadioButton.TabStop = true;
            this.monthRecordsRadioButton.Text = "Month";
            this.monthRecordsRadioButton.UseVisualStyleBackColor = true;
            this.monthRecordsRadioButton.CheckedChanged += new System.EventHandler(this.monthRecordsRadioButton_CheckedChanged);
            // 
            // yearRecordsRadioButton
            // 
            this.yearRecordsRadioButton.AutoSize = true;
            this.yearRecordsRadioButton.Location = new System.Drawing.Point(16, 70);
            this.yearRecordsRadioButton.Name = "yearRecordsRadioButton";
            this.yearRecordsRadioButton.Size = new System.Drawing.Size(47, 17);
            this.yearRecordsRadioButton.TabIndex = 8;
            this.yearRecordsRadioButton.Text = "Year";
            this.yearRecordsRadioButton.UseVisualStyleBackColor = true;
            this.yearRecordsRadioButton.CheckedChanged += new System.EventHandler(this.yearRecordsRadioButton_CheckedChanged);
            // 
            // receivableManagementDgv
            // 
            this.receivableManagementDgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.receivableManagementDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.receivableManagementDgv.Location = new System.Drawing.Point(15, 232);
            this.receivableManagementDgv.Name = "receivableManagementDgv";
            this.receivableManagementDgv.ReadOnly = true;
            this.receivableManagementDgv.Size = new System.Drawing.Size(773, 219);
            this.receivableManagementDgv.TabIndex = 9;
            this.receivableManagementDgv.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.receivableManagementDgv_CellContextMenuStripNeeded);
            this.receivableManagementDgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.receivableManagementDgv_CellFormatting);
            this.receivableManagementDgv.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.receivableManagementDgv_CellMouseDown);
            // 
            // saveReceivableChangesButton
            // 
            this.saveReceivableChangesButton.Enabled = false;
            this.saveReceivableChangesButton.Location = new System.Drawing.Point(16, 860);
            this.saveReceivableChangesButton.Name = "saveReceivableChangesButton";
            this.saveReceivableChangesButton.Size = new System.Drawing.Size(99, 23);
            this.saveReceivableChangesButton.TabIndex = 10;
            this.saveReceivableChangesButton.Text = "Save changes";
            this.saveReceivableChangesButton.UseVisualStyleBackColor = true;
            this.saveReceivableChangesButton.Click += new System.EventHandler(this.saveReceivableChangesButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(690, 860);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(98, 23);
            this.exitButton.TabIndex = 11;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // pendingChangesLabel
            // 
            this.pendingChangesLabel.AutoSize = true;
            this.pendingChangesLabel.BackColor = System.Drawing.SystemColors.Control;
            this.pendingChangesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pendingChangesLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.pendingChangesLabel.Location = new System.Drawing.Point(16, 472);
            this.pendingChangesLabel.Name = "pendingChangesLabel";
            this.pendingChangesLabel.Size = new System.Drawing.Size(0, 13);
            this.pendingChangesLabel.TabIndex = 12;
            this.pendingChangesLabel.Visible = false;
            // 
            // discardChangesButton
            // 
            this.discardChangesButton.Enabled = false;
            this.discardChangesButton.Location = new System.Drawing.Point(342, 860);
            this.discardChangesButton.Name = "discardChangesButton";
            this.discardChangesButton.Size = new System.Drawing.Size(104, 23);
            this.discardChangesButton.TabIndex = 13;
            this.discardChangesButton.Text = "Discard changes";
            this.discardChangesButton.UseVisualStyleBackColor = true;
            this.discardChangesButton.Click += new System.EventHandler(this.discardChangesButton_Click);
            // 
            // ReceivableManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 895);
            this.Controls.Add(this.discardChangesButton);
            this.Controls.Add(this.pendingChangesLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.saveReceivableChangesButton);
            this.Controls.Add(this.receivableManagementDgv);
            this.Controls.Add(this.yearRecordsRadioButton);
            this.Controls.Add(this.monthRecordsRadioButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.displayReceivablesButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.receivableManagemenDatePicker);
            this.MaximizeBox = false;
            this.Name = "ReceivableManagementForm";
            this.Text = "Receivable management";
            this.Load += new System.EventHandler(this.ReceivableManagementForm_Load);
            this.updateReceivableCtxMenu.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.receivableManagementDgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker receivableManagemenDatePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button displayReceivablesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ContextMenuStrip updateReceivableCtxMenu;
        private System.Windows.Forms.ToolStripMenuItem markAsPaidItem;
        private System.Windows.Forms.ToolStripMenuItem partialPaymentItem;
        private System.Windows.Forms.ToolStripMenuItem updateDetailsItem;
        private System.Windows.Forms.RadioButton monthRecordsRadioButton;
        private System.Windows.Forms.RadioButton yearRecordsRadioButton;
        private System.Windows.Forms.DataGridView receivableManagementDgv;
        private System.Windows.Forms.FlowLayoutPanel receivablesManagementPanel;
        private System.Windows.Forms.Button saveReceivableChangesButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label pendingChangesLabel;
        private System.Windows.Forms.Button discardChangesButton;
    }
}