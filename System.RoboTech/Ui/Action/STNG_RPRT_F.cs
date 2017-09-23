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
using System.RoboTech.ExceptionHandlings;
using System.RoboTech.Controller;
using System.Xml.Linq;

namespace System.RoboTech.Ui.Action
{
   public partial class STNG_RPRT_F : UserControl
   {
      public STNG_RPRT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private XElement xinput;

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         MdrpBs.DataSource = iRoboTech.Modual_Reports.Where(mr => mr.MDUL_NAME == xinput.Attribute("modul").Value);

         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Save_butn_Click(object sender, EventArgs e)
      {
         try
         {
            MdrpBs.EndEdit();

            iRoboTech.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void SelectFile_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         if(e.Button.Index == 0)
         {
            if (Ofd_ReportFiles.ShowDialog() != DialogResult.OK) return;

            SelectFile_Butn.Text = Ofd_ReportFiles.FileName;
         }
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         MdrpBs.AddNew();

         var mdrp = MdrpBs.Current as Data.Modual_Report;
         mdrp.MDUL_NAME = xinput.Attribute("modul").Value;
         mdrp.SECT_NAME = xinput.Attribute("section").Value;
         mdrp.MDUL_DESC = mdrp.MDUL_NAME;
         mdrp.SECT_DESC = mdrp.SECT_NAME;
         mdrp.STAT = mdrp.SHOW_PRVW = mdrp.DFLT = mdrp.PRNT_AFTR_PAY = "002";
      }

      private void Save_Butn_Click_1(object sender, EventArgs e)
      {
         try
         {
            MdrpBs.EndEdit();

            iRoboTech.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف گزارش موافق هستین", "حذف گزارش") != DialogResult.Yes) return;

            var mdrp = MdrpBs.Current as Data.Modual_Report;

            iRoboTech.Modual_Reports.DeleteOnSubmit(mdrp);

            iRoboTech.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }
   }
}
