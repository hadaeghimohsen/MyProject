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

namespace System.CRM.Ui.BaseDefination
{
   public partial class STNG_DFIN_F : UserControl
   {
      public STNG_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private async void Execute_Query()
      {
         var result = await Task.Run(() =>
         {
            using (var ctx = new Data.iCRMDataContext(ConnectionString))
            {
               return new
               {
                  Setting = ctx.Settings.FirstOrDefault(s => s.USER_NAME == CurrentUser),
                  SignDigtTemplates = ctx.Templates.Where(t => t.TEMP_TYPE == "003").ToList(),
                  EmalTemplates = ctx.Templates.Where(t => t.TEMP_TYPE == "002").ToList(),
                  SmsTemplates = ctx.Templates.Where(t => t.TEMP_TYPE == "001").ToList()
               };
            }
         });

         iCRM = new Data.iCRMDataContext(ConnectionString);
         StngBs.DataSource = result.Setting;
         SignDigtTempBs.DataSource = result.SignDigtTemplates;
         EmalTempBs.DataSource = result.EmalTemplates;
         SmsTempBs.DataSource = result.SmsTemplates;
      }

      private void Refresh_Clicked(object sender, EventArgs e)
      {
         Execute_Query();
         requery = false;
      }

      private void StngSubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            StngBs.EndEdit();

            iCRM.SubmitChanges();
            requery = true;
         }
         catch
         {}
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void AddNewAppBase_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 79 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "SECTION_FORM"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
