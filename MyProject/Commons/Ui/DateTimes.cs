using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProject.Commons.Ui
{
   public partial class DateTimes : UserControl
   {
      public DateTimes()
      {
         InitializeComponent();
      }

      private void SetTime_Tmr_Tick(object sender, EventArgs e)
      {
         try
         {
            Hours_Lbl.Text = DateTime.Now.Hour.ToString();
            Minute_Lbl.Text = DateTime.Now.Minute.ToString();

            if(DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 18)
            {
               DayType_Pic.BackgroundImage = Properties.Resources.IMAGE_1251;
            }
            else
            {
               DayType_Pic.BackgroundImage = Properties.Resources.IMAGE_1250;
            }

            switch (DateTime.Now.Second)
            {
               #region 10
               case 1:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1301;
                  Second_Lbl.Text = "01s";
                  break;
               case 2:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1302;
                  Second_Lbl.Text = "02s";
                  break;
               case 3:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1303;
                  Second_Lbl.Text = "03s";
                  break;
               case 4:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1304;
                  Second_Lbl.Text = "04s";
                  break;
               case 5:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1305;
                  Second_Lbl.Text = "05s";
                  break;
               case 6:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1306;
                  Second_Lbl.Text = "06s";
                  break;
               case 7:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1307;
                  Second_Lbl.Text = "07s";
                  break;
               case 8:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1308;
                  Second_Lbl.Text = "08s";
                  break;
               case 9:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1309;
                  Second_Lbl.Text = "09s";
                  break;
               case 10:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1310;
                  Second_Lbl.Text = "10s";
                  break;
               #endregion
               #region 20
               case 11:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1311;
                  Second_Lbl.Text = "11s";
                  break;
               case 12:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1312;
                  Second_Lbl.Text = "12s";
                  break;
               case 13:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1313;
                  Second_Lbl.Text = "13s";
                  break;
               case 14:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1314;
                  Second_Lbl.Text = "14s";
                  break;
               case 15:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1315;
                  Second_Lbl.Text = "15s";
                  break;
               case 16:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1316;
                  Second_Lbl.Text = "16s";
                  break;
               case 17:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1317;
                  Second_Lbl.Text = "17s";
                  break;
               case 18:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1318;
                  Second_Lbl.Text = "18s";
                  break;
               case 19:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1319;
                  Second_Lbl.Text = "19s";
                  break;
               case 20:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1320;
                  Second_Lbl.Text = "20s";
                  break;
               #endregion
               #region 30
               case 21:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1321;
                  Second_Lbl.Text = "21s";
                  break;
               case 22:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1322;
                  Second_Lbl.Text = "22s";
                  break;
               case 23:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1323;
                  Second_Lbl.Text = "23s";
                  break;
               case 24:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1324;
                  Second_Lbl.Text = "24s";
                  break;
               case 25:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1325;
                  Second_Lbl.Text = "25s";
                  break;
               case 26:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1326;
                  Second_Lbl.Text = "26s";
                  break;
               case 27:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1327;
                  Second_Lbl.Text = "27s";
                  break;
               case 28:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1328;
                  Second_Lbl.Text = "28s";
                  break;
               case 29:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1329;
                  Second_Lbl.Text = "29s";
                  break;
               case 30:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1330;
                  Second_Lbl.Text = "30s";
                  break;
               #endregion
               #region 40
               case 31:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1331;
                  Second_Lbl.Text = "31s";
                  break;
               case 32:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1332;
                  Second_Lbl.Text = "32s";
                  break;
               case 33:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1333;
                  Second_Lbl.Text = "33s";
                  break;
               case 34:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1334;
                  Second_Lbl.Text = "34s";
                  break;
               case 35:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1335;
                  Second_Lbl.Text = "35s";
                  break;
               case 36:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1336;
                  Second_Lbl.Text = "36s";
                  break;
               case 37:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1337;
                  Second_Lbl.Text = "37s";
                  break;
               case 38:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1338;
                  Second_Lbl.Text = "38s";
                  break;
               case 39:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1339;
                  Second_Lbl.Text = "39s";
                  break;
               case 40:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1340;
                  Second_Lbl.Text = "40s";
                  break;
               #endregion
               #region 50
               case 41:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1341;
                  Second_Lbl.Text = "41s";
                  break;
               case 42:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1342;
                  Second_Lbl.Text = "42s";
                  break;
               case 43:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1343;
                  Second_Lbl.Text = "43s";
                  break;
               case 44:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1344;
                  Second_Lbl.Text = "44s";
                  break;
               case 45:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1345;
                  Second_Lbl.Text = "45s";
                  break;
               case 46:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1346;
                  Second_Lbl.Text = "46s";
                  break;
               case 47:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1347;
                  Second_Lbl.Text = "47s";
                  break;
               case 48:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1348;
                  Second_Lbl.Text = "48s";
                  break;
               case 49:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1349;
                  Second_Lbl.Text = "49s";
                  break;
               case 50:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1350;
                  Second_Lbl.Text = "50s";
                  break;
               #endregion
               #region 60
               case 51:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1351;
                  Second_Lbl.Text = "51s";
                  break;
               case 52:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1352;
                  Second_Lbl.Text = "52s";
                  break;
               case 53:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1353;
                  Second_Lbl.Text = "53s";
                  break;
               case 54:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1354;
                  Second_Lbl.Text = "54s";
                  break;
               case 55:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1355;
                  Second_Lbl.Text = "55s";
                  break;
               case 56:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1356;
                  Second_Lbl.Text = "56s";
                  break;
               case 57:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1357;
                  Second_Lbl.Text = "57s";
                  break;
               case 58:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1358;
                  Second_Lbl.Text = "58s";
                  break;
               case 59:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1359;
                  Second_Lbl.Text = "59s";
                  break;
               case 00:
                  Second_Pic.BackgroundImage = Properties.Resources.IMAGE_1360;                  
                  Second_Lbl.Text = "60s";
                  break;
               #endregion
               default:
                  break;
            }
            Second_Pic.Invalidate();
         }
         catch (Exception exc)
         {

         }
      }
   }
}
