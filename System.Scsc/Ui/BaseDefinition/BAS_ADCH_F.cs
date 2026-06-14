using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_ADCH_F : UserControl
   {
      public BAS_ADCH_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void GetMaxFngrPrint_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FngrPrnt_Txt.EditValue =
                iScsc.Fighters
                .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                .Select(f => f.FNGR_PRNT_DNRM)
                .ToList()
                .Where(f => f.All(char.IsDigit))
                .Max(f => Convert.ToInt64(f)) + 1;
         }
         catch
         {
            FngrPrnt_Txt.Text = "1";
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FrstName_Txt.Text == "") { FrstName_Txt.Focus(); return; }
            if (LastName_Txt.Text == "") { LastName_Txt.Focus(); return; }
            if (NatlCode_Txt.Text == "" && !ModifierKeys.HasFlag(Keys.Control)) { NatlCode_Txt.Focus(); return; }
            if (CellPhon_Txt.Text == "" && !ModifierKeys.HasFlag(Keys.Control)) { CellPhon_Txt.Focus(); return; }
            long mtod = 0;
            if (Mtod_Lov.EditValue == null || !long.TryParse(Mtod_Lov.EditValue.ToString(), out mtod)) { Mtod_Lov.Focus(); return; }
            int sex = 0;
            if (SexType_Lov.EditValue == null || !int.TryParse(SexType_Lov.EditValue.ToString(), out sex)) { SexType_Lov.Focus(); return; }
            

            // 1404/08/10 * اگر مربی تکراری ثبت داره میکنه
            var _existsCoch = iScsc.Fighters.Any(a => a.CONF_STAT == "002" && a.FGPB_TYPE_DNRM == "003" && a.FRST_NAME_DNRM.Contains(FrstName_Txt.Text) && a.LAST_NAME_DNRM.Contains(LastName_Txt.Text));
            if (_existsCoch && MessageBox.Show(this, "با این مشخصات قبلا این پرسنل ثبت شده، آیا نیاز به بررسی میبینید؟", "خطای ثبت تکراری", MessageBoxButtons.YesNo) == DialogResult.Yes) return;            

            iScsc.ADM_MSAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "001"),
                     new XAttribute("rqttcode", "003"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName_Txt.Text),
                        new XElement("Last_Name", LastName_Txt.Text),
                        //new XElement("Fath_Name", ""),
                        //new XElement("Coch_Deg", ""),
                        new XElement("Coch_Crtf_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        //new XElement("Gudg_Deg", ""),
                        //new XElement("Glob_Code", ""),
                        new XElement("Sex_Type", SexType_Lov.EditValue),
                        new XElement("Natl_Code", NatlCode_Txt.Text),
                        new XElement("No_Chek_Natl_Code", (ModifierKeys.HasFlag(Keys.Control) ? "002" : "001")),
                        new XElement("Brth_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("Cell_Phon", CellPhon_Txt.Text),
                        new XElement("No_Chek_Cell_Phon", (ModifierKeys.HasFlag(Keys.Control) ? "002" : "001")),
                        //new XElement("Tell_Phon", ""),
                        new XElement("Type", "003"),
                        //new XElement("Post_Adrs", ""),
                        //new XElement("Emal_Adrs", ""),
                        //new XElement("Insr_Numb", ""),
                        new XElement("Insr_Date", DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")),
                        //new XElement("Educ_Deg", ""),
                        new XElement("Mtod_Code", Mtod_Lov.EditValue),
                        //new XElement("Dise_Code", ""),
                        //new XElement("Calc_Expn_Type", ""),
                        //new XElement("Blod_grop", ""),
                        new XElement("Fngr_Prnt", FngrPrnt_Txt.Text),
                        //new XElement("Dpst_Acnt_Slry_Bank", ""),
                        //new XElement("Dpst_Acnt_Slry", ""),
                        new XElement("Chat_Id", Chat_Id_TextEdit.Text)
                     )
                  )
               )
            );

            // Save Card In Device
            if (CardNum_Txt.Text != "")
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
                  {
                     Input =
                     new XElement("User",
                        new XAttribute("enrollnumb", FngrPrnt_Txt.Text),
                        new XAttribute("cardnumb", CardNum_Txt.Text),
                        new XAttribute("namednrm", FrstName_Txt.Text + ", " + LastName_Txt.Text)
                     )
                  }
               );

            CardNum_Txt.Text = "";

            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("TabPage",
                                 new XAttribute("showtabpage", "tp_005")
                              )
                        }
                     }
                  )
               );
               Back_Butn_Click(null, null);
               requery = false;
            }
         }
      }

      private void SaveNewCoch_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FrstName_Txt.Text == "") { FrstName_Txt.Focus(); return; }
            if (LastName_Txt.Text == "") { LastName_Txt.Focus(); return; }
            if (NatlCode_Txt.Text == "" && !ModifierKeys.HasFlag(Keys.Control)) { NatlCode_Txt.Focus(); return; }
            if (CellPhon_Txt.Text == "" && !ModifierKeys.HasFlag(Keys.Control)) { CellPhon_Txt.Focus(); return; }
            long mtod = 0;
            if (Mtod_Lov.EditValue == null || !long.TryParse(Mtod_Lov.EditValue.ToString(), out mtod)) { Mtod_Lov.Focus(); return; }
            int sex = 0;
            if (SexType_Lov.EditValue == null || !int.TryParse(SexType_Lov.EditValue.ToString(), out sex)) { SexType_Lov.Focus(); return; }

            // 1404/08/10 * اگر مربی تکراری ثبت داره میکنه
            var _existsCoch = iScsc.Fighters.Any(a => a.CONF_STAT == "002" && a.FGPB_TYPE_DNRM == "003" && a.FRST_NAME_DNRM.Contains(FrstName_Txt.Text) && a.LAST_NAME_DNRM.Contains(LastName_Txt.Text));
            if (_existsCoch && MessageBox.Show(this, "با این مشخصات قبلا این پرسنل ثبت شده، آیا نیاز به بررسی میبینید؟", "خطای ثبت تکراری", MessageBoxButtons.YesNo) == DialogResult.Yes) return;

            iScsc.ADM_MSAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "001"),
                     new XAttribute("rqttcode", "003"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName_Txt.Text),
                        new XElement("Last_Name", LastName_Txt.Text),
                        //new XElement("Fath_Name", ""),
                        //new XElement("Coch_Deg", ""),
                        new XElement("Coch_Crtf_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        //new XElement("Gudg_Deg", ""),
                        //new XElement("Glob_Code", ""),
                        new XElement("Sex_Type", SexType_Lov.EditValue),
                        new XElement("Natl_Code", NatlCode_Txt.Text),
                        new XElement("No_Chek_Natl_Code", (ModifierKeys.HasFlag(Keys.Control) ? "002" : "001")),
                        new XElement("Brth_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("Cell_Phon", CellPhon_Txt.Text),
                        new XElement("No_Chek_Cell_Phon", (ModifierKeys.HasFlag(Keys.Control) ? "002" : "001")),
                        //new XElement("Tell_Phon", ""),
                        new XElement("Type", "003"),
                        //new XElement("Post_Adrs", ""),
                        //new XElement("Emal_Adrs", ""),
                        //new XElement("Insr_Numb", ""),
                        new XElement("Insr_Date", DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")),
                        //new XElement("Educ_Deg", ""),
                        new XElement("Mtod_Code", Mtod_Lov.EditValue),
                        //new XElement("Dise_Code", ""),
                        //new XElement("Calc_Expn_Type", ""),
                        //new XElement("Blod_grop", ""),
                        new XElement("Fngr_Prnt", FngrPrnt_Txt.Text),
                        //new XElement("Dpst_Acnt_Slry_Bank", ""),
                        //new XElement("Dpst_Acnt_Slry", ""),
                        new XElement("Chat_Id", Chat_Id_TextEdit.Text)
                     )
                  )
               )
            );

            // Save Card In Device
            if (CardNum_Txt.Text != "")
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
                  {
                     Input =
                     new XElement("User",
                        new XAttribute("enrollnumb", FngrPrnt_Txt.Text),
                        new XAttribute("cardnumb", CardNum_Txt.Text),
                        new XAttribute("namednrm", FrstName_Txt.Text + ", " + LastName_Txt.Text)
                     )
                  }
               );

            CardNum_Txt.Text = "";

            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("TabPage",
                                 new XAttribute("showtabpage", "tp_005")
                              )
                        }
                     }
                  )
               );
               FrstName_Txt.Text = LastName_Txt.Text = FngrPrnt_Txt.Text = Chat_Id_TextEdit.Text = NatlCode_Txt.Text = CellPhon_Txt.Text = "";
               FrstName_Txt.Focus();
               requery = false;
            }
         }
      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("Command",
            //            new XAttribute("type", "fngrprntdev"),
            //            new XAttribute("fngractn", "enroll"),
            //            new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
            //         )
            //   }
            //);
         }
         catch{ }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", FngrPrnt_Txt.Text)
                     )
               }
            );
         }
         catch{ }
      }

      private void RqstBnDeleteFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", FngrPrnt_Txt.Text)
                     )
               }
            );
         }
         catch{ }
      }
      #endregion

      private void EnrlFngrPrnt_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == null || FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            if (ModifierKeys.HasFlag(Keys.Control))
            {

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                  
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Process", new XAttribute("type", "fngrdevlst"), new XAttribute("fngrprnt", FngrPrnt_Txt.Text))}
                     })
               );
               return;
            }            

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", FngrPrnt_Txt.Text))},
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", FngrPrnt_Txt.Text))}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CopyFngrDevc_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == null || FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", FngrPrnt_Txt.Text))}
                  })
            );
         }
         catch (Exception exc) { }
      }
   }
}
