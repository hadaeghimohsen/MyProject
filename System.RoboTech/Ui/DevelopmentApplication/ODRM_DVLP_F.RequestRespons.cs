using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   partial class ODRM_DVLP_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iRoboTechDataContext iRoboTech;
      private string ConnectionString;
      private List<long?> Fga_Ugov_U;

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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iRoboTech</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );

         ConnectionString = GetConnectionString.Output.ToString();
         iRoboTech = new Data.iRoboTechDataContext(GetConnectionString.Output.ToString());

         Fga_Ugov_U = (iRoboTech.FGA_UGOV_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         //var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         //_DefaultGateway.Gateway(GetHostInfo);

         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         //);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         //Job _Paint = new Job(SendType.External, "Desktop",
         //   new List<Job>
         //   {
         //      //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("RoboTech:{0}", this.GetType().Name), this }  },
         //      new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PastOnWall */) { Input = this }               
         //   });
         //_DefaultGateway.Gateway(_Paint);
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("RoboTech:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "right:in-screen:stretch:center"} }               
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
         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
         //         new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
         //         //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
         //      })
         //   );
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
               new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

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
                           Input = new List<string> {"<Privilege>73</Privilege><Sub_Sys>12</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 73 امنیتی", "خطا دسترسی");
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
         DelmtBs.DataSource = iRoboTech.D_ELMTs;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if(xinput != null)
         {
            srrmRwno = 0;
            servFileNo = 0;
            roboRbid = 0;
            ordtOrdrCode = 0;
            ordtRwno = 0;
            srmgrwno = 0;

            if (xinput.Attribute("srrmrwno") != null)
               srrmRwno = Convert.ToInt64(xinput.Attribute("srrmrwno").Value);
            
            // New Code
            if (xinput.Attribute("srbtmsg") != null)
               srmgrwno = Convert.ToInt64(xinput.Attribute("srbtmsg").Value);
            
            if (xinput.Attribute("servfileno") != null)
               servFileNo = Convert.ToInt64(xinput.Attribute("servfileno").Value);
            
            if (xinput.Attribute("roborbid") != null)
               roboRbid = Convert.ToInt64(xinput.Attribute("roborbid").Value);

            if (xinput.Attribute("ordtordrcode") != null)
               ordtOrdrCode = Convert.ToInt64(xinput.Attribute("ordtordrcode").Value);

            if (xinput.Attribute("ordtrwno") != null)
               ordtRwno = Convert.ToInt64(xinput.Attribute("ordtrwno").Value);

            if (ordtOrdrCode != 0 && srrmRwno == 0)
            {
               var query =                
                  iRoboTech.Service_Robot_Replay_Messages
                  .FirstOrDefault(m =>
                     m.ORDT_ORDR_CODE == ordtOrdrCode &&
                     m.ORDT_RWNO == ordtRwno &&
                     m.RWNO ==
                        iRoboTech.Service_Robot_Replay_Messages
                        .Where(rm =>
                           rm.ORDT_ORDR_CODE == ordtOrdrCode &&
                           rm.ORDT_RWNO == ordtRwno)
                        .Max(rm => rm.RWNO));

               if (query != null)
                  srrmRwno = query.RWNO;
            }

            SrrmBs.List.Clear();
            if(xinput.Attribute("type").Value == "new")
            {
               SrrmBs.AddNew();
               var srrm = SrrmBs.Current as Data.Service_Robot_Replay_Message;
               if(servFileNo != 0)
               {
                  srrm.SRBT_SERV_FILE_NO = servFileNo;
                  srrm.SRBT_ROBO_RBID = roboRbid;
               }

               if(ordtOrdrCode != 0)
               {
                  srrm.ORDT_ORDR_CODE = ordtOrdrCode;
                  srrm.ORDT_RWNO = ordtRwno;
                  var ordt = iRoboTech.Order_Details.FirstOrDefault(od => od.ORDR_CODE == ordtOrdrCode && od.RWNO == ordtRwno);
                  servFileNo = srrm.SRBT_SERV_FILE_NO = ordt.Order.SRBT_SERV_FILE_NO;
                  roboRbid = srrm.SRBT_ROBO_RBID = ordt.Order.SRBT_ROBO_RBID;
               }
            }
            else
            {
               SrrmBs.DataSource = iRoboTech.Service_Robot_Replay_Messages.FirstOrDefault(m => m.RWNO == srrmRwno);
            }

            //Execute_Query();
         }
         job.Status = StatusType.Successful;
      }

   }
}
