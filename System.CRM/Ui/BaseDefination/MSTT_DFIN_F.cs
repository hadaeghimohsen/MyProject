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
   public partial class MSTT_DFIN_F : UserControl
   {
      public MSTT_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formCaller = "";

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
            int b = MsttBs.Position;
            int t = SsttBs.Position;
            MsttBs.DataSource = iCRM.Main_States;
            MsttBs.Position = b;
            SsttBs.Position = t;
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
            Mstt_Gv.PostEditor();
            Sstt_Gv.PostEditor();
            MsttBs.EndEdit();
            SsttBs.EndEdit();

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
            }
         }
      }

      private void Tsb_DelMstt_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Mstt = MsttBs.Current as Data.Main_State;

            iCRM.DEL_MSTT_P(Mstt.CODE);
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

      private void Tsb_DelSstt_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var sstt = SsttBs.Current as Data.Sub_State;

            iCRM.DEL_SSTT_P(sstt.MSTT_CODE, sstt.CODE);
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

      private void AddMstt_Butn_Click(object sender, EventArgs e)
      {
         MsttBs.AddNew();
         var mstt = MsttBs.Current as Data.Main_State;

         mstt.CODE = (short)(MsttBs.List.Count == 1 ? 1 : MsttBs.List.OfType<Data.Main_State>().Max(a => a.CODE) + 1);
      }

      private void AddSstt_Butn_Click(object sender, EventArgs e)
      {
         SsttBs.AddNew();
         var sstt = SsttBs.Current as Data.Sub_State;

         sstt.CODE = (short)(SsttBs.List.Count == 1 ? 1 : SsttBs.List.OfType<Data.Sub_State>().Max(a => a.CODE) + 1);
      }

   }
}
