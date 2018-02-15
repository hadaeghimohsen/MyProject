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

namespace System.CRM.Ui.Contacts
{
   public partial class INF_CONT_F : UserControl
   {
      public INF_CONT_F()
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

         MsttBs.DataSource = iCRM.Main_States;
         //RqstChngBs.DataSource = iCRM.VF_Request_Changing(null, fileno, null, null).OrderByDescending(r => r.SAVE_DATE).Take(5);
         //PymtSaveBs.DataSource = iCRM.VF_Save_Payments(null, fileno, null);
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

         // 1396/09/27 * خالی کردن تمامی اطلاعات داخل فرم
         NoteBs.List.Clear();
         TaskBs.List.Clear();
         AponBs.List.Clear();
         FileBs.List.Clear();
         MesgBs.List.Clear();
         LogcBs.List.Clear();
         EmalBs.List.Clear();
         PymtSaveBs.List.Clear();

         Activity4_Butn.Visible = Activity4_Txt.Visible = Activity3_Butn.Visible = Activity3_Txt.Visible =
         Activity2_Butn.Visible = Activity2_Txt.Visible = Activity1_Butn.Visible = Activity1_Txt.Visible =
         Activity0_Butn.Visible = Activity0_Txt.Visible = false;

         var projrqst = RqstProjBs.Position;
         var shis = ShisBs.Position;
         var shid = ShidBs.Position;

         RqstProjBs.DataSource =
            iCRM.Requests.Where(t =>
               t.RQTP_CODE == "013" &&
               t.RQTT_CODE == "004" &&
               //t.Request_Rows.Any(rr => rr.SERV_FILE_NO == fileno) &&
               t.Request_Rows.FirstOrDefault(rr => rr.SERV_FILE_NO == fileno).Service_Projects.Any(sp => sp.REC_STAT == "002" && sp.Job_Personnel_Relation.Job_Personnel.USER_NAME == CurrentUser)
            );

         RqstProjBs.Position = projrqst;
         ShisBs.Position = shis;
         ShidBs.Position = shid;
      }

      #region Note
      private void AddNote_Butn_Click(object sender, EventArgs e)
      {
         /*NoteBs.AddNew();*/
         /*NoteBs.AddNew();*/
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                        new XAttribute("projrqstrqid", rqstproj.RQID),
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
         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                           new XAttribute("projrqstrqid", rqstproj.RQID),
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

            var rqstproj = RqstProjBs.Current as Data.Request;
            if (rqstproj == null) return;

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
                           new XAttribute("projrqstrqid", rqstproj.RQID),
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
            if (serv == null || MessageBox.Show(this, "آیا با تغییر نوع مشتری موافق هستید؟", "تغییر نوع مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.CHNG_SRTP_P(
               new XElement("Service",
                  new XAttribute("fileno", serv.FILE_NO),
                  new XAttribute("type", serv.SRPB_TYPE_DNRM == "001" ? "002" : "001"),
                  new XAttribute("frstname", serv.FRST_NAME_DNRM),
                  new XAttribute("lastname", serv.LAST_NAME_DNRM),
                  
                  new XAttribute("emaladdr", serv.EMAL_ADRS_DNRM),
                  new XAttribute("compcode", serv.COMP_CODE_DNRM)
               )
            );
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
               Back_Butn_Click(null, null);
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {                        
                        new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),
                        new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", fileno))},
                     }
                  )
               );
            }
         }
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
                        new Job(SendType.Self, 33 /* Execute Shw_Cont_F */),
                        new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 10 /* Execute Actn_CalF_P */)
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

            var rqstproj = RqstProjBs.Current as Data.Request;
            if (rqstproj == null) return;

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
                           new XAttribute("projrqstrqid", rqstproj.RQID),
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
         if (serv == null) return;

         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 13 /* Execute Adm_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Service", new XAttribute("type", "changeinfo"), new XAttribute("fileno", fileno), new XAttribute("auto", "true"), new XAttribute("srpbtype", serv.SRPB_TYPE_DNRM), new XAttribute("projrqstrqid", rqstproj.RQID))}
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

      private void EditLogCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var logc = LogcBs.Current as Data.Log_Call;
            if (logc == null) return;

            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            var rqstproj = RqstProjBs.Current as Data.Request;
            if (rqstproj == null) return;

            var _InteractWithCRM =
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
                        new XAttribute("projrqstrqid", rqstproj.RQID),
                        new XAttribute("formcaller", GetType().Name)
                     )
                  },
              });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
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

      private void EditEmal_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var emal = EmalBs.Current as Data.Email;
            if (emal == null) return;

            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            var rqstproj = RqstProjBs.Current as Data.Request;
            if (rqstproj == null) return;

            var _InteractWithCRM =
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
                          new XAttribute("projrqstrqid", rqstproj.RQID),
                          new XAttribute("formcaller", GetType().Name)
                       )
                    },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #endregion 

      #region Task
      private void AddTask_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                                 new XAttribute("projrqstrqid", rqstproj.RQID),
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
         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;


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
                           new XAttribute("projrqstrqid", rqstproj.RQID),
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

         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;


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
                        new XAttribute("projrqstrqid", rqstproj.RQID),
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
         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                           new XAttribute("projrqstrqid", rqstproj.RQID),
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

         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                        new XAttribute("projrqstrqid", rqstproj.RQID),
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
         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                           new XAttribute("rqrorqstrqid", sndf.RQRO_RQST_RQID),
                           new XAttribute("projrqstrqid", rqstproj.RQID),
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

         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                        new XAttribute("projrqstrqid", rqstproj.RQID),
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
         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

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
                           new XAttribute("msid", mesg.MSID), 
                           new XAttribute("cellphon", serv.CELL_PHON_DNRM ?? ""),
                           new XAttribute("projrqstrqid", rqstproj.RQID),
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

      #region Deals
      private void AddDeals_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            var rqstproj = RqstProjBs.Current as Data.Request;
            if (rqstproj == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 35 /* Execute Tol_Deal_F */),
                   new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", serv.FILE_NO), 
                           new XAttribute("cashcode", 0), 
                           new XAttribute("rqid", 0), 
                           new XAttribute("projrqstrqid", rqstproj.RQID),
                           new XAttribute("pymtdesc", rqstproj.RQST_DESC),
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void Pymt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqstproj = RqstProjBs.Current as Data.Request;
            if (rqstproj == null) return;

            var pymt = PymtSaveBs.Current as Data.VF_Save_PaymentsResult;
            switch (e.Button.Index)
            {
               case 0:
                  #region Add Deals
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
                                 new XAttribute("rqstrqid", pymt.RQID),
                                 new XAttribute("emid", 0),
                                 new XAttribute("projrqstrqid", rqstproj.RQID),
                                 new XAttribute("formcaller", GetType().Name),
                                 new XAttribute("toemail", serv.EMAL_ADRS_DNRM ?? "")
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 1:
                  #region Add Call Log
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
                                 new XAttribute("rqstrqid", pymt.RQID),
                                 new XAttribute("lcid", 0),
                                 new XAttribute("projrqstrqid", rqstproj.RQID),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 2:
                  #region Edit Payment
                  serv = ServBs.Current as Data.Service;
                  if (serv == null) return;

                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 35 /* Execute Tol_Deal_F */),
                         new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("cashcode", pymt.CASH_CODE), 
                                 new XAttribute("rqid", pymt.RQID), 
                                 new XAttribute("projrqstrqid", rqstproj.RQID),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 3:
                  #region Delete Payment
                  //if (pymt == null || MessageBox.Show(this, "آیا با حذف تماس تلفنی موافق هستید؟", "حذف تماس نلفنی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  //iCRM.Log_Calls.DeleteOnSubmit(pymt);
                  //iCRM.SubmitChanges();
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

      private void EditDeal_Butn_Click(object sender, EventArgs e)
      {
         Pymt_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Pymt_Butn.Buttons[2]));
      }

      private void DelDeal_Butn_Click(object sender, EventArgs e)
      {
         Pymt_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Pymt_Butn.Buttons[3]));
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

      private string GetTextActivity(string rqtpdesc, string user, DateTime savedate)
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

      private void PymtSaveBs_DataSourceChanged(object sender, EventArgs e)
      {
         try
         {
            var amntunittypecode = "001";
            var amntunittypedesc = iCRM.D_ATYPs.FirstOrDefault(d => d.VALU == amntunittypecode).DOMN_DESC;

            PymtNotEnd_Lb.Text =
               string.Format(
                  "<size=9>{0}</size><size=10>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=red>معامله نا تمام</color></size> <size=8><color=red>{3}</color></size>",
                  amntunittypedesc,
                  0,
                  0,
                  0
               );
            PymtEnd_Lb.Text = 
               string.Format(
                  "<size=9>{0}</size><size=10>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=green>معامله انجام شده</color></size> <size=8><color=green>{3}</color></size>", 
                  amntunittypedesc,
                  0,
                  0,
                  0
               );
            if (PymtSaveBs.Count == 0) return;

            amntunittypecode = (PymtSaveBs.Current as Data.VF_Save_PaymentsResult).AMNT_UNIT_TYPE_DNRM;
            amntunittypedesc = iCRM.D_ATYPs.FirstOrDefault(d => d.VALU == amntunittypecode).DOMN_DESC;

            PymtNotEnd_Lb.Text =
               string.Format(
                  "<size=9>{0}</size><size=10>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=red>معامله نا تمام</color></size> <size=8><color=red>{3}</color></size>",
                  amntunittypedesc,
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG != "007" && p.PYMT_STAG != "006").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"),
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG != "007" && p.PYMT_STAG != "006").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"),
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG != "007" && p.PYMT_STAG != "006").Count()
               );
            PymtEnd_Lb.Text =
               string.Format(
                  "<size=9>{0}</size><size=10>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=green>معامله انجام شده</color></size> <size=8><color=green>{3}</color></size>",
                  amntunittypedesc,
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "006").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - ((int)p.SUM_PYMT_DSCN_DNRM)).ToString("n0"),
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "006").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"),
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "006").Count()
               );

         }
         catch (Exception exc)
         {

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

      private void ChangeTarf_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 54 /* Execute Chg_Tarf_F */),
                   new Job(SendType.SelfToUserInterface, "CHG_TARF_F", 10 /* Execute ACTN_CALF_P */)
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
      }

      private void RqstRelatedService_Butn_Click(object sender, EventArgs e)
      {
         var xinput = (sender as RoundedButton).Tag as XElement;
         var rqstproj = RqstProjBs.Current as Data.Request;
         if (rqstproj == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
            {
               Input =
                  new XElement("Request",
                     new XAttribute("rqtpcode", xinput.Attribute("rqtpcode").Value),
                     new XAttribute("rqid", xinput.Attribute("rqid").Value),
                     new XAttribute("projrqstrqid", rqstproj.RQID)
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

      private void ServCont_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.Service;
         if (serv == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 60 /* Execute Inf_Serv_F */),
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

      private void Deal_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "PYMT_CRET_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "PYMT_CRET_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
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
                  new Job(SendType.Self, 01 /* GetUi */){Input = "shw_cont_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 11 /* Execute GetNewRecord */)
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
                  new Job(SendType.Self, 01 /* GetUi */){Input = "shw_cont_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 11 /* Execute GetNewRecord */)
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

      private void NewProj_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 88 /* Execute Opt_Prjt_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_PRJT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Project",
                              new XAttribute("fileno", fileno),
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("rqid", 0)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void EditProj_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstProjBs.Current as Data.Request;
            if (rqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 88 /* Execute Opt_Prjt_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_PRJT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Project",
                              new XAttribute("fileno", fileno),
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("rqid", rqst.RQID)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstProjBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pr = RqstProjBs.Current as Data.Request;
            if (pr == null) return;

            NoteBs.DataSource = iCRM.Notes.Where(n => n.Request_Row.SERV_FILE_NO == fileno && n.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            TaskBs.DataSource = iCRM.Tasks.Where(t => t.Request_Row.SERV_FILE_NO == fileno && t.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            AponBs.DataSource = iCRM.Appointments.Where(a => a.Request_Row.SERV_FILE_NO == fileno && a.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            FileBs.DataSource = iCRM.Send_Files.Where(sf => sf.Request_Row.SERV_FILE_NO == fileno && sf.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            MesgBs.DataSource = iCRM.Messages.Where(m => m.Request_Row.SERV_FILE_NO == fileno && m.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            LogcBs.DataSource = iCRM.Log_Calls.Where(l => l.Request_Row.SERV_FILE_NO == fileno && l.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            EmalBs.DataSource = iCRM.Emails.Where(em => em.Request_Row.SERV_FILE_NO == fileno && em.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);

            RqstChngBs.DataSource = iCRM.VF_Request_Changing(null, fileno, null, pr.RQID).OrderByDescending(r => r.SAVE_DATE).Take(5);
            PymtSaveBs.DataSource = iCRM.VF_Save_Payments(null, fileno, null, pr.RQID);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveMsttSstt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            MsttBs.EndEdit();
            SsttBs.EndEdit();

            Shis_Gv.PostEditor();
            Shid_Gv.PostEditor();

            iCRM.SubmitChanges();

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

            }
         }
      }

      private void ShisBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var shis = ShisBs.Current as Data.Step_History_Summery;
            if (shis == null) return;

            SsttBs.DataSource = iCRM.Sub_States.Where(s => s.MSTT_CODE == shis.SSTT_MSTT_CODE);
         }
         catch (Exception exc)
         {}
      }

      private void AddMstt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 89 /* Execute Mstt_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "MSTT_DFIN_F", 10 /* Execute Actn_CalF_P */) 
                     {
                        Input = 
                           new XElement("MainSub_Stat",
                              new XAttribute("formcaller", GetType().Name)                           
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MainStat_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  var msttcode = (short?)MainStat_Lov.EditValue;
                  if (msttcode == null) return;
                  var projrqst = RqstProjBs.Current as Data.Request;
                  if (projrqst == null) return;

                  ShisBs.AddNew();

                  var shis = ShisBs.Current as Data.Step_History_Summery;
                  shis.RQST_RQID = projrqst.RQID;
                  shis.SSTT_MSTT_CODE = (short)msttcode;
                  shis.SSTT_MSTT_SUB_SYS = 1;

                  ShisBs.EndEdit();

                  iCRM.SubmitChanges();
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void SubStat_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  var shis = ShisBs.Current as Data.Step_History_Summery;
                  if (shis == null) return;

                  var ssttcode = (short?)SubStat_Lov.EditValue;
                  if (ssttcode == null) return;
                  var projrqst = RqstProjBs.Current as Data.Request;
                  if (projrqst == null) return;

                  ShidBs.AddNew();

                  var shid = ShidBs.Current as Data.Step_History_Detail;
                  shid.SHIS_RQST_RQID = projrqst.RQID;
                  shid.SHIS_RWNO = shis.RWNO;
                  shid.SSTT_CODE = (short)ssttcode;
                  shid.SSTT_MSTT_SUB_SYS = 1;
                  shid.SSTT_MSTT_CODE = shis.SSTT_MSTT_CODE;

                  ShidBs.EndEdit();

                  iCRM.SubmitChanges();
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void MoreInfo_Butn_Click(object sender, EventArgs e)
      {         
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 91 /* Execute Opt_Info_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_INFO_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Object",
                           new XAttribute("fileno", fileno),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }                     
               }
            )
         );
      }

      private void ImageProfile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = iCRM.VF_Request_Changing(null, fileno, null, null).FirstOrDefault(r => r.RQTP_CODE == "001");
            if (rqst == null) return;            

            var result = (
                     from r in iCRM.Regulations
                     join rqrq in iCRM.Request_Requesters on r equals rqrq.Regulation
                     join rqdc in iCRM.Request_Documents on rqrq equals rqdc.Request_Requester
                     join rcdc in iCRM.Receive_Documents on rqdc equals rcdc.Request_Document
                     where r.TYPE == "001"
                        && r.REGL_STAT == "002"
                        && rqrq.RQTP_CODE == rqst.RQTP_CODE
                        && rqrq.RQTT_CODE == rqst.RQTT_CODE
                        && rqdc.DCMT_DSID == 13962055684640 // عکس 4*3
                        && rcdc.RQRO_RQST_RQID == rqst.RQID
                        && rcdc.RQRO_RWNO == 1
                     select rcdc).FirstOrDefault();
            if (result == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self,  41 /* Execute Serv_Camr_F */),
                     new Job(SendType.SelfToUserInterface, "SERV_CAMR_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = result                           
                     }
                  }
               )
            );

         }
         catch { }
      }
   }
}
