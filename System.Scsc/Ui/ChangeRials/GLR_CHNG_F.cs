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
using System.IO;
using System.Scsc.ExtCode;

namespace System.Scsc.Ui.ChangeRials
{
   public partial class GLR_CHNG_F : UserControl
   {
      public GLR_CHNG_F()
      {
         InitializeComponent();
      }

      bool requery = false;
      long _glid = 0;

      private void Back_Btn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);

            RqstBs1.DataSource = 
               iScsc.Requests
               .FirstOrDefault(r => 
                  r.Request_Rows
                  .Any(rr => 
                     rr.Gain_Loss_Rials1
                     .Any(g => g.GLID == _glid)));

            requery = false;
         }
         catch (Exception ) { }         
      }

      private void GlrlBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _glrl = GlrlBs1.Current as Data.Gain_Loss_Rial;
            if (_glrl == null) return;

            switch(_glrl.DPST_STAT)
            {
               case "001":
                  DecDspt_Rb.Checked = true;
                  break;
               case "002":
                  IncDpst_Rb.Checked = true;
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            GlrlBs1.EndEdit();
            Glrd_gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Add_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _glrl = GlrlBs1.Current as Data.Gain_Loss_Rial;
            if (_glrl == null) return;

            if (GlrdBs1.List.OfType<Data.Gain_Loss_Rial>().Any(gd => gd.GLID == 0)) return;

            if (GlrdBs1.List.OfType<Data.Gain_Loss_Rail_Detail>().Sum(g => g.AMNT) >= _glrl.AMNT) return;

            GlrdBs1.AddNew();
            var glrd = GlrdBs1.Current as Data.Gain_Loss_Rail_Detail;
            glrd.GLRL_GLID = _glrl.GLID;

            // 1402/08/29 * اگر نوع سپرده گذاری افزایش یا کاهش باشد بتوانیم نوع پرداخت را تغییر دهیم
            if (IncDpst_Rb.Checked)
               glrd.RCPT_MTOD = "003";
            else if (DecDspt_Rb.Checked)
               glrd.RCPT_MTOD = "009";

            glrd.AMNT = _glrl.AMNT - (GlrdBs1.List.OfType<Data.Gain_Loss_Rail_Detail>().Sum(g => g.AMNT));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _glrd = GlrdBs1.Current as Data.Gain_Loss_Rail_Detail;
            if (_glrd == null) return;

            if (MessageBox.Show(this, "آیا با حذف ردیف پرداختی سپرده موافق هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.DEL_GLRD_P(
               new XElement("Gain_Loss_Rail_Detail",
                  new XAttribute("glrlglid", _glrd.GLRL_GLID),
                  new XAttribute("rwno", _glrd.RWNO)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Glrd_gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }
   }
}
