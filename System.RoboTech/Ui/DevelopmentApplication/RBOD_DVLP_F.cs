﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.RoboTech.ExceptionHandlings;
using System.Xml.Linq;
using System.IO;
using System.Drawing.Imaging;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class RBOD_DVLP_F : UserControl
   {
      public RBOD_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int ordr = OrdrBs.Position;
         int oddt = OrdtBs.Position;
        
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         OrdrBs.Position = ordr;
         OrdtBs.Position = oddt;

         requery = false;
      }

      private void Tsb_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Invalidate();

            OrdrBs.EndEdit();
            OrdtBs.EndEdit();

            iRoboTech.SubmitChanges();
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
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if(robo == null)return;

            long? chatid = null;
            if (Serv_Lov.EditValue != null && Serv_Lov.EditValue.ToString() != "")
               chatid = (long)Serv_Lov.EditValue;

            string ordrtype = null;
            if (OrdrType_Lov.EditValue != null && OrdrType_Lov.EditValue.ToString() != "")
               ordrtype = OrdrType_Lov.EditValue.ToString();

            DateTime? ordrdate = null;
            if (OrdrDate_Dat.Value.HasValue)
               ordrdate = OrdrDate_Dat.Value.Value.Date;

            string ordrcmnt = null;
            if (OrdrCmnt_Txt.EditValue != null && OrdrCmnt_Txt.EditValue.ToString() != "")
               ordrcmnt = OrdrCmnt_Txt.EditValue.ToString();

            OrdrBs.DataSource =
               iRoboTech.ExecuteQuery<Data.Order>(
                  string.Format(
                     @"SELECT * 
                         FROM dbo.[Order] o
                        WHERE o.SRBT_ROBO_RBID = {0}
                          AND o.Chat_Id = {1}
                          AND o.Ordr_Type = {2}
                          AND CAST(o.Strt_Date AS DATE) = {3}
                          AND {4}",
                     robo.RBID,
                     (chatid == null ? "Chat_ID" : chatid.ToString()),
                     (ordrtype == null ? "Ordr_Type" : "\'" + ordrtype + "\'"),
                     (ordrdate == null ? "CAST(STRT_DATE AS DATE)" : ordrdate.Value.ToString("yyyy-MM-DD")),
                     (ordrcmnt == null ? "1=1" : string.Format("EXISTS (SELECT * FROM Order_Detail od WHERE od.Ordr_Code = o.Code AND od.Ordr_Cmnt LIKE N'%{0}%')", ordrcmnt.Replace(' ', '%')))
                  )
               ).ToList();
         }
         catch (Exception exc)
         {
            iRoboTech.SaveException(exc);
         }
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) { OrdrBs.List.Clear(); return; }

            Search_Butn_Click(null, null);

         }
         catch (Exception exc)
         {
            iRoboTech.SaveException(exc);
         }
      }

      private void ClearParm_Butn_Click(object sender, EventArgs e)
      {
         Serv_Lov.EditValue = null;
         OrdrType_Lov.EditValue = null;
         OrdrDate_Dat.Value = null;
         Search_Butn_Click(null, null);
      }

      private void ExcelResult_Butn_Click(object sender, EventArgs e)
      {
         if (OrdrBs.Current == null) return;
         var crnt = OrdrBs.Current as Data.Order;

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.CODE))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void SettingReport_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 14 /* Execute Stng_Rprt_F */),
                  new Job(SendType.SelfToUserInterface, "STNG_RPRT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void SelectOtherReport_Butn_Click(object sender, EventArgs e)
      {
         if (OrdrBs.Current == null) return;
         var crnt = OrdrBs.Current as Data.Order;

         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         long? chatid = null;
         if (Serv_Lov.EditValue != null && Serv_Lov.EditValue.ToString() != "")
            chatid = (long)Serv_Lov.EditValue;

         string ordrtype = null;
         if (OrdrType_Lov.EditValue != null && OrdrType_Lov.EditValue.ToString() != "")
            ordrtype = OrdrType_Lov.EditValue.ToString();

         DateTime? ordrdate = null;
         if (OrdrDate_Dat.Value.HasValue)
            ordrdate = OrdrDate_Dat.Value.Value.Date;

         string ordrcmnt = null;
         if (OrdrCmnt_Txt.EditValue != null && OrdrCmnt_Txt.EditValue.ToString() != "")
            ordrcmnt = OrdrCmnt_Txt.EditValue.ToString();

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 15 /* Execute Rpt_Mngr_F */)
                     {
                        Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Selection"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), 
                              string.Format("AND o.Srbt_Robo_Rbid = {0} AND o.Chat_Id = {1} AND o.Ordr_Type = {2} AND CAST(o.Strt_Date AS DATE) = {3} AND {4}", 
                                 robo.RBID,
                                 (chatid == null ? "o.Chat_ID" : chatid.ToString()),
                                 (ordrtype == null ? "o.Ordr_Type" : "\'" + ordrtype + "\'"),
                                 (ordrdate == null ? "CAST(o.STRT_DATE AS DATE)" : ordrdate.Value.ToString("yyyy-MM-DD")),
                                 (ordrcmnt == null ? "1=1" : string.Format("EXISTS (SELECT * FROM Order_Detail od WHERE od.Ordr_Code = o.Code AND od.Ordr_Cmnt LIKE N'%{0}%')", ordrcmnt.Replace(' ', '%')))
                              ))
                     }
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void SelectDefaultReport_Butn_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         long? chatid = null;
         if (Serv_Lov.EditValue != null && Serv_Lov.EditValue.ToString() != "")
            chatid = (long)Serv_Lov.EditValue;

         string ordrtype = null;
         if (OrdrType_Lov.EditValue != null && OrdrType_Lov.EditValue.ToString() != "")
            ordrtype = OrdrType_Lov.EditValue.ToString();

         DateTime? ordrdate = null;
         if (OrdrDate_Dat.Value.HasValue)
            ordrdate = OrdrDate_Dat.Value.Value.Date;

         string ordrcmnt = null;
         if (OrdrCmnt_Txt.EditValue != null && OrdrCmnt_Txt.EditValue.ToString() != "")
            ordrcmnt = OrdrCmnt_Txt.EditValue.ToString();

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 15 /* Execute Rpt_Mngr_F */){
                        Input = 
                           new XElement("Print", new XAttribute("type", "Default"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), 
                              string.Format("AND o.Srbt_Robo_Rbid = {0} AND o.Chat_Id = {1} AND o.Ordr_Type = {2} AND CAST(o.Strt_Date AS DATE) = {3} AND {4}", 
                                 robo.RBID,
                                 (chatid == null ? "o.Chat_ID" : chatid.ToString()),
                                 (ordrtype == null ? "o.Ordr_Type" : "\'" + ordrtype + "\'"),
                                 (ordrdate == null ? "CAST(o.STRT_DATE AS DATE)" : ordrdate.Value.ToString("yyyy-MM-DD")),
                                 (ordrcmnt == null ? "1=1" : string.Format("EXISTS (SELECT * FROM Order_Detail od WHERE od.Ordr_Code = o.Code AND od.Ordr_Cmnt LIKE N'%{0}%')", ordrcmnt.Replace(' ', '%')))
                              )
                           )
                     }
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void DeleteOrder_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if (ordr == null) return;

            if (MessageBox.Show(this, "آیا با حذف درخواست مطمئن هستین؟", "حذف درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iRoboTech.Orders.DeleteOnSubmit(ordr);

            iRoboTech.SubmitChanges();
            requery = true;

         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void Download_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = RoboBs.Current as Data.Robot;
            if (crnt == null) return;

            var ordrdtil = OrdtBs.Current as Data.Order_Detail;
            if (ordrdtil == null) return;

            var result = MessageBox.Show(this, "فایل عکس را در هارد ذخیره شود یا پایگاه داده؟اگر موافق باشید فایل تنها در هارد شما ذخیره میشود. در غیر اینصورت اگر جواب خیر باشد عکس در پایگاه داده ذخیره میشود", "نوع ذخیره سازی", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            Controller.iRobot bot = new Controller.iRobot(crnt.TKON_CODE, ConnectionString, RobotTrace_Txt, false, crnt);

            var parentmenu = iRoboTech.Menu_Ussds.FirstOrDefault(m => m.ROBO_RBID == ordrdtil.Order.SRBT_ROBO_RBID && m.USSD_CODE == ordrdtil.BASE_USSD_CODE);
            var datenow = iRoboTech.GET_MTOS_U(DateTime.Now).Replace("/", "_");
            var fileupload = "";
            if(parentmenu != null)
                fileupload = (parentmenu.UPLD_FILE_PATH ?? @"D:") + "\\" + bot.Me.Username + "\\" + datenow + "\\" + ordrdtil.Order.CHAT_ID + "\\" + parentmenu.MENU_TEXT;
            else
               fileupload = @"D:" + "\\" + bot.Me.Username + "\\" + datenow + "\\" + ordrdtil.Order.CHAT_ID + "\\" + "OthersMessage";

            var filename = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (!Directory.Exists(fileupload))
            {
               try
               {
                  DirectoryInfo di = Directory.CreateDirectory(fileupload);
               }
               catch
               {
                  if (Dlg_FolderBrowse.ShowDialog() != DialogResult.OK) return;

                  fileupload = Dlg_FolderBrowse.SelectedPath;
               }
            }

            string file_extension = "";
            switch(ordrdtil.ELMN_TYPE)
            {
               case "002":
                  file_extension = "jpg";
                  break;
               case "004":
               case "003":
                  file_extension = ordrdtil.MIME_TYPE.Substring(ordrdtil.MIME_TYPE.LastIndexOf('/') + 1);
                  break;


            }             

            if (result == DialogResult.Yes)
            {
               // ذخیره کردن در هارد
               bot.GetFile("002", ordrdtil.ORDR_DESC, fileupload + "\\" + filename + "." + file_extension);
               OrdrImag_Picbox.Image = Image.FromFile(fileupload + "\\" + filename + "." + file_extension);
            }
            else if (result == DialogResult.No)
            {
               // ذخیره سازی در پایگاه داده
               bot.GetFile("002", ordrdtil.ORDR_DESC, fileupload + "\\" + filename + "." + file_extension);
               ordrdtil.IMAG_PATH = fileupload + "\\" + filename + "." + file_extension;
               /*OrdrImag_Picbox.Image = Image.FromFile(ordrdtil.ORDR_CMNT);
            
               Image img = OrdrImag_Picbox.Image;
               byte[] bytes = null;
               MemoryStream ms = new MemoryStream();
               img.Save(ms, ImageFormat.Bmp);
               bytes = ms.ToArray();

               ordrdtil.ORDR_IMAG = bytes;
               */
               try
               {
                  iRoboTech.SubmitChanges();
                  requery = true;
               }
               catch (Exception exc)
               {
                  requery = false;
                  iRoboTech.SaveException(exc);
               }
               finally
               {
                  if (requery)
                  {
                     Execute_Query();
                  }
               }

               //bot.StopReceiving();
            }
         }catch(Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);            
         }
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordrdtil = OrdtBs.Current as Data.Order_Detail;
            if (ordrdtil == null) return;

            if(ordrdtil.IMAG_PATH != null)
               Dlg_OpenFile.InitialDirectory = ordrdtil.IMAG_PATH.Substring(0, ordrdtil.IMAG_PATH.LastIndexOf('\\'));

            if (Dlg_OpenFile.ShowDialog() != DialogResult.OK) return;

            
            ordrdtil.IMAG_PATH = Dlg_OpenFile.FileName;

            OrdrImag_Picbox.Image = Image.FromFile(ordrdtil.IMAG_PATH);

            Image img = OrdrImag_Picbox.Image;
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            ordrdtil.ORDR_IMAG = bytes;


            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordrdtil = OrdtBs.Current as Data.Order_Detail;
            if (ordrdtil == null) return;

            if (ordrdtil.ORDR_IMAG == null) return;

            if (MessageBox.Show(this, "آیا با حذف عکس سند ارسالی موافق هستید؟", "حذف عکس سند ارسالی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            ordrdtil.ORDR_IMAG = null;
            ordrdtil.IMAG_PATH = null;

            iRoboTech.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void OrdtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var ordrdtil = OrdtBs.Current as Data.Order_Detail;
            if (ordrdtil == null) return;

            var stream = new MemoryStream(ordrdtil.ORDR_IMAG.ToArray());
            OrdrImag_Picbox.Image = Image.FromStream(stream);
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
      }

      private void OrdrBs_CurrentChanged(object sender, EventArgs e)
      {
         OrdrImag_Picbox.Image = null;
         ServProfilePhoto_Butn.ImageProfile = null;
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if (ordr == null) return;

            var stream = new MemoryStream(ordr.Service_Robot.Service.IMAG_PROF.ToArray());
            ServProfilePhoto_Butn.ImageProfile = Image.FromStream(stream);
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
      }

      private void GetUserProfilePhotos_Butn_Click(object sender, EventArgs e)
      {
         var crnt = RoboBs.Current as Data.Robot;
         if (crnt == null) return;

         var ordrdtil = OrdtBs.Current as Data.Order_Detail;
         if (ordrdtil == null) return;

         Controller.iRobot bot = new Controller.iRobot(crnt.TKON_CODE, ConnectionString, RobotTrace_Txt, false, crnt);

         if (Dlg_SaveFile.ShowDialog() != DialogResult.OK) return;

         if (!Directory.Exists(Path.GetDirectoryName(Dlg_SaveFile.FileName) + "\\" + ordrdtil.Order.CHAT_ID))
         {
            DirectoryInfo di = Directory.CreateDirectory(Path.GetDirectoryName(Dlg_SaveFile.FileName) + "\\" + ordrdtil.Order.CHAT_ID);
         }

         // ذخیره کردن در هارد
         bot.GetUserProfilePhotos((int)ordrdtil.Order.CHAT_ID, Path.GetDirectoryName(Dlg_SaveFile.FileName) + "\\" + ordrdtil.Order.CHAT_ID + "\\" + Path.GetFileName(Dlg_SaveFile.FileName));
         
         //bot.StopReceiving();
      }

      private void SelectUserProfilePhoto_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordrdtil = OrdtBs.Current as Data.Order_Detail;
            if (ordrdtil == null) return;

            if(Dlg_SaveFile.FileName != null)
               Dlg_OpenFile.InitialDirectory = Path.GetFullPath(Dlg_SaveFile.FileName);

            if (Dlg_OpenFile.ShowDialog() != DialogResult.OK) return;


            ServProfilePhoto_Butn.ImageProfile = Image.FromFile(Dlg_OpenFile.FileName);

            Image img = ServProfilePhoto_Butn.ImageProfile;
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            ordrdtil.Order.Service_Robot.Service.IMAG_PROF = bytes;

            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }

      }

      private void DeleteUserProfilePhoto_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordrdtil = OrdtBs.Current as Data.Order_Detail;
            if (ordrdtil == null) return;

            if (ordrdtil.Order.Service_Robot.Service.IMAG_PROF == null) return;

            if (MessageBox.Show(this, "آیا با حذف عکس پروفایل مشتری موافق هستید؟", "حذف عکس پروفایل مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            ordrdtil.Order.Service_Robot.Service.IMAG_PROF = null;

            iRoboTech.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iRoboTech.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Ordt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var ordt = OrdtBs.Current as Data.Order_Detail;
         if(ordt == null)return;

         switch (e.Button.Index)
         {
            case 0:
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 17 /* Execute Odrm_Dvlp_F */),
                        new Job(SendType.SelfToUserInterface, "ODRM_DVLP_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input =
                              new XElement("Order_Detail",
                                 new XAttribute("ordtordrcode", ordt.ORDR_CODE),
                                 new XAttribute("ordtrwno", ordt.RWNO),
                                 new XAttribute("type", "new")
                              )
                        }
                     }
                  )
               );
               break;
            case 1:
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 18 /* Execute Orml_Dvlp_F */),
                        new Job(SendType.SelfToUserInterface, "ORML_DVLP_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input =
                              new XElement("Order_Detail",
                                 new XAttribute("ordtordrcode", ordt.ORDR_CODE),
                                 new XAttribute("ordtrwno", ordt.RWNO),
                                 new XAttribute("servfileno", ordt.Order.SRBT_SERV_FILE_NO),
                                 new XAttribute("roborbid", ordt.Order.SRBT_ROBO_RBID)
                              )
                        }
                     }
                  )
               );
               break;
            default:
               break;
         }


      }

      private void FinalOrder_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با نهایی کردن درخواست ها موافق هستید؟", "نهایی کردن درخواست ها", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iRoboTech.FINL_ORDR_P();

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
   }
}
