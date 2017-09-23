using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Regulation
{
   partial class REGL_EXPN_F
   {
      partial void SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            ExpnBs1.EndEdit();

            iScsc.SubmitChanges();
            iScsc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, iScsc.Expenses);
            ExpnBs1.DataSource = iScsc.Expenses;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      partial void Btn_Epit_Click(object sender, EventArgs e)
      {
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
                           new XAttribute("type", "001")
                        )
                  }
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      partial void Btn_ReglDcmt_Click(object sender, EventArgs e)
      {
         //var Current = ReglBs.Current as Data.Regulation;
         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, Rg2}}
                  })
            );
      }

      partial void Bt_INDP_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با تغییر گروهی هزینه های آیین نامه مطمئن هستید", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            Func<XElement> GetRqtp = 
               new Func<XElement>(
               () =>
               {
                  XElement X = 
                     new XElement("Request_Types",
                        CL_RQTP.Properties
                        .GetItems()
                        .OfType<CheckedListBoxItem>()
                        .Where(r => r.CheckState == CheckState.Checked)
                        .Select(r => 
                           new XElement("Request_Type", 
                              new XAttribute("code", r.Value)
                              )
                           )
                     );
                  return X;
               });

            Func<XElement> GetRqtt = 
               new Func<XElement>(
               () =>
               {
                  XElement X = 
                     new XElement("Requester_Types",
                        CL_RQTT.Properties
                        .GetItems()
                        .OfType<CheckedListBoxItem>()
                        .Where(r => r.CheckState == CheckState.Checked)
                        .Select(r => 
                           new XElement("Requester_Type", 
                              new XAttribute("code", r.Value)
                              )
                           )
                     );
                  return X;
               });

            Func<XElement> GetEpit =
               new Func<XElement>(
               () =>
               {
                  XElement X =
                     new XElement("Expense_Items",
                        CL_EPIT.Properties
                        .GetItems()
                        .OfType<CheckedListBoxItem>()
                        .Where(r => r.CheckState == CheckState.Checked)
                        .Select(r =>
                           new XElement("Expense_Item",
                              new XAttribute("code", r.Value)
                              )
                           )
                     );
                  return X;
               });

            Func<XElement> GetMtod =
               new Func<XElement>(
               () =>
               {
                  XElement X =
                     new XElement("Methods",
                        CL_MTOD.Properties
                        .GetItems()
                        .OfType<CheckedListBoxItem>()
                        .Where(r => r.CheckState == CheckState.Checked)
                        .Select(r =>
                           new XElement("Method",
                              new XAttribute("code", r.Value)
                              )
                           )
                     );
                  return X;
               });

            Func<XElement> GetCtgy =
               new Func<XElement>(
               () =>
               {
                  XElement X =
                     new XElement("Categories",
                        CL_CTGY.Properties
                        .GetItems()
                        .OfType<CheckedListBoxItem>()
                        .Where(r => r.CheckState == CheckState.Checked)
                        .Select(r =>
                           new XElement("Category",
                              new XAttribute("code", r.Value)
                              )
                           )
                     );
                  return X;
               });
            iScsc.REGL_INDP_P(
               new XElement("Process",
                  new XElement("Regulation",
                     new XAttribute("year", yEARSpinEdit.EditValue),
                     new XAttribute("code", cODESpinEdit.EditValue),
                     new XAttribute("actntype", Rb_IncPric.Checked ? "001" /* inc */ : (Rb_DecPric.Checked ? "002" /* dec */ : "003" /* optn */ )),
                     new XAttribute("prictype", Rb_Percentage.Checked),
                     new XAttribute("prct", UD_Prct.Value),
                     new XAttribute("pric", Te_Pric.EditValue ?? "0"),
                     new XAttribute("enblctgy", Cb_EnableCtgy.Checked),
                     GetRqtp(),
                     GetRqtt(),
                     GetEpit(),
                     GetMtod(),
                     GetCtgy()
                  )
               )
            );
            MessageBox.Show(this, "هزینه های آیین نامه با موفقیت تغییر یافت");
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }
   }
}
