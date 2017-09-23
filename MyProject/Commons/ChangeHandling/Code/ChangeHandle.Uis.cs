using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.ChangeHandling.Code
{
   partial class ChangeHandle 
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.ChangeName _ChangeName { get; set; }      
   }
}
