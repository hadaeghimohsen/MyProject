using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.MaxUi
{
   public class RoundedButtonDesigner : System.Windows.Forms.Design.ControlDesigner
   {
      public RoundedButtonDesigner()
      {
      }

      protected override void PostFilterProperties(IDictionary Properties)
      {
         //test
         Properties.Remove("AllowDrop");
         Properties.Remove("BackColor");
         Properties.Remove("BackgroundImage");
         Properties.Remove("ContextMenu");
         Properties.Remove("FlatStyle");
         Properties.Remove("Image");
         Properties.Remove("ImageAlign");
         Properties.Remove("ImageIndex");
         Properties.Remove("ImageList");
         Properties.Remove("TextAlign");
         Properties.Remove("Enabled");
      }
   }
}
