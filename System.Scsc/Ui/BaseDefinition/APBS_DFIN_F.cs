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

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class APBS_DFIN_F : UserControl
   {
      public APBS_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formCaller = "";
      private string tableName = "";

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if(tb_master.SelectedTab == tp_001)
         {
            int b = ApbsBs.Position;
            int t = SApbBs.Position;
            ApbsBs.DataSource = iScsc.App_Base_Defines.Where(a => a.ENTY_NAME == tableName && a.REF_CODE == null);
            ApbsBs.Position = b;
            SApbBs.Position = t;
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
            Apbs_Gv.PostEditor();
            SApb_Gv.PostEditor();
            ApbsBs.EndEdit();
            SApbBs.EndEdit();

            iScsc.SubmitChanges();
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

      private void Tsb_DelApbs_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Apbs = ApbsBs.Current as Data.App_Base_Define;

            iScsc.DEL_APBS_P(Apbs.CODE);
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

      private void Tsb_DelSapb_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var sapb = SApbBs.Current as Data.App_Base_Define;

            iScsc.DEL_APBS_P(sapb.CODE);
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

      private void AddApbs_Butn_Click(object sender, EventArgs e)
      {
         if (ApbsBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         ApbsBs.AddNew();
         var apbs = ApbsBs.Current as Data.App_Base_Define;
         apbs.ENTY_NAME = tableName;

         apbs.RWNO = ApbsBs.List.Count == 1 ? 1 : ApbsBs.List.OfType<Data.App_Base_Define>().Max(a => a.RWNO) + 1;
      }

      private void AddSapb_Butn_Click(object sender, EventArgs e)
      {
         if (SApbBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         SApbBs.AddNew();
         var apbs = SApbBs.Current as Data.App_Base_Define;
         apbs.ENTY_NAME = tableName;

         apbs.RWNO = SApbBs.List.Count == 1 ? 1 : SApbBs.List.OfType<Data.App_Base_Define>().Max(a => a.RWNO) + 1;
      }

   }
}
