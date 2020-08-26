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
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.RoboTech.ExtCode;
using System.IO;
using System.Data.SqlClient;

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

         SaveProdDataOnSrvr_Pb.Visible = SaveProdsDataOnSrvr_Pb.Visible = false;
         requery = false;
      }

      #region Group Expense
      private void AddGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Ordr_Txt.Text == "") { Ordr_Txt.Focus(); return; }

            if (GropDesc_Txt.Text == "") { GropDesc_Txt.Focus(); return; }

            var grop = VGexpBs.Current as Data.V_Group_Expense;            

            if(grop == null)
               iRoboTech.DBL_INS_GEXP_P(null, "001", Ordr_Txt.Text.ToInt16(), GropDesc_Txt.Text, "002");
            else
               iRoboTech.DBL_INS_GEXP_P(grop.CODE, "001", Ordr_Txt.Text.ToInt16(), GropDesc_Txt.Text, "002");

            Ordr_Txt.Text = GropDesc_Txt.Text = "";
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

            iRoboTech.DBL_UPD_GEXP_P(grop.CODE, grop.GEXP_CODE, grop.GROP_TYPE, grop.ORDR, grop.GROP_DESC, grop.STAT);
            
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

            iRoboTech.DBL_INS_GEXP_P(null, "002", 0, BrndDesc_Txt.Text, "002");

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

            iRoboTech.DBL_UPD_GEXP_P(brnd.CODE, brnd.GEXP_CODE, brnd.GROP_TYPE, brnd.ORDR, brnd.GROP_DESC, brnd.STAT);

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

            iRoboTech.INS_APBS_P(UnitDesc_Txt.Text, "PRODUCTUNIT_INFO", null, null);

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

            iRoboTech.Robot_Products.InsertOnSubmit(rbpr);
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
                              new XAttribute("mesg", string.Join(";",  fileNames))
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
                              new XAttribute("mesg", string.Join(";",  fileNames))
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
                              new XAttribute("mesg", string.Join(";",  fileNames))
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
                                    new XAttribute("mesg", string.Join(";",  fileNames))
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
                                    new XAttribute("mesg", string.Join(";",  fileNames))
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
   }
}
