namespace BudgetManager.non_mvc {
    partial class DiscardReceivableChangeForm {
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
            this.receivableChangesToDiscardDgv = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pendingChangesInfoLabel = new System.Windows.Forms.Label();
            this.discardChangesButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.receivableChangesToDiscardDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // receivableChangesToDiscardDgv
            // 
            this.receivableChangesToDiscardDgv.AllowUserToAddRows = false;
            this.receivableChangesToDiscardDgv.AllowUserToDeleteRows = false;
            this.receivableChangesToDiscardDgv.AllowUserToResizeColumns = false;
            this.receivableChangesToDiscardDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.receivableChangesToDiscardDgv.Location = new System.Drawing.Point(27, 133);
            this.receivableChangesToDiscardDgv.Name = "receivableChangesToDiscardDgv";
            this.receivableChangesToDiscardDgv.Size = new System.Drawing.Size(787, 285);
            this.receivableChangesToDiscardDgv.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select one or more changes to discard from the below list: ";
            // 
            // pendingChangesInfoLabel
            // 
            this.pendingChangesInfoLabel.AutoSize = true;
            this.pendingChangesInfoLabel.BackColor = System.Drawing.Color.Transparent;
            this.pendingChangesInfoLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.pendingChangesInfoLabel.Location = new System.Drawing.Point(27, 435);
            this.pendingChangesInfoLabel.Name = "pendingChangesInfoLabel";
            this.pendingChangesInfoLabel.Size = new System.Drawing.Size(0, 13);
            this.pendingChangesInfoLabel.TabIndex = 2;
            // 
            // discardChangesButton
            // 
            this.discardChangesButton.Location = new System.Drawing.Point(146, 501);
            this.discardChangesButton.Name = "discardChangesButton";
            this.discardChangesButton.Size = new System.Drawing.Size(97, 23);
            this.discardChangesButton.TabIndex = 3;
            this.discardChangesButton.Text = "Discard changes";
            this.discardChangesButton.UseVisualStyleBackColor = true;
            this.discardChangesButton.Click += new System.EventHandler(this.discardChangesButton_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(374, 501);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AutoEllipsis = true;
            this.cancelButton.Location = new System.Drawing.Point(590, 501);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseMnemonic = false;
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // DiscardReceivableChangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 549);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.discardChangesButton);
            this.Controls.Add(this.pendingChangesInfoLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.receivableChangesToDiscardDgv);
            this.Name = "DiscardReceivableChangeForm";
            this.Text = "Discard receivable changes";
            ((System.ComponentModel.ISupportInitialize)(this.receivableChangesToDiscardDgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView receivableChangesToDiscardDgv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label pendingChangesInfoLabel;
        private System.Windows.Forms.Button discardChangesButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button cancelButton;
    }
}