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
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.Settings
{
   public partial class BAS_CPR_F : UserControl
   {
      public BAS_CPR_F()
      {
         InitializeComponent();
      }
      bool requery = default(bool);

      private void Execute_Query(bool runQueryAllTabPage)
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if(tc_master.SelectedTab == tp_001 || runQueryAllTabPage)
         {
            int _c = CntyBs1.Position;
            int _p = PrvnBs1.Position;
            int _r = RegnBs1.Position;
            CntyBs1.DataSource = iScsc.Countries;
            CntyBs1.Position = _c;
            PrvnBs1.Position = _p;
            RegnBs1.Position = _r;
         }
         if(tc_master.SelectedTab == tp_002 || runQueryAllTabPage)
         {
            int _c = CntyBs2.Position;
            int _p = PrvnBs2.Position;
            int _r = RegnBs2.Position;
            int _cb = ClubBs2.Position;
            int _cbmt = CbmtBs2.Position;
            CntyBs2.DataSource = iScsc.Countries;
            CntyBs2.Position = _c;
            PrvnBs2.Position = _p;
            RegnBs2.Position = _r;
            ClubBs2.Position = _cb;
            CbmtBs2.Position = _cbmt;
         }
         if(tc_master.SelectedTab == tp_003 || runQueryAllTabPage)
         {
            int _c = CntyBs2.Position;
            int _p = PrvnBs2.Position;
            int _r = RegnBs2.Position;
            int _cb = ClubBs3.Position;
            int _u = VUserBs2.Position;
            int _fur = FURgnBs3.Position;
            int _fuc = FUClbBs3.Position;
            CntyBs2.DataSource = iScsc.Countries;
            VUserBs2.DataSource = iScsc.V_Users;
            CntyBs2.Position = _c;
            PrvnBs2.Position = _p;
            RegnBs2.Position = _r;
            ClubBs3.Position = _cb;
            VUserBs2.Position = _u;
            FURgnBs3.Position = _fur;
            FUClbBs3.Position = _fuc;
         }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      #region Tab Page 001
      #region Cnty
      private void Gv_Cnty_DoubleClick(object sender, EventArgs e)
      {
         if (CntyBs1.Current == null) return;

         var cnty = CntyBs1.Current as Data.Country;

         Txt_CntyCode.Text = cnty.CODE;
         Txt_CntyName.Text = cnty.NAME;
      }

      private void Txt_CntyCode_Leave(object sender, EventArgs e)
      {
         if (Txt_CntyCode.Text.Length == 0) return;
         Txt_CntyCode.Text = Txt_CntyCode.Text.PadLeft(3, '0');
      }

      private void Btn_NewRecCnty_Click(object sender, EventArgs e)
      {
         Txt_CntyCode.EditValue = null;
         Txt_CntyName.EditValue = null;
      }

      private void Btn_DelRecCnty_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_CntyCode.Text == null || Txt_CntyCode.Text == "" || Txt_CntyCode.Text.Length != 3) return;

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "009"),
                  new XElement("Delete",
                     new XElement("Country",
                        new XAttribute("code", Txt_CntyCode.Text)
                     )
                  )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void Btn_UpdRecCnty_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_CntyCode.Text == null || Txt_CntyCode.Text == "" || Txt_CntyCode.Text.Length != 3) { Txt_CntyCode.Focus(); return; }
            if (Txt_CntyName.Text == null || Txt_CntyName.Text == "") { Txt_CntyName.Focus(); return; }

            if (MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "009"),
                     new XElement("Update",
                        new XElement("Country",
                           new XAttribute("code", Txt_CntyCode.Text),
                           new XAttribute("name", Txt_CntyName.Text)
                        )
                     )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void Btn_InsRecCnty_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_CntyCode.Text == null || Txt_CntyCode.Text == "" || Txt_CntyCode.Text.Length != 3) { Txt_CntyCode.Focus(); return; }
            if (Txt_CntyName.Text == null || Txt_CntyName.Text == "") { Txt_CntyName.Focus(); return; }

            //if (MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "009"),
                     new XElement("Insert",
                        new XElement("Country",
                           new XAttribute("code", Txt_CntyCode.Text),
                           new XAttribute("name", Txt_CntyName.Text)
                        )
                     )
               )
            );
            requery = true;
         }catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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
      #endregion

      #region Prvn
      private void Gv_Prvn_DoubleClick(object sender, EventArgs e)
      {
         if (PrvnBs1.Current == null) return;

         var prvn = PrvnBs1.Current as Data.Province;
         Txt_PrvnCntyCode.Text = prvn.CNTY_CODE;
         Txt_PrvnCode.Text = prvn.CODE;
         Txt_PrvnName.Text = prvn.NAME;
      }

      private void Txt_PrvnCntyCode_Leave(object sender, EventArgs e)
      {
         if (Txt_PrvnCntyCode.Text.Length == 0) return;
         Txt_PrvnCntyCode.Text = Txt_PrvnCntyCode.Text.PadLeft(3, '0');
      }

      private void Txt_PrvnCode_Leave(object sender, EventArgs e)
      {
         if (Txt_PrvnCode.Text.Length == 0) return;
         Txt_PrvnCode.Text = Txt_PrvnCode.Text.PadLeft(3, '0');
      }

      private void Txt_PrvnCntyCode_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if (e.NewValue == null || e.NewValue.ToString() == "" || e.NewValue.ToString().Length != 3) return;

         if (iScsc.Countries.FirstOrDefault(c => c.CODE == e.NewValue.ToString()) != null)
            Lbl_PrvnCntyName.Text = iScsc.Countries.FirstOrDefault(c => c.CODE == e.NewValue.ToString()).NAME;
         else
            Lbl_PrvnCntyName.Text = "";
      }

      private void Btn_NewRecPrvn_Click(object sender, EventArgs e)
      {
         Txt_PrvnCntyCode.EditValue = null;
         Txt_PrvnCode.EditValue = null;
         Txt_PrvnName.EditValue = null;
      }

      private void Btn_DelRecPrvn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_PrvnCntyCode.Text == null || Txt_PrvnCntyCode.Text == "" || Txt_PrvnCntyCode.Text.Length != 3) return;
            if (Txt_PrvnCode.Text == null || Txt_PrvnCode.Text == "" || Txt_PrvnCode.Text.Length != 3) return;

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "008"),
                  new XElement("Delete",
                     new XElement("Province",
                        new XAttribute("code", Txt_PrvnCntyCode.Text),
                        new XAttribute("cntycode", Txt_CntyCode.Text)
                     )
                  )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void Btn_UpdRecPrvn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_PrvnCntyCode.Text == null || Txt_PrvnCntyCode.Text == "" || Txt_PrvnCntyCode.Text.Length != 3) { Txt_PrvnCntyCode.Focus(); return; }
            if (Txt_PrvnCode.Text == null || Txt_PrvnCode.Text == "" || Txt_PrvnCode.Text.Length != 3) { Txt_PrvnCode.Focus(); return; }
            if (Txt_PrvnName.Text == null || Txt_PrvnName.Text == "") { Txt_PrvnName.Focus(); return; }

            if (MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "008"),
                        new XElement("Update",
                        new XElement("Province",
                           new XAttribute("cntycode", Txt_PrvnCntyCode.Text),
                           new XAttribute("code", Txt_PrvnCode.Text),
                           new XAttribute("name", Txt_PrvnName.Text)
                        )
                     )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void Btn_InsRecPrvn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_PrvnCntyCode.Text == null || Txt_PrvnCntyCode.Text == "" || Txt_PrvnCntyCode.Text.Length != 3) { Txt_PrvnCntyCode.Focus(); return; }
            if (Txt_PrvnCode.Text == null || Txt_PrvnCode.Text == "" || Txt_PrvnCode.Text.Length != 3) { Txt_PrvnCode.Focus(); return; }
            if (Txt_PrvnName.Text == null || Txt_PrvnName.Text == "") { Txt_PrvnName.Focus(); return; }

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "008"),
                        new XElement("Insert",
                        new XElement("Province",
                           new XAttribute("cntycode", Txt_PrvnCntyCode.Text),
                           new XAttribute("code", Txt_PrvnCode.Text),
                           new XAttribute("name", Txt_PrvnName.Text)
                        )
                     )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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
      #endregion

      #region Regn
      private void Gv_Regn_DoubleClick(object sender, EventArgs e)
      {
         if (RegnBs1.Current == null) return;

         var regn = RegnBs1.Current as Data.Region;
         Txt_RegnPrvnCntyCode.Text = regn.PRVN_CNTY_CODE;
         Txt_RegnPrvnCode.Text = regn.PRVN_CODE;
         Txt_RegnCode.Text = regn.CODE;
         Txt_RegnName.Text = regn.NAME;
         Txt_RegnRegnCode.Text = regn.REGN_CODE;
      }

      private void Txt_RegnPrvnCntyCode_Leave(object sender, EventArgs e)
      {
         if (Txt_RegnPrvnCntyCode.Text.Length == 0) return;
         Txt_RegnPrvnCntyCode.Text = Txt_RegnPrvnCntyCode.Text.PadLeft(3, '0');
      }

      private void Txt_RegnPrvnCode_Leave(object sender, EventArgs e)
      {
         if (Txt_RegnPrvnCode.Text.Length == 0) return;
         Txt_RegnPrvnCode.Text = Txt_RegnPrvnCode.Text.PadLeft(3, '0');
      }

      private void Txt_RegnCode_Leave(object sender, EventArgs e)
      {
         if (Txt_RegnCode.Text.Length == 0) return;
         Txt_RegnCode.Text = Txt_RegnCode.Text.PadLeft(3, '0');
      }

      private void Txt_RegnRegnCode_Leave(object sender, EventArgs e)
      {
         if (Txt_RegnRegnCode.Text.Length == 0) return;
         Txt_RegnRegnCode.Text = Txt_RegnRegnCode.Text.PadLeft(3, '0');
      }

      private void Txt_RegnPrvnCntyCode_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if (e.NewValue == null || e.NewValue.ToString() == "" || e.NewValue.ToString().Length != 3) return;

         if (iScsc.Countries.FirstOrDefault(c => c.CODE == e.NewValue.ToString()) != null)
            Lbl_RegnPrvnCntyName.Text = iScsc.Countries.FirstOrDefault(c => c.CODE == e.NewValue.ToString()).NAME;
         else
            Lbl_RegnPrvnCntyName.Text = "";
      }

      private void Txt_RegnPrvnCode_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if (e.NewValue == null || e.NewValue.ToString() == "" || e.NewValue.ToString().Length != 3) return;

         if (iScsc.Provinces.FirstOrDefault(p => p.CODE == e.NewValue.ToString()) != null)
            Lbl_RegnPrvnName.Text = iScsc.Provinces.FirstOrDefault(p => p.CODE == e.NewValue.ToString()).NAME;
         else
            Lbl_RegnPrvnName.Text = "";
      }

      private void Txt_RegnRegnCode_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if (e.NewValue == null || e.NewValue.ToString() == "" || e.NewValue.ToString().Length != 3) return;

         if (iScsc.Regions.FirstOrDefault(r => r.CODE == e.NewValue.ToString()) != null)
            Lbl_RegnRegnName.Text = iScsc.Regions.FirstOrDefault(r => r.CODE == e.NewValue.ToString()).NAME;
         else
            Lbl_RegnRegnName.Text = "";
      }

      private void Btn_NewRecRegn_Click(object sender, EventArgs e)
      {
         Txt_RegnPrvnCntyCode.EditValue = null;
         Txt_RegnPrvnCode.EditValue = null;
         Txt_RegnCode.EditValue = null;
         Txt_RegnName.EditValue = null;
         Txt_RegnRegnCode.EditValue = null;
      }

      private void Btn_DelRecRegn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_RegnPrvnCntyCode.Text == null || Txt_RegnPrvnCntyCode.Text == "" || Txt_RegnPrvnCntyCode.Text.Length != 3) return;
            if (Txt_RegnPrvnCode.Text == null || Txt_RegnPrvnCode.Text == "" || Txt_RegnPrvnCode.Text.Length != 3) return;
            if (Txt_RegnCode.Text == null || Txt_RegnCode.Text == "" || Txt_RegnCode.Text.Length != 3) return;

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "007"),
                  new XElement("Delete",
                     new XElement("Region",
                        new XAttribute("code", Txt_RegnCode.Text),
                        new XAttribute("cntycode", Txt_RegnPrvnCntyCode.Text),
                        new XAttribute("prvncode", Txt_RegnPrvnCode.Text)
                     )
                  )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void Btn_UpdRecRegn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_RegnPrvnCntyCode.Text == null || Txt_RegnPrvnCntyCode.Text == "" || Txt_RegnPrvnCntyCode.Text.Length != 3) { Txt_RegnPrvnCntyCode.Focus(); return; }
            if (Txt_RegnPrvnCode.Text == null || Txt_RegnPrvnCode.Text == "" || Txt_RegnPrvnCode.Text.Length != 3) { Txt_RegnPrvnCode.Focus(); return; }
            if (Txt_RegnCode.Text == null || Txt_RegnCode.Text == "" || Txt_RegnCode.Text.Length != 3) { Txt_RegnCode.Focus(); return; }
            if (Txt_RegnName.Text == null || Txt_RegnName.Text == "") { Txt_RegnName.Focus(); return; }

            if (MessageBox.Show(this, "آیا با ویرایش کردن رکورد موافقید؟", "ویرایش رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "007"),
                     new XElement("Update",
                        new XElement("Region",
                           new XAttribute("cntycode", Txt_RegnPrvnCntyCode.Text),
                           new XAttribute("prvncode", Txt_RegnPrvnCode.Text),
                           new XAttribute("code", Txt_RegnCode.Text),
                           new XAttribute("regncode", Txt_RegnRegnCode.Text ?? ""),
                           new XAttribute("name", Txt_RegnName.Text)
                        )
                     )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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

      private void Btn_InsRecRegn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_RegnPrvnCntyCode.Text == null || Txt_RegnPrvnCntyCode.Text == "" || Txt_RegnPrvnCntyCode.Text.Length != 3) { Txt_RegnPrvnCntyCode.Focus(); return; }
            if (Txt_RegnPrvnCode.Text == null || Txt_RegnPrvnCode.Text == "" || Txt_RegnPrvnCode.Text.Length != 3) { Txt_RegnPrvnCode.Focus(); return; }
            if (Txt_RegnCode.Text == null || Txt_RegnCode.Text == "" || Txt_RegnCode.Text.Length != 3) { Txt_RegnCode.Focus(); return; }
            if (Txt_RegnName.Text == null || Txt_RegnName.Text == "") { Txt_RegnName.Focus(); return; }

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "007"),
                     new XElement("Insert",
                        new XElement("Region",
                           new XAttribute("cntycode", Txt_RegnPrvnCntyCode.Text),
                           new XAttribute("prvncode", Txt_RegnPrvnCode.Text),
                           new XAttribute("code", Txt_RegnCode.Text),
                           new XAttribute("regncode", Txt_RegnRegnCode.Text ?? ""),
                           new XAttribute("name", Txt_RegnName.Text)
                        )
                     )
               )
            );
            requery = true;
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
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
      #endregion
      #endregion

      #region Tab Page 002
      #region Club
      private void Btn_InsRecClub_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_ClubName.Text == "" || Txt_ClubName.Text == null) { Txt_ClubName.Focus(); return; }

            var c = ClubBs2.Current as Data.Club;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "006"),
                     new XElement("Insert",
                        new XElement("Club",
                           new XAttribute("cntycode", (CntyBs2.Current as Data.Country).CODE ?? ""),
                           new XAttribute("prvncode", (PrvnBs2.Current as Data.Province).CODE ?? ""),
                           new XAttribute("regncode", (RegnBs2.Current as Data.Region).CODE ?? ""),
                           new XAttribute("name", c.NAME ?? ""),
                           new XAttribute("clubcode", c.CLUB_CODE ?? 0),
                           new XAttribute("postadrs", c.POST_ADRS ?? ""),
                           new XAttribute("emaladrs", c.EMAL_ADRS ?? ""),
                           new XAttribute("website", c.WEB_SITE ?? "")
                        )
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_UpdRecClub_Click(object sender, EventArgs e)
      {
         try
         {            
            if (Txt_ClubName.Text == "" || Txt_ClubName.Text == null) { Txt_ClubName.Focus(); return; }

            if (MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var c = ClubBs2.Current as Data.Club;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "006"),
                     new XElement("Update",
                        new XElement("Club",
                           new XAttribute("code", c.CODE),
                           new XAttribute("name", c.NAME ?? ""),
                           new XAttribute("clubcode", c.CLUB_CODE ?? 0),
                           new XAttribute("postadrs", c.POST_ADRS ?? ""),
                           new XAttribute("emaladrs", c.EMAL_ADRS ?? ""),
                           new XAttribute("website", c.WEB_SITE ?? "")
                        )
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_DelRecClub_Click(object sender, EventArgs e)
      {
         try
         {
            var club = ClubBs2.Current as Data.Club;
            if (club == null) return;
            if(club.CRET_BY == null)
            {
               ClubBs2.RemoveCurrent();
               return;
            }

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "006"),
                  new XElement("Delete",
                     new XElement("Club",
                        new XAttribute("code", club.CODE)
                     )
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_NewRecClub_Click(object sender, EventArgs e)
      {
         ClubBs2.AddNew();
      }

      private void ClubBs2_CurrentChanged(object sender, EventArgs e)
      {
         var club = ClubBs2.Current as Data.Club;

         if (club == null || club.CRET_BY == null)
         {
            Btn_NewRecClub.Enabled = Btn_InsRecClub.Enabled = Btn_DelRecClub.Enabled == true;
            Btn_UpdRecClub.Enabled = false;
         }
         else
         {
            Btn_InsRecClub.Enabled = false;
            Btn_NewRecClub.Enabled = Btn_UpdRecClub.Enabled = Btn_DelRecClub.Enabled = true;
         }
      }
      #endregion

      #region Club_Method
      private void Btn_InsRecCbmt_Click(object sender, EventArgs e)
      {
         try
         {
            if (Lov_CbmtMtodCode.ItemIndex == -1) { Lov_CbmtMtodCode.Focus(); return; }
            if (Lov_CbmtCochFileNo.ItemIndex == -1) { Lov_CbmtCochFileNo.Focus(); return; }
            if (Lov_CbmtDayType.ItemIndex == -1) { Lov_CbmtDayType.Focus(); return; }
            if (Lov_CbmtMtodStat.ItemIndex == -1) { Lov_CbmtMtodStat.Focus(); return; }

            var c = CbmtBs2.Current as Data.Club_Method;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                     new XElement("Insert",
                        new XElement("Club_Method",
                           new XAttribute("clubcode", (ClubBs2.Current as Data.Club).CODE),
                           new XAttribute("mtodcode", c.MTOD_CODE),
                           new XAttribute("cochfileno", c.COCH_FILE_NO),
                           new XAttribute("daytype", c.DAY_TYPE),
                           new XAttribute("strttime", c.STRT_TIME.ToString()),
                           new XAttribute("endtime", c.END_TIME.ToString()),
                           new XAttribute("mtodstat", c.MTOD_STAT ?? "002"),
                           new XAttribute("sextype", c.SEX_TYPE ?? "002")
                        )
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
            if (requery)
            {
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_UpdRecCbmt_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            if (Lov_CbmtMtodCode.ItemIndex == -1) { Lov_CbmtMtodCode.Focus(); return; }
            if (Lov_CbmtCochFileNo.ItemIndex == -1) { Lov_CbmtCochFileNo.Focus(); return; }
            if (Lov_CbmtDayType.ItemIndex == -1) { Lov_CbmtDayType.Focus(); return; }
            if (Lov_CbmtMtodStat.ItemIndex == -1) { Lov_CbmtMtodStat.Focus(); return; }

            var c = CbmtBs2.Current as Data.Club_Method;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                     new XElement("Update",
                        new XElement("Club_Method",
                           new XAttribute("code", c.CODE),
                           new XAttribute("clubcode", c.CLUB_CODE),
                           new XAttribute("mtodcode", c.MTOD_CODE),
                           new XAttribute("cochfileno", c.COCH_FILE_NO),
                           new XAttribute("daytype", c.DAY_TYPE),
                           new XAttribute("strttime", c.STRT_TIME.ToString()),
                           new XAttribute("endtime", c.END_TIME.ToString()),
                           new XAttribute("mtodstat", c.MTOD_STAT),
                           new XAttribute("sextype", c.SEX_TYPE),
                           new XElement("Club_Method_Weekdays",
                              CbmwBs2.List.OfType<Data.Club_Method_Weekday>().Select( cbmw =>
                                 new XElement("Club_Method_Weekday",
                                    new XAttribute("code", cbmw.CODE),
                                    new XAttribute("weekday", cbmw.WEEK_DAY),
                                    new XAttribute("stat", cbmw.STAT)
                                 )
                              )
                           )
                        )
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_DelRecCbmt_Click(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtBs2.Current as Data.Club_Method;
            if (cbmt == null) return;
            if(cbmt.CRET_BY == null)
            {
               CbmtBs2.RemoveCurrent();
               return;
            }

            if (MessageBox.Show(this, "آیا با حذف کردن رکورد موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                  new XElement("Delete",
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
                     )
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_NewRecCbmt_Click(object sender, EventArgs e)
      {
         CbmtBs2.AddNew();
      }

      private void Btn_CbmtCreateMethod_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>17</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 08 /* Execute Mstr_Mtod_F */){Input = GetType().Name}
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_CbmtCreatCoch_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */){Input = GetType().Name},
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "coach"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void CbmtBs2_CurrentChanged(object sender, EventArgs e)
      {
         var cbmt = CbmtBs2.Current as Data.Club_Method;

         if (cbmt == null || cbmt.CRET_BY == null)
         {
            Btn_NewRecCbmt.Enabled = Btn_InsRecCbmt.Enabled = Btn_DelRecCbmt.Enabled = true;
            Btn_UpdRecCbmt.Enabled = false;
         }
         else
         {
            Btn_InsRecCbmt.Enabled = false;
            Btn_NewRecCbmt.Enabled = Btn_UpdRecCbmt.Enabled = Btn_DelRecCbmt.Enabled = true;
         }
      }

      private void Btn_EvenSelect_Click(object sender, EventArgs e)
      {
         CbmwBs2.List.OfType<Data.Club_Method_Weekday>().Where(w => w.WEEK_DAY == "002" || w.WEEK_DAY == "004" || w.WEEK_DAY == "007").ToList().ForEach(w => w.STAT = "002");         
      }

      private void Btn_OddSelect_Click(object sender, EventArgs e)
      {
         CbmwBs2.List.OfType<Data.Club_Method_Weekday>().Where(w => w.WEEK_DAY == "001" || w.WEEK_DAY == "003" || w.WEEK_DAY == "005").ToList().ForEach(w => w.STAT = "002");
      }

      private void Btn_Deselect_Click(object sender, EventArgs e)
      {
         CbmwBs2.List.OfType<Data.Club_Method_Weekday>().ToList().ForEach(w => w.STAT = "001");
      }

      #endregion
      #endregion

      #region Tab Page 003
      #region Fga User Region
      private void Btn_InsRecFUR_Click(object sender, EventArgs e)
      {
         try
         {
            var regn = RegnBs2.Current as Data.Region;
            var user = VUserBs2.Current as Data.V_User;

            if (regn == null) return;
            if (user == null) return;            

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "001"),
                  new XElement("FgaURegn",
                     new XAttribute("cntycode", regn.PRVN_CNTY_CODE),
                     new XAttribute("prvncode", regn.PRVN_CODE),
                     new XAttribute("regncode", regn.CODE),
                     new XAttribute("sysuser", user.USER_DB)
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_DelRecFUR_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا می خواهید کاربر را از ناحیه خارج کنید؟", "حذف کاربر", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes) return;

            var Fga_URegn = FURgnBs3.Current as Data.User_Region_Fgac;
            if (Fga_URegn == null) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "002"),
                  new XElement("FgaURegn",
                     new XAttribute("cntycode", Fga_URegn.REGN_PRVN_CNTY_CODE),
                     new XAttribute("prvncode", Fga_URegn.REGN_PRVN_CODE),
                     new XAttribute("regncode", Fga_URegn.REGN_CODE),
                     new XAttribute("sysuser", Fga_URegn.SYS_USER)
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void RegnBs2_CurrentChanged(object sender, EventArgs e)
      {
         GV_FURG.ActiveFilterString = "Rec_Stat = '002'";
      }

      private void FURgnBs3_CurrentChanged(object sender, EventArgs e)
      {
         var ur = FURgnBs3.Current as Data.User_Region_Fgac;

         if (ur == null) return;

         ClubBs3.DataSource = iScsc.Clubs.Where(c => ur.Region == c.Region && !c.User_Club_Fgacs.Any(uc => uc.SYS_USER == ur.SYS_USER && uc.REC_STAT == "002"));

         FUClbBs3.DataSource = iScsc.User_Club_Fgacs.Where(uc => uc.SYS_USER == ur.SYS_USER && uc.REC_STAT == "002");
      }      
      #endregion

      #region Fga User Club
      private void Btn_InsRecFUC_Click(object sender, EventArgs e)
      {
         try
         {
            var Fga_URegn = FURgnBs3.Current as Data.User_Region_Fgac;
            var club = ClubBs3.Current as Data.Club;
            
            if (Fga_URegn == null) return;            
            if (club == null) return;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "003"),
                  new XElement("FgaUClub",
                     new XAttribute("sysuser", Fga_URegn.SYS_USER),
                     new XAttribute("mstrsysuser", ""),
                     new XAttribute("clubcode", club.CODE)
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_DelRecFUC_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا می خواهید کاربر را از باشگاه خارج کنید؟", "حذف کاربر", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes) return;
            
            var Fga_Club = FUClbBs3.Current as Data.User_Club_Fgac;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "004"),
                  new XElement("FgaUClub",
                     new XAttribute("sysuser", Fga_Club.SYS_USER),
                     new XAttribute("mstrsysuser", Fga_Club.MAST_SYS_USER),
                     new XAttribute("clubcode", Fga_Club.CLUB_CODE)
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
               Execute_Query(false);
               requery = false;
            }
         }
      }
      #endregion      

      #endregion

   }
}
