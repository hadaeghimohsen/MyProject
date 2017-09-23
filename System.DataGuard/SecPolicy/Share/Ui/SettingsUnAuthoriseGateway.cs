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
   public partial class SettingsUnAuthoriseGateway : UserControl
   {
      public SettingsUnAuthoriseGateway()
      {
         InitializeComponent();
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
         GatewayBs.DataSource = iProject.Gateways.Where(g => g.AUTH_TYPE_DNRM == "001");
         CreateGatewayMenu();
         UserGatewayBs.Clear();
         GatewayInfo_Lbl.Text = "";
         if(UserGateway_Gv.Tag != null)
         {
            var gateway = UserGateway_Gv.Tag as Data.Gateway;
            GatewayInfo_Lbl.Text = string.Format("{0}<br><color=DimGray><size=10>{1}</size></color><br>", gateway.COMP_NAME_DNRM, gateway.IP_DNRM);
            UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.Gateway == gateway && ug.RWNO == iProject.User_Gateways.Where(ugt => ugt.Gateway == gateway && ugt.USER_ID == ug.USER_ID).Max(ugt => ugt.RWNO));
         }
      }

      private void CreateGatewayMenu()
      {
         AuthListGateway_Flp.Controls.Clear();
         foreach (Data.Gateway gateway in GatewayBs.List.OfType<Data.Gateway>())
         {
            var simplebutton = new SimpleButton();

            simplebutton.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            simplebutton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            simplebutton.Appearance.BackColor = Color.SkyBlue;
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
            simplebutton.Tag = gateway;
            simplebutton.Text = string.Format("<b><u>{0}</u></b><br><color=DimGray><size=9>{1}</size></color><br>", gateway.COMP_NAME_DNRM, gateway.IP_DNRM);
            simplebutton.Click += GatewayInfo_Click;
            AuthListGateway_Flp.Controls.Add(simplebutton);
         }
      }

      private void GatewayInfo_Click(object sender, EventArgs e)
      {
         var gateway = ((SimpleButton)sender).Tag as Data.Gateway;
         GatewayInfo_Lbl.Text = string.Format("{0}<br><color=DimGray><size=10>{1}</size></color><br>", gateway.COMP_NAME_DNRM, gateway.IP_DNRM);
         UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.Gateway == gateway && ug.RWNO == iProject.User_Gateways.Where(ugt => ugt.Gateway == gateway && ugt.USER_ID == ug.USER_ID).Max(ugt => ugt.RWNO));
         UserGateway_Gv.Tag = gateway;
      }

      private void AuthoriseGateway_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UserGatewayBs.Current as Data.User_Gateway;
            if (usgw == null || MessageBox.Show(this, "آیا با فعال کردن سیستم موافق هستید؟", "فعال شدن سیستم", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "004"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );

            UserGateway_Gv.Tag = null;
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void BlockGateway_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var usgw = UserGatewayBs.Current as Data.User_Gateway;
            if (usgw == null || MessageBox.Show(this, "آیا با بلوکه کردن سیستم موافق هستید؟", "بلوکه شدن سیستم", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.ChangeHostAuthorized(
               new XElement("Request",
                  new XAttribute("actntype", "002"),
                  new XElement("Gateway",
                     new XAttribute("mac", usgw.GTWY_MAC_ADRS)
                  )
               )
            );

            UserGateway_Gv.Tag = null;
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }
   }
}
