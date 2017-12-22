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

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
                     })
               );
               break;
            case 2:
               if (MessageBox.Show(this, "آیا با حذف هنرجو موافق هستید؟", "عملیات حذف موقت هنرجو", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))},
                        new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 07 /* Execute Load_Data */),
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
               PBLC.ActiveFilterString = "TYPE != '003' And DEBT_DNRM < 0";
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

      private void DeltFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         var users = vF_Last_Info_FighterResultBindingSource.List.OfType<Data.VF_Last_Info_FighterResult>().Where(f => f.END_DATE.HasValue && f.END_DATE.Value.AddDays((double)Days_Nud.Value) < DateTime.Now);
         Users_Lb.Text = string.Format("تعداد کاربران : {0}", users.Count());
         if (MessageBox.Show(this, "آیا با حذف اطلاعات کاربر از دستگاه موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         
         foreach (Data.VF_Last_Info_FighterResult figh in users)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 43, SendType.SelfToUserInterface)
               {
                  Input =
                  new XElement("User",
                     new XAttribute("enrollnumb", figh.FNGR_PRNT_DNRM),
                     new XAttribute("functype", "5.2.3.10")
                  )
               }
            );
         }
      }

      private void Days_Nud_ValueChanged(object sender, EventArgs e)
      {
         try
         {
            var users = vF_Last_Info_FighterResultBindingSource.List.OfType<Data.VF_Last_Info_FighterResult>().Where(f => f.END_DATE.HasValue && f.END_DATE.Value.AddDays((double)Days_Nud.Value) < DateTime.Now);
            Users_Lb.Text = string.Format("تعداد کاربران : {0}", users.Count());            
         }
         catch { }
      }
   }
}
