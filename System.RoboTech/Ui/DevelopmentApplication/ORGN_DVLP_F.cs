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

         requery = false;
      }

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            Invalidate();

            Menus_TreeList.PostEditor();

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

            var srgp = GrmuBs.Current as Data.Service_Robot_Group;

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
                  gridView16.ActiveFilterString = string.Format("[USSD_CODE] = '{0}'", ussdcode);
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
                  gridView18.ActiveFilterString = string.Format("[USSD_CODE] = '{0}'", ussdcode);
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
               CMND_TYPE = "000"
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
   }
}
