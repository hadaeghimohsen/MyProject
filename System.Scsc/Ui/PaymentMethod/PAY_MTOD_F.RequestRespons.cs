using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.PaymentMethod
{
   partial class PAY_MTOD_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string CallerForm;
      private string TabFocued;

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
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               Actn_CalF_P(job);
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

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            if(CallerForm != null && CallerForm != "")
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", CallerForm, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) 
                  { 
                     Input = 
                        new XElement("Process",
                           new XAttribute("type", "refresh"),
                           new XAttribute("tabfocued", TabFocued ?? "")
                        )
                  }
               );
            }

            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            
         }
         else if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            
         }
         else if (keyData == Keys.F8)
         {
            
         }
         else if (keyData == Keys.F5)
         {
            
         }
         else if (keyData == Keys.F3)
         {
          
         }
         else if (keyData == Keys.F10)
         {
          
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
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         CallerForm = "";
         PymtBs1.DataSource = job.Input as Data.Payment;
         var p = PymtBs1.Current as Data.Payment;

         // 1396/08/19 * اگر درخواست به غسر از تمدید بود گزینه های تخفیف مبلغ مابه التفاوت باید خاموش باشد
         SaveDifferenceAmnt_Butn.Enabled = CalcDiffAmnt_Cb.Enabled = p.Request.RQTP_CODE == "009";

         CashBs1.DataSource = iScsc.Cashes;
         ExpnBs3.DataSource = 
            iScsc.Expenses.Where(
               e => 
                  p.Request.RQTP_CODE == e.Expense_Type.Request_Requester.RQTP_CODE &&
                  p.Request.RQTT_CODE == e.Expense_Type.Request_Requester.RQTT_CODE &&
                  e.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" &&
                  e.EXPN_STAT == "002" &&
                  e.ADD_QUTS == "002"
            );
         DRcmtBs1.DataSource = iScsc.D_RCMTs;
         DPydsBs2.DataSource = iScsc.D_PYDS;
         DActvBs2.DataSource = iScsc.D_ACTVs;
         DCktpBs4.DataSource = iScsc.D_CKTPs;
         DEBT_DNRMTextEdit.EditValue = p.Request.Request_Rows.FirstOrDefault().Fighter.DEBT_DNRM;
         if (p.Request.Request_Rows.FirstOrDefault().Fighter.DEBT_DNRM >= 0)
         {
            CashByDeposit_Txt002.Properties.ReadOnly = true;
            dEBT_DNRMLabel.Text = "مبلغ بدهی :";
            GB_DebtStat.Text = "مشترک بدهکار یا هیچ سپرده ای ندارد و نمی توان از تخفیف سپرده استفاده کنید";
            AddDebtDiscount_Butn002.Enabled = false;
         }
         else
         {
            CashByDeposit_Txt002.Properties.ReadOnly = false;
            dEBT_DNRMLabel.Text = "مبلغ بستانکاری :";
            GB_DebtStat.Text = "مشترک بستانکار می باشد، می توانید از تخفیف سپرده استفاده کنید";
            AddDebtDiscount_Butn002.Enabled = true;
         }

         var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");
         if(regl.AMNT_TYPE == "001")
         {
            AmntType1_Lb.Text = AmntType2_Lb.Text = AmntType3_Lb.Text = AmntType4_Lb.Text = AmntType5_Lb.Text = AmntType6_Lb.Text = "( ریال )";
         }
         else
         {
            AmntType1_Lb.Text = AmntType2_Lb.Text = AmntType3_Lb.Text = AmntType4_Lb.Text = AmntType5_Lb.Text = AmntType6_Lb.Text = "( تومان )";
         }

         Execute_Query();

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P (Job job)
      {
         var xinput = job.Input as XElement;
         CallerForm = xinput.Attribute("callerform").Value;
         if(xinput.Attribute("tabfocued")!=null)
            TabFocued = xinput.Attribute("tabfocued").Value;
         job.Status = StatusType.Successful;
      }
   }
}
