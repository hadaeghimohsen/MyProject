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
using System.RoboTech.ExceptionHandlings;
using System.Xml.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Net;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class SALE_DVLP_F : UserControl
   {
      public SALE_DVLP_F()
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
         int prsn = PrsnBs.Position;
         int ordr = OrdrBs.Position;
         int ordt = OrdtBs.Position;
         int odst = OdstBs.Position;
        
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         PrsnBs.Position = prsn;
         OrdrBs.Position = ordr;
         OrdtBs.Position = ordt;
         OdstBs.Position = odst;

         IntrApbsBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "INTRODUCTION_INFO");
         JobApbsBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "JOBS_INFO");
         OrdrStatApbsBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "ORDERSTATS_INFO");
         OrdrApbsBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "ORDER_INFO");
         GhitBs.DataSource = iRoboTech.Group_Header_Items;
         DamtpBs.DataSource = iRoboTech.D_AMTPs;
         DodstBs.DataSource = iRoboTech.D_ODSTs;
         DysnoBs.DataSource = iRoboTech.D_YSNOs;
         requery = false;
      }

      private void SavePersonelInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Prsn_gv.PostEditor();
            PrsnBs.EndEdit();

            iRoboTech.SubmitChanges();

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

      private void AddOrdr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if(PrsnBs.List.Count == 0)return;
            if (OrdrBs.List.OfType<Data.Order>().Any(o => o.CODE == 0)) return;

            OrdrBs.AddNew();

            var ordr = OrdrBs.Current as Data.Order;
            if(ordr == null)return;

            var prob = PrsnBs.Current as Data.Personal_Robot;

            ordr.Personal_Robot = prob;
            ordr.STRT_DATE = DateTime.Now;
            ordr.ORDR_STAT = "001";
            //ordr.ORDR_NUMB = iRoboTech.Orders.Where(o => o.Robot == prob.Robot).Count() + 1;
            ordr.CHAT_ID = prob.CHAT_ID;
            ordr.ORDR_TYPE = "004";
            ordr.MDFR_STAT = "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void sERV_INTR_APBS_CODELookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute Isig_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("App_Base",
                                 new XAttribute("tablename", "INTRODUCTION_INFO"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void sERV_JOB_APBS_CODELookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute Isig_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("App_Base",
                                 new XAttribute("tablename", "JOBS_INFO"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveOrdr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Odst_gv.PostEditor();
            OrdrBs.EndEdit();
            OdstBs.EndEdit();

            iRoboTech.SubmitChanges();

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

      private void DeleteOrdr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if (ordr == null) return;
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            
            iRoboTech.DEL_ORDR_P(ordr.CODE);

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
            }
         }
      }

      private void CancelChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات نادیده گرفته شود؟", "از بین رفتن نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;            

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
            }
         }
      }

      private void PublicOrder_Tg_Toggled(object sender, EventArgs e)
      {
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if (ordr == null) return;            

            ordr.MDFR_STAT = PublicOrder_Tg.IsOn ? "002" : "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Ghit_LookupEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute Isig_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Lookup",
                                 new XAttribute("tablename", "groupheaderitem"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddOrderDetail_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Ghit_LookupEdit.EditValue == null || Ghit_LookupEdit.EditValue.ToString() == "") return;

            if (OrdrBs.Current == null) return;

            if (OrdtBs.List.OfType<Data.Order_Detail>().Any(od => od.CRET_BY == null)) return;

            if (OrdtBs.List.Count == 0 || OrdtBs.List.OfType<Data.Order_Detail>().Any(od => od.GHIT_CODE != (long)Ghit_LookupEdit.EditValue))
               OrdtBs.AddNew();
            else
               OrdtBs.Position = OrdtBs.IndexOf(OrdtBs.List.OfType<Data.Order_Detail>().FirstOrDefault(od => od.GHIT_CODE != (long)Ghit_LookupEdit.EditValue));
            
            var ordt = OrdtBs.Current as Data.Order_Detail;
            
            ordt.GHIT_CODE = (long)Ghit_LookupEdit.EditValue;

            var ghit = GhitBs.List.OfType<Data.Group_Header_Item>().FirstOrDefault(g => g.CODE == ordt.GHIT_CODE);

            ordt.NUMB = ordt.NUMB == null ? 1 : ++ordt.NUMB;

            ordt.GHIT_MIN_DATE = ordt.GHIT_MAX_DATE = DateTime.Now.AddSeconds((int)ghit.SCND_NUMB).AddMinutes((int)ghit.MINT_NUMB).AddHours((int)ghit.HORS_NUMB).AddDays((int)ghit.DAYS_NUMB).AddMonths((int)ghit.MONT_NUMB).AddYears((int)ghit.YEAR_NUMB);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Actn_butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var ordt = OrdtBs.Current as Data.Order_Detail;
            if (ordt == null) return;

            switch(e.Button.Index)
            {
               case 0:
                  Ordt_gv.PostEditor();
                  OrdtBs.EndEdit();
                  iRoboTech.SubmitChanges();
                  break;
               case 1:
                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iRoboTech.DEL_ODRT_P(ordt.ORDR_CODE, ordt.RWNO);
                  break;
            }
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

      private void OrdrStat_Lookup_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute Isig_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("App_Base",
                                 new XAttribute("tablename", "ORDERSTATS_INFO"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddOrderStat_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (OrdrStat_Lookup.EditValue == null || OrdrStat_Lookup.EditValue.ToString() == "") return;

            if (OrdrBs.Current == null) return;

            if (OdstBs.List.OfType<Data.Order_State>().Any(od => od.CODE == 0)) return;

            OdstBs.AddNew();
            var odst = OdstBs.Current as Data.Order_State;

            odst.APBS_CODE = (long)OrdrStat_Lookup.EditValue;
            odst.ORDR_CODE = (OrdrBs.Current as Data.Order).CODE;

            odst.STAT_DATE = DateTime.Now;            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DeleteOrdrStat_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var odst = OdstBs.Current as Data.Order_State;
            if (odst == null) return;
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iRoboTech.DEL_ODST_P(odst.ORDR_CODE, odst.CODE);

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
            }
         }
      }

      private void AddOrac_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (PersOrac_Lov.EditValue == null || PersOrac_Lov.EditValue.ToString() == "") return;

            if (OrdrBs.Current == null) return;

            if (OracBs.List.OfType<Data.Order_Access>().Any(oa => oa.CODE == 0)) return;
            if (OracBs.List.OfType<Data.Order_Access>().Any(oa => oa.PROB_SERV_FILE_NO == (long)PersOrac_Lov.EditValue)) return;

            OracBs.AddNew();

            var orac = OracBs.Current as Data.Order_Access;

            orac.PROB_SERV_FILE_NO = (long)PersOrac_Lov.EditValue;
            orac.PROB_ROBO_RBID = (OrdrBs.Current as Data.Order).PROB_ROBO_RBID;
            orac.ORDR_CODE = (OrdrBs.Current as Data.Order).CODE;

            orac.RECD_STAT = "002";

            OracBs.EndEdit();

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
               Execute_Query();
         }
      }

      private void OracActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  var orac = OracBs.Current as Data.Order_Access;
                  if (orac == null) return;
                  if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  iRoboTech.DEL_ORAC_P(orac.CODE);

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
               Execute_Query();
         }
      }

      private void OrdrApbs_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute Isig_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("App_Base",
                                 new XAttribute("tablename", "ORDER_INFO"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CalcExtrPrct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if (ordr == null) return;

            if (ordr.EXPN_AMNT != null)
               ordr.EXTR_PRCT = (ordr.EXPN_AMNT * Convert.ToInt64(Tax_Txt.Text)) / 100;
         }
         catch (Exception exc)
         {

         }
      }      
   }  
}
