using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.CRM.Ui.Leads
{
   partial class INF_LEAD_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iCRMDataContext iCRM;
      private string ConnectionString;
      private string CurrentUser;
      private long fileno;

      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
               CheckSecurity(job);
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 40:
               CordinateGetSet(job);
               break;
            case 150:
               SetMentioned(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iCRM</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );

         ConnectionString = GetConnectionString.Output.ToString();
         iCRM = new Data.iCRMDataContext(GetConnectionString.Output.ToString());

         CurrentUser = iCRM.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));
         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("CRM:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PastOnWall */) { Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         splitContainerControl2.Width = panel6.Width;

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
         Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>67</Privilege><Sub_Sys>11</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 67 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithJob);
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if (InvokeRequired)
         {
            Invoke(
               new Action(() =>
               {
                  TrcbBs.DataSource = iCRM.Transaction_Currency_Bases;
                  JobpBs.DataSource = iCRM.Job_Personnels.Where(o => o.STAT == "002");

                  DtrgtBs.DataSource = iCRM.D_TRGTs;
                  DstdyBs.DataSource = iCRM.D_STDies;
                  DysnoBs.DataSource = iCRM.D_YSNOs;
                  DcmstBs.DataSource = iCRM.D_CMSTs;
                  DcntpBs.DataSource = iCRM.D_CNTPs;

                  LstCampBs.DataSource = iCRM.Campaigns;
                  LstCompBs.DataSource = iCRM.Companies.Where(c => c.RECD_STAT == "002");
                  LstServBs.DataSource = iCRM.Services.Where(s => s.CONF_STAT == "002");
                  LstLeadBs.DataSource = iCRM.Leads;
               })
            );
         }
         else
         {
            TrcbBs.DataSource = iCRM.Transaction_Currency_Bases;
            JobpBs.DataSource = iCRM.Job_Personnels.Where(o => o.STAT == "002");

            DtrgtBs.DataSource = iCRM.D_TRGTs;
            DstdyBs.DataSource = iCRM.D_STDies;
            DysnoBs.DataSource = iCRM.D_YSNOs;
            DcmstBs.DataSource = iCRM.D_CMSTs;
            DcntpBs.DataSource = iCRM.D_CNTPs;

            LstCampBs.DataSource = iCRM.Campaigns;
            LstCompBs.DataSource = iCRM.Companies.Where(c => c.RECD_STAT == "002");
            LstServBs.DataSource = iCRM.Services.Where(s => s.CONF_STAT == "002");
            LstLeadBs.DataSource = iCRM.Leads;
         }
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         fileno = Convert.ToInt64(xinput.Attribute("fileno").Value);
         Execute_Query();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void CordinateGetSet(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput != null)
         {
            var serv = LstServBs.Current as Data.Service;
            if (xinput.Attribute("outputtype").Value == "servcord")
            {
               var cordx = Convert.ToDouble(xinput.Attribute("cordx").Value);
               var cordy = Convert.ToDouble(xinput.Attribute("cordy").Value);
               
               if(cordx != serv.CORD_X_DNRM && cordy != serv.CORD_Y_DNRM)
               {
                  // Call Update Service_Public
                  try
                  {
                     iCRM.CHNG_SRPB_P(
                        new XElement("Service",
                           new XAttribute("fileno", serv.FILE_NO),
                           new XAttribute("actntype", "001"),
                           new XAttribute("cordx", cordx),
                           new XAttribute("cordy", cordy)
                        )
                     );
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     MessageBox.Show(exc.Message);                     
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

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 150
      /// </summary>
      /// <param name="job"></param>
      private void SetMentioned(Job job)
      {
         var xinput = job.Input as XElement;
         
         switch(xinput.Attribute("section").Value)
         {
            case "note":
               NoteComment_Txt.Text = NoteComment_Txt.Text.Insert(NoteComment_Txt.SelectionStart, xinput.Attribute("user_mentioned").Value);
               break;
         }
      }
   }
}


