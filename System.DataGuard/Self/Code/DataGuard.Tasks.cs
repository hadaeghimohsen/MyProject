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
using System.Threading;

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
            string cpuID = string.Empty;
            

            job.Output =
               new XElement("Computer",
                  new XAttribute("name", result.HostName),
                  //new XAttribute("ip", result.AddressList[result.AddressList.Count() / 2].ToString()),
                  new XAttribute("ip", ipaddress),
                  //new XAttribute("mac", macAddrLen > 0 ? string.Join(":", str) : "00:00:00:00:00:00"),
                  new XAttribute("mac", string.Join(":", str)),
                  new XAttribute("cpu", cpu),
                  new XElement("IPAddresses",
                      Dns.GetHostAddresses(Dns.GetHostName()).Select(ipadrs => new XElement("IP", ipadrs.ToString()))
                  )
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

      /// <summary>
      /// Code 12
      /// Retrieving Processor Id.
      /// </summary>
      /// <returns></returns>
      /// 
      private void DoWork4GetProcessorId(Job job)
      {
         ManagementClass mc = new ManagementClass("win32_processor");
         ManagementObjectCollection moc = mc.GetInstances();
         String Id = String.Empty;
         foreach (ManagementObject mo in moc)
         {

            Id = mo.Properties["processorID"].Value.ToString();
            break;
         }

         job.Output = Id;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 13
      /// Retrieving HDD Serial No.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetHDDSerialNo(Job job)
      {
         ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
         ManagementObjectCollection mcol = mangnmt.GetInstances();
         string result = "";
         foreach (ManagementObject strt in mcol)
         {
            result += Convert.ToString(strt["VolumeSerialNumber"]);
         }

         job.Output = result;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 14
      /// Retrieving System MAC Address.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetMACAddress(Job job)
      {
         ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
         ManagementObjectCollection moc = mc.GetInstances();
         string MACAddress = String.Empty;
         foreach (ManagementObject mo in moc)
         {
            if (MACAddress == String.Empty)
            {
               if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
            }
            mo.Dispose();
         }

         MACAddress = MACAddress.Replace(":", "");
         job.Output = MACAddress;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 15
      /// Retrieving CD-DVD Drive Path.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetCdRomDrive(Job job)
      {
         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("Drive").ToString();
               job.Status = StatusType.Successful;
               return;
            }
            catch { }
         }

         job.Output = "CD ROM Drive Letter: Unknown";
         job.Status = StatusType.Failed;
      }

      /// <summary>
      /// Code 16
      /// Retrieving BIOS Maker.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetBIOSmaker(Job job)
      {

         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("Manufacturer").ToString();
               job.Status = StatusType.Successful;
               return;
            }
            catch { }

         }

         job.Output = "BIOS Maker: Unknown";
         job.Status = StatusType.Failed;
      }

      /// <summary>
      /// Code 17
      /// Retrieving BIOS Serial No.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetBIOSserNo(Job job)
      {

         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("SerialNumber").ToString();
               job.Status = StatusType.Successful;
               return;
            }

            catch { }

         }

         job.Output = "BIOS Serial Number: Unknown";
         job.Status = StatusType.Failed;
      }

      /// <summary>
      /// Code 18
      /// Retrieving BIOS Caption.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetBIOScaption(Job job)
      {

         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("Caption").ToString();
               job.Status = StatusType.Successful;
               return;
            }
            catch { }
         }

         job.Output = "BIOS Caption: Unknown";
         job.Status = StatusType.Failed;
      }

      /// <summary>
      /// Code 19
      /// Retrieving System Account Name.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetAccountName(Job job)
      {
         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("Name").ToString();
               job.Status = StatusType.Successful;
               return;
            }
            catch { }
         }
         job.Output = "User Account Name: Unknown";
         job.Status = StatusType.Failed;
      }

      /// <summary>
      /// Code 20
      /// Retrieving Physical Ram Memory.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetPhysicalMemory(Job job)
      {
         ManagementScope oMs = new ManagementScope();
         ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
         ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
         ManagementObjectCollection oCollection = oSearcher.Get();

         long MemSize = 0;
         long mCap = 0;

         // In case more than one Memory sticks are installed
         foreach (ManagementObject obj in oCollection)
         {
            mCap = Convert.ToInt64(obj["Capacity"]);
            MemSize += mCap;
         }
         MemSize = (MemSize / 1024) / 1024;
         job.Output = MemSize.ToString() + " MB";
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 21
      /// Retrieving No of Ram Slot on Motherboard.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetNoRamSlots(Job job)
      {

         int MemSlots = 0;
         ManagementScope oMs = new ManagementScope();
         ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
         ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
         ManagementObjectCollection oCollection2 = oSearcher2.Get();
         foreach (ManagementObject obj in oCollection2)
         {
            MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

         }
         job.Output = MemSlots.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 22
      /// method for retrieving the CPU Manufacturer
      /// using the WMI class
      /// </summary>
      /// <returns>CPU Manufacturer</returns>
      private void DoWork4GetCPUManufacturer(Job job)
      {
         string cpuMan = String.Empty;
         //create an instance of the Managemnet class with the
         //Win32_Processor class
         ManagementClass mgmt = new ManagementClass("Win32_Processor");
         //create a ManagementObjectCollection to loop through
         ManagementObjectCollection objCol = mgmt.GetInstances();
         //start our loop for all processors found
         foreach (ManagementObject obj in objCol)
         {
            if (cpuMan == String.Empty)
            {
               // only return manufacturer from first CPU
               cpuMan = obj.Properties["Manufacturer"].Value.ToString();
            }
         }
         job.Output = cpuMan;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 23
      /// method to retrieve the CPU's current
      /// clock speed using the WMI class
      /// </summary>
      /// <returns>Clock speed</returns>
      private void DoWork4GetCPUCurrentClockSpeed(Job job)
      {
         int cpuClockSpeed = 0;
         //create an instance of the Managemnet class with the
         //Win32_Processor class
         ManagementClass mgmt = new ManagementClass("Win32_Processor");
         //create a ManagementObjectCollection to loop through
         ManagementObjectCollection objCol = mgmt.GetInstances();
         //start our loop for all processors found
         foreach (ManagementObject obj in objCol)
         {
            if (cpuClockSpeed == 0)
            {
               // only return cpuStatus from first CPU
               cpuClockSpeed = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
            }
         }
         //return the status
         job.Output = cpuClockSpeed;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 24
      /// method to retrieve the network adapters
      /// default IP gateway using WMI
      /// </summary>
      /// <returns>adapters default IP gateway</returns>
      private void DoWork4GetDefaultIPGateway(Job job)
      {
         //create out management class object using the
         //Win32_NetworkAdapterConfiguration class to get the attributes
         //of the network adapter
         ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
         //create our ManagementObjectCollection to get the attributes with
         ManagementObjectCollection objCol = mgmt.GetInstances();
         string gateway = String.Empty;
         //loop through all the objects we find
         foreach (ManagementObject obj in objCol)
         {
            if (gateway == String.Empty)  // only return MAC Address from first card
            {
               //grab the value from the first network adapter we find
               //you can change the string to an array and get all
               //network adapters found as well
               //check to see if the adapter's IPEnabled
               //equals true
               if ((bool)obj["IPEnabled"] == true)
               {
                  gateway = obj["DefaultIPGateway"].ToString();
               }
            }
            //dispose of our object
            obj.Dispose();
         }
         //replace the ":" with an empty space, this could also
         //be removed if you wish
         gateway = gateway.Replace(":", "");
         //return the mac address
         job.Output = gateway;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 25
      /// Retrieving Motherboard Manufacturer.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetBoardMaker(Job job)
      {

         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("Manufacturer").ToString();
               job.Status = StatusType.Successful;
               return;
            }

            catch { }

         }

         job.Output = "Board Maker: Unknown";
         job.Status = StatusType.Failed;

      }

      /// <summary>
      /// Code 26
      /// Retrieving Motherboard Product Id.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetBoardProductId(Job job)
      {

         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("Product").ToString();
               job.Status = StatusType.Successful;
               return;

            }

            catch { }

         }

         job.Output = "Product: Unknown";
         job.Status = StatusType.Failed;

      }

      /// <summary>
      /// Code 27
      /// Retrieve CPU Speed.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetCpuSpeedInGHz(Job job)
      {
         double? GHz = null;
         using (ManagementClass mc = new ManagementClass("Win32_Processor"))
         {
            foreach (ManagementObject mo in mc.GetInstances())
            {
               GHz = 0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value;
               break;
            }
         }
         job.Output = GHz;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 28
      /// Retrieving Current Language
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetCurrentLanguage(Job job)
      {

         ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = wmi.GetPropertyValue("CurrentLanguage").ToString();
               job.Status = StatusType.Successful;
               return;
            }

            catch { }

         }

         job.Output = "BIOS Maker: Unknown";
         job.Status = StatusType.Failed;

      }

      /// <summary>
      /// Code 29
      /// Retrieving Current Language.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetOSInformation(Job job)
      {
         ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
         foreach (ManagementObject wmi in searcher.Get())
         {
            try
            {
               job.Output = ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["Version"] + ", " + (string)wmi["OSArchitecture"];
               job.Status = StatusType.Successful;
               return;
            }
            catch { }
         }
         job.Output = "BIOS Maker: Unknown";
         job.Status = StatusType.Failed;
      }

      /// <summary>
      /// Code 30
      /// Retrieving Processor Information.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetProcessorInformation(Job job)
      {
         ManagementClass mc = new ManagementClass("win32_processor");
         ManagementObjectCollection moc = mc.GetInstances();
         String info = String.Empty;
         foreach (ManagementObject mo in moc)
         {
            string name = (string)mo["Name"];
            name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

            info = name + ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"];
            //mo.Properties["Name"].Value.ToString();
            //break;
         }
         job.Output = info;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 31
      /// Retrieving Computer Name.
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetComputerName(Job job)
      {
         ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
         ManagementObjectCollection moc = mc.GetInstances();
         String info = String.Empty;
         foreach (ManagementObject mo in moc)
         {
            info = (string)mo["Name"];
            //mo.Properties["Name"].Value.ToString();
            //break;
         }
         job.Output = info;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 32
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4CheckInstallTinyLock(Job job)
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
                  //GetUi(null);
                  var serialdb = job.Input.ToString();//iProject.GETDATA(new XElement("Request", new XAttribute("code", "001")));

                  var _jobHashCode =
                     new Job(SendType.External, "Localhost", "DataGuard", 06 /* Execute DoWork4HashCode */, SendType.Self) { Input = serialdb };
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
      /// Code 33
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetLicenseDay(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(               
               new Job(SendType.External, "SecurityPolicy",
               new List<Job>
                  {
                     new Job(SendType.Self, 10 /* Execute DoWork4SettingsSystem */){Input = "SettingsSystem"},
                     new Job(SendType.SelfToUserInterface, "SettingsSystem", 04 /* Execute UnPaint */),
                     new Job(SendType.SelfToUserInterface, "SettingsSystem", 10 /* Execute ActionCallWindow */){Input = job.Input}                     
                  }));
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 34
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Backup(Job job)
      {
         try
         {
            if (iProject == null)
               iProject = new Data.iProjectDataContext(ConnectionString);

            var xinput = job.Input as XElement;

            switch(xinput.Attribute("type").Value)
            { 
               case "exit":
                  if (iProject.Sub_Systems.Any(s => s.STAT == "002" && s.INST_STAT == "002" && s.BACK_UP_APP_EXIT == "002"))
                     iProject.TakedbBackup(new XElement("Backup", ""));
                  break;
               case "immediate":
                  iProject.TakedbBackup(new XElement("Backup", ""));
                  break;
            }
            job.Status = StatusType.Successful;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            job.Status = StatusType.Failed;
         }
      }

      /// <summary>
      /// Code 35
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4PinCode(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Login",
               new List<Job>
                  {
                     new Job(SendType.Self, 07 /* Execute DoWork4PinCode */),
                     new Job(SendType.SelfToUserInterface, "PinCode", 10 /* Execute ActionCallWindow */){Input = job.Input}
                  }));
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 36
      /// Get Serial Number 
      /// </summary>
      /// <returns></returns>
      private void DoWork4GetSerialNumber(Job job)
      {
         job.Output = iProject.V_Settings.FirstOrDefault().TINY_SERL;
         job.Status = StatusType.Successful;
      }
   }
}
