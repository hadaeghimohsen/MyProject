using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.CRM.Ui.Contacts
{
   partial class SHW_CONT_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iCRMDataContext iCRM;
      private string ConnectionString;
      private string CurrentUser;

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
            case 11:
               GetNewRecord(job);
               break;
            case 100:
               SetFilterOnQuery(job);
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
            ImageProfile_Butn_Click(null, null);
         }
         else if(keyData == (Keys.F11 | Keys.Control))
         {
            Execute_Query();
         }
         else if(keyData == Keys.F11)
         {
            FrstName_Txt.Focus();
            FrstName_Txt.Text = LastName_Txt.Text = CellPhon_Txt.Text = TellPhon_Txt.Text = NatlCode_Txt.Text = ServNo_Txt.Text = PostAddr_Txt.Text = CordX_Txt.Text = CordY_Txt.Text = Radius_Txt.Text = EmalAddr_Txt.Text = "";
            BothSex_Rb.Checked = true;
            ConfDate_Dat.Value = null;
            MainStat_Lov.EditValue = SubStat_Lov.EditValue = null;
            tc_master.SelectedTab = tp_002;
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
         //DsstgBs.DataSource = iCRM.D_SSTGs;
         //DslonBs.DataSource = iCRM.D_SLONs;         
         MsttBs.DataSource = iCRM.Main_States;
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
            if (xinput.Attribute("onoftag") != null)
               onoftag = xinput.Attribute("onoftag").Value;
            
            if (xinput.Attribute("compcode") != null)
               compcode = Convert.ToInt64(xinput.Attribute("compcode").Value);
            else
               compcode = 0;
         }

         var runqury = iCRM.Settings.FirstOrDefault(s => s.DFLT_STAT == "002").RUN_QURY;

         if (InvokeRequired)
         {
            Invoke(
               new Action(
                  () =>
                  {
                     if(runqury == "002")
                        Execute_Query();
                  }
               )
            );
         }
         else
         {
            if(runqury == "002")
               Execute_Query();
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void GetNewRecord(Job job)
      {
         var serv = ServBs.Current as Data.VF_ServicesResult;
         if (serv == null) return;

         var xinput = job.Input as XElement;
         var newserv = ServBs.Current as Data.VF_ServicesResult;

         if (xinput != null)
         {
            switch (xinput.Attribute("moveposition").Value)
            {
               case "next":
                  ServBs.MoveNext();
                  newserv = ServBs.Current as Data.VF_ServicesResult;

                  if (serv == newserv) return;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                       new List<Job>
                       { 
                          new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", newserv.FILE_NO))},
                       })
                  );
                  break;
               case "previous":
                  ServBs.MovePrevious();
                  newserv = ServBs.Current as Data.VF_ServicesResult;

                  if (serv == newserv) return;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                       new List<Job>
                       { 
                          new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", newserv.FILE_NO))},
                       })
                  );
                  break;
               default:
                  break;
            }
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// 100
      /// </summary>
      /// <param name="job"></param>
      private void SetFilterOnQuery(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput != null)
         {
            var count = 0;
            Lb_FilterCount.Visible = true;
            if (xinput.Element("Tags") != null)
               count += Convert.ToInt32(xinput.Element("Tags").Attribute("cont").Value);
            if (xinput.Element("Regions") != null)
               count += Convert.ToInt32(xinput.Element("Regions").Attribute("cont").Value);
            if (xinput.Element("Extra_Infos") != null)
               count += Convert.ToInt32(xinput.Element("Extra_Infos").Attribute("cont").Value);
            if (xinput.Element("Contact_Infos") != null)
               count += Convert.ToInt32(xinput.Element("Contact_Infos").Attribute("cont").Value);

            if (count != 0)
            {
               Lb_FilterCount.Visible = true;
               Lb_FilterCount.Text = count.ToString();
               Filter_Butn.ImageProfile = Properties.Resources.IMAGE_1598;
               Filter_Butn.Tag = xinput;
            }
            else
            {
               Lb_FilterCount.Visible = false;
               Filter_Butn.ImageProfile = Properties.Resources.IMAGE_1597;
               Filter_Butn.Tag = null;
            }
         }
         else
         {
            Lb_FilterCount.Visible = false;
            Filter_Butn.ImageProfile = Properties.Resources.IMAGE_1597;
            Filter_Butn.Tag = null;
         }
         Execute_Query();
         job.Status = StatusType.Successful;
      }

   }
}


