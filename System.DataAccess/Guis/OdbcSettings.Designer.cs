namespace System.DataAccess.Guis
{
    partial class OdbcSettings
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
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_TestConnection = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Build = new System.Windows.Forms.Button();
            this.txt_dsnconnectionstring = new System.Windows.Forms.TextBox();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.cmb_dsnlist = new System.Windows.Forms.ComboBox();
            this.rb_UseConnectionString = new System.Windows.Forms.RadioButton();
            this.rb_UseReadyDataSource = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(283, 359);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 9;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(364, 359);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 10;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_TestConnection
            // 
            this.btn_TestConnection.Location = new System.Drawing.Point(27, 359);
            this.btn_TestConnection.Name = "btn_TestConnection";
            this.btn_TestConnection.Size = new System.Drawing.Size(141, 23);
            this.btn_TestConnection.TabIndex = 8;
            this.btn_TestConnection.Text = "&Test Connection";
            this.btn_TestConnection.UseVisualStyleBackColor = true;
            this.btn_TestConnection.Click += new System.EventHandler(this.btn_TestConnection_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_password);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txt_username);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(27, 220);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 100);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Login information";
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(92, 58);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(247, 20);
            this.txt_password.TabIndex = 1;
            this.txt_password.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "&Password:";
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(92, 32);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(247, 20);
            this.txt_username.TabIndex = 0;
            this.txt_username.Leave += new System.EventHandler(this.txt_username_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "User &name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Build);
            this.groupBox1.Controls.Add(this.txt_dsnconnectionstring);
            this.groupBox1.Controls.Add(this.btn_Refresh);
            this.groupBox1.Controls.Add(this.cmb_dsnlist);
            this.groupBox1.Controls.Add(this.rb_UseConnectionString);
            this.groupBox1.Controls.Add(this.rb_UseReadyDataSource);
            this.groupBox1.Location = new System.Drawing.Point(27, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 156);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data source specification";
            // 
            // btn_Build
            // 
            this.btn_Build.Location = new System.Drawing.Point(306, 119);
            this.btn_Build.Name = "btn_Build";
            this.btn_Build.Size = new System.Drawing.Size(75, 23);
            this.btn_Build.TabIndex = 5;
            this.btn_Build.Text = "Build...";
            this.btn_Build.UseVisualStyleBackColor = true;
            this.btn_Build.Click += new System.EventHandler(this.btn_Build_Click);
            // 
            // txt_dsnconnectionstring
            // 
            this.txt_dsnconnectionstring.Location = new System.Drawing.Point(53, 121);
            this.txt_dsnconnectionstring.Name = "txt_dsnconnectionstring";
            this.txt_dsnconnectionstring.Size = new System.Drawing.Size(247, 20);
            this.txt_dsnconnectionstring.TabIndex = 4;
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Enabled = false;
            this.btn_Refresh.Location = new System.Drawing.Point(306, 62);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_Refresh.TabIndex = 2;
            this.btn_Refresh.Text = "&Refresh";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // cmb_dsnlist
            // 
            this.cmb_dsnlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_dsnlist.Enabled = false;
            this.cmb_dsnlist.FormattingEnabled = true;
            this.cmb_dsnlist.Location = new System.Drawing.Point(53, 63);
            this.cmb_dsnlist.Name = "cmb_dsnlist";
            this.cmb_dsnlist.Size = new System.Drawing.Size(247, 21);
            this.cmb_dsnlist.TabIndex = 1;
            // 
            // rb_UseConnectionString
            // 
            this.rb_UseConnectionString.AutoSize = true;
            this.rb_UseConnectionString.Checked = true;
            this.rb_UseConnectionString.Location = new System.Drawing.Point(31, 98);
            this.rb_UseConnectionString.Name = "rb_UseConnectionString";
            this.rb_UseConnectionString.Size = new System.Drawing.Size(131, 17);
            this.rb_UseConnectionString.TabIndex = 3;
            this.rb_UseConnectionString.TabStop = true;
            this.rb_UseConnectionString.Tag = "UseConnectionString";
            this.rb_UseConnectionString.Text = "Use c&onnection string:";
            this.rb_UseConnectionString.UseVisualStyleBackColor = true;
            this.rb_UseConnectionString.Click += new System.EventHandler(this.rb_GlobalRadioButton_CheckedChanged);
            // 
            // rb_UseReadyDataSource
            // 
            this.rb_UseReadyDataSource.AutoSize = true;
            this.rb_UseReadyDataSource.Location = new System.Drawing.Point(31, 34);
            this.rb_UseReadyDataSource.Name = "rb_UseReadyDataSource";
            this.rb_UseReadyDataSource.Size = new System.Drawing.Size(205, 17);
            this.rb_UseReadyDataSource.TabIndex = 0;
            this.rb_UseReadyDataSource.Tag = "UseDsn";
            this.rb_UseReadyDataSource.Text = "Use user or system &data source name:";
            this.rb_UseReadyDataSource.UseVisualStyleBackColor = true;
            this.rb_UseReadyDataSource.Click += new System.EventHandler(this.rb_GlobalRadioButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Enter information to connect to the selected data source.";
            // 
            // OdbcSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 400);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_TestConnection);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(479, 438);
            this.MinimumSize = new System.Drawing.Size(479, 438);
            this.Name = "OdbcSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Odbc Settings";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Windows.Forms.Button btn_OK;
        private Windows.Forms.Button btn_Cancel;
        private Windows.Forms.Button btn_TestConnection;
        private Windows.Forms.GroupBox groupBox2;
        private Windows.Forms.TextBox txt_password;
        private Windows.Forms.Label label3;
        private Windows.Forms.TextBox txt_username;
        private Windows.Forms.Label label2;
        private Windows.Forms.GroupBox groupBox1;
        private Windows.Forms.Button btn_Build;
        private Windows.Forms.TextBox txt_dsnconnectionstring;
        private Windows.Forms.Button btn_Refresh;
        private Windows.Forms.ComboBox cmb_dsnlist;
        private Windows.Forms.RadioButton rb_UseConnectionString;
        private Windows.Forms.RadioButton rb_UseReadyDataSource;
        private Windows.Forms.Label label1;
    }
}