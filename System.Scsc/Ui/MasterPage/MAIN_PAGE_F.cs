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

namespace System.Scsc.Ui.MasterPage
{
   public partial class MAIN_PAGE_F : UserControl
   {
      public MAIN_PAGE_F()
      {
         InitializeComponent();         
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
            HeaderAttnButn_Tooltip.Text = string.Format("وضعیت دستگاه گیت : {0}", Convert.ToBoolean(xdata.Attribute("stat").Value) ? "فعال" : "غیرفعال");
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

         

         return SuperToolTips;
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

            if (barCodeSetting.ATTN_SYST_TYPE != "001") return;

            this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;

            Sp_Barcode.PortName = barCodeSetting.COMM_PORT_NAME;
            Sp_Barcode.BaudRate = (int)barCodeSetting.BAND_RATE;
            Sp_Barcode.Open();

            if (Sp_Barcode.IsOpen)
            {
               AttnType_Lov.EditValue = "001";
               this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1212;
               Tsp_AttnSys.Text = "سیستم بارکد خوان فعال";
               Tsp_AttnSys.ForeColor = Color.Green;
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
            iScsc = new Data.iScscDataContext(ConnectionString);
            var barCodeSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
            var enrollNumber = Sp_Barcode.ReadLine();
            enrollNumber = enrollNumber.Substring(0, enrollNumber.IndexOf('\r'));
            //if (enrollNumber == oldenrollnumber && MessageBox.Show(this, "کارت مشترک دوباره قرار گرفته آیا می خواهید دوباره مورد بررسی قرار گیرد؟", "تکرار قرار گیری کارت مشترک", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            //   return;
            //oldenrollnumber = enrollNumber;

            if (barCodeSetting.BAR_CODE_DATA_TYPE == "001")
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
            
            //MessageBox.Show(enrollNumber);

            if (InvokeRequired)
               Invoke(new Action(() => axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1)));
            else 
               axCZKEM1_OnAttTransactionEx(enrollNumber, 0, 0, 0, 1395, 1, 1, 1, 1, 1, 1);
         }
         catch { }
      }
      #endregion

      #region GateAttn
      private void Start_GateAttn()
      {
         try
         {
            var gateAttnSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();

            if (gateAttnSetting == null) return;

            if (gateAttnSetting.GATE_ATTN_STAT == "001") return;            

            Sp_GateAttn.PortName = gateAttnSetting.GATE_COMM_PORT_NAME;
            Sp_GateAttn.BaudRate = (int)gateAttnSetting.GATE_BAND_RATE;
            GateAttn_Butn.Tag = gateAttnSetting;
            Sp_GateAttn.Open();

            if (Sp_GateAttn.IsOpen)
            {
               GateAttn_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1561;               
            }
            else
            {
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
                     new XAttribute("stat", bIsConnected)
                  )
               );
         }
      }

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

      private void Open_Gate()
      {
         try
         {
            // Open This Gate
            var gateAttnStng = GateAttn_Butn.Tag as Data.Setting;
            if (gateAttnStng == null) return;

            if (gateAttnStng.GATE_ENTR_OPEN == "001") return;
            Sp_GateAttn.Write("in");
            //MessageBox.Show("Gate is Open");
         }catch(Exception exc){}
      }

      private void Close_Gate()
      {
         try
         {
            // Close This Gate
            var gateAttnStng = GateAttn_Butn.Tag as Data.Setting;
            if (gateAttnStng == null) return;

            if (gateAttnStng.GATE_EXIT_OPEN == "001") return;
            Sp_GateAttn.Write("out");
            //MessageBox.Show("Gate is Close");
         }
         catch (Exception exc) { }
      }
      #endregion

      #region ExpnExtr
      private void Start_ExpnExtr()
      {
         var expnExtrSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
         try
         {
            if (expnExtrSetting == null) return;

            if (expnExtrSetting.EXPN_EXTR_STAT == "001") { ExpnExtr_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196; return; }

            Sp_ExpnExtr.PortName = expnExtrSetting.EXPN_COMM_PORT_NAME;
            Sp_ExpnExtr.BaudRate = (int)expnExtrSetting.EXPN_BAND_RATE;
            ExpnExtr_Butn.Tag = expnExtrSetting;
            Sp_ExpnExtr.Open();

            if (Sp_ExpnExtr.IsOpen)
            {
               ExpnExtr_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1607;
            }
            else
            {
               ExpnExtr_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            }
         }
         catch
         {
            ExpnExtr_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
         }
         finally
         {
            ExpnExtr_Butn.SuperTip =
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
               ExpnExtr_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1196;
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }
      #endregion

      #region Finger Print
      public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
      bool bIsConnected = false;
      void Start_FingerPrint()
      {
         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
               {
                  AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     var host = output as XElement;

                     #region 1st Device
                     if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CONCT == host.Attribute("cpu").Value))
                     {
                        var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CONCT == host.Attribute("cpu").Value).FirstOrDefault();

                        if (fingerPrintSetting == null) return;

                        if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                        if (fingerPrintSetting.IP_ADDR != null && fingerPrintSetting.PORT_NUMB != null)
                        {
                           Tsp_AttnSys.Text = "در حال اتصال به دستگاه حضور غیاب...";
                           this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;
                           Tsp_AttnSys.ForeColor = SystemColors.ControlText;

                           if (!bIsConnected)
                           {
                              AttnType_Lov.EditValue = "001";
                              bIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADDR, Convert.ToInt32(fingerPrintSetting.PORT_NUMB));
                              // fire event for fetch 
                              axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                              // New code 
                              axCZKEM1.OnHIDNum += axCZKEM1_OnHIDNum;
                           }
                           if (bIsConnected == true)
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
                                    new XAttribute("stat", bIsConnected),
                                    host
                                 )
                              );
                        }
                     }
                     #endregion
                     #region 2nd Device
                     else if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC2 == host.Attribute("cpu").Value))
                     {
                        var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_COMP_CNC2 == host.Attribute("cpu").Value).FirstOrDefault();

                        if (fingerPrintSetting == null) return;

                        if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                        if (fingerPrintSetting.IP_ADR2 != null && fingerPrintSetting.PORT_NUM2 != null)
                        {
                           Tsp_AttnSys.Text = "در حال اتصال به دستگاه حضور غیاب...";
                           this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;
                           Tsp_AttnSys.ForeColor = SystemColors.ControlText;

                           if (!bIsConnected)
                           {
                              AttnType_Lov.EditValue = "001";
                              bIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADR2, Convert.ToInt32(fingerPrintSetting.PORT_NUM2));
                              // fire event for fetch 
                              axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                              // New code 
                              axCZKEM1.OnHIDNum += axCZKEM1_OnHIDNum;
                           }
                           if (bIsConnected == true)
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
                                    new XAttribute("ip", fingerPrintSetting.IP_ADR2),
                                    new XAttribute("stat", bIsConnected),
                                    host
                                 )
                              );
                        }
                     }
                     #endregion
                     #region other wise
                     else if(iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && (s.ATN1_EVNT_ACTN_TYPE == "002" || s.ATN2_EVNT_ACTN_TYPE == "002" || s.ATN3_EVNT_ACTN_TYPE == "003")))
                     {
                        var fingerPrintSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_SYST_TYPE == "002").FirstOrDefault();

                        if (fingerPrintSetting == null) return;

                        if (fingerPrintSetting.ATTN_SYST_TYPE != "002") return;

                        if (fingerPrintSetting.IP_ADDR != null && fingerPrintSetting.PORT_NUMB != null)
                        {
                           Tsp_AttnSys.Text = "در حال اتصال به دستگاه حضور غیاب...";
                           this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1218;
                           Tsp_AttnSys.ForeColor = SystemColors.ControlText;

                           if (!bIsConnected)
                           {
                              bIsConnected = axCZKEM1.Connect_Net(fingerPrintSetting.IP_ADDR, Convert.ToInt32(fingerPrintSetting.PORT_NUMB));
                              // fire event for fetch 
                              axCZKEM1.OnAttTransactionEx += axCZKEM1_OnAttTransactionEx;
                              // New code 
                              axCZKEM1.OnHIDNum += axCZKEM1_OnHIDNum;
                           }
                           if (bIsConnected == true)
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
                                    new XAttribute("stat", bIsConnected),
                                    host
                                 )
                              );
                        }
                     }
                     #endregion
                     else
                     {
                        this.AttendanceSystemAlert_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1227;
                        Tsp_AttnSys.Text = "***";
                     }
                  })
               }
            );
      }

      void axCZKEM1_OnHIDNum(int CardNumber)
      {
         CardNumb_Text.Text = CardNumber.ToString();
         var control = spc_desktop.Panel1.Controls.OfType<Control>().FirstOrDefault();
         if (control == null) return;
         if (control.Name == "ADM_FIGH_F" || control.Name == "ADM_CHNG_F" || control.Name == "BAS_ADCH_F" || control.Name == "OIC_SMSN_F")
         {
            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, control.Name, 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", CardNumber))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }
      
      private void axCZKEM1_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
      {
         /*
          * در این قسمت بعد از اینکه کاربر درون دستگاه تعریف شد باید برای اولین بار در سیستم عمل ثبت نام صورت پذیرد
          * این حالت اولیه زمانی رخ میدهد که ما هیچ هنرجویی با این شماره اثر انگشت درون سیستم تعریف نشده باشد
          */
         //EnrollNumber = Microsof2t.VisualBasic.Interaction.InputBox("لطفا کد کاربری خود را وارد کنید");
         //if (enrollnumber == EnrollNumber && enrolldate.AddSeconds(5) <= DateTime.Now) { enrollnumber = ""; return; }
         //else { enrollnumber = EnrollNumber; enrolldate = DateTime.Now; }
         //ExtCode.ScreenSaver.KillScreenSaver();

         if (InvokeRequired)
            Invoke(new Action(() => OnAttTransactionEx(EnrollNumber)));
         else
            OnAttTransactionEx(EnrollNumber);
            return;
      }

      private void OnAttTransactionEx(string EnrollNumber)
      {
         if (AttnType_Lov.EditValue.ToString() != "001") { FngrPrnt_Txt.Text = EnrollNumber; if (AttnType_Lov.EditValue.ToString() == "003") { ShowInfo_Butn_Click(null, null); } return; }

         if (EnrollNumber == oldenrollnumber && MessageBox.Show(this, "شناسایی دوبار انجام شده است، آیا می خواهید دوباره مورد بررسی قرار گیرد؟", "تکرار قرار گیری اثرانگشت اعضا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
         oldenrollnumber = EnrollNumber;

         FngrPrnt_Txt.Text = EnrollNumber;
         if (
            !iScsc.Fighters
            .Any(f =>
               //Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) && 
               f.FNGR_PRNT_DNRM == EnrollNumber &&
               Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 &&
               !(f.FGPB_TYPE_DNRM == "002" && f.FGPB_TYPE_DNRM == "003")
            //(Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&                              
            )
         )
         {
            var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == EnrollNumber);

            if (figh != null && Convert.ToInt32(figh.ACTV_TAG_DNRM ?? "101") <= 100)
            {
               if (MessageBox.Show(this, "هنرجو مورد نظر در حالت حذف از سیستم قرار گرفته است. مایل به فعال کردن مجدد هنرجو هستید؟", "حضور مجدد هنرجوی غیرفعال", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               {
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
                                 new XElement("Actv_Tag", "101")
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
               }
               else
                  return;
            }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 99 /* Execute New_Fngr_F */),
                     new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("enrollnumber", EnrollNumber),
                           new XAttribute("isnewenroll", true)
                        )
                     }
                  })
            );
         }
         else
         {
            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", EnrollNumber))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      void Stop_FingerPrint()
      {
         if (bIsConnected)
         {
            axCZKEM1.Disconnect();
            bIsConnected = false;
            Tsp_AttnSys.Text = "دستگاه حضورغیاب غیرفعال شد";
         }
      }

      private void Tm_FingerPrintWorker_Tick(object sender, EventArgs e)
      {
         Start_FingerPrint();
         Start_BarCode();
         Start_GateAttn();
         Start_ExpnExtr();
         Tm_FingerPrintWorker.Enabled = false;
      }
      #endregion

      #region اطلاعات پایه
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

      #region اطلاعات ثبت نام
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
                  new Job(SendType.Self, 80 /* Execute Ins_Totl_F */)
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_flstbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
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

      #region ثبت نام متفرقه
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

      #region آمارها
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

      #region تنظیمات نرم افزار
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

      #region گزینه های متفرقه باشگاه
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

      #region شاخص مربیان
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

      #region گزینه کارهای تجمعی
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
                  new Job(SendType.Self, 121 /* Execute Aop_Mbsp_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_aglsbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                    
                  new Job(SendType.Self, 126 /* Execute Aop_Attn_F */)
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
                  new Job(SendType.Self, 125 /* Execute Aop_Mtod_F */)
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
      #endregion

      #region گزارشات
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
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("fromdate", DateTime.Now), new XAttribute("todate", DateTime.Now), new XAttribute("useraccount", "manager"))}
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
                  new Job(SendType.SelfToUserInterface, "DAP_PIVT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                  #endregion
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void bbi_lusrbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {

      }
      #endregion

      #region داشبورد
      private void bbi_bislbutn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  #region DoWork
                  new Job(SendType.Self, 135 /* Execute Rpt_Pmmt_F */),
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("fromdate", DateTime.Now), new XAttribute("todate", DateTime.Now), new XAttribute("useraccount", "user"))}
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
                  new Job(SendType.Self, 139 /* Execute Dap_Dshb_F */),
                  new Job(SendType.SelfToUserInterface, "DAP_DSHB_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
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

      }

      private void FIGH_FILE_NOLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if(e.Button.Index == 6)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
               return;
            }

            var fileno = Convert.ToInt64(FIGH_FILE_NOLookUpEdit.EditValue);

            var figh = FighBs.List.OfType<Data.Fighter>().First(f => f.FILE_NO == fileno);
            switch (e.Button.Index)
            {
               case 5:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                          new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                        })
                  );
                  break;
               case 1:
                  if (!(figh.FGPB_TYPE_DNRM == "001" || figh.FGPB_TYPE_DNRM == "005" || figh.FGPB_TYPE_DNRM == "006")) return;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
                     })
                  );
                  break;
               case 3:
                  if (MessageBox.Show(this, "آیا با حذف هنرجو موافق هستید؟", "عملیات حذف موقت هنرجو", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))},
                        new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 07 /* Execute Load_Data */),

                     })
                  );
                  break;
               case 2:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                     {
                        new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))}
                     })
                  );
                  break;
               case 4:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO)) }
                  );
                  break;
               default:
                  break;
            }
         }
         catch { }
      }
      #endregion

      private void CardNumb_Text_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (CardNumb_Text.Text == "") return;

            var control = spc_desktop.Panel1.Controls.OfType<Control>().FirstOrDefault();
            if (control == null) return;
            if (control.Name == "ADM_FIGH_F" || control.Name == "ADM_CHNG_F")
            {
               Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, control.Name, 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", CardNumb_Text.Text))}
                  });
               _DefaultGateway.Gateway(_InteractWithScsc);
            }
         }
         catch (Exception exc)
         {

         }
      }

      private void AttendanceSystemAlert_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 141 /* Execute WHO_ARYU_F */),
                  new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )
               })
         );
      }

      private void Tm_ShowTime_Tick(object sender, EventArgs e)
      {
         AdjustDateTime_Butn.Text = DateTime.Now.ToString("HH:mm:ss");
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
   }
}
