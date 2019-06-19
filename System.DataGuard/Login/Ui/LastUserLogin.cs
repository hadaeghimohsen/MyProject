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
using System.IO;
using System.Drawing.Imaging;
using System.MaxUi;
using System.Runtime.InteropServices;
using libzkfpcsharp;
using System.Threading;
using System.Xml.Linq;

namespace System.DataGuard.Login.Ui
{
   public partial class LastUserLogin : UserControl
   {
      public LastUserLogin()
      {
         InitializeComponent();
      }

      private void SwitchUser_RondButn_Click(object sender, EventArgs e)
      {
         //try { bnClose_Click(null, null); }
         //catch { }

         _DefaultGateway.Gateway(
             new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4LastUserLogin */, SendType.SelfToUserInterface) { Input = Keys.Escape }
          );

         Job _SwitchUser =
            new Job(SendType.External, "Program", "DataGuard:Login", 02 /* Execute DoWork4Login */, SendType.Self);
         _DefaultGateway.Gateway(_SwitchUser);
      }

      private void User_RondButn_Click(object sender, EventArgs e)
      {
         // Get User Info
         RoundedButton rb = (RoundedButton)sender;
         if (rb.Tag != null)
         {
            Job _SelectedUser =
               new Job(SendType.External, "Localhost",
                 new List<Job>
                   {
                      new Job(SendType.Self, 06 /* Execute DoWork4SelectedLastUserLogin */){Input = rb.Tag}
                   });
            _DefaultGateway.Gateway(_SelectedUser);
         }
         else
         {
            // Guest User
         }
      }

      private Image GetUserImage(Data.User user)
      {
         if (user == null)
         {
            return global::System.DataGuard.Properties.Resources.IMAGE_1482;
         }
         else if (user.USER_IMAG == null)
         {
            return global::System.DataGuard.Properties.Resources.IMAGE_1429;
         }
         else
         {
            var stream = new MemoryStream(user.USER_IMAG.ToArray());
            return Image.FromStream(stream);
         }
      }

      private int SetUserList(IQueryable<Data.User> users, int i)
      {
         switch (users.Count())
         {
            case 1:
               User2_RondButn.Visible = User3_RondButn.Visible = User2_Txt.Visible = User3_Txt.Visible = true;
               User4_RondButn.Visible = User1_RondButn.Visible = User4_Txt.Visible = User1_Txt.Visible = User5_RondButn.Visible = User5_Txt.Visible = false;

               User2_RondButn.ImageProfile = GetUserImage(users.First());
               User2_Txt.Text = users.First().USERDB;
               User2_RondButn.Tag = users.First();
               User2_Txt.Tag = "realuser";

               User3_RondButn.ImageProfile = GetUserImage(null);
               User3_Txt.Text = "Guest User";
               User3_RondButn.Tag = null;
               User3_Txt.Tag = "guestuser";

               break;
            case 2:
               User1_RondButn.Visible = User2_RondButn.Visible = User3_RondButn.Visible = User1_Txt.Visible = User2_Txt.Visible = User3_Txt.Visible = true;
               User4_RondButn.Visible = User4_Txt.Visible = User5_RondButn.Visible = User5_Txt.Visible = false;

               foreach (Data.User user in users)
               {
                  if (i == 0)
                  {
                     User2_RondButn.ImageProfile = GetUserImage(user);
                     User2_Txt.Text = user.USERDB;
                     User2_RondButn.Tag = user;
                     User2_Txt.Tag = "realuser";
                     ++i;
                  }
                  else if (i == 1)
                  {
                     User1_RondButn.ImageProfile = GetUserImage(user);
                     User1_Txt.Text = user.USERDB;
                     User1_RondButn.Tag = user;
                     User1_Txt.Tag = "realuser";
                     ++i;
                  }
               }
               User3_RondButn.ImageProfile = GetUserImage(null);
               User3_Txt.Text = "Guest User";
               User3_RondButn.Tag = null;
               User3_Txt.Tag = "guestuser";
               break;
            case 3:
               User5_RondButn.Visible = User2_RondButn.Visible = User3_RondButn.Visible = User4_RondButn.Visible = User2_Txt.Visible = User3_Txt.Visible = User4_Txt.Visible = User5_Txt.Visible = true;
               User1_RondButn.Visible = User1_Txt.Visible = false;

               foreach (Data.User user in users)
               {
                  if (i == 0)
                  {
                     User4_RondButn.ImageProfile = GetUserImage(user);
                     User4_Txt.Text = user.USERDB;
                     User4_RondButn.Tag = user;
                     User4_Txt.Tag = "realuser";
                     ++i;
                  }
                  else if (i == 1)
                  {
                     User2_RondButn.ImageProfile = GetUserImage(user);
                     User2_Txt.Text = user.USERDB;
                     User2_RondButn.Tag = user;
                     User2_Txt.Tag = "realuser";
                     ++i;
                  }
                  else if (i == 2)
                  {
                     User3_RondButn.ImageProfile = GetUserImage(user);
                     User3_Txt.Text = user.USERDB;
                     User3_RondButn.Tag = user;
                     User3_Txt.Tag = "realuser";
                     ++i;
                  }
               }
               User5_RondButn.ImageProfile = GetUserImage(null);
               User5_Txt.Text = "Guest User";
               User5_RondButn.Tag = null;
               User5_Txt.Tag = "guestuser";

               break;
            case 4:
               User1_RondButn.Visible = User2_RondButn.Visible = User3_RondButn.Visible = User4_RondButn.Visible = User5_RondButn.Visible = User1_Txt.Visible = User2_Txt.Visible = User3_Txt.Visible = User4_Txt.Visible = User5_Txt.Visible = true;
               foreach (Data.User user in users)
               {
                  if (i == 0)
                  {
                     User4_RondButn.ImageProfile = GetUserImage(user);
                     User4_Txt.Text = user.USERDB;
                     User4_RondButn.Tag = user;
                     User4_Txt.Tag = "realuser";
                     ++i;
                  }
                  else if (i == 1)
                  {
                     User2_RondButn.ImageProfile = GetUserImage(user);
                     User2_Txt.Text = user.USERDB;
                     User2_RondButn.Tag = user;
                     User2_Txt.Tag = "realuser";
                     ++i;
                  }
                  else if (i == 2)
                  {
                     User3_RondButn.ImageProfile = GetUserImage(user);
                     User3_Txt.Text = user.USERDB;
                     User3_RondButn.Tag = user;
                     User3_Txt.Tag = "realuser";
                     ++i;
                  }
                  else if (i == 3)
                  {
                     User1_RondButn.ImageProfile = GetUserImage(user);
                     User1_Txt.Text = user.USERDB;
                     User1_RondButn.Tag = user;
                     User1_Txt.Tag = "realuser";
                     ++i;
                  }
               }
               User5_RondButn.ImageProfile = GetUserImage(null);
               User5_Txt.Text = "Guest User";
               User5_RondButn.Tag = null;
               User5_Txt.Tag = "guestuser";
               break;
            default:
               break;
         }
         return i;
      }

      //#region FingerPrint
      //IntPtr mDevHandle = IntPtr.Zero;
      //IntPtr mDBHandle = IntPtr.Zero;
      //IntPtr FormHandle = IntPtr.Zero;
      //bool bIsTimeToDie = false;
      //bool IsRegister = false;
      //bool bIdentify = true;
      //byte[] FPBuffer;
      //int RegisterCount = 0;
      //const int REGISTER_FINGER_COUNT = 3;

      //byte[][] RegTmps = new byte[3][];
      //byte[] RegTmp = new byte[2048];
      //byte[] CapTmp = new byte[2048];

      //int cbCapTmp = 2048;
      //int cbRegTmp = 0;
      //int iFid = 1;

      //private int mfpWidth = 0;
      //private int mfpHeight = 0;
      //private int mfpDpi = 0;

      //const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

      //[DllImport("user32.dll", EntryPoint = "SendMessageA")]
      //public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

      //#region ZkFinger4500K
      //private void bnInit_Click(object sender, EventArgs e)
      //{
      //   //cmbIdx.Items.Clear();
      //   int ret = zkfperrdef.ZKFP_ERR_OK;
      //   if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
      //   {
      //      int nCount = zkfp2.GetDeviceCount();
      //      if (nCount > 0)
      //      {
      //         //for (int i = 0; i < nCount; i++)
      //         //{
      //         //   cmbIdx.Items.Add(i.ToString());
      //         //}
      //         //cmbIdx.SelectedIndex = 0;
      //         bnOpen_Click(null, null);
      //      }
      //      else
      //      {
      //         zkfp2.Terminate();
      //         MessageBox.Show("No device connected!");
      //      }
      //   }
      //   else
      //   {
      //      //MessageBox.Show("Initialize fail, ret=" + ret + " !");
      //      throw new Exception("Error");
      //   }
      //}

      //private void bnFree_Click(object sender, EventArgs e)
      //{
      //   zkfp2.Terminate();
      //   cbRegTmp = 0;
      //}

      //private void bnOpen_Click(object sender, EventArgs e)
      //{
      //   int ret = zkfp.ZKFP_ERR_OK;
      //   if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(0)))
      //   {
      //      MessageBox.Show("OpenDevice fail");
      //      return;
      //   }
      //   if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
      //   {
      //      MessageBox.Show("Init DB fail");
      //      zkfp2.CloseDevice(mDevHandle);
      //      mDevHandle = IntPtr.Zero;
      //      return;
      //   }
      //   RegisterCount = 0;
      //   cbRegTmp = 0;
      //   iFid = 1;
      //   for (int i = 0; i < 3; i++)
      //   {
      //      RegTmps[i] = new byte[2048];
      //   }
      //   byte[] paramValue = new byte[4];
      //   int size = 4;
      //   zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
      //   zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

      //   size = 4;
      //   zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
      //   zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

      //   FPBuffer = new byte[mfpWidth * mfpHeight];

      //   size = 4;
      //   zkfp2.GetParameters(mDevHandle, 3, paramValue, ref size);
      //   zkfp2.ByteArray2Int(paramValue, ref mfpDpi);

      //   //textRes.AppendText("reader parameter, image width:" + mfpWidth + ", height:" + mfpHeight + ", dpi:" + mfpDpi + "\n");
      //   FngrDev_Pb.Visible = true;

      //   Thread captureThread = new Thread(new ThreadStart(DoCapture));
      //   captureThread.IsBackground = true;
      //   captureThread.Start();
      //   bIsTimeToDie = false;
      //   //textRes.AppendText("Open succ\n");
      //}

      //private void bnClose_Click(object sender, EventArgs e)
      //{
      //   bIsTimeToDie = true;
      //   RegisterCount = 0;
      //   //Thread.Sleep(1000);
      //   zkfp2.CloseDevice(mDevHandle);
      //   bnFree_Click(null, null);
      //}

      //private void DoCapture()
      //{
      //   while (!bIsTimeToDie)
      //   {
      //      cbCapTmp = 2048;
      //      int ret = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);
      //      if (ret == zkfp.ZKFP_ERR_OK)
      //      {
      //         SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
      //      }
      //      Thread.Sleep(200);
      //   }
      //}

      //protected override void DefWndProc(ref Message m)
      //{
      //   switch (m.Msg)
      //   {
      //      case MESSAGE_CAPTURED_OK:
      //         {
      //            String strShow = zkfp2.BlobToBase64(CapTmp, cbCapTmp);

      //            byte[] blob2 = Convert.FromBase64String(strShow.Trim());

      //            foreach (var fngr in iProject.User_Fingers.Where(f => f.FNGR_TMPL != null && !f.User.IsLock))
      //            {
      //               byte[] blob1 = Convert.FromBase64String(fngr.FNGR_TMPL.Trim());
      //               int ret = zkfp2.DBMatch(mDBHandle, blob1, blob2);
      //               //textRes.AppendText("Fngr id : ( " + fngr.FNGR_INDX.ToString() + " ), Match score=" + ret + "!\n");
      //               if (ret >= 80 && ret <= 100)
      //               {
      //                  bnClose_Click(null, null);
      //                  _DefaultGateway.Gateway(
      //                     new Job(SendType.External, "localhost",
      //                        new List<Job>
      //                        {
      //                           new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */) { Input = Keys.Escape },
      //                           new Job(SendType.Self, 02 /*Execute DoWork4Login */),
      //                           new Job(SendType.SelfToUserInterface, "Login", 10 /* Execute ActionCallForm */) { Input = new XElement("Login", new XAttribute("type", "autologin"), new XAttribute("usr", fngr.User.TitleEn), new XAttribute("pwd", fngr.User.Password))}
      //                        }
      //                     )
      //                  );                        
      //                  break;
      //               }
      //            }
      //         }
      //         break;

      //      default:
      //         base.DefWndProc(ref m);
      //         break;
      //   }
      //}

      //public class BitmapFormat
      //{
      //   public struct BITMAPFILEHEADER
      //   {
      //      public ushort bfType;
      //      public int bfSize;
      //      public ushort bfReserved1;
      //      public ushort bfReserved2;
      //      public int bfOffBits;
      //   }

      //   public struct MASK
      //   {
      //      public byte redmask;
      //      public byte greenmask;
      //      public byte bluemask;
      //      public byte rgbReserved;
      //   }

      //   public struct BITMAPINFOHEADER
      //   {
      //      public int biSize;
      //      public int biWidth;
      //      public int biHeight;
      //      public ushort biPlanes;
      //      public ushort biBitCount;
      //      public int biCompression;
      //      public int biSizeImage;
      //      public int biXPelsPerMeter;
      //      public int biYPelsPerMeter;
      //      public int biClrUsed;
      //      public int biClrImportant;
      //   }

      //   /*******************************************
      //   * ؛¯ت‎أû³ئ£؛RotatePic       
      //   * ؛¯ت‎¹¦ؤـ£؛ذ‎×ھح¼ئ¬£¬ؤ؟µؤتا±£´و؛حدشت¾µؤح¼ئ¬سë°´µؤض¸خئ·½دٍ²»ح¬     
      //   * ؛¯ت‎بë²خ£؛BmpBuf---ذ‎×ھا°µؤض¸خئ×ض·û´®
      //   * ؛¯ت‎³ِ²خ£؛ResBuf---ذ‎×ھ؛َµؤض¸خئ×ض·û´®
      //   * ؛¯ت‎·µ»ط£؛خق
      //   *********************************************/
      //   public static void RotatePic(byte[] BmpBuf, int width, int height, ref byte[] ResBuf)
      //   {
      //      int RowLoop = 0;
      //      int ColLoop = 0;
      //      int BmpBuflen = width * height;

      //      try
      //      {
      //         for (RowLoop = 0; RowLoop < BmpBuflen; )
      //         {
      //            for (ColLoop = 0; ColLoop < width; ColLoop++)
      //            {
      //               ResBuf[RowLoop + ColLoop] = BmpBuf[BmpBuflen - RowLoop - width + ColLoop];
      //            }

      //            RowLoop = RowLoop + width;
      //         }
      //      }
      //      catch (Exception ex)
      //      {
      //         //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
      //         //logger.Append();
      //      }
      //   }

      //   /*******************************************
      //   * ؛¯ت‎أû³ئ£؛StructToBytes       
      //   * ؛¯ت‎¹¦ؤـ£؛½«½ل¹¹جه×ھ»¯³ةخق·û؛إ×ض·û´®ت‎×é     
      //   * ؛¯ت‎بë²خ£؛StructObj---±»×ھ»¯µؤ½ل¹¹جه
      //   *           Size---±»×ھ»¯µؤ½ل¹¹جهµؤ´َذ،
      //   * ؛¯ت‎³ِ²خ£؛خق
      //   * ؛¯ت‎·µ»ط£؛½ل¹¹جه×ھ»¯؛َµؤت‎×é
      //   *********************************************/
      //   public static byte[] StructToBytes(object StructObj, int Size)
      //   {
      //      int StructSize = Marshal.SizeOf(StructObj);
      //      byte[] GetBytes = new byte[StructSize];

      //      try
      //      {
      //         IntPtr StructPtr = Marshal.AllocHGlobal(StructSize);
      //         Marshal.StructureToPtr(StructObj, StructPtr, false);
      //         Marshal.Copy(StructPtr, GetBytes, 0, StructSize);
      //         Marshal.FreeHGlobal(StructPtr);

      //         if (Size == 14)
      //         {
      //            byte[] NewBytes = new byte[Size];
      //            int Count = 0;
      //            int Loop = 0;

      //            for (Loop = 0; Loop < StructSize; Loop++)
      //            {
      //               if (Loop != 2 && Loop != 3)
      //               {
      //                  NewBytes[Count] = GetBytes[Loop];
      //                  Count++;
      //               }
      //            }

      //            return NewBytes;
      //         }
      //         else
      //         {
      //            return GetBytes;
      //         }
      //      }
      //      catch (Exception ex)
      //      {
      //         //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
      //         //logger.Append();

      //         return GetBytes;
      //      }
      //   }

      //   /*******************************************
      //   * ؛¯ت‎أû³ئ£؛GetBitmap       
      //   * ؛¯ت‎¹¦ؤـ£؛½«´«½ّہ´µؤت‎¾ف±£´وخھح¼ئ¬     
      //   * ؛¯ت‎بë²خ£؛buffer---ح¼ئ¬ت‎¾ف
      //   *           nWidth---ح¼ئ¬µؤ؟ي¶ب
      //   *           nHeight---ح¼ئ¬µؤ¸ك¶ب
      //   * ؛¯ت‎³ِ²خ£؛خق
      //   * ؛¯ت‎·µ»ط£؛خق
      //   *********************************************/
      //   public static void GetBitmap(byte[] buffer, int nWidth, int nHeight, ref MemoryStream ms)
      //   {
      //      int ColorIndex = 0;
      //      ushort m_nBitCount = 8;
      //      int m_nColorTableEntries = 256;
      //      byte[] ResBuf = new byte[nWidth * nHeight * 2];

      //      try
      //      {
      //         BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
      //         BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
      //         MASK[] ColorMask = new MASK[m_nColorTableEntries];

      //         int w = (((nWidth + 3) / 4) * 4);

      //         //ح¼ئ¬ح·ذإد¢
      //         BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
      //         BmpInfoHeader.biWidth = nWidth;
      //         BmpInfoHeader.biHeight = nHeight;
      //         BmpInfoHeader.biPlanes = 1;
      //         BmpInfoHeader.biBitCount = m_nBitCount;
      //         BmpInfoHeader.biCompression = 0;
      //         BmpInfoHeader.biSizeImage = 0;
      //         BmpInfoHeader.biXPelsPerMeter = 0;
      //         BmpInfoHeader.biYPelsPerMeter = 0;
      //         BmpInfoHeader.biClrUsed = m_nColorTableEntries;
      //         BmpInfoHeader.biClrImportant = m_nColorTableEntries;

      //         //خؤ¼‏ح·ذإد¢
      //         BmpHeader.bfType = 0x4D42;
      //         BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
      //         BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
      //         BmpHeader.bfReserved1 = 0;
      //         BmpHeader.bfReserved2 = 0;

      //         ms.Write(StructToBytes(BmpHeader, 14), 0, 14);
      //         ms.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)), 0, Marshal.SizeOf(BmpInfoHeader));

      //         //µ÷تش°هذإد¢
      //         for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
      //         {
      //            ColorMask[ColorIndex].redmask = (byte)ColorIndex;
      //            ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
      //            ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
      //            ColorMask[ColorIndex].rgbReserved = 0;

      //            ms.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])), 0, Marshal.SizeOf(ColorMask[ColorIndex]));
      //         }

      //         //ح¼ئ¬ذ‎×ھ£¬½â¾ِض¸خئح¼ئ¬µ¹ء¢µؤختجâ
      //         RotatePic(buffer, nWidth, nHeight, ref ResBuf);

      //         byte[] filter = null;
      //         if (w - nWidth > 0)
      //         {
      //            filter = new byte[w - nWidth];
      //         }
      //         for (int i = 0; i < nHeight; i++)
      //         {
      //            ms.Write(ResBuf, i * nWidth, nWidth);
      //            if (w - nWidth > 0)
      //            {
      //               ms.Write(ResBuf, 0, w - nWidth);
      //            }
      //         }
      //      }
      //      catch (Exception ex)
      //      {
      //         // ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
      //         // logger.Append();
      //      }
      //   }

      //   /*******************************************
      //   * ؛¯ت‎أû³ئ£؛WriteBitmap       
      //   * ؛¯ت‎¹¦ؤـ£؛½«´«½ّہ´µؤت‎¾ف±£´وخھح¼ئ¬     
      //   * ؛¯ت‎بë²خ£؛buffer---ح¼ئ¬ت‎¾ف
      //   *           nWidth---ح¼ئ¬µؤ؟ي¶ب
      //   *           nHeight---ح¼ئ¬µؤ¸ك¶ب
      //   * ؛¯ت‎³ِ²خ£؛خق
      //   * ؛¯ت‎·µ»ط£؛خق
      //   *********************************************/
      //   public static void WriteBitmap(byte[] buffer, int nWidth, int nHeight)
      //   {
      //      int ColorIndex = 0;
      //      ushort m_nBitCount = 8;
      //      int m_nColorTableEntries = 256;
      //      byte[] ResBuf = new byte[nWidth * nHeight];

      //      try
      //      {

      //         BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
      //         BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
      //         MASK[] ColorMask = new MASK[m_nColorTableEntries];
      //         int w = (((nWidth + 3) / 4) * 4);
      //         //ح¼ئ¬ح·ذإد¢
      //         BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
      //         BmpInfoHeader.biWidth = nWidth;
      //         BmpInfoHeader.biHeight = nHeight;
      //         BmpInfoHeader.biPlanes = 1;
      //         BmpInfoHeader.biBitCount = m_nBitCount;
      //         BmpInfoHeader.biCompression = 0;
      //         BmpInfoHeader.biSizeImage = 0;
      //         BmpInfoHeader.biXPelsPerMeter = 0;
      //         BmpInfoHeader.biYPelsPerMeter = 0;
      //         BmpInfoHeader.biClrUsed = m_nColorTableEntries;
      //         BmpInfoHeader.biClrImportant = m_nColorTableEntries;

      //         //خؤ¼‏ح·ذإد¢
      //         BmpHeader.bfType = 0x4D42;
      //         BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
      //         BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
      //         BmpHeader.bfReserved1 = 0;
      //         BmpHeader.bfReserved2 = 0;

      //         Stream FileStream = File.Open("finger.bmp", FileMode.Create, FileAccess.Write);
      //         BinaryWriter TmpBinaryWriter = new BinaryWriter(FileStream);

      //         TmpBinaryWriter.Write(StructToBytes(BmpHeader, 14));
      //         TmpBinaryWriter.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)));

      //         //µ÷تش°هذإد¢
      //         for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
      //         {
      //            ColorMask[ColorIndex].redmask = (byte)ColorIndex;
      //            ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
      //            ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
      //            ColorMask[ColorIndex].rgbReserved = 0;

      //            TmpBinaryWriter.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])));
      //         }

      //         //ح¼ئ¬ذ‎×ھ£¬½â¾ِض¸خئح¼ئ¬µ¹ء¢µؤختجâ
      //         RotatePic(buffer, nWidth, nHeight, ref ResBuf);

      //         //ذ´ح¼ئ¬
      //         //TmpBinaryWriter.Write(ResBuf);
      //         byte[] filter = null;
      //         if (w - nWidth > 0)
      //         {
      //            filter = new byte[w - nWidth];
      //         }
      //         for (int i = 0; i < nHeight; i++)
      //         {
      //            TmpBinaryWriter.Write(ResBuf, i * nWidth, nWidth);
      //            if (w - nWidth > 0)
      //            {
      //               TmpBinaryWriter.Write(ResBuf, 0, w - nWidth);
      //            }
      //         }

      //         FileStream.Close();
      //         TmpBinaryWriter.Close();
      //      }
      //      catch (Exception ex)
      //      {
      //         //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
      //         //logger.Append();
      //      }
      //   }
      //}
      //#endregion           
      //#endregion
   }
}
