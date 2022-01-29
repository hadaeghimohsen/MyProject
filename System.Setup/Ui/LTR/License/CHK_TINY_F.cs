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
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace System.Setup.Ui.LTR.License
{
   public partial class CHK_TINY_F : UserControl
   {
      public CHK_TINY_F()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         Application.Exit();
         Process.GetCurrentProcess().Kill();
      }

      private void Connect_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (TinySNInstaller_Txt.Text == "") { TinySNInstaller_Txt.Focus(); return; }

            // Install Tiny SDK
            var execpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var parentpath = new System.IO.DirectoryInfo(execpath).Parent;
            var tinypath = parentpath.FullName + "\\tools\\tinysdk";
            //tinypath = @"D:\AnarSys\tools\tinysdk";
            if (Environment.Is64BitOperatingSystem)
            {               
               foreach (var dllfile in IO.Directory.GetFiles(tinypath).Where(f => f.Contains(".ocx")))
               {
                  IO.File.Copy(dllfile, Environment.ExpandEnvironmentVariables(@"%windir%\syswow64\") + new IO.FileInfo(dllfile).Name, true);
               }
               Process.Start(tinypath + "\\Just64bit_register_TinySDK.bat");
            }
            else
            {
               foreach (var dllfile in IO.Directory.GetFiles(tinypath).Where(f => f.Contains(".ocx")))
               {
                  IO.File.Copy(dllfile, Environment.ExpandEnvironmentVariables(@"%windir%\system32\") + new IO.FileInfo(dllfile).Name, true);
               }
               Process.Start(tinypath + "\\Just32bit_register_TinySDK.bat");
            }

            if (!ModifierKeys.HasFlag(Keys.Control))
            {
               // Check Input Tiny Serial No With Tiny Lock
               var _CheckInstallTinyLock =
                  new Job(SendType.External, "localhost", "DefaultGateway:DataGuard", 32 /* Execute DoWork4CheckInstallTinyLock */, SendType.Self) { Input = TinySNInstaller_Txt.Text };
               _DefaultGateway.Gateway(_CheckInstallTinyLock);

               if (_CheckInstallTinyLock.Output != null)
               {
                  var _jobUnSecureHashCode =
                     new Job(SendType.External, "Localhost", "DefaultGateway:DataGuard", 08 /* Execute DoWork4UnSecureHashCode  */, SendType.Self) { Input = (_CheckInstallTinyLock.Output as XElement).Value };
                  _DefaultGateway.Gateway(_jobUnSecureHashCode);
                  MessageBox.Show(_jobUnSecureHashCode.Output.ToString());
               }
               else
               {
                  // Open First Page Setup
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 02 /* Execute Frst_Page_F */)
                        }
                     )
                  );
               }
            }
            else
            {
               // Open First Page Setup
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                        {
                           new Job(SendType.Self, 02 /* Execute Frst_Page_F */)
                        }
                  )
               );
            }
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Clear_Butn_Click(object sender, EventArgs e)
      {
         TinySNInstaller_Txt.EditValue = PblcKey_Txt.EditValue = LicnKey_Txt.EditValue = null;
      }
   }

   public static class StringCipher
   {
      // This constant is used to determine the keysize of the encryption algorithm in bits.
      // We divide this by 8 within the code below to get the equivalent number of bytes.
      private const int Keysize = 256;

      // This constant determines the number of iterations for the password bytes generation function.
      private const int DerivationIterations = 1000;

      public static string Encrypt(string plainText, string passPhrase)
      {
         // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
         // so that the same Salt and IV values can be used when decrypting.  
         var saltStringBytes = Generate256BitsOfRandomEntropy();
         var ivStringBytes = Generate256BitsOfRandomEntropy();
         var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
         using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
         {
            var keyBytes = password.GetBytes(Keysize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
               symmetricKey.BlockSize = 256;
               symmetricKey.Mode = CipherMode.CBC;
               symmetricKey.Padding = PaddingMode.PKCS7;
               using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
               {
                  using (var memoryStream = new MemoryStream())
                  {
                     using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                     {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                        var cipherTextBytes = saltStringBytes;
                        cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                        cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                        memoryStream.Close();
                        cryptoStream.Close();
                        return Convert.ToBase64String(cipherTextBytes);
                     }
                  }
               }
            }
         }
      }

      public static string Decrypt(string cipherText, string passPhrase)
      {
         // Get the complete stream of bytes that represent:
         // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
         var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
         // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
         var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
         // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
         var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
         // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
         var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

         using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
         {
            var keyBytes = password.GetBytes(Keysize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
               symmetricKey.BlockSize = 256;
               symmetricKey.Mode = CipherMode.CBC;
               symmetricKey.Padding = PaddingMode.PKCS7;
               using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
               {
                  using (var memoryStream = new MemoryStream(cipherTextBytes))
                  {
                     using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                     {
                        var plainTextBytes = new byte[cipherTextBytes.Length];
                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        memoryStream.Close();
                        cryptoStream.Close();
                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                     }
                  }
               }
            }
         }
      }

      private static byte[] Generate256BitsOfRandomEntropy()
      {
         var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
         using (var rngCsp = new RNGCryptoServiceProvider())
         {
            // Fill the array with cryptographically secure random bytes.
            rngCsp.GetBytes(randomBytes);
         }
         return randomBytes;
      }
   }
}
