using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.Desktop.Ui
{
   partial class Desktop
   {
      private class DesktopMenuItem
      {         
         internal int ID { get; set; }
         internal string Caption { get; set; }
         internal Image Picture { get; set; }
         private SimpleButton menuItem;
         internal SimpleButton MenuItem
         { 
            get { return menuItem; }
            set 
            {
               menuItem = value;
               menuItem.Tag = this;
               Picture = menuItem.Image;
            }
         }
      }

      private List<DesktopMenuItem> _DesktopMenuItems;
   }
}
