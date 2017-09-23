using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Telerik.WinControls.UI;

namespace System.Scsc.Ui.MasterPage
{
   partial class FRST_PAGE_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private List<long?> Fga_Uclb_U;
      private string Crnt_User;

      //private bool requery = default(bool);

      private XElement x = null;
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
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               PostOnWall(job);
               break;
            case 09:
               TakeOnWall(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 40:
               SetToolTip(job);
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

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if(keyData == Keys.F9)
         {
            var fngrprnt = Microsoft.VisualBasic.Interaction.InputBox("EnrollNumber", "Input EnrollNumber");

            axCZKEM1_OnAttTransactionEx(fngrprnt.ToString(), 1, 1, 1, 2016, 05, 10, 09, 31, 50, 20);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
         Lbs_CrntUser.Text = Crnt_User = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         #region Package Item
         rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self) ;
         _DefaultGateway.Gateway( GetHostInfo );

         var Pkac = iScsc.VF_AccessPackage(GetHostInfo.Output as XElement).ToList();

         rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null || Pkac.Any(i => i.RWNO == Convert.ToInt32(rbe.Tag)) ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         /* Initial Sp_Barcode For Running */
         ///Start_BarCode();
         /* Initial Sp_FingerPrint For Running */
         //Start_FingerPrint();         

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
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }               
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
         //Stop_BarCode();
         //Stop_FingerPrint();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {  
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void PostOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;

            if(InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Dock = DockStyle.Fill;
                  c.Visible = true;
                  spc_desktop.Panel2.Controls.Add(c);
                  spc_desktop.Panel2.Controls.SetChildIndex(c, 0);
               }), obj);
            else
            {
               obj.Dock = DockStyle.Fill;
               obj.Visible = true;
               spc_desktop.Panel2.Controls.Add(obj);
               spc_desktop.Panel2.Controls.SetChildIndex(obj, 0);
            }

         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void TakeOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => spc_desktop.Panel2.Controls.Remove(c)), obj);
            this.Focus();
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Successful; UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => spc_desktop.Panel2.Controls.Remove(c)), obj);
            this.Focus();
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         if (iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.NOTF_STAT == "002" && (s.NOTF_VIST_DATE.HasValue ? s.NOTF_VIST_DATE.Value : DateTime.Now.AddDays(-1)) != DateTime.Now).Count() >= 1)
         {
            var expday = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.NOTF_STAT == "002" && (s.NOTF_VIST_DATE.HasValue ? s.NOTF_VIST_DATE.Value : DateTime.Now.AddDays(-1)) != DateTime.Now).ToList();
            expday.ForEach(s => s.NOTF_VIST_DATE = DateTime.Now);
            iScsc.SubmitChanges();

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endfigh"), new XAttribute("expday", expday.Max(s => s.NOTF_EXP_DAY)))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void SetToolTip(Job job)
      {
         Lbs_Tooltip.Text = job.Input.ToString();
         job.Status = StatusType.Successful;
      }
   }
}
