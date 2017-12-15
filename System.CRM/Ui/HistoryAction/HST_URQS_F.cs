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
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing.Imaging;

namespace System.CRM.Ui.HistoryAction
{
   public partial class HST_URQS_F : UserControl
   {
      public HST_URQS_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private DateTime datetime;
      private string srchtype = "002";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         try
         {
            if(Tb_Master.SelectedTab == tp_001)
            {
               srchtype = RqstChng_Pn.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked).Tag.ToString();
               #region Request
               switch (srchtype)
	            {
                  case "001":
                     RqstChngBs.DataSource = 
                        from rq in iCRM.VF_Request_Changing(null, null, null, null)
                        where rq.SAVE_DATE.Value.Date < DateTime.Now.Date
                           && rq.CRET_BY.ToUpper() == CurrentUser.ToUpper()
                        orderby rq.SAVE_DATE descending
                        select rq;
                     break;
                  case "002":
                     RqstChngBs.DataSource =
                        from rq in iCRM.VF_Request_Changing(null, null, null, null)
                        where rq.SAVE_DATE.Value.Date == DateTime.Now.Date
                           && rq.CRET_BY.ToUpper() == CurrentUser.ToUpper()
                        orderby rq.SAVE_DATE descending
                        select rq;
                     break;
                  case "003":
                     if(RqstToDate_Date.Value.HasValue)
                        datetime = RqstToDate_Date.Value.Value;
                     else
                        RqstToDate_Date.Value = datetime = DateTime.Now.AddDays(-1);

                     RqstChngBs.DataSource =
                        from rq in iCRM.VF_Request_Changing(null, null, null, null)
                        where rq.SAVE_DATE.Value.Date == datetime.Date
                           && rq.CRET_BY.ToUpper() == CurrentUser.ToUpper()
                        orderby rq.SAVE_DATE descending
                        select rq;
                     break;
               }
               #endregion
            }
            else if(Tb_Master.SelectedTab == tp_002)
            {
               RqstFind_Butn_Click(null, null);
            }
            else if(Tb_Master.SelectedTab == tp_003)
            {
               srchtype = AdvSearchRqstChng_Pn.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked).Tag.ToString();
               var Qxml =
                  new XElement("Search",
                     new XElement("Query_String",
                        new XAttribute("textsrch", string.Format("%{0}%", TextSearch_Txt.Text.Replace(' ', '%')))
                     )
                  );

               if (TextSearch_Txt.Properties.Buttons[2].Tag != null)
                  Qxml.Add(TextSearch_Txt.Properties.Buttons[2].Tag as XElement);
               else
                  Qxml.Add(new XElement("Tags", new XAttribute("cont", 0)));

               if (TextSearch_Txt.Properties.Buttons[3].Tag != null)
                  Qxml.Add(TextSearch_Txt.Properties.Buttons[3].Tag as XElement);
               else
                  Qxml.Add(new XElement("Regions", new XAttribute("cont", 0)));


               #region Request
               switch (srchtype)
               {
                  case "001":
                     AdvSearchRqstChngBs.DataSource =
                        from rq in iCRM.VF_Advance_Search_Request_Changing(Qxml)
                        where rq.SAVE_DATE.Value.Date < DateTime.Now.Date
                           && rq.CRET_BY.ToUpper() == CurrentUser.ToUpper()
                        orderby rq.SAVE_DATE descending
                        select rq;
                     break;
                  case "002":
                     AdvSearchRqstChngBs.DataSource =
                        from rq in iCRM.VF_Advance_Search_Request_Changing(Qxml)
                        where rq.SAVE_DATE.Value.Date == DateTime.Now.Date
                           && rq.CRET_BY.ToUpper() == CurrentUser.ToUpper()
                        orderby rq.SAVE_DATE descending
                        select rq;
                     break;
                  case "003":
                     if (AdvSearchRqstToDate_Date.Value.HasValue)
                        datetime = AdvSearchRqstToDate_Date.Value.Value;
                     else
                        AdvSearchRqstToDate_Date.Value = datetime = DateTime.Now.AddDays(-1);

                     AdvSearchRqstChngBs.DataSource =
                        from rq in iCRM.VF_Advance_Search_Request_Changing(Qxml)
                        where rq.SAVE_DATE.Value.Date == datetime.Date
                           && rq.CRET_BY.ToUpper() == CurrentUser.ToUpper()
                        orderby rq.SAVE_DATE descending
                        select rq;
                     break;
               }
               #endregion
            }
         }
         catch { }
      }

      private void rb_requestsearch_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            var rb = sender as RadioButton;
            srchtype = rb.Tag.ToString();                        
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            Execute_Query();
         }
      }

      private string GetGroupTimePriod(DateTime currentdate)
      {
         var result = "نامشخص";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 30 /* Execute DoWork4GetTimePeriod */, SendType.Self)
            {
               Input =
                  new XElement("TimePeriod",
                     new XAttribute("timetype", "group"),
                     new XAttribute("crntdate", currentdate)
                  ),
               AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     if (output != null)
                        result = output.ToString();
                  })
            }
         );

         return result;
      }

      private string GetTimePeriod(DateTime currentdate)
      {
         var result = "نامشخص";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 30 /* Execute DoWork4GetTimePeriod */, SendType.Self)
            {
               Input =
                  new XElement("TimePeriod",
                     new XAttribute("timetype", "normal"),
                     new XAttribute("crntdate", currentdate)
                  ),
               AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     if (output != null)
                        result = output.ToString();
                  })
            }
         );

         return result;
      }

      private void Rqst_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "SAVE_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "SAVE_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void RqstChngBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstChngBs.Current as Data.VF_Request_ChangingResult;

            RqstRelatedService_Pb.ImageVisiable = true;
            RqstRelatedService_Pb.ImageProfile = GetImageActivity(rqst.RQTP_CODE);

            try
            {
               ServiceProfile_Pb.ImageVisiable = true;
               ServiceProfile_Pb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", rqst.FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               
               ServiceProfile_Pb.ImageProfile = bm;
            }
            catch
            {
               ServiceProfile_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
            }

            var comp = iCRM.Companies.FirstOrDefault(c => c.CODE == rqst.COMP_CODE);
            if (comp.LOGO == null)
            {
               CompanyLogo_Pb.ImageVisiable = true;
               byte[] bytes = null;
               MemoryStream ms = new MemoryStream();
               Image img = Properties.Resources.IMAGE_1565;
               img.Save(ms, ImageFormat.Bmp);
               bytes = ms.ToArray();

               //comp.LOGO = bytes;

               CompanyLogo_Pb.ImageProfile = Properties.Resources.IMAGE_1565;
            }
            else
            {
               var stream = new MemoryStream(comp.LOGO.ToArray());
               CompanyLogo_Pb.ImageProfile = Image.FromStream(stream);
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private Image GetImageActivity(string RqtpCode)
      {
         if (RqtpCode == "001")
            return System.CRM.Properties.Resources.IMAGE_1528;
         else if (RqtpCode == "002")
            return System.CRM.Properties.Resources.IMAGE_1531;
         else if (RqtpCode == "003")
            return System.CRM.Properties.Resources.IMAGE_1196;
         else if (RqtpCode == "004")
            return System.CRM.Properties.Resources.IMAGE_1532;
         else if (RqtpCode == "005")
            return System.CRM.Properties.Resources.IMAGE_1526;
         else if (RqtpCode == "006")
            return System.CRM.Properties.Resources.IMAGE_1530;
         else if (RqtpCode == "007")
            return System.CRM.Properties.Resources.IMAGE_1535;
         else if (RqtpCode == "008")
            return System.CRM.Properties.Resources.IMAGE_1533;
         else if (RqtpCode == "009")
            return System.CRM.Properties.Resources.IMAGE_1534;
         else if (RqtpCode == "010")
            return System.CRM.Properties.Resources.IMAGE_1527;
         else if (RqtpCode == "011")
            return System.CRM.Properties.Resources.IMAGE_1544;
         else if (RqtpCode == "012")
            return System.CRM.Properties.Resources.IMAGE_1573;

         return null;
      }

      private void ServiceProfile_Pb_Click(object sender, EventArgs e)
      {
         try
         {
            if (Tb_Master.SelectedTab == tp_001)
            {
               var rqst = RqstChngBs.Current as Data.VF_Request_ChangingResult;

               var serv = iCRM.Services.FirstOrDefault(s => s.FILE_NO == rqst.FILE_NO);

               if (serv.SRPB_TYPE_DNRM == "001")
               {
                  // Lead Info
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                             new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                             new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", rqst.FILE_NO))},
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch { }
               }
               else if (serv.SRPB_TYPE_DNRM == "002")
               {
                  // Contact
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", rqst.FILE_NO))},
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch { }
               }
            }
            else if(Tb_Master.SelectedTab == tp_003)
            {
               var rqst = AdvSearchRqstChngBs.Current as Data.VF_Advance_Search_Request_ChangingResult;

               var serv = iCRM.Services.FirstOrDefault(s => s.FILE_NO == rqst.FILE_NO);

               if (serv.SRPB_TYPE_DNRM == "001")
               {
                  // Lead Info
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                             new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                             new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", rqst.FILE_NO))},
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch { }
               }
               else if (serv.SRPB_TYPE_DNRM == "002")
               {
                  // Contact
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", rqst.FILE_NO))},
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch { }
               }
            }
         }
         catch { }
      }

      private void CompanyLogo_Pb_Click(object sender, EventArgs e)
      {
         try
         {
            if (Tb_Master.SelectedTab == tp_001)
            {
               var rqstchng = RqstChngBs.Current as Data.VF_Request_ChangingResult;
               if (rqstchng == null) return;

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 39 /* Execute Inf_Acnt_F */),                
                      new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Company", new XAttribute("code", rqstchng.COMP_CODE))},
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
            else if(Tb_Master.SelectedTab == tp_003)
            {
               var rqst = AdvSearchRqstChngBs.Current as Data.VF_Advance_Search_Request_ChangingResult;

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 39 /* Execute Inf_Acnt_F */),                
                      new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Company", new XAttribute("code", rqst.COMP_CODE))},
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void RqstRelatedService_Pb_Click(object sender, EventArgs e)
      {
         try
         {
            if (Tb_Master.SelectedTab == tp_001)
            {
               var rqstchng = RqstChngBs.Current as Data.VF_Request_ChangingResult;
               if (rqstchng == null) return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
                  {
                     Input =
                        new XElement("Request",
                           new XAttribute("rqtpcode", rqstchng.RQTP_CODE),
                           new XAttribute("rqid", rqstchng.RQID)
                        )
                  }
               );
            }
            else if(Tb_Master.SelectedTab == tp_003)
            {
               var rqst = AdvSearchRqstChngBs.Current as Data.VF_Advance_Search_Request_ChangingResult;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
                  {
                     Input =
                        new XElement("Request",
                           new XAttribute("rqtpcode", rqst.RQTP_CODE),
                           new XAttribute("rqid", rqst.RQID)
                        )
                  }
               );
            }
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstActn_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = RqstChngBs.Current as Data.VF_Request_ChangingResult;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("Request",
                              new XAttribute("rqtpcode", rqst.RQTP_CODE),
                              new XAttribute("rqid", rqst.RQID)
                           )
                     }
                  );
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

      private void RqstFind_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (string.IsNullOrEmpty(Rqid_Txt.Text.Trim())) return;
            if (UpRqst_Rb.Checked)
            {
               var query = iCRM.ExecuteQuery<long>(string.Format(@"WITH RqstTree AS
               (
                  SELECT R.RQID, R.RQST_RQID
                    FROM dbo.Request R
                   WHERE R.RQID = {0}

                   UNION ALL

                  SELECT Rl.RQID, Rl.Rqst_RQID
                    FROM dbo.Request RL, RqstTree
                   WHERE RqstTree.Rqst_Rqid = RL.Rqid
               )
               SELECT * 
                 FROM RqstTree
               ", Rqid_Txt.Text));

               RqstBs.DataSource =
                  from r in iCRM.Requests
                  where query.ToList().Contains(r.RQID)
                  select r;
            }
            else
            {
               var query = iCRM.ExecuteQuery<long>(string.Format(@"WITH RqstTree(Rqid) AS
               (
                  SELECT R.RQID
                    FROM dbo.Request R
                   WHERE R.RQID = {0}
    
                   UNION ALL
   
                  SELECT Rl.RQID
                    FROM dbo.Request RL, RqstTree
                   WHERE RqstTree.Rqid = RL.RQST_RQID
               )
               SELECT * 
                 FROM RqstTree
               ", Rqid_Txt.Text));

               RqstBs.DataSource =
                  from r in iCRM.Requests
                  where query.ToList().Contains(r.RQID)
                  select r;
            }
            
         }
         catch { Rqid_Txt.Text = ""; Rqid_Txt.Focus(); }
      }

      private void UpRqst_Rb_CheckedChanged(object sender, EventArgs e)
      {
         RqstFind_Butn_Click(null, null);
      }

      private void RqstBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;
            if (rqst == null) return;

            RqstInfo_Butn.ImageProfile = GetImageActivity(rqst.RQTP_CODE);

            ServBs.DataSource = 
               from s in iCRM.Services
               join rr in iCRM.Request_Rows on s.FILE_NO equals rr.SERV_FILE_NO
               where rr.RQST_RQID == rqst.RQID
               select s;

            

         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
      }

      private void ServBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;

            try
            {
               ServInfo_Butn.ImageVisiable = true;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", serv.FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();


               ServInfo_Butn.ImageProfile = bm;
            }
            catch
            {
               ServInfo_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
            }
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
      }

      private void RqroBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqro = RqroBs.Current as Data.Request_Row;
            var comp = rqro.Company;
            if (comp == null) { CompInfo_Butn.ImageVisiable = false; return; }

            if (comp.LOGO == null)
            {
               CompInfo_Butn.ImageVisiable = true;
               byte[] bytes = null;
               MemoryStream ms = new MemoryStream();
               Image img = Properties.Resources.IMAGE_1565;
               img.Save(ms, ImageFormat.Bmp);
               bytes = ms.ToArray();

               //comp.LOGO = bytes;

               CompInfo_Butn.ImageProfile = Properties.Resources.IMAGE_1565;
            }
            else
            {
               var stream = new MemoryStream(comp.LOGO.ToArray());
               CompInfo_Butn.ImageProfile = Image.FromStream(stream);
            }
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
      }

      private void ServInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            switch (serv.SRPB_TYPE_DNRM)
            {
               case "001":
                  _DefaultGateway.Gateway(
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                          {                  
                            new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                          })
                  );
                  break;
               case "002":
                  _DefaultGateway.Gateway(
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                          {                  
                            new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                            new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                          })
                  );
                  break;
            }
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
      }

      private void CompInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqro = RqroBs.Current as Data.Request_Row;
            if (rqro == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 39 /* Execute Inf_Acnt_F */),                
                   new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Company", new XAttribute("code", rqro.COMP_CODE))},
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void RqstInfo_Butn_Click(object sender, EventArgs e)
      {
         var rqst = RqstBs.Current as Data.Request;
         if (rqst == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
            {
               Input =
                  new XElement("Request",
                     new XAttribute("rqtpcode", rqst.RQTP_CODE),
                     new XAttribute("rqid", rqst.RQID)
                  )
            }
         );
      }

      private void rb_AdvSearchRqstChang_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            var rb = sender as RadioButton;
            srchtype = rb.Tag.ToString();
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            Execute_Query();
         }
      }

      private void AdvSearchRqstChngBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = AdvSearchRqstChngBs.Current as Data.VF_Advance_Search_Request_ChangingResult;

            AdvSearchRqstRelatedService_Pb.ImageVisiable = true;
            AdvSearchRqstRelatedService_Pb.ImageProfile = GetImageActivity(rqst.RQTP_CODE);

            try
            {
               AdvSearchServiceProfile_Pb.ImageVisiable = true;
               AdvSearchServiceProfile_Pb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", rqst.FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();


               AdvSearchServiceProfile_Pb.ImageProfile = bm;
            }
            catch
            {
               AdvSearchServiceProfile_Pb.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
            }

            var comp = iCRM.Companies.FirstOrDefault(c => c.CODE == rqst.COMP_CODE);
            if (comp.LOGO == null)
            {
               AdvSearchCompanyLogo_Pb.ImageVisiable = true;
               byte[] bytes = null;
               MemoryStream ms = new MemoryStream();
               Image img = Properties.Resources.IMAGE_1565;
               img.Save(ms, ImageFormat.Bmp);
               bytes = ms.ToArray();

               //comp.LOGO = bytes;

               AdvSearchCompanyLogo_Pb.ImageProfile = Properties.Resources.IMAGE_1565;
            }
            else
            {
               var stream = new MemoryStream(comp.LOGO.ToArray());
               AdvSearchCompanyLogo_Pb.ImageProfile = Image.FromStream(stream);
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void RqstActn003_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = AdvSearchRqstChngBs.Current as Data.VF_Advance_Search_Request_ChangingResult;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("Request",
                              new XAttribute("rqtpcode", rqst.RQTP_CODE),
                              new XAttribute("rqid", rqst.RQID)
                           )
                     }
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

      private void TextSearch_Txt_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  TextSearch_Txt.Text = "";
                  Execute_Query();
                  break;
               case 1:
                  Execute_Query();
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

      private void TextSearch_Txt_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 2:
                  // انتخاب برچسب
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", 
                        new List<Job>{
                           new Job(SendType.Self, 76 /* Execute Hst_Utag_F */),
                           new Job(SendType.SelfToUserInterface, "HST_UTAG_F", 10 /* Execute Hst_Utag_F */)
                           {                     
                              Input = 
                                 new XElement("Filter",
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        }
                     )
                  );
                  break;
               case 3:
                  // انتخاب ناحیه
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>{
                           new Job(SendType.Self, 77 /* Execute Hst_Urgn_F */),
                           new Job(SendType.SelfToUserInterface, "HST_URGN_F", 10 /* Execute Hst_Urgn_F */)
                           {                     
                              Input = 
                                 new XElement("Filter",
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        }
                     )
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }
   }
}
