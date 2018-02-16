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

namespace System.CRM.Ui.PublicInformation
{
   public partial class LST_SERV_F : UserControl
   {
      public LST_SERV_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long projrqstrqid;
      private XElement xinput;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => s.CONF_STAT == "002" && Convert.ToInt32(s.ONOF_TAG_DNRM) >= 101);
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void ServBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;

            ServiceProfile_Pb.ImageVisiable = true;

            try
            {
               ServiceProfile_Pb.ImageVisiable = true;
               ServiceProfile_Pb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", serv.FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               ServiceProfile_Pb.ImageProfile = bm;
            }
            catch
            {
               ServiceProfile_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
            }
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
      }

      private void ServiceProfile_Pb_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            FormCaller = "";
            Btn_Back_Click(null, null);
            FormCaller = xinput.Attribute("formcaller").Value;

            if(xinput != null)
            {
               /*
                * <Request fileno="?" rqid="123" />
                */
               // Update FileNo in Xml variable               
               xinput.Attribute("fileno").Value = serv.FILE_NO.ToString();
               xinput.Attribute("formcaller").Value = GetType().Name;
               xinput.Attribute("projrqstrqid").Value = projrqstrqid.ToString(); ;
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 56 /* Execute Opt_Rqst_F */),
                        new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 10 /* Execute Actn_Calf_F */)
                        {
                           Input = xinput                                 
                        }                     
                     }
                  )
               );
            }
            else
            {
               //serv = ServBs.Current as Data.Service;
               switch(serv.SRPB_TYPE_DNRM)
               {
                  case "001":
                     _DefaultGateway.Gateway(
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                          })
                     );
                     break;
                  case "002":
                     _DefaultGateway.Gateway(
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                          })
                     );
                     break;
               }
            }            
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }         
      }

      private void CreateNewService_Butn_Click(object sender, EventArgs e)
      {
         FormCaller = "";
         Btn_Back_Click(null, null);

         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.Self, 58 /* Execute Add_Serv_F */),
         //         new Job(SendType.SelfToUserInterface, "ADD_SERV_F", 10 /* Execute Actn_CalF_P */)
         //         {
         //            Input = new XElement("Request", new XAttribute("rqid", xinput.Attribute("rqid").Value), new XAttribute("formcaller", GetType().Name))
         //         }
         //      }
         //   )
         //);
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),                
                new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Lead", new XAttribute("srpbtype", "002"), new XAttribute("formcaller", GetType().Name), new XAttribute("rqstrqid", xinput.Attribute("rqid").Value))},
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
   }
}
