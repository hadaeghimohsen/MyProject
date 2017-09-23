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
   public partial class INF_CTWK_F : UserControl
   {
      public INF_CTWK_F()
      {
         InitializeComponent();         
      }

      private XElement xinput;
      private bool requery = false;
      private long fileno, compcode;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if (fileno != 0)
         {
            CtifBs.DataSource = iCRM.Contact_Infos.Where(sc => sc.SERV_FILE_NO == fileno);
            WkifBs.DataSource = iCRM.Weekday_Infos.Where(sw => sw.SERV_FILE_NO == fileno);
         }
         else if(compcode != 0)
         {
            CtifBs.DataSource = iCRM.Contact_Infos.Where(sc => sc.COMP_CODE == compcode);
            WkifBs.DataSource = iCRM.Weekday_Infos.Where(sw => sw.COMP_CODE == compcode);
         }

         ApbsBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "CONTACT_INFO" && a.REF_CODE == null);

         foreach (var wkdy in WkifBs.List.OfType<Data.Weekday_Info>())
         {
            var rslt = Weekdays_Flp.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag.ToString() == wkdy.WEEK_DAY);
            rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.Gainsboro : Color.YellowGreen;
         }
         requery = false;
      }      

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CtifBs.EndEdit();

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

      private void Wkdy00i_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sb = sender as SimpleButton;
            var wkdy = WkifBs.List.OfType<Data.Weekday_Info>().FirstOrDefault(wd => wd.WEEK_DAY == sb.Tag.ToString());

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

            Save_Butn_Click(null, null);
         }
         catch(Exception exc)
         {
            iCRM.SaveException(exc);
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
                              new XAttribute("tablename", "CONTACT_INFO"),
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

      private void ApbsList_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (ApbsList_Lov.EditValue == null || ApbsList_Lov.EditValue.ToString() == "") { ApbsList_Lov.Focus(); return; }
                  //if (CtifBs.List.OfType<Data.Contact_Info>().Any(t => t.APBS_CODE == (long)ApbsList_Lov.EditValue)) { MessageBox.Show("این آیتم قبلا ثبت شده است"); return; }

                  CtifBs.AddNew();
                  var ctif = CtifBs.Current as Data.Contact_Info;

                  if (fileno != 0)
                     ctif.SERV_FILE_NO = fileno;
                  else if (compcode != 0)
                     ctif.COMP_CODE = compcode;

                  ctif.APBS_CODE = (long)ApbsList_Lov.EditValue;                  
                  Ctif_Gv.PostEditor();
                  CtifBs.EndEdit();

                  iCRM.SubmitChanges();
                  requery = true;
                  break;
               default:
                  break;
            }
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

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  var ctif = CtifBs.Current as Data.Contact_Info;

                  iCRM.DEL_CTIF_P(ctif.CODE);
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void CtifBs_CurrentChanged(object sender, EventArgs e)
      {
         Mesg_Butn.Enabled = Email_Butn.Enabled = false;
         var ctif = CtifBs.Current as Data.Contact_Info;
         if (ctif == null || ctif.CONT_DESC == null) { Email_Butn.Enabled = Mesg_Butn.Enabled = false; return; }

         // Message Butn Check Validation
         if (ctif.CONT_DESC.Substring(0, 2) == "09" && ctif.CONT_DESC.Length == 11) { Mesg_Butn.Enabled = true; }

         // Email Butn Check Validation
         if (ctif.CONT_DESC.Contains("@")) { Email_Butn.Enabled = true; }
      }

      private void Email_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CtifBs_CurrentChanged(null, null);

            var ctif = CtifBs.Current as Data.Contact_Info;
            if (ctif == null || ctif.CONT_DESC == null) { return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", ctif.SERV_FILE_NO), 
                           new XAttribute("emid", 0),
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("toemail", ctif.CONT_DESC ?? "")
                        )
                   },
                 })
            );
         }
         catch { }
      }

      private void Mesg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CtifBs_CurrentChanged(null, null);

            var ctif = CtifBs.Current as Data.Contact_Info;
            if (ctif == null || ctif.CONT_DESC == null) { return; }
            
            _DefaultGateway.Gateway(

               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.Self, 53 /* Execute Opt_Mesg_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_MESG_F", 10 /* Execute ACTN_CALF_P */)
                     {
                        Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", ctif.SERV_FILE_NO), 
                           new XAttribute("msid", 0), 
                           new XAttribute("cellphon", ctif.CONT_DESC ?? ""),
                           new XAttribute("formcaller", GetType().Name)
                        )
                     },
                  })
            );
         }
         catch { }
      }
   }
}
