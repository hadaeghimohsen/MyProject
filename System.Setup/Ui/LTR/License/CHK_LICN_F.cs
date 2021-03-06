﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace System.Setup.Ui.LTR.License
{
   public partial class CHK_LICN_F : UserControl
   {
      public CHK_LICN_F()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         Application.Exit();
         Process.GetCurrentProcess().Kill();
      }      

      #region Request License Key
      private string MtoS(DateTime dt)
      {
         PersianCalendar pc = new PersianCalendar();
         return string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
      }

      private string NumToUnit(double? byteCount)      
      {
         try
         {
            string[] suf = { "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
               return "0 " + suf[0];
            long bytes = Math.Abs((long)byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign((long)byteCount) * num).ToString() + " " + suf[place];
         }
         catch { return "0 MB"; }
      }

      private void GenerateKey_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (HostInstaller_Txt.EditValue == null || HostInstaller_Txt.Text == "") { HostInstaller_Txt.Focus(); return; }
            if (UserInstaller_Txt.EditValue == null || UserInstaller_Txt.Text == "") { UserInstaller_Txt.Focus(); return; }
            if (Company_Txt.EditValue == null || Company_Txt.Text == "") { Company_Txt.Focus(); return; }
            if (CellPhon_Txt.EditValue == null || CellPhon_Txt.Text == "") { CellPhon_Txt.Focus(); return; }
            if (EmailAddress_Txt.EditValue == null || EmailAddress_Txt.Text == "") { EmailAddress_Txt.Focus(); return; }
            if (LicenseKey_Txt.EditValue == null || LicenseKey_Txt.Text == "") { LicenseKey_Txt.Focus(); return; }

            var dataclient = 
               new List<string> { 
                  HostInstaller_Txt.Text, // 0
                  UserInstaller_Txt.Text, // 1
                  Company_Txt.Text, // 0
                  CellPhon_Txt.Text, // 1
                  EmailAddress_Txt.Text, // 2
                  LicServer_Rb.Checked ? "002" : "001", // 5
                  LicenseKey_Txt.Text // 6
               };

            // get unvisible data
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "DefaultGateway",
                        new List<Job>
                        {
                           new Job(SendType.External, "DataGuard", 
                              new List<Job>
                              {
                                 new Job(SendType.Self, 19 /* Execute DoWork4GetAccountName */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); AccountName_Txt.Text = output.ToString();})},
                                 new Job(SendType.Self, 18 /* Execute DoWork4GetBIOScaption */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); BIOSCaption_Txt.Text = output.ToString();})},
                                 new Job(SendType.Self, 25 /* Execute DoWork4GetBoardMaker */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); BoardMaker_Txt.Text = output.ToString();})},
                                 //new Job(SendType.Self, 26 /* Execute DoWork4GetBoardProductId */ ){AfterChangedOutput = new Action<object>((output) => dataclient.Add(output.ToString()))},
                                 new Job(SendType.Self, 13 /* Execute DoWork4GetHDDSerialNo */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); HDDSerialNo_Txt.Text = output.ToString();})},
                                 new Job(SendType.Self, 14 /* Execute DoWork4GetMACAddress */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); MACAddress_Txt.Text = output.ToString();})},
                                 new Job(SendType.Self, 29 /* Execute DoWork4GetOSInformation */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); OSInfo_Txt.Text = output.ToString();})},
                                 new Job(SendType.Self, 20 /* Execute DoWork4GetPhysicalMemory */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); PhysicalMemory_Txt.Text = output.ToString();})},
                                 new Job(SendType.Self, 12 /* Execute DoWork4GetProcessorId */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); ProcessorID_Txt.Text = output.ToString();})},
                                 new Job(SendType.Self, 30 /* Execute DoWork4GetProcessorInformation */ ){AfterChangedOutput = new Action<object>((output) => {dataclient.Add(output.ToString()); ProcessorInfo_Txt.Text = output.ToString();})}
                              }
                           )                           
                        }
                     )
                  }
               )
            );

            var datastring = string.Join("::##::", dataclient);

            ClientKey0_Text.Text = Encrypt(datastring);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ClipboardSet_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as SimpleButton;
            switch (butn.Tag.ToString())
            {
               case "0":
                  Clipboard.SetText(ClientKey0_Text.Text);
                  break;
               case "1":
                  Clipboard.SetText(ServerKey1_Text.Text);
                  break;
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

      private void ClipboardGet_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as SimpleButton;
            switch(butn.Tag.ToString())
            {
               case "0":
                  ServerKey0_Text.Text = Clipboard.GetText();
                  break;
               case "1":
                  ClientKey1_Text.Text = Clipboard.GetText();
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OpenServerGenerateKey_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ServerKey0_Text.Text == "") return;

            var serverkeyTemp = ServerKey0_Text.Text;
            var datastring = Decrypt(ServerKey0_Text.Text);
            var dataclient = datastring.Split(new string[] { "::##::" }, StringSplitOptions.None).ToList();

            ServerKey0_Text.Text = serverkeyTemp;

            if (HostInstaller_Txt.Text != dataclient[0]) { MessageBox.Show("License key is not valid!"); return; }
            if (UserInstaller_Txt.Text != dataclient[1]) { MessageBox.Show("License key is not valid!"); return; }
            if (Company_Txt.Text != dataclient[2]) { MessageBox.Show("License key is not valid!"); return; }
            if (CellPhon_Txt.Text != dataclient[3]) { MessageBox.Show("License key is not valid!"); return; }
            if (EmailAddress_Txt.Text != dataclient[4]) { MessageBox.Show("License key is not valid!"); return; }
            if ((LicServer_Rb.Checked ? "002" : "001") != dataclient[5]) { MessageBox.Show("License key is not valid!"); return; }
            if (LicenseKey_Txt.Text != dataclient[6]) { MessageBox.Show("License key is not valid!"); return; }
            if (AccountName_Txt.Text != dataclient[7]) { MessageBox.Show("License key is not valid!"); return; }
            if (BIOSCaption_Txt.Text != dataclient[8]) { MessageBox.Show("License key is not valid!"); return; }
            if (BoardMaker_Txt.Text != dataclient[9]) { MessageBox.Show("License key is not valid!"); return; }
            if (HDDSerialNo_Txt.Text != dataclient[10]) { MessageBox.Show("License key is not valid!"); return; }
            if (MACAddress_Txt.Text != dataclient[11]) { MessageBox.Show("License key is not valid!"); return; }
            if (OSInfo_Txt.Text != dataclient[12]) { MessageBox.Show("License key is not valid!"); return; }
            if (PhysicalMemory_Txt.Text != dataclient[13]) { MessageBox.Show("License key is not valid!"); return; }
            if (ProcessorID_Txt.Text != dataclient[14]) { MessageBox.Show("License key is not valid!"); return; }
            if (ProcessorInfo_Txt.Text != dataclient[15]) { MessageBox.Show("License key is not valid!"); return; }
            Kernel1_Cb.Checked = (dataclient[16] == "Kernel+") ? true : false;
            Arta1_Cb.Checked = (dataclient[17] == "Arta+") ? true : false;
            CRM1_Cb.Checked = (dataclient[18] == "CRM+") ? true : false;
            Telegram1_Cb.Checked = (dataclient[19] == "Telegram+") ? true : false;

            // Open First Page Setup
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 02 /* Execute Frst_Page_F */)
                  }
               )
            );
            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion      

      #region CryptoStream
      readonly string PasswordHash = "P@@Sw0rd";
      readonly string SaltKey = "S@LT&KEY";
      readonly string VIKey = "@1B2c3D4e5F6g7H8";

      public string Encrypt(string plainText)
      {
         byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

         byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
         var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
         var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

         byte[] cipherTextBytes;

         using (var memoryStream = new MemoryStream())
         {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
               cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
               cryptoStream.FlushFinalBlock();
               cipherTextBytes = memoryStream.ToArray();
               cryptoStream.Close();
            }
            memoryStream.Close();
         }
         return Convert.ToBase64String(cipherTextBytes);
      }

      public string Decrypt(string encryptedText)
      {
         byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
         byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
         var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

         var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
         var memoryStream = new MemoryStream(cipherTextBytes);
         var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
         byte[] plainTextBytes = new byte[cipherTextBytes.Length];

         int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
         memoryStream.Close();
         cryptoStream.Close();
         return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
      }
      #endregion

      #region Recieve License Key
      private void OpenClientGenerateKey_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ClientSysInfo1_Pn.Visible = false;
            if (ClientKey1_Text.Text == "") return;

            var datastring = Decrypt(ClientKey1_Text.Text);
            var dataclient = datastring.Split(new string[] { "::##::" }, StringSplitOptions.None).ToList();

            HostInstaller1_Txt.EditValue = dataclient[0];
            UserInstaller1_Txt.Text = dataclient[1];            
            Company1_Txt.Text = dataclient[2];
            Cellphon1_Txt.Text = dataclient[3];
            EmailAddress1_Txt.Text = dataclient[4];
            if (dataclient[5] == "002") LicServer1_Rb.Checked = true; else LicClient1_Rb.Checked = true;
            LicenseKey1_Txt.Text = dataclient[6];
            AccountName1_Txt.Text = dataclient[7];
            BIOSCaption1_Txt.Text = dataclient[8];
            BoardMaker1_Txt.Text = dataclient[9];
            HDDSerialNo1_Txt.Text = dataclient[10];
            MACAddress1_Txt.Text = dataclient[11];
            OSInfo1_Txt.Text = dataclient[12];
            PhysicalMemory1_Txt.Text = dataclient[13];
            ProcessorID1_Txt.Text = dataclient[14];
            ProcessorInfo1_Txt.Text = dataclient[15];
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GenerateServerKey_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Company1_Txt.EditValue == null || Company1_Txt.Text == "") { Company1_Txt.Focus(); return; }
            if (Cellphon1_Txt.EditValue == null || Cellphon1_Txt.Text == "") { Cellphon1_Txt.Focus(); return; }
            if (EmailAddress1_Txt.EditValue == null || EmailAddress1_Txt.Text == "") { EmailAddress1_Txt.Focus(); return; }
            if (UserInstaller1_Txt.EditValue == null || UserInstaller1_Txt.Text == "") { UserInstaller1_Txt.Focus(); return; }
            if (HostInstaller1_Txt.EditValue == null || HostInstaller1_Txt.Text == "") { HostInstaller1_Txt.Focus(); return; }
            

            if (PasswordHash_Txt.Text != PasswordHash) { PasswordHash_Txt.Focus(); return; }
            if (SaltKey_Txt.Text != SaltKey) { SaltKey_Txt.Focus(); return; }
            if (VIKey_Txt.Text != VIKey) { VIKey_Txt.Focus(); return; }

            var dataclient =
               new List<string> { 
                  HostInstaller1_Txt.Text, // 0
                  UserInstaller1_Txt.Text, // 1
                  Company1_Txt.Text, // 2
                  Cellphon1_Txt.Text, // 3
                  EmailAddress1_Txt.Text, // 4
                  LicServer1_Rb.Checked ? "002" : "001", // 5
                  LicenseKey1_Txt.Text, // 6                  
                  AccountName1_Txt.Text,
                  BIOSCaption1_Txt.Text,
                  BoardMaker1_Txt.Text,
                  HDDSerialNo1_Txt.Text,
                  MACAddress1_Txt.Text,
                  OSInfo1_Txt.Text,
                  PhysicalMemory1_Txt.Text,
                  ProcessorID1_Txt.Text,
                  ProcessorInfo1_Txt.Text,
                  Kernel_Cb.Checked ? "Kernel+" : "Kernel-",
                  Arta_Cb.Checked ? "Arta+" : "Arta-",
                  CRM_Cb.Checked ? "CRM+" : "CRM-",
                  Telegram_Cb.Checked ? "Telegram+" : "Telegram-"
               };
            var datastring = string.Join("::##::", dataclient);

            ServerKey1_Text.Text = Encrypt(datastring);
            ClientSysInfo1_Pn.Visible = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion            
   }
}
