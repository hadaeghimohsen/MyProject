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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
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
                     if (MessageBox.Show(this, "با خروج دستی هنرجو موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.INS_ATTN_P(attn.CLUB_CODE, attn.FIGH_FILE_NO, null, null, "003");
                     iScsc = new Data.iScscDataContext(ConnectionString);
                     AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
                  }
                  break;
               case 2:
                  if (attn.ATTN_STAT == "002")
                  {
                     if (MessageBox.Show(this, "با ابطال رکورد هنرجو هنرجو موافق هستید؟", "ابطال رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.UPD_ATTN_P(
                        new XElement("Process",
                           new XElement("Attendance",
                              new XAttribute("code", attn.CODE),
                              new XAttribute("type", "001") // ابطال رکورد هنرجو
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
                                 new XAttribute("attndate", (AttnBs2.Current as Data.Attendance).ATTN_DATE)
                              )
                           }
                        })
                  );
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

         if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
               })
         );
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
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewinscard"), new XAttribute("fileno", figh.FILE_NO))}
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
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))}
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

         if (MessageBox.Show(this, "آیا با حذف هنرجو موافق هستید؟", "عملیات حذف موقت هنرجو", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
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
            figh.MDFY_DATE = DateTime.Now;

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
         }
         catch (Exception exc)
         {

         }
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
         catch (Exception exc)
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
                                 new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM),
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
         catch (Exception exc)
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
                  new Job(SendType.SelfToUserInterface, "ADM_MBFZ_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "block"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
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

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                     new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
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
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", fileno),
                           new XAttribute("mbsprwno", Mbsp_Rwno_Text.Text)
                        )
                  }
               }
            )
         );
      }
   }
}
