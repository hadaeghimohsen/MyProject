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
   public partial class REGN_DFIN_F : UserControl
   {
      public REGN_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formcaller;

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
            int c = CntyBs.Position;
            int p = PrvnBs.Position;
            int r = RegnBs.Position;
            int a = CompBs.Position;
            CntyBs.DataSource = iCRM.Countries;
            CntyBs.Position = c;
            PrvnBs.Position = p;
            RegnBs.Position = r;
            CompBs.Position = a;
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

            CntyBs.EndEdit();
            PrvnBs.EndEdit();
            RegnBs.EndEdit();
            CompBs.EndEdit();
            CmctBs1.EndEdit();
            ComwBs1.EndEdit();
            CmacBs1.EndEdit();

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

      private void Tsb_DelCnty_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var cnty = CntyBs.Current as Data.Country;

            iCRM.DEL_CNTY_P(cnty.CODE);
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

      private void Tsb_DelPrvn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var prvn = PrvnBs.Current as Data.Province;

            iCRM.DEL_PRVN_P(prvn.CNTY_CODE, prvn.CODE);
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

      private void Tsb_DelRegn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var regn = RegnBs.Current as Data.Region;

            iCRM.DEL_REGN_P(regn.PRVN_CNTY_CODE, regn.PRVN_CODE, regn.CODE);
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

      private void Tsb_DelAgnc_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var comp = CompBs.Current as Data.Company;

            iCRM.DEL_COMP_P(comp.CODE);
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

      private void Tsb_DelCmct_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            //var cmct = CmctBs1.Current as Data.Company_Contact;

            //iCRM.DEL_CMCT_P(cmct.CODE);
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

      private void Even_Butn_Click(object sender, EventArgs e)
      {
         ComwBs1.List.OfType<Data.Weekday_Info>().Where(w => Convert.ToInt32(w.WEEK_DAY) % 2 == 0).ToList().ForEach(w => w.STAT = "002");
      }

      private void Odd_Butn_Click(object sender, EventArgs e)
      {
         ComwBs1.List.OfType<Data.Weekday_Info>().Where(w => Convert.ToInt32(w.WEEK_DAY) % 2 != 0).ToList().ForEach(w => w.STAT = "002");
      }

      private void Close_Butn_Click(object sender, EventArgs e)
      {
         ComwBs1.List.OfType<Data.Weekday_Info>().ToList().ForEach(w => w.STAT = "001");
      }

      private void Tsb_DelCmac_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var cmac = CmacBs1.Current as Data.Company_Activity;

            iCRM.DEL_CMAC_P(cmac.CODE);
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
