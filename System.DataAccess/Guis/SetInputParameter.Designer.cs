namespace System.DataAccess.Guis
{
    partial class SetInputParameter
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
            this.cb_InputParamType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_SetInputParam = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_InputParamValue = new System.Windows.Forms.RichTextBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_InputParamType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_SetInputParam);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Parameter:";
            // 
            // cb_InputParamType
            // 
            this.cb_InputParamType.FormattingEnabled = true;
            this.cb_InputParamType.Items.AddRange(new object[] {
            "xml",
            "sqlquery"});
            this.cb_InputParamType.Location = new System.Drawing.Point(56, 55);
            this.cb_InputParamType.Name = "cb_InputParamType";
            this.cb_InputParamType.Size = new System.Drawing.Size(129, 21);
            this.cb_InputParamType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type: ";
            // 
            // cb_SetInputParam
            // 
            this.cb_SetInputParam.AutoSize = true;
            this.cb_SetInputParam.Location = new System.Drawing.Point(16, 29);
            this.cb_SetInputParam.Name = "cb_SetInputParam";
            this.cb_SetInputParam.Size = new System.Drawing.Size(118, 17);
            this.cb_SetInputParam.TabIndex = 0;
            this.cb_SetInputParam.Text = "Set input parameter";
            this.cb_SetInputParam.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_InputParamValue);
            this.groupBox2.Location = new System.Drawing.Point(12, 106);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 291);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input Parameter Value: ";
            // 
            // txt_InputParamValue
            // 
            this.txt_InputParamValue.Location = new System.Drawing.Point(16, 31);
            this.txt_InputParamValue.Name = "txt_InputParamValue";
            this.txt_InputParamValue.Size = new System.Drawing.Size(312, 244);
            this.txt_InputParamValue.TabIndex = 0;
            this.txt_InputParamValue.Text = "";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(281, 403);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // SetInputParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 432);
            this.ControlBox = false;
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(387, 470);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(387, 470);
            this.Name = "SetInputParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Input Parameter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.Forms.GroupBox groupBox1;
        private Windows.Forms.ComboBox cb_InputParamType;
        private Windows.Forms.Label label1;
        private Windows.Forms.CheckBox cb_SetInputParam;
        private Windows.Forms.GroupBox groupBox2;
        private Windows.Forms.RichTextBox txt_InputParamValue;
        private Windows.Forms.Button btn_OK;
    }
}