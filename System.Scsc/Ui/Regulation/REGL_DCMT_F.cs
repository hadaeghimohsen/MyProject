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
using System.Data.SqlClient;

namespace System.Scsc.Ui.Regulation
{
   public partial class REGL_DCMT_F : UserControl
   {
      public REGL_DCMT_F()
      {
         InitializeComponent();
      }

      partial void SubmitRqrq_Click(object sender, EventArgs e);

      partial void SubmitRqdc_Click(object sender, EventArgs e);

      partial void HL_NEW_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      partial void RqrqBs_BindingComplete(object sender, BindingCompleteEventArgs e);

      partial void request_RequestersBindingSource_PositionChanged(object sender, EventArgs e);

      partial void RqdcBsAddNewItem_Click(object sender, EventArgs e);

      partial void Btn_SaveRqdc_Click(object sender, EventArgs e);

      partial void Btn_Cncl_Click(object sender, EventArgs e);

      partial void HL_ACTV_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      private void EXTPBS_CurrentChanged(object sender, EventArgs e)
      {
         if(Rg2 == null)
            Rg2 = iScsc.Regulations.Where(rg => rg.TYPE == "002" && rg.REGL_STAT == "002").SingleOrDefault();
         EXCSBS.DataSource = iScsc.Expense_Cashes.Where(ec => ec.REGL_YEAR == Rg2.YEAR && ec.REGL_CODE == Rg2.CODE && ec.Expense_Type == EXTPBS.Current && Fga_Urgn_U.Split(',').Contains(ec.REGN_PRVN_CODE + ec.REGN_CODE));
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      bool requery = false;

      private void expense_TypesGridControl_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با حذف کردن نوع هزینه موافقید؟", "حذف نوع هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iScsc.REGL_TOTL_P(
                     new XElement("Config",
                        new XAttribute("type", "001"),
                        new XElement("Delete",
                           new XElement("Expense_Type",
                              new XAttribute("code", (EXTPBS.Current as Data.Expense_Type).CODE)                              
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
                  var crnt = EXTPBS.Current as Data.Expense_Type;
                  iScsc.REGL_TOTL_P(
                     new XElement("Config",
                        new XAttribute("type", "001"),
                        EXTPBS.List.OfType<Data.Expense_Type>().Where(c => c.CRET_BY == null).Select(c =>
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
            }
         }
      }

      private void Execute_Query()
      {
         var CrntRqrq = RqrqBs.Position;
         var CrntExtp = EXTPBS.Position;
         var CrntExpn = ExpnBs.Position;
         var CrntExcs = EXCSBS.Position;
         iScsc = new Data.iScscDataContext(ConnectionString);
         RqrqBs.DataSource = iScsc.Request_Requesters.Where(rg => rg.Regulation == (Data.Regulation)ReglBs.Current).OrderBy(rq => rq.RQTP_CODE).ThenBy(rq => rq.RQTT_CODE);
         GV_RQRQ.TopRowIndex = CrntRqrq;
         RqrqBs.Position = CrntRqrq;
         EXTPBS.Position = CrntExtp;
         ExpnBs.Position = CrntExpn;
         EXCSBS.Position = CrntExcs;
         requery = false;
      }

      private void EXPN_TYPELookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
          //if (EXPN_TYPELookUpEdit.EditValue == null || EXPN_TYPELookUpEdit.EditValue.ToString().Length != 3 ) return;
          //if(EXPN_TYPELookUpEdit.EditValue.ToString() == "001")
          //{
          //    SE_BuyPric.Enabled = /*SE_BuyExtrPrct.Enabled = */SE_NumbOfStok.Enabled = SE_NumbOfSale.Enabled = SE_NumbOfRemnDnrm.Enabled = false;
          //}
          //else if(EXPN_TYPELookUpEdit.EditValue.ToString() == "002")
          //{
          //    SE_BuyPric.Enabled = /*SE_BuyExtrPrct.Enabled = */SE_NumbOfStok.Enabled = SE_NumbOfSale.Enabled = SE_NumbOfRemnDnrm.Enabled = true;
          //}
      }

      private void AddPreExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PexpBs1.AddNew();
            var crnt = PexpBs1.Current as Data.Pre_Expense;
            crnt.FREE_STAT = "001";
            crnt.STAT = "002";
            crnt.QNTY = 1;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelPreExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PexpBs1.EndEdit();

            var crnt = PexpBs1.Current as Data.Pre_Expense;

            if (crnt == null) return;

            if (MessageBox.Show(this, "آیا با حذف هزینه موافق هستین؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Pre_Expenses.DeleteOnSubmit(crnt);

            iScsc.SubmitChanges();

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
               SubmitRqrq_Click(null, null);
               requery = false;
            }
         }
      }

      private void Grop_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         switch (e.Button.Index)
         {
            case 0:
               break;
            case 1:
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
                              "<Privilege>37</Privilege><Sub_Sys>5</Sub_Sys>", 
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
                  new Job(SendType.Self, 10 /* Execute Mstr_Epit_F */)
                  {
                     Input = 
                        new XElement("Action",
                           new XAttribute("type", "002")
                        )
                  }
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
               break;
            case 2:
               GropBs.DataSource = iScsc.Group_Expenses.ToList();
               break;
            default:
               break;
         }
      }

      private void SetDescExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs.Current as Data.Expense;
            if (expn == null) return;

            expn.EXPN_DESC = string.Format("{0} {1}، {2}", expn.Expense_Type.Expense_Item.EPIT_DESC, expn.Method.MTOD_DESC, expn.Category_Belt.CTGY_DESC);
         }
         catch (Exception )
         {

         }
      }

      private void SetAllExpnDesc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ExpnBs.MoveFirst();
            foreach (var expn in ExpnBs.List.OfType<Data.Expense>())
            {
               expn.EXPN_DESC = string.Format("{0} {1}، {2}", expn.Expense_Type.Expense_Item.EPIT_DESC, expn.Method.MTOD_DESC, expn.Category_Belt.CTGY_DESC);
            }
         }
         catch (Exception )
         {}
      }

      private void SetCashActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var excs = EXCSBS.Current as Data.Expense_Cash;
            if (excs == null) return;

            excs.EXCS_STAT = excs.EXCS_STAT == "001" ? "002" : "001";

            iScsc.SubmitChanges();
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
            }
         }
      }

      private void AddExtp_Butn_Click(object sender, EventArgs e)
      {
         EXTPBS.AddNew();
      }

      private void DeleExtp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف کردن نوع هزینه موافقید؟", "حذف نوع هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.REGL_TOTL_P(
               new XElement("Config",
                  new XAttribute("type", "001"),
                  new XElement("Delete",
                     new XElement("Expense_Type",
                        new XAttribute("code", (EXTPBS.Current as Data.Expense_Type).CODE)
                     )
                  )
               )
            );
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
            }
         }
      }

      private void SaveExtp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = EXTPBS.Current as Data.Expense_Type;
            iScsc.REGL_TOTL_P(
               new XElement("Config",
                  new XAttribute("type", "001"),
                  EXTPBS.List.OfType<Data.Expense_Type>().Where(c => c.CRET_BY == null).Select(c =>
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
            }
         }
      }

      private void ExpnMtodActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var expn = ExpnBs.Current as Data.Expense;
            if (expn == null) return;

            ExpnBs.List.OfType<Data.Expense>().Where(ex => ex.MTOD_CODE == expn.MTOD_CODE).ToList().ForEach(ex => ex.EXPN_STAT = ex.EXPN_STAT == "001" ? "002" : "001");

            iScsc.SubmitChanges();
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
            }
         }
      }
   }
}
