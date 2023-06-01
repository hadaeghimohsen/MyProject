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
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.Xml.Linq;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class ORGN_DVLP_F : UserControl
   {
      public ORGN_DVLP_F()
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
         int job = JobBs.Position;
         int prbt = PrbtBs.Position;
         int prjb = PrjbBs.Position;
         int rmnu = RmnusBs.Position;
         int smnu = SmnusBs.Position;
         int grmu = GrmuBs.Position;
         int srbt = SrbtBs.Position;
         int srgp = SrgpBs.Position;
         int grop = GropBs.Position;
         int rbmd = RbmdBs.Position;
         int rbdc = RbdcBs.Position;
         int gphd = GphdBs.Position;
         int ghit = GhitBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         JobBs.Position = job;
         PrbtBs.Position = prbt;
         PrjbBs.Position = prjb;
         RmnusBs.Position = rmnu;
         SmnusBs.Position = smnu;
         GrmuBs.Position = grmu;
         SrbtBs.Position = srbt;
         SrgpBs.Position = srgp;
         GropBs.Position = grop;
         RbmdBs.Position = rbmd;
         RbdcBs.Position = rbdc;
         GphdBs.Position = gphd;
         GhitBs.Position = ghit;

         requery = false;
      }

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            Invalidate();

            Menus_TreeList.PostEditor();

            Grop_Gv.PostEditor();
            Srgp_Gv.PostEditor();
            Rbmd_Gv.PostEditor();
            Rbdc_Gv.PostEditor();
            Gphd_Gv.PostEditor();
            Ghit_Gv.PostEditor();
            Prjb_gv.PostEditor();
            Prbt_Gv.PostEditor();
            Grmu_Gv.PostEditor();
            
            OrgnBs.EndEdit();
            RoboBs.EndEdit();
            JobBs.EndEdit();
            PrbtBs.EndEdit();
            PrjbBs.EndEdit();
            RmnusBs.EndEdit();
            SmnusBs.EndEdit();
            GrmuBs.EndEdit();
            SrbtBs.EndEdit();
            SrgpBs.EndEdit();
            GropBs.EndEdit();
            RbmdBs.EndEdit();
            RbdcBs.EndEdit();
            GphdBs.EndEdit();
            GhitBs.EndEdit();
            

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
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Tsb_DelOrgn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var orgn = OrgnBs.Current as Data.Organ;

            iRoboTech.DEL_ORGN_P(orgn.OGID);
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

      private void Tsb_DelRobo_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;

            iRoboTech.DEL_ROBO_P(robo.RBID);
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

      private void Tsb_DelJob_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var job = JobBs.Current as Data.Job;

            iRoboTech.DEL_JOB_P(job.ROBO_RBID, job.CODE);
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

      private void Tsb_DelPrbt_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var prbt = PrbtBs.Current as Data.Personal_Robot;

            iRoboTech.DEL_PRBT_P(prbt.SERV_FILE_NO, prbt.ROBO_RBID);
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

      private void Tsb_DelPrjb_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var prjb = PrjbBs.Current as Data.Personal_Robot_Job;

            iRoboTech.DEL_PRJB_P(prjb.CODE);
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

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         var robot = RoboBs.Current as Data.Robot;
         ServPrbtBs.DataSource = iRoboTech.Services.Where(s => s.Service_Robots.Any(sr => sr.Robot == robot));
         ServPrjbBs.DataSource = iRoboTech.Services.Where(s => s.Personal_Robots.Any(pr => pr.Robot == robot));
      }

      private void Tsb_DelMenu_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف منوی ربات موافق هستید؟", "حذف منو ربات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var menu = RmnusBs.Current as Data.Menu_Ussd;

            iRoboTech.DEL_MNUS_P(menu.ROBO_RBID, menu.MUID);
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

      private void Tsb_DelSubMenu_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var menu = SmnusBs.Current as Data.Menu_Ussd;

            iRoboTech.DEL_MNUS_P(menu.ROBO_RBID, menu.MUID);
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

      private void Tsb_CreateBackMenu_Click(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا با ایجاد منوی بازگشت موافق هستید؟", "ایجاد منوی بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var StepBack = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "🔺 بازگشت",
               MNUS_DESC = "بازگشت",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "000"
            };

            RmnusBs.Add(StepBack);
            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
               requery = false;
            }
         }
      }

      private void ConvertToStartMenu_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا با تبدیل منوی فعلی به منوی اصلی موافق هستید؟", "تبدیل به منوی اصلی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            menu.ROOT_MENU = "002";
            menu.MNUS_MUID = null;

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Tsb_SearchInMenu_Click(object sender, EventArgs e)
      {
         Menus_TreeList.OptionsFind.AllowFindPanel = Menus_Gv.OptionsFind.AlwaysVisible = !Menus_Gv.OptionsFind.AlwaysVisible;
         //Menus_TreeList.OptionsFind.AlwaysVisible = !Menus_TreeList.OptionsFind.AlwaysVisible;
      }

      private void Tsb_DelGrmu_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var grmu = GrmuBs.Current as Data.Group_Menu_Ussd;

            iRoboTech.DEL_GRMU_P(grmu.GROP_GPID, grmu.MNUS_MUID, grmu.MNUS_ROBO_RBID);
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

      private void Tsb_DelSrgp_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var srgp = SrgpBs.Current as Data.Service_Robot_Group;

            iRoboTech.DEL_SRGP_P(srgp.GROP_GPID, srgp.SRBT_SERV_FILE_NO, srgp.SRBT_ROBO_RBID);
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

      private void Tsb_DelGrop_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var grop = GropBs.Current as Data.Group;

            iRoboTech.DEL_GROP_P(grop.ROBO_RBID, grop.GPID);
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

      private void Tsb_DelRbmd_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var rbmd = RbmdBs.Current as Data.Organ_Media;

            iRoboTech.DEL_RBMD_P(rbmd.ORGN_OGID, rbmd.ROBO_RBID, rbmd.OPID);
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

      private void DuplicateMedia_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = RbmdBs.Current as Data.Organ_Media;

            var newobj = RbmdBs.AddNew() as Data.Organ_Media;

            newobj.ROBO_RBID = crnt.ROBO_RBID;
            newobj.USSD_CODE = crnt.USSD_CODE;
            newobj.IMAG_DESC = crnt.IMAG_DESC;
            newobj.STAT = crnt.STAT;
            newobj.SHOW_STRT = crnt.SHOW_STRT;
            newobj.ORDR = crnt.ORDR + 1;

            iRoboTech.Organ_Medias.InsertOnSubmit(newobj);

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
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Tsb_DelRbdc_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var rbdc = RbdcBs.Current as Data.Organ_Description;

            iRoboTech.DEL_RBDC_P(rbdc.ORGN_OGID, rbdc.ROBO_RBID, rbdc.ODID);
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

      private void DuplicateInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = RbdcBs.Current as Data.Organ_Description;

            var newobj = RbdcBs.AddNew() as Data.Organ_Description;

            newobj.ROBO_RBID = crnt.ROBO_RBID;
            newobj.USSD_CODE = crnt.USSD_CODE;
            newobj.ITEM_DESC = crnt.ITEM_DESC;
            newobj.ITEM_VALU = crnt.ITEM_VALU;
            newobj.STAT = crnt.STAT;
            newobj.SHOW_STRT = crnt.SHOW_STRT;
            newobj.ORDR = crnt.ORDR + 1;

            iRoboTech.Organ_Descriptions.InsertOnSubmit(newobj);

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
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            dynamic ussdcode = "";
            if (tb_master.SelectedTab == tp_002)
            {

            }
            else if (tb_master.SelectedTab == tp_004)
            {
               ussdcode = (RmnusBs.Current as Data.Menu_Ussd).USSD_CODE;
            }
            switch (e.Button.Index)
            {
               case 0:
                  #region Organ Description
                  Rbdc_Gv.ActiveFilterString = string.Format("[USSD_CODE] = '{0}'", ussdcode);
                  tb_master.SelectedTab = tp_006;
                  if (MessageBox.Show(this, "آیا مایل به اضافه کردن رکورد هستید؟", "ایجاد رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                  {
                     try
                     {
                        var crnt = RbdcBs.Current as Data.Organ_Description;

                        var newobj = RbdcBs.AddNew() as Data.Organ_Description;

                        newobj.ROBO_RBID = crnt.ROBO_RBID;
                        newobj.USSD_CODE = ussdcode;
                        newobj.ITEM_DESC = crnt.ITEM_DESC;
                        newobj.ITEM_VALU = crnt.ITEM_VALU;
                        newobj.STAT = crnt.STAT;
                        newobj.SHOW_STRT = crnt.SHOW_STRT;
                        newobj.ORDR = crnt.ORDR + 1;

                        iRoboTech.Organ_Descriptions.InsertOnSubmit(newobj);

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
                        {
                           Execute_Query();
                           requery = false;
                        }
                     }
                  }
                  #endregion
                  break;
               case 1:
                  #region Organ Media
                  Rbmd_Gv.ActiveFilterString = string.Format("[USSD_CODE] = '{0}'", ussdcode);
                  tb_master.SelectedTab = tp_006;
                  if (MessageBox.Show(this, "آیا مایل به اضافه کردن رکورد هستید؟", "ایجاد رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                  {
                     try
                     {
                        var crnt = RbmdBs.Current as Data.Organ_Media;

                        var newobj = RbmdBs.AddNew() as Data.Organ_Media;

                        newobj.ROBO_RBID = crnt.ROBO_RBID;
                        newobj.USSD_CODE = ussdcode;
                        newobj.IMAG_DESC = (RmnusBs.Current as Data.Menu_Ussd).MENU_TEXT;
                        newobj.STAT = crnt.STAT;
                        newobj.SHOW_STRT = crnt.SHOW_STRT;
                        newobj.ORDR = crnt.ORDR + 1;
                        newobj.IMAG_TYPE = "002";

                        iRoboTech.Organ_Medias.InsertOnSubmit(newobj);

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
                        {
                           Execute_Query();
                           requery = false;
                        }
                     }
                  }
                  #endregion
                  break;
               case 2:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 13 /* Execute Rbmn_Dvlp_F */),
                           new Job(SendType.SelfToUserInterface, "RBMN_DVLP_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input = 
                                 new XElement("Menu",
                                    new XAttribute("muid", (RmnusBs.Current as Data.Menu_Ussd).MUID)
                                 )
                           }
                        }
                     )
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception)
         {

         }
      }

      private void AddSubMenu_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا با ایجاد زیر منوی موافق هستید؟", "ایجاد زیر منوی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;
            var menutext = Microsoft.VisualBasic.Interaction.InputBox("نام منو", "لطفا متن منو را وارد کنید");

            var SubMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = menutext,
               MNUS_DESC = menutext,
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "001",
               STEP_BACK_USSD_CODE = "",
               CLMN = 1,
               CMND_TYPE = "000",
               MENU_TYPE = "001"
            };

            RmnusBs.Add(SubMenu);
            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
               requery = false;
            }
         }
      }

      private void AddTopSubMenu_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا با ایجاد زیر منوی موافق هستید؟", "ایجاد زیر منوی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var menutext = Microsoft.VisualBasic.Interaction.InputBox("نام منو", "لطفا متن منو را وارد کنید");

            var StepBack = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MNUS_MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussd1.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.Menu_Ussd1.USSD_CODE.Substring(0, menu.Menu_Ussd1.USSD_CODE.Length - 1), menu.Menu_Ussd1.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = menutext,
               MNUS_DESC = menutext,
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "001",
               STEP_BACK_USSD_CODE = "",
               CLMN = 1,
               CMND_TYPE = "000"
            };

            RmnusBs.Add(StepBack);
            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
               requery = false;
            }
         }
      }

      private DragDropEffects GetDragDropEffect(TreeList tl, TreeListNode dragNode)
      {
         TreeListNode targetNode;
         Point p = tl.PointToClient(MousePosition);
         targetNode = tl.CalcHitInfo(p).Node;

         if (dragNode != null && targetNode != null
             && dragNode != targetNode
             /*&& dragNode.ParentNode == targetNode.ParentNode*/)
            return DragDropEffects.Move;
         else
            return DragDropEffects.None;
      }

      private void TreeMenu_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
      {
         TreeListNode dragNode = e.Data.GetData(typeof(TreeListNode)) as TreeListNode;
         e.Effect = GetDragDropEffect(sender as TreeList, dragNode);
      }

      private void TreeMenu_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
      {
         try
         {
            TreeListNode dragNode, targetNode;
            TreeList tl = sender as TreeList;
            Point p = tl.PointToClient(new Point(e.X, e.Y));

            dragNode = e.Data.GetData(typeof(TreeListNode)) as TreeListNode;
            targetNode = tl.CalcHitInfo(p).Node;

            iRoboTech.UPD_PMNU_P((long?)dragNode.GetValue("MUID"), (long?)targetNode.GetValue("MUID"));
            requery = true;
            tl.SetNodeIndex(dragNode, tl.GetNodeIndex(targetNode));
            e.Effect = DragDropEffects.None;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
         finally { 
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void TreeMenu_CalcNodeDragImageIndex(object sender, DevExpress.XtraTreeList.CalcNodeDragImageIndexEventArgs e)
      {
         TreeList tl = sender as TreeList;
         if (GetDragDropEffect(tl, tl.FocusedNode) == DragDropEffects.None)
            e.ImageIndex = -1;  // no icon
         else
            e.ImageIndex = 1;  // the reorder icon (a curved arrow)
      }

      private void DownLoad_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (FilePath_Fbd.ShowDialog() != DialogResult.OK) return;

            robo.DOWN_LOAD_FILE_PATH = FilePath_Fbd.SelectedPath;
         }
         catch (Exception exc)
         { }
         finally { RoboBs.EndEdit(); }
      }

      private void GropActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            var grop = GropBs.Current as Data.Group;
            if (grop == null) return;

            iRoboTech.INS_SRGP_P(grop.GPID, srbt.SERV_FILE_NO, srbt.ROBO_RBID, "002", "001");

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

      private void UpLoad_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (FilePath_Fbd.ShowDialog() != DialogResult.OK) return;

            robo.UP_LOAD_FILE_PATH = FilePath_Fbd.SelectedPath;
         }
         catch (Exception exc)
         { }
         finally { RoboBs.EndEdit(); }
      }

      private void GrmuBs_CurrentChanged_1(object sender, EventArgs e)
      {
         try
         {
            var grmu = GrmuBs.Current as Data.Group_Menu_Ussd;
            if (grmu == null) return;

            var gphd = GphdBs.Current as Data.Group_Header;
            if (gphd == null) return;

            GhitBs.DataSource = iRoboTech.Group_Header_Items.Where(ghit => ghit.Group_Menu_Ussd == grmu && ghit.Group_Header == gphd);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GphdBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var gphd = GphdBs.Current as Data.Group_Header;
            if (gphd == null) return;

            var grmu = GrmuBs.Current as Data.Group_Menu_Ussd;
            if (grmu == null) return;            

            GhitBs.DataSource = iRoboTech.Group_Header_Items.Where(ghit => ghit.Group_Menu_Ussd == grmu && ghit.Group_Header == gphd);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Tsb_DelGphd_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var gphd = GphdBs.Current as Data.Group_Header;

            iRoboTech.DEL_GRPH_P(gphd.GHID);
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
            }
         }
      }

      private void Tsb_DelGhit_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var ghit = GhitBs.Current as Data.Group_Header_Item;

            iRoboTech.DEL_GHIT_P(ghit.CODE);
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
            }
         }
      }

      private void Tsb_AddGhit_Click(object sender, EventArgs e)
      {
         try
         {
            var grmu = GrmuBs.Current as Data.Group_Menu_Ussd;
            if (grmu == null) return;

            var gphd = GphdBs.Current as Data.Group_Header;
            if (gphd == null) return;

            var ghit = GhitBs.AddNew() as Data.Group_Header_Item;
            ghit.Group_Menu_Ussd = grmu;
            ghit.Group_Header = gphd;
            ghit.STAT = "002";

            iRoboTech.Group_Header_Items.InsertOnSubmit(ghit);            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddCartMenu_Butn_Click(object sender, EventArgs e)
      {
         #region Add To Cart
         try
         {         
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var AddCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "➕ اضافه کردن به سبد خرید",
               MNUS_DESC = "➕ اضافه کردن به سبد خرید",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "008"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(AddCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion

         #region Delete from Cart
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var DelCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)1 : (short)(menu.Menu_Ussds.Count),
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "❌ حذف کردن از سبد خرید",
               MNUS_DESC = "❌ حذف کردن از سبد خرید",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "008"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(DelCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion

         #region Proceed Checkout
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var ProceedCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)1 : (short)(menu.Menu_Ussds.Count),
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "💳 عملیات پرداخت",
               MNUS_DESC = "💳 عملیات پرداخت",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "027"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(ProceedCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion

         #region Enter Manual Count Product
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var ProceedCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)1 : (short)(menu.Menu_Ussds.Count),
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "#️⃣ تعداد دستی",
               MNUS_DESC = "#️⃣ تعداد دستی",
               CMND_FIRE = "002",
               STAT = "002",
               CMND_PLAC = "002",
               STEP_BACK = "001",
               STEP_BACK_USSD_CODE = "",
               CLMN = 1,
               CMND_TYPE = "000"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(ProceedCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion

         #region Show Cart Items
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var ProceedCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)1 : (short)(menu.Menu_Ussds.Count),
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "🛍 نمایش سبد خرید",
               MNUS_DESC = "🛍 نمایش سبد خرید",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "027"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(ProceedCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion

         #region Step Back Main
         try
         {
            //if (MessageBox.Show(this, "آیا با ایجاد منوی بازگشت موافق هستید؟", "ایجاد منوی بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var StepBack = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "🔺 بازگشت",
               MNUS_DESC = "بازگشت",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "000"
            };

            RmnusBs.Add(StepBack);
            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion

         #region Step Back Enter Manual Count Product
         try
         {
            //if (MessageBox.Show(this, "آیا با ایجاد منوی بازگشت موافق هستید؟", "ایجاد منوی بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = (RmnusBs.Current as Data.Menu_Ussd).Menu_Ussds.FirstOrDefault(m => m.MENU_TEXT == "#️⃣ تعداد دستی");

            var StepBack = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "🔺 بازگشت",
               MNUS_DESC = "بازگشت",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "000"
            };

            RmnusBs.Add(StepBack);
            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
      }

      private void UApb_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute Isig_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Lookup",
                                 new XAttribute("tablename", "PRODUCTUNIT_INFO"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
               {
                  Input =
                     new XElement("GMapNets",
                        new XAttribute("requesttype", "get"),
                        new XAttribute("formcaller", "Program:RoboTech:" + GetType().Name),
                        new XAttribute("callback", 40 /* CordinateGetSet */),
                        new XAttribute("outputtype", "robotpostadrs"),
                        new XAttribute("initalset", true),
                        new XAttribute("cordx", robo.CORD_X == null ? "29.610420210528" : robo.CORD_X.ToString()),
                        new XAttribute("cordy", robo.CORD_Y == null ? "52.5152599811554" : robo.CORD_Y.ToString()),
                        new XAttribute("zoom", "1600")
                     )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RmnusBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var menu = RmnusBs.Current as Data.Menu_Ussd;
            if (menu == null)
            {
               ContTextInfo_Txt.Text = ContMedia_Txt.Text = "0";
               return;
            }

            switch(menu.MENU_TYPE)
            {                
               case "001":
                  // Keyboard markup
                  InlineQury_Pn.Visible = false;
                  break;
               case "002":
               case "003":
                  // Inline query
                  InlineQury_Pn.Visible = true;
                  break;
               default: break;
            }


            ContTextInfo_Txt.Text = RbdcBs.List.OfType<Data.Organ_Description>().Where(od => od.Robot == menu.Robot && od.USSD_CODE == menu.USSD_CODE).Count().ToString();
            ContMedia_Txt.Text = RbmdBs.List.OfType<Data.Organ_Media>().Where(om => om.Robot == menu.Robot && om.USSD_CODE == menu.USSD_CODE).Count().ToString();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddDateMenu_Tsm_Click(object sender, EventArgs e)
      {
         #region Today
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var AddCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "امروز",
               MNUS_DESC = "امروز",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "008",
               TIME_FRAM = "001"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(AddCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
         #region Yesterday
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var AddCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "دیروز",
               MNUS_DESC = "دیروز",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "008",
               TIME_FRAM = "002"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(AddCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
         #region Week
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var AddCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "این هفته",
               MNUS_DESC = "این هفته",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "008",
               TIME_FRAM = "003"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(AddCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
         #region Month
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var AddCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "این ماه",
               MNUS_DESC = "این ماه",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "008",
               TIME_FRAM = "004"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(AddCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
         #region Year
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var AddCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "امسال",
               MNUS_DESC = "امسال",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE, //.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "008",
               TIME_FRAM = "005"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(AddCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
         #region Between
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var AddCartMenu = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "بازه دلخواه",
               MNUS_DESC = "کاربر گرامی برای انتخاب بازه زمانی مورد دلخواه لطفا طبق این استاندار زیر اطلاعات خود را وارد کنید  *تاریخ شروع* *#* *تاریخ پایان*  برای مثال *1401/12/29* *#* *1401/12/01* *1401/05/31* *#* *1401/05/01*",
               CMND_FIRE = "002",
               STAT = "002",
               CMND_PLAC = "002",
               STEP_BACK = "001",               
               CLMN = 1,
               CMND_TYPE = "000"
            };

            iRoboTech.Menu_Ussds.InsertOnSubmit(AddCartMenu);

            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
         #region Step Back Main
         try
         {
            //if (MessageBox.Show(this, "آیا با ایجاد منوی بازگشت موافق هستید؟", "ایجاد منوی بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = RmnusBs.Current as Data.Menu_Ussd;

            var StepBack = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "🔺 بازگشت",
               MNUS_DESC = "بازگشت",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "000"
            };

            RmnusBs.Add(StepBack);
            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
         #region Step Back Between
         try
         {
            //if (MessageBox.Show(this, "آیا با ایجاد منوی بازگشت موافق هستید؟", "ایجاد منوی بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;

            var robo = RoboBs.Current as Data.Robot;
            var menu = (RmnusBs.Current as Data.Menu_Ussd).Menu_Ussds.FirstOrDefault(m => m.MENU_TEXT == "بازه دلخواه");

            var StepBack = new Data.Menu_Ussd()
            {
               ROBO_RBID = robo.RBID,
               MNUS_MUID = menu == null ? null : (long?)menu.MUID,
               ORDR = menu == null ? (short)0 : (short)menu.Menu_Ussds.Count,
               USSD_CODE = menu == null ? "*0#" : string.Format("{0}*{1}#", menu.USSD_CODE.Substring(0, menu.USSD_CODE.Length - 1), menu.Menu_Ussds.Count),
               ROOT_MENU = "001",
               MENU_TEXT = "🔺 بازگشت",
               MNUS_DESC = "بازگشت",
               CMND_FIRE = "001",
               STAT = "002",
               CMND_PLAC = "001",
               STEP_BACK = "002",
               STEP_BACK_USSD_CODE = menu.USSD_CODE.Substring(0, menu.USSD_CODE.LastIndexOf('*')) + "#",
               CLMN = 1,
               CMND_TYPE = "000"
            };

            RmnusBs.Add(StepBack);
            RmnusBs.EndEdit();
            iRoboTech.SubmitChanges();
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
               Execute_Query();
            }
         }
         #endregion
      }

      private void DestType_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var menu = RmnusBs.Current as Data.Menu_Ussd;
            if (menu == null) return;
            if(e.NewValue == null)return;

            menu.DEST_TYPE = (string)e.NewValue;

            if((string)e.NewValue == "002")
            {
               // اگر درخواست پردازش به سمت پایگاه داده باشد
               menu.PATH_TEXT = menu.USSD_CODE;
               CmndText_Txt.Focus();
            }
            else if((string)e.NewValue == "001")
            {
               // اگر درخواست پردازش به سمت نرم افزار باشد
               menu.PATH_TEXT = "";
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NotiOrdrShipPath_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (NotiSondPath_Ofd.ShowDialog() != DialogResult.OK) return;

            robo.NOTI_SOND_ORDR_SHIP_PATH = NotiSondPath_Ofd.FileName;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NotiOrdrRcptPath_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (NotiSondPath_Ofd.ShowDialog() != DialogResult.OK) return;

            robo.NOTI_SOND_ORDR_RCPT_PATH = NotiSondPath_Ofd.FileName;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NotiOrdrRecpPath_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (NotiSondPath_Ofd.ShowDialog() != DialogResult.OK) return;

            robo.NOTI_SOND_ORDR_RECP_PATH = NotiSondPath_Ofd.FileName;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveNoti_Butn_Click(object sender, EventArgs e)
      {
         SubmitChanged_Clicked(null, null);
      }

      private void RegSrbt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            if (srbt.CELL_PHON != null && srbt.NATL_CODE != null) return;

            if(SrbtFrstName_Txt.Text == ""){SrbtFrstName_Txt.Focus(); return;}
            if(SrbtLastName_Txt.Text == ""){SrbtLastName_Txt.Focus(); return;}
            if(SrbtCellPhon_Txt.Text == ""){SrbtCellPhon_Txt.Focus(); return;}
            if(SrbtNatlCode_Txt.Text == ""){SrbtNatlCode_Txt.Focus(); return;}

            var xRet = new XElement("Result");

            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                   new XAttribute("token", srbt.Robot.TKON_CODE),
                   new XElement("Message",
                       new XAttribute("cbq", "001"),
                       new XAttribute("ussd", "*1*0*0#"),
                       new XAttribute("chatid", srbt.CHAT_ID),
                       new XAttribute("mesgid", 0),
                       new XElement("Text",
                           string.Format("{0}#{1}#{2}#{3}", SrbtFrstName_Txt.Text, SrbtLastName_Txt.Text, SrbtCellPhon_Txt.Text, SrbtNatlCode_Txt.Text)
                       )
                   )
               ),
               ref xRet
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

      private void RetryReg_Butn_Click(object sender, EventArgs e)
      {
         SrbtFrstName_Txt.Text = SrbtLastName_Txt.Text = SrbtCellPhon_Txt.Text = SrbtNatlCode_Txt.Text = "";
      }
   }
}
