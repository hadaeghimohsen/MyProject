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
using System.Threading;

namespace System.RoboTech.Ui.Inspection
{
   public partial class MNGR_INSP_F : UserControl
   {
      public MNGR_INSP_F()
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
         int srbt = InspBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         InspBs.Position = srbt;

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _robo = RoboBs.Current as Data.Robot;
            if (_robo == null) return;

            InspBs.DataSource =
               iRoboTech.Service_Robots.Where(sr => sr.Robot == _robo && sr.Service_Robot_Groups.Any(g => g.GROP_GPID == 143));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SrbtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _srbt = InspBs.Current as Data.Service_Robot;
            if (_srbt == null) return;

            var _srgp = _srbt.Service_Robot_Groups.FirstOrDefault(g => g.GROP_GPID == 143);
            if (_srgp == null) return;

            switch (_srgp.STAT)
            {
               case "002":
                  GropStat_Lb.Text = "عضویت در گروه بازرسی فعال میباشد";
                  GropStat_Lb.BackColor = Color.YellowGreen;
                  break;
               case "001":
                  GropStat_Lb.Text = "عضویت در گروه بازرسی غیرفعال میباشد";
                  GropStat_Lb.BackColor = Color.Gray;
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveSrbt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>85</Privilege><Sub_Sys>12</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 85 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
            _DefaultGateway.Gateway(_InteractWithJob);

            Srbt_Gv.PostEditor();
            InspBs.EndEdit();

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

      private void ChngPswd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>86</Privilege><Sub_Sys>12</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 86 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
            _DefaultGateway.Gateway(_InteractWithJob);

            var _srbt = InspBs.Current as Data.Service_Robot;
            if (_srbt == null) return;

            if (_srbt.PASS_WORD != CrntPswd_Txt.Text) { CrntPswd_Txt.Focus(); return; }
            if (NewPwsd_Txt.Text != CnfmPswd_Txt.Text) { NewPwsd_Txt.Focus(); return; }

            _srbt.PASS_WORD = NewPwsd_Txt.Text;

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

      private void JoinGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>87</Privilege><Sub_Sys>12</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 87 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
            _DefaultGateway.Gateway(_InteractWithJob);

            var _srbt = InspBs.Current as Data.Service_Robot;
            if (_srbt == null) return;

            iRoboTech.ExecuteCommand(
               string.Format(
                  "Merge Service_Robot_Group T" + Environment.NewLine + 
                  "Using (SELECT {0} AS Chat_Id ) S ON (T.Chat_Id = S.Chat_Id AND T.Srbt_Robo_Rbid = {1} AND T.Grop_Gpid = 143)" + Environment.NewLine + 
                  "WHEN NOT MATCHED THEN INSERT (Srbt_Serv_File_No, Srbt_Robo_Rbid, Grop_Gpid) VALUES ({2}, {1}, 143)" + Environment.NewLine + 
                  "WHEN MATCHED THEN UPDATE SET T.STAT = CASE T.Stat WHEN '001' THEN '002' ELSE '001' END;"
                  , _srbt.CHAT_ID
                  , _srbt.ROBO_RBID
                  , _srbt.SERV_FILE_NO
               )
            );

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

      private void NewInsp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _robo = RoboBs.Current as Data.Robot;
            if (_robo == null) return;

            if (iNewPswd_Txt.Text != iCnfmPswd_Txt.Text) { iNewPswd_Txt.Focus(); return; }

            if (iRoboTech.Service_Robots.Any(sr => sr.ROBO_RBID == _robo.RBID && sr.NATL_CODE == NatlCode_Txt.Text)) { NatlCode_Txt.Focus(); return; }

            iRoboTech.INS_SRBT_V3P(
               new XElement("Request",
                   new XAttribute("rbid", _robo.RBID),
                   new XElement("Service", 
                       new XAttribute("cellphon", CellPhon_Txt.Text),
                       new XAttribute("natlcode", NatlCode_Txt.Text),
                       new XAttribute("password", iNewPswd_Txt.Text),
                       new XAttribute("frstname", FrstName_Txt.Text),
                       new XAttribute("lastname", LastName_Txt.Text),
                       new XAttribute("servcode", ServCode_Txt.Text),
                       new XAttribute("idtynumb", IdtyNumb_Txt.Text)
                   )
               )
            );

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

      private void SaveSlerShop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SlerShop_Gv.PostEditor();
            SlerShopBs.EndEdit();

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

      private void SaveSler_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Sler_Gv.PostEditor();
            SlerBs.EndEdit();

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

      private void SlerShopBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _sler = SlerBs.Current as Data.Service_Robot;
            if (_sler == null) return;

            var _ordr036 = iRoboTech.Orders.Where(o => o.ORDR_TYPE == "036" && o.ORDR_STAT != "003" && o.CHAT_ID == _sler.CHAT_ID);
            int _erorOrdr036 = _ordr036.Where(o => o.Order_Tags.Any()).Count();

            ErorOrdr036_Lnk.Text = _erorOrdr036.ToString();
            NormOrdr036_Lnk.Text = (_ordr036.Count() - _erorOrdr036).ToString();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Ordr036Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _ordr = Ordr036Bs.Current as Data.Order;
            if (_ordr == null) return;

            RegnBs.DataSource = iRoboTech.Regions.Where(rg => rg.PRVN_CODE == _ordr.Service_Robot.REGN_PRVN_CODE && rg.STAT == "002");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Find_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _robo = RoboBs.Current as Data.Robot;
            if (_robo == null) return;

            FromOrdrDate_Dt.CommitChanges();
            ToOrdrDate_Dt.CommitChanges();

            DateTime? _fromordrdate = DateTime.Now, _toordrdate = DateTime.Now;
            if (FromOrdrDate_Cbx.Checked && !FromOrdrDate_Dt.Value.HasValue) { FromOrdrDate_Dt.Value = DateTime.Now; FromOrdrDate_Dt.Focus(); }
            if (FromOrdrDate_Cbx.Checked && FromOrdrDate_Dt.Value.HasValue) { _fromordrdate = FromOrdrDate_Dt.Value.Value.Date; }
            if (ToOrdrDate_Cbx.Checked && !ToOrdrDate_Dt.Value.HasValue) { ToOrdrDate_Dt.Value = DateTime.Now; ToOrdrDate_Dt.Focus(); }
            if (ToOrdrDate_Cbx.Checked && ToOrdrDate_Dt.Value.HasValue) { _toordrdate = ToOrdrDate_Dt.Value.Value.Date; }

            List<long> _otags = new List<long>();
            foreach (var i in OTag_Gv.GetSelectedRows())
            {
               _otags.Add((OTag_Gv.GetRow(i) as Data.App_Base_Define).CODE);
                  ;
            }

            Ordr036Bs.DataSource =
               iRoboTech.Orders.Where(o =>
                  o.ORDR_TYPE == "036" &&
                  o.ORDR_STAT == "004" &&
                  o.SRBT_ROBO_RBID == _robo.RBID &&
                  (
                     BOrdr36_Rb.Checked || 
                     (NOrdr36_Rb.Checked && !o.Service_Robot_Seller_Inspection_Units.Any()) ||
                     (EOrdr36_Rb.Checked && o.Service_Robot_Seller_Inspection_Units.Any()) 
                  ) &&
                  
                  (!FromOrdrDate_Cbx.Checked || o.STRT_DATE >= _fromordrdate) &&
                  (!ToOrdrDate_Cbx.Checked || o.STRT_DATE <= _toordrdate) &&
                  
                  (!InspName_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller_Inspection_Unit_Inspectors.Any(ii => ii.Service_Robot.NAME.Contains(InspName_Txt.Text)))) &&
                  (!InspNatlCode_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller_Inspection_Unit_Inspectors.Any(ii => ii.Service_Robot.NATL_CODE.Contains(InspNatlCode_Txt.Text)))) &&
                  (!InspServCode_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller_Inspection_Unit_Inspectors.Any(ii => ii.Service_Robot.SERV_CODE.Contains(InspServCode_Txt.Text)))) &&
                  (!InspCell_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller_Inspection_Unit_Inspectors.Any(ii => ii.Service_Robot.CELL_PHON.Contains(InspCell_Txt.Text)))) &&

                  (!ShopName_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller.SHOP_NAME.Contains(ShopName_Txt.Text))) &&
                  (!ShopZipCode_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller.SHOP_ZIP_CODE.Contains(ShopZipCode_Txt.Text))) &&
                  (!ShopIsicNumb_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller.SHOP_ISIC_NUMB.Contains(ShopIsicNumb_Txt.Text))) &&

                  (!SlerName_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller.Service_Robot.NAME.Contains(SlerName_Txt.Text))) &&
                  (!SlerNatlCode_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller.Service_Robot.NATL_CODE.Contains(SlerNatlCode_Txt.Text))) &&
                  (!SlerCell_Cbx.Checked || o.Service_Robot_Seller_Inspection_Units.Any(i => i.Service_Robot_Seller.Service_Robot.CELL_PHON.Contains(SlerCell_Txt.Text))) &&

                  (!OTag_Cbx.Checked || o.Order_Tags.Any(ot => _otags.Contains(ot.CODE)))
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }      
   }
}
