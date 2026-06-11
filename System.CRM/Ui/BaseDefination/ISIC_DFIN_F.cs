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
   public partial class ISIC_DFIN_F : UserControl
   {
      public ISIC_DFIN_F()
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

      private async void Execute_Query()
      {
         int g = ISCGBs1.Position;
         int a = ISCABs1.Position;
         int p = ISCPBs1.Position;
         bool isTab001 = tb_master.SelectedTab == tp_001;
         
         var result = await Task.Run(() =>
         {
            using (var ctx = new Data.iCRMDataContext(ConnectionString))
            {
               return new
               {
                  IsicGroups = ctx.Isic_Groups.ToList(),
               };
            }
         });
         
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if (isTab001)
         {
            ISCGBs1.DataSource = result.IsicGroups;
            ISCGBs1.Position = g;
            ISCABs1.Position = a;
            ISCPBs1.Position = p;
         }
         
         requery = false;
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

            ISCGBs1.EndEdit();
            ISCABs1.EndEdit();
            ISCPBs1.EndEdit();

            iCRM.SubmitChanges();
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

      private void DelIscg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var iscg = ISCGBs1.Current as Data.Isic_Group;

            iCRM.DEL_ISCG_P(iscg.CODE);
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

      private void DelIsca_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var isca = ISCABs1.Current as Data.Isic_Activity;

            iCRM.DEL_ISCA_P(isca.ISCG_CODE, isca.CODE);
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

      private void DelIscp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var iscp = ISCABs1.Current as Data.Isic_Product;

            iCRM.DEL_ISCP_P(iscp.ISCA_ISCG_CODE, iscp.ISCA_CODE, iscp.CODE);
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

   }
}
