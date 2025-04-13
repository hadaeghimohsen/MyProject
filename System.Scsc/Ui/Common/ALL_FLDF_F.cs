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
using System.IO;

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

         tb_master_SelectedIndexChanged(null, null);
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
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  })
            );
         }
         else if(tb_master.SelectedTab == tp_007)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_007_F"))}
                  })
            );
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

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
                  })
            );
         }
         else if(tb_master.SelectedTab == tp_007)
         {
            if (GlrlBs.Current == null) return;
            var _glrl = GlrlBs.Current as Data.Gain_Loss_Rial;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_007_F"), string.Format("Request.Rqid = {0}", _glrl.RQRO_RQST_RQID))}
                  })
            );
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

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                 {
                    new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Fighter.File_No = {0}", crnt.FILE_NO))}
                 })
            );
         }
         else if (tb_master.SelectedTab == tp_007)
         {
            if (GlrlBs.Current == null) return;
            var _glrl = GlrlBs.Current as Data.Gain_Loss_Rial;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                 new List<Job>
                 {
                    new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_007_F"), string.Format("Request.Rqid = {0}", _glrl.RQRO_RQST_RQID))}
                 })
            );
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
         else if(rqst.RQTP_CODE == "034")
         {
            RouterMethod = 166;
            RouterGateway = "SHOW_MBSC_F";
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
            var _attn = AttnBs2.Current as Data.Attendance;

            switch (e.Button.Index)
            {
               /*case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", attn.FIGH_FILE_NO)) }
                  );
                  break;*/
               case 1:
                  if (_attn.EXIT_TIME == null)
                  {
                     if (MessageBox.Show(this, "با خروج دستی مشتری موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.INS_ATTN_P(_attn.CLUB_CODE, _attn.FIGH_FILE_NO, null, null, "003", _attn.MBSP_RWNO_DNRM, "001", "002");
                     iScsc = new Data.iScscDataContext(ConnectionString);
                     AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
                  }
                  break;
               case 2:
                  if (_attn.ATTN_STAT == "002")
                  {
                     bool _ctrlHold = ModifierKeys.HasFlag(Keys.Control);
                     if (MessageBox.Show(this, "با ابطال رکورد مشتری مشتری موافق هستید؟", "ابطال رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.UPD_ATTN_P(
                        new XElement("Process",
                           new XElement("Attendance",
                              new XAttribute("code", _attn.CODE),
                              new XAttribute("type", "001") // ابطال رکورد مشتری
                           )
                        )
                     );

                     if(_ctrlHold)
                     {
                        iScsc.ExecuteCommand("DELETE dbo.Dresser_Attendance WHERE Attn_Code = {0}; DELETE dbo.Attendance WHERE Code = {0};", _attn.CODE);
                     }

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
                  _attn.EXIT_TIME = null;
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Attendance SET Exit_Time = null WHERE Code = {0};", _attn.CODE));
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
         dynamic _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (_figh == null)
            _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         if (_figh.FNGR_PRNT_DNRM == null || _figh.FNGR_PRNT_DNRM == "") { FngrPrnt_Txt.Focus(); MessageBox.Show(this, "کد شناسایی برای مشتری وارد نشده. لطفا بررسی و اصلاح کنید", "عدم وجود کد شناسایی برای مشتری", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

         if (_figh.FIGH_STAT == "001") { MessageBox.Show(this, "مشتری در وضعیت قفل قرار دارد، و آن را اول آزاد کنید و دوباره درخواست را انجام دهید.", "مشتری قفل میباشد", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

         //if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == fileno && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;
         if (_figh.TYPE == "002" || _figh.TYPE == "003" || _figh.TYPE == "004") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", _figh.FNGR_PRNT_DNRM), new XAttribute("formcaller", GetType().Name))}
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
         dynamic _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
         if (_figh == null)
            _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

         //if (_figh.FNGR_PRNT_DNRM == null || _figh.FNGR_PRNT_DNRM == "") { FngrPrnt_Txt.Focus(); MessageBox.Show(this, "کد شناسایی برای مشتری وارد نشده. لطفا بررسی و اصلاح کنید", "عدم وجود کد شناسایی برای مشتری", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

         if (_figh.FIGH_STAT == "001") { MessageBox.Show(this, "مشتری در وضعیت قفل قرار دارد، و آن را اول آزاد کنید و دوباره درخواست را انجام دهید.", "مشتری قفل میباشد", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", _figh.FILE_NO), new XAttribute("auto", "true"), new XAttribute("formcaller", GetType().Name))}
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
            string _fngrprnt = "";
            if(ModifierKeys.HasFlag(Keys.Control))
            {
               if (FngrPrnt_Txt.Text != "")return;

               _fngrprnt = iScsc.VF_All_Info_Fighters(fileno).Where(f => f.FNGR_PRNT != null && f.FNGR_PRNT != "" && f.FNGR_PRNT.Length >= 1).OrderByDescending(f => f.RWNO).Take(1).FirstOrDefault().FNGR_PRNT;
               if (MessageBox.Show(this, "آیا با بازیابی کد شناسایی موافق هستید؟" + Environment.NewLine + "کد شناسایی : " + _fngrprnt, "بازیابی کد شناسایی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
            }
            else
            {
               if (MessageBox.Show(this, "آیا با حذف کد شناسایی موافق هستید؟", "حذف کد شناسایی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
            }
            //var figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            iScsc.SCV_PBLC_P(
               new XElement("Process",
                  new XElement("Fighter",
                     new XAttribute("fileno", fileno),
                     new XAttribute("columnname", "FNGR_PRNT"),
                     new XAttribute("newvalue", _fngrprnt)
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

            iScsc.ExecuteCommand(string.Format("UPDATE Fighter SET Debt_Dnrm = dbo.GET_DBTF_U(FILE_NO), DPST_AMNT_DNRM = dbo.GET_DPST_U(FILE_NO) WHERE FILE_NO = {0}", figh.FILE_NO));
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
               PmmtGv.PostEditor();

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
            var pd = PydtsBs1.Current as Data.Payment_Detail;
            if (pd == null) return;

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
                                 new XAttribute("code", pd.CODE)
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
                                 new XAttribute("code", pd.CODE),
                                 new XAttribute("expncode", pd.EXPN_CODE),
                                 new XAttribute("expnpric", pd.EXPN_PRIC),
                                 new XAttribute("pydtdesc", pd.PYDT_DESC ?? ""),
                                 new XAttribute("qnty", pd.QNTY ?? 1),
                                 new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                                 new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0),
                                 new XAttribute("mtodcodednrm", pd.MTOD_CODE_DNRM),
                                 new XAttribute("ctgycodednrm", pd.CTGY_CODE_DNRM),
                                 new XAttribute("mbsprwno", pd.MBSP_RWNO ?? 0),
                                 new XAttribute("exprdate", pd.EXPR_DATE == null ? "" : pd.EXPR_DATE.Value.ToString("yyyy-MM-dd")),
                                 new XAttribute("fromnumb", pd.FROM_NUMB ?? 0),
                                 new XAttribute("tonumb", pd.TO_NUMB ?? 0),
                                 new XAttribute("extscode", pd.EXTS_CODE ?? 0),
                                 new XAttribute("extsrsrvdate", pd.EXTS_RSRV_DATE == null ? "" : pd.EXTS_RSRV_DATE.Value.ToString("yyyy-MM-dd")),
                                 new XAttribute("totlwegh", pd.TOTL_WEGH ?? 0),
                                 new XAttribute("unitnumb", pd.UNIT_NUMB ?? 0),
                                 new XAttribute("unitapbscode", pd.UNIT_APBS_CODE ?? 0),
                                 new XAttribute("cmnt", pd.CMNT ?? "")
                              )
                           )
                        )
                     );
                     requery = true;
                  }
                  break;
               case 2:
                  return;
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
                  if (checkOK)
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
                                       new XAttribute("pydtcode", pd.CODE),
                                       new XAttribute("fileno", fileno)
                                    )
                              }
                           }
                        )
                     );
                  }
                  break;
               case 3:
                  if (pd.Request_Row.RQTP_CODE != "016") return;
                  if (MessageBox.Show(this, "آیا با حذف صاحب هزینه موافق هستید؟", "حذف صاحب هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                  if (ModifierKeys == Keys.Control)
                  {
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET FIGH_FILE_NO = NULL WHERE Pymt_Rqst_Rqid = {0} AND FIGH_FILE_NO = {1};", pd.PYMT_RQST_RQID, pd.FIGH_FILE_NO));
                  }
                  else
                  {
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET FIGH_FILE_NO = NULL WHERE Code = {0};", pd.CODE));
                  }

                  requery = true;
                  break;
               case 4:
                  if (pd.Request_Row.RQTP_CODE != "016") return;
                  if (MessageBox.Show(this, "آیا با حذف رکورد هزینه خدمات وابسته به دوره موافق هستید؟", "حذف رکورد خدمات وابسته به دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                  if (ModifierKeys == Keys.Control)
                  {
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET Mbsp_Figh_File_No = NULL, Mbsp_Rwno = NULL, Mbsp_Rect_Code = NULL WHERE Pymt_Rqst_Rqid = {0} AND FIGH_FILE_NO = {1};", pd.PYMT_RQST_RQID, pd.FIGH_FILE_NO));
                  }
                  else
                  {
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET Mbsp_Figh_File_No = NULL, Mbsp_Rwno = NULL, Mbsp_Rect_Code = NULL WHERE Code = {0};", pd.CODE));
                  }

                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch { }
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
            dynamic _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (_figh == null)
               _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;
            
            if (_figh.FNGR_PRNT_DNRM == null || _figh.FNGR_PRNT_DNRM == "") { FngrPrnt_Txt.Focus(); MessageBox.Show(this, "کد شناسایی برای مشتری وارد نشده. لطفا بررسی و اصلاح کنید", "عدم وجود کد شناسایی برای مشتری", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            //if (_figh.FIGH_STAT == "001") { MessageBox.Show(this, "مشتری در وضعیت قفل قرار دارد، و آن را اول آزاد کنید و دوباره درخواست را انجام دهید.", "مشتری قفل میباشد", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            var mbsp = MbspBs.Current as Data.Member_Ship;
            if (mbsp == null) return;

            // 1403/06/03 * IF EXISTS Grouping Permission CANNOT Attendance
            if (iScsc.Fighter_Grouping_Permissions.Any(gp => gp.Fighter_Grouping.FIGH_FILE_NO == mbsp.FIGH_FILE_NO && gp.Fighter_Grouping.GROP_STAT == "002" /* وضعیت */ && gp.PERM_TYPE == "001" /* حضور و غیاب */ && gp.PERM_STAT == "001" /* غیرمجاز */))
            {
               MessageBox.Show(this, "خطا - مشتری به دلیل تصمیم مدیریتی مجاز به ورود نمیباشد، لطفا با بخش مدیریت صحبت کنید", "");
               return;
            }

            if (_figh.TYPE == "003")
            {
               bool _acesPerm = true;
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> {"<Privilege>288</Privilege><Sub_Sys>5</Sub_Sys>", "DataGuard"},
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 #region Show Error
                                 MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 288 امنیتی", "خطا دسترسی");
                                 _acesPerm = false;
                                 #endregion                           
                              })
                           },
                           #endregion                        
                        })                     
                     })
               );

               if (!_acesPerm) return;
            }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                     
                     new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                     new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", _figh.FNGR_PRNT_DNRM), new XAttribute("mbsprwno", mbsp.RWNO), new XAttribute("attnsystype", "001"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            //Refresh_Butn_Click(null, null);
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
            var _mbsp = MbspBs.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            if (_mbsp.Fighter.FNGR_PRNT_DNRM == null || _mbsp.Fighter.FNGR_PRNT_DNRM == "") { FngrPrnt_Txt.Focus(); MessageBox.Show(this, "کد شناسایی برای مشتری وارد نشده. لطفا بررسی و اصلاح کنید", "عدم وجود کد شناسایی برای مشتری", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            if (_mbsp.Fighter.FIGH_STAT == "001") { MessageBox.Show(this, "مشتری در وضعیت قفل قرار دارد، و آن را اول آزاد کنید و دوباره درخواست را انجام دهید.", "مشتری قفل میباشد", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

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
                                 new XAttribute("mbsprwno", _mbsp.RWNO),
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
            //LstDVipGv.ActiveFilterString = string.Format("IP_ADRS = '{0}'", LstDVipBs.List.OfType<Data.Dresser>().Where(d => iScsc.exter));

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
            QStrtTime_Tim.EditValue = mbsp.STRT_DATE;//.Fighter_Public.Club_Method.STRT_TIME;
            QEndTime_Tim.EditValue = mbsp.END_DATE;//.Fighter_Public.Club_Method.END_TIME;
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
                  if(Deposit_Tc.SelectedTab == DTp_001)
                  {
                     GlrlBs.DataSource = iScsc.Gain_Loss_Rials.Where(g => g.FIGH_FILE_NO == fileno && g.CONF_STAT == "002" && g.AMNT > 0);
                     GPymBs.DataSource = iScsc.Payment_Methods.Where(p => p.FIGH_FILE_NO_DNRM == fileno && p.RCPT_MTOD == "005");
                  }
                  else if(Deposit_Tc.SelectedTab == DTp_002)
                  {
                     MsexBs.DataSource = iScsc.Misc_Expenses.Where(m => m.VALD_TYPE == "002" && m.COCH_FILE_NO == fileno);
                  }
                  break;
               case 4:
                  vF_Request_DocumentBs.DataSource = iScsc.VF_Request_Document(fileno); ;
                  break;
               case 5:
                  vF_Request_ChangingBs.DataSource = iScsc.VF_Request_Changing(fileno).OrderBy(r => r.RQST_DATE);
                  LOptBs.DataSource =
                     iScsc.Log_Operations
                     .Where(lo =>
                        lo.FIGH_FILE_NO == fileno
                     );
                  break;
               case 6:
                  AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
                  break;
               case 9:
                  int _rqst = RqstBs.Position;
                  int _rqpm = RqpmBs.Position;
                  int _rqpv = RqpvBs.Position;
                  RqstBs.DataSource = 
                     from r in iScsc.Requests
                     join rr in iScsc.Request_Rows on r.RQID equals rr.RQST_RQID
                     where r.RQST_STAT == "002" &&
                           (r.RQTP_CODE == "001" || r.RQTP_CODE == "009" || r.RQTP_CODE == "016") &&
                           rr.FIGH_FILE_NO == fileno
                     orderby r.SAVE_DATE
                     select r;
                  RqstBs.Position = _rqst;
                  RqpmBs.Position = _rqpm;
                  RqpvBs.Position = _rqpv;
                  break;
               case 10:
                  int _pmct = PmctBs1.Position;
                  PmctBs1.DataSource =
                     iScsc.Payment_Contracts
                     .Where(pc => pc.Payment.Request.RQST_STAT == "002" && pc.Payment.Request.Request_Rows.Any(rr => rr.FIGH_FILE_NO == fileno))
                     .OrderByDescending(pc => pc.CRET_DATE);
                  PmctBs1.Position = _pmct;
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
         if (tb_master.SelectedTab == tp_001)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  })
            );
         }
         else if(tb_master.SelectedTab == tp_007)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_007_F"))}
                  })
            );
         }
      }

      private void Print_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //Back_Butn_Click(null, null);
            if (tb_master.SelectedTab == tp_001)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("File_No = {0}", fileno))}
                    })
               );
            }
            else if(tb_master.SelectedTab == tp_007)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_007_F"), string.Format("File_No = {0}", fileno))}
                    })
               );
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintDefault_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               //Back_Butn_Click(null, null);
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */)
                       {
                          Input = 
                           new XElement("Print", 
                              new XAttribute("type", "Default"), 
                              new XAttribute("modual", GetType().Name), 
                              new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), 
                              string.Format("File_No = {0}", fileno) 
                           )
                       }
                    })
               );
            }
            else if (tb_master.SelectedTab == tp_007)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_007_F"), string.Format("File_No = {0}", fileno))}
                    })
               );
            }

         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void GlrIndc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (_figh == null)
               _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            if (_figh.FNGR_PRNT_DNRM == null || _figh.FNGR_PRNT_DNRM == "") { FngrPrnt_Txt.Focus(); MessageBox.Show(this, "کد شناسایی برای مشتری وارد نشده. لطفا بررسی و اصلاح کنید", "عدم وجود کد شناسایی برای مشتری", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            if (_figh.FIGH_STAT == "001") { MessageBox.Show(this, "مشتری در وضعیت قفل قرار دارد، و آن را اول آزاد کنید و دوباره درخواست را انجام دهید.", "مشتری قفل میباشد", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

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
            dynamic _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;
            if (_figh == null)
               _figh = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_Deleted_FighterResult;

            if (_figh.FNGR_PRNT_DNRM == null || _figh.FNGR_PRNT_DNRM == "") { FngrPrnt_Txt.Focus(); MessageBox.Show(this, "کد شناسایی برای مشتری وارد نشده. لطفا بررسی و اصلاح کنید", "عدم وجود کد شناسایی برای مشتری", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            if (_figh.FIGH_STAT == "001") { MessageBox.Show(this, "مشتری در وضعیت قفل قرار دارد، و آن را اول آزاد کنید و دوباره درخواست را انجام دهید.", "مشتری قفل میباشد", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

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
            //RcmtType_Butn.Text = RcmtType_Butn.Tag.ToString() == "0" ? "POS" : "نقدی";
            //RcmtType_Butn.Tag = RcmtType_Butn.Tag.ToString() == "0" ? "1" : "0";
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

            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;

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

            // 1401/09/19 * #MahsaAmini
            // اگر تخفیف برای پرسنل بخواهیم ثبت کنیم باید چک کنیم که آیا تخفیف وارد شده بیشتر سهم پرسنل نباشد
            if (PydsType_Lov.EditValue.ToString() == "005")
            {
               var _pydt = PydtsBs1.Current as Data.Payment_Detail;

               var _calcexpn =
                  iScsc.CALC_EXPN_U(
                     new XElement("Request",
                         new XAttribute("rqid", pymt.RQID),
                         new XAttribute("expncode", _pydt.EXPN_CODE)
                     )
                  );

               // اگر مبلغ تخفیف بیشتر از سهم پرسنل باشد باید جلو آن گرفته شود
               if (_calcexpn < amnt)
               {
                  MessageBox.Show(this, "مبلغ تخفیف وارد شده از سهم پرسنل بیشتر حق پرداختی ایشان میباشد، لطفا درصد تخفیف یا مبلغ تخفیف را اصلاح کنید", "تخفیف غیرمجاز پرسنل");
                  return;
               }
            }

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQID, (short?)1, pydt.EXPN_CODE, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text, null, null);

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

            //1403/08/26 * اگر تاریخ پرداخت بیشتر از تاریخ جاری باشد
            if (PymtDate_DateTime001.Value.HasValue && PymtDate_DateTime001.Value.Value.Date > DateTime.Now.Date)
            {
               MessageBox.Show(this, "پرداختی در گذشته داریم ولی پرداختی در آینده نداریم، اینجاست که باید بگم داش داری اشتباه میزنی");
               PymtDate_DateTime001.Focus();
               PymtDate_DateTime001.Value = DateTime.Now;
               return;
            }

            switch ((RcmtType_Lov.EditValue ?? "001").ToString())
            {
               case "003":
                  if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

                  if (UsePos_Cb.Checked && (!PymtDate_DateTime001.Value.HasValue || PymtDate_DateTime001.Value.Value.Date == DateTime.Now.Date))
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
                        PymtAmnt_Txt.EditValue = Convert.ToInt64(PymtAmnt_Txt.EditValue) * 10;

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
                                             new XAttribute("amnt", Convert.ToInt64(PymtAmnt_Txt.EditValue)),
                                             new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                                             new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                                             new XAttribute("rcptfilepath", /*RcptFilePath_Txt.EditValue ??*/ "")
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
                                 new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                                 new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                                 new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                                 new XAttribute("rcptfilepath", /*RcptFilePath_Txt.EditValue ??*/ "")
                              )
                           )
                        )
                     );
                  }
                  break;
               default:
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQID),
                              new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                              new XAttribute("rcptmtod", RcmtType_Lov.EditValue ?? "001"),
                              new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                              new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                              new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                              new XAttribute("rcptfilepath", /*RcptFilePath_Txt.EditValue ??*/ "")
                           )
                        )
                     )
                  );
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
            RqstBnDeleteFngrPrnt1_Click(null, null);

            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */)
                     {
                        Input = 
                           new XElement("DeviceControlFunction", 
                              new XAttribute("functype", (ModifierKeys == Keys.Control ? "5.2.3.8.1" /* Add Face */ : "5.2.3.8" /* Add Finger */)), 
                              new XAttribute("funcdesc", "Add User Info"), 
                              new XAttribute("enrollnumb", FngrPrnt_Txt.Text)
                           )
                     }
                  })
            );
         }
         catch { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */)
                     {
                        Input = 
                           new XElement("DeviceControlFunction", 
                              new XAttribute("functype", (ModifierKeys == Keys.Control ? "5.2.3.8.2" /* Delete Face */ : "5.2.3.5" /* Delete Finger */)), 
                              new XAttribute("funcdesc", "Delete User Info"), 
                              new XAttribute("enrollnumb", FngrPrnt_Txt.Text)
                           )
                     }
                  })
            );
         }
         catch (Exception exc) { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */)
                     {
                        Input = new XElement("DeviceControlFunction", 
                           new XAttribute("functype", "5.2.7.2" /* Duplicate */), 
                           new XAttribute("funcdesc", "Duplicate User Info Into All Device"), 
                           new XAttribute("enrollnumb", FngrPrnt_Txt.Text)
                        )
                     }
                  })
            );
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrNewEnrollPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف اثر انگشت از مشتری و اختصاص برای کاربر جدید موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var fngrprnt = FngrPrnt_Txt.Text;

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
            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", FngrPrnt_Txt.Text)
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
            if (FngrPrnt_Txt.Text == "") { FngrPrnt_Txt.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", FngrPrnt_Txt.Text)
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
               // 1404/01/05 * Fill Payment if empty
               if (vF_SavePaymentsBs.List.Count == 0)
               {
                  vF_SavePaymentsBs.DataSource = iScsc.VF_Save_Payments(null, fileno).OrderByDescending(p => p.PYMT_CRET_DATE);
                  ShowCrntReglYear_Butn_Click(null, null);
               }

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
                        new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "search"), new XAttribute("enrollnumber", FngrPrnt_Txt.Text), new XAttribute("formcaller", GetType().Name))}
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
            if (MessageBox.Show(this, "آیا با حذف کامل مشتری موافق هستید، بعد از انجام عملیات به هیچ عنوان اطلاعات قابلیت بازیابی را ندارند", "تاییدیه اول : هشدار جهت عملیات بدون بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            if (MessageBox.Show(this, "آیا با حذف کامل مشتری موافق هستید، بعد از انجام عملیات به هیچ عنوان اطلاعات قابلیت بازیابی را ندارند", "تاییدیه دوم : هشدار جهت عملیات بدون بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            if (MessageBox.Show(this, "آیا با حذف کامل مشتری موافق هستید، بعد از انجام عملیات به هیچ عنوان اطلاعات قابلیت بازیابی را ندارند", "تاییدیه سوم : هشدار جهت عملیات بدون بازگشت", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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

            iScsc.ExecuteCommand(string.Format("EXEC dbo.DEL_FGRP_P @X = N'<Fighter_Grouping code=\"{0}\" />';", fgrp.CODE));
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
            FgprGv.PostEditor();

            var _a = iScsc.GetChangeSet();

            //iScsc.SubmitChanges();
            FGrpBs.List.OfType<Data.Fighter_Grouping>()
               .Where(g => g.CODE == 0).ToList()
               .ForEach(g =>
               {
                  iScsc.ExecuteCommand(string.Format("INSERT INTO dbo.Fighter_Grouping(CODE, FIGH_FILE_NO, GROP_CODE, GROP_DESC) VALUES (0, {0}, {1}, N'{2}');", g.FIGH_FILE_NO, g.GROP_CODE, g.GROP_DESC));
               });

            FGrpBs.List.OfType<Data.Fighter_Grouping>()
               .Where(g => g.CODE != 0).ToList()
               .ForEach(g =>
               {
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Grouping SET GROP_DESC = N'{1}' WHERE CODE = {0};", g.CODE, g.GROP_DESC));
               });
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

            if (_fgdc.RQST_RQID != null && _fgdc.STAT == "001") { MessageBox.Show(this, string.Format("این رکورد تخفیف برای شماره درخواست " + "[ {0} ]" + " استفاده شده است و دیگر قادر به ویرایش آن نیستید.", _fgdc.RQST_RQID), "عدم تغییر در رکورد تخفیف"); return; }

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

            if (_fgdc.RQST_RQID != null && _fgdc.STAT == "001") { MessageBox.Show(this, string.Format("این رکورد تخفیف برای شماره درخواست " + "[ {0} ]" + " استفاده شده است و دیگر قادر به ویرایش آن نیستید.", _fgdc.RQST_RQID), "عدم تغییر در رکورد تخفیف"); return; }

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

            _call.App_Base_Define = DRCalBs.List.OfType<Data.App_Base_Define>().SingleOrDefault(a => a.CODE == (long)e.NewValue);
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

      private void vF_All_Info_FightersBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            RefHImgProf_Rb.ImageVisiable = true;

            var _histServ = vF_All_Info_FightersBs.Current as Data.VF_All_Info_FightersResult;
            if (_histServ == null) return;

            if (_histServ.REF_CODE != null)
            {
               RhServBs.DataSource = iScsc.Fighters.Where(s => s.FILE_NO == _histServ.REF_CODE);
               if (RhServBs.List.Count == 0)
               {
                  HRefCode_Gb.Visible = false;
               }
               else
               {
                  HRefCode_Gb.Visible = true;
                  RefHCont_Txt.Text = iScsc.Fighters.Where(s => s.REF_CODE_DNRM == _histServ.REF_CODE).Count().ToString();
                  try
                  {
                     RefHImgProf_Rb.ImageProfile = null;
                     MemoryStream mStream = new MemoryStream();
                     byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _histServ.REF_CODE))).ToArray();
                     mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                     Bitmap bm = new Bitmap(mStream, false);
                     mStream.Dispose();

                     if (InvokeRequired)
                        Invoke(new Action(() => RefHImgProf_Rb.ImageProfile = bm));
                     else
                        RefHImgProf_Rb.ImageProfile = bm;
                  }
                  catch
                  {
                     RefHImgProf_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
                  }
               }
            }
            else
            {
               HRefCode_Gb.Visible = false;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddFRlt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void DelFRlt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaveFRlt_Butn_Click(object sender, EventArgs e)
      {

      }

      private void FRlt_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "Fighter_RelationShip"),
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

      private void FRltLoad_Butn_Click(object sender, EventArgs e)
      {
         Refresh_Butn_Click(null, null);
         tb_master.SelectedTab = tp_009;
      }

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RtServBs.DataSource =
               iScsc.Fighters
               .Where(f => 
                  (f.FRST_NAME_DNRM == null || f.FRST_NAME_DNRM.Contains(FrstName_Txt.Text)) &&
                  (f.FRST_NAME_DNRM == null || f.LAST_NAME_DNRM.Contains(LastName_Txt.Text)) &&
                  (f.CELL_PHON_DNRM == null || f.CELL_PHON_DNRM.Contains(CellPhon_Txt.Text)) &&
                  (f.NATL_CODE_DNRM == null || f.NATL_CODE_DNRM.Contains(NatlCode_Txt.Text)) &&
                  (BothSex_Rb.Checked || (Men_Rb.Checked && f.SEX_TYPE_DNRM == "001") || (Women_Rb.Checked && f.SEX_TYPE_DNRM == "002"))
               ).Take((int)FtchRows_Nud.Value);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void RqstBnPrint_Click(object sender, EventArgs e)
      {
         if (vF_SavePaymentsBs.Current == null) return;
         var crnt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);

      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (vF_SavePaymentsBs.Current == null) return;
         var crnt = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;

         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_002_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void SaveRqpm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RqpmBs.EndEdit();
            RqpmGv.PostEditor();

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
               tb_master.SelectedTab = tp_010;
            }
         }
      }

      private void FRqpm_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "Request_Parameter"),
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

      private void RlodRqpm_Butn_Click(object sender, EventArgs e)
      {
         Refresh_Butn_Click(null, null);
         tb_master.SelectedTab = tp_010;
      }

      private void InsFRqpm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs.Current as Data.Request;
            if (_rqst == null) return;

            iScsc.ExecuteCommand(
               string.Format(
               "MERGE dbo.Request_Parameter T" + Environment.NewLine +
               "USING (SELECT Code FROM dbo.App_Base_Define WHERE ENTY_NAME = 'Request_Parameter' AND REF_CODE IS NULL) S" + Environment.NewLine +
               "ON (T.Rqst_Rqid = {0} AND T.APBS_CODE = S.CODE)" + Environment.NewLine +
               "WHEN NOT MATCHED THEN" + Environment.NewLine +
               "INSERT (Rqst_Rqid, CODE, APBS_CODE) VALUES ({0}, dbo.GNRT_NVID_U(), S.Code);", _rqst.RQID
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
               tb_master.SelectedTab = tp_010;
            }
         }
      }

      private void SaveRqpv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RqpvBs.EndEdit();
            RqpvGv.PostEditor();

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
               tb_master.SelectedTab = tp_010;
            }
         }
      }

      private void InsRqpv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqpm = RqpmBs.Current as Data.Request_Parameter;
            if (_rqpm == null) return;

            iScsc.ExecuteCommand(
               string.Format(
               "MERGE dbo.Request_Parameter_Value T" + Environment.NewLine +
               "USING (SELECT Code FROM dbo.App_Base_Define WHERE Ref_Code = {1}) S" + Environment.NewLine +
               "ON (T.Rqpm_Code = {0} AND T.APBS_CODE = S.CODE)" + Environment.NewLine +
               "WHEN NOT MATCHED THEN" + Environment.NewLine +
               "INSERT (Rqpm_Code, CODE, APBS_CODE) VALUES ({0}, dbo.GNRT_NVID_U(), S.Code);", _rqpm.CODE, _rqpm.APBS_CODE
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
               tb_master.SelectedTab = tp_010;
            }
         }
      }

      private void RqpmBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _rqpm = RqpmBs.Current as Data.Request_Parameter;
            if (_rqpm == null) return;

            DRqpmBs.DataSource = _rqpm.App_Base_Define;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddMexm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            if (MexmBs.List.OfType<Data.Misc_Expense_Method>().Any(m => m.CODE == 0)) return;

            var _mexm = MexmBs.AddNew() as Data.Misc_Expense_Method;
            _mexm.MSEX_CODE = _msxc.CODE;
            _mexm.RCPT_MTOD = "003";
            _mexm.ACTN_DATE = DateTime.Now;
            _mexm.AMNT = _msxc.SUM_NET_AMNT_DNRM - (_msxc.SUM_RCPT_PYMT_DNRM + _msxc.SUM_COST_AMNT_DNRM + _msxc.SUM_DSCT_AMNT_DNRM);

            iScsc.Misc_Expense_Methods.InsertOnSubmit(_mexm);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelMexm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mexm = MexmBs.Current as Data.Misc_Expense_Method;
            if (_mexm == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Methods.DeleteOnSubmit(_mexm);
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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void SaveMexm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Mexm_Gv.PostEditor();

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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void AddMeck_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            if (MeckBs.List.OfType<Data.Misc_Expense_Check>().Any(m => m.CODE == 0)) return;

            var _meck = MeckBs.AddNew() as Data.Misc_Expense_Check;
            _meck.MSEX_CODE = _msxc.CODE;
            _meck.CHEK_DATE = DateTime.Now;

            iScsc.Misc_Expense_Checks.InsertOnSubmit(_meck);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelMeck_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _meck = MeckBs.Current as Data.Misc_Expense_Check;
            if (_meck == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Checks.DeleteOnSubmit(_meck);
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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void SaveMeck_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Meck_Gv.PostEditor();

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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void AddMexd_Butn_Click(object sender, EventArgs e)
      {

      }

      private void DelMexd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mexd = MexdBs.Current as Data.Misc_Expense_Discount;
            if (_mexd == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Discounts.DeleteOnSubmit(_mexd);
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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void SaveMexd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Mexd_Gv.PostEditor();

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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void DsctDef_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "Misc_Expense_Discount_INFO"),
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

      private void MexdActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            var _dmexd = DMexdBs.Current as Data.App_Base_Define;
            if (_dmexd == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  var _mexd = MexdBs.AddNew() as Data.Misc_Expense_Discount;
                  _mexd.MSEX_CODE = _msxc.CODE;
                  _mexd.DSCT_APBS_CODE = _dmexd.CODE;

                  iScsc.Misc_Expense_Discounts.InsertOnSubmit(_mexd);
                  iScsc.SubmitChanges();
                  break;
               default:
                  break;
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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void DelMexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mexc = MexcBs.Current as Data.Misc_Expense_Cost;
            if (_mexc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            iScsc.Misc_Expense_Costs.DeleteOnSubmit(_mexc);
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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void SaveMexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Mexc_Gv.PostEditor();

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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void MexcDef_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "Misc_Expense_Cost_INFO"),
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

      private void MexcActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _msxc = MsexBs.Current as Data.Misc_Expense;
            if (_msxc == null) return;

            var _dmexc = DMexcBs.Current as Data.App_Base_Define;
            if (_dmexc == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  var _mexc = MexcBs.AddNew() as Data.Misc_Expense_Cost;
                  _mexc.MSEX_CODE = _msxc.CODE;
                  _mexc.COST_APBS_CODE = _dmexc.CODE;

                  iScsc.Misc_Expense_Costs.InsertOnSubmit(_mexc);
                  iScsc.SubmitChanges();
                  break;
               default:
                  break;
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
               tb_master.SelectedTab = tp_007;
               Deposit_Tc.SelectedTab = DTp_002;
            }
         }
      }

      private void PrntActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _msex = MsexBs.Current as Data.Misc_Expense;
            if (_msex == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  // Print Settings
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                           new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_0i7_F"))}
                        })
                  );

                  // string.Format("Misc_Expense.Code = {0}", _msex.CODE)
                  break;
               case 1:
                  // Select Print
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_0i7_F"), string.Format("Misc_Expense.Code = {0}", _msex.CODE))}
                        })
                  );
                  break;
               case 2:
                  // Default Print
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_0i7_F"), string.Format("Misc_Expense.Code = {0}", _msex.CODE))}
                        })
                  );
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

      private void SaveMsex_Butn_Click(object sender, EventArgs e)
      {

      }

      private void DelMsex_Butn_Click(object sender, EventArgs e)
      {

      }

      private void MsexActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var pyde = PydeBs.Current as Data.Payment_Expense;
            if (pyde == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", pyde.Payment_Detail.Request_Row.FIGH_FILE_NO)) }
            );
         }
         catch (Exception)
         {

         }
      }

      private void JoinDasr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (AdatnBs.List.OfType<Data.Dresser_Attendance>().Any(a => a.DERS_NUMB == DresNumb_Txt.Text.ToInt32() && a.FIGH_FILE_NO == fileno && a.LEND_TIME != null && a.TKBK_TIME == null)) return;

            if (!iScsc.Dressers.Any(d => d.DRES_NUMB == DresNumb_Txt.Text.ToInt32())) return;

            var _mbsp = MbspBs.Current as Data.Member_Ship;
            if (_mbsp == null) return;
               
            var _adatn = AdatnBs.AddNew() as Data.Dresser_Attendance;
            _adatn.FIGH_FILE_NO = fileno;
            _adatn.DERS_NUMB = DresNumb_Txt.Text.ToInt32();
            _adatn.MBSP_RWNO = _mbsp.RWNO;
            _adatn.MBSP_RECT_CODE = _mbsp.RECT_CODE;

            iScsc.Dresser_Attendances.InsertOnSubmit(_adatn);
            iScsc.SubmitChanges();

            DresNumb_Txt.Text = "";
            DresNumb_Txt.Focus();

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

      private void ConfDasr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  "UPDATE dbo.Dresser_Attendance SET Conf_Stat = '002' WHERE FIGH_FILE_NO = {0} AND TKBK_TIME IS NULL;" + Environment.NewLine +
                  "UPDATE dbo.Dresser_Attendance SET TKBK_TIME = GETDATE() WHERE FIGH_FILE_NO != {0} AND TKBK_TIME IS NULL AND Ders_Numb IN (SELECT da.Ders_Numb FROM dbo.Dresser_Attendance da WHERE da.Figh_File_No = {0} AND da.TKBK_TIME IS NULL);",
                  fileno
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

      private void DelDasr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  "DELETE dbo.Dresser_Attendance WHERE FIGH_FILE_NO = {0} AND TKBK_TIME IS NULL AND CONF_STAT = '001';",
                  fileno
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

      private void DresNumb_Txt_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            JoinDasr_Butn_Click(null, null);
         }
      }

      private void MbspActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _mbsp = MbspBs.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            bool _chckAces = true;

            switch (e.Button.Index)
            {
               case 0:
                  // Decrement Session                  
                  _DefaultGateway.Gateway(
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
                                       "<Privilege>290</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       _chckAces = false;
                                       MessageBox.Show(this, "عدم دسترسی به ردیف 290 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                    })
                                 }
                                 #endregion                        
                              })                     
                           })
                  );

                  if(_chckAces)
                  {
                     if (_mbsp.NUMB_OF_ATTN_MONT >= 1 && _mbsp.SUM_ATTN_MONT_DNRM < _mbsp.NUMB_OF_ATTN_MONT)
                     {
                        _mbsp.SUM_ATTN_MONT_DNRM++;
                        iScsc.SubmitChanges();
                        //iScsc.ExecuteCommand(
                        //   string.Format("UPADTE dbo.Member_Ship SET SUM_ATTN_MONT_DNRM += 1 WHERE FIGH_FILE_NO = {0} AND RECT_CODE = '004' AND RWNO = {1}", _mbsp.FIGH_FILE_NO, _mbsp.RWNO)
                        //);
                        iScsc.INS_LGOP_P(
                           new XElement("Log",
                               new XAttribute("fileno", _mbsp.FIGH_FILE_NO),
                               new XAttribute("type", "011"),
                               new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + TitlForm_Lb.Text + " یک جلسه به صورت دستی کم کرد")
                           )
                        );
                        requery = true;
                     }
                  }
                  break;
               case 1:
                  // increment Session
                  _DefaultGateway.Gateway(
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
                                       "<Privilege>289</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       _chckAces = false;
                                       MessageBox.Show(this, "عدم دسترسی به ردیف 289 امنیتی", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Stop);                             
                                    })
                                 }
                                 #endregion                        
                              })                     
                           })
                  );

                  if (_chckAces)
                  {
                     if (_mbsp.NUMB_OF_ATTN_MONT >= 1 && _mbsp.SUM_ATTN_MONT_DNRM <= _mbsp.NUMB_OF_ATTN_MONT && _mbsp.SUM_ATTN_MONT_DNRM > 0)
                     {
                        _mbsp.SUM_ATTN_MONT_DNRM--;
                        iScsc.SubmitChanges();
                        //iScsc.ExecuteCommand(
                        //   string.Format("UPADTE dbo.Member_Ship SET SUM_ATTN_MONT_DNRM -= 1 WHERE FIGH_FILE_NO = {0} AND RECT_CODE = '004' AND RWNO = {1}", _mbsp.FIGH_FILE_NO, _mbsp.RWNO)
                        //);
                        iScsc.INS_LGOP_P(
                           new XElement("Log",
                               new XAttribute("fileno", _mbsp.FIGH_FILE_NO),
                               new XAttribute("type", "012"),
                               new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + TitlForm_Lb.Text + " یک جلسه به صورت دستی برگشت داد")
                           )
                        );
                        requery = true;
                     }
                  }
                  break;
               case 2:
                  // Edit Member_ship
                  Mbsp_Rwno_Text_DoubleClick(null, null);
                  break;
               case 3:
                  tb_1_tp_001.SelectedTab = tp_1_tp_001_006;
                  DresNumb_Txt.Focus();
                  break;
               default:
                  break;
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

      private void DartActv_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _dart = AdatnBs.Current as Data.Dresser_Attendance;
            if (_dart == null) return;

            iScsc.Dresser_Attendances.DeleteOnSubmit(_dart);
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

      private void SetPymtContItem_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _pmct = PmctBs1.Current as Data.Payment_Contract;
            if (_pmct == null) return;

            iScsc.ExecuteCommand(
               string.Format(
                  "MERGE dbo.Payment_Contract_Detail T" + Environment.NewLine +
                  "USING (SELECT {0} AS PMCT_CODE, a.CODE AS ITEM_CODE, a.REF_CODE AS GROP_CODE FROM dbo.App_Base_Define a WHERE a.ENTY_NAME = 'PaymentContractItem_INFO' AND a.REF_CODE IS NOT NULL) S" + Environment.NewLine +
                  "ON (T.Pmct_Code = S.Pmct_Code AND T.Grop_Item_Apbs_Code = S.Grop_Code AND T.Sub_Item_Apbs_Code = S.Item_Code)" + Environment.NewLine +
                  "WHEN NOT MATCHED THEN INSERT (Pmct_Code, Grop_Item_Apbs_Code, Sub_Item_Apbs_Code, Code) VALUES (S.Pmct_Code, S.Grop_Code, S.Item_Code, dbo.GNRT_NVID_U());",
                  _pmct.CODE
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

      private void ContRecd_Butn_Click(object sender, EventArgs e)
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
                              new XAttribute("tablename", "PaymentContractItem_INFO"),
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

      private void SavePmct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Pcdt_Gv.PostEditor();
            PmctBs1.EndEdit();
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

      private void FlpcActn1_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _flpc = FlpcBs.Current as Data.Fighter_Link_Payment_Contarct_Item;
            if (_flpc == null) return;

            var _pcdt = PcdtBs1.Current as Data.Payment_Contract_Detail;
            if (_pcdt == null) return;

            if (_pcdt.SUB_ITEM_APBS_CODE != _flpc.PMCT_ITEM_APBS_CODE) return;

            iScsc.ExecuteCommand(
               string.Format("UPDATE dbo.Payment_Contract_Detail SET FLPC_CODE = {0} WHERE CODE = {1};", _flpc.CODE, _pcdt.CODE)
            );
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

      private void PmctItemActn_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _item = DPmctBs.Current as Data.App_Base_Define;
            if (_item == null) return;

            var _personel = CochBs1.Current as Data.Fighter;
            if (_personel == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  if (FlpcBs.List.OfType<Data.Fighter_Link_Payment_Contarct_Item>().Any(a => a.FIGH_FILE_NO == _personel.FILE_NO && a.PMCT_ITEM_APBS_CODE == _item.CODE)) return;

                  var _flpc = FlpcBs.AddNew() as Data.Fighter_Link_Payment_Contarct_Item;
                  _flpc.FIGH_FILE_NO = _personel.FILE_NO;
                  _flpc.PMCT_ITEM_APBS_CODE = _item.CODE;

                  iScsc.Fighter_Link_Payment_Contarct_Items.InsertOnSubmit(_flpc);
                  iScsc.SubmitChanges();
                  break;
               default:
                  break;
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

      private void FlcpActn_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _flpc = FlpcBs.Current as Data.Fighter_Link_Payment_Contarct_Item;
            if (_flpc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            //iScsc.Fighter_Link_Payment_Contarct_Items.DeleteOnSubmit(_flpc);
            //iScsc.SubmitChanges();
            iScsc.ExecuteCommand(string.Format("DELETE dbo.Fighter_Link_Payment_Contarct_Item WHERE Code = {0};", _flpc.CODE));

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

      private void FocusPymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            tb_master.SelectedTab = tp_003;
            var _pmct = PmctBs1.Current as Data.Payment_Contract;
            if (_pmct == null) return;

            vF_SavePaymentsBs.Position = vF_SavePaymentsBs.IndexOf(vF_SavePaymentsBs.List.OfType<Data.VF_Save_PaymentsResult>().FirstOrDefault(p => p.RQID == _pmct.Payment.RQST_RQID));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PcdtBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _pcdt = PcdtBs1.Current as Data.Payment_Contract_Detail;
            if (_pcdt == null) return;

            Flpc_Gv.ActiveFilterString = string.Format("PMCT_ITEM_APBS_CODE = {0}", _pcdt.SUB_ITEM_APBS_CODE);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GrntCoch_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Chatid_Txt.Text == "") return;
            if (iScsc.Fighters.FirstOrDefault(c => c.FILE_NO == fileno && c.FGPB_TYPE_DNRM != "003") == null) return;

            iScsc.ExecuteCommand(
               string.Format(@"MERGE iRoboTech.dbo.Service_Robot_Group T
                              USING (
                                 SELECT sr.SERV_FILE_NO, sr.ROBO_RBID, sr.CHAT_ID, g.GPID, 
                                   FROM iRoboTech.dbo.Service_Robot sr, iRoboTech.dbo.[Group] g 
                                  WHERE sr.CHAT_ID = {0}
                                    AND g.ROBO_RBID = 391 
                                    AND g.GPID = 121) S
                              ON (T.SRBT_SERV_FILE_NO = S.SERV_FILE_NO AND 
                                  T.SRBT_ROBO_RBID = S.ROBO_RBID AND
                                  T.GROP_GPID = s.GPID)
                              WHEN NOT MATCHED THEN 
                                 INSERT (SRBT_SERV_FILE_NO, SRBT_ROBO_RBID, GROP_GPID, STAT)
                                 VALUES (S.SERV_FILE_NO, s.ROBO_RBID, s.GPID, '002')
                              WHEN MATCHED THEN
                                 UPDATE SET
                                    T.STAT = CASE T.STAT WHEN '002' THEN '001' ELSE '002' END;", Chatid_Txt.Text)
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GlrcActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _glrl = GlrlBs.Current as Data.Gain_Loss_Rial;
            if (_glrl == null) return;

            bool checkOK = true;
            switch (e.Button.Index)
            {
               case 0:
                  // Delete
                  _DefaultGateway.Gateway(
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
                                       "<Privilege>270</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       checkOK = false;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 270 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),                           
                        })
                  );
                  if(checkOK)
                  {
                     if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                     iScsc.DEL_PYMT_P(
                        new XElement("Deposit",
                           new XAttribute("rqid", _glrl.RQRO_RQST_RQID)
                        )
                     );
                     requery = true;
                  }
                  break;
               case 1:
                  // Edit
                  _DefaultGateway.Gateway(
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
                                       "<Privilege>269</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 269 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           #region DoWork
                              new Job(SendType.Self, 169 /* Execute GLR_CHNG_F */),
                              new Job(SendType.SelfToUserInterface, "GLR_CHNG_F", 10 /* execute Actn_CalF_F */)
                              {
                                 Input = 
                                    new XElement("Gain_Loss_Rial",
                                       new XAttribute("fileno", fileno),
                                       new XAttribute("glid", _glrl.GLID),
                                       new XAttribute("formcaller", GetType().Name)
                                    )
                              }
                           #endregion
                        })
                  );
                  break;
               default:
                  break;
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

      private void ActnDVip_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _advip = ADVipBs.Current as Data.Dresser_Vip_Fighter;
            if (_advip == null) return;

            if (MessageBox.Show(this, "ایا با آزاد کردن کمد VIP موافق هستید?", "آزاد کردن کمد VIP", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Dresser_Vip_Fighter SET Stat = '001', Expr_Date = GETDATE(), Lock_Stat = '001' WHERE Code = {0};", _advip.CODE));

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

      private void LstDVip_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (LstDVip_Butn.EditValue == null || LstDVip_Butn.EditValue.ToString() == "") return;

            var _dvipcode = Convert.ToInt64(LstDVip_Butn.EditValue);
            var _mbsp = MbspBs.Current as Data.Member_Ship;            

            switch (e.Button.Index)
            {
               case 1:
                  bool _freelockvip = false;
                  var _lockbydvip = iScsc.Dresser_Vip_Fighters.FirstOrDefault(d => d.DRES_CODE == _dvipcode && d.STAT == "002" && d.MBSP_FIGH_FILE_NO != fileno);
                  if(_lockbydvip != null)
                  {
                     if (MessageBox.Show(this, string.Format("در حال حاضر کمد در اختیار {0} مباشد آیا با آزاد کردن کمد موافق هستید?", _lockbydvip.Fighter.NAME_DNRM), "خطا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
                     _freelockvip = true;
                  }

                  // اگر مشتری دارای کمد اختصاصی یا اجاره ای باشه نباید کمد دیگری به آن داده شود
                  if(iScsc.Dresser_Vip_Fighters.Any(d => d.MBSP_FIGH_FILE_NO == fileno && d.STAT == "002"))
                  {
                     MessageBox.Show(this, "مشتری دارای کمد اختصاصی یا اجاره ای میباشد، شما قادر به اختصاص کمد جدید به این مشتری نیستید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                  }

                  // 1402/10/10 * بررسی اینکه آیا این کمد برای این دوره درست انتخاب شده یا خیر
                  var _edlm = iScsc.External_Device_Link_Methods;
                  if(_edlm.Any())
                  {
                     if(!_edlm.Any(i => i.MTOD_CODE == _mbsp.FGPB_MTOD_CODE_DNRM && iScsc.Dressers.Any(d => d.CODE == _dvipcode && d.IP_ADRS == i.External_Device.IP_ADRS)))
                     {
                        MessageBox.Show(this, "کمد انتخاب شده برای این گروه خدمات تعریف نشده، لطفا اصلاح کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                     }
                  }

                  if(_freelockvip)
                     iScsc.ExecuteCommand(string.Format("UPDATE dbo.Dresser_Vip_Fighter SET Stat = '001' WHERE Code = {0};", _lockbydvip.CODE));
                  iScsc.ExecuteCommand("INSERT INTO dbo.Dresser_Vip_Fighter (Dres_Code, Mbsp_Figh_File_No, Mbsp_Rwno, Mbsp_Rect_Code, Code, Stat) VALUES ({0}, {1}, {2}, '004', 0, '002');", _dvipcode, fileno, _mbsp.RWNO);
                  break;
               default:
                  break;
            }

            DresVipNorm_Tb.SelectedTab = DresVip_Tp;

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

      private void DresVipNormType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _btn = (SimpleButton)sender;

            switch (_btn.Tag.ToString())
            {
               case "norm":
                  _btn.Tag = "vip";
                  _btn.Text = "کمد VIP";

                  LstDVipBs.DataSource = iScsc.Dressers.Where(d => d.VIP_STAT == "002" && d.REC_STAT == "002" && !d.Dresser_Vip_Fighters.Any(dv => dv.STAT == "002"));
                  break;
               case "vip":
                  _btn.Tag = "norm";
                  _btn.Text = "کمد اجاره ای";

                  LstDVipBs.DataSource = iScsc.Dressers.Where(d => d.VIP_STAT == "001" && d.REC_STAT == "002" && !d.Dresser_Attendances.Any(da => da.Attendance.EXIT_TIME == null) && !d.Dresser_Vip_Fighters.Any(dv => dv.STAT == "002"));
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DpstTran_Butn_Click(object sender, EventArgs e)
      {

      }

      private void CyclTran_Butn_Click(object sender, EventArgs e)
      {

      }

      private void PmmtActn_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  DeltPymt_Butn_Click(null, null);
                  break;
               case 1:
                  PymtSave_Butn_Click(null, null);
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

      private void PydsActn_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  DeltPyds_Butn_Click(null, null);
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

      private void Rtoa_Lov_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                           new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("App_Base",
                                    new XAttribute("tablename", "Payment_To_Another_Account"),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        }
                     )
                  );
                  break;
               case 2:
                  PymtSave_Butn_Click(null, null);
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

      private void PydsDesc_Butn_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  PymtSave_Butn_Click(null, null);
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

      private void PayDepositeDebt_Tsmi_Click(object sender, EventArgs e)
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

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
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
                           new XAttribute("rcptmtod", "005"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            Search_Butn_Click(null, null);
         }
      }

      private void PayCard2CardDebt_Tsmi_Click(object sender, EventArgs e)
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
                           new XAttribute("rcptmtod", "009"),
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

      private void ActnMbsp_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            var _mbsp = MbspBs.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            if (_pydt.Request_Row.RQTP_CODE != "016") return;

            switch (e.Button.Index)
            {
               case 0:
                  if (_pydt.MBSP_RWNO == _mbsp.RWNO) return;
                  if (_pydt.MBSP_RWNO != null && MessageBox.Show(this, "آیا با ویرایش کردن ردیف تمدید دوره موافق هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET Mbsp_Figh_File_No = {1}, Mbsp_Rwno = {2}, Mbsp_Rect_Code = '004' WHERE Code = {0};", _pydt.CODE, fileno, _mbsp.RWNO));
                  requery = true;
                  break;
               default:
                  break;
            }
            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               UpdateDebt_Btn_Click(null, null);
               //Execute_Query();
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void ActnCbmt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            var _cbmt = CbmtBs1.Current as Data.Club_Method;
            if (_cbmt == null) return;

            if (_pydt.Request_Row.RQTP_CODE != "016") return;

            switch (e.Button.Index)
            {
               case 0:
                  if (_pydt.CBMT_CODE_DNRM == _cbmt.CODE && _pydt.FIGH_FILE_NO == _cbmt.COCH_FILE_NO) return;
                  if (_pydt.FIGH_FILE_NO != null && MessageBox.Show(this, "آیا با اصلاح صاحب هزینه آیتم درآمد موافق هستید؟", "اصلاح صاحب هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Payment_Detail SET Cbmt_Code_DNRM = {1}, Figh_File_No = {2} WHERE Code = {0};", _pydt.CODE, _cbmt.CODE, _cbmt.COCH_FILE_NO));
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               UpdateDebt_Btn_Click(null, null);
               //Execute_Query();
               tb_master.SelectedTab = tp_003;
            }
         }
      }

      private void PydtsBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (_pydt == null) return;

            PdCbmt_Gv.ActiveFilterString = string.Format("Mtod_Code = {0}", _pydt.MTOD_CODE_DNRM);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FGropActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _fgrp = FGrpBs.Current as Data.Fighter_Grouping;
            if (_fgrp == null) return;

            switch (e.Button.Index)
            {
               case 0:                  
                  iScsc.ExecuteCommand(string.Format("EXEC dbo.UPD_FGRP_P @X = N'<Fighter_Grouping code=\"{0}\" />';", _fgrp.CODE));
                  _fgrp.GROP_STAT = _fgrp.GROP_STAT == "002" ? "001" : "002";
                  break;
               case 1:
                  iScsc.ExecuteCommand(
                     string.Format(
                     "MERGE dbo.Fighter_Grouping_Permission T" + Environment.NewLine + 
                     "USING (SELECT fg.CODE, d.VALU AS PERM_TYPE FROM dbo.Fighter_Grouping fg, dbo.[D$Prmt] d WHERE fg.CODE = {0} AND d.VALU != '000') S" + Environment.NewLine + 
                     "ON (T.FGRP_CODE = S.CODE AND T.PERM_TYPE = S.PERM_TYPE)" + Environment.NewLine +
                     "WHEN NOT MATCHED THEN INSERT (FGRP_CODE, CODE, PERM_TYPE) VALUES (S.CODE, dbo.GNRT_NVID_U(), S.PERM_TYPE);"
                     ,_fgrp.CODE)
                  );
                  SaveFGrp_Butn_Click(null, null);
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

      private void PrmsActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _fgpr = FgprBs.Current as Data.Fighter_Grouping_Permission;
            if (_fgpr == null) return;

            switch (e.Button.Index)
            {
               case 0:                  
                  iScsc.ExecuteCommand(string.Format("EXEC dbo.UPD_FGPR_P @X = N'<Fighter_Grouping_Permission code=\"{0}\" />';", _fgpr.CODE));
                  _fgpr.PERM_STAT = _fgpr.PERM_STAT == "002" ? "001" : "002";
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

      private void RcmtType_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         RcmtType_Butn_Click(null, null);
      }

      private void ExcpDebtActv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '002' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '002', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '002';", fileno
               )
            );
            MessageBox.Show("عملیات استثناء برای بدهی مشتری با موفقیت فعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExcpDebtDact_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '002' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '001', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '001';", fileno
               )
            );
            MessageBox.Show("عملیات استثناء برای بدهی مشتری با موفقیت غیرفعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExcpLockActv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '003' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '002', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '002';", fileno
               )
            );
            MessageBox.Show("عملیات استثناء برای عدم دریافت کمد انلاین با موفقیت فعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExcpLockDact_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               string.Format(
                  @"MERGE dbo.Exception_Operation T
                    USING (SELECT {0} AS FILE_NO, '003' AS EXCP_TYPE) S
                    ON (T.FIGH_FILE_NO = S.FILE_NO AND 
                        T.EXCP_TYPE = S.EXCP_TYPE)
                    WHEN NOT MATCHED THEN
                       INSERT (FIGH_FILE_NO, EXCP_TYPE, STAT, CODE)
                       VALUES (S.FILE_NO, S.EXCP_TYPE, '001', 0)
                    WHEN MATCHED THEN
                       UPDATE SET T.STAT = '001';", fileno
               )
            );
            MessageBox.Show("عملیات استثناء برای عدم دریافت کمد انلاین با موفقیت غیرفعال شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ClearFingerPrint_Butn_MouseMove(object sender, MouseEventArgs e)
      {
         try
         {
            if(ModifierKeys.HasFlag(Keys.Control))
            {
               ClearFingerPrint_Butn.ToolTip = "بازیابی کد شناسایی";
            }
            else
            {
               ClearFingerPrint_Butn.ToolTip = "حذف کد شناسایی";
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddFgbm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FgbmBs.List.OfType<Data.Fighter_Body_Measurement>().Any(i => i.CODE == 0)) return;

            iScsc.CRET_FGBM_P(fileno);
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

      private void DelFgbm_Butn_Click(object sender, EventArgs e)
      {
         try
         {

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveFgbm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Fgbm_Gv.PostEditor();

            FgbmBs.List.OfType<Data.Fighter_Body_Measurement>()
               .ToList()
               .ForEach(b =>
                  iScsc.ExecuteCommand(
                     string.Format("UPDATE dbo.Fighter_Body_Measurement SET MESR_VALU = {0}, CMNT = N'{1}' WHERE CODE = {2};", b.MESR_VALU, (b.CMNT ?? ""), b.CODE)
                  ));

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

      private void CyclSort_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mbsp = MbspBs.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            if (_mbsp.VALD_TYPE == "001") { MessageBox.Show(this, "این رکورد دوره غیرفعال میباشد و نمیتوانید به عنوان مرجع از آن استفاده کنید", "خطا"); return; }

            iScsc.MBSP_SORT_P(
               new XElement("Member_Ship",
                   new XAttribute("fileno", _mbsp.FIGH_FILE_NO),
                   new XAttribute("rwno", _mbsp.RWNO),
                   new XAttribute("enddate", _mbsp.END_DATE)
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
   }
}
