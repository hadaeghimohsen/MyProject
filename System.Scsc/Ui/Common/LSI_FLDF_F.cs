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
using System.Xml.Linq;
using DevExpress.XtraEditors;
using System.IO;

namespace System.Scsc.Ui.Common
{
   public partial class LSI_FLDF_F : UserControl
   {
      public LSI_FLDF_F()
      {
         InitializeComponent();
      }

      int index = 0;

      private void HL_INVSFILENO_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var CrntFigh = vF_Last_Info_FighterResultBindingSource.Current as Data.VF_Last_Info_FighterResult;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", CrntFigh.FILE_NO)) }
            );
         }
         catch { }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void FighBnSettingPrint_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void FighBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (vF_Last_Info_FighterResultBindingSource.Current == null) return;
         string filenos = "";

         foreach (int i in PBLC.GetSelectedRows())
         {
            vF_Last_Info_FighterResultBindingSource.Position = i;
            var figh = vF_Last_Info_FighterResultBindingSource.Current as Data.VF_Last_Info_FighterResult;
            filenos += (filenos.Length == 0 ? "" : ",") + figh.FILE_NO;
         }

         if (filenos.Length == 0) return;

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No IN ( {0} )", filenos))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      XElement xHost;
      private void colActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         index = vF_Last_Info_FighterResultBindingSource.Position;
         var figh = vF_Last_Info_FighterResultBindingSource.Current as Data.VF_Last_Info_FighterResult;
         switch (e.Button.Index)
         {
            case 0:
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                       new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                        })
               );
               break;
            case 1:
               if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == figh.FILE_NO && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

               // 1396/10/14 * بررسی اینکه آیا مشتری چند کلاس ثبت نام کرده است
               //if (iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == figh.FILE_NO && mb.RECT_CODE == "004" && mb.TYPE == "001" && mb.END_DATE.Value.Date >= DateTime.Now.Date && (mb.RWNO == 1 || mb.Request_Row.RQTT_CODE == "001") && (mb.NUMB_OF_ATTN_MONT > 0 && mb.NUMB_OF_ATTN_MONT > mb.SUM_ATTN_MONT_DNRM)).Count() >= 2)
               //{
               //   _DefaultGateway.Gateway(
               //      new Job(SendType.External, "localhost",
               //         new List<Job>
               //         {
               //            new Job(SendType.Self, 152 /* Execute Chos_Mbsp_F */),
               //            new Job(SendType.SelfToUserInterface, "CHOS_MBSP_F", 10 /* Execute Actn_CalF_F*/ )
               //            {
               //               Input = 
               //               new XElement("Fighter",
               //                  new XAttribute("fileno", figh.FILE_NO),
               //                  new XAttribute("namednrm", figh.NAME_DNRM),
               //                  new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM)
               //               )
               //            }
               //         }
               //      )
               //   );
               //}
               //else
               //   _DefaultGateway.Gateway(
               //      new Job(SendType.External, "Localhost",
               //         new List<Job>
               //         {
               //            new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
               //            new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
               //         })
               //   );
               break;
            case 2:
               if (MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "عملیات حذف موقت مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))},
                        new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 07 /* Execute Load_Data */){Input = new XElement("LoadData", new XAttribute("requery", "1"))},
                     })
               );
               break;
            case 3:               
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))}
                     })
               );
               break;
            case 4:
               if (figh.FNGR_PRNT_DNRM == "" && !(figh.TYPE == "002" || figh.TYPE == "003")) { MessageBox.Show(this, "برای عضو مورد نظر هیچ کد انگشتی وارد نشده، لطفا کد عضو را از طریق تغییرات مشخصات عمومی تغییر لازم را اعمال کنید"); return; }
               if (figh.COCH_FILE_NO == null && !(figh.TYPE == "009" || figh.TYPE == "002" || figh.TYPE == "003" || figh.TYPE == "004")) { MessageBox.Show(this, "برای عضو شما مربی و ساعت کلاسی مشخصی وجود ندارد که مشخص کنیم در چه کلاس حضوری ثبت کنیم"); return; }
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                        
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "accesscontrol"), new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM))}
                     })
               );
               break;
            case 5:
               if (figh.FNGR_PRNT_DNRM == "" && !(figh.TYPE == "002" || figh.TYPE == "003")) { MessageBox.Show(this, "برای عضو مورد نظر هیچ کد انگشتی وارد نشده، لطفا کد عضو را از طریق تغییرات مشخصات عمومی تغییر لازم را اعمال کنید"); return; }
               if (figh.COCH_FILE_NO == null && !(figh.TYPE == "009" || figh.TYPE == "002" || figh.TYPE == "003" || figh.TYPE == "004")) { MessageBox.Show(this, "برای عضو شما مربی و ساعت کلاسی مشخصی وجود ندارد که مشخص کنیم در چه کلاس حضوری ثبت کنیم"); return; }

               /* 1395/03/15 * اگر سیستم بتواند حضوری را برای فرد ذخیره کند باید عملیات نمایش ورود فرد را آماده کنیم. */
               var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_NOTF_STAT == "002").FirstOrDefault();
               if (attnNotfSetting != null && attnNotfSetting.ATTN_NOTF_STAT == "002" && figh.FILE_NO != 0 && iScsc.Attendances.Any(a => figh.FILE_NO == a.FIGH_FILE_NO && a.ATTN_DATE.Date == DateTime.Now.Date))
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", figh.FILE_NO),
                                 new XAttribute("attndate", DateTime.Now)
                              )
                           }
                        })
                  );
               }
               break;
            case 6:
               try
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                              Input = 
                                 new XElement("Fighter",
                                    new XAttribute("fileno", figh.FILE_NO)                               
                                 )
                           },
                           new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", figh.FILE_NO),
                                 new XAttribute("type", "refresh"),
                                 new XAttribute("tabfocued", "tp_003")
                              )
                           }
                        })
                  );
               }
               catch { }
               break;
            default:
               break;
         }

         vF_Last_Info_FighterResultBindingSource.Position = index;
      }

      private void Lbls_Click(object sender, EventArgs e)
      {
         LabelControl lbl = (LabelControl)sender;
         switch (lbl.Name)
         {
            case "Green_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003' And [colRemnDay] >= 4";
               break;
            case "Red_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003' And [colRemnDay] = 0";
               break;
            case "Yellow_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003' And [colRemnDay] <= 3 And [colRemnDay] >= 1";
               break;
            case "Gray_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003' And [colRemnDay] < 0";
               break;
            case "YellowGreen_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003'";
               break;
            case "DebtUp_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003' And DEBT_DNRM > 0";
               break;
            case "DebtDown_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003' And DPST_AMNT_DNRM > 0";
               break;
         }
      }

      private void TrnsFngrPrnt_Butn_Click(object sender, EventArgs e)
      {         
         if (MessageBox.Show(this, "آیا با انتقال شناسایی کارت اعضا به دستگاه موافق هستید؟", "عملیات انتقال", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         foreach (Data.VF_Last_Info_FighterResult figh in vF_Last_Info_FighterResultBindingSource.List.OfType<Data.VF_Last_Info_FighterResult>().Where(f => f.END_DATE < DateTime.Now))
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
               {
                  Input =
                  new XElement("User",
                     new XAttribute("enrollnumb", figh.FNGR_PRNT_DNRM),
                     new XAttribute("cardnumb", figh.FNGR_PRNT_DNRM),
                     new XAttribute("namednrm", figh.FNGR_PRNT_DNRM)
                  )
               }
            );
         }
      }

      private void vF_Last_Info_FighterResultGridControl_DoubleClick(object sender, EventArgs e)
      {
         HL_INVSFILENO_ButtonClick(null, null);
      }

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            MbspBs.List.Clear();
            ExpnAmnt_Txt.Text = PymtAmnt_Txt.Text = DscnAmnt_Txt.Text = "";

            vF_Last_Info_FighterResultBindingSource.DataSource = iScsc.VF_Last_Info_Fighter(null, FrstName_Txt.Text, LastName_Txt.Text, NatlCode_Txt.Text, FngrPrnt_Txt.Text, CellPhon_Txt.Text, TellPhon_Txt.Text, (Men_Rb.Checked ? "001" : Women_Rb.Checked ? "002" : null), ServNo_Txt.Text, GlobCode_Txt.Text, null, null, null, null);
            vF_Last_Info_FighterResultGridControl.Focus();
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void vF_Last_Info_FighterResultBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Last_Info_FighterResultBindingSource.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;

            MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == figh.FILE_NO && mb.RECT_CODE == "004" && (mb.TYPE == "001" || mb.TYPE == "005"));
            Mbsp_gv.TopRowIndex = 0;

            UserProFile_Rb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            //Pb_FighImg.Visible = true;

            if (InvokeRequired)
               Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
            else
               UserProFile_Rb.ImageProfile = bm;
         }
         catch
         { //Pb_FighImg.Visible = false;
            UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
         }
      }

      private void MbspBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;            

            long? rqid = 0;
            if (mbsp.RWNO == 1)
               rqid = mbsp.Request_Row.Request.Request1.RQID;
            else
               rqid = mbsp.RQRO_RQST_RQID;

            ExpnAmnt_Txt.EditValue = iScsc.Payment_Details.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => (pd.EXPN_PRIC + pd.EXPN_EXTR_PRCT) * pd.QNTY);
            DscnAmnt_Txt.EditValue = iScsc.Payment_Discounts.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => pd.AMNT);
            PymtAmnt_Txt.EditValue = iScsc.Payment_Methods.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => pd.AMNT);
         }
         catch
         {
         }
      }

      private void GlobCodeDnrm_Txt_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.F11 }
            );

            GlobCode_Txt.Text = GlobCodeDnrm_Txt.Text;
            Search_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MbspValdType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Last_Info_FighterResultBindingSource.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;

            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            if (mbsp.TYPE == "005")
            {
               MessageBox.Show(this, "شما اجازه غیرفعال کردن رکورد بلوکه کردن را ندارید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }

            if (mbsp.VALD_TYPE == "002")
            {
               if (MessageBox.Show(this, "آیا با غیرفعال کردن دوره موافق هستید؟", "غیرفعال کردن دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

               iScsc.ExecuteCommand(string.Format("UPDATE Member_Ship SET Vald_Type = '001' WHERE Rqro_Rqst_Rqid = {0};", mbsp.RQRO_RQST_RQID));
            }
            else if (mbsp.VALD_TYPE == "001")
            {
               if (MbspBs.List.OfType<Data.Member_Ship>().Any(m => m.RWNO > mbsp.RWNO && m.TYPE == "005"))
               {
                  MessageBox.Show(this, "شما اجازه فعال کردن دوره ابطال شده توسط فرآیند بلوکه کردن را ندارید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }

               if (MessageBox.Show(this, "آیا با فعال کردن دوره موافق هستید؟", "فعال کردن دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

               iScsc.ExecuteCommand(string.Format("UPDATE Member_Ship SET Vald_Type = '002' WHERE Rqro_Rqst_Rqid = {0};", mbsp.RQRO_RQST_RQID));
            }

            iScsc = new Data.iScscDataContext(ConnectionString);
            MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == figh.FILE_NO && mb.RECT_CODE == "004" && (mb.TYPE == "001" || mb.TYPE == "005"));
            Mbsp_gv.TopRowIndex = 0;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

      private void MbspInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            int RouterMethod = 105;
            string RouterGateway = "SHOW_UCRQ_F";

            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, RouterMethod /* Execute RouterMethod */),
                     new Job(SendType.SelfToUserInterface, RouterGateway, 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("rqid", mbsp.RQRO_RQST_RQID), 
                              new XElement("Request_Row", 
                                 new XAttribute("fighfileno", mbsp.FIGH_FILE_NO)
                              )
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void AttnMbsp_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var figh = vF_Last_Info_FighterResultBindingSource.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;

            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  try
                  {

                     Job _InteractWithScsc =
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                              new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("mbsprwno", mbsp.RWNO))}
                           });
                     _DefaultGateway.Gateway(_InteractWithScsc);

                     iScsc = new Data.iScscDataContext(ConnectionString);
                     MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == figh.FILE_NO && mb.RECT_CODE == "004" && (mb.TYPE == "001" || mb.TYPE == "005"));
                     Mbsp_gv.TopRowIndex = 0;
                  }
                  catch (Exception exc)
                  {
                     MessageBox.Show(exc.Message);
                  }
                  break;
               case 1:
                  try
                  {
                     if (figh.TYPE == "002" || figh.TYPE == "003" || figh.TYPE == "004") return;

                     var fp = mbsp.Fighter_Public;
                     iScsc.ExecuteCommand(string.Format("UPDATE Fighter SET Mtod_Code_Dnrm = {0}, Ctgy_Code_Dnrm = {1}, Cbmt_Code_Dnrm = {2} WHERE File_No = {3};", fp.MTOD_CODE, fp.CTGY_CODE, fp.CBMT_CODE, fp.FIGH_FILE_NO));

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                              new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("formcaller", GetType().Name))}
                           })
                     );
                  }
                  catch (Exception exc) { MessageBox.Show(exc.Message); }
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            
         }
      }

      private void Mbsp_Rwno_Text_DoubleClick(object sender, EventArgs e)
      {
          try
          {
              var mbsp = MbspBs.Current as Data.Member_Ship;
              if (mbsp == null) return;

              _DefaultGateway.Gateway(
                 new Job(SendType.External, "localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
                     new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", mbsp.FIGH_FILE_NO),
                              new XAttribute("mbsprwno", mbsp.RWNO),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
                 )
              );
          }
          catch { }
      }
   }
}
