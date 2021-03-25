﻿namespace BudgetManager.mvc.views {
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBPManagement)).BeginInit();
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
            this.dataGridViewBPManagement.Size = new System.Drawing.Size(409, 168);
            this.dataGridViewBPManagement.TabIndex = 4;
            // 
            // submitButtonBPManagement
            // 
            this.submitButtonBPManagement.Enabled = false;
            this.submitButtonBPManagement.Location = new System.Drawing.Point(29, 416);
            this.submitButtonBPManagement.Name = "submitButtonBPManagement";
            this.submitButtonBPManagement.Size = new System.Drawing.Size(106, 23);
            this.submitButtonBPManagement.TabIndex = 5;
            this.submitButtonBPManagement.Text = "Submit changes";
            this.submitButtonBPManagement.UseVisualStyleBackColor = true;
            // 
            // deleteButtonBPManagement
            // 
            this.deleteButtonBPManagement.Enabled = false;
            this.deleteButtonBPManagement.Location = new System.Drawing.Point(341, 416);
            this.deleteButtonBPManagement.Name = "deleteButtonBPManagement";
            this.deleteButtonBPManagement.Size = new System.Drawing.Size(92, 23);
            this.deleteButtonBPManagement.TabIndex = 6;
            this.deleteButtonBPManagement.Text = "Delete record";
            this.deleteButtonBPManagement.UseVisualStyleBackColor = true;
            // 
            // BudgetPlanManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 517);
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
    }
}