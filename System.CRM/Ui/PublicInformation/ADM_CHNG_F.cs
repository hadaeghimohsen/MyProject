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

namespace System.CRM.Ui.PublicInformation
{
   public partial class ADM_CHNG_F : UserControl
   {
      public ADM_CHNG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int rqstindx = 0;
      private bool reloading = false;
      private string srpbtype;
      private long projrqstrqid;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if(tb_master.SelectedTab == tp_001)
         {
            iCRM = new Data.iCRMDataContext(ConnectionString);
            var Rqids = iCRM.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
               .Where(rqst =>
                     rqst.RQTP_CODE == "002" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.RQTT_CODE == "004" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iCRM.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               )
               .OrderByDescending(
                  rqst => 
                     rqst.RQST_DATE
               );

            ServsBs1.DataSource = iCRM.Services;

            RqstBs1.Position = rqstindx;
         }
      }

      private void Refresh_Clicked(object sender, EventArgs e)
      {
         Execute_Query();
         requery = false;
      }

      #region Toolbar Buttons
      private void RqstBnLoad1_Click(object sender, EventArgs e)
      {
         reloading = true;
         isFirstLoaded = false;
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.SelfToUserInterface, GetType().Name, 07 /* Execute Load */),
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void RqstBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;

            if (rqst == null) return;
            if (rqst.RQID == 0) return;

            if (MessageBox.Show(this, "آیا از انصراف درخواست مطمئن هستید؟", "انصراف درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.CNCL_RQST_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("rqtpcode", rqst.RQTP_CODE)
                  )
               )
            );
            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
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

      private void RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            RqstBs1.EndEdit();
            SrpbBs1.EndEdit();

            StatusSaving_Sic.StateIndex = 2; // Saving Request

            var rqst = RqstBs1.Current as Data.Request;
            //var serv = ServBs1.Current as Data.Service;
            var srpb = SrpbBs1.Current as Data.Service_Public;
            long fileno = 0;
            if (srpb == null)
            {
               if (ServFileNo_Lov.EditValue == null || ServFileNo_Lov.EditValue.ToString() == "") return;
               fileno = Convert.ToInt64(ServFileNo_Lov.EditValue);
            }

            rqstindx = RqstBs1.Position;

            if (rqst == null || rqst.RQID == 0)
            {
               iCRM.ADM_ARQT_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                        new XAttribute("rqtpcode", "002"),
                        new XAttribute("rqttcode", "004"),
                        new XAttribute("projrqstrqid", projrqstrqid),
                        new XElement("Service",
                           new XAttribute("fileno", srpb == null ? fileno : srpb.SERV_FILE_NO)
                        )
                     )
                  )
               );
            }
            else
            {
               iCRM.ADM_ARQT_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                        new XAttribute("rqtpcode", "002"),
                        new XAttribute("rqttcode", rqst == null ? "004" : rqst.RQTT_CODE ?? "004"),
                        new XAttribute("cntycode", rqst == null || rqst.RQID == 0 ? CntyCode_Lov.SelectedValue : srpb.REGN_PRVN_CNTY_CODE),
                        new XAttribute("prvncode", rqst == null || rqst.RQID == 0 ? PrvnCode_Lov.SelectedValue : srpb.REGN_PRVN_CODE),
                        new XAttribute("regncode", rqst == null || rqst.RQID == 0 ? RegnCode_Lov.SelectedValue : srpb.REGN_CODE),
                        new XAttribute("projrqstrqid", rqst == null || rqst.RQID == 0 ? projrqstrqid : rqst.PROJ_RQST_RQID),
                        new XElement("Service",
                           new XAttribute("fileno", srpb == null ? fileno : srpb.SERV_FILE_NO),
                           new XElement("Service_Public",
                              new XElement("Comp_Code", srpb == null ? CompCode_Lov.SelectedValue : srpb.COMP_CODE),
                              new XElement("Frst_Name", srpb == null ? FrstName_Txt.Text : srpb.FRST_NAME),
                              new XElement("Last_Name", srpb == null ? LastName_Txt.Text : srpb.LAST_NAME),
                              new XElement("Fath_Name", srpb == null ? FathName_Txt.Text : srpb.FATH_NAME),
                              new XElement("Natl_Code", srpb == null ? NatlCode_Txt.Text : srpb.NATL_CODE),
                              new XElement("Brth_Date", srpb == null ? BrthDate_Dat.Value.Value.ToString("yyyy-MM-dd") : srpb.BRTH_DATE.Value.ToString("yyyy-MM-dd")),
                              new XElement("Cell_Phon", srpb == null ? CellPhon_Txt.Text : srpb.CELL_PHON),
                              new XElement("Tell_Phon", srpb == null ? TellPhon_Txt.Text : srpb.TELL_PHON),
                              new XElement("Idty_Code", srpb == null ? IdtyCode_Txt.Text : srpb.IDTY_CODE),
                              new XElement("Telg_Chat_Code", srpb == null ? 0 : srpb.TELG_CHAT_CODE),
                              new XElement("Post_Adrs", srpb == null ? PostAdrs_Txt.Text : srpb.POST_ADRS),
                              new XElement("Cord_X", srpb == null ? CordX_Txt.Text : srpb.CORD_X.ToString()),
                              new XElement("Cord_Y", srpb == null ? CordY_Txt.Text : srpb.CORD_Y.ToString()),
                              new XElement("Emal_Adrs", srpb == null ? EmalAdrs_Txt.Text : srpb.EMAL_ADRS),
                              new XElement("Sunt_Bunt_Dept_Orgn_Code", srpb == null ? OrgnCode_Lov.SelectedValue : srpb.SUNT_BUNT_DEPT_ORGN_CODE),
                              new XElement("Sunt_Bunt_Dept_Code", srpb == null ? DeptCode_Lov.SelectedValue : srpb.SUNT_BUNT_DEPT_CODE),
                              new XElement("Sunt_Bunt_Code", srpb == null ? BuntCode_Lov.SelectedValue : srpb.SUNT_BUNT_CODE),
                              new XElement("Sunt_Code", srpb == null ? SuntCode_Lov.SelectedValue : srpb.SUNT_CODE),
                              new XElement("Sex_Type", srpb == null ? SexType_Lov.SelectedValue : srpb.SEX_TYPE),
                              new XElement("Mrid_Type", srpb == null ? MridType_Lov.SelectedValue : srpb.MRID_TYPE),
                              new XElement("Rlgn_Type", srpb == null ? RlgnType_Lov.SelectedValue : srpb.RLGN_TYPE),
                              new XElement("Ethn_City", srpb == null ? EthnCity_Lov.SelectedValue : srpb.ETHN_CITY),
                              new XElement("Cust_Type", srpb == null ? CustType_Lov.SelectedValue : srpb.CUST_TYPE),
                              new XElement("Job_Titl", srpb == null ? JobTitl_Lov.SelectedValue : srpb.JOB_TITL),
                              new XElement("Iscp_Isca_Iscg_Code", srpb == null ? IscgCode_Lov.SelectedValue : srpb.ISCP_ISCA_ISCG_CODE),
                              new XElement("Iscp_Isca_Code", srpb == null ? IscaCode_Lov.SelectedValue : srpb.ISCP_ISCA_CODE),
                              new XElement("Iscp_Code", srpb == null ? IscpCode_Lov.SelectedValue : srpb.ISCP_CODE),
                              new XElement("Type", srpb == null ? SrpbType_Lov.SelectedValue ?? "" : srpb.TYPE),
                              new XElement("Serv_Stag_Code", "001"),
                              new XElement("Face_Book_Url", srpb == null ? FacebookUrl_Txt.Text ?? "" : srpb.FACE_BOOK_URL),
                              new XElement("Link_In_Url", srpb == null ? LinkedIn_Txt.Text ?? "" : srpb.LINK_IN_URL),
                              new XElement("Twtr_Url", srpb == null ? TwitterUrl_Txt.Text ?? "" : srpb.TWTR_URL),
                              new XElement("Serv_No", srpb == null ? ServNo_Txt.Text ?? "" : srpb.SERV_NO)
                           )
                        )
                     )
                  )
               );
            }

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)         
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            switch (exc.Message)
            {
               case "Object reference not set to an instance of an object.":
               case "Value cannot be null.\r\nParameter name: value":
                  MessageBox.Show("لطفا گزینه های قرمز که اجباری هستند را وارد کنید");
                  break;
               default:
                  MessageBox.Show(exc.Message);
                  break;
            }
            iCRM.SaveException(exc);
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
            RqstBs1.EndEdit();
            SrpbBs1.EndEdit();

            StatusSaving_Sic.StateIndex = 2; // Saving Request

            var rqst = RqstBs1.Current as Data.Request;
            var srpb = SrpbBs1.Current as Data.Service_Public;

            rqstindx = RqstBs1.Position;

            iCRM.ADM_ASAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XElement("Request_Row",
                        new XAttribute("rwno", srpb.RQRO_RWNO),
                        new XAttribute("servfileno", srpb.SERV_FILE_NO)
                     )
                  )
               )
            );

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;

            // نمایش اطلاعات مشتری
            if (ServInfo_Pikb.PickChecked)
            {
               switch (srpbtype)
               {
                  case "001":
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", srpb.SERV_FILE_NO))},
                          })
                     );
                     break;
                  case "002":
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", srpb.SERV_FILE_NO))},
                          })
                     );
                     break;
               }
            }

         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            switch (exc.Message)
            {
               case "Object reference not set to an instance of an object.":
               case "Value cannot be null.\r\nParameter name: value":
                  MessageBox.Show("لطفا گزینه های قرمز که اجباری هستند را وارد کنید");
                  break;
               default:
                  MessageBox.Show(exc.Message);
                  break;
            }
            iCRM.SaveException(exc);
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

      private void RqstBnAResn1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnADocPicProfile1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            var result = (
                     from r in iCRM.Regulations
                     join rqrq in iCRM.Request_Requesters on r equals rqrq.Regulation
                     join rqdc in iCRM.Request_Documents on rqrq equals rqdc.Request_Requester
                     join rcdc in iCRM.Receive_Documents on rqdc equals rcdc.Request_Document
                     where r.TYPE == "001"
                        && r.REGL_STAT == "002"
                        && rqrq.RQTP_CODE == rqst.RQTP_CODE
                        && rqrq.RQTT_CODE == rqst.RQTT_CODE
                        && rqdc.DCMT_DSID == 13962055684640 // عکس 4*3
                        && rcdc.RQRO_RQST_RQID == rqst.RQID
                        && rcdc.RQRO_RWNO == 1
                     select rcdc).FirstOrDefault();
            if (result == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self,  41 /* Execute Serv_Camr_F */),
                     new Job(SendType.SelfToUserInterface, "SERV_CAMR_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = result                           
                     }
                  }
               )
            );

         }
         catch
         {

         }
      }

      private void RqstBnADoc1_Click(object sender, EventArgs e)
      {
         var rqst = RqstBs1.Current as Data.Request;
         if (rqst == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {
                 new Job(SendType.Self, 40 /* Execute Cmn_Dcmt_F */),
                 new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_Calf_F */){ Input = iCRM.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
              })

         );
      }

      private void RqstBnRegl01_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var Rg1 = iCRM.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 08 /* Execute Rqrq_Dfin_F */){Input = Rg1},
                     new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", Rqtp_Lov.Tag)))}
                  })
               );
         }
      }
      #endregion      

      #region Binding Events
      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst == null) return;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               //Btn_RqstDelete1.Visible = true; Btn_RqstSav1.Visible = false;
               RqstBnASav1.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               RqstBnASav1.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               StatusSaving_Sic.StateIndex = 0; // New Request
               RqstBnASav1.Enabled = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false; DefaultTabPage001();
            }
         }
         catch
         {
            StatusSaving_Sic.StateIndex = 0; // Error On Request
            RqstBnASav1.Enabled = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false; DefaultTabPage001();
         }
      }

      private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
      {
         try
         {
            if (requery)
               return;

            var rqst = RqstBs1.Current as Data.Request;

            if (rqst.RQID == 0)
            {
               StatusSaving_Sic.StateIndex = 0;
            }
            else
            {
               StatusSaving_Sic.StateIndex = 2;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }      
      #endregion

      #region Button Events
      private void Regn_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 03 /* Execute Regn_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Comp_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 03 /* Execute Regn_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Orgn_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 10 /* Execute Orgn_Dfin_F */),
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
      #endregion

      private void FindGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         if (PostAdrs_Txt.Text == "" || PostAdrs_Txt.Text.Trim() == "") return;

         // "Commons:GMapNets", 10 /* Execute ActionCallForm */, SendType.SelfToUserInterface
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "GMapNets", 10 /* Execute ActionCallForm */)
                        {
                           Input =
                              new XElement("GMapNets",
                                 new XAttribute("requesttype", "GetXYFromAddress"),
                                 new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                                 new XAttribute("callback", 40 /* CordinateGetSet */),
                                 new XAttribute("outputtype", "postaddress"),
                                 new XAttribute("initalset", true),
                                 new XElement("Address",
                                    new XAttribute("cnty", CntyCode_Lov.Text),
                                    new XAttribute("prvn", PrvnCode_Lov.Text),
                                    new XAttribute("regn", RegnCode_Lov.Text),
                                    new XAttribute("value", PostAdrs_Txt.Text)
                                 )
                              ),
                           AfterChangedOutput = 
                              new Action<object>(
                                 output =>
                                 {
                                    var result = output as XElement;
                                    var srpb = SrpbBs1.Current as Data.Service_Public;
                                    srpb.CORD_X = Convert.ToDouble( result.Attribute("cordx").Value );
                                    srpb.CORD_Y = Convert.ToDouble( result.Attribute("cordy").Value );

                                    tb_slave.SelectedTab = tp_0012;
                                 }
                              )
                        }
                     }
                  )
               }
            )
         );
      }

      private void ShowGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         var srpb = SrpbBs1.Current as Data.Service_Public;
         if (srpb == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
            {
               Input =
                  new XElement("GMapNets",
                     new XAttribute("requesttype", "get"),
                     new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                     new XAttribute("callback", 40 /* CordinateGetSet */),
                     new XAttribute("outputtype", "servcord"),
                     new XAttribute("initalset", true),
                     new XAttribute("cordx", srpb.CORD_X == null ? "29.622045" : srpb.CORD_X.ToString()),
                     new XAttribute("cordy", srpb.CORD_Y == null ? "52.522728" : srpb.CORD_Y.ToString()),
                     new XAttribute("zoom", "1600")
                  )
            }
         );
      }

      private void Tag_Butn_Click(object sender, EventArgs e)
      {
         var srpb = SrpbBs1.Current as Data.Service_Public;
         if (srpb == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 50 /* Execute Tsk_Tag_F */),
                  new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", srpb.SERV_FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void AddInfo_Butn_Click(object sender, EventArgs e)
      {
         var srpb = SrpbBs1.Current as Data.Service_Public;
         if (srpb == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 71 /* Execute Add_Info_F */),
                  new Job(SendType.SelfToUserInterface, "ADD_INFO_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", srpb.SERV_FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void ServCont_Butn_Click(object sender, EventArgs e)
      {
         var srpb = SrpbBs1.Current as Data.Service_Public;
         if (srpb == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 60 /* Execute Inf_Ctwk_F */),
                  new Job(SendType.SelfToUserInterface, "INF_CTWK_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", srpb.SERV_FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }
      
   }
}
