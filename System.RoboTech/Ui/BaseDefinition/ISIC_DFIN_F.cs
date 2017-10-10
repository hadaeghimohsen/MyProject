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

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int isic = IsicBs.Position;
         int grph = GrphBs.Position;
         int apbs = ApbsBs.Position;

         IsicBs.DataSource = iRoboTech.Isic_Categories;
         GrphBs.DataSource = iRoboTech.Group_Headers;
         if(entityname == "")
            ApbsBs.DataSource = iRoboTech.App_Base_Defines;
         else
            ApbsBs.DataSource = iRoboTech.App_Base_Defines.Where(a => a.ENTY_NAME == entityname);

         IsicBs.Position = isic;
         GrphBs.Position = grph;
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
            Apbs_gv.PostEditor();

            IsicBs.EndEdit();
            GrphBs.EndEdit();
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
   }
}
