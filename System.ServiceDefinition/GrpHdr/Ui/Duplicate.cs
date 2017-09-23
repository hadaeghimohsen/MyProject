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

namespace System.ServiceDefinition.GrpHdr.Ui
{
   public partial class Duplicate : UserControl
   {
      public Duplicate()
      {
         InitializeComponent();
      }

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "GroupHeader", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "GroupHeader", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }

      private void sb_duplicate_Click(object sender, EventArgs e)
      {
         if (te_titlefa.Text.Trim() == "")
         {
            te_titlefa.Focus();
            return;
         }

         Func<string> XmlData = new Func<string>(
            () =>
            {
               string xmldata = string.Format("<Duplicate><GhID>{0}</GhID><FaName>{1}</FaName><EnName>{2}</EnName><CascadeSettings><Set>{3}</Set><SubObject>{4}</SubObject></CascadeSettings></Duplicate>",
                  te_source.Tag, te_titlefa.Text,te_titleen.Text,rb_manualset.Checked,tb_setobject.Value);
               return xmldata;
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
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
                              "<Privilege>5</Privilege><Sub_Sys>1</Sub_Sys>",
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;

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
                              XmlData(),
                              "{ Call ServiceDef.DuplicateGroupHeader(?) }",
                              "iProject",
                              "scott"
                           }
                        },
                        #endregion
                     }),
                  new Job(SendType.SelfToUserInterface, "Duplicate", 04 /* Execute UnPaint */),
                  new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 09 /* Execute LoadGroupHeaders */)
               }));
      }
   }
}
