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
using System.Xml.Linq;
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;
using DevExpress.XtraEditors;
using System.MaxUi;

namespace System.CRM.Ui.BaseDefination
{
   public partial class SJBP_DFIN_F : UserControl
   {
      public SJBP_DFIN_F()
      {
         InitializeComponent();
      }

      private string formcaller;
      private XElement xinput;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Approve_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var jobp = JobpBs1.Current as Data.Job_Personnel;
            if (jobp == null) return;

            xinput.Add(
               new XAttribute("user_mentioned", string.Format("@{{{0}}}", jobp.USER_DESC_DNRM))
            );

            Back_Butn_Click(null, null);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, formcaller, 150 /* Execute Mentioned */)
                     {
                        Input = xinput                           
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            //throw;
         }
      }
   }
}
