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
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using libzkfpcsharp;
using System.Threading;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsAccountFinger : UserControl
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

      public SettingsAccountFinger()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, ImageAccount_Pb.Width, ImageAccount_Pb.Height);
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         ImageAccount_Pb.Region = rg;
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
         UserBs.DataSource = iProject.Users.Where(u => u.USERDB.ToUpper() == CurrentUser.ToUpper());
      }

      private void UserBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void UserBs_CurrentChanged(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         if (user.USER_IMAG == null)
         {
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            Image img = global::System.DataGuard.Properties.Resources.IMAGE_1429;
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            user.USER_IMAG = bytes;
            ImageAccount_Pb.Image = global::System.DataGuard.Properties.Resources.IMAGE_1429;
         }
         else
         {
            var stream = new MemoryStream(user.USER_IMAG.ToArray());
            ImageAccount_Pb.Image = Image.FromStream(stream);
         }

         if(UsrfBs.List.Count > 0)
         {
            foreach (var fngr in UsrfBs.List.OfType<Data.User_Finger>())
            {
               if(fngr.IMAG != null)
               {
                  var uf = tp_001.Controls.OfType<MaxUi.RoundedButton>().FirstOrDefault(f => f.Caption == fngr.FNGR_INDX.ToString());
                  uf.NormalColorA = uf.NormalColorB = Color.YellowGreen;
               }
               else
               {
                  var uf = tp_001.Controls.OfType<MaxUi.RoundedButton>().FirstOrDefault(f => f.Caption == fngr.FNGR_INDX.ToString());
                  uf.NormalColorA = uf.NormalColorB = Color.White;
               }
            }
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            UsrfBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch { }
         finally
         {
            Execute_Query();
         }
      }

      private void Ts_FingerDeviceStat_Toggled(object sender, EventArgs e)
      {
         try
         {
            if (Ts_FingerDeviceStat.IsOn)
            {               
               bnInit_Click(null, null);
               FngrDevStat_Pb.Image = Properties.Resources.IMAGE_1671;
               FngrDevStat_Pn.Visible = true;
            }
            else
            {
               bnClose_Click(null, null);
               FngrDevStat_Pb.Image = Properties.Resources.IMAGE_1674;
               FngrDevStat_Pn.Visible = false;
               StrLen_Lbl.Text = "0";
            }
         }
         catch { FngrDevStat_Pb.Image = Properties.Resources.IMAGE_1674; Ts_FingerDeviceStat.IsOn = false; FngrDevStat_Pn.Visible = false; }
      }

      private void Ts_IdentifyFingerDeviceStat_Toggled(object sender, EventArgs e)
      {

      }

      private void FngrIndxi_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var fngrindxb = sender as MaxUi.RoundedButton;
            var fngrindx = UsrfBs.List.OfType<Data.User_Finger>().FirstOrDefault(f => f.FNGR_INDX.ToString() == fngrindxb.Caption);

            if(fngrindx != null)
            {
               if(fngrindx.IMAG != null)
               {
                  var ms = new MemoryStream(fngrindx.IMAG.ToArray());
                  //BitmapFormat.GetBitmap(fngrindx.IMAG.ToArray(), mfpWidth, mfpHeight, ref ms);
                  Bitmap bmp = new Bitmap(ms);
                  this.picFPImg.Image = bmp;                  
               }
               else
               {
                  picFPImg.Image = System.DataGuard.Properties.Resources.IMAGE_1675;
               }

               textFngr.Text = fngrindx.FNGR_TMPL;
               StrLen_Lbl.Text = textFngr.TextLength.ToString();
               UsrfBs.Position = UsrfBs.IndexOf(fngrindx);
            }
            else
            {
               if (UsrfBs.List.OfType<Data.User_Finger>().Any(uf => uf.CODE == 0)) return;

               var newobj = UsrfBs.AddNew() as Data.User_Finger;
               newobj.FNGR_INDX = Convert.ToInt16(fngrindxb.Caption);
               iProject.User_Fingers.InsertOnSubmit(newobj);

               UsrfBs.Position = UsrfBs.IndexOf(newobj);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      #region ZkFinger4500K
      private void bnInit_Click(object sender, EventArgs e)
      {
         //cmbIdx.Items.Clear();
         int ret = zkfperrdef.ZKFP_ERR_OK;
         if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
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
            }
         }
         else
         {
            //MessageBox.Show("Initialize fail, ret=" + ret + " !");
            throw new Exception("Error");
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
                  if (Ts_FingerDeviceStat.IsOn && !Ts_IdentifyFingerDeviceStat.IsOn)
                  {
                     MemoryStream ms = new MemoryStream();
                     BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                     Bitmap bmp = new Bitmap(ms);
                     this.picFPImg.Image = bmp;

                     String strShow = zkfp2.BlobToBase64(CapTmp, cbCapTmp);
                     //textRes.AppendText("capture template data:" + strShow + "\n");
                     textFngr.Text = strShow;
                     StrLen_Lbl.Text = strShow.Length.ToString();

                     var fngrindx = UsrfBs.Current as Data.User_Finger;
                     byte[] bytes = null;
                     bmp.Save(ms, ImageFormat.Bmp);
                     bytes = ms.ToArray();
                     //fngrindx.IMAG = FPBuffer;//bytes;
                     fngrindx.IMAG = bytes;
                     fngrindx.FNGR_TMPL = strShow;

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
                           //textRes.AppendText("Please register your finger first!\n");
                           textRes.AppendText("Read your finger successfully!\n");
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
                  else
                  {
                     String strShow = zkfp2.BlobToBase64(CapTmp, cbCapTmp);
                     textFngrTemp.Text = strShow;

                     textRes.Text = "";
                     
                     byte[] blob2 = Convert.FromBase64String(textFngrTemp.Text.Trim());

                     IdentifyFingerIndex_Lb.Text = "";
                     foreach (var fngr in UsrfBs.List.OfType<Data.User_Finger>().Where(f => f.FNGR_TMPL != null))
                     {
                        byte[] blob1 = Convert.FromBase64String(fngr.FNGR_TMPL.Trim());
                        int ret = zkfp2.DBMatch(mDBHandle, blob1, blob2);
                        textRes.AppendText("Fngr id : ( " + fngr.FNGR_INDX.ToString() + " ), Match score=" + ret + "!\n");
                        if(ret >= 80 && ret <= 100)
                           IdentifyFingerIndex_Lb.Text = fngr.FNGR_INDX.ToString();
                     }

                     if(IdentifyFingerIndex_Lb.Text == "")
                     {
                        IdentifyFingerIndex_Lb.Text = "شناسایی انجام نشد";
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
   }
}
