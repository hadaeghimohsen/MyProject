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
      private bool requery = false;

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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No IN ( {0} )", filenos))}
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
               PBLC.ActiveFilterString = "TYPE != '003' And [colRemnDay] < -7";
               break;
            case "Gold_Lbl":
               PBLC.ActiveFilterString = "TYPE != '003' And [colRemnDay] < 0 And [colRemnDay] >= -7";
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

            string SuntCode = "";
            if (SuntCode_Lov.EditValue == null || SuntCode_Lov.Text == "")
               SuntCode = null;
            else
               SuntCode = SuntCode_Lov.EditValue.ToString();

            long? ClubCode = null;
            if (ClubCode_Lov.EditValue == null || ClubCode_Lov.Text == "")
               ClubCode = null;
            else
               ClubCode = (long?)ClubCode_Lov.EditValue;

            vF_Fighs.DataSource = iScsc.VF_Last_Info_Fighter(null, FrstName_Txt.Text, LastName_Txt.Text, NatlCode_Txt.Text, FngrPrnt_Txt.Text, CellPhon_Txt.Text, TellPhon_Txt.Text, (Men_Rb.Checked ? "001" : Women_Rb.Checked ? "002" : null), ServNo_Txt.Text, GlobCode_Txt.Text, null, null, null, null, SuntCode).Where(f => f.CLUB_CODE == (ClubCode ?? f.CLUB_CODE)).OrderByDescending(s => s.END_DATE).Take((int)FtchRows_Nud.Value);
            vF_Last_Info_FighterResultGridControl.Focus();

            requery = false;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void vF_Last_Info_FighterResultBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;

            // 1401/05/22 * Focused
            ServProf_Tc.SelectedIndex = 0;

            RqstBnFignInfo_Lb.Text = figh.NAME_DNRM;
            PayDebtAmnt_Txt.Text = figh.DEBT_DNRM.ToString();

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
            DebtPymtAmnt_Txt.EditValue = Convert.ToInt64(ExpnAmnt_Txt.EditValue) - (Convert.ToInt64(PymtAmnt_Txt.EditValue) + Convert.ToInt64(DscnAmnt_Txt.EditValue));
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
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;
            if (figh.FNGR_PRNT_DNRM == "") { return; }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", figh.FNGR_PRNT_DNRM))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;
            if (figh.FNGR_PRNT_DNRM == "") { return; }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", figh.FNGR_PRNT_DNRM))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;
            if (figh.FNGR_PRNT_DNRM == "") { return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", figh.FNGR_PRNT_DNRM))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrNewEnrollPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف اثر انگشت از مشتری و اختصاص برای کاربر جدید موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;
            if (figh.FNGR_PRNT_DNRM == "") { return; }

            // اثر انگشت را از دستگاه پاک میکنیم
            RqstBnDeleteFngrPrnt1_Click(null, null);

            // ابتدا کد انگشتی را از مشتری میگیریم
            ClearFingerPrint_Butn_Click(null, null);

            // باز کردن فرم ثبت نام برای مشتری جدید
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", figh.FNGR_PRNT_DNRM))}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
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

      private void ClearFingerPrint_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;
            if (figh.FNGR_PRNT_DNRM == "") { return; }

            iScsc.SCV_PBLC_P(
               new XElement("Process",
                  new XElement("Fighter",
                     new XAttribute("fileno", figh.FILE_NO),
                     new XAttribute("columnname", "FNGR_PRNT"),
                     new XAttribute("newvalue", "")
                  )
               )
            );
            Search_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

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

      private void ExcpAttnActv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null)
               figh = vF_Fighs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '001' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '002', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '002';", figh.FILE_NO
               )
            );
            MessageBox.Show("عملیات استثناء ورود با موفقیت فعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExcpAttnDact_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null)
               figh = vF_Fighs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '001' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '001', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '001';", figh.FILE_NO
               )
            );
            MessageBox.Show("عملیات استثناء ورود با موفقیت غیرفعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PayCashDebt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt_Txt.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "001"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            Search_Butn_Click(null, null);
         }
      }

      private void PayPosDebt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt_Txt.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            // مشخص شدن پوز
            var VPosBs1 = iScsc.V_Pos_Devices;//.Where(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value);
            var UsePos_Cb = true;
            var DefPos = VPosBs1.FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value);

            if (VPosBs1.Count() == 0) UsePos_Cb = false;

            if (UsePos_Cb)
            {
               var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

               long psid;
               if (DefPos == null)
               {
                  var posdflts = VPosBs1.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
                  if (posdflts.Count() == 1)
                  {
                     DefPos = posdflts.FirstOrDefault();
                     psid = DefPos.PSID;
                  }
                  else
                  {
                     return;
                  }
               }
               else
               {
                  psid = (long)DefPos.PSID;
               }

               if (regl.AMNT_TYPE == "002")
                  paydebt = paydebt * 10;

               // از این گزینه برای این استفاده میکنیم که بعد از پرداخت نباید درخواست ثبت نام پایانی شود
               UsePos_Cb = false;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.External, "Commons",
                           new List<Job>
                           {
                              new Job(SendType.Self, 34 /* Execute PosPayment */)
                              {
                                 Input = 
                                    new XElement("PosRequest",
                                       new XAttribute("psid", psid),
                                       new XAttribute("subsys", 5),
                                       new XAttribute("rqid", 0),
                                       new XAttribute("rqtpcode", ""),
                                       new XAttribute("router", GetType().Name),
                                       new XAttribute("callback", 21),
                                       new XAttribute("amnt", paydebt )
                                    )
                              }
                           }
                        )
                     }
                  )
               );

               UsePos_Cb = true;
            }
            else
            {
               var vf_SavePayment =
                  iScsc.VF_Save_Payments(null, figh.FILE_NO)
                  .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);

               foreach (var pymt in vf_SavePayment)
               {
                  var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
                  long amnt = 0;

                  if (debt > paydebt)
                     // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                     amnt = paydebt;
                  else
                     // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                     amnt = debt;

                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQID),
                              new XAttribute("amnt", amnt),
                              new XAttribute("rcptmtod", "003"),
                              new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                           )
                        )
                     )
                  );

                  paydebt -= amnt;
                  if (paydebt == 0) break;
               }
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            Search_Butn_Click(null, null);
         }
      }

      private void PayDepositeDebt_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt_Txt.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "005"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            Search_Butn_Click(null, null);
         }
      }

      private void PayCashDepositeAmnt_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;            

            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;
            var paydeposite = Convert.ToInt64(PayDepositeAmnt_Tsmi.Text.Replace(",", ""));
            if (paydeposite == 0) return;

            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", figh.FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("glid", 0),
                        new XAttribute("type", "002" /* روش جدید برای ذخیره سازی اطلاعات */),
                        new XAttribute("amnt", paydeposite),
                        new XAttribute("paiddate", ""),
                        new XAttribute("dpststat", "002"),
                        new XAttribute("resndesc", "افزایش سپرده در فرم کنترل میز"),
                        new XElement("Gain_Loss_Rial_Detials",
                           new XElement("Gain_Loss_Rial_Detial",
                              new XAttribute("rwno", 1),
                              new XAttribute("amnt", paydeposite),
                              new XAttribute("rcptmtod", "001")
                           )
                        )
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "020" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
              iScsc.Requests
              .Where(
                 rqst =>
                    Rqids.Contains(rqst.RQID)
              ).FirstOrDefault();

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );

            PayDepositeAmnt_Tsmi.Text = "0";

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Search_Butn_Click(null, null);
         }
      }

      private void PayPosDepositeAmnt_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;            

            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;
            var paydeposite = Convert.ToInt64(PayDepositeAmnt_Tsmi.Text.Replace(",", ""));
            if (paydeposite == 0) return;

            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", figh.FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("glid", 0),
                        new XAttribute("type", "002" /* روش جدید برای ذخیره سازی اطلاعات */),
                        new XAttribute("amnt", paydeposite),
                        new XAttribute("paiddate", ""),
                        new XAttribute("dpststat", "002"),
                        new XAttribute("resndesc", "افزایش سپرده در فرم کنترل میز"),
                        new XElement("Gain_Loss_Rial_Detials",
                           new XElement("Gain_Loss_Rial_Detial",
                              new XAttribute("rwno", 1),
                              new XAttribute("amnt", paydeposite),
                              new XAttribute("rcptmtod", "003")
                           )
                        )
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "020" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
              iScsc.Requests
              .Where(
                 rqst =>
                    Rqids.Contains(rqst.RQID)
              ).FirstOrDefault();

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );

            PayDepositeAmnt_Tsmi.Text = "0";

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Search_Butn_Click(null, null);
         }
      }

      private void RqstBnGrnt_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null) return;

            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            if (figh.FIGH_STAT == "001") return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.Self, 162 /* Execute Wrn_Serv_F */),
                     new Job(SendType.SelfToUserInterface, "WRN_SERV_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SelectExportContactFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            if (ExportFile_Sfd.ShowDialog() != DialogResult.OK) return;
            SelectExportContactFile_Butn.Tag = ExportFile_Sfd.FileName;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExportContact_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if(SelectExportContactFile_Butn.Tag == null)
            {
               SelectExportContactFile_Butn_Click(null, null);
               if (ExportFile_Sfd.FileName == null) return;
            }

            if(ConfDate_Cbx.Checked && !ServConfDate_Dt.Value.HasValue)
            {
               ServConfDate_Dt.Focus();
               return;
            }

            string fileExport = SelectExportContactFile_Butn.Tag.ToString();

            if (GoogleContact_Rb.Checked)
            {
               File.AppendAllText(fileExport, @"Name,Given Name,Additional Name,Family Name,Yomi Name,Given Name Yomi,Additional Name Yomi,Family Name Yomi,Name Prefix,Name Suffix,Initials,Nickname,Short Name,Maiden Name,Birthday,Gender,Location,Billing Information,Directory Server,Mileage,Occupation,Hobby,Sensitivity,Priority,Subject,Notes,Language,Photo,Group Membership,E-mail 1 - Type,E-mail 1 - Value,Phone 1 - Type,Phone 1 - Value,Phone 2 - Type,Phone 2 - Value,Phone 3 - Type,Phone 3 - Value,Phone 4 - Type,Phone 4 - Value,Phone 5 - Type,Phone 5 - Value,Organization 1 - Type,Organization 1 - Name,Organization 1 - Yomi Name,Organization 1 - Title,Organization 1 - Department,Organization 1 - Symbol,Organization 1 - Location,Organization 1 - Job Description" + Environment.NewLine);

               string contactsList = "";
               foreach (var contact in vF_Fighs.List.OfType<Data.VF_Last_Info_FighterResult>().Where(s => (ConfDate_Cbx.Checked && s.CONF_DATE.Value.Date >= ServConfDate_Dt.Value.Value.Date) || !ConfDate_Cbx.Checked))
               {
                  contactsList +=
                     string.Format(
                        "{0},{1},,{1},,,,,,,,,,,{2},,,,,,,,,,,,,,,,,Mobile,{3},Home,{4},Dad Mobile,{5},Mom Mobile,{6},,,,,,,,,,{7}",
                        contact.FRST_NAME,
                        contact.LAST_NAME + " ( " + ExportLabel_Txt.Text + " )",
                        contact.BRTH_DATE_DNRM,
                        contact.CELL_PHON_DNRM,
                        contact.TELL_PHON_DNRM,
                        contact.DAD_CELL_PHON_DNRM,
                        contact.MOM_CELL_PHON_DNRM,
                        Environment.NewLine
                     );
               }

               File.AppendAllText(fileExport, contactsList);
            }
            else if(TextFile_Rb.Checked)
            {
               int i = 0, j = 1;
               var splitFiles = fileExport.Replace(".txt", string.Format(" [1-{0}].txt", ServRecd_Spn.Value));
               File.Create(splitFiles).Close();
               foreach (var contact in vF_Fighs.List.OfType<Data.VF_Last_Info_FighterResult>().Where(s => (ConfDate_Cbx.Checked && s.CONF_DATE.Value.Date >= ServConfDate_Dt.Value.Value.Date) || !ConfDate_Cbx.Checked))
               {
                  // Self Service
                  if (contact.CELL_PHON_DNRM != null && contact.CELL_PHON_DNRM.Length >= 10)
                  {
                     ++i;
                     File.AppendAllText(splitFiles, (contact.CELL_PHON_DNRM.StartsWith("0") ? "98" + contact.CELL_PHON_DNRM.Substring(1) : "98" + contact.CELL_PHON_DNRM) + Environment.NewLine);
                  }
                  // Dad Service
                  if (contact.DAD_CELL_PHON_DNRM != null && contact.DAD_CELL_PHON_DNRM.Length >= 10)
                  {
                     ++i;
                     File.AppendAllText(splitFiles, (contact.DAD_CELL_PHON_DNRM.StartsWith("0") ? "98" + contact.DAD_CELL_PHON_DNRM.Substring(1) : "98" + contact.DAD_CELL_PHON_DNRM) + Environment.NewLine);
                  }
                  // Mom Service
                  if (contact.MOM_CELL_PHON_DNRM != null && contact.MOM_CELL_PHON_DNRM.Length >= 10)
                  {
                     ++i;
                     File.AppendAllText(splitFiles, (contact.MOM_CELL_PHON_DNRM.StartsWith("0") ? "98" + contact.MOM_CELL_PHON_DNRM.Substring(1) : "98" + contact.MOM_CELL_PHON_DNRM) + Environment.NewLine);
                  }

                  if(i % ServRecd_Spn.Value == 0)
                  {
                     ++j;
                     splitFiles = fileExport.Replace(".txt", string.Format(" [{0}-{1}].txt", i + 1, ServRecd_Spn.Value * j));
                     File.Create(splitFiles).Close();
                  }
               }
            }

            MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ChatHist_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SavePblc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _LastInfo = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            var _dbLastInfo = iScsc.VF_Last_Info_Fighter(_LastInfo.FILE_NO, null, null, null, null, null, null, null, null, null, null, null, null, null, null).FirstOrDefault();

            string _chngClmn = ""; //= "ستون هایی که شما تغییر داده اید :" + Environment.NewLine;
            bool _savedb = false;

            if (_LastInfo.CELL_PHON_DNRM != null && _LastInfo.CELL_PHON_DNRM != _dbLastInfo.CELL_PHON_DNRM)
            {
               _chngClmn += string.Format("شماره تلفن همراه" + " : {0}", _LastInfo.CELL_PHON_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.TELL_PHON_DNRM != null && _LastInfo.TELL_PHON_DNRM != _dbLastInfo.TELL_PHON_DNRM)
            {
               _chngClmn += string.Format("شماره تلفن ثابت" + " : {0}", _LastInfo.TELL_PHON_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.DAD_CELL_PHON_DNRM != null && _LastInfo.DAD_CELL_PHON_DNRM != _dbLastInfo.DAD_CELL_PHON_DNRM)
            {
               _chngClmn += string.Format("شماره تلفن همراه پدر" + " : {0}", _LastInfo.DAD_CELL_PHON_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.MOM_CELL_PHON_DNRM != null && _LastInfo.MOM_CELL_PHON_DNRM != _dbLastInfo.MOM_CELL_PHON_DNRM)
            {
               _chngClmn += string.Format("شماره تلفن همراه مادر" + " : {0}", _LastInfo.MOM_CELL_PHON_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.SUNT_CODE != null && _LastInfo.SUNT_CODE != _dbLastInfo.SUNT_CODE)
            {
               _chngClmn += string.Format("اطلاعات سازمان" + " : {0}", _LastInfo.SUNT_CODE) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.GLOB_CODE != null && _LastInfo.GLOB_CODE != _dbLastInfo.GLOB_CODE)
            {
               _chngClmn += string.Format("کد پرسنلی" + " : {0}", _LastInfo.GLOB_CODE) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.NATL_CODE != null && _LastInfo.NATL_CODE != _dbLastInfo.NATL_CODE)
            {
               _chngClmn += string.Format("کد ملی" + " : {0}", _LastInfo.NATL_CODE) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.SERV_NO_DNRM != null && _LastInfo.SERV_NO_DNRM != _dbLastInfo.SERV_NO_DNRM)
            {
               _chngClmn += string.Format("کد اشتراک" + " : {0}", _LastInfo.SERV_NO_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.CHAT_ID_DNRM != null && _LastInfo.CHAT_ID_DNRM != _dbLastInfo.CHAT_ID_DNRM)
            {
               _chngClmn += string.Format("کد بله" + " : {0}", _LastInfo.CHAT_ID_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.DAD_CHAT_ID_DNRM != null && _LastInfo.DAD_CHAT_ID_DNRM != _dbLastInfo.DAD_CHAT_ID_DNRM)
            {
               _chngClmn += string.Format("کد بله پدر" + " : {0}", _LastInfo.DAD_CHAT_ID_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.MOM_CHAT_ID_DNRM != null && _LastInfo.MOM_CHAT_ID_DNRM != _dbLastInfo.MOM_CHAT_ID_DNRM)
            {
               _chngClmn += string.Format("کد بله مادر" + " : {0}", _LastInfo.MOM_CHAT_ID_DNRM) + Environment.NewLine;
               _savedb = true;
            }

            if (_LastInfo.INSR_DATE_DNRM != null && _LastInfo.INSR_DATE_DNRM != _dbLastInfo.INSR_DATE_DNRM)
            {
               _chngClmn += string.Format("تاریخ بیمه" + " : {0}", _LastInfo.INSR_DATE_DNRM.Value.ToString("yyyy-MM-dd")) + Environment.NewLine;
               _savedb = true;
            }

            if (!_savedb) return;

            if (MessageBox.Show(this, "آیا با ذخیره کردن اطلاعات موافق هستید؟" + Environment.NewLine + _chngClmn, "ثبت اطلاعات", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.SCVF_PBLC_P(
               new XElement("Fighter",
                   new XAttribute("fileno", _LastInfo.FILE_NO),
                   new XAttribute("cellphon", (_LastInfo.CELL_PHON_DNRM ?? "").ToString().Trim()),
                   new XAttribute("tellphon", (_LastInfo.TELL_PHON_DNRM ?? "").ToString().Trim()),
                   new XAttribute("dadcellphon", (_LastInfo.DAD_CELL_PHON_DNRM ?? "").ToString().Trim()),
                   new XAttribute("momcellphon", (_LastInfo.MOM_CELL_PHON_DNRM ?? "").ToString().Trim()),
                   new XAttribute("suntcode", _LastInfo.SUNT_CODE ?? ""),
                   new XAttribute("globcode", (_LastInfo.GLOB_CODE ?? "").ToString().Trim()),
                   new XAttribute("servno", (_LastInfo.SERV_NO_DNRM ?? "").ToString().Trim()),
                   new XAttribute("natlcode", (_LastInfo.NATL_CODE ?? "").ToString().Trim()),
                   new XAttribute("insrdate", _LastInfo.INSR_DATE_DNRM.HasValue ? _LastInfo.INSR_DATE_DNRM.Value.Date.ToString("yyyy-MM-dd") : ""),
                   new XAttribute("chatid", _LastInfo.CHAT_ID_DNRM ?? 0),
                   new XAttribute("dadchatid", _LastInfo.DAD_CHAT_ID_DNRM ?? 0),
                   new XAttribute("momchatid", _LastInfo.MOM_CHAT_ID_DNRM ?? 0)
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ConfDate_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         ServConfDate_Dt.Visible = ConfDate_Cbx.Checked;
      }

      private void TypeContact_Rb_CheckedChanged(object sender, EventArgs e)
      {
         var rb = sender as RadioButton;
         if (rb == null) return;

         switch (rb.Text)
         {
            case "Google":
               ServRecd_Spn.Visible = false;
               break;
            case "Text":
               ServRecd_Spn.Visible = true;
               break;
         }
      }

      private void ServProf_Tc_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            var _serv = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            if (_serv == null) return;            

            switch (ServProf_Tc.SelectedTab.Name)
            {
               case "tp_002":
                  vF_SavePaymentsBs.DataSource = iScsc.VF_Save_Payments(null, _serv.FILE_NO);
                  break;
               case "tp_003":
                  FgdcBs.DataSource = iScsc.Fighter_Discount_Cards.Where(d => d.FIGH_FILE_NO == _serv.FILE_NO);
                  break;
               case "tp_004":
                  VSmsBs7.DataSource = iScsc.V_Sms_Message_Boxes.Where(s => s.PHON_NUMB == _serv.CELL_PHON_DNRM);
                  break;
               case "tp_005":
                  var _mbsp = MbspBs.Current as Data.Member_Ship;
                  if (_mbsp == null) return;

                  AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == _serv.FILE_NO && a.MBSP_RWNO_DNRM == _mbsp.RWNO);
                  break;
               default: break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PydtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {

      }
   }
}
