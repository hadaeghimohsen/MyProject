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
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.RoboTech.ExtCode;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class PROD_DVLP_F : UserControl
   {
      public PROD_DVLP_F()
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
         int rbpr = RbprBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         RbprBs.Position = rbpr;

         int grop = VGexpBs.Position;
         int brnd = VBexpBs.Position;
         int unit = UnitBs.Position;

         VGexpBs.DataSource = iRoboTech.V_Group_Expenses.Where(g => g.GROP_TYPE == "001");
         VBexpBs.DataSource = iRoboTech.V_Group_Expenses.Where(g => g.GROP_TYPE == "002");
         UnitBs.DataSource = iRoboTech.App_Base_Defines.Where(u => u.ENTY_NAME == "PRODUCTUNIT_INFO");

         VGexpBs.Position = grop;
         VBexpBs.Position = brnd;
         UnitBs.Position = unit;

         SaveProdDataOnSrvr_Pb.Visible = SaveProdsDataOnSrvr_Pb.Visible = SaveProdFileOnSrvr_Pb.Visible = false;
         requery = false;
      }

      #region Group Expense
      private void AddGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (Ordr_Txt.Text == "") { Ordr_Txt.Focus(); return; }
            
            if (GropDesc_Txt.Text == "") { GropDesc_Txt.Focus(); return; }

            var grop = VGexpBs.Current as Data.V_Group_Expense;            

            if(grop == null)
               iRoboTech.DBL_INS_GEXP_P(null, "001", 0, GropDesc_Txt.Text, "002", LinkJoin_Txt.Text, null);
            else
               iRoboTech.DBL_INS_GEXP_P((CretNewSuprGrop_Cbx.Checked ? null : (long?)grop.CODE), "001", 0, GropDesc_Txt.Text, "002", LinkJoin_Txt.Text, null);

            LinkJoin_Txt.Text = GropDesc_Txt.Text = "";
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

      private void SaveGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Gexp_Tl.PostEditor();
            
            var grop = VGexpBs.Current as Data.V_Group_Expense;
            if (grop == null) return;

            iRoboTech.DBL_UPD_GEXP_P(grop.CODE, grop.GEXP_CODE, grop.GROP_TYPE, grop.ORDR, grop.GROP_DESC, grop.STAT, "", grop.GROP_ORDR);
            
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

      private void DelGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا حذف گروه موافق هستین؟", "حذف گروه", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var grop = VGexpBs.Current as Data.V_Group_Expense;
            if (grop == null) return;

            iRoboTech.DBL_DEL_GEXP_P(grop.CODE);

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
      #endregion

      #region Brands
      private void AddBrnd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (BrndDesc_Txt.Text == "") { BrndDesc_Txt.Focus(); return; }

            var brnd = VBexpBs.Current as Data.V_Group_Expense;

            iRoboTech.DBL_INS_GEXP_P(null, "002", 0, BrndDesc_Txt.Text, "002", "", null);

            BrndDesc_Txt.Text = "";
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

      private void SaveBrnd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Brnd_Gv.PostEditor();

            var brnd = VBexpBs.Current as Data.V_Group_Expense;
            if (brnd == null) return;

            iRoboTech.DBL_UPD_GEXP_P(brnd.CODE, brnd.GEXP_CODE, brnd.GROP_TYPE, brnd.ORDR, brnd.GROP_DESC, brnd.STAT, "", null);

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

      private void DelBrnd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا حذف برند موافق هستین؟", "حذف برند", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var brnd = VBexpBs.Current as Data.V_Group_Expense;
            if (brnd == null) return;

            iRoboTech.DBL_DEL_GEXP_P(brnd.CODE);

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
      #endregion

      #region Unit
      private void AddUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (UnitDesc_Txt.Text == "") { UnitDesc_Txt.Focus(); return; }

            iRoboTech.INS_APBS_P(UnitDesc_Txt.Text, "PRODUCTUNIT_INFO", null, null, null, null, null, null, null, null, null, null, null, null);

            UnitDesc_Txt.Text = "";
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

      private void SaveUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Unit_Gv.PostEditor();

            var unit = UnitBs.Current as Data.App_Base_Define;
            if (unit == null) return;

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

      private void DelUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا حذف واحد موافق هستین؟", "حذف واحد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var unit = UnitBs.Current as Data.App_Base_Define;
            if (unit == null) return;

            iRoboTech.DEL_APBS_P(unit.CODE);

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
      #endregion

      private void AddRoboProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RbprBs.List.OfType<Data.Robot_Product>().Any(p => p.CODE == 0)) return;

            var rbpr = RbprBs.AddNew() as Data.Robot_Product;
            rbpr.STAT = "002";
            rbpr.PROD_LIFE_STAT = "001";
            rbpr.MIN_ORDR_DNRM = 1;
            rbpr.ALRM_MIN_NUMB_DNRM = 1;
            rbpr.WEGH_AMNT_DNRM = 1000;
            rbpr.PROD_TYPE_DNRM = "002";
            rbpr.NUMB_TYPE = "001";
            rbpr.GRNT_STAT_DNRM = "001";
            rbpr.GRNT_NUMB_DNRM = 0;
            rbpr.GRNT_TIME_DNRM = "000";
            rbpr.GRNT_TYPE_DNRM = "000";
            rbpr.WRNT_STAT_DNRM = "001";
            rbpr.WRNT_NUMB_DNRM = 0;
            rbpr.WRNT_TIME_DNRM = "000";
            rbpr.WRNT_TYPE_DNRM = "000";
            rbpr.PROD_SUPL_LOCT_STAT = "001";

            iRoboTech.Robot_Products.InsertOnSubmit(rbpr);

            Master_Tc.SelectedTab = ProductDef_Tp;
            Product_Tc.SelectedTab = ProductInfo_Tp;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveRoboProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Rbpr_Gv.PostEditor();
            RbprBs.EndEdit(); 

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

      private void DelRoboProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            if (MessageBox.Show(this, "آیا با حذف محصول موافق هستین؟", "حذف محصول", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.DEL_RBPR_P(rbpr.CODE);

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

      private void Unit_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            if (e.NewValue != null)
            {               
               rbpr.App_Base_Define = iRoboTech.App_Base_Defines.SingleOrDefault(u => u.CODE == (long)e.NewValue);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RbprBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            Unit_Lov.EditValue = rbpr.UNIT_APBS_CODE;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddAltrProd_Butn_Click(object sender, EventArgs e)
      {
         try         
         {
            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            if (RpalBs.List.OfType<Data.Robot_Product_Alternative>().Any(a => a.CODE == 0)) return;

            var rpal = RpalBs.AddNew() as Data.Robot_Product_Alternative;
            rpal.RBPR_CODE = rbpr.CODE;

            iRoboTech.Robot_Product_Alternatives.InsertOnSubmit(rpal);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveAltrProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Rpal_Gv.PostEditor();

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

      private void DelAltrProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rpal = RpalBs.Current as Data.Robot_Product_Alternative;
            if (rpal == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف کالای جایگزین", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iRoboTech.Robot_Product_Alternatives.DeleteOnSubmit(rpal);

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

      private void AddOffProd_Butn_Click(object sender, EventArgs e)
      {
         if (RpdcBs.List.OfType<Data.Robot_Product_Discount>().Any(d => d.CODE == 0)) return;

         var robo = RoboBs.Current as Data.Robot;
         var rbpr = RbprBs.Current as Data.Robot_Product;

         if (robo == null || rbpr == null) return;

         var rpdc = RpdcBs.AddNew() as Data.Robot_Product_Discount;
         rpdc.ROBO_RBID = robo.RBID;
         rpdc.RBPR_CODE = rbpr.CODE;
         iRoboTech.Robot_Product_Discounts.InsertOnSubmit(rpdc);
      }

      private void SaveOffProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Rpdc_Gv.PostEditor();

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

      private void DelOffProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rpdc = RpdcBs.Current as Data.Robot_Product_Discount;
            if (rpdc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف نخفیف کالا", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Robot_Product_Discounts.DeleteOnSubmit(rpdc);

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

      private void AddGiftProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SlpgBs.List.OfType<Data.Service_Robot_Seller_Product_Gift>().Any(g => g.CODE == 0)) return;

            //var rbpr = RbprBs.Current as Data.Robot_Product;
            var srsp = SrspBs.Current as Data.Service_Robot_Seller_Product;

            var slpg = SlpgBs.AddNew() as Data.Service_Robot_Seller_Product_Gift;
            slpg.SRSP_CODE = srsp.CODE;//srsp.Service_Robot_Seller_Products.FirstOrDefault(sp => sp.RBPR_CODE == rbpr.CODE).CODE;

            slpg.STAT = "002";

            iRoboTech.Service_Robot_Seller_Product_Gifts.InsertOnSubmit(slpg);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelGiftProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var slpg = SlpgBs.Current as Data.Service_Robot_Seller_Product_Gift;
            if (slpg == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف کالای هدیه", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Service_Robot_Seller_Product_Gifts.DeleteOnSubmit(slpg);

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

      private void GiftProd_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var slpg = SlpgBs.Current as Data.Service_Robot_Seller_Product_Gift;
            if (slpg == null) return;

            slpg.SSPG_CODE = (long)e.NewValue;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

      private void SaveGiftProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Slpg_Gv.PostEditor();

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

      private void AddStorProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SlpsBs.List.OfType<Data.Service_Robot_Seller_Product_Store>().Any(s => s.CODE == 0)) return;

            //var rbpr = RbprBs.Current as Data.Robot_Product;
            var srsp = SrspBs.Current as Data.Service_Robot_Seller_Product;

            if (srsp == null) return;

            var slps = SlpsBs.AddNew() as Data.Service_Robot_Seller_Product_Store;
            slps.SRSP_CODE = srsp.CODE;
            slps.STOR_DATE = slps.MAKE_DATE = DateTime.Now;
            slps.EXPR_DATE = DateTime.Now.AddYears(1);

            iRoboTech.Service_Robot_Seller_Product_Stores.InsertOnSubmit(slps);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelStorProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var slps = SlpsBs.Current as Data.Service_Robot_Seller_Product_Store;
            if (slps == null) return;

            if (MessageBox.Show(this, "ایا با حذف رکورد موافق هستید؟", "حذف رکورد موجودی انبار", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Service_Robot_Seller_Product_Stores.DeleteOnSubmit(slps);

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

      private void SaveStorProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Slps_Gv.PostEditor();

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

      private void SrspBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var srsp = SrspBs.Current as Data.Service_Robot_Seller_Product;
            if (srsp == null) return;

            SspsBs.DataSource = iRoboTech.Service_Robot_Seller_Products.Where(a => a.SRBS_CODE == srsp.SRBS_CODE);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowFltrProd_Butn_Click(object sender, EventArgs e)
      {
         Rbpr_Gv.OptionsFind.AlwaysVisible = !Rbpr_Gv.OptionsFind.AlwaysVisible;         
      }

      private void SaveRbpp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Rbpp_Gv.PostEditor();

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

      private void DelRbpp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbpp = RbppBs.Current as Data.Robot_Product_Preview;
            if (rbpp == null) return;

            if (MessageBox.Show(this, "آیا با حذف فایل های نمایش محصول موافق هستید؟", "حذف فایل نمایش محصول", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Robot_Product_Previews.DeleteOnSubmit(rbpp);

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

      private void UpldImagFileProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (UpldImgeFileProd_Ofd.ShowDialog(this) != DialogResult.OK) return;

            var adminChat =
               iRoboTech.Service_Robots.FirstOrDefault(sr => sr.Service_Robot_Groups.Any(srg => srg.STAT == "002" && (srg.Group.GPID == 131 || srg.Group.GPID == 133 || srg.Group.GPID == 134)));

            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null) return;

            var fileNames = UpldImgeFileProd_Ofd.FileNames.Where(f => !RbppBs.List.OfType<Data.Robot_Product_Preview>().Any(p => p.SORC_FILE_PATH != null && p.SORC_FILE_PATH == f));
            if (fileNames.Count() == 0) return;

            SaveProdDataOnSrvr_Pb.Visible = true;

            // Step 1 Send Media to The BALE Server
            // Step 2 Get FileId from Server
            // Step 3 Save FileId For Selected Product
            #region Send Message
            // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Robot", 
                              new XAttribute("runrobot", "start"),
                              new XAttribute("actntype", "upldmediafile"),
                              new XAttribute("chatid", adminChat.CHAT_ID),
                              new XAttribute("command", prod.TARF_CODE),
                              new XAttribute("rbid", adminChat.ROBO_RBID),
                              new XAttribute("mesg", string.Join(";",  fileNames)),
                              new XAttribute("trgttype", "preview")
                           )
                     }                     
                  }
               )
            );
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void UpldImagFldrProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //bool? folderNameIsProdTarfCode = null;
            //// مرحله اول از کاربر سوال میپرسیم که آیا نحوه آپلود کردن به صورت نام پوشه میباشد یا محتویات فایل پوشه کد کالا را مشخص میکند
            //if(MessageBox.Show(this, "نام پوشه مشخص کننده کد کالای مورد نظر میباشد؟", "نحوه بررسی و اپلود اطلاعات", MessageBoxButtons.YesNo) != DialogResult.Yes)
            //{
            //   // نام پوشه همان کد کالا میباشد
            //   folderNameIsProdTarfCode = true;
            //}
            //else
            //{
            //   // محتوای درون پوشه معرف کد کالا می باشد
            //   folderNameIsProdTarfCode = false;
            //}

            if (UpldImagFldrProd_Fbd.ShowDialog() != DialogResult.OK) return;

            var adminChat =
               iRoboTech.Service_Robots.FirstOrDefault(sr => sr.Service_Robot_Groups.Any(srg => srg.STAT == "002" && (srg.Group.GPID == 131 || srg.Group.GPID == 133 || srg.Group.GPID == 134)));

            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null) return;

            var fileNames = Directory.GetFiles(UpldImagFldrProd_Fbd.SelectedPath, "*", SearchOption.AllDirectories).ToList().Where(f => !RbppBs.List.OfType<Data.Robot_Product_Preview>().Any(p => p.SORC_FILE_PATH != null && p.SORC_FILE_PATH == f));
            if (fileNames.Count() == 0) return;

            SaveProdDataOnSrvr_Pb.Visible = true;

            // Step 1 Send Media to The BALE Server
            // Step 2 Get FileId from Server
            // Step 3 Save FileId For Selected Product
            #region Send Message
            // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Robot", 
                              new XAttribute("runrobot", "start"),
                              new XAttribute("actntype", "upldmediafile"),
                              new XAttribute("chatid", adminChat.CHAT_ID),
                              new XAttribute("command", prod.TARF_CODE),
                              new XAttribute("rbid", adminChat.ROBO_RBID),
                              new XAttribute("mesg", string.Join(";",  fileNames)),
                              new XAttribute("trgttype", "preview")
                           )
                     }                     
                  }
               )
            );
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      // Large Upload file by filename must be Product Tarf Code
      private void UpldImagFileProds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (UpldImgeFileProd_Ofd.ShowDialog(this) != DialogResult.OK) return;

            var adminChat =
               iRoboTech.Service_Robots.FirstOrDefault(sr => sr.Service_Robot_Groups.Any(srg => srg.STAT == "002" && (srg.Group.GPID == 131 || srg.Group.GPID == 133 || srg.Group.GPID == 134)));

            var fileNames = UpldImgeFileProd_Ofd.FileNames.Where(f => RbprBs.List.OfType<Data.Robot_Product>().Any(p => p.TARF_CODE == Path.GetFileNameWithoutExtension(f)) && !iRoboTech.Robot_Product_Previews.Any(p => p.SORC_FILE_PATH != null && p.SORC_FILE_PATH == f && p.TARF_CODE_DNRM == Path.GetFileNameWithoutExtension(f)));
            if (fileNames.Count() == 0) return;

            SaveProdsDataOnSrvr_Pb.Visible = true;

            // Step 1 Send Media to The BALE Server
            // Step 2 Get FileId from Server
            // Step 3 Save FileId For Selected Product
            #region Send Message
            // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Robot", 
                              new XAttribute("runrobot", "start"),
                              new XAttribute("actntype", "upldmediafile"),
                              new XAttribute("chatid", adminChat.CHAT_ID),
                              new XAttribute("command", "*"),
                              new XAttribute("rbid", adminChat.ROBO_RBID),
                              new XAttribute("mesg", string.Join(";",  fileNames)),
                              new XAttribute("trgttype", "preview")
                           )
                     }                     
                  }
               )
            );
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void UpldImagFldrProds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            bool? folderNameIsProdTarfCode = null;
            // مرحله اول از کاربر سوال میپرسیم که آیا نحوه آپلود کردن به صورت نام پوشه میباشد یا محتویات فایل پوشه کد کالا را مشخص میکند
            if (MessageBox.Show(this, "نام پوشه مشخص کننده کد کالای مورد نظر میباشد؟", "نحوه بررسی و اپلود اطلاعات", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
               // نام پوشه همان کد کالا میباشد
               folderNameIsProdTarfCode = true;
            }
            else
            {
               // محتوای درون پوشه معرف کد کالا می باشد
               folderNameIsProdTarfCode = false;
            }

            if (UpldImagFldrProd_Fbd.ShowDialog() != DialogResult.OK) return;

            var adminChat =
               iRoboTech.Service_Robots.FirstOrDefault(sr => sr.Service_Robot_Groups.Any(srg => srg.STAT == "002" && (srg.Group.GPID == 131 || srg.Group.GPID == 133 || srg.Group.GPID == 134)));

            // Initial Form
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape}                     
                  }
               )
            );

            if(folderNameIsProdTarfCode == true)
            {
               // اگر نام پوشه به عنوان کد کالا در نظر گرفته شود
               foreach (
                  var dirTarfCode in 
                  Directory.GetDirectories(UpldImagFldrProd_Fbd.SelectedPath).ToList().
                  Where(d => RbprBs.List.OfType<Data.Robot_Product>().Any(p => p.TARF_CODE == new DirectoryInfo(d).Name))
               )
               {
                  var fileNames = Directory.GetFiles(dirTarfCode, "*", SearchOption.AllDirectories).ToList().Where(f => iRoboTech.Robot_Product_Previews.Where(p => p.SORC_FILE_PATH != null && p.SORC_FILE_PATH == f && p.TARF_CODE_DNRM == new DirectoryInfo(dirTarfCode).Name).Count() == 0);
                  if (fileNames.Count() == 0) continue;

                  SaveProdsDataOnSrvr_Pb.Visible = true;

                  // Step 1 Send Media to The BALE Server
                  // Step 2 Get FileId from Server
                  // Step 3 Save FileId For Selected Product
                  #region Send Message
                  // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           //new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                           //new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Robot", 
                                    new XAttribute("runrobot", "start"),
                                    new XAttribute("actntype", "upldmediafile"),
                                    new XAttribute("chatid", adminChat.CHAT_ID),
                                    new XAttribute("command", new DirectoryInfo(dirTarfCode).Name),
                                    new XAttribute("rbid", adminChat.ROBO_RBID),
                                    new XAttribute("mesg", string.Join(";",  fileNames)),
                                    new XAttribute("trgttype", "preview")
                                 )
                           }                     
                        }
                     )
                  );
                  #endregion
               }
            }
            else
            {
               var fileNames = Directory.GetFiles(UpldImagFldrProd_Fbd.SelectedPath, "*", SearchOption.AllDirectories).ToList().Where(f => RbprBs.List.OfType<Data.Robot_Product>().Any(p => p.TARF_CODE == Path.GetFileNameWithoutExtension(f)) && iRoboTech.Robot_Product_Previews.Where(p => p.SORC_FILE_PATH != null && p.SORC_FILE_PATH == f).Count() == 0);
               if (fileNames.Count() == 0) return;

               SaveProdsDataOnSrvr_Pb.Visible = true;

               // Step 1 Send Media to The BALE Server
               // Step 2 Get FileId from Server
               // Step 3 Save FileId For Selected Product
               #region Send Message
               // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                        {
                           //new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                           //new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Robot", 
                                    new XAttribute("runrobot", "start"),
                                    new XAttribute("actntype", "upldmediafile"),
                                    new XAttribute("chatid", adminChat.CHAT_ID),
                                    new XAttribute("command", "*"),
                                    new XAttribute("rbid", adminChat.ROBO_RBID),
                                    new XAttribute("mesg", string.Join(";",  fileNames)),
                                    new XAttribute("trgttype", "preview")
                                 )
                           }                     
                        }
                  )
               );
               #endregion
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddStepPric_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RpspBs.List.OfType<Data.Robot_Product_StepPrice>().Any(sp => sp.RWNO == 0)) return;

            var rpsp = RpspBs.AddNew() as Data.Robot_Product_StepPrice;
            iRoboTech.Robot_Product_StepPrices.InsertOnSubmit(rpsp);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelStepPric_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rpsp = RpspBs.Current as Data.Robot_Product_StepPrice;
            if (rpsp == null) return;

            iRoboTech.ExecuteCommand("BEGIN DELETE Robot_Product_StepPrice WHERE Rbpr_Code = {0} AND RWNO = {1} END;", rpsp.RBPR_CODE, rpsp.RWNO);
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

      private void SaveStepPric_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Rpsp_Gv.PostEditor();

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

      private void AddRexq_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (RexqBs.List.OfType<Data.Robot_External_Query>().Any(eq => eq.CODE == 0)) return;

            var rexq = RexqBs.AddNew() as Data.Robot_External_Query;
            rexq.Robot = robo;

            iRoboTech.Robot_External_Queries.InsertOnSubmit(rexq);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelRexq_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rexq = RexqBs.Current as Data.Robot_External_Query;
            if (rexq == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iRoboTech.ExecuteCommand("BEGIN DELETE dbo.Robot_External_Query WHERE Code = {0}; END;", rexq.CODE);

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

      private void SaveRexq_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RexqGv.PostEditor();
            RexqBs.EndEdit();

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
      
      private void ExecRexq_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rexq = RexqBs.Current as Data.Robot_External_Query;
            if (rexq == null) return;

            var sqlQuery =
               new SqlDataAdapter(
                  new SqlCommand(
                     RexqSql_Txt.Text,
                     new SqlConnection(rexq.DATA_SORC_TYPE == "001" /* Local Server Connection String */ ? LoclSrvrConStr_Txt.Text : WebSrvrConStr_Txt.Text)
                  )
               );

            DataSet _dsExtrQury = new DataSet();
            sqlQuery.Fill(_dsExtrQury);

            Rexq_Gv.Columns.Clear();
            Rexq_Gc.DataSource = _dsExtrQury.Tables[0];
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void LoclServTest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var loclSrvrSqlCon = new SqlConnection(LoclSrvrConStr_Txt.Text);

            loclSrvrSqlCon.Open();

            loclSrvrSqlCon.Close();

            MessageBox.Show("Test Connection Local Server Successfully");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void WebSrvrTest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var WebSrvrSqlCon = new SqlConnection(WebSrvrConStr_Txt.Text);

            WebSrvrSqlCon.Open();

            WebSrvrSqlCon.Close();

            MessageBox.Show("Test Connection Web Server Successfully");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveRoboData_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RoboBs.EndEdit();

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

      private void NewProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DataSet _dsExtrQury = (Rexq_Gc.DataSource as DataTable).DataSet;
            // First Step Check Duplicate Product and Delete it and another send to save new product
            if (_dsExtrQury.Tables[0].Rows.Count == 0) return;

            // Remove Duplicate Row
            //RbprBs.List.OfType<Data.Robot_Product>().ToList().
            //   ForEach(p => 
            //      {
            //         var rows = _dsExtrQury.Tables[0].Select(string.Format("TARF_CODE = '{0}'" , p.TARF_CODE));
            //         foreach (var row in rows)
            //         {
            //            _dsExtrQury.Tables[0].Rows.Remove(row);
            //         }
            //      });

            //if (_dsExtrQury.Tables[0].Rows.Count == 0) return;

            if (MessageBox.Show(this, "آیا با ذخیره کردن داده های انتخابی بر روی سرور فروشگاه انلاین خود موافق هستید؟", "انتقال اطلاعات بر روی سرور فروشگاه انلاین", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var rexq = RexqBs.Current as Data.Robot_External_Query;
            if(rexq == null)return;

            MessageBox.Show(this, string.Format("تعداد {0} رکورد آماده ذخیره سازی می باشد، فقط ممکن است این گزینه زمان بر باشد", _dsExtrQury.Tables[0].Rows.Count.ToString()));

            var xParam = _dsExtrQury.ToXml();

            iRoboTech.CommandTimeout = int.MaxValue;
            iRoboTech.ExecuteCommand(string.Format("BEGIN EXEC dbo.{0} @X = N'{1}'; END;", rexq.INS_EXEC_WITH, xParam.ToString().Replace("'", "''")));

            MessageBox.Show(this, "اطلاعات با موفقیت درون سیستم فروشگاه انلاین شما قرار گرفت", "انتقال اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

      private void UpdtProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DataSet _dsExtrQury = (Rexq_Gc.DataSource as DataTable).DataSet;
            // First Step Check Duplicate Product and Delete it and another send to save new product
            if (_dsExtrQury.Tables[0].Rows.Count == 0) return;

            // Remove Duplicate Row
            //RbprBs.List.OfType<Data.Robot_Product>().ToList().
            //   ForEach(p =>
            //   {
            //      var rows = _dsExtrQury.Tables[0].Select(string.Format("TARF_CODE = '{0}'", p.TARF_CODE));
            //      bool rowChange = false;
            //      foreach (var row in rows)
            //      {
            //         if(TarfText_Chkb.Checked)
            //            if (p.TARF_TEXT_DNRM != row["TARF_TEXT"].ToString())
            //               rowChange = true;

            //         if (ExpnPric_Chkb.Checked)
            //            if (p.EXPN_PRIC_DNRM != Convert.ToInt64(row["EXPN_PRIC"]))
            //               rowChange = true;

            //         if (p.CRNT_NUMB_DNRM != Convert.ToDouble(row["QNTY"]))
            //            rowChange = true;

            //         if(!rowChange)
            //            _dsExtrQury.Tables[0].Rows.Remove(row);
            //      }
            //   });

            //if (_dsExtrQury.Tables[0].Rows.Count == 0) return;

            if (MessageBox.Show(this, "آیا با ذخیره کردن داده های انتخابی بر روی سرور فروشگاه انلاین خود موافق هستید؟", "انتقال اطلاعات بر روی سرور فروشگاه انلاین", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var rexq = RexqBs.Current as Data.Robot_External_Query;
            if (rexq == null) return;

            MessageBox.Show(this, string.Format("تعداد {0} رکورد آماده ذخیره سازی می باشد، فقط ممکن است این گزینه زمان بر باشد", _dsExtrQury.Tables[0].Rows.Count.ToString()));

            var xParam = _dsExtrQury.ToXml();

            iRoboTech.CommandTimeout = int.MaxValue;
            iRoboTech.ExecuteCommand(string.Format("BEGIN EXEC dbo.{0} @X = N'{1}'; END;", rexq.UPD_EXEC_WITH, xParam.ToString().Replace("'", "''")));

            MessageBox.Show(this, "اطلاعات با موفقیت درون سیستم فروشگاه انلاین شما قرار گرفت", "انتقال اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

      private void AddSlerPrtnr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            // اطلاعات محصول
            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null) return;

            // اطلاعات همکار فروش
            if (Srbt_Lov.EditValue == null) return;
            var srbt = SrbtBs.List.OfType<Data.Service_Robot>().FirstOrDefault(sr => sr.SERV_FILE_NO == (long)Srbt_Lov.EditValue);

            // ایا از قبل قیمت همکار برای کالا ثبت شده یا خیر
            if (SrsprBs.List.OfType<Data.Service_Robot_Seller_Partner>().Any(i => i.Robot_Product == prod && i.Service_Robot == srbt)) return;

            var srspr = SrsprBs.AddNew() as Data.Service_Robot_Seller_Partner;
            srspr.Robot_Product = prod;
            srspr.Service_Robot = srbt;

            iRoboTech.Service_Robot_Seller_Partners.InsertOnSubmit(srspr);
         }
         catch { }
      }

      private void SaveSlerPrtnr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SrsprGv.PostEditor();

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

      private void DelSlerPrtnr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srspr = SrsprBs.Current as Data.Service_Robot_Seller_Partner;
            if (srspr == null) return;

            if (srspr.Order_Details.Any()) return;

            if (MessageBox.Show(this, "آیا با حذف قیمت هنکار موافق هستید؟", "حذف قیمت همکار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iRoboTech.Service_Robot_Seller_Partners.DeleteOnSubmit(srspr);

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

      private void ChngLyot2_Butn_Click(object sender, EventArgs e)
      {
         Lyot2_Scc.Horizontal = !Lyot2_Scc.Horizontal;
      }

      private void UpldFileProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (UpldImgeFileProd_Ofd.ShowDialog(this) != DialogResult.OK) return;

            var adminChat =
               iRoboTech.Service_Robots.FirstOrDefault(sr => sr.Service_Robot_Groups.Any(srg => srg.STAT == "002" && (srg.Group.GPID == 131 || srg.Group.GPID == 133 || srg.Group.GPID == 134)));

            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null) return;

            var fileNames = UpldImgeFileProd_Ofd.FileNames.Where(f => !RbppBs.List.OfType<Data.Robot_Product_Download>().Any(p => p.SORC_FILE_PATH != null && p.SORC_FILE_PATH == f));
            if (fileNames.Count() == 0) return;

            SaveProdFileOnSrvr_Pb.Visible = true;

            // Step 1 Send Media to The BALE Server
            // Step 2 Get FileId from Server
            // Step 3 Save FileId For Selected Product
            #region Send Message
            // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Robot", 
                              new XAttribute("runrobot", "start"),
                              new XAttribute("actntype", "upldmediafile"),
                              new XAttribute("chatid", adminChat.CHAT_ID),
                              new XAttribute("command", prod.TARF_CODE),
                              new XAttribute("rbid", adminChat.ROBO_RBID),
                              new XAttribute("mesg", string.Join(";",  fileNames)),
                              new XAttribute("trgttype", "download")
                           )
                     }                     
                  }
               )
            );
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void UpldFldrProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //bool? folderNameIsProdTarfCode = null;
            //// مرحله اول از کاربر سوال میپرسیم که آیا نحوه آپلود کردن به صورت نام پوشه میباشد یا محتویات فایل پوشه کد کالا را مشخص میکند
            //if(MessageBox.Show(this, "نام پوشه مشخص کننده کد کالای مورد نظر میباشد؟", "نحوه بررسی و اپلود اطلاعات", MessageBoxButtons.YesNo) != DialogResult.Yes)
            //{
            //   // نام پوشه همان کد کالا میباشد
            //   folderNameIsProdTarfCode = true;
            //}
            //else
            //{
            //   // محتوای درون پوشه معرف کد کالا می باشد
            //   folderNameIsProdTarfCode = false;
            //}

            if (UpldImagFldrProd_Fbd.ShowDialog() != DialogResult.OK) return;

            var adminChat =
               iRoboTech.Service_Robots.FirstOrDefault(sr => sr.Service_Robot_Groups.Any(srg => srg.STAT == "002" && (srg.Group.GPID == 131 || srg.Group.GPID == 133 || srg.Group.GPID == 134)));

            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null) return;

            var fileNames = Directory.GetFiles(UpldImagFldrProd_Fbd.SelectedPath, "*", SearchOption.AllDirectories).ToList().Where(f => !RbppBs.List.OfType<Data.Robot_Product_Download>().Any(p => p.SORC_FILE_PATH != null && p.SORC_FILE_PATH == f));
            if (fileNames.Count() == 0) return;

            SaveProdFileOnSrvr_Pb.Visible = true;

            // Step 1 Send Media to The BALE Server
            // Step 2 Get FileId from Server
            // Step 3 Save FileId For Selected Product
            #region Send Message
            // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Robot", 
                              new XAttribute("runrobot", "start"),
                              new XAttribute("actntype", "upldmediafile"),
                              new XAttribute("chatid", adminChat.CHAT_ID),
                              new XAttribute("command", prod.TARF_CODE),
                              new XAttribute("rbid", adminChat.ROBO_RBID),
                              new XAttribute("mesg", string.Join(";",  fileNames)),
                              new XAttribute("trgttype", "download")
                           )
                     }                     
                  }
               )
            );
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DellRpdl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rpdl = RpdlBs.Current as Data.Robot_Product_Download;
            if (rpdl == null) return;

            if (MessageBox.Show(this, "آیا با حذففایل دانلودی محصول موافق هستید؟", "حذف فایل دانلودی", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Robot_Product_Downloads.DeleteOnSubmit(rpdl);

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

      private void SaveRpdl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RpdlBs.EndEdit();
            Rpdl_Gv.PostEditor();

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

      private void Galery_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null || prod.CODE == 0) return;

            Product_Tc.SelectedTab = MoreInfoProduct_Tp;
            MoreInfo_Tc.SelectedTab = Galery_Tp;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Store_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null || prod.CODE == 0) return;

            Product_Tc.SelectedTab = MoreInfoProduct_Tp;
            MoreInfo_Tc.SelectedTab = Store_Tp;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Unit_Butn_Click(object sender, EventArgs e)
      {
         Master_Tc.SelectedTab = ServDef_Tp;
         ServDef_Tc.SelectedTab = Unit_Tp;
      }

      private void Brand_Butn_Click(object sender, EventArgs e)
      {
         Master_Tc.SelectedTab = ServDef_Tp;
         ServDef_Tc.SelectedTab = Brand_Tp;
      }

      private void Group_Butn_Click(object sender, EventArgs e)
      {
         Master_Tc.SelectedTab = ServDef_Tp;
         ServDef_Tc.SelectedTab = Group_Tp;
      }

      private void InfoProd_Tsb_Click(object sender, EventArgs e)
      {
         Master_Tc.SelectedTab = ProductDef_Tp;
         Product_Tc.SelectedTab = ProductInfo_Tp;
      }

      private void AddRlcg_Tsb_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         if (RlcgBs.List.OfType<Data.Robot_Limited_Commodity_Group>().Any(i => i.CODE == 0)) return;

         var rlcg = RlcgBs.AddNew() as Data.Robot_Limited_Commodity_Group;
         rlcg.Robot = robo;
         iRoboTech.Robot_Limited_Commodity_Groups.InsertOnSubmit(rlcg);
      }

      private void DelRlcg_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            var rlcg = RlcgBs.Current as Data.Robot_Limited_Commodity_Group;
            if (rlcg == null) return;

            if (MessageBox.Show(this, "آیا با حذف موافق هستید؟", "حذف", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Robot_Limited_Commodity_Groups.DeleteOnSubmit(rlcg);
            iRoboTech.SubmitChanges();

            requery = true;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveRlcg_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            RlcgGv.PostEditor();

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

      private void AddRplm_Tsb_Click(object sender, EventArgs e)
      {
         var rlcg = RlcgBs.Current as Data.Robot_Limited_Commodity_Group;
         if (rlcg == null) return;

         var prod = RbprBs.Current as Data.Robot_Product;
         if(prod == null)return;

         if (RplmBs.List.OfType<Data.Robot_Product_Limited>().Any(i => i.CODE == 0 || i.Robot_Product == prod)) return;

         var rplm = RplmBs.AddNew() as Data.Robot_Product_Limited;
         rplm.Robot_Limited_Commodity_Group = rlcg;
         rplm.Robot_Product = prod;

         iRoboTech.Robot_Product_Limiteds.InsertOnSubmit(rplm);
      }

      private void DelRplm_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            var rplm = RplmBs.Current as Data.Robot_Product_Limited;
            if (rplm == null) return;

            if (MessageBox.Show(this, "آیا با حذف موافق هستید؟", "حذف", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Robot_Product_Limiteds.DeleteOnSubmit(rplm);
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

      private void SaveRplm_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            RplmGv.PostEditor();

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

      private void AddSral_Tsb_Click(object sender, EventArgs e)
      {
         var rlcg = RlcgBs.Current as Data.Robot_Limited_Commodity_Group;
         if (rlcg == null) return;

         var srbt = SrbtBs.Current as Data.Service_Robot;
         if (srbt == null) return;

         if (SralBs.List.OfType<Data.Service_Robot_Access_Limited_Group_Product>().Any(i => i.CODE == 0 || i.Service_Robot == srbt)) return;

         var sral = SralBs.AddNew() as Data.Service_Robot_Access_Limited_Group_Product;
         sral.Robot_Limited_Commodity_Group = rlcg;
         sral.Service_Robot = srbt;

         iRoboTech.Service_Robot_Access_Limited_Group_Products.InsertOnSubmit(sral);
      }

      private void DelSral_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            var sral =  SralBs.Current as Data.Service_Robot_Access_Limited_Group_Product;
            if (sral == null) return;

            if (MessageBox.Show(this, "آیا با حذف موافق هستید؟", "حذف", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Service_Robot_Access_Limited_Group_Products.DeleteOnSubmit(sral);
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

      private void SaveSral_Tsb_Click(object sender, EventArgs e)
      {
         try
         {
            SralGv.PostEditor();

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

      private void BuyPric_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         var prod = RbprBs.Current as Data.Robot_Product;
         if (prod == null) return;

         if(SaleBuyAmntLock_Pkb.PickChecked)
         {
            prod.EXPN_PRIC_DNRM = Convert.ToInt64(e.NewValue) + (Convert.ToInt64(e.NewValue) * PrctAmnt_Txt.Text.ToInt64() / 100);
         }
      }

      private void GetMaxTarfCode_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            if (rbpr.TARF_CODE != null && rbpr.TARF_CODE.Length > 0 && MessageBox.Show(this, "آیا مایل به تغییر کد تعرفه محصول هستین؟", "تغییر کد تعرفه محصول", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            if (!RbprBs.List.OfType<Data.Robot_Product>().Any(p => p.CODE != 0))
            {
               rbpr.TARF_CODE = "1";
            }
            else
            {
               //rbpr.TARF_CODE =
               //   (iRoboTech.Robot_Products
               //       .Where(p => p.TARF_CODE != null && p.TARF_CODE.Length > 0)
               //       .Select(p => p.TARF_CODE)
               //       .ToList()
               //       .Where(p => p.All(char.IsDigit))
               //       .Max(p => Convert.ToInt64(p)) + 1).ToString();
               rbpr.TARF_CODE =
                  (iRoboTech.Robot_Products
                      .Where(p => p.TARF_CODE != null && p.TARF_CODE.Length > 0)
                      //.Select(p => p.TARF_CODE)
                      .Count() + 1).ToString();
                      //.Where(p => p.All(char.IsDigit))
                      //.Max(p => Convert.ToInt64(p)) + 1).ToString();
            }
         }
         catch { }
      }

      private void GetMaxRwnoSubGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var grop = VGexpBs.Current as Data.V_Group_Expense;
            if (grop == null) return;

            if (grop.CODE != 0) return;

            // اگر سرگروه باشد
            if (CretNewSuprGrop_Cbx.Checked)
               LinkJoin_Txt.Text = 
                  (iRoboTech.V_Group_Expenses
                  .Where(g => g.GEXP_CODE == null)
                  .Max(g => g.ORDR) + 1).ToString();
            else
               LinkJoin_Txt.Text = 
                  (iRoboTech.V_Group_Expenses
                  .Where(g => g.GEXP_CODE == grop.CODE)
                  .Max(g => g.ORDR) + 1).ToString();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SnglDuplTarf_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var starf = RbprBs.Current as Data.Robot_Product;
            if (starf == null) return;

            iRoboTech.DUP_RBPR_P(
               new XElement("Duplicate",
                   new XAttribute("idty", DupTarfIdty_Cbtn.Checked),
                   new XAttribute("gift", DupTarfGift_Cbtn.Checked),
                   new XAttribute("stor", DupTarfStor_Cbtn.Checked),
                   new XAttribute("rlat", DupTarfRlat_Cbtn.Checked),
                   new XAttribute("sprc", DupTarfSprc_Cbtn.Checked),
                   new XAttribute("altr", DupTarfAltr_Cbtn.Checked),
                   new XAttribute("dsct", DupTarfDsct_Cbtn.Checked),
                   new XAttribute("sprt", DupTarfSprt_Cbtn.Checked),
                   new XAttribute("psam", DupTarfPSam_Cbtn.Checked),
                   new XAttribute("type", "single"),
                   new XElement("Robot_Product",
                       new XElement("Source",
                           new XAttribute("tarfcode", starf.TARF_CODE)
                       )                       
                   )
               )
            );

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

      private void MoreDuplTarf_Butn_Click(object sender, EventArgs e)
      {
         IEnumerable<string> _fileNames = null;
         try
         {
            var starf = RbprBs.Current as Data.Robot_Product;
            if (starf == null) return;

            if(UpldImagFldrProd_Fbd.ShowDialog() != DialogResult.OK)return;
            var fileNames = Directory.GetFiles(UpldImagFldrProd_Fbd.SelectedPath, "*", SearchOption.AllDirectories).ToList().Where(f => !RbprBs.List.OfType<Data.Robot_Product>().Any(p => p.TARF_CODE == Path.GetFileNameWithoutExtension(f)));
            if (fileNames.Count() == 0) return;

            _fileNames = fileNames;

            //SaveProdsDataOnSrvr_Pb.Visible = true;

            iRoboTech.DUP_RBPR_P(
               new XElement("Duplicate",
                   new XAttribute("idty", DupTarfIdty_Cbtn.Checked),
                   new XAttribute("gift", DupTarfGift_Cbtn.Checked),
                   new XAttribute("stor", DupTarfStor_Cbtn.Checked),
                   new XAttribute("rlat", DupTarfRlat_Cbtn.Checked),
                   new XAttribute("sprc", DupTarfSprc_Cbtn.Checked),
                   new XAttribute("altr", DupTarfAltr_Cbtn.Checked),
                   new XAttribute("dsct", DupTarfDsct_Cbtn.Checked),
                   new XAttribute("sprt", DupTarfSprt_Cbtn.Checked),
                   new XAttribute("psam", DupTarfPSam_Cbtn.Checked),
                   new XAttribute("type", "array"),
                   new XElement("Robot_Product",
                       new XElement("Source",
                           new XAttribute("tarfcode", starf.TARF_CODE)
                       ),
                       new XElement("List",
                          fileNames.Select(f =>
                             new XElement("Product",
                                new XAttribute("tarfcode", Path.GetFileNameWithoutExtension(f))                                
                             )
                          )
                       )                       
                   )
               )
            );

            var adminChat =
               iRoboTech.Service_Robots.FirstOrDefault(sr => sr.Service_Robot_Groups.Any(srg => srg.STAT == "002" && (srg.Group.GPID == 131 || srg.Group.GPID == 133 || srg.Group.GPID == 134)));

            // Initial Form
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                     {
                        new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape}                     
                     }
               )
            );
            // Step 1 Send Media to The BALE Server
            // Step 2 Get FileId from Server
            // Step 3 Save FileId For Selected Product
            #region Send Message
            // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                        {
                           //new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                           //new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Robot", 
                                    new XAttribute("runrobot", "start"),
                                    new XAttribute("actntype", "upldmediafile"),
                                    new XAttribute("chatid", adminChat.CHAT_ID),
                                    new XAttribute("command", "*"),
                                    new XAttribute("rbid", adminChat.ROBO_RBID),
                                    new XAttribute("mesg", string.Join(";", _fileNames)),
                                    new XAttribute("trgttype", "preview")
                                 )
                           }                     
                        }
               )
            );
            #endregion

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

      private void DsctActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            var discountType = "discount";
            switch (e.Button.Index)
            {
               case 0:
                  discountType = "discountall";
                  break;
            }

            var xRet = new XElement("Respons");
            iRoboTech.SEND_MEOJ_P(
               new XElement("Robot", 
                   new XAttribute("rbid", robo.RBID),
                   new XElement("Order",
                       new XAttribute("type", "012"),
                       new XAttribute("valu", rbpr.TARF_CODE),
                       new XAttribute("oprt", discountType)
                   )
               ),
               ref xRet
            );

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
               #region Send Message
               // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                        {
                           new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                           new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Robot", 
                                    new XAttribute("runrobot", "start"),
                                    new XAttribute("actntype", "sendordrs")
                                 )
                           }                     
                        }
                  )
               );
               #endregion
            }
         }
      }

      private void StorActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            var AlertType = "addprodtostor";
            switch (e.Button.Index)
            {
               case 0:
                  AlertType = "addprodtostorall";
                  break;
            }

            var xRet = new XElement("Respons");
            iRoboTech.SEND_MEOJ_P(
               new XElement("Robot",
                   new XAttribute("rbid", robo.RBID),
                   new XElement("Order",
                       new XAttribute("type", "012"),
                       new XAttribute("valu", rbpr.TARF_CODE),
                       new XAttribute("oprt", AlertType)
                   )
               ),
               ref xRet
            );

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
               #region Send Message
               // فراخوانی ربات برای ارسال مدیا برای ثبت و گرفتن آدرس لینک فایل سرور
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                        {
                           new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                           new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Robot", 
                                    new XAttribute("runrobot", "start"),
                                    new XAttribute("actntype", "sendordrs")
                                 )
                           }                     
                        }
                  )
               );
               #endregion
            }
         }
      }

      private void SlerPartActn_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            throw new Exception("پیاده سازی این قسمت هنوز انجام نشده");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Serp_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            var rose = RoseBs.Current as Data.Robot_Search_Engine;
            if (rose == null) return;

            if(FaSe_Rb.Checked)
               Process.Start(string.Format(rose.WEB_SITE, rbpr.TARF_TEXT_DNRM));
            else
               Process.Start(string.Format(rose.WEB_SITE, rbpr.TARF_ENGL_TEXT));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddSe_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (RoseBs.List.OfType<Data.Robot_Search_Engine>().Any(se => se.SGID == 0)) return;

            var rose = RoseBs.AddNew() as Data.Robot_Search_Engine;
            rose.Robot = robo;

            iRoboTech.Robot_Search_Engines.InsertOnSubmit(rose);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelSe_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rose = RoseBs.Current as Data.Robot_Search_Engine;
            if (rose == null) return;

            if (MessageBox.Show(this, "آیا با حذف وب سایت جستجو موافق هستین؟", "حذف وب سایت", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Robot_Search_Engines.DeleteOnSubmit(rose);
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

      private void SaveSe_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RoseGv.PostEditor();
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

      private void GetNowCrnc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var csor = RbcsBs.Current as Data.Robot_Currency_Source;
            if (csor == null) return;

            string urlAddress = csor.WEB_SITE;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
               Stream receiveStream = response.GetResponseStream();
               StreamReader readStream = null;

               if (String.IsNullOrWhiteSpace(response.CharacterSet))
                  readStream = new StreamReader(receiveStream);
               else
                  readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

               var htmlDoc = new HtmlAgilityPack.HtmlDocument();
               htmlDoc.LoadHtml(readStream.ReadToEnd());

               var htmlBody = htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr");

               iRoboTech.GET_CSOR_P(
                  new XElement("Robot_Currency_Source",
                      new XAttribute("code", csor.CODE),
                      new XElement("Currencies",
                          htmlBody.Take(32)
                          .Select(c => 
                              new XElement("Currency", 
                                  new XAttribute("data", c.InnerText.Replace("\n", "#"))
                              )
                          )
                          
                      )
                  )
               );

               requery = true;
               response.Close();
               readStream.Close();
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

      private void SaveRobo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RoboBs.EndEdit();
            RoboGv.PostEditor();
            RbcsGv.PostEditor();

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

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "FRST_PAGE_F", 07 /* Execute LoadData */, SendType.SelfToUserInterface)
               );
            }
         }
      }

      private void CalcCrncExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbpr = RbprBs.Current as Data.Robot_Product;
            if (rbpr == null) return;

            if(rbpr.CRNC_CALC_STAT == "002")
            {
               rbpr.EXPN_PRIC_DNRM = (long)(rbpr.CRNC_EXPN_AMNT * (decimal?)rbpr.Robot.CRNC_AMNT_DNRM);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void VGexpBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var gexp = VGexpBs.Current as Data.V_Group_Expense;
            if (gexp == null) return;

            var user = vUsrBs.Current as Data.V_User;
            if (user == null) return;

            UagpBs.DataSource = iRoboTech.User_Access_Group_Products.Where(ga => ga.USER_ID == user.USER_ID && ga.GROP_CODE == gexp.CODE);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GrntGropProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var gexp = VGexpBs.Current as Data.V_Group_Expense;
            if (gexp == null) return;

            var user = vUsrBs.Current as Data.V_User;
            if (user == null) return;

            var uagp = UagpBs.Current as Data.User_Access_Group_Product;

            if (uagp == null)
            {
               iRoboTech.ExecuteCommand(string.Format("INSERT INTO dbo.User_Access_Group_Product (User_Id, Grop_Code, Aces_Stat, Code) VALUES ({0}, {1}, '002', 0);", user.USER_ID, gexp.CODE));
            }
            else
            {
               uagp.ACES_STAT = "002";
            }

            if (ModifierKeys == Keys.Control)
            {

               foreach (var gp in VGexpBs.List.OfType<Data.V_Group_Expense>().Where(g => g.GEXP_CODE == gexp.CODE))
               {
                  iRoboTech.ExecuteCommand(
                     string.Format(
                        "MERGE dbo.User_Access_Group_Product T " + "\n" + 
                        "USING (SELECT {0} AS User_Id, {1} AS Grop_Code) S" + "\n" +
                        "ON (T.User_Id = S.User_Id AND T.Grop_Code = S.Grop_Code)" + "\n" + 
                        "WHEN NOT MATCHED THEN INSERT (User_Id, Grop_Code, Code) VALUES({0}, {1}, 0)" + "\n" + 
                        "WHEN MATCHED THEN UPDATE SET Aces_Stat = '002';", 
                        user.USER_ID, gp.CODE
                     )
                  );
               }
            }

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

      private void RvokGropProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var uagp = UagpBs.Current as Data.User_Access_Group_Product;
            if (uagp == null) return;

            uagp.ACES_STAT = "001";

            if (ModifierKeys == Keys.Control)
            {

               foreach (var gp in VGexpBs.List.OfType<Data.V_Group_Expense>().Where(g => g.GEXP_CODE == uagp.GROP_CODE))
               {
                  iRoboTech.ExecuteCommand(
                     string.Format(
                        "MERGE dbo.User_Access_Group_Product T " + "\n" +
                        "USING (SELECT {0} AS User_Id, {1} AS Grop_Code) S" + "\n" +
                        "ON (T.User_Id = S.User_Id AND T.Grop_Code = S.Grop_Code)" + "\n" +                        
                        "WHEN MATCHED THEN UPDATE SET Aces_Stat = '001';",
                        uagp.USER_ID, gp.CODE
                     )
                  );
               }
            }

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

      private void vUsrBs_CurrentChanged(object sender, EventArgs e)
      {
         VGexpBs_CurrentChanged(null, null);
      }

      private void DefPtyp_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 32 /* Execute Tree_Base_F */),
                new Job(SendType.SelfToUserInterface, "TREE_BASE_F", 10 /* Execute Actn_CalF_P */){
                   Input = 
                     new XElement("Params",
                         new XAttribute("formcaller", GetType().Name),
                         new XAttribute("tablename", "PartnerType_Info"),
                         new XAttribute("gototab", "tp_006"),
                         new XAttribute("action", "newuser")
                     )
                }
              })
         );
      }

      private void SbmtChngSlerPrtnr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _robo = RoboBs.Current as Data.Robot;
            if(_robo == null) return;

            var _rbpr = RbprBs.Current as Data.Robot_Product;
            if (_rbpr == null) return;

            foreach (var i in Srsp_Gv.GetSelectedRows())
            {
               var _item = Srsp_Gv.GetRow(i) as Data.App_Base_Define;

               IEnumerable<Data.Service_Robot_Seller_Partner> _selrPrtnr = null;

               // گام اول مشخص کردن دامنه تغییرات کالا
               if (CrntRbpr_Rb.Checked)
               {
                  _selrPrtnr = SrsprBs.List.OfType<Data.Service_Robot_Seller_Partner>().Where(a => a.TYPE_APBS_CODE == _item.CODE && (SelrPrtnrAll_Rb.Checked || a.STAT == "002"));
               }
               else
               {
                  _selrPrtnr = iRoboTech.Service_Robot_Seller_Partners.Where(a => a.TYPE_APBS_CODE == _item.CODE && (SelrPrtnrAll_Rb.Checked || a.STAT == "002"));
               }

               if (_selrPrtnr.Count() == 0) return;

               // گام دوم مشخص کردن نحوه تغییرات قیمتی
               if(UpChng_Rb.Checked)
               {
                  // گام سوم مشخص کردن منبغ مالی از قیمت خرید یا قیمت فروش
                  if (BuyAmnt_Rb.Checked)
                  {
                     // گام بعدی نحوع افزایش مبلغ
                     if (PrctChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp => 
                              sp.BUY_PRIC = 
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) +
                                 ((RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) * PrctChng_Txt.Text.ToInt64()) / 100
                           );
                     }
                     else if (AmntChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.BUY_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) +
                                 AmntChng_Txt.EditValue.ToString().ToInt64()
                           );
                     }
                  }
                  else if (SellAmnt_Rb.Checked)
                  {
                     // گام بعدی نحوع افزایش مبلغ
                     if (PrctChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.EXPN_PRIC_DNRM : sp.EXPN_PRIC) +
                                 ((RbprRecd_Rb.Checked ? sp.Robot_Product.EXPN_PRIC_DNRM : sp.EXPN_PRIC) * PrctChng_Txt.Text.ToInt64()) / 100
                           );
                     }
                     else if (AmntChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.EXPN_PRIC_DNRM : sp.EXPN_PRIC) +
                                 AmntChng_Txt.EditValue.ToString().ToInt64()
                           );
                     }
                  }
                  else if(Buy2SellAmnt_Rb.Checked)
                  {
                     // گام بعدی نحوع افزایش مبلغ
                     if (PrctChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) +
                                 ((RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) * PrctChng_Txt.Text.ToInt64()) / 100
                           );
                     }
                     else if (AmntChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) +
                                 AmntChng_Txt.EditValue.ToString().ToInt64()
                           );
                     }
                  }
               }
               else if(DownChng_Rb.Checked)
               {
                  // گام سوم مشخص کردن منبغ مالی از قیمت خرید یا قیمت فروش
                  if (BuyAmnt_Rb.Checked)
                  {
                     // گام بعدی نحوع افزایش مبلغ
                     if (PrctChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.BUY_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) -
                                 ((RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) * PrctChng_Txt.Text.ToInt64()) / 100
                           );
                     }
                     else if (AmntChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.BUY_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) -
                                 AmntChng_Txt.EditValue.ToString().ToInt64()
                           );
                     }
                  }
                  else if (SellAmnt_Rb.Checked)
                  {
                     // گام بعدی نحوع افزایش مبلغ
                     if (PrctChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.EXPN_PRIC_DNRM : sp.EXPN_PRIC) -
                                 ((RbprRecd_Rb.Checked ? sp.Robot_Product.EXPN_PRIC_DNRM : sp.EXPN_PRIC) * PrctChng_Txt.Text.ToInt64()) / 100
                           );
                     }
                     else if (AmntChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.EXPN_PRIC_DNRM : sp.EXPN_PRIC) -
                                 AmntChng_Txt.EditValue.ToString().ToInt64()
                           );
                     }
                  }
                  else if (Buy2SellAmnt_Rb.Checked)
                  {
                     // گام بعدی نحوع افزایش مبلغ
                     if (PrctChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) -
                                 ((RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) * PrctChng_Txt.Text.ToInt64()) / 100
                           );
                     }
                     else if (AmntChng_Rb.Checked)
                     {
                        _selrPrtnr.ToList().
                           ForEach(sp =>
                              sp.EXPN_PRIC =
                                 (RbprRecd_Rb.Checked ? sp.Robot_Product.BUY_PRIC : sp.BUY_PRIC) -
                                 AmntChng_Txt.EditValue.ToString().ToInt64()
                           );
                     }
                  }
               }
            }

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

      private void DupSelrProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _selrProd = SrsprBs.Current as Data.Service_Robot_Seller_Partner;
            if (_selrProd == null) return;

            iRoboTech.ExecuteCommand(
               string.Format(
                  "MERGE Service_Robot_Seller_Partner T" + Environment.NewLine + 
                  "USING (SELECT rp.TARF_CODE, rp.CODE, {1} AS CHAT_ID FROM Robot_Product rp WHERE rp.Robo_Rbid = {3}) S" + Environment.NewLine + 
                  "ON (T.CHAT_ID = S.CHAT_ID AND T.TARF_CODE_DNRM = S.TARF_CODE)" + Environment.NewLine + 
                  "WHEN NOT MATCHED THEN" + Environment.NewLine +
                  "INSERT (SRBT_SERV_FILE_NO, SRBT_ROBO_RBID, RBPR_CODE,CODE, STAT, TYPE_APBS_CODE) VALUES({2}, {3}, S.CODE, dbo.GNRT_NVID_U(), '002', {4});",
                  _selrProd.TARF_CODE_DNRM,
                  _selrProd.CHAT_ID,
                  _selrProd.SRBT_SERV_FILE_NO,
                  _selrProd.SRBT_ROBO_RBID,
                  _selrProd.TYPE_APBS_CODE
               )
            );
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

      private void GropActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _grop = VGexpBs.Current as Data.V_Group_Expense;
            if (_grop == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  // Save
                  SaveGrop_Butn_Click(null, null);
                  break;
               case 1:
                  // Del
                  DelGrop_Butn_Click(null, null);
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

      private void DupProdColm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _prod = RbprBs.Current as Data.Robot_Product;
            if (_prod == null) return;

            var _sender = sender as C1.Win.C1Input.C1Button;
            if (_sender == null) return;

            if (MessageBox.Show(this, "آیا با تغییرات به صورت نمونه برداری برای تمام محصولات موافق هستید؟", "نمونه برداری کلی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            switch (_sender.Tag.ToString())
            {
               case "SHOW_PRIC_TYPE":
                  RbprBs.List.OfType<Data.Robot_Product>().ToList().ForEach(p => p.SHOW_PRIC_TYPE = _prod.SHOW_PRIC_TYPE);
                  break;
               case "NUMB_TYPE":
                  RbprBs.List.OfType<Data.Robot_Product>().ToList().ForEach(p => p.NUMB_TYPE = _prod.NUMB_TYPE);
                  break;
               case "PROD_LIFE_STAT":
                  RbprBs.List.OfType<Data.Robot_Product>().ToList().ForEach(p => p.PROD_LIFE_STAT = _prod.PROD_LIFE_STAT);
                  break;
               case "CRNC_CALC_STAT":
                  RbprBs.List.OfType<Data.Robot_Product>().ToList().ForEach(p => p.CRNC_CALC_STAT = _prod.CRNC_CALC_STAT);
                  break;
               case "PROD_TYPE_DNRM":
                  RbprBs.List.OfType<Data.Robot_Product>().ToList().ForEach(p => p.PROD_TYPE_DNRM = _prod.PROD_TYPE_DNRM);
                  break;
               case "STAT":
                  RbprBs.List.OfType<Data.Robot_Product>().ToList().ForEach(p => p.STAT = _prod.STAT);
                  break;
               case "UNIT_APBS_CODE":
                  RbprBs.List.OfType<Data.Robot_Product>().ToList().ForEach(p => p.UNIT_APBS_CODE = _prod.UNIT_APBS_CODE);
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }      
   }
}
