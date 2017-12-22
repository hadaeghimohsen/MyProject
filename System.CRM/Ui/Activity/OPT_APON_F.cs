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
   public partial class OPT_APON_F : UserControl
   {
      public OPT_APON_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long fileno, projrqstrqid;
      private long rqstrqid;
      private bool needclose = true;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => s.FILE_NO == fileno);
         JobpBs.DataSource = iCRM.Job_Personnels.Where(jp => jp.STAT == "002");
         requery = false;
      }

      private bool Apply()
      {
         try
         {
            var apon = AponBs.Current as Data.Appointment;

            if (apon.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  AponBs.DataSource =
                     iCRM.Appointments.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.APID == iCRM.Appointments.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.APID));
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
            AponBs.EndEdit();
            var Apon = AponBs.Current as Data.Appointment;

            if (Apon == null) throw new Exception("خطا * شی قرارملاقات خالی می باشد"); ;
            if (Apon.SERV_FILE_NO == null || Apon.SERV_FILE_NO == 0) { Serv_Lov.Focus(); throw new Exception("خطا * شماره پرونده قرارملاقات خالی می باشد"); }
            if (Apon.FROM_DATE == null) { FromApon_Date.Focus(); throw new Exception("خطا * از تاریخ قرارملاقات خالی می باشد"); }
            if (Apon.TO_DATE == null) { ToApon_Date.Focus(); throw new Exception("خطا * تا تاریخ قرارملاقات خالی می باشد"); }
            if (Apon.SUBJ_DESC == null || Apon.SUBJ_DESC == "") { Subject_Txt.Focus(); throw new Exception("خطا * موضوع قرارملاقات خالی می باشد"); }
            if (Apon.APON_CMNT == null || Apon.APON_CMNT == "") { Comment_Txt.Focus(); throw new Exception("خطا * متن قرارملاقات خالی می باشد"); }
            if (Apon.APON_WHER == null || Apon.APON_WHER == "") { AponWher_Txt.Focus(); throw new Exception("خطا * مکان قرارملاقات خالی می باشد"); }
            if (ColbBs.Count == 0) { JobpCode_Lov.Focus(); throw new Exception("خطا * حاضرین قرارملاقات خالی می باشد"); }

            iCRM.OPR_ASAV_P(
               new XElement("Appointment",
                  new XAttribute("rqstrqid", rqstrqid),
                  new XAttribute("projrqstrqid", projrqstrqid),
                  new XAttribute("servfileno", fileno),
                  new XAttribute("allday", Apon.ALL_DAY),
                  new XAttribute("fromdate", GetDateTimeString(Apon.FROM_DATE)),
                  new XAttribute("todate", GetDateTimeString(Apon.TO_DATE)),
                  new XAttribute("rqrorqstrqid", Apon.RQRO_RQST_RQID ?? 0),
                  new XAttribute("rqrorwno", Apon.RQRO_RWNO ?? 0),
                  new XAttribute("colr", Apon.Request_Row != null ? Apon.Request_Row.Request.COLR : "#ADFF2F"),
                  new XAttribute("apid", Apon.APID),
                  new XElement("Where",
                     new XAttribute("cordx", Apon.CORD_X ?? 0),
                     new XAttribute("cordy", Apon.CORD_Y ?? 0),
                     Apon.APON_WHER
                  ),
                  new XElement("Comment",
                     new XAttribute("subject", Apon.SUBJ_DESC),
                     Apon.APON_CMNT
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
            if(requery && needclose)
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

      private void AponBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = AponBs.Current as Data.Appointment;

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

            if (rqst.Request_Row != null)
            {
               rqst.Request_Row.Request.COLR = rqst.Request_Row.Request.COLR == null ? "#ADFF2F" : rqst.Request_Row.Request.COLR;
               SelectColor_Butn.NormalColorA = SelectColor_Butn.NormalColorB = SelectColor_Butn.HoverColorA = SelectColor_Butn.HoverColorB = ColorTranslator.FromHtml(rqst.Request_Row.Request.COLR);
            }
            
            rqst.ALL_DAY = rqst.ALL_DAY == null ? "001" : rqst.ALL_DAY;

            AllDay_Tg_Toggled(AllDay_Tg, null);
         }
         catch (Exception exc)
         {}
      }

      private void AllDay_Tg_Toggled(object sender, EventArgs e)
      {
         var apon = AponBs.Current as Data.Appointment;
         if (apon == null) return;

         apon.ALL_DAY = (sender as ToggleSwitch).IsOn ? "002" : "001";
         switch ((sender as ToggleSwitch).IsOn)
         {
            case true:
               FromApon_Time.Visible = ToApon_Time.Visible = false;
               break;
            case false:
               FromApon_Time.Visible = ToApon_Time.Visible = true;
               break;
            default:
               break;
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

      private void CallRequestService_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;            
            if (!Apply()) return;
            var apon = AponBs.Current as Data.Appointment;

            switch(butn.Tag.ToString())
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
                                    new XAttribute("fileno", apon.SERV_FILE_NO),
                                    new XAttribute("rqid", apon.RQRO_RQST_RQID)
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
                                    new XAttribute("rqid", apon.RQRO_RQST_RQID),
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
            var apon = AponBs.Current as Data.Appointment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 59 /* Execute Fin_Rslt_F */),
                     new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Final_Result",                              
                              new XAttribute("rqid", apon.RQRO_RQST_RQID)
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
                           new XAttribute("section", "appointment")
                        )
                  }
               }
            )
         );
      }

      private void Location_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!Apply()) return;
            var apon = AponBs.Current as Data.Appointment;
            if (apon == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
               {
                  Input =
                     new XElement("GMapNets",
                        new XAttribute("requesttype", "get"),
                        new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                        new XAttribute("callback", 40 /* CordinateGetSet */),
                        new XAttribute("outputtype", "appointmentaddress"),
                        new XAttribute("initalset", true),
                        new XAttribute("cordx", apon.CORD_X == null ? "29.622045" : apon.CORD_X.ToString()),
                        new XAttribute("cordy", apon.CORD_Y == null ? "52.522728" : apon.CORD_Y.ToString()),
                        new XAttribute("zoom", "1800")
                     )
               }
            );
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
         }
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
         var apon = AponBs.Current as Data.Appointment;

         if (apon == null || !LinkText_Pk.PickChecked) return;

         apon.SUBJ_DESC = Subject_Txt.Text;
         apon.APON_CMNT = Subject_Txt.Text;
      }

      private void SelectColor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var apon = AponBs.Current as Data.Appointment;

            if (apon.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  AponBs.DataSource =
                     iCRM.Appointments.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.APID == iCRM.Appointments.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.APID));
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
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 48 /* Execute Tsk_Colr_F */),
                        new Job(SendType.SelfToUserInterface, "TSK_COLR_F", 10 /* Execute Actn_Calf_P */)
                        {
                            Input = 
                              new XElement("Request", 
                                 //new XAttribute("fileno", task.SERV_FILE_NO), 
                                 //new XAttribute("tkid", task.TKID),
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
   }
}
