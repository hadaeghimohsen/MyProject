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
            var CrntFigh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
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
         if (vF_Fighs.Current == null) return;
         string filenos = "";

         foreach (int i in PBLC.GetSelectedRows())
         {
            vF_Fighs.Position = i;
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
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
         index = vF_Fighs.Position;
         var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
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
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "accesscontrol"), new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM), new XAttribute("attnsystype", "001"))}
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

         vF_Fighs.Position = index;
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
            case "NullProfile_Lbl":
               break;
            case "NullDcmt_Lbl":
               break;
            case "YesInsr_Lbl":
               PBLC.ActiveFilterString = string.Format("TYPE != '003' And INSR_DATE_DNRM >= #{0}#", DateTime.Now.ToShortDateString());
               break;
            case "NoInsr_Lbl":
               PBLC.ActiveFilterString = string.Format("TYPE != '003' And (IsNullOrEmpty(INSR_DATE_DNRM) OR INSR_DATE_DNRM < #{0}#)", DateTime.Now.ToShortDateString());
               break;
         }
      }

      private void TrnsFngrPrnt_Butn_Click(object sender, EventArgs e)
      {         
         if (MessageBox.Show(this, "آیا با انتقال شناسایی کارت اعضا به دستگاه موافق هستید؟", "عملیات انتقال", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         foreach (Data.VF_Last_Info_FighterResult figh in vF_Fighs.List.OfType<Data.VF_Last_Info_FighterResult>().Where(f => f.END_DATE < DateTime.Now))
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

            vF_Fighs.DataSource = iScsc.VF_Last_Info_Fighter(null, FrstName_Txt.Text, LastName_Txt.Text, NatlCode_Txt.Text, FngrPrnt_Txt.Text, CellPhon_Txt.Text, TellPhon_Txt.Text, (Men_Rb.Checked ? "001" : Women_Rb.Checked ? "002" : null), ServNo_Txt.Text, GlobCode_Txt.Text, null, null, null, null);
            vF_Last_Info_FighterResultGridControl.Focus();
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void vF_Last_Info_FighterResultBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;

            RqstBnFignInfo_Lb.Text = figh.NAME_DNRM;

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

            MbspValdType_Butn.Text = mbsp.VALD_TYPE == "001" ? "فعال کردن" : "غیرفعال کردن";

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
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
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
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
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

              Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>231</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 231 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),
                     #region DoWork
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
                     #endregion
                  });
              _DefaultGateway.Gateway(_InteractWithScsc);

              //_DefaultGateway.Gateway(
              //   new Job(SendType.External, "localhost",
              //      new List<Job>
              //    {
              //       new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
              //       new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
              //       {
              //          Input = 
              //             new XElement("Fighter",
              //                new XAttribute("fileno", mbsp.FIGH_FILE_NO),
              //                new XAttribute("mbsprwno", mbsp.RWNO),
              //                new XAttribute("formcaller", GetType().Name)
              //             )
              //       }
              //    }
              //   )
              //);
          }
          catch { }
      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            //if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("Command",
            //            new XAttribute("type", "fngrprntdev"),
            //            new XAttribute("fngractn", "enroll"),
            //            new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
            //         )
            //   }
            //);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            //if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("Command",
            //            new XAttribute("type", "fngrprntdev"),
            //            new XAttribute("fngractn", "enroll"),
            //            new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
            //         )
            //   }
            //);
         }
         catch (Exception exc) { }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;
            if (figh.FNGR_PRNT_DNRM == "") { return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM)
                     )
               }
            );
         }
         catch { }
      }

      private void RqstBnDeleteFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;
            if (figh.FNGR_PRNT_DNRM == "") { return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM)
                     )
               }
            );
         }
         catch { }
      }
      #endregion

      private void RqstBnNewMbsp_Click(object sender, EventArgs e)
      {
         AttnMbsp_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(AttnMbsp_Butn.Buttons[1]));
      }

      private void RqstBnEditPblc_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Fighs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"), new XAttribute("formcaller", GetType().Name))}
               })
         );
      }

      private void RqstBnInsr_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Fighs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 80 /* Execute Ins_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewinscard"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("formcaller", GetType().Name))}
               })
         );
      }

      private void RqstBnBlok_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Fighs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 133 /* Execute Adm_Mbfz_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBFZ_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "block"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("formcaller", GetType().Name))}
               })
         );
      }

      private void PymtBnDebt_Click(object sender, EventArgs e)
      {
         colActn_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(colActn_Butn.Buttons[6]));
      }

      private void MbspMark_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            Job _InteractWithScsc =
             new Job(SendType.External, "Localhost",
                new List<Job>
                  {                     
                     new Job(SendType.Self, 155 /* Execute Mbsp_Mark_F */),
                     new Job(SendType.SelfToUserInterface, "MBSP_MARK_F", 10 /* execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Member_Ship",
                              new XAttribute("fileno", mbsp.FIGH_FILE_NO),
                              new XAttribute("rwno", mbsp.RWNO),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void SmsHist_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sms = sender as SimpleButton;

            string phon = "";
            switch(sms.Tag.ToString())
            {
               case "Cell_Phon":
                  phon = CellPhon01_Txt.Text;
                  break;
               case "Dad_Phon":
                  phon = DadPhon01_Txt.Text;
                  break;
               case "Mom_Phon":
                  phon = MomPhon01_Txt.Text;
                  break;
            }

            if (phon == null || phon == "")
               return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "DefaultGateway:Msgb", 07 /* Execute Send_Mesg_F */, SendType.Self)
            );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "DefaultGateway:Msgb:SEND_MESG_F", 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Message", new XAttribute("tab", "tp_004"), new XAttribute("filtering", "phonnumb"), new XAttribute("valu", phon)) }
            );
         }
         catch (Exception)
         {

            throw;
         }
      }
   }
}
