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
using System.CRM.ExceptionHandlings;

namespace System.CRM.Ui.Deals
{
   public partial class DTL_DEAL_F : UserControl
   {
      public DTL_DEAL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool needclose = true;
      private long fileno;
      private string srpbtype = "001";
      private bool islock = false;
      private long cashcode, rqstrqid, projrqstrqid;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);

         int pydt = PydtBs.Position;
         PymtBs.DataSource = iCRM.Payments.FirstOrDefault(p => p.RQST_RQID == rqstrqid && p.CASH_CODE == cashcode);
         PydtBs.Position = pydt;
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private string GetDateTimeString(DateTime? dt)
      {
         return
            string.Format("{0}-{1}-{2} {3}:{4}:{5}",
               dt.Value.Year,
               dt.Value.Month,
               dt.Value.Day,
               dt.Value.Hour,
               dt.Value.Minute,
               dt.Value.Second
            );
      }

      private void Expn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.Payment;
            if (pymt == null) return;

            var expn = ExpnBs.Current as Data.Expense;
            
            // چک میکنیم که قبلا از این آیتم هزینه در جدول ریز هزینه وجود نداشته باشد
            if (!PydtBs.List.OfType<Data.Payment_Detail>().Any(p => p.EXPN_CODE == expn.CODE))
            {
               PydtBs.AddNew();
               var pydt = PydtBs.Current as Data.Payment_Detail;
               ExpnBs.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.EXPN_CODE = ex.CODE; pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; pydt.PYDT_DESC = ex.EXPN_DESC; pydt.PAY_STAT = "001"; pydt.RQRO_RWNO = 1; });
            }
            else
            {
               var pydt = PydtBs.List.OfType<Data.Payment_Detail>().Where(p => p.EXPN_CODE == expn.CODE).First();
               ExpnBs.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.QNTY += 1; });
            }

            PydtBs.EndEdit();
            iCRM.SubmitChanges();

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
      }

      private void Pydt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.Payment;
            if (pymt == null) return;

            var pydt = PydtBs.Current as Data.Payment_Detail;

            switch (e.Button.Index)
            {
               case 0:
                  ++pydt.QNTY;
                  break;
               case 1:
                  if(pydt.QNTY > 1)
                     --pydt.QNTY;
                  break;
               case 3:
                  if (MessageBox.Show(this, "آیا با حذف آیتم موافق هستید؟", "حذف آیتم فاکتور", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  iCRM.DEL_PYDT_P(pydt.CODE, null, null, null, null);
                  break;
               default:
                  break;
            }

            PydtBs.EndEdit();
            iCRM.SubmitChanges();

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
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         Pydt_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Pydt_Butn.Buttons[2]));

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "DTL_DEAL_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                  new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 10 /* Execute Actn_CalF_P */)
                  {
                     Input = 
                        new XElement("Service", 
                              new XAttribute("fileno", fileno), 
                              new XAttribute("cashcode", cashcode), 
                              new XAttribute("rqid", rqstrqid), 
                              new XAttribute("projrqstrqid", projrqstrqid), 
                              new XAttribute("formcaller", "INF_CONT_F")
                           )
                  }
               }
            )
         );
      }

      private void ShowPymtMtod_Butn_Click(object sender, EventArgs e)
      {
         Pydt_Butn_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Pydt_Butn.Buttons[2]));

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 12 /* Execute Pay_Mtod_F */),
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 10 /* Execute Actn_CalF_P */)
                  {
                     Input = PymtBs.Current
                        /*new XElement("Service", 
                              new XAttribute("fileno", fileno), 
                              new XAttribute("cashcode", cashcode), 
                              new XAttribute("rqstrqid", rqstrqid), 
                              new XAttribute("srpbtype", "002"), 
                              new XAttribute("islock", true), 
                              new XAttribute("formcaller", "INF_CONT_F")
                           )*/
                  }
               }
            )
         );
      }
   }
}
