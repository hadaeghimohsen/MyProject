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

namespace System.Scsc.Ui.MasterPage
{
   public partial class STRT_MENU_F : UserControl
   {
      public STRT_MENU_F()
      {
         InitializeComponent();
      }

      private void StartFixMenu_Butn_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         switch (e.Button.Properties.Tag.ToString())
         {            
            case "1":
               var GetCurrentUser = new Job(SendType.External, "Localhost", "Commons", 12 /* Execute GetCurrentUser */, SendType.Self);
               _DefaultGateway.Gateway(
                  GetCurrentUser
               );

               // Open Current User Info
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "Commons:Program:DataGuard:SecurityPolicy:User", 08 /* Execute DoWork4CurrentUserInfo */, SendType.Self) { Input = GetCurrentUser.Output }
               );
               break;
            case "2":
               _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard", 03 /* Execute DoWork4SecuritySettings */, SendType.Self));
               break;
            case "3":
               _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 25 /* Execute DoWork4Shutingdown */, SendType.Self));
               break;
            case "4":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "", 142 /* Execute Usr_Actn_F */, SendType.Self)
               );
               break;
         }
      }

      private void bbi_adm1butn_Click(object sender, EventArgs e)
      {

      }

      private void StartMenu_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
