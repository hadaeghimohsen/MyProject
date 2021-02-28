using System;
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

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsSystemLicense : UserControl
   {
      public SettingsSystemLicense()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }      

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         if(Tb_Master.SelectedTab == tp_001)
         {
            int subsys = SubSysBs.Position;
            SubSysBs.DataSource = iProject.Sub_Systems.Where(s => s.STAT == "002");
            SubSysBs.Position = subsys;

            GatewayBs.DataSource = iProject.Gateways.Where(g => g.CONF_STAT == "002");
         }
         requery = false;
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

      private void SubSysBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            if (Tb_Master.SelectedTab == tp_001)
            {
               if (subsys.CLNT_LICN_DESC == null || subsys.CLNT_LICN_DESC == "")
               {
                  CompName_Txt.Text = "";
                  CellPhon_Txt.Text = "";
                  Emal_Txt.Text = "";
                  FullName_Txt.Text = "";
                  Gateway_Lov.EditValue = "";
                  Lic30Day_Rb.Checked = true;
                  TrialDate_Dt.Value = null;
                  LicAmnt_Txt.Text = "";
               }
               else
               {
                  var datastring = Decrypt(subsys.CLNT_LICN_DESC);
                  var dataclient = datastring.Split(':').ToList();

                  //SubSysBs.Position = SubSysBs.List.OfType<Data.Sub_System>().FirstOrDefault(s => s.SUB_SYS == Convert.ToInt32(dataclient[6])).SUB_SYS;

                  CompName_Txt.Text = dataclient[0];
                  CellPhon_Txt.Text = dataclient[1];
                  Emal_Txt.Text = dataclient[2];
                  FullName_Txt.Text = dataclient[3];
                  Gateway_Lov.EditValue = dataclient[4];
                  if (dataclient[5] == "001") Lic30Day_Rb.Checked = true; else LicNDay_Rb.Checked = true;

                  if (subsys.SRVR_LICN_DESC == null || subsys.SRVR_LICN_DESC == "")
                  {
                     TrialDate_Dt.Value = null;
                     LicAmnt_Txt.Text = "";
                     DayRemn_Txt.Text = "";
                  }
                  else
                  {
                     TrialDate_Dt.Value = Convert.ToDateTime(dataclient[7]);
                     LicAmnt_Txt.Text = dataclient[8];

                     DayRemn_Txt.Text = ((TrialDate_Dt.Value.Value.Date) - (DateTime.Now.Date)).Days.ToString();
                  }
               }
            }
         }
         catch (Exception exc)
         {

         }
      }

      private void SaveSubSys_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            if (ServerKey0_Text.Text == "") { return; }
            if (ClientKey0_Text.Text == "") { return; }

            var serverkey = ServerKey0_Text.Text;
            var clientkey = ClientKey0_Text.Text;

            if (ServerKey0_Text.Text != "")
            {
               var datastring = Decrypt(ServerKey0_Text.Text);
               var dataclient = datastring.Split(':').ToList();

               if (!GatewayBs.List.OfType<Data.Gateway>().Any(g => g.MAC_ADRS == dataclient[4]))
               {
                  subsys.CLNT_LICN_DESC = "";
                  subsys.SRVR_LICN_DESC = "";
               }    
               else
               {
                  if (dataclient[5] == "001") subsys.LICN_TYPE = "002"; else subsys.LICN_TYPE = "001";
                  subsys.LICN_TRIL_DATE = Convert.ToDateTime(dataclient[7]);
                  subsys.CLNT_LICN_DESC = clientkey;
                  subsys.SRVR_LICN_DESC = serverkey;
                  subsys.SUPR_YEAR_PRIC = Convert.ToInt64(LicAmnt_Txt.EditValue);
               }
            }

            iProject.UpdateSubSystem(subsys.SUB_SYS, subsys.STAT, subsys.INST_STAT, subsys.INST_DATE, subsys.LICN_TYPE, subsys.LICN_TRIL_DATE, subsys.CLNT_LICN_DESC, subsys.SRVR_LICN_DESC, subsys.SUB_DESC, subsys.JOBS_STAT, subsys.FREQ_INTR, subsys.VERS_NO, subsys.SUPR_YEAR_PRIC);
         }
         catch (Exception exc)
         {}
      }

      private void ClearSubSysDesc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            subsys.SUB_DESC = "";

            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {

         }
      }

      private void GenerateKey_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            if (CompName_Txt.EditValue == null || CompName_Txt.Text == "") { CompName_Txt.Focus(); return; }
            if (CellPhon_Txt.EditValue == null || CellPhon_Txt.Text == "") { CellPhon_Txt.Focus(); return; }
            if (Emal_Txt.EditValue == null || Emal_Txt.Text == "") { Emal_Txt.Focus(); return; }
            if (FullName_Txt.EditValue == null || FullName_Txt.Text == "") { FullName_Txt.Focus(); return; }
            if (Gateway_Lov.EditValue == null || Gateway_Lov.Text == "") { Gateway_Lov.Focus(); return; }
            if (LicAmnt_Txt.EditValue == null || LicAmnt_Txt.Text == "") { LicAmnt_Txt.Focus(); return; }
            if (LicAmntCode_Txt.EditValue == null || LicAmntCode_Txt.Text == "") { LicAmntCode_Txt.Focus(); return; }

            var dataclient = 
               new List<string> { 
                  CompName_Txt.Text, // 0
                  CellPhon_Txt.Text, // 1
                  Emal_Txt.Text, // 2
                  FullName_Txt.Text, // 3
                  Gateway_Lov.EditValue.ToString(), //4
                  Lic30Day_Rb.Checked ? "001" : "002", // 5
                  subsys.SUB_SYS.ToString(), //6
                  LicAmnt_Txt.EditValue.ToString(), // 7
                  LicAmntCode_Txt.EditValue.ToString() // 8
               };
            var datastring = string.Join(":", dataclient);

            ClientKey0_Text.Text = Encrypt(datastring);

            SaveSubSys_Butn_Click(null, null);
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
               case "2":
                  Clipboard.SetText(ServerKey2_Text.Text);
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
               case "2":
                  ClientKey2_Text.Text = Clipboard.GetText();
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
            var dataclient = datastring.Split(':').ToList();

            SubSysBs.Position = SubSysBs.IndexOf(SubSysBs.List.OfType<Data.Sub_System>().First(s => s.SUB_SYS == Convert.ToInt32(dataclient[6])));

            ServerKey0_Text.Text = serverkeyTemp;

            CompName_Txt.Text = dataclient[0];
            CellPhon_Txt.Text = dataclient[1];
            Emal_Txt.Text = dataclient[2];
            FullName_Txt.Text = dataclient[3];
            Gateway_Lov.EditValue = dataclient[4];
            if (dataclient[5] == "001") Lic30Day_Rb.Checked = true; else LicNDay_Rb.Checked = true;

            GenerateKey_Butn_Click(null, null);

            TrialDate_Dt.Value = Convert.ToDateTime(dataclient[7]);
            LicAmnt_Txt.Text = dataclient[8];
            LicAmntCode_Txt.Text = dataclient[9];

            DayRemn_Txt.Text = ((TrialDate_Dt.Value.Value.Date) - (DateTime.Now.Date)).Days.ToString();
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

      // P@@Sw0rdS@LT&KEY@1B2c3D4e5F6g7H8
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
            if (ClientKey1_Text.Text == "") return;

            var datastring = Decrypt(ClientKey1_Text.Text);
            var dataclient = datastring.Split(':').ToList();

            CompName1_Txt.Text = dataclient[0];
            CellPhon1_Txt.Text = dataclient[1];
            Emal1_Txt.Text = dataclient[2];
            FullName1_Txt.Text = dataclient[3];
            Gateway1_Lov.EditValue = dataclient[4];
            if (dataclient[5] == "001") Lic30Day1_Rb.Checked = true; else LicNDay1_Rb.Checked = true;
            SubSys1_Txt.Text = SubSysBs.List.OfType<Data.Sub_System>().FirstOrDefault(s => s.SUB_SYS == Convert.ToInt32(dataclient[6])).DESC;
            SubSys1_Txt.Tag = dataclient[6];
            LicAmnt1_Txt.Text = dataclient[7];
            LicAmntCode1_Txt.Text = dataclient[8];

            if (Lic30Day1_Rb.Checked) { TrialDate1_Dt.Value = DateTime.Now.AddDays(30); }
            else { TrialDate1_Dt.Value = DateTime.Now.AddDays(365); }
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
            if (CompName1_Txt.EditValue == null || CompName1_Txt.Text == "") { CompName1_Txt.Focus(); return; }
            if (CellPhon1_Txt.EditValue == null || CellPhon1_Txt.Text == "") { CellPhon1_Txt.Focus(); return; }
            if (Emal1_Txt.EditValue == null || Emal1_Txt.Text == "") { Emal1_Txt.Focus(); return; }
            if (FullName1_Txt.EditValue == null || FullName1_Txt.Text == "") { FullName1_Txt.Focus(); return; }
            if (Gateway1_Lov.EditValue == null || Gateway1_Lov.Text == "") { Gateway1_Lov.Focus(); return; }
            if (SubSys1_Txt.EditValue == null || SubSys1_Txt.Text == "") { SubSys1_Txt.Focus(); return; }
            if (LicAmnt1_Txt.EditValue == null || LicAmnt1_Txt.Text == "") { LicAmnt1_Txt.Focus(); return; }
            if (LicAmntCode1_Txt.EditValue == null || LicAmntCode1_Txt.Text == "") { LicAmntCode1_Txt.Focus(); return; }

            if (PasswordHash_Txt.Text != PasswordHash) { PasswordHash_Txt.Focus(); return; }
            if (SaltKey_Txt.Text != SaltKey) { SaltKey_Txt.Focus(); return; }
            if (VIKey_Txt.Text != VIKey) { VIKey_Txt.Focus(); return; }

            if (Lic30Day1_Rb.Checked) { LicAmnt1_Txt.Text = "0"; }
            if (LicAmnt1_Txt.EditValue == null || LicAmnt1_Txt.Text == "") { LicAmnt1_Txt.Focus(); return; }

            var dataclient =
               new List<string> { 
                  CompName1_Txt.Text, // 0
                  CellPhon1_Txt.Text, // 1
                  Emal1_Txt.Text, // 2
                  FullName1_Txt.Text, // 3
                  Gateway1_Lov.EditValue.ToString(), // 4
                  Lic30Day1_Rb.Checked ? "001" : "002", // 5
                  SubSys1_Txt.Tag.ToString(), // 6
                  TrialDate1_Dt.Value.Value.Date.ToString("yyyy-MM-dd"), // 7
                  LicAmnt1_Txt.EditValue.ToString(), // 8
                  LicAmntCode1_Txt.EditValue.ToString(), // 9
               };
            var datastring = string.Join(":", dataclient);

            ServerKey1_Text.Text = Encrypt(datastring);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion      

      private void GenerateServerKey2_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            if (CmndText_Txt.EditValue == null || CmndText_Txt.Text == "") { CmndText_Txt.Focus(); return; }
            if (TinyLock_Txt.EditValue == null || TinyLock_Txt.Text == "") { TinyLock_Txt.Focus(); return; }
            if (CmndDate_Dt.Value == null) { CmndDate_Dt.Focus(); return; }
            if (CmndParmValu_Txt.EditValue == null || CmndParmValu_Txt.Text == "") { CmndParmValu_Txt.Focus(); return; }
            if (Password_Txt.EditValue == null || Password_Txt.Text == "" || Password_Txt.Text != (PasswordHash + SaltKey + VIKey)) { Password_Txt.Text = ""; Password_Txt.Focus(); return; }

            var dataclient =
               new List<string> { 
                  CmndText_Txt.Text, // 0
                  TinyLock_Txt.Text, // 1
                  CmndDate_Dt.Value.Value.ToString("yyyy-MM-dd"), // 2
                  CmndParmValu_Txt.Text, // 3
                  subsys.SUB_SYS.ToString() //4
               };
            var datastring = string.Join(":", dataclient);

            ServerKey2_Text.Text = Encrypt(datastring);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveCommand_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ClientKey2_Text.Text == "") return;

            var serverkeyTemp = ClientKey2_Text.Text;
            var datastring = Decrypt(ClientKey2_Text.Text);

            iProject.EXEC_CMND_P(
               new XElement("Command",
                   new XAttribute("text", datastring)
               )
            );

            MessageBox.Show("دستور با موفقیت اجرا شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      
   }
}
