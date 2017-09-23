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

namespace System.CRM.Ui.BaseDefination
{
   public partial class ORGN_DFIN_F : UserControl
   {
      public ORGN_DFIN_F()
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
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if(tb_master.SelectedTab == tp_001)
         {
            int c = OrgnBs.Position;
            OrgnBs.DataSource = iCRM.Organs;
            RqrqBs.DataSource = iCRM.Request_Requesters.Where(rq => rq.Regulation.REGL_STAT == "002" && rq.Regulation.TYPE == "001");
            OrgnBs.Position = c;
         }
      }

      private void Refresh_Clicked(object sender, EventArgs e)
      {
         Execute_Query();
         requery = false;
      }

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            OrgnBs.EndEdit();
            DeptBs.EndEdit();
            BuntBs.EndEdit();
            SuntBs.EndEdit();
            OgdcBs.EndEdit();

            iCRM.SubmitChanges();
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
               requery = false;
            }
         }
      }

      private void Tsb_DelOrgn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var epit = OrgnBs.Current as Data.Expense_Item;

            iCRM.DEL_EPIT_P(epit.CODE);
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

      private void TransferOrgnDcmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqrq = RqrqBs.Current as Data.Request_Requester;
            var sunt = SuntBs.Current as Data.Sub_Unit;

            foreach (var rqdc in rqrq.Request_Documents)
            {
               if (!sunt.Organ_Documents.Any(ogdc => ogdc.Request_Document == rqdc))
               {
                  iCRM.INS_OGDC_P(sunt.BUNT_DEPT_ORGN_CODE, sunt.BUNT_DEPT_CODE, sunt.BUNT_CODE, sunt.CODE, rqdc.RDID, rqdc.NEED_TYPE, "002");
               }
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
               requery = false;
            }
         }
      }

      private void Ogdc_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var ogdc = OgdcBs.Current as Data.Organ_Document;

            iCRM.DEL_OGDC_P(ogdc.ODID);
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

      private void RqrqBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqrq = RqrqBs.Current as Data.Request_Requester;
            var sunt = SuntBs.Current as Data.Sub_Unit;

            if (sunt == null || rqrq == null) { BcdsBs.List.Clear(); OgdcBs.List.Clear(); return; }

            BcdsBs.DataSource = iCRM.Basic_Calculate_Discounts.Where(b => b.Sub_Unit == sunt);
            OgdcBs.DataSource = iCRM.Organ_Documents.Where(o => o.Sub_Unit == sunt && o.Request_Document.Request_Requester == rqrq);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OrgnBs_CurrentChanged(object sender, EventArgs e)
      {
         var orgn = OrgnBs.Current as Data.Organ;
         if (orgn == null) { DeptBs.List.Clear(); return; }

         DeptBs.DataSource = iCRM.Departments.Where(d => d.Organ == orgn);
      }

      private void DeptBs_CurrentChanged(object sender, EventArgs e)
      {
         var dept = DeptBs.Current as Data.Department;
         if (dept == null) { BuntBs.List.Clear(); return; }

         BuntBs.DataSource = iCRM.Base_Units.Where(d => d.Department == dept);
      }

      private void BuntBs_CurrentChanged(object sender, EventArgs e)
      {
         var bunt = BuntBs.Current as Data.Base_Unit;
         if (bunt == null) { SuntBs.List.Clear(); return; }

         SuntBs.DataSource = iCRM.Sub_Units.Where(d => d.Base_Unit == bunt);
      }
   }
}
