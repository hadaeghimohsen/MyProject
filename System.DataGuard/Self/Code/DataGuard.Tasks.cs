using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.Runtime.InteropServices;
using System.Net;
using System.Xml.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace System.DataGuard.Self.Code
{
   partial class DataGuard
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         if (iProject == null)
         {
            var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };
            _DefaultGateway.Gateway(
               GetConnectionString
            );
            ConnectionString = GetConnectionString.Output.ToString();
            iProject = new Data.iProjectDataContext(ConnectionString);
         }
         if (job == null) return;

         string value = (job.Input as string).ToLower();
         if (value == "main")
         {
            if (_Main == null)
               _Main = new Ui.Main { _DefaultGateway = this };
            job.Output = _Main;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Login(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;

            /// Get Connection String For Scott User
            var GetConnectionString =
               new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };
            
            _DefaultGateway.Gateway( GetConnectionString );
            if (GetConnectionString.Output != null)
            {
               ConnectionString = GetConnectionString.Output.ToString();
               iProject = new Data.iProjectDataContext(ConnectionString);
            }
            /// Get Query On V#TopNActiveSessionCurrentGateway View And Check Count of Records
            /// if no any one system must call DoWork4Login
            /// but query have one record system must show LastUserLogin
            /// and then iProject variable must new null set
            try
            {
               if (GetConnectionString.Output != null && iProject.V_TopNActiveSessionCurrentGateways.Any())
               {
                  job.OwnerDefineWorkWith.Add(
                  new Job(SendType.External, "Login",
                     new List<Job>
                     {
                        new Job(SendType.Self, 05 /* Execute DoWork4LastUserLogin */)
                     }));
               }
               else
               {
                  job.OwnerDefineWorkWith.Add(
                  new Job(SendType.External, "Login",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute DoWork4Login */)
                     }));
               }
            }
            catch {
               job.OwnerDefineWorkWith.Add(
                  new Job(SendType.External, "Login",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute DoWork4Login */)
                     }));
            }

         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SecuritySettings(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;

            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "main"},
                  new Job(SendType.SelfToUserInterface, "Main", 02 /* Execute Set */)
                  {
                     Next = new Job(SendType.SelfToUserInterface, "Main", 08 /* Execute GoDirect */)
                  }
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      [DllImport("iphlpapi.dll", ExactSpelling = true)]
      public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetHosInfo(Job job)
      {
         try
         {
            
            var result = Dns.GetHostEntry("localhost");
            
            //int intAddress = BitConverter.ToInt32((result.AddressList[result.AddressList.Count() / 2]).GetAddressBytes(), 0);
            
            //byte[] macAddr = new byte[6];
            //uint macAddrLen = (uint)macAddr.Length;
            
            //if (SendARP(intAddress, 0, macAddr, ref macAddrLen) != 0)
            //{               
            //   job.Status = StatusType.Failed;
            //   return;
            //}

            //string[] str = new string[(int)macAddrLen];
            //for (int i = 0; i < macAddrLen; i++)
            //   str[i] = macAddr[i].ToString("x2");


            NetworkInterface[] nics = 
               NetworkInterface.GetAllNetworkInterfaces()
               .Where(n => 
                  n.OperationalStatus == OperationalStatus.Up && 
                  n.Supports(NetworkInterfaceComponent.IPv4) && 
                  (
                     n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                     n.NetworkInterfaceType == NetworkInterfaceType.Ethernet3Megabit ||
                     n.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx ||
                     n.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT ||
                     n.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet ||
                     n.NetworkInterfaceType == NetworkInterfaceType.TokenRing ||
                     n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 
                  )
               ).Take(1).ToArray();

            string[] str = new string[6];
            string ipaddress = "";
            //for each j you can get the MAC
            for (int j = 0; j < nics.Length; j++)
            {
               PhysicalAddress address = nics[j].GetPhysicalAddress();
               byte[] bytes = address.GetAddressBytes();
               for (int i = 0; i < bytes.Length; i++)
                  str[i] = bytes[i].ToString("X2");

               foreach (UnicastIPAddressInformation ip in nics[j].GetIPProperties().UnicastAddresses)
               {
                  if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                  {
                     ipaddress = ip.Address.ToString();
                  }
               }
            }

            /* Cpu ID & MB ID */
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            mbsList = mbs.Get();
            string cpu = "";
            foreach (ManagementObject mo in mbsList)
            {
               cpu = mo["ProcessorID"].ToString();
            }            

            job.Output =
               new XElement("Computer",
                  new XAttribute("name", result.HostName),
                  //new XAttribute("ip", result.AddressList[result.AddressList.Count() / 2].ToString()),
                  new XAttribute("ip", ipaddress),
                  //new XAttribute("mac", macAddrLen > 0 ? string.Join(":", str) : "00:00:00:00:00:00"),
                  new XAttribute("mac", string.Join(":", str)),
                  new XAttribute("cpu", cpu)
               );

            job.Status = StatusType.Successful;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
            job.Status = StatusType.Failed;
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4TinyLock(Job job)
      {
         try
         {
            #region DeHashCode
             var _jobkey =
                      new Job(SendType.External, "Localhost", "DataGuard", 08 /* Execute DoWork4UnSecureHashCode */, SendType.Self) { Input = key };
             _DefaultGateway.Gateway(
                _jobkey
             );

             var _jobStrSafeKey1 =
                      new Job(SendType.External, "Localhost", "DataGuard", 08 /* Execute DoWork4UnSecureHashCode */, SendType.Self) { Input = strSafeKey1 };
             _DefaultGateway.Gateway(
                _jobStrSafeKey1
             );

             var _jobStrSafeKey2 =
                      new Job(SendType.External, "Localhost", "DataGuard", 08 /* Execute DoWork4UnSecureHashCode */, SendType.Self) { Input = strSafeKey2 };
             _DefaultGateway.Gateway(
                _jobStrSafeKey2
             );
            #endregion

            TINYLib.TinyPlusCtrl tinyPlusCntl = new TINYLib.TinyPlusCtrl();
            if (tinyPlusCntl.FindFirstTPlus(_jobkey.Output.ToString(), _jobStrSafeKey1.Output.ToString(), _jobStrSafeKey2.Output.ToString()) == 0)
            {
               int rnd = new Random().Next(5000);
               int mainrnd = DateTime.Now.Millisecond + rnd;

               if (mainrnd >= 5000)
                  mainrnd /= 3;
               if (tinyPlusCntl.GetTPlusQuery(ArrRequest[mainrnd]) == ArrResponse[mainrnd])
               {
                  string serialtiny = tinyPlusCntl.GetTPlusData(TINYLib.EnumTPlusData.TPLUS_DATAPARTITION);
                  GetUi(null);
                  var serialdb = iProject.GETDATA(new XElement("Request", new XAttribute("code", "001")));

                  var _jobHashCode =
                     new Job(SendType.External, "Localhost", "DataGuard", 06 /* Execute DoWork4HashCode */, SendType.Self) { Input = serialdb.Attribute("serialno").Value.Trim() };
                  _DefaultGateway.Gateway(
                     _jobHashCode
                  );

                  if (serialtiny.Trim() != _jobHashCode.Output.ToString().Trim())
                     job.Output =
                        new XElement("TL",
                           new XAttribute("stat", "001"),
                           new XAttribute("erorcode", "900002"),
                           "ؿِزؼْ+ؾؼۗزُ+ٌٍُ+ِطزؽ+ِّۗ+سزؿغ,"
                        );
               }
               else
               {
                  job.Output =
                     new XElement("TL",
                        new XAttribute("stat", "001"),
                        new XAttribute("erorcode", "900003"),
                        "ٌٍُ+ؾعص+زٌؽزؼۗ+ۗزٌص+ّؿغ,"
                     );
               }
            }
            else
            {
               job.Output =
                     new XElement("TL",
                        new XAttribute("stat", "001"),
                        new XAttribute("erorcode", "900003"),
                        "ٌٍُ+ؾعص+زٌؽزؼۗ+ۗزٌص+ّؿغ,"
                     );
            }
         }
         catch (Exception)
         {
            job.Output =
                  new XElement("TL",
                     new XAttribute("stat", "001"),
                     new XAttribute("erorcode", "900001"),
                     "سؼؼؾۗ+ٌٍُ+سز+عقز+ِٓزطْ+ؿغ,"
                  );
         }
         finally
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4HashCode(Job job)
      {
         var sha1 = SHA1.Create();
         var inputbyte = Encoding.ASCII.GetBytes(job.Input.ToString());
         var hash = sha1.ComputeHash(inputbyte);
         var sb = new StringBuilder();
         for (int i = 0; i < hash.Length; i++)
         {
            sb.Append(hash[i].ToString("X2"));
         }
         job.Output = sb.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SecureHashCode(Job job)
      {
         string temp = "";
         string input = job.Input.ToString();
         for (int i = 0; i < input.ToString().Length; i++)
         {
            temp += Convert.ToChar(System.Convert.ToInt32(input[i]) + 11);
         }
         job.Output = temp;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4UnSecureHashCode(Job job)
      {
         string temp = "";
         string input = job.Input.ToString();
         for (int i = 0; i < input.ToString().Length; i++)
         {
            temp += Convert.ToChar(System.Convert.ToInt32(input[i]) - 11);
         }
         job.Output = temp;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LockScreen(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;

            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Login",
                  new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Login */)
                     }));
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4TryLogin(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
            new Job(SendType.External, "Login",
               new List<Job>
                  {
                     new Job(SendType.Self, 02 /* Execute DoWork4Login */)
                  }));
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetServer(Job job)
      {
         job.Output = iProject.GET_SRVR_U(job.Input as XElement);
         job.Status = StatusType.Successful;         
      }

   }
}
