using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Regulation
{
   partial class MSTR_REGL_F
   {
      partial void regulationBindingNavigatorSaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            regulationBindingSource.EndEdit();
            iScsc.SubmitChanges();

         }
         catch 
         {
            MessageBox.Show("خطا در انجام عملیات. لطفا بررسی کنید");
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            regulationBindingSource.DataSource = iScsc.Regulations;
         }
      }

      partial void Btn_InsRegl_Click(object sender, EventArgs e)
      {
         PNL_REGL.Visible = true;
         regulationBindingNavigator.Enabled = false;
         regulationGridControl.Enabled = false;

         Spn_Year.EditValue = 0;
         Txt_LettNo.EditValue = "";
         Dat_LettDate.EditValue = DateTime.Now;
         Txt_LettOwnr.EditValue = "";
         Lov_ReglType.EditValue = "001";
         Spn_TaxPrct.EditValue = 0;
         Spn_DutyPrct.EditValue = 0;
         Dat_StrtDate.EditValue = DateTime.Now;
         Dat_EndDate.EditValue = DateTime.Now.AddMonths(12);

         CanDirectExitForm = false;
      }

      partial void Btn_SubmitInsRegl_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            var newRegulation = new Data.Regulation()
            {
               YEAR = Convert.ToInt16(Spn_Year.EditValue),
               LETT_NO = Txt_LettNo.EditValue.ToString(),
               LETT_DATE = string.IsNullOrEmpty(Dat_LettDate.EditValue.ToString()) ? DateTime.Now : Convert.ToDateTime(Dat_LettDate.EditValue),
               LETT_OWNR = (string)Txt_LettOwnr.EditValue,
               TYPE = string.IsNullOrEmpty(Lov_ReglType.EditValue.ToString()) ? "001" : (string)Lov_ReglType.EditValue,
               TAX_PRCT = string.IsNullOrEmpty(Spn_TaxPrct.EditValue.ToString()) ? 0 : Convert.ToSingle(Spn_TaxPrct.EditValue),
               DUTY_PRCT = string.IsNullOrEmpty(Spn_DutyPrct.EditValue.ToString()) ? 0 : Convert.ToSingle(Spn_DutyPrct.EditValue),
               STRT_DATE = string.IsNullOrEmpty(Dat_StrtDate.EditValue.ToString()) ? DateTime.Now : Convert.ToDateTime(Dat_StrtDate.EditValue),
               END_DATE = string.IsNullOrEmpty(Dat_EndDate.EditValue.ToString()) ? DateTime.Now : Convert.ToDateTime(Dat_EndDate.EditValue),
               CRET_DATE = DateTime.Now,
               MDFY_DATE = DateTime.Now
            };
            iScsc.CommandTimeout = 18000;
            iScsc.Regulations.InsertOnSubmit(newRegulation);

            iScsc.SubmitChanges();

            regulationBindingNavigator.Enabled = true;
            regulationGridControl.Enabled = true;

            CanDirectExitForm = true;
         }
         catch 
         {
            MessageBox.Show("خطا در انجام عملیات. لطفا بررسی کنید");
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            regulationBindingSource.DataSource = iScsc.Regulations;
         }
      }

      partial void Btn_CnclInsRegl_Click(object sender, EventArgs e)
      {
         PNL_REGL.Visible = false;
         regulationBindingNavigator.Enabled = true;
         regulationGridControl.Enabled = true;

         CanDirectExitForm = true;
      }

      partial void HL_INVSREGL_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         Data.Regulation Current = (Data.Regulation)regulationBindingSource.Current;

         if (Current.TYPE == "002")
         {
            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "Localhost",
            //      new List<Job>
            //      {
            //         new Job(SendType.Self, 04 /* Execute Regl_Acnt_F */){Input = Current}
            //      })
            //);

            var Rg1 = iScsc.Regulations.Where(rg => rg.TYPE == "001" && rg.REGL_STAT == "002").SingleOrDefault();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, Current}}
                  })
            );
         }
         else if (Current.TYPE == "001")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 05 /* Execute Regl_Expn_F */){Input = new List<Data.Regulation>{Current, null}}
                  })
            );
         }
         else if (Current.TYPE == "004")
         {

         }
      }

      partial void DEL_REGL_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا مطمئن هستید که آیین نامه حذف شود", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var CrntRegl = regulationBindingSource.Current as Data.Regulation;
            iScsc.DEL_REGL_P(CrntRegl.YEAR, CrntRegl.CODE);
         }
         catch
         {
            MessageBox.Show("خطا در انجام عملیات. لطفا بررسی کنید");
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            regulationBindingSource.DataSource = iScsc.Regulations;
         }
      }
   }
}
