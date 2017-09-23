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

namespace System.DataGuard.SecPolicy.Role.Ui
{
   public partial class PropertySingleRoleMenu : UserControl
   {
      public PropertySingleRoleMenu()
      {
         InitializeComponent();
      }
      private string Role;

      private void sb_changename_Click(object sender, EventArgs e)
      {
         Job _ChangingName = new Job(SendType.External, "Role", "", 05 /* Execute DoWork4RoleChanginName */, SendType.Self)
            {
               Input = new List<object>
               {
                 new Action(() => {
                     Job _LoadData = new Job(SendType.External, "Role",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "PropertySingleRoleMenu", 04 /* Execute UnPaint */),
                           new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 07 /* Execute LoadData */, SendType.SelfToUserInterface)
                        });
                     _DefaultGateway.Gateway(_LoadData);
                  }),
                  "Commons.ChangeObject", /* Scope Action */
                  "DataGuard.ChangeRoleName",
                  Role,
                  "iProject",
                  "scott",
                  "<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>3</SectionID><Explain>{0}</Expalin>",
                  new List<string>
                  {
                     "<Privilege>5</Privilege>",
                     "DataGuard"
                  }
               }
            };
         _DefaultGateway.Gateway(_ChangingName);
      }

      private void sb_deactiverole_Click(object sender, EventArgs e)
      {
         Job _ChangingName = new Job(SendType.External, "Role", "", 03 /* Execute DoWork4RoleChanginName */, SendType.Self)
         {
            Input = new List<object>
               {
                 new Action(() => {
                     Job _LoadData = new Job(SendType.External, "Role",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "PropertySingleRoleMenu", 04 /* Execute UnPaint */),
                           new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 07 /* Execute LoadData */, SendType.SelfToUserInterface)
                        });
                     _DefaultGateway.Gateway(_LoadData);
                  }),
                  "Commons.DeactiveObject", /* Scope Action */
                  "DataGuard.DeactiveRole",
                  Role,
                  "iProject",
                  "scott",
                  "<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>3</SectionID><Explain>{0}</Expalin>",
                  new List<string>
                  {
                     "<Privilege>6</Privilege>",
                     "DataGuard"
                  }
               }
         };
         _DefaultGateway.Gateway(_ChangingName);
      }

      private void sb_activerole_Click(object sender, EventArgs e)
      {
         Job _ChangingName = new Job(SendType.External, "Role", "", 03 /* Execute DoWork4RoleChanginName */, SendType.Self)
         {
            Input = new List<object>
               {
                 new Action(() => {
                     Job _LoadData = new Job(SendType.External, "Role",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "PropertySingleRoleMenu", 04 /* Execute UnPaint */),
                           new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 07 /* Execute LoadData */, SendType.SelfToUserInterface)
                        });
                     _DefaultGateway.Gateway(_LoadData);
                  }),
                  "Commons.RestoreDeactiveObject", /* Scope Action */
                  "DataGuard.RestoreDeactiveRole",
                  Role,
                  "iProject",
                  "scott",
                  "<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>3</SectionID><Explain>{0}</Expalin>",
                  new List<string>
                  {
                     "<Privilege>7</Privilege>",
                     "DataGuard"
                  }
               }
         };
         _DefaultGateway.Gateway(_ChangingName);
      }

      private void sb_duplicaterole_Click(object sender, EventArgs e)
      {
         Job _DuplicateRole = new Job(SendType.External, "Role", "", 06 /* Execute DoWork4DuplicateRole */, SendType.Self) { Input = Role};
         _DefaultGateway.Gateway(_DuplicateRole);
      }
   }
}
