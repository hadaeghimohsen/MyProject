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

namespace System.Scsc.Ui.ReportManager
{
   public partial class RQST_TRAC_F : UserControl
   {
      public RQST_TRAC_F()
      {
         InitializeComponent();
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query(bool runAllQuery)
      {
         if(tb_master.SelectedTab == tp_001 || runAllQuery)
         {

         }
      }

      private void RqstTrac_Butn_Click(object sender, EventArgs e)
      {
         if (Rqid_Text.Text.Trim() == "" && FileNo_Text.Text.Trim() == "") return;


         if (Rqid_Text.Text.Trim() != "")
         {
            var query = iScsc.ExecuteQuery<long>(string.Format(@"WITH RqstTree(Rqid) AS
               (
                  SELECT R.RQID
                    FROM dbo.Request R
                   WHERE R.RQID = {0}
    
                   UNION ALL
   
                  SELECT Rl.RQID
                    FROM dbo.Request RL, RqstTree
                   WHERE RqstTree.Rqid = RL.RQST_RQID
               )
               SELECT * 
                 FROM RqstTree
               ", Rqid_Text.Text));
            RqstBs1.DataSource = from r in iScsc.Requests
                                 where query.ToList().Contains(r.RQID)
                                 select r;

         }
         else if (FileNo_Text.Text.Trim() != "")
         {
            var query = iScsc.ExecuteQuery<long>(string.Format(@"WITH RqstTree(Rqid) AS
               (
                  SELECT R.RQID
                    FROM dbo.Request R, Fighter F
                   WHERE R.RQID = {0}
                     AND R.Rqid = F.Rqst_Rqid
    
                   UNION ALL
   
                  SELECT Rl.RQID
                    FROM dbo.Request RL, RqstTree
                   WHERE RqstTree.Rqid = RL.RQST_RQID
               )
               SELECT * 
                 FROM RqstTree
               ", FileNo_Text.Text));
            RqstBs1.DataSource = from r in iScsc.Requests
                                 where query.ToList().Contains(r.RQID)
                                 select r;

         }

      }
   }
}
