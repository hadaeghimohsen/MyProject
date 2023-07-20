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
   public partial class BANK_DVLP_F : UserControl
   {
      public BANK_DVLP_F()
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
         int rcba = RcbaBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         RcbaBs.Position = rcba;

         requery = false;
      }

      private void RcbaBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rcba = RcbaBs.Current as Data.Robot_Card_Bank_Account;
            if (rcba == null) return;

            //if(rcba.ACNT_TYPE.In("001", "003"))
            //{
            //   SaveBankActn_Butn.Enabled = DelBankAcnt_Butn.Enabled = ActvBankAcnt_Butn.Enabled = AcntDesc1_Txt.Enabled = IdPayAdrs1_Txt.Enabled = false;
            //}
            //else
            //{
            //   SaveBankActn_Butn.Enabled = DelBankAcnt_Butn.Enabled = ActvBankAcnt_Butn.Enabled = AcntDesc1_Txt.Enabled = IdPayAdrs1_Txt.Enabled = true;
            //}

            SaveBankActn_Butn.Enabled = DelBankAcnt_Butn.Enabled = ActvBankAcnt_Butn.Enabled = AcntDesc1_Txt.Enabled = IdPayAdrs1_Txt.Enabled = rcba.ACNT_TYPE.In("001", "003") ? false : true;
            SrbtLinkAcnt_Butn.Enabled = rcba.ACNT_TYPE.In("001", "002") ? true : false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddBankAcnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if(CardNumb_Txt.Text.Length != 16)
            {
               throw new Exception("شماره کارت بانکی به درستی وارد نشده");
            }

            if(ShbaNumb_Txt.Text.Length != 26)
            {
               throw new Exception("شماره شبا بانکی به درستی وارد نشده");
            }

            if(AcntOwnr_Txt.Text.Length == 0)
            {
               throw new Exception("اطلاعات دارنده حساب وارد نشده");
            }

            if (OrdrType_Lov.EditValue == null)
            {
               throw new Exception("نوع واریزی رات مشخص نکرده اید");
            }

            if(!IdpyAdrs_Txt.Text.Length.IsBetween(5, 30))
            {
               throw new Exception("شناسه واریز باید بین 5 تا 30 کاراکتر باشد");
            }

            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if(!iRoboTech.Robot_Card_Bank_Accounts.Any(a => a.ACNT_TYPE == "002" && a.ACNT_STAT == "002" && a.ORDR_TYPE == OrdrType_Lov.EditValue.ToString()))
            {
               // اگر حساب فعالی نداشته باشیم
               iRoboTech.INS_RCBA_P(robo.RBID, CardNumb_Txt.Text, ShbaNumb_Txt.Text, "002", AcntOwnr_Txt.Text, AcntDesc_Txt.Text, OrdrType_Lov.EditValue.ToString(), "002", IdpyAdrs_Txt.Text);
            }
            else
            {
               // اگر حساب فعال داشته باشیم
               iRoboTech.INS_RCBA_P(robo.RBID, CardNumb_Txt.Text, ShbaNumb_Txt.Text, "002", AcntOwnr_Txt.Text, AcntDesc_Txt.Text, OrdrType_Lov.EditValue.ToString(), "001", IdpyAdrs_Txt.Text);
            }

            // Empty TextBox
            CardNumb_Txt.Text = ShbaNumb_Txt.Text = AcntOwnr_Txt.Text = AcntDesc_Txt.Text = "";

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

      private void SaveBankActn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Rcba_Gv.PostEditor();

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

      private void DelBankAcnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف حساب بانکی موافق هستین؟", "حذف حساب", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            
            var rcba = RcbaBs.Current as Data.Robot_Card_Bank_Account;
            if (rcba == null) return;

            iRoboTech.DEL_RCBA_P(rcba.CODE);

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

      private void ActvBankAcnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با فعال کردن حساب بانکی موافق هستین؟", "فعال سازی حساب", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var rcba = RcbaBs.Current as Data.Robot_Card_Bank_Account;
            if (rcba == null) return;

            if (rcba.ACNT_STAT == "002") return;

            RcbaBs.List.OfType<Data.Robot_Card_Bank_Account>().Where(ba => ba.ACNT_TYPE == "002" && ba.ORDR_TYPE == rcba.ORDR_TYPE).ToList().ForEach(ba => ba.ACNT_STAT = "001");

            rcba.ACNT_STAT = "002";

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

      private void IdPayOpenSite_Butn_Click(object sender, EventArgs e)
      {
         Process.Start("https://idpay.ir/user/auth");
      }

      private void SrbtLinkAcnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rcba = RcbaBs.Current as Data.Robot_Card_Bank_Account;
            if (rcba == null) return;

            if(!SrcbBs.List.OfType<Data.Service_Robot_Card_Bank>().Any(a => a.SRBT_ROBO_RBID == rcba.ROBO_RBID && a.ACNT_TYPE_DNRM == rcba.ACNT_TYPE && a.ORDR_TYPE_DNRM == rcba.ORDR_TYPE))
            {
               var srcb = SrcbBs.AddNew() as Data.Service_Robot_Card_Bank;
               var srbt = 
                  iRoboTech.Service_Robots
                  .FirstOrDefault(sr => 
                     sr.Robot == rcba.Robot &&
                     (rcba.ACNT_TYPE == "001" && sr.CELL_PHON == "09033927103" && sr.NATL_CODE == "2372499424") ||
                     (
                        rcba.ACNT_TYPE == "002" && (sr.ROBO_RBID == rcba.ROBO_RBID && sr.ROBO_RBID == 401 && sr.Service_Robot_Groups.Any(g => g.GROP_GPID == 131 && g.STAT == "002") ||
                                                     sr.ROBO_RBID == rcba.ROBO_RBID && sr.ROBO_RBID == 391 && sr.Service_Robot_Groups.Any(g => g.GROP_GPID == 122 && g.STAT == "002"))
                     )
                  );
               srcb.Service_Robot = srbt;
               srcb.Robot_Card_Bank_Account = rcba;

               iRoboTech.Service_Robot_Card_Banks.InsertOnSubmit(srcb);
               iRoboTech.SubmitChanges();
               requery = true;
            }
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

      private void DupBankAcnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rcba = RcbaBs.Current as Data.Robot_Card_Bank_Account;
            if (_rcba == null) return;

            if (_rcba.ACNT_TYPE.In("001", "003")) { MessageBox.Show("لطفا حساب خود را درست انتخاب کنید"); return; }

            if (CardNumb_Txt.Text.Length != 16)
            {
               throw new Exception("شماره کارت بانکی به درستی وارد نشده");
            }

            if (ShbaNumb_Txt.Text.Length != 26)
            {
               throw new Exception("شماره شبا بانکی به درستی وارد نشده");
            }

            if (AcntOwnr_Txt.Text.Length == 0)
            {
               throw new Exception("اطلاعات دارنده حساب وارد نشده");
            }

            iRoboTech.ExecuteCommand(
               string.Format(
                  "INSERT INTO dbo.Robot_Card_Bank_Account (Robo_Rbid, Code, Card_Numb, Shba_Numb, Acnt_Type, Acnt_Ownr, Acnt_Desc, Ordr_Type, Acnt_Stat)" + Environment.NewLine +
                  "SELECT Robo_Rbid, dbo.GNRT_NVID_U(), '{2}', '{3}', Acnt_Type, N'{4}', Acnt_Desc, Ordr_Type, '001'" + Environment.NewLine +
                  "FROM dbo.Robot_Card_Bank_Account a" + Environment.NewLine + 
                  "WHERE a.Acnt_Type = '002' AND a.Robo_Rbid = {0} AND a.Card_Numb = '{1}' AND" + Environment.NewLine +
                  "NOT EXISTS (SELECT * FROM dbo.Robot_Card_Bank_Account at WHERE at.Robo_Rbid = a.Robo_Rbid AND at.Acnt_Type = a.Acnt_Type AND at.Card_Numb = '{2}');",
                  _rcba.ROBO_RBID,
                  _rcba.CARD_NUMB,
                  CardNumb_Txt.EditValue,
                  ShbaNumb_Txt.EditValue,
                  AcntOwnr_Txt.EditValue
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }      
   }
}
