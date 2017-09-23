using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.DataGuard.SecPolicy.Role.Code
{
   partial class Role
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.CreateNewRole _CreateNewRole { get; set; }
      internal Ui.PropertySingleRoleMenu _PropertySingleRoleMenu { get; set; }
      internal Ui.DuplicateRole _DuplicateRole { get; set; }
      internal Ui.JoinOrLeaveCurrentUserToRoles _JoinOrLeaveCurrentUserToRoles { get; set; }
   }
}
