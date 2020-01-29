using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Scsc.Ui.WarrantyService
{
   public partial class WRN_SERV_F : UserControl
   {
      public WRN_SERV_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long? fileno = null;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }      

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         
         int rqstindex = RqstBs.Position;

         var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
            .Where(rqst =>
                  rqst.RQTP_CODE == "031" &&
                  rqst.RQST_STAT == "001" &&
                  rqst.RQTT_CODE == "004" &&
                  rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

         RqstBs.DataSource =
            iScsc.Requests
            .Where(
               rqst =>
                  Rqids.Contains(rqst.RQID)
            )
            .OrderByDescending(
               rqst =>
                  rqst.RQST_DATE
            );

         RqstBs.Position = rqstindex;

         requery = true;
      }

      private void RqstBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انصراف درخواست مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Rqst = RqstBs.Current as Data.Request;

            if (Rqst != null && Rqst.RQID > 0)
            {
               iScsc.CNCL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
            }
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;

            if(rqst == null || rqst.RQID == 0)
            {
               // فراخوانی تابع مربوط به ثبت درخواست
               iScsc.GRN_TRQT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", 0),
                        new XElement("Fighter",
                           new XAttribute("fileno", fileno)
                        )
                     )
                  )
               );
            }
            else
            {
               // دیگر نیاز به فراخوانی تابع نیست فقط اطلاعات جدول مربوطه را ذخیره میکنیم
               Pdsr_Gv.PostEditor();
               iScsc.SubmitChanges();
            }

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

      private void RqstBnDefaultPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnSettingPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            if (rqst.RQID != 0)
            {
               if (MessageBox.Show(this, "آیا با پایانی کردن درخواست موافق هستید؟", "پایانی شدن درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               // فراخوانی تابع مربوط به ثبت درخواست
               iScsc.GRN_TSAV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", rqst.RQID)                        
                     )
                  )
               );
            }

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

      private void RqstBnAResn1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnADoc1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnRegl01_Click(object sender, EventArgs e)
      {
         var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
         if (Rg1 == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "031")))}
                  })
            );
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         Back_Butn_Click(null, null);
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }

      private void AddDevServ_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            var rqro = RqroBs.Current as Data.Request_Row;
            if(rqro == null)return;

            if (PdsrBs.List.OfType<Data.Product_Service>().Any(p => p.CODE == 0)) return;

            var pdsr = PdsrBs.AddNew() as Data.Product_Service;
            pdsr.Request_Row = rqro;

            iScsc.Product_Services.InsertOnSubmit(pdsr);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            RqstBnASav1.Enabled = false;
            
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null || rqst.RQID == 0) return;

            PdcsBs.DataSource =
               iScsc.Payment_Detail_Commodity_Sales
               .Where(p => 
                  p.Payment_Detail.Request_Row.FIGH_FILE_NO == rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO &&
                  p.Payment_Detail.Request_Row.Request.RQST_STAT == "002"
               );

            RqstBnASav1.Enabled = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SlctDev2Serv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pdcs = PdcsBs.Current as Data.Payment_Detail_Commodity_Sale;
            if (pdcs == null) return;

            if (PdsrBs.List.OfType<Data.Product_Service>().Any(p => p.PDCS_CODE == pdcs.CODE)) return;

            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            var rqro = RqroBs.Current as Data.Request_Row;
            if (rqro == null) return;

            if (PdsrBs.List.OfType<Data.Product_Service>().Any(p => p.CODE == 0)) return;

            var pdsr = PdsrBs.AddNew() as Data.Product_Service;
            pdsr.Request_Row = rqro;
            pdsr.Payment_Detail_Commodity_Sale = pdcs;
            pdsr.MAKE_DATE = pdcs.Product.MAKE_DATE;
            pdsr.EXPR_DATE = pdcs.Product.EXPR_DATE;
            pdsr.SERL_NO = pdcs.Product.SERL_NUMB;
            pdsr.STAT = "001";

            iScsc.Product_Services.InsertOnSubmit(pdsr);

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

      private void DelDevServ_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pdsr = PdsrBs.Current as Data.Product_Service;
            if (pdsr == null) return;

            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            // چک کنیم که آیا درخواست هزینه دار ثبت شده یا خیر
            if(iScsc.Requests.Any(r => r.RQST_RQID == rqst.RQID && r.LETT_NO == pdsr.SERL_NO && r.Payments.Any()))
            {
               throw new Exception("برای خدمات گارانتی هزینه صادر شده، لطفا با بخش حسابداری یا مدیریت هماهنگ کنید.");
            }

            iScsc.Product_Services.DeleteOnSubmit(pdsr);

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

      private void SmsDevServ_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pdsr = PdsrBs.Current as Data.Product_Service;
            if (pdsr == null) return;

            var rqst = RqstBs.Current as Data.Request;
            var figh = rqst.Request_Rows.FirstOrDefault().Fighter;

            var phonnumbfrmt = new Regex(@"^09\d{2}\s*?\d{3}\s*?\d{4}$");
            if (figh.CELL_PHON_DNRM == null || !phonnumbfrmt.IsMatch(figh.CELL_PHON_DNRM))
            {
               throw new Exception("شماره تلفن وارد شده صحیح نمی باشد.");
            }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "DefaultGateway",
                        new List<Job>
                        {
                           new Job(SendType.External, "Msgb",
                              new List<Job>
                              {
                                 new Job(SendType.Self, 07 /* Execute Send_Mesg_F */),
                                 new Job(SendType.SelfToUserInterface, "SEND_MESG_F", 10 /* Execute Actn_Calf_P */)
                                 {
                                    Input = 
                                       new XElement("Message", 
                                          new XAttribute("tab", "tp_001"), 
                                          new XAttribute("subsys", "5"), 
                                          new XAttribute("cellphon", figh.CELL_PHON_DNRM), 
                                          new XAttribute("rfid", rqst.RQID), 
                                          new XAttribute("key1", pdsr.SERL_NO)
                                       )
                                 }
                              }
                           )
                        }
                     )
                  }
               )
            );

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "DefaultGateway:Msgb", 07 /* Execute Send_Mesg_F */, SendType.Self)
            //);
            
            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "DefaultGateway:Msgb:SEND_MESG_F", 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Message", new XAttribute("tab", "tp_001"), new XAttribute("subsys", "5"), new XAttribute("cellphon", figh.CELL_PHON_DNRM), new XAttribute("rfid", rqst.RQID), new XAttribute("key1", pdsr.SERL_NO)) }
            //);            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PdsrBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pdsr = PdsrBs.Current as Data.Product_Service;
            if (pdsr == null) return;

            var rqst = RqstBs.Current as Data.Request;

            SmsbBs.DataSource = iScsc.V_Sms_Message_Boxes.Where(s => s.RFID == rqst.RQID && s.KEY1_RFID == pdsr.SERL_NO);
            ExpnRqstPymtBs.DataSource =
               iScsc.Payments.Where(p => p.Request.RQST_RQID == rqst.RQID && p.Request.RQST_STAT == "002" && p.Request.LETT_NO == pdsr.SERL_NO);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExpnDevServ_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pdsr = PdsrBs.Current as Data.Product_Service;
            if (pdsr == null) return;

            var rqst = RqstBs.Current as Data.Request;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                  
                        new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Request", 
                                 new XAttribute("type", "01"), 
                                 new XElement("Request_Row", 
                                    new XAttribute("fileno", rqst.Fighters.FirstOrDefault().FILE_NO)),
                                 //new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
                                 new XAttribute("rqstrqid", rqst.RQID),
                                 new XAttribute("lettno", pdsr.SERL_NO)
                                 //new XAttribute("lettdate", pdsr.Request_Row.Request.RQST_DATE)
                              )
                        }
                     })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
