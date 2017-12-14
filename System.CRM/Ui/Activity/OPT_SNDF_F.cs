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
using System.IO;
using System.Threading;
using System.Globalization;
using System.MaxUi;

namespace System.CRM.Ui.Activity
{
   public partial class OPT_SNDF_F : UserControl
   {
      public OPT_SNDF_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long fileno;
      private string sendtype;
      private long rqstrqid, projrqstrqid;
      private long cmid;
      private bool needclose = true;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => s.FILE_NO == fileno);
         DsdtpBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "SENDFILE_INFO");
         requery = false;
      }

      private bool Apply()
      {
         try
         {
            var sndf = SndfBs.Current as Data.Send_File;

            if (sndf.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  SndfBs.DataSource =
                     iCRM.Send_Files.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.SFID == iCRM.Send_Files.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.SFID));
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
            SndfBs.EndEdit();
            var sndf = SndfBs.Current as Data.Send_File;

            if (sndf == null) throw new Exception("خطا * شی ارسال فایل خالی می باشد");
            if (sndf.SERV_FILE_NO == null || sndf.SERV_FILE_NO == 0) { Serv_Lov.Focus(); throw new Exception("خطا * شماره پرونده ارسال فایل خالی می باشد"); }
            if (sndf.SEND_DATE == null) { Send_Date.Focus(); throw new Exception("خطا * تاریخ ارسال فایل خالی می باشد"); }
            if (sndf.SHER_TEAM == null) { ShareTeam_Tg.Focus(); throw new Exception("خطا * وضعیت اشتراک ارسال فایل خالی می باشد"); }
            if (sndf.SUBJ_DESC == null || sndf.SUBJ_DESC == "") { Subject_Txt.Focus(); throw new Exception("خطا * موضوع ارسال فایل خالی می باشد"); }
            if (sndf.SEND_TYPE == null || sndf.SEND_TYPE == 0) { SendType_Lov.Focus(); throw new Exception("خطا * نوع ارسال فایل خالی می باشد"); }
            if (sndf.UPLD_TYPE == null || sndf.UPLD_TYPE == "") { LocalComputer_Rb.Focus(); throw new Exception("خطا * نوع بارگذاری ارسال فایل خالی می باشد"); }
            if (sndf.SDRC_TYPE == null || sndf.SDRC_TYPE == "") { Sdrc_Lov.Focus(); throw new Exception("خطا * نوع منبغ ارسال فایل خالی می باشد"); }
            
            if(LocalComputer_Rb.Checked)
               if (sndf.FILE_PATH == null || sndf.FILE_PATH == "") { SelectedFile_TextButn.Focus(); throw new Exception("خطا * فایل ارسالی خالی می باشد"); }
            if(Url_Rb.Checked)
            {
               if (sndf.URL_LINK == null || sndf.URL_LINK == "") { UrlLink_TextButn.Focus(); throw new Exception("خطا * لینک ارسال فایل خالی می باشد"); }
               if (sndf.URL_NAME == null || sndf.URL_NAME == "") { UrlName_TextButn.Focus(); throw new Exception("خطا * نام لینک ارسال فایل خالی می باشد"); }
            }


            iCRM.OPR_SSAV_P(
               new XElement("SendFile",
                  new XAttribute("rqstrqid", rqstrqid),
                  new XAttribute("projrqstrqid", projrqstrqid),
                  new XAttribute("cmid", cmid),
                  new XAttribute("servfileno", fileno),
                  new XAttribute("sherteam", sndf.SHER_TEAM),
                  new XAttribute("senddate", GetDateTimeString(sndf.SEND_DATE)),
                  new XAttribute("rqrorqstrqid", sndf.RQRO_RQST_RQID ?? 0),
                  new XAttribute("rqrorwno", sndf.RQRO_RWNO ?? 0),
                  new XAttribute("sfid", sndf.SFID),
                  new XAttribute("subject", sndf.SUBJ_DESC),
                  new XAttribute("sendtype", sndf.SEND_TYPE ?? 0),
                  new XAttribute("sdrctype", sndf.SDRC_TYPE),
                  new XAttribute("upldtype", sndf.UPLD_TYPE),
                  new XAttribute("filepath", sndf.FILE_PATH ?? ""),
                  new XAttribute("filesrvrlink", sndf.FILE_SRVR_LINK ?? ""),
                  new XAttribute("urllink", sndf.URL_LINK ?? ""),
                  new XAttribute("urlname", sndf.URL_NAME ?? "")
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
                  new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno), new XAttribute("formcaller", GetType().Name), new XAttribute("rqstrqid", rqstrqid)) }
               );
               
               //_DefaultGateway.Gateway(
               //   new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno)) }
               //);
               
               Btn_Back_Click(null, null);
            }
         }
      }

      private void SndfBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = SndfBs.Current as Data.Send_File;           

            if (rqst == null ) { RqstFolw_Butn.Visible = false; return; }

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

            rqst.SHER_TEAM = rqst.SHER_TEAM == null ? "001" : rqst.SHER_TEAM;
            switch (rqst.SHER_TEAM)
            {
               case "001":
                  ShareTeam_Tg.IsOn = false;
                  break;
               case "002":
                  ShareTeam_Tg.IsOn = true;
                  break;
               default:
                  break;
            }
            rqst.UPLD_TYPE = rqst.UPLD_TYPE == null ? "002" : rqst.UPLD_TYPE;
            switch (rqst.UPLD_TYPE)
            {
               case "001":
                  LocalComputer_Rb.Checked = true;
                  break;
               case "002":
                  Url_Rb.Checked = true;
                  break;
               default:
                  break;
            }
            ShareTeam_Tg_Toggled(ShareTeam_Tg, null);
            LocalComputer_Rb_CheckedChanged(LocalComputer_Rb, null);
         }
         catch (Exception exc)
         {}
      }

      private void ShareTeam_Tg_Toggled(object sender, EventArgs e)
      {
         var send = SndfBs.Current as Data.Send_File;
         if (send == null) return;

         send.SHER_TEAM = (sender as ToggleSwitch).IsOn ? "002" : "001";
      }

      private void SelectedFile_TextButn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var send = SndfBs.Current as Data.Send_File;
            if (send == null) return;

            if(send.SEND_TYPE == null || send.SEND_TYPE == 0){SendType_Lov.Focus(); return;}

            if (OpenFile_Ofd.ShowDialog() != DialogResult.OK) return;

            send.FILE_PATH = OpenFile_Ofd.FileName;
            send.UPLD_TYPE = "001";
            LocalComputer_Rb.Checked = true;
            // تنظیم کردن پنل مربوط به سایز فایل 
            // بهتره به صورت تابع باشد که برای حالت های مختلف قابل فراخوانی باشد
            // اجرا شدنش هم به صورت ناهمزمان باشه
            FileStream fsSource = new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read);
            send.FILE_SRVR_LINK = 
               SaveFileOnServer(
                  fsSource, 
                  string.Format(@"\{0}\{1}\{2}\{3}\{4}\{5}", 
                     fileno, 
                     CurrentUser, 
                     "Send", 
                     GetPersianDate(DateTime.Now), 
                     DsdtpBs.List.OfType<Data.App_Base_Define>().FirstOrDefault(s => s.CODE == send.SEND_TYPE).TITL_DESC,
                     OpenFile_Ofd.SafeFileName.Substring(0, OpenFile_Ofd.SafeFileName.LastIndexOf('.')) + "_" + Guid.NewGuid() + OpenFile_Ofd.SafeFileName.Substring(OpenFile_Ofd.SafeFileName.LastIndexOf('.'))
                  )
               );
            
         }
         catch (Exception exc)
         {}
      }

      private void LocalComputer_Rb_CheckedChanged(object sender, EventArgs e)
      {
         var send = SndfBs.Current as Data.Send_File;
         if (send == null) return;

         send.UPLD_TYPE = (sender as RadioButton).Checked ? "001" : "002";
         if(LocalComputer_Rb.Checked)
            Url_Rb.Checked = !LocalComputer_Rb.Checked;
      }

      private void Url_Rb_CheckedChanged(object sender, EventArgs e)
      {
         var send = SndfBs.Current as Data.Send_File;
         if (send == null) return;

         send.UPLD_TYPE = (sender as RadioButton).Checked ? "002" : "001";
         if(Url_Rb.Checked)
            LocalComputer_Rb.Checked = !Url_Rb.Checked;
      }

      private string SaveFileOnServer(Stream fileStream, string filename)
      {
         try
         {
            var stng = iCRM.Settings.FirstOrDefault(s => s.DFLT_STAT == "002");

            if (!Directory.Exists(stng.UPLD_FILE + filename.Substring(0, filename.LastIndexOf('\\'))))
            {
               DirectoryInfo di = Directory.CreateDirectory(stng.UPLD_FILE + filename.Substring(0, filename.LastIndexOf('\\')));
            }

            
            using (var newFile = File.Create(stng.UPLD_FILE + filename))
            {
               fileStream.CopyTo(newFile);

               fileStream.Close();
               fileStream.Dispose();
            }

            return stng.UPLD_FILE + filename;
         }
         catch (Exception exc)
         {
            //Log if something goes wrong
            iCRM.SaveException(exc);
            return "";
         }
      }

      private string GetPersianDate(DateTime currentDate)
      {
         PersianCalendar pc = new PersianCalendar();

         return 
            string.Format("{0,4:D4}{1,2:D2}{2,2:D2}",
               pc.GetYear(currentDate),
               pc.GetMonth(currentDate),
               pc.GetDayOfMonth(currentDate)
            );
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

      private void CallRequest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var sndf = SndfBs.Current as Data.Send_File;
            

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
                                    new XAttribute("fileno", sndf.SERV_FILE_NO),
                                    new XAttribute("rqid", sndf.RQRO_RQST_RQID)
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
                                    new XAttribute("rqid", sndf.RQRO_RQST_RQID),
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
            var sndf = SndfBs.Current as Data.Send_File;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 59 /* Execute Fin_Rslt_F */),
                     new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Final_Result",                              
                              new XAttribute("rqid", sndf.RQRO_RQST_RQID)
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

      private void ShowFileServer_TextButn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var sndf = SndfBs.Current as Data.Send_File;
            if (sndf == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  OpenFile_Ofd.InitialDirectory = sndf.FILE_SRVR_LINK.Substring(0,sndf.FILE_SRVR_LINK.LastIndexOf('\\'));
                  OpenFile_Ofd.ShowDialog();
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {

         }
      }

      private void SendTypeAppBase_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 79 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "SENDFILE_INFO"),
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
   }
}
