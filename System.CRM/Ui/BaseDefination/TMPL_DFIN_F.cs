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
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;

namespace System.CRM.Ui.BaseDefination
{
   public partial class TMPL_DFIN_F : UserControl
   {
      public TMPL_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formcaller = "";
      private string fileno;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         TempBs.DataSource = iCRM.Templates;
         requery = false;
      }

      private void AddTemp_Butn_Click(object sender, EventArgs e)
      {
         TempBs.AddNew();
         var temp = TempBs.Current as Data.Template;
         temp.SHER_TEAM = "002";
         temp.TEMP_TYPE = "001";
      }

      private void SaveTemp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            TempBs.EndEdit();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
            requery = false;
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Tmit_Lov.EditValue == null || Tmit_Lov.EditValue.ToString() == "") return;
            var tmit = TmitBs.List.OfType<Data.Template_Item>().FirstOrDefault(t => t.CODE == Convert.ToInt64(Tmit_Lov.EditValue));

            TempText_Txt.Text = TempText_Txt.Text.Insert(TempText_Txt.SelectionStart, tmit.PLAC_HLDR);
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void DelTemp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var temp = TempBs.Current as Data.Template;
            if (temp == null) return;

            if (MessageBox.Show(this, "آیا با حذف قالب متنی موافق هستید؟", "جذف قالب متنی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.Templates.DeleteOnSubmit(temp);

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
            requery = false;
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void RetrunTemplate_Butn_Click(object sender, EventArgs e)
      {
         if (!RetrunTemplate_Butn.Visible) return;

         var temp = TempBs.Current as Data.Template;
         if(temp == null)return;

         Back_Butn_Click(null, null);

         _DefaultGateway.Gateway(
            new Job(SendType.Self, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, formcaller, 100 /* Execute SetTemplateText */)
                  {
                     Input = 
                        new XElement("Template",
                           new XAttribute("tmid", temp.TMID),
                           new XElement("Temp_Text", 
                              (  formcaller == "OPT_AEML_F" ?                                  
                                 new XElement("Result", temp.TEMP_TEXT) 
                                 : 
                                 iCRM.GET_TEXT_F(
                                    new XElement("TemplateToText",
                                       new XAttribute("fileno", fileno),
                                       new XAttribute("tmid", temp.TMID)
                                    )
                                 )
                              )
                           )
                        )
                  }
               }
            )
         );
      }     
   }
}
