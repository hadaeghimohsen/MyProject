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
   public partial class BTRF_DFIN_F : UserControl
   {
      public BTRF_DFIN_F()
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
         int b = BtrfBs.Position;
         int t = TrfdBs.Position;
         int a = SrtpBs.Position;
         bool isTab001 = tb_master.SelectedTab == tp_001;
         bool isTab002 = tb_master.SelectedTab == tp_002;
         
         var result = await Task.Run(() =>
         {
            using (var ctx = new Data.iCRMDataContext(ConnectionString))
            {
               return new
               {
                  BaseTariffs = ctx.Base_Tariffs.ToList(),
                  ServiceTypes = ctx.Service_Types.ToList(),
               };
            }
         });
         
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if (isTab001)
         {
            BtrfBs.DataSource = result.BaseTariffs;
            BtrfBs.Position = b;
            TrfdBs.Position = t;
         }
         else if (isTab002)
         {
            SrtpBs.DataSource = result.ServiceTypes;
            SrtpBs.Position = a;
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
            //if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            SrtpBs.EndEdit();
            BtrfBs.EndEdit();
            TrfdBs.EndEdit();

            Srtp_gv.PostEditor();

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

      private void Tsb_DelBtrf_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var btrf = BtrfBs.Current as Data.Base_Tariff;

            iCRM.DEL_BTRF_P(btrf.CODE);
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

      private void Tsb_DelTrfd_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var trfd = TrfdBs.Current as Data.Base_Tariff_Detail;

            iCRM.DEL_TRFD_P(trfd.CODE);
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

      private void DelSrtp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var srtp = SrtpBs.Current as Data.Service_Type;

            iCRM.DEL_SRTP_P(srtp.CODE);
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
