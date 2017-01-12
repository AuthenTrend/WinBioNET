namespace WindowsFormsTest
{
    partial class WinBioForm
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
            this.buttonIdentify = new System.Windows.Forms.Button();
            this.buttonLocateSensor = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonEnroll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboUnitId = new System.Windows.Forms.ComboBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.groupFP = new System.Windows.Forms.GroupBox();
            this.comboBoxFp = new System.Windows.Forms.ComboBox();
            this.groupFP.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonIdentify
            // 
            this.buttonIdentify.Location = new System.Drawing.Point(205, 12);
            this.buttonIdentify.Name = "buttonIdentify";
            this.buttonIdentify.Size = new System.Drawing.Size(51, 23);
            this.buttonIdentify.TabIndex = 0;
            this.buttonIdentify.Text = "Identify";
            this.buttonIdentify.UseVisualStyleBackColor = true;
            this.buttonIdentify.Click += new System.EventHandler(this.buttonIdentify_Click);
            // 
            // buttonLocateSensor
            // 
            this.buttonLocateSensor.Location = new System.Drawing.Point(117, 12);
            this.buttonLocateSensor.Name = "buttonLocateSensor";
            this.buttonLocateSensor.Size = new System.Drawing.Size(82, 23);
            this.buttonLocateSensor.TabIndex = 1;
            this.buttonLocateSensor.Text = "LocateSensor";
            this.buttonLocateSensor.UseVisualStyleBackColor = true;
            this.buttonLocateSensor.Click += new System.EventHandler(this.buttonLocateSensor_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(12, 44);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(576, 280);
            this.richTextBox.TabIndex = 2;
            this.richTextBox.Text = "";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(262, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(51, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonEnroll
            // 
            this.buttonEnroll.Location = new System.Drawing.Point(112, 14);
            this.buttonEnroll.Name = "buttonEnroll";
            this.buttonEnroll.Size = new System.Drawing.Size(75, 23);
            this.buttonEnroll.TabIndex = 4;
            this.buttonEnroll.Text = "Enroll";
            this.buttonEnroll.UseVisualStyleBackColor = true;
            this.buttonEnroll.Click += new System.EventHandler(this.buttonEnroll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Unit Id";
            // 
            // comboUnitId
            // 
            this.comboUnitId.FormattingEnabled = true;
            this.comboUnitId.Location = new System.Drawing.Point(53, 12);
            this.comboUnitId.Name = "comboUnitId";
            this.comboUnitId.Size = new System.Drawing.Size(58, 21);
            this.comboUnitId.TabIndex = 6;
            this.comboUnitId.SelectedIndexChanged += new System.EventHandler(this.comboUnitId_SelectionChanged);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(193, 14);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // groupFP
            // 
            this.groupFP.Controls.Add(this.comboBoxFp);
            this.groupFP.Controls.Add(this.buttonEnroll);
            this.groupFP.Controls.Add(this.buttonDelete);
            this.groupFP.Location = new System.Drawing.Point(314, -2);
            this.groupFP.Name = "groupFP";
            this.groupFP.Size = new System.Drawing.Size(274, 40);
            this.groupFP.TabIndex = 7;
            this.groupFP.TabStop = false;
            this.groupFP.Text = "FP";
            // 
            // comboBoxFp
            // 
            this.comboBoxFp.FormattingEnabled = true;
            this.comboBoxFp.Location = new System.Drawing.Point(5, 16);
            this.comboBoxFp.Name = "comboBoxFp";
            this.comboBoxFp.Size = new System.Drawing.Size(101, 21);
            this.comboBoxFp.TabIndex = 5;
            // 
            // WinBioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 336);
            this.Controls.Add(this.groupFP);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.comboUnitId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.buttonLocateSensor);
            this.Controls.Add(this.buttonIdentify);
            this.Name = "WinBioForm";
            this.Text = "WinBioForm";
            this.groupFP.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonIdentify;
        private System.Windows.Forms.Button buttonLocateSensor;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonEnroll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboUnitId;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.GroupBox groupFP;
        private System.Windows.Forms.ComboBox comboBoxFp;
    }
}

