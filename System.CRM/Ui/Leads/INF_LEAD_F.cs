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
using System.CRM.ExceptionHandlings;
using System.Globalization;
using System.IO;
using Itenso.TimePeriod;
using DevExpress.XtraGrid.Views.Grid;
using System.MaxUi;

namespace System.CRM.Ui.Leads
{
   public partial class INF_LEAD_F : UserControl
   {
      public INF_LEAD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.FirstOrDefault(s => s.FILE_NO == fileno );
         RqstChngBs.DataSource = iCRM.VF_Request_Changing(null, fileno, null).OrderByDescending(r => r.SAVE_DATE).Take(5);
         requery = false;
      }

      private void ServBs_CurrentChanged(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;

         if (serv == null) return;

         if (serv.FACE_BOOK_URL_DNRM == null || serv.FACE_BOOK_URL_DNRM == "")
            FacebookUrl_Butn.Enabled = false;
         else
            FacebookUrl_Butn.Enabled = true;

         if (serv.LINK_IN_URL_DNRM == null || serv.LINK_IN_URL_DNRM == "")
            LinkedInUrl_Butn.Enabled = false;
         else
            LinkedInUrl_Butn.Enabled = true;

         if (serv.TWTR_URL_DNRM == null || serv.TWTR_URL_DNRM == "")
            TwitterUrl_Butn.Enabled = false;
         else
            TwitterUrl_Butn.Enabled = true;

         if ((serv.RQST_RQID ?? 0) > 0)
            RqstRqid_Lbl.BackColor = Color.YellowGreen;
         else
            RqstRqid_Lbl.BackColor = Color.FromArgb(241, 241, 241);

         if (Convert.ToInt32(serv.ONOF_TAG_DNRM) <= 100)
            DeleteRecycleService_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1541;
         else
            DeleteRecycleService_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1518;

         try
         {
            ImageProfile_Butn.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", serv.FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            if (InvokeRequired)
               Invoke(new Action(() => ImageProfile_Butn.ImageProfile = bm));
            else
               ImageProfile_Butn.ImageProfile = bm;
         }
         catch {
            ImageProfile_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
      }

      #region Note
      private void AddNote_Butn_Click(object sender, EventArgs e)
      {
         /*NoteBs.AddNew();*/
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 75 /* Execute Opt_Note_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_NOTE_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                     new XElement("Service", 
                        new XAttribute("fileno", serv.FILE_NO), 
                        new XAttribute("ntid", "0"),
                        new XAttribute("formcaller", GetType().Name)
                     )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void SaveNote_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;
         var note = NoteBs.Current as Data.Note;
         if (note == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 75 /* Execute Opt_Note_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_NOTE_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("ntid", note.NTID), 
                           new XAttribute("rqrorqstrqid", note.RQRO_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
         //try
         //{
         //   NoteBs.EndEdit();
         //   var note = NoteBs.Current as Data.Note;
         //   if (note == null) return;
         //   if (note.NOTE_CMNT == null || note.NOTE_CMNT == "") { NoteComment_Txt.Focus(); return; }

         //   iCRM.OPR_NSAV_P(
         //      new XElement("Note",                  
         //         new XAttribute("servfileno", note.SERV_FILE_NO),
         //         new XAttribute("rqrorqstrqid", note.RQRO_RQST_RQID ?? 0),
         //         new XAttribute("rqrorwno", note.RQRO_RWNO ?? 0),
         //         new XAttribute("ntid", note.NTID),
         //         new XElement("Comment",                     
         //            note.NOTE_CMNT
         //         )
         //      )
         //   );

         //   // 1396/04/30 * بروز رسانی فرم اصلی برای ذکر واقع شدن
         //   _DefaultGateway.Gateway(
         //      new Job(SendType.External, "Localhost",
         //         new List<Job>
         //         {                     
         //            new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 42 /* Execute SetMention */){Executive = ExecutiveType.Asynchronous}
         //         }
         //      )
         //   );
         //   requery = true;
         //}
         //catch (Exception exc)
         //{

         //}
         //finally
         //{
         //   if(requery)
         //   {
         //      Execute_Query();
         //   }
         //}
      }

      private void DeleteNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var note = NoteBs.Current as Data.Note;
            if (note == null || MessageBox.Show(this, "آیا با حذف یادداشت موافق هستید؟", "حذف یادداشت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.Notes.DeleteOnSubmit(note);
            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {

         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      #endregion

      #region Service Button
      private void AddLogCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("lcid", 0),
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void ChangeServiceType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 37 /* Execute Opt_Cnvt_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_CNVT_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("srpbtype", "002"), 
                           new XAttribute("islock", true), 
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }


         //try
         //{
         //   var serv = ServBs.Current as Data.Service;
         //   if (serv == null || MessageBox.Show(this, "آیا با تغییر نوع مشتری موافق هستید؟", "تغییر نوع مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         //   iCRM.CHNG_SRTP_P(
         //      new XElement("Service",
         //         new XAttribute("fileno", serv.FILE_NO),
         //         new XAttribute("type", serv.SRPB_TYPE_DNRM == "001" ? "002" : "001")
         //      )
         //   );
         //   requery = true;
         //}
         //catch (Exception exc)
         //{
         //   iCRM.SaveException(exc);
         //}
         //finally
         //{
         //   if (requery)
         //   {
         //      Execute_Query();
         //   }
         //}
      }

      private void DeleteRecycleService_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         try
         {            
            if (Convert.ToInt32(serv.ONOF_TAG_DNRM) >= 101)
            {
               if (serv == null || MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "حذف مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               iCRM.DEL_SERV_P(
                  new XElement("Service",
                     new XAttribute("fileno", serv.FILE_NO)
                  )
               );
            }
            else
            {
               if (serv == null || MessageBox.Show(this, "آیا با بازیابی مشتری موافق هستید؟", "بازیابی مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               iCRM.UND_SERV_P(
                  new XElement("Service",
                     new XAttribute("fileno", serv.FILE_NO)
                  )
               );
            }
            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               Job _InteractWithCRM =
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.Self, 23 /* Execute Shw_Lead_F */),
                        new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Executive = ExecutiveType.Asynchronous,
                           Input = 
                              new XElement("Service", 
                                 new XAttribute("onoftag", Convert.ToInt32(serv.ONOF_TAG_DNRM) <= 100 ? "on" : "off")
                              )
                        }
                     }
                  );
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
      }

      private void CloneService_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 30 /* Execute Opt_Clon_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_CLON_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void SendEmail_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("emid", 0),
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("toemail", serv.EMAL_ADRS_DNRM ?? "")
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void EditService_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 13 /* Execute Adm_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Service", new XAttribute("type", "changeinfo"), new XAttribute("fileno", fileno), new XAttribute("auto", "true"), new XAttribute("srpbtype", serv.SRPB_TYPE_DNRM))}
               })
         );
      }
      #endregion

      #region LogCall
      private void LeadActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var logc = LogcBs.Current as Data.Log_Call;
            switch (e.Button.Index)
            {
               case 0:
                  #region Add Task
                  var serv = ServBs.Current as Data.Service;
                  if (serv == null) return;

                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 26 /* Execute Opt_Logc_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("rqstrqid", logc.RQRO_RQST_RQID), 
                                 new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 1:
                  #region Add Appointment
                  serv = ServBs.Current as Data.Service;
                  if (serv == null) return;

                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("rqstrqid", logc.RQRO_RQST_RQID), 
                                 new XAttribute("appointmenttype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 2:
                  #region Edit Log Call
                  serv = ServBs.Current as Data.Service;
                  if (serv == null) return;

                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("lcid", logc.LCID),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 3:
                  #region Delete Log Call
                  if (logc == null || MessageBox.Show(this, "آیا با حذف تماس تلفنی موافق هستید؟", "حذف تماس نلفنی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iCRM.Log_Calls.DeleteOnSubmit(logc);
                  iCRM.SubmitChanges();
                  requery = true;
                  #endregion
                  break;
               default:
                  break;
            }
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

      private void CallLog_Gv_DoubleClick(object sender, EventArgs e)
      {
         LeadActn_Butn_ButtonClick(LeadActn_Butn, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(LeadActn_Butn.Buttons[2]));
      }

      private void DelLogCal_Butn_Click(object sender, EventArgs e)
      {
         LeadActn_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(LeadActn_Butn.Buttons[3]));
      }

      private void EditLogCall_Butn_Click(object sender, EventArgs e)
      {
         LeadActn_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(LeadActn_Butn.Buttons[2]));
      }
      #endregion

      #region Email
      private void EmalActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var emal = EmalBs.Current as Data.Email;
            switch (e.Button.Index)
            {
               case 0:
                  #region Add Task
                  var serv = ServBs.Current as Data.Service;
                  if (serv == null) return;

                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 26 /* Execute Opt_Task_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("rqstrqid", emal.RQRO_RQST_RQID), 
                                 new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 1:
                  #region Add Appointment
                  serv = ServBs.Current as Data.Service;
                  if (serv == null) return;

                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("rqstrqid", emal.RQRO_RQST_RQID), 
                                 new XAttribute("appointmenttype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 2:
                  #region Edit Email
                  serv = ServBs.Current as Data.Service;
                  if (serv == null) return;

                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("emid", emal.EMID),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 3:
                  #region Delete Email
                  if (emal == null || MessageBox.Show(this, "آیا با حذف ایمیل ارسالی موافق هستید؟", "حذف ایمیل ارسالی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iCRM.Emails.DeleteOnSubmit(emal);
                  iCRM.SubmitChanges();
                  requery = true;
                  #endregion
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Email_Gv_DoubleClick(object sender, EventArgs e)
      {
         EmalActn_Butn_ButtonClick(EmalActn_Butn, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(EmalActn_Butn.Buttons[2]));
      }

      private void EditEmal_Butn_Click(object sender, EventArgs e)
      {
         EmalActn_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(EmalActn_Butn.Buttons[2]));
      }

      private void DelEmal_Butn_Click(object sender, EventArgs e)
      {
         EmalActn_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(EmalActn_Butn.Buttons[3]));
      }

      #endregion 

      #region Task
      private void AddTask_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 26 /* Execute Opt_Logc_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                     new XElement("Service", 
                        new XAttribute("fileno", serv.FILE_NO), 
                        new XAttribute("tasktype", "new"),
                        new XAttribute("formcaller", GetType().Name)
                     )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void EditTask_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;
         var task = TaskBs.Current as Data.Task;
         if (task == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 26 /* Execute Opt_Logc_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("tasktype", "edit"), 
                           new XAttribute("tkid", task.TKID), 
                           new XAttribute("rqrorqstrqid", task.RQRO_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void DeleteTask_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Appointment
      private void AddAppointment_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                     new XElement("Service", 
                        new XAttribute("fileno", serv.FILE_NO), 
                        new XAttribute("appointmenttype", "new"),
                        new XAttribute("formcaller", GetType().Name)
                     )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void EditAppointment_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;
         var apon = AponBs.Current as Data.Appointment;
         if (apon == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("appointmenttype", "edit"), 
                           new XAttribute("apid", apon.APID), 
                           new XAttribute("rqrorqstrqid", apon.RQRO_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void DeleteAppointment_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Send File
      private void AddSendFile_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;
                  
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 29 /* Execute Opt_Sndf_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_SNDF_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                     new XElement("Service", 
                        new XAttribute("fileno", serv.FILE_NO), 
                        new XAttribute("sendfiletype", "new"),
                        new XAttribute("formcaller", GetType().Name)
                     )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void EditSendFile_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;
         var sndf = FileBs.Current as Data.Send_File;
         if (sndf == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 29 /* Execute Opt_Sndf_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_SNDF_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("sendfiletype", "edit"), 
                           new XAttribute("sfid", sndf.SFID), 
                           //new XAttribute("rqrorqstrqid", sndf.RQRO_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void DeleteSendFile_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Message
      private void AddMesg_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 53 /* Execute Opt_Mesg_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_MESG_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                     new XElement("Service", 
                        new XAttribute("fileno", serv.FILE_NO), 
                        new XAttribute("msid", 0), 
                        new XAttribute("cellphon", serv.CELL_PHON_DNRM ?? ""),
                        new XAttribute("formcaller", GetType().Name)
                     )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void EditMesg_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;
         var mesg = MesgBs.Current as Data.Message;
         if (mesg == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 53 /* Execute Opt_Mesg_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_MESG_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("tasktype", "edit"), 
                           new XAttribute("msid", mesg.MSID), 
                           new XAttribute("cellphon", serv.CELL_PHON_DNRM ?? ""),
                           //new XAttribute("rqrorqstrqid", mesg.RQRO_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void DeleteMesg_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      private void RqstChngBs_DataSourceChanged(object sender, EventArgs e)
      {
         try
         {
            Activity4_Butn.Visible = Activity4_Txt.Visible = Activity3_Butn.Visible = Activity3_Txt.Visible =
            Activity2_Butn.Visible = Activity2_Txt.Visible = Activity1_Butn.Visible = Activity1_Txt.Visible =
            Activity0_Butn.Visible = Activity0_Txt.Visible = false;

            int i = 0;
            TodayActivity_Txt.Text = GetTextActivity("امروز", CurrentUser, DateTime.Now);

            switch (RqstChngBs.Count)
            {
               case 1:
                  foreach (Data.VF_Request_ChangingResult rqstchng in RqstChngBs.List)
                  {
                     if (i == 0)
                     {
                        Activity2_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity2_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity2_Butn.Visible = Activity2_Txt.Visible = true;
                        Activity2_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                     }
                  }
                  break;
               case 2:
                  foreach (Data.VF_Request_ChangingResult rqstchng in RqstChngBs.List)
                  {
                     if (i == 0)
                     {
                        Activity3_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity3_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity3_Butn.Visible = Activity3_Txt.Visible = true;
                        Activity3_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }

                     else if (i == 1)
                     {
                        Activity1_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity1_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity1_Butn.Visible = Activity1_Txt.Visible = true;
                        Activity1_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }
                  }
                  break;
               case 3:
                  foreach (Data.VF_Request_ChangingResult rqstchng in RqstChngBs.List)
                  {
                     if (i == 0)
                     {
                        Activity3_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity3_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity3_Butn.Visible = Activity3_Txt.Visible = true;
                        Activity3_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }

                     else if (i == 1)
                     {
                        Activity2_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity2_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity2_Butn.Visible = Activity2_Txt.Visible = true;
                        Activity2_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }

                     else if (i == 2)
                     {
                        Activity1_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity1_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity1_Butn.Visible = Activity1_Txt.Visible = true;
                        Activity1_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }
                  }
                  break;
               case 4:
                  foreach (Data.VF_Request_ChangingResult rqstchng in RqstChngBs.List)
                  {
                     if (i == 0)
                     {
                        Activity4_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity4_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity4_Butn.Visible = Activity4_Txt.Visible = true;
                        Activity4_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }

                     else if (i == 1)
                     {
                        Activity3_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity3_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity3_Butn.Visible = Activity3_Txt.Visible = true;
                        Activity3_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }

                     else if (i == 2)
                     {
                        Activity1_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity1_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity1_Butn.Visible = Activity1_Txt.Visible = true;
                        Activity1_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }

                     else if (i == 3)
                     {
                        Activity0_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity0_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity0_Butn.Visible = Activity0_Txt.Visible = true;
                        Activity0_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        ++i;
                     }
                  }
                  break;
               case 5:
                  foreach (Data.VF_Request_ChangingResult rqstchng in RqstChngBs.List)
                  {
                     if (i == 0)
                     {
                        Activity4_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity4_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity4_Butn.Visible = Activity4_Txt.Visible = true;
                        Activity4_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        i++;
                     }

                     else if (i == 1)
                     {
                        Activity3_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity3_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity3_Butn.Visible = Activity3_Txt.Visible = true;
                        Activity3_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        i++;
                     }

                     else if (i == 2)
                     {
                        Activity2_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity2_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity2_Butn.Visible = Activity2_Txt.Visible = true;
                        Activity2_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        i++;
                     }

                     else if (i == 3)
                     {
                        Activity1_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity1_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity1_Butn.Visible = Activity1_Txt.Visible = true;
                        Activity1_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        i++;
                     }

                     else if (i == 4)
                     {
                        Activity0_Butn.ImageProfile = GetImageActivity(rqstchng.RQTP_CODE);
                        Activity0_Txt.Text = GetTextActivity(rqstchng.RQTP_DESC, rqstchng.CRET_BY, (DateTime)rqstchng.SAVE_DATE);
                        Activity0_Butn.Visible = Activity0_Txt.Visible = true;
                        Activity0_Butn.Tag = new XElement("Request", new XAttribute("rqtpcode", rqstchng.RQTP_CODE), new XAttribute("rqid", rqstchng.RQID));
                        i++;
                     }
                  }
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private Image GetImageActivity(string RqtpCode)
      {
         if (RqtpCode == "001")
            return System.CRM.Properties.Resources.IMAGE_1528;
         else if (RqtpCode == "002")
            return System.CRM.Properties.Resources.IMAGE_1531;
         else if (RqtpCode == "003")
            return System.CRM.Properties.Resources.IMAGE_1196;
         else if (RqtpCode == "004")
            return System.CRM.Properties.Resources.IMAGE_1532;
         else if (RqtpCode == "005")
            return System.CRM.Properties.Resources.IMAGE_1526;
         else if (RqtpCode == "006")
            return System.CRM.Properties.Resources.IMAGE_1530;
         else if (RqtpCode == "007")
            return System.CRM.Properties.Resources.IMAGE_1535;
         else if (RqtpCode == "008")
            return System.CRM.Properties.Resources.IMAGE_1533;
         else if (RqtpCode == "009")
            return System.CRM.Properties.Resources.IMAGE_1534;
         else if (RqtpCode == "010")
            return System.CRM.Properties.Resources.IMAGE_1527;
         else if (RqtpCode == "011")
            return System.CRM.Properties.Resources.IMAGE_1544;
         else if (RqtpCode == "012")
            return System.CRM.Properties.Resources.IMAGE_1573;

         return null;
      }
       
      private string GetTextActivity (string rqtpdesc, string user, DateTime savedate)
      {
         return
            string.Format("<b>{0}</b><br><color=Gray><size=8>{1}</size></color><br><color=green><size=8>{2}</size></color><br><color=blue><size=8>{3}</size></color>",
               rqtpdesc,
               user,
               GetPersianDate(savedate),
               GetTimePeriod(savedate)
            );
      }

      private string GetPersianDate(DateTime currentDate)
      {
         PersianCalendar pc = new PersianCalendar();

         return
            string.Format("{0,4:D4}/{1,2:D2}/{2,2:D2} {3}",
               pc.GetYear(currentDate),
               pc.GetMonth(currentDate),
               pc.GetDayOfMonth(currentDate),
               currentDate.ToShortTimeString()
            );
      }

      private string GetGroupTimePriod(DateTime currentdate)
      {
         var result = "نامشخص";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 30 /* Execute DoWork4GetTimePeriod */, SendType.Self)
            {
               Input =
                  new XElement("TimePeriod",
                     new XAttribute("timetype", "group"),
                     new XAttribute("crntdate", currentdate)
                  ),
               AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     if (output != null)
                        result = output.ToString();
                  })
            }
         );

         return result;
      }

      private string GetTimePeriod(DateTime currentdate)
      {
         var result = "نامشخص";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 30 /* Execute DoWork4GetTimePeriod */, SendType.Self)
            {
               Input =
                  new XElement("TimePeriod",
                     new XAttribute("timetype", "normal"),
                     new XAttribute("crntdate", currentdate)
                  ),
               AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     if (output != null)
                        result = output.ToString();
                  })
            }
         );

         return result;
      }

      private void CnclRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;

            if (serv.RQST_RQID != null && MessageBox.Show(this, "آیا با انصراف درخواست موافق هستین?", "انصراف درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.CNCL_RQST_P(
               new XElement("Request",
                  new XAttribute("rqid", serv.RQST_RQID)
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {

         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
            
      }

      private void Rb_RequestDocument_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 42 /* Execute Serv_Dcmt_F */),
                   new Job(SendType.SelfToUserInterface, "SERV_DCMT_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO),                            
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void CallLog_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "LOG_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "LOG_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void Email_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "SEND_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "SEND_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void Note_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "NOTE_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "NOTE_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void Task_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "DUE_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "DUE_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void Appointment_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "FROM_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "FROM_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void File_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "SEND_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "SEND_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void Mesg_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "MESG_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "MESG_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void RqstRelatedService_Butn_Click(object sender, EventArgs e)
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

      private void Tag_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 50 /* Execute Tsk_Tag_F */),
                  new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
                        )
                  }
               }
            )
         );
      }

      private void ServCont_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 60 /* Execute Inf_Ctwk_F */),
                  new Job(SendType.SelfToUserInterface, "INF_CTWK_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", serv.FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void AddInfo_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 71 /* Execute Add_Info_F */),
                  new Job(SendType.SelfToUserInterface, "ADD_INFO_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", fileno)
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
                           new XAttribute("section", "note")
                        )
                  }
               }
            )
         );
      }

      private void Lb_Comp_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 39 /* Execute Inf_Acnt_F */),                
                   new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Company", new XAttribute("code", serv.COMP_CODE_DNRM))},
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void Gis_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         //if (serv.CORD_X_DNRM == 0.0 && serv.CORD_Y_DNRM == 0.0) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
            {
               Input =
                  new XElement("GMapNets",
                     new XAttribute("requesttype", "get"),
                     new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                     new XAttribute("callback", 40 /* CordinateGetSet */),
                     new XAttribute("outputtype", "servcord"),
                     new XAttribute("initalset", true),
                     new XAttribute("cordx", serv.CORD_X_DNRM == null ? "29.622045" : serv.CORD_X_DNRM.ToString()),
                     new XAttribute("cordy", serv.CORD_Y_DNRM == null ? "52.522728" : serv.CORD_Y_DNRM.ToString()),
                     new XAttribute("zoom", "1800")
                  )
            }
         );
      }

      private void RelationShip_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute RLAT_SINF_F */),
                  new Job(SendType.SelfToUserInterface, "RLAT_SINF_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", serv.FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void Next_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* GetUi */){Input = "shw_lead_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 11 /* Execute GetNewRecord */)
                  {
                     Input =
                        new XElement("Service",
                           new XAttribute("moveposition", "next")
                        )
                  }
               }
            )
         );         
      }

      private void Previous_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* GetUi */){Input = "shw_lead_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 11 /* Execute GetNewRecord */)
                  {
                     Input =
                        new XElement("Service",
                           new XAttribute("moveposition", "previous")
                        )
                  }
               }
            )
         );
      }

      private void JoinToComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 38 /* Execute Shw_Cont_F */),
                   new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 10 /* Execute Actn_CalF_P */)
                   {
                      Executive = ExecutiveType.Asynchronous,
                      Input = 
                        new XElement("Company", 
                           new XAttribute("onoftag", "on"),
                           new XAttribute("actntype", "join"),
                           new XAttribute("fileno", serv.FILE_NO),
                           new XAttribute("formcaller", GetType().Name)
                        )
                   }
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }
   }
}
