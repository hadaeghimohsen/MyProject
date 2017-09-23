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
using System.Drawing.Imaging;
using System.MaxUi;

namespace System.DataGuard.Login.Ui
{
   public partial class LastUserLogin : UserControl
    {
        public LastUserLogin()
        {
            InitializeComponent();
        }

        private void SwitchUser_RondButn_Click(object sender, EventArgs e)
        {
           _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4LastUserLogin */, SendType.SelfToUserInterface) { Input = Keys.Escape }
            );

           Job _SwitchUser = 
              new Job(SendType.External, "Program", "DataGuard:Login", 02 /* Execute DoWork4Login */, SendType.Self);
           _DefaultGateway.Gateway(_SwitchUser);
        }

        private void User_RondButn_Click(object sender, EventArgs e)
        {
           // Get User Info
           RoundedButton rb = (RoundedButton)sender;
           if (rb.Tag != null)
           {
              Job _SelectedUser = 
                 new Job(SendType.External, "Localhost",
                   new List<Job>
                   {
                      new Job(SendType.Self, 06 /* Execute DoWork4SelectedLastUserLogin */){Input = rb.Tag}
                   });
              _DefaultGateway.Gateway(_SelectedUser);
           }else
           {
              // Guest User
           }
        }

        private Image GetUserImage(Data.User user)
        {
           if(user == null)
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

        private int SetUserList(IQueryable<Data.User> users, int i)
        {
           switch (users.Count())
           {
              case 1:
                 User2_RondButn.Visible = User3_RondButn.Visible = User2_Txt.Visible = User3_Txt.Visible = true;
                 User4_RondButn.Visible = User1_RondButn.Visible = User4_Txt.Visible = User1_Txt.Visible = User5_RondButn.Visible = User5_Txt.Visible = false;

                 User2_RondButn.ImageProfile = GetUserImage(users.First());
                 User2_Txt.Text = users.First().USERDB;
                 User2_RondButn.Tag = users.First();
                 User2_Txt.Tag = "realuser";

                 User3_RondButn.ImageProfile = GetUserImage(null);
                 User3_Txt.Text = "Guest User";
                 User3_RondButn.Tag = null;
                 User3_Txt.Tag = "guestuser";

                 break;
              case 2:
                 User1_RondButn.Visible = User2_RondButn.Visible = User3_RondButn.Visible = User1_Txt.Visible = User2_Txt.Visible = User3_Txt.Visible = true;
                 User4_RondButn.Visible = User4_Txt.Visible = User5_RondButn.Visible = User5_Txt.Visible = false;

                 foreach (Data.User user in users)
                 {
                    if (i == 0)
                    {
                       User2_RondButn.ImageProfile = GetUserImage(user);
                       User2_Txt.Text = user.USERDB;
                       User2_RondButn.Tag = user;
                       User2_Txt.Tag = "realuser";
                       ++i;
                    }
                    else if (i == 1)
                    {
                       User1_RondButn.ImageProfile = GetUserImage(user);
                       User1_Txt.Text = user.USERDB;
                       User1_RondButn.Tag = user;
                       User1_Txt.Tag = "realuser";
                       ++i;
                    }
                 }
                 User3_RondButn.ImageProfile = GetUserImage(null);
                 User3_Txt.Text = "Guest User";
                 User3_RondButn.Tag = null;
                 User3_Txt.Tag = "guestuser";
                 break;
              case 3:
                 User5_RondButn.Visible = User2_RondButn.Visible = User3_RondButn.Visible = User4_RondButn.Visible = User2_Txt.Visible = User3_Txt.Visible = User4_Txt.Visible = User5_Txt.Visible = true;
                 User1_RondButn.Visible = User1_Txt.Visible = false;

                 foreach (Data.User user in users)
                 {
                    if (i == 0)
                    {
                       User4_RondButn.ImageProfile = GetUserImage(user);
                       User4_Txt.Text = user.USERDB;
                       User4_RondButn.Tag = user;
                       User4_Txt.Tag = "realuser";
                       ++i;
                    }
                    else if (i == 1)
                    {
                       User2_RondButn.ImageProfile = GetUserImage(user);
                       User2_Txt.Text = user.USERDB;
                       User2_RondButn.Tag = user;
                       User2_Txt.Tag = "realuser";
                       ++i;
                    }
                    else if (i == 2)
                    {
                       User3_RondButn.ImageProfile = GetUserImage(user);
                       User3_Txt.Text = user.USERDB;
                       User3_RondButn.Tag = user;
                       User3_Txt.Tag = "realuser";
                       ++i;
                    }
                 }
                 User5_RondButn.ImageProfile = GetUserImage(null);
                 User5_Txt.Text = "Guest User";
                 User5_RondButn.Tag = null;
                 User5_Txt.Tag = "guestuser";

                 break;
              case 4:
                 User1_RondButn.Visible = User2_RondButn.Visible = User3_RondButn.Visible = User4_RondButn.Visible = User5_RondButn.Visible = User1_Txt.Visible = User2_Txt.Visible = User3_Txt.Visible = User4_Txt.Visible = User5_Txt.Visible = true;
                 foreach (Data.User user in users)
                 {
                    if (i == 0)
                    {
                       User4_RondButn.ImageProfile = GetUserImage(user);
                       User4_Txt.Text = user.USERDB;
                       User4_RondButn.Tag = user;
                       User4_Txt.Tag = "realuser";
                       ++i;
                    }
                    else if (i == 1)
                    {
                       User2_RondButn.ImageProfile = GetUserImage(user);
                       User2_Txt.Text = user.USERDB;
                       User2_RondButn.Tag = user;
                       User2_Txt.Tag = "realuser";
                       ++i;
                    }
                    else if (i == 2)
                    {
                       User3_RondButn.ImageProfile = GetUserImage(user);
                       User3_Txt.Text = user.USERDB;
                       User3_RondButn.Tag = user;
                       User3_Txt.Tag = "realuser";
                       ++i;
                    }
                    else if (i == 3)
                    {
                       User1_RondButn.ImageProfile = GetUserImage(user);
                       User1_Txt.Text = user.USERDB;
                       User1_RondButn.Tag = user;
                       User1_Txt.Tag = "realuser";
                       ++i;
                    }
                 }
                 User5_RondButn.ImageProfile = GetUserImage(null);
                 User5_Txt.Text = "Guest User";
                 User5_RondButn.Tag = null;
                 User5_Txt.Tag = "guestuser";
                 break;
              default:
                 break;
           }
           return i;
        }
    }
}
