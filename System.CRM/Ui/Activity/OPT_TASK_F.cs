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
using System.MaxUi;

namespace System.CRM.Ui.Activity
{
   public partial class OPT_TASK_F : UserControl
   {
      public OPT_TASK_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long fileno, projrqstrqid;
      private bool needclose = true;
      private long rqstrqid;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => s.FILE_NO == fileno );
         JobpBs.DataSource = iCRM.Job_Personnels.Where(jp => jp.STAT == "002");
         requery = false;
      }

      private bool Apply()
      {
         try
         {
            var task = TaskBs.Current as Data.Task;

            if (task.RQRO_RQST_RQID == null || ColbBs.List.OfType<Data.Collaborator>().Any(c => c.CLID == 0))
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  TaskBs.DataSource =
                     iCRM.Tasks.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.TKID == iCRM.Tasks.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.TKID));
                  task = TaskBs.Current as Data.Task;
                  ColbBs.DataSource = iCRM.Collaborators.Where(c => c.RQRO_RQST_RQID == task.RQRO_RQST_RQID);
                  requery = true;
               }
            }
            else
               requery = true;

            return requery;

         }
         catch { return false; }
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
            TaskBs.EndEdit();
            var task = TaskBs.Current as Data.Task;

            if (task == null) throw new Exception("خطا * شی وظیفه خالی می باشد"); ;
            if (task.SERV_FILE_NO == null || task.SERV_FILE_NO == 0) { Serv_Lov.Focus(); throw new Exception("خطا * شماره پرونده وظیفه خالی می باشد"); }
            if (task.DUE_DATE == null) { Due_Date.Focus(); throw new Exception("خطا * تاریخ انجام وظیفه خالی می باشد"); }
            if (task.SUBJ_DESC == null || task.SUBJ_DESC == "") { Subject_Txt.Focus(); throw new Exception("خطا * موضوع وظیفه خالی می باشد"); }
            if (task.TASK_CMNT == null || task.TASK_CMNT == "") { Comment_Txt.Focus(); throw new Exception("خطا * متن وظیفه خالی می باشد"); }
            if (ColbBs.Count == 0) { JobpCode_Lov.Focus(); throw new Exception("خطا * همکاران وظیفه خالی می باشد"); }

            iCRM.OPR_TSAV_P(
               new XElement("Task",
                  new XAttribute("rqstrqid", rqstrqid),
                  new XAttribute("projrqstrqid", projrqstrqid),
                  new XAttribute("servfileno", fileno),
                  new XAttribute("duedate", GetDateTimeString(task.DUE_DATE)),
                  new XAttribute("rqrorqstrqid", task.RQRO_RQST_RQID ?? 0),
                  new XAttribute("rqrorwno", task.RQRO_RWNO ?? 0),
                  new XAttribute("tkid", task.TKID),
                  new XAttribute("colr", task.COLR ?? "#ADFF2F"),
                  new XAttribute("taskstat", task.TASK_STAT ?? "001"),
                  new XAttribute("deadlinestat", task.DEAD_LINE_STAT ?? "001"),
                  new XAttribute("deadline", GetDateTimeString(task.DEAD_LINE ?? DateTime.Now)),
                  new XAttribute("archstat", task.ARCH_STAT ?? "001"),
                  new XElement("Comment",
                     new XAttribute("subject", task.SUBJ_DESC),
                     task.TASK_CMNT
                  ),
                  new XElement("Collaborators",
                     ColbBs.List.OfType<Data.Collaborator>().
                     Select(c =>
                        new XElement("Collaborator",
                           new XAttribute("jobpcode", c.JOBP_CODE)
                        )
                     )
                  )                  
               )
            );

            // 1396/03/22 * بروز رسانی فرم اصلی برای یادآوری ها
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 41 /* Execute SetNotification */){Executive = ExecutiveType.Asynchronous},
                     new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 42 /* Execute SetMention */){Executive = ExecutiveType.Asynchronous}
                  }
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery && needclose)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno)) }
               );
               Btn_Back_Click(null, null);
            }
         }
      }

      private void JobpCode_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               if (JobpCode_Lov.EditValue == null || JobpCode_Lov.EditValue.ToString() == "") return;

               var jobpcode = (long)JobpCode_Lov.EditValue;

               // اگر قبلا همین فرد را وارد لیست کرده باشیم
               if (!JobpBs.List.OfType<Data.Job_Personnel>().Any(jp => jp.CODE == jobpcode) || ColbBs.List.OfType<Data.Collaborator>().Any(jp => jp.JOBP_CODE == jobpcode)) return;
               ColbBs.AddNew();
               var newcolb = ColbBs.Current as Data.Collaborator;
               newcolb.JOBP_CODE = jobpcode;
            }
         }
         catch (Exception exc)
         {

         }
      }

      private string GetDateTimeString(DateTime? dt)
      {
         return
            string.Format("{0}-{1}-{2} {3}:{4}:{5}",
               dt.Value.Year,
               dt.Value.Month,
               dt.Value.Day,
               dt.Value.Hour,
               dt.Value.Minute,
               dt.Value.Second
            );
      }

      private void SelectColor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var task = TaskBs.Current as Data.Task;

            if (task.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  TaskBs.DataSource =
                     iCRM.Tasks.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.TKID == iCRM.Tasks.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.TKID));
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
               var task = TaskBs.Current as Data.Task;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 48 /* Execute Tsk_Colr_F */),
                        new Job(SendType.SelfToUserInterface, "TSK_COLR_F", 10 /* Execute Actn_Calf_P */)
                        {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", task.SERV_FILE_NO), 
                                 new XAttribute("tkid", task.TKID),
                                 //new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         }
                     }
                  )
               );
            }
         }
         
      }

      private void TaskBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = TaskBs.Current as Data.Task;            

            if (rqst == null) { RqstFolw_Butn.Visible = false; return; }

            if (rqst.Request_Row != null && rqst.Request_Row.Request.RQST_RQID != null)
            {
               RqstFolw_Butn.Visible = true;
               RqstFolw_Butn.Tooltip = string.Format("درخواست پیرو {0}", rqst.Request_Row.Request.Request1.Request_Type.RQTP_DESC);
               RqstFolw_Butn.Tag =
                  new XElement("Request", new XAttribute("rqtpcode", rqst.Request_Row.Request.Request1.RQTP_CODE), new XAttribute("rqid", rqst.Request_Row.Request.Request1.RQID));
            }
            else
            {
               RqstFolw_Butn.Visible = false;
            }

            rqst.DEAD_LINE_STAT = rqst.DEAD_LINE_STAT == null ? "001" : rqst.DEAD_LINE_STAT;
            switch (rqst.DEAD_LINE_STAT)
            {
               case "001":
                  DeadLine_Dt.Visible = DeadLine_Tim.Visible = false;
                  break;
               case "002":
                  DeadLine_Dt.Visible = DeadLine_Tim.Visible = true;
                  break;
               default:
                  break;
            }

            rqst.ARCH_STAT = rqst.ARCH_STAT == null ? "001" : rqst.ARCH_STAT;

            rqst.TASK_STAT = rqst.TASK_STAT == null ? "001" : rqst.TASK_STAT;

            rqst.COLR = rqst.COLR == null ? "#ADFF2F" : rqst.COLR;
            SelectColor_Butn.NormalColorA = SelectColor_Butn.NormalColorB = SelectColor_Butn.HoverColorA = SelectColor_Butn.HoverColorB = ColorTranslator.FromHtml(rqst.COLR);
         }
         catch { }
      }

      private void Tg_DeadLine_Toggled(object sender, EventArgs e)
      {
         var task = TaskBs.Current as Data.Task;
         if (task == null) return;

         var tg = sender as ToggleSwitch;

         switch (tg.IsOn)
         {
            case true:
               task.DEAD_LINE_STAT = "002";
               DeadLine_Dt.Visible = DeadLine_Tim.Visible = true;
               break;
            case false:
               task.DEAD_LINE_STAT = "001";
               DeadLine_Dt.Visible = DeadLine_Tim.Visible = false;
               break;
         }
      }

      private void Tg_ArchStat_Toggled(object sender, EventArgs e)
      {
         var task = TaskBs.Current as Data.Task;
         if (task == null) return;

         var tg = sender as ToggleSwitch;

         switch (tg.IsOn)
         {
            case true:
               task.ARCH_STAT = "002";
               break;
            case false:
               task.ARCH_STAT = "001";
               break;
         }
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (!Apply()) return;

            var colb = ColbBs.Current as Data.Collaborator;           

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 49 /* Execute Tsk_Ckls_F */),
                     new Job(SendType.SelfToUserInterface, "TSK_CKLS_F", 10 /* Execute Actn_Calf_P */)
                     {
                        Input = 
                           new XElement("Collaboration",
                              new XAttribute("clid", colb.CLID)
                           )
                     }
                  }
               )
            );
         }
         catch
         { }
      }

      private void RemoveColb_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var colb = ColbBs.Current as Data.Collaborator;
            if (colb == null) return;
            if (MessageBox.Show(this, "آیا با حذف اطلاعات همکار در این قسمت موافق هستید؟ امکان دارد این گزینه از همکار شما اطلاعاتی برای وظیفه جاری ثبت کرده باشد و این امر ممکن است تمامی آون اطلاعات از بین رود!", "حذف همکار", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            
            if(colb.CLID > 0)
               iCRM.DEL_COLB_P(colb.CLID);

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
               var task = TaskBs.Current as Data.Task;               
               ColbBs.DataSource = iCRM.Collaborators.Where(c => c.RQRO_RQST_RQID == task.RQRO_RQST_RQID);
            }
         }
      }

      private void Tag_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var task = TaskBs.Current as Data.Task;

            if (task.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  TaskBs.DataSource =
                     iCRM.Tasks.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.TKID == iCRM.Tasks.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.TKID));
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
               var task = TaskBs.Current as Data.Task;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 50 /* Execute Tsk_Tag_F */),
                        new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 10 /* Execute Actn_Calf_P */)
                        {
                            Input = 
                              new XElement("Tag", 
                                 new XAttribute("rqid", task.RQRO_RQST_RQID)
                              )
                         }
                     }
                  )
               );
            }
         }
      }

      private void AttachFile_butn_Click(object sender, EventArgs e)
      {
         try
         {
            var task = TaskBs.Current as Data.Task;

            if (task.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  TaskBs.DataSource =
                     iCRM.Tasks.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.TKID == iCRM.Tasks.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.TKID));
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
               var task = TaskBs.Current as Data.Task;

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                       {                  
                         new Job(SendType.Self, 32 /* Execute Fss_Show_F */),
                         new Job(SendType.SelfToUserInterface, "FSS_SHOW_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", task.SERV_FILE_NO), 
                                 new XAttribute("rqstrqid", task.RQRO_RQST_RQID),
                                 //new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
      }

      private void Comment_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var task = TaskBs.Current as Data.Task;

            if (task.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  TaskBs.DataSource =
                     iCRM.Tasks.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.TKID == iCRM.Tasks.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.TKID));
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
               var task = TaskBs.Current as Data.Task;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 51 /* Execute Tsk_Cmnt_F */),
                        new Job(SendType.SelfToUserInterface, "TSK_CMNT_F", 10 /* Execute Actn_Calf_P */)
                        {
                            Input = 
                              new XElement("Comment", 
                                 new XAttribute("rqid", task.RQRO_RQST_RQID)
                              )
                         }
                     }
                  )
               );
            }
         }
      }

      private void CallRequest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var task = TaskBs.Current as Data.Task;

            switch (butn.Tag.ToString())
            {
               case "002":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 56 /* Execute Opt_Rqst_F */),
                           new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input = 
                                 new XElement("Request",
                                    new XAttribute("fileno", task.SERV_FILE_NO),
                                    new XAttribute("rqid", task.RQRO_RQST_RQID)
                                 )
                           }                     
                        }
                     )
                  );
                  break;
               case "001":
                  Btn_Back_Click(null, null);
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 57 /* Execute Lst_Serv_F */),
                           new Job(SendType.SelfToUserInterface, "LST_SERV_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input = 
                                 new XElement("Request",
                                    new XAttribute("fileno", ""),
                                    new XAttribute("rqid", task.RQRO_RQST_RQID),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }                     
                        }
                     )
                  );
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void Finr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var task = TaskBs.Current as Data.Task;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 59 /* Execute Fin_Rslt_F */),
                     new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Final_Result",                              
                              new XAttribute("rqid", task.RQRO_RQST_RQID)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void InfoServ_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 101 /* Execute ShowServiceInfo */)
                  {
                     Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
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
                           new XAttribute("section", "task")
                        )
                  }
               }
            )
         );
      }

      private void RqstFolw_Butn_Click(object sender, EventArgs e)
      {
         var xinput = (sender as RoundedButton).Tag as XElement;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
            {
               Input =
                  new XElement("Request",
                     new XAttribute("rqtpcode", xinput.Attribute("rqtpcode").Value),
                     new XAttribute("rqid", xinput.Attribute("rqid").Value)
                  )
            }
         );
      }

      private void Subject_Txt_TextChanged(object sender, EventArgs e)
      {
         var task = TaskBs.Current as Data.Task;

         if (task == null || !LinkText_Pk.PickChecked) return;

         task.SUBJ_DESC = Subject_Txt.Text;
         task.TASK_CMNT = Subject_Txt.Text;
      }
   }
}
