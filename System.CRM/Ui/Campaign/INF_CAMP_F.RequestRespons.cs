using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.CRM.Ui.Campaign
{
   partial class INF_CAMP_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iCRMDataContext iCRM;
      private string ConnectionString;
      private string CurrentUser = "";

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

         if (keyData == (Keys.Control | Keys.Insert))
         {
         }
         else if(keyData == (Keys.Control | Keys.Delete))
         {
         }
         else if(keyData == Keys.F5)
         {
         }
         else if(keyData == (Keys.Control | Keys.P))
         {
            RqstBnDefaultPrint1_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.Shift | Keys.P))
         {
            RqstBnPrint1_Click(null, null);
         }
         else if(keyData == Keys.F10 && SubmitChangeClose_Butn.Enabled)
         {
         }
         else if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.Escape)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 04 /* Execute UnPaint */, SendType.SelfToUserInterface)
            );

            switch (formCaller)
            {
               case "SHW_CAMP_F":                  
                  _DefaultGateway.Gateway(
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
                       })
                  );
                  break;
            }
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

         //var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         //_DefaultGateway.Gateway(GetHostInfo);

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
                           Input = new List<string> {"<Privilege>58</Privilege><Sub_Sys>11</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 58 امنیتی", "خطا دسترسی");
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
            Invoke(new Action(() =>
               {
                  LstMkltBs.DataSource = iCRM.Marketing_Lists;
                  TrcbBs.DataSource = iCRM.Transaction_Currency_Bases;
                  JobpBs.DataSource = iCRM.Job_Personnels.Where(o => o.STAT == "002");

                  DcmstBs.DataSource = iCRM.D_CMSTs;
                  DcamtBs.DataSource = iCRM.D_CAMTs;
                  DysnoBs.DataSource = iCRM.D_YSNOs;
               }));            
         }
         else
         {
            LstMkltBs.DataSource = iCRM.Marketing_Lists;
            TrcbBs.DataSource = iCRM.Transaction_Currency_Bases;
            JobpBs.DataSource = iCRM.Job_Personnels.Where(o => o.STAT == "002");

            DcmstBs.DataSource = iCRM.D_CMSTs;
            DcamtBs.DataSource = iCRM.D_CAMTs;
            DysnoBs.DataSource = iCRM.D_YSNOs;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         xinput = job.Input as XElement;
         if (xinput != null)
         {            
            if (xinput.Attribute("formcaller") != null)
               formCaller = xinput.Attribute("formcaller").Value;

            if (xinput.Attribute("campcode") != null)
               campcode = Convert.ToInt64(xinput.Attribute("campcode").Value);
            else
               campcode = null;

            if (xinput.Attribute("mkltcode") != null)
               mkltcode = Convert.ToInt64(xinput.Attribute("mkltcode").Value);
            else
               mkltcode = null;

         }
         if (InvokeRequired)
            Invoke(new Action(() => Execute_Query()));
         else
            Execute_Query();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void CordinateGetSet(Job job)
      {
         //var xinput = job.Input as XElement;
         //if (xinput != null)
         //{
         //   var srpb = SrpbBs1.Current as Data.Service_Public;
         //   if (xinput.Attribute("outputtype").Value == "servcord")
         //   {
         //      var cordx = Convert.ToDouble(xinput.Attribute("cordx").Value);
         //      var cordy = Convert.ToDouble(xinput.Attribute("cordy").Value);

         //      if (cordx != srpb.CORD_X && cordy != srpb.CORD_Y)
         //      {
         //         // Call Update Service_Public
         //         try
         //         {
         //            srpb.CORD_X = cordx;
         //            srpb.CORD_Y = cordy;
         //            requery = true;
         //         }
         //         catch (Exception exc)
         //         {
         //            MessageBox.Show(exc.Message);
         //         }
         //      }
         //   }
         //}

         job.Status = StatusType.Successful;
      }

   }
}
