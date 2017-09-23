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
using System.IO;
using DevExpress.XtraEditors;

namespace System.DataGuard.Login.Ui
{
   public partial class SelectedLastUserLogin : UserControl
   {
      public SelectedLastUserLogin()
      {
         InitializeComponent();
      }

      private void Cancel_RondButn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
             new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4LastUserLogin */, SendType.SelfToUserInterface) { Input = Keys.Escape }
          );
      }

      private void Password_Be_TextChanged(object sender, EventArgs e)
      {
         ErrorValidation_Lbl.Visible = false;
         if (Password_Be.Text.Length != 0)
            Password_Be.Properties.Buttons[0].Visible = true;
         else
            Password_Be.Properties.Buttons[0].Visible = false;
      }

      private void Password_Be_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         if (e.Button.Index == 0)
         {
            Password_Be.Properties.UseSystemPasswordChar = false;
         }
      }

      private void Password_Be_MouseUp(object sender, MouseEventArgs e)
      {
         Password_Be.Properties.UseSystemPasswordChar = true;
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

      private void CheckValidation_RondButn_Click(object sender, EventArgs e)
      {
         if (Password_Be.Text.Trim().Length == 0)
         {
            Password_Be.Focus();
            return;
         }
         if (Password_Be.Text != SelectedUser.Password)
         {
            ErrorValidation_Lbl.Visible = true;
            return;
         }

         // Goto Validation User


         _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4SelectedLastUserLogin */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         Job _LoginUser =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {
                    //new Job(SendType.Self, 02 /* Execute DoWork4Login */),
                    new Job(SendType.SelfToUserInterface, "Login", 08 /* Execute LoadDataAsync */){Input = SelectedUser},
                 }
              );
         _DefaultGateway.Gateway(_LoginUser);
      }

      private void Control_Enter(object sender, EventArgs e)
      {
         ButtonEdit control = sender as ButtonEdit;
         control.SelectAll();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 09 /* Execute LangChangToEnglish */, SendType.Self)
         );
      }

      private void InputValidation(object sender, KeyEventArgs e)
      {
         if (e.KeyData != Keys.Return)
            return;

         CheckValidation_RondButn_Click(null, null);
      }
   }
}
