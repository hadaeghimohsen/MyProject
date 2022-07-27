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
using DevExpress.XtraEditors;
using System.Scsc.ExtCode;

namespace System.Scsc.Ui.Common
{
   public partial class ALL_FLDF_F : UserControl
   {
      public ALL_FLDF_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 07 /* Execute LoadData */, SendType.SelfToUserInterface) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
         );
         requery = false;
      }

      //private void button1_Click(object sender, EventArgs e)
      //{
      //   _DefaultGateway.Gateway(
      //      new Job(SendType.External, "localhost", "", 47, SendType.Self)
      //   );
      //}

      private void HL_INVSDCMT_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         if (vF_Request_DocumentBs.Current == null) return;

         var CrntRqstDcmt = vF_Request_DocumentBs.Current as Data.VF_Request_DocumentResult;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == CrntRqstDcmt.RQID && rr.RWNO == CrntRqstDcmt.RWNO).Single() }
         );
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void FighBnSettingPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void FighBnPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (vF_Last_Info_FighterBs.Current == null) return;
            dynamic crnt = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (crnt == null)
               crnt = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void FighBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (vF_Last_Info_FighterBs.Current == null) return;
            dynamic crnt = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (crnt == null)
               crnt = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void HL_InvsRqst_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         /* مشخص کردن هر نوع تقاضا و باز کردن فرم مربوط به آن */
         dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         var rqst = vF_Request_ChangingBs.Current as Data.VF_Request_ChangingResult;
         var RouterMethod = 0;
         var RouterGateway = "";

         if (rqst.RQTP_CODE == "001")
         {
            RouterMethod = 101;
            RouterGateway = "SHOW_ATRQ_F";
         }
         else if (rqst.RQTP_CODE == "002")
         {
            RouterMethod = 102;
            RouterGateway = "SHOW_ACRQ_F";
         }
         else if (rqst.RQTP_CODE == "011")
         {
            RouterMethod = 103;
            RouterGateway = "SHOW_CTRQ_F";
         }
         else if (rqst.RQTP_CODE == "013" || rqst.RQTP_CODE == "014")
         {
            RouterMethod = 104;
            RouterGateway = "SHOW_DERQ_F";
         }
         else if (rqst.RQTP_CODE == "009")
         {
            RouterMethod = 105;
            RouterGateway = "SHOW_UCRQ_F";
         }
         else if (rqst.RQTP_CODE == "016")
         {
            if (rqst.RQTT_CODE == "008")
            {
               RouterMethod = 106;
               RouterGateway = "SHOW_OMRQ_F";
            }
         }
         else if (rqst.RQTP_CODE == "019")
         {
            if (rqst.RQTT_CODE == "008")
            {
               RouterMethod = 107;
               RouterGateway = "SHOW_EMRQ_F";
            }
            else if(rqst.RQTT_CODE == "004")
            {
               RouterMethod = 112;
               RouterGateway = "SHOW_RFDT_F";
            }
         }
         else if (rqst.RQTP_CODE == "020")
         {
            RouterMethod = 118;
            RouterGateway = "SHOW_GLRL_F";
         }
         else
            return;


         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, RouterMethod /* Execute RouterMethod */),
                     new Job(SendType.SelfToUserInterface, RouterGateway, 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("rqid", rqst.RQID), 
                              new XElement("Request_Row", 
                                 new XAttribute("fighfileno", figh.FILE_NO)
                              )
                           )
                     }
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);

      }

      private void ActnAttn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var attn = AttnBs2.Current as Data.Attendance;

            switch (e.Button.Index)
            {
               /*case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", attn.FIGH_FILE_NO)) }
                  );
                  break;*/
               case 1:
                  if (attn.EXIT_TIME == null)
                  {
                     if (MessageBox.Show(this, "با خروج دستی مشتری موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, null, null, "003", attn.MBSP_RWNO_DNRM, "001", "002");
                     iScsc = new Data.iScscDataContext(ConnectionString);
                     AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
                  }
                  break;
               case 2:
                  if (attn.ATTN_STAT == "002")
                  {
                     if (MessageBox.Show(this, "با ابطال رکورد مشتری مشتری موافق هستید؟", "ابطال رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.UPD_ATTN_P(
                        new XElement("Process",
                           new XElement("Attendance",
                              new XAttribute("code", attn.CODE),
                              new XAttribute("type", "001") // ابطال رکورد مشتری
                           )
                        )
                     );

                     iScsc = new Data.iScscDataContext(ConnectionString);
                     AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
                  }
                  break;
               case 3:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", (AttnBs2.Current as Data.Attendance).FIGH_FILE_NO),
                                 new XAttribute("attndate", (AttnBs2.Current as Data.Attendance).ATTN_DATE),
                                 new XAttribute("mbsprwno", (AttnBs2.Current as Data.Attendance).MBSP_RWNO_DNRM)
                              )
                           }
                        })
                  );
                  break;
               case 4:
                  if (MessageBox.Show(this, "آیا با پاک کردن ساعا خروج موافق هستید؟", "حذف ساعت خروج", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                  attn.EXIT_TIME = null;
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Attendance SET Exit_Time = null WHERE Code = {0};", attn.CODE));
                  break;
               default:
                  break;
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_Mbsp_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         //if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;
         if (figh.TYPE == "002" || figh.TYPE == "003" || figh.TYPE == "004") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("formcaller", GetType().Name))}
               })
         );

         //switch(MessageBox.Show(this, "آیا میخواهید به صورت تک رشته ای تمدید کنید؟", "روش تمدید مجدد", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading))
         //{
         //   case DialogResult.Yes:               
         //      break;
         //   case DialogResult.No:
         //      _DefaultGateway.Gateway(
         //         new Job(SendType.External, "Localhost",
         //            new List<Job>
         //            {
         //               new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
         //               new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
         //            })
         //      );
         //      break;
         //   default:
         //      break;
         //}
         
      }

      private void Btn_Insr_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 80 /* Execute Ins_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewinscard"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("formcaller", GetType().Name))}
               })
         );
      }

      private void Btn_Pblc_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"), new XAttribute("formcaller", GetType().Name))}
               })
         );
      }

      private void Del_Butn_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         //if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

         //Job _InteractWithScsc =
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.Self, 127 /* Execute Adm_Mbco_F */),
         //         new Job(SendType.SelfToUserInterface, "ADM_MBCO_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("fileno", fileno))}
         //      });
         //_DefaultGateway.Gateway(_InteractWithScsc);

         if (MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "عملیات حذف موقت مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))},
                        new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_Calf_F */){Input = new XElement("Request", new XAttribute("type", "refresh"))},
                     })
         );
      }

      private void ClearFingerPrint_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //var figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            iScsc.SCV_PBLC_P(
               new XElement("Process",
                  new XElement("Fighter",
                     new XAttribute("fileno", fileno),
                     new XAttribute("columnname", "FNGR_PRNT"),
                     new XAttribute("newvalue", "")
                  )
               )
            );
            requery = true;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
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

      private void StopBlocking_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا می خواهید تاریخ بلوکه شدن را لغو کنید", "لغو بلوکه شدن", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            //var figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;

            iScsc.SCV_MBSP_P(
               new XElement("Process",
                  new XElement("Fighter",
                     new XAttribute("fileno", fileno)                     
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
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Butn_TakePicture_Click(object sender, EventArgs e)
      {
         try
         {
            if (true)
            {
               var rqst = (from r in iScsc.Requests
                           join rr in iScsc.Request_Rows on r.RQID equals rr.RQST_RQID
                           where rr.FIGH_FILE_NO == Convert.ToInt64(fileno)
                              && (r.RQTP_CODE == "001" || r.RQTP_CODE == "025")
                           select r).FirstOrDefault();
               if (rqst == null) return;

               var result = (
                        from r in iScsc.Regulations
                        join rqrq in iScsc.Request_Requesters on r equals rqrq.Regulation
                        join rqdc in iScsc.Request_Documents on rqrq equals rqdc.Request_Requester
                        join rcdc in iScsc.Receive_Documents on rqdc equals rcdc.Request_Document
                        where r.TYPE == "001"
                           && r.REGL_STAT == "002"
                           && rqrq.RQTP_CODE == rqst.RQTP_CODE
                           && rqrq.RQTT_CODE == rqst.RQTT_CODE
                           && rqdc.DCMT_DSID == 13930903120048833 // عکس 4*3
                           && rcdc.RQRO_RQST_RQID == rqst.RQID
                           && rcdc.RQRO_RWNO == 1
                        select rcdc).FirstOrDefault();
               if (result == null) return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self,  59 /* Execute Cmn_Dcmt_F */){ Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() },
                        new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Action",
                                 new XAttribute("type", "001"),
                                 new XAttribute("typedesc", "Force Active Camera Picture Profile"),
                                 new XElement("Document",
                                    new XAttribute("rcid", result.RCID)
                                 )
                              )
                        }
                     }
                  )
               );

            }
         }
         catch
         {

         }
      }

      private void Pony_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 117 /* Execute Debt_List_F */),
                  new Job(SendType.SelfToUserInterface, "DEBT_LIST_F" , 10 /* Execute Actn_CalF_F */){Input = new XElement("Debt", new XAttribute("type", "query"), new XAttribute("fileno", fileno))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void UpdateDebt_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = iScsc.Fighters.Where(f => f.FILE_NO == fileno).FirstOrDefault();

            iScsc.ExecuteCommand(string.Format("UPDATE Fighter SET Debt_Dnrm = 0 WHERE FILE_NO = {0}", figh.FILE_NO));
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
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

      private void vF_SavePaymentsBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            GustInfo_Pn.Visible = false;
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) { PydtsBs1.List.Clear(); PydsBs1.List.Clear(); PmmtBs1.List.Clear(); return; }

            PydtsBs1.DataSource = iScsc.Payment_Details.Where(pdt => pdt.PYMT_RQST_RQID == pymt.RQID && pdt.PYMT_CASH_CODE == pymt.CASH_CODE);
            PydsBs1.DataSource = iScsc.Payment_Discounts.Where(pds => pds.PYMT_RQST_RQID == pymt.RQID && pds.PYMT_CASH_CODE == pymt.CASH_CODE);
            PmmtBs1.DataSource = iScsc.Payment_Methods.Where(pm => pm.PYMT_RQST_RQID == pymt.RQID && pm.PYMT_CASH_CODE == pymt.CASH_CODE);
            PycoBs1.DataSource = iScsc.Payment_Costs.Where(pc => pc.PYMT_RQST_RQID == pymt.RQID && pc.PYMT_CASH_CODE == pymt.CASH_CODE);

            dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null)
               figh = vF_Last_Info_FighterBs.Current is Data.VF_Last_Info_Deleted_FighterResult;

            if(figh.TYPE == "005")
            {
               GustInfo_Pn.Visible = true;
               FgpbBs.DataSource = iScsc.Fighter_Publics.Where(fp => fp.RQRO_RQST_RQID == pymt.RQID);
            }
            else
            {
               GustInfo_Pn.Visible = false;
            }

            // 1398/10/02 * بررسی اینکه چه صورتحساب هایی قادر به عملیات ابطال هستند
            CnclPymt_Tsmi.Enabled = EditPymt_Tsmi.Enabled = false;
            if(pymt.RQTP_CODE.In("001", "009"))
            {
               CnclPymt_Tsmi.Enabled = EditPymt_Tsmi.Enabled = true;
            }
            else if(pymt.RQTP_CODE.In("012", "016"))
            {
               CnclPymt_Tsmi.Enabled = true;
            }            
         }
         catch {}
      }

      private void CnclRqst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انصراف درخواست مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Rqst = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;

            if (Rqst != null && Rqst.RQST_RQID > 0)
            {
               /*
                *  Remove Data From Tables
                */
               iScsc.CNCL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQST_RQID)
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
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void PymtSave_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            bool checkOK = true;
            Job _InteractWithScsc =
               new Job(SendType.External, "Desktop",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>221</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 checkOK = false;
                                 MessageBox.Show(this, "عدم دسترسی به ردیف 221 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                              })
                           }
                           #endregion                        
                        })                     
                     });
            _DefaultGateway.Gateway(_InteractWithScsc);
            if(checkOK)
            {
               if (MessageBox.Show(this, "آیا با اعمال تغییرات موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
               PydtsBs1.EndEdit();
               PydsBs1.EndEdit();
               PmmtBs1.EndEdit();

               iScsc.SubmitChanges();
               requery = true;
            }
            
         }
         catch 
         {
            requery = false;
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void CmpsRcpt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            bool checkOK = true;
            Job _InteractWithScsc =
               new Job(SendType.External, "Desktop",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>221</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 checkOK = false;
                                 MessageBox.Show(this, "عدم دسترسی به ردیف 221 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                              })
                           }
                           #endregion                        
                        })                     
                     });
            _DefaultGateway.Gateway(_InteractWithScsc);
            if (checkOK)
            {               
               var savepymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
               if (savepymt == null) return;
               var pymt = iScsc.Payments.Where(p => p.RQST_RQID == savepymt.RQID).FirstOrDefault();

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 86 /* Execute Pay_Mtod_F */){Input = pymt},
                        new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 10 /* Execute Actn_CalF_F*/)
                        {
                           Input = 
                              new XElement("Payment_Method",
                                 new XAttribute("callerform", GetType().Name),
                                 new XAttribute("tabfocued", "tp_003")
                              )
                        }
                     }
                  )
               );
            }
         }
         catch
         {
            requery = false;
         }
      }

      private void Tsb_ShowRqtp16_Click(object sender, EventArgs e)
      {
         try
         {
            SavePayment_Gv.ActiveFilterString = string.Format("[RQTP_DESC] = 'درآمد متفرقه'");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Tsb_ClearRqtp_Click(object sender, EventArgs e)
      {
         try
         {
            SavePayment_Gv.ActiveFilterString = "";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Pymt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  bool checkOK = true;
                  #region Check Security
                  Job _InteractWithScsc =
                     new Job(SendType.External, "Desktop",
                        new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 #region Access Privilege
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>222</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       checkOK = false;
                                       MessageBox.Show(this, "عدم دسترسی به ردیف 222 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                    })
                                 }
                                 #endregion                        
                              })                     
                           });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  #endregion
                  if (checkOK)
                  {
                     if (MessageBox.Show(this, "آیا با حذف کامل صورتحساب درخواست موافقید؟", "حذف صورتحساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                     if (iScsc.Payment_Methods.Any(pm => pm.PYMT_CASH_CODE == pymt.CASH_CODE && pm.PYMT_RQST_RQID == pymt.RQID) && MessageBox.Show(this, "صورتحساب دارای مبلغ پرداختی می باشد با حذف صورتحساب کلیه پرداختی های آن نیز حذف و هیچ گونه قابل برگشت نیست، آیا عملیات انجام پذیرد؟", "حذف پرداختی های صورتحساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                     iScsc.DEL_PYMT_P(
                        new XElement("Payment",
                           new XAttribute("rqid", pymt.RQID),
                           new XAttribute("cashcode", pymt.CASH_CODE)
                        )
                     );
                     requery = true;
                  }
                  break;
               case 1:
                  if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "CheckoutWithoutPOS"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQID),
                              new XAttribute("paystat", "002"),
                              new XAttribute("fileno", fileno)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception )
         {}
         finally
         {
            if(requery)
            {
               UpdateDebt_Btn_Click(null, null);
               //Execute_Query();
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void Pydt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  bool checkOK = true;
                  #region Check Security
                  Job _InteractWithScsc =
                     new Job(SendType.External, "Desktop",
                        new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 #region Access Privilege
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>223</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       checkOK = false;
                                       MessageBox.Show(this, "عدم دسترسی به ردیف 223 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                    })
                                 }
                                 #endregion                        
                              })                     
                           });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  #endregion
                  if (checkOK)
                  {
                     if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     /* Do Delete Payment_Detail */
                     iScsc.DEL_SEXP_P(
                        new XElement("Request",
                           new XAttribute("rqid", pymt.RQID),
                           new XElement("Payment",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XElement("Payment_Detail",
                                 new XAttribute("code", pydt.CODE)
                              )
                           )
                        )
                     );
                     requery = true;
                  }
                  break;
               case 1:
                  checkOK = true;
                  #region Check Security
                  _InteractWithScsc =
                     new Job(SendType.External, "Desktop",
                        new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 #region Access Privilege
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>224</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       checkOK = false;
                                       MessageBox.Show(this, "عدم دسترسی به ردیف 224 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                    })
                                 }
                                 #endregion                        
                              })                     
                           });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  #endregion
                  if (checkOK)
                  {
                     if (MessageBox.Show(this, "آیا با ویرایش کردن هزینه درخواست موافقید؟", "ویرایش هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.UPD_SEXP_P(
                        new XElement("Request",
                           new XAttribute("rqid", pymt.RQID),
                           new XElement("Payment",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XElement("Payment_Detail",
                                 new XAttribute("code", pydt.CODE),
                                 new XAttribute("expncode", pydt.EXPN_CODE),
                                 new XAttribute("expnpric", pydt.EXPN_PRIC),
                                 new XAttribute("pydtdesc", pydt.PYDT_DESC ?? ""),
                                 new XAttribute("qnty", pydt.QNTY ?? 1),
                                 new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                                 new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),
                                 new XAttribute("mtodcodednrm", pydt.MTOD_CODE_DNRM),
                                 new XAttribute("ctgycodednrm", pydt.CTGY_CODE_DNRM)
                              )
                           )
                        )
                     );
                     requery = true;
                  }
                  break;
               case 2:
                  checkOK = true;
                  #region Check Security
                  _InteractWithScsc =
                     new Job(SendType.External, "Desktop",
                        new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 #region Access Privilege
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>226</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       checkOK = false;
                                       MessageBox.Show(this, "عدم دسترسی به ردیف 226 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                    })
                                 }
                                 #endregion                        
                              })                     
                           });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  #endregion
                  if(checkOK)
                  {
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 150 /* Execute Tran_Expn_F */),
                              new Job(SendType.SelfToUserInterface, "TRAN_EXPN_F", 10 /* execute Actn_CalF_F */)
                              {
                                 Input = 
                                    new XElement("Payment",
                                       new XAttribute("pydtcode", pydt.CODE),
                                       new XAttribute("fileno", fileno)
                                    )
                              }
                           }
                        )
                     );
                  }
                  break;
               default:
                  break;
            }
         }
         catch (Exception )
         {}
         finally
         {
            if(requery)
            {
               UpdateDebt_Btn_Click(null, null);
               //Execute_Query();
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void ShowCrntReglYear_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var regl = iScsc.Regulations.FirstOrDefault(rg => rg.TYPE == "001" && rg.REGL_STAT == "002");
            SavePayment_Gv.ActiveFilterString = string.Format("REGL_YEAR_DNRM = {0}", regl.YEAR);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowOthrReglYear_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var regl = iScsc.Regulations.FirstOrDefault(rg => rg.TYPE == "001" && rg.REGL_STAT == "002");
            SavePayment_Gv.ActiveFilterString = string.Format("REGL_YEAR_DNRM != {0}", regl.YEAR);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Btn_Blok_Click(object sender, EventArgs e)
      {
         dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 133 /* Execute Adm_Mbfz_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBFZ_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "block"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("formcaller", GetType().Name))}
               })
         );
      }

      private void TrnsFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         Data.VF_Last_Info_FighterResult figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (figh == null)
            return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
            {
               Input =
               new XElement("User",
                  new XAttribute("enrollnumb", figh.FNGR_PRNT_DNRM),
                  new XAttribute("cardnumb", figh.FNGR_PRNT_DNRM),
                  new XAttribute("namednrm", figh.FNGR_PRNT_DNRM)
               )
            }
         );
      }

      private void Refresh_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null)
               figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
            );
         }
         catch { }
      }

      private void AttnActn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null)
               figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                     new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("mbsprwno", mbsp.RWNO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            Refresh_Butn_Click(null, null);
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CbmtInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (figh == null)
               figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 149 /* Execute Bas_Wkdy_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_WKDY_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("code", figh.CBMT_CODE), new XAttribute("showonly", "002"))}
                  }
               )
            );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         } 
      }

      private void CbmtHistInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_All_Info_FightersBs.Current as Data.VF_All_Info_FightersResult;
            if (figh == null) return;            

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 149 /* Execute Bas_Wkdy_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_WKDY_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("code", figh.CBMT_CODE), new XAttribute("showonly", "002"))}
                  }
               )
            );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         } 
      }

      private void Mbsp_Rwno_Text_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>231</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 231 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),
                     #region DoWork
                        new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
                        new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", fileno),
                                 new XAttribute("mbsprwno", mbsp.RWNO),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost",
            //      new List<Job>
            //      {
            //         new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
            //         new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
            //         {
            //            Input = 
            //               new XElement("Fighter",
            //                  new XAttribute("fileno", fileno),
            //                  new XAttribute("mbsprwno", mbsp.RWNO),
            //                  new XAttribute("formcaller", GetType().Name)
            //               )
            //         }
            //      }
            //   )
            //);
         }
         catch { }
      }

      private void MbspBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;
            Attn_gv.ActiveFilterString = string.Format("MBSP_RWNO_DNRM = '{0}' AND ATTN_STAT = '002'", mbsp.RWNO);

            long? rqid = 0;
            if(mbsp.RWNO == 1)            
               rqid = mbsp.Request_Row.Request.Request1.RQID;
            else
               rqid = mbsp.RQRO_RQST_RQID;

            MbspValdType_Butn.Text = mbsp.VALD_TYPE == "001" ? "فعال کردن" : "غیرفعال کردن";

            ExpnAmnt_Txt.EditValue = iScsc.Payment_Details.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => (pd.EXPN_PRIC + pd.EXPN_EXTR_PRCT) * pd.QNTY);
            DscnAmnt_Txt.EditValue = iScsc.Payment_Discounts.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => pd.AMNT);
            PymtAmnt1_Txt.EditValue = iScsc.Payment_Methods.Where(pd => pd.PYMT_RQST_RQID == rqid).Sum(pd => pd.AMNT);
            DebtPymtAmnt1_Txt.EditValue = Convert.ToInt64(ExpnAmnt_Txt.EditValue) - (Convert.ToInt64(PymtAmnt1_Txt.EditValue) + Convert.ToInt64(DscnAmnt_Txt.EditValue));
            QStrtTime_Tim.EditValue = mbsp.Fighter_Public.Club_Method.STRT_TIME;
            QEndTime_Tim.EditValue = mbsp.Fighter_Public.Club_Method.END_TIME;
            MbspFngrPrnt_Txt.EditValue = mbsp.Fighter_Public.FNGR_PRNT;

            PdtMBs.DataSource = iScsc.Payment_Details.Where(pd => pd.Payment.Request.RQST_STAT == "002" && pd.MBSP_FIGH_FILE_NO == fileno && pd.MBSP_RWNO == mbsp.RWNO);
         }
         catch{
         }
      }

      private void MbspInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            int RouterMethod = 105;
            string RouterGateway = "SHOW_UCRQ_F";

            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, RouterMethod /* Execute RouterMethod */),
                     new Job(SendType.SelfToUserInterface, RouterGateway, 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("rqid", mbsp.RQRO_RQST_RQID), 
                              new XElement("Request_Row", 
                                 new XAttribute("fighfileno", mbsp.FIGH_FILE_NO)
                              )
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }catch{}
      }

      private void MbspValdType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            if(mbsp.TYPE == "005")
            {
               MessageBox.Show(this, "شما اجازه غیرفعال کردن رکورد بلوکه کردن را ندارید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }           

            if(mbsp.VALD_TYPE == "002")
            {
               if (MessageBox.Show(this, "آیا با غیرفعال کردن دوره موافق هستید؟", "غیرفعال کردن دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

               iScsc.ExecuteCommand(string.Format("UPDATE Member_Ship SET Vald_Type = '001' WHERE Rqro_Rqst_Rqid = {0};", mbsp.RQRO_RQST_RQID));
            }
            else if(mbsp.VALD_TYPE == "001")
            {
               if(MbspBs.List.OfType<Data.Member_Ship>().Any(m => m.RWNO > mbsp.RWNO && m.TYPE == "005"))
               {
                  MessageBox.Show(this, "شما اجازه فعال کردن دوره ابطال شده توسط فرآیند بلوکه کردن را ندارید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }

               if (MessageBox.Show(this, "آیا با فعال کردن دوره موافق هستید؟", "فعال کردن دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

               iScsc.ExecuteCommand(string.Format("UPDATE Member_Ship SET Vald_Type = '002' WHERE Rqro_Rqst_Rqid = {0};", mbsp.RQRO_RQST_RQID));
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

      private void tb_master_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            switch (tb_master.SelectedIndex)
            {
               case 1:
                  vF_All_Info_FightersBs.DataSource = iScsc.VF_All_Info_Fighters(fileno).OrderByDescending(f => f.RWNO);
                  break;
               case 2:
                  vF_SavePaymentsBs.DataSource = iScsc.VF_Save_Payments(null, fileno).OrderByDescending(p => p.PYMT_CRET_DATE);
                  ShowCrntReglYear_Butn_Click(null, null);
                  break;
               case 3:
                  GlrlBs.DataSource = iScsc.Gain_Loss_Rials.Where(g => g.FIGH_FILE_NO == fileno && g.CONF_STAT == "002" && g.AMNT > 0);
                  GPymBs.DataSource = iScsc.Payment_Methods.Where(p => p.FIGH_FILE_NO_DNRM == fileno && p.RCPT_MTOD == "005");
                  break;
               case 4:
                  vF_Request_DocumentBs.DataSource = iScsc.VF_Request_Document(fileno); ;
                  break;
               case 5:
                  vF_Request_ChangingBs.DataSource = iScsc.VF_Request_Changing(fileno).OrderBy(r => r.RQST_DATE);
                  break;
               case 6:
                  AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
                  break;
               default:
                  break;
            }
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PrintSetting_Butn_Click(object sender, EventArgs e)
      {
         //Back_Butn_Click(null, null);
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Print_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //Back_Butn_Click(null, null);
            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("File_No = '{0}'", fileno))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintDefault_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            //Back_Butn_Click(null, null);
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("File_No = '{0}'", fileno))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void GlrIndc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 153 /* Execute Glr_Indc_F */),
                     new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("type", "newrequest"), 
                              new XAttribute("fileno", fileno),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OthrIncome_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                     new List<Job>
                  {                  
                     new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                     new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", fileno)))}
                  })
            );
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PosStng_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 33 /* Execute PosSettings */, SendType.Self) { Input = "Pos_Butn" }
         );
      }

      private void PydsType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydsType_Butn.Text = PydsType_Butn.Tag.ToString() == "0" ? "مبلغی" : "درصدی";
            PydsType_Butn.Tag = PydsType_Butn.Tag.ToString() == "0" ? "1" : "0";
            if (PydsType_Lov.EditValue != null || PydsType_Lov.EditValue.ToString() != "") PydsType_Lov.EditValue = "002";

            if (PydsType_Butn.Tag.ToString() == "0")
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "درصد تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 3;
            }
            else
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "مبلغ تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 0;
            }
            PydsAmnt_Txt.Focus();
         }
         catch { }
      }

      private void RcmtType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RcmtType_Butn.Text = RcmtType_Butn.Tag.ToString() == "0" ? "POS" : "نقدی";
            RcmtType_Butn.Tag = RcmtType_Butn.Tag.ToString() == "0" ? "1" : "0";
            PymtAmnt_Txt.Focus();
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;
            PymtAmnt_Txt.EditValue = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
         }
         catch { }
      }

      private void SavePyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;

            long? amnt = null;
            switch (PydsType_Butn.Tag.ToString())
            {
               case "0":
                  if (!(Convert.ToInt64(PydsAmnt_Txt.EditValue) >= 0 && Convert.ToInt64(PydsAmnt_Txt.EditValue) <= 100))
                  {
                     PydsAmnt_Txt.EditValue = null;
                     PydsAmnt_Txt.Focus();
                  }

                  amnt = (pymt.SUM_EXPN_PRIC * Convert.ToInt64(PydsAmnt_Txt.EditValue)) / 100;
                  break;
               case "1":
                  amnt = Convert.ToInt64(PydsAmnt_Txt.EditValue);
                  if (amnt == 0) return;
                  break;
            }

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQID, (short?)1, null, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text);

            PydsAmnt_Txt.EditValue = null;
            PydsDesc_Txt.EditValue = null;
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
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void DeltPyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pyds = PydsBs1.Current as Data.Payment_Discount;
            if (pyds == null) return;

            iScsc.DEL_PYDS_P(pyds.PYMT_CASH_CODE, pyds.PYMT_RQST_RQID, pyds.RQRO_RWNO, pyds.RWNO);

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
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void SavePymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PymtDate_DateTime001.CommitChanges();
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;

            if (PymtAmnt_Txt.EditValue == null || PymtAmnt_Txt.EditValue.ToString() == "" || Convert.ToInt64(PymtAmnt_Txt.EditValue) == 0) return;

            switch (RcmtType_Butn.Tag.ToString())
            {
               case "0":
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQID),
                              new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                              new XAttribute("rcptmtod", "001"),
                              new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
                           )
                        )
                     )
                  );
                  break;
               case "1":
                  if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

                  if (UsePos_Cb.Checked)
                  {
                     var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

                     long psid;
                     if (Pos_Lov.EditValue == null)
                     {
                        var posdflts = VPosBs1.List.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
                        if (posdflts.Count() == 1)
                           Pos_Lov.EditValue = psid = posdflts.FirstOrDefault().PSID;
                        else
                        {
                           Pos_Lov.Focus();
                           return;
                        }
                     }
                     else
                     {
                        psid = (long)Pos_Lov.EditValue;
                     }

                     if (regl.AMNT_TYPE == "002")
                        PymtAmnt_Txt.EditValue = Convert.ToInt64( PymtAmnt_Txt.EditValue) * 10;

                     // از این گزینه برای این استفاده میکنیم که بعد از پرداخت نباید درخواست ثبت نام پایانی شود
                     UsePos_Cb.Checked = false;

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.External, "Commons",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 34 /* Execute PosPayment */)
                                    {
                                       Input = 
                                          new XElement("PosRequest",
                                             new XAttribute("psid", psid),
                                             new XAttribute("subsys", 5),
                                             new XAttribute("rqid", pymt.RQID),
                                             new XAttribute("rqtpcode", ""),
                                             new XAttribute("router", GetType().Name),
                                             new XAttribute("callback", 20),
                                             new XAttribute("amnt", Convert.ToInt64( PymtAmnt_Txt.EditValue) )
                                          )
                                    }
                                 }
                              )
                           }
                        )
                     );

                     UsePos_Cb.Checked = true;
                  }
                  else
                  {
                     iScsc.PAY_MSAV_P(
                        new XElement("Payment",
                           new XAttribute("actntype", "InsertUpdate"),
                           new XElement("Insert",
                              new XElement("Payment_Method",
                                 new XAttribute("cashcode", pymt.CASH_CODE),
                                 new XAttribute("rqstrqid", pymt.RQID),
                                 new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                                 new XAttribute("rcptmtod", "003"),
                                 new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
                              )
                           )
                        )
                     );
                  }
                  break;
               default:
                  break;
            }

            PymtAmnt_Txt.EditValue = null;
            PymtDate_DateTime001.Value = DateTime.Now;
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
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void DeltPymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pmmt = PmmtBs1.Current as Data.Payment_Method;
            if (pmmt == null) return;

            iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "Delete"),
                  new XAttribute("cashcode", pmmt.PYMT_CASH_CODE),
                  new XAttribute("rqstrqid", pmmt.PYMT_RQST_RQID),
                  new XAttribute("rwno", pmmt.RWNO)
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
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrNewEnrollPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف اثر انگشت از مشتری و اختصاص برای کاربر جدید موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var fngrprnt = FNGR_PRNT_TextEdit.Text;

            // اثر انگشت را از دستگاه پاک میکنیم
            RqstBnDeleteFngrPrnt1_Click(null, null);

            // ابتدا کد انگشتی را از مشتری میگیریم
            ClearFingerPrint_Butn_Click(null, null);

            // باز کردن فرم ثبت نام برای مشتری جدید
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", fngrprnt))}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch (Exception exc) { }
      }
      #endregion

      private void PymtBnDebt_Click(object sender, EventArgs e)
      {
         tb_master.SelectedTab = tp_003;
         SavePayment_Gv.ActiveFilterString = "colTOTL_DEBT_PYMT > 0";
      }

      private void SmsHist_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sms = sender as SimpleButton;

            string phon = "";
            switch (sms.Tag.ToString())
            {
               case "Cell_Phon":
                  phon = CellPhon01_Txt.Text;
                  break;
               case "Dad_Phon":
                  phon = DadPhon01_Txt.Text;
                  break;
               case "Mom_Phon":
                  phon = MomPhon01_Txt.Text;
                  break;
               case "Cell_Phon01":
                  phon = CellPhon02_Txt.Text;
                  break;
               case "Dad_Phon01":
                  phon = DadPhon02_Txt.Text;
                  break;
               case "Mom_Phon01":
                  phon = MomPhon02_Txt.Text;
                  break;
            }

            if (phon == null || phon == "")
               return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "DefaultGateway:Msgb", 07 /* Execute Send_Mesg_F */, SendType.Self)
            );

            if (ModifierKeys.HasFlag(Keys.Control))
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "DefaultGateway:Msgb:SEND_MESG_F", 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Message", new XAttribute("tab", "tp_001"), new XAttribute("subsys", "5"), new XAttribute("cellphon", phon)) }
               );
            }
            else
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "DefaultGateway:Msgb:SEND_MESG_F", 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Message", new XAttribute("tab", "tp_004"), new XAttribute("filtering", "phonnumb"), new XAttribute("valu", phon)) }
               );
            }
         }
         catch (Exception exc)
         {

            throw;
         }
      }

      private void TlgrmSndMesg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mesg = sender as SimpleButton;

            var chatid = "";
            switch (mesg.Tag.ToString())
            {
               case "Chatid":
                  chatid = Chatid_Txt.Text;
                  break;
               case "Chatid01":
                  chatid = Chatid01_Txt.Text;
                  break;
               default:
                  break;
            }

            if (chatid == null || chatid == "") return;

            Job _InteractWithScsc =
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>1</Privilege><Sub_Sys>12</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 1 سطوح امینتی", "عدم دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                        new Job(SendType.External, "Program", "RoboTech", 02 /* Execute Frst_Page_F */,SendType.Self)
                     }
                  ),
                  #region DoWork
                  new Job(SendType.External, "DefaultGateway", "RoboTech", 09 /* Execute Rbsa_Dvlp_F */,SendType.Self),
                  new Job(SendType.External, "DefaultGateway", "RoboTech:RBSA_DVLP_F", 10 /* Execute Actn_Calf_P */,SendType.SelfToUserInterface){Input = new XElement("Message", new XAttribute("filtering", "chatid"), new XAttribute("valu", chatid))}
                  #endregion
            });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception)
         {
            
            throw;
         }
      }

      private void ExcpAttnActv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '001' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '002', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '002';", fileno
               )
            );
            MessageBox.Show("عملیات استثناء ورود با موفقیت فعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

      private void ExcpAttnDact_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '001' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '001', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '001';", fileno
               )
            );
            MessageBox.Show("عملیات استثناء ورود با موفقیت غیرفعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PayCashDebt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno);
            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;
            
            var paydebt = Convert.ToInt64(PayDebtAmnt_Txt.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;


            foreach (var pymt in vF_SavePaymentsBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date))
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;
               
               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "001"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

            Refresh_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PayPosDebt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var figh = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno);
            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt_Txt.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

            if (UsePos_Cb.Checked)
            {
               var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

               long psid;
               if (Pos_Lov.EditValue == null)
               {
                  var posdflts = VPosBs1.List.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
                  if (posdflts.Count() == 1)
                     Pos_Lov.EditValue = psid = posdflts.FirstOrDefault().PSID;
                  else
                  {
                     Pos_Lov.Focus();
                     return;
                  }
               }
               else
               {
                  psid = (long)Pos_Lov.EditValue;
               }

               if (regl.AMNT_TYPE == "002")
                  paydebt = paydebt * 10;

               // از این گزینه برای این استفاده میکنیم که بعد از پرداخت نباید درخواست ثبت نام پایانی شود
               UsePos_Cb.Checked = false;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.External, "Commons",
                           new List<Job>
                           {
                              new Job(SendType.Self, 34 /* Execute PosPayment */)
                              {
                                 Input = 
                                    new XElement("PosRequest",
                                       new XAttribute("psid", psid),
                                       new XAttribute("subsys", 5),
                                       new XAttribute("rqid", 0),
                                       new XAttribute("rqtpcode", ""),
                                       new XAttribute("router", GetType().Name),
                                       new XAttribute("callback", 21),
                                       new XAttribute("amnt", paydebt )
                                    )
                              }
                           }
                        )
                     }
                  )
               );

               UsePos_Cb.Checked = true;
            }
            else
            {
               foreach (var pymt in vF_SavePaymentsBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date))
               {
                  var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
                  long amnt = 0;

                  if (debt > paydebt)
                     // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                     amnt = paydebt;
                  else
                     // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                     amnt = debt;

                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQID),
                              new XAttribute("amnt", amnt),
                              new XAttribute("rcptmtod", "003"),
                              new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                           )
                        )
                     )
                  );

                  paydebt -= amnt;
                  if (paydebt == 0) break;
               }

               Refresh_Butn_Click(null, null);
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NewMbsp_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            
            var fp = mbsp.Fighter_Public;
            iScsc.ExecuteCommand(string.Format("UPDATE Fighter SET Mtod_Code_Dnrm = {0}, Ctgy_Code_Dnrm = {1}, Cbmt_Code_Dnrm = {2} WHERE File_No = {3};", fp.MTOD_CODE, fp.CTGY_CODE, fp.CBMT_CODE, fp.FIGH_FILE_NO));

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", fp.FNGR_PRNT), new XAttribute("formcaller", GetType().Name))}
                  })
            );
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void RecalcPymt_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            iScsc.CALC_SEXP_P(
               new XElement("Request",
                  new XAttribute("rqid", mbsp.RQRO_RQST_RQID)
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

      private void CnclPymt_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            // نتایج ابطال دوره و صورتحساب
            var resultStr = "نتیجه ابطال";            

            // آیا دوره مشتری فعال می باشد یا خیر * اگر دوره تمام شده باشد و بخواهیم ابطال کنیم نیاز به دسترسی مدیر می باشد
            if(mbsp.VALD_TYPE == "001")
            {
               throw new Exception("دوره غیرفعال قابلیت ابطال ندارد");
            }

            if(!mbsp.Fighter_Public.Method.Category_Belts.Any(mc => mc.NUMB_OF_ATTN_MONT == 1))
            {
               throw new Exception("لطفا برای گروه نرخ تک جلسه ای تعریف کنید تا برای فرآیند ابطال بتوان استفاده کرد");
            }

            if(!(DateTime.Now.Date.IsBetween((DateTime)mbsp.STRT_DATE, (DateTime)mbsp.END_DATE) && (mbsp.NUMB_OF_ATTN_MONT == 0 || mbsp.NUMB_OF_ATTN_MONT > mbsp.SUM_ATTN_MONT_DNRM)))
            {
               resultStr += Environment.NewLine + "* نیاز به [ مجوز 247 ] بابت ابطال دوره گذشته یا بایگانی شده";
            }

            // مرحله بعدی اینکه آیا مشتری از خدمات دوره جلسه ای استفاده کرده یا خیر * اگر که دوره صورتحساب پرداختی داشته باشد ابتدا باید به تعداد جلسات استفاده شده درآمد متفرقه تک جلسه ای از آن دوره ثبت شود
            // فقط نکته ای که وجود دارد این هست اگر ما در این قسمت مشخص کرده ایم که از 12 جلسه ای که مشتری دارد 4 جلسه کسر شده باید در  جدول حضورو غیاب هم به همان تعداد 
            // رکورد حضورو غیاب داشته باشیم وگرنه باید درخواستی به تعداد جلسات مصرف شده در تاریخ اعمال ابطال ثبت کنیم
            // اگر دوره صورتحساب هم داشته باشد مبلغ هر جلسه را از دوره کسر میکنیم
            if (DateTime.Now.Date.IsBetween((DateTime)mbsp.STRT_DATE, (DateTime)mbsp.END_DATE) && (mbsp.SUM_ATTN_MONT_DNRM > 0))
            {
               resultStr += Environment.NewLine + string.Format("* {0} {1} {2}", "ثبت تعداد", mbsp.SUM_ATTN_MONT_DNRM, "صورتحساب تکجلسه ای [ آزاد ] برای مشتری");
            }

            // اینکه دوره صورتحساب دارد یا خیر و اگر داشته باشد مبلغ باقیمانده از صورتحساب را به صورت اعتبار ذخیره میکنیم
            if(mbsp.RWNO == 1)
            {
               if(!mbsp.Request_Row.Request.Request1.Payments.Any())
               {
                  // اگر درخواست صورتحساب نداشته باشد
                  resultStr += Environment.NewLine + "* دوره صورتحساب ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if (!mbsp.Request_Row.Request.Request1.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی نداشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب دوره پرداختی ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if(mbsp.Request_Row.Request.Request1.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی داشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب [ دوره ] پرداختی دارد و با کسر مبلغ [ جلسات استفاده شده ] و [ تسویه صورتحساب های بدهکار قبلی ] ما بقی مبلغ به صورت سپرده برای مشتری در نظر گرفته میشود";
               }
            }
            else
            {
               if (!mbsp.Request_Row.Request.Payments.Any())
               {
                  // اگر درخواست صورتحساب نداشته باشد
                  resultStr += Environment.NewLine + "* دوره صورتحساب ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if (!mbsp.Request_Row.Request.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی نداشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب دوره پرداختی ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if (mbsp.Request_Row.Request.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی داشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب [ دوره ] پرداختی دارد و با کسر مبلغ [ جلسات استفاده شده ] و [ تسویه صورتحساب های بدهکار قبلی ] ما بقی مبلغ به صورت سپرده برای مشتری در نظر گرفته میشود";
               }
            }

            if (MessageBox.Show(this, resultStr, "فرآیند ابطال", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.CNCL_PYMT_P(
               new XElement("Payment", 
                  new XAttribute("rqid", mbsp.RQRO_RQST_RQID),
                  new XAttribute("cncltype", "001") // ابطال عادی صورتحساب                  
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

      private void EditPymt_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            // نتایج ابطال دوره و صورتحساب
            var resultStr = "نتیجه ابطال";

            // آیا دوره مشتری فعال می باشد یا خیر * اگر دوره تمام شده باشد و بخواهیم ابطال کنیم نیاز به دسترسی مدیر می باشد
            if (mbsp.VALD_TYPE == "001")
            {
               throw new Exception("دوره غیرفعال قابلیت ابطال ندارد");
            }

            if (!mbsp.Fighter_Public.Method.Category_Belts.Any(mc => mc.NUMB_OF_ATTN_MONT == 1))
            {
               throw new Exception("لطفا برای گروه نرخ تک جلسه ای تعریف کنید تا برای فرآیند ابطال بتوان استفاده کرد");
            }

            if (!(DateTime.Now.Date.IsBetween((DateTime)mbsp.STRT_DATE, (DateTime)mbsp.END_DATE) && (mbsp.NUMB_OF_ATTN_MONT == 0 || mbsp.NUMB_OF_ATTN_MONT > mbsp.SUM_ATTN_MONT_DNRM)))
            {
               resultStr += Environment.NewLine + "* نیاز به [ مجوز 247 ] بابت ابطال دوره گذشته یا بایگانی شده";
            }

            // مرحله بعدی اینکه آیا مشتری از خدمات دوره جلسه ای استفاده کرده یا خیر * اگر که دوره صورتحساب پرداختی داشته باشد ابتدا باید به تعداد جلسات استفاده شده درآمد متفرقه تک جلسه ای از آن دوره ثبت شود
            // فقط نکته ای که وجود دارد این هست اگر ما در این قسمت مشخص کرده ایم که از 12 جلسه ای که مشتری دارد 4 جلسه کسر شده باید در  جدول حضورو غیاب هم به همان تعداد 
            // رکورد حضورو غیاب داشته باشیم وگرنه باید درخواستی به تعداد جلسات مصرف شده در تاریخ اعمال ابطال ثبت کنیم
            // اگر دوره صورتحساب هم داشته باشد مبلغ هر جلسه را از دوره کسر میکنیم
            if (DateTime.Now.Date.IsBetween((DateTime)mbsp.STRT_DATE, (DateTime)mbsp.END_DATE) && (mbsp.SUM_ATTN_MONT_DNRM > 0))
            {
               resultStr += Environment.NewLine + string.Format("* {0} {1} {2}", "ثبت تعداد", mbsp.SUM_ATTN_MONT_DNRM, "صورتحساب تکجلسه ای [ آزاد ] برای مشتری");
            }

            // اینکه دوره صورتحساب دارد یا خیر و اگر داشته باشد مبلغ باقیمانده از صورتحساب را به صورت اعتبار ذخیره میکنیم
            if (mbsp.RWNO == 1)
            {
               if (!mbsp.Request_Row.Request.Request1.Payments.Any())
               {
                  // اگر درخواست صورتحساب نداشته باشد
                  resultStr += Environment.NewLine + "* دوره صورتحساب ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if (!mbsp.Request_Row.Request.Request1.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی نداشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب دوره پرداختی ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if (mbsp.Request_Row.Request.Request1.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی داشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب [ دوره ] پرداختی دارد و با کسر مبلغ [ جلسات استفاده شده ] و [ تسویه صورتحساب های بدهکار قبلی ] ما بقی مبلغ به صورت سپرده برای مشتری در نظر گرفته میشود";
               }
            }
            else
            {
               if (!mbsp.Request_Row.Request.Payments.Any())
               {
                  // اگر درخواست صورتحساب نداشته باشد
                  resultStr += Environment.NewLine + "* دوره صورتحساب ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if (!mbsp.Request_Row.Request.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی نداشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب دوره پرداختی ندارد و به مشتری مبلغی استرداد نمی شود";
               }
               else if (mbsp.Request_Row.Request.Payments.Any(p => p.SUM_RCPT_EXPN_PRIC > 0))
               {
                  // اگر درخواست صورتحساب داشته باشد و پرداختی داشته باشد
                  resultStr += Environment.NewLine + "* صورتحساب [ دوره ] پرداختی دارد و با کسر مبلغ [ جلسات استفاده شده ] و [ تسویه صورتحساب های بدهکار قبلی ] ما بقی مبلغ به صورت سپرده برای مشتری در نظر گرفته میشود";
               }
            }

            if (MessageBox.Show(this, resultStr, "فرآیند صدور صورتحساب اصلاحی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.CNCL_PYMT_P(
               new XElement("Payment",
                  new XAttribute("rqid", mbsp.RQRO_RQST_RQID),
                  new XAttribute("cncltype", "002") // صدور صورتحساب اصلاحی
               )
            );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "search"), new XAttribute("enrollnumber", mbsp.Fighter.FNGR_PRNT_DNRM), new XAttribute("formcaller", GetType().Name))}
                  })
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

      private void CnclPymt_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;

            if(pymt.RQTP_CODE.In("001", "009", "012", "016"))
            {
               iScsc.CNCL_PYMT_P(
                  new XElement("Payment",
                     new XAttribute("rqid", pymt.RQID),
                     new XAttribute("cncltype", "001") // ابطال عادی صورتحساب
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
            {
               Execute_Query();
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void EditPymt_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;

            if (pymt.RQTP_CODE.In("001", "009"))
            {
               iScsc.CNCL_PYMT_P(
                  new XElement("Payment",
                     new XAttribute("rqid", pymt.RQID),
                     new XAttribute("cncltype", "002") // صدور صورتحساب اصلاحی
                  )
               );

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "search"), new XAttribute("enrollnumber", FNGR_PRNT_TextEdit.Text), new XAttribute("formcaller", GetType().Name))}
                     })
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
            {
               Execute_Query();
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void DelPymt_Tsmi_Click(object sender, EventArgs e)
      {

      }

      private void AcptPymt_Tsmi_Click(object sender, EventArgs e)
      {

      }

      private void BlokPymt_Tsmi_Click(object sender, EventArgs e)
      {

      }

      private void UnBlokPymt_Tsmi_Click(object sender, EventArgs e)
      {

      }

      private void CnclPymtWithoutRcpt_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            iScsc.CNCL_PYMT_P(
               new XElement("Payment",
                  new XAttribute("rqid", mbsp.RQRO_RQST_RQID),
                  new XAttribute("cncltype", "003") // ابطال عادی صورتحساب بدون بازگشت
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

      private void AddNote_Butn_Click(object sender, EventArgs e)
      {
         var figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;

         if (NoteBs.List.OfType<Data.Note>().Any(n => n.CODE == 0)) return;

         var note = NoteBs.AddNew() as Data.Note;
         note.FIGH_FILE_NO = fileno;
         iScsc.Notes.InsertOnSubmit(note);
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var note = NoteBs.Current as Data.Note;
            if (note == null) return;

            if (MessageBox.Show(this, "آیا با حذف توضیحات موافق هستید؟", "حذف توضیحات", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Notes.DeleteOnSubmit(note);
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
            {
               Execute_Query();
               tb_master.SelectedTab = tp_004;
            }
         }
      }

      private void SaveNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Note_Gv.PostEditor();
            NoteBs.EndEdit();

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
            {
               Execute_Query();
               tb_master.SelectedTab = tp_004;
            }
         }
      }

      private void CalcAttn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FromAttnDate_Dt.CommitChanges();
            ToAttnDate_Dt.CommitChanges();

            if (!FromAttnDate_Dt.Value.HasValue) { FromAttnDate_Dt.Focus(); return; }
            if (!ToAttnDate_Dt.Value.HasValue) { ToAttnDate_Dt.Focus(); return; }

            var attns = 
               iScsc.Attendances
               .Where(
                  a => a.FIGH_FILE_NO == fileno &&
                       a.ATTN_DATE >= FromAttnDate_Dt.Value.Value.Date &&
                       a.ATTN_DATE <= ToAttnDate_Dt.Value.Value.Date &&
                       a.ATTN_STAT == "002" &&
                       a.EXIT_TIME != null
               );

            int h = attns.Sum(a => (int)(a.EXIT_TIME.Value.TotalHours - a.ENTR_TIME.Value.TotalHours));
            int m = Math.Abs(attns.Sum(a => (int)(a.EXIT_TIME.Value.TotalHours - a.ENTR_TIME.Value.TotalHours) * 60 - (int)(a.EXIT_TIME.Value.TotalMinutes - a.ENTR_TIME.Value.TotalMinutes)));

            if(m >= 60)
            {
               h += m / 60;
               m = m % 60;
            }

            TotlHourAttn_Txt.Text = h.ToString();
            TotlMinAttn_Txt.Text = m.ToString();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GPymBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var gpym = GPymBs.Current as Data.Payment_Method;
            if (gpym == null) return;

            GPym_Txt.Text = 
               "کسر مبلغ اعتبار سپرده بابت شماره درخواست [ " + gpym.Payment.RQST_RQID.ToString() + " ] در تاریخ [ " + gpym.Payment.CRET_DATE.GetPersianDate() + " - " + gpym.Payment.CRET_DATE.Value.ToString("HH:mm:ss") + " ] برای اقلام زیر صادر شده است : \n" + 
               (
                  string.Join("\n\r, ", gpym.Payment.Payment_Details.Select(pd => string.Format("[ {0} ] [ {1} ] [ {2} ] ", pd.Expense.EXPN_DESC, pd.Method.MTOD_DESC, pd.Category_Belt.CTGY_DESC)))
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GotoRqstForm_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = iScsc.Requests.FirstOrDefault(r => r.RQID == (vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult).RQST_RQID);
            if (Rqst == null) return;

            int SpecFormNumb = 0;
            string SpecFormString = "";

            switch (Rqst.RQTP_CODE)
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
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelFigh_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف کامل مشتری موافق هستید، بعد از انجام عملیات به هیچ عنوان اطلاعات قابلیت بازیابی را ندارند", "هشدار جهت عملیات بدون بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;

            iScsc.DEL_FIGH_P(
               new XElement("Fighter",
                   new XAttribute("fileno", fileno)
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
               Btn_Back_Click(null, null);
         }
      }

      private void PydtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {

      }

      private void CGrp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Fighter_Grouping"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddFGrp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FGrpBs.List.OfType<Data.Fighter_Grouping>().Any(g => g.CODE == 0)) return;

            var fgrp = FGrpBs.AddNew() as Data.Fighter_Grouping;
            fgrp.FIGH_FILE_NO = fileno;

            iScsc.Fighter_Groupings.InsertOnSubmit(fgrp);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelFGrp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var fgrp = FGrpBs.Current as Data.Fighter_Grouping;
            if (fgrp == null) return;

            if (MessageBox.Show(this, "آیا با حذف گروه برای مشتری موافق هستید؟", "حذف گروه", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Fighter_Groupings.DeleteOnSubmit(fgrp);
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

      private void SaveFGrp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FGrpGv.PostEditor();

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

      private void CellPhon_Lsbx_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            vSmsdBs.DataSource = iScsc.V_Smsd_Message_Boxes.Where(s => s.PHON_NUMB == CellPhon_Lsbx.Text);
            vSmslBs.List.Clear();
         }
         catch (Exception exc)
         {
            iScsc.SaveException(exc);
         }
      }

      private void vSmsdBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var vsmsd = vSmsdBs.Current as Data.V_Smsd_Message_Box;
            if (vsmsd == null) return;

            vSmslBs.DataSource = iScsc.V_Sms_Message_Boxes.Where(s => s.PHON_NUMB == CellPhon_Lsbx.Text && s.ACTN_DATE.Value.Date == vsmsd.ACTN_DATE);
         }
         catch (Exception exc)
         {
            iScsc.SaveException(exc);
         }
      }

      private void AddDsct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FgdcBs.List.OfType<Data.Fighter_Discount_Card>().Any(fd => fd.CODE == 0)) return;

            var _fgdc = FgdcBs.AddNew() as Data.Fighter_Discount_Card;
            _fgdc.FIGH_FILE_NO = fileno;
            _fgdc.STAT = "002";
            _fgdc.DSCT_TYPE = "001";
            _fgdc.DSCT_AMNT = 10;
            _fgdc.DISC_CODE = Guid.NewGuid().ToString().Split('-')[0].ToUpper();
            _fgdc.EXPR_DATE = DateTime.Now.AddDays(7);

            iScsc.Fighter_Discount_Cards.InsertOnSubmit(_fgdc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelDsct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _fgdc = FgdcBs.Current as Data.Fighter_Discount_Card;
            if (_fgdc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Fighter_Discount_Cards.DeleteOnSubmit(_fgdc);

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

      private void SaveDsct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FgdcBs.EndEdit();
            Fgdc_gv.PostEditor();

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

      private void ChngRecdDsct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _fgdc = FgdcBs.Current as Data.Fighter_Discount_Card;
            if (_fgdc == null || _fgdc.CODE == 0) return;


            if(_fgdc.MTOD_CODE != null && MessageBox.Show(this, "آیا با تغییرات کد تخفیف از حالت خصوصی به حالت عمومی موافق هستید؟", "تغییر وضعیت رکورد تخفیف", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
               iScsc.ExecuteCommand("UPDATE dbo.Fighter_Discount_Card SET MTOD_CODE = NULL, CTGY_CODE = NULL WHERE CODE = {0};", _fgdc.CODE);
               requery = true;
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

      private void RecdDsct_Pkbt_PickCheckedChange(object sender)
      {
         try
         {
            MtodCtgy_Splt.Visible = MtodCtgyAcpt_Butn.Visible = RecdDsct_Pkbt.PickChecked;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MtodCtgyAcpt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _fgdc = FgdcBs.Current as Data.Fighter_Discount_Card;
            if (_fgdc == null) return;

            var _ctgy = CtgyBs.Current as Data.Category_Belt;
            if (_ctgy == null) return;
            
            iScsc.ExecuteCommand("UPDATE dbo.Fighter_Discount_Card SET MTOD_CODE = {1}, CTGY_CODE = {2} WHERE CODE = {0};", _fgdc.CODE, _ctgy.MTOD_CODE, _ctgy.CODE);
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

      private void ChngStat_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _fgdc = FgdcBs.Current as Data.Fighter_Discount_Card;
            if (_fgdc == null) return;

            iScsc.ExecuteCommand("UPDATE dbo.Fighter_Discount_Card SET STAT = {1} WHERE CODE = {0};", _fgdc.CODE, (_fgdc.STAT == "001" ? "002" : "001"));
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

      private void AddCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (CallBs.List.OfType<Data.Fighter_Call>().Any(c => c.CODE == 0)) return;

            var _call = CallBs.AddNew() as Data.Fighter_Call;
            if (_call == null) return;

            _call.FIGH_FILE_NO = fileno;
            _call.CALL_DATE = DateTime.Now;
            iScsc.Fighter_Calls.InsertOnSubmit(_call);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _call = CallBs.Current as Data.Fighter_Call;
            if (_call == null) return;

            if (MessageBox.Show(this, "با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.Fighter_Call WHERE Code = {0};", _call.CODE));
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
               tb_master.SelectedTab = tp_008;
            }
         }
      }

      private void SaveCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CallBs.EndEdit();
            Call_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               tb_master.SelectedTab = tp_008;
            }
         }
      }

      private void FCal_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Fighter_Call"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RlodFCal_Butn_Click(object sender, EventArgs e)
      {
         Refresh_Butn_Click(null, null);
         tb_master.SelectedTab = tp_008;
      }

      private void AddSurv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SurvBs.List.OfType<Data.Survey>().Any(s => s.CODE == 0)) return;

            var _surv = SurvBs.AddNew() as Data.Survey;
            if (_surv == null) return;

            iScsc.Surveys.InsertOnSubmit(_surv);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelSurv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _surv = SurvBs.Current as Data.Survey;
            if (_surv == null) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.Survey WHERE Code = {0};", _surv.CODE));
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               tb_master.SelectedTab = tp_008;
            }
         }
      }

      private void SaveSurv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Surv_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               tb_master.SelectedTab = tp_008;
            }
         }
      }

      private void CSurv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Fighter_Call_Survey"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RlodSurv_Butn_Click(object sender, EventArgs e)
      {
         Refresh_Butn_Click(null, null);
         tb_master.SelectedTab = tp_008;
      }

      private void InsAlSurv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _call = CallBs.Current as Data.Fighter_Call;
            if (_call == null) return;

            iScsc.ExecuteCommand(
               string.Format(
               "MERGE dbo.Survey T" + Environment.NewLine +
               "USING (SELECT Code FROM dbo.App_Base_Define WHERE ENTY_NAME = 'Fighter_Call_Survey') S" + Environment.NewLine + 
               "ON (T.Call_Code = {0} AND T.SURV_APBS_CODE = S.CODE)" + Environment.NewLine + 
               "WHEN NOT MATCHED THEN" + Environment.NewLine +
               "INSERT (CALL_CODE, CODE, SURV_APBS_CODE) VALUES ({0}, dbo.GNRT_NVID_U(), S.Code);", _call.CODE
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
            if(requery)
            {
               Execute_Query();
               tb_master.SelectedTab = tp_008;
            }
         }
      }

      private void RCal_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var _call = CallBs.Current as Data.Fighter_Call;
            if (_call == null || e.NewValue == null) return;

            _call.App_Base_Define = RCalBs.List.OfType<Data.App_Base_Define>().SingleOrDefault(a => a.CODE == (long)e.NewValue);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CallBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _call = CallBs.Current as Data.Fighter_Call;
            if (_call == null) return;

            RCal_Lov.EditValue = _call.RSLT_APBS_CODE;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GnrtDsctSend_Butn_Click(object sender, EventArgs e)
      {
         tb_master.SelectedTab = tp_001;
         tb_1_tp_001.SelectedTab = tabPage7;
      }

      private void TempDsct_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _dsct = FgdcBs.Current as Data.Fighter_Discount_Card;
            if (_dsct == null || _dsct.TEMP_TMID == null) return;

            if(_dsct.TEMP_TMID != (long?)TempDsct_Lov.EditValue)
            {
               _dsct.Template = TempBs.List.OfType<Data.Template>().FirstOrDefault(t => t.TMID == (long?)TempDsct_Lov.EditValue);
            }

            #region Precondiotion
            var _crnt = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "005");
            string msg = _dsct.Template.TEMP_TEXT;

            string clubname = "";

            if (_crnt.INSR_CNAM_STAT == "002")
            {
               if (_crnt.CLUB_NAME.Trim().Length == 0 && _crnt.CLUB_CODE == null)
               {
                  MessageBox.Show("نام بخش مشخص نشده است");
                  return;
               }
               if (_crnt.CLUB_NAME.Trim().Length > 0)
                  clubname = "\n" + _crnt.CLUB_NAME;
               else
                  clubname = "\n" + _crnt.Club.NAME;
            }            
            #endregion

            switch (e.Button.Index)
            {
               case 1:
                  // ارسال پیامک به مشتری از طریق پیامک
                  if (CellPhon01_Txt.Text.Length == 0)
                  {
                     MessageBox.Show("شماره تلفن وارد نشده");
                     return;
                  }
                  
                  iScsc.MSG_SEND_P(
                     new XElement("Process",
                        new XElement("Contacts",
                           new XAttribute("subsys", 5),
                           new XAttribute("linetype", _crnt.LINE_TYPE),
                           new XElement("Contact",
                              new XAttribute("phonnumb", CellPhon01_Txt.Text),
                              new XElement("Message",
                                 new XAttribute("type", _crnt.MSGB_TYPE),
                                 new XAttribute("scdldate", DateTime.Now),
                                 new XAttribute("btchnumb", _crnt.BTCH_NUMB ?? 0),
                                 new XAttribute("stepmin", _crnt.STEP_MIN ?? 0),
                                 new XAttribute("sendtype", "002"), // Bulk Send
                                 string.Format("{0}{1}", 
                                    iScsc.GET_TEXT_F(
                                       new XElement("TemplateToText",
                                           new XAttribute("fileno", fileno),
                                           new XAttribute("mbsprwno", 1),
                                           new XAttribute("fgdccode", _dsct.CODE),
                                           new XAttribute("text", msg)
                                       )                                       
                                    ).Value, 
                                    clubname
                                 )
                              )
                           )
                        )
                     )
                  );

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new List<object>
                           {
                              ToolTipIcon.Info,
                              "پیام شما با موفقیت درون لیست ارسال سامانه پیامکی قرار گرفت",
                              "سامانه پیامکی",
                              2000
                           }
                     }
                  );
                  break;
               case 2:
                  // ارسال پیام به مشتری از طریق بله 
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
