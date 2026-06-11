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

namespace System.RoboTech.Ui.BaseDefinition
{
   public partial class ISIC_DFIN_F : UserControl
   {
      public ISIC_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string entityname = "";
      private string formcaller = "";

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private async void Execute_Query()
      {
         int isic = IsicBs.Position;
         int grph = GrphBs.Position;
         int ghit = GhitBs.Position;
         int apbs = ApbsBs.Position;
         string _entityname = entityname;

         var result = await Task.Run(() =>
         {
            var db = new Data.iRoboTechDataContext(ConnectionString);
            var isics = db.Isic_Categories.ToList();
            var grphs = db.Group_Headers.ToList();
            var apbsList = _entityname == ""
               ? db.App_Base_Defines.ToList()
               : db.App_Base_Defines.Where(a => a.ENTY_NAME == _entityname).ToList();
            return new { isics, grphs, apbsList };
         });

         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         IsicBs.DataSource = result.isics;
         GrphBs.DataSource = result.grphs;
         ApbsBs.DataSource = result.apbsList;

         IsicBs.Position = isic;
         GrphBs.Position = grph;
         GhitBs.Position = ghit;
         ApbsBs.Position = apbs;

         requery = false;
      }

      private void Tsb_DelIsic_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var isic = IsicBs.Current as Data.Isic_Category;

            iRoboTech.DEL_ISIC_P(isic.CODE);
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

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Isic_gv.PostEditor();
            Grph_gv.PostEditor();
            Ghit_gv.PostEditor();
            Apbs_gv.PostEditor();


            IsicBs.EndEdit();
            GrphBs.EndEdit();
            GhitBs.EndEdit();
            ApbsBs.EndEdit();

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

      private void Tsb_DelGrph_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var grph = GrphBs.Current as Data.Group_Header;

            iRoboTech.DEL_GRPH_P(grph.GHID);
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

      private void Tbs_AddApbs_Click(object sender, EventArgs e)
      {
         try
         {
            if (ApbsBs.List.OfType<Data.App_Base_Define>().Any(a => a.CODE == 0)) return;
            if(entityname == "")return;

            ApbsBs.AddNew();

            var apbs = ApbsBs.Current as Data.App_Base_Define;
            if (ApbsBs.List.OfType<Data.App_Base_Define>().Where(a => a.ENTY_NAME == entityname).Count() == 0)
               apbs.RWNO = 1;
            else
               apbs.RWNO = ApbsBs.List.OfType<Data.App_Base_Define>().Where(a => a.ENTY_NAME == entityname).Max(a => a.RWNO) + 1;

            apbs.ENTY_NAME = entityname;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Tbs_DelApbs_Click(object sender, EventArgs e)
      {
         try
         {
            if (entityname == "") return;
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var apbs = ApbsBs.Current as Data.App_Base_Define;

            iRoboTech.DEL_APBS_P(apbs.CODE);
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

      private void AddGroupHeaderItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (GhitBs.List.OfType<Data.Group_Header_Item>().Any(a => a.CODE == 0)) return;
            if (GrphBs.Current == null) return;

            GhitBs.AddNew();

            var ghit = GhitBs.Current as Data.Group_Header_Item;
            ghit.STAT = "002";
            ghit.COEF_STAT = "001";
            ghit.SCND_NUMB = ghit.MINT_NUMB = ghit.HORS_NUMB = ghit.MONT_NUMB = ghit.YEAR_NUMB = 0;
            ghit.DAYS_NUMB = 1;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DeleteGroupHeaderItem_Butn_Click(object sender, EventArgs e)
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
   }
}
