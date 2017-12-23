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
using System.Drawing.Imaging;
using DevExpress.XtraGrid.Views.Grid;
using System.MaxUi;

namespace System.CRM.Ui.Acounts
{
   public partial class INF_ACNT_F : UserControl
   {
      public INF_ACNT_F()
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
         CompBs.DataSource = iCRM.Companies.FirstOrDefault(c => c.CODE == compcode );
         //RqstChngBs.DataSource = iCRM.VF_Request_Changing(null, null, compcode, null).OrderByDescending(r => r.SAVE_DATE).Take(5);
         //PymtSaveBs.DataSource = iCRM.VF_Save_Payments(null, null, compcode, null);
         MsttBs.DataSource = iCRM.Main_States;
         requery = false;
      }

      private void CompBs_CurrentChanged(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;

         if (comp == null) return;

         if (comp.FACE_BOOK_URL == null || comp.FACE_BOOK_URL == "")
            FacebookUrl_Butn.Enabled = false;
         else
            FacebookUrl_Butn.Enabled = true;

         if (comp.LINK_IN_URL == null || comp.LINK_IN_URL == "")
            LinkedInUrl_Butn.Enabled = false;
         else
            LinkedInUrl_Butn.Enabled = true;

         if (comp.TWTR_URL == null || comp.TWTR_URL == "")
            TwitterUrl_Butn.Enabled = false;
         else
            TwitterUrl_Butn.Enabled = true;
         
         if (comp.RECD_STAT == "001")
            DeleteRecycleCompany_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1541;
         else
            DeleteRecycleCompany_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1518;

         try
         {
            if (comp.LOGO == null)
            {
               byte[] bytes = null;
               MemoryStream ms = new MemoryStream();
               Image img = Properties.Resources.IMAGE_1565;
               img.Save(ms, ImageFormat.Bmp);
               bytes = ms.ToArray();

               //comp.LOGO = bytes;
               CompanyLogo_Pb.ImageProfile = Properties.Resources.IMAGE_1565;
            }
            else
            {
               var stream = new MemoryStream(comp.LOGO.ToArray());
               CompanyLogo_Pb.ImageProfile = Image.FromStream(stream);
            }
         }
         catch {
            CompanyLogo_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
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
               //t.Request_Rows.Any(rr => rr.COMP_CODE == compcode) &&
               t.Request_Rows.FirstOrDefault(rr => rr.COMP_CODE == compcode).Service_Projects.Any(sp => sp.REC_STAT == "002" && sp.Job_Personnel_Relation.Job_Personnel.USER_NAME == CurrentUser)
            );

         RqstProjBs.Position = projrqst;
         ShisBs.Position = shis;
         ShidBs.Position = shid;
      }

      #region Note
      private void NoteBs_CurrentChanged(object sender, EventArgs e)
      {
         var note = NoteBs.Current as Data.Note;
         if (note == null) return;

         NoteRelatedService_Pb.ImageVisiable = true;

         try
         {
            NoteRelatedService_Pb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", note.SERV_FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            NoteRelatedService_Pb.ImageProfile = bm;
         }
         catch
         {
            NoteRelatedService_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
      }

      private void EditNote_Butn_Click(object sender, EventArgs e)
      {
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
                           new XAttribute("fileno", note.SERV_FILE_NO), 
                           new XAttribute("ntid", note.NTID), 
                           new XAttribute("rqrorqstrqid", note.RQRO_RQST_RQID),
                           new XAttribute("projrqstrqid", note.Request_Row.Request.PROJ_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      #endregion

      #region Company Button
      private void AddLogCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var comp = CompBs.Current as Data.Company;
            if (comp == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", comp.LAST_SERV_FILE_NO_DNRM), 
                           new XAttribute("lcid", 0),
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }      

      private void DeleteRecycleCompany_Butn_Click(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;
         try
         {            
            if (comp.RECD_STAT == "002")
            {
               if (comp == null || MessageBox.Show(this, "آیا با حذف شرکت موافق هستید؟", "حذف شرکت", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               iCRM.ONF_COMP_P(
                  new XElement("Company",
                     new XAttribute("code", comp.CODE)
                  )
               );
            }
            else
            {
               if (comp == null || MessageBox.Show(this, "آیا با بازیابی شرکت موافق هستید؟", "بازیابی شرکت", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               iCRM.ONF_COMP_P(
                  new XElement("Company",
                     new XAttribute("code", comp.CODE)
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
                        new Job(SendType.Self, 38 /* Execute Shw_Acnt_F */),
                        new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Executive = ExecutiveType.Asynchronous,
                           Input = 
                              new XElement("Company", 
                                 new XAttribute("onoftag", comp.RECD_STAT == "001" ? "on" : "off")
                              ) 
                        }
                     }
                  );
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
      }

      private void AddSubCompany_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var comp = CompBs.Current as Data.Company;
            if (comp == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 46 /* Execute Opt_Clnc_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_CLNC_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Company", 
                           new XAttribute("code", comp.CODE), 
                           new XAttribute("formcaller", GetType().Name)
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void SendEmailLastRelatedService_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var comp = CompBs.Current as Data.Company;
            if (comp == null) return;
            if (comp.LAST_SERV_FILE_NO_DNRM == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                   new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", comp.LAST_SERV_FILE_NO_DNRM), 
                           new XAttribute("emid", 0),
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("toemail", comp.Service.EMAL_ADRS_DNRM ?? "")
                        )
                   },
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void EditCompany_Butn_Click(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 47 /* Execute Regn_Dfin_F */),                
                new Job(SendType.SelfToUserInterface, "COMP_CHNG_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Company", 
                        new XAttribute("formtype", "edit"),
                        new XAttribute("code", comp.CODE),
                        new XAttribute("formcaller", GetType().Name)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
      #endregion

      #region LogCall
      private void LogcActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var logc = LogcBs.Current as Data.Log_Call;
            switch (e.Button.Index)
            {
               case 0:
                  #region Add Task
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 26 /* Execute Opt_Logc_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", logc.SERV_FILE_NO), 
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

                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", logc.SERV_FILE_NO), 
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
                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", logc.SERV_FILE_NO), 
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

      private void LogcBs_CurrentChanged(object sender, EventArgs e)
      {
         var logc = LogcBs.Current as Data.Log_Call;
         if (logc == null) return;

         LogCallRelatedService_Pb.ImageVisiable = true;

         try
         {
            LogCallRelatedService_Pb.ImageProfile = null;
            
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", logc.SERV_FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            LogCallRelatedService_Pb.ImageProfile = bm;
         }
         catch
         {
            LogCallRelatedService_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
      }

      private void EditLogc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var logc = LogcBs.Current as Data.Log_Call;
            if (logc == null) return;

            var _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
              {                  
                  new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                  new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                  {
                     Input = 
                     new XElement("Service", 
                        new XAttribute("fileno", logc.SERV_FILE_NO), 
                        new XAttribute("lcid", logc.LCID),
                        new XAttribute("projrqstrqid", logc.Request_Row.Request.PROJ_RQST_RQID),
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
      private void EmalBs_CurrentChanged(object sender, EventArgs e)
      {
         var emal = EmalBs.Current as Data.Email;
         if (emal == null) return;

         EmailRelatedService_Pb.ImageVisiable = true;

         try
         {
            EmailRelatedService_Pb.ImageProfile = null;
            
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", emal.SERV_FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            EmailRelatedService_Pb.ImageProfile = bm;
         }
         catch
         {
            EmailRelatedService_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
      }

      private void EmalActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var emal = EmalBs.Current as Data.Email;
            switch (e.Button.Index)
            {
               case 0:
                  #region Add Task
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 26 /* Execute Opt_Task_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", emal.SERV_FILE_NO), 
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
                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", emal.SERV_FILE_NO), 
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

                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", emal.SERV_FILE_NO), 
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

            var _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                    new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                    new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                    {
                       Input = 
                       new XElement("Service", 
                          new XAttribute("fileno", emal.SERV_FILE_NO), 
                          new XAttribute("emid", emal.EMID),
                          new XAttribute("projrqstrqid", emal.Request_Row.Request.PROJ_RQST_RQID),
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
      private void EditTask_Butn_Click(object sender, EventArgs e)
      {
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
                           new XAttribute("fileno", task.SERV_FILE_NO), 
                           new XAttribute("tasktype", "edit"), 
                           new XAttribute("tkid", task.TKID), 
                           new XAttribute("rqrorqstrqid", task.RQRO_RQST_RQID),
                           new XAttribute("projrqstrqid", task.Request_Row.Request.PROJ_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void TaskBs_CurrentChanged(object sender, EventArgs e)
      {
         var task = TaskBs.Current as Data.Task;
         if (task == null) return;

         TaskRelatedService_Pb.ImageVisiable = true;

         try
         {
            TaskRelatedService_Pb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", task.SERV_FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            TaskRelatedService_Pb.ImageProfile = bm;
         }
         catch
         {
            TaskRelatedService_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
      }
      #endregion

      #region Appointment
      private void EditAppointment_Butn_Click(object sender, EventArgs e)
      {
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
                           new XAttribute("fileno", apon.SERV_FILE_NO), 
                           new XAttribute("appointmenttype", "edit"), 
                           new XAttribute("apid", apon.APID), 
                           new XAttribute("rqrorqstrqid", apon.RQRO_RQST_RQID),
                           new XAttribute("projrqstrqid", apon.Request_Row.Request.PROJ_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void AponBs_CurrentChanged(object sender, EventArgs e)
      {
         var apon = AponBs.Current as Data.Appointment;
         if (apon == null) return;

         AppointmentRelatedService_Pb.ImageVisiable = true;

         try
         {
            AppointmentRelatedService_Pb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", apon.SERV_FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            AppointmentRelatedService_Pb.ImageProfile = bm;
         }
         catch
         {
            AppointmentRelatedService_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
      }
      #endregion

      #region Send File
      private void EditSendFile_Butn_Click(object sender, EventArgs e)
      {
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
                           new XAttribute("fileno", sndf.SERV_FILE_NO), 
                           new XAttribute("sendfiletype", "edit"), 
                           new XAttribute("sfid", sndf.SFID), 
                           new XAttribute("rqrorqstrqid", sndf.RQRO_RQST_RQID),
                           new XAttribute("projrqstrqid", sndf.Request_Row.Request.PROJ_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void FileBs_CurrentChanged(object sender, EventArgs e)
      {
         var file = FileBs.Current as Data.Send_File;
         if (file == null) return;

         SendFileRelatedService_Pb.ImageVisiable = true;

         try
         {
            SendFileRelatedService_Pb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", file.SERV_FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            SendFileRelatedService_Pb.ImageProfile = bm;
         }
         catch
         {
            SendFileRelatedService_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
      }
      #endregion

      #region Message
      private void EditMesg_Butn_Click(object sender, EventArgs e)
      {
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
                           new XAttribute("fileno", mesg.SERV_FILE_NO), 
                           new XAttribute("msid", mesg.MSID),                            
                           new XAttribute("projrqstrqid", mesg.Request_Row.Request.PROJ_RQST_RQID),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  },
               });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void MesgBs_CurrentChanged(object sender, EventArgs e)
      {
         var mesg = MesgBs.Current as Data.Message;
         if (mesg == null) return;

         MessageRelatedService_Pb.ImageVisiable = true;

         try
         {
            MessageRelatedService_Pb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", mesg.SERV_FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            MessageRelatedService_Pb.ImageProfile = bm;
         }
         catch
         {
            MessageRelatedService_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
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

      //private string GetTimePeriod(DateTime currentdate)
      //{
      //   DateDiff datediff = new DateDiff(currentdate, DateTime.Now);

      //   string result = "";

      //   // زمان گذشته
      //   if (currentdate <= DateTime.Now)
      //   {
      //      if (datediff.ElapsedYears > 0)
      //         return string.Format("مدت زمان {0} سال قبل", datediff.ElapsedYears);

      //      if (datediff.ElapsedMonths > 0)
      //         return string.Format("مدت زمان {0} ماه قبل", datediff.ElapsedMonths);

      //      if (datediff.ElapsedDays >= 2)
      //         return string.Format("مدت زمان {0} روز قبل", datediff.ElapsedDays);

      //      if (datediff.ElapsedDays == 1)
      //         return "دیروز";

      //      if (datediff.ElapsedHours > 0)
      //         return string.Format("مدت زمان {0} ساعت قبل", datediff.ElapsedHours);

      //      if (datediff.ElapsedMinutes > 0)
      //         return string.Format("مدت زمان {0} دقیقه قبل", datediff.ElapsedMinutes);

      //      if (datediff.ElapsedSeconds > 0)
      //         return string.Format("مدت زمان {0} ثانیه قبل", datediff.ElapsedSeconds);

      //      return "همین الان";
      //   }
      //   // زمان آینده 
      //   else
      //   {
      //      if (datediff.ElapsedYears > 0)
      //         return string.Format("مدت زمان {0} سال دیگه", datediff.ElapsedYears);

      //      if (datediff.ElapsedMonths > 0)
      //         return string.Format("مدت زمان {0} ماه دیگه", datediff.ElapsedMonths);

      //      if (datediff.ElapsedDays >= 2)
      //         return string.Format("مدت زمان {0} روز دیگه", datediff.ElapsedDays);

      //      if (datediff.ElapsedDays == 1)
      //         return "فردا";

      //      if (datediff.ElapsedHours > 0)
      //         return string.Format("مدت زمان {0} ساعت دیگه", datediff.ElapsedHours);

      //      if (datediff.ElapsedMinutes > 0)
      //         return string.Format("مدت زمان {0} دقیقه دیگه", datediff.ElapsedMinutes);

      //      if (datediff.ElapsedSeconds > 0)
      //         return string.Format("مدت زمان {0} ثانیه دیگه", datediff.ElapsedSeconds);
      //   }

      //   return result;
      //}

      private void AddDeals_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var comp = CompBs.Current as Data.Company;
            if (comp == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 35 /* Execute Tol_Deal_F */),
                   new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 10 /* Execute ACTN_CALF_P */)
                   {
                      Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", 0), 
                           new XAttribute("compcode", comp.CODE), 
                           new XAttribute("cashcode", 0), 
                           new XAttribute("rqid", 0), 
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
                  #region Add Email
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", pymt.SERV_FILE_NO), 
                                 new XAttribute("rqstrqid", pymt.RQID),
                                 new XAttribute("emid", 0),
                                 new XAttribute("formcaller", GetType().Name),
                                 new XAttribute("toemail", iCRM.Services.FirstOrDefault(s => s.FILE_NO == pymt.SERV_FILE_NO).EMAL_ADRS_DNRM ?? "")
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 1:
                  #region Add Call Log
                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                         new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", pymt.SERV_FILE_NO), 
                                 new XAttribute("rqstrqid", pymt.RQID),
                                 new XAttribute("lcid", 0),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 2:
                  #region Edit Payment
                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 35 /* Execute Tol_Deal_F */),
                         new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", pymt.SERV_FILE_NO), 
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
                  #region Delete Log Call
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

      private void PymtSaveBs_DataSourceChanged(object sender, EventArgs e)
      {
         try
         {
            var amntunittypecode = "001";
            var amntunittypedesc = iCRM.D_ATYPs.FirstOrDefault(d => d.VALU == amntunittypecode).DOMN_DESC;

            PymtNotEnd_Lb.Text =
               string.Format(
                  "<size=9>{0}</size> <size=12>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=red>معامله نا تمام</color></size> <size=8><color=red>{3}</color></size>",
                  amntunittypedesc,
                  0,
                  0,
                  0
               );
            PymtEnd_Lb.Text = 
               string.Format(
                  "<size=9>{0}</size> <size=12>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=green>معامله انجام شده</color></size> <size=8><color=green>{3}</color></size>", 
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
                  "<size=9>{0}</size> <size=12>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=red>معامله نا تمام</color></size> <size=8><color=red>{3}</color></size>",
                  amntunittypedesc,
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG != "007" && p.PYMT_STAG != "006").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"),
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG != "007" && p.PYMT_STAG != "006").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"),
                  PymtSaveBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG != "007" && p.PYMT_STAG != "006").Count()
               );
            PymtEnd_Lb.Text =
               string.Format(
                  "<size=9>{0}</size> <size=12>{1}</size> (<size=8><color=blue>{2}</color></size>)<br><size=8><color=green>معامله انجام شده</color></size> <size=8><color=green>{3}</color></size>",
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

      private void Pymt_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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

      private void Apon_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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

      private void BillCord_Butn_Click(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;
         if (comp == null) return;

         if (comp.BILL_ADDR_X == 0.0 && comp.BILL_ADDR_Y == 0.0) return; 

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
            {
               Input =
                  new XElement("GMapNets",
                     new XAttribute("requesttype", "queryonly"),
                     new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                     new XAttribute("callback", 40 /* CordinateGetSet */),
                     new XAttribute("outputtype", "billaddress"),
                     new XAttribute("initalset", true),
                     new XAttribute("cordx", comp.BILL_ADDR_X == null ? "29.622045" : comp.BILL_ADDR_X.ToString()),
                     new XAttribute("cordy", comp.BILL_ADDR_Y == null ? "52.522728" : comp.BILL_ADDR_Y.ToString()),
                     new XAttribute("zoom", comp.BILL_ADDR_ZOOM == null ? "1800" : comp.BILL_ADDR_ZOOM.ToString())
                  )
            }
         );
      }

      private void ShipCord_Butn_Click(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;
         if (comp == null) return;

         if (comp.SHIP_ADDR_X == 0.0 && comp.SHIP_ADDR_Y == 0.0) return; 

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
            {
               Input =
                  new XElement("GMapNets",
                     new XAttribute("requesttype", "queryonly"),
                     new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                     new XAttribute("callback", 40 /* CordinateGetSet */),
                     new XAttribute("outputtype", "shippingaddress"),
                     new XAttribute("initalset", true),
                     new XAttribute("cordx", comp.SHIP_ADDR_X == null ? "29.622045" : comp.SHIP_ADDR_X.ToString()),
                     new XAttribute("cordy", comp.SHIP_ADDR_Y == null ? "52.522728" : comp.SHIP_ADDR_Y.ToString()),
                     new XAttribute("zoom", comp.SHIP_ADDR_ZOOM == null ? "1800" : comp.SHIP_ADDR_ZOOM.ToString())
                  )
            }
         );
      }

      private void AddNewService_Butn_Click(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;

         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.Self, 58 /* Execute Add_Serv_F */),
         //         new Job(SendType.SelfToUserInterface, "ADD_SERV_F", 10 /* Execute Actn_CalF_P */)
         //         {
         //            Input = new XElement("Request", new XAttribute("formcaller", GetType().Name), new XAttribute("compcode", comp.CODE), new XAttribute("regncode", comp.REGN_CODE), new XAttribute("prvncode", comp.REGN_PRVN_CODE), new XAttribute("cntycode", comp.REGN_PRVN_CNTY_CODE))
         //         }
         //      }
         //   )
         //);
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),                
                new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Request", new XAttribute("srpbtype", "002"), new XAttribute("formcaller", GetType().Name), new XAttribute("compcode", comp.CODE), new XAttribute("regncode", comp.REGN_CODE), new XAttribute("prvncode", comp.REGN_PRVN_CODE), new XAttribute("cntycode", comp.REGN_PRVN_CNTY_CODE))}
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
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
                           new XAttribute("compcode", compcode)
                        )
                  }
               }
            )
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
                           new XAttribute("compcode", compcode)
                        )
                  }
               }
            )
         );
      }

      private void CompCont_Butn_Click(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;
         if (comp == null) return;

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
                           new XAttribute("compcode", comp.CODE)
                        )
                  }
               }
            )
         );
      }

      private void EditPymt_Butn_Click(object sender, EventArgs e)
      {
         Pymt_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Pymt_Butn.Buttons[2]));
      }

      private void RelationShip_Butn_Click(object sender, EventArgs e)
      {
         var comp = CompBs.Current as Data.Company;
         if (comp == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 85 /* Execute RLAT_CINF_F */),
                  new Job(SendType.SelfToUserInterface, "RLAT_CINF_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Company",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("compcode", comp.CODE)                       
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
                  new Job(SendType.Self, 01 /* GetUi */){Input = "shw_acnt_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 11 /* Execute GetNewRecord */)
                  {
                     Input =
                        new XElement("Company",
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
                  new Job(SendType.Self, 01 /* GetUi */){Input = "shw_acnt_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 11 /* Execute GetNewRecord */)
                  {
                     Input =
                        new XElement("Company",
                           new XAttribute("moveposition", "previous")
                        )
                  }
               }
            )
         );
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

      private void SaveMstt_Butn_Click(object sender, EventArgs e)
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

      private void RqstProjBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pr = RqstProjBs.Current as Data.Request;
            if (pr == null) return;

            NoteBs.DataSource = iCRM.Notes.Where(n => n.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            TaskBs.DataSource = iCRM.Tasks.Where(t => t.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            AponBs.DataSource = iCRM.Appointments.Where(a => a.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            FileBs.DataSource = iCRM.Send_Files.Where(sf => sf.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            MesgBs.DataSource = iCRM.Messages.Where(m => m.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            LogcBs.DataSource = iCRM.Log_Calls.Where(l => l.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);
            EmalBs.DataSource = iCRM.Emails.Where(em => em.Request_Row.Request.PROJ_RQST_RQID == pr.RQID);

            RqstChngBs.DataSource = iCRM.VF_Request_Changing(null, null, compcode, pr.RQID).OrderByDescending(r => r.SAVE_DATE).Take(5);
            PymtSaveBs.DataSource = iCRM.VF_Save_Payments(null, null, compcode, pr.RQID);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
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
         { }
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
                              new XAttribute("fileno", rqst.Request_Rows.FirstOrDefault().SERV_FILE_NO),
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
                           new XAttribute("compcode", compcode),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }                     
               }
            )
         );
      }
   }
}
