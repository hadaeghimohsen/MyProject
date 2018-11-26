using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.UserAction
{
   public partial class USR_CTBL_F : UserControl
   {
      public USR_CTBL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>                        
                        rqst.RQST_STAT == "001" &&                        
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

         RqstBs1.DataSource =
            iScsc.Requests
            .Where(
               rqst =>
                  Rqids.Contains(rqst.RQID)
            )
            .OrderByDescending(
               rqst =>
                  rqst.RQST_DATE
            );
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Reload_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void CnclAllRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انصراف و حذف درخواست ها مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            foreach (var Rqst in RqstBs1.List.OfType<Data.Request>())
            {
               if (Rqst != null && Rqst.RQID > 0)
               {
                  /*
                   *  Remove Data From Tables
                   */
                  iScsc.CNCL_RQST_F(
                     new XElement("Process",
                        new XElement("Request",
                           new XAttribute("rqid", Rqst.RQID)
                        )
                     )
                  );
               }
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
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         switch (e.Button.Index)
         {
            case 0:
               var Rqst = RqstBs1.Current as Data.Request;
               if (Rqst == null) return;

               int SpecFormNumb = 0;
               string SpecFormString = "";

               switch(Rqst.RQTP_CODE)
               {
                  case "001":
                     // ثبت نام
                     SpecFormNumb = 123;
                     SpecFormString = "ADM_FIGH_F";
                     break;
                  case "002":
                     // تغییر مشخصات عمومی
                     SpecFormNumb = 70;
                     SpecFormString = "ADM_CHNG_F";
                     break;
                  case "009":
                     // تمدید دوره
                     SpecFormNumb = 64;
                     SpecFormString = "ADM_TOTL_F";
                     break;
                  case "012":
                     // تمدید کارت بیمه
                     SpecFormNumb = 80;
                     SpecFormString = "INS_TOTL_F";
                     break;
                  case "016":
                     // درآمد متفرقه
                     SpecFormNumb = 92;
                     SpecFormString = "OIC_TOTL_F";
                     break;
                  case "020":
                     // تغییرات ریالی
                     SpecFormNumb = 153;
                     SpecFormString = "GLR_INDC_F";
                     break;
                  case "026":
                     // بلوکه کردن
                     SpecFormNumb = 133;
                     SpecFormString = "ADM_MBFZ_F";
                     break;
               }

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, SpecFormNumb /* Execute Specify Form */),
                        new Job(SendType.SelfToUserInterface, SpecFormString, 10 /* Execute Actn_Calf_F */)
                        {
                           Input = 
                              new XElement("Request",
                                 new XAttribute("type", "rqidfocus"),
                                 new XAttribute("rqid", Rqst.RQID)
                              )
                        }
                     }
                  )
               );

               Back_Butn_Click(null, null);

               break;
            case 1:
               try
               {
                  if (MessageBox.Show(this, "آیا با انصراف و حذف درخواست مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  Rqst = RqstBs1.Current as Data.Request;
                  {
                     if (Rqst != null && Rqst.RQID > 0)
                     {
                        /*
                         *  Remove Data From Tables
                         */
                        iScsc.CNCL_RQST_F(
                           new XElement("Process",
                              new XElement("Request",
                                 new XAttribute("rqid", Rqst.RQID)
                              )
                           )
                        );
                     }
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
                  {
                     Execute_Query();
                     requery = false;
                  }
               }
               break;
            default:
               break;
         }
      }
   }
}
