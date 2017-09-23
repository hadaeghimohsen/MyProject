using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.JobRouting.Jobs;

namespace System.CRM.Ui.PublicInformation
{
   public partial class CMN_DCMT_F : UserControl
   {
      public CMN_DCMT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private Data.Request_Row Rqro;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         RcdcBs.DataSource = iCRM.Receive_Documents.Where(rd => rd.Request_Row == Rqro);
         requery = false;
      }

      private void SelectImageGallery_Butn_Click(object sender, EventArgs e)
      {
         if (DG_SelectImage.ShowDialog() != DialogResult.OK) return;

         //FILE_NAME_TextBox.Text = DG_SelectImage.FileName;
         Image img = Image.FromFile(DG_SelectImage.FileName);

         switch (CB_ImageSize.SelectedIndex)
         {
            case 0:
               img = new Bitmap(img, new Size(169, 224));
               break;
            case 1:
               img = new Bitmap(img, new Size(200, 300));
               break;
            case 2:
               img = new Bitmap(img, new Size(300, 421));
               break;
            case 3:
               img = new Bitmap(img, new Size(597, 842));
               break;
            case 4:
               img = new Bitmap(img, new Size(842, 597));
               break;
            case 5: break;
            default:
               MessageBox.Show("ابعاد عکس مشخص نشده");
               return;
         }
         PE_ImageShow.Image = img;

         byte[] bytes = null;
         MemoryStream ms = new MemoryStream();
         img = PE_ImageShow.Image;
         img.Save(ms, ImageFormat.Bmp);
         bytes = ms.ToArray();

         var CrntRcdc = (RcdcBs.Current as Data.Receive_Document);
         CrntRcdc.Image_Documents.Single().IMAG = Convert.ToBase64String(bytes);
         CrntRcdc.Image_Documents.Single().FILE_NAME = DG_SelectImage.FileName;
         CrntRcdc.Image_Documents.Single().MDFY_STAT = 1;

         Rcdc_Save_Butn_Click(null, null);
      }

      private void Rcdc_Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            RcdcBs.EndEdit();
            iCRM.CommandTimeout = 180;

            iCRM.DCT_SAVE_P(
               new XElement("Prcess",
                  RcdcBs.List.OfType<Data.Receive_Document>()
                  .ToList()
                  .Select(rc =>
                     new XElement("Receive_Document",
                        new XAttribute("rcid", rc.RCID),
                        new XAttribute("delvdate", rc.DELV_DATE.Value.Date),
                        new XAttribute("rcdcstat", rc.RCDC_STAT),
                        new XAttribute("permstat", rc.PERM_STAT),
                        new XAttribute("strtdate", rc.STRT_DATE.Value.Date),
                        new XAttribute("enddate", rc.END_DATE.Value.Date),
                        new XAttribute("rcdcdesc", rc.RCDC_DESC ?? "")
                     )
                  )
               )
            );

            if (RcdcBs.List.OfType<Data.Receive_Document>()
                  .Where(rd => rd.Image_Documents.Single().MDFY_STAT == 1).Count() >= 1)
               iCRM.DCT_SAVE_P(
                  new XElement("Prcess",
                     RcdcBs.List.OfType<Data.Receive_Document>()
                     .Where(rd => rd.Image_Documents.Single().MDFY_STAT == 1)
                     .ToList()
                     .Select(rc =>
                        new XElement("Receive_Document",
                           new XAttribute("rcid", rc.RCID),
                           new XAttribute("delvdate", rc.DELV_DATE.Value.Date),
                           new XAttribute("rcdcstat", rc.RCDC_STAT),
                           new XAttribute("permstat", rc.PERM_STAT),
                           new XAttribute("strtdate", rc.STRT_DATE.Value.Date),
                           new XAttribute("enddate", rc.END_DATE.Value.Date),
                           new XAttribute("rcdcdesc", rc.RCDC_DESC ?? ""),
                           new XElement("Image_Document",
                              new XAttribute("filename", rc.Image_Documents.Single().FILE_NAME ?? ""),
                              rc.Image_Documents.Single().IMAG ?? ""
                           )
                        )
                     )
                  )
               );
            requery = true;
         }
         catch (Exception Ex)
         {
            MessageBox.Show(Ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Bt_RemvImage_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از پاک کردن تصویر مدرک مطمئن هستید؟", "حذف تصویر مدرک", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         try
         {
            var CrntRcdc = (RcdcBs.Current as Data.Receive_Document);
            iCRM.DEL_DIMG_P(
               new XElement("Process",
                  new XElement("Image_Document",
                     new XAttribute("rcid", CrntRcdc.Image_Documents.Single().RCDC_RCID),
                     new XAttribute("rwno", CrntRcdc.Image_Documents.Single().RWNO)
                  )
               )
            );
            PE_ImageShow.Image = null;
            requery = true;
         }
         catch (Exception Ex)
         {
            MessageBox.Show(Ex.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void CB_ShowScroll_CheckedChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.ShowScrollBars = !PE_ImageShow.Properties.ShowScrollBars;
      }

      private void CB_AllowMouseDrag_CheckedChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;
         PE_ImageShow.Properties.AllowScrollViaMouseDrag = !PE_ImageShow.Properties.AllowScrollViaMouseDrag;
      }

      private void CB_Alignment_SelectedIndexChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.PictureAlignment = (ContentAlignment)(CB_Alignment.SelectedIndex + 1);
      }

      private void UD_Interpolation_ValueChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.PictureInterpolationMode = (Drawing.Drawing2D.InterpolationMode)(UD_Interpolation.Value);
      }

      private void ZC_ZoomImage_ValueChanged(object sender, EventArgs e)
      {
         PE_ImageShow.Properties.ZoomPercent = ZC_ZoomImage.Value;
      }

      private void SetProfileImage_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = ImdcBs1.Current as Data.Image_Document;
            iCRM.SET_PIMG_P(
               new XElement("Service",
                  new XAttribute("fileno", Rqro.SERV_FILE_NO),
                  new XElement("Image",
                     new XAttribute("rcdcrcid", crnt.RCDC_RCID),
                     new XAttribute("rwno", crnt.RWNO)
                  )
               )
            );
            MessageBox.Show("پروفایل عکس شما با موفقیت تایید شد");
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void TakeImage_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 41 /* Execute Serv_Camr_F */),
                  new Job(SendType.SelfToUserInterface, "SERV_CAMR_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = RcdcBs.Current as Data.Receive_Document
                  }
               }
            )
         );
      }

      private void Tc_Dcmt_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (Tc_Dcmt.SelectedIndex == 1)
         {
            try
            {
               var CrntRcdc = (RcdcBs.Current as Data.Receive_Document);
               byte[] bytes = null;
               bytes = Convert.FromBase64String(CrntRcdc.Image_Documents.Single().IMAG);
               MemoryStream ms = new MemoryStream(bytes);
               Image img = new Bitmap(ms);
               PE_ImageShow.Image = img;
            }
            catch { PE_ImageShow.Image = null; }
         }
      }

   }
}
