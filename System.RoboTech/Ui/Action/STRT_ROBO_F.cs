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

         OrgnBs.MoveFirst();
         foreach (var orgn in OrgnBs.List.OfType<Data.Organ>())
         {
            foreach (var robot in RoboBs.List.OfType<Data.Robot>().Where(r => r.Organ == orgn && r.STAT == "002"))
            {
               ConsoleOutLog_MemTxt.Text += string.Format("{0} has Starting\r\n", robot.NAME);
               if(robot.BOT_TYPE == "001")
                  ConsoleOutLog_MemTxt.Text += string.Format("{0} has Reject\r\n", robot.NAME);
                  //iRobots.Add(
                  //   new TelegramApiBot(robot.TKON_CODE, ConnectionString, ConsoleOutLog_MemTxt, true, robot, this)
                  //);
               else if(robot.BOT_TYPE == "002")
                  iRobots.Add(
                     new BaleApiBot(robot.TKON_CODE, ConnectionString, ConsoleOutLog_MemTxt, true, robot, this)
                  );
               ConsoleOutLog_MemTxt.Text += string.Format("{0} has Started\r\n", robot.NAME);
            }
            OrgnBs.MoveNext();
         }         
      }

      private void StopRobot_Butn_Click(object sender, EventArgs e)
      {         
         foreach (var robot in iRobots)
         {
            ConsoleOutLog_MemTxt.Text = string.Format("{0} has Stoping\r\n", robot.BotName);
            if (robot.Started)
               robot.StopReceiving();            
            ConsoleOutLog_MemTxt.Text = string.Format("{0} has Stoped\r\n", robot.BotName);
         }

         iRobots.Clear();

         robotStarted = false;
      }
   }
}
