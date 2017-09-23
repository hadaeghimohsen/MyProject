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
using System.Data.SqlClient;

namespace System.Scsc.Ui.ReceiptAnnouncement
{
   public partial class MNG_RCAN_F : UserControl
   {
      public MNG_RCAN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if (tb_master.SelectedTab == tp_001)
         {
            mb_QueryPayIn_Click(null, null);
            Tpb_PayIn_PickCheckedChange(null);
            Tpb_CanclePayIn_PickCheckedChange(null);
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            VUserBs1.DataSource = iScsc.V_Users.Where(u => u.USER_DB != iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))));
            mb_QueryPayOut_Click(null, null);
            Tpb_PayTrn_PickCheckedChange(null);
            Tpb_PayOut_PickCheckedChange(null);
            Tpb_CancelPayOut_PickCheckedChange(null);
         }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void cb_cashoutcometoday_StatusChange(object sender)
      {
         Gb_CashOutDate.Visible = !cb_cashoutcometoday.Status;
      }

      private void Btn_PayOut_Click(object sender, EventArgs e)
      {
         try
         {
            if (RcanBs1.Current == null) return;

            iScsc.CRET_RCAN_P(
               new XElement("Receipt_Announcements",
                  new XElement("Receipt_Announcement", 
                     new XAttribute("touser", PayOutBy_LookUpEdit.EditValue ?? ""),
                     new XAttribute("amnt", (RcanBs1.Current as Data.Receipt_Announcement).AMNT),
                     new XAttribute("rcanstat", "005"),
                     new XAttribute("autodoc", "001"),
                     new XAttribute("rcanraid", (RcanBs1.Current as Data.Receipt_Announcement).RAID)
                  )
               )
            );
            MessageBox.Show("عملیات انتقال وجه با موفقیت انجام شد. حال گیرنده اگر انتقال را تایید کنند کل عملیات پرداخت با موفقیت انجام میشود.");
            requery = true;
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Tpb_PayTrn_PickCheckedChange(object sender)
      {
         if (!Tpb_PayTrn.PickChecked) return;

         PaySharedBs1.DataSource = iScsc.Receipt_Announcements.Where(ra => ra.FROM_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))) && ra.RCAN_STAT == "005");
      }

      private void Tpb_PayOut_PickCheckedChange(object sender)
      {
         if (!Tpb_PayOut.PickChecked) return;

         PaySharedBs1.DataSource = iScsc.Receipt_Announcements.Where(ra => ra.FROM_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))) && ra.RCAN_STAT == "001");
      }

      private void Tpb_CancelPayOut_PickCheckedChange(object sender)
      {
         if (!Tpb_CancelPayOut.PickChecked) return;

         PaySharedBs1.DataSource = iScsc.Receipt_Announcements.Where(ra => ra.FROM_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))) && ra.RCAN_STAT == "004");
      }

      private void mb_QueryPayOut_Click(object sender, EventArgs e)
      {
         if (cb_cashoutcometoday.Status)
            RcanBs1.DataSource = iScsc.Receipt_Announcements.Where(ra =>  ra.ACTN_DATE.Date == DateTime.Now.Date && ra.RCAN_STAT == "003" && ra.TO_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))));
         else
            RcanBs1.DataSource = 
               iScsc.Receipt_Announcements.Where(ra => 
                  ra.ACTN_DATE.Date <= (Pdt_CashOutPaymentFromDate.Value ?? DateTime.Now.Date) && 
                  ra.ACTN_DATE.Date >= (Pdt_CashOutPaymentToDate.Value ?? DateTime.Now.AddYears(-100).Date) && 
                  ra.RCAN_STAT == "003" && 
                  ra.TO_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")))
               );
      }

      private void Tpb_PayIn_PickCheckedChange(object sender)
      {
         if(!Tpb_PayIn.PickChecked) return;

         PaySharedBs1.DataSource = iScsc.Receipt_Announcements.Where(ra => ra.TO_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))) && ra.RCAN_STAT == "003");
      }

      private void Tpb_CanclePayIn_PickCheckedChange(object sender)
      {
         if (!Tpb_CanclePayIn.PickChecked) return;

         PaySharedBs1.DataSource = iScsc.Receipt_Announcements.Where(ra => ra.TO_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))) && ra.RCAN_STAT == "004");
      }

      private void mb_QueryPayIn_Click(object sender, EventArgs e)
      {
         if (cb_cashincometoday.Status)
            RcanBs1.DataSource = iScsc.Receipt_Announcements.Where(ra => ra.ACTN_DATE.Date == DateTime.Now.Date && ra.RCAN_STAT == "005" && ra.TO_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001"))));
         else
            RcanBs1.DataSource =
               iScsc.Receipt_Announcements.Where(ra =>
                  ra.ACTN_DATE.Date <= (Pdt_CashInPaymentFromDate.Value ?? DateTime.Now.Date) &&
                  ra.ACTN_DATE.Date >= (Pdt_CashInPaymentToDate.Value ?? DateTime.Now.AddYears(-100).Date) &&
                  ra.RCAN_STAT == "005" &&
                  ra.TO_USER == iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")))
               );
      }

      private void Btn_PayIn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RcanBs1.Current == null) return;

            iScsc.CRET_RCAN_P(
               new XElement("Receipt_Announcements",
                  new XElement("Receipt_Announcement",
                     new XAttribute("rcanstat", "003"),                     
                     new XAttribute("raid", (RcanBs1.Current as Data.Receipt_Announcement).RAID)
                  )
               )
            );
            MessageBox.Show("عملیات دریافت وجه با موفقیت انجام شد و مبلغ به حساب شما قرار گرفت.");
            requery = true;
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
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

      private void Btn_CancelPayIn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RcanBs1.Current == null) return;
            iScsc.CRET_RCAN_P(
               new XElement("Receipt_Announcements",
                  new XElement("Receipt_Announcement",
                     new XAttribute("rcanstat", "004"),
                     new XAttribute("raid", (RcanBs1.Current as Data.Receipt_Announcement).RAID)
                  )
               )
            );
            MessageBox.Show("عملیات عدم دریافت وجه با موفقیت انجام شد و مبلغ به حساب فرستنده برگشت داده شده.");
            requery = true;
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
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

      private void cb_cashincometoday_StatusChange(object sender)
      {
         Gb_CashInDate.Visible = !cb_cashincometoday.Status;
      }
   }
}
