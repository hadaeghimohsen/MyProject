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
using DevExpress.XtraEditors;
using System.Globalization;
using System.Xml.Linq;
using zkemkeeper;
using System.Runtime.InteropServices;
using libzkfpcsharp;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Data.SqlClient;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsDevice : UserControl
   {
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

      [DllImport("user32.dll", EntryPoint = "SendMessageA")]
      public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

      public SettingsDevice()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
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
         iProject = new Data.iProjectDataContext(ConnectionString);
         if(Tb_Master.SelectedTab == tp_001)
         {
            ActiveSessionBs.DataSource = iProject.Active_Sessions.Where(a => a.RWNO == iProject.Active_Sessions.Where(at => at.USGW_GTWY_MAC_ADRS == a.USGW_GTWY_MAC_ADRS && at.USGW_USER_ID == a.USGW_USER_ID && at.USGW_RWNO == a.USGW_RWNO && at.AUDS_ID == a.AUDS_ID && at.ACTN_DATE.Value.Date == a.ACTN_DATE.Value.Date).Max(at => at.RWNO));
            CreateActiveSessionMenu();
         }
         else if (Tb_Master.SelectedTab == tp_002)
         {
            PortList_Cb.Items.Clear();
            PortList_Cb.Items.AddRange(SerialPort.GetPortNames());
            BandRate_Txt.Text = "9600";
         }
         else if(Tb_Master.SelectedTab == tp_003)
         {
            PosBs.DataSource = iProject.Pos_Devices;
            CreatePosMenu();
         }
      }

      private string MtoS(DateTime dt)
      {
         PersianCalendar pc = new PersianCalendar();
         return string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
      }

      private void CreateActiveSessionMenu()
      {
         ActiveSessionList_Flp.Controls.Clear();
         foreach (var item in ActiveSessionBs.List.OfType<Data.Active_Session>())
         {
            var activesession = new SimpleButton();
            activesession.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            activesession.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            activesession.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            activesession.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            activesession.Appearance.Options.UseFont = true;
            activesession.Appearance.Options.UseForeColor = true;
            activesession.Appearance.Options.UseTextOptions = true;
            activesession.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            activesession.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            activesession.Image = global::System.DataGuard.Properties.Resources.IMAGE_1415;
            activesession.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            activesession.Location = new System.Drawing.Point(146, 3);
            activesession.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            activesession.LookAndFeel.UseDefaultLookAndFeel = false;
            activesession.Name = "simpleButton1";
            activesession.Size = new System.Drawing.Size(215, 72);
            activesession.TabIndex = 1;
            activesession.Tag = "1";
            activesession.Text = string.Format("<b>{0}</b><br><color=Gray><size=-2>{1}</size></color><br><size=-3><color=Green>{2}</color></size><br><color=Red><size=-3>{3}</size></color>", item.User_Gateway.Gateway.COMP_NAME_DNRM, item.Access_User_Datasource.User.USERDB, item.Access_User_Datasource.DataSource.Database_Alias, MtoS((DateTime)item.ACTN_DATE));
            ActiveSessionList_Flp.Controls.Add(activesession);
         }

      }

      private void CreatePosMenu()
      {
         PosList_Flp.Controls.Clear();
         foreach (var item in PosBs.List.OfType<Data.Pos_Device>())
         {
            var pos = new SimpleButton();
            pos.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            pos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            if (item.POS_STAT == "002")
            {
               pos.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
               pos.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            }
            else
            {
               pos.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
               pos.Appearance.BorderColor = System.Drawing.Color.Silver;
            }
            pos.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pos.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            pos.Appearance.Options.UseBackColor = true;
            pos.Appearance.Options.UseBorderColor = true;
            pos.Appearance.Options.UseFont = true;
            pos.Appearance.Options.UseForeColor = true;
            pos.Appearance.Options.UseTextOptions = true;
            pos.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            pos.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            pos.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            pos.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
            pos.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            pos.Location = new System.Drawing.Point(306, 3);
            pos.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            pos.LookAndFeel.UseDefaultLookAndFeel = false;
            pos.Name = "simpleButton2";
            pos.Size = new System.Drawing.Size(215, 59);
            pos.TabIndex = 1;
            pos.Tag = item;
            pos.Click += Pos_Click;
            pos.Text = string.Format("{0}  {1}{4}<br><color=Gray><size=9>{2}</size></color><br>" + "<color=Green><size=9>{3}</size></color><br>", item.POS_DESC, item.POS_DFLT == "002" ? "<b>*</b>" : "", iProject.D_BANKs.FirstOrDefault(b => item.BANK_TYPE == b.VALU).DOMN_DESC + " : " + item.BNKB_CODE, "شماره حساب : " + item.BNKA_ACNT_NUMB, item.AUTO_COMM == "002" ? "<b>@</b>" : "");
            
            PosList_Flp.Controls.Add(pos);
         }
      }

      void Pos_Click(object sender, EventArgs e)
      {
         try
         {
            var pos = (sender as SimpleButton).Tag as Data.Pos_Device;
            if (pos == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 38 /* Execute DoWork4SettingsPaymentPos */),
                     new Job(SendType.SelfToUserInterface, "SettingsPaymentPos", 10 /* Execute ActionCallWindow */)
                     {
                        Input = 
                           new XElement("Pos",
                              new XAttribute("psid", pos.PSID)
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

      private void NewPos_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 37 /* Execute DoWork4SettingsNewPos */),
               }
            )
         );
      }

      #region دستگاه های متصل
      public class DeviceInfo
      {
         public string IP { get; set; }
         public int Port { get; set; }
         public int Id { get; set; }
         public string Status { get; set; }
         public DateTime StartDateTime { get; set; }
         public DateTime EndDateTime { get; set; }
         public string Oprt_Stat { get; set; }
      }

      private CZKEMClass iFngrMstr = new CZKEMClass();
      private CZKEMClass iFngrSlav = new CZKEMClass();
      private bool iFgnrMstrIsCnct = false;
      private bool iFngrSlavIsCnct = false;
      private void ConnectToDev_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if(Network_Rb.Checked)
            {
               iFgnrMstrIsCnct = iFngrMstr.Connect_Net(MasterDeviceIP_Txt.Text, Convert.ToInt32(MasterDevicePort_Txt.Text));

               if (iFgnrMstrIsCnct)
                  MessageBox.Show(this, "برقراری ارتباط با دستگاه با موفقیت انجام شد!", "Devicec", MessageBoxButtons.OK, MessageBoxIcon.Information);
               else
                  MessageBox.Show(this, "عدم برقراری با دستگاه!", "Devicec", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if(Usb_Rb.Checked)
            {
               bnInit_Click(null , null);                
            }
            DisConnectFromDev_Butn.Enabled = true;
            ConnectToDev_Butn.Enabled = false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DisConnectFromDev_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Network_Rb.Checked)
            {
               iFngrMstr.Disconnect();               
            }
            else if (Usb_Rb.Checked)
            {
               bnClose_Click(null, null);               
            }
            ConnectToDev_Butn.Enabled = true;
            DisConnectFromDev_Butn.Enabled = false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Usb_Rb_CheckedChanged(object sender, EventArgs e)
      {
         if (Network_Rb.Checked)
         {
            panel2.Visible = true;
            panel3.Visible = false;
            NewEnroll_Butn.Visible = true;
         }
         else if (Usb_Rb.Checked)
         {
            panel2.Visible = false;
            panel3.Visible = true;
            NewEnroll_Butn.Visible = false;
         }
      }

      private void AddDev_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (DevInfoBs.List.OfType<DeviceInfo>().Any(d => d.IP == SlaveDeviceIP_Txt.Text)) return;

            var dev = DevInfoBs.AddNew() as DeviceInfo;
            dev.IP = SlaveDeviceIP_Txt.Text;
            dev.Port = Convert.ToInt32(SlaveDevicePort_Txt.Text);
            dev.Id = Convert.ToInt32(SlaveDeviceId_Txt.Text);
            dev.Status = "No Connected";

            SlaveDeviceIP_Txt.Text = SlaveDevicePort_Txt.Text = "";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ReadFromFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (IPDev_Ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            // Read the file and display it line by line.              
            System.IO.StreamReader file =
                new System.IO.StreamReader(IPDev_Ofd.FileName);
            string line;
            while ((line = file.ReadLine()) != null)
            {
               SlaveDeviceIP_Txt.Text = line.Split(':')[0];
               SlaveDevicePort_Txt.Text = line.Split(':')[1];
               SlaveDeviceId_Txt.Text = line.Split(':')[2];

               AddDev_Butn_Click(null, null);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelDev_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DevInfoBs.RemoveCurrent();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NewEnroll_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (iFgnrMstrIsCnct)
            {
               var result = iFngrMstr.SSR_SetUserInfo(1, UserId_Txt.Text, UserId_Txt.Text, "", 0, true);
               if (iFngrMstr.StartEnrollEx(UserId_Txt.Text, Convert.ToInt32(FngrIndx_Txt.Text), 0))
               {
                  MessageBox.Show("لطفا 3 باراثر انگشت خود را روی سنسور قرار دهید");
               }
               else
               {
                  iFngrMstr.SSR_DelUserTmpExt(1, UserId_Txt.Text, 6);
                  iFngrMstr.DeleteUserInfoEx(1, Convert.ToInt32(UserId_Txt.Text));
                  MessageBox.Show("دوباره امتحان کنید");
               }
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SyncAllDevByFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انجام عملیات ارسال اثر انگشت موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            string tmpData = "";
            int tmplen = 0;
            int flag = 0;

            if (Network_Rb.Checked && iFgnrMstrIsCnct)
            {
               var result = iFngrMstr.GetUserTmpExStr(1, UserId_Txt.Text, 6, out flag, out tmpData, out tmplen);               
            }
            else if (Usb_Rb.Checked)
            {
               tmpData = textFngr.Text;
            }
            foreach (var dev in DevInfoBs.List.OfType<DeviceInfo>())
            {
               iFngrSlavIsCnct = iFngrSlav.Connect_Net(dev.IP, dev.Port);
               if(iFngrSlavIsCnct)
               {
                  dev.Status = "Connected";
                  dev.Oprt_Stat = "002";
                  var result = iFngrSlav.SSR_SetUserInfo(1, UserId_Txt.Text, UserName_Txt.Text, "", 0, true);
                  result = iFngrSlav.SetUserTmpExStr(1, UserId_Txt.Text, Convert.ToInt32(FngrIndx_Txt.Text), flag, tmpData);
                  //if(result)
                  //{
                  //   MessageBox.Show("Enroll Successfully  done");
                  //}
               }
               else
               {
                  dev.Status = "NotConnected!";
                  dev.Oprt_Stat = "001";
               }
            }
            MessageBox.Show("ارسال اثر انگشت با موفقیت انجام شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #region ZkFinger4500K
      private void bnInit_Click(object sender, EventArgs e)
      {
         cmbIdx.Items.Clear();
         int ret = zkfperrdef.ZKFP_ERR_OK;
         if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
         {
            int nCount = zkfp2.GetDeviceCount();
            if (nCount > 0)
            {
               for (int i = 0; i < nCount; i++)
               {
                  cmbIdx.Items.Add(i.ToString());
               }
               cmbIdx.SelectedIndex = 0;
               bnOpen_Click(null, null);
            }
            else
            {
               zkfp2.Terminate();
               MessageBox.Show("No device connected!");
            }
         }
         else
         {
            MessageBox.Show("Initialize fail, ret=" + ret + " !");
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
         if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(cmbIdx.SelectedIndex)))
         {
            MessageBox.Show("OpenDevice fail");
            return;
         }
         if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
         {
            MessageBox.Show("Init DB fail");
            zkfp2.CloseDevice(mDevHandle);
            mDevHandle = IntPtr.Zero;
            return;
         }         
         RegisterCount = 0;
         cbRegTmp = 0;
         iFid = 1;
         for (int i = 0; i < 3; i++)
         {
            RegTmps[i] = new byte[2048];
         }
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

         textRes.AppendText("reader parameter, image width:" + mfpWidth + ", height:" + mfpHeight + ", dpi:" + mfpDpi + "\n");

         Thread captureThread = new Thread(new ThreadStart(DoCapture));
         captureThread.IsBackground = true;
         captureThread.Start();
         bIsTimeToDie = false;
         textRes.AppendText("Open succ\n");

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
                  MemoryStream ms = new MemoryStream();
                  BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                  Bitmap bmp = new Bitmap(ms);
                  this.picFPImg.Image = bmp;


                  String strShow = zkfp2.BlobToBase64(CapTmp, cbCapTmp);
                  //textRes.AppendText("capture template data:" + strShow + "\n");
                  textFngr.Text = strShow;

                  if (IsRegister)
                  {
                     int ret = zkfp.ZKFP_ERR_OK;
                     int fid = 0, score = 0;
                     ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                     if (zkfp.ZKFP_ERR_OK == ret)
                     {
                        textRes.AppendText("This finger was already register by " + fid + "!\n");
                        return;
                     }

                     if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                     {
                        textRes.AppendText("Please press the same finger 3 times for the enrollment.\n");
                        return;
                     }

                     Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                     String strBase64 = zkfp2.BlobToBase64(CapTmp, cbCapTmp);
                     byte[] blob = zkfp2.Base64ToBlob(strBase64);
                     RegisterCount++;
                     if (RegisterCount >= REGISTER_FINGER_COUNT)
                     {
                        RegisterCount = 0;
                        if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)) &&
                               zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBAdd(mDBHandle, iFid, RegTmp)))
                        {
                           iFid++;
                           textRes.AppendText("enroll succ\n");
                        }
                        else
                        {
                           textRes.AppendText("enroll fail, error code=" + ret + "\n");
                        }
                        IsRegister = false;
                        return;
                     }
                     else
                     {
                        textRes.AppendText("You need to press the " + (REGISTER_FINGER_COUNT - RegisterCount) + " times fingerprint\n");
                     }
                  }
                  else
                  {
                     if (cbRegTmp <= 0)
                     {
                        textRes.AppendText("Please register your finger first!\n");
                        return;
                     }
                     if (bIdentify)
                     {
                        int ret = zkfp.ZKFP_ERR_OK;
                        int fid = 0, score = 0;
                        ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                        if (zkfp.ZKFP_ERR_OK == ret)
                        {
                           textRes.AppendText("Identify succ, fid= " + fid + ",score=" + score + "!\n");
                           return;
                        }
                        else
                        {
                           textRes.AppendText("Identify fail, ret= " + ret + "\n");
                           return;
                        }
                     }
                     else
                     {
                        int ret = zkfp2.DBMatch(mDBHandle, CapTmp, RegTmp);
                        if (0 < ret)
                        {
                           textRes.AppendText("Match finger succ, score=" + ret + "!\n");
                           return;
                        }
                        else
                        {
                           textRes.AppendText("Match finger fail, ret= " + ret + "\n");
                           return;
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

      private void OpenClosPort_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as Button;

            switch(butn.Tag.ToString())
            {
               case "close":
                  if(PortList_Cb.Text != "")
                  {
                     CardRedrDev_Sp.PortName = PortList_Cb.Text;
                     CardRedrDev_Sp.BaudRate = Convert.ToInt32(BandRate_Txt.Text);

                     CardRedrDev_Sp.Open();

                     if(CardRedrDev_Sp.IsOpen)
                     {
                        butn.Tag = "open";
                        butn.Text = "قطع ارتباط";
                     }
                  }
                  break;
               case "open":
                  if (CardRedrDev_Sp.IsOpen)
                  {
                     butn.Tag = "close";
                     butn.Text = "اتصال به دستگاه کارتخوان";
                     CardRedrDev_Sp.Close();
                  }
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion

      private void CardRedrDev_Sp_DataReceived(object sender, IO.Ports.SerialDataReceivedEventArgs e)
      {
         try
         {
            if (InvokeRequired)
               Invoke(new Action(() =>
               {
                  CardNumb_Txt.Text = CardRedrDev_Sp.ReadLine();

                  if (CardNumb_Txt.Text.IndexOf('\r') != -1)
                     CardNumb_Txt.Text = CardNumb_Txt.Text.Substring(0, CardNumb_Txt.Text.IndexOf('\r'));
               }));
            else
            {
               CardNumb_Txt.Text = CardRedrDev_Sp.ReadLine();
               
               if (CardNumb_Txt.Text.IndexOf('\r') != -1)
                  CardNumb_Txt.Text = CardNumb_Txt.Text.Substring(0, CardNumb_Txt.Text.IndexOf('\r'));
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SyncAllDevByCardNumb_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انجام عملیات ارسال شماره کارت موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (var dev in DevInfoBs.List.OfType<DeviceInfo>())
            {
               iFngrSlavIsCnct = iFngrSlav.Connect_Net(dev.IP, dev.Port);
               if (iFngrSlavIsCnct)
               {
                  dev.Status = "Connected";                  
                  int idwErrorCode = 0;

                  bool bEnabled = true;
                  int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                  Cursor = Cursors.WaitCursor;
                  iFngrSlav.EnableDevice(iMachineNumber, false);

                  iFngrSlav.SetStrCardNumber(CardNumb_Txt.Text);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
                  if (iFngrSlav.SSR_SetUserInfo(iMachineNumber, UserId_Txt.Text, UserName_Txt.Text, "", 0, bEnabled))//upload the user's information(card number included)
                  {
                     dev.Oprt_Stat = "002";
                  }
                  else
                  {
                     dev.Oprt_Stat = "001";
                     iFngrSlav.GetLastError(ref idwErrorCode);
                     MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                  }
                  iFngrSlav.RefreshData(iMachineNumber);//the data in the device should be refreshed
                  iFngrSlav.EnableDevice(iMachineNumber, true);
                  Cursor = Cursors.Default;
               }
               else
               {
                  dev.Status = "NotConnected!";
                  dev.Oprt_Stat = "001";
               }
            }
            MessageBox.Show("ارسال شماره کارت به دستگاه با موفقیت انجام شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SyncAllDevByClerFngrPrnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انجام عملیات حذف اثر انگشت موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            foreach (var dev in DevInfoBs.List.OfType<DeviceInfo>())
            {
               iFngrSlavIsCnct = iFngrSlav.Connect_Net(dev.IP, dev.Port);
               if (iFngrSlavIsCnct)
               {
                  dev.Status = "Connected";
                  
                  Cursor = Cursors.WaitCursor;
                  iFngrSlav.SSR_DelUserTmpExt(1, UserId_Txt.Text, Convert.ToInt32(FngrIndx_Txt.Text));
                  iFngrSlav.DeleteUserInfoEx(1, Convert.ToInt32(UserId_Txt.Text));
                  iFngrSlav.ClearSLog(1);

                  dev.Oprt_Stat = "002";
                  Cursor = Cursors.Default;
               }
               else
               {
                  dev.Status = "NotConnected!";
                  dev.Oprt_Stat = "001";
               }
            }
            MessageBox.Show("حذف اثر انگشت از دستگاه با موفقیت انجام شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SyncAllDevByDelEnrlData_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با انجام عملیات حذف کامل کاربر موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            foreach (var dev in DevInfoBs.List.OfType<DeviceInfo>())
            {
               iFngrSlavIsCnct = iFngrSlav.Connect_Net(dev.IP, dev.Port);
               if (iFngrSlavIsCnct)
               {
                  dev.Status = "Connected";

                  Cursor = Cursors.WaitCursor;
                  iFngrSlav.SSR_DelUserTmpExt(1, UserId_Txt.Text, Convert.ToInt32(FngrIndx_Txt.Text));
                  iFngrSlav.SSR_DeleteEnrollDataExt(1, UserId_Txt.Text, 1);
                  iFngrSlav.ClearSLog(1);

                  dev.Oprt_Stat = "002";
                  Cursor = Cursors.Default;
               }
               else
               {
                  dev.Status = "NotConnected!";
                  dev.Oprt_Stat = "001";
               }
            }
            MessageBox.Show("حذف کامل اطلاعات کاربر از دستگاه با موفقیت انجام شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GetGeneralLogData_btn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!AutoOprt009_cb.Checked && MessageBox.Show(this, "آیا با انجام عملیات دریافت رکورد موافق هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            foreach (var dev in DevInfoBs.List.OfType<DeviceInfo>())
            {
               iFngrSlavIsCnct = iFngrSlav.Connect_Net(dev.IP, dev.Port);
               LogResult_Txt.Text = "";
               if (iFngrSlavIsCnct)
               {
                  dev.Status = "Connected";
                  int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                  iFngrSlav.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)

                  Cursor = Cursors.WaitCursor;
                  string sdwEnrollNumber = "";
                  int idwTMachineNumber = 0;
                  int idwEMachineNumber = 0;
                  int idwVerifyMode = 0;
                  int idwInOutMode = 0;
                  int idwYear = 0;
                  int idwMonth = 0;
                  int idwDay = 0;
                  int idwHour = 0;
                  int idwMinute = 0;
                  int idwSecond = 0;
                  int idwWorkcode = 0;

                  int idwErrorCode = 0;
                  int iGLCount = 0;
                  int iIndex = 0;
                  LogRecordsCount_Txt.Text = iIndex.ToString();
                  LogResult_Txt.Text = dev.IP + " Processing " + DateTime.Now.ToString() + Environment.NewLine;
                  string sqlInsertInto = "INSERT INTO DataFile([STATUS], [DATE_] ,[TIME_] ,[EMP_NO] ,[Clock_No], [Modify]) VALUES";
                  string sqlInsertValues = "";
                  iFngrSlav.EnableDevice(iMachineNumber, false);//disable the device
                  if (iFngrSlav.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
                  {
                     
                     while (iFngrSlav.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                                out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                     {
                        iGLCount++;
                        //LogResult_Txt.Text += sdwEnrollNumber + " * " + idwVerifyMode.ToString() + " * " + idwInOutMode.ToString() + " * " + idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString() + " * " + idwWorkcode.ToString() + Environment.NewLine;
                        //lvLogs.Items.Add(iGLCount.ToString());
                        //lvLogs.Items[iIndex].SubItems.Add(sdwEnrollNumber);//modify by Darcy on Nov.26 2009
                        //lvLogs.Items[iIndex].SubItems.Add(idwVerifyMode.ToString());
                        //lvLogs.Items[iIndex].SubItems.Add(idwInOutMode.ToString());
                        //lvLogs.Items[iIndex].SubItems.Add(idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                        //lvLogs.Items[iIndex].SubItems.Add(idwWorkcode.ToString());
                        if (sqlInsertValues.Length > 0) sqlInsertValues += ",";
                        sqlInsertValues += 
                           string.Format("({0}, CAST(CONVERT(DATETIME, '{1}') as FLOAT), {2}, {3}, {4}, 0)", 
                              idwWorkcode,
                              idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString(),
                              idwHour.ToString() + idwMinute.ToString(),
                              sdwEnrollNumber,
                              dev.Id
                           );
                        iIndex++;
                     }
                     
                     // Execute Sql Insert On Server
                     if (iIndex > 0 && ServerDataBase_Cmb.Text != "")
                     {
                        LogResult_Txt.Text += sqlInsertInto + sqlInsertValues + Environment.NewLine;
                        SqlCommand sqlcmd =
                           new SqlCommand(sqlInsertInto + sqlInsertValues,
                               new SqlConnection(string.Format("server={0};database={1};user={2};password={3}", SeverIPAddress_Txt.Text, ServerDataBase_Cmb.Text, ServerUserId_Txt.Text, ServerPassword_Txt.Text))
                           );
                        sqlcmd.Connection.Open();
                        int result = sqlcmd.ExecuteNonQuery();
                        sqlcmd.Connection.Close();

                        if (ClearSLogRecords_cb.Checked)
                           iFngrSlav.ClearGLog(iMachineNumber);
                     }
                     else
                        ServerDataBase_Cmb.Focus();
                  }
                  else
                  {
                     Cursor = Cursors.Default;
                     iFngrSlav.GetLastError(ref idwErrorCode);

                     if (idwErrorCode != 0)
                     {
                        LogResult_Txt.Text += "Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString() + Environment.NewLine;
                     }
                     else
                     {
                        LogResult_Txt.Text += "No data from terminal returns!" + Environment.NewLine;
                     }
                  }
                  iFngrSlav.EnableDevice(iMachineNumber, true);//enable the device

                  if (ShowLogRecordCount_cb.Checked)
                     LogRecordsCount_Txt.Text = iIndex.ToString();

                  dev.Oprt_Stat = "002";
                  Cursor = Cursors.Default;
               }
               else
               {
                  dev.Status = "NotConnected!";
                  dev.Oprt_Stat = "001";
               }
            }
            if(!AutoOprt009_cb.Checked)
               MessageBox.Show("عملیات دریافت رکورد با موفقیت انجام شد");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {

         }
      }

      private void AutoOprt009_cb_CheckedChanged(object sender, EventArgs e)
      {
         AutoOprt009_Tmr.Enabled = AutoOprt009_cb.Checked;

         AutoOprt009_Tmr.Interval = (int)(Convert.ToDouble(AutoOprt009Intrval_Txt.Text) * 60 * 1000);
      }
   }
}
