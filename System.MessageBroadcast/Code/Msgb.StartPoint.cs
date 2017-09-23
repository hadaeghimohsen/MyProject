using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.MessageBroadcast.Code
{
   public partial class Msgb
   {
      public Msgb(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         // Set Method For Timers
         _SenderBgwk = new Timer(){Interval = 1000};
         _CustBgwk = new Timer() { Interval = 1000};

         _SenderBgwk.Tick += _SenderBgwk_Tick;
         _CustBgwk.Tick += _CustBgwk_Tick;

         _SenderBgwk.Enabled = true;
         _CustBgwk.Enabled = true;
      }      
   }
}
