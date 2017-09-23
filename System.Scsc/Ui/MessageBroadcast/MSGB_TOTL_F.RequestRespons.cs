using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.MessageBroadcast
{
   partial class MSGB_TOTL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private bool isFirstLoaded = false;

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
            case 08:
               LoadDataSource(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            //case 11:
            //   ValidatePhoneNumber(job);
            //   break;
            default:
               break;
         }
      }

      //private void ValidatePhoneNumber(Job job)
      //{
      //  try
      //  {
      //     if (string.IsNullOrEmpty(cLUB_NAMETextEdit.Text))
      //          return;
      //     var r = new Regex(@"^09\d{2}\s*?\d{3}\s*?\d{4}$");
      //      MessageBox.Show( r.IsMatch(cLUB_NAMETextEdit.Text).ToString());
      //  }
      //  catch (Exception)
      //  {
      //      throw;
      //  }
      //}

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
         }
         //else if (keyData == Keys.F11)
         //{
         //   job.Next =
         //      new Job(SendType.SelfToUserInterface, this.GetType().Name, 11 /* Execute ValidatePhoneNumber */);
         //}
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

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
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
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
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
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
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>190</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              job.Status = StatusType.Successful;
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به کد 190");
                              #endregion                           
                           })
                        },
                        #endregion
                     })
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);         
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if (!isFirstLoaded)
         {
            MsgbBs1.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "001");
            MsgbBs2.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "003");
            MsgbBs3.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "004");
            MsgbBs4.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "002");
            MsgbBs5.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "005");
            DActvBs1.DataSource = iScsc.D_ACTVs;
            DYsnoBs1.DataSource = iScsc.D_YSNOs;
            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DLntpBs.DataSource = iScsc.D_LNTPs;
            ClubBs1.DataSource = iScsc.Clubs;
            CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);               
            CochBs1.DataSource = iScsc.Fighters.Where(c => c.CONF_STAT == "002" && (c.FGPB_TYPE_DNRM == "002" || c.FGPB_TYPE_DNRM == "003"));
            //FighBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && !(f.FGPB_TYPE_DNRM == "002" || f.FGPB_TYPE_DNRM == "003") && Convert.ToInt32(f.ACTV_TAG_DNRM) >= 101);
            MtodBs1.DataSource = iScsc.Methods;
            isFirstLoaded = true;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         job.Status = StatusType.Successful;
      }
   }
}
