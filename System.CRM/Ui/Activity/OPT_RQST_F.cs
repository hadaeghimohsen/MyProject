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
using System.MaxUi;

namespace System.CRM.Ui.Activity
{
   public partial class OPT_RQST_F : UserControl
   {
      public OPT_RQST_F()
      {
         InitializeComponent();
      }

      private long fileno, rqid;
      private XElement xinput;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void RqstType_Butn_Click(object sender, EventArgs e)
      {
         var rqtpcode = (sender as RoundedButton).Tag.ToString();

         FormCaller = "";
         Btn_Back_Click(null, null);

         if(rqtpcode == "005")
         {
            // تماس تلفنی
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                     {
                        Input = 
                           new XElement("Service", 
                              new XAttribute("fileno", fileno), 
                              new XAttribute("rqstrqid", rqid),
                              new XAttribute("lcid", 0),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     },
                  })
            );
         }
         else if(rqtpcode == "006")
         {
            // ارسال فایل
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 29 /* Execute Opt_Sndf_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_SNDF_F", 10 /* Execute ACTN_CALF_P */)
                     {
                        Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("sendfiletype", "new"),
                           new XAttribute("formcaller", GetType().Name)
                        )
                     },
                  })
            );
         }
         else if(rqtpcode == "007")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                     {
                        Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("appointmenttype", "new"),
                           new XAttribute("formcaller", GetType().Name)
                        )
                     },
                  })
            );
         }
         else if(rqtpcode == "008")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 75 /* Execute Opt_Note_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_NOTE_F", 10 /* Execute ACTN_CALF_P */)
                     {
                        Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("ntid", "0"),
                           new XAttribute("formcaller", GetType().Name)
                        )
                     },
                  })
            );
         }
         else if(rqtpcode == "009")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 26 /* Execute Opt_Logc_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                     {
                        Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("tasktype", "new"),
                           new XAttribute("formcaller", GetType().Name)
                        )
                     },
                  })
            );
         }
         else if(rqtpcode == "010")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("emid", 0),
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("toemail", iCRM.Services.FirstOrDefault(s => s.FILE_NO == fileno).EMAL_ADRS_DNRM ?? "")
                        )
                   },
                 })   
            );
         }
         else if(rqtpcode == "011")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 35 /* Execute Tol_Deal_F */),
                   new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("cashcode", 0), 
                           new XAttribute("rqid", 0), 
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 })
            );
         }
         else if(rqtpcode == "012")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 53 /* Execute Opt_Mesg_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_MESG_F", 10 /* Execute ACTN_CALF_P */)
                     {
                        Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("msid", 0), 
                           new XAttribute("cellphon", iCRM.Services.FirstOrDefault(s => s.FILE_NO == fileno).CELL_PHON_DNRM ?? ""),
                           new XAttribute("formcaller", GetType().Name)
                        )
                     },
                  })
            );
         }
      }
   }
}
