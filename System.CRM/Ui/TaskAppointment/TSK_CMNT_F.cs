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
using System.Xml.Linq;
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;
using DevExpress.XtraEditors;
using System.MaxUi;

namespace System.CRM.Ui.TaskAppointment
{
   public partial class TSK_CMNT_F : UserControl
   {
      public TSK_CMNT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long rqid;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         CmntBs1.DataSource = iCRM.Comments.Where(t => t.RQRO_RQST_RQID == rqid);

         requery = false;
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         CmntBs1.AddNew();

         var cmnt = CmntBs1.Current as Data.Comment;
         cmnt.RQRO_RQST_RQID = rqid;
         cmnt.RQRO_RWNO = 1;

      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Invalidate();

            CmntBs1.EndEdit();

            iCRM.SubmitChanges();

            // 1396/03/22 * بروز رسانی فرم اصلی برای یادآوری ها
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                     
                     new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 42 /* Execute SetMention */){Executive = ExecutiveType.Asynchronous}
                  }
               )
            );

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
               Execute_Query();
            }
         }
      }

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cmnt = CmntBs1.Current as Data.Comment;
            if (cmnt == null) return;
            if (MessageBox.Show(this, "آیا با حذف دست نوشته موافق هستید؟", "حذف دست نوشته", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.DEL_CMNT_P(cmnt.CMID);

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
               Execute_Query();
            }
         }
      }

      private void CmntBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var cmnt = CmntBs1.Current as Data.Comment;
            if (cmnt == null) return;

            if(cmnt.Likes.Any(c => c.Job_Personnel.USER_NAME.ToUpper() == CurrentUser.ToUpper()))
               Like_Butn.ImageProfile = Properties.Resources.IMAGE_1572;
            else
               Like_Butn.ImageProfile = Properties.Resources.IMAGE_1571;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void Like_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cmnt = CmntBs1.Current as Data.Comment;
            if (cmnt == null) return;

            if (cmnt.Likes.Any(c => c.Job_Personnel.USER_NAME.ToUpper() == CurrentUser.ToUpper()))
               iCRM.DEL_LIKE_P(cmnt.CMID);
            else
               iCRM.INS_LIKE_P(cmnt.CMID, "");

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
               Execute_Query();
            }
         }
      }

      private void AttachFile_butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cmnt = CmntBs1.Current as Data.Comment;

            if (cmnt.RQRO_RQST_RQID == null)
            {
               Save_Butn_Click(null, null);
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  CmntBs1.DataSource =
                     iCRM.Comments.FirstOrDefault(t =>
                        t.RQRO_RQST_RQID == rqid);
                  requery = true;
               }
            }
            else
               requery = true;

         }
         catch { }
         finally
         {
            if (requery)
            {
               var cmnt = CmntBs1.Current as Data.Comment;

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                       {                  
                         new Job(SendType.Self, 32 /* Execute Fss_Show_F */),
                         new Job(SendType.SelfToUserInterface, "FSS_SHOW_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", cmnt.SERV_FILE_NO_DNRM), 
                                 new XAttribute("cmid", cmnt.CMID),
                                 //new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
      }

      private void UserMentioned_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 72 /* Execute Sjbp_Dfin_F */),
                  new Job(SendType.SelfToUserInterface, "SJBP_DFIN_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Mentioned",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("section", "comment")
                        )
                  }
               }
            )
         );
      }
   }
}
