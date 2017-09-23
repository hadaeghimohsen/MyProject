using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.DataGuard.SecPolicy.User.Code
{
   partial class User
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.PropertySingleMenu _PropertySingleMenu { get; set; }
      internal Ui.CreateNew _CreateNew { get; set; }
      internal Ui.Duplicate _Duplicate { get; set; }      
      internal Ui.ShowAll _ShowAllUsers { get; set; }
      internal Ui.Profile _Profile { get; set; }
      internal Ui.CurrentUserInfo _CurrentUserInfo { get; set; }
      internal Ui.CurrentChangeUserName _CurrentChangeUserName { get; set; }
      internal Ui.CurrentChangePassword _CurrentChangePassword { get; set; }
      internal Ui.CurrentChangeUserType _CurrentChangeUserType { get; set; }
      internal Ui.OtherUsers _OtherUsers { get; set; }
   }
}
