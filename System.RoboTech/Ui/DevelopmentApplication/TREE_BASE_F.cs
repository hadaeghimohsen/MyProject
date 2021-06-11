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
   public partial class TREE_BASE_F : UserControl
   {
      public TREE_BASE_F()
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

         int trbs = TrbsBs.Position;
         int unit = UnitBs.Position;
         int rbsc = RbscBs.Position;
         int rbss = RbssBs.Position;
         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int srbt = SrbtBs.Position;
         int sral = SralBs.Position;

         TrbsBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "TREEBASEACCOUNT_INFO");
         UnitBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "PRODUCTUNIT_INFO");
         RbscBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "SECTION_INFO");
         RbssBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == "STORESPEC_INFO");
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         TrbsBs.Position = trbs;
         UnitBs.Position = unit;
         RbscBs.Position = rbsc;
         RbssBs.Position = rbss;
         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         SrbtBs.Position = srbt;
         SralBs.Position = sral;
         requery = false;
      }

      private void AddTrbs_Butn_Click(object sender, EventArgs e)
      {
         if (TrbsBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var trbs = TrbsBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(trbs);
         trbs.ENTY_NAME = "TREEBASEACCOUNT_INFO";
         trbs.LEVL_LENT = 1;
         trbs.RWNO = (TrbsBs.List.OfType<Data.App_Base_Define>().Where(t => t.REF_CODE == trbs.REF_CODE).Max(t => t.RWNO) ?? 0) + 1;
      }

      private void DelTrbs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var trbs = TrbsBs.Current as Data.App_Base_Define;
            if (trbs == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;

            iRoboTech.DEL_APBS_P(trbs.CODE);

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

      private void SaveTrbs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            TrbsBs.EndEdit();
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

      private void AddUnit_Butn_Click(object sender, EventArgs e)
      {
         if (UnitBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var unit = UnitBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(unit);
         unit.ENTY_NAME = "PRODUCTUNIT_INFO";
      }

      private void DelUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var unit = UnitBs.Current as Data.App_Base_Define;
            if (unit == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;
            iRoboTech.DEL_APBS_P(unit.CODE);

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

      private void SaveUnit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            UnitBs.EndEdit();
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

      private void RootTrbs_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var trbs = TrbsBs.Current as Data.App_Base_Define;
            if(trbs == null)return;

            if(e.Button.Index == 1)
            {
               RootTrbs_Lov.EditValue = trbs.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RootUnit_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var unit = UnitBs.Current as Data.App_Base_Define;
            if (unit == null) return;

            if (e.Button.Index == 1)
            {
               RootUnit_Lov.EditValue = unit.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddRbsc_Butn_Click(object sender, EventArgs e)
      {
         if (RbscBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var rbsc = RbscBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(rbsc);
         rbsc.ENTY_NAME = "SECTION_INFO";
      }

      private void DelRbsc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbsc = RbscBs.Current as Data.App_Base_Define;
            if (rbsc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;
            iRoboTech.DEL_APBS_P(rbsc.CODE);

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

      private void SaveRbsc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RbscBs.EndEdit();
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

      private void RootRbsc_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rbsc = RbscBs.Current as Data.App_Base_Define;
            if (rbsc == null) return;

            if (e.Button.Index == 1)
            {
               RootRbsc_Lov.EditValue = rbsc.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RootRbss_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rbss = RbssBs.Current as Data.App_Base_Define;
            if (rbss == null) return;

            if (e.Button.Index == 1)
            {
               RootRbss_Lov.EditValue = rbss.REF_CODE = null;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddRbss_Butn_Click(object sender, EventArgs e)
      {
         if (RbssBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;

         var rbss = RbssBs.AddNew() as Data.App_Base_Define;
         iRoboTech.App_Base_Defines.InsertOnSubmit(rbss);
         rbss.ENTY_NAME = "STORESPEC_INFO";
      }

      private void DelRbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rbss = RbssBs.Current as Data.App_Base_Define;
            if (rbss == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;
            iRoboTech.DEL_APBS_P(rbss.CODE);

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

      private void SaveRbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RbssBs.EndEdit();
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

      private void RootSrbt_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  //RootSrbt_Lov.EditValue = srbt.REF_CHAT_ID = null;
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

      private void AddSrbt_Butn_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         if (SrbtBs.List.OfType<Data.Service_Robot>().Any(sr => sr.SERV_FILE_NO == 0)) return;

         var srbt = SrbtBs.AddNew() as Data.Service_Robot;
         srbt.Robot = robo;
         srbt.REGS_TYPE = "002"; // ثبت دستی

         if (SrbtBs.List.OfType<Data.Service_Robot>().Count(sr => sr.CHAT_ID > 0) > 0)
            srbt.CHAT_ID = SrbtBs.List.OfType<Data.Service_Robot>().Where(sr => sr.CHAT_ID > 0).Max(sr => sr.CHAT_ID) + 1;
         else
            srbt.CHAT_ID = 1;

         iRoboTech.Service_Robots.InsertOnSubmit(srbt);
      }

      private void DelSrbt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaveSrbt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SrbtBs.EndEdit();

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

      private void RefChatId_Txt_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            srbt.REF_CHAT_ID = srbt.REF_SERV_FILE_NO = srbt.REF_ROBO_RBID = null;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddSral_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            if (SralBs.List.OfType<Data.Service_Robot_Account_Link>().Any(a => a.CODE == 0)) return;

            var sral = SralBs.AddNew() as Data.Service_Robot_Account_Link;
            sral.Service_Robot = srbt;

            iRoboTech.Service_Robot_Account_Links.InsertOnSubmit(sral);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelSral_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sral = SralBs.Current as Data.Service_Robot_Account_Link;
            if (sral == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            if(iRoboTech.Orders.Any(o => o.APBS_CODE == sral.CODE))

            iRoboTech.Service_Robot_Account_Links.DeleteOnSubmit(sral);

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

      private void SaveSral_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Sral_gv.PostEditor();

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
   }
}
