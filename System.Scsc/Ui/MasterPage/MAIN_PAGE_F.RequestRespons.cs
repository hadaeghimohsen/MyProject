﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.MasterPage
{
   partial class MAIN_PAGE_F: ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private List<long?> Fga_Uclb_U;
      private string Crnt_User;

      //private bool requery = default(bool);

      private XElement x = null;
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
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               PostOnWall(job);
               break;
            case 09:
               TakeOnWall(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 40:
               SetToolTip(job);
               break;
            case 41:
               SetCardinDevice(job);
               break;
            case 42:
               ShowMessageOnLCD(job);
               break;
            case 43:
               DeviceControlFunction(job);
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
         else if(keyData == Keys.F2)
         {
            bbi_adm1butn_ItemClick(null, null);
         }
         else if (keyData == Keys.F3)
         {
            bbi_mbs1butn_ItemClick(null, null);
         }
         else if(keyData == Keys.F4)
         {
            bbi_flstbutn_ItemClick(null, null);
         }
         else if(keyData == Keys.F5)
         {
            bbi_oincbutn_ItemClick(null, null);
         }
         else if(keyData == Keys.F6)
         {
            bbi_exdsbutn_ItemClick(null, null);
         }
         else if(keyData == Keys.F9)
         {
            var fngrprnt = Microsoft.VisualBasic.Interaction.InputBox("EnrollNumber", "Input EnrollNumber");

            axCZKEM1_OnAttTransactionEx(fngrprnt.ToString(), 1, 1, 1, 2016, 05, 10, 09, 31, 50, 20);
         }
         else if (keyData == Keys.Escape)
         {            
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
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

         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
         Lbs_CrntUser.Text = Crnt_User = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         Tm_ShowTime_Tick(null, null);

         #region Package Item
         //rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self) ;
         _DefaultGateway.Gateway( GetHostInfo );

         var Pkac = iScsc.VF_AccessPackage(GetHostInfo.Output as XElement).ToList();

         //rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null || Pkac.Any(i => i.RWNO == Convert.ToInt32(rbe.Tag)) ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         /* Initial Sp_Barcode For Running */
         ///Start_BarCode();
         /* Initial Sp_FingerPrint For Running */
         //Start_FingerPrint();         

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
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }               
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
         //Stop_BarCode();
         //Stop_FingerPrint();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
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
         try { spc_desktop.Panel1.BackgroundImage = Image.FromFile(@".\Media\SubSys\Scsc\Desktop\Wallpaper\Wallpaper.jpg"); }
         catch { }
         //FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
         DaeatBs.DataSource = iScsc.D_AEATs;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void PostOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;

            if(InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Dock = DockStyle.Fill;
                  c.Visible = true;
                  spc_desktop.Panel1.Controls.Add(c);
                  spc_desktop.Panel1.Controls.SetChildIndex(c, 0);
               }), obj);
            else
            {
               obj.Dock = DockStyle.Fill;
               obj.Visible = true;
               spc_desktop.Panel1.Controls.Add(obj);
               spc_desktop.Panel1.Controls.SetChildIndex(obj, 0);
            }

         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void TakeOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => spc_desktop.Panel1.Controls.Remove(c)), obj);
            this.Focus();
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Successful; UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => spc_desktop.Panel1.Controls.Remove(c)), obj);
            this.Focus();
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         if (iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.NOTF_STAT == "002" && (s.NOTF_VIST_DATE.HasValue ? s.NOTF_VIST_DATE.Value : DateTime.Now.AddDays(-1)) != DateTime.Now).Count() >= 1)
         {
            var expday = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.NOTF_STAT == "002" && (s.NOTF_VIST_DATE.HasValue ? s.NOTF_VIST_DATE.Value : DateTime.Now.AddDays(-1)) != DateTime.Now).ToList();
            expday.ForEach(s => s.NOTF_VIST_DATE = DateTime.Now);
            iScsc.SubmitChanges();

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endfigh"), new XAttribute("expday", expday.Max(s => s.NOTF_EXP_DAY)))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }

         if(job.Input != null && (job.Input as XElement).Attribute("type").Value == "accesscontrol")
         {
            try
            {
               OnAttTransactionEx((job.Input as XElement).Attribute("fngrprnt").Value);
            }
            catch { }
         }


         if (job.Input != null && (job.Input as XElement).Attribute("type").Value == "gatecontrol")
         {
            if((job.Input as XElement).Attribute("gateactn").Value == "open")
            {
               Open_Gate();
            }
            else
            {
               Close_Gate();
            }
         }         

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void SetToolTip(Job job)
      {
         Lbs_Tooltip.Text = job.Input.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 41
      /// </summary>
      /// <param name="job"></param>
      private void SetCardinDevice(Job job)
      {
         if (bIsConnected == false)
         {
            MessageBox.Show("Please connect the device first!", "Error");
            return;
         }

         string EnrollNumber, CardNumber, NameDnrm;
         EnrollNumber = (job.Input as XElement).Attribute("enrollnumb").Value;
         CardNumber = (job.Input as XElement).Attribute("cardnumb").Value;
         NameDnrm = (job.Input as XElement).Attribute("namednrm").Value;

         int idwErrorCode = 0;

         bool bEnabled = true;
         int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
         Cursor = Cursors.WaitCursor;
         axCZKEM1.EnableDevice(iMachineNumber, false);
         
         axCZKEM1.SetStrCardNumber(CardNumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
         if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, EnrollNumber, NameDnrm, "", 0, bEnabled))//upload the user's information(card number included)
         {
            //MessageBox.Show("کارت شما در سیستم ثبت گردید");
            FngrPrnt_Txt.Text = EnrollNumber;
            if (CardNumb_Text.Text == "") CardNumb_Text.Text = "0";
            CardNumb_Text.Text = (Convert.ToInt64(CardNumb_Text.Text) + 1).ToString();
            //MessageBox.Show(string.Format("FngrPrnt {0} CardNumb {1} NameDnrm {2}", EnrollNumber, CardNumber, NameDnrm));
         }
         else
         {
            axCZKEM1.GetLastError(ref idwErrorCode);
            MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
         }
         axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
         axCZKEM1.EnableDevice(iMachineNumber, true);
         Cursor = Cursors.Default;
      }

      /// <summary>
      /// Code 42
      /// </summary>
      /// <param name="job"></param>
      private void ShowMessageOnLCD(Job job)
      {
         var message = (job.Input as XElement).Value;
         // بررسی اینکه آیا دستگاه انگشتی متصل می باشد
         if (bIsConnected)
         {
            axCZKEM1.ClearLCD();
            axCZKEM1.WriteLCD(0, 0, message);
            goto L_End;
         }

         // بررسی اینکه آیا دستگاه بارکد خوان متصل می باشد
         //if (Sp_Barcode.IsOpen)
         //{
         //   Sp_Barcode.WriteLine(message);
         //   goto L_End;
         //}

         L_End:
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 43
      /// </summary>
      /// <param name="job"></param>
      private void DeviceControlFunction(Job job)
      {
         var xinput = job.Input as XElement;
         // بررسی اینکه آیا دستگاه انگشتی متصل می باشد
         if (bIsConnected)
         {
            var result = false;
            int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
            switch(xinput.Attribute("functype").Value)
            {
               case "5.5.1": // ClearAdministrators
                  result = axCZKEM1.ClearAdministrators(iMachineNumber);
                  break;
               case "5.5.2":
                  
                  break;
            }

            if (result) MessageBox.Show(this, "عملیات با موفقیت انجام شد", "نتجیه عملیات", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show(this, "عملیات با شکست مواجه شد", "نتجیه عملیات", MessageBoxButtons.OK, MessageBoxIcon.Error);

            goto L_End;
         }         

         L_End:
         job.Status = StatusType.Successful;
      }
   }
}

