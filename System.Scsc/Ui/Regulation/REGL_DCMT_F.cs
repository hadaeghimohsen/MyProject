﻿using System;
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

      private string rqtpcode = "";

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
         EXCSBS.DataSource = iScsc.Expense_Cashes.Where(ec => ec.REGL_YEAR == Rg2.YEAR && ec.REGL_CODE == Rg2.CODE && ec.Expense_Type == ExtpBs.Current && Fga_Urgn_U.Split(',').Contains(ec.REGN_PRVN_CODE + ec.REGN_CODE));
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
                  iScsc.REGL_TOTL_P(
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
            }
         }
      }

      private void Execute_Query()
      {
         var CrntRqrq = RqrqBs.Position;
         var CrntExtp = ExtpBs.Position;
         var CrntExpn = ExpnBs.Position;
         var CrntExcs = EXCSBS.Position;
         var crntpexp = PexpBs1.Position;
         var crntexts = ExtsBs.Position;
         var crntcexc = CexcBs.Position;
         var crntbcds = BcdsBs1.Position;
         iScsc = new Data.iScscDataContext(ConnectionString);
         RqrqBs.DataSource = iScsc.Request_Requesters.Where(rg => rg.Regulation == (Data.Regulation)ReglBs.Current).OrderBy(rq => rq.RQTP_CODE).ThenBy(rq => rq.RQTT_CODE);
         GV_RQRQ.TopRowIndex = CrntRqrq;
         RqrqBs.Position = CrntRqrq;
         RqrqBs.MoveNext();
         RqrqBs.MovePrevious();
         ExtpBs.Position = CrntExtp;
         ExpnBs.Position = CrntExpn;
         EXCSBS.Position = CrntExcs;
         PexpBs1.Position = crntpexp;
         ExtsBs.Position = crntexts;
         CexcBs.Position = crntcexc;
         BcdsBs1.Position = crntbcds;
         requery = false;
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
               GropBs.DataSource = iScsc.Group_Expenses.Where(ge => ge.GROP_TYPE == "001").ToList();
               BrndBs.DataSource = iScsc.Group_Expenses.Where(ge => ge.GROP_TYPE == "002").ToList();
               break;
            case 3:
               if (MessageBox.Show(this, "آیا با گروه بندی درآمد ها موافق هستید؟", "گروه بندی", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

               if (MessageBox.Show(this, "آیا همه رکوردها بروز شوند؟", "گروه بندی", MessageBoxButtons.YesNo) == DialogResult.Yes)
                  iScsc.ExecuteCommand(
                     "INSERT INTO dbo.Group_Expense(Code, Grop_Type, Ordr, Grop_Desc, Stat)" + Environment.NewLine + 
                     "SELECT m.Code, '001', m.natl_code, m.Mtod_Desc, '002' FROM dbo.Method m" + Environment.NewLine + 
                     "WHERE m.Code NOT IN (SELECT ge.Code FROM dbo.Group_Expense ge);" + Environment.NewLine + 
                     "UPDATE Expense SET Grop_Code = Mtod_Code;"
                  );
               else
                  iScsc.ExecuteCommand(
                     "INSERT INTO dbo.Group_Expense(Code, Grop_Type, Ordr, Grop_Desc, Stat)" + Environment.NewLine +
                     "SELECT m.Code, '001', m.natl_code, m.Mtod_Desc, '002' FROM dbo.Method m" + Environment.NewLine +
                     "WHERE m.Code NOT IN (SELECT ge.Code FROM dbo.Group_Expense ge);" + Environment.NewLine +
                     "UPDATE Expense SET Grop_Code = Mtod_Code WHERE Grop_Code IS NULL;"
                  );
               SubmitRqrq_Click(null, null);
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

            expn.EXPN_DESC = 
               string.Format("{0} {1}، {2}",  
               ExtpDesc_Cbx.Checked ? expn.Expense_Type.Expense_Item.EPIT_DESC : "", 
               MtodDesc_Cbx.Checked ? expn.Method.MTOD_DESC : "", 
               CtgyDesc_Cbx.Checked ? expn.Category_Belt.CTGY_DESC : ""
            );
         }
         catch (Exception )
         {

         }
      }

      private void SetAllExpnDesc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
               foreach (var _extp in ExtpBs.List.OfType<Data.Expense_Type>())
               {
                  _extp.Expenses
                     .ToList()
                     .ForEach(_expn => 
                        _expn.EXPN_DESC =
                           string.Format("{0} {1}، {2}",
                           ExtpDesc_Cbx.Checked ? _expn.Expense_Type.Expense_Item.EPIT_DESC : "",
                           MtodDesc_Cbx.Checked ? _expn.Method.MTOD_DESC : "",
                           CtgyDesc_Cbx.Checked ? _expn.Category_Belt.CTGY_DESC : ""
                        )
                     );
               }
            }
            else
            {
               ExpnBs.MoveFirst();
               foreach (var expn in ExpnBs.List.OfType<Data.Expense>())
               {
                  expn.EXPN_DESC =
                     string.Format("{0} {1}، {2}",
                     ExtpDesc_Cbx.Checked ? expn.Expense_Type.Expense_Item.EPIT_DESC : "",
                     MtodDesc_Cbx.Checked ? expn.Method.MTOD_DESC : "",
                     CtgyDesc_Cbx.Checked ? expn.Category_Belt.CTGY_DESC : ""
                  );
               }
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
         if (ModifierKeys.HasFlag(Keys.Control))
         {
            var _rqrq = RqrqBs.Current as Data.Request_Requester;
            iScsc.ExecuteCommand(
               string.Format(
                  "MERGE dbo.Expense_Type T" + Environment.NewLine +
                  "USING (SELECT CODE AS EPIT_CODE, {0} AS RQRO_CODE FROM dbo.Expense_Item WHERE TYPE = '001') S" + Environment.NewLine +
                  "ON (T.RQRQ_CODE = S.RQRO_CODE AND" + Environment.NewLine +
                  "T.EPIT_CODE = S.EPIT_CODE)" + Environment.NewLine +
                  "WHEN NOT MATCHED THEN" + Environment.NewLine +
                  "INSERT (RQRQ_CODE, EPIT_CODE, CODE)" + Environment.NewLine +
                  "VALUES (S.RQRO_CODE, S.EPIT_CODE, dbo.GNRT_NVID_U());",
                  _rqrq.CODE
               )
            );
         }
         else
         {
            // 1403/06/01 * اگر رکوردی ذخیره نشده وجود دارد بتوانیم تکلیف آن را مشخص کنیم
            if (ExtpBs.List.OfType<Data.Expense_Type>().Any(et => et.CODE == 0)) return;

            ExtpBs.AddNew();
         }
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
                        new XAttribute("code", (ExtpBs.Current as Data.Expense_Type).CODE)
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
            var crnt = ExtpBs.Current as Data.Expense_Type;
            iScsc.REGL_TOTL_P(
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

      private void ExpnBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs.Current as Data.Expense;
            if (expn == null) return;

            Grop_Lov.EditValue = expn.GROP_CODE;

            // 1397/10/08 * بارگذاری اطلاعات مربوط به تخفیفات سیستم
            //BcdsBs1.DataSource = 
            //   iScsc.Basic_Calculate_Discounts
            //   .Where(
            //      b =>
            //         b.Regulation == expn.Regulation &&
            //         b.RQTP_CODE == rqtpcode &&
            //         b.RQTT_CODE == expn.Expense_Type.Request_Requester.RQTT_CODE &&
            //         b.Category_Belt == expn.Category_Belt &&
            //         b.Expense_Item == expn.Expense_Type.Expense_Item
            //   );
         }
         catch { }
      }

      private void GropIng_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            var gropcode = Grop_Lov.EditValue;
            if(gropcode == null || gropcode.ToString() == "")return;

            iScsc.UPD_EXPN_P(_expn.CODE, _expn.PRIC, _expn.EXPN_STAT, _expn.ADD_QUTS, _expn.COVR_DSCT, _expn.EXPN_TYPE, _expn.BUY_PRIC, _expn.BUY_EXTR_PRCT, _expn.NUMB_OF_STOK, _expn.NUMB_OF_SALE, _expn.COVR_TAX, _expn.NUMB_OF_ATTN_MONT, _expn.NUMB_OF_ATTN_WEEK, _expn.MODL_NUMB_BAR_CODE, _expn.PRVT_COCH_EXPN, _expn.NUMB_CYCL_DAY, _expn.NUMB_MONT_OFER, _expn.MIN_NUMB, (long)gropcode, _expn.EXPN_DESC, _expn.MIN_TIME, _expn.RELY_CMND, _expn.ORDR_ITEM, _expn.BRND_CODE, _expn.MIN_PRIC, _expn.MAX_PRIC, _expn.UNIT_APBS_CODE, _expn.CAN_CALC_PROF, _expn.MUST_FILL_OWNR);
            requery = true;
         }
         catch { }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void DelGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            iScsc.UPD_EXPN_P(_expn.CODE, _expn.PRIC, _expn.EXPN_STAT, _expn.ADD_QUTS, _expn.COVR_DSCT, _expn.EXPN_TYPE, _expn.BUY_PRIC, _expn.BUY_EXTR_PRCT, _expn.NUMB_OF_STOK, _expn.NUMB_OF_SALE, _expn.COVR_TAX, _expn.NUMB_OF_ATTN_MONT, _expn.NUMB_OF_ATTN_WEEK, _expn.MODL_NUMB_BAR_CODE, _expn.PRVT_COCH_EXPN, _expn.NUMB_CYCL_DAY, _expn.NUMB_MONT_OFER, _expn.MIN_NUMB, null, _expn.EXPN_DESC, _expn.MIN_TIME, _expn.RELY_CMND, _expn.ORDR_ITEM, _expn.BRND_CODE, _expn.MIN_PRIC, _expn.MAX_PRIC, _expn.UNIT_APBS_CODE, _expn.CAN_CALC_PROF, _expn.MUST_FILL_OWNR);
         }
         catch { }
      }

      private void DelRqdc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqdc = RqdcBs2.Current as Data.Request_Document;
            if (rqdc == null) return;

            iScsc.DEL_RQDC_P(rqdc.RDID);

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
               Execute_Query();
         }
      }

      private void AddBcds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (BcdsBs1.List.OfType<Data.Basic_Calculate_Discount>().Any(b => b.RWNO == 0)) return;

            var expn = ExpnBs.Current as Data.Expense;
            if(expn == null)return;

            var bcds = BcdsBs1.AddNew() as Data.Basic_Calculate_Discount;
            bcds.REGL_YEAR = (short)expn.REGL_YEAR;
            bcds.REGL_CODE = (short)expn.REGL_CODE;
            bcds.CTGY_CODE = expn.CTGY_CODE;
            bcds.MTOD_CODE = expn.MTOD_CODE;
            bcds.RQTP_CODE = rqtpcode;
            bcds.RQTT_CODE = expn.Expense_Type.Request_Requester.RQTT_CODE;
            bcds.EPIT_CODE = expn.Expense_Type.EPIT_CODE;
            bcds.EXPN_CODE = expn.CODE;

            iScsc.Basic_Calculate_Discounts.InsertOnSubmit(bcds);
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

      private void DelBcds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crntobj = BcdsBs1.Current as Data.Basic_Calculate_Discount;
            if (crntobj != null && MessageBox.Show(this, "آیا با حذف آیتم انتخاب شده موافقید؟", "عملیات حذف", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;

            iScsc.Basic_Calculate_Discounts.DeleteOnSubmit(crntobj);
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
               Execute_Query();
         }
      }

      private void SaveBcds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SubmitRqrq_Click(null, null);
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

      private void Sunt_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var bcds = BcdsBs1.Current as Data.Basic_Calculate_Discount;
            if (bcds == null) return;

            var sunt = SuntBs1.List.OfType<Data.Sub_Unit>().FirstOrDefault(s => s.CODE == e.NewValue);
            bcds.SUNT_BUNT_DEPT_ORGN_CODE = sunt.BUNT_DEPT_ORGN_CODE;
            bcds.SUNT_BUNT_DEPT_CODE = sunt.BUNT_DEPT_CODE;
            bcds.SUNT_BUNT_CODE = sunt.BUNT_CODE;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void BaseInfo_Butn_Click(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 144 /* Execute Bas_Dfin_F */),
                new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Actn_CalF_P */)
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void BackExtp_Butn_Click(object sender, EventArgs e)
      {
         Extp_gv.MovePrev();
         //EXTPBS.MovePrevious();
      }

      private void NextExtp_Butn_Click(object sender, EventArgs e)
      {
         Extp_gv.MoveNext();
         //EXTPBS.MoveNext();
      }

      private void BrndLng_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            var brndcode = Brnd_Lov.EditValue;
            if (brndcode == null || brndcode.ToString() == "") return;

            iScsc.UPD_EXPN_P(_expn.CODE, _expn.PRIC, _expn.EXPN_STAT, _expn.ADD_QUTS, _expn.COVR_DSCT, _expn.EXPN_TYPE, _expn.BUY_PRIC, _expn.BUY_EXTR_PRCT, _expn.NUMB_OF_STOK, _expn.NUMB_OF_SALE, _expn.COVR_TAX, _expn.NUMB_OF_ATTN_MONT, _expn.NUMB_OF_ATTN_WEEK, _expn.MODL_NUMB_BAR_CODE, _expn.PRVT_COCH_EXPN, _expn.NUMB_CYCL_DAY, _expn.NUMB_MONT_OFER, _expn.MIN_NUMB, _expn.GROP_CODE, _expn.EXPN_DESC, _expn.MIN_TIME, _expn.RELY_CMND, _expn.ORDR_ITEM, (long)brndcode, _expn.MIN_PRIC, _expn.MAX_PRIC, _expn.UNIT_APBS_CODE, _expn.CAN_CALC_PROF, _expn.MUST_FILL_OWNR);
            requery = true;
         }
         catch { }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void DelBrnd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            iScsc.UPD_EXPN_P(_expn.CODE, _expn.PRIC, _expn.EXPN_STAT, _expn.ADD_QUTS, _expn.COVR_DSCT, _expn.EXPN_TYPE, _expn.BUY_PRIC, _expn.BUY_EXTR_PRCT, _expn.NUMB_OF_STOK, _expn.NUMB_OF_SALE, _expn.COVR_TAX, _expn.NUMB_OF_ATTN_MONT, _expn.NUMB_OF_ATTN_WEEK, _expn.MODL_NUMB_BAR_CODE, _expn.PRVT_COCH_EXPN, _expn.NUMB_CYCL_DAY, _expn.NUMB_MONT_OFER, _expn.MIN_NUMB, _expn.GROP_CODE, _expn.EXPN_DESC, _expn.MIN_TIME, _expn.RELY_CMND, _expn.ORDR_ITEM, null, _expn.MIN_PRIC, _expn.MAX_PRIC, _expn.UNIT_APBS_CODE, _expn.CAN_CALC_PROF, _expn.MUST_FILL_OWNR);
         }
         catch { }
      }

      private void AddExco_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            if (ExcoBs.List.OfType<Data.Expense_Cost>().Any(ec => ec.CODE == 0)) return;

            var _exco = ExcoBs.AddNew() as Data.Expense_Cost;
            iScsc.Expense_Costs.InsertOnSubmit(_exco);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelExco_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _exco = ExcoBs.Current as Data.Expense_Cost;
            if (_exco == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Expense_Costs.DeleteOnSubmit(_exco);
            iScsc.SubmitChanges();
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

      private void SaveExco_Butn_Click(object sender, EventArgs e)
      {
         SubmitRqrq_Click(null, null);
      }

      private void AExco_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Expense_Cost"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AExcoLoad_Butn_Click(object sender, EventArgs e)
      {

      }

      private void CalcExco_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            iScsc.CALC_EXCO_P(
               new XElement("Expense",
                   new XAttribute("code", _expn.CODE),
                   new XAttribute("pric", _expn.PRIC)
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
            if (requery)
               Execute_Query();
         }
      }

      private void AddExts_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _extp = ExtpBs.Current as Data.Expense_Type;
            if (_extp == null) return;

            if (ExtsBs.List.OfType<Data.Expense_Type_Step>().Any(ets => ets.CODE == 0)) return;

            var _exts = ExtsBs.AddNew() as Data.Expense_Type_Step;
            iScsc.Expense_Type_Steps.InsertOnSubmit(_exts);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelExts_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _exts = ExtsBs.Current as Data.Expense_Type_Step;
            if (_exts == null) return;

            if(iScsc.Payment_Details.Any(pd => pd.EXTS_CODE == _exts.CODE))
            {
               throw new Exception("در جدول اقلام فاکتور ها سوابقی وجود دارد که دیگر نمیتوان این رکورد را پاک کنید، لطفا آن را غیرفعال کنید");
            }

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید?", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.Expense_Type_Step WHERE Code = {0};", _exts.CODE));
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

      private void SaveExts_Butn_Click(object sender, EventArgs e)
      {
         SubmitRqrq_Click(null, null);
      }

      private void AddCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _expn = ExpnBs.Current as Data.Expense;
            if (_expn == null) return;

            if (CexcBs.List.OfType<Data.Calculate_Expense_Coach>().Any(c => c.CODE == 0)) return;
            var _cexc = CexcBs.AddNew() as Data.Calculate_Expense_Coach;

            _cexc.EXPN_CODE = _expn.CODE;
            _cexc.EPIT_CODE = _expn.Expense_Type.EPIT_CODE;
            _cexc.EXTP_CODE = _expn.EXTP_CODE;
            _cexc.MTOD_CODE = _expn.MTOD_CODE;
            _cexc.CTGY_CODE = _expn.CTGY_CODE;
            _cexc.RQTP_CODE = _expn.Expense_Type.Request_Requester.RQTP_CODE;
            _cexc.RQTT_CODE = _expn.Expense_Type.Request_Requester.RQTT_CODE;
            _cexc.CALC_EXPN_TYPE = _cexc.RQTP_CODE == "016" ? null : "001";
            _cexc.CALC_TYPE = "001";
            _cexc.PRCT_VALU = 10;
            _cexc.STAT = "002";
            _cexc.PYMT_STAT = "002";            
            _cexc.EFCT_DATE_TYPE = _cexc.RQTP_CODE == "016" ? "004" : "002";

            iScsc.Calculate_Expense_Coaches.InsertOnSubmit(_cexc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Calculate_Expense_Coaches.DeleteOnSubmit(_cexc);
            iScsc.SubmitChanges();
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

      private void SaveCexc_Butn_Click(object sender, EventArgs e)
      {
         SubmitRqrq_Click(null, null);
      }

      private void DupCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null) return;

            iScsc.DUP_CEXC_P(
               new XElement("OpIran",                   
                   new XAttribute("cexccode", _cexc.CODE)
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
            if (requery)
               Execute_Query();
         }
      }

      private void Coch_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var _cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (_cexc == null || _cexc.CODE == 0) return;

            iScsc.ExecuteCommand(
               string.Format("UPDATE dbo.Calculate_Expense_Coach SET Coch_File_No = {0} WHERE Code = {1};", e.NewValue, _cexc.CODE)
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

      private void CopyRsvr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _extp = ExtpBs.Current as Data.Expense_Type;
            if (_extp == null) return;

            iScsc.DUP_EXTS_P(
               new XElement("OpIran",
                   new XAttribute("extpcode", _extp.CODE),
                   new XAttribute("fromhour", string.Format("{0}:00", FromHour_Txt.Text)),
                   new XAttribute("tohour", string.Format("{0}:00", ToHour_Txt.Text)),
                   new XAttribute("gaphour", GapHour_Txt.Text),
                   new XAttribute("qnty", Qnty_Txt.Text)
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
               Execute_Query();
         }
      }

      private void CopyOExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _bcd = BcdsBs1.Current as Data.Basic_Calculate_Discount;
            if (_bcd == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Basic_Calculate_Discount T
                  USING (SELECT e.CODE AS EXPN_CODE, 
                                e.MTOD_CODE, e.CTGY_CODE, 
                                bd.SUNT_BUNT_DEPT_ORGN_CODE, 
                                bd.SUNT_BUNT_DEPT_CODE, 
                                bd.SUNT_BUNT_CODE, 
                                bd.SUNT_CODE,
                                bd.ORGN_CODE_DNRM,
                                e.REGL_YEAR, e.REGL_CODE,
                                et.EPIT_CODE, rr.RQTP_CODE, rr.RQTT_CODE,
                                bd.AMNT_DSCT, bd.PRCT_DSCT, bd.DSCT_TYPE, bd.STAT, bd.ACTN_TYPE, bd.FROM_DATE, bd.TO_DATE
                           FROM dbo.Expense e, 
                                dbo.Expense_Type et, dbo.Request_Requester rr,
                                dbo.Basic_Calculate_Discount bd
                          WHERE bd.Code = '{0}'
                            AND e.EXTP_CODE = et.CODE
                            AND et.RQRQ_CODE = rr.CODE) S
                  ON (T.ORGN_CODE_DNRM = S.ORGN_CODE_DNRM AND 
                      T.EXPN_CODE = S.EXPN_CODE AND 
                      T.Actn_Type = s.Actn_Type )
                  WHEN NOT MATCHED THEN 
                     INSERT (SUNT_BUNT_DEPT_ORGN_CODE, SUNT_BUNT_DEPT_CODE, SUNT_BUNT_CODE, SUNT_CODE, 
                             REGL_YEAR, REGL_CODE, CODE, EPIT_CODE, RQTP_CODE, RQTT_CODE, AMNT_DSCT, 
                             PRCT_DSCT, DSCT_TYPE, STAT, ACTN_TYPE, FROM_DATE, TO_DATE, MTOD_CODE, CTGY_CODE, EXPN_CODE)
                     VALUES (s.SUNT_BUNT_DEPT_ORGN_CODE, s.SUNT_BUNT_DEPT_CODE, s.SUNT_BUNT_CODE, s.SUNT_CODE, 
                             s.REGL_YEAR, s.REGL_CODE, dbo.GNRT_NVID_U(), s.EPIT_CODE, s.RQTP_CODE, s.RQTT_CODE, s.AMNT_DSCT,
                             s.PRCT_DSCT, s.DSCT_TYPE, s.STAT, s.ACTN_TYPE, s.FROM_DATE, s.TO_DATE, s.MTOD_CODE, s.CTGY_CODE, s.EXPN_CODE)
                  WHEN MATCHED THEN 
                     UPDATE SET
                        T.DSCT_TYPE = s.DSCT_TYPE,
                        T.AMNT_DSCT = s.AMNT_DSCT,
                        T.PRCT_DSCT = s.PRCT_DSCT,
                        T.STAT = S.STAT,
                        T.FROM_DATE = S.FROM_DATE,
                        T.TO_DATE = S.TO_DATE;", _bcd.CODE
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
            if (requery)
               Execute_Query();
         }
      }
   }
}
