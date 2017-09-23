namespace System.DataAccess.Guis
{
    partial class Build
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Build));
            this.btn_Retype = new System.Windows.Forms.Button();
            this.btn_SaveDsn = new System.Windows.Forms.Button();
            this.cb_DriverList = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_TestConnection = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_DsnName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_Desc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_Server = new System.Windows.Forms.TextBox();
            this.cb_TrustedConnection = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Database = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_MachineDataSource = new System.Windows.Forms.TabPage();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.lb_Dsnlist = new System.Windows.Forms.ListBox();
            this.tp_DefineNewDataSource = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tp_MachineDataSource.SuspendLayout();
            this.tp_DefineNewDataSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Retype
            // 
            this.btn_Retype.Location = new System.Drawing.Point(376, 192);
            this.btn_Retype.Name = "btn_Retype";
            this.btn_Retype.Size = new System.Drawing.Size(75, 23);
            this.btn_Retype.TabIndex = 15;
            this.btn_Retype.Text = "Retype";
            this.btn_Retype.UseVisualStyleBackColor = true;
            // 
            // btn_SaveDsn
            // 
            this.btn_SaveDsn.Location = new System.Drawing.Point(295, 192);
            this.btn_SaveDsn.Name = "btn_SaveDsn";
            this.btn_SaveDsn.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveDsn.TabIndex = 14;
            this.btn_SaveDsn.Text = "Save";
            this.btn_SaveDsn.UseVisualStyleBackColor = true;
            this.btn_SaveDsn.Click += new System.EventHandler(this.btn_SaveDsn_Click);
            // 
            // cb_DriverList
            // 
            this.cb_DriverList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_DriverList.FormattingEnabled = true;
            this.cb_DriverList.Location = new System.Drawing.Point(110, 98);
            this.cb_DriverList.Name = "cb_DriverList";
            this.cb_DriverList.Size = new System.Drawing.Size(200, 21);
            this.cb_DriverList.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label11.Location = new System.Drawing.Point(316, 101);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "(optional)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label10.Location = new System.Drawing.Point(316, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "(optional)";
            // 
            // btn_TestConnection
            // 
            this.btn_TestConnection.Location = new System.Drawing.Point(333, 148);
            this.btn_TestConnection.Name = "btn_TestConnection";
            this.btn_TestConnection.Size = new System.Drawing.Size(102, 23);
            this.btn_TestConnection.TabIndex = 17;
            this.btn_TestConnection.Text = "Test Conection";
            this.btn_TestConnection.UseVisualStyleBackColor = true;
            this.btn_TestConnection.Click += new System.EventHandler(this.btn_TestConnection_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Dsn name:";
            // 
            // txt_DsnName
            // 
            this.txt_DsnName.Location = new System.Drawing.Point(110, 20);
            this.txt_DsnName.Name = "txt_DsnName";
            this.txt_DsnName.Size = new System.Drawing.Size(200, 20);
            this.txt_DsnName.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 265);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 72);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(458, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_DriverList);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btn_TestConnection);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txt_DsnName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txt_Desc);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txt_Server);
            this.groupBox2.Controls.Add(this.cb_TrustedConnection);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txt_Database);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(10, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(441, 177);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create New Odbc Data Source:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(316, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "*";
            // 
            // txt_Desc
            // 
            this.txt_Desc.Location = new System.Drawing.Point(110, 46);
            this.txt_Desc.Name = "txt_Desc";
            this.txt_Desc.Size = new System.Drawing.Size(200, 20);
            this.txt_Desc.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(316, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Server:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(316, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "*";
            // 
            // txt_Server
            // 
            this.txt_Server.Location = new System.Drawing.Point(110, 72);
            this.txt_Server.Name = "txt_Server";
            this.txt_Server.Size = new System.Drawing.Size(200, 20);
            this.txt_Server.TabIndex = 5;
            // 
            // cb_TrustedConnection
            // 
            this.cb_TrustedConnection.AutoSize = true;
            this.cb_TrustedConnection.Location = new System.Drawing.Point(110, 150);
            this.cb_TrustedConnection.Name = "cb_TrustedConnection";
            this.cb_TrustedConnection.Size = new System.Drawing.Size(119, 17);
            this.cb_TrustedConnection.TabIndex = 10;
            this.cb_TrustedConnection.Text = "Trusted Connection";
            this.cb_TrustedConnection.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Driver name:";
            // 
            // txt_Database
            // 
            this.txt_Database.Location = new System.Drawing.Point(110, 124);
            this.txt_Database.Name = "txt_Database";
            this.txt_Database.Size = new System.Drawing.Size(200, 20);
            this.txt_Database.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Database:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tp_MachineDataSource);
            this.tabControl1.Controls.Add(this.tp_DefineNewDataSource);
            this.tabControl1.Location = new System.Drawing.Point(12, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(470, 249);
            this.tabControl1.TabIndex = 4;
            // 
            // tp_MachineDataSource
            // 
            this.tp_MachineDataSource.Controls.Add(this.btn_Refresh);
            this.tp_MachineDataSource.Controls.Add(this.lb_Dsnlist);
            this.tp_MachineDataSource.Location = new System.Drawing.Point(4, 22);
            this.tp_MachineDataSource.Name = "tp_MachineDataSource";
            this.tp_MachineDataSource.Padding = new System.Windows.Forms.Padding(3);
            this.tp_MachineDataSource.Size = new System.Drawing.Size(462, 223);
            this.tp_MachineDataSource.TabIndex = 0;
            this.tp_MachineDataSource.Text = "Machine Data Source";
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(370, 194);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_Refresh.TabIndex = 1;
            this.btn_Refresh.Text = "Refresh";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // lb_Dsnlist
            // 
            this.lb_Dsnlist.FormattingEnabled = true;
            this.lb_Dsnlist.Location = new System.Drawing.Point(15, 17);
            this.lb_Dsnlist.Name = "lb_Dsnlist";
            this.lb_Dsnlist.Size = new System.Drawing.Size(430, 173);
            this.lb_Dsnlist.TabIndex = 0;
            this.lb_Dsnlist.Click += new System.EventHandler(this.lb_Dsnlist_DoubleClick);
            // 
            // tp_DefineNewDataSource
            // 
            this.tp_DefineNewDataSource.Controls.Add(this.groupBox2);
            this.tp_DefineNewDataSource.Controls.Add(this.btn_Retype);
            this.tp_DefineNewDataSource.Controls.Add(this.btn_SaveDsn);
            this.tp_DefineNewDataSource.Location = new System.Drawing.Point(4, 22);
            this.tp_DefineNewDataSource.Name = "tp_DefineNewDataSource";
            this.tp_DefineNewDataSource.Padding = new System.Windows.Forms.Padding(3);
            this.tp_DefineNewDataSource.Size = new System.Drawing.Size(462, 223);
            this.tp_DefineNewDataSource.TabIndex = 1;
            this.tp_DefineNewDataSource.Text = "Define New Data Source";
            // 
            // Build
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 346);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.MaximumSize = new System.Drawing.Size(510, 384);
            this.MinimumSize = new System.Drawing.Size(510, 384);
            this.Name = "Build";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Data Source";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tp_MachineDataSource.ResumeLayout(false);
            this.tp_DefineNewDataSource.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.Forms.Button btn_Retype;
        private Windows.Forms.Button btn_SaveDsn;
        private Windows.Forms.ComboBox cb_DriverList;
        private Windows.Forms.Label label11;
        private Windows.Forms.Label label10;
        private Windows.Forms.Button btn_TestConnection;
        private Windows.Forms.Label label2;
        private Windows.Forms.TextBox txt_DsnName;
        private Windows.Forms.GroupBox groupBox1;
        private Windows.Forms.Label label1;
        private Windows.Forms.GroupBox groupBox2;
        private Windows.Forms.Label label3;
        private Windows.Forms.Label label9;
        private Windows.Forms.TextBox txt_Desc;
        private Windows.Forms.Label label8;
        private Windows.Forms.Label label4;
        private Windows.Forms.Label label7;
        private Windows.Forms.TextBox txt_Server;
        private Windows.Forms.CheckBox cb_TrustedConnection;
        private Windows.Forms.Label label5;
        private Windows.Forms.TextBox txt_Database;
        private Windows.Forms.Label label6;
        private Windows.Forms.TabControl tabControl1;
        private Windows.Forms.TabPage tp_MachineDataSource;
        private Windows.Forms.Button btn_Refresh;
        private Windows.Forms.ListBox lb_Dsnlist;
        private Windows.Forms.TabPage tp_DefineNewDataSource;
    }
}