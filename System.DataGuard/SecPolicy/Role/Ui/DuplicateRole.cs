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

namespace System.DataGuard.SecPolicy.Role.Ui
{
   public partial class DuplicateRole : UserControl
   {
      public DuplicateRole()
      {
         InitializeComponent();
      }

      private string Role;

      private void sb_duplicaterole_Click(object sender, EventArgs e)
      {
         if (SUBSYS_LOV.SelectedIndex < 0) return;

         Func<string, string> UpdateXmlData = (doc) =>
            {
               var datasource = XElement.Parse(doc);
               datasource.Descendants("RoleName").FirstOrDefault().SetValue(te_titlefa.Text);
               datasource.Add(new[] { new XElement("STitleFa", te_titlefa.Text.Replace(" ", string.Empty)), new XElement("TitleEn", te_titleen.Text), new XElement("SavePrivileges", cb_saveprivileges.Checked), new XElement("SaveUsers", cb_saveusers.Checked), new XElement("SubSys", SUBSYS_LOV.SelectedIndex) });
               return datasource.ToString();
            };

         Func<string, string, string> UpdateXmlRedoLog = (docRedolog, docRole) =>
         {
            //var dsRedolog = XElement.Parse(docRedolog);
            var dsRole = XElement.Parse(docRole);

            return docRedolog = string.Format(docRedolog, string.Format("نمونه برداری از گروه دسترسی {0} به نام {1}", dsRole.Descendants("RoleName").FirstOrDefault().Value, te_titlefa.Text));
         };

         if (te_titlefa.Text.Trim().Length == 0)
            return;

         Job _CreateNewRole = new Job(SendType.External, "Role",
            new List<Job>
            {
               #region Not Exists Role
               new Job(SendType.Self, 07 /* Execute DoWork4NotExistsRole */)
               {
                  Input = string.Format("<STitleFa>{0}</STitleFa>",te_titlefa.Text.Replace(" ", string.Empty)),                        
                  AfterChangedOutput = new Action<object>((output) => {
                     if ((bool)output)
                        return;

                     Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                     {
                        Input = @"Not Imp"
                     };
                     _DefaultGateway.Gateway(_ShowError);
                  })
               },
               #endregion
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = new List<string>
                        {
                           "<Privilege>8</Privilege>",
                           "DataGuard"
                        },
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;

                           Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                           {
                              Input = @"Not Imp"
                           };
                           _DefaultGateway.Gateway(_ShowError);
                        })
                     },
                     #endregion
                     #region DoWork
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */){
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           false,
                           "xml",
                           UpdateXmlData(Role),
                           "{ Call DataGuard.DuplicateRole(?) }",
                           "iProject",
                           "scott"
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint",  UpdateXmlRedoLog(@"<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>7</SectionID><Explain>{0}</Explain>", Role)}}
                     #endregion
                  }),
                  new Job (SendType.SelfToUserInterface, "DuplicateRole", 04 /* Execute UnPaint */),
                  new Job(SendType.External ,"SecurityPolicy", "SecuritySettings", 07 /* Execute LoadData */, SendType.SelfToUserInterface )
            });
         _DefaultGateway.Gateway(_CreateNewRole);
      }

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "Role", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "Role", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }
   }
}
