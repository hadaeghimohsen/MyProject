using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.BaseDefinition
{
   partial class BAS_ADCL_F : ISendRequest
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
                  case "cordx_lb":
                     CordX_Lb.Text = control.LABL_TEXT;
                     //CordX_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CordX_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "title_lbl":
                     Title_Lbl.Text = control.LABL_TEXT;
                     //Title_Lbl.Text = control.LABL_TEXT; // ToolTip
                     //Title_Lbl.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "desc1_lb":
                     Desc1_Lb.Text = control.LABL_TEXT;
                     //Desc1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Desc1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "name_lb":
                     Name_Lb.Text = control.LABL_TEXT;
                     //Name_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Name_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "desc2_lb":
                     Desc2_Lb.Text = control.LABL_TEXT;
                     //Desc2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Desc2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "regncode_lb":
                     RegnCode_Lb.Text = control.LABL_TEXT;
                     //RegnCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RegnCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "desc3_lb":
                     Desc3_Lb.Text = control.LABL_TEXT;
                     //Desc3_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Desc3_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cordy_lb":
                     CordY_Lb.Text = control.LABL_TEXT;
                     //CordY_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CordY_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "desc4_lb":
                     Desc4_Lb.Text = control.LABL_TEXT;
                     //Desc4_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Desc4_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "postadrs_lb":
                     PostAdrs_Lb.Text = control.LABL_TEXT;
                     //PostAdrs_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PostAdrs_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tellphon_lb":
                     TellPhon_Lb.Text = control.LABL_TEXT;
                     //TellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon_lb":
                     CellPhon_Lb.Text = control.LABL_TEXT;
                     //CellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "website_lb":
                     WebSite_Lb.Text = control.LABL_TEXT;
                     //WebSite_Lb.Text = control.LABL_TEXT; // ToolTip
                     //WebSite_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "emaladrs_lb":
                     EmalAdrs_Lb.Text = control.LABL_TEXT;
                     //EmalAdrs_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EmalAdrs_Lb.Text = control.LABL_TEXT; // Place Holder
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
         ClubName_Text.Text =CordX_Text.Text = CordY_Text.Text = PostAddress_Text.Text = CellPhon_Text.Text = TellPhon_Text.Text = WebSite_Text.Text = EmailAddress_Text.Text = "";
         Regn_Lov.EditValue = null;
         
         RegnBs1.DataSource = iScsc.Regions;
         ClubBs.List.Clear();
         ClubBs.AddNew();

         Title_Lbl.Text = "اضافه کردن شیفت جدید";
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         if(job.Input != null)
         {
            //ClubBs.List.Clear();
            ClubBs.DataSource = iScsc.Clubs.FirstOrDefault(c => c == (job.Input as Data.Club));
            Title_Lbl.Text = "ویرایش کردن شیفت";
         }
         job.Status = StatusType.Successful;
      }
   }
}
