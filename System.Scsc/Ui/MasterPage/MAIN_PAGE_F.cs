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
using System.Net.NetworkInformation;
using System.Net;
using System.Scsc.ExtCode;
using DevExpress.XtraBars;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System.Runtime.InteropServices;
using libzkfpcsharp;
using System.Threading;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Text.RegularExpressions;
using WebSocket4Net;
using SuperSocket.WebSocket;
using SuperSocket.SocketBase;
using System.Net.Sockets;
using EventBasedTCP;
using System.Globalization;
using DevExpress.XtraEditors;


namespace System.Scsc.Ui.MasterPage
{
   public partial class MAIN_PAGE_F : UserControl
   {
      public MAIN_PAGE_F()
      {
         InitializeComponent();         
      }

      private string attnsystype = "002";
      private IEnumerable<Data.V_Setting> _settings;
      private long _doActionStep = 0;
      private bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);

         int _compa = CompaBs.Position;
         int _note = NoteBs.Position;
         int _exdv = ExdvBs.Position;
         int _dvip = DVipBs.Position;

         CompaBs.DataSource = iScsc.Computer_Actions.Where(c => c.Dressers.Any());
         NoteBs.DataSource = iScsc.Notes;
         ExdvBs.DataSource = iScsc.External_Devices.Where(ed => ed.STAT == "002");
         DVipBs.DataSource = iScsc.Dresser_Vip_Fighters.Where(dv => dv.STAT == "002");

         CompaBs.Position = _compa;
         NoteBs.Position = _note;
         ExdvBs.Position = _exdv;
         DVipBs.Position = _dvip;

         requery = false;
      }

      private bool CheckInternetConnection()
      {
         try
         {
            Ping ping = new Ping();
            //PingReply pingStatus = ping.Send("google.com");
            PingReply pingStatus = ping.Send(new IPAddress(new byte[] { 8, 8, 8, 8 }), 2000);
            if (pingStatus.Status == IPStatus.Success)
               return true;
            return false;
         }
         catch { return false; }
      }

      private DevExpress.Utils.SuperToolTip SuperToolTipAttnButn(XElement xdata)
      {
         try
         {
            DevExpress.Utils.SuperToolTip SuperToolTips = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem HeaderAttnButn_Tooltip = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem DetialAttnButn_Tooltip = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipTitleItem FooterAttnButn_Tooltip = new DevExpress.Utils.ToolTipTitleItem();

            if (xdata.Attribute("device").Value == "Attn")
            {
               /*
                * <System device="Attn" ip="192.168.100.203">
                *    <Computer ip="192.168.100.204" name="Mohsen-Lt">
                * </System>
                */
               HeaderAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
               HeaderAttnButn_Tooltip.Appearance.Options.UseImage = true;
               HeaderAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
               HeaderAttnButn_Tooltip.Text = string.Format("IP : {0}\r\nوضعیت : {1}", xdata.Attribute("ip").Value, Convert.ToBoolean(xdata.Attribute("stat").Value) ? "فعال" : "غیرفعال");
               DetialAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1067;
               DetialAttnButn_Tooltip.Appearance.Options.UseImage = true;
               DetialAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1067;
               DetialAttnButn_Tooltip.LeftIndent = 6;
               DetialAttnButn_Tooltip.Text = string.Format("IP : {0}\r\nComputer Name : {1}", xdata.Element("Computer").Attribute("ip").Value, xdata.Element("Computer").Attribute("name").Value);
               FooterAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1116;
               FooterAttnButn_Tooltip.Appearance.Options.UseImage = true;
               FooterAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1116;
               FooterAttnButn_Tooltip.LeftIndent = 6;
               FooterAttnButn_Tooltip.Text = string.Format("تیم پشتیبانی\r\nوضعیت اینترنت : {0}", CheckInternetConnection() ? "قعال" : "غیرفعال");

               SuperToolTips.Items.Add(HeaderAttnButn_Tooltip);
               SuperToolTips.Items.Add(DetialAttnButn_Tooltip);
               SuperToolTips.Items.Add(FooterAttnButn_Tooltip);
            }
            else if (xdata.Attribute("device").Value == "Gate")
            {
               HeaderAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1561;
               HeaderAttnButn_Tooltip.Appearance.Options.UseImage = true;
               HeaderAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1561;
               HeaderAttnButn_Tooltip.Text = string.Format("وضعیت دستگاه گیت : {0}", xdata.Attribute("stat").Value == "001" ? "غیرفعال" : "فعال");
               //DetialAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1067;
               //DetialAttnButn_Tooltip.Appearance.Options.UseImage = true;
               //DetialAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1067;
               //DetialAttnButn_Tooltip.LeftIndent = 6;
               //DetialAttnButn_Tooltip.Text = "ارتباط متصل";
               //FooterAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1116;
               //FooterAttnButn_Tooltip.Appearance.Options.UseImage = true;
               //FooterAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1116;
               //FooterAttnButn_Tooltip.LeftIndent = 6;
               //FooterAttnButn_Tooltip.Text = string.Format("تیم پشتیبانی\r\nوضعیت اینترنت : {0}", CheckInternetConnection() ? "قعال" : "غیرفعال");

               SuperToolTips.Items.Add(HeaderAttnButn_Tooltip);
               //SuperToolTips.Items.Add(DetialAttnButn_Tooltip);
               //SuperToolTips.Items.Add(FooterAttnButn_Tooltip);
            }
            else if (xdata.Attribute("device").Value == "ExpnExtr")
            {
               HeaderAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1607;
               HeaderAttnButn_Tooltip.Appearance.Options.UseImage = true;
               HeaderAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1607;
               HeaderAttnButn_Tooltip.Text = string.Format("وضعیت دستگاه : {0}", Convert.ToBoolean(xdata.Attribute("stat").Value) ? "فعال" : "غیرفعال");

               SuperToolTips.Items.Add(HeaderAttnButn_Tooltip);
            }
            else if (xdata.Attribute("device").Value == "AttnDvic")
            {
               HeaderAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1607;
               HeaderAttnButn_Tooltip.Appearance.Options.UseImage = true;
               HeaderAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1607;
               HeaderAttnButn_Tooltip.Text = xdata.Attribute("desc").Value;

               SuperToolTips.Items.Add(HeaderAttnButn_Tooltip);
            }
            else if (xdata.Attribute("device").Value == "TlgrmBot")
            {
               HeaderAttnButn_Tooltip.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1616;
               HeaderAttnButn_Tooltip.Appearance.Options.UseImage = true;
               HeaderAttnButn_Tooltip.Image = global::System.Scsc.Properties.Resources.IMAGE_1616;
               HeaderAttnButn_Tooltip.Text = xdata.Attribute("desc").Value;

               SuperToolTips.Items.Add(HeaderAttnButn_Tooltip);
            }


            return SuperToolTips;
         }
         catch { return null; }
      }

      #region BarCode
      void Start_BarCode()
      {
         try
         {
            /*if (iScsc.Settings.Count(s => Fga_Uclb_U.Contains(s.CLUB_CODE)) > 1)
            {
               Tsp_AttnSys.Text = "***";
               return;
            }*/

            var barCodeSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();

            if (barCodeSetting == null) return;

            // 1402/04/02 * Ribbon Menu and Left Side
            if (barCodeSetting.SHOW_RBON_MNUI == "002")
               ShowRbonMenu_Btn_Click(null, null);
            //if (barCodeSetting.SHOW_SLID_MNUI == "001")
               //ShowMain_Btn_Click(null, null);

            if (barCodeSetting.ATTN_SYST_TYPE.NotIn("001", "004")) { Sp_Barcode.PortName = "NoPort"; return; }

            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;

            Sp_Barcode.PortName = barCodeSetting.COMM_PORT_NAME;
            Sp_Barcode.BaudRate = (int)barCodeSetting.BAND_RATE;
            Sp_Barcode.Handshake = Handshake.None;
            Sp_Barcode.Open();            

            if (Sp_Barcode.IsOpen)
            {
               AttnType_Lov.EditValue = "001";
               this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1212;
               Tsp_AttnSys.Text = "سیستم بارکد خوان فعال";
               Tsp_AttnSys.ForeColor = Color.Green;
               AttendanceSystemAlert_Butn.Tag = barCodeSetting.ATTN_SYST_TYPE;
            }
            else
            {
               AttnType_Lov.EditValue = null;
               this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
               Tsp_AttnSys.Text = "سیستم بارکد خوان غیرفعال";
               Tsp_AttnSys.ForeColor = Color.Red;
            }
         }
         catch
         {
            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            //MessageBox.Show(ex.Message);
            Tsp_AttnSys.Text = "سیستم بارکد خوان غیرفعال";
            Tsp_AttnSys.ForeColor = Color.Red;
         }
      }

      void Stop_BarCode()
      {
         try
         {
            if (Sp_Barcode.IsOpen)
            {
               Sp_Barcode.Close();
               Tsp_AttnSys.Text = "سیستم بارکد خوان غیرفعال";
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      string oldenrollnumber = "";
      private void Sp_Barcode_DataReceived(object sender, IO.Ports.SerialDataReceivedEventArgs e)
      {
         try
         {
            new Thread(AlarmShow).Start();
            iScsc = new Data.iScscDataContext(ConnectionString);
            var barCodeSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
            var enrollNumber = Sp_Barcode.ReadLine();
            
            if (enrollNumber.IndexOf('\r') != -1)
               enrollNumber = enrollNumber.Substring(0, enrollNumber.IndexOf('\r')).ToUpper();
            else
               enrollNumber = enrollNumber.ToUpper();

            if (enrollNumber.IndexOf(' ') != -1)
               enrollNumber = enrollNumber.Replace(" ", "");

            if (enrollNumber.IndexOf('') != -1)
               enrollNumber = enrollNumber.Replace("", "");

            if (enrollNumber.IndexOf('') != -1)
               enrollNumber = enrollNumber.Replace("", "");

            enrollNumber = enrollNumber.Trim();

            // 1402/11/16 * Send Signal to receive data
            GetValuDataFromExtrDev(null, enrollNumber);

            //enrollNumber = Regex.Replace(enrollNumber, "[^a-zA-Z][^0-9]", "");

            //1397/09/28 * مشخص کردن نوع ثبت حضور و غیاب که با دستگاه یا دستی
            attnsystype = "002";

            // 1397/05/08 * بررسی اینکه آیا در داده ورودی علاما غیرمجاز وجود دارد
            if (enrollNumber.Like("%?%"))
               return;

            if (barCodeSetting.CLER_ZERO == "002")
               enrollNumber = enrollNumber.TrimStart('0');

            if (barCodeSetting.BAR_CODE_DATA_TYPE == "001")
            {
               try
               {
                  var figh = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == Convert.ToInt64(enrollNumber));
                  if (figh != null)
                  {
                     enrollNumber = figh.FNGR_PRNT_DNRM;
                     if (enrollNumber.Length == 0)
                     {
                        MessageBox.Show(string.Format("{0} فاقد کد انگشتی کارت می باشد لطفا جهت انتصاب کد انگشتی برای سیستم حضور و غیاب اقدام فرمایید", figh.NAME_DNRM));
                        oldenrollnumber = "";
                        return;
                     }
                  }
               }
               catch { MessageBox.Show("داده خوانده شده از دستگاه قابل تبدیل به عددی را ندارد"); }
            }                       

            // 1398/10/03 * سیستم کدینگ کارت برای نرم افزار برای مشتریان چموش
            //if(iScsc.Card_Datasources.Any())
            //{
            //   if(!iScsc.Card_Datasources.Any(cd => cd.FNGR_PRNT == enrollNumber))
            //   {
            //      AlarmShow();
            //      return;
            //   }
            //}

            if (InvokeRequired)
               Invoke(new Action(() => axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1)));
            else 
               axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1);
         }
         catch { BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Purple; }
      }

      private void Sp_GateAttn_DataReceived(object sender, IO.Ports.SerialDataReceivedEventArgs e)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var barCodeSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
            var enrollNumber = Sp_GateAttn.ReadLine();
            
            if (enrollNumber.IndexOf('\r') != -1)
               enrollNumber = enrollNumber.Substring(0, enrollNumber.IndexOf('\r')).ToUpper();
            else
               enrollNumber = enrollNumber.ToUpper();

            if (enrollNumber.IndexOf(' ') != -1)
               enrollNumber = enrollNumber.Replace(" ", "");

            //enrollNumber = Regex.Replace(enrollNumber, "[^a-zA-Z][^0-9]", "");

            //1397/09/28 * مشخص کردن نوع ثبت حضور و غیاب که با دستگاه یا دستی
            attnsystype = "002";


            // 1397/05/08 * بررسی اینکه آیا در داده ورودی علاما غیرمجاز وجود دارد
            if (enrollNumber.Like("%?%"))
            {
               //FngrPrnt_Txt.BackColor = SystemColors.Info;
               //FngrPrnt_Txt.Properties.NullText = FngrPrnt_Txt.Properties.NullValuePrompt = "خطا در دریافت اطلاعات";
               return;
            }
            else
            {
               //FngrPrnt_Txt.BackColor = Color.White;
               //FngrPrnt_Txt.Properties.NullText = FngrPrnt_Txt.Properties.NullValuePrompt = "شماره ملی یا کد انگشتی";
            }

            if (barCodeSetting.CLER_ZERO == "002")
               enrollNumber = enrollNumber.TrimStart('0');
            //if (enrollNumber == oldenrollnumber && MessageBox.Show(this, "کارت مشترک دوباره قرار گرفته آیا می خواهید دوباره مورد بررسی قرار گیرد؟", "تکرار قرار گیری کارت مشترک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            //   return;
            //oldenrollnumber = enrollNumber;

            // 1402/11/16 * Send Signal to receive data
            GetValuDataFromExtrDev(null, enrollNumber);

            if (barCodeSetting.BAR_CODE_DATA_TYPE == "001")
            {
               //var enrollnumb = Convert.ToInt64(enrollNumber);
               //if (enrollnumb < 0)
               //{
               //   enrollnumb = 4294967296 + enrollnumb;
               //}
               try
               {
                  var figh = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == Convert.ToInt64(enrollNumber));
                  if (figh != null)
                  {
                     enrollNumber = figh.FNGR_PRNT_DNRM;
                     if (enrollNumber.Length == 0)
                     {
                        MessageBox.Show(string.Format("{0} فاقد کد انگشتی کارت می باشد لطفا جهت انتصاب کد انگشتی برای سیستم حضور و غیاب اقدام فرمایید", figh.NAME_DNRM));
                        oldenrollnumber = "";
                        return;
                     }
                  }
               }
               catch { MessageBox.Show("داده خوانده شده از دستگاه قابل تبدیل به عددی را ندارد"); }
            }

            //var temp = Convert.ToInt64(enrollNumber);
            //if (temp < 0)
            //{
            //   temp = 4294967296 + temp;
            //   enrollNumber = temp.ToString();
            //}

            //MessageBox.Show(enrollNumber);

            // 1398/10/03 * سیستم کدینگ کارت برای نرم افزار برای مشتریان چموش
            if (iScsc.Card_Datasources.Any())
            {
               if (!iScsc.Card_Datasources.Any(cd => cd.FNGR_PRNT == enrollNumber))
               {
                  AlarmShow();
                  return;
               }
            }

            if (InvokeRequired)
               Invoke(new Action(() => axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1)));
            else
               axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1);
         }
         catch { BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Purple; }
      }
      #endregion

      #region GateAttn
      private void Start_GateAttn()
      {
         try
         {
            var gateAttnSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();

            if (gateAttnSetting == null) return;

            if (gateAttnSetting.GATE_ATTN_STAT == "001") { Sp_GateAttn.PortName = "NoPort"; return; }

            Sp_GateAttn.PortName = gateAttnSetting.GATE_COMM_PORT_NAME;
            Sp_GateAttn.BaudRate = (int)gateAttnSetting.GATE_BAND_RATE;
            GateAttn_Butn.Tag = gateAttnSetting;
            Sp_GateAttn.Open();

            if (Sp_GateAttn.IsOpen)
            {
               AttnType_Lov.EditValue = "001";
               GateAttn_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1561;               
            }
            else
            {
               AttnType_Lov.EditValue = null;
               GateAttn_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;               
            }            
         }
         catch
         {
            GateAttn_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;               
         }
         finally
         {
            GateAttn_Butn.SuperTip =
               SuperToolTipAttnButn(
                  new XElement("System",
                     new XAttribute("device", "Gate"),
                     new XAttribute("stat", Fp1DevIsConnected)
                  )
               );
         }
      }

      IQueryable<Data.External_Device> _gatesDevice;
      IQueryable<Data.External_Device> _readersDevice;
      private void Stop_GateAttn()
      {
         try
         {
            if (Sp_GateAttn.IsOpen)
            {
               Sp_GateAttn.Close();
               GateAttn_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;               
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Open_Gate(XElement xinput)
      {
         try
         {
            try
            {
               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.YellowGreen;
               GateAttn_Butn.Image = Properties.Resources.IMAGE_1641;
               if (GateAttn_Butn.Tag != null)
               {
                  // Open This Gate
                  var gateAttnStng = GateAttn_Butn.Tag as Data.Setting;
                  if (gateAttnStng == null) return;

                  GateAttn_Butn.Image = Properties.Resources.IMAGE_1641;
                  if (gateAttnStng.GATE_ENTR_OPEN == "001") return;
                  Sp_GateAttn.Write("in");
               }
            }
            catch { }

            // در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
            var _listIPHost = xHost.Descendants("IP").Select(ip => ip.Value).ToList();

            // 1400/01/12 * اگر گیت به صورت دستی باز شود
            if (xinput.Attribute("fngrprnt") == null)
            {
               #region Manual Set Open Gate Connected To Current PC
               if (_gatesDevice == null)
                  _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
               if (_gatesDevice != null && _gatesDevice.Any())
               {
                  _gatesDevice.ToList()
                     .ForEach(g =>
                        OprtExtDev(
                           new XElement("MainPage",
                              new XAttribute("type", "extdev"),
                              new XAttribute("devtype", "006"),
                              new XAttribute("contype", "002"),
                              new XAttribute("cmdtype", "open"),
                              new XAttribute("ip", g.IP_ADRS),
                              new XAttribute("sendport", g.PORT_SEND)
                           )
                        )
                     );

                  // 1400/01/12 * Reset All
                  _gatesDevice = null;
               }
               #endregion
            }
            else // اگر گیت به صورت اتومات باز شود مشتری حضور و غیاب کرده باشد
            {
               // در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
               // 1400/01/11
               // در اینجا باید مشخص کنیم که ایا گیت ها بر اساس رشته های مختلف باید واکنش نشان دهند یا خیر               
               if (_gatesDevice == null)
               {
                  // 1400/01/12
                  // یدا کردن گیت هایی که تردد گروه های مختلف را چک میکنند
                  _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && d.STAT == "002" && d.MTOD_CODE == null && d.External_Device_Link_Methods.Any(em => em.MTOD_CODE == xinput.Attribute("mtodcode").Value.ToInt64() && em.STAT == "002"));
                  
                  // 1400/01/12
                  // اگر گیت مانند باشگاه نیایش باشد همان مسیر قبلی را برایش انجام میدهیم
                  if (_gatesDevice != null && _gatesDevice.Count() == 0)
                     _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
               }
               
               if (_gatesDevice != null && _gatesDevice.Any())
               {
                  //MessageBox.Show(string.Format("{0} - {1}", _gatesDevice.Count(), string.Join(",", _gatesDevice.Select(g => g.IP_ADRS))));
                  _gatesDevice.ToList()
                     .ForEach(g =>
                        OprtExtDev(
                           new XElement("MainPage",
                              new XAttribute("type", "extdev"),
                              new XAttribute("devtype", "006"),
                              new XAttribute("contype", "002"),
                              new XAttribute("cmdtype", "open"),
                              new XAttribute("ip", g.IP_ADRS),
                              new XAttribute("sendport", g.PORT_SEND)
                           )
                        )
                     );

                  // 1400/01/12 * Reset All
                  _gatesDevice = null;
               }
            }

            if(_readersDevice == null)
               _readersDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "002" && d.DEV_TYPE == "001" && d.DEV_CON == "002" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002");
            if (_readersDevice != null && _readersDevice.Any())
            {
               //var enddate = Convert.ToDateTime(xinput.Attribute("enddate").Value);
               //var numbattnmont = Convert.ToInt32(xinput.Attribute("numbattnmont").Value);
               //var sumattnmont = Convert.ToInt32(xinput.Attribute("sumattnmont").Value);
               //var debt = Convert.ToInt64(xinput.Attribute("debt").Value);
               //var fngrprnt = xinput.Attribute("fngrprnt").Value;

               _readersDevice.ToList()
                  .ForEach(r =>
                  {
                     SendCommandDevExpn("in", r.DEV_NAME, "");
                     //var pc = new PersianCalendar();
                     //if(numbattnmont != 0)
                     //{
                     //   SendCommandDevExpn("in:" + 
                     //      string.Format("{0}/{1}/{2}-{3}&{4:n0}", /*pc.GetYear(enddate)*/ 99, pc.GetMonth(enddate).ToString().PadLeft(2, '0'), pc.GetDayOfMonth(enddate).ToString().PadLeft(2, '0'), sumattnmont.ToString().PadLeft(3, ' '), debt.ToString().PadLeft(16, ' ')), r.DEV_NAME, fngrprnt
                     //   );
                     //}
                     //else
                     //{
                     //   SendCommandDevExpn("in:" +
                     //      string.Format("{0}/{1}/{2}-   &{3:n0}", /*pc.GetYear(enddate)*/ 99, pc.GetMonth(enddate).ToString().PadLeft(2, '0'), pc.GetDayOfMonth(enddate).ToString().PadLeft(2, '0'), debt.ToString().PadLeft(16, ' ')), r.DEV_NAME, fngrprnt
                     //   );
                     //}

                     //SendCommandDevExpn(
                     //   "df:" + "WellCome".PadLeft(13, ' ') + 
                     //   "&" + "Genetic Gym".PadLeft(16, ' '), r.DEV_NAME, ""
                     //);
                  }
               );               
            }
            //MessageBox.Show("Gate is Open");
         }catch(Exception ){}
         finally
         {
            try
            {
               //GateAttn_Butn.Image = Properties.Resources.IMAGE_1642;
               System.Media.SoundPlayer opengatesound = new Media.SoundPlayer(@".\Media\SubSys\Kernel\Desktop\Sounds\Successfull.wav");
               opengatesound.Play();
            }
            catch { }
         }
      }

      private void Close_Gate(XElement xinput)
      {
         try
         {
            try
            {
               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Yellow;
               GateAttn_Butn.Image = Properties.Resources.IMAGE_1640;
               if (GateAttn_Butn.Tag != null)
               {
                  // Close This Gate
                  var gateAttnStng = GateAttn_Butn.Tag as Data.Setting;
                  if (gateAttnStng == null) return;

                  GateAttn_Butn.Image = Properties.Resources.IMAGE_1640;
                  if (gateAttnStng.GATE_EXIT_OPEN == "001") return;
                  Sp_GateAttn.Write("out");
                  //MessageBox.Show("Gate is Close");  
               }
            }
            catch { }

            // در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
            var _listIPHost = xHost.Descendants("IP").Select(ip => ip.Value).ToList();

            // 1400/01/12 * اگر گیت به صورت دستی باز شود
            if (xinput.Attribute("fngrprnt") == null)
            {
               #region Manual Set Open Gate Connected To Current PC
               if (_gatesDevice == null)
                  _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
               if (_gatesDevice != null && _gatesDevice.Any())
               {
                  _gatesDevice.ToList()
                     .ForEach(g =>
                        OprtExtDev(
                           new XElement("MainPage",
                              new XAttribute("type", "extdev"),
                              new XAttribute("devtype", "006"),
                              new XAttribute("contype", "002"),
                              new XAttribute("cmdtype", "close"),
                              new XAttribute("ip", g.IP_ADRS),
                              new XAttribute("sendport", g.PORT_SEND)
                           )
                        )
                     );

                  // 1400/01/12 * Reset All
                  _gatesDevice = null;
               }
               #endregion
            }
            else // اگر گیت به صورت اتومات باز شود مشتری حضور و غیاب کرده باشد
            {
               // در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
               // 1400/01/11
               // در اینجا باید مشخص کنیم که ایا گیت ها بر اساس رشته های مختلف باید واکنش نشان دهند یا خیر               
               if (_gatesDevice == null)
               {
                  // 1400/01/12
                  // یدا کردن گیت هایی که تردد گروه های مختلف را چک میکنند
                  _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && d.STAT == "002" && d.MTOD_CODE == null && d.External_Device_Link_Methods.Any(em => em.MTOD_CODE == xinput.Attribute("mtodcode").Value.ToInt64() && em.STAT == "002"));

                  // 1400/01/12
                  // اگر گیت مانند باشگاه نیایش باشد همان مسیر قبلی را برایش انجام میدهیم
                  if (_gatesDevice != null && _gatesDevice.Count() == 0)
                     _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
               }
               if (_gatesDevice != null && _gatesDevice.Any())
               {
                  _gatesDevice.ToList()
                     .ForEach(g =>
                        OprtExtDev(
                           new XElement("MainPage",
                              new XAttribute("type", "extdev"),
                              new XAttribute("devtype", "006"),
                              new XAttribute("contype", "002"),
                              new XAttribute("cmdtype", "close"),
                              new XAttribute("ip", g.IP_ADRS),
                              new XAttribute("sendport", g.PORT_SEND)
                           )
                        )
                     );

                  // 1400/01/12 * Reset All
                  _gatesDevice = null;
               }
            }

            //// در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
            //var _listIPHost = xHost.Descendants("IP").Select(ip => ip.Value).ToList();
            //if (_gatesDevice == null)
            //   _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
            //if (_gatesDevice != null && _gatesDevice != null && _gatesDevice.Any())
            //{
            //   _gatesDevice.ToList()
            //      .ForEach(g =>
            //         OprtExtDev(
            //            new XElement("MainPage",
            //                     new XAttribute("type", "extdev"),
            //                     new XAttribute("devtype", "006"),
            //                     new XAttribute("contype", "002"),
            //                     new XAttribute("cmdtype", "close"),
            //                     new XAttribute("ip", g.IP_ADRS),
            //                     new XAttribute("sendport", g.PORT_SEND)
            //                  )
            //         )
            //      );
            //}

            if (_readersDevice == null)
               _readersDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "002" && d.DEV_TYPE == "001" && d.DEV_CON == "002"  && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002");
            if (_readersDevice != null && _readersDevice.Any())
            {
               //var enddate = Convert.ToDateTime(xinput.Attribute("enddate").Value);
               //var numbattnmont = Convert.ToInt32(xinput.Attribute("numbattnmont").Value);
               //var sumattnmont = Convert.ToInt32(xinput.Attribute("sumattnmont").Value);
               //var debt = Convert.ToInt64(xinput.Attribute("debt").Value);
               //var fngrprnt = xinput.Attribute("fngrprnt").Value;

               _readersDevice.ToList()
                  .ForEach(r =>
                  {
                     SendCommandDevExpn("out", r.DEV_NAME, "");
                     //var pc = new PersianCalendar();
                     //if (numbattnmont != 0)
                     //{
                     //   SendCommandDevExpn("ot:" +
                     //      string.Format("{0}/{1}/{2}-{3}&{4:n0}", /*pc.GetYear(enddate)*/ 99, pc.GetMonth(enddate).ToString().PadLeft(2, '0'), pc.GetDayOfMonth(enddate).ToString().PadLeft(2, '0'), sumattnmont.ToString().PadLeft(3, ' '), debt.ToString().PadLeft(16, ' ')), r.DEV_NAME, fngrprnt
                     //   );
                     //}
                     //else
                     //{
                     //   SendCommandDevExpn("ot:" +
                     //      string.Format("{0}/{1}/{2}-   &{3:n0}", /*pc.GetYear(enddate)*/ 99, pc.GetMonth(enddate).ToString().PadLeft(2, '0'), pc.GetDayOfMonth(enddate).ToString().PadLeft(2, '0'), debt.ToString().PadLeft(16, ' ')), r.DEV_NAME, fngrprnt
                     //   );
                     //}

                     //SendCommandDevExpn(
                     //   "df:" + "WellCome".PadLeft(13, ' ') +
                     //   "&" + "Genetic Gym".PadLeft(16, ' '), r.DEV_NAME, ""
                     //);
                  });               
            }
         }
         catch (Exception ) { }
         finally
         {
            try
            {
               //GateAttn_Butn.Image = Properties.Resources.IMAGE_1642;
               System.Media.SoundPlayer closegatesound = new Media.SoundPlayer(@".\Media\SubSys\Kernel\Desktop\Sounds\Successfull.wav");
               closegatesound.Play();
            }
            catch { }
         }
      }

      private void Error_Gate(XElement xinput)
      {
         try
         {
            try
            {
               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Tomato;
               GateAttn_Butn.Image = Properties.Resources.IMAGE_1642;

               if (Sp_GateAttn.IsOpen)
               {
                  // Error This Gate                        
                  Sp_GateAttn.Write("error");
               }
            }
            catch { }

            // در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
            var _listIPHost = xHost.Descendants("IP").Select(ip => ip.Value).ToList();

            // 1400/01/12 * اگر گیت به صورت دستی باز شود
            if (xinput.Attribute("fngrprnt") == null)
            {
               #region Manual Set Open Gate Connected To Current PC
               if (_gatesDevice == null)
                  _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
               if (_gatesDevice != null && _gatesDevice.Any())
               {
                  _gatesDevice.ToList()
                     .ForEach(g =>
                        OprtExtDev(
                           new XElement("MainPage",
                              new XAttribute("type", "extdev"),
                              new XAttribute("devtype", "006"),
                              new XAttribute("contype", "002"),
                              new XAttribute("cmdtype", "error"),
                              new XAttribute("ip", g.IP_ADRS),
                              new XAttribute("sendport", g.PORT_SEND)
                           )
                        )
                     );

                  // 1400/01/12 * Reset All
                  _gatesDevice = null;
               }
               #endregion
            }
            else // اگر گیت به صورت اتومات باز شود مشتری حضور و غیاب کرده باشد
            {
               // در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
               // 1400/01/11
               // در اینجا باید مشخص کنیم که ایا گیت ها بر اساس رشته های مختلف باید واکنش نشان دهند یا خیر               
               if (_gatesDevice == null)
               {
                  // 1400/01/12
                  // یدا کردن گیت هایی که تردد گروه های مختلف را چک میکنند
                  _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && d.STAT == "002" && d.MTOD_CODE == null && d.External_Device_Link_Methods.Any(em => em.MTOD_CODE == xinput.Attribute("mtodcode").Value.ToInt64() && em.STAT == "002"));

                  // 1400/01/12
                  // اگر گیت مانند باشگاه نیایش باشد همان مسیر قبلی را برایش انجام میدهیم
                  if (_gatesDevice != null && _gatesDevice.Count() == 0)
                     _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
               }
               if (_gatesDevice != null && _gatesDevice.Any())
               {
                  _gatesDevice.ToList()
                     .ForEach(g =>
                        OprtExtDev(
                           new XElement("MainPage",
                              new XAttribute("type", "extdev"),
                              new XAttribute("devtype", "006"),
                              new XAttribute("contype", "002"),
                              new XAttribute("cmdtype", "error"),
                              new XAttribute("ip", g.IP_ADRS),
                              new XAttribute("sendport", g.PORT_SEND)
                           )
                        )
                     );

                  // 1400/01/12 * Reset All
                  _gatesDevice = null;
               }
            }

            // در این قسمت می توانیم بررسی کنیم که این سیستم به چه گیتی متصل می باشد که بتوانید به آن گیت فرمان دهیم که گیت را باز کند
            //var _listIPHost = xHost.Descendants("IP").Select(ip => ip.Value).ToList();
            //if (_gatesDevice == null)
            //   _gatesDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "006" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002" && d.MTOD_CODE == null);
            //if (_gatesDevice != null && _gatesDevice.Any())
            //{
            //   _gatesDevice.ToList()
            //      .ForEach(g =>
            //         OprtExtDev(
            //            new XElement("MainPage",
            //               new XAttribute("type", "extdev"),
            //               new XAttribute("devtype", "006"),
            //               new XAttribute("contype", "002"),
            //               new XAttribute("cmdtype", "error"),
            //               new XAttribute("ip", g.IP_ADRS),
            //               new XAttribute("sendport", g.PORT_SEND)
            //            )
            //         )
            //      );
            //}

            if (_readersDevice == null)
               _readersDevice = iScsc.External_Devices.Where(d => d.DEV_COMP_TYPE == "002" && d.DEV_TYPE == "001"  && d.DEV_CON == "002" && _listIPHost.Contains(d.SERV_IP_ADRS) && d.STAT == "002");
            if (_readersDevice != null && _readersDevice.Any())
            {
               _readersDevice.ToList()
                  .ForEach(r =>
                  {                     
                     SendCommandDevExpn("er" , r.DEV_NAME, FngrPrnt_Txt.Text);

                     //SendCommandDevExpn(
                     //   "df:" + "WellCome".PadLeft(13, ' ') +
                     //   "&" + "Genetic Gym".PadLeft(16, ' '), r.DEV_NAME, ""
                     //);
                  }
                  );
            }

            //MessageBox.Show("Gate is Close");
         }
         catch (Exception) { }
         finally
         {
            try
            {
               //GateAttn_Butn.Image = Properties.Resources.IMAGE_1642;
               System.Media.SoundPlayer errorgatesound = new Media.SoundPlayer(@".\Media\SubSys\Kernel\Desktop\Sounds\BuzzError.wav");
               errorgatesound.Play();
            }
            catch { }
         }
      }
      #endregion

      #region ExpnExtr
      private void Start_ExpnExtr()
      {
         var expnExtrSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
         try
         {
            if (expnExtrSetting == null) return;

            if (expnExtrSetting.EXPN_EXTR_STAT == "001") { SrvrPing_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196; return; }

            Sp_ExpnExtr.PortName = expnExtrSetting.EXPN_COMM_PORT_NAME;
            Sp_ExpnExtr.BaudRate = (int)expnExtrSetting.EXPN_BAND_RATE;
            GameHours_Butn.Tag = expnExtrSetting;
            Sp_ExpnExtr.Open();

            if (Sp_ExpnExtr.IsOpen)
            {
               GameHours_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1428;
            }
            else
            {
               GameHours_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            }
         }
         catch
         {
            GameHours_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
         }
         finally
         {
            if(expnExtrSetting != null)
               GameHours_Butn.SuperTip =
                  SuperToolTipAttnButn(
                     new XElement("System",
                        new XAttribute("device", "ExpnExtr"),
                        new XAttribute("stat", expnExtrSetting.EXPN_EXTR_STAT == "002" ? true : false)
                     )
                  );
         }
      }

      private void Stop_ExpnExtr()
      {
         try
         {
            if (Sp_ExpnExtr.IsOpen)
            {
               Sp_ExpnExtr.Close();
               GameHours_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      class DataReadFromCardReader { public string EnrollNumber { get; set; } public string MacAdrs { get; set; } public DateTime LastTimeRead { get; set; } }
      List<DataReadFromCardReader> lastDataRead = new List<DataReadFromCardReader>();
      private bool frstVistTablCntlF = false;
      private void Sp_ExpnExtr_DataReceived(object sender, IO.Ports.SerialDataReceivedEventArgs e)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            // input data e.g : {device code} "-" {Card Code}
            var enrollNumber = Sp_ExpnExtr.ReadLine();
            if (enrollNumber.IndexOf('\r') != -1)
               enrollNumber = enrollNumber.Substring(0, enrollNumber.IndexOf('\r')).ToUpper();
            else
               enrollNumber = enrollNumber.ToUpper();

            enrollNumber = Regex.Replace(enrollNumber, "[^a-zA-Z0-9-]", "");

            var devName = enrollNumber.Split('-').ToList()[0];
            var fngrPrnt = enrollNumber.Split('-').ToList()[1];

            //System.Diagnostics.Debug.WriteLine(enrollNumber);
            //new Thread(AlarmShow).Start();
            //return;

            // اگر داده ای درون سیستم از سمت کارتخوان آمده باشد باید تا پاسخ دهی به دستگاه هیچ ورودی دیگری قابل پردازش نیست
            // اگر داده قبلی بر اساس 10 ثانیه هنوز درون بافر باشد باید از لیست حذف شود
            if (lastDataRead.Any(d => d.MacAdrs == devName && d.EnrollNumber == fngrPrnt && DateTime.Now.Subtract(d.LastTimeRead).TotalSeconds >= 10))
            {
               lastDataRead.Remove(lastDataRead.FirstOrDefault(d => d.MacAdrs == devName && d.EnrollNumber == fngrPrnt));
               //System.Diagnostics.Debug.WriteLine("Remove Last Time Read");
            }

            // اگر داده ای از کارتخوان و شماره کارت وجود داشته باشد ورودی جدید را رد میکنیم
            if (lastDataRead.Any(d => d.MacAdrs == devName && d.EnrollNumber == fngrPrnt)) { /*System.Diagnostics.Debug.WriteLine("Reject New Read Data");*/ return; }

            // اگر اطلاعاتی از کارتخوان و کارت عضویت وجود نداشته باشد برای اولین بار آن را ثبت میکنیم
            lastDataRead.Add(new DataReadFromCardReader() { MacAdrs = devName, EnrollNumber = fngrPrnt, LastTimeRead = DateTime.Now });
            //System.Diagnostics.Debug.WriteLine("Add New Data Read");

            //System.Diagnostics.Debug.WriteLine(enrollNumber);
            //new Thread(AlarmShow).Start();
            //return;

            if (InvokeRequired)
            {               
               Invoke(
                  new Action(() =>
                     {
                        // ابتدا بررسی میکنیم که داده ورودی مربوط به کدام بخش دستگاه های بازی میشود
                        // 1 - بازی های زمان متغییر مانند بیلیارد
                        // 2 - بازی های زمان ثابت مانند شهربازی
                        devName = Regex.Replace(devName, "[^0-9]", "");
                        var getInfoDev = iScsc.External_Devices.FirstOrDefault(d => d.DEV_NAME == devName);
                        if (getInfoDev == null) { System.Diagnostics.Debug.WriteLine("Reject because can't found device" + devName); return; }

                        // set Finger Print Data on Text Box
                        FngrPrnt_Txt.Text = fngrPrnt;

                        // Check Exists Service and Valid Card
                        var Serv = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == fngrPrnt);

                        // بدست آوردن آیین نامه اصلی
                        var regl = iScsc.Regulations.FirstOrDefault(rg => rg.REGL_STAT == "002" && rg.TYPE == "001");

                        // اگر مشتری وجود نداشته یا اینکه مشتری اصلا سپرده نداشته باشد
                        if (Serv == null || (regl.AMNT_TYPE == "001" && Serv.DPST_AMNT_DNRM < 10000) || (regl.AMNT_TYPE == "002" && Serv.DPST_AMNT_DNRM < 1000 ))
                        {
                           // اگر کارت عضویت خام باشد
                           if(Serv == null)
                           {
                              // باز کردن فرم ثبت نام مشتری
                              Job _InteractWithScsc =
                                 new Job(SendType.External, "Localhost",
                                    new List<Job>
                                    {
                                       new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                                       new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", fngrPrnt))}
                                    });
                              _DefaultGateway.Gateway(_InteractWithScsc);
                           }
                           //else if ((regl.AMNT_TYPE == "001" && Serv.DPST_AMNT_DNRM < 10000) || (regl.AMNT_TYPE == "002" && Serv.DPST_AMNT_DNRM < 1000))
                           //{
                           //   Job _InteractWithScsc =
                           //      new Job(SendType.External, "Localhost",
                           //         new List<Job>
                           //         {
                           //            new Job(SendType.Self, 153 /* Execute Glr_Indc_F */),
                           //            new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 10 /* Execute Actn_CalF_F */)
                           //            {
                           //               Input = 
                           //                  new XElement("Request", 
                           //                     new XAttribute("type", "newrequest"), 
                           //                     new XAttribute("fileno", Serv.FILE_NO),
                           //                     new XAttribute("formcaller", GetType().Name)
                           //                  )
                           //            }
                           //         });
                           //   _DefaultGateway.Gateway(_InteractWithScsc);
                           //}
                           SendCommandDevExpn("er", devName, fngrPrnt);
                           return;
                        }
 
                        if (getInfoDev.DEV_TYPE == "007" /* بازی های زمان متغییر مانند بیلیارد */)
                        {
                           #region بارگذاری فرم مربوط به رزرو میز
                           Job _GetAopBufeF =
                              new Job(SendType.External, "localhost",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_bufe_f"}
                                 }
                              );
                           _DefaultGateway.Gateway(_GetAopBufeF);
                           #endregion

                           #region اگر فرم رزرو میز درون حافظه قرار گرفت
                           if (_GetAopBufeF.Output != null)
                           {
                              if (frstVistTablCntlF)
                              {
                                 #region برای روالهای بعدی دریافت ورودی
                                 _DefaultGateway.Gateway(
                                    new Job(SendType.External, "Localhost",
                                       new List<Job>
                                       {
                                          new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){
                                             Input = 
                                                new XElement("Request", 
                                                   new XAttribute("type", "tp_001"),
                                                   new XAttribute("fngrprnt", fngrPrnt),
                                                   new XAttribute("macadrs", devName),
                                                   new XAttribute("expncode", getInfoDev.EXPN_CODE ?? 0)
                                                )
                                          }
                                       }
                                    )
                                 );
                                 #endregion
                              }
                              else
                              {
                                 #region اگر برای اولین بار فرم میز باز میشود
                                 frstVistTablCntlF = true;
                                 _DefaultGateway.Gateway(
                                    new Job(SendType.External, "Localhost",
                                       new List<Job>
                                       {
                                          new Job(SendType.Self, 131 /* Execute Aop_Bufe_F */),
                                          new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){
                                             Input = 
                                                new XElement("Request", 
                                                   new XAttribute("type", "tp_001"),
                                                   new XAttribute("fngrprnt", fngrPrnt),
                                                   new XAttribute("macadrs", devName),
                                                   new XAttribute("expncode", getInfoDev.EXPN_CODE ?? 0)
                                                )
                                          }
                                       }
                                    )
                                 );
                                 #endregion
                              }
                           }
                           #endregion
                        }
                        else if (getInfoDev.DEV_TYPE == "008" /* بازی های زمان ثابت مانند شهربازی */)
                        {
                           #region فعالیت های مربوط به دستگاه های بازی با زمان ثابت
                           var devExpn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == getInfoDev.EXPN_CODE);
                           // اگر مبلغ بازی از میزان اعتبار مشتری بیشتر باشد
                           if (Serv.DPST_AMNT_DNRM < devExpn.PRIC)
                              SendCommandDevExpn("er", devName, fngrPrnt);

                           // بررسی اینکه آیا مشتری قبلا کارت خود را وارد نکرده باشد و دوباره کارت نزده باشد 
                           // نکته : اگر مشتری کارت زده باشد و دوره بازی تمام شده باشد می تواند دوباره کارت بزند و بازی کند
                           var lastRqst016 = iScsc.VF_Request_Changing(null).Where(r => r.RQTP_CODE == "016" && r.SAVE_DATE.Value.Date == DateTime.Now.Date && iScsc.Payment_Details.Any(pd => pd.PYMT_RQST_RQID == r.RQID && pd.EXPN_CODE == devExpn.CODE)).OrderByDescending(r => r.SAVE_DATE).Take(1).FirstOrDefault();

                           // اگر درخواستی برای دستگاه بازی برای مشتری وجود داشته باشه
                           if (lastRqst016 != null)
                           {
                              // اگر بازه زمانی برای مشتری تمام شده باشد با کسر اعتبار جدید می تواند دوباره بازی کند
                              if (!DateTime.Now.IsBetween((DateTime)lastRqst016.SAVE_DATE, (DateTime)lastRqst016.SAVE_DATE.Value.AddMinutes(devExpn.MIN_TIME.Value.Minute)))
                              {
                                 // ثبت درخواست و کسر اعتبار از مشتری

                                 // مجوز اجرای بازی
                                 SendCommandDevExpn("st", devName, fngrPrnt);
                              }
                           }
                           else
                           {
                              // ثبت درخواست و کسر اعتبار از مشتری

                              // مجوز اجرای بازی
                              SendCommandDevExpn("st", devName, fngrPrnt);
                           }
                           #endregion
                        }
                     })
               );               
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void SendCommandDevExpn(string cmndText, string devName, string enrollNumber)
      {
         try
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.YellowGreen;
            
            var devInfo = iScsc.External_Devices.FirstOrDefault(d => d.DEV_NAME == devName && d.STAT == "002");

            if (devInfo.DEV_TYPE.In( "001", "007", "008", "009", "010", "011"))
               server.ConnectedClients.Where(d => d.ConnectAddress == devName).ToList()
                  .ForEach(d =>
                  {
                     try
                     {
                        d.SendMessage(
                           cmndText
                        );
                     }
                     catch { }
                  }
                  );
            //if(devInfo.DEV_TYPE == "007")
            //   server.ConnectedClients.Where(d => d.ConnectAddress == devName).ToList()
            //      .ForEach(d =>
            //      {
            //         try
            //         {
            //            d.SendMessage(
            //               cmndText
            //            );
            //         }
            //         catch { }
            //      }
            //      );
            //else if (devInfo.DEV_TYPE == "008")
            //   server.ConnectedClients.Where(d => d.ConnectAddress == devName).ToList()
            //      .ForEach(d =>
            //         {
            //            try
            //            {
            //               d.SendMessage(
            //                  cmndText
            //               );
            //            }
            //            catch { }
            //         }
            //      );
            if (devInfo.DEV_TYPE.In("009", "010"))
            {
               // Unlock
               //server.ConnectedClients.Where(d => d.ConnectAddress == devName).ToList()
               //   .ForEach(d =>
               //   {
               //      try
               //      {
               //         d.SendMessage(
               //            cmndText
               //         );
               //      }
               //      catch { }
               //   }
               //   );

               Thread.Sleep(100);

               // Show Seven Segment
               server.ConnectedClients.Where(d => d.ConnectAddress == iScsc.External_Device_Link_External_Devices.FirstOrDefault(ld => ld.LINK_EDEV_CODE == devInfo.CODE).External_Device.IP_ADRS).ToList()
                  .ForEach(d =>
                  {
                     try
                     {
                        d.SendMessage(
                           enrollNumber.PadLeft(3, '0')
                        );
                     }
                     catch { }
                  }
                  );
            }

            switch (cmndText.Substring(0, 2))
            {
               case "in":
               case "st":
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                  break;
               case "out":
               case "sp":
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlueViolet;
                  break;
               case "er":
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Tomato;
                  break;
               default:
                  break;
            }
         }
         catch
         {
            //MessageBox.Show(exc.Message);
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Tomato;
         }
         finally
         {
            lastDataRead.Remove(lastDataRead.FirstOrDefault(d => d.MacAdrs == devName && d.EnrollNumber == enrollNumber));
         }
      }
      #endregion

      #region Telegram Bot
      private void Start_TlgrmBot()
      {
         var TlgrmBotSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
         try
         {
            if (TlgrmBotSetting == null) return;

            if (TlgrmBotSetting.RUN_RBOT == "001") { TlgrmBot_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196; return; }

            // Check Telegram Bot
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", 
                  new List<Job>
                  {
                     new Job(SendType.External, "DefaultGateway",
                        new List<Job>
                        {
                           new Job(SendType.External, "RoboTech",
                              new List<Job>
                              {
                                 new Job(SendType.Self, 01 /* Execute GetUi */){Input = "strt_robo_f"},
                                 new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 02 /* Execute Set */),
                                 new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 07 /* Execute Load_Data */),
                                 new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Robot", new XAttribute("runrobot", "start"))}
                              }
                           )
                        }
                     )
                  }
               )
            );
            
         }
         catch
         {
            TlgrmBot_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
         }
         finally
         {
            if(TlgrmBotSetting != null)
               TlgrmBot_Butn.SuperTip =
                  SuperToolTipAttnButn(
                     new XElement("System",
                        new XAttribute("device", "TlgrmBot"),
                        new XAttribute("stat", TlgrmBotSetting.RUN_RBOT == "002" ? true : false)
                     )
                  );
         }
      }
      #endregion

      #region FingerPrint Kaviani
      private void Enroll_Finger(string enrollId)
      {
         try
         {
            Sp_Barcode.Write("enroll");
            Threading.Thread.Sleep(2000);
            Sp_Barcode.Write(enrollId);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Delete_Finger(string enrollId)
      {
         try
         {
            Sp_Barcode.Write("delete");
            Threading.Thread.Sleep(2000);
            Sp_Barcode.Write(enrollId);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Truncate_Finger()
      {
         Sp_Barcode.Write("empty");
      }
      #endregion

      #region ZKT Finger Print Sensor
      private void Start_ZKTFPSensor()
      {
         try
         {  
            var ZKTFPSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();

            if (ZKTFPSetting == null) return;

            if (ZKTFPSetting.ATTN_SYST_TYPE.NotIn("005")) return;

            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;

            bnInit_Click(null, null);

            AttnType_Lov.EditValue = ZKTFPSetting.ATN3_EVNT_ACTN_TYPE;
         }
         catch
         {
            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            //MessageBox.Show(ex.Message);
            Tsp_AttnSys.Text = "سیستم سنسور انگشتی غیرفعال";
            Tsp_AttnSys.ForeColor = Color.Red;
         }
      }

      IntPtr mDevHandle = IntPtr.Zero;
      IntPtr mDBHandle = IntPtr.Zero;
      IntPtr FormHandle = IntPtr.Zero;
      bool bIsTimeToDie = false;
      bool IsRegister = false;
      bool bIdentify = true;
      byte[] FPBuffer;
      int RegisterCount = 0;
      const int REGISTER_FINGER_COUNT = 3;

      byte[][] RegTmps = new byte[3][];
      byte[] RegTmp = new byte[2048];
      byte[] CapTmp = new byte[2048];

      int cbCapTmp = 2048;
      int cbRegTmp = 0;
      int iFid = 1;

      private int mfpWidth = 0;
      private int mfpHeight = 0;
      private int mfpDpi = 0;

      const int MESSAGE_CAPTURED_OK = 0x0400 + 6;
      private string ZktFpAttnStat = "attendance";

      [DllImport("user32.dll", EntryPoint = "SendMessageA")]
      public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

      #region ZkFinger4500K
      private void bnInit_Click(object sender, EventArgs e)
      {
         //cmbIdx.Items.Clear();
         int ret = zkfperrdef.ZKFP_ERR_OK;
         if ((ret = zkfp2.Init()).In(zkfperrdef.ZKFP_ERR_OK /*, zkfperrdef.ZKFP_ERR_ALREADY_INIT*/))
         {
            int nCount = zkfp2.GetDeviceCount();
            if (nCount > 0)
            {
               //for (int i = 0; i < nCount; i++)
               //{
               //   cmbIdx.Items.Add(i.ToString());
               //}
               //cmbIdx.SelectedIndex = 0;
               bnOpen_Click(null, null);
            }
            else
            {
               zkfp2.Terminate();
               MessageBox.Show("No device connected!");
               this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            }
         }
         else
         {
            //MessageBox.Show("Initialize fail, ret=" + ret + " !");
            //throw new Exception("Error");
            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
         }
      }

      private void bnFree_Click(object sender, EventArgs e)
      {
         zkfp2.Terminate();
         cbRegTmp = 0;
      }

      private void bnOpen_Click(object sender, EventArgs e)
      {
         int ret = zkfp.ZKFP_ERR_OK;
         if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(0)))
         {
            MessageBox.Show("OpenDevice fail");
            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            return;
         }
         if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
         {
            MessageBox.Show("Init DB fail");
            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            zkfp2.CloseDevice(mDevHandle);
            mDevHandle = IntPtr.Zero;
            return;
         }
         RegisterCount = 0;
         cbRegTmp = 0;
         iFid = 1;
         //for (int i = 0; i < 3; i++)
         //{
         //   RegTmps[i] = new byte[2048];
         //}
         byte[] paramValue = new byte[4];
         int size = 4;
         zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
         zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

         size = 4;
         zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
         zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

         FPBuffer = new byte[mfpWidth * mfpHeight];

         size = 4;
         zkfp2.GetParameters(mDevHandle, 3, paramValue, ref size);
         zkfp2.ByteArray2Int(paramValue, ref mfpDpi);

         //textRes.AppendText("reader parameter, image width:" + mfpWidth + ", height:" + mfpHeight + ", dpi:" + mfpDpi + "\n");
         //FngrDev_Pb.Visible = true;

         Thread captureThread = new Thread(new ThreadStart(DoCapture));
         captureThread.IsBackground = true;
         captureThread.Start();
         bIsTimeToDie = false;
         this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1211;
         //textRes.AppendText("Open succ\n");
      }

      private void bnClose_Click(object sender, EventArgs e)
      {
         bIsTimeToDie = true;
         RegisterCount = 0;
         Thread.Sleep(1000);
         zkfp2.CloseDevice(mDevHandle);
         bnFree_Click(null, null);
      }

      private void DoCapture()
      {
         while (!bIsTimeToDie)
         {
            cbCapTmp = 2048;
            int ret = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);
            if (ret == zkfp.ZKFP_ERR_OK)
            {
               SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
            }
            Thread.Sleep(200);
         }
      }

      protected override void DefWndProc(ref Message m)
      {
         switch (m.Msg)
         {
            case MESSAGE_CAPTURED_OK:
               {
                  String fngrTmpl = zkfp2.BlobToBase64(CapTmp, cbCapTmp);

                  byte[] blob2 = Convert.FromBase64String(fngrTmpl.Trim());

                  if(ZktFpAttnStat == "enroll")
                  {
                     MemoryStream ms = new MemoryStream();
                     BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                     Bitmap fngrImg = new Bitmap(ms);
                     
                     var fngr_img_tmpl = new List<object>();
                     fngr_img_tmpl.Add(fngrImg);
                     fngr_img_tmpl.Add(fngrTmpl);

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_Calf_P */)
                              {
                                 Input = fngr_img_tmpl                                    
                              }
                           }
                        )
                     );
                  }
                  else if(ZktFpAttnStat == "attendance")
                  {
                     iScsc = new Data.iScscDataContext(ConnectionString);
                     foreach (var fngr in iScsc.Image_Documents.Where(f => f.Receive_Document.Request_Document.DCMT_DSID == 13980505495708 && f.IMAG != null))
                     {
                        byte[] blob1 = Convert.FromBase64String(fngr.IMAG.Trim());
                        int ret = zkfp2.DBMatch(mDBHandle, blob1, blob2);
                        //textRes.AppendText("Fngr id : ( " + fngr.FNGR_INDX.ToString() + " ), Match score=" + ret + "!\n");
                        if (ret >= 80 && ret <= 100)
                        {
                           axCZKEM1_OnAttTransactionEx(fngr.Receive_Document.Request_Row.Fighter.FNGR_PRNT_DNRM, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);                           
                           break;
                        }
                     }
                  }                  
               }
               break;

            default:
               base.DefWndProc(ref m);
               break;
         }
      }

      public class BitmapFormat
      {
         public struct BITMAPFILEHEADER
         {
            public ushort bfType;
            public int bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public int bfOffBits;
         }

         public struct MASK
         {
            public byte redmask;
            public byte greenmask;
            public byte bluemask;
            public byte rgbReserved;
         }

         public struct BITMAPINFOHEADER
         {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
         }

         /*******************************************
         * ؛¯ت‎أû³ئ£؛RotatePic       
         * ؛¯ت‎¹¦ؤـ£؛ذ‎×ھح¼ئ¬£¬ؤ؟µؤتا±£´و؛حدشت¾µؤح¼ئ¬سë°´µؤض¸خئ·½دٍ²»ح¬     
         * ؛¯ت‎بë²خ£؛BmpBuf---ذ‎×ھا°µؤض¸خئ×ض·û´®
         * ؛¯ت‎³ِ²خ£؛ResBuf---ذ‎×ھ؛َµؤض¸خئ×ض·û´®
         * ؛¯ت‎·µ»ط£؛خق
         *********************************************/
         public static void RotatePic(byte[] BmpBuf, int width, int height, ref byte[] ResBuf)
         {
            int RowLoop = 0;
            int ColLoop = 0;
            int BmpBuflen = width * height;

            try
            {
               for (RowLoop = 0; RowLoop < BmpBuflen; )
               {
                  for (ColLoop = 0; ColLoop < width; ColLoop++)
                  {
                     ResBuf[RowLoop + ColLoop] = BmpBuf[BmpBuflen - RowLoop - width + ColLoop];
                  }

                  RowLoop = RowLoop + width;
               }
            }
            catch (Exception ex)
            {
               //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
               //logger.Append();
            }
         }

         /*******************************************
         * ؛¯ت‎أû³ئ£؛StructToBytes       
         * ؛¯ت‎¹¦ؤـ£؛½«½ل¹¹جه×ھ»¯³ةخق·û؛إ×ض·û´®ت‎×é     
         * ؛¯ت‎بë²خ£؛StructObj---±»×ھ»¯µؤ½ل¹¹جه
         *           Size---±»×ھ»¯µؤ½ل¹¹جهµؤ´َذ،
         * ؛¯ت‎³ِ²خ£؛خق
         * ؛¯ت‎·µ»ط£؛½ل¹¹جه×ھ»¯؛َµؤت‎×é
         *********************************************/
         public static byte[] StructToBytes(object StructObj, int Size)
         {
            int StructSize = Marshal.SizeOf(StructObj);
            byte[] GetBytes = new byte[StructSize];

            try
            {
               IntPtr StructPtr = Marshal.AllocHGlobal(StructSize);
               Marshal.StructureToPtr(StructObj, StructPtr, false);
               Marshal.Copy(StructPtr, GetBytes, 0, StructSize);
               Marshal.FreeHGlobal(StructPtr);

               if (Size == 14)
               {
                  byte[] NewBytes = new byte[Size];
                  int Count = 0;
                  int Loop = 0;

                  for (Loop = 0; Loop < StructSize; Loop++)
                  {
                     if (Loop != 2 && Loop != 3)
                     {
                        NewBytes[Count] = GetBytes[Loop];
                        Count++;
                     }
                  }

                  return NewBytes;
               }
               else
               {
                  return GetBytes;
               }
            }
            catch (Exception ex)
            {
               //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
               //logger.Append();

               return GetBytes;
            }
         }

         /*******************************************
         * ؛¯ت‎أû³ئ£؛GetBitmap       
         * ؛¯ت‎¹¦ؤـ£؛½«´«½ّہ´µؤت‎¾ف±£´وخھح¼ئ¬     
         * ؛¯ت‎بë²خ£؛buffer---ح¼ئ¬ت‎¾ف
         *           nWidth---ح¼ئ¬µؤ؟ي¶ب
         *           nHeight---ح¼ئ¬µؤ¸ك¶ب
         * ؛¯ت‎³ِ²خ£؛خق
         * ؛¯ت‎·µ»ط£؛خق
         *********************************************/
         public static void GetBitmap(byte[] buffer, int nWidth, int nHeight, ref MemoryStream ms)
         {
            int ColorIndex = 0;
            ushort m_nBitCount = 8;
            int m_nColorTableEntries = 256;
            byte[] ResBuf = new byte[nWidth * nHeight * 2];

            try
            {
               BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
               BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
               MASK[] ColorMask = new MASK[m_nColorTableEntries];

               int w = (((nWidth + 3) / 4) * 4);

               //ح¼ئ¬ح·ذإد¢
               BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
               BmpInfoHeader.biWidth = nWidth;
               BmpInfoHeader.biHeight = nHeight;
               BmpInfoHeader.biPlanes = 1;
               BmpInfoHeader.biBitCount = m_nBitCount;
               BmpInfoHeader.biCompression = 0;
               BmpInfoHeader.biSizeImage = 0;
               BmpInfoHeader.biXPelsPerMeter = 0;
               BmpInfoHeader.biYPelsPerMeter = 0;
               BmpInfoHeader.biClrUsed = m_nColorTableEntries;
               BmpInfoHeader.biClrImportant = m_nColorTableEntries;

               //خؤ¼‏ح·ذإد¢
               BmpHeader.bfType = 0x4D42;
               BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
               BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
               BmpHeader.bfReserved1 = 0;
               BmpHeader.bfReserved2 = 0;

               ms.Write(StructToBytes(BmpHeader, 14), 0, 14);
               ms.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)), 0, Marshal.SizeOf(BmpInfoHeader));

               //µ÷تش°هذإد¢
               for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
               {
                  ColorMask[ColorIndex].redmask = (byte)ColorIndex;
                  ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
                  ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
                  ColorMask[ColorIndex].rgbReserved = 0;

                  ms.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])), 0, Marshal.SizeOf(ColorMask[ColorIndex]));
               }

               //ح¼ئ¬ذ‎×ھ£¬½â¾ِض¸خئح¼ئ¬µ¹ء¢µؤختجâ
               RotatePic(buffer, nWidth, nHeight, ref ResBuf);

               byte[] filter = null;
               if (w - nWidth > 0)
               {
                  filter = new byte[w - nWidth];
               }
               for (int i = 0; i < nHeight; i++)
               {
                  ms.Write(ResBuf, i * nWidth, nWidth);
                  if (w - nWidth > 0)
                  {
                     ms.Write(ResBuf, 0, w - nWidth);
                  }
               }
            }
            catch (Exception ex)
            {
               // ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
               // logger.Append();
            }
         }

         /*******************************************
         * ؛¯ت‎أû³ئ£؛WriteBitmap       
         * ؛¯ت‎¹¦ؤـ£؛½«´«½ّہ´µؤت‎¾ف±£´وخھح¼ئ¬     
         * ؛¯ت‎بë²خ£؛buffer---ح¼ئ¬ت‎¾ف
         *           nWidth---ح¼ئ¬µؤ؟ي¶ب
         *           nHeight---ح¼ئ¬µؤ¸ك¶ب
         * ؛¯ت‎³ِ²خ£؛خق
         * ؛¯ت‎·µ»ط£؛خق
         *********************************************/
         public static void WriteBitmap(byte[] buffer, int nWidth, int nHeight)
         {
            int ColorIndex = 0;
            ushort m_nBitCount = 8;
            int m_nColorTableEntries = 256;
            byte[] ResBuf = new byte[nWidth * nHeight];

            try
            {

               BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
               BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
               MASK[] ColorMask = new MASK[m_nColorTableEntries];
               int w = (((nWidth + 3) / 4) * 4);
               //ح¼ئ¬ح·ذإد¢
               BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
               BmpInfoHeader.biWidth = nWidth;
               BmpInfoHeader.biHeight = nHeight;
               BmpInfoHeader.biPlanes = 1;
               BmpInfoHeader.biBitCount = m_nBitCount;
               BmpInfoHeader.biCompression = 0;
               BmpInfoHeader.biSizeImage = 0;
               BmpInfoHeader.biXPelsPerMeter = 0;
               BmpInfoHeader.biYPelsPerMeter = 0;
               BmpInfoHeader.biClrUsed = m_nColorTableEntries;
               BmpInfoHeader.biClrImportant = m_nColorTableEntries;

               //خؤ¼‏ح·ذإد¢
               BmpHeader.bfType = 0x4D42;
               BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
               BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
               BmpHeader.bfReserved1 = 0;
               BmpHeader.bfReserved2 = 0;

               Stream FileStream = File.Open("finger.bmp", FileMode.Create, FileAccess.Write);
               BinaryWriter TmpBinaryWriter = new BinaryWriter(FileStream);

               TmpBinaryWriter.Write(StructToBytes(BmpHeader, 14));
               TmpBinaryWriter.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)));

               //µ÷تش°هذإد¢
               for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
               {
                  ColorMask[ColorIndex].redmask = (byte)ColorIndex;
                  ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
                  ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
                  ColorMask[ColorIndex].rgbReserved = 0;

                  TmpBinaryWriter.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])));
               }

               //ح¼ئ¬ذ‎×ھ£¬½â¾ِض¸خئح¼ئ¬µ¹ء¢µؤختجâ
               RotatePic(buffer, nWidth, nHeight, ref ResBuf);

               //ذ´ح¼ئ¬
               //TmpBinaryWriter.Write(ResBuf);
               byte[] filter = null;
               if (w - nWidth > 0)
               {
                  filter = new byte[w - nWidth];
               }
               for (int i = 0; i < nHeight; i++)
               {
                  TmpBinaryWriter.Write(ResBuf, i * nWidth, nWidth);
                  if (w - nWidth > 0)
                  {
                     TmpBinaryWriter.Write(ResBuf, 0, w - nWidth);
                  }
               }

               FileStream.Close();
               TmpBinaryWriter.Close();
            }
            catch (Exception ex)
            {
               //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
               //logger.Append();
            }
         }
      }
      #endregion
      #endregion

      #region Finger Print
      public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
      public zkemkeeper.CZKEMClass axCZKEM2 = new zkemkeeper.CZKEMClass();
      public zkemkeeper.CZKEMClass axCZKEM3 = new zkemkeeper.CZKEMClass();
      // 1402/10/02 * دستگاه های رختکن
      public zkemkeeper.CZKEMClass axCZKEM4 = new zkemkeeper.CZKEMClass();
      public zkemkeeper.CZKEMClass axCZKEM5 = new zkemkeeper.CZKEMClass();
      public zkemkeeper.CZKEMClass axCZKEM6 = new zkemkeeper.CZKEMClass();

      bool Fp1DevIsConnected = false;
      bool Fp2DevIsConnected = false;
      bool Fp3DevIsConnected = false;
      // 1402/10/02 * دستگاه های رختکن
      bool Fp4DevIsConnected = false;
      bool Fp5DevIsConnected = false;
      bool Fp6DevIsConnected = false;
      XElement xHost = null;
      void Start_FingerPrint()
      {
         try
         {
            _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
                  {
                     AfterChangedOutput =
                     new Action<object>((output) =>
                     {
                        var host = output as XElement;

                        xHost = host;
                        #region 1st Device
                        if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CONCT == host.Attribute("cpu").Value))
                        {
                           var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CONCT == host.Attribute("cpu").Value).FirstOrDefault();

                           if (fingerPrintSetting == null) return;

                           // اگر حضور غیاب با دستگاه انگشتی نباشد
                           if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                           // اگر حضور و غیاب با دستگاه انگشتی باشد و ارتباط را چک میکنیم
                           if (fingerPrintSetting.IP_ADDR != null && fingerPrintSetting.PORT_NUMB != null)
                           {
                              Tsp_AttnSys.Text = "در حال اتصال به دستگاه حضور غیاب...";
                              this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;
                              Tsp_AttnSys.ForeColor = SystemColors.ControlText;

                              // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                              var dev = 
                                 new Device_On_Network(fingerPrintSetting.IP_ADDR) 
                                 { 
                                    SPort = (int)fingerPrintSetting.PORT_NUMB, 
                                    RPort = (int)fingerPrintSetting.PORT_NUMB, 
                                    DeviceType = "FngrPrnt", 
                                    DeviceName = "دستگاه اثر انگشت پذیرش"
                                 };
                              dev.CallBack =
                                 new Action(() =>
                                    {
                                       Fp1DevIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADDR, Convert.ToInt32(fingerPrintSetting.PORT_NUMB));
                                       if (!dev.Init)
                                       {
                                          // fire event for fetch 
                                          axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                                          // New code 
                                          axCZKEM1.OnHIDNum += axCZKEM1_OnHIDNum;
                                          dev.Init = true;
                                       }

                                       if (Fp1DevIsConnected == true)
                                       {
                                          Tsp_AttnSys.Text = "دستگاه حضور غیاب فعال می باشد";
                                          this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
                                          Tsp_AttnSys.ForeColor = Color.Green;
                                          int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                          axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                                       }
                                       else
                                       {
                                          Tsp_AttnSys.Text = "دستگاه حضور غیاب غیرفعال می باشد";
                                          this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
                                          Tsp_AttnSys.ForeColor = Color.Red;
                                       }

                                       AttendanceSystemAlert_Butn.SuperTip =
                                          SuperToolTipAttnButn(
                                             new XElement("System",
                                                new XAttribute("device", "Attn"),
                                                new XAttribute("ip", fingerPrintSetting.IP_ADDR),
                                                new XAttribute("stat", Fp1DevIsConnected),
                                                host
                                             )
                                          );
                                    });
                              DeviceOnNetworks.Add(dev);

                              if (dev.PingStatus)
                              {
                                 if (!Fp1DevIsConnected)
                                 {
                                    AttnType_Lov.EditValue = "001";
                                    Fp1DevIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADDR, Convert.ToInt32(fingerPrintSetting.PORT_NUMB));
                                    // fire event for fetch 
                                    axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                                    // New code 
                                    axCZKEM1.OnHIDNum += axCZKEM1_OnHIDNum;
                                    dev.Init = true;
                                 }
                                 if (Fp1DevIsConnected == true)
                                 {
                                    Tsp_AttnSys.Text = "دستگاه حضور غیاب فعال می باشد";
                                    this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
                                    Tsp_AttnSys.ForeColor = Color.Green;
                                    int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                    axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                    // 1402/10/05 * اضافه کردن دستگاه اثرانگشت به لیست
                                    AddFngrDevOpr(axCZKEM1, fingerPrintSetting.IP_ADDR, "پذیرش 1");
                                 }
                                 else
                                 {
                                    Tsp_AttnSys.Text = "دستگاه حضور غیاب غیرفعال می باشد";
                                    this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
                                    Tsp_AttnSys.ForeColor = Color.Red;
                                 }

                                 AttendanceSystemAlert_Butn.SuperTip =
                                    SuperToolTipAttnButn(
                                       new XElement("System",
                                          new XAttribute("device", "Attn"),
                                          new XAttribute("ip", fingerPrintSetting.IP_ADDR),
                                          new XAttribute("stat", Fp1DevIsConnected),
                                          host
                                       )
                                    );
                              }
                           }
                        }
                        #endregion
                        #region 2nd Device
                        if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC2 == host.Attribute("cpu").Value))
                        {
                           var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC2 == host.Attribute("cpu").Value).FirstOrDefault();

                           if (fingerPrintSetting == null) return;

                           if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                           if (fingerPrintSetting.IP_ADR2 != null && fingerPrintSetting.PORT_NUM2 != null)
                           {
                              Tsp_AttnSys.Text = "در حال اتصال به دستگاه حضور غیاب...";
                              this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;
                              Tsp_AttnSys.ForeColor = SystemColors.ControlText;

                              // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                              var dev =
                                 new Device_On_Network(fingerPrintSetting.IP_ADR2)
                                 {
                                    SPort = (int)fingerPrintSetting.PORT_NUM2,
                                    RPort = (int)fingerPrintSetting.PORT_NUM2,
                                    DeviceType = "FngrPrnt",
                                    DeviceName = "دستگاه اثر انگشت کنترل گیت"             
                                 };
                              dev.CallBack = 
                                 new Action(() =>
                                    {
                                       Fp2DevIsConnected = axCZKEM2.Connect_Net(fingerPrintSetting.IP_ADR2, Convert.ToInt32(fingerPrintSetting.PORT_NUM2));
                                       if (!dev.Init)
                                       {
                                          // fire event for fetch 
                                          axCZKEM2.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                                          // New code 
                                          axCZKEM2.OnHIDNum += axCZKEM1_OnHIDNum;
                                          dev.Init = true;
                                       }
                                       if (Fp2DevIsConnected == true)
                                       {
                                          Tsp_AttnSys.Text = "دستگاه حضور غیاب فعال می باشد";
                                          this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
                                          Tsp_AttnSys.ForeColor = Color.Green;
                                          int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                          axCZKEM2.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                                       }
                                       else
                                       {
                                          Tsp_AttnSys.Text = "دستگاه حضور غیاب غیرفعال می باشد";
                                          this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
                                          Tsp_AttnSys.ForeColor = Color.Red;
                                       }

                                       AttendanceSystemAlert_Butn.SuperTip =
                                          SuperToolTipAttnButn(
                                             new XElement("System",
                                                new XAttribute("device", "Attn"),
                                                new XAttribute("ip", fingerPrintSetting.IP_ADR2),
                                                new XAttribute("stat", Fp1DevIsConnected),
                                                host
                                             )
                                          );
                                    });
                              DeviceOnNetworks.Add(dev);

                              if (dev.PingStatus)
                              {
                                 if (!Fp2DevIsConnected)
                                 {
                                    AttnType_Lov.EditValue = "001";
                                    Fp2DevIsConnected = axCZKEM2.Connect_Net(fingerPrintSetting.IP_ADR2, Convert.ToInt32(fingerPrintSetting.PORT_NUM2));
                                    // fire event for fetch 
                                    axCZKEM2.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                                    // New code 
                                    axCZKEM2.OnHIDNum += axCZKEM1_OnHIDNum;
                                    dev.Init = true;
                                 }
                                 if (Fp2DevIsConnected == true)
                                 {
                                    Tsp_AttnSys.Text = "دستگاه حضور غیاب فعال می باشد";
                                    this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
                                    Tsp_AttnSys.ForeColor = Color.Green;
                                    int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                    axCZKEM2.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                    // 1402/10/05 * اضافه کردن دستگاه اثرانگشت به لیست
                                    AddFngrDevOpr(axCZKEM2, fingerPrintSetting.IP_ADR2, "تردد 1");
                                 }
                                 else
                                 {
                                    Tsp_AttnSys.Text = "دستگاه حضور غیاب غیرفعال می باشد";
                                    this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
                                    Tsp_AttnSys.ForeColor = Color.Red;
                                 }

                                 AttendanceSystemAlert_Butn.SuperTip =
                                    SuperToolTipAttnButn(
                                       new XElement("System",
                                          new XAttribute("device", "Attn"),
                                          new XAttribute("ip", fingerPrintSetting.IP_ADR2),
                                          new XAttribute("stat", Fp1DevIsConnected),
                                          host
                                       )
                                    );
                              }
                           }
                        }
                        #endregion
                        #region other wise
                        else if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && (s.ATN1_EVNT_ACTN_TYPE == "002" || s.ATN2_EVNT_ACTN_TYPE == "002" || s.ATN3_EVNT_ACTN_TYPE == "003")))
                        {
                           var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_SYST_TYPE == "002").FirstOrDefault();

                           if (fingerPrintSetting == null) return;

                           if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                           if (fingerPrintSetting.IP_ADDR != null && fingerPrintSetting.PORT_NUMB != null)
                           {
                              Tsp_AttnSys.Text = "در حال اتصال به دستگاه حضور غیاب...";
                              this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;
                              Tsp_AttnSys.ForeColor = SystemColors.ControlText;

                              // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                              var dev =
                                 new Device_On_Network(fingerPrintSetting.IP_ADR3)
                                 {
                                    SPort = (int)fingerPrintSetting.PORT_NUM3,
                                    RPort = (int)fingerPrintSetting.PORT_NUM3,
                                    DeviceType = "FngrPrnt",
                                    DeviceName = "دستگاه اثر انگشت پذیرش"                 
                                 };
                              dev.CallBack = 
                                 new Action(() =>
                                    {
                                       Fp1DevIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADDR, Convert.ToInt32(fingerPrintSetting.PORT_NUMB));
                                       if (!dev.Init)
                                       {
                                          // fire event for fetch 
                                          axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                                          // New code 
                                          axCZKEM1.OnHIDNum += axCZKEM1_OnHIDNum;
                                          dev.Init = true;
                                       }

                                       if (Fp1DevIsConnected == true)
                                       {
                                          AttnType_Lov.EditValue = "002";
                                          Tsp_AttnSys.Text = "دستگاه حضور غیاب فعال می باشد";
                                          this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
                                          Tsp_AttnSys.ForeColor = Color.Green;
                                          int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                          axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                                       }
                                       else
                                       {
                                          AttnType_Lov.EditValue = null;
                                          Tsp_AttnSys.Text = "دستگاه حضور غیاب غیرفعال می باشد";
                                          this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
                                          Tsp_AttnSys.ForeColor = Color.Red;
                                       }

                                       AttendanceSystemAlert_Butn.SuperTip =
                                          SuperToolTipAttnButn(
                                             new XElement("System",
                                                new XAttribute("device", "Attn"),
                                                new XAttribute("ip", fingerPrintSetting.IP_ADDR),
                                                new XAttribute("stat", Fp1DevIsConnected),
                                                host
                                             )
                                          );
                                    });
                              DeviceOnNetworks.Add(dev);

                              if (dev.PingStatus)
                              {
                                 if (!Fp1DevIsConnected)
                                 {
                                    Fp1DevIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADDR, Convert.ToInt32(fingerPrintSetting.PORT_NUMB));
                                    // fire event for fetch 
                                    axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                                    // New code 
                                    axCZKEM1.OnHIDNum += axCZKEM1_OnHIDNum;
                                    dev.Init = true;
                                 }
                                 if (Fp1DevIsConnected == true)
                                 {
                                    AttnType_Lov.EditValue = "002";
                                    Tsp_AttnSys.Text = "دستگاه حضور غیاب فعال می باشد";
                                    this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1219;
                                    Tsp_AttnSys.ForeColor = Color.Green;
                                    int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                    axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                                 }
                                 else
                                 {
                                    AttnType_Lov.EditValue = null;
                                    Tsp_AttnSys.Text = "دستگاه حضور غیاب غیرفعال می باشد";
                                    this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
                                    Tsp_AttnSys.ForeColor = Color.Red;
                                 }

                                 AttendanceSystemAlert_Butn.SuperTip =
                                    SuperToolTipAttnButn(
                                       new XElement("System",
                                          new XAttribute("device", "Attn"),
                                          new XAttribute("ip", fingerPrintSetting.IP_ADDR),
                                          new XAttribute("stat", Fp1DevIsConnected),
                                          host
                                       )
                                    );
                              }
                           }
                        }
                        #endregion
                        else
                        {
                           if (!Fp1DevIsConnected && !Fp2DevIsConnected)
                           {
                              this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1227;
                              Tsp_AttnSys.Text = "***";
                           }
                        }
                     })
                  }
               );
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      void axCZKEM1_OnHIDNum(int CardNumber)
      {
         try
         {
            //return;
            CardNumb_Text.Text = CardNumber.ToString();
            //var control = spc_desktop.Panel1.Controls.OfType<Control>().FirstOrDefault();
            //if (control == null) return;
            //if (control.Name == "ADM_FIGH_F" || control.Name == "ADM_CHNG_F" || control.Name == "BAS_ADCH_F" || control.Name == "OIC_SMSN_F")

            // اگر کارت در سیستم قبلا ثبت شده باشد
            if (!iScsc.Fighters.Any(f => f.FNGR_PRNT_DNRM == CardNumb_Text.Text) && CardNumb_Text.Text != "0")
            {
               Job _InteractWithScsc =
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        //new Job(SendType.SelfToUserInterface, control.Name, 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", CardNumber))}
                        new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", CardNumb_Text.Text))},
                        new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", CardNumb_Text.Text))}                     
                     });
                  _DefaultGateway.Gateway(_InteractWithScsc);
            }
         }
         catch (Exception exc) { /*MessageBox.Show(exc.Message);*/ }
      }
      
      private void axCZKEM1_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
      {
         /*
          * در این قسمت بعد از اینکه کاربر درون دستگاه تعریف شد باید برای اولین بار در سیستم عمل ثبت نام صورت پذیرد
          * این حالت اولیه زمانی رخ میدهد که ما هیچ مشترییی با این شماره اثر انگشت درون سیستم تعریف نشده باشد
          */
         //EnrollNumber = Microsof2t.VisualBasic.Interaction.InputBox("لطفا کد کاربری خود را وارد کنید");
         //if (enrollnumber == EnrollNumber && enrolldate.AddSeconds(5) <= DateTime.Now) { enrollnumber = ""; return; }
         //else { enrollnumber = EnrollNumber; enrolldate = DateTime.Now; }
         //ExtCode.ScreenSaver.KillScreenSaver();

         try
         {
            // 1402/10/28 * if enrollnumber is not valid
            if (EnrollNumber == "0") return;

            if (InvokeRequired)
               Invoke(new Action(() => OnAttTransactionEx(EnrollNumber)));
            else
               OnAttTransactionEx(EnrollNumber);
            return;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void OnAttTransactionEx(string EnrollNumber)
      {
         try
         {
            if (AttnType_Lov.EditValue == null) { AttnType_Lov.EditValue = "003"; }
            if (AttnType_Lov.EditValue.ToString() != "001") 
            { 
               FngrPrnt_Txt.Text = EnrollNumber; 
               if (AttnType_Lov.EditValue.ToString() == "003") 
               { 
                  // نمایش اطلاعات مربوط به کارت خوانده شده
                  ShowInfo_Butn_Click(null, null); 
               } 
               else if(AttnType_Lov.EditValue.ToString() == "004")
               {
                  // باز کردن فرم مربوط به ثبت اطلاعات گروهی برای شارژ کردن کارت های آماده
                  //_DefaultGateway.Gateway(
                  //   new Job(SendType.External, "localhost", "", 163, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fngrprnt", FngrPrnt_Txt.Text)) }
                  //);
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 163 /* Execute Aop_Gimp_F */),
                           new Job(SendType.SelfToUserInterface, "AOP_GIMP_F", 10 /* Actn_CalF_P */){ Input = new XElement("Fighter", new XAttribute("fngrprnt", FngrPrnt_Txt.Text)) }
                        })
                  );
               }
               else if (AttnType_Lov.EditValue.ToString() == "005")
               {
                  if (iScsc.Fighters.Any(f => f.FNGR_PRNT_DNRM == EnrollNumber && f.CONF_STAT == "002"))
                  {
                     CardNumb_Text_Properties_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(CardNumb_Text.Properties.Buttons[4]));
                     CardNumb_Text.Text = "";
                  }
                  else
                  {
                     // باز کردن فرم ثبت مشتری
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                              new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", FngrPrnt_Txt.Text))}
                           })
                     );
                  }

               }
               else if (AttnType_Lov.EditValue.ToString() == "006")
               {
                  string stat = "START"
                        ,macadrs = "DEFAULT";

                  if (iScsc.Aggregation_Operation_Details.Any(a => a.Fighter.FNGR_PRNT_DNRM == EnrollNumber && a.STRT_TIME.Value.Date == DateTime.Now.Date && (a.STAT != "002" && a.STAT != "003")))
                     stat = "STOP";
                  else
                     stat = "START";

                  #region Open Table Expense
                  Job _GetAopBufeF =
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_bufe_f"}
                        }
                     );
                  _DefaultGateway.Gateway(_GetAopBufeF);

                  if (_GetAopBufeF.Output != null)
                  {
                     if (frstVistTablCntlF)
                     {
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "Localhost",
                              new List<Job>
                                 {
                                    new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){
                                       Input = 
                                          new XElement("Request", 
                                             new XAttribute("type", "tp_001"),
                                             new XAttribute("stat", stat),
                                             new XAttribute("macadrs", macadrs),
                                             new XAttribute("fngrprnt", EnrollNumber)
                                          )
                                    }
                                 }
                           )
                        );
                     }
                     else
                     {
                        frstVistTablCntlF = true;
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "Localhost",
                              new List<Job>
                                 {
                                    new Job(SendType.Self, 131 /* Execute Aop_Bufe_F */),
                                    new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){
                                       Input = 
                                          new XElement("Request", 
                                             new XAttribute("type", "tp_001"),
                                             new XAttribute("stat", stat),
                                             new XAttribute("macadrs", macadrs),
                                             new XAttribute("fngrprnt", EnrollNumber)
                                          )
                                    }
                                 }
                           )
                        );
                     }
                  }
                  #endregion
                  
               }
               else if (AttnType_Lov.EditValue.ToString() == "008")
               {
                  // اگر این گزینه باشد که برای مدیریت مجموعه های ورزشی بخواهیم عمل کنیم
                  // به این صورت که کارت خام باشد فرم ثبت نام مشتری باز شود
                  // اگر کارت متعلق به عضویت خاصی باشد فرم پروفایل مشتری باز شود
                  var dev008host = iScsc.External_Devices.FirstOrDefault(ed => ed.SERV_IP_ADRS == xHost.Attribute("ip").Value && ed.STAT == "002" && ed.DEV_TYPE == "001");
                  var Serv = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == EnrollNumber && f.CONF_STAT == "002");
                  if(Serv == null)
                  {
                     // باز کردن فرم مربوط به ثبت نام مشتری
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                              new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", FngrPrnt_Txt.Text))}
                           })
                     );

                     // ارسال پیام خطا به دستگاه
                     if (dev008host != null)
                     {
                        SendCommandDevExpn(
                           "er:          " +
                           "&  " +
                           ":         ", dev008host.DEV_NAME, FngrPrnt_Txt.Text);
                     }
                  }
                  else
                  {
                     // 1402/07/07 * اگر کارت ارسال شده متغلق به دستبند ها باشد فرم درآمد متفرقه باز شود
                     if (iScsc.Dressers.Any(l => l.CMND_SEND == FngrPrnt_Txt.Text))
                     {
                        var _figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == FngrPrnt_Txt.Text);

                        // Check not Lock
                        if (_figh.FIGH_STAT == "001")
                        {
                           // Cancel Request anyway
                           iScsc.CNCL_RQST_F(
                              new XElement("Request",
                                  new XAttribute("rqid", _figh.RQST_RQID)
                              )
                           );
                        }

                        // 1402/11/14 * بررسی اینکه آیا دستبند نیاز به آزاد شدن دارد یا خیر
                        if(iScsc.Dresser_Attendances.Any(da => da.Dresser.CMND_SEND == FngrPrnt_Txt.Text && da.TKBK_TIME == null))
                        {
                           if (MessageBox.Show(this, "آیا با آزاد کردن دستبند موافق هستید؟", "آزاد کردن دستبند", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                           {
                              var _drat = iScsc.Dresser_Attendances.FirstOrDefault(da => da.Dresser.CMND_SEND == FngrPrnt_Txt.Text && da.TKBK_TIME == null);
                              iScsc.ExecuteCommand(string.Format("UPDATE dbo.Dresser_Attendance SET Tkbk_Time = GETDATE() WHERE Rqst_Rqid = {0} AND TKbk_Time IS NULL;", _drat.RQST_RQID));

                              if (MessageBox.Show(this, "آیا مایل هستین فاکتور مجدد صادر شود؟", "فاکتور مجدد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                           }
                           else
                           {
                              // Show Profile
                              // باز کردن فرم مربوط به پروفایل مشتری
                              ShowInfo_Butn_Click(null, null);
                              return;
                           }
                        }

                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "Localhost",
                               new List<Job>
                               {                  
                                  new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                                  new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */)
                                  {
                                     Input = 
                                       new XElement("Request", 
                                           new XAttribute("type", "01"), 
                                           new XElement("Request_Row", 
                                               new XAttribute("fileno", _figh.FILE_NO)
                                           )
                                       )
                                  }
                               })
                        );
                     }
                     else
                        // باز کردن فرم مربوط به پروفایل مشتری
                        ShowInfo_Butn_Click(null, null);

                     // بررسی اینکه مشتری شارژ اعتباری داری یا خیر
                     if (Serv.DPST_AMNT_DNRM > 0)
                     {
                        // اگر مشتری اعتبار دارد
                        if (dev008host != null)
                        {
                           SendCommandDevExpn(
                              "in:          " +
                              "&  " +
                              ":         ", dev008host.DEV_NAME, FngrPrnt_Txt.Text);
                        }
                     }
                     else
                     {
                        // اگر مشتری اعتبار نداشته باشد
                        if (dev008host != null)
                        {
                           SendCommandDevExpn(
                              "er:          " +
                              "&  " +
                              ":         ", dev008host.DEV_NAME, FngrPrnt_Txt.Text);
                        }
                     }
                  }
               }
               else if (AttnType_Lov.EditValue.ToString() == "009" /* بلیط فروشی استخر */)
               {
                  #region قسمت مربوط به بلیط فروشی استخر
                  // اگر این گزینه باشد که برای مدیریت مجموعه های ورزشی بخواهیم عمل کنیم
                  // به این صورت که کارت خام باشد فرم ثبت نام مشتری باز شود
                  // اگر کارت متعلق به عضویت خاصی باشد فرم پروفایل مشتری باز شود
                  var dev008host = iScsc.External_Devices.FirstOrDefault(ed => ed.SERV_IP_ADRS == xHost.Attribute("ip").Value && ed.STAT == "002" && ed.DEV_TYPE == "001");
                  var Serv = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == EnrollNumber && f.CONF_STAT == "002");
                  if (Serv == null)
                  {
                     // باز کردن فرم مربوط به ثبت نام مشتری
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                              new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", FngrPrnt_Txt.Text))}
                           })
                     );

                     // ارسال پیام خطا به دستگاه
                     if (dev008host != null)
                     {
                        SendCommandDevExpn(
                           "er:          " +
                           "&  " +
                           ":         ", dev008host.DEV_NAME, FngrPrnt_Txt.Text);
                     }
                  }
                  else
                  {
                     // باز کردن فرم مربوط به پروفایل مشتری
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                              new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", FngrPrnt_Txt.Text), new XAttribute("formcaller", GetType().Name))}
                           })
                     );
                  }
                  #endregion
               }
               else if (AttnType_Lov.EditValue.ToString() == "010" /* حضور و غیاب بلیط فروشی */)
               {
                  #region قسمت مربوط به حضور و غیاب بلیط فروشی استخر
                  #endregion
               }
               else if(AttnType_Lov.EditValue.ToString() == "011")
               {
                  if (!iScsc.Request_Duplicates.Any(rd => rd.STAT == "002" && rd.CRET_BY == CurrentUser))
                  {
                     _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\disconnect.wav";
                     new Thread(AlarmShow).Start();
                  }
                  else
                  {
                     iScsc.DUP_RQST_P(
                        new XElement("Duplicate",
                            new XAttribute("type", "copy"),
                            new XAttribute("fngrprnt", FngrPrnt_Txt.Text)
                        )
                     );
                     _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\expert.wav";
                     new Thread(AlarmShow).Start();

                     CardNumb_Text.EditValue = null;
                  }
               }
               else if (AttnType_Lov.EditValue.ToString() == "012")
               {
                  #region تعریف کردن دستبند کمدی
                  // first step CardNumb_Txt is not empty
                  if (CardNumb_Text.Text == "") { CardNumb_Text.Focus(); return; }

                  // If not exists must we create in table
                  if(!iScsc.Dressers.Any(d => d.CMND_SEND == FngrPrnt_Txt.Text && d.DRES_NUMB == CardNumb_Text.Text.ToInt32() && d.Computer_Action.COMP_NAME == xHost.Attribute("name").Value))
                  {
                     iScsc.ExecuteCommand(
                        string.Format("INSERT INTO dbo.Dresser (Coma_Code, Code, Dres_Numb, Rec_Stat, Ordr, Cmnd_Send)" + Environment.NewLine +
                        "SELECT Code, 0, {0}, '002', {0}, '{1}' FROM dbo.Computer_Action WHERE Comp_Name = '{2}'",
                        CardNumb_Text.Text,
                        FngrPrnt_Txt.Text,
                        xHost.Attribute("name").Value)
                     );

                     FngrPrnt_Txt.Text = "";
                     CardNumb_Text.Text = (CardNumb_Text.Text.ToInt32() + 1).ToString();

                     _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\expert.wav";
                     new Thread(AlarmShow).Start();
                  }
                  else if (iScsc.Dressers.Any(d => d.CMND_SEND != FngrPrnt_Txt.Text && d.DRES_NUMB == CardNumb_Text.Text.ToInt32() && d.Computer_Action.COMP_NAME == xHost.Attribute("name").Value))
                  {
                     iScsc.ExecuteCommand(
                        string.Format("UPDATE dbo.Dresser SET Cmnd_Send = '{0}' WHERE Dres_Numb = {1} AND Coma_Code IN (Select Code From dbo.Computer_Action WHERE Comp_Name = '{2}');", FngrPrnt_Txt.Text, CardNumb_Text.Text, xHost.Attribute("name").Value)
                     );

                     FngrPrnt_Txt.Text = "";

                     _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\expert.wav";
                     new Thread(AlarmShow).Start();
                  }
                  #endregion
               }
               else if (AttnType_Lov.EditValue.ToString() == "014")
               {
                  #region ارائه و تحویل دستبند کمدی
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){ Input = Keys.Escape },
                           //new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                                 new XElement("Fighter",
                                    new XAttribute("cmndtype", "sendparam"),
                                    new XAttribute("enroll", EnrollNumber),
                                    new XAttribute("checkforce", ModifierKeys.HasFlag(Keys.Control)),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        })
                  );
                  #endregion
               }
               return; 
            }

            // 1396/10/26 * اگر سیستم به صورتی باشد که نرم افزار اپراتور پشت آن قرار ندارد            
            if(iScsc.Computer_Actions.FirstOrDefault(ca => ca.COMP_NAME == xHost.Attribute("name").Value).CHCK_DOBL_ATTN_STAT == "002")
               if (EnrollNumber == oldenrollnumber && MessageBox.Show(this, "شناسایی دوبار انجام شده است، آیا می خواهید دوباره مورد بررسی قرار گیرد؟", "تکرار قرار گیری اثرانگشت اعضا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                  return;

            // 1397/05/10 * تست اینکه آیا سرور برقرار هست یا خیر
            if (SrvrPing_Butn.Appearance.BackColor != Color.LightGreen) return;

            oldenrollnumber = EnrollNumber;

            bool recycleService = false;

            FngrPrnt_Txt.Text = EnrollNumber;
            
            if (
               !iScsc.Fighters
               .Any(f =>
                  f.FNGR_PRNT_DNRM == EnrollNumber &&
                  Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 &&
                  !(f.FGPB_TYPE_DNRM == "002" && f.FGPB_TYPE_DNRM == "003")
               //(Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&                              
               )
            )
            {
               #region Check Disabled Fighter
               var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == EnrollNumber);

               if (figh != null && Convert.ToInt32(figh.ACTV_TAG_DNRM ?? "101") <= 100)
               {
                  if (MessageBox.Show(this, "مشتری مورد نظر در حالت حذف از سیستم قرار گرفته است. مایل به فعال کردن مجدد مشتری هستید؟", "حضور مجدد مشتریی غیرفعال", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                  {
                     // 1396/09/04 * بازیابی کد انگشتی یا کارتی مشتری
                     var fighhist = iScsc.Fighter_Publics.Where(fp => fp.FIGH_FILE_NO == figh.FILE_NO && fp.RECT_CODE == "004" && (fp.FNGR_PRNT ?? "") != "").OrderByDescending(fp => fp.RWNO).FirstOrDefault();
                     if (fighhist != null && MessageBox.Show(this, string.Format("آخرین وضعیت کد انگشتی یا کارت مشتری {0} می باشد آیا مایل به جای گیزینی مجدد هستید؟", fighhist.FNGR_PRNT), "بازیابی کد انگشتی یا کارت مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        fighhist.FNGR_PRNT = "";

                     if (fighhist.FNGR_PRNT == "" && MessageBox.Show(this, "آیا می خواهید که کد انگشتی یا کارت جدیدی به مشتری اختصاص دهید", "الحاق انگشتی یا کارت جدید به مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                     {
                     getfngrprnt:
                        fighhist.FNGR_PRNT = Microsoft.VisualBasic.Interaction.InputBox("EnrollNumber", "Input EnrollNumber");
                        if (fighhist.FNGR_PRNT == "")
                           goto getfngrprnt;
                     }

                     // این قسمت برنامه باید با واحد مربوطه انتقال یابد که پراکندگی کد وجود نداشته باشد
                     #region Disable To Enabled
                     iScsc.AET_RQST_F(
                        new XElement("Process",
                           new XElement("Request",
                              new XAttribute("rqid", 0),
                              new XAttribute("rqtpcode", "014"),
                              new XAttribute("rqttcode", "004"),
                              new XElement("Request_Row",
                                 new XAttribute("fileno", figh.FILE_NO),
                                 new XElement("Fighter_Public",
                                    new XElement("Actv_Tag", "101"),
                                    new XElement("Fngr_Prnt", fighhist.FNGR_PRNT.ToUpper())
                                 )
                              )
                           )
                        )
                     );

                     var Rqst = iScsc.Requests.FirstOrDefault(r => r.Request_Rows.Any(rr => rr.FIGH_FILE_NO == figh.FILE_NO) && r.RQTP_CODE == "014" && r.RQTT_CODE == "004");

                     iScsc.AET_SAVE_F(
                        new XElement("Process",
                           new XElement("Request",
                              new XAttribute("rqid", Rqst.RQID),
                              new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                              new XAttribute("regncode", Rqst.REGN_CODE),
                              new XElement("Request_Row",
                                 new XAttribute("fileno", figh.FILE_NO)
                              )
                           )
                        )
                     );
                     #endregion

                     recycleService = true;
                  }
                  else
                     return;
               }

               if (!recycleService)
               {
                  // 1396/11/15 * اگر سیستم منشی داشته باشد
                  var host = iScsc.Computer_Actions.FirstOrDefault(mb => mb.COMP_NAME == xHost.Attribute("name").Value);
                  if (host.CHCK_ATTN_ALRM == "001")
                  {
                     Job _InteractWithScsc =
                        new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 123 /* Execute Adm_FIGH_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", EnrollNumber))}
                        });
                     _DefaultGateway.Gateway(_InteractWithScsc);
                  }
                  //else
                  {
                     // ارسال پیام خطا در برای دستگاه
                     // 1396/11/15 * 16:45
                     // اگر سیستم حضور غیاب دستگاه های کارتی یا انگشتی باشد که مانیتور داشته باشید می توانیم یک پیام برای دستگاه ارسال کنیم که نمایش دهد
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
                              {
                                 Input = 
                                    new XElement("Request",
                                       new XAttribute("type", "gatecontrol"),
                                       new XAttribute("gateactn", "error")
                                    )
                              }
                           }
                        )
                     );
                  }
               }
               else
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
                  );
               #endregion
            }
            else
            {
               var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == EnrollNumber);
               if(figh == null)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", EnrollNumber))}
                        })
                  );
               }
               // 1396/10/14 * بررسی اینکه آیا مشتری چند کلاس ثبت نام کرده است
               // 1396/10/26 * بررسی اینکه سیستمی که اپرداتور ندارد
               var host = iScsc.Computer_Actions.FirstOrDefault(mb => mb.COMP_NAME == xHost.Attribute("name").Value);

               #region Check Freez Fighter
               var mbfz =
                  iScsc.ExecuteQuery<Data.Member_Ship>(
                     string.Format(
                        @"SELECT M.*
                            FROM Fighter f, Member_Ship m
                           WHERE F.File_No = M.Figh_File_No
                             AND F.Mbfz_Rwno_Dnrm = M.Rwno
                             AND M.Rect_Code = '004'
                             AND M.Vald_Type = '002'
                             AND F.File_No = {0}
                             AND CAST(GETDATE() AS DATE) BETWEEN M.STRT_DATE AND M.END_DATE
                        ", figh.FILE_NO
                     )
                  );

               if (mbfz.Count() >= 1)
               {
                  // 1396/08/01 * 16:02
                  // اگر سیستم حضور غیاب دستگاه های کارتی یا انگشتی باشد که مانیتور داشته باشید می توانیم یک پیام برای دستگاه ارسال کنیم که نمایش دهد
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Request",
                                    new XAttribute("type", "gatecontrol"),
                                    new XAttribute("gateactn", "error")
                                 )
                           }
                        }
                     )
                  );

                  DialogResult result = DialogResult.None;
                  if (/*mtod.CHCK_ATTN_ALRM == null || mtod.CHCK_ATTN_ALRM*/ host.CHCK_ATTN_ALRM == "001")
                     result = MessageBox.Show(this, "هشدار!!!\n\rعضو مربوطه کد خود را بلوکه کرده است." + "\r\n" + "آیا مایل به رسیدگی هستید؟", "خطای حضورغیاب", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);


                  if (result == DialogResult.Yes)
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                              new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                              {
                                 Input = 
                                 new XElement("Fighter",
                                    new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM),
                                    new XAttribute("isnewenroll", false)
                                 )
                              }
                           })
                     );

                  return;
               }
               #endregion

               #region Check Any Member_Ship
               var mbsp =
                  iScsc.ExecuteQuery<Data.Member_Ship>(
                     host.CHCK_ATTN_ALRM == "001" ? 
                        /* منشی پشت سیستم حضور دارد */
                        string.Format(@"SELECT ms.*
                                          FROM Member_Ship ms, Fighter_Public fp, Method mt
                                         WHERE ms.Figh_File_No = {0}
                                           AND ms.Figh_File_No = fp.Figh_File_No
                                           AND fp.Mtod_Code = mt.Code
                                           AND mt.Code = ISNULL({1}, mt.Code)
                                           AND ms.Rect_Code = '004'
                                           AND ms.Type = '001'
                                           AND ms.Vald_Type = '002'
                                           AND ms.Fgpb_Rwno_Dnrm = fp.Rwno
                                           AND ms.Fgpb_Rect_Code_Dnrm = fp.Rect_Code
                                           AND CAST(ms.STRT_DATE as DATE) <= CAST(GETDATE() AS DATE)
                                           AND (ms.Numb_Of_Attn_Mont = 0 OR ms.Numb_Of_Attn_Mont > ms.Sum_Attn_Mont_Dnrm)
                                           AND (mt.Chck_Attn_Alrm = '001' AND CAST(ms.End_Date AS DATE) >= CAST(GETDATE() AS DATE))
                                       ", figh.FILE_NO
                                        , host.MTOD_CODE == null ? "NULL" : host.MTOD_CODE.ToString()) 
                        :
                        /* منشی پشت سیستم حضور ندارد */
                        string.Format(@"SELECT ms.*
                                          FROM Member_Ship ms, Fighter_Public fp, Method mt
                                         WHERE ms.Figh_File_No = {0}
                                           AND ms.Figh_File_No = fp.Figh_File_No
                                           AND fp.Mtod_Code = mt.Code
                                           AND mt.Code = ISNULL({1}, mt.Code)
                                           AND ms.Rect_Code = '004'
                                           AND ms.Type = '001'
                                           AND ms.Vald_Type = '002'
                                           AND ms.Fgpb_Rwno_Dnrm = fp.Rwno
                                           AND ms.Fgpb_Rect_Code_Dnrm = fp.Rect_Code
                                           AND CAST(ms.STRT_DATE as DATE) <= CAST(GETDATE() AS DATE)
                                           AND (ms.Numb_Of_Attn_Mont = 0 OR ms.Numb_Of_Attn_Mont > ms.Sum_Attn_Mont_Dnrm)
                                           AND (mt.Chck_Attn_Alrm = '002' AND CAST(GETDATE() AS DATE) BETWEEN CAST(ms.Strt_Date AS DATE) AND CAST(ms.End_Date AS DATE))
                                       ", figh.FILE_NO
                                        , host.MTOD_CODE == null ? "NULL" : host.MTOD_CODE.ToString()) 
                  ).ToList<Data.Member_Ship>();

               // 1396/12/11 * اصلاح حضور و غیاب مربیان
               if(figh.FGPB_TYPE_DNRM == "003")
               {
                  Job _InteractWithScsc =
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                           new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", EnrollNumber), new XAttribute("mbsprwno", 1), new XAttribute("compname", xHost.Attribute("name").Value), new XAttribute("chckattnalrm", host.CHCK_ATTN_ALRM), new XAttribute("attnsystype", attnsystype))}
                        });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  return;
               }

               if (mbsp.Count() >= 2)
               {
                  _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
                  new Thread(AlarmShow).Start();

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 152 /* Execute Chos_Mbsp_F */),
                           new Job(SendType.SelfToUserInterface, "CHOS_MBSP_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", figh.FILE_NO),
                                 new XAttribute("namednrm", figh.NAME_DNRM),
                                 new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM),
                                 new XAttribute("attnsystype", attnsystype)
                              )
                           }
                        }
                     )
                  );
               }
               else
               {
                  // 1396/10/27 * منشی پشت سیستم حضور ندارد
                  if(host.CHCK_ATTN_ALRM == "002")
                  {
                     mbsp =
                        iScsc.ExecuteQuery<Data.Member_Ship>(
                           string.Format(@"SELECT TOP 1 ms.*
                                             FROM Member_Ship ms, Fighter_Public fp, Method mt
                                            WHERE ms.Figh_File_No = {0}
                                              AND ms.Figh_File_No = fp.Figh_File_No
                                              AND fp.Mtod_Code = mt.Code
                                              AND mt.Code = ISNULL({1}, mt.Code)
                                              AND ms.Rect_Code = '004'
                                              AND ms.Type = '001'
                                              AND ms.Vald_Type = '002'
                                              AND ms.Fgpb_Rwno_Dnrm = fp.Rwno
                                              AND ms.Fgpb_Rect_Code_Dnrm = fp.Rect_Code
                                              AND ( mt.Chck_Attn_Alrm = '002' AND CAST(GETDATE() AS DATE) BETWEEN CAST(ms.Strt_Date AS DATE) AND CAST(ms.End_Date AS DATE) )
                                            ORDER BY ms.Rwno DESC
                                       ", figh.FILE_NO
                                        , host.MTOD_CODE == null ? "NULL" : host.MTOD_CODE.ToString()) 
                        ).ToList<Data.Member_Ship>();                        
                  }
                  Job _InteractWithScsc =
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                           new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", EnrollNumber), new XAttribute("mbsprwno", mbsp.Count() > 0 ? mbsp.FirstOrDefault().RWNO : 0), new XAttribute("compname", xHost.Attribute("name").Value), new XAttribute("chckattnalrm", host.CHCK_ATTN_ALRM), new XAttribute("attnsystype", attnsystype))}
                        });
                  _DefaultGateway.Gateway(_InteractWithScsc);
               }
               #endregion
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      void Stop_FingerPrint()
      {
         if (Fp1DevIsConnected)
         {
            axCZKEM1.Disconnect();
            Fp1DevIsConnected = false;
            Tsp_AttnSys.Text = "دستگاه حضورغیاب غیرفعال شد";
         }
      }

      private void Tm_FingerPrintWorker_Tick(object sender, EventArgs e)
      {
         Tm_FingerPrintWorker.Enabled = false;

         FngrDevList_Flp.Controls.Clear();

         Start_FingerPrint();
         Start_OnlineDresser();
         Start_BarCode();
         Start_GateAttn();
         //Start_ExpnExtr();
         Start_TlgrmBot();
         Start_ZKTFPSensor();
         Start_DetectExternalDevice();
         
         Tm_ShowTime.Enabled = true;
      }

      private bool Start_Enroll_Finger(string enrollid)
      {
         try
         {
            if (Fp1DevIsConnected)
            {
               var result = axCZKEM1.SSR_SetUserInfo(1, enrollid, enrollid, "", 0, true);
               if (axCZKEM1.StartEnrollEx(enrollid, 6, 0))
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
                  return true;
               }
               else { BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red; return false; }
            }
            else if(Fp2DevIsConnected)
            {
               var result = axCZKEM2.SSR_SetUserInfo(1, enrollid, enrollid, "", 0, true);
               if (axCZKEM2.StartEnrollEx(enrollid, 6, 0))
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
                  return true;
               }
               else { BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red; return false;}
            }
            else if (Fp3DevIsConnected)
            {
               var result = axCZKEM3.SSR_SetUserInfo(1, enrollid, enrollid, "", 0, true);
               if (axCZKEM3.StartEnrollEx(enrollid, 6, 0))
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
                  return true;
               }
               else { BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red; return false; }
            }
            return true;
         }
         catch (Exception exc)
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
            MessageBox.Show(exc.Message);
            return false;
         }
      }
      
      private bool Delete_Enroll_Finger(string enrollid)
      {
         try
         {
            if (Fp1DevIsConnected)
            {
               for (int i = 0; i < 10; i++)
               {
                  axCZKEM1.SSR_DelUserTmpExt(1, enrollid, i);
               }
               //axCZKEM1.SSR_DelUserTmpExt(1, enrollid, 6);
               axCZKEM1.DeleteUserInfoEx(1, Convert.ToInt32(enrollid));
               axCZKEM1.ClearSLog(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if(Fp2DevIsConnected)
            {
               for (int i = 0; i < 10; i++)
               {
                  axCZKEM2.SSR_DelUserTmpExt(1, enrollid, i);
               }
               //axCZKEM2.SSR_DelUserTmpExt(1, enrollid, 6);
               axCZKEM2.DeleteUserInfoEx(1, Convert.ToInt32(enrollid));
               axCZKEM2.ClearSLog(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp3DevIsConnected)
            {
               for (int i = 0; i < 10; i++)
               {
                  axCZKEM3.SSR_DelUserTmpExt(1, enrollid, i);
               }
               //axCZKEM3.SSR_DelUserTmpExt(1, enrollid, 6);
               axCZKEM3.DeleteUserInfoEx(1, Convert.ToInt32(enrollid));
               axCZKEM3.ClearSLog(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp4DevIsConnected)
            {
               for (int i = 0; i < 10; i++)
               {
                  axCZKEM4.SSR_DelUserTmpExt(1, enrollid, i);
               }
               //axCZKEM3.SSR_DelUserTmpExt(1, enrollid, 6);
               axCZKEM4.DeleteUserInfoEx(1, Convert.ToInt32(enrollid));
               axCZKEM4.ClearSLog(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp5DevIsConnected)
            {
               for (int i = 0; i < 10; i++)
               {
                  axCZKEM5.SSR_DelUserTmpExt(1, enrollid, i);
               }
               //axCZKEM3.SSR_DelUserTmpExt(1, enrollid, 6);
               axCZKEM5.DeleteUserInfoEx(1, Convert.ToInt32(enrollid));
               axCZKEM5.ClearSLog(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp6DevIsConnected)
            {
               for (int i = 0; i < 10; i++)
               {
                  axCZKEM6.SSR_DelUserTmpExt(1, enrollid, i);
               }
               //axCZKEM3.SSR_DelUserTmpExt(1, enrollid, 6);
               axCZKEM6.DeleteUserInfoEx(1, Convert.ToInt32(enrollid));
               axCZKEM6.ClearSLog(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            return true;
         }
         catch (Exception exc)
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
            MessageBox.Show(exc.Message);
            return false;
         }
      }

      private bool Truncate_Enroll_Fingers()
      {
         try
         {
            if (Fp1DevIsConnected)
            {
               var result = axCZKEM1.ClearKeeperData(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if(Fp2DevIsConnected)
            {
               var result = axCZKEM2.ClearKeeperData(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp3DevIsConnected)
            {
               var result = axCZKEM3.ClearKeeperData(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp4DevIsConnected)
            {
               var result = axCZKEM4.ClearKeeperData(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp5DevIsConnected)
            {
               var result = axCZKEM5.ClearKeeperData(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            if (Fp6DevIsConnected)
            {
               var result = axCZKEM6.ClearKeeperData(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            return true;
         }
         catch (Exception exc)
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
            MessageBox.Show(exc.Message);
            return false;
         }
      }

      private bool Duplicate_Enroll_Fingers(string enrollid)
      {
         try
         {
            string tmpData = "";
            int tmplen = 0;
            int flag = 0;
            var result = axCZKEM1.GetUserTmpExStr(1, enrollid, 6, out flag, out tmpData, out tmplen);
            
            // 1402/10/14 * اگر این گزینه خروجی هیچ داده ای وجود نداشته باشید
            if (tmpData == null) return false;

            if (Fp2DevIsConnected)
            {
               //MessageBox.Show("2nd Device Enrolling");
               result = axCZKEM2.SSR_SetUserInfo(1, enrollid, "", "", 0, true);
               result = axCZKEM2.SetUserTmpExStr(1, enrollid, 6, flag, tmpData);
            }
            if (Fp3DevIsConnected)
            {
               //MessageBox.Show("3rd Device Enrolling");
               result = axCZKEM3.SSR_SetUserInfo(1, enrollid, "", "", 0, true);
               result = axCZKEM3.SetUserTmpExStr(1, enrollid, 6, flag, tmpData);
            }
            if (Fp4DevIsConnected)
            {
               //MessageBox.Show("3rd Device Enrolling");
               result = axCZKEM4.SSR_SetUserInfo(1, enrollid, "", "", 0, true);
               result = axCZKEM4.SetUserTmpExStr(1, enrollid, 6, flag, tmpData);
            }
            if (Fp5DevIsConnected)
            {
               //MessageBox.Show("3rd Device Enrolling");
               result = axCZKEM5.SSR_SetUserInfo(1, enrollid, "", "", 0, true);
               result = axCZKEM5.SetUserTmpExStr(1, enrollid, 6, flag, tmpData);
            }
            if (Fp6DevIsConnected)
            {
               //MessageBox.Show("3rd Device Enrolling");
               result = axCZKEM6.SSR_SetUserInfo(1, enrollid, "", "", 0, true);
               result = axCZKEM6.SetUserTmpExStr(1, enrollid, 6, flag, tmpData);
            }
            return true;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
            return false;
         }
      }

      private bool SetPassword_Enroll_Finger(string enrollid, string password)
      {
         try
         {
            if (Fp1DevIsConnected)
            {
               var result = axCZKEM1.SSR_SetUserInfo(1, enrollid, enrollid, password, 0, true);
               if (result)
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;                  
               }
               else
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
               }
               return result;
            }
            if (Fp2DevIsConnected)
            {
               var result = axCZKEM2.SSR_SetUserInfo(1, enrollid, enrollid, password, 0, true);
               if (result)
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
               }
               return result;
            }
            if (Fp3DevIsConnected)
            {
               var result = axCZKEM3.SSR_SetUserInfo(1, enrollid, enrollid, password, 0, true);
               if (result)
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
               }
               return result;
            }
            if (Fp4DevIsConnected)
            {
               var result = axCZKEM4.SSR_SetUserInfo(1, enrollid, enrollid, password, 0, true);
               if (result)
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
               }
               return result;
            }
            if (Fp5DevIsConnected)
            {
               var result = axCZKEM5.SSR_SetUserInfo(1, enrollid, enrollid, password, 0, true);
               if (result)
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
               }
               return result;
            }
            if (Fp6DevIsConnected)
            {
               var result = axCZKEM6.SSR_SetUserInfo(1, enrollid, enrollid, password, 0, true);
               if (result)
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
               }
               return result;
            }
            return true;
         }
         catch (Exception exc)
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
            MessageBox.Show(exc.Message);
            return false;
         }
      }
      #endregion

      #region External_Device
      List<SerialPort> Exdvs = new List<SerialPort>();
      Server server;
      private void Start_DetectExternalDevice()
      {
         try
         {
            var devs = iScsc.External_Devices.Where(ed => ed.STAT == "002");

            var _listIPHost = xHost.Descendants("IP").Select(ip => ip.Value).ToList();

            #region Saela & Nuoro Company
            #region card reader
            foreach (var cardreader in devs.Where(d => d.DEV_COMP_TYPE == "001" && d.DEV_TYPE == "001"))
            {
               // IF NOT EXISTS SERIAL PORT IN COMPUTER OR NOT PLUGED CARD READER IN DEVICE
               if (SerialPort.GetPortNames().Any(p => p == cardreader.PORT_NAME))
               {
                  // Create Serial Port Instance
                  SerialPort sp = new SerialPort(cardreader.PORT_NAME, (int)cardreader.BAND_RATE) { DataBits = 8, StopBits = StopBits.One, Handshake = Handshake.None, Parity = Parity.None };

                  // Port Open
                  sp.Open();

                  Exdvs.Add(sp);

                  #region Device Stat
                  if (sp.IsOpen)
                  {
                     AttnType_Lov.EditValue = "001";
                     this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1212;
                     Tsp_AttnSys.Text = "سیستم کارت خوان  صائلا فعال";
                     Tsp_AttnSys.ForeColor = Color.Green;
                     AttendanceSystemAlert_Butn.Tag = "001";
                  }
                  else
                  {
                     AttnType_Lov.EditValue = null;
                     this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
                     Tsp_AttnSys.Text = "سیستم کارت خوان صائلا غیرفعال";
                     Tsp_AttnSys.ForeColor = Color.Red;
                  }
                  #endregion

                  if (cardreader.CYCL_READ == "001")
                  {
                     // Normal Card Reader
                  }
                  else if (cardreader.CYCL_READ == "002")
                  {
                     // Encoder Reader
                     sp.DataReceived += SaelaEncoderDevice_DataReceivedHandler;
                     Tm_Exdv.Enabled = true;
                  }
               }
            }
            #endregion

            #region gate control
            foreach (var gate in 
               devs.Where(d => 
                  _listIPHost.Contains(d.SERV_IP_ADRS) && 
                  (
                     // Saela && Nuoro for gate control
                     ((d.DEV_COMP_TYPE == "001" || d.DEV_COMP_TYPE == "003") && d.DEV_TYPE == "006") ||
                     // Nuoro for online locker for action with managment locker room with card reader
                     (d.DEV_COMP_TYPE == "003" && d.DEV_TYPE == "010" && d.ACTN_TYPE == "013") ||
                     // Nuoro for card reader with define for attendance
                     (d.DEV_COMP_TYPE == "003" && d.DEV_TYPE == "001" && (d.ACTN_TYPE == "001" /* حضوری */ || d.ACTN_TYPE == "003" /* نمایش */ || d.ACTN_TYPE == "011" /* کپی کارت گروهی */ || d.ACTN_TYPE == "014" /* ارائه و تحویل دستبند کمدی */))
                  )
            ))
            {
               if(gate.DEV_CON == "001")
               {

               }
               else if(gate.DEV_CON == "002")
               {
                  // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                  var dev =
                     new Device_On_Network(gate.IP_ADRS)
                     {
                        SPort = (int)gate.PORT_SEND,
                        RPort = (int)gate.PORT_RECV,
                        DeviceType = "Gate",
                        DeviceName = gate.DEV_NAME                        
                     };
                  dev.CallBack = 
                     new Action(() =>
                        {
                           // IP Address Setting
                           var lsgate_cb = new TcpListenerX((int)gate.PORT_RECV);
                           dev.TcpListenerX = lsgate_cb;
                           //if (!dev.Init)
                           {
                              // Init Send Instance                  
                              //lsgate_cb.StartListening();

                              lsgate_cb.OnDataRecived += LsGate_OnDataRecived;                              
                              dev.Init = true;
                           }
                        });
                  DeviceOnNetworks.Add(dev);

                  if (dev.PingStatus)
                  {
                     // IP Address Setting
                     var lsgate = new TcpListenerX((int)gate.PORT_RECV);

                     // 1399/12/11 * اضافه شدن گزینه ای اطلاع رسانی بابت اتصال گیت به سرور
                     new Thread(AlarmShow).Start();
                     if (!GameHours_Butn.ToolTip.Contains(gate.IP_ADRS))
                     {
                        GameHours_Butn.ToolTip +=
                           Environment.NewLine +
                           string.Format("Host Server : {0}, Port Recv : {1}, Gate Control : {2}, Port Send : {3}", gate.SERV_IP_ADRS, gate.PORT_RECV, gate.IP_ADRS, gate.PORT_SEND);
                     }

                     // Init Send Instance                  
                     //lsgate.StartListening();
                     lsgate.OnDataRecived += LsGate_OnDataRecived;
                     dev.TcpListenerX = lsgate;
                     dev.Init = true;
                  }
                  //var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x23, 0x00, 0x03, 0x00, 0x00, 0xd4, 0xDD };
                  //SendCommand("192.168.1.200", 6070, cmd);
                  //SendCommand("192.168.1.201", 6070, cmd);
                  //MessageBox.Show(gate.PORT_RECV.ToString() + " is OK");
               }
            }

            #endregion
            #endregion

            //MessageBox.Show(xHost.ToString());            

            #region RelaySoft Company
            #region Device Bilard & CityGame & Reader & Seven Segment & Online LockerS Gym
            // آیا کامپیوتر مورد نظر به عنوان سرور تلقی میشود که باید به بعضی از دستگاه ها پاسخگو باشد
            if (devs.Where(d => d.DEV_COMP_TYPE == "002" && (d.DEV_TYPE == "007" || d.DEV_TYPE == "008" || d.DEV_TYPE == "009" || d.DEV_TYPE == "010" || (d.DEV_TYPE == "001" && d.DEV_CON == "002")) && _listIPHost.Contains(d.SERV_IP_ADRS)).Any())
            {
               var _thisServersListener = devs.Where(d => d.DEV_COMP_TYPE == "002" && (d.DEV_TYPE == "007" || d.DEV_TYPE == "008" || d.DEV_TYPE == "009" || d.DEV_TYPE == "010" || (d.DEV_TYPE == "001" && d.DEV_CON == "002")) && _listIPHost.Contains(d.SERV_IP_ADRS)).Select(d => new { d.SERV_IP_ADRS, d.PORT_SEND }).Distinct().FirstOrDefault();

               // اگر سیستم فعلی به عنوان سرور تلقی میشود باید پورت 6450 که به صورت پیش شرکت میباشد شنود شود
               server = new Server(Server.GetLocalIPAddress(), /*13001*/Convert.ToInt32(_thisServersListener.PORT_SEND));
               GameHours_Butn.ToolTip = string.Format("Server : {0}:{1}", _thisServersListener.SERV_IP_ADRS, _thisServersListener.PORT_SEND);
               server.ClientConnected += Server_ClientConnected;
               server.MessageReceived += Server_MessageReceived;
               server.ClientDisconnected += Server_ClientDisconnected;
               var rickroll = new ResponseEvent()
               {
                  Content = "never gunna give you up",
                  Mode = ContentMode.Contains,
                  Event = Rickroll,
               };
               server.Responses.Add(rickroll);
            }
            #endregion            
            #endregion
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
            ActionCenter_Butn.ToolTip = exc.Message;
         }
      }      

      #region CardReader
      private void Tm_Exdv_Tick(object sender, EventArgs e)
      {
         try
         {
            byte[] data = new byte[] { 0xaa, 0x00, 0x03, 0x25, 0x26, 0x00, 0x00, 0xbb };
            // Listener Encoders
            foreach (var encoder in Exdvs.Where(ed => ed.IsOpen))
            {
               // Request to Encoder for set card on device
               encoder.Write(data, 0, data.Length);               
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaelaEncoderDevice_DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
      {
         try
         {
            SerialPort sp = sender as SerialPort;
            // Response from Encoder
            Thread.Sleep(500);
            byte[] recieve = new byte[sp.BytesToRead];
            sp.Read(recieve, 0, recieve.Length);            

            // Analysis Encoder data received
            
            // if card not in encoder
            if (recieve.Length < 10 || recieve.Length > 11)
               return;

            // if data is ok
            var recieveStr = BitConverter.ToString(recieve).Split('-');
            string enrollNumber = "";
            for (int i = recieveStr.Count() - 6; i < recieveStr.Count() - 2; i++)
			   {
               enrollNumber += recieveStr[i];
			   }
            
            if (InvokeRequired)
               Invoke(new Action(() =>
                  {
                     // اگر کارت قبلا خوانده شده
                     if (FngrPrnt_Txt.Text == enrollNumber) return;

                     axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1);
                  }));
            else
            {
               // اگر کارت قبلا خوانده شده
               if (FngrPrnt_Txt.Text == enrollNumber) return;

               axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion
      #region Gate Send To Server
      public class TcpListenerX
      {
         #region (Private Properties)
         public TcpListener _serverListener = null;
         TcpClient _tcpClient;
         IPAddress localAddr;
         Byte[] bytes;
         NetworkStream _stream;
         #endregion
         public bool IsEnable { set; get; }
         public delegate void DataRecived(int port, byte[] data);
         public event DataRecived OnDataRecived;

         public TcpListenerX(/*string IP,*/ int Port)
         {
            try
            {
               localAddr = IPAddress.Any;//IPAddress.Parse(IP);
               _serverListener = new TcpListener(localAddr, Port);
               IsEnable = false;
               _serverListener.Start();
               _tcpClient = _serverListener.AcceptTcpClient();
               // 1403/01/22 * How to create a TcpClient in C# that can recover from a lost network connection?
               if (_stream != null)
                  _stream.Dispose();
               _stream = _tcpClient.GetStream();
               StartListening();
            }
            catch
            {
               //MessageBox.Show(string.Format("{0}\n\r{1}", "TcpListenerX:" + Port.ToString(), exc.Message));
            }
         }

         public void StartListening()
         {
            try
            {
               // Saela Byte Length : 17
               // Nuoro Byte Length : 12
               IsEnable = true;
               bytes = new Byte[17];
               _stream.BeginRead(bytes, 0, bytes.Length, Callback, null);
            }
            catch
            {
               //MessageBox.Show(string.Format("{0}\n\r{1}", "Start Listening:" + listener.LocalEndpoint.ToString().Split(':')[1], exc.Message));
            }
         }

         private void Callback(IAsyncResult ar)
         {
            try
            {               
               if (!IsEnable)
                  return;
               OnDataRecived(Convert.ToInt32(_serverListener.LocalEndpoint.ToString().Split(':')[1]), bytes);
               _stream.BeginRead(bytes, 0, bytes.Length, Callback, null);
            }
            catch
            {
               //MessageBox.Show(string.Format("{0}\n\r{1}", "CallBack:" + listener.LocalEndpoint.ToString().Split(':')[1], exc.Message));
               //ToolTip = string.Format("{0}\n\r{1}", "CallBack:" + listener.LocalEndpoint.ToString().Split(':')[1], exc.Message);
            }
         }

         public void StopListening()
         {
            IsEnable = false;
            // 1403/01/22 * closes all server
            _stream.Close();
            _stream.Dispose();
            _tcpClient.Close();
            _tcpClient.Dispose();
            _serverListener.Stop();            
         }

         public bool IsAlive()
         {
            try
            {
               if (_tcpClient != null && _tcpClient.Client != null && _tcpClient.Client.Connected)
               {
                  /* pear to the documentation on Poll:
                   * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                   * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                   * -or- true if data is available for reading; 
                   * -or- true if the connection has been closed, reset, or terminated; 
                   * otherwise, returns false
                   */

                  // Detect if client disconnected
                  if (_tcpClient.Client.Poll(0, SelectMode.SelectRead))
                  {
                     byte[] buff = new byte[1];
                     if (_tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        return false;
                     else
                        return true;
                  }
                  return true;
               }
               return false;
            }
            catch
            {
               return false;
            }            
         }
      }

      private void LsGate_OnDataRecived(int port, byte[] recieve)
      {
         // if data is ok
         var recieveStr = BitConverter.ToString(recieve).Split('-');
         //CrntValuGetData_Butn.EditValue = recieveStr;
         //CardOpr_Txt.EditValue = recieveStr;
         string enrollNumber = "";

         #region Data recieved from saela and Nuoro gate control
         var gate = iScsc.External_Devices.Where(ed => (ed.DEV_COMP_TYPE == "001" || ed.DEV_COMP_TYPE == "003") && ed.DEV_TYPE == "006" && ed.STAT == "002" && ed.PORT_RECV == port).FirstOrDefault();
         if (gate != null)
         {
            try
            {
               // 1402/11/05 * اگر میخواهیم چک کنیم که این دستگاه تایم استفاده کردنش هست یا خیر
               /*
                  001	یکشنبه
                  002	دو شنبه
                  003	سه شنبه
                  004	چهار شنبه
                  005	پنج شنبه
                  006	جمعه
                  007	شنبه
                */
               var _weekday = DateTime.Now.DayOfWeek;
               if (gate.External_Device_Weekdays.Count() > 0)
               {
                  if (gate.External_Device_Weekdays
                     .Any(ew => ew.STAT == "002"
                             && ew.WEEK_DAY == string.Format("00{0}", (int)(_weekday + 1)))
                  )
                  {
                     // checking must be Continue
                     if (gate.External_Device_Weekdays
                     .Any(ew => ew.STAT == "002"
                             && ew.WEEK_DAY == string.Format("00{0}", (int)(_weekday + 1))
                             && ew.External_Device_Weekday_Timings.Count() > 0)
                     )
                     {
                        if (!gate.External_Device_Weekdays
                           .Any(ew => ew.STAT == "002"
                                   && ew.WEEK_DAY == string.Format("00{0}", (int)(_weekday + 1))
                                   && ew.External_Device_Weekday_Timings.Any(et => et.STAT == "002"
                                   && DateTime.Now.TimeOfDay >= et.STRT_TIME.Value.TimeOfDay
                                   && DateTime.Now.TimeOfDay <= et.END_TIME.Value.TimeOfDay))
                        )
                        {
                           // Error 
                           if (gate.DEV_COMP_TYPE == "001")
                           {
                              var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                              SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                           }
                           else if (gate.DEV_COMP_TYPE == "003")
                           {
                              var cmd = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                              SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                           }
                           return;
                        }
                     }
                  }
                  else
                  {
                     // Error 
                     if (gate.DEV_COMP_TYPE == "001")
                     {
                        var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                        SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                     }
                     else if (gate.DEV_COMP_TYPE == "003")
                     {
                        var cmd = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                        SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                     }
                     return;
                  }
               }

               // Saela gate control
               if (gate.DEV_COMP_TYPE == "001")
               {
                  for (int i = recieveStr.Count() - 6; i < recieveStr.Count() - 2; i++)
                  {
                     enrollNumber += recieveStr[i];
                  }
               }
               // Nuoro gate control
               else if (gate.DEV_COMP_TYPE == "003")
               {
                  //for (int i = recieveStr.Count() - 6; i < recieveStr.Count() - 2; i++)
                  //{
                  //   enrollNumber += recieveStr[i];
                  //}
                  // Mifare
                  if (gate.CARD_READ_TYPE == "001")
                  {

                  }
                  // RO
                  else if (gate.CARD_READ_TYPE == "002")
                  {
                     enrollNumber =
                        Convert.ToInt32(
                           ConvertHex2Ascii(
                              string.Join("", /*recieveStr.Reverse().Where(i => i != "00").Take(4)*/ recieveStr.Skip(4).Take(6))
                           ),
                           16
                        ).ToString().PadLeft(10, '0');
                     //return;
                  }
               }

               // 1402/11/16 * Send Signal to receive data
               GetValuDataFromExtrDev(gate, enrollNumber);

               // IF NOT VALID ENROLCODE
               if (enrollNumber.In("0004000", "00040000", "00010000", "00000000")) return;

               // 1402/11/02 * اگر نیاز باشه برای کپی برداری از این آیتم استفاده کنیم
               if (AttnType_Lov.EditValue != null && AttnType_Lov.EditValue.ToString() == "011")
               {
                  OnAttTransactionEx(enrollNumber);
                  return;
               }

               // Alarm 
               new Thread(AlarmShow).Start();

               iScsc = new Data.iScscDataContext(ConnectionString);

               if (InvokeRequired)
                  Invoke(new Action(() =>
                  {
                     // اگر کارت قبلا خوانده شده
                     if (FngrPrnt_Txt.Text == enrollNumber) return;
                     FngrPrnt_Txt.Text = enrollNumber;
                  }));
               else
               {
                  // اگر کارت قبلا خوانده شده
                  if (FngrPrnt_Txt.Text == enrollNumber) return;
                  FngrPrnt_Txt.Text = enrollNumber;
               }

               //var gate = iScsc.External_Devices.Where(ed => ed.STAT == "002" && ed.PORT_RECV == port).FirstOrDefault();

               // IF NOT EXISTS ANY SERVICE RETURN AND STOPED!!
               if (!iScsc.Fighters.Any(f => f.FNGR_PRNT_DNRM == enrollNumber))
               {
                  // Error 
                  if (gate.DEV_COMP_TYPE == "001")
                  {
                     var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                     SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                  }
                  else if (gate.DEV_COMP_TYPE == "003")
                  {
                     var cmd = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                  }
                  return;
               }

               var mbsp = new Data.Member_Ship();
               var _gateMtods = gate.External_Device_Link_Methods.Where(gm => gm.STAT == "002").Select(gm => gm.MTOD_CODE);

               // بررسی اینکه برای پرسنل هایی که داریم نیازی به بررسی تردد نداریم
               if (iScsc.Fighters.Any(f => f.FNGR_PRNT_DNRM == enrollNumber && f.FGPB_TYPE_DNRM == "003"))
               {
                  mbsp =
                  iScsc.Member_Ships
                  .Where(ms =>
                     ms.Fighter.FNGR_PRNT_DNRM == enrollNumber &&
                     ms.VALD_TYPE == "002" &&
                     ms.RECT_CODE == "004" &&
                     DateTime.Now.IsBetween(ms.STRT_DATE.Value.Date, ms.END_DATE.Value.Date)
                  ).FirstOrDefault();
               }
               else
               {
                  mbsp =
                     iScsc.Member_Ships
                     .Where(ms =>
                        ms.Fighter.FNGR_PRNT_DNRM == enrollNumber &&
                           //ms.Fighter_Public.MTOD_CODE == gate.MTOD_CODE &&
                        _gateMtods.Contains(ms.Fighter_Public.MTOD_CODE) &&
                        ms.VALD_TYPE == "002" &&
                        ms.RECT_CODE == "004" &&
                        ms.STRT_DATE <= DateTime.Now.Date &&
                        ms.END_DATE >= DateTime.Now.Date &&
                        (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM)
                     ).FirstOrDefault();
               }

               // این گزینه برای مشتریان لحاظ میشود
               if (mbsp == null)
               {
                  // 1398/12/23 * اگرمشتری اخرین جلسه وارد شده و بخواهد که خارج شود
                  var lastinputattn = iScsc.Attendances.Where(a => a.FNGR_PRNT_DNRM == enrollNumber && /*a.MTOD_CODE_DNRM == gate.MTOD_CODE*/_gateMtods.Contains(a.MTOD_CODE_DNRM) && a.ATTN_DATE.Date == DateTime.Now.Date && a.EXIT_TIME == null);
                  if (lastinputattn != null && lastinputattn.Count() >= 1)
                  {
                     // Send [Close] command to Gate
                     // یک خروج به مشتری زده میشود
                     if (gate.DEV_COMP_TYPE == "001")
                     {
                        var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x08, 0x00, 0x00, 0x00, 0xf7, 0xDD };
                        SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                     }
                     else if (gate.DEV_COMP_TYPE == "003")
                     {
                        var cmd = new byte[] { 0xFA, 0xBA, 0x02, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                        SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                     }

                     iScsc.INS_ATTN_P(null, lastinputattn.FirstOrDefault().FIGH_FILE_NO, DateTime.Now, null, "001", lastinputattn.FirstOrDefault().MBSP_RWNO_DNRM, "002", "001");
                     return;
                  }
                  else
                  {
                     // Send [Error] command to gate
                     if (gate.DEV_COMP_TYPE == "001")
                     {
                        var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                        SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                     }
                     else if (gate.DEV_COMP_TYPE == "003")
                     {
                        var cmd = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                        SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                     }
                     return;
                  }
               }

               iScsc.INS_ATTN_P(null, mbsp.FIGH_FILE_NO, DateTime.Now, null, "001", mbsp.RWNO, "002", "001");

               // Find Attendance in today
               var attn = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == mbsp.FIGH_FILE_NO && a.MBSP_RWNO_DNRM == mbsp.RWNO && a.ATTN_DATE == DateTime.Now.Date).OrderByDescending(a => a.ENTR_TIME).FirstOrDefault();

               // Send [Open] or [Close] command to gate
               if (attn == null || attn.EXIT_TIME == null)
               {
                  if (gate.DEV_COMP_TYPE == "001")
                  {
                     // Send [Open] command to gate
                     var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x08, 0x00, 0x00, 0x00, 0xf0, 0xDD };

                     //byte xorByte = 0;
                     //for (int i = 1; i <= cmd.Length - 3; i++)
                     //   xorByte ^= cmd[i];
                     //cmd[cmd.Length - 2] = xorByte;

                     SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                  }
                  else if (gate.DEV_COMP_TYPE == "003")
                  {
                     var cmd = new byte[] { 0xFA, 0xBA, 0x02, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                  }
               }
               else
               {
                  // Send [Close] command to gate
                  if (gate.DEV_COMP_TYPE == "001")
                  {
                     var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x08, 0x00, 0x00, 0x00, 0xf7, 0xDD };
                     SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                  }
                  else if (gate.DEV_COMP_TYPE == "003")
                  {
                     var cmd = new byte[] { 0xFA, 0xBA, 0x02, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
                  }
               }
            }
            catch (Exception exc)
            {
               //MessageBox.Show(exc.Message);
               // Send [Error] command to gate
               if (gate.DEV_COMP_TYPE == "001")
               {
                  var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                  SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
               }
               else if (gate.DEV_COMP_TYPE == "003")
               {
                  var cmd = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                  SendCommand(gate.IP_ADRS, (int)gate.PORT_SEND, cmd);
               }
            }
         }
         #endregion
         #region Data recieved from Nuoro online locker
         var nuorodevs = iScsc.External_Devices.Where(ed => ed.DEV_COMP_TYPE == "003" /* Nuoro */ && (ed.DEV_TYPE == "001" /* Card Reader */ || ed.DEV_TYPE == "010" /* Online Locker */) && ed.STAT == "002" && ed.PORT_RECV == port).FirstOrDefault();
         if (nuorodevs != null)
         {
            if (nuorodevs.CARD_READ_TYPE == "001")
            {

            }
            // RO
            else if (nuorodevs.CARD_READ_TYPE == "002")
            {
               enrollNumber =
                  Convert.ToInt32(
                     ConvertHex2Ascii(
                        string.Join("", /*recieveStr.Reverse().Where(i => i != "00").Take(4)*/ recieveStr.Skip(4).Take(6))
                     ),
                     16
                  ).ToString().PadLeft(10, '0');
               //return;
            }
            // 1402/10/28 * if enrollnumber is not valid
            if (enrollNumber == "0") return;

            // 1402/11/16 * Send Signal to receive data
            GetValuDataFromExtrDev(gate, enrollNumber);

            // Alarm 
            new Thread(AlarmShow).Start();

            // Nuoro for card reader with define for attendance
            if (nuorodevs.DEV_TYPE == "001" && (nuorodevs.ACTN_TYPE == "001" || nuorodevs.ACTN_TYPE == "003" || nuorodevs.ACTN_TYPE == "011" || nuorodevs.ACTN_TYPE == "014"))
            {
               var _tmp = AttnType_Lov.EditValue;
               AttnType_Lov.EditValue = nuorodevs.ACTN_TYPE;
               if (InvokeRequired)
                  Invoke(new Action(() => OnAttTransactionEx(enrollNumber)));
               else
                  OnAttTransactionEx(enrollNumber);
               AttnType_Lov.EditValue = _tmp;
               return;
            }
            // Nuoro for online locker for action with managment locker room with card reader
            else if (nuorodevs.DEV_TYPE == "010" && nuorodevs.ACTN_TYPE == "013")
            {
               //MessageBox.Show(enrollNumber);
               //if (enrollNumber.In("0004000", "00040000", "00010000", "00000000", "AABBCCDD")) { return; }

               OnOpenDresserWithWrist(enrollNumber);
            }
         }
         #endregion
      }

      WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      string _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
      Color _evencolor = Color.YellowGreen, _oddcolor = Color.LimeGreen;
      private void AlarmShow()
      {
         if (InvokeRequired)
         {
            try
            {
               wplayer.URL = _wplayer_url;
               wplayer.controls.play();
            }
            catch { }

            var tempcolor = BackGrnd_Butn.NormalColorA;
            for (int i = 0; i < 5; i++)
            {
               if (i % 2 == 0)
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = _evencolor;//Color.YellowGreen;
               else
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = _oddcolor;//Color.LimeGreen;

               Thread.Sleep(100);
            }
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = tempcolor;

            _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
            _evencolor = Color.YellowGreen; _oddcolor = Color.LimeGreen;
         }
      }

      private void AlarmShow(MaxUi.RoundedButton _obj)
      {
         if (InvokeRequired)
         {
            //_obj.Visible = true;
            try
            {
               wplayer.URL = _wplayer_url;
               wplayer.controls.play();
            }
            catch { }

            var tempcolorA = _obj.NormalColorA;
            var tempcolorB = _obj.NormalColorB;
            for (int i = 0; i < 5; i++)
            {
               if (i % 2 == 0)
                  _obj.NormalColorA = _obj.NormalColorB = _evencolor;//Color.YellowGreen;
               else
                  _obj.NormalColorA = _obj.NormalColorB = _oddcolor;//Color.LimeGreen;

               Thread.Sleep(100);
            }
            _obj.NormalColorA = tempcolorA;
            _obj.NormalColorB = tempcolorB;
            //_obj.Visible = false;
            _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
            _evencolor = Color.Yellow; _oddcolor = Color.Lime;
         }
      }

      public string ConvertHex2Ascii(String hexString)
      {
         try
         {
            string ascii = string.Empty;

            for (int i = 0; i < hexString.Length; i += 2)
            {
               String hs = string.Empty;

               hs = hexString.Substring(i, 2);
               uint decval = System.Convert.ToUInt32(hs, 16);
               char character = System.Convert.ToChar(decval);
               ascii += character;

            }

            return ascii;
         }
         catch {  }

         return string.Empty;
      }     
									
      public string ConvertASCIIToDecimal(string str)
      {
	      string dec = string.Empty;

	      for (int i = 0; i < str.Length; ++i)
	      {
		      string cDec = ((byte)str[i]).ToString();

		      if (cDec.Length < 3)
			      cDec = cDec.PadLeft(3, '0');

		      dec += cDec;
	      }

	      return dec;
      }

      private void SendCommand(String server, int port, byte[] message)
      {
         try
         {
            if (message == null) return;
            TcpClient client = new TcpClient(server, port);

            NetworkStream stream = client.GetStream();

            stream.Write(message, 0, message.Length);

            //var data = new Byte[17];

            //// String to store the response ASCII representation.
            //String responseData = String.Empty;

            //// Read the first batch of the TcpServer response bytes.
            //Int32 bytes = stream.Read(data, 0, data.Length);

            stream.Close();
            client.Close();
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OprtExtDev(XElement xextdev)
      {
         var devtype = xextdev.Attribute("devtype").Value;
         var contype = xextdev.Attribute("contype").Value;
         var cmdtype = xextdev.Attribute("cmdtype").Value;
         string cmdsend = "";
         if(xextdev.Attribute("cmdsend") != null)
            cmdsend = xextdev.Attribute("cmdsend").Value;         

         if(devtype == "001")
         {
            // Card Reader
         }
         else if(devtype == "006")
         {
            // Gate Device
            if(contype == "001")
            {
               // Usb Cable
            }
            else if(contype == "002")
            {
               // Lan Cable
               var ip = xextdev.Attribute("ip").Value;
               var sendport = Convert.ToInt32(xextdev.Attribute("sendport").Value);

               var _gate = iScsc.External_Devices.FirstOrDefault(d => d.DEV_TYPE == "006" && d.IP_ADRS == ip && d.PORT_SEND == sendport && d.STAT == "002");

               byte[] cmdbyte = null;

               switch (cmdtype)
               {
                  case "test":
                     if(_gate.DEV_COMP_TYPE == "001")
                        cmdbyte = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0xFB, 0xDD };
                     else if(_gate.DEV_COMP_TYPE == "003")
                        cmdbyte = new byte[] { 0xFA, 0xBA, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     break;
                  case "gotoonline":
                     if(_gate.DEV_COMP_TYPE == "001")
                        cmdbyte = new byte[]{ 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x23, 0x00, 0x03, 0x00, 0x00, 0xd4, 0xDD };
                     else if(_gate.DEV_COMP_TYPE == "003")
                        cmdbyte = new byte[] { 0xFA, 0xBA, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     break;
                  case "gotooffline":
                     if (_gate.DEV_COMP_TYPE == "001")
                        cmdbyte = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x23, 0x00, 0x03, 0x00, 0x00, 0xd4, 0xDD };
                     else if (_gate.DEV_COMP_TYPE == "003")
                        cmdbyte = new byte[] { 0xFA, 0xBA, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     break;
                  case "open":
                     if(_gate.DEV_COMP_TYPE == "001")
                        cmdbyte = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x08, 0x00, 0x00, 0x00, 0xf0, 0xDD };
                     else if(_gate.DEV_COMP_TYPE == "003")
                        cmdbyte = new byte[] { 0xFA, 0xBA, 0x02, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     break;
                  case "close":
                     if(_gate.DEV_COMP_TYPE == "001")
                        cmdbyte = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x08, 0x00, 0x00, 0x00, 0xf7, 0xDD };
                     else if(_gate.DEV_COMP_TYPE == "003")
                        cmdbyte = new byte[] { 0xFA, 0xBA, 0x02, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     break;  
                  case "error":
                     if (_gate.DEV_COMP_TYPE == "001")
                        cmdbyte = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                     else if(_gate.DEV_COMP_TYPE == "003")
                        cmdbyte = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                     break;
               }

               SendCommand(ip, sendport, cmdbyte);
            }
         }
         else if (devtype.In( "010", "009"))
         {
            if(contype == "002")
            {
               // Lan Cable
               var ip = xextdev.Attribute("ip").Value;
               var sendport = Convert.ToInt32(xextdev.Attribute("sendport").Value);

               var _dev = iScsc.External_Devices.FirstOrDefault(d => d.DEV_TYPE == devtype && d.DEV_CON == contype && d.IP_ADRS == ip);

               // 1402/08/14 * Show service profile for open dresser
               int _rslt = 0;
               if (int.TryParse(cmdtype, out _rslt))
               {
                  CrntOpenBs.DataSource = iScsc.Attendances.Where(a => a.ATTN_DATE.Date == DateTime.Now.Date && a.ATTN_STAT == "002" && (a.DERS_NUMB == _rslt || a.Dresser_Attendances.Any(da => da.DERS_NUMB == _rslt)) && a.EXIT_TIME == null).OrderByDescending(a => a.ENTR_TIME).Take(1);

                  if (CrntOpenBs.List.Count != 0)
                  {
                     var _attn = CrntOpenBs.Current as Data.Attendance;
                     if (_attn != null)
                     {
                        if (_attn.IMAG_RCDC_RCID_DNRM != null)
                        {
                           try
                           {
                              ServProFile_Rb.ImageProfile = null;
                              ServProFile_Rb.ImageVisiable = true;
                              MemoryStream mStream = new MemoryStream();
                              byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _attn.FIGH_FILE_NO))).ToArray();
                              mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                              Bitmap bm = new Bitmap(mStream, false);
                              mStream.Dispose();

                              if (InvokeRequired)
                                 Invoke(new Action(() => ServProFile_Rb.ImageProfile = bm));
                              else
                                 ServProFile_Rb.ImageProfile = bm;

                              ServProFile_Rb.Tag = _attn.FIGH_FILE_NO;
                           }
                           catch { }
                        }
                        else
                        {
                           ServProFile_Rb.ImageProfile = null;
                           ServProFile_Rb.Tag = null;
                        }

                        if (ServProFile_Rb.ImageProfile == null && _attn.Fighter1.SEX_TYPE_DNRM == "002")
                           ServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1148;
                        else if (ServProFile_Rb.ImageProfile == null)
                           ServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1149;
                     }
                     else
                     {
                        ServProFile_Rb.ImageProfile = null;
                        ServProFile_Rb.Tag = null;
                     }
                  }
               }

               if(_dev.DEV_COMP_TYPE == "002")
               {
                  switch (cmdtype)
                  {
                     case "test":
                     case "all":
                        // Test 
                        SendCommandDevExpn("000", _dev.DEV_NAME, "");
                        break;                     
                     default:
                        // Open
                        SendCommandDevExpn(cmdsend, _dev.DEV_NAME, cmdtype);
                        break;
                  }
               }
               else if (_dev.DEV_COMP_TYPE == "003")
               {

                  byte[] cmdbyte = null;
                  switch (cmdtype)
                  {
                     case "test":
                        cmdbyte = new byte[] { 0xFA, 0xCA, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                        break;
                     case "all":
                        cmdbyte = new byte[] { 0xFA, 0xCA, 0x03, 0x88, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                        break;
                     default:
                        var dresNumb = cmdtype.ToInt32();
                        if (dresNumb <= 255)
                           cmdbyte = new byte[] { 0xFA, 0xCA, 0x02, Convert.ToByte(dresNumb.ToString("X2"), 16), 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                        else
                        {
                           var _numbStr = dresNumb.ToString("X4");
                           cmdbyte = new byte[] { 0xFA, 0xCA, 0x02, Convert.ToByte(dresNumb.ToString("X4").Substring(0, 2), 16), Convert.ToByte(dresNumb.ToString("X4").Substring(2), 16), 0x00, 0x00, 0x00, 0x00, 0xEE };
                        }
                        break;
                  }

                  SendCommand(ip, sendport, cmdbyte);
               }
            }
         }
      }
      #endregion
      #region Bilyard Function & CityGame & Reader & Seven Segment & Online LockerS Gym
      public void Rickroll(EventBasedTCP.MessageReceivedEventArgs e)
      {
         e.Client.SendMessage("never gunna let you down");
      }

      private void Server_ClientDisconnected(object sender, ClientToggleEventArgs e)
      {
         Console.WriteLine("Client Disconnected: " + e.ConnectedClient.ConnectAddress);
      }

      private void Server_MessageReceived(object sender, EventBasedTCP.MessageReceivedEventArgs e)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            // input data e.g : {device code} "-" {Card Code}            
            var devName = e.Client.ConnectAddress;
            var fngrPrnt = e.Message;

            //System.Diagnostics.Debug.WriteLine(enrollNumber);
            //new Thread(AlarmShow).Start();
            //return;

            // اگر داده ای درون سیستم از سمت کارتخوان آمده باشد باید تا پاسخ دهی به دستگاه هیچ ورودی دیگری قابل پردازش نیست
            // اگر داده قبلی بر اساس 10 ثانیه هنوز درون بافر باشد باید از لیست حذف شود
            if (lastDataRead.Any(d => d.MacAdrs == devName && d.EnrollNumber == fngrPrnt && DateTime.Now.Subtract(d.LastTimeRead).TotalSeconds >= 10))
            {
               lastDataRead.Remove(lastDataRead.FirstOrDefault(d => d.MacAdrs == devName && d.EnrollNumber == fngrPrnt));
               //System.Diagnostics.Debug.WriteLine("Remove Last Time Read");
            }

            // اگر داده ای از کارتخوان و شماره کارت وجود داشته باشد ورودی جدید را رد میکنیم
            if (lastDataRead.Any(d => d.MacAdrs == devName && d.EnrollNumber == fngrPrnt)) { /*System.Diagnostics.Debug.WriteLine("Reject New Read Data");*/ return; }

            // اگر اطلاعاتی از کارتخوان و کارت عضویت وجود نداشته باشد برای اولین بار آن را ثبت میکنیم
            lastDataRead.Add(new DataReadFromCardReader() { MacAdrs = devName, EnrollNumber = fngrPrnt, LastTimeRead = DateTime.Now });
            //System.Diagnostics.Debug.WriteLine("Add New Data Read");

            //System.Diagnostics.Debug.WriteLine(enrollNumber);
            //new Thread(AlarmShow).Start();
            //return;

            if (InvokeRequired)
            {
               Invoke(
                  new Action(() =>
                  {
                     // ابتدا بررسی میکنیم که داده ورودی مربوط به کدام بخش دستگاه های بازی میشود
                     // 1 - بازی های زمان متغییر مانند بیلیارد
                     // 2 - بازی های زمان ثابت مانند شهربازی
                     //devName = Regex.Replace(devName, "[^0-9]", "");
                     var getInfoDev = iScsc.External_Devices.FirstOrDefault(d => d.DEV_NAME == devName && d.STAT == "002");
                     if (getInfoDev == null) { System.Diagnostics.Debug.WriteLine("Reject because can't found device" + devName); return; }

                     // set Finger Print Data on Text Box
                     FngrPrnt_Txt.Text = fngrPrnt;

                     // 1402/11/16 * Send Signal to receive data
                     GetValuDataFromExtrDev(getInfoDev, fngrPrnt);

                     // Check Exists Service and Valid Card
                     var Serv = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == fngrPrnt);

                     // بدست آوردن آیین نامه اصلی
                     var regl = iScsc.Regulations.FirstOrDefault(rg => rg.REGL_STAT == "002" && rg.TYPE == "001");

                     if(getInfoDev.DEV_TYPE == "001")
                     {
                        axCZKEM1_OnAttTransactionEx(FngrPrnt_Txt.Text, 1, 1, 1, 2016, 05, 10, 09, 31, 50, 20);
                        return;
                     }
                     else if (getInfoDev.DEV_TYPE == "011" /* سیستم کنترلگر ریدر با دستبند */)
                     {
                        #region دستگاه های کنترلگر ریدر های دستبندی
                        // در اولین گام باید چک کنیم که این دستبند به کسی تعلق دارد یا خیر
                        if (iScsc.Dresser_Attendances.Any(da => da.Dresser.CMND_SEND == FngrPrnt_Txt.Text && da.LEND_TIME != null && da.TKBK_TIME == null && da.CONF_STAT == "002"))
                        {
                           SendCommandDevExpn("in", getInfoDev.DEV_NAME, "");

                           // Save History for enter or exit
                           iScsc.ExecuteCommand(
                              string.Format(
                              "INSERT INTO dbo.Dresser_Attendance (Dres_Code, Figh_File_No, Code, Ders_Numb, Rqst_Rqid, Mbsp_Rwno, Mbsp_Rect_Code, Conf_Stat)" + Environment.NewLine +
                              "SELECT TOP 1 Dres_Code, Figh_File_No, 0, Ders_Numb, Rqst_Rqid, Mbsp_Rwno, Mbsp_Rect_Code, Conf_Stat FROM dbo.Dresser_Attendance da" + Environment.NewLine +
                              "WHERE da.Dres_Code IN (SELECT Code FROM dbo.Dresser d WHERE d.Cmnd_Send = '{0}')" + Environment.NewLine +
                              "  AND da.Tkbk_Time IS NULL AND da.Lend_Time IS NOT NULL AND da.Conf_Stat = '002'" + Environment.NewLine +
                              "ORDER BY da.CRET_DATE DESC",
                              FngrPrnt_Txt.Text
                              )
                           );
                        }
                        else
                           SendCommandDevExpn("er", getInfoDev.DEV_NAME, "");
                        #endregion

                        return;
                     }

                     // اگر مشتری وجود نداشته یا اینکه مشتری اصلا سپرده نداشته باشد
                     if (Serv == null || (regl.AMNT_TYPE == "001" && Serv.DPST_AMNT_DNRM < 10000) || (regl.AMNT_TYPE == "002" && Serv.DPST_AMNT_DNRM < 1000))
                     {
                        #region اگر مشتری وجود نداشته یا اینکه مشتری اصلا سپرده نداشته باشد
                        // اگر کارت عضویت خام باشد
                        if (Serv == null)
                        {
                           if(getInfoDev.DEV_TYPE == "007")
                              SendCommandDevExpn("er", devName, fngrPrnt);
                           else if (getInfoDev.DEV_TYPE.In("008" /* مدیریت مجتمع */))
                           {
                              var devExpn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == getInfoDev.EXPN_CODE);

                              SendCommandDevExpn(
                                 "er:" + "0".PadLeft(10, ' ') +
                                 "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                                 ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' '), devName, fngrPrnt);
                           }

                           // باز کردن فرم ثبت نام مشتری
                           Job _InteractWithScsc =
                              new Job(SendType.External, "Localhost",
                                 new List<Job>
                                    {
                                       new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                                       new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", fngrPrnt))}
                                    });
                           _DefaultGateway.Gateway(_InteractWithScsc);
                        }

                        if(getInfoDev.DEV_TYPE == "007")
                           SendCommandDevExpn("er", devName, fngrPrnt);
                        else
                           if (Serv != null)
                           {
                              var devExpn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == getInfoDev.EXPN_CODE);
                              // مجوز اجرای بازی
                              SendCommandDevExpn(
                                 "er:" + (Serv.DPST_AMNT_DNRM.Value).ToString("n0").PadLeft(10, ' ') +
                                 "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                                 ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' '), devName, fngrPrnt);
                           }
                           else
                           {
                              var devExpn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == getInfoDev.EXPN_CODE);
                              // مجوز اجرای بازی
                              if (devExpn != null)
                              {
                                 SendCommandDevExpn(
                                    "er:" + "0".PadLeft(10, ' ') +
                                    "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                                    ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' '), devName, fngrPrnt);
                              }
                              else
                              {
                                 SendCommandDevExpn(
                                    "er:" + "0".PadLeft(10, ' ') +
                                    "&" + "  " +
                                    ":" + "         ", devName, fngrPrnt);
                              }
                           }
                        return;
                        #endregion
                     }

                     if (getInfoDev.DEV_TYPE == "007" /* بازی های زمان متغییر مانند بیلیارد */)
                     {
                        #region بارگذاری فرم مربوط به رزرو میز
                        Job _GetAopBufeF =
                           new Job(SendType.External, "localhost",
                              new List<Job>
                                 {
                                    new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_bufe_f"}
                                 }
                           );
                        _DefaultGateway.Gateway(_GetAopBufeF);
                        #endregion

                        #region اگر فرم رزرو میز درون حافظه قرار گرفت
                        if (_GetAopBufeF.Output != null)
                        {
                           if (frstVistTablCntlF)
                           {
                              #region برای روالهای بعدی دریافت ورودی
                              _DefaultGateway.Gateway(
                                 new Job(SendType.External, "Localhost",
                                    new List<Job>
                                       {
                                          new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){
                                             Input = 
                                                new XElement("Request", 
                                                   new XAttribute("type", "tp_001"),
                                                   new XAttribute("fngrprnt", fngrPrnt),
                                                   new XAttribute("macadrs", devName),
                                                   new XAttribute("expncode", getInfoDev.EXPN_CODE ?? 0)
                                                )
                                          }
                                       }
                                 )
                              );
                              #endregion
                           }
                           else
                           {
                              #region اگر برای اولین بار فرم میز باز میشود
                              frstVistTablCntlF = true;
                              _DefaultGateway.Gateway(
                                 new Job(SendType.External, "Localhost",
                                    new List<Job>
                                       {
                                          new Job(SendType.Self, 131 /* Execute Aop_Bufe_F */),
                                          new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){
                                             Input = 
                                                new XElement("Request", 
                                                   new XAttribute("type", "tp_001"),
                                                   new XAttribute("fngrprnt", fngrPrnt),
                                                   new XAttribute("macadrs", devName),
                                                   new XAttribute("expncode", getInfoDev.EXPN_CODE ?? 0)
                                                )
                                          }
                                       }
                                 )
                              );
                              #endregion
                           }
                        }
                        #endregion
                     }
                     else if (getInfoDev.DEV_TYPE == "008" /* بازی های زمان ثابت مانند شهربازی */)
                     {
                        #region فعالیت های مربوط به دستگاه های بازی با زمان ثابت
                        var devExpn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == getInfoDev.EXPN_CODE);
                        // اگر مبلغ بازی از میزان اعتبار مشتری بیشتر باشد
                        if (Serv.DPST_AMNT_DNRM < devExpn.PRIC)
                        {
                           SendCommandDevExpn(
                              "er:" + (Serv.DPST_AMNT_DNRM.Value).ToString("n0").PadLeft(10, ' ') +
                              "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                              ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' '), devName, fngrPrnt);
                           return;
                        }

                        // بررسی اینکه آیا مشتری قبلا کارت خود را وارد نکرده باشد و دوباره کارت نزده باشد 
                        // نکته : اگر مشتری کارت زده باشد و دوره بازی تمام شده باشد می تواند دوباره کارت بزند و بازی کند
                        var lastRqst016 = iScsc.VF_Request_Changing(null).Where(r => r.RQTP_CODE == "016" && r.SAVE_DATE.Value.Date == DateTime.Now.Date && iScsc.Payment_Details.Any(pd => pd.PYMT_RQST_RQID == r.RQID && pd.EXPN_CODE == devExpn.CODE)).OrderByDescending(r => r.SAVE_DATE).Take(1).FirstOrDefault();

                        // اگر درخواستی برای دستگاه بازی برای مشتری وجود داشته باشه
                        if (lastRqst016 != null)
                        {
                           // اگر بازه زمانی برای مشتری تمام شده باشد با کسر اعتبار جدید می تواند دوباره بازی کند
                           if (!DateTime.Now.IsBetween((DateTime)lastRqst016.SAVE_DATE, (DateTime)lastRqst016.SAVE_DATE.Value.AddMinutes(devExpn.MIN_TIME.Value.Minute).AddSeconds(devExpn.MIN_TIME.Value.Second)))
                           {
                              // مجوز اجرای بازی
                              SendCommandDevExpn(
                                 "st:" + (Serv.DPST_AMNT_DNRM.Value - devExpn.PRIC).ToString("n0").PadLeft(10, ' ') +
                                 "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                                 ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' '), devName, fngrPrnt);

                              var xRet = new XElement("Result");
                              // ثبت درخواست و کسر اعتبار از مشتری
                              iScsc.RunnerdbCommand(
                                 new XElement("Router_Command",
                                     new XAttribute("subsys", 5),
                                     new XAttribute("cmndcode", 100),
                                     new XAttribute("refsubsys", 5),
                                     new XAttribute("refcode", new Random().Next()),
                                     new XAttribute("refnumb", fngrPrnt),
                                     new XAttribute("fngrprnt", fngrPrnt),
                                     new XAttribute("amnt", devExpn.PRIC),
                                     new XAttribute("pymtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                                     new XAttribute("pymtmtod", "005"),
                                     new XElement("Expense",
                                         new XAttribute("tarfcode", devExpn.ORDR_ITEM),
                                         new XAttribute("tarfdate", DateTime.Now.ToString("yyyy-MM-dd")),
                                         new XAttribute("expnpric", devExpn.PRIC),
                                         new XAttribute("rqtpcode", "016"),
                                         new XAttribute("numb", 1),
                                         devExpn.EXPN_DESC
                                     )
                                 ),
                                 ref xRet
                              );                             
                           }
                           else
                           {
                              // مجوز اجرای بازی
                              SendCommandDevExpn(
                                 "df:" + (Serv.DPST_AMNT_DNRM.Value).ToString("n0").PadLeft(10, ' ') +
                                 "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                                 ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' '), devName, fngrPrnt);
                           }
                        }
                        else
                        {
                           // مجوز اجرای بازی
                           SendCommandDevExpn(
                              "st:" + (Serv.DPST_AMNT_DNRM.Value - devExpn.PRIC).ToString("n0").PadLeft(10, ' ') +
                              "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') + 
                              ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' ') , devName, fngrPrnt);

                           var xRet = new XElement("Result");
                           // ثبت درخواست و کسر اعتبار از مشتری
                           iScsc.RunnerdbCommand(
                              new XElement("Router_Command",
                                  new XAttribute("subsys", 5),
                                  new XAttribute("cmndcode", 100),
                                  new XAttribute("refsubsys", 5),
                                  new XAttribute("refcode", new Random().Next()),
                                  new XAttribute("refnumb", fngrPrnt),
                                  new XAttribute("fngrprnt", fngrPrnt),
                                  new XAttribute("amnt", devExpn.PRIC),
                                  new XAttribute("pymtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                                  new XAttribute("pymtmtod", "005"),
                                  new XElement("Expense",
                                      new XAttribute("tarfcode", devExpn.ORDR_ITEM),
                                      new XAttribute("tarfdate", DateTime.Now.ToString("yyyy-MM-dd")),
                                      new XAttribute("expnpric", devExpn.PRIC),
                                      new XAttribute("rqtpcode", "016"),
                                      new XAttribute("numb", 1),
                                      devExpn.EXPN_DESC
                                  )
                              ),
                              ref xRet
                           );
                        }
                        #endregion
                     }                     
                  })
               );
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
         
      }

      private void Server_ClientConnected(object sender, ClientToggleEventArgs e)
      {
         //Console.WriteLine("Client Connected: " + e.ConnectedClient.ConnectAddress);
         try
         {
            new Thread(AlarmShow).Start();
            if (!GameHours_Butn.ToolTip.Contains(e.ConnectedClient.ConnectAddress))
            {
               GameHours_Butn.ToolTip +=
                  Environment.NewLine +
                  string.Format("{2} - Device : {0}:{1}", e.ConnectedClient.ConnectAddress, e.ConnectedClient.Port, server.ConnectedClients.Count);
            }

            //server.ConnectedClients.Where(c => c.ConnectAddress == e.ConnectedClient.ConnectAddress && c.Port != e.ConnectedClient.Port ).ToList().ForEach(c => server.ConnectedClients.Remove(c));

            iScsc = new Data.iScscDataContext(ConnectionString);
            // input data e.g : {device code} "-" {Card Code}            
            var devName = e.ConnectedClient.ConnectAddress;
            var getInfoDev = iScsc.External_Devices.FirstOrDefault(d => d.DEV_NAME == devName && d.STAT == "002");
            if (getInfoDev.DEV_COMP_TYPE == "002" && getInfoDev.DEV_TYPE == "008")
            {
               var devExpn = iScsc.Expenses.FirstOrDefault(ex => ex.CODE == getInfoDev.EXPN_CODE);
               // مجوز اجرای بازی
               SendCommandDevExpn(
                  "df:" + "0".PadLeft(10, ' ') +
                  "&" + devExpn.MIN_TIME.Value.Minute.ToString().PadLeft(2, ' ') +
                  ":" + devExpn.PRIC.ToString("n0").PadLeft(9, ' '), devName, "");
            }
            else if(getInfoDev.DEV_COMP_TYPE == "002" && getInfoDev.DEV_TYPE == "001")
            {
               //SendCommandDevExpn(
               //   "df:" + "WellCome".PadLeft(13, ' ') +
               //   "&" + "Genetic Gym".PadLeft(16, ' '), devName, ""
               //);
               AttnType_Lov.EditValue = getInfoDev.ACTN_TYPE;
               this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1212;
            }
         }
         catch { }
      }
      #endregion
      #endregion

      #region Device_On_Network
      class Device_On_Network
      {
         public Device_On_Network(string ipAddress)
         {
            IPAddress = ipAddress;
            Ping();
         }
         public string IPAddress { get; set; }
         public int SPort { get; set; }
         public int RPort { get; set; }
         public bool PingStatus { get; set; }
         public string DeviceType { get; set; }
         public string DeviceName { get; set; }
         public bool Init { get; set; }
         public TcpListenerX TcpListenerX { get; set; }

         public Action CallBack { get; set; }
         public void Ping()
         {
            try
            {
               Ping ping = new Ping();
               var pingstatus = ping.Send(IPAddress, 5000);

               if (pingstatus.Status == IPStatus.Success)
               {
                  PingStatus = true;
               }
               else
               {
                  PingStatus = false;
               }
            }
            catch { }
         }
         
         public void Stop()
         {
            TcpListenerX.StopListening();
         }

         public bool IsALive()
         {
            return TcpListenerX.IsAlive();
         }
      }

      private List<Device_On_Network> DeviceOnNetworks = new List<Device_On_Network>();
      
      #endregion

      #region Online Dresser
      // فعال سازی سیستم قفل کمد های انلاین
      private void Start_OnlineDresser()
      {
         try
         {
            _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
                  {
                     AfterChangedOutput =
                     new Action<object>((output) =>
                     {
                        var host = output as XElement;

                        xHost = host;
                        #region 3st Device
                        if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC3 == host.Attribute("cpu").Value))
                        {
                           var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC3 == host.Attribute("cpu").Value).FirstOrDefault();

                           if (fingerPrintSetting == null) return;

                           // اگر حضور غیاب با دستگاه انگشتی نباشد
                           if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                           // اگر حضور و غیاب با دستگاه انگشتی باشد و ارتباط را چک میکنیم
                           if (fingerPrintSetting.IP_ADR3 != null && fingerPrintSetting.IP_ADR3.Length >= 10 && fingerPrintSetting.PORT_NUM3 != null)
                           {
                              // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                              var dev =
                                 new Device_On_Network(fingerPrintSetting.IP_ADR3)
                                 {
                                    SPort = (int)fingerPrintSetting.PORT_NUM3,
                                    RPort = (int)fingerPrintSetting.PORT_NUM3,
                                    DeviceType = "FngrPrnt",
                                    DeviceName = "دستگاه اثر انگشت رختکن"
                                 };
                              dev.CallBack = 
                                 new Action(() =>
                                    {                                       
                                       Fp3DevIsConnected = axCZKEM3.Connect_Net(fingerPrintSetting.IP_ADR3, Convert.ToInt32(fingerPrintSetting.PORT_NUM3));
                                       if (!dev.Init)
                                       {
                                          // fire event for fetch 
                                          axCZKEM3.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                          dev.Init = true;
                                       }
                                       
                                       if (Fp3DevIsConnected)
                                       {
                                          OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                          int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                          axCZKEM3.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                          BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                       }
                                       else
                                       {
                                          OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                       }
                                    });
                              DeviceOnNetworks.Add(dev);

                              if (dev.PingStatus)
                              {
                                 if (!Fp3DevIsConnected)
                                 {
                                    Fp3DevIsConnected = axCZKEM3.Connect_Net(fingerPrintSetting.IP_ADR3, Convert.ToInt32(fingerPrintSetting.PORT_NUM3));
                                    // fire event for fetch 
                                    axCZKEM3.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                    dev.Init = true;
                                 }
                                 if (Fp3DevIsConnected == true)
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                    int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                    axCZKEM3.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                    // 1402/10/05 * اضافه کردن دستگاه اثرانگشت به لیست
                                    AddFngrDevOpr(axCZKEM3, fingerPrintSetting.IP_ADR3, "رختکن 1");

                                    BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                 }
                                 else
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                 }
                              }
                           }
                        }
                        #endregion
                        #region 4st Device
                        if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC4 == host.Attribute("cpu").Value))
                        {
                           var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC4 == host.Attribute("cpu").Value).FirstOrDefault();

                           if (fingerPrintSetting == null) return;

                           // اگر حضور غیاب با دستگاه انگشتی نباشد
                           if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                           // اگر حضور و غیاب با دستگاه انگشتی باشد و ارتباط را چک میکنیم
                           if (fingerPrintSetting.DEV4_STAT == "002" && fingerPrintSetting.IP_ADR4 != null && fingerPrintSetting.IP_ADR4.Length >= 10 && fingerPrintSetting.PORT_NUM4 != null)
                           {
                              // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                              var dev =
                                 new Device_On_Network(fingerPrintSetting.IP_ADR4)
                                 {
                                    SPort = (int)fingerPrintSetting.PORT_NUM4,
                                    RPort = (int)fingerPrintSetting.PORT_NUM4,
                                    DeviceType = "FngrPrnt",
                                    DeviceName = "دستگاه اثر انگشت رختکن"
                                 };
                              dev.CallBack =
                                 new Action(() =>
                                 {
                                    Fp4DevIsConnected = axCZKEM4.Connect_Net(fingerPrintSetting.IP_ADR4, Convert.ToInt32(fingerPrintSetting.PORT_NUM4));
                                    if (!dev.Init)
                                    {
                                       // fire event for fetch 
                                       axCZKEM4.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                       dev.Init = true;
                                    }

                                    if (Fp4DevIsConnected)
                                    {
                                       OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                       int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                       axCZKEM4.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                       BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                    }
                                    else
                                    {
                                       OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                    }
                                 });
                              DeviceOnNetworks.Add(dev);

                              if (dev.PingStatus)
                              {
                                 if (!Fp4DevIsConnected)
                                 {
                                    Fp4DevIsConnected = axCZKEM4.Connect_Net(fingerPrintSetting.IP_ADR4, Convert.ToInt32(fingerPrintSetting.PORT_NUM4));
                                    // fire event for fetch 
                                    axCZKEM4.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                    dev.Init = true;
                                 }
                                 if (Fp4DevIsConnected == true)
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                    int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                    axCZKEM4.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                    // 1402/10/05 * اضافه کردن دستگاه اثرانگشت به لیست
                                    AddFngrDevOpr(axCZKEM4, fingerPrintSetting.IP_ADR4, "رختکن 2");

                                    BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                 }
                                 else
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                 }
                              }
                           }
                        }
                        #endregion
                        #region 5st Device
                        if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC5 == host.Attribute("cpu").Value))
                        {
                           var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC5 == host.Attribute("cpu").Value).FirstOrDefault();

                           if (fingerPrintSetting == null) return;

                           // اگر حضور غیاب با دستگاه انگشتی نباشد
                           if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                           // اگر حضور و غیاب با دستگاه انگشتی باشد و ارتباط را چک میکنیم
                           if (fingerPrintSetting.DEV5_STAT == "002" && fingerPrintSetting.IP_ADR5 != null && fingerPrintSetting.IP_ADR5.Length >= 10 && fingerPrintSetting.PORT_NUM5 != null)
                           {
                              // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                              var dev =
                                 new Device_On_Network(fingerPrintSetting.IP_ADR5)
                                 {
                                    SPort = (int)fingerPrintSetting.PORT_NUM5,
                                    RPort = (int)fingerPrintSetting.PORT_NUM5,
                                    DeviceType = "FngrPrnt",
                                    DeviceName = "دستگاه اثر انگشت رختکن"
                                 };
                              dev.CallBack =
                                 new Action(() =>
                                 {
                                    Fp5DevIsConnected = axCZKEM5.Connect_Net(fingerPrintSetting.IP_ADR5, Convert.ToInt32(fingerPrintSetting.PORT_NUM5));
                                    if (!dev.Init)
                                    {
                                       // fire event for fetch 
                                       axCZKEM5.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                       dev.Init = true;
                                    }

                                    if (Fp5DevIsConnected)
                                    {
                                       OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                       int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                       axCZKEM5.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                       BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                    }
                                    else
                                    {
                                       OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                    }
                                 });
                              DeviceOnNetworks.Add(dev);

                              if (dev.PingStatus)
                              {
                                 if (!Fp5DevIsConnected)
                                 {
                                    Fp5DevIsConnected = axCZKEM5.Connect_Net(fingerPrintSetting.IP_ADR5, Convert.ToInt32(fingerPrintSetting.PORT_NUM5));
                                    // fire event for fetch 
                                    axCZKEM5.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                    dev.Init = true;
                                 }
                                 if (Fp5DevIsConnected == true)
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                    int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                    axCZKEM5.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                    // 1402/10/05 * اضافه کردن دستگاه اثرانگشت به لیست
                                    AddFngrDevOpr(axCZKEM5, fingerPrintSetting.IP_ADR5, "رختکن 3");

                                    BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                 }
                                 else
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                 }
                              }
                           }
                        }
                        #endregion
                        #region 6st Device
                        if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC6 == host.Attribute("cpu").Value))
                        {
                           var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC6 == host.Attribute("cpu").Value).FirstOrDefault();

                           if (fingerPrintSetting == null) return;

                           // اگر حضور غیاب با دستگاه انگشتی نباشد
                           if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                           // اگر حضور و غیاب با دستگاه انگشتی باشد و ارتباط را چک میکنیم
                           if (fingerPrintSetting.DEV6_STAT == "002" && fingerPrintSetting.IP_ADR6 != null && fingerPrintSetting.IP_ADR6.Length >= 10 && fingerPrintSetting.PORT_NUM6 != null)
                           {
                              // 1398/12/25 * اضافه کردن دستگاه به لیست دستگاه های داخل شبکه
                              var dev =
                                 new Device_On_Network(fingerPrintSetting.IP_ADR6)
                                 {
                                    SPort = (int)fingerPrintSetting.PORT_NUM6,
                                    RPort = (int)fingerPrintSetting.PORT_NUM6,
                                    DeviceType = "FngrPrnt",
                                    DeviceName = "دستگاه اثر انگشت رختکن"
                                 };
                              dev.CallBack =
                                 new Action(() =>
                                 {
                                    Fp6DevIsConnected = axCZKEM6.Connect_Net(fingerPrintSetting.IP_ADR6, Convert.ToInt32(fingerPrintSetting.PORT_NUM6));
                                    if (!dev.Init)
                                    {
                                       // fire event for fetch 
                                       axCZKEM6.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                       dev.Init = true;
                                    }

                                    if (Fp6DevIsConnected)
                                    {
                                       OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                       int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                       axCZKEM6.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                       BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                    }
                                    else
                                    {
                                       OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                    }
                                 });
                              DeviceOnNetworks.Add(dev);

                              if (dev.PingStatus)
                              {
                                 if (!Fp6DevIsConnected)
                                 {
                                    Fp6DevIsConnected = axCZKEM6.Connect_Net(fingerPrintSetting.IP_ADR6, Convert.ToInt32(fingerPrintSetting.PORT_NUM6));
                                    // fire event for fetch 
                                    axCZKEM6.OnAttTransactionEx += axCZKEM3_OnAttTransactionEx;
                                    dev.Init = true;
                                 }
                                 if (Fp6DevIsConnected == true)
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین فعال می باشد";
                                    int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                                    axCZKEM6.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                                    // 1402/10/05 * اضافه کردن دستگاه اثرانگشت به لیست
                                    AddFngrDevOpr(axCZKEM6, fingerPrintSetting.IP_ADR6, "رختکن 4");

                                    BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
                                 }
                                 else
                                 {
                                    OnlineDres_Butn.ToolTip = "سیستم کمد های انلاین غیرفعال می باشد";
                                 }
                              }
                           }
                        }
                        #endregion
                     })
                  }
               );
         }
         catch { BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red; }
      }

      private void OnOpenDresser(string EnrollNumber)
      {
         try
         {
            //_wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\tick.wav";
            if (RSignalOpen_Butn.Tag == null)
            {
               RSignalOpen_Butn.Tag = "key";
               LSignalOpen_Butn.Tag = null;
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(RSignalOpen_Butn))).Start();               
            }
            else
            {
               RSignalOpen_Butn.Tag = null;
               LSignalOpen_Butn.Tag = "key";
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(LSignalOpen_Butn))).Start();               
            }

            Partners_Butn.Visible = PrtnrCont_Butn.Visible = PrtnrPos_Butn.Visible = PartnerDresNum_Butn.Visible = false;
            PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = PartnerDresNum_Butn.Visible = false;
            // شماره کد انگشتی را وارد باکس میکنیم
            OnlineDres_Butn.Focus();
            OnlineDres_Butn.Text = EnrollNumber;

            // ابتدا پیدا میکنیم که امروز کدام ردیف حضور و غیاب را داریم
            var attncode = iScsc.Attendances.Where(a => a.FNGR_PRNT_DNRM == EnrollNumber && a.ATTN_DATE.Date == DateTime.Now.Date && a.EXIT_TIME == null).Max(a => a.CODE);

            // ثبت ساعت باز کردن کمد            
            iScsc.INS_DART_P(attncode, null, null);

            // اینجا باید شماره سریال پورت را پیدا کنیم و پیام را بهش ارسال کنیم
            var dresrattn = iScsc.Dresser_Attendances.FirstOrDefault(da => da.ATTN_CODE == attncode && da.DRAT_CODE == null);

            // پیدا کردن پورت برای ارسال
            //var ports = OnlineDres_Butn.Tag as List<SerialPort>;
            //var port = ports.FirstOrDefault(p => p.PortName == dresrattn.Dresser.COMM_PORT);
            //port.Write(dresrattn.Attendance.DERS_NUMB.ToString());
            var ctrldev = iScsc.Dressers.FirstOrDefault(d => d.Computer_Action.COMP_NAME == xHost.Attribute("name").Value && d.REC_STAT == "002" && /*d.DRES_NUMB == dresrattn.Dresser.DRES_NUMB*/d.CODE == dresrattn.Dresser.CODE);
            var _dev = iScsc.External_Devices.FirstOrDefault(d => d.DEV_TYPE == "010" && d.STAT == "002" && ctrldev.IP_ADRS == d.IP_ADRS);

            OprtExtDev(
               new XElement("MainPage",
                     new XAttribute("devtype", _dev.DEV_TYPE),
                     new XAttribute("contype", _dev.DEV_CON),
                     new XAttribute("cmdtype", dresrattn.Dresser.DRES_NUMB),
                     new XAttribute("ip", _dev.IP_ADRS),
                     new XAttribute("sendport", _dev.PORT_SEND)
               )
            );
            // ابتدا نمایش  صفحه نمایش اتفاق بیوفتد
            //SendCommandDevExpn(dresrattn.Attendance.DERS_NUMB.ToString().PadLeft(3, '0'), devsName.FirstOrDefault(d => d.DEV_TYPE == "009").DEV_NAME, dresrattn.Attendance.FNGR_PRNT_DNRM);

            // مرحله بعدی ارسال پیام به دستگاه کنترلر مربوط به کمدهای قفل انلاین هست
            //SendCommandDevExpn(dresrattn.Attendance.DERS_NUMB.ToString().PadLeft(3, '0'), devsName.FirstOrDefault(d => d.DEV_TYPE == "010" && d.IP_ADRS == ctrldev.IP_ADRS).DEV_NAME, dresrattn.Attendance.FNGR_PRNT_DNRM);

            // 1402/10/21 * باز کردن کمدهای همراهان
            if(iScsc.Dresser_Attendances.Any(da => da.ATTN_CODE == attncode && da.DRAT_CODE != null))
            {
               Partners_Butn.Visible = PrtnrCont_Butn.Visible = PrtnrPos_Butn.Visible = PartnerDresNum_Butn.Visible = true;
               PrtnrCont_Butn.Visible = PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = true;
               new Thread(new ThreadStart(() => OpenDresPart_Tmr_Tick(attncode))).Start();
            }

            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
         }
         catch (Exception exc)
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
         }
      }

      private void OnOpenDresserWithWrist(string EnrollNumber)
      {
         try
         {
            //_wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\tick.wav";
            if (RSignalOpen_Butn.Tag == null)
            {
               RSignalOpen_Butn.Tag = "key";
               LSignalOpen_Butn.Tag = null;
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(RSignalOpen_Butn))).Start();
            }
            else
            {
               RSignalOpen_Butn.Tag = null;
               LSignalOpen_Butn.Tag = "key";
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(LSignalOpen_Butn))).Start();
            }

            Partners_Butn.Visible = PrtnrCont_Butn.Visible = PrtnrPos_Butn.Visible = PartnerDresNum_Butn.Visible = false;
            PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = PartnerDresNum_Butn.Visible = false;
            // شماره کد انگشتی را وارد باکس میکنیم
            OnlineDres_Butn.Focus();
            OnlineDres_Butn.Text = EnrollNumber;            

            // در اولین گام باید چک کنیم که این داده ورودی برای دستبند میباشد
            var _atnw = iScsc.Attendance_Wrists.FirstOrDefault(aw => aw.STAT == "001" && aw.ATNW_FNGR_PRNT_DNRM == EnrollNumber && aw.Attendance.EXIT_TIME == null && aw.Attendance.ATTN_DATE.Date == DateTime.Now.Date && aw.DRES_CODE_DNRM != null);
            if (_atnw != null)
            {
               var _dev = iScsc.External_Devices.FirstOrDefault(d => d.DEV_TYPE == "010" && d.STAT == "002" && _atnw.Dresser.IP_ADRS == d.IP_ADRS);

               OprtExtDev(
                  new XElement("MainPage",
                        new XAttribute("devtype", _dev.DEV_TYPE),
                        new XAttribute("contype", _dev.DEV_CON),
                        new XAttribute("cmdtype", _atnw.Dresser.DRES_NUMB),
                        new XAttribute("ip", _dev.IP_ADRS),
                        new XAttribute("sendport", _dev.PORT_SEND)
                  )
               );
            }
            else
            {

               // ابتدا پیدا میکنیم که امروز کدام ردیف حضور و غیاب را داریم
               var attncode = iScsc.Attendances.Where(a => a.FNGR_PRNT_DNRM == EnrollNumber && a.ATTN_DATE.Date == DateTime.Now.Date && a.EXIT_TIME == null).Max(a => a.CODE);

               // ثبت ساعت باز کردن کمد            
               iScsc.INS_DART_P(attncode, null, null);

               // اینجا باید شماره سریال پورت را پیدا کنیم و پیام را بهش ارسال کنیم
               var dresrattn = iScsc.Dresser_Attendances.FirstOrDefault(da => da.ATTN_CODE == attncode && da.DRAT_CODE == null);

               // پیدا کردن پورت برای ارسال
               //var ports = OnlineDres_Butn.Tag as List<SerialPort>;
               //var port = ports.FirstOrDefault(p => p.PortName == dresrattn.Dresser.COMM_PORT);
               //port.Write(dresrattn.Attendance.DERS_NUMB.ToString());
               var ctrldev = iScsc.Dressers.FirstOrDefault(d => d.Computer_Action.COMP_NAME == xHost.Attribute("name").Value && d.REC_STAT == "002" && d.CODE == dresrattn.Dresser.CODE);
               var _dev = iScsc.External_Devices.FirstOrDefault(d => d.DEV_TYPE == "010" && d.STAT == "002" && ctrldev.IP_ADRS == d.IP_ADRS);

               OprtExtDev(
                  new XElement("MainPage",
                        new XAttribute("devtype", _dev.DEV_TYPE),
                        new XAttribute("contype", _dev.DEV_CON),
                        new XAttribute("cmdtype", dresrattn.Dresser.DRES_NUMB),
                        new XAttribute("ip", _dev.IP_ADRS),
                        new XAttribute("sendport", _dev.PORT_SEND)
                  )
               );
               // ابتدا نمایش  صفحه نمایش اتفاق بیوفتد
               //SendCommandDevExpn(dresrattn.Attendance.DERS_NUMB.ToString().PadLeft(3, '0'), devsName.FirstOrDefault(d => d.DEV_TYPE == "009").DEV_NAME, dresrattn.Attendance.FNGR_PRNT_DNRM);

               // مرحله بعدی ارسال پیام به دستگاه کنترلر مربوط به کمدهای قفل انلاین هست
               //SendCommandDevExpn(dresrattn.Attendance.DERS_NUMB.ToString().PadLeft(3, '0'), devsName.FirstOrDefault(d => d.DEV_TYPE == "010" && d.IP_ADRS == ctrldev.IP_ADRS).DEV_NAME, dresrattn.Attendance.FNGR_PRNT_DNRM);

               // 1402/10/21 * باز کردن کمدهای همراهان
               if (iScsc.Dresser_Attendances.Any(da => da.ATTN_CODE == attncode && da.DRAT_CODE != null))
               {
                  Partners_Butn.Visible = PrtnrCont_Butn.Visible = PrtnrPos_Butn.Visible = PartnerDresNum_Butn.Visible = true;
                  PrtnrCont_Butn.Visible = PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = true;
                  new Thread(new ThreadStart(() => OpenDresPart_Tmr_Tick(attncode))).Start();
               }

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
         }
         catch (Exception exc)
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
         }
      }

      private void axCZKEM3_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
      {
         try
         {
            if (InvokeRequired)
               Invoke(new Action(() => OnOpenDresser(EnrollNumber)));
            else
               OnOpenDresser(EnrollNumber);
            return;
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void SendOprtDresser(string devIP, string cmndName, string cmndsend)
      {
         try
         {
            // پیدا کردن دستگاه برای ارسال
            var _dev = iScsc.External_Devices.FirstOrDefault(ed => (ed.DEV_TYPE == "010" || ed.DEV_TYPE == "009") && ed.IP_ADRS == devIP && ed.STAT == "002");

            switch (_dev.DEV_COMP_TYPE)
            {
               case "002":
               case "003":
                  // Noro Company
                  OprtExtDev(
                     new XElement("MainPage",
                         new XAttribute("devtype", _dev.DEV_TYPE),
                         new XAttribute("contype", _dev.DEV_CON),
                         new XAttribute("cmdtype", cmndName),
                         new XAttribute("cmdsend", cmndsend),
                         new XAttribute("ip", _dev.IP_ADRS),
                         new XAttribute("sendport",_dev.PORT_SEND)
                     )
                  );
                  break;
               default:
                  break;
            }            
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
         }
         catch (Exception exc)
         {
            BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
         }
      }
      #endregion

      #region Base Info
      private void bbi_basdifnbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 144 /* Execute Bas_Dfin_F */),
                new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Actn_CalF_P */)
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_epitbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "UserRegionClub"), new XAttribute("section", "userview"))}
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cashbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         //Job _InteractWithScsc =
         //  new Job(SendType.External, "Localhost",
         //     new List<Job>
         //     {                  
         //       new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
         //       new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "UserRegionClub"), new XAttribute("section", "userview"))}
         //     });
         //_DefaultGateway.Gateway(_InteractWithScsc);
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 138 /* Execute Cnf_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CNF_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_mtodbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
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
                              "<Privilege>17</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 08 /* Execute Mstr_Mtod_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_regnbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 98 /* Execute Bas_Cpr_F */),
                  new Job(SendType.SelfToUserInterface, "BAS_CPR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_clubbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 98 /* Execute Bas_Cpr_F */),
                  new Job(SendType.SelfToUserInterface, "BAS_CPR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_ucplbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 98 /* Execute Bas_Cpr_F */),
                  new Job(SendType.SelfToUserInterface, "BAS_CPR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_003"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_reglbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
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
                              "<Privilege>2</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 03 /* Execute Mstr_Regl_F */)
                  #endregion

                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_crglbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
         if (Rg1 == null) return;

         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}}
                  })
            );
      }

      private void bbi_idrgbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
         if (Rg1 == null) return;

         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 05 /* Execute Regl_Expn_F */){Input = new List<Data.Regulation>{Rg1, null}}
                  })
            );
      }

      private void bbi_orgnbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
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
                              "<Privilege>171</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show("خطا: عدم دسترسی به کد 171");
                              #endregion                           
                           })
                        },
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>175</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show("خطا: عدم دسترسی به کد 175");
                              #endregion                           
                           })
                        }
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 108 /* Execute Orgn_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 10 /* Actn_CalF_P */)
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cclcbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 68 /* Execute Cal_Expn_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_movpbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 120 /* Execute Bsc_Bmov_F */),
                  new Job(SendType.SelfToUserInterface, "BSC_BMOV_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_coptbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 138 /* Execute Cnf_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CNF_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region Admission Info
      private void bbi_adm1butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_mbs1butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change ****
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cyclbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_mbsnbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }


      private void bbi_mbfzbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change ****
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 133 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBFZ_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "block"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_senmbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change ****
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 134 /* Execute Adm_Mbsm_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBSM_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "session"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }


      private void bbi_insrbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 80 /* Execute Ins_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_flstbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
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
                              "<Privilege>260</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 260 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */){Input = x},
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fmdfbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must be change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fdlsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 140 /* Execute Lsi_Fldf_F */){Input = x},
                  new Job(SendType.SelfToUserInterface, "LSI_FDLF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fdbtbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 117 /* Execute Debt_List_F */),
                  new Job(SendType.SelfToUserInterface, "DEBT_LIST_F" , 10 /* Execute Actn_CalF_F */){Input = new XElement("Debt", new XAttribute("type", "query"), new XAttribute("fileno", ""))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fdelbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 82 /* Execute Adm_Ends_F */),
                  //new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fundbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 83 /* Execute Adm_Dsen_F */),
                  //new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fmtdbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 79 /* Execute Mcc_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fchmbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 75 /* Execute Cmc_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fcrlbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {                  
                  new Job(SendType.Self, 113 /* Execute Glr_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "GLR_TOTL_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_adm2butn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change ***
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "coach"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_clstbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _CheckSecurity =
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
                              "<Privilege>217</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 217 سطوح امینتی", "عدم دسترسی");
                              #endregion                           
                           })
                        }
                        #endregion                        
                     }),
                     new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */){Input = x},
                     new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "003"))}
                  });
         _DefaultGateway.Gateway(_CheckSecurity);

         //Job _InteractWithScsc =
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {                  
                  
         //      });
         //_DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cdelbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 82 /* Execute Adm_Ends_F */),
                  //new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cundbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 83 /* Execute Adm_Dsen_F */),
                  //new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cchmbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 75 /* Execute Cmc_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_mbcobutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 127 /* Execute Adm_Mbco_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBCO_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }      
      #endregion

      #region Misc InCome
      private void bbi_hoicbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 95 /* Execute Oic_Srzh_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SRZH_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_hlstbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 95 /* Execute Oic_Srzh_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SRZH_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region Statistics
      private void bbi_attnbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fembbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endfigh"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_frkdbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
               {
                  new Job(SendType.Self, 100 /* Execute Fngr_Rsvd_F */),
                  new Job(SendType.SelfToUserInterface, "FNGR_RSVD_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_feinbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endinsr"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_femtbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endmtod"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_rptcbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "reportcheck"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_rptsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "sesnmeet"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #endregion

      #region Settings
      private void bbi_bkupbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "BackupRestore"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_rstrbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "BackupRestore"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_trnsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }

      private void bbi_embkbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cfg_stng_f"},
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 07 /* Execute Load_Data */),                  
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "EmerjncyBackup"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #endregion

      #region Other Operation
      private void bbi_oincbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                     //new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Payment", new XAttribute("type", "Out"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cmanbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_exdsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 131 /* Execute Adm_Brsr_F */),
                  new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_oexpbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cincbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_aprsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 129 /* Execute Adm_Hrsr_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_dprsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }

      private void bbi_cprsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 132 /* Execute Adm_Hrsc_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_HRSC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cpaybutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_hlstbutn_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */){Input = x},
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "004"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #endregion

      #region Coach
      private void bbi_testbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {         
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 76 /* Execute Tst_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_compbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 77 /* Execute Cmp_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_exambutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 71 /* Execute Exm_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_pyftbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 72 /* Execute Psf_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_calcbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 73 /* Execute Clc_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_hertbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 74 /* Execute Hrt_totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fnmvbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 119 /* Execute Bdf_Pros_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fnfdbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }

      private void bbi_fnanbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }
      #endregion

      #region Aggregate Operation
      private void bbi_smsbbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 116 /* Execute Msgb_Totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_agmbbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 121 /* Execute Aop_Mbsp_F */),
                  new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Member_Ship",                              
                              new XAttribute("attndate", DateTime.Now)
                           )
                     }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_aglsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 126 /* Execute Aop_Attn_F */),
                  new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Attendance",                              
                              new XAttribute("attndate", DateTime.Now)
                           )
                     }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_agcbbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 124 /* Execute Aop_Cbmt_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_agmcbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 125 /* Execute Aop_Mtod_F */),
                  new Job(SendType.SelfToUserInterface, "AOP_MTOD_F", 10 /* Execute Actn_Calf_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_incgbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 156 /* Execute Aop_Incm_F */),
                  new Job(SendType.SelfToUserInterface, "AOP_INCM_F", 10 /* Execute Actn_Calf_F */)
                  {
                     Input = 
                        new XElement("Other_Income",                              
                           new XAttribute("attndate", DateTime.Now)
                        )
                  }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_dsctcard_ItemClick(object sender, ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 165 /* Execute Adv_Base_F */),
                  new Job(SendType.SelfToUserInterface, "ADV_BASE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("formcaller", GetType().Name))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region Report
      private void bbi_acntbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
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
                              "<Privilege>193</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 193 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                     #region DoWork
                  new Job(SendType.Self, 91 /* Execute Rpt_List_F */),
                  new Job(SendType.SelfToUserInterface, "ACN_LIST_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_inotbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }

      private void bbi_incmbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
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
                              "<Privilege>218</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 218 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 135 /* Execute Rpt_Pmmt_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("fromdate", DateTime.Now), new XAttribute("todate", DateTime.Now), new XAttribute("useraccount", "manager"), new XAttribute("formname", "RPT_PYM1_F"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_expnbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }

      private void bbi_shmdbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }

      private void bbi_salebutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
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
                              "<Privilege>219</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 219 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 136 /* Execute Dap_Pivt_F */),
                  new Job(SendType.SelfToUserInterface, "DAP_PIVT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("formname", "RPT_PMT1_F"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_lusrbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }
      #endregion

      #region Dashboard
      private void bbi_bislbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  #region DoWork
                  new Job(SendType.Self, 135 /* Execute Rpt_Pmmt_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("formname", "RPT_PYM2_F"), new XAttribute("fromdate", DateTime.Now), new XAttribute("todate", DateTime.Now), new XAttribute("useraccount", "user"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_dsrdbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
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
                              "<Privilege>220</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 220 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 137 /* Execute Dap_Chrt_F */),
                  new Job(SendType.SelfToUserInterface, "DAP_CHRT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_dshbbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
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
                              "<Privilege>220</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 220 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  //new Job(SendType.Self, 139 /* Execute Dap_Dshb_F */),
                  //new Job(SendType.SelfToUserInterface, "DAP_DSHB_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                  new Job(SendType.Self, 158 /* Execute Dap_Dsbr_F */),
                  new Job(SendType.SelfToUserInterface, "DAP_DSBR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region Taskbar & Menus
      private void StartMenu_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 128 /* Execute Adm_Figh_F */),                  
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void ActionCenter_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard:SecurityPolicy", 07 /* Execute DoWork4ActionCenter */ , SendType.Self) 
            { 
               Input = 
               new XElement("ActionCenter",
                     new XAttribute("currentuser", ""),
                     new XAttribute("subsys", "5")
                   )
            }
         );
      }

      private void More_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 143 /* Execute USR_CTBL_F */)                  
               })
         );
      }

      private void ShowDesktop_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      //private void FIGH_FILE_NOLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      //{
      //   try
      //   {
      //      if(e.Button.Index == 6)
      //      {
      //         iScsc = new Data.iScscDataContext(ConnectionString);
      //         FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
      //         return;
      //      }

      //      var fileno = Convert.ToInt64(FIGH_FILE_NOLookUpEdit.EditValue);

      //      var figh = FighBs.List.OfType<Data.Fighter>().First(f => f.FILE_NO == fileno);
      //      switch (e.Button.Index)
      //      {
      //         case 5:
      //            _DefaultGateway.Gateway(
      //               new Job(SendType.External, "Localhost",
      //                    new List<Job>
      //                  {                  
      //                     new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
      //                     new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
      //                  })
      //            );
      //            break;
      //         case 1:
      //            if (!(figh.FGPB_TYPE_DNRM == "001" || figh.FGPB_TYPE_DNRM == "005" || figh.FGPB_TYPE_DNRM == "006")) return;

      //            _DefaultGateway.Gateway(
      //               new Job(SendType.External, "Localhost",
      //                  new List<Job>
      //               {
      //                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
      //                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
      //               })
      //            );
      //            break;
      //         case 3:
      //            if (MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "عملیات حذف موقت مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
      //            _DefaultGateway.Gateway(
      //               new Job(SendType.External, "Localhost",
      //                  new List<Job>
      //               {
      //                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
      //                  new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
      //                  new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
      //                  new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))},
      //                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 07 /* Execute Load_Data */),

      //               })
      //            );
      //            break;
      //         case 2:
      //            _DefaultGateway.Gateway(
      //               new Job(SendType.External, "Localhost",
      //                  new List<Job>
      //               {
      //                  new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
      //                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))}
      //               })
      //            );
      //            break;
      //         case 4:
      //            _DefaultGateway.Gateway(
      //               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
      //            );
      //            break;
      //         default:
      //            break;
      //      }
      //   }
      //   catch { }
      //}
      #endregion

      private void CardNumb_Text_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            #region ShortCut InCome 
            // Men
            if (e.Button.Index == 5)
            {
               var figh = iScsc.Fighters.FirstOrDefault(f => f.FGPB_TYPE_DNRM == "005" && f.SEX_TYPE_DNRM == "001" && f.FIGH_STAT == "002");

               if (figh == null) 
               {
                  More_Butn_Click(null, null);
                  return; 
               }

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                        new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                        })
               );
               return;
            }
            // Women
            else if (e.Button.Index == 6)
            {
               var figh = iScsc.Fighters.FirstOrDefault(f => f.FGPB_TYPE_DNRM == "005" && f.SEX_TYPE_DNRM == "002" && f.FIGH_STAT == "002");

               if (figh == null)
               {
                  More_Butn_Click(null, null);
                  return;
               }

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                        new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                        })
               );
               return;
            }
            #endregion

            if (CardNumb_Text.Text == "")
            {
               if (FngrPrnt_Txt.Text == "") return;
               else if (FngrPrnt_Txt.Text != "")
                  CardNumb_Text.Text = FngrPrnt_Txt.Text;
            }

            if (e.Button.Index == 0 || e.Button.Index == 1)
            {
               //var control = spc_desktop.Panel1.Controls.OfType<Control>().FirstOrDefault();
               //if (control == null) return;
               //if (control.Name == "ADM_FIGH_F" || control.Name == "ADM_CHNG_F")
               {
                  Job _InteractWithScsc =
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", CardNumb_Text.Text))},
                     });
                  _DefaultGateway.Gateway(_InteractWithScsc);

                  CardNumb_Text.Text = "";
               }
            }
            else if(e.Button.Index == 2)
            {
               var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == CardNumb_Text.Text || (CardNumb_Text.Text.Length == 10 && f.NATL_CODE_DNRM == CardNumb_Text.Text));

               if (figh == null) return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
               );
            }
            else if(e.Button.Index == 3)
            {
               axCZKEM1_OnAttTransactionEx(CardNumb_Text.Text, 1, 1, 1, 2016, 05, 10, 09, 31, 50, 20);
            }    
            else if(e.Button.Index == 4)
            {
               var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == CardNumb_Text.Text || (CardNumb_Text.Text.Length == 10 && f.NATL_CODE_DNRM == CardNumb_Text.Text));

               if (figh == null) return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                  
                        new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                     })
               );
            }            
         }
         catch (Exception )
         { CardNumb_Text.Text = ""; }
      }

      private void AttendanceSystemAlert_Butn_Click(object sender, EventArgs e)
      {
         if (ModifierKeys.HasFlag(Keys.Control))
         {
            Tm_FingerPrintWorker.Enabled = true;
         }
         else if (ModifierKeys.HasFlag(Keys.Shift))
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var _attn = iScsc.Attendances.Where(a => a.ATTN_DATE.Date == DateTime.Now.Date && a.ATTN_STAT == "002").OrderByDescending(a => a.ENTR_TIME).FirstOrDefault();
            if (_attn == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){ Input = Keys.Escape },
                     new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                     new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", _attn.FIGH_FILE_NO),
                           new XAttribute("attndate",_attn.ATTN_DATE.Date),
                           new XAttribute("attncode", _attn.CODE),
                           new XAttribute("formcaller", GetType().Name)
                        )
                     }
                  })
            );
         }
         else
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 141 /* Execute WHO_ARYU_F */),
                     new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )
                  })
            );

            _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\tick.wav";
            new Thread(AlarmShow).Start();
         }
      }

      PersianCalendar _pc = new PersianCalendar();
      private void Tm_ShowTime_Tick(object sender, EventArgs e)
      {
         //AdjustDateTime_Butn.Text = DateTime.Now.ToString("HH:mm:ss");
         Tm_ShowTime.Interval = 60000;
         AdjustDateTime_Butn.Text = DateTime.Now.ToString("HH:mm");
         AdjustDateTime_Lab.Text = string.Format("{0}/{1}/{2}", _pc.GetYear(DateTime.Now), _pc.GetMonth(DateTime.Now), _pc.GetDayOfMonth(DateTime.Now)) + Environment.NewLine;
         string weekdaydesc = "";
         switch (_pc.GetDayOfWeek(DateTime.Now))
         {
            case DayOfWeek.Friday:
               weekdaydesc = "جمعه";
               break;
            case DayOfWeek.Saturday:
               weekdaydesc = "شنبه";
               break;
            case DayOfWeek.Sunday:
               weekdaydesc = "یکشنبه";
               break;
            case DayOfWeek.Monday:
               weekdaydesc = "دوشنبه";
               break;
            case DayOfWeek.Tuesday:
               weekdaydesc = "سه شنبه";
               break;
            case DayOfWeek.Wednesday:
               weekdaydesc = "چهارشنبه";
               break;
            case DayOfWeek.Thursday:
               weekdaydesc = "پنجشنبه";
               break;
         }
         string dayofmonthdesc = _pc.GetDayOfMonth(DateTime.Now).ToString();
         string monthdesc = "";
         switch (_pc.GetMonth(DateTime.Now))
         {
            case 1:
               monthdesc = "فروردین";
               break;
            case 2:
               monthdesc = "اردیبهشت";
               break;
            case 3:
               monthdesc = "خرداد";
               break;
            case 4:
               monthdesc = "تیر";
               break;
            case 5:
               monthdesc = "مرداد";
               break;
            case 6:
               monthdesc = "شهریور";
               break;
            case 7:
               monthdesc = "مهر";
               break;
            case 8:
               monthdesc = "آبان";
               break;
            case 9:
               monthdesc = "آذر";
               break;
            case 10:
               monthdesc = "دی";
               break;
            case 11:
               monthdesc = "بهمن";
               break;
            case 12:
               monthdesc = "اسفند";
               break;
         }
         string year = _pc.GetYear(DateTime.Now).ToString();
         AdjustDateTime_Lab.Text += string.Format("{0}, {1} {2}\n\r{3}", weekdaydesc, dayofmonthdesc, monthdesc, year);

         if (_settings == null)
            _settings = iScsc.V_Settings;

         // برای هر یک ساعت یک بار سیستم چک کن که آیا نرم افزار لاینسس دارد یا خیر
         if (_doActionStep < 900)
         {
            _doActionStep++;
            //return;
         }
         else
         {
            _doActionStep = 0;

            // 1399/12/06 * بررسی اینکه مشتریان خلافکار را از استفاده کردن از سیستم ناامید کنیم
            //if (_settings.Any(s => s.EXPR_TYPE.Value))
            //{
            //   if (_settings.Any(s => s.LAST_DATE.Value.Date != DateTime.Now.Date))
            //   {
            //      if (_settings.Any(s => s.EXPR_VALU.Value.Date < DateTime.Now.Date))
            //      {
            //         if (_settings.Any(s => s.LAST_DATE.Value.Date < DateTime.Now.Date))
            //         {
            //            // در غیر اینصورت سیستم باید کلا بسته شود بدون هیچ گونه اعتراضی
            //            MessageBox.Show(this, "مدت زمان پشتیبانی نرم افزار به اتمام رسیده، لطفا جهت تمدید پشتیبانی با شماره 09033927103 تماس حاصل فرمایید" + Environment.NewLine +
            //                                  "ضمنا تمامی رکورد های ثبت شده خارج از تاریخ پشتیبانی فاقد اعتبار میباشند و بعد از بسته شدن نرم افزار تمامی رکوردها به صورت اتومات پاک میشوند",
            //                                  "هشدار جهت استفاده از لایسنس نامعتبر", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            Application.Exit();
            //            Process.GetCurrentProcess().Kill();
            //         }
            //      }
            //   }
            //}
         }

         // 1400/12/14 * چک کردن اینکه مشتری تاریخ سیستم را تغییر ندهد و ما رو کیر کنه
         //if ((ExprInstDate.Date - DateTime.Now.Date).Days < 0 || (CrntDate.Date - DateTime.Now.Date).Days <= -2)
         //{
         //   MessageBox.Show("تاریخ اعتبار شما به پایان رسیده", "خطای انقضای ماهیانه محصول", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

         //   Application.Exit();
         //   Process.GetCurrentProcess().Kill();
         //}

         try
         {
            // 1397/05/10 * بررسی اینکه آیا ارتباط با سرور بر قرار میباشد یا خیر
            if (SrvrPing_Butn.Tag == null)
            {
               var GetServer =
                  new Job(SendType.External, "Localhost", "Commons", 35 /* Execute DoWork4GetServer */, SendType.Self) { 
                     Input = new XElement("Request",
                                 new XAttribute("Rqtp_Code", "SRVRADRS"),
                                 new XElement("Database", "iScsc")
                             ) 
                  };
               _DefaultGateway.Gateway(
                  GetServer
               );
               SrvrPing_Butn.Tag = GetServer.Output.ToString();

               _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\connect.wav";
               new Thread(AlarmShow).Start();
            }

            Ping ping = new Ping();
            var pingstatus = ping.Send(SrvrPing_Butn.Tag.ToString(), 500);

            if (pingstatus.Status == IPStatus.Success)
            {
               SrvrPing_Butn.Image = Properties.Resources.IMAGE_1408;
               SrvrPing_Butn.Appearance.BackColor = Color.LightGreen;
               SrvrPing_Butn.ToolTip = string.Format("Server IP : {0} Network connected.", SrvrPing_Butn.Tag);               
            }
            else
            {
               SrvrPing_Butn.Image = Properties.Resources.IMAGE_1418;
               SrvrPing_Butn.Appearance.BackColor = Color.Pink;
               SrvrPing_Butn.ToolTip = string.Format("Server IP : {0} Network disconnected.", SrvrPing_Butn.Tag);

               _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\disconnect.wav";
               new Thread(AlarmShow).Start();
            }

            // اگر چک کردن تست ارتباط فعال باشد
            if (TryPing_Cbx.Checked)
            {
               //ActionCenter_Butn.ToolTip += "Checked external device..." + Environment.NewLine;
               // بررسی دستگاه های درون شبکه
               foreach (Device_On_Network dev in DeviceOnNetworks)
               {
                  //ActionCenter_Butn.ToolTip += "IP Address :" + dev.IPAddress + " call is a lived method." + Environment.NewLine;
                  // 1403/01/22 * if client is connected and keep a live
                  if (!dev.IsALive())
                  {
                     ActionCenter_Butn.ToolTip += "IP Address :" + dev.IPAddress + " not a lived." + Environment.NewLine;
                     _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\alert2.wav";
                     new Thread(AlarmShow).Start(); 
                     dev.PingStatus = false;
                  }
                  // اگر دستگاه غیرفعال بوده
                  if (!dev.PingStatus)
                  {
                     // دستگاه را دوباره تست میکنیم
                     dev.Ping();
                     // اگر دستگاه انلاین شد
                     if (dev.PingStatus /*&& !dev.IsALive()*/)
                     {
                        // Kill client and reconnenct
                        dev.Stop();
                        dev.CallBack();
                        //_DefaultGateway.Gateway(
                        //   new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                        //   {
                        //      Input =
                        //         new List<object>
                        //         {
                        //            ToolTipIcon.Info,
                        //            string.Format("Device Name : {0}\n\rIP Address : {1}", dev.DeviceName, dev.IPAddress),
                        //            "دستگاه به شبکه متصل شد",
                        //            2000
                        //         }
                        //   }
                        //);
                        ActionCenter_Butn.ToolTip += "IP Address :" + dev.IPAddress + " connected." + Environment.NewLine;

                        _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\connect.wav";
                        new Thread(AlarmShow).Start();
                     }
                     else
                     {
                        //_DefaultGateway.Gateway(
                        //   new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                        //   {
                        //      Input =
                        //         new List<object>
                        //         {
                        //            ToolTipIcon.Warning,
                        //            string.Format("Device Name : {0}\n\rIP Address : {1}", dev.DeviceName, dev.IPAddress),
                        //            "دستگاه به شبکه متصل نیست",
                        //            2000
                        //         }
                        //   }
                        //);

                        ActionCenter_Butn.ToolTip += "IP Address :" + dev.IPAddress + " disconnected." + Environment.NewLine;

                        _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\disconnect.wav";
                        new Thread(AlarmShow).Start();
                     }
                  }
                  else
                     dev.Ping();

                  SrvrPing_Butn.ToolTip += string.Format("\nDevice IP : {0} Status : {1}", dev.IPAddress, dev.PingStatus ? "Connected" : "Disconnected");
               }

               // 1401/03/05 * Check Serial port
               if(Sp_Barcode.PortName != "NoPort" && Sp_Barcode.PortName.Length > 0)
               {
                  if(!Sp_Barcode.IsOpen)
                  {
                     new Thread(AlarmShow).Start();
                     Sp_Barcode.Close();
                     Sp_Barcode.Open();
                  }
               }
               if (Sp_GateAttn.PortName != "NoPort" && Sp_GateAttn.PortName.Length > 0)
               {
                  if (!Sp_GateAttn.IsOpen)
                  {
                     new Thread(AlarmShow).Start();
                     Sp_GateAttn.Close();
                     Sp_GateAttn.Open();
                  }
               }
            }
         }
         catch (Exception exc) { ActionCenter_Butn.ToolTip = exc.Message; }
      }

      private void AdjustDateTime_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 26 /* Execute DoWork4DateTimes */, SendType.Self));
      }

      private void GateAttn_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 148 /* Execute GATE_ACTN_F */)                  
               })
         );
      }

      private void ShowInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FngrPrnt_Txt.Text == "") return;

            var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == FngrPrnt_Txt.Text || (FngrPrnt_Txt.Text.Length == 10 && f.NATL_CODE_DNRM == FngrPrnt_Txt.Text));
            if (figh == null)
            {
               ShowInfo_Butn.SuperTip =
                  SuperToolTipAttnButn(
                     new XElement("System",
                        new XAttribute("device", "AttnDvic"),
                        new XAttribute("desc", "با این شماره عضویی شناسایی نشد")
                     )
                  );

               _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\alert2.wav";
               new Thread(AlarmShow).Start();
               return;
            }
            else
            {
               ShowInfo_Butn.SuperTip =
                  SuperToolTipAttnButn(
                     new XElement("System",
                        new XAttribute("device", "AttnDvic"),
                        new XAttribute("desc", figh.NAME_DNRM)
                     )
                  );
            }
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
            );
         }
         catch { }
      }

      private void CardNumb_Text_KeyDown(object sender, KeyEventArgs e)
      {
         if(e.KeyData == (Keys.Enter))
         {
            CardNumb_Text_Properties_ButtonClick(sender, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(CardNumb_Text.Properties.Buttons[2]));
         }
      }

      private void TlgrmBot_Butn_Click(object sender, EventArgs e)
      {
         if (ModifierKeys.HasFlag(Keys.Control))
         {
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
                        #region DoWork
                        new Job(SendType.External, "Program", "RoboTech", 02 /* Execute Frst_Page_F */,SendType.Self)
                        #endregion
                     })                     
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void ChngAttnActn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (AttnType_Lov.EditValue == null || AttnType_Lov.EditValue.ToString() == "") { AttnType_Lov.EditValue = "001"; }
            switch (AttnType_Lov.EditValue.ToString())
            {
               case "001":
                  ChngAttnActn_Butn.Image = Properties.Resources.IMAGE_1608;
                  AttnType_Lov.EditValue = "003";

                  _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\ok.wav";
                  new Thread(AlarmShow).Start();
                  break;
               case "003":
                  ChngAttnActn_Butn.Image = Properties.Resources.IMAGE_1609;
                  AttnType_Lov.EditValue = "001";

                  _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\request.wav";
                  new Thread(AlarmShow).Start();
                  break;
               default:
                  break;
            }

            AttnType_Lov.Focus();
         }
         catch { }
      }

      private void button1_Click(object sender, EventArgs e)
      {
         Enroll_Finger(FngrPrnt_Txt.Text);
      }

      private void button2_Click(object sender, EventArgs e)
      {
         Delete_Finger(FngrPrnt_Txt.Text);
      }

      private void button3_Click(object sender, EventArgs e)
      {
         Sp_Barcode.Write("empty");
      }

      private void bbi_basdifnbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_basdifnbutn_ItemClick(null, null);
      }

      private void bbi_adm1butn_Mn_Click(object sender, EventArgs e)
      {
         bbi_adm1butn_ItemClick(null, null);
      }

      private void bbi_mbs1butn_Mn_Click(object sender, EventArgs e)
      {
         bbi_mbs1butn_ItemClick(null, null);
      }

      private void bbi_mbfzbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_mbfzbutn_ItemClick(null, null);
      }

      private void bbi_fmdfbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_fmdfbutn_ItemClick(null, null);
      }

      private void bbi_flstbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_flstbutn_ItemClick(null, null);
      }

      private void bbi_oincbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_oincbutn_ItemClick(null, null);
      }

      private void bbi_exdsbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_exdsbutn_ItemClick(null, null);
      }

      private void bbi_oexpbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_oexpbutn_ItemClick(null, null);
      }

      private void bbi_cpaybutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_cpaybutn_ItemClick(null, null);
      }

      private void bbi_smsbbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_smsbbutn_ItemClick(null, null);
      }

      private void bbi_incmbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_incmbutn_ItemClick(null, null);
      }

      private void bbi_bislbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_bislbutn_ItemClick(null, null);
      }

      private void bbi_bkupbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_bkupbutn_ItemClick(null, null);
      }

      private void bbi_embkbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_embkbutn_ItemClick(null, null);
      }

      private void bbi_agmbbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_agmbbutn_ItemClick(null, null);
      }

      private void bbi_agcbbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_agcbbutn_ItemClick(null, null);
      }

      private void bbi_aglsbutn_Mn_Click(object sender, EventArgs e)
      {
         bbi_aglsbutn_ItemClick(null, null);
      }

      private void ksk_incmbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 157 /* Execute Ksk_Incm_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void button4_Click(object sender, EventArgs e)
      {
         if (InvokeRequired)
            Invoke(new Action(() => OnOpenDresser(OnlineDres_Butn.Text)));
         else
            OnOpenDresser(OnlineDres_Butn.Text);
      }

      private static void OpenHttpClient()
      {
         WebRequest request = WebRequest.Create(
           "http://192.168.1.11/1");
         // If required by the server, set the credentials.  
         request.Credentials = CredentialCache.DefaultCredentials;
         // Get the response.  
         WebResponse response = request.GetResponse();
         // Display the status.  
         Console.WriteLine(((HttpWebResponse)response).StatusDescription);
         // Get the stream containing content returned by the server.  
         Stream dataStream = response.GetResponseStream();
         // Open the stream using a StreamReader for easy access.  
         StreamReader reader = new StreamReader(dataStream);
         // Read the content.  
         string responseFromServer = reader.ReadToEnd();
         // Display the content.  
         Console.WriteLine(responseFromServer);
         // Clean up the streams and the response.  
         reader.Close();
         response.Close();
      }

      private void tol_ibutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         try
         {
            //var butn = sender as BarButtonItem;
            switch (e.Item.Tag.ToString())
            {
               case "calc":
                  Process.Start("calc.exe");
                  break;
               case "devicemanager":
                  Process.Start("devmgmt.msc");
                  break;
               case "localservices":
                  Process.Start("services.msc");
                  break;
               case "sharefolder":
                  Process.Start("explorer.exe", @"\\localhost");
                  break;
               case "appfolder":
                  Process.Start("explorer.exe", Environment.CurrentDirectory);
                  break;
               case "ssms":
                  Process.Start("ssms.exe");
                  break;
               case "mstsc":
                  Process.Start("mstsc.exe");
                  break;
               case "magnifire":
                  Process.Start("magnify.exe");
                  break;
               case "controlpanel":
                  Process.Start("control.exe");
                  break;
               case "cmd":
                  Process.Start("cmd.exe");
                  break;
               case "anydesk":
                  Process.Start(@"C:\Program Files (x86)\AnyDesk\AnyDesk.exe");
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

      private void OnlineDres_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch(e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 160 /* Execute Mngr_Dres_F */),
                           new Job(SendType.SelfToUserInterface, "MNGR_DRES_F", 10 /* Execute MNGR_DRES_F*/ )
                        })
                  );
                  break;
               case 1:
                  if (OnlineDres_Butn.Text == "") return;
                  var _attn = iScsc.Attendances.Where(a => a.ATTN_DATE == DateTime.Now.Date && a.EXIT_TIME == null && a.ATTN_STAT == "002" && a.DERS_NUMB == OnlineDres_Butn.Text.ToInt32());
                  if(_attn.Count() == 1)
                  {
                     AttnType_Lov.EditValue = "003";
                     ChngAttnActn_Butn_Click(null, null);
                     CardNumb_Text.Text = _attn.FirstOrDefault().FNGR_PRNT_DNRM;
                     CardNumb_Text_Properties_ButtonClick(CardNumb_Text, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(CardNumb_Text.Properties.Buttons[3]));
                  }
                  else if(_attn.Count() > 1)
                  {
                     AttendanceSystemAlert_Butn_Click(null, null);
                  }
                  break;
               case 2:
                  if (OnlineDres_Butn.Text != "")
                  {
                     axCZKEM3_OnAttTransactionEx(OnlineDres_Butn.Text, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                  }
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

      private void tol_closegatebutn_ItemClick(object sender, ItemClickEventArgs e)
      {
         iScsc.INS_LGOP_P(
            new XElement("Log",
               new XAttribute("fileno", ""),
               new XAttribute("type", "007"),
               new XAttribute("text", "گیت به صورت دستی توسط کاربر روبه بیرون باز شد")
            )
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                  new Job(SendType.SelfToUserInterface, GetType().Name, 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "gatecontrol"),
                           new XAttribute("gateactn", "close")
                        )
                  }
               }
            )
         );
      }

      private void tol_opengatebutn_ItemClick(object sender, ItemClickEventArgs e)
      {
         iScsc.INS_LGOP_P(
            new XElement("Log",
               new XAttribute("fileno", ""),
               new XAttribute("type", "007"),
               new XAttribute("text", "گیت به صورت دستی توسط کاربر روبه داخل باز شد")
            )
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                  new Job(SendType.SelfToUserInterface, GetType().Name, 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "gatecontrol"),
                           new XAttribute("gateactn", "open")
                        )
                  }
               }
            )
         );
      }

      private void OpenGate_Tsm_Click(object sender, EventArgs e)
      {
         tol_opengatebutn_ItemClick(null, null);
      }

      private void CloseGate_Tsm_Click(object sender, EventArgs e)
      {
         tol_closegatebutn_ItemClick(null, null);
      }

      private void bbi_grntbutn_ItemClick(object sender, ItemClickEventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 162 /* Execute Wrn_Serv_F */),
                     new Job(SendType.SelfToUserInterface, "WRN_SERV_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Fighter")}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void button5_Click(object sender, EventArgs e)
      {
         server.Dispose();
      }

      private void CWlet_Tm_Tick(object sender, EventArgs e)
      {
         try
         {
            // بررسی اینکه آیا مشتری درصدی میباشد یا خیر
            if (!iScsc.V_Transaction_Fees.Any(t => t.STAT == "002" && t.TXFE_PRCT > 0)) { CWlet_Pb.Visible = CWlet_Lnk.Visible = CWlet_Txt.Visible = CWlet_Tm.Enabled = false; return; }

            var _cwlt = iScsc.V_Admin_Wallets.Where(w => w.WLET_TYPE == "001").FirstOrDefault();
            if (_cwlt == null) { CWlet_Pb.Visible = CWlet_Lnk.Visible = CWlet_Txt.Visible = CWlet_Tm.Enabled = false; return; }

            CWlet_Pb.Visible = CWlet_Lnk.Visible = CWlet_Txt.Visible = CWlet_Tm.Enabled = true;
            CWlet_Txt.EditValue = _cwlt.AMNT_DNRM;
            CWlet_Lnk.Tag = _cwlt;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CWlet_Pb_Click(object sender, EventArgs e)
      {
         CWlet_Tm_Tick(null, null);
      }

      private void bbi_basdifnbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 144 /* Execute Bas_Dfin_F */),
                new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Actn_CalF_P */)
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_adm1butn_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_mbs1butn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change ****
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_insrbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 80 /* Execute Ins_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 10 /* Actn_CalF_P */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_flstbutn_ItemClick(object sender, EventArgs e)
      {
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
                              "<Privilege>260</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 260 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */){Input = x},
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_fdlsbutn_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 140 /* Execute Lsi_Fldf_F */){Input = x},
                  new Job(SendType.SelfToUserInterface, "LSI_FDLF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cmanbutn_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_cpaybutn_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_oexpbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 69 /* Execute Cal_Cexc_F */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_002"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_exdsbutn_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 131 /* Execute Adm_Brsr_F */),
                  new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_smsbbutn_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 116 /* Execute Msgb_Totl_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_dsctcard_ItemClick(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 165 /* Execute Adv_Base_F */),
                  new Job(SendType.SelfToUserInterface, "ADV_BASE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("formcaller", GetType().Name))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_incmbutn_ItemClick(object sender, EventArgs e)
      {
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
                              "<Privilege>218</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 218 سطوح امینتی", "عدم دسترسی");
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 135 /* Execute Rpt_Pmmt_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("fromdate", DateTime.Now), new XAttribute("todate", DateTime.Now), new XAttribute("useraccount", "manager"), new XAttribute("formname", "RPT_PYM1_F"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_bkupbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "BackupRestore"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_rstrbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "BackupRestore"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_embkbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cfg_stng_f"},
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 07 /* Execute Load_Data */),                  
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "EmerjncyBackup"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_oincbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                     //new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Payment", new XAttribute("type", "Out"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void ShowMain_Btn_Click(object sender, EventArgs e)
      {
         if (spc_desktopnew.SplitterPosition != 265)
            spc_desktopnew.SplitterPosition = 265;
         else
            spc_desktopnew.SplitterPosition = 0;

         _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\tick.wav";
         new Thread(AlarmShow).Start();
      }

      private void tol_ibutn_ItemClick(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as System.MaxUi.Button;
            switch (butn.Tag.ToString())
            {
               case "calc":
                  Process.Start("calc.exe");
                  break;
               case "devicemanager":
                  Process.Start("devmgmt.msc");
                  break;
               case "localservices":
                  Process.Start("services.msc");
                  break;
               case "sharefolder":
                  Process.Start("explorer.exe", @"\\localhost");
                  break;
               case "appfolder":
                  Process.Start("explorer.exe", Environment.CurrentDirectory);
                  break;
               case "ssms":
                  Process.Start("ssms.exe");
                  break;
               case "mstsc":
                  Process.Start("mstsc.exe");
                  break;
               case "magnifire":
                  Process.Start("magnify.exe");
                  break;
               case "controlpanel":
                  Process.Start("control.exe");
                  break;
               case "cmd":
                  Process.Start("cmd.exe");
                  break;
               case "anydesk":
                  Process.Start(@"C:\Program Files (x86)\AnyDesk\AnyDesk.exe");
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

      private void bbi_AddCoch_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "", 145 /* Execute Bas_Adch_F */, SendType.Self)
         );
      }

      private void bbi_AllCoch_Butn_Click(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 144 /* Execute Bas_Dfin_F */),
                new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("showtabpage", "tp_005"))}
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Master_Tc_SelectedIndexChanged(object sender, EventArgs e)
      {
         
      }

      private void EventX_Rb_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            if(EventToday_Rb.Checked)
            {
               vEvntBs.DataSource = iScsc.V_Events.Where(ev => ev.EVNT_DATE.Value.Date == DateTime.Now.Date);
            }
            else if (EventFuture_Rb.Checked)
            {
               vEvntBs.DataSource = iScsc.V_Events.Where(ev => ev.EVNT_DATE.Value.Date > DateTime.Now.Date && ev.EVNT_DATE.Value.Date <= DateTime.Now.Date.AddDays((int)EventDays_Spn.Value));
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void EventDays_Spn_ValueChanged(object sender, EventArgs e)
      {
         EventX_Rb_CheckedChanged(null, null);
      }

      private void tol_opengatebutn_ItemClick(object sender, EventArgs e)
      {
         iScsc.INS_LGOP_P(
            new XElement("Log",
               new XAttribute("fileno", ""),
               new XAttribute("type", "007"),
               new XAttribute("text", "گیت به صورت دستی توسط کاربر روبه داخل باز شد")
            )
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                  new Job(SendType.SelfToUserInterface, GetType().Name, 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "gatecontrol"),
                           new XAttribute("gateactn", "open")
                        )
                  }
               }
            )
         );
      }

      private void tol_closegatebutn_ItemClick(object sender, EventArgs e)
      {
         iScsc.INS_LGOP_P(
            new XElement("Log",
               new XAttribute("fileno", ""),
               new XAttribute("type", "007"),
               new XAttribute("text", "گیت به صورت دستی توسط کاربر روبه بیرون باز شد")
            )
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                  new Job(SendType.SelfToUserInterface, GetType().Name, 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("MainPage",
                           new XAttribute("type", "gatecontrol"),
                           new XAttribute("gateactn", "close")
                        )
                  }
               }
            )
         );
      }

      private void ksk_incmbutn_ItemClick(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {                  
                     new Job(SendType.Self, 157 /* Execute Ksk_Incm_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void ShowRbonMenu_Btn_Click(object sender, EventArgs e)
      {
         MainRbonMenu_Rbnc.Visible = !MainRbonMenu_Rbnc.Visible;

         _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\tick.wav";
         new Thread(AlarmShow).Start();
      }

      private void bbi_clenupbutn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.ExecuteCommand(
               "DELETE dbo.Payment_Detail WHERE CODE = 0;" + Environment.NewLine + 
               "DELETE dbo.Payment_Method WHERE Pymt_Rqst_Rqid IN (SELECT r.Rqid FROM Request r WHERE r.Rqst_Stat = '003');"
            );

            _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\timeout.wav";
            new Thread(AlarmShow).Start();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void bbi_oprcompbutn_Click(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 167 /* Execute Opr_Comp_F */),
                new Job(SendType.SelfToUserInterface, "OPR_COMP_F", 10 /* Actn_CalF_P */)
                {
                   Input = new XElement("Request", "")
                }
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_ProdDef_Butn_Click(object sender, EventArgs e)
      {
         /// Must Be Change
         Job _InteractWithScsc =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 168 /* Execute Opr_Comp_F */),
                new Job(SendType.SelfToUserInterface, "DEF_PROD_F", 10 /* Actn_CalF_P */)
                {
                   Input = new XElement("Request", "")
                }
              });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void DresBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _dres = DresBs.Current as Data.Dresser;
            if (_dres == null) return;

            AttnBs.DataSource =
               iScsc.Attendances
               .Where(a => a.ATTN_DATE == DateTime.Now.Date && a.ATTN_STAT == "002" && a.EXIT_TIME == null && a.DERS_NUMB == _dres.DRES_NUMB).OrderByDescending(a => a.ENTR_TIME).Take(1);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ActnCrntServOpenDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RSignalOpen_Butn.Tag == null)
            {
               RSignalOpen_Butn.Tag = "key";
               LSignalOpen_Butn.Tag = null;
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(RSignalOpen_Butn))).Start();
            }
            else
            {
               RSignalOpen_Butn.Tag = null;
               LSignalOpen_Butn.Tag = "key";
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(LSignalOpen_Butn))).Start();
            }

            Partners_Butn.Visible = PrtnrCont_Butn.Visible = PrtnrPos_Butn.Visible = PartnerDresNum_Butn.Visible = false;
            PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = PartnerDresNum_Butn.Visible = false;
            var _attn = CrntOpenBs.Current as Data.Attendance;
            if (_attn == null) return;

            var dres = _attn.Dresser_Attendances.FirstOrDefault().Dresser as Data.Dresser;
            if (dres == null) return;

            if (AcptActnDres_Cbx.Checked && MessageBox.Show(this, "آیا با انجام عملیات موافق هستید؟", "تایید فعالیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            // 1402/08/30 * User access to open online locker
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
                                 "<Privilege>272</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 272 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),
                     #region Dowork
                     new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("OprtDres",
                                 new XAttribute("type", "sendoprtdres"),
                                 new XAttribute("cmndname", dres.DRES_NUMB),
                                 new XAttribute("devip", dres.IP_ADRS),
                                 new XAttribute("cmndsend", dres.CMND_SEND ?? "")
                                 )
                     }
                     #endregion
                  })
            );

            // 1402/10/21 * باز کردن کمدهای همراهان
            if (iScsc.Dresser_Attendances.Any(da => da.ATTN_CODE == _attn.CODE && da.DRAT_CODE != null))
            {
               Partners_Butn.Visible = PartnerDresNum_Butn.Visible = PrtnrPos_Butn.Visible = true;
               PrtnrCont_Butn.Visible = PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = true;
               new Thread(new ThreadStart(() => OpenDresPart_Tmr_Tick(_attn.CODE))).Start();
            }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("OprtDres",
            //               new XAttribute("type", "sendoprtdres"),
            //               new XAttribute("cmndname", dres.DRES_NUMB),
            //               new XAttribute("devip", dres.IP_ADRS),
            //               new XAttribute("cmndsend", dres.CMND_SEND ?? "")
            //             )
            //   }
            //);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DresConf_Butn_Click(object sender, EventArgs e)
      {
         OnlineDres_Butn_ButtonClick(OnlineDres_Butn, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(OnlineDres_Butn.Properties.Buttons[0]));
      }

      private void CrntServOpenDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            long fileno = 0;
            var _btn = (MaxUi.Button)sender;
            switch (_btn.Tag.ToString())
            {
               case "attn":
                  var _attn = CrntOpenBs.Current as Data.Attendance;
                  if (_attn == null) return;
                  fileno = _attn.FIGH_FILE_NO;
                  break;
               case "figh":
                  var _figh = FighBs.Current as Data.Fighter;
                  if (_figh == null) return;
                  fileno = _figh.FILE_NO;
                  break;
               default:
                  return;
            }            

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SlctServDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs.Current as Data.Attendance;
            if (_attn == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _attn.FIGH_FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ActnSlctServOpenDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs.Current as Data.Attendance;
            if (_attn == null) return;

            var dres = _attn.Dresser_Attendances.FirstOrDefault().Dresser as Data.Dresser;
            if (dres == null) return;

            if (AcptActnDres_Cbx.Checked && MessageBox.Show(this, "آیا با انجام عملیات موافق هستید؟", "تایید فعالیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("OprtDres",
                           new XAttribute("type", "sendoprtdres"),
                           new XAttribute("cmndname", dres.DRES_NUMB),
                           new XAttribute("devip", dres.IP_ADRS),
                           new XAttribute("cmndsend", dres.CMND_SEND ?? "")
                         )
               }
            );

            // 1402/10/21 * باز کردن کمدهای همراهان
            if (iScsc.Dresser_Attendances.Any(da => da.ATTN_CODE == _attn.CODE && da.DRAT_CODE != null))
            {
               Partners_Butn.Visible = true;
               PartnerDresNum_Butn.Visible = false;
               new Thread(new ThreadStart(() => OpenDresPart_Tmr_Tick(_attn.CODE))).Start();
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DresActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _dres = DresBs.Current as Data.Dresser;
            if (_dres == null) return;

            int? _ordr = null;

            if (AcptActnDres_Cbx.Checked && MessageBox.Show(this, "آیا با انجام عملیات موافق هستید؟", "تایید فعالیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            switch (e.Button.Index)
            {
               case 0:
                  DresBs.MovePrevious();
                  var _getUp = DresBs.Current as Data.Dresser;
                  if (_getUp == _dres) return;

                  _ordr = _dres.ORDR;
                  _dres.ORDR = _getUp.ORDR;
                  _getUp.ORDR = _ordr;
                  break;
               case 1:
                  DresBs.MoveNext();
                  var _getDown = DresBs.Current as Data.Dresser;
                  if (_getDown == _dres) return;

                  _ordr = _dres.ORDR;
                  _dres.ORDR = _getDown.ORDR;
                  _getDown.ORDR = _ordr;
                  break;
               case 2:
                  // 1402/08/30 * User access to open online locker
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
                                       "<Privilege>272</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 272 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           #region Dowork
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input =
                                 new XElement("OprtDres",
                                       new XAttribute("type", "sendoprtdres"),
                                       new XAttribute("cmndname", _dres.DRES_NUMB),
                                       new XAttribute("devip", _dres.IP_ADRS),
                                       new XAttribute("cmndsend", _dres.CMND_SEND ?? "")
                                       )
                           }
                           #endregion
                        })
                  );
                  break;
               case 3:
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
                                       "<Privilege>271</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                       {
                                          //_dres.REC_STAT = _dres.REC_STAT == "002" ? "001" : "002";
                                          iScsc.ExecuteCommand(string.Format("UPDATE dbo.Dresser SET Rec_Stat = '{1}' WHERE Code = {0};", _dres.CODE, _dres.REC_STAT == "002" ? "001" : "002"));

                                          return;
                                       }
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 271 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),                           
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

      private void ExdvOprt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _btn = sender as MaxUi.Button;
            if (_btn == null) return;

            var _exdv = ExdvBs.Current as Data.External_Device;
            if (_exdv == null) return;

            // 1402/11/05 * Checked Device Use Limited
            if(AcptActnExdv_Cbx.Checked)
            {
               // 1402/11/05 * اگر میخواهیم چک کنیم که این دستگاه تایم استفاده کردنش هست یا خیر
               /*
                  001	یکشنبه
                  002	دو شنبه
                  003	سه شنبه
                  004	چهار شنبه
                  005	پنج شنبه
                  006	جمعه
                  007	شنبه
                */
               var _weekday = DateTime.Now.DayOfWeek;
               if (_exdv.External_Device_Weekdays.Count() > 0)
               {
                  if (_exdv.External_Device_Weekdays
                     .Any(ew => ew.STAT == "002"
                             && ew.WEEK_DAY == string.Format("00{0}", (int)(_weekday + 1)))
                  )
                  {
                     // checking must be Continue
                     if (_exdv.External_Device_Weekdays
                     .Any(ew => ew.STAT == "002"
                             && ew.WEEK_DAY == string.Format("00{0}", (int)(_weekday + 1))
                             && ew.External_Device_Weekday_Timings.Count() > 0)
                     )
                     {
                        if (!_exdv.External_Device_Weekdays
                           .Any(ew => ew.STAT == "002"
                                   && ew.WEEK_DAY == string.Format("00{0}", (int)(_weekday + 1))
                                   && ew.External_Device_Weekday_Timings.Any(et => et.STAT == "002"
                                   && DateTime.Now.TimeOfDay >= et.STRT_TIME.Value.TimeOfDay
                                   && DateTime.Now.TimeOfDay <= et.END_TIME.Value.TimeOfDay))
                        )
                        {
                           // Error 
                           if (_exdv.DEV_COMP_TYPE == "001")
                           {
                              var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                              SendCommand(_exdv.IP_ADRS, (int)_exdv.PORT_SEND, cmd);
                           }
                           else if (_exdv.DEV_COMP_TYPE == "003")
                           {
                              var cmd = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                              SendCommand(_exdv.IP_ADRS, (int)_exdv.PORT_SEND, cmd);
                           }
                           return;
                        }
                     }
                  }
                  else
                  {
                     // Error 
                     if (_exdv.DEV_COMP_TYPE == "001")
                     {
                        var cmd = new byte[] { 0xCC, 0x0D, 0x40, 0x28, 0x6B, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x04, 0x00, 0x00, 0xD1, 0xDD };
                        SendCommand(_exdv.IP_ADRS, (int)_exdv.PORT_SEND, cmd);
                     }
                     else if (_exdv.DEV_COMP_TYPE == "003")
                     {
                        var cmd = new byte[] { 0xFA, 0xBA, 0x05, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEE };
                        SendCommand(_exdv.IP_ADRS, (int)_exdv.PORT_SEND, cmd);
                     }
                     return;
                  }
               }
            }

            switch (_btn.Tag.ToString())
            {
               case "opst":
                  #region OpenStart
                  switch (_exdv.DEV_COMP_TYPE)
	               {
                     case "001":
                     case "003":
                        if (_exdv.DEV_TYPE != "006") return;

                        iScsc.INS_LGOP_P(
                           new XElement("Log",
                              new XAttribute("fileno", ""),
                              new XAttribute("type", "007"),
                              new XAttribute("text", "گیت به صورت دستی توسط کاربر روبه داخل باز شد")
                           )
                        );

                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                              {
                                 new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                                 {
                                    Input = 
                                       new XElement("MainPage",
                                          new XAttribute("type", "extdev"),
                                          new XAttribute("devtype", "006"),
                                          new XAttribute("contype", "002"),
                                          new XAttribute("cmdtype", "open"),
                                          new XAttribute("cmdsend", ""),
                                          new XAttribute("ip", _exdv.IP_ADRS),
                                          new XAttribute("sendport", _exdv.PORT_SEND)
                                       )
                                 }
                              }
                           )
                        );
                        break;
                     case "002":
                        iScsc.INS_LGOP_P(
                           new XElement("Log",
                              new XAttribute("fileno", ""),
                              new XAttribute("type", "007"),
                              new XAttribute("text", "رله فرمان دستگاه به صورت دستی توسط کاربر باز شد")
                           )
                        );

                        SendCommandDevExpn("in", _exdv.DEV_NAME, "");
                        break;
	               }
                  #endregion
                  break;
               case "clst":
                  #region CloseStop
                  switch (_exdv.DEV_COMP_TYPE)
                  {
                     case "001":
                     case "003":
                        if (_exdv.DEV_TYPE != "006") return;
                        iScsc.INS_LGOP_P(
                           new XElement("Log",
                              new XAttribute("fileno", ""),
                              new XAttribute("type", "007"),
                              new XAttribute("text", "گیت به صورت دستی توسط کاربر روبه بیرون باز شد")
                           )
                        );

                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                              {
                                 new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                                 {
                                    Input = 
                                       new XElement("MainPage",
                                          new XAttribute("type", "extdev"),
                                          new XAttribute("devtype", "006"),
                                          new XAttribute("contype", "002"),
                                          new XAttribute("cmdtype", "close"),
                                          new XAttribute("cmdsend", ""),
                                          new XAttribute("ip", _exdv.IP_ADRS),
                                          new XAttribute("sendport", _exdv.PORT_SEND)
                                       )
                                 }
                              }
                           )
                        );
                        break;
                     case "002":
                        iScsc.INS_LGOP_P(
                           new XElement("Log",
                              new XAttribute("fileno", ""),
                              new XAttribute("type", "007"),
                              new XAttribute("text", "رله فرمان دستگاه به صورت دستی توسط کاربر باز شد")
                           )
                        );

                        SendCommandDevExpn("out", _exdv.DEV_NAME, "");
                        break;
                  }
                  #endregion
                  break;
               case "er":
                  #region Error
                  switch (_exdv.DEV_COMP_TYPE)
                  {
                     case "001":
                     case "003":
                        if (_exdv.DEV_TYPE != "006") return;
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                              {
                                 new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                                 {
                                    Input = 
                                       new XElement("MainPage",
                                          new XAttribute("type", "extdev"),
                                          new XAttribute("devtype", "006"),
                                          new XAttribute("contype", "002"),
                                          new XAttribute("cmdtype", "error"),
                                          new XAttribute("cmdsend", ""),
                                          new XAttribute("ip", _exdv.IP_ADRS),
                                          new XAttribute("sendport", _exdv.PORT_SEND)
                                       )
                                 }
                              }
                           )
                        );
                        break;
                     case "002":
                        SendCommandDevExpn("er", _exdv.DEV_NAME, "");
                        break;
                  }
                  #endregion
                  break;
               case "ston":
                  #region SetOnline
                  switch (_exdv.DEV_COMP_TYPE)
                  {
                     case "001":
                     case "003":
                        if (_exdv.DEV_TYPE != "006") return;
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                              {
                                 new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                                 {
                                    Input = 
                                       new XElement("MainPage",
                                          new XAttribute("type", "extdev"),
                                          new XAttribute("devtype", "006"),
                                          new XAttribute("contype", "002"),
                                          new XAttribute("cmdtype", "gotoonline"),
                                          new XAttribute("cmdsend", ""),
                                          new XAttribute("ip", _exdv.IP_ADRS),
                                          new XAttribute("sendport", _exdv.PORT_SEND)
                                       )
                                 }
                              }
                           )
                        );
                        break;
                     case "002":
                        break;
                  }
                  #endregion
                  break;
               case "stof":
                  #region SetOffline
                  switch (_exdv.DEV_COMP_TYPE)
                  {
                     case "001":
                     case "003":
                        if (_exdv.DEV_TYPE != "006") return;
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                              {
                                 new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                                 {
                                    Input = 
                                       new XElement("MainPage",
                                          new XAttribute("type", "extdev"),
                                          new XAttribute("devtype", "006"),
                                          new XAttribute("contype", "002"),
                                          new XAttribute("cmdtype", "gotooffline"),
                                          new XAttribute("cmdsend", ""),
                                          new XAttribute("ip", _exdv.IP_ADRS),
                                          new XAttribute("sendport", _exdv.PORT_SEND)
                                       )
                                 }
                              }
                           )
                        );
                        break;
                     case "002":
                        break;
                  }
                  #endregion
                  break;
               case "slts":
                  #region SelfTest
                  switch (_exdv.DEV_COMP_TYPE)
                  {
                     case "001":
                     case "003":
                        if (_exdv.DEV_TYPE != "006") return;
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                              {
                                 new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_F */)
                                 {
                                    Input = 
                                       new XElement("MainPage",
                                          new XAttribute("type", "extdev"),
                                          new XAttribute("devtype", "006"),
                                          new XAttribute("contype", "002"),
                                          new XAttribute("cmdtype", "test"),
                                          new XAttribute("cmdsend", ""),
                                          new XAttribute("ip", _exdv.IP_ADRS),
                                          new XAttribute("sendport", _exdv.PORT_SEND)
                                       )
                                 }
                              }
                           )
                        );
                        break;
                     case "002":
                        break;
                     //case "003":
                     //   _DefaultGateway.Gateway(
                     //      new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                     //      {
                     //         Input =
                     //            new XElement("OprtDres",
                     //                new XAttribute("type", "sendoprtdres"),
                     //                new XAttribute("cmndname", "test"),
                     //                new XAttribute("devip", _exdv.IP_ADRS)
                     //            )
                     //      }
                     //   );
                     //   break;
                  }
                  #endregion
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Master_Tc_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
      {
         try
         {
            switch (e.Page.Name)
            {
               case "xTp_001":
                  EventX_Rb_CheckedChanged(null, null);
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

      private void AddFngrDevOpr(zkemkeeper.CZKEMClass _dev, string _ip, string _name)
      {
         try
         {
            if (!FngrDevList_Flp.Controls.OfType<MaxUi.Button>().Any(b => (b.Tag as zkemkeeper.CZKEMClass) == _dev))
            {
               var _btnDev = new MaxUi.Button() { Size = new Size(180, 25), Text = string.Format("{0}-( {1} )", _ip, _name), Tag = _dev };
               _btnDev.Click += FngrDevOpr_Butn_Click;
               FngrDevList_Flp.Controls.Add(_btnDev);
            }
         }
         catch { }
      }

      private void FngrDevOpr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _dev = (sender as MaxUi.Button).Tag as zkemkeeper.CZKEMClass;

            if (_dev == null) return;

            if(StrtNewEnrl_Rb.Checked)
            {
               // Remove all data from finger print device
               for (int i = 0; i < 10; i++)
               {
                  _dev.SSR_DelUserTmpExt(1, FngrPrntOpr_Txt.Text, i);
               }
               //axCZKEM1.SSR_DelUserTmpExt(1, enrollid, 6);
               _dev.DeleteUserInfoEx(1, Convert.ToInt32(FngrPrntOpr_Txt.Text));
               _dev.ClearSLog(1);

               _dev.SSR_SetUserInfo(1, FngrPrntOpr_Txt.Text, FngrPrntOpr_Txt.Text, "", 0, true);
               if (_dev.StartEnrollEx(FngrPrntOpr_Txt.Text, 6, 0))
               {
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else { BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red; }
            }
            else if(DelEnrl_Rb.Checked)
            {
               for (int i = 0; i < 10; i++)
               {
                  _dev.SSR_DelUserTmpExt(1, FngrPrntOpr_Txt.Text, i);
               }
               //axCZKEM1.SSR_DelUserTmpExt(1, enrollid, 6);
               _dev.DeleteUserInfoEx(1, Convert.ToInt32(FngrPrntOpr_Txt.Text));
               _dev.ClearSLog(1);

               BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Green;
            }
            else if(StrtCardEnrl_Rb.Checked)
            {
               _dev.EnableDevice(1, false);

               _dev.SetStrCardNumber(CardOpr_Txt.Text);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
               if (_dev.SSR_SetUserInfo(1, FngrPrntOpr_Txt.Text, FngrPrntOpr_Txt.Text, "", 0, true))//upload the user's information(card number included)
               {
                  //MessageBox.Show("کارت شما در سیستم ثبت گردید");
                  //FngrPrnt_Txt.Text = EnrollNumber;
                  //if (CardNumb_Text.Text == "") CardNumb_Text.Text = "0";
                  //CardNumb_Text.Text = (Convert.ToInt64(CardNumb_Text.Text) + 1).ToString();
                  //MessageBox.Show(string.Format("FngrPrnt {0} CardNumb {1} NameDnrm {2}", EnrollNumber, CardNumber, NameDnrm));
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else
               {
                  int idwErrorCode = 0;
                  _dev.GetLastError(ref idwErrorCode);
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
                  //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
               }
               _dev.RefreshData(1);//the data in the device should be refreshed
               _dev.EnableDevice(1, true);
            }
            else if(DelRecd_Rb.Checked)
            {
               /*
                * Clear the record specified by DataFlag from the device. DataFlag Type of the records to be cleared. 
                * The value ranges from 1 to 5. The meanings are as follows: 
                * 1. Attendance record 
                * 2. Fingerprint template data 
                * 3. None 
                * 4. Operation record 
                * 5. User information When the value of this parameter is 5, all user data in the device is deleted. 
                * Note: All fingerprint templates are also deleted.
                */
               int iDataFlag = (int)DelRecd_Cmx.SelectedIndex;
               //iDataFlag = 1;
               if (iDataFlag.In(2, 5) && MessageBox.Show(this, "با انجام این عملیات کلیه اطلاعات درون دستگاه پاک میشود، ایا مطمئن به انجام کار هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               if (_dev.ClearData(1, iDataFlag))
               {
                  _dev.RefreshData(1);
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.BlanchedAlmond;
               }
               else
               {
                  int idwErrorCode = 0;
                  _dev.GetLastError(ref idwErrorCode);
                  BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.Red;
               }
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FngrPrntOpr_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _btnedt = (ButtonEdit)sender;
            FngrPrnt_Txt.Text = _btnedt.Text;

            ShowInfo_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddNewNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (NoteBs.List.OfType<Data.Note>().Any(n => n.CODE == 0)) return;

            var _note = NoteBs.AddNew() as Data.Note;
            _note.NOTE_DATE = DateTime.Now;

            iScsc.Notes.InsertOnSubmit(_note);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SubmitNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
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
               Execute_Query();
         }
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _note = NoteBs.Current as Data.Note;
            if (_note == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Notes.DeleteOnSubmit(_note);
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

      private void ClenNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _note = NoteBs.Current as Data.Note;
            if (_note == null) return;

            _note.NOTE_SUBJ = _note.NOTE_CMNT = null;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ActnDvip_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _dvip = DVipBs.Current as Data.Dresser_Vip_Fighter;
            if (_dvip == null) return;

            if (AcptActnDres_Cbx.Checked && MessageBox.Show(this, "آیا با انجام عملیات موافق هستید؟", "تایید فعالیت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            switch (e.Button.Index)
            {
               case 0:
                  if (MessageBox.Show(this, "ایا با آزاد کردن کمد VIP موافق هستید?", "آزاد کردن کمد VIP", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Dresser_Vip_Fighter SET Stat = '001', Expr_Date = GETDATE() WHERE Code = {0};", _dvip.CODE));
                  break;
               case 1:
                  // 1402/08/30 * User access to open online locker
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
                                       "<Privilege>272</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 272 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           #region Dowork
                           new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input =
                                 new XElement("OprtDres",
                                       new XAttribute("type", "sendoprtdres"),
                                       new XAttribute("cmndname", _dvip.Dresser.DRES_NUMB),
                                       new XAttribute("devip", _dvip.Dresser.IP_ADRS),
                                       new XAttribute("cmndsend", _dvip.Dresser.CMND_SEND ?? "")
                                       )
                           }
                           #endregion
                        })
                  );
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ServDvip_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _dvip = DVipBs.Current as Data.Dresser_Vip_Fighter;
            if (_dvip == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _dvip.MBSP_FIGH_FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void StrtOpenDvip_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            TestLockerInLoop_Tmr.Enabled = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      List<Data.Dresser> _dressersList = new List<Data.Dresser>();
      Data.Dresser_Vip_Fighter _dvip = null;
      private void TestLockerInLoop_Tmr_Tick(object sender, EventArgs e)
      {
         try
         {
            TestLockerInLoop_Tmr.Enabled = false;
            TestLockerInLoop_Tmr.Interval = (int)(4000);

            if(_dvip == null)
               _dvip = DVipBs.Current as Data.Dresser_Vip_Fighter;
            
            if (_dvip == null) return;

            if (_dressersList.Count == DVipBs.List.OfType<Data.Dresser_Vip_Fighter>().Where(dv => dv.STAT == "002" && (!LockCntrl_Cbx.Checked || dv.Dresser.IP_ADRS == _dvip.Dresser.IP_ADRS)).Count())
            {
               _dressersList.Clear();
               _dvip = null;
               return;
            }

            Data.Dresser_Vip_Fighter _iDvipRec = DVipBs.List.OfType<Data.Dresser_Vip_Fighter>().Where(dv => dv.STAT == "002" && (!LockCntrl_Cbx.Checked || dv.Dresser.IP_ADRS == _dvip.Dresser.IP_ADRS) && !_dressersList.Any(_d => _d.CODE == dv.DRES_CODE)).OrderBy(d => d.Dresser.DRES_NUMB).FirstOrDefault();
            Data.Dresser _iRec = _iDvipRec.Dresser;
            _dressersList.Add(_iRec);
            DVipBs.Position = DVipBs.IndexOf(_iDvipRec);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("OprtDres",
                        new XAttribute("type", "sendoprtdres"),
                        new XAttribute("cmndname", _iRec.DRES_NUMB),
                        new XAttribute("devip", _iRec.IP_ADRS),
                        new XAttribute("cmndsend", _iRec.CMND_SEND ?? "")
                     )
               }
            );

            TestLockerInLoop_Tmr.Enabled = true;         
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void OpenDresPart_Tmr_Tick(long attncode)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var _drat = iScsc.Dresser_Attendances.Where(da => da.ATTN_CODE == attncode && da.DRAT_CODE != null).OrderBy(d => d.DERS_NUMB);
            int i = 0;
            PrtnrProc1_Pbc.Properties.Maximum = PrtnrProc2_Pbc.Properties.Maximum = PrtnrProc3_Pbc.Properties.Maximum = _drat.Count();
            PrtnrCont_Butn.Caption = _drat.Count().ToString();
            foreach (var _dres in _drat)
            {
               ++i;
               Thread.Sleep(4000);
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                  {
                     Input =
                        new XElement("OprtDres",
                           new XAttribute("type", "sendoprtdres"),
                           new XAttribute("cmndname", _dres.Dresser.DRES_NUMB),
                           new XAttribute("devip", _dres.Dresser.IP_ADRS),
                           new XAttribute("cmndsend", _dres.Dresser.CMND_SEND ?? "")
                        )
                  }
               );
               PartnerDresNum_Butn.Caption = _dres.Dresser.DRES_NUMB.ToString();
               PrtnrPos_Butn.Caption = i.ToString();
               PrtnrProc1_Pbc.Position = PrtnrProc2_Pbc.Position = PrtnrProc3_Pbc.Position = i;
            }
         }
         catch(Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void ANote_Lov_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  break;
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
                                    new XAttribute("tablename", "Note_Tag_Info"),
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
            MessageBox.Show(exc.Message);
         }
      }      

      private void GetValuDataFromExtrDev(Data.External_Device exdv, string valu)
      {
         try
         {
            if (RSignalGet_Butn.Tag == null)
            {
               RSignalGet_Butn.Tag = "key";
               LSignalGet_Butn.Tag = null;
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(RSignalGet_Butn))).Start();
            }
            else
            {
               RSignalGet_Butn.Tag = null;
               LSignalGet_Butn.Tag = "key";
               _wplayer_url = "";
               new Thread(new ThreadStart(() => AlarmShow(LSignalGet_Butn))).Start();
            }

            CrntValuGetData_Butn.Text = valu;
            if (exdv != null)
               CrntDevGetData_Butn.Text = string.Format("{0}:{1}:{2}", exdv.IP_ADRS, exdv.PORT_RECV, exdv.PORT_SEND);
            else
               CrntDevGetData_Butn.Text = "normal reader...";

            if (!GetSignalDataProc_Cbx.Checked) return;            

            FighBs.DataSource = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == valu);
            if (FighBs.List.Count != 0)
            {
               var _figh = FighBs.Current as Data.Fighter;
               if (_figh != null)
               {
                  if (_figh.IMAG_RCDC_RCID_DNRM != null)
                  {
                     try
                     {
                        ServProFileGetData_Rb.ImageProfile = null;
                        ServProFileGetData_Rb.ImageVisiable = true;
                        MemoryStream mStream = new MemoryStream();
                        byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _figh.FILE_NO))).ToArray();
                        mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                        Bitmap bm = new Bitmap(mStream, false);
                        mStream.Dispose();

                        if (InvokeRequired)
                           Invoke(new Action(() => ServProFileGetData_Rb.ImageProfile = bm));
                        else
                           ServProFileGetData_Rb.ImageProfile = bm;

                        ServProFileGetData_Rb.Tag = _figh.FILE_NO;
                     }
                     catch { }
                  }
                  else
                  {
                     ServProFileGetData_Rb.ImageProfile = null;
                     ServProFileGetData_Rb.Tag = null;
                  }

                  if (ServProFileGetData_Rb.ImageProfile == null && _figh.SEX_TYPE_DNRM == "002")
                     ServProFileGetData_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1148;
                  else if (ServProFileGetData_Rb.ImageProfile == null)
                     ServProFileGetData_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1149;
               }
               else
               {
                  ServProFileGetData_Rb.ImageProfile = null;
                  ServProFileGetData_Rb.Tag = null;
               }
            }               
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FighBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _figh = FighBs.Current as Data.Fighter;
            if (_figh == null) { CrntServGetData_Butn.Text = "---"; return; }

            CrntServGetData_Butn.Text = _figh.NAME_DNRM;
         }
         catch { }
      }

      private void SrvrPing_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
               _wplayer_url = @".\Media\SubSys\Kernel\Desktop\Sounds\radar.mp3";
               new Thread(AlarmShow).Start();
               DeviceOnNetworks.OfType<Device_On_Network>().ToList().ForEach(d => { d.PingStatus = false; });
               Tm_ShowTime_Tick(null, null);
            }
         }
         catch { }
      }

      private void AtnwFind_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            if(AtnwDate_Rb.Checked && !AtnwDate_Dt.Value.HasValue){AtnwDate_Dt.Focus(); AtnwDate_Dt.Value = DateTime.Now;}
            var _date = AtnwToday_Rb.Checked ? DateTime.Now : (AtnwDate_Rb.Checked ? AtnwDate_Dt.Value : null);
            if (AtnwBfor_Rb.Checked) { _date = DateTime.Now; }
            
            AtnwBs.DataSource =
               iScsc.Attendance_Wrists
               .Where(aw => 
                  (AtnwBfor_Rb.Checked ? (aw.Attendance.ATTN_DATE < DateTime.Now.Date) : aw.Attendance.ATTN_DATE == _date.Value.Date) &&
                  (aw.STAT == (AtnwStat001_Cbx.Checked ? "001" : "000") || aw.STAT == (AtnwStat002_Cbx.Checked ? "002" : "000"))
               );
         }
         catch { }
      }

      private void AtnwActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _atnw = AtnwBs.Current as Data.Attendance_Wrist;
            if (_atnw == null) return;

            string _tmp = "";

            switch (e.Button.Index)
            {
               case 0:
                  if (_atnw.Dresser == null) return;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new XElement("OprtDres",
                                 new XAttribute("type", "sendoprtdres"),
                                 new XAttribute("cmndname", _atnw.Dresser.DRES_NUMB),
                                 new XAttribute("devip", _atnw.Dresser.IP_ADRS),
                                 new XAttribute("cmndsend", _atnw.Dresser.CMND_SEND ?? "")
                               )
                     }
                  );

                  _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                  iScsc.INS_LGOP_P(
                     new XElement("Log",
                           new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                           new XAttribute("type", "016"),
                           new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " به صورت دستی کمد شماره " + _tmp + " باز کرد")
                     )
                  );
                  break;
               case 1:
                  if ( MessageBox.Show(this, "آیا با تحویل و برگشت دستبند موافق هستید؟", "عملیات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                  // اگر قبلا این دستبند برگشت داده شده باشد
                  if (AtnwBs.List.OfType<Data.Attendance_Wrist>().Any(aw => /*aw.ATTN_CODE == _atnw.ATTN_CODE && aw.ATNW_FNGR_PRNT_DNRM == WristBand_Txt.Text*/aw.CODE == _atnw.CODE && aw.STAT == "002")) return;

                  // وضعیت برگشت دستبند را فعال میکنیم
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Attendance_Wrist SET STAT = '002' WHERE CODE = {0};", /*_atnw.ATTN_CODE, WristBand_Txt.Text*/ _atnw.CODE));

                  _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                  iScsc.INS_LGOP_P(
                     new XElement("Log",
                           new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                           new XAttribute("type", "015"),
                           new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " به صورت دستی کمد شماره " + _tmp + " را تحویل گرفت")
                     )
                  );
                  break;
               case 2:
                  if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف دستبند", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                  iScsc.ExecuteCommand(string.Format("DELETE dbo.Attendance_Wrist WHERE Code = {0};", _atnw.CODE));

                  _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                  iScsc.INS_LGOP_P(
                     new XElement("Log",
                           new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                           new XAttribute("type", "014"),
                           new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " به صورت دستی کمد شماره " + _tmp + " را حذف کرد")
                     )
                  );

                  if (AtnwBs.List.Count > 1)
                  {
                     iScsc.ExecuteCommand(
                        string.Format(
                           "UPDATE dbo.Attendance SET SUM_ATTN_MONT_DNRM -= 1 WHERE CODE = {0};" + Environment.NewLine +
                           "UPDATE ms SET ms.SUM_ATTN_MONT_DNRM -= 1 FROM dbo.Member_Ship ms, dbo.Attendance a WHERE a.Code = {0} AND a.MBSP_RWNO_DNRM = ms.RWNO AND a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND ms.RECT_CODE = '004';",
                        _atnw.ATTN_CODE)
                     );

                     _tmp = _atnw.Dresser == null ? _atnw.Fighter1.FNGR_PRNT_DNRM : _atnw.Dresser.DRES_NUMB.ToString();

                     iScsc.INS_LGOP_P(
                        new XElement("Log",
                              new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM),
                              new XAttribute("type", "012"),
                              new XAttribute("text", "کاربر " + CurrentUser + " برای مشتری " + _atnw.Attendance.NAME_DNRM + " برگشت یک جلسه به دوره مشتری به صورت دستی بابت حذف دستبند شماره " + _tmp + " انجام شد")
                        )
                     );
                  }
                  break;
               case 3:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM)) }
                  );
                  break;
               default:
                  break;
            }
         }
         catch { }
      }

      private void AtnwBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _atnw = AtnwBs.Current as Data.Attendance_Wrist;
            if (_atnw == null) return;

            //AtnwCont_Btn.Text = 

            var _figh = _atnw.Fighter;
            if (_figh != null)
            {
               if (_figh.IMAG_RCDC_RCID_DNRM != null)
               {
                  try
                  {
                     ServProFileAtnw_Rb.ImageProfile = null;
                     ServProFileAtnw_Rb.ImageVisiable = true;
                     MemoryStream mStream = new MemoryStream();
                     byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _figh.FILE_NO))).ToArray();
                     mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                     Bitmap bm = new Bitmap(mStream, false);
                     mStream.Dispose();

                     if (InvokeRequired)
                        Invoke(new Action(() => ServProFileAtnw_Rb.ImageProfile = bm));
                     else
                        ServProFileAtnw_Rb.ImageProfile = bm;

                     ServProFileAtnw_Rb.Tag = _figh.FILE_NO;
                  }
                  catch { }
               }
               else
               {
                  ServProFileAtnw_Rb.ImageProfile = null;
                  ServProFileAtnw_Rb.Tag = null;
               }

               if (ServProFileAtnw_Rb.ImageProfile == null && _figh.SEX_TYPE_DNRM == "002")
                  ServProFileAtnw_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1148;
               else if (ServProFileAtnw_Rb.ImageProfile == null)
                  ServProFileAtnw_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1149;
            }
            else
            {
               ServProFileAtnw_Rb.ImageProfile = null;
               ServProFileAtnw_Rb.Tag = null;
            }
         }
         catch { }
      }

      private void AtwnWrst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _atnw = AtnwBs.Current as Data.Attendance_Wrist;
            if (_atnw == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _atnw.ATNW_FIGH_FILE_NO)) }
            );
         }
         catch { }
      }

      private void AtnwFigh_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _atnw = AtnwBs.Current as Data.Attendance_Wrist;
            if (_atnw == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _atnw.FIGH_FILE_NO_DNRM)) }
            );
         }
         catch { }
      }
   }
}
