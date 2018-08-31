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
using System.CRM.ExceptionHandlings;
using System.Globalization;
using System.IO;
using Itenso.TimePeriod;
using DevExpress.XtraGrid.Views.Grid;
using System.MaxUi;
using DevExpress.XtraEditors;
using System.CRM.ExtCode;

namespace System.CRM.Ui.Leads
{
   public partial class INF_LEAD_F : UserControl
   {
      public INF_LEAD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool queryAllRequest = false;
      private int rqstindex;

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

            rqstindex = RqstBs.Position;

            var Rqids = iCRM.VF_Requests(new XElement("Request", new XAttribute("cretby", queryAllRequest ? "" : CurrentUser)))
               .Where(rqst =>
                     rqst.RQTP_CODE == "014" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.RQTT_CODE == "004" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs.DataSource =
               iCRM.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               )
               .OrderByDescending(
                  rqst =>
                     rqst.RQST_DATE
               );

            RqstBs.Position = rqstindex;
            requery = false;
         }
         catch { }
      }

      private void ObjectBaseEdit_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
            {
               var control = sender as BaseEdit;
               if (control != null)
                  control.EditValue = null;
            }
         }
         catch { }
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

      private void IsicGrop_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            IsicActvBs.DataSource = iCRM.Isic_Activities.Where(a => a.ISCG_CODE == e.NewValue.ToString());
         }
         catch { }
      }

      private void IsicActv_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            IsicProdBs.DataSource = iCRM.Isic_Products.Where(p => p.ISCA_CODE == e.NewValue.ToString() && p.ISCA_ISCG_CODE == IsicGrop_Lov.EditValue.ToString());
         }
         catch { }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            EmstClosDate_Dt.CommitChanges();
            var leadtype = xinput.Attribute("type").Value;

            var rqst = RqstBs.Current as Data.Request;
            if (rqst != null && rqst.RQID != 0)
               leadtype += "update";

            #region New Lead & Update
            if (leadtype.In("newlead"))
            {
               if (Topc_Txt.EditValue == null || Topc_Txt.EditValue.ToString() == "") { Topc_Txt.Focus(); return; }

               if (FrstName_Txt.EditValue == null || FrstName_Txt.EditValue.ToString() == "") { FrstName_Txt.Focus(); return; }
               if (LastName_Txt.EditValue == null || LastName_Txt.EditValue.ToString() == "") { LastName_Txt.Focus(); return; }

               if (Cnty_Lov.EditValue == null || Cnty_Lov.EditValue.ToString() == "") { Cnty_Lov.Focus(); return; }
               if (Prvn_Lov.EditValue == null || Prvn_Lov.EditValue.ToString() == "") { Prvn_Lov.Focus(); return; }
               if (Regn_Lov.EditValue == null || Regn_Lov.EditValue.ToString() == "") { Regn_Lov.Focus(); return; }
               
               if (Name_Txt.EditValue == null || Name_Txt.EditValue.ToString() == "") { Name_Txt.EditValue = "نامشخص"; }
            }

            if (leadtype.In("newlead", "newleadupdate"))
            {
               iCRM.OPR_LEAD_P(
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("rqstrqid", ""),
                     new XAttribute("cntycode", Cnty_Lov.EditValue),
                     new XAttribute("prvncode", Prvn_Lov.EditValue),
                     new XAttribute("regncode", Regn_Lov.EditValue),
                     new XAttribute("leadtype", leadtype),
                     new XElement("Service",
                        new XAttribute("fileno", FileNo_Txt.EditValue),
                        new XElement("Service_Public",
                           new XElement("Frst_Name", FrstName_Txt.EditValue),
                           new XElement("Last_Name", LastName_Txt.EditValue),
                           new XElement("Tell_Phon", TellPhon_Txt.EditValue),
                           new XElement("Cell_Phon", CellPhon_Txt.EditValue),
                           new XElement("Emal_Adrs", EmalAdrs_Txt.EditValue)
                        )
                     ),
                     new XElement("Company",
                        new XAttribute("code", CompCode_Txt.EditValue),
                        new XElement("Name", Name_Txt.EditValue),
                        new XElement("Web_Site", WebSite_Txt.EditValue),
                        new XElement("Regn_Prvn_Cnty_Code", Cnty_Lov.EditValue),
                        new XElement("Regn_Prvn_Code", Prvn_Lov.EditValue),
                        new XElement("Regn_Code", Regn_Lov.EditValue),
                        new XElement("Post_Adrs", PostAdrs_Txt.EditValue),
                        new XElement("Iscp_Isca_Iscg_Code", IsicGrop_Lov.EditValue),
                        new XElement("Iscp_Isca_Code", IsicActv_Lov.EditValue),
                        new XElement("Iscp_Code", IsicProd_Lov.EditValue),
                        new XElement("Trcb_Tcid", TrcbTcid_Lov.EditValue),
                        new XElement("Econ_Code", EconCode_Txt.EditValue),
                        new XElement("Comp_Desc", CompDesc_Txt.EditValue),
                        new XElement("Cont_Mtod", ContMtod_Lov.EditValue ?? "000"),
                        new XElement("Alow_Emal", AlowEmal_Lov.EditValue ?? "001"),
                        new XElement("Alow_Bulk_Emal", AlowBulkEmal_Lov.EditValue ?? "001"),
                        new XElement("Alow_Phon", AlowPhon_Lov.EditValue ?? "001"),
                        new XElement("Alow_Fax", AlowFax_Lov.EditValue ?? "001"),
                        new XElement("Alow_Lett", AlowLett_Lov.EditValue ?? "001")
                     ),
                     new XElement("Lead",
                        new XAttribute("ownrcode", Ownr_Lov.EditValue),
                        new XElement("Ldid", LeadId_Txt.EditValue),
                        new XElement("Camp_Cmid", Camp_Lov.EditValue),
                        new XElement("Topc", Topc_Txt.EditValue),
                        new XElement("Prch_Time", PrchTime_Txt.EditValue ?? "000"),
                        new XElement("Bdgt_Amnt", BdgtAmnt_Txt.EditValue),
                        new XElement("Prch_Proc", PrchProc_Txt.EditValue ?? "000"),
                        new XElement("Sorc", Sorc_Lov.EditValue ?? "000"),
                        new XElement("Rtng", Rtng_Lov.EditValue ?? "000"),
                        new XElement("Stat", Stat_Lov.EditValue ?? "000"),                        
                        new XElement("Cust_Need", CustNeed_Txt.EditValue),
                        new XElement("Crnt_Situ", CrntSitu_Txt.EditValue),
                        new XElement("Emst_Clos_Date", EmstClosDate_Dt.Value.HasValue ? EmstClosDate_Dt.Value.Value.ToString("yyyy-MM-dd") : ""),
                        new XElement("Emst_Revn_Amnt", EmstRevnAmnt_Txt.EditValue),
                        new XElement("Prop_Solt", PropSolt_Txt.EditValue),
                        new XElement("Send_Thnk_You", SendThnkYou_Lov.EditValue ?? "001")
                     )
                  )
               );
            }
            #endregion

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

      private void SubmitChangeClose_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ExecuteQuery_Butn_ButtonClick(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void ExecQuryCrntUser_Butn_Click(object sender, EventArgs e)
      {
         queryAllRequest = false;
         Execute_Query();
      }

      private void ExecQuryAllUser_Butn_Click(object sender, EventArgs e)
      {
         queryAllRequest = true;
         Execute_Query();
      }

   }
}
