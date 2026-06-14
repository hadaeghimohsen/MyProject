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
using System.CRM.ExceptionHandlings;
using DevExpress.XtraEditors;

namespace System.CRM.Ui.Activity
{
   public partial class OPT_CLNC_F : UserControl
   {
      public OPT_CLNC_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long code;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ParentCompBs.DataSource = iCRM.Companies.Where(c => c.RECD_STAT == "002" && c.CODE == code);
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Comp_Lov.EditValue == null || Comp_Lov.EditValue.ToString() == "") { Comp_Lov.Focus(); return; }
            if (Name_Txt.EditValue == null || Name_Txt.EditValue.ToString() == "") { Name_Txt.Focus(); return; }
            if (PostAddr_Txt.EditValue == null || PostAddr_Txt.EditValue.ToString() == "") { PostAddr_Txt.Focus(); return; }

            iCRM.CLON_COMP_P(
               new XElement("Company",
                  new XAttribute("compcode", Comp_Lov.EditValue),

                  new XAttribute("cntycode", Cnty_Lov.EditValue ?? ""),
                  new XAttribute("prvncode", Prvn_Lov.EditValue ?? ""),
                  new XAttribute("regncode", Regn_Lov.EditValue ?? ""),

                  new XAttribute("iscgcode", Iscg_Lov.EditValue ?? ""),
                  new XAttribute("iscacode", Isca_Lov.EditValue ?? ""),
                  new XAttribute("iscpcode", Iscp_Lov.EditValue ?? ""),

                  new XAttribute("name", Name_Txt.EditValue ?? ""),
                  new XAttribute("postadrs", PostAddr_Txt.EditValue ?? ""),
                  new XAttribute("emaladrs", EmalAddr_Txt.EditValue ?? ""),
                  new XAttribute("website", WebSite_Txt.EditValue ?? ""),                  
                  new XAttribute("regsdate", RegsDate_Dat.Value ?? DateTime.Now),
                  new XAttribute("zipcode", ZipCode_Txt.EditValue ?? ""),
                  new XAttribute("econcode", EconCode_Txt.EditValue ?? ""),
                  new XAttribute("strttime", StrtTime_Tim.EditValue ?? ""),
                  new XAttribute("endtime", EndTime_Tim.EditValue ?? ""),
                  new XAttribute("billaddrx", BillXCord_Txt.EditValue ?? ""),
                  new XAttribute("billaddry", BillYCord_Txt.EditValue ?? ""),
                  new XAttribute("billaddrzoom", BillAddrZoom_Txt.EditValue ?? ""),
                  new XAttribute("shipaddrx", ShipXCord_Txt.EditValue ?? ""),
                  new XAttribute("shipaddry", ShipYCord_Txt.EditValue ?? ""),
                  new XAttribute("shipaddrzoom", ShipAddrZoom_Txt.EditValue ?? ""),
                  new XAttribute("dfltstat", DfltStat_Butn.Tag ?? ""),
                  new XAttribute("facebookurl", Facebook_Txt.EditValue ?? ""),
                  new XAttribute("linkinurl", LinkedIn_Txt.EditValue ?? ""),
                  new XAttribute("twtrurl", Twiter_Txt.EditValue ?? ""),
                  new XAttribute("recdstat", "002"),
                  new XElement("WeekDays",
                     Weekdays_Flp.Controls.OfType<SimpleButton>().
                     Select(w => 
                        new XElement("WeekDay",
                           new XAttribute("code", w.Tag),
                           new XAttribute("stat", w.Appearance.BackColor == Color.YellowGreen ? "002" : "001")
                        )
                     )
                  )
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Btn_Back_Click(null, null);
               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 38 /* Execute Shw_Cont_F */),
                      new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 10 /* Execute Actn_CalF_P */)
                      {
                         Executive = ExecutiveType.Asynchronous,
                         Input = 
                           new XElement("Company", 
                              new XAttribute("onoftag", "on")
                           )
                      }
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
      }

      private void Cnty_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            PrvnBs.DataSource = iCRM.Provinces.Where(p => p.CNTY_CODE == e.NewValue.ToString());
         }
         catch { }
      }

      private void Prvn_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            RegnBs.DataSource = iCRM.Regions.Where(r => r.PRVN_CODE == e.NewValue.ToString() && r.PRVN_CNTY_CODE == Cnty_Lov.EditValue.ToString());
         }
         catch { }
      }

      private void Iscg_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            IscaBs.DataSource = iCRM.Isic_Activities.Where(a => a.ISCG_CODE == e.NewValue.ToString());
         }
         catch { }
      }

      private void Isca_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            IscpBs.DataSource = iCRM.Isic_Products.Where(p => p.ISCA_CODE == e.NewValue.ToString() && p.ISCA_ISCG_CODE == Iscg_Lov.EditValue.ToString());
         }
         catch { }
      }

      private void DfltStat_Butn_Click(object sender, EventArgs e)
      {
         if (DfltStat_Butn.Appearance.BackColor == Color.Gainsboro)
         {
            DfltStat_Butn.Appearance.BackColor = Color.YellowGreen;
            DfltStat_Butn.Text = "بلی";
            DfltStat_Butn.Tag = "002";
         }
         else
         {
            DfltStat_Butn.Appearance.BackColor = Color.Gainsboro;
            DfltStat_Butn.Text = "خیر";
            DfltStat_Butn.Tag = "001";
         }
      }

      private void Wkdy00i_Butn_Click(object sender, EventArgs e)
      {
         var sb = sender as SimpleButton;

         if (sb.Appearance.BackColor == Color.Gainsboro)
            sb.Appearance.BackColor = Color.YellowGreen;
         else
            sb.Appearance.BackColor = Color.Gainsboro;
      }

      private void BillCord_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
            {
               Input = 
                  new XElement("GMapNets",
                     new XAttribute("requesttype", "get"),
                     new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                     new XAttribute("callback", 40 /* CordinateGetSet */),
                     new XAttribute("outputtype", "billaddress"),
                     new XAttribute("initalset", true),
                     new XAttribute("cordx", string.IsNullOrEmpty(BillXCord_Txt.Text) ? "29.622045" : BillXCord_Txt.Text),
                     new XAttribute("cordy", string.IsNullOrEmpty(BillYCord_Txt.Text) ? "52.522728" : BillYCord_Txt.Text),
                     new XAttribute("zoom", string.IsNullOrEmpty(BillAddrZoom_Txt.Text) ? "1800" : BillAddrZoom_Txt.Text)
                  )
            }
         );
      }

      private void ShipCord_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
            {
               Input =
                  new XElement("GMapNets",
                     new XAttribute("requesttype", "get"),
                     new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                     new XAttribute("callback", 40 /* CordinateGetSet */),
                     new XAttribute("outputtype", "shippingaddress"),
                     new XAttribute("initalset", true),
                     new XAttribute("cordx", string.IsNullOrEmpty(ShipXCord_Txt.Text) ? "29.622045" : ShipXCord_Txt.Text),
                     new XAttribute("cordy", string.IsNullOrEmpty(ShipYCord_Txt.Text) ? "52.522728" : ShipYCord_Txt.Text),
                     new XAttribute("zoom", string.IsNullOrEmpty(ShipAddrZoom_Txt.Text) ? "1800" : ShipAddrZoom_Txt.Text)
                  )
            }
         );
      }      
   }
}
