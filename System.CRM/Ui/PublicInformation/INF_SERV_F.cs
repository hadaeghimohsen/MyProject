using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;
using System.CRM.ExceptionHandlings;
using System.IO;
using System.MaxUi;
using DevExpress.XtraEditors;

namespace System.CRM.Ui.PublicInformation
{
   public partial class INF_SERV_F : UserControl
   {
      public INF_SERV_F()
      {
         InitializeComponent();         
      }

      private XElement xinput;
      private bool requery = false;
      private long fileno;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         SrvcBs.DataSource = iCRM.Service_Contacts.Where(sc => sc.SERV_FILE_NO == fileno);
         SrvwBs.DataSource = iCRM.Service_Weekdays.Where(sw => sw.SERV_FILE_NO == fileno);

         foreach (var wkdy in SrvwBs.List.OfType<Data.Service_Weekday>())
         {
            var rslt = Weekdays_Flp.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag.ToString() == wkdy.WEEK_DAY);
            rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.Gainsboro : Color.YellowGreen;
         }
         requery = false;
      }

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var srvc = SrvcBs.Current as Data.Service_Contact;

            iCRM.DEL_SRVC_P(srvc.CODE);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SrvcBs.EndEdit();

            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         SrvcBs.AddNew();
         var Srvc = SrvcBs.Current as Data.Service_Contact;
         Srvc.SERV_FILE_NO = fileno;
      }

      private void Wkdy00i_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sb = sender as SimpleButton;
            var wkdy = SrvwBs.List.OfType<Data.Service_Weekday>().FirstOrDefault(wd => wd.WEEK_DAY == sb.Tag.ToString());

            if (sb.Appearance.BackColor == Color.Gainsboro)
            {
               sb.Appearance.BackColor = Color.YellowGreen;
               wkdy.STAT = "002";
            }
            else
            {
               sb.Appearance.BackColor = Color.Gainsboro;
               wkdy.STAT = "001";
            }
         }
         catch(Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }
   }
}
