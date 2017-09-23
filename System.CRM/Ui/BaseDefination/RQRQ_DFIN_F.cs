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
using System.CRM.ExceptionHandlings;
using System.Xml.Linq;
using System.Data.SqlClient;


namespace System.CRM.Ui.BaseDefination
{
   public partial class RQRQ_DFIN_F : UserControl
   {
      public RQRQ_DFIN_F()
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
         iCRM = new Data.iCRMDataContext(ConnectionString);
         int _Rqrq = RqrqBs.Position;
         int _Extp = ExtpBs.Position;
         int _Expn = ExpnBs.Position;
         int _Excs = ExcsBs.Position;
         int _Dcsp = DcspBs.Position;
         int _Rqdc = RqdcBs.Position;

         RqrqBs.DataSource = iCRM.Request_Requesters.Where(rqrq => rqrq.Regulation == crntRegulation);
         
         RqrqBs.Position = _Rqrq;
         ExtpBs.Position = _Extp;
         ExpnBs.Position = _Expn;
         DcspBs.Position = _Dcsp;
         RqdcBs.Position = _Rqdc;
      }

      private void Refresh_Clicked(object sender, EventArgs e)
      {
         Execute_Query();
         requery = false;
      }

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            RqrqBs.EndEdit();
            ExtpBs.EndEdit();
            ExpnBs.EndEdit();
            ExcsBs.EndEdit();
            DcspBs.EndEdit();
            RqdcBs.EndEdit();
            
            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void InvsRegl_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {

      }

      private void Actv_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var extp = ExtpBs.Current as Data.Expense_Type;
         switch (e.Button.Index)
         {
            case 0:
               extp.Expenses.ToList().ForEach(ex => ex.EXPN_STAT = "002");
               break;
            case 1:
               extp.Expenses.ToList().ForEach(ex => ex.EXPN_STAT = "001");
               break;
            case 2:
               try
               {
                  if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  iCRM.DEL_EXTP_P(extp.CODE);
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
               break;
            default:
               break;
         }
      }

      private void ExpnAcDc_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {         
         var expn = ExpnBs.Current as Data.Expense;
         switch (e.Button.Index)
         {
            case 0:
               expn.EXPN_STAT = expn.EXPN_STAT == "001" ? "002" : "001";
               break;
            default:
               break;
         }         
      }

      private void Excs_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var excs = ExcsBs.Current as Data.Expense_Cash;
         switch (e.Button.Index)
         {
            case 0:
               excs.EXCS_STAT = excs.EXCS_STAT == "001" ? "002" : "001";
               break;
            default:
               break;
         }
      }

      private void Dcsp_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var rqdc = RqdcBs.Current as Data.Request_Document;
         switch (e.Button.Index)
         {
            case 0:
               DcspBs.DataSource = iCRM.Document_Specs.ToList();
               break;
            case 1:
               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 09 /* Execute Dcsp_Dfin_F */),                
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
               break;
            case 2:
               try
               {
                  if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  iCRM.DEL_RQDC_P(rqdc.RDID);
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
               break;
            default:
               break;
         }
      }

      private void TotlPricDiscrt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs.Current as Data.Expense;
            var regl = expn.Expense_Type.Request_Requester.Regulation;

            int totlpric = Convert.ToInt32(TotlPric_Txt.EditValue);
            
            expn.PRIC = (int)((100 * totlpric) / (100 + regl.TAX_PRCT + regl.DUTY_PRCT));
            expn.EXPN_STAT = "002";
            expn.COVR_TAX = "002";
            TotlPric_Txt.EditValue = "";
            ExpnBs.MoveNext();
            eXPN_DESCTextEdit.Focus();
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }         
      }

      bool ExpandedOrCollapsed = true;
      private void OpenCloseExpnGroup_Butn_Click(object sender, EventArgs e)
      {
         if (ExpandedOrCollapsed)
         {
            Expn_GridView.CollapseAllGroups();
            ExpandedOrCollapsed = false;
         }
         else
         {
            Expn_GridView.ExpandAllGroups();
            ExpandedOrCollapsed = true;
         }
      }

      private void Extp_Gc_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            ExtpBs.EndEdit();
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با حذف کردن نوع هزینه موافقید؟", "حذف نوع هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iCRM.REGL_TOTL_P(
                     new XElement("Config",
                        new XAttribute("type", "001"),
                        new XElement("Delete",
                           new XElement("Expense_Type",
                              new XAttribute("code", (ExtpBs.Current as Data.Expense_Type).CODE)
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
                  var crnt = ExtpBs.Current as Data.Expense_Type;
                  iCRM.REGL_TOTL_P(
                     new XElement("Config",
                        new XAttribute("type", "001"),
                        ExtpBs.List.OfType<Data.Expense_Type>().Where(c => c.CRET_BY == null).Select(c =>
                           new XElement("Insert",
                              new XElement("Expense_Type",
                                 new XAttribute("rqrqcode", (RqrqBs.Current as Data.Request_Requester).CODE),
                                 new XAttribute("epitcode", c.EPIT_CODE)
                              )
                           )
                        ),
                        crnt.CRET_BY != null ?
                           new XElement("Update",
                              new XElement("Expense_Type",
                                 new XAttribute("code", crnt.CODE),
                                 new XAttribute("extpdesc", crnt.EXTP_DESC)
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
               default:
                  MessageBox.Show(se.Message);
                  break;
            }
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

      private void Rqdc_Gc_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            RqdcBs.EndEdit();
            var rqdc = RqdcBs.Current as Data.Request_Document;

            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با حذف کردن نوع مدرک موافقید؟", "حذف نوع مدرک", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  
                  iCRM.DEL_RQDC_P(rqdc.RDID);
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  iCRM.SubmitChanges();
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
               default:
                  MessageBox.Show(se.Message);
                  break;
            }
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
   }
}
