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

namespace System.CRM.Ui.MasterPage
{
   public partial class FRST_PAGE_F : UserControl
   {
      public FRST_PAGE_F()
      {
         InitializeComponent();

         var path = new System.Drawing.Drawing2D.GraphicsPath();
         path.AddEllipse(0, 0, Lb_Notification.Width, Lb_Notification.Height);         

         this.Lb_iTask.Region = this.Lb_oTask.Region = this.Lb_Mention.Region = this.Lb_Notification.Region = new Region(path);
      }

      private long fileno, projrqstrqid;
      private string formCaller;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => Convert.ToInt32(s.ONOF_TAG_DNRM) >= 101 && s.CONF_STAT == "002");
      }

      #region basedefinition
      private void rb_stngdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 28 /* Execute Stng_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);

      }

      private void rb_regndfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 03 /* Execute Regn_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_epitdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 04 /* Execute Epit_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_btrfdifn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 05 /* Execute Btrf_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_cashdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 06 /* Execute Cash_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_regldfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 07 /* Execute Regl_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_crgldfin_Click(object sender, EventArgs e)
      {
         var crntregl = iCRM.Regulations.Where(rg => rg.REGL_STAT == "002" && rg.TYPE == "001");
         if(crntregl == null) return;
         if(crntregl.Count() > 1) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 08 /* Execute Rqrq_Dfin_F */){Input = crntregl.First()},
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_orgndfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 10 /* Execute Orgn_Dfin_F */),
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_jobpdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 16 /* Execute Jobp_Dfin_F */),
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_isicdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 20 /* Execute Isic_Dfin_F */),
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_tmpldfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 69 /* Execute Tmpl_Dfin_F */),
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_cmphdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 86 /* Execute Cmph_Dfin_F */),
                new Job(SendType.SelfToUserInterface, "CMPH_DFIN_F", 10 /* Execute Actn_CalF_P */)
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
      #endregion

      #region MainMenu
      private void rb_adms_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_admc_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 13 /* Execute Adm_Chng_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_actlogs_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 17 /* Execute Act_Logc_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_actsndf_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 18 /* Execute Act_Sndf_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_acttrat_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 19 /* Execute Act_Trat_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_taskflow_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 55 /* Execute Hst_Urqs_F */),                
                new Job(SendType.SelfToUserInterface, "HST_URQS_F", 10 /* Execute Actn_CalF_P */)
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_notftotl_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 21 /* Execute Notf_Totl_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_showleads_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 23 /* Execute Shw_Lead_F */),
                new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Service", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_showdeleteleads_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 23 /* Execute Shw_Lead_F */),
                new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous, 
                   Input = 
                     new XElement("Service", 
                        new XAttribute("onoftag", "off")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_showcontact_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 33 /* Execute Shw_Cont_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Service", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_showdeletecontact_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 33 /* Execute Shw_Cont_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Service", 
                        new XAttribute("onoftag", "off")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_listcomp_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 38 /* Execute Shw_Cont_F */),
                new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Company", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_deltlistcomp_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 38 /* Execute Shw_Cont_F */),
                new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Company", 
                        new XAttribute("onoftag", "off")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_listdeals_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 43 /* Execute Shw_Deal_F */),
                new Job(SendType.SelfToUserInterface, "SHW_DEAL_F", 10 /* Execute Actn_CalF_P */)                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_taskappointment_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 45 /* Execute Tol_Tsap_F */),
                new Job(SendType.SelfToUserInterface, "TOL_TSAP_F", 10 /* Execute Actn_CalF_P */)                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Notification_Butn_Click(object sender, EventArgs e)
      {
         Notification_Butn.ImageProfile = Properties.Resources.IMAGE_1508;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 21 /* Execute Notf_Totl_F */),
                new Job(SendType.SelfToUserInterface, "NOTF_TOTL_F", 10 /* Execute Actn_CalF_P */)                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void iTask_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 21 /* Execute Notf_Totl_F */),
                   new Job(SendType.SelfToUserInterface, "NOTF_TOTL_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Notification", new XAttribute("type", "iTask"))}
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void User_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 29 /* Execute DoWork4ShowUserProfile */, SendType.Self));
      }

      private void FeedBack_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 32 /* Execute DoWork4SendFeedBack */, SendType.Self)
         {
            Input = 
               new XElement("SendEmail",
                  new XAttribute("username", CurrentUser),
                  new XAttribute("type", "feedback"),
                  new XAttribute("subsys", "11")
               )
         });
      }

      private void rb_AddleadsButn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {
                new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),
                new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Lead", new XAttribute("srpbtype", "001"))},
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_AddContactButn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),                
                new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Lead", new XAttribute("srpbtype", "002"))},
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void rb_AddAccountButn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 47 /* Execute Regn_Dfin_F */),                
                new Job(SendType.SelfToUserInterface, "COMP_CHNG_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Company", 
                        new XAttribute("formtype", "add"),
                        new XAttribute("code", 0),
                        new XAttribute("formcaller", GetType().Name)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
      #endregion

      #region Service Info Radial Menu
      private void HistLogc_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 61 /* Execute Hst_Logc_F */),
                  new Job(SendType.SelfToUserInterface, "HST_LOGC_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistMesg_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 62 /* Execute Hst_Mesg_F */),
                  new Job(SendType.SelfToUserInterface, "HST_MESG_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistEmal_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 63 /* Execute Hst_Emal_F */),
                  new Job(SendType.SelfToUserInterface, "HST_EMAL_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistTask_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Hst_Task_F */),
                  new Job(SendType.SelfToUserInterface, "HST_TASK_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistApon_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 65 /* Execute Hst_Apon_F */),
                  new Job(SendType.SelfToUserInterface, "HST_APON_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistSndf_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 66 /* Execute Hst_Sndf_F */),
                  new Job(SendType.SelfToUserInterface, "HST_SNDF_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistNote_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 67 /* Execute Hst_Note_F */),
                  new Job(SendType.SelfToUserInterface, "HST_NOTE_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistTag_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 68 /* Execute Opt_Tag_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_TAG_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void HistFinr_Butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         InfoServ_Rm.HidePopup();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 70 /* Execute Hst_Finr_F */),
                  new Job(SendType.SelfToUserInterface, "HST_FINR_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Service_Information",                              
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      #endregion      

      #region Region 1
      private void Mention_Butn_Click(object sender, EventArgs e)
      {
         Mention_Butn.ImageProfile = Properties.Resources.IMAGE_1595;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 73 /* Execute Mntn_Totl_F */),
                new Job(SendType.SelfToUserInterface, "MNTN_TOTL_F", 10 /* Execute Actn_CalF_P */)                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void oTask_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 21 /* Execute Notf_Totl_F */),
                   new Job(SendType.SelfToUserInterface, "NOTF_TOTL_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Notification", new XAttribute("type", "oTask"))}
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void Refreshing_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Tm_Refreshing_Tick(null, null);
         }
         catch { }
      }

      private void Tm_Refreshing_Tick(object sender, EventArgs e)
      {
         #region iTask Reminder
         var iTask =
            from jp in iCRM.Job_Personnels
            join rm in iCRM.Reminders on jp.CODE equals rm.TO_JOBP_CODE
            join r in iCRM.Requests on rm.RQST_RQID equals r.RQID
            join t in iCRM.Tasks on r.RQID equals t.RQRO_RQST_RQID
            where jp.USER_NAME == CurrentUser
               //&& rm.FROM_JOBP_CODE != jp.CODE
               && r.RQTP_CODE == "009"
               && (t.TASK_STAT == "001" || t.TASK_STAT == "002")
            select t;
         Lb_iTask.Text = iTask.Count().ToString();
         Lb_iTask.Visible = Lb_iTask.Text == "0" ? false : true;
         #endregion

         #region oTask Reminder
         var oTask =
            from jp in iCRM.Job_Personnels
            join rm in iCRM.Reminders on jp.CODE equals rm.FROM_JOBP_CODE
            join r in iCRM.Requests on rm.RQST_RQID equals r.RQID
            join t in iCRM.Tasks on r.RQID equals t.RQRO_RQST_RQID
            where jp.USER_NAME == CurrentUser
               && rm.TO_JOBP_CODE != jp.CODE
               && r.RQTP_CODE == "009"
               && (t.TASK_STAT == "001" || t.TASK_STAT == "002")
            select t;
         Lb_oTask.Text = oTask.Count().ToString();
         Lb_oTask.Visible = Lb_oTask.Text == "0" ? false : true;
         #endregion

         Tm_Refreshing.Interval = (int)(IntervalTime_Nud.Value * 1000 * 60);
         Refreshing_Butn.Tooltip = string.Format("بروزرسانی \n\r آخرین زمان بروزرسانی {0} انجام شده است", DateTime.Now.ToString("HH:mm:ss"));
      }

      private void Comp_Rtab_Select(object sender, EventArgs e)
      {
         rb_listcomp_Click(null, null);
      }

      private void Serv_Rtab_Select(object sender, EventArgs e)
      {
         rb_showleads_Click(null, null);
      }

      private void Cont_Rtab_Select(object sender, EventArgs e)
      {
         rb_showcontact_Click(null, null);
      }
      #endregion

      /// <summary>
      /// منوهای اصلی نرم افزار در پایین می باشد و مابقی بایستی حذف گردند
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      #region Supplier
      #endregion

      #region Sale
      private void SaleDashboard_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleWhatsNew_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleActivity_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleShowAcnt_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 38 /* Execute Shw_Cont_F */),
                new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Company", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void SaleShowCont_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 33 /* Execute Shw_Cont_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Service", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void SaleShowLead_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 23 /* Execute Shw_Lead_F */),
                new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Lead", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void SaleShowOptn_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 103 /* Execute Shw_Oprt_F */),
                new Job(SendType.SelfToUserInterface, "SHW_OPRT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Lead", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void SaleShowCmpt_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 92 /* Execute Shw_Cmpt_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CMPT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Competitor", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void SaleQuotes_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleOrder_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleInvoice_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleProduct_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleLiterature_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleMarketingList_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleQuickCampaign_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleGoal_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleGoalMetrics_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleRollupQuery_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleReport_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleAlert_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaleCalendar_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Service
      private void ServiceDashboard_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceActivity_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceShowAcnt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceShowCont_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceShowSocialProfile_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceCase_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceServiceCalendar_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceQueues_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceArticles_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ribbonButton36_Click(object sender, EventArgs e)
      {

      }

      private void ServiceProduct_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceService_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceGoal_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceGoalMetrics_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceRollupQuery_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceReport_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceAlert_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ServiceCalendar_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Marketing
      private void MarketingDashboard_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingActivity_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingShowAcnt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingShowCont_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingLead_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingList_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 94 /* Execute Shw_Mklt_F */),
                new Job(SendType.SelfToUserInterface, "SHW_MKLT_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Marketing_List", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void MarketingCampaign_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 96 /* Execute Shw_Camp_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CAMP_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Campaign", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void MarketingQuickCampaign_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 98 /* Execute Shw_Camq_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CAMQ_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Campaign_Quick", 
                        new XAttribute("onoftag", "on")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void MarketingProduct_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingSaleLiterature_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingReport_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingAlert_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MarketingCalendar_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Settings
      private void SettingBussinesManagement_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingTemplate_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingProductCatlog_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingServiceManagement_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingCustomization_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingSolution_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingDynamicMarketplace_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingPluginHistory_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingAdministration_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingSecurity_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingDataManagment_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingSystemJob_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingDocumentManagement_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingAuditing_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingEmailConfig_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingActivityFeedConfig_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingActivityFeedRole_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingSocialNetwork_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingProcess_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SettingInteractiveServiceHub_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Help
      private void HelpHelp_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpSaleVideoTrainging_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpApplyClassTraining_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpApplyRemoteTraining_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Utility
      private void UtilityNew_Butn_Click(object sender, EventArgs e)
      {

      }

      private void UtilityLastActivity_Butn_Click(object sender, EventArgs e)
      {

      }

      private void UtilityAdvanceFilter_Butn_Click(object sender, EventArgs e)
      {

      }

      private void UtilityOption_Butn_Click(object sender, EventArgs e)
      {

      }

      private void UtilitySearch_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion
   }
}
