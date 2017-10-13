using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;
using System.CRM.ExceptionHandlings;
using DevExpress.XtraEditors;

namespace System.CRM.Ui.FileServerStorage
{
   public partial class FSS_SHOW_F : UserControl
   {
      public FSS_SHOW_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long fileno;
      private long rqstrqid;
      //private long emid;
      //private long tkid;
      private long cmid;

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if (Tb_Master.SelectedTab == tp_001)
            EmalRqstFileBs.DataSource = 
               iCRM.Send_Files
               .Where(
                  sf => 
                     sf.SERV_FILE_NO == fileno && 
                     ( 
                        //sf.Request_Row.Request.Request1.Request_Rows.Any(rr => rr.Emails.FirstOrDefault().EMID == emid) ||
                        //sf.Request_Row.Request.Request1.Request_Rows.Any(rr => rr.Tasks.FirstOrDefault().TKID == tkid) ||
                        sf.Request_Row.Request.Request1.RQID == rqstrqid ||
                        sf.CMNT_CMID == cmid
                     )
               );
         else
            ShareFileBs.DataSource = iCRM.Send_Files.Where(em => em.SHER_TEAM == "002");
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //EmalBs.EndEdit();
            //var logc = EmalBs.Current as Data.Log_Call;

            //if (logc == null) return;
            //if (logc.LOG_TYPE == null || logc.LOG_TYPE == "") { LogCallType_Lov.Focus(); return; }
            //if (logc.SERV_FILE_NO == null || logc.SERV_FILE_NO == 0) { Serv_Lov.Focus(); return; }
            //if (logc.LOG_DATE == null) { LogCall_Date.Focus(); return; }
            //if (logc.SUBJ_DESC == null || logc.SUBJ_DESC == "") { Subject_Txt.Focus(); return; }
            //if (logc.LOG_CMNT == null || logc.LOG_CMNT == "") { Comment_Txt.Focus(); return; }

            //iCRM.OPR_LSAV_P(
            //   new XElement("LogCall",
            //      new XAttribute("type", logc.LOG_TYPE),
            //      new XAttribute("servfileno", logc.SERV_FILE_NO),
            //      new XAttribute("datetime", logc.LOG_DATE),
            //      new XAttribute("rqrorqstrqid", logc.RQRO_RQST_RQID ?? 0),
            //      new XAttribute("rqrorwno", logc.RQRO_RWNO ?? 0),
            //      new XAttribute("lcid", logc.LCID),
            //      new XElement("Comment",
            //         new XAttribute("subject", logc.SUBJ_DESC),
            //         logc.LOG_CMNT
            //      )
            //   )
            //);

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               //_DefaultGateway.Gateway(
               //   new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno), new XAttribute("formcaller", GetType().Name), new XAttribute("emid", emid)) }
               //);
               Btn_Back_Click(null, null);
            }
         }
      }

      private void AddNewFile_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 29 /* Execute Opt_Sndf_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_SNDF_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                     new XElement("Service", 
                        new XAttribute("fileno", fileno), 
                        //new XAttribute("emid", emid), 
                        //new XAttribute("tkid", tkid), 
                        new XAttribute("rqstrqid", rqstrqid),
                        new XAttribute("cmid", cmid),
                        new XAttribute("sendfiletype", "new"),
                        //new XAttribute("sendtype", "001"),
                        new XAttribute("formcaller", GetType().Name)
                     )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void AcceptSelectedFiles_Butn_Click(object sender, EventArgs e)
      {

      }      
   }
}
