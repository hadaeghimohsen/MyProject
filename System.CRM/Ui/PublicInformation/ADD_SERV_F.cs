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

namespace System.CRM.Ui.PublicInformation
{
   public partial class ADD_SERV_F : UserControl
   {
      public ADD_SERV_F()
      {
         InitializeComponent();
      }

      private XElement xinput;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void CreateNewService_Butn_Click(object sender, EventArgs e)
      {
         formCaller = "";
         Btn_Back_Click(null, null);

         var butn = sender as RoundedButton;

         if (xinput.Attribute("formcaller").Value == "LST_SERV_F")
         {
            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                 
                    new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),                
                    new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("srpbtype", butn.Tag.ToString()), new XAttribute("formcaller", "LST_SERV_F"), new XAttribute("rqstrqid", xinput.Attribute("rqid").Value))},
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         else
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                 
                    new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),                
                    new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("srpbtype", butn.Tag.ToString()), new XAttribute("formcaller", xinput.Attribute("formcaller").Value), new XAttribute("compcode", xinput.Attribute("compcode").Value), new XAttribute("regncode", xinput.Attribute("regncode").Value), new XAttribute("prvncode", xinput.Attribute("prvncode").Value), new XAttribute("cntycode", xinput.Attribute("cntycode").Value))},
                 })
            );
         }
      }
   }
}
