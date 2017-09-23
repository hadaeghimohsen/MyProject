using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Regulation
{
   partial class MSTR_DCMT_F
   {
      partial void document_SpecBindingNavigatorSaveItem_Click(object sender, EventArgs e)
      {
          try
          {
              this.Focus();
              Validate();
              document_SpecBindingSource.EndEdit();

              iScsc.SubmitChanges();
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
          }
          finally
          {
              iScsc = new Data.iScscDataContext(ConnectionString);
              document_SpecBindingSource.DataSource = iScsc.Document_Specs;
          }
      }

      partial void Btn_AddNewItem_Click(object sender, EventArgs e)
      {
         Pnl_Item.Visible = true;
         document_SpecGridControl.Enabled = false;

         CanDirectExitForm = false;
      }

      partial void Btn_Cancel_Click(object sender, EventArgs e)
      {
         Pnl_Item.Visible = false;
         document_SpecGridControl.Enabled = true;

         CanDirectExitForm = true;
      }

      partial void Btn_SubmitChange_Click(object sender, EventArgs e)
      {
          try
          {
              if (!string.IsNullOrEmpty(Txt_DcmtDesc.Text))
              {
                  iScsc.INS_DCMT_P(Txt_DcmtDesc.Text);
                  //document_SpecBindingSource.DataSource = iScsc.Document_Specs.ToList();

                  Btn_Cancel_Click(null, null);
              }
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
          }
          finally
          {
              iScsc = new Data.iScscDataContext(ConnectionString);
              document_SpecBindingSource.DataSource = iScsc.Document_Specs;
          }
      }

      partial void LV_UPDEL_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
          try
          {
              if (e.Button.Tag.ToString() == "Delete")
              {
                  if (MessageBox.Show(this, "آیا از پاک کردن سند از درون سیستم مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                  {
                      var current = document_SpecBindingSource.Current as Data.Document_Spec;
                      iScsc.DEL_DCMT_P(current.DSID);
                      //document_SpecBindingSource.DataSource = iScsc.Document_Specs.ToList();
                  }
              }
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
          }
          finally
          {
              iScsc = new Data.iScscDataContext(ConnectionString);
              document_SpecBindingSource.DataSource = iScsc.Document_Specs;
          }
      }
   }
}
