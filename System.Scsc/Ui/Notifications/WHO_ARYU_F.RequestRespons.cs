using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Notifications
{
   partial class WHO_ARYU_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      //private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      int Sleeping = 1;
      int step = 15;
      bool isPainted = false;
      string fileno;
      long? attncode = 0;
      bool gateControl = false;
      private short? mbsprwno;
      private string formcaller = "";
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
               OpenDrawer(job);
               break;
            case 06:
               CloseDrawer(job);
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

         if (keyData == Keys.Escape)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
                  })
            );

            switch (formcaller)
            {               
               case "ATTN_DAYN_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 141 /* Execute Attn_Dayn_F */),
                           //new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )
                        })
                  );
                  break;
               default:
                  break;
            }

            //job.Next =
            //   new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
            //if (!(Btn_Search.Focused))
            //   SendKeys.Send("{TAB}");
         }
         else if(keyData == Keys.F5)
         {
            Butn_Zoom_Click(null, null);
         }
         else if(keyData == Keys.F8)
         {
            Butn_TakePicture_Click(null, null);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         //Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         //Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void OpenDrawer(Job job)
      {
         int width = 0;

         for (; ; )
         {
            if (width + step <= Width)
            {
               Invoke(new Action(() =>
               {
                  Left += step;
                  width += step;
               }));
               Thread.Sleep(Sleeping);
            }
            else
               break;
         }
         var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
         if(attnNotfSetting.ATTN_NOTF_STAT == "002" && attnNotfSetting.ATTN_NOTF_CLOS_TYPE == "002")
         {
            Thread.Sleep((int)attnNotfSetting.ATTN_NOTF_CLOS_INTR);
            if(isPainted)
               RqstBnExit1_Click(null, null);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void CloseDrawer(Job job)
      {
         int width = Width;

         for (; ; )
         {
            if (width - step >= 0)
            {
               Invoke(new Action(() =>
               {
                  Left -= step;
                  width -= step;
               }));
               Thread.Sleep(Sleeping);
            }
            else
               break;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         isPainted = true;
         //Job _Paint = new Job(SendType.External, "Desktop",
         //   new List<Job>
         //   {
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
         //      new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
         //   });
         //_DefaultGateway.Gateway(_Paint);

         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}",GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "left:in-screen:default:center"} }               
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
         isPainted = false;
         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
         //         new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
         //         new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
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
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if (isFirstLoaded) goto finishcommand;
         isFirstLoaded = true;

         finishcommand:
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cm => cm.MTOD_STAT == "002");

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;

            Lbl_Dresser.BackColor = SystemColors.Control;
            fileno = xinput.Attribute("fileno").Value;
            AttnDate_Date.Value = Convert.ToDateTime(xinput.Attribute("attndate").Value);
            // 1396/07/16 * اضافه شدن 
            if (xinput.Attribute("attncode") != null)
               attncode = Convert.ToInt64(xinput.Attribute("attncode").Value);
            else
               attncode = null;

            if (xinput.Attribute("mbsprwno") != null)
               mbsprwno = Convert.ToInt16(xinput.Attribute("mbsprwno").Value);
            else
               mbsprwno = null;

            if (xinput.Attribute("formcaller") != null)
               formcaller = xinput.Attribute("formcaller").Value;
            else
               formcaller = "";

            if (xinput.Attribute("gatecontrol") != null)
               gateControl = true;
            else
               gateControl = false;
         }
         catch { }
         finally
         {
            Execute_Query(true);
         }
         job.Status = StatusType.Successful;
      }
   }
}
