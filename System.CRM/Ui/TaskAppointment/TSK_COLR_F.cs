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
using DevExpress.XtraEditors;
using System.MaxUi;

namespace System.CRM.Ui.TaskAppointment
{
   public partial class TSK_COLR_F : UserControl
   {
      public TSK_COLR_F()
      {
         InitializeComponent();
      }

      private long fileno;
      private long tkid;
      private string formcaller;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void SelectedColor_Butn_Click(object sender, EventArgs e)
      {
         var colr = sender as RoundedButton;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface,GetType().Name, 00 /* Execute ProcessCmdKey */) { Input = Keys.Escape },
                  new Job(SendType.SelfToUserInterface, formcaller, 100 /* Execute SetColor */)
                  {
                        Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno), 
                           new XAttribute("tkid", tkid),
                           //new XAttribute("tasktype", "new"),
                           new XAttribute("colr", ColorTranslator.ToHtml(colr.NormalColorB))
                        )
                     }
               }
            )
         );
      }

      private void ChosOthrColr_Butn_Click(object sender, EventArgs e)
      {
         if (OthrColr_Cd.ShowDialog() != DialogResult.OK) return;
         ChosOthrColr_Butn.NormalColorA = ChosOthrColr_Butn.NormalColorB = ChosOthrColr_Butn.HoverColorA = ChosOthrColr_Butn.HoverColorB = OthrColr_Cd.Color;

         SelectedColor_Butn_Click(ChosOthrColr_Butn, null);
      }
   }
}
