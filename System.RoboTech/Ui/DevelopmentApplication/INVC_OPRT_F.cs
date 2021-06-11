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
         int odst029 = Odst029Bs.Position;
         int ordr030 = Ordr030Bs.Position;
         int ordt030 = Ordt030Bs.Position;
         int odst030 = Odst030Bs.Position;
         int ordr031 = Ordr031Bs.Position;
         int ordt031 = Ordt031Bs.Position;
         int odst031 = Odst031Bs.Position;
         int ordr032 = Ordr032Bs.Position;
         int ordt032 = Ordt032Bs.Position;
         int odst032 = Odst032Bs.Position;
         int ordr033 = Ordr033Bs.Position;
         int ordt033 = Ordt033Bs.Position;
         int rbpr = RbprBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         Ordr029Bs.Position = ordr029;
         Ordt029Bs.Position = ordt029;
         Odst029Bs.Position = odst029;
         Ordr030Bs.Position = ordr030;
         Ordt030Bs.Position = ordt030;
         Odst030Bs.Position = odst030;
         Ordr031Bs.Position = ordr031;
         Ordt031Bs.Position = ordt031;
         Odst031Bs.Position = odst031;
         Ordr032Bs.Position = ordr032;
         Ordt032Bs.Position = ordt032;
         Odst032Bs.Position = odst032;
         Ordr033Bs.Position = ordr033;
         Ordt033Bs.Position = ordt033;
         RbprBs.Position = rbpr;

         // Initial Controls
         CretByOrdrI34_Cb.SelectedIndex = CretByOrdrI33_Cb.SelectedIndex = CretByOrdrI31_Cb.SelectedIndex = CretByOrdrI30_Cb.SelectedIndex = CretByOrdrI29_Cb.SelectedIndex = (CretByOrdrI29_Cb.SelectedIndex == -1 ? 0 : CretByOrdrI29_Cb.SelectedIndex);

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
                  Ordr029Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "029" && o.ORDR_STAT == "001" && o.CRET_BY == (CretByOrdrI29_Cb.SelectedIndex == 0 ? CurrentUser : o.CRET_BY));
                  break;
               case "tp_030":
                  Ordr030Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "030" && o.ORDR_STAT == "001" && o.CRET_BY == (CretByOrdrI30_Cb.SelectedIndex == 0 ? CurrentUser : o.CRET_BY));
                  break;
               case "tp_031":
                  Ordr031Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "031" && o.ORDR_STAT == "001" && o.CRET_BY == (CretByOrdrI31_Cb.SelectedIndex == 0 ? CurrentUser : o.CRET_BY));
                  break;
               case "tp_032":
                  Ordr032Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "032" && o.ORDR_STAT == "001" && o.CRET_BY == (CretByOrdrI32_Cb.SelectedIndex == 0 ? CurrentUser : o.CRET_BY));
                  break;
               case "tp_033":
                  Ordr033Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "033" && o.ORDR_STAT == "001" && o.CRET_BY == (CretByOrdrI33_Cb.SelectedIndex == 0 ? CurrentUser : o.CRET_BY));
                  break;
               case "tp_034":
                  Ordr034Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "034" && o.ORDR_STAT == "001" && o.CRET_BY == (CretByOrdrI34_Cb.SelectedIndex == 0 ? CurrentUser : o.CRET_BY));
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
               case "030":
                  if (Ordr030Bs.List.OfType<Data.Order>().Any(o => o.CODE == 0)) return;

                  var ordr030 = Ordr030Bs.AddNew() as Data.Order;
                  ordr030.ORDR_TYPE = "030";
                  ordr030.SUB_SYS = 12;
                  break;
               case "031":
                  if (Ordr031Bs.List.OfType<Data.Order>().Any(o => o.CODE == 0)) return;

                  var ordr031 = Ordr031Bs.AddNew() as Data.Order;
                  ordr031.ORDR_TYPE = "031";
                  ordr031.SUB_SYS = 12;
                  break;
               case "032":
                  if (Ordr032Bs.List.OfType<Data.Order>().Any(o => o.CODE == 0)) return;

                  var ordr032 = Ordr032Bs.AddNew() as Data.Order;
                  ordr032.ORDR_TYPE = "032";
                  ordr032.SUB_SYS = 12;
                  break;
               case "033":
                  if (Ordr033Bs.List.OfType<Data.Order>().Any(o => o.CODE == 0)) return;

                  var ordr033 = Ordr033Bs.AddNew() as Data.Order;
                  ordr033.ORDR_TYPE = "033";
                  ordr033.SUB_SYS = 12;
                  break;
               case "034":
                  if (Ordr034Bs.List.OfType<Data.Order>().Any(o => o.CODE == 0)) return;

                  var ordr034 = Ordr034Bs.AddNew() as Data.Order;
                  ordr034.ORDR_TYPE = "034";
                  ordr034.SUB_SYS = 12;
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
               case "030":
                  var ordr030 = Ordr030Bs.Current as Data.Order;
                  if (ordr030 == null) return;

                  iRoboTech.DEL_ORDR_P(ordr030.CODE);
                  break;
               case "031":
                  var ordr031 = Ordr031Bs.Current as Data.Order;
                  if (ordr031 == null) return;

                  iRoboTech.DEL_ORDR_P(ordr031.CODE);
                  break;
               case "032":
                  var ordr032 = Ordr032Bs.Current as Data.Order;
                  if (ordr032 == null) return;

                  iRoboTech.DEL_ORDR_P(ordr032.CODE);
                  break;
               case "033":
                  var ordr033 = Ordr033Bs.Current as Data.Order;
                  if (ordr033 == null) return;

                  iRoboTech.DEL_ORDR_P(ordr033.CODE);
                  break;
               case "034":
                  var ordr034 = Ordr034Bs.Current as Data.Order;
                  if (ordr034 == null) return;

                  iRoboTech.DEL_ORDR_P(ordr034.CODE);
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
            Odst029_Gv.PostEditor();

            Ordr030Bs.EndEdit();
            Ordt030_Gv.PostEditor();
            Odst030_Gv.PostEditor();

            Ordr031Bs.EndEdit();
            Ordt031_Gv.PostEditor();
            Odst031_Gv.PostEditor();

            Ordr032Bs.EndEdit();
            Ordt032_Gv.PostEditor();
            Odst032_Gv.PostEditor();            
            
            Ordr033Bs.EndEdit();
            Ordt033_gv.PostEditor();

            Ordr034Bs.EndEdit();
            Ordt034_Gv.PostEditor();

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
                        var sral = iRoboTech.Service_Robot_Account_Links.FirstOrDefault(a => a.APBS_CODE == ordr029.APBS_CODE && a.SRBT_SERV_FILE_NO == ordr029.SRBT_SERV_FILE_NO  && a.STAT == "002");
                        ordr029.Service_Robot = sral.Service_Robot;
                     }
                  }                  
                  break;
               case "tp_030":
                  if (Ordr030Bs.List.OfType<Data.Order>().Any(o => o.APBS_CODE == null))
                  {
                     MessageBox.Show("در یکی از درخواست های برگشت از خرید شما نام فروشنده مشخص نمی باشد، لطفا فروشنده فاکتور را مشخص کنید");
                     return;
                  }

                  // برای درخواست جدید فاکتور برگشت از خرید
                  var ordr030 = Ordr030Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CODE == 0);
                  if (ordr030 != null)
                  {
                     if (ordr030.APBS_CODE != null && !iRoboTech.Service_Robot_Account_Links.Any(a => a.APBS_CODE == ordr030.APBS_CODE && a.STAT == "002"))
                     {
                        MessageBox.Show("برای درخواست ایجادی شماکد حساب فروشنده به درستی انتخاب نشده است، لطفا کد فروشنده را به درستی وارد کنید");
                        return;
                     }
                     else
                     {
                        var sral = iRoboTech.Service_Robot_Account_Links.FirstOrDefault(a => a.APBS_CODE == ordr030.APBS_CODE && a.SRBT_SERV_FILE_NO == ordr030.SRBT_SERV_FILE_NO && a.STAT == "002");
                        ordr030.Service_Robot = sral.Service_Robot;
                     }
                  }
                  break;
               case "tp_031":
                  if (Ordr031Bs.List.OfType<Data.Order>().Any(o => o.APBS_CODE == null))
                  {
                     MessageBox.Show("در یکی از درخواست های فروش شما نام فروشنده مشخص نمی باشد، لطفا خریدار فاکتور را مشخص کنید");
                     return;
                  }

                  // برای درخواست جدید فاکتور فروش
                  var ordr031 = Ordr031Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CODE == 0);
                  if (ordr031 != null)
                  {
                     if (ordr031.APBS_CODE != null && !iRoboTech.Service_Robot_Account_Links.Any(a => a.APBS_CODE == ordr031.APBS_CODE && a.STAT == "002"))
                     {
                        MessageBox.Show("برای درخواست ایجادی شماکد حساب خریدار به درستی انتخاب نشده است، لطفا کد خریدار را به درستی وارد کنید");
                        return;
                     }
                     else
                     {
                        var sral = iRoboTech.Service_Robot_Account_Links.FirstOrDefault(a => a.APBS_CODE == ordr031.APBS_CODE && a.SRBT_SERV_FILE_NO == ordr031.SRBT_SERV_FILE_NO && a.STAT == "002");
                        ordr031.Service_Robot = sral.Service_Robot;
                     }
                  }
                  break;
               case "tp_032":
                  if (Ordr032Bs.List.OfType<Data.Order>().Any(o => o.APBS_CODE == null))
                  {
                     MessageBox.Show("در یکی از درخواست های فروش شما نام خریدار مشخص نمی باشد، لطفا خریدار فاکتور را مشخص کنید");
                     return;
                  }

                  // برای درخواست جدید فاکتور فروش
                  var ordr032 = Ordr032Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CODE == 0);
                  if (ordr032 != null)
                  {
                     if (ordr032.APBS_CODE != null && !iRoboTech.Service_Robot_Account_Links.Any(a => a.APBS_CODE == ordr032.APBS_CODE && a.STAT == "002"))
                     {
                        MessageBox.Show("برای درخواست ایجادی شماکد حساب خریدار به درستی انتخاب نشده است، لطفا کد خریدار را به درستی وارد کنید");
                        return;
                     }
                     else
                     {
                        var sral = iRoboTech.Service_Robot_Account_Links.FirstOrDefault(a => a.APBS_CODE == ordr032.APBS_CODE && a.SRBT_SERV_FILE_NO == ordr032.SRBT_SERV_FILE_NO && a.STAT == "002");
                        ordr032.Service_Robot = sral.Service_Robot;
                     }
                  }
                  break;
               case "tp_033":
                  if(Ordr033Bs.List.OfType<Data.Order>().Any(o => o.APBS_CODE == null))
                  {
                     MessageBox.Show("در یکی از درخواست های رسید شما نام کاربر ثبت کننده مشخص نمی باشد، لطفا درخت حساب را مشخص کنید");
                     return;
                  }

                  // برای درخواست جدید فاکتور رسید
                  var ordr033 = Ordr033Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CODE == 0);
                  if (ordr033 != null)
                  {
                     if ((ordr033.APBS_CODE != null && !iRoboTech.Service_Robot_Account_Links.Any(a => a.APBS_CODE == ordr033.APBS_CODE && a.STAT == "002")) ||
                         (ordr033.APBS_CODE != null && ordr033.SRBT_SERV_FILE_NO == null))
                     {
                        MessageBox.Show("برای درخواست ایجادی شماکد حساب ثبت کننده به درستی انتخاب نشده است، لطفا کد کاربر ثبت کننده را به درستی وارد کنید");
                        return;
                     }
                     else
                     {
                        var sral = iRoboTech.Service_Robot_Account_Links.FirstOrDefault(a => a.APBS_CODE == ordr033.APBS_CODE && a.SRBT_SERV_FILE_NO == ordr033.SRBT_SERV_FILE_NO && a.STAT == "002");
                        ordr033.Service_Robot = sral.Service_Robot;
                     }
                  }                  
                  break;
               case "tp_034":
                  if(Ordr034Bs.List.OfType<Data.Order>().Any(o => o.APBS_CODE == null))
                  {
                     MessageBox.Show("در یکی از درخواست های حواله شما نام کاربر ثبت کننده مشخص نمی باشد، لطفا درخت حساب را مشخص کنید");
                     return;
                  }

                  // برای درخواست جدید فاکتور حواله
                  var ordr034 = Ordr034Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CODE == 0);
                  if (ordr034 != null)
                  {
                     if ((ordr034.APBS_CODE != null && !iRoboTech.Service_Robot_Account_Links.Any(a => a.APBS_CODE == ordr034.APBS_CODE && a.STAT == "002")) ||
                         (ordr034.APBS_CODE != null && ordr034.SRBT_SERV_FILE_NO == null))
                     {
                        MessageBox.Show("برای درخواست ایجادی شماکد حساب ثبت کننده به درستی انتخاب نشده است، لطفا کد کاربر ثبت کننده را به درستی وارد کنید");
                        return;
                     }
                     else
                     {
                        var sral = iRoboTech.Service_Robot_Account_Links.FirstOrDefault(a => a.APBS_CODE == ordr034.APBS_CODE && a.SRBT_SERV_FILE_NO == ordr034.SRBT_SERV_FILE_NO && a.STAT == "002");
                        ordr034.Service_Robot = sral.Service_Robot;
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
            Ordr030Bs.List.Clear();
            Ordr031Bs.List.Clear();
            Ordr033Bs.List.Clear();
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

      private void FinalSaveOrdrI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tsb = sender as ToolStripButton;
            if (_tsb == null) return;

            if (MessageBox.Show(this, "آیا با ذخیره نهایی کردن درخواست خود موافق هستید؟", "ذخیره نهایی درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            switch (_tsb.Tag.ToString())
            {
               case "029":
                  var ordr029 = Ordr029Bs.Current as Data.Order;
                  if (ordr029 == null) return;

                  iRoboTech.O029_SAVE_P(
                     new XElement("Order",
                         new XAttribute("code", ordr029.CODE)
                     )
                  );                                  
                  break;
               case "030":
                  var ordr030 = Ordr030Bs.Current as Data.Order;
                  if (ordr030 == null) return;

                  iRoboTech.O030_SAVE_P(
                     new XElement("Order",
                         new XAttribute("code", ordr030.CODE)
                     )
                  );
                  break;
               case "031":
                  var ordr031 = Ordr031Bs.Current as Data.Order;
                  if (ordr031 == null) return;

                  iRoboTech.O031_SAVE_P(
                     new XElement("Order",
                         new XAttribute("code", ordr031.CODE)
                     )
                  );
                  break;
               case "032":
                  var ordr032 = Ordr032Bs.Current as Data.Order;
                  if (ordr032 == null) return;

                  iRoboTech.O032_SAVE_P(
                     new XElement("Order",
                         new XAttribute("code", ordr032.CODE)
                     )
                  );
                  break;
               case "033":
                  var ordr033 = Ordr033Bs.Current as Data.Order;
                  if (ordr033 == null) return;

                  iRoboTech.O033_SAVE_P(
                     new XElement("Order",
                         new XAttribute("code", ordr033.CODE)
                     )
                  );
                  break;
               case "034":
                  var ordr034 = Ordr034Bs.Current as Data.Order;
                  if (ordr034 == null) return;

                  iRoboTech.O034_SAVE_P(
                     new XElement("Order",
                         new XAttribute("code", ordr034.CODE)
                     )
                  );
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
               case "030":
                  var ordr030 = Ordr030Bs.Current as Data.Order;
                  if (ordr030 == null) return;

                  // اگر ردیفی جدیدی وجود داشته باشه که هنوز ذخیره نشده باشد
                  if (Ordt030Bs.List.OfType<Data.Order_Detail>().Any(od => od.RWNO == 0)) return;

                  var ordt030 = Ordt030Bs.AddNew() as Data.Order_Detail;
                  ordt030.Order = ordr030;
                  ordt030.NUMB = 1;
                  ordt030.OFF_PRCT = 0;
                  ordt030.DSCN_AMNT_DNRM = 0;
                  iRoboTech.Order_Details.InsertOnSubmit(ordt030);
                  break;
               case "031":
                  var ordr031 = Ordr031Bs.Current as Data.Order;
                  if (ordr031 == null) return;

                  // اگر ردیفی جدیدی وجود داشته باشه که هنوز ذخیره نشده باشد
                  if (Ordt031Bs.List.OfType<Data.Order_Detail>().Any(od => od.RWNO == 0)) return;

                  var ordt031 = Ordt031Bs.AddNew() as Data.Order_Detail;
                  ordt031.Order = ordr031;
                  ordt031.NUMB = 1;
                  ordt031.OFF_PRCT = 0;
                  ordt031.DSCN_AMNT_DNRM = 0;
                  iRoboTech.Order_Details.InsertOnSubmit(ordt031);
                  break;
               case "032":
                  var ordr032 = Ordr032Bs.Current as Data.Order;
                  if (ordr032 == null) return;

                  // اگر ردیفی جدیدی وجود داشته باشه که هنوز ذخیره نشده باشد
                  if (Ordt032Bs.List.OfType<Data.Order_Detail>().Any(od => od.RWNO == 0)) return;

                  var ordt032 = Ordt032Bs.AddNew() as Data.Order_Detail;
                  ordt032.Order = ordr032;
                  ordt032.NUMB = 1;
                  ordt032.OFF_PRCT = 0;
                  ordt032.DSCN_AMNT_DNRM = 0;
                  iRoboTech.Order_Details.InsertOnSubmit(ordt032);
                  break;
               case "033":
                  var ordr033 = Ordr033Bs.Current as Data.Order;
                  if (ordr033 == null) return;

                  // اگر ردیفی جدیدی وجود داشته باشه که هنوز ذخیره نشده باشد
                  if (Ordt033Bs.List.OfType<Data.Order_Detail>().Any(od => od.RWNO == 0)) return;

                  var ordt033 = Ordt033Bs.AddNew() as Data.Order_Detail;
                  ordt033.Order = ordr033;
                  ordt033.NUMB = 1;
                  ordt033.OFF_PRCT = 0;
                  ordt033.DSCN_AMNT_DNRM = 0;
                  iRoboTech.Order_Details.InsertOnSubmit(ordt033);
                  break;
               case "034":
                  var ordr034 = Ordr034Bs.Current as Data.Order;
                  if (ordr034 == null) return;

                  // اگر ردیفی جدیدی وجود داشته باشه که هنوز ذخیره نشده باشد
                  if (Ordt034Bs.List.OfType<Data.Order_Detail>().Any(od => od.RWNO == 0)) return;

                  var ordt034 = Ordt034Bs.AddNew() as Data.Order_Detail;
                  ordt034.Order = ordr034;
                  ordt034.NUMB = 1;
                  ordt034.OFF_PRCT = 0;
                  ordt034.DSCN_AMNT_DNRM = 0;
                  iRoboTech.Order_Details.InsertOnSubmit(ordt034);
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
               case "030":
                  var ordt030 = Ordt030Bs.Current as Data.Order_Detail;
                  if (ordt030 == null) return;

                  iRoboTech.DEL_ODRT_P(ordt030.ORDR_CODE, ordt030.RWNO);
                  break;
               case "031":
                  var ordt031 = Ordt031Bs.Current as Data.Order_Detail;
                  if (ordt031 == null) return;

                  iRoboTech.DEL_ODRT_P(ordt031.ORDR_CODE, ordt031.RWNO);
                  break;
               case "032":
                  var ordt032 = Ordt032Bs.Current as Data.Order_Detail;
                  if (ordt032 == null) return;

                  iRoboTech.DEL_ODRT_P(ordt032.ORDR_CODE, ordt032.RWNO);
                  break;
               case "033":
                  var ordt033 = Ordt033Bs.Current as Data.Order_Detail;
                  if (ordt033 == null) return;

                  iRoboTech.DEL_ODRT_P(ordt033.ORDR_CODE, ordt033.RWNO);
                  break;
               case "034":
                  var ordt034 = Ordt034Bs.Current as Data.Order_Detail;
                  if (ordt034 == null) return;

                  iRoboTech.DEL_ODRT_P(ordt034.ORDR_CODE, ordt034.RWNO);
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
                     ordt029.NUMB = 1;
                     ordt029.OFF_PRCT = 0;
                     ordt029.DSCN_AMNT_DNRM = 0;

                     iRoboTech.Order_Details.InsertOnSubmit(ordt029);
                  }
                  else
                  {
                     ordt029.NUMB++;
                  }
                  break;
               case "030":
                  var ordr030 = Ordr030Bs.Current as Data.Order;
                  if (ordr030 == null) return;

                  var prod030 = RbprBs.Current as Data.Robot_Product;
                  if (prod030 == null) return;

                  var ordt030 = Ordt030Bs.List.OfType<Data.Order_Detail>().FirstOrDefault(od => od.Order == ordr030 && od.TARF_CODE == prod030.TARF_CODE);
                  if (ordt030 == null)
                  {
                     ordt030 = Ordt030Bs.AddNew() as Data.Order_Detail;
                     ordt030.Order = ordr030;
                     ordt030.TARF_CODE = prod030.TARF_CODE;
                     ordt030.NUMB = 1;
                     ordt030.OFF_PRCT = 0;
                     ordt030.DSCN_AMNT_DNRM = 0;

                     iRoboTech.Order_Details.InsertOnSubmit(ordt030);
                  }
                  else
                  {
                     ordt030.NUMB++;
                  }
                  break;
               case "031":
                  var ordr031 = Ordr031Bs.Current as Data.Order;
                  if (ordr031 == null) return;

                  var prod031 = RbprBs.Current as Data.Robot_Product;
                  if (prod031 == null) return;

                  var ordt031 = Ordt031Bs.List.OfType<Data.Order_Detail>().FirstOrDefault(od => od.Order == ordr031 && od.TARF_CODE == prod031.TARF_CODE);
                  if (ordt031 == null)
                  {
                     ordt031 = Ordt031Bs.AddNew() as Data.Order_Detail;
                     ordt031.Order = ordr031;
                     ordt031.TARF_CODE = prod031.TARF_CODE;
                     ordt031.NUMB = 1;
                     ordt031.OFF_PRCT = 0;
                     ordt031.DSCN_AMNT_DNRM = 0;

                     iRoboTech.Order_Details.InsertOnSubmit(ordt031);
                  }
                  else
                  {
                     ordt031.NUMB++;
                  }
                  break;
               case "032":
                  var ordr032 = Ordr032Bs.Current as Data.Order;
                  if (ordr032 == null) return;

                  var prod032 = RbprBs.Current as Data.Robot_Product;
                  if (prod032 == null) return;

                  var ordt032 = Ordt032Bs.List.OfType<Data.Order_Detail>().FirstOrDefault(od => od.Order == ordr032 && od.TARF_CODE == prod032.TARF_CODE);
                  if (ordt032 == null)
                  {
                     ordt032 = Ordt032Bs.AddNew() as Data.Order_Detail;
                     ordt032.Order = ordr032;
                     ordt032.TARF_CODE = prod032.TARF_CODE;
                     ordt032.NUMB = 1;
                     ordt032.OFF_PRCT = 0;
                     ordt032.DSCN_AMNT_DNRM = 0;

                     iRoboTech.Order_Details.InsertOnSubmit(ordt032);
                  }
                  else
                  {
                     ordt032.NUMB++;
                  }
                  break;
               case "033":
                  var ordr033 = Ordr033Bs.Current as Data.Order;
                  if (ordr033 == null) return;

                  var prod033 = RbprBs.Current as Data.Robot_Product;
                  if (prod033 == null) return;

                  var ordt033 = Ordt033Bs.List.OfType<Data.Order_Detail>().FirstOrDefault(od => od.Order == ordr033 && od.TARF_CODE == prod033.TARF_CODE);
                  if (ordt033 == null)
                  {
                     ordt033 = Ordt029Bs.AddNew() as Data.Order_Detail;
                     ordt033.Order = ordr033;
                     ordt033.TARF_CODE = prod033.TARF_CODE;
                     ordt033.NUMB = 1;
                     ordt033.OFF_PRCT = 0;
                     ordt033.DSCN_AMNT_DNRM = 0;

                     iRoboTech.Order_Details.InsertOnSubmit(ordt033);
                  }
                  else
                  {
                     ordt033.NUMB++;
                  }
                  break;
               case "034":
                  var ordr034 = Ordr034Bs.Current as Data.Order;
                  if (ordr034 == null) return;

                  var prod034 = RbprBs.Current as Data.Robot_Product;
                  if (prod034 == null) return;

                  var ordt034 = Ordt034Bs.List.OfType<Data.Order_Detail>().FirstOrDefault(od => od.Order == ordr034 && od.TARF_CODE == prod034.TARF_CODE);
                  if (ordt034 == null)
                  {
                     ordt034 = Ordt029Bs.AddNew() as Data.Order_Detail;
                     ordt034.Order = ordr034;
                     ordt034.TARF_CODE = prod034.TARF_CODE;
                     ordt034.NUMB = 1;
                     ordt034.OFF_PRCT = 0;
                     ordt034.DSCN_AMNT_DNRM = 0;

                     iRoboTech.Order_Details.InsertOnSubmit(ordt034);
                  }
                  else
                  {
                     ordt034.NUMB++;
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

      private void LevlCodeOrdrI_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var _tll = sender as TreeListLookUpEdit;
            if (_tll == null) return;

            switch (_tll.Tag.ToString())
            {
               case "029":
                  Sral029_Gv.ActiveFilterString = string.Format("[APBS_CODE] == '{0}'", e.NewValue);
                  var _sral029 = SralBs.List.OfType<Data.Service_Robot_Account_Link>().Where(a => a.APBS_CODE == (long)e.NewValue);
                  if (_sral029.Count() == 1)
                     Srbt029_Lov.EditValue = _sral029.FirstOrDefault().SRBT_SERV_FILE_NO;
                  else
                     Srbt029_Lov.Focus();
                  break;
               case "030":
                  Sral030_Gv.ActiveFilterString = string.Format("[APBS_CODE] == '{0}'", e.NewValue);
                  var _sral030 = SralBs.List.OfType<Data.Service_Robot_Account_Link>().Where(a => a.APBS_CODE == (long)e.NewValue);
                  if (_sral030.Count() == 1)
                     Srbt030_Lov.EditValue = _sral030.FirstOrDefault().SRBT_SERV_FILE_NO;
                  else
                     Srbt030_Lov.Focus();
                  break;
               case "031":
                  Sral031_Gv.ActiveFilterString = string.Format("[APBS_CODE] == '{0}'", e.NewValue);
                  var _sral031 = SralBs.List.OfType<Data.Service_Robot_Account_Link>().Where(a => a.APBS_CODE == (long)e.NewValue);
                  if (_sral031.Count() == 1)
                     Srbt031_Lov.EditValue = _sral031.FirstOrDefault().SRBT_SERV_FILE_NO;
                  else
                     Srbt031_Lov.Focus();
                  break;
               case "032":
                  Sral032_Gv.ActiveFilterString = string.Format("[APBS_CODE] == '{0}'", e.NewValue);
                  var _sral032 = SralBs.List.OfType<Data.Service_Robot_Account_Link>().Where(a => a.APBS_CODE == (long)e.NewValue);
                  if (_sral032.Count() == 1)
                     Srbt032_Lov.EditValue = _sral032.FirstOrDefault().SRBT_SERV_FILE_NO;
                  else
                     Srbt032_Lov.Focus();
                  break;
               case "033":
                  Sral033_Gv.ActiveFilterString = string.Format("[APBS_CODE] == '{0}'", e.NewValue);
                  var _sral033 = SralBs.List.OfType<Data.Service_Robot_Account_Link>().Where(a => a.APBS_CODE == (long)e.NewValue);
                  if (_sral033.Count() == 1)
                     Srbt033_Lov.EditValue = _sral033.FirstOrDefault().SRBT_SERV_FILE_NO;
                  else
                     Srbt033_Lov.Focus();
                  break;
               case "034":
                  Sral034_Gv.ActiveFilterString = string.Format("[APBS_CODE] == '{0}'", e.NewValue);
                  var _sral034 = SralBs.List.OfType<Data.Service_Robot_Account_Link>().Where(a => a.APBS_CODE == (long)e.NewValue);
                  if (_sral034.Count() == 1)
                     Srbt034_Lov.EditValue = _sral034.FirstOrDefault().SRBT_SERV_FILE_NO;
                  else
                     Srbt034_Lov.Focus();
                  break;
               default:
                  break;
            }
         }
         catch
         {
            Sral033_Gv.ActiveFilterString = "";
         }
      }

      private void AddOdstI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tsb = (ToolStripButton)sender;

            switch (_tsb.Tag.ToString())
            {
               case "029":
                  var ordr029 = Ordr029Bs.Current as Data.Order;
                  if(ordr029 == null)return;

                  if (Odst029Bs.List.OfType<Data.Order_State>().Any(od => od.CODE == 0)) return;
                  var odst029 = Odst029Bs.AddNew() as Data.Order_State;
                  odst029.Order = ordr029;
                  odst029.STAT_DATE = DateTime.Now;
                  odst029.CONF_STAT = "002";                  

                  iRoboTech.Order_States.InsertOnSubmit(odst029);
                  break;
               case "030":
                  var ordr030 = Ordr030Bs.Current as Data.Order;
                  if (ordr030 == null) return;

                  if (Odst030Bs.List.OfType<Data.Order_State>().Any(od => od.CODE == 0)) return;
                  var odst030 = Odst030Bs.AddNew() as Data.Order_State;
                  odst030.Order = ordr030;
                  odst030.STAT_DATE = DateTime.Now;
                  odst030.CONF_STAT = "002";

                  iRoboTech.Order_States.InsertOnSubmit(odst030);
                  break;
               case "031":
                  var ordr031 = Ordr031Bs.Current as Data.Order;
                  if (ordr031 == null) return;

                  if (Odst031Bs.List.OfType<Data.Order_State>().Any(od => od.CODE == 0)) return;
                  var odst031 = Odst031Bs.AddNew() as Data.Order_State;
                  odst031.Order = ordr031;
                  odst031.STAT_DATE = DateTime.Now;
                  odst031.CONF_STAT = "002";

                  iRoboTech.Order_States.InsertOnSubmit(odst031);
                  break;
               case "032":
                  var ordr032 = Ordr032Bs.Current as Data.Order;
                  if (ordr032 == null) return;

                  if (Odst032Bs.List.OfType<Data.Order_State>().Any(od => od.CODE == 0)) return;
                  var odst032 = Odst032Bs.AddNew() as Data.Order_State;
                  odst032.Order = ordr032;
                  odst032.STAT_DATE = DateTime.Now;
                  odst032.CONF_STAT = "002";

                  iRoboTech.Order_States.InsertOnSubmit(odst032);
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

      private void DelOdstI_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _tsb = (ToolStripButton)sender;
            if (_tsb == null) return;

            switch (_tsb.Tag.ToString())
            {
               case "029":
                  var odst029 = Odst029Bs.Current as Data.Order_State;
                  if (odst029 == null) return;

                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                  iRoboTech.DEL_ODST_P(odst029.ORDR_CODE, odst029.CODE);
                  break;
               case "030":
                  var odst030 = Odst030Bs.Current as Data.Order_State;
                  if (odst030 == null) return;

                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                  iRoboTech.DEL_ODST_P(odst030.ORDR_CODE, odst030.CODE);
                  break;
               case "031":
                  var odst031 = Odst031Bs.Current as Data.Order_State;
                  if (odst031 == null) return;

                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                  iRoboTech.DEL_ODST_P(odst031.ORDR_CODE, odst031.CODE);
                  break;
               case "032":
                  var odst032 = Odst032Bs.Current as Data.Order_State;
                  if (odst032 == null) return;

                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                  iRoboTech.DEL_ODST_P(odst032.ORDR_CODE, odst032.CODE);
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
      #endregion      

   }
}
