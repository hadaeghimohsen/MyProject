﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;


namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SettingsSystemScript : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iProjectDataContext iProject;
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
            case 07:
               LoadData(job);
               break;
            case 10:
               ActionCallWindow(job);
               break;
            case 100:
               ExecuteNoneQuery(job);
               break;
            case 101:
               ExecuteDataAdapter(job);
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
               //new Job(SendType.SelfToUserInterface, GetType().Name, 06 /* Execute CloseDrawer */)
               //{
               //   Next = new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
               //};
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };
         
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
         iProject = new Data.iProjectDataContext(GetConnectionString.Output.ToString());

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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "DataGuard:SecurityPolicy:" + GetType().Name, this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */){ Input = this }               
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
                           Input = new List<string> {"<Privilege>30</Privilege><Sub_Sys>0</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 30 امنیتی", "خطا دسترسی");
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
         DptypBs.DataSource = iProject.D_PTYPs;
         DpsrcBs.DataSource = iProject.D_PSRCs;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallWindow(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput.Attribute("subsys") != null)
            subsys = Convert.ToInt32(xinput.Attribute("subsys").Value);

         SwitchButtonsTabPage(Apps_Butn);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 100
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteNoneQuery(Job job)
      {
         try
         {
            var script = ScrpBs.Current as Data.Script;
            if (script == null) return;

            var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self);            

            switch (script.SUB_SYS)
            {
               case 0:
                  GetConnectionString.Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>";
                  break;
               case 5:
                  GetConnectionString.Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>";
                  break;
               case 11:
                  GetConnectionString.Input = "<Database>iCRM</Database><Dbms>SqlServer</Dbms>";
                  break;
               default:
                  break;
            }

            _DefaultGateway.Gateway(
               GetConnectionString
            );

            var constr = GetConnectionString.Output.ToString();

            SqlCommand sqlcom =
               new SqlCommand(job.Input.ToString(), 
                  new SqlConnection(constr)
               );

            sqlcom.CommandTimeout = 18000;
            sqlcom.Connection.Open();
            job.Output = sqlcom.ExecuteNonQuery();
            sqlcom.Connection.Close();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            job.Status = StatusType.Failed;
            job.Output = exc.Message;
         }
      }

      /// <summary>
      /// Code 101
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteDataAdapter(Job job)
      {
         try
         {
            var script = ScrpBs.Current as Data.Script;
            if (script == null) return;

            var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self);

            switch (script.SUB_SYS)
            {
               case 0:
                  GetConnectionString.Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>";
                  break;
               case 5:
                  GetConnectionString.Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>";
                  break;
               case 11:
                  GetConnectionString.Input = "<Database>iCRM</Database><Dbms>SqlServer</Dbms>";
                  break;
               default:
                  break;
            }

            _DefaultGateway.Gateway(
               GetConnectionString
            );

            var constr = GetConnectionString.Output.ToString();

            SqlDataAdapter SqlAdp = new SqlDataAdapter(            
               new SqlCommand(job.Input.ToString(),
                  new SqlConnection(constr)
               )
            );

            SqlAdp.SelectCommand.CommandTimeout = 18000;
            DataSet ds = new DataSet();
            SqlAdp.Fill(ds);

            job.Output = ds;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            job.Status = StatusType.Failed;
            job.Output = exc.Message;
         }
      }
   }
}
