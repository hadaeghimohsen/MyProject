namespace System.DataAccess.Guis
{
    partial class ExecuteDsnName
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmb_DnsNames = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb_DnsNames);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 75);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Dns Name :";
            // 
            // cmb_DnsNames
            // 
            this.cmb_DnsNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_DnsNames.FormattingEnabled = true;
            this.cmb_DnsNames.Location = new System.Drawing.Point(83, 36);
            this.cmb_DnsNames.Name = "cmb_DnsNames";
            this.cmb_DnsNames.Size = new System.Drawing.Size(163, 21);
            this.cmb_DnsNames.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DNS Name :";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(197, 93);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // ExecuteDnsName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 127);
            this.ControlBox = false;
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(300, 165);
            this.MinimumSize = new System.Drawing.Size(300, 165);
            this.Name = "ExecuteDnsName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExecuteDnsName";
            this.Shown += new System.EventHandler(this.ExecuteDnsName_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.Forms.GroupBox groupBox1;
        private Windows.Forms.ComboBox cmb_DnsNames;
        private Windows.Forms.Label label1;
        private Windows.Forms.Button btn_OK;
    }
}