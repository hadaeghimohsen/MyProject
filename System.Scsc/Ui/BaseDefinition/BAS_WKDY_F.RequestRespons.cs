using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.BaseDefinition
{
   partial class BAS_WKDY_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string CurrentUser;
      private string RegnLang = "054";


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

         if (keyData == Keys.F1)
         {

         }
         else if (keyData == Keys.Escape)
         {
            job.Next = new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
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

         var GetUserAccount =
            new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);

         _DefaultGateway.Gateway(
            GetUserAccount
         );
         CurrentUser = GetUserAccount.Output.ToString();

         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());


         #region Set Localization
         var regnlang = iScsc.V_User_Localization_Forms.Where(rl => rl.FORM_NAME == GetType().Name);
         if (regnlang.Count() > 0 && regnlang.First().REGN_LANG != RegnLang)
         {
            RegnLang = regnlang.First().REGN_LANG;
            // Ready To Change Text Title
            foreach (var control in regnlang)
            {
               switch (control.CNTL_NAME.ToLower())
               {
                  case "mtod_lb":
                     Mtod_Lb.Text = control.LABL_TEXT;
                     //Mtod_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Mtod_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "wkdy003_butn":
                     Wkdy003_Butn.Text = control.LABL_TEXT;
                     //Wkdy003_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Wkdy003_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cmwdtitl_lb":
                     CmwdTitl_Lb.Text = control.LABL_TEXT;
                     //CmwdTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CmwdTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "desc1_lb":
                     Desc1_Lb.Text = control.LABL_TEXT;
                     //Desc1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Desc1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochname_lb":
                     CochName_Lb.Text = control.LABL_TEXT;
                     //CochName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "strttime_lb":
                     StrtTime_Lb.Text = control.LABL_TEXT;
                     //StrtTime_Lb.Text = control.LABL_TEXT; // ToolTip
                     //StrtTime_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "endtime_lb":
                     EndTime_Lb.Text = control.LABL_TEXT;
                     //EndTime_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EndTime_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype_lb":
                     SexType_Lb.Text = control.LABL_TEXT;
                     //SexType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SexType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "daytype_lb":
                     DayType_Lb.Text = control.LABL_TEXT;
                     //DayType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DayType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "selectday_butn":
                     SelectDay_Butn.Text = control.LABL_TEXT;
                     //SelectDay_Butn.Text = control.LABL_TEXT; // ToolTip
                     //SelectDay_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "wkdy007_butn":
                     Wkdy007_Butn.Text = control.LABL_TEXT;
                     //Wkdy007_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Wkdy007_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "wkdy001_butn":
                     Wkdy001_Butn.Text = control.LABL_TEXT;
                     //Wkdy001_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Wkdy001_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "wkdy002_butn":
                     Wkdy002_Butn.Text = control.LABL_TEXT;
                     //Wkdy002_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Wkdy002_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "wkdy004_butn":
                     Wkdy004_Butn.Text = control.LABL_TEXT;
                     //Wkdy004_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Wkdy004_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "wkdy005_butn":
                     Wkdy005_Butn.Text = control.LABL_TEXT;
                     //Wkdy005_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Wkdy005_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "wkdy006_butn":
                     Wkdy006_Butn.Text = control.LABL_TEXT;
                     //Wkdy006_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Wkdy006_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "submitchange_butn":
                     SubmitChange_Butn.Text = control.LABL_TEXT;
                     //SubmitChange_Butn.Text = control.LABL_TEXT; // ToolTip
                     //SubmitChange_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
               }
            }
         }
         #endregion

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
         //      new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {  GetType().Name, this }  },
         //      new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */){ Input = new List<object> { this, "cntrhrz:normal" } }               
         //   });
         //_DefaultGateway.Gateway(_Paint);

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
         //job.Next =
         //   new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
         //      new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
         //         new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

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
                           Input = new List<string> {"<Privilege>34</Privilege><Sub_Sys>0</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 34 امنیتی", "خطا دسترسی");
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
         DSxtpBs1.DataSource = iScsc.D_SXTPs;
         DdytpBs1.DataSource = iScsc.D_DYTPs;         

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
            code = Convert.ToInt64(xinput.Attribute("code").Value);
            //if(xinput.Attribute("showonly") !=null)
            //{
            //   SubmitChange_Butn.Visible = true;
            //   if(xinput.Attribute("showonly").Value == "002")
            //      SubmitChange_Butn.Visible = false;
            //}
            //else
            //   SubmitChange_Butn.Visible = true;
         }
         Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
