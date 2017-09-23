using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.DataGuard.Self.Code
{
   public partial class DataGuard
   {
      public DataGuard(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _Login = new Login.Code.Login(_commons, _wall) { _DefaultGateway = this };
         _SecurityPolicy = new SecPolicy.Share.Code.SecurityPolicy(_commons, _wall) { _DefaultGateway = this };
      }

      private string ConnectionString;
      private Data.iProjectDataContext iProject;
   }
}
