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
      private string formType = "normal";

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

            if (formType == "normal") // in process
            {
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
            }
            else if(formType == "showhistory") // ended
            {
               var Rqids = iCRM.VF_Requests(new XElement("Request", new XAttribute("cretby", queryAllRequest ? "" : CurrentUser)))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "014" &&
                        //rqst.RQST_STAT == "001" &&
                        rqst.RQID == rqid &&
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
            }
            RqstBs.Position = rqstindex;
            requery = false;
         }
         catch { }
      }

      private void RqstBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            SubmitChange_Butn.Enabled = SubmitChangeClose_Butn.Enabled = RqstCncl_Butn.Enabled = Approve_Butn.Enabled = Disapprove_Butn.Enabled = RqstAdd_Butn.Enabled = true;

            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null || rqst.RQID == 0) 
            {
               Approve_Butn.Image = Properties.Resources.IMAGE_1635;
               Approve_Butn.ToolTipText = "تایید صلاحیت";
               Disapprove_Butn.Image = Properties.Resources.IMAGE_1636;
               Disapprove_Butn.ToolTipText = "عدم تایید صلاحیت";

               RqstMsttColor_Butn.HoverColorA = RqstMsttColor_Butn.HoverColorB = RqstMsttColor_Butn.NormalColorA = RqstMsttColor_Butn.NormalColorB = Color.Gold;
               return; 
            }

            if(rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1)
            {
               Approve_Butn.Image = Properties.Resources.IMAGE_1635;
               Approve_Butn.ToolTipText = "تایید صلاحیت";
               Disapprove_Butn.Image = Properties.Resources.IMAGE_1636;
               Disapprove_Butn.ToolTipText = "عدم تایید صلاحیت";
            }
            else
            {
               Approve_Butn.Image = Properties.Resources.IMAGE_1643;
               Approve_Butn.ToolTipText = "بستن به عنوان برنده";
               Disapprove_Butn.Image = Properties.Resources.IMAGE_1644;
               Disapprove_Butn.ToolTipText = "بستن به عنوان از دست رفته";
            }
            
            if(rqst.SSTT_MSTT_CODE == 99 && rqst.SSTT_CODE == 99)
            {
               SubmitChange_Butn.Enabled = SubmitChangeClose_Butn.Enabled = RqstCncl_Butn.Enabled = Approve_Butn.Enabled = Disapprove_Butn.Enabled = RqstAdd_Butn.Enabled = false;
            }

            RqstMsttColor_Butn.HoverColorA = RqstMsttColor_Butn.HoverColorB = RqstMsttColor_Butn.NormalColorA = RqstMsttColor_Butn.NormalColorB = ColorTranslator.FromHtml(rqst.COLR);

            // 1397/08/27            
            if(rqst.Request_Rows.FirstOrDefault().Service.CONF_STAT == "002")
               xinput.Attribute("type").Value = "servicelead";
            else
               if(rqst.Request_Rows.FirstOrDefault().Company != null)
                  xinput.Attribute("type").Value = "companylead";
               else
                  xinput.Attribute("type").Value = "newlead";

         }
         catch { }
      }

      private void SrpbBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var srpb = SrpbBs.Current as Data.Service_Public;
            if (srpb == null) { CompBs.List.Clear(); return; }

            CompBs.DataSource = iCRM.Companies.Where(c => c.CODE == srpb.COMP_CODE);
            PrvnBs.DataSource = iCRM.Provinces.Where(p => p.CNTY_CODE == srpb.REGN_PRVN_CNTY_CODE);
            RegnBs.DataSource = iCRM.Regions.Where(r => r.PRVN_CNTY_CODE == srpb.REGN_PRVN_CNTY_CODE && r.PRVN_CODE == srpb.REGN_PRVN_CODE);
            IsicGropBs.DataSource = iCRM.Isic_Groups.Where(g => g.CODE == srpb.ISCP_ISCA_ISCG_CODE);
            IsicActvBs.DataSource = iCRM.Isic_Activities.Where(a => a.ISCG_CODE == srpb.ISCP_ISCA_ISCG_CODE);
            IsicProdBs.DataSource = iCRM.Isic_Products.Where(p => p.ISCA_ISCG_CODE == srpb.ISCP_ISCA_ISCG_CODE && p.ISCA_CODE == srpb.ISCP_ISCA_CODE);            
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

            var sp = SrpbBs.Current as Data.Service_Public;
            var lead = LeadBs.Current as Data.Lead;

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

            if (leadtype.In("newlead", "newleadupdate", "serviceleadupdate", "companyleadupdate"))
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
                        new XAttribute("fileno", sp == null ? "" : sp.SERV_FILE_NO.ToString()),
                        new XElement("Service_Public",
                           new XElement("Frst_Name", FrstName_Txt.EditValue),
                           new XElement("Last_Name", LastName_Txt.EditValue),
                           new XElement("Tell_Phon", TellPhon_Txt.EditValue),
                           new XElement("Cell_Phon", CellPhon_Txt.EditValue),
                           new XElement("Emal_Adrs", EmalAdrs_Txt.EditValue)
                        )
                     ),
                     new XElement("Company",
                        new XAttribute("code", sp == null ? 0 : sp.COMP_CODE),
                        new XElement("Name", Name_Txt.EditValue),
                        new XElement("Web_Site", WebSite_Txt.EditValue),
                        new XElement("Regn_Prvn_Cnty_Code", Cnty_Lov.EditValue),
                        new XElement("Regn_Prvn_Code", Prvn_Lov.EditValue),
                        new XElement("Regn_Code", Regn_Lov.EditValue),
                        new XElement("Post_Adrs", PostAdrs_Txt.EditValue),
                        new XElement("Prim_Serv_File_No", sp == null ? "" : sp.SERV_FILE_NO.ToString()),
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
                        new XElement("Ldid", lead == null ? "" : lead.LDID.ToString()),
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
                        new XElement("Expn_Cost_Amnt", ExpnCostAmnt_Txt.EditValue),
                        new XElement("Prop_Solt", PropSolt_Txt.EditValue),
                        new XElement("Send_Thnk_You", SendThnkYou_Lov.EditValue ?? "001"),
                        new XElement("Send_Camp_Info", SendCampInfo_Lov.EditValue ?? "001"),
                        new XElement("Psbl_Numb", PsblNumb_Txt.EditValue ?? "0")
                     )
                  )
               );
            }
            #endregion
            #region Service Lead
            if (leadtype.In("servicelead", "companylead"))
            {
               iCRM.OPR_LEAD_P(
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("rqstrqid", ""),
                     new XAttribute("leadtype", leadtype),
                     new XElement("Service",
                        new XAttribute("fileno", fileno)
                     ),
                     new XElement("Company",
                        new XAttribute("code", compcode)
                     ),
                     new XElement("Lead",
                        new XAttribute("ownrcode", Ownr_Lov.EditValue)
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

      private void RqstCncl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null || rqst.RQID == 0) return;

            if (MessageBox.Show(this, "آیا با انصراف درخواست موافق هستید؟", "انصراف درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.CNCL_RQST_P(
               new XElement("Request",
                  new XAttribute("rqid", rqst.RQID)
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
               Execute_Query();
         }
      }

      private void Approve_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            string conftype = "", colr = "";

            // ثبت موقت باشد
            if (rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1)
            {
               conftype = "002";
               colr = ColorTranslator.ToHtml(Color.YellowGreen);
               //rqst.SSTT_CODE = 3;
               //rqst.COLR = ColorTranslator.ToHtml(Color.YellowGreen);

               iCRM.CONF_LEAD_P(
                  new XElement("Lead",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("colr", colr),
                     new XAttribute("conftype", conftype)
                  )
               );

               requery = true;
            }
            // عدم تایید صلاحیت
            //else if ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 4) && MessageBox.Show(this, "سرنخ تجاری شما قبلا در وضعیت عدم تایید صلاحیت قرار گرفته است.\n\rآیا مایل به تایید صلاحیت آن هستید؟", "تایید صلاحیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading) == DialogResult.Yes)
            //{
            //   conftype = "002";
            //   //rqst.SSTT_CODE = 3;
            //   //rqst.COLR = ColorTranslator.ToHtml(Color.YellowGreen);
            //}
            // موفق به فروش
            else if (rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 3)
            {
               conftype = "003";
               colr = ColorTranslator.ToHtml(Color.Lime);
               // تبدیل شدن به فروش نهایی و ثبت اطلاعات مشتری و شرکت در سیستم

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 102 /* Execute Rsl_Lead_F */),
                      new Job(SendType.SelfToUserInterface, "RSL_LEAD_F", 10 /* Execute Actn_Calf_F */)
                      {
                         Input = 
                           new XElement("Lead",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("conftype", conftype),
                              new XAttribute("colr", colr),
                              new XAttribute("ldid", lead.LDID)
                           )
                      }
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }            
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally {
            if (requery)
               Execute_Query();
         }
      }

      private void Disapprove_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            string conftype = "", colr = "";

            // ثبت موقت باشد
            if (rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1)
            {
               conftype = "001";
               colr = ColorTranslator.ToHtml(Color.Red);
               //rqst.SSTT_CODE = 4;
               //rqst.COLR = ColorTranslator.ToHtml(Color.Red);

               iCRM.CONF_LEAD_P(
                  new XElement("Lead",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("colr", colr),
                     new XAttribute("conftype", conftype)
                  )
               );

               requery = true;
            }
            // عدم تایید صلاحیت
            //else if ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 3) && MessageBox.Show(this, "سرنخ تجاری شما قبلا در وضعیت تایید صلاحیت قرار گرفته است.\n\rآیا مایل به عدم تایید صلاحیت آن هستید؟", "عدم تایید صلاحیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading) == DialogResult.Yes)
            //{
            //   //rqst.SSTT_CODE = 4;
            //   //rqst.COLR = ColorTranslator.ToHtml(Color.Red);
            //}
            // شکست در فروش محصول
            else if (rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 3)
            {
               conftype = "004";
               colr = ColorTranslator.ToHtml(Color.LightGray);

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 102 /* Execute Rsl_Lead_F */),
                      new Job(SendType.SelfToUserInterface, "RSL_LEAD_F", 10 /* Execute Actn_Calf_F */)
                      {
                         Input = 
                           new XElement("Lead",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("conftype", conftype),
                              new XAttribute("colr", colr),
                              new XAttribute("ldid", lead.LDID)
                           )
                      }
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }            
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

      #region Note
      private void AddNote_Butn_Click(object sender, EventArgs e)
      {
         var lead = LeadBs.Current as Data.Lead;
         if (lead == null || lead.Request_Row == null) return;
         if (NoteBs.List.OfType<Data.Note>().Any(n => n.NTID == 0)) return;

         NoteBs.AddNew();
         var note = NoteBs.Current as Data.Note;
         note.Request_Row = lead.Request_Row;
         note.SERV_FILE_NO = lead.SRPB_SERV_FILE_NO;
         note.COMP_CODE_DNRM = lead.COMP_CODE;

         Note_Gv.SelectRow(Note_Gv.RowCount - 1);

         iCRM.Notes.InsertOnSubmit(note);
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Note_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Note)Note_Gv.GetRow(r);
               iCRM.Notes.DeleteOnSubmit(row);
            }

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            NoteBs.EndEdit();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }
      #endregion

      #region Sale Team
      private void AddSaleTeam_Butn_Click(object sender, EventArgs e)
      {
         var lead = LeadBs.Current as Data.Lead;
         if (lead == null) return;
         if (SltmBs.List.OfType<Data.Sale_Team>().Any(st => st.STID == 0)) return;

         SltmBs.AddNew();
         var sltm = SltmBs.Current as Data.Sale_Team;
         sltm.LEAD_LDID = lead.LDID;

         Sltm_Gv.SelectRow(Sltm_Gv.RowCount - 1);

         iCRM.Sale_Teams.InsertOnSubmit(sltm);
      }

      private void DelSaleTeam_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Sltm_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Sale_Team)Sltm_Gv.GetRow(r);
               iCRM.Sale_Teams.DeleteOnSubmit(row);
            }

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveSaleTeam_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SltmBs.EndEdit();
            Sltm_Gv.PostEditor();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowSaleTeam_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpSaleTeam_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Campatitor
      private void AddCampatitor_Butn_Click(object sender, EventArgs e)
      {
         var lead = LeadBs.Current as Data.Lead;
         if (lead == null) return;
         if (LdcmBs.List.OfType<Data.Lead_Competitor>().Any(lc => lc.LCID == 0)) return;

         LdcmBs.AddNew();
         var ldcm = LdcmBs.Current as Data.Lead_Competitor;
         ldcm.LEAD_LDID = lead.LDID;

         Ldcm_Gv.SelectRow(Ldcm_Gv.RowCount - 1);

         iCRM.Lead_Competitors.InsertOnSubmit(ldcm);
      }

      private void DelCampatitor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Ldcm_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Lead_Competitor)Ldcm_Gv.GetRow(r);
               iCRM.Lead_Competitors.DeleteOnSubmit(row);
            }

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveCampatitor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            LdcmBs.EndEdit();
            Ldcm_Gv.PostEditor();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowCampatitor_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpCampatitor_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Stakeholder
      private void NewStakeholder_Butn_Click(object sender, EventArgs e)
      {
         
      }

      private void AddStakeholder_Butn_Click(object sender, EventArgs e)
      {
         var lead = LeadBs.Current as Data.Lead;
         if (lead == null) return;
         if (StkhBs.List.OfType<Data.Stakeholder>().Any(sh => sh.SHID == 0)) return;

         StkhBs.AddNew();
         var stkh = StkhBs.Current as Data.Stakeholder;
         stkh.LEAD_LDID = lead.LDID;

         Stkh_Gv.SelectRow(Stkh_Gv.RowCount - 1);

         iCRM.Stakeholders.InsertOnSubmit(stkh);
      }

      private void DelStakeholder_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Ldcm_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Stakeholder)Stkh_Gv.GetRow(r);
               iCRM.Stakeholders.DeleteOnSubmit(row);
            }

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveStakeholder_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            StkhBs.EndEdit();
            Stkh_Gv.PostEditor();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowStakeholder_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpStakeholder_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion
   }
}
