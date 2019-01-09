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
using System.MaxUi;
using System.Xml.Linq;

namespace System.Scsc.Ui.Notifications
{
   public partial class ATTN_DAYN_F : UserControl
   {
      public ATTN_DAYN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);

            FromAttnDate_Date.Value = FromAttnDate_Date.Value.HasValue ? FromAttnDate_Date.Value.Value : DateTime.Now;
            if (!ToAttnDate_Date.Value.HasValue)
               ToAttnDate_Date.Value = FromAttnDate_Date.Value;

            if (CBMT_CODE_GridLookUpEdit.EditValue == null || CBMT_CODE_GridLookUpEdit.EditValue.ToString() == "")
               AttnBs1.DataSource =
                  iScsc.Attendances
                  .Where(a =>
                     a.ATTN_DATE.Date >= FromAttnDate_Date.Value.Value.Date &&
                     a.ATTN_DATE.Date <= ToAttnDate_Date.Value.Value.Date &&
                     a.ATTN_STAT == "002" &&
                     Fga_Uclb_U.Contains(a.CLUB_CODE)
                  );
            else
            {
               var cbmtcode = (long?)CBMT_CODE_GridLookUpEdit.EditValue;
               var cbmtobj = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.CODE == cbmtcode);

               AttnBs1.DataSource =
                  iScsc.Attendances
                  .Where(a =>
                     a.ATTN_DATE.Date >= FromAttnDate_Date.Value.Value.Date &&
                     a.ATTN_DATE.Date <= ToAttnDate_Date.Value.Value.Date &&
                     (Coch_Pkb.PickChecked == false || a.COCH_FILE_NO == cbmtobj.COCH_FILE_NO) &&
                     (Mtod_Pkb.PickChecked == false || a.MTOD_CODE_DNRM == cbmtobj.MTOD_CODE) &&
                     (Cbmt_Pkb.PickChecked == false || a.CBMT_CODE_DNRM == cbmtobj.CODE) &&
                     a.ATTN_STAT == "002" &&
                     Fga_Uclb_U.Contains(a.CLUB_CODE)
                  );
            }
         }catch(Exception exc)
         {
            CBMT_CODE_GridLookUpEdit.EditValue = null;
            Execute_Query();
         }
      }

      private void Reload_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var attn = AttnBs1.Current as Data.Attendance;
            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){ Input = Keys.Escape },
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", attn.FIGH_FILE_NO),
                                 new XAttribute("attndate",attn.ATTN_DATE.Date),
                                 new XAttribute("attncode", attn.CODE),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                           }
                        })
                  );
                  break;
               case 1:
                  attn.MDFY_DATE = DateTime.Now;
                  iScsc.SubmitChanges();
                  requery = true;
                  break;
               case 2:
                  Back_Butn_Click(null, null);
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", attn.FIGH_FILE_NO)) }
                  );
                  break;
               case 3:
                  if (attn.EXIT_TIME == null)
                  {
                     if (MessageBox.Show(this, "با خروج دستی مشتری موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, null, null, "003", attn.MBSP_RWNO_DNRM, "001", "002");
                     requery = true;
                  }
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            //MsgBox.Show(exc.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, exc.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Btn_AutoExitAttn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "با خروج دستی همه مشترییان موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.AUTO_AEXT_P(new XElement("Process"));
            requery = true;
         }
         catch (Exception ex)
         {
            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query();
            }
         }
      }

      private void PrintDefault_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!FromAttnDate_Date.Value.HasValue)
            {
               FromAttnDate_Date.Focus();
               return;
            }

            Back_Butn_Click(null, null);
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Attn_Date BETWEEN '{0}' AND '{1}'", FromAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd")))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void Print_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Back_Butn_Click(null, null);
            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Attn_Date BETWEEN '{0}' AND '{1}'", FromAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd")))}
                    });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintSetting_Butn_Click(object sender, EventArgs e)
      {
         Back_Butn_Click(null, null);
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void ClearCbmt_Butn_Click(object sender, EventArgs e)
      {
         CBMT_CODE_GridLookUpEdit.EditValue = null;
      }
   }
}
