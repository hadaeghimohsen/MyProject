using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.CRM.ExceptionHandlings;

namespace System.CRM.Ui.MasterPage
{
   partial class FRST_PAGE_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iCRMDataContext iCRM;
      private string ConnectionString;
      //private List<long?> Fga_Uclb_U;
      private string CurrentUser;

      private XElement x = null;
      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
               CheckSecurity(job);;
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               PostOnWall(job);
               break;
            case 09:
               TakeOnWall(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 40:
               SetToolTip(job);
               break;
            case 41:
               SetNotification(job);
               break;
            case 42:
               SetMention(job);
               break;
            case 100:
               ShowRequest(job);
               break;
            case 101:
               ShowServiceInfo(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.F9)
         {
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iCRM</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iCRM = new Data.iCRMDataContext(GetConnectionString.Output.ToString());

         //Fga_Uclb_U = (iCRM.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
         //Lbs_CrntUser.Text = 
         CurrentUser = iCRM.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         #region Package Item
         //**rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);

         //**var Pkac = iCRM.VF_AccessPackage(GetHostInfo.Output as XElement).ToList();

         //**rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null || Pkac.Any(i => i.RWNO == Convert.ToInt32(rbe.Tag)) ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, GetType().Name, 41 /* Execute SetNotification */),
                  new Job(SendType.SelfToUserInterface, GetType().Name, 42 /* Execute SetMention */)
               }
            )
         );

         var _GetUserImageProfile =
            new Job(SendType.External, "Localhost", "Commons", 28 /* Execute DoWork4GetUserProfile */, SendType.Self) { Input = new XElement("Account", new XAttribute("need", "imageprofile")) };

         _DefaultGateway.Gateway(_GetUserImageProfile);

         User_Butn.ImageProfile = _GetUserImageProfile.Output as Image;


         /* Initial Sp_Barcode For Running */
         ///Start_BarCode();
         /* Initial Sp_FingerPrint For Running */
         //Start_FingerPrint();         

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("CRM:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         //Stop_BarCode();
         //Stop_FingerPrint();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void PostOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;

            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Dock = DockStyle.Fill;
                  c.Visible = true;
                  Pnl_Desktop.Controls.Add(c);
                  Pnl_Desktop.Controls.SetChildIndex(c, 0);
               }), obj);
            else
            {
               obj.Dock = DockStyle.Fill;
               obj.Visible = true;
               Pnl_Desktop.Controls.Add(obj);
               Pnl_Desktop.Controls.SetChildIndex(obj, 0);
            }

         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void TakeOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => Pnl_Desktop.Controls.Remove(c)), obj);
            this.Focus();
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Successful; UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => Pnl_Desktop.Controls.Remove(c)), obj);
            this.Focus();
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         //Execute_Query();
         Tm_Refreshing.Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void SetToolTip(Job job)
      {
         //Lbs_Tooltip.Text = job.Input.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 41
      /// </summary>
      /// <param name="job"></param>
      private void SetNotification(Job job)
      {
         try
         {
            iCRM = new Data.iCRMDataContext(ConnectionString);
            //var result = iCRM.Reminders.Where(r => r.Job_Personnel1.USER_NAME.ToLower() == CurrentUser.ToLower() && r.ALRM_DATE.Value.Date <= DateTime.Now.Date && r.READ_RMND == "001").Count();
            var result = iCRM.Job_Personnels.Where(jp => jp.USER_NAME.ToLower() == CurrentUser.ToLower());

            if(InvokeRequired)
            {
               Invoke(new Action(() => {
                  if (result.Where(jp => jp.RMND_STAT == "002").Count() > 0)
                     Notification_Butn.ImageProfile = Properties.Resources.IMAGE_1509;
                  else
                     Notification_Butn.ImageProfile = Properties.Resources.IMAGE_1508;

                  if (result.Sum(r => r.RMND_NOT_READ_DNRM) > 0)
                  {
                     Lb_Notification.Visible = true;
                     Lb_Notification.Text = result.Sum(r => r.RMND_NOT_READ_DNRM).ToString();
                     this.ToolTipInfo.SetToolTip(this.Lb_Notification, result.Sum(r => r.RMND_NOT_READ_DNRM).ToString());
                  }
                  else
                     Lb_Notification.Visible = false;

                  // 1396/09/07 *  نمایش اطلاعات مربوط به وظایف , قرار ملاقات های ارسال شده و دریافت شده
                  Refreshing_Butn_Click(null, null);
               }));               
            }
            else
            {
               if (result.Where(jp => jp.RMND_STAT == "002").Count() > 0)
                  Notification_Butn.ImageProfile = Properties.Resources.IMAGE_1509;
               else
                  Notification_Butn.ImageProfile = Properties.Resources.IMAGE_1508;

               if (result.Sum(r => r.RMND_NOT_READ_DNRM) > 0)
               {
                  Lb_Notification.Visible = true;
                  Lb_Notification.Text = result.Sum(r => r.RMND_NOT_READ_DNRM).ToString();
                  this.ToolTipInfo.SetToolTip(this.Lb_Notification, result.Sum(r => r.RMND_NOT_READ_DNRM).ToString());
               }
               else
                  Lb_Notification.Visible = false;

               // 1396/09/07 *  نمایش اطلاعات مربوط به وظایف , قرار ملاقات های ارسال شده و دریافت شده
               Refreshing_Butn_Click(null, null);
            }
         }
         catch (Exception exc){}
      }

      /// <summary>
      /// Code 42
      /// </summary>
      /// <param name="job"></param>
      private void SetMention(Job job)
      {
         try
         {
            iCRM = new Data.iCRMDataContext(ConnectionString);
            var result = iCRM.Job_Personnels.Where(jp => jp.USER_NAME.ToLower() == CurrentUser.ToLower());

            if (InvokeRequired)
            {
               Invoke(new Action(() =>
               {
                  if (result.Where(jp => jp.MNTN_STAT == "002").Count() > 0)
                     Mention_Butn.ImageProfile = Properties.Resources.IMAGE_1596;
                  else
                     Mention_Butn.ImageProfile = Properties.Resources.IMAGE_1595;

                  if (result.Sum(r => r.MNTN_NOT_READ_DNRM) > 0)
                  {
                     Lb_Mention.Visible = true;
                     Lb_Mention.Text = result.Sum(r => r.MNTN_NOT_READ_DNRM).ToString();
                     this.ToolTipInfo.SetToolTip(this.Lb_Mention, result.Sum(r => r.MNTN_NOT_READ_DNRM).ToString());
                  }
                  else
                     Lb_Mention.Visible = false;

               }));
            }
            else
            {
               if (result.Where(jp => jp.MNTN_STAT == "002").Count() > 0)
                  Mention_Butn.ImageProfile = Properties.Resources.IMAGE_1596;
               else
                  Mention_Butn.ImageProfile = Properties.Resources.IMAGE_1595;

               if (result.Sum(r => r.MNTN_NOT_READ_DNRM) > 0)
               {
                  Lb_Mention.Visible = true;
                  Lb_Mention.Text = result.Sum(r => r.MNTN_NOT_READ_DNRM).ToString();
                  this.ToolTipInfo.SetToolTip(this.Lb_Mention, result.Sum(r => r.MNTN_NOT_READ_DNRM).ToString());
               }
               else
                  Lb_Mention.Visible = false;
            }
         }
         catch (Exception exc) { }
      }

      /// <summary>
      /// Code 100
      /// </summary>
      /// <param name="job"></param>
      private void ShowRequest(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            if(xinput != null)
            {
               var rqtpcode = xinput.Attribute("rqtpcode").Value;
               if(rqtpcode == "001")
               {

               }
               else if(rqtpcode == "002")
               {

               }
               else if (rqtpcode == "003")
               {

               }
               else if (rqtpcode == "004")
               {

               }
               else if (rqtpcode == "005")
               {
                  // ثبت تماس تلفنی                  
                  var logc = iCRM.Log_Calls.FirstOrDefault(lc => lc.RQRO_RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));
                  
                  _DefaultGateway.Gateway(
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
                       })
                  );
               }
               else if (rqtpcode == "006")
               {
                  // ارسال فایل
                  var sndf = iCRM.Send_Files.FirstOrDefault(sf => sf.RQRO_RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));

                  _DefaultGateway.Gateway(
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
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           },
                        })
                  );
               }
               else if (rqtpcode == "007")
               {
                  // ثبت جلسه حضوری
                  var apon = iCRM.Appointments.FirstOrDefault(ap => ap.RQRO_RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));

                  _DefaultGateway.Gateway(
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
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           },
                        })
                  );
               }
               else if (rqtpcode == "008")
               {
                  // ثبت یاداشت روزانه
                  var note = iCRM.Notes.FirstOrDefault(n => n.RQRO_RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));

                  // هنوز فرمی برای یاداشت درست نشده
                  _DefaultGateway.Gateway(
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
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           },
                        })
                  );
               }
               else if (rqtpcode == "009")
               {
                  // ثبت وظیفه
                  var task = iCRM.Tasks.FirstOrDefault(t => t.RQRO_RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                       new List<Job>
                        {                  
                           new Job(SendType.Self, 26 /* Execute Opt_Task_F */),
                           new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 10 /* Execute ACTN_CALF_P */)
                           {
                              Input = 
                                 new XElement("Service", 
                                    new XAttribute("fileno", task.SERV_FILE_NO), 
                                    new XAttribute("tasktype", "edit"), 
                                    new XAttribute("tkid", task.TKID), 
                                    new XAttribute("rqrorqstrqid", task.RQRO_RQST_RQID),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           },
                        })
                  );
               }
               else if (rqtpcode == "010")
               {
                  // ثبت ارسال ایمیل
                  var emal = iCRM.Emails.FirstOrDefault(e => e.RQRO_RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));

                  _DefaultGateway.Gateway(
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
                       })
                  );
               }
               else if (rqtpcode == "011")
               {
                  // ثبت معامله
                  var pymt = iCRM.Payments.FirstOrDefault(p => p.RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));

                  _DefaultGateway.Gateway(
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
                                 new XAttribute("rqid", pymt.RQST_RQID), 
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       })
                  );
               }
               else if (rqtpcode == "012")
               {
                  // ارسال پیامک
                  var mesg = iCRM.Messages.FirstOrDefault(msg => msg.RQRO_RQST_RQID == Convert.ToInt64(xinput.Attribute("rqid").Value));

                  _DefaultGateway.Gateway(
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
                                    new XAttribute("cellphon", mesg.CELL_PHON ?? ""),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           },
                        })
                  );
               }
            }
         }
         catch (Exception exc) 
         {
            iCRM.SaveException(exc);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 101
      /// </summary>
      /// <param name="job"></param>
      private void ShowServiceInfo(Job job)
      {
         InfoServ_Rm.ShowPopup(new Point(Width / 2, Height / 2));
         InfoServ_Rm.Expand();

         var xinput = job.Input as XElement;
         if (xinput != null)
         {
            formCaller = xinput.Attribute("formcaller").Value;
            fileno = Convert.ToInt64(xinput.Attribute("fileno").Value);
         }

         job.Status = StatusType.Successful;
      }      
   }
}
