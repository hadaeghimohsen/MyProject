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
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;

namespace System.DataGuard.Login.Ui
{
   public partial class PinCode : UserControl
   {
      public PinCode()
      {
         InitializeComponent();
      }

      private void Successful_Butn_Clicked(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 04 /* Execute UnPaint */, SendType.SelfToUserInterface) //{ Input = Keys.Escape }
         );
      }

      private Image GetUserImage(Data.User user)
      {
         if (user == null)
         {
            return global::System.DataGuard.Properties.Resources.IMAGE_1482;
         }
         else if (user.USER_IMAG == null)
         {
            return global::System.DataGuard.Properties.Resources.IMAGE_1429;
         }
         else
         {
            var stream = new MemoryStream(user.USER_IMAG.ToArray());
            return Image.FromStream(stream);
         }
      }

      private void Numi_Butn_Click(object sender, EventArgs e)
      {
         var numi = sender as MaxUi.XRoundButton;

         PinCode_Txt.Text += numi.Tag.ToString();
      }

      private void Retype_Lb_Click(object sender, EventArgs e)
      {
         PinCode_Txt.Text = "";
      }

      private void BackSpace_Lb_Click(object sender, EventArgs e)
      {
         if (PinCode_Txt.Text.Length > 0)
            PinCode_Txt.Text = PinCode_Txt.Text.Substring(0, PinCode_Txt.Text.Length - 1);
      }

      private void PinCode_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (SelectedUser.PIN_CODE == Convert.ToInt32(e.NewValue))
            {
               // Close PinCode Form
               PinCode_Txt.EditValue = "";
               e.Cancel = true;
               Successful_Butn_Clicked(null, null);
            }
            else if (e.NewValue.ToString().Length >= SelectedUser.PIN_CODE.ToString().Length)
            {
               //ChckVald_Tm.Enabled = true;               
               PinCode_Txt.EditValue = "";
               e.Cancel = true;
            }
         }
         catch { }
      }

      bool _leftDirection = true;
      int _iteration = 0;
      int _StepMove = 10;

      private void ChckVald_Tm_Tick(object sender, EventArgs e)
      {
         if (_leftDirection)
         {
            _leftDirection = !_leftDirection;
            PinCode_Txt.Left -= _StepMove;            
            _iteration++;
         }
         else
         {
            _leftDirection = !_leftDirection;
            PinCode_Txt.Left += _StepMove;            
            _iteration++;
         }

         if (_iteration == 4)
         {            
            ChckVald_Tm.Enabled = false;
            PinCode_Txt.Left -= _StepMove;
            PinCode_Txt.EditValue = "";
            _iteration = 0;
         }
      }
   }
}
