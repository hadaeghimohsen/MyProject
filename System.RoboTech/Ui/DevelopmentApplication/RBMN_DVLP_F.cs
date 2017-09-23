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
using DevExpress.XtraEditors;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class RBMN_DVLP_F : UserControl
   {
      public RBMN_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long muid;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         MenuBs.DataSource = iRoboTech.Menu_Ussds.FirstOrDefault(m => m.MUID == muid);
      }

      private void Tsb_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            Invalidate();
            MenuBs.EndEdit();

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
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProccessCmd */){Input = Keys.Escape},
                        new Job(SendType.SelfToUserInterface, "ORGN_DVLP_F", 10 /* Execute Actn_Calf_P */)
                     }
                  )
               );
            }
         }
      }

      private void MenuBs_CurrentChanged(object sender, EventArgs e)
      {
         var menu = MenuBs.Current as Data.Menu_Ussd;
         if (menu == null) return;

         // Set Menu Stat
         menu.STAT = menu.STAT == null ? "001" : menu.STAT;
         switch (menu.STAT)
         {
            case "001":
               Tg_Stat.IsOn = false;
               break;
            case "002":
               Tg_Stat.IsOn = true;
               break;
         }

         // Set Root Menu
         menu.ROOT_MENU = menu.ROOT_MENU == null ? "001" : menu.ROOT_MENU;
         switch (menu.ROOT_MENU)
         {
            case "001":
               Tg_RootMenu.IsOn = false;
               break;
            case "002":
               Tg_RootMenu.IsOn = true;
               break;
         }

         // Set Cmnd Fire
         menu.CMND_FIRE = menu.CMND_FIRE == null ? "001" : menu.CMND_FIRE;
         switch (menu.CMND_FIRE)
         {
            case "001":
               Tg_CmndFire.IsOn = false;
               break;
            case "002":
               Tg_CmndFire.IsOn = true;
               break;
         }

         // Set Cmnd Plac
         menu.CMND_PLAC = menu.CMND_PLAC == null ? "001" : menu.CMND_PLAC;
         switch (menu.CMND_PLAC)
         {
            case "001":
               Tg_CmndPlac.IsOn = false;
               break;
            case "002":
               Tg_CmndPlac.IsOn = true;
               break;
         }

         // Set Set Back
         menu.STEP_BACK = menu.STEP_BACK == null ? "001" : menu.STEP_BACK;
         switch (menu.STEP_BACK)
         {
            case "001":
               Tg_StepBack.IsOn = false;
               break;
            case "002":
               Tg_StepBack.IsOn = true;
               break;
         }
      }

      private void Tg_00i_Toggled(object sender, EventArgs e)
      {
         var tg = sender as ToggleSwitch;

         var menu = MenuBs.Current as Data.Menu_Ussd;
         switch (tg.Tag.ToString())
         {
            case "001":
               menu.STAT = tg.IsOn ? "002" : "001";
               break;
            case "002":
               menu.ROOT_MENU = tg.IsOn ? "002" : "001";
               break;
            case "003":
               menu.CMND_FIRE = tg.IsOn ? "002" : "001";
               break;
            case "004":
               menu.CMND_PLAC = tg.IsOn ? "002" : "001";
               break;
            case "005":
               menu.STEP_BACK = tg.IsOn ? "002" : "001";
               break;
            default:
               break;
         }
      }
   }
}
