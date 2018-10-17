using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Guis
{
   partial class OdbcSettings
   {
      internal Odbccfg.Odbc _Odbc { get; set; }

      private string ODBC_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
      private string ODBCINST_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBCINST.INI\\";
      
      private const short ODBC_ADD_DSN = 1;
      private const short ODBC_CONFIG_DSN = 2;
      private const short ODBC_REMOVE_DSN = 3;
      private const short ODBC_ADD_SYS_DSN = 4;
      private const short ODBC_CONFIG_SYS_DSN = 5;
      private const short ODBC_REMOVE_SYS_DSN = 6;
      private const int vbAPINull = 0;

      internal void RequestToUserInterface(Job job)
      {
         switch (job.Method)
         {
            case 01:
               ShowDialog(job);
               break;
            case 02:
               CreateDSN(job);
               break;
            case 03:
               RemoveDSN(job);
               break;
            case 04:
               DSNExists(job);
               break;
            case 05:
               GetInstalledDrivers(job);
               break;
            case 06:
               GetSystemDataSourceNames(job);
               break;
            case 07:
               GetUserDataSourceNames(job);
               break;
            case 08:
               GetAllDataSourceName(job);
               break;
            case 09:
               SetDsnNameOnConnectionString(job);
               break;
            case 10:
               Import2Odbc(job);
               break;
            case 11:
               WindowsPlatform(job);
               break;
            case 12:
               CreateDsnNameWithoutGrant(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void ShowDialog(Job job)
      {
         job.Status = StatusType.Successful;
         ShowDialog();
      }

      /// <summary>
      /// Creates a new DSN entry with the specified values. If the DSN exists, the values are updated.
      /// Code 02
      /// </summary>
      /// <param name="dsnName">Name of the DSN for use by client applications</param>
      /// <param name="description">Description of the DSN that appears in the ODBC control panel applet</param>
      /// <param name="server">Network name or IP address of database server</param>
      /// <param name="driverName">Name of the driver to use</param>
      /// <param name="trustedConnection">True to use NT authentication, false to require applications to supply username/password in the connection string</param>
      /// <param name="database">Name of the datbase to connect to</param>
      private void CreateDSN(Job job)
      {
         try
         {
            string dsnName = (job.Input as List<object>)[0].ToString(),
                   description = (job.Input as List<object>)[1].ToString(),
                   server = (job.Input as List<object>)[2].ToString(),
                   driverName = (job.Input as List<object>)[3].ToString(),
                   database = (job.Input as List<object>)[4].ToString();
            bool trustedConnection = (bool)(job.Input as List<object>)[5];

            // Lookup driver path from driver name
            var driverKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + driverName);
            if (driverKey == null) throw new Exception(string.Format("ODBC Registry key for driver '{0}' does not exist", driverName));
            string driverPath = driverKey.GetValue("Driver").ToString();

            // Add value to odbc data sources
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(ODBC_INI_REG_PATH + "ODBC Data Sources");
            if (datasourcesKey == null) throw new Exception("ODBC Registry key for datasources does not exist");
            datasourcesKey.SetValue(dsnName, driverName);

            // Create new key in odbc.ini with dsn name and add values
            var dsnKey = Registry.LocalMachine.CreateSubKey(ODBC_INI_REG_PATH + dsnName);
            if (dsnKey == null) throw new Exception("ODBC Registry key for DSN was not created");
            dsnKey.SetValue("DSN", dsnName);
            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("Description", description);
            dsnKey.SetValue("Driver", driverPath);
            dsnKey.SetValue("LastUser", Environment.UserName);
            dsnKey.SetValue("Server", server);
            dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");
            if (driverName.Contains("Oracle"))
            {
               dsnKey.SetValue("UserID", "NoUser");
               dsnKey.SetValue("Password", "NoPassword");
               dsnKey.SetValue("Application Attributes", "T");
               dsnKey.SetValue("Attributes", "W");
               dsnKey.SetValue("BatchAutocommitMode", "IfAllSuccessful");
               dsnKey.SetValue("BindAsDATE", "F");
               dsnKey.SetValue("BindAsFLOAT", "F");
               dsnKey.SetValue("CacheBufferSize", "20");
               dsnKey.SetValue("CloseCursor", "F");
               dsnKey.SetValue("DisableDPM", "F");
               dsnKey.SetValue("DisableMTS", "T");
               dsnKey.SetValue("DisableRULEHint", "T");
               dsnKey.SetValue("EXECSchemaOpt", "");
               dsnKey.SetValue("EXECSyntax", "F");
               dsnKey.SetValue("Failover", "T");
               dsnKey.SetValue("FailoverDelay", "10");
               dsnKey.SetValue("FailoverRetryCount", "10");
               dsnKey.SetValue("FetchBufferSize", "64000");
               dsnKey.SetValue("ForceWCHAR", "F");
               dsnKey.SetValue("Lobs", "T");
               dsnKey.SetValue("MetadataldDefault", "F");
               dsnKey.SetValue("NumericSetting", "NLS");
               dsnKey.SetValue("QueryTimeout", "T");
               dsnKey.SetValue("ResultSets", "T");
               dsnKey.SetValue("StatmentCache", "F");
               dsnKey.SetValue("ServerName", server);
            }

            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Failed;
         }
      }

      /// <summary>
      /// Removes a DSN entry
      /// Code 03
      /// </summary>
      /// <param name="dsnName">Name of the DSN to remove.</param>
      private void RemoveDSN(Job job)
      {
         try
         {
            string dsnName = job.Input.ToString();
            // Remove DSN key
            Registry.LocalMachine.DeleteSubKeyTree(ODBC_INI_REG_PATH + dsnName);

            // Remove DSN name from values list in ODBC Data Sources key
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(ODBC_INI_REG_PATH + "ODBC Data Sources");
            if (datasourcesKey == null) throw new Exception("ODBC Registry key for datasources does not exist");
            datasourcesKey.DeleteValue(dsnName);
         }
         catch { }
         finally
         {
            job.Status = StatusType.Successful;
         }
      }

      ///<summary>
      /// Checks the registry to see if a DSN exists with the specified name
      /// Code 04
      ///</summary>
      ///<param name="dsnName"></param>
      ///<returns></returns>
      private void DSNExists(Job job)
      {
         try
         {
            string dsnName = job.Input.ToString();
            //if(Is64Bit())
            //{
            //   ODBC_INI_REG_PATH = "Software\\Wow6432Node\\ODBC\\ODBC.INI\\";
            //   ODBCINST_INI_REG_PATH = "Software\\Wow6432Node\\ODBC\\ODBCINST.INI\\";
            //}
            //else
            //{
            //   ODBC_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
            //   ODBCINST_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBCINST.INI\\";
            //}

            //var driversKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + "ODBC Drivers");
            //if (driversKey == null)
            //{
            //   //throw new Exception("ODBC Registry key for drivers does not exist");
            //   job.Status = StatusType.Failed;
            //   return;
            //}


            //job.Output = driversKey.GetValue(dsnName) != null;
            //if ((bool)job.Output)
            //   job.Status = StatusType.Successful;
            //else
            //   job.Status = StatusType.Failed;

            int iData;
            string strRetBuff = "";
            iData = SQLGetPrivateProfileString("ODBC Data Sources", dsnName, "", strRetBuff, 200, "odbc.ini");
            if(iData != 0)
               job.Status = StatusType.Successful;
            else
               job.Status = StatusType.Failed;
         }
         catch { job.Status = StatusType.Successful; }
      }

      ///<summary>
      /// Returns an array of driver names installed on the system
      /// Code 05
      ///</summary>
      ///<returns></returns>
      private void GetInstalledDrivers(Job job)
      {
         var driversKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + "ODBC Drivers");
         if (driversKey == null) throw new Exception("ODBC Registry key for drivers does not exist");

         var driverNames = driversKey.GetValueNames();

         var ret = new List<string>();

         foreach (var driverName in driverNames)
         {
            if (driverName != "(Default)")
            {
               ret.Add(driverName);
            }
         }

         job.Output = ret;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Gets all System data source names for the local machine.
      /// Case 06
      /// </summary>
      private void GetSystemDataSourceNames(Job job)
      {
         List<String> dsnList = new List<string>();

         // get system dsn's
         Microsoft.Win32.RegistryKey reg = (Microsoft.Win32.Registry.LocalMachine).OpenSubKey("Software");
         if (reg != null)
         {
            reg = reg.OpenSubKey("ODBC");
            if (reg != null)
            {
               reg = reg.OpenSubKey("ODBC.INI");
               if (reg != null)
               {
                  reg = reg.OpenSubKey("ODBC Data Sources");
                  if (reg != null)
                  {
                     // Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
                     foreach (string sName in reg.GetValueNames())
                     {
                        dsnList.Add(sName);
                     }
                  }
                  try
                  {
                     reg.Close();
                  }
                  catch { /* ignore this exception if we couldn't close */ }
               }
            }
         }

         job.Output = dsnList;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Gets all User data source names for the local machine.
      /// Case 07
      /// </summary>
      private void GetUserDataSourceNames(Job job)
      {
         List<string> dsnList = new List<string>();

         // get user dsn's
         Microsoft.Win32.RegistryKey reg = (Microsoft.Win32.Registry.CurrentUser).OpenSubKey("Software");
         if (reg != null)
         {
            reg = reg.OpenSubKey("ODBC");
            if (reg != null)
            {
               reg = reg.OpenSubKey("ODBC.INI");
               if (reg != null)
               {
                  reg = reg.OpenSubKey("ODBC Data Sources");
                  if (reg != null)
                  {
                     // Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
                     foreach (string sName in reg.GetValueNames())
                     {
                        dsnList.Add(sName);
                     }
                  }
                  try
                  {
                     reg.Close();
                  }
                  catch { /* ignore this exception if we couldn't close */ }
               }
            }
         }

         job.Output = dsnList;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void GetAllDataSourceName(Job job)
      {
         Job _GetAllDataSource = new Job(SendType.External, "Odbc",
             new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "OdbcSettings", 06 /* Execute GetSystemDataSourceNames */),
                    new Job(SendType.SelfToUserInterface, "OdbcSettings", 07 /* Execute GetUserDataSourceNames */)
                });
         _Odbc.Gateway(_GetAllDataSource);

         var getSystemDsn = _GetAllDataSource.OwnerDefineWorkWith[0].Output as List<string>;
         var getUserDsn = _GetAllDataSource.OwnerDefineWorkWith[1].Output as List<string>;
         getSystemDsn.ForEach(dsn => getUserDsn.Add(dsn));
         job.Output = getUserDsn;
         job.Status = StatusType.Successful;

      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void SetDsnNameOnConnectionString(Job job)
      {
         txt_dsnconnectionstring.Text = string.Format("Dsn={0};", job.Input.ToString());
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Import2Odbc(Job job)
      {
         DataSet ds = job.Input as DataSet;

         ds.Tables["DataSources"]
            .Rows.OfType<DataRow>()
            .ToList()
            .ForEach(dbs =>
            {
               _Odbc.Gateway(
                  new Job(SendType.External, "Odbc",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "OdbcSettings", 03 /* Execute RemoveDSN */){Input = dbs["Dbid"]},
                        new Job(SendType.SelfToUserInterface,"OdbcSettings",  02 /* Execute CreateDSN */)
                        {
                           Input = new List<object>
                           {
                              dbs["Dbid"],
                              "no description",
                              Convert.ToInt16(dbs["DatabaseServer"]) == 0 ? string.Format("{0}:1521/{1}", dbs["IPAddress"], dbs["Database"]) : dbs["IPAddress"],
                              //Convert.ToInt16(dbs["DatabaseServer"]) == 0 ? "Microsoft ODBC for Oracle" : "SQL Server",
                              Convert.ToInt16(dbs["DatabaseServer"]) == 0 ? "Oracle in OraClient11g_home1" : "SQL Server",
                              dbs["Database"],
                              false,
                           }
                        }
                     }) { FaultTolerance = FaultToleranceType.Ignore });
            });
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void WindowsPlatform(Job job)
      {
         job.Output = Is64Bit() ? "64" : "32";
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void CreateDsnNameWithoutGrant(Job job)
      {
         try
         {
            string dsnName = (job.Input as List<object>)[0].ToString(),
                   description = (job.Input as List<object>)[1].ToString(),
                   server = (job.Input as List<object>)[2].ToString(),
                   driverName = (job.Input as List<object>)[3].ToString(),
                   database = (job.Input as List<object>)[4].ToString(),
                   uid = (job.Input as List<object>)[5].ToString(),
                   pwd = (job.Input as List<object>)[6].ToString();

            string strAttributes = "DSN=" + dsnName + "\0";
            strAttributes += "SYSTEM=" + server + "\0";
            strAttributes += "UID=" + uid + "\0";
            strAttributes += "PWD=" + pwd + "\0";

            string strDSN = "";
            strDSN = strDSN + "System = " + server + "\n";
            strDSN = strDSN + "Description = " + description + "\n";

            if (SQLConfigDataSource((IntPtr)vbAPINull, ODBC_ADD_SYS_DSN, driverName, strAttributes))
            {
               job.Output = "Successful";
               job.Status = StatusType.Successful;
            }
            else
            {
               job.Output = "Failed";
               job.Status = StatusType.Failed;
            }
         }
         catch(Exception exc)
         {
            job.Output = exc.Message;
            job.Status = StatusType.Failed;            
         }
      }
   }
}
