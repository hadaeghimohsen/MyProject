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
using DevExpress.XtraEditors.Controls;

namespace System.ServiceDefinition.SrvDef.Ui
{
   public partial class Create : UserControl
   {
      public Create()
      {
         InitializeComponent();         
      }

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "Service", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "Service", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }

      #region Group Header
      private void sb_loadgrphdr_Click(object sender, EventArgs e)
      {

      }

      private void sb_defgrphdr_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Service Type
      private void sb_loadservicetype_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Service",
               new List<Job>
               {
                  new Job(SendType.External, "UnitType",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute DoWork4ServiceUnitType */){Input = 2 /* Type */}
                     })
               }));
      }

      private void sb_defservicetype_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Service Unit
      private void sb_loadserviceunit_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Service",
               new List<Job>
               {
                  new Job(SendType.External, "UnitType",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute DoWork4ServiceUnitType */){Input = 1 /* Unit */}
                     })
               }));
      }

      private void sb_defserviceunit_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Price
      private void cb_serviceisexpens_CheckedChanged(object sender, EventArgs e)
      {
         te_rw_price.Enabled = cb_serviceisexpens.Checked;
      }
      #endregion

      #region Edit Menu
      private void sb_save_Click(object sender, EventArgs e)
      {
         if (Controls.OfType<Control>().Where(c => c.Name.Contains("_f_") && c.Text.Trim().Length == 0).Count() > 0)
         {
            Controls.OfType<Control>().Where(c => c.Name.Contains("_f_") && c.Text.Trim().Length == 0).Last().Focus();
            return;
         }

         Func<string> grphdrXmlData = new Func<string>(
            () =>
            {
               string xmldata = "";
               ccbe_f_groupheaders.Properties.GetItems().OfType<CheckedListBoxItem>().Where(grp => grp.CheckState == CheckState.Checked).ToList().ForEach(grp => xmldata += string.Format("<GhID>{0}</GhID>",grp.Value));
               return xmldata;
            });

         Func<string> XmlData = new Func<string>(
            () =>
            {
               string xmldata = 
                  string.Format(
                      @"<Create>
                           <ParentID>{0}</ParentID>
                           <Level>{1}</Level>
                           <Shortcut>{2}</Shortcut>
                           <TitleFa>{3}</TitleFa>
                           <TitleEn>{4}</TitleEn>
                           <ServiceType>{5}</ServiceType>
                           <ServiceUnit>{6}</ServiceUnit>
                           <Price Type=""{7}"">{8}</Price>
                           <GroupHeader>{9}</GroupHeader>
                        </Create>",
                                  te_parent.Tag,
                                  sb_save.Tag,
                                  te_rw_shortcut.EditValue,
                                  te_rw_f_titlefa.Text,
                                  te_rw_f_titleen.Text,
                                  cb_servicetype.SelectedValue,
                                  cb_serviceunit.SelectedValue,
                                  cb_serviceisexpens.Checked,
                                  te_rw_price.EditValue,
                                  grphdrXmlData()
                  );
               return xmldata;
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Service",
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
                              string.Format("<Privilege>{0}</Privilege><Sub_Sys>1</Sub_Sys>",Convert.ToInt16(sb_save.Tag) == 1 ? 12 /* Group Service */: 26 /* Service */),
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;

                              Job _ShowError = new Job(SendType.External, "Service", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
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
                           })
                        },
                        #endregion                     
                        #region DoWork
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              false,
                              "xml",
                              XmlData(),
                              "{ Call ServiceDef.SaveServiceOrGrpSrv(?) }",
                              "iProject",
                              "scott"
                           }
                        }
                        #endregion
                     }),
                  new Job(SendType.SelfToUserInterface, "Create", 04 /* Execute UnPaint */),
                  new Job(SendType.External, "ServiceDefinition", "Services", 10 /* Execute LoadParentServicesOfGroupHeaders */, SendType.SelfToUserInterface)
               }));
      }

      private void sb_cancel_Click(object sender, EventArgs e)
      {

      }

      private void sb_retype_Click(object sender, EventArgs e)
      {
         Controls.OfType<Control>().Where(c => c.Name.Contains("_rw_")).ToList().ForEach(c => c.Text = "");
      }

      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         Job _RoleSettings4CurrentUser =
            new Job(SendType.External, "Service",
               new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                              })
                        });
         _DefaultGateway.Gateway(_RoleSettings4CurrentUser);
      }
      #endregion      

      private void sb_filepath_Click(object sender, EventArgs e)
      {
         if (ofd_selector.ShowDialog() != DialogResult.OK)
            return;

         te_rw_f_titleen.Text = ofd_selector.FileName;
      }
   }
}
