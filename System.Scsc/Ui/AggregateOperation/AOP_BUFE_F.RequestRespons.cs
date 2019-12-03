using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.AggregateOperation
{
   partial class AOP_BUFE_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      //private long Rqid, FileNo;
      private bool isFirstLoaded = false;
      private XElement HostNameInfo;
      private string CurrentUser;

      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
               CheckSecurity(job);
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               LoadDataSource(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 20:
               Pay_Oprt_F(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if(keyData == Keys.F5)
         {
            try
            {
               var crnt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
               if (crnt.CELL_PHON != "" && iScsc.Fighters.Any(f => f.FILE_NO != crnt.FIGH_FILE_NO && f.CELL_PHON_DNRM != crnt.CELL_PHON))
               {
                  var lastInfo = (from p in iScsc.Fighter_Publics
                                  join f in iScsc.Fighters on p.FIGH_FILE_NO equals f.FILE_NO
                                  where p.RECT_CODE == "004"
                                     && p.RWNO == f.FGPB_RWNO_DNRM
                                     && f.FILE_NO == crnt.FIGH_FILE_NO
                                  select p).ToList().FirstOrDefault();
                  lastInfo.CELL_PHON = crnt.CELL_PHON;

                  iScsc.SubmitChanges();
                  MessageBox.Show("شماره تلفن ثبت شد");
               }
            }
            catch(Exception exc)
            {
               MessageBox.Show(exc.Message);
            }
         }
         else if (keyData == Keys.Enter)
         {            
            SendKeys.Send("{TAB}");
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
         CurrentUser = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
         Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>206</Privilege><Sub_Sys>5</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 206 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithJob); 
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if (!isFirstLoaded)
         {
            DRcmtBs1.DataSource = iScsc.D_RCMTs.Where(c => c.VALU == "001" || c.VALU == "003" || c.VALU == "005");
            isFirstLoaded = true;
         }
         //FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;

         if (xinput.Attribute("stat") != null)
            stat = xinput.Attribute("stat").Value;
         else
            stat = null;

         if (xinput.Attribute("macadrs") != null)
            macadrs = xinput.Attribute("macadrs").Value;
         else
            macadrs = null;

         if (xinput.Attribute("fngrprnt") != null)
            fngrprnt = xinput.Attribute("fngrprnt").Value;
         else
            fngrprnt = null;

         Execute_Query();

         #region Set FileNo In Lookup
         if (fngrprnt != null)
            Figh_Lov.EditValue = FighBs.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FNGR_PRNT_DNRM == fngrprnt && f.CONF_STAT == "002").FILE_NO;
         #endregion

         #region Device Input
         if (stat != null)
         {            
            if (!AgopBs1.List.OfType<Data.Aggregation_Operation>().Any(a => a.FROM_DATE.Value.Date == DateTime.Now.Date))
            {
               AgopBs1.AddNew();
               var agop = AgopBs1.Current as Data.Aggregation_Operation;
               agop.FROM_DATE = agop.TO_DATE = DateTime.Now;
               Edit_Butn_Click(null, null);
            }

            AgopBs1.Position = AgopBs1.IndexOf(AgopBs1.List.OfType<Data.Aggregation_Operation>().FirstOrDefault(a => a.FROM_DATE.Value.Date == DateTime.Now.Date));
            var expn = ExpnDeskBs1.List.OfType<Data.Expense>().FirstOrDefault(e => e.RELY_CMND == macadrs);

            // 1397/08/07 * اگر دستگاه به هزینه ای به عنوان میز متصل نباشد پیام مک آدرس دستگاه رو نشان میدهیم
            /// و بعد کار را به اتمام میرسانیم
            if(expn == null)
            {
               Clipboard.SetText(macadrs);
               MessageBox.Show(this, 
                  string.Format(
                     "دستگاه جدیدی قصد اضافه شدن را دارد آدرس آن در حافظه ذخیره شده لطفا بررسی و اقدام کنید" + 
                     "\n\r" +
                     "\n\r" +
                     "Mac Address : {0}", 
                     macadrs
                  ), 
                  "تشخیص دستگاه جدید");
               return;
            }

            if(AodtBs1.Count == 0)
            { 
               if(stat == "START")
               {
                  ExpnDesk_GridLookUpEdit.EditValue = expn.CODE;
                  OpenDesk_Butn_Click(null, null);
               }
            }
            else
            {
               switch (stat)
               {
                  case "START":
                     if( 
                         (fngrprnt != null && AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(a => a.EXPN_CODE == expn.CODE && a.STAT == "001" && a.Fighter.FNGR_PRNT_DNRM == fngrprnt)) ||
                         (fngrprnt == null && AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(a => a.EXPN_CODE == expn.CODE && a.STAT == "001"))
                       )
                     {
                        int lastPosition = AodtBs1.Position;
                        var filterType = AllRcrd_Lbl.Tag;
                        Lbl_Click(AllRcrd_Lbl, null);                        
                        AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.EXPN_CODE == expn.CODE && a.STAT == "001"));
                        var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
                        aodt.END_TIME = null;
                        CalcDesk_Butn_Click(null, null);
                        AllRcrd_Lbl.Tag = filterType;
                        Lbl_Click(InfoParm_Gb.Controls.OfType<LabelControl>().FirstOrDefault(l => l.Name == AllRcrd_Lbl.Tag.ToString()), null);
                        AodtBs1.Position = lastPosition;
                     }
                     else
                     {
                        bool visited = false;
                        if (fngrprnt != null && ExpnDesk_GridLookUpEdit.Properties.Buttons[1].Tag.ToString() == "auto")
                        {
                           visited = true;
                           ExpnDesk_GridLookUpEdit_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(ExpnDesk_GridLookUpEdit.Properties.Buttons[1]));
                        }

                        ExpnDesk_GridLookUpEdit.EditValue = expn.CODE;
                        OpenDesk_Butn_Click(null, null);
                        
                        if(visited)
                        {
                           visited = false;
                           ExpnDesk_GridLookUpEdit_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(ExpnDesk_GridLookUpEdit.Properties.Buttons[1]));
                        }
                     }
                     break;
                  case "STOP":
                     if (
                           (fngrprnt != null && (AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(a => a.EXPN_CODE == expn.CODE && a.STAT == "001" && a.Fighter.FNGR_PRNT_DNRM == fngrprnt))) ||
                           (fngrprnt == null && (AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().Any(a => a.EXPN_CODE == expn.CODE && a.STAT == "001")))
                        )
                     {
                        if(fngrprnt == null)
                           AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.EXPN_CODE == expn.CODE && a.STAT == "001"));
                        else
                           AodtBs1.Position = AodtBs1.IndexOf(AodtBs1.List.OfType<Data.Aggregation_Operation_Detail>().FirstOrDefault(a => a.EXPN_CODE == expn.CODE && a.STAT == "001" && a.Fighter.FNGR_PRNT_DNRM == fngrprnt));
                        DeskClose_Butn_Click(null, null);
                     }
                     break;
               }
            }
         }
         #endregion

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void Pay_Oprt_F(Job job)
      {
         try
         {
            XElement RcevXData = job.Input as XElement;

            var aodt = AodtBs1.Current as Data.Aggregation_Operation_Detail;
            if (aodt == null) return;

            var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            var amnt = Convert.ToInt64(RcevXData.Attribute("amnt").Value);
            var termno = RcevXData.Attribute("termno").Value;
            var tranno = RcevXData.Attribute("tranno").Value;
            var cardno = RcevXData.Attribute("cardno").Value;
            var flowno = RcevXData.Attribute("flowno").Value;
            var refno = RcevXData.Attribute("refno").Value;
            var actndate = RcevXData.Attribute("actndate").Value;

            if (regl.AMNT_TYPE == "002")
               amnt /= 10;

            iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "CheckoutWithPOS4Agop"),
                  new XElement("Insert",
                     new XElement("Payment_Method",
                        new XAttribute("apdtagopcode", aodt.AGOP_CODE),
                        new XAttribute("apdtrwno", aodt.RWNO),
                        new XAttribute("amnt", amnt),
                        new XAttribute("termno", termno),
                        new XAttribute("tranno", tranno),
                        new XAttribute("cardno", cardno),
                        new XAttribute("flowno", flowno),
                        new XAttribute("refno", refno),
                        new XAttribute("actndate", actndate)
                     )
                  )
               )
            );

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
      }

   }
}
