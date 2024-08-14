using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.RoboTech.ExceptionHandlings;
using System.RoboTech.Controller;

namespace System.RoboTech.Ui.Action
{
   public partial class STRT_ROBO_F : UserControl
   {
      public STRT_ROBO_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool robotStarted = false;

      List<IApiBot> iRobots;

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      
      private void StrtBot_Butn_Click(object sender, EventArgs e)
      {
         // آیا اجازه اجرا کردن ربات بر روی این سرور را داریم یا خیر
         if (!iRoboTech.V_URLFGAs.Any(host => host.HOST_NAME == HostNameInfo.Attribute("cpu").Value)) { return; }

         if(iRobots == null)
         {
            iRobots = new List<IApiBot>();
         }

         if (robotStarted) return;

         robotStarted = true;

         ActvProcMsg_Tm.Enabled = true;

         OrgnBs.MoveFirst();
         foreach (var orgn in OrgnBs.List.OfType<Data.Organ>())
         {
            foreach (var robot in RoboBs.List.OfType<Data.Robot>().Where(r => r.Organ == orgn && r.STAT == "002"))
            {
               if (robot.RUN_STAT == "002") ConsoleOutLog_MemTxt.Text += string.Format("{0} has Starting\r\n", robot.NAME);
               if(robot.BOT_TYPE == "001" && robot.RUN_STAT == "002")                  
                  iRobots.Add(
                     new TelegramApiBot(robot.TKON_CODE, ConnectionString, ConsoleOutLog_MemTxt, true, robot, this)
                  );
               else if(robot.BOT_TYPE == "002" && robot.RUN_STAT == "002")
                  iRobots.Add(
                     new BaleApiBot(robot.TKON_CODE, ConnectionString, ConsoleOutLog_MemTxt, true, robot, this)
                  );
               if(robot.RUN_STAT == "002") ConsoleOutLog_MemTxt.Text += string.Format("{0} has Started\r\n", robot.NAME);
            }
            OrgnBs.MoveNext();
         }         
      }

      private void StopRobot_Butn_Click(object sender, EventArgs e)
      {         
         foreach (var robot in iRobots)
         {
            ConsoleOutLog_MemTxt.Text = string.Format("{0} has Stoping\r\n", robot.Robot.NAME);
            if (robot.Started)
               robot.StopReceiving();            
            ConsoleOutLog_MemTxt.Text = string.Format("{0} has Stoped\r\n", robot.Robot.NAME);
         }

         iRobots.Clear();

         robotStarted = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            ActvDactv_Tsmi.Text = robo.STAT == "002" ? "غیرفعال کردن" : "فعال کردن";
            RunNoRun_Tsmi.Text = robo.RUN_STAT == "002" ? "عدم اجرای ربات" : "اجرای ربات";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ActvDactv_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            robo.STAT = robo.STAT == "002" ? "001" : "002";

            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void RunNoRun_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            robo.RUN_STAT = robo.RUN_STAT == "002" ? "001" : "002";

            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ActvProcMsg_Tm_Tick(object sender, EventArgs e)
      {
         try
         {
            ActvProcMsg_Tm.Enabled = false;
            //Threading.Thread.Sleep(60000);
            Actn4Mesg_Cbx.Checked = true;            
         }
         catch { }
      }
   }
}
