using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Method
{
   partial class MSTR_MTOD_F
   {
      partial void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         int _m = MtodBs1.Position;
         int _c = CtgyBs1.Position;
         int _d = DsctBs1.Position;
         MtodBs1.DataSource = iScsc.Methods;
         MtodBs1.Position = _m;
         CtgyBs1.Position = _c;
         DsctBs1.Position = _d;
      }

      partial void SubmitChange_Mtod_Click(object sender, EventArgs e)
      {
         try
         {
            this.Focus();
            Validate();
            MtodBs1.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            MtodBs1.DataSource = iScsc.Methods;
         }
      }

      partial void SubmitChange_Ctgy_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.CommandTimeout = 1800;
            this.Focus();
            Validate();
            CtgyBs1.EndEdit();

            iScsc.SubmitChanges();

         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            MtodBs1.DataSource = iScsc.Methods;
         }
      }

      partial void SubmitChange_Dsct_Click(object sender, EventArgs e)
      {
         try
         {
            this.Focus();
            Validate();
            DsctBs1.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            MtodBs1.DataSource = iScsc.Methods;
         }
      }

      partial void DelCtgy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = CtgyBs1.Current as Data.Category_Belt;
            
            if(crnt != null && MessageBox.Show(this, "آیا با حذف رسته موافق هستید؟", "حذف رسته", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Category_Belts.DeleteOnSubmit(crnt);

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message.Substring(0, exc.Message.IndexOf("\r\n")));
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

      partial void DelMtod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = MtodBs1.Current as Data.Method;

            if (crnt != null && MessageBox.Show(this, "آیا با حذف سبک موافق هستید؟", "حذف سبک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Methods.DeleteOnSubmit(crnt);

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message.Substring(0, exc.Message.IndexOf("\r\n")));
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
   }
}
