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
using System.Xml.Linq;
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;

namespace System.CRM.Ui.Deals
{
   public partial class SHW_DEAL_F : UserControl
   {
      public SHW_DEAL_F()
      {
         InitializeComponent();

         var path = new System.Drawing.Drawing2D.GraphicsPath();
         path.AddEllipse(0, 0, rbl_pymtstag001.Width, rbl_pymtstag001.Height);

         this.rbl_pymtstag001.Region = this.rbl_pymtstag002.Region = this.rbl_pymtstag003.Region = this.rbl_pymtstag004.Region = this.rbl_pymtstag005.Region = this.rbl_pymtstag006.Region = this.rbl_pymtstag007.Region = new Region(path);
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            iCRM = new Data.iCRMDataContext(ConnectionString);
            PymtBs.DataSource =
               iCRM.VF_Save_Payments(null, null, null);
         }
         catch { }
      }

      private Image GetProfileImage(long servfileno)
      {
         try
         {            
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", servfileno))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            return bm;
         }
         catch
         {
            return System.CRM.Properties.Resources.IMAGE_1149;
         }
      }

      private void PymtBs_DataSourceChanged(object sender, EventArgs e)
      {
         try
         {
            var amntunittypecode = (PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Select(p => p.AMNT_UNIT_TYPE_DNRM)).Distinct().FirstOrDefault();
            var amntunittypedesc = iCRM.D_ATYPs.FirstOrDefault(d => d.VALU == amntunittypecode).DOMN_DESC;

            // Count of Payments
            rbl_pymtstag001.Text = PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "001").Count().ToString();
            rbl_pymtstag002.Text = PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "002").Count().ToString();
            rbl_pymtstag003.Text = PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "003").Count().ToString();
            rbl_pymtstag004.Text = PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "004").Count().ToString();
            rbl_pymtstag005.Text = PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "005").Count().ToString();
            rbl_pymtstag006.Text = PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "006").Count().ToString();
            rbl_pymtstag007.Text = PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "007").Count().ToString();

            // Check Enabled And Disabled
            lb_pymtstag001.Visible = rbl_pymtstag001.Visible = rbl_pymtstag001.Text == "0" ? false : true;
            lb_pymtstag002.Visible = rbl_pymtstag002.Visible = rbl_pymtstag002.Text == "0" ? false : true;
            lb_pymtstag003.Visible = rbl_pymtstag003.Visible = rbl_pymtstag003.Text == "0" ? false : true;
            lb_pymtstag004.Visible = rbl_pymtstag004.Visible = rbl_pymtstag004.Text == "0" ? false : true;
            lb_pymtstag005.Visible = rbl_pymtstag005.Visible = rbl_pymtstag005.Text == "0" ? false : true;
            lb_pymtstag006.Visible = rbl_pymtstag006.Visible = rbl_pymtstag006.Text == "0" ? false : true;
            lb_pymtstag007.Visible = rbl_pymtstag007.Visible = rbl_pymtstag007.Text == "0" ? false : true;


            // Sum Amount of Payments
            lb_pymtstag001.Text = string.Format("<size=11>{0}</size> <size=14>{1}</size> (<size=12><color=blue>{2}</color></size>)", amntunittypedesc, PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "001").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"), PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "001").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"));
            lb_pymtstag002.Text = string.Format("<size=11>{0}</size> <size=14>{1}</size> (<size=12><color=blue>{2}</color></size>)", amntunittypedesc, PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "002").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"), PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "002").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"));
            lb_pymtstag003.Text = string.Format("<size=11>{0}</size> <size=14>{1}</size> (<size=12><color=blue>{2}</color></size>)", amntunittypedesc, PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "003").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"), PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "003").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"));
            lb_pymtstag004.Text = string.Format("<size=11>{0}</size> <size=14>{1}</size> (<size=12><color=blue>{2}</color></size>)", amntunittypedesc, PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "004").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"), PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "004").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"));
            lb_pymtstag005.Text = string.Format("<size=11>{0}</size> <size=14>{1}</size> (<size=12><color=blue>{2}</color></size>)", amntunittypedesc, PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "005").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"), PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "005").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"));
            lb_pymtstag006.Text = string.Format("<size=11>{0}</size> <size=14>{1}</size> (<size=12><color=blue>{2}</color></size>)", amntunittypedesc, PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "006").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"), PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "006").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"));
            lb_pymtstag007.Text = string.Format("<size=11>{0}</size> <size=14>{1}</size> (<size=12><color=blue>{2}</color></size>)", amntunittypedesc, PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "007").Sum(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT)).ToString("n0"), PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => p.PYMT_STAG == "007").Sum(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + (int)p.SUM_PYMT_DSCN_DNRM))).ToString("n0"));
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void rbt_pymtstag00i_Click(object sender, EventArgs e)
      {
         SelectedPymtStga_Butn.ImageProfile = (sender as System.MaxUi.RoundedButton).ImageProfile;
         SelectedPymtStga_Butn.Tag = (sender as System.MaxUi.RoundedButton).Tag;

         servicerelatedpayment_flp.Controls.Clear();

         SelectedPymtStga_Butn.Enabled = false;

         foreach (var servfileno in PymtBs.List.OfType<Data.VF_Save_PaymentsResult>().Where( p => p.PYMT_STAG == SelectedPymtStga_Butn.Tag.ToString()).Select(s => s.SERV_FILE_NO).Distinct())
         {
            System.MaxUi.RoundedButton rb = new MaxUi.RoundedButton();

            rb.Active = true;
            rb.Anchor = System.Windows.Forms.AnchorStyles.Top;
            rb.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
            rb.Caption = "";
            rb.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            rb.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
            rb.HoverBorderColor = System.Drawing.Color.Gold;
            rb.HoverColorA = System.Drawing.Color.LightGray;
            rb.HoverColorB = System.Drawing.Color.LightGray;
            rb.ImageProfile = GetProfileImage((long)servfileno);
            rb.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            rb.ImageVisiable = true;
            rb.Location = new System.Drawing.Point(22, 3);
            rb.Name = "rb_servicerelatedpayment";
            rb.NormalBorderColor = System.Drawing.Color.LightGray;
            rb.NormalColorA = System.Drawing.Color.White;
            rb.NormalColorB = System.Drawing.Color.White;
            rb.Size = new System.Drawing.Size(70, 70);
            rb.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
            rb.Tag = servfileno;
            rb.Click += rb_servicerelatedpayment_Click;

            servicerelatedpayment_flp.Controls.Add(rb);

            SelectedPymtStga_Butn.Enabled = true;
         }
      }

      private void rb_servicerelatedpayment_Click(object sender, EventArgs e)
      {
         try
         {
            var relatedservice = Convert.ToInt64((sender as System.MaxUi.RoundedButton).Tag);
            var serv = iCRM.Services.FirstOrDefault(s => s.FILE_NO == relatedservice);

            if (serv.SRPB_TYPE_DNRM == "001")
            {
               // Lead Info
               try
               {
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                          new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                          new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", relatedservice))},
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
               }
               catch { }
            }
            else if (serv.SRPB_TYPE_DNRM == "002")
            {
               // Contact
               try
               {
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                         new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", relatedservice))},
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
               }
               catch { }
            }
         }
         catch { }
      }

      private void rb_pymtstag00i_Click(object sender, EventArgs e)
      {
         try
         {
            var rb = (sender as MaxUi.RoundedButton);
            if (rb.Tag == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                       {                  
                         new Job(SendType.Self, 44 /* Execute Inf_Deal_F */),                
                         new Job(SendType.SelfToUserInterface, "INF_DEAL_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Payment", new XAttribute("pymtstag", rb.Tag))},
                       });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }
   }
}
