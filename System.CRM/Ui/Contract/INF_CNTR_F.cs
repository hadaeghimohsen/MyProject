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

namespace System.CRM.Ui.Contract
{
   public partial class INF_CNTR_F : UserControl
   {
      public INF_CNTR_F()
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
                        rqst.RQTP_CODE == "015" &&
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
                        rqst.RQTP_CODE == "015" &&
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
            SubmitChange_Butn.Enabled = SubmitChangeClose_Butn.Enabled = RqstCncl_Butn.Enabled = ConfirmContract_Butn.Enabled = CopyContract_Butn.Enabled = RqstAdd_Butn.Enabled = true;

            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null || rqst.RQID == 0) 
            {
               RqstMsttColor_Butn.HoverColorA = RqstMsttColor_Butn.HoverColorB = RqstMsttColor_Butn.NormalColorA = RqstMsttColor_Butn.NormalColorB = Color.Gold;
               return; 
            }            
            
            if(rqst.SSTT_MSTT_CODE == 99 && rqst.SSTT_CODE == 99)
            {
               SubmitChange_Butn.Enabled = SubmitChangeClose_Butn.Enabled = RqstCncl_Butn.Enabled = ConfirmContract_Butn.Enabled = CopyContract_Butn.Enabled = RqstAdd_Butn.Enabled = false;
            }

            RqstMsttColor_Butn.HoverColorA = RqstMsttColor_Butn.HoverColorB = RqstMsttColor_Butn.NormalColorA = RqstMsttColor_Butn.NormalColorB = ColorTranslator.FromHtml(rqst.COLR);
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
            else if(e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 79 /* Execute Apbs_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("App_Base",
                                 new XAttribute("tablename", "CONTRACT"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch { }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            var casetype = xinput.Attribute("type").Value;

            var rqst = RqstBs.Current as Data.Request;

            if (Titl_Txt.EditValue == null || Titl_Txt.EditValue.ToString() == "") { Titl_Txt.Focus(); return; }
            if (ServFileNo_Lov.EditValue == null || ServFileNo_Lov.EditValue.ToString() == "") { ServFileNo_Lov.Focus(); return; }
            if (CompCode_Lov.EditValue == null || CompCode_Lov.EditValue.ToString() == "") { CompCode_Lov.Focus(); return; }

            iCRM.OPR_CASE_P(
               new XElement("Request",
                  new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                  new XAttribute("rqstrqid", ""),
                  new XAttribute("casetype", casetype),                  
                  new XElement("Request_Row",
                     new XAttribute("servfileno", ServFileNo_Lov.EditValue),
                     new XAttribute("compcode", CompCode_Lov.EditValue),
                     new XElement("Case",
                        new XAttribute("ownrcode", Ownr_Lov.EditValue),
                        new XAttribute("titl", Titl_Txt.Text),
                        new XAttribute("stat", Stat_Lov.EditValue),
                        new XAttribute("cmnt", Cmnt_Txt.Text),
                        new XAttribute("type", DscnType_Lov.EditValue),
                        new XAttribute("servlevl", ServLevl_Lov.EditValue)
                     )
                  )
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

      private void CreateChild_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            var lead = ContBs.Current as Data.Lead;
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

      private void ResolveCase_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            var cases = ContBs.Current as Data.Case;
            if (cases == null) return;

            Job _InteractWithCRM =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.Self, 106 /* Execute Rsl_Case_F */),
                     new Job(SendType.SelfToUserInterface, "RSL_CASE_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                        new XElement("Case",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("colr", ColorTranslator.ToHtml(Color.LightGray)),
                           new XAttribute("csid", cases.CSID)
                        )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithCRM);
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
         var lead = ContBs.Current as Data.Lead;
         if (lead == null) return;
         if (NoteBs.List.OfType<Data.Note>().Any(n => n.NTID == 0)) return;

         NoteBs.AddNew();
         var note = NoteBs.Current as Data.Note;
         note.LEAD_LDID = lead.LDID;
         note.SERV_FILE_NO = lead.SRPB_SERV_FILE_NO;
         note.COMP_CODE_DNRM = lead.COMP_CODE;

         Note_Gv.SelectRow(Note_Gv.RowCount - 1);

         iCRM.Notes.InsertOnSubmit(note);
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var lead = ContBs.Current as Data.Lead;
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

      private void RightMenuLink_Lb_Click(object sender, EventArgs e)
      {
         var menulink = sender as LabelControl;
         switch (menulink.Tag.ToString())
         {
            case "1":
               Titl_Txt.Focus();
               break;
            case "2":
               DscnType_Lov.Focus();
               break;
            case "3":
               
               break;
            default:
               break;
         }
      }
   }
}
