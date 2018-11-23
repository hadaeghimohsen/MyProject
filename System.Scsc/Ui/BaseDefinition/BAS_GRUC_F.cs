using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_GRUC_F : UserControl
   {
      public BAS_GRUC_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         UserClubBs.DataSource = iScsc.User_Club_Fgacs.Where(uc => uc.Club == Club && uc.REC_STAT == "002");
      }

      private void GrantUserToClub_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.V_User;
            if (user == null) return;
            if (iScsc.User_Club_Fgacs.Any(uc => uc.Club == Club && uc.SYS_USER == user.USER_DB && uc.REC_STAT == "002")) return;

            //var regn = Club.Region;
            //iScsc.STNG_SAVE_P(
            //   new XElement("Config",
            //      new XAttribute("type", "001"),
            //      new XElement("FgaURegn",
            //         new XAttribute("cntycode", regn.PRVN_CNTY_CODE),
            //         new XAttribute("prvncode", regn.PRVN_CODE),
            //         new XAttribute("regncode", regn.CODE),
            //         new XAttribute("sysuser", user.USER_DB)
            //      )
            //   )
            //);

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "003"),
                  new XElement("FgaUClub",
                     new XAttribute("sysuser", user.USER_DB),
                     new XAttribute("mstrsysuser", ""),
                     new XAttribute("clubcode", Club.CODE)
                  )
               )
            );

            requery = true;
         }
         catch (Exception )
         {
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }

      }

      private void RevokeUserFromClub_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var Fga_Club = UserClubBs.Current as Data.User_Club_Fgac;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "004"),
                  new XElement("FgaUClub",
                     new XAttribute("sysuser", Fga_Club.SYS_USER),
                     //new XAttribute("mstrsysuser", Fga_Club.MAST_SYS_USER),
                     new XAttribute("clubcode", Fga_Club.CLUB_CODE)
                  )
               )
            );

            // 1396/03/19 * زمانی که دسترسی کاربر از شیفت باشگاه گرفته می شود نیازی به گرفتن دسترسی از ناحیه نمی باشد
            //var regn = Club.Region;
            //iScsc.STNG_SAVE_P(
            //   new XElement("Config",
            //      new XAttribute("type", "002"),
            //      new XElement("FgaURegn",
            //         new XAttribute("cntycode", regn.PRVN_CNTY_CODE),
            //         new XAttribute("prvncode", regn.PRVN_CODE),
            //         new XAttribute("regncode", regn.CODE),
            //         new XAttribute("sysuser", Fga_Club.SYS_USER)
            //      )
            //   )
            //);

            requery = true;
         }
         catch (Exception )
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }

      }

      private void User_Gv_DoubleClick(object sender, EventArgs e)
      {
         GrantUserToClub_Butn_Click(null, null);
      }

      private void UserClub_Gv_DoubleClick(object sender, EventArgs e)
      {
         RevokeUserFromClub_Butn_Click(null, null);
      }
   }
}
