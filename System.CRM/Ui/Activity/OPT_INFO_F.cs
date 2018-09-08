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
   public partial class OPT_INFO_F : UserControl
   {
      public OPT_INFO_F()
      {
         InitializeComponent();
      }

      private long? fileno, compcode;
      private XElement xinput;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Info_Butn_Click(object sender, EventArgs e)
      {
         var infocode = (sender as RoundedButton).Tag.ToString();

         FormCaller = "";
         Btn_Back_Click(null, null);

         if(infocode == "001")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 50 /* Execute Tsk_Tag_F */),
                     new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 10 /* Execute Actn_CalF_P */) 
                     {
                        Input = 
                           new XElement("Service",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("fileno", fileno),
                              new XAttribute("compcode", compcode)
                           )
                     }
                  }
               )
            );
         }
         else if(infocode == "002")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 71 /* Execute Add_Info_F */),
                     new Job(SendType.SelfToUserInterface, "ADD_INFO_F", 10 /* Execute Actn_CalF_P */) 
                     {
                        Input = 
                           new XElement("Service",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("fileno", fileno),
                              new XAttribute("compcode", compcode)
                           )
                     }
                  }
               )
            );
         }
         else if(infocode == "003")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 60 /* Execute Inf_Serv_F */),
                     new Job(SendType.SelfToUserInterface, "INF_CTWK_F", 10 /* Execute Actn_CalF_P */) 
                     {
                        Input = 
                           new XElement("Service",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("fileno", fileno),
                              new XAttribute("compcode", compcode)
                           )
                     }
                  }
               )
            );
         }
         else if(infocode == "004")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute RLAT_SINF_F */),
                     new Job(SendType.SelfToUserInterface, "RLAT_SINF_F", 10 /* Execute Actn_CalF_P */) 
                     {
                        Input = 
                           new XElement("Service",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("fileno", fileno),
                              new XAttribute("compcode", compcode)
                           )
                     }
                  }
               )
            );
         }
         else if(infocode == "005")
         {
            _DefaultGateway.Gateway(
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),
                   new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute Actn_Calf_F */)
                   {
                      Input = 
                        new XElement("Lead",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("type", fileno != 0 ? "servicelead" : "companylead"),
                           new XAttribute("fileno", fileno),
                           new XAttribute("compcode", compcode)
                        )
                   }
                 }
               )
            );
         }
      }
   }
}
