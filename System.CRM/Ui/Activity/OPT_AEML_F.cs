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
using System.MaxUi;
using System.IO;

namespace System.CRM.Ui.Activity
{
   public partial class OPT_AEML_F : UserControl
   {
      public OPT_AEML_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool needclose = true;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);         
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
            var xfilter = Lb_ServCount.Tag as XElement;
            if (xfilter == null) return;

            if (Subject_Txt.Text.Trim() == "") { Subject_Txt.Focus(); return; }
            if (Comment_Txt.Text.Trim() == "") { Comment_Txt.Focus(); return; }

            var schedualdatetime = Email_Date.Value.Value.Date;
            schedualdatetime = schedualdatetime .Date + Email_Time.Time.TimeOfDay;

            xfilter.Add(
               new XElement("SendOptions", 
                  new XElement("Schedule", 
                     new XAttribute("type", Schedular_Tg.IsOn ? "002" : "001"),
                     new XAttribute("datetime", GetDateTimeString( schedualdatetime ))
                  ),
                  new XElement("Contact_Infos",
                     CtifBs.List.OfType<Data.Contact_Info>().
                     Select(ct =>
                        new XElement("Contact_Info",
                           new XAttribute("code", ct.APBS_CODE)
                        )
                     )
                  ),
                  new XElement("Request",
                     new XAttribute("srvctype", "001"), // سیستم محلی شرکت
                     new XAttribute("sorctype", "002") // درخواست گروهی
                  ),
                  new XElement("Caption",
                     new XAttribute("subject", Subject_Txt.Text),
                     new XAttribute("comment", Comment_Txt.Text)
                  ),
                  new XElement("FileAttachments",
                     new XAttribute("cont", SndfBs.Count),
                     SndfBs.List.OfType<Data.Send_File>().
                     Select(a =>
                        new XElement("FileAttachment",
                           new XAttribute("filepath", a.FILE_PATH),
                           new XAttribute("fileserverlink", a.FILE_SRVR_LINK)
                        )
                     )
                  )
               )
            );

            iCRM.OPT_AEML_P(xfilter);            

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Btn_Back_Click(null, null);
            }
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

      private void FileAttachment_Butn_Click(object sender, EventArgs e) { }

      private void TemplateText_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Btn_Back_Click(null, null);
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 69 /* Execute Tmpl_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "TMPL_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("fileno", "")
                           )
                     }
                  }
               )
            );
         }
         catch { }
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
                           new XAttribute("section", "email")
                        )
                  }
               }
            )
         );
      }

      private void Schedular_Tg_Toggled(object sender, EventArgs e)
      {
         if (Schedular_Tg.IsOn)
            Email_Date.Visible = Email_Time.Visible = false;
         else
            Email_Date.Visible = Email_Time.Visible = true;
      }

      private void ApbsList_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (ApbsList_Lov.EditValue == null || ApbsList_Lov.EditValue.ToString() == "") { ApbsList_Lov.Focus(); return; }
                  if (CtifBs.List.OfType<Data.Contact_Info>().Any(t => t.APBS_CODE == (long)ApbsList_Lov.EditValue)) { MessageBox.Show("این آیتم قبلا ثبت شده است"); return; }

                  CtifBs.AddNew();
                  var ctif = CtifBs.Current as Data.Contact_Info;                  

                  ctif.APBS_CODE = (long)ApbsList_Lov.EditValue;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  var ctif = CtifBs.Current as Data.Contact_Info;
                  CtifBs.Remove(ctif);
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void AddFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (OpenFile_Ofd.ShowDialog() != DialogResult.OK) return;

            if (OpenFile_Ofd.FileName.Trim() == "") return;
            SndfBs.AddNew();

            var send = SndfBs.Current as Data.Send_File;
            send.FILE_PATH = OpenFile_Ofd.FileName;
            send.UPLD_TYPE = "001";
            
            // تنظیم کردن پنل مربوط به سایز فایل 
            // بهتره به صورت تابع باشد که برای حالت های مختلف قابل فراخوانی باشد
            // اجرا شدنش هم به صورت ناهمزمان باشه
            FileStream fsSource = new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read);
            send.FILE_SRVR_LINK =
               SaveFileOnServer(
                  fsSource,
                  string.Format(@"\{0}\{1}\{2}\{3}",
                     "BoardcastEmailSends",
                     CurrentUser,
                     Guid.NewGuid(),
                     OpenFile_Ofd.SafeFileName.Substring(0, OpenFile_Ofd.SafeFileName.LastIndexOf('.')) + "_" + Guid.NewGuid() + OpenFile_Ofd.SafeFileName.Substring(OpenFile_Ofd.SafeFileName.LastIndexOf('.'))
                  )
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
         }
      }

      private string SaveFileOnServer(Stream fileStream, string filename)
      {
         try
         {
            //var stng = iCRM.Settings.FirstOrDefault(s => s.USER_NAME == CurrentUser);
            var stng = iCRM.V_ConfigurationSystems.FirstOrDefault();

            if (!Directory.Exists(stng.UPLD_FILE_PATH_ADRS + filename.Substring(0, filename.LastIndexOf('\\'))))
            {
               DirectoryInfo di = Directory.CreateDirectory(stng.UPLD_FILE_PATH_ADRS + filename.Substring(0, filename.LastIndexOf('\\')));
            }


            using (var newFile = File.Create(stng.UPLD_FILE_PATH_ADRS + filename))
            {
               fileStream.CopyTo(newFile);

               fileStream.Close();
               fileStream.Dispose();
            }

            return stng.UPLD_FILE_PATH_ADRS + filename;
         }
         catch (Exception exc)
         {
            //Log if something goes wrong
            iCRM.SaveException(exc);
            return "";
         }
      }

      private void ActnSndf_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var sndf = SndfBs.Current as Data.Send_File;
            if (sndf == null || sndf.FILE_SRVR_LINK == null) { SndfBs.Remove(sndf); return; }

            File.Delete(sndf.FILE_SRVR_LINK);
            Directory.Delete(sndf.FILE_SRVR_LINK.Substring(0, sndf.FILE_SRVR_LINK.LastIndexOf('\\')));
            SndfBs.Remove(sndf);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
         }
      }

   }
}
