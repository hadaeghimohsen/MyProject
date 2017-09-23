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

namespace MyProject.Commons.ChangeHandling.Ui
{
   public partial class ChangeName : UserControl
   {
      public ChangeName()
      {
         InitializeComponent();
      }

      private Action AfterSelectedAccepted;
      private string CommandName;
      private string XMLParameter;
      private string DsnName;
      private string UserName;
      private string RedoLog;
      private List<string> AccessPrivilege;

      private void sb_changenewrolename_Click(object sender, EventArgs e)
      {
         Func<string, string> ChangeTagRoleName = (doc) =>
            {
               var datasource = XElement.Parse(doc);
               datasource.Descendants("RoleName").FirstOrDefault().SetValue(te_titlefa.Text);
               datasource.Add(new XElement("STitleFa", te_titlefa.Text.Trim()));
               return datasource.ToString();
            };

         Func<string, string, string> UpdateXmlRedoLog = (docRedolog, docRole) =>
            {
               //var dsRedolog = XElement.Parse(docRedolog);
               var dsRole = XElement.Parse(docRole);

               return docRedolog = string.Format(docRedolog, string.Format("تغییر نام گروه دسترسی {0} به {1}", dsRole.Descendants("RoleName").FirstOrDefault().Value, te_titlefa.Text));               
            };

         Job _DoWork4Accept = new Job(SendType.External, "ChangeName",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = AccessPrivilege,                        
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;

                           Job _ShowError = new Job(SendType.External, "ChangeHandling", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                           {
                              Input = @".\Documents\DataGuard\SecPolicy\Privilege\AccessPrivilege.Error.html"
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
                           ChangeTagRoleName(XMLParameter),
                           string.Format("{{ Call {0}(?) }}", CommandName),
                           DsnName,
                           UserName
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint", UpdateXmlRedoLog(RedoLog,XMLParameter)}}
                     #endregion
                  }),

                  new Job (SendType.SelfToUserInterface, "ChangeName", 04 /* Execute UnPaint */),
                  new Job (SendType.SelfToUserInterface, "ChangeName", 08 /* Execute Finalization */)
            });
         _DefaultGateway.Gateway(_DoWork4Accept);
      }

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }
   }
}
