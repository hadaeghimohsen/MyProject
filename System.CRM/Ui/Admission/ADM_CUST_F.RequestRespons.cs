using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.CRM.Ui.Admission
{
   partial class ADM_CUST_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iCRMDataContext iCRM;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uagc_U;
      private string CurrentUser = "";

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
            RqstBs1.AddNew();
         }
         else if(keyData == (Keys.Control | Keys.Delete))
         {
            RqstBnDelete1_Click(null, null);
         }
         else if(keyData == Keys.F5)
         {
            RqstBnARqt1_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.P))
         {
            RqstBnDefaultPrint1_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.Shift | Keys.P))
         {
            RqstBnPrint1_Click(null, null);
         }
         else if(keyData == Keys.F10 && RqstBnASav1.Enabled)
         {
            RqstBnASav1_Click(null, null);
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
               case "LST_SERV_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 57 /* Execute Lst_Serv_F */),
                           new Job(SendType.SelfToUserInterface, "LST_SERV_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input = 
                                 new XElement("Request", 
                                    new XAttribute("fileno", ""),
                                    new XAttribute("rqid", xinput.Attribute("rqstrqid").Value),
                                    new XAttribute("formcaller", xinput.Attribute("formcaller").Value)
                                 )
                           }                     
                        }
                     )
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
         Fga_Uprv_U = iCRM.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iCRM.FGA_URGN_U() ?? "";
         Fga_Uagc_U = (iCRM.FGA_UAGC_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
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
         if(!isFirstLoaded)
         {
            iCRM = new Data.iCRMDataContext(ConnectionString);            
            //PrvnBs1.DataSource = iCRM.Provinces;
            CntyBs.DataSource = iCRM.Countries;
            OrgnBs1.DataSource = iCRM.Organs;
            DCyclBs1.DataSource = iCRM.D_CYCLs;
            RqtpBs1.DataSource = iCRM.Request_Types;
            DmrtpBs1.DataSource = iCRM.D_MRTPs;
            DrltpBs1.DataSource = iCRM.D_RLTPs;
            DcttpBs1.DataSource = iCRM.D_CTTPs;
            DectpBs1.DataSource = iCRM.D_ECTPs;
            IscgBs1.DataSource = iCRM.Isic_Groups;
            DjbtpBs1.DataSource = iCRM.D_JBTPs;
            DsrtpBs.DataSource = iCRM.D_SRTPs;
            DsstgBs.DataSource = iCRM.D_SSTGs;
            DsxtpBs.DataSource = iCRM.D_SXTPs;
            RqttBs1.DataSource = iCRM.Requester_Types;
            isFirstLoaded = true;
         }

         if (!reloading)
         {
            StatusSaving_Sic.StateIndex = 0;
            requery = true;
            Execute_Query();
            requery = false;
         }
         else
            reloading = false;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         xinput = job.Input as XElement;
         if(xinput != null)
         {
            SrpbType_Lov.SelectedValue = srpbtype = xinput.Attribute("srpbtype").Value;
            switch (srpbtype)
            {
               case "001":
                  tp_001.Text = "ثبت مشتری احتمالی";
                  break;
               case "002":
                  tp_001.Text = "اطلاعات شخص";
                  break;
               default:
                  break;
            }
            if (xinput.Attribute("formcaller") != null)
               formCaller = xinput.Attribute("formcaller").Value;

            if (xinput.Attribute("rqstrqid") != null)
               rqstrqid = Convert.ToInt64(xinput.Attribute("rqstrqid").Value);

            if (xinput.Attribute("cntycode") != null)
            {
               cntycode = xinput.Attribute("cntycode").Value;
               CntyCode_Lov.SelectedValue = cntycode;
            }

            if (xinput.Attribute("prvncode") != null)
            {
               prvncode = xinput.Attribute("prvncode").Value;
               PrvnCode_Lov.SelectedValue = prvncode;
            }

            if (xinput.Attribute("regncode") != null)
            {
               regncode = xinput.Attribute("regncode").Value;
               RegnCode_Lov.SelectedValue = regncode;
            }

            if (xinput.Attribute("compcode") != null)
            {
               compcode = Convert.ToInt64(xinput.Attribute("compcode").Value);
               CompCode_Lov.SelectedValue = compcode;
            }

            // 1396/07/24 * اگر فرم جدید باز شود اطلاعات اولیه به صورت اتوماتیک لود شود
            if (cntycode == null || prvncode == null || regncode == null)
            {
               var prsn = iCRM.Job_Personnels.FirstOrDefault(p => p.USER_NAME == CurrentUser).Service;
               cntycode = prsn.REGN_PRVN_CNTY_CODE;
               prvncode = prsn.REGN_PRVN_CODE;
               regncode = prsn.REGN_CODE;
            }
            if (compcode == 0)
            {
               compcode = iCRM.Companies.FirstOrDefault(c => c.REGN_PRVN_CNTY_CODE == cntycode && c.REGN_PRVN_CODE == prvncode && c.REGN_CODE == regncode && c.RECD_STAT == "002" && c.DFLT_STAT == "002").CODE;
            }

            if (cntycode != null && cntycode.Length != 1)
               CntyCode_Lov.SelectedValue = cntycode;

            if (prvncode != null && prvncode.Length != 1)
               PrvnCode_Lov.SelectedValue = prvncode;

            if (regncode != null && regncode.Length != 1)
               RegnCode_Lov.SelectedValue = regncode;

            if (compcode != 0)
               CompCode_Lov.SelectedValue = compcode;
         }
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
            var srpb = SrpbBs1.Current as Data.Service_Public;
            if (xinput.Attribute("outputtype").Value == "servcord")
            {
               var cordx = Convert.ToDouble(xinput.Attribute("cordx").Value);
               var cordy = Convert.ToDouble(xinput.Attribute("cordy").Value);

               if (cordx != srpb.CORD_X && cordy != srpb.CORD_Y)
               {
                  // Call Update Service_Public
                  try
                  {
                     srpb.CORD_X = cordx;
                     srpb.CORD_Y = cordy;
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     MessageBox.Show(exc.Message);
                  }
               }
            }
         }

         job.Status = StatusType.Successful;
      }

   }
}
