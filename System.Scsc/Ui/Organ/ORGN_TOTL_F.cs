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

namespace System.Scsc.Ui.Organ
{
   public partial class ORGN_TOTL_F : UserControl
   {
      public ORGN_TOTL_F()
      {
         InitializeComponent();
      }
      bool requery = false;

      private void Execute_Query(bool runAllQuery)
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         int orgn = OrgnBs1.Position;
         int bcds = BcdsBs1.Position;
         OrgnBs1.DataSource = iScsc.Organs;
         OrgnBs1.Position = orgn;
         BcdsBs1.Position = bcds;

         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void OrgnAdd_Butn_Click(object sender, EventArgs e)
      {
         OrgnBs1.AddNew();
      }

      private void OrgnDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crntobj = OrgnBs1.Current as Data.Organ;
            if (crntobj != null && MessageBox.Show(this, "آیا با حذف آیتم انتخاب شده موافقید؟", "عملیات حذف", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            iScsc.Organs.DeleteOnSubmit(crntobj);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void OrgnUpd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Orgn_Gv.PostEditor();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void DeptAdd_Butn_Click(object sender, EventArgs e)
      {
         DeptBs1.AddNew();
      }

      private void DeptDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crntobj = DeptBs1.Current as Data.Department;
            if (crntobj != null && MessageBox.Show(this, "آیا با حذف آیتم انتخاب شده موافقید؟", "عملیات حذف", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            iScsc.Departments.DeleteOnSubmit(crntobj);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void DeptUpd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Dept_Gv.PostEditor();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void BuntAdd_Butn_Click(object sender, EventArgs e)
      {
         BuntBs1.AddNew();
      }

      private void BuntDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crntobj = BuntBs1.Current as Data.Base_Unit;
            if (crntobj != null && MessageBox.Show(this, "آیا با حذف آیتم انتخاب شده موافقید؟", "عملیات حذف", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            iScsc.Base_Units.DeleteOnSubmit(crntobj);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void BuntUpd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Bunt_Gv.PostEditor();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void SuntAdd_Butn_Click(object sender, EventArgs e)
      {
         var _bunt = BuntBs1.Current as Data.Base_Unit;
         if (_bunt == null) return;

         if (SuntBs1.List.OfType<Data.Sub_Unit>().Any(s => s.CODE == null)) return;

         SuntBs1.AddNew();
      }

      private void SuntDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crntobj = SuntBs1.Current as Data.Sub_Unit;
            if (crntobj != null && MessageBox.Show(this, "آیا با حذف آیتم انتخاب شده موافقید؟", "عملیات حذف", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            iScsc.Sub_Units.DeleteOnSubmit(crntobj);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void SuntUpd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Sunt_Gv.PostEditor();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void BcdsAdd_Butn_Click(object sender, EventArgs e)
      {
         var sunt = SuntBs1.Current as Data.Sub_Unit;
         if (sunt == null) return;

         if (BcdsBs1.List.OfType<Data.Basic_Calculate_Discount>().Any(b => b.RWNO == 0)) return;
         BcdsBs1.AddNew();
         var bcds = BcdsBs1.Current as Data.Basic_Calculate_Discount;
         bcds.Sub_Unit = sunt;
         iScsc.Basic_Calculate_Discounts.InsertOnSubmit(bcds);
      }

      private void BcdsDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crntobj = BcdsBs1.Current as Data.Basic_Calculate_Discount;
            if (crntobj != null && MessageBox.Show(this, "آیا با حذف آیتم انتخاب شده موافقید؟", "عملیات حذف", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            iScsc.Basic_Calculate_Discounts.DeleteOnSubmit(crntobj);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void BcdsUpd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            BcdsBs1.EndEdit();
            Bcds_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query(false);
            }
         }
      }

      private void BcdsIncPn_Butn_Click(object sender, EventArgs e)
      {
         Bcds_SpltCont.SplitterDistance -= 100;
      }

      private void BcdsDecPn_Butn_Click(object sender, EventArgs e)
      {
         Bcds_SpltCont.SplitterDistance += 100;
      }

      private void SaveCtgyBcds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var bcds = BcdsBs1.Current as Data.Basic_Calculate_Discount;
            if (bcds == null) return;

            var sunt = SuntBs1.Current as Data.Sub_Unit;

            iScsc.Category_Belts.Where(c => c.CTGY_STAT == "002").ToList()
               .ForEach(c =>
               {
                  if (!BcdsBs1.List.OfType<Data.Basic_Calculate_Discount>().Any(b => b.Sub_Unit == bcds.Sub_Unit && b.RQTP_CODE == bcds.RQTP_CODE && b.RWNO != 0 && b.CTGY_CODE == c.CODE))
                     iScsc.INS_BCDS_P(sunt.BUNT_DEPT_ORGN_CODE, sunt.BUNT_DEPT_CODE, sunt.BUNT_CODE, sunt.CODE, null, null, bcds.EPIT_CODE, bcds.RQTP_CODE, bcds.RQTT_CODE, bcds.AMNT_DSCT, bcds.PRCT_DSCT, bcds.DSCT_TYPE, bcds.ACTN_TYPE, bcds.DSCT_DESC, bcds.FROM_DATE, bcds.TO_DATE, c.CODE, bcds.EXPN_CODE);
               });
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query(true);
         }
      }
   }
}
