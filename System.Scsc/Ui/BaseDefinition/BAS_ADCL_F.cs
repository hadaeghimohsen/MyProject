using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_ADCL_F : UserControl
   {
      public BAS_ADCL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ClubName_Text.Text == "") { ClubName_Text.Focus(); return; }
            
            if (Regn_Lov.EditValue == null || Regn_Lov.EditorContainsFocus.ToString() == "") { Regn_Lov.Focus(); return; }

            var regn = RegnBs1.List.OfType<Data.Region>().FirstOrDefault(r => r.CODE == Regn_Lov.EditValue.ToString());

            var club = ClubBs.Current as Data.Club;
            club.Region = regn;

            iScsc.SubmitChanges();
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
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("TabPage",
                                 new XAttribute("showtabpage", "tp_006")
                              )
                        }
                     }
                  )
               );
               Back_Butn_Click(null, null);
               requery = false;
            }
         }
      }
   }
}
