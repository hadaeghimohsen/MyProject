﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.RoboTech.Ui.Inspection
{
   partial class MNGR_INSP_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iRoboTechDataContext iRoboTech;
      private string ConnectionString;
      private List<long?> Fga_Ugov_U;
      private XElement HostNameInfo;
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
         CurrentUser = iRoboTech.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         Fga_Ugov_U = (iRoboTech.FGA_UGOV_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

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
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("RoboTech:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PastOnWall */) { Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         //Job _Paint = new Job(SendType.External, "Desktop",
         //   new List<Job>
         //   {
         //      new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("RoboTech:{0}", this.GetType().Name), this }  },
         //      new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "right:in-screen:stretch:center"} }               
         //   });
         //_DefaultGateway.Gateway(_Paint);

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
         //job.Next =
         //   new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
         //      new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
         //         new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

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
                           Input = new List<string> {"<Privilege>83</Privilege><Sub_Sys>12</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 83 امنیتی", "خطا دسترسی");
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

         //WletType01_Flb.Images = WletType02_Flb.Images = new Image[] { Properties.Resources.IMAGE_1545, Properties.Resources.IMAGE_1547 };
         DOdstBs.DataSource = iRoboTech.D_ODSTs;
         PrvnBs.DataSource = iRoboTech.Provinces;
         DAOdTgBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "ORDER036_INFO");
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
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
            if (xinput.Attribute("outputtype").Value == "srbspostadrs")
            {
               //MessageBox.Show(xinput.ToString());

               var srbs = InspBs.Current as Data.Service_Robot;

               var cordx = Convert.ToDouble(xinput.Attribute("cordx").Value);
               var cordy = Convert.ToDouble(xinput.Attribute("cordy").Value);

               if (cordx != srbs.CORD_X && cordy != srbs.CORD_Y)
               {
                  // Call Update Service_Public
                  try
                  {
                     srbs.CORD_X = cordx;
                     srbs.CORD_Y = cordy;
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
