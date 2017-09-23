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
using DevExpress.XtraEditors;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsChangeNetwork : UserControl
   {
      public SettingsChangeNetwork()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, ImageAccount_Pb.Width, ImageAccount_Pb.Height);
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         ImageAccount_Pb.Region = rg;
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }            
         );
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         UserBs.DataSource = iProject.Users.FirstOrDefault(u => u.USERDB.ToLower() == User.ToLower());
         SubSysBs.DataSource = iProject.Sub_Systems.FirstOrDefault(s => s.SUB_SYS == SubSys);
         GatewayBs.DataSource = iProject.Gateways.FirstOrDefault(g => g.MAC_ADRS == HostName);
         AccessUserDatasourceBs.DataSource = iProject.Access_User_Datasources.Where(aud => aud.User.USERDB.ToLower() == User.ToLower() && aud.DataSource.SUB_SYS == SubSys && aud.HOST_NAME == HostName);

         var user = UserBs.Current as Data.User;
         if (user.USER_IMAG == null)
         {
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            Image img = global::System.DataGuard.Properties.Resources.IMAGE_1429;
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            user.USER_IMAG = bytes;
            ImageAccount_Pb.Image = global::System.DataGuard.Properties.Resources.IMAGE_1429;
         }
         else
         {
            var stream = new MemoryStream(user.USER_IMAG.ToArray());
            ImageAccount_Pb.Image = Image.FromStream(stream);
         }

         CreateAudMenu();
      }

      private void CreateAudMenu()
      {
         AccessUserDatasource_Flp.Controls.Clear();
         foreach (Data.Access_User_Datasource aud in AccessUserDatasourceBs.List.OfType<Data.Access_User_Datasource>())
         {
            var simplebutton = new SimpleButton();

            simplebutton.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            simplebutton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            simplebutton.Appearance.BackColor = aud.STAT == "001" ? Color.Gainsboro : Color.SkyBlue;
            simplebutton.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            simplebutton.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            simplebutton.Appearance.Options.UseBackColor = true;
            simplebutton.Appearance.Options.UseFont = true;
            simplebutton.Appearance.Options.UseForeColor = true;
            simplebutton.Appearance.Options.UseTextOptions = true;
            simplebutton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            simplebutton.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            simplebutton.Image = global::System.DataGuard.Properties.Resources.IMAGE_1384;
            simplebutton.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            simplebutton.Location = new System.Drawing.Point(530, 3);
            simplebutton.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            simplebutton.LookAndFeel.UseDefaultLookAndFeel = false;
            simplebutton.Name = "simpleButton2";
            simplebutton.Size = new System.Drawing.Size(169, 57);
            simplebutton.TabIndex = 3;
            simplebutton.Tag = aud;
            simplebutton.Text = string.Format("<b>{0}</b><br><color=DimGray><size=9>{1}</size></color><br>", aud.DataSource.TitleFa, aud.DataSource.IPAddress);
            simplebutton.Click += AccessUserDatasource_Click;
            AccessUserDatasource_Flp.Controls.Add(simplebutton);
         }
      }

      private void AccessUserDatasource_Click(object sender, EventArgs e)
      {
         try
         {
            var aud = ((SimpleButton)sender).Tag as Data.Access_User_Datasource;
            if (aud == null || MessageBox.Show(this, "آیا با تغییر پل ارتباطی موافق هستید؟", "تغییر پل ارتباطی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            aud.STAT = "002";
            AccessUserDatasourceBs.List.OfType<Data.Access_User_Datasource>().Where(a => a != aud).ToList().ForEach(a => a.STAT = "001");
            iProject.SubmitChanges();

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     //new Job(SendType.Self, 25 /* Execute DoWork4SettingsAccountChangePassword */),                  
                     new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 10 /* Execute ActionCallWindows */){Input = new XElement("TabPage", new XAttribute("showtabpage", "tp_001"))}
                  }
               )
            );

            Back_Butn_Click(null, null);
         }
         catch { }
      }
   }
}
