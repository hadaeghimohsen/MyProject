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
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SecurityManagment : UserControl
   {
      public SecurityManagment()
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

      private void Execute_Query(bool allrunning)
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         if (tb_master.SelectedTab == tp_001 || allrunning)
         {
            ScmgBs1.DataSource = iProject.Security_Managments;
         }
         if (tb_master.SelectedTab == tp_002 || allrunning)
         {
            // کامپیوتر های تایید شده و مجاز برای کار
            GtwyBs2.DataSource = iProject.Gateways.Where(g => g.CONF_STAT == "002" && g.VALD_TYPE_DNRM == "002" && g.AUTH_TYPE_DNRM == "002");
         }
         if (tb_master.SelectedTab == tp_003 || allrunning)
         {
            // کامپیوتر های بلوکه شده
            GtwyBs3.DataSource = iProject.Gateways.Where(g => g.AUTH_TYPE_DNRM == "003");
         }
         if (tb_master.SelectedTab == tp_004 || allrunning)
         {
            // کامپیوتر های غیرمجاز
            GtwyBs4.DataSource = iProject.Gateways.Where(g => g.AUTH_TYPE_DNRM == "001"); ;
         }
      }

      private void GtwyBs2_CurrentChanged(object sender, EventArgs e)
      {
         if (GtwyBs2.Current == null)
         {
            UsgwBs2.List.Clear();
            return;
         }
         var g = GtwyBs2.Current as Data.Gateway;

         UsgwBs2.DataSource = iProject.User_Gateways.Where(ug => ug.Gateway == g && ug.RWNO == iProject.User_Gateways.Where(ugt => ugt.Gateway == g && ugt.USER_ID == ug.USER_ID).Max(ugt => ugt.RWNO));
      }

      private void GtwyBs3_CurrentChanged(object sender, EventArgs e)
      {
         if (GtwyBs3.Current == null)
         {
            UsgwBs3.List.Clear();
            return;
         }
         var g = GtwyBs3.Current as Data.Gateway;

         UsgwBs3.DataSource = iProject.User_Gateways.Where(ug => ug.Gateway == g && ug.RWNO == iProject.User_Gateways.Where(ugt => ugt.Gateway == g && ugt.USER_ID == ug.USER_ID).Max(ugt => ugt.RWNO));
      }

      private void GtwyBs4_CurrentChanged(object sender, EventArgs e)
      {
         if (GtwyBs4.Current == null)
         {
            UsgwBs4.List.Clear();
            return;
         }
         var g = GtwyBs4.Current as Data.Gateway;

         UsgwBs4.DataSource = iProject.User_Gateways.Where(ug => ug.Gateway == g && ug.RWNO == iProject.User_Gateways.Where(ugt => ugt.Gateway == g && ugt.USER_ID == ug.USER_ID).Max(ugt => ugt.RWNO));
      }

      private void UsgwBs2_CurrentChanged(object sender, EventArgs e)
      {         
         var ug = UsgwBs2.Current as Data.User_Gateway;

         if (ug == null) return;

         if (ug.VALD_TYPE == "001")
            Tbn_ChngVldt2.ImageIndex = 0;
         else if (ug.VALD_TYPE == "002")
            Tbn_ChngVldt2.ImageIndex = 1;
      }

      private void Tbn_ChngVldt2_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UsgwBs2.Current as Data.User_Gateway;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "001"),
                  new XElement("User_Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS),
                     new XAttribute("userid", usgw.USER_ID)
                  )
               )
            );
            requery = true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {               
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Mb_SaveSecMan_Click(object sender, EventArgs e)
      {
         try
         {
            var sm = ScmgBs1.Current as Data.Security_Managment;
            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "000"),
                  new XElement("SecurityManagment",
                     new XAttribute("plcymacaddrfltr", sm.PLCY_MAC_ADDR_FLTR),
                     new XAttribute("plcycompblok", sm.PLCY_COMP_BLOK),
                     new XAttribute("logncompblok", sm.LOGN_COMP_BLOK),
                     new XAttribute("showalrmunat", sm.SHOW_ALRM_UNAT),
                     new XAttribute("postponeunat", sm.POST_PONE_UNAT),
                     new XAttribute("plcyforcsafeentr", sm.PLCY_FORC_SAFE_ENTR),
                     new XAttribute("plcysecrpswd", sm.PLCY_SECR_PSWD),
                     new XAttribute("pswdhistnumb", sm.PSWD_HIST_NUMB),
                     new XAttribute("maxpswdage", sm.MAX_PSWD_AGE),
                     new XAttribute("minpswdlen", sm.MIN_PSWD_LEN),
                     new XAttribute("plcypswdcmpx", sm.PLCY_PSWD_CMPX),
                     new XAttribute("usevpn", sm.USE_VPN)
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Tbn_ChngBlok2_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UsgwBs2.Current as Data.User_Gateway;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "002"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void Tbn_ChngUnat2_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UsgwBs2.Current as Data.User_Gateway;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "003"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void Tbn_ChngAuth3_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UsgwBs3.Current as Data.User_Gateway;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "004"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void Tbn_ChngUnat3_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UsgwBs3.Current as Data.User_Gateway;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "003"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void Tbn_ChngAuth4_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UsgwBs4.Current as Data.User_Gateway;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "004"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void Tbn_ChngBlok4_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UsgwBs4.Current as Data.User_Gateway;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "002"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query(true);
               requery = false;
            }
         }
      }

      private void CurrentHost_Chkbox_CheckedChanged(object sender, EventArgs e)
      {
         OtherHost_Pnl.Visible = !CurrentHost_Chkbox.Checked;
      }

      private void SaveHost4User_Butn_Click(object sender, EventArgs e)
      {
         XElement host = null;
         if(User_Lov.EditValue == null)
         {
            User_Lov.Focus();
            return;
         }

         if(!CurrentHost_Chkbox.Checked && Computer_Lov.EditValue == null)
         {
            Computer_Lov.Focus();
            return;
         }
         
         if(CurrentHost_Chkbox.Checked)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
               {
                  AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     host = (XElement)output;
                  })
               }
            );
         }
         else
         {
            host =
               new XElement("Computer",
                  new XAttribute("mac", ""),
                  new XAttribute("ip", ""),
                  new XAttribute("cpusnro", "")
               );
         }

         iProject.SaveHostInfo(host);
      }
   }
}
