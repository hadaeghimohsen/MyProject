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
   public partial class SRBT_INFO_F : UserControl
   {
      public SRBT_INFO_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long servfileno = 0;
      private long roborbid = 0;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         ListSrbtBs.DataSource = iRoboTech.Service_Robots.Where(sr => sr.ROBO_RBID == roborbid);
         SrbtBs.DataSource = iRoboTech.Service_Robots.FirstOrDefault(sr => sr.SERV_FILE_NO == servfileno && sr.ROBO_RBID == roborbid);
         requery = false;
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SrbtBs.EndEdit();
            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Btn_Back_Click(null, null);
            }
         }
      }
   }
}
