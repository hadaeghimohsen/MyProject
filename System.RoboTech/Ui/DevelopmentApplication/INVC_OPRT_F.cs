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
using System.RoboTech.ExceptionHandlings;
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.RoboTech.ExtCode;
using System.Diagnostics;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class INVC_OPRT_F : UserControl
   {
      public INVC_OPRT_F()
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
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int ordr029 = Ordr029Bs.Position;
         int ordt029 = Ordt029Bs.Position;
         int rbpr = RbprBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         Ordr029Bs.Position = ordr029;
         Ordt029Bs.Position = ordt029;
         RbprBs.Position = rbpr;

         // Initial Controls
         CretByOrdrI_Cb.SelectedIndex = CretByOrdrI_Cb.SelectedIndex == -1 ? 0 : CretByOrdrI_Cb.SelectedIndex;

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            // Step 1
            Orders_Tc_SelectedIndexChanged(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Orders_Tc_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            switch (Orders_Tc.SelectedTab.Name)
            {
               case "tp_029":
                  Ordr029Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "029" && o.ORDR_STAT == "001" && o.CRET_BY == (CretByOrdrI_Cb.SelectedIndex == 0 ? CurrentUser : o.CRET_BY));
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #region Public Opreation Invoice
      private void AddOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tsb = (ToolStripButton)sender;
            if (_tsb == null) return;

            switch (_tsb.Tag.ToString())
            {
               case "029":
                  if (Ordr029Bs.List.OfType<Data.Order>().Any(o => o.CODE == 0)) return;

                  var ordr029 = Ordr029Bs.AddNew() as Data.Order;
                  ordr029.ORDR_TYPE = "029";
                  ordr029.SUB_SYS = 12;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tsb = (ToolStripButton)sender;
            if (_tsb == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            switch (_tsb.Tag.ToString())
            {
               case "029":
                  var ordr029 = Ordr029Bs.Current as Data.Order;
                  if (ordr029 == null) return;

                  iRoboTech.DEL_ORDR_P(ordr029.CODE);
                  break;
               default:
                  break;
            }
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Ordr029Bs.EndEdit();
            Ordt029_Gv.PostEditor();

            Ordr030Bs.EndEdit();
            Ordr031Bs.EndEdit();
            Ordr032Bs.EndEdit();
            Ordr033Bs.EndEdit();
            Ordr034Bs.EndEdit();

            switch (Orders_Tc.SelectedTab.Name)
            {
               case "tp_029":
                  if(Ordr029Bs.List.OfType<Data.Order>().Any(o => o.APBS_CODE == null))
                  {
                     MessageBox.Show("در یکی از درخواست های خرید شما نام فروشنده مشخص نمی باشد، لطفا فروشنده فاکتور را مشخص کنید");
                     return;
                  }

                  // برای درخواست جدید فاکتور خرید
                  var ordr029 = Ordr029Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CODE == 0);
                  if (ordr029 != null)
                  {
                     if (ordr029.APBS_CODE != null && !iRoboTech.Service_Robot_Account_Links.Any(a => a.APBS_CODE == ordr029.APBS_CODE && a.STAT == "002"))
                     {
                        MessageBox.Show("برای درخواست ایجادی شماکد حساب فروشنده به درستی انتخاب نشده است، لطفا کد فروشنده را به درستی وارد کنید");
                        return;
                     }
                     else
                     {
                        var sral = iRoboTech.Service_Robot_Account_Links.FirstOrDefault(a => a.APBS_CODE == ordr029.APBS_CODE && a.STAT == "002");
                        ordr029.Service_Robot = sral.Service_Robot;
                     }
                  }                  
                  break;
               default:
                  break;
            }

            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void QuryOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Execute_Query();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void BlnkOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Ordr029Bs.List.Clear();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CretByOrdrI_Cb_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            Execute_Query();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddItemOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tsb = (ToolStripButton)sender;

            switch (_tsb.Tag.ToString())
            {
               case "029":
                  var ordr029 = Ordr029Bs.Current as Data.Order;
                  if (ordr029 == null) return;

                  // اگر ردیفی جدیدی وجود داشته باشه که هنوز ذخیره نشده باشد
                  if (Ordt029Bs.List.OfType<Data.Order_Detail>().Any(od => od.RWNO == 0)) return;

                  var ordt029 = Ordt029Bs.AddNew() as Data.Order_Detail;
                  ordt029.Order = ordr029;
                  ordt029.NUMB = 1;
                  ordt029.OFF_PRCT = 0;
                  ordt029.DSCN_AMNT_DNRM = 0;
                  iRoboTech.Order_Details.InsertOnSubmit(ordt029);
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelItemOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tsb = (ToolStripButton)sender;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            switch (_tsb.Tag.ToString())
            {
               case "029":
                  var ordt029 = Ordt029Bs.Current as Data.Order_Detail;
                  if (ordt029 == null) return;

                  iRoboTech.DEL_ODRT_P(ordt029.ORDR_CODE, ordt029.RWNO);
                  break;
               default:
                  break;
            }

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);            
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void RqryOrdrI_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void AddProdOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _sb = (SimpleButton)sender;

            switch (_sb.Tag.ToString())
            {
               case "029":
                  var ordr029 = Ordr029Bs.Current as Data.Order;
                  if (ordr029 == null) return;

                  var prod029 = RbprBs.Current as Data.Robot_Product;
                  if (prod029 == null) return;

                  var ordt029 = Ordt029Bs.List.OfType<Data.Order_Detail>().FirstOrDefault(od => od.Order == ordr029 && od.TARF_CODE == prod029.TARF_CODE);
                  if (ordt029 == null)
                  {
                     ordt029 = Ordt029Bs.AddNew() as Data.Order_Detail;
                     ordt029.Order = ordr029;
                     ordt029.TARF_CODE = prod029.TARF_CODE;
                     ordt029.NUMB = 0;
                     ordt029.OFF_PRCT = 0;
                     ordt029.DSCN_AMNT_DNRM = 0;

                     iRoboTech.Order_Details.InsertOnSubmit(ordt029);
                  }
                  else
                  {
                     ordt029.NUMB++;
                  }
                  break;
               default:
                  break;
            }

            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void CretNewProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                 {
                   new Job(SendType.Self, 26 /* Execute Prod_Dvlp_F */),
                   new Job(SendType.SelfToUserInterface, "PROD_DVLP_F", 10 /* Execute Actn_CalF_P */)
                 })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion      
   }
}
