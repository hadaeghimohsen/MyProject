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
         iScsc = new Data.iScscDataContext(ConnectionString);
         AttnDate_Date.Value = AttnDate_Date.Value.HasValue ? AttnDate_Date.Value.Value : DateTime.Now;
         AttnBs1.DataSource =
            iScsc.Attendances
            .Where(a => 
               a.ATTN_DATE.Date == AttnDate_Date.Value.Value.Date &&
               a.ATTN_STAT == "002" &&
               Fga_Uclb_U.Contains(a.CLUB_CODE)
            );
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
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", attn.FIGH_FILE_NO),
                                 new XAttribute("attndate",attn.ATTN_DATE.Date),
                                 new XAttribute("attncode", attn.CODE)
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
                     if (MessageBox.Show(this, "با خروج دستی هنرجو موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, null, null, "003");
                     requery = true;
                  }
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MsgBox.Show(exc.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
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
            if (MessageBox.Show(this, "با خروج دستی همه هنرجویان موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.AUTO_AEXT_P(new XElement("Process"));
            requery = true;
         }
         catch (Exception ex)
         {
            //MessageBox.Show(ex.Message);
            var result = MsgBox.Show(ex.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
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
   }
}
