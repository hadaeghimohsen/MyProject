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
   public partial class INF_COMP_F : UserControl
   {
      public INF_COMP_F()
      {
         InitializeComponent();         
      }

      private XElement xinput;
      private bool requery = false;
      private long compcode;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         CmpcBs.DataSource = iCRM.Company_Contacts.Where(cc => cc.COMP_CODE == compcode);
         CmpwBs.DataSource = iCRM.Company_Weekdays.Where(sw => sw.COMP_CODE == compcode);

         foreach (var wkdy in CmpwBs.List.OfType<Data.Company_Weekday>())
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

            var cmpc = CmpcBs.Current as Data.Company_Contact;

            iCRM.DEL_CMPC_P(cmpc.CODE);
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
            CmpcBs.EndEdit();

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
         CmpcBs.AddNew();
         var Cmpc = CmpcBs.Current as Data.Company_Contact;
         Cmpc.COMP_CODE = compcode;
      }

      private void Wkdy00i_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sb = sender as SimpleButton;
            var wkdy = CmpwBs.List.OfType<Data.Company_Weekday>().FirstOrDefault(wd => wd.WEEK_DAY == sb.Tag.ToString());

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
