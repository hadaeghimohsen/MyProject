using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.JobRouting.Jobs;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SoftVersionControl : UserControl
   {
      public SoftVersionControl()
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
         if (allrunning || tb_master.SelectedTab == tp_001)
         {
            int subsys_indx = SubSysBs1.Position;
            int pakg_indx = PakgBs1.Position;
            int pkac_indx = PkacBs1.Position;
            int pkin_indx = PkinBs1.Position;
            int piug_indx = PiugBs1.Position;
            
            SubSysBs1.DataSource = iProject.Sub_Systems;

            SubSysBs1.Position = subsys_indx;
            PakgBs1.Position = pakg_indx;
            PkacBs1.Position = pkac_indx;
            PkinBs1.Position = pkin_indx;
            PiugBs1.Position = piug_indx;

            PIUG1.ActiveFilterString = "STAT = '002'";
            PKIN1.ActiveFilterString = "STAT = '002'";
         }
         else if (allrunning || tb_master.SelectedTab == tp_002)
         {

         }
      }

      private void SubSysBs1_CurrentChanged(object sender, EventArgs e)
      {
         var subs = SubSysBs1.Current as Data.Sub_System;
         SsitBs1.DataSource = iProject.Sub_System_Items.Where(ssit => ssit.SUB_SYS == subs.SUB_SYS);
      }

      private void PakgGv_Nb_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن بسته مورد نظر موافقید؟", "حذف بسته", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "001"),
                        new XElement("Delete",
                           new XElement("Package",
                              new XAttribute("subsys", (PakgBs1.Current as Data.Package).SUB_SYS),
                              new XAttribute("code", (PakgBs1.Current as Data.Package).CODE)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  PakgBs1.EndEdit();
                  var crntpakg = PakgBs1.Current as Data.Package;
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "001"),
                        PakgBs1.List.OfType<Data.Package>().Where(c => c.CRET_BY == null).Select(c =>
                           new XElement("Insert",
                              new XElement("Package",
                                 new XAttribute("subsys", c.SUB_SYS),
                                 new XAttribute("code", c.CODE),
                                 new XAttribute("name", c.NAME)
                              )
                           )
                        ),
                        crntpakg.CRET_BY != "" ?
                           new XElement("Update",
                              new XElement("Package",
                                 new XAttribute("subsys", crntpakg.SUB_SYS),
                                 new XAttribute("code", crntpakg.CODE),
                                 new XAttribute("name", crntpakg.NAME)
                              )
                          ) : new XElement("Update")

                     )
                  );
                  requery = true;
                  break;
            }
         }
         catch (SqlException se)
         {
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               case 547:
                  MessageBox.Show("زمان پایان کلاس باید از زمان شروع بزرگتر باشید");
                  break;
               default:
                  MessageBox.Show(se.Message);
                  break;
            }
         }
         finally
         {
            if (requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void PkacGv_Nb_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن آیتم بسته مورد نظر موافقید؟", "حذف آیتم بسته", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "002"),
                        new XElement("Delete",
                           new XElement("Package_Activity",
                              new XAttribute("pakgsubsys", (PkacBs1.Current as Data.Package_Activity).PAKG_SUB_SYS),
                              new XAttribute("pakgcode", (PkacBs1.Current as Data.Package_Activity).PAKG_CODE),
                              new XAttribute("ssitrwno", (PkacBs1.Current as Data.Package_Activity).SSIT_RWNO)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  PkacBs1.EndEdit();
                  var crntpkac = PkacBs1.Current as Data.Package_Activity;
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "002"),
                        PkacBs1.List.OfType<Data.Package_Activity>().Where(c => c.CRET_BY == null).Select(c =>
                           new XElement("Insert",
                              new XElement("Package_Activity",
                                 new XAttribute("pakgsubsys", c.PAKG_SUB_SYS),
                                 new XAttribute("pakgcode", c.PAKG_CODE),
                                 new XAttribute("ssitrwno", c.SSIT_RWNO),
                                 new XAttribute("stat", c.STAT ?? "002")
                              )
                           )
                        ),
                        crntpkac.CRET_BY != "" ?
                           new XElement("Update",
                              new XElement("Package_Activity",
                                 new XAttribute("pakgsubsys", crntpkac.PAKG_SUB_SYS),
                                 new XAttribute("pakgcode", crntpkac.PAKG_CODE),
                                 new XAttribute("ssitrwno", crntpkac.SSIT_RWNO),
                                 new XAttribute("stat", crntpkac.STAT ?? "002")
                              )
                          ) : new XElement("Update")

                     )
                  );
                  requery = true;
                  break;
            }
         }
         catch (SqlException se)
         {
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               case 547:
                  MessageBox.Show("زمان پایان کلاس باید از زمان شروع بزرگتر باشید");
                  break;
               default:
                  MessageBox.Show(se.Message);
                  break;
            }
         }
         finally
         {
            if (requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void PkinGv_Nb_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن تعداد نمونه مورد نظر موافقید؟", "حذف تعداد نمونه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "003"),
                        new XElement("Delete",
                           new XElement("Package_Instance",
                              new XAttribute("pakgsubsys", (PkinBs1.Current as Data.Package_Instance).PAKG_SUB_SYS),
                              new XAttribute("pakgcode", (PkinBs1.Current as Data.Package_Instance).PAKG_CODE),
                              new XAttribute("rwno", (PkinBs1.Current as Data.Package_Instance).RWNO)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  PkinBs1.EndEdit();
                  var crnt = PkinBs1.Current as Data.Package_Instance;
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "003"),
                        PkinBs1.List.OfType<Data.Package_Instance>().Where(c => c.CRET_BY == null).Select(c =>
                           new XElement("Insert",
                              new XElement("Package_Instance",
                                 new XAttribute("pakgsubsys", c.PAKG_SUB_SYS),
                                 new XAttribute("pakgcode", c.PAKG_CODE),
                                 new XAttribute("rwno", c.RWNO),
                                 new XAttribute("instdesc", c.INST_DESC),
                                 new XAttribute("stat", c.STAT ?? "002")
                              )
                           )
                        ),
                        crnt.CRET_BY != "" ?
                           new XElement("Update",
                              new XElement("Package_Instance",
                                 new XAttribute("pakgsubsys", crnt.PAKG_SUB_SYS),
                                 new XAttribute("pakgcode", crnt.PAKG_CODE),
                                 new XAttribute("rwno", crnt.RWNO),
                                 new XAttribute("instdesc", crnt.INST_DESC),
                                 new XAttribute("stat", crnt.STAT ?? "002")
                              )
                          ) : new XElement("Update")

                     )
                  );
                  requery = true;
                  break;
            }
         }
         catch (SqlException se)
         {
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               case 547:
                  MessageBox.Show("زمان پایان کلاس باید از زمان شروع بزرگتر باشید");
                  break;
               default:
                  MessageBox.Show(se.Message);
                  break;
            }
         }
         finally
         {
            if (requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void PiugGv_Nb_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن تعداد نمونه مورد نظر موافقید؟", "حذف تعداد نمونه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "004"),
                        new XElement("Delete",
                           new XElement("Package_Instance_User_Gateway",
                              new XAttribute("pkinpakgsubsys", (PiugBs1.Current as Data.Package_Instance_User_Gateway).PKIN_PAKG_SUB_SYS),
                              new XAttribute("pkinpakgcode", (PiugBs1.Current as Data.Package_Instance_User_Gateway).PKIN_PAKG_CODE),
                              new XAttribute("pkinrwno", (PiugBs1.Current as Data.Package_Instance_User_Gateway).PKIN_RWNO),
                              new XAttribute("usgwgtwymacadrs", (PiugBs1.Current as Data.Package_Instance_User_Gateway).USGW_GTWY_MAC_ADRS),
                              new XAttribute("usgwuserid", (PiugBs1.Current as Data.Package_Instance_User_Gateway).USGW_USER_ID),
                              new XAttribute("usgwrwno", (PiugBs1.Current as Data.Package_Instance_User_Gateway).USGW_RWNO)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  PiugBs1.EndEdit();
                  var crnt = PiugBs1.Current as Data.Package_Instance_User_Gateway;
                  
                  iProject.SaveVersionControl(
                     new XElement("Config",
                        new XAttribute("type", "004"),
                        PiugBs1.List.OfType<Data.Package_Instance_User_Gateway>().Where(c => c.CRET_BY == null).Select(c =>
                           new XElement("Insert",
                              new XElement("Package_Instance_User_Gateway",
                                 new XAttribute("pkinpakgsubsys", c.PKIN_PAKG_SUB_SYS),
                                 new XAttribute("pkinpakgcode", c.PKIN_PAKG_CODE),
                                 new XAttribute("pkinrwno", c.PKIN_RWNO),
                                 new XAttribute("usgwgtwymacadrs", c.USGW_GTWY_MAC_ADRS),
                                 new XAttribute("usgwuserid", c.USGW_USER_ID),
                                 new XAttribute("usgwrwno", iProject.User_Gateways.Where(ug => ug.GTWY_MAC_ADRS == c.USGW_GTWY_MAC_ADRS && ug.USER_ID == c.USGW_USER_ID).Max(ug => ug.RWNO)),
                                 new XAttribute("stat", c.STAT ?? "002")
                              )
                           )
                        ),
                        crnt.CRET_BY != "" ?
                           new XElement("Update",
                              new XElement("Package_Instance_User_Gateway",
                                 new XAttribute("pkinpakgsubsys", crnt.PKIN_PAKG_SUB_SYS),
                                 new XAttribute("pkinpakgcode", crnt.PKIN_PAKG_CODE),
                                 new XAttribute("pkinrwno", crnt.PKIN_RWNO),
                                 new XAttribute("usgwgtwymacadrs", crnt.USGW_GTWY_MAC_ADRS),
                                 new XAttribute("usgwuserid", crnt.USGW_USER_ID),
                                 new XAttribute("usgwrwno", crnt.USGW_RWNO),
                                 new XAttribute("stat", crnt.STAT ?? "002")
                              )
                          ) : new XElement("Update")

                     )
                  );
                  requery = true;
                  break;
            }
         }
         catch (SqlException se)
         {
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               case 547:
                  MessageBox.Show("زمان پایان کلاس باید از زمان شروع بزرگتر باشید");
                  break;
               default:
                  MessageBox.Show(se.Message);
                  break;
            }
         }
         finally
         {
            if (requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }
   }
}
