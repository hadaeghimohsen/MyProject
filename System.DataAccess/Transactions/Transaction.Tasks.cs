using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;
using System.ServiceProcess;

namespace System.DataAccess.Transactions
{
   partial class Transaction
   {
      /*
       * Index of method code is between 1 and 100
       */

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteOdbcTransaction(Job job)
      {
         string _executeType = (string)(job.Input as List<object>)[0];
         OdbcCommand _odbcCommand = (job.Input as List<object>)[1] as OdbcCommand;

         Job _ExecuteTransaction = null;
         switch (_executeType)
         {
            case "ExecuteNonQuery":
               _ExecuteTransaction = new Job(SendType.Self, 02) { Input = _odbcCommand };
               break;
            case "ExecuteScalar":
               _ExecuteTransaction = new Job(SendType.Self, 03) { Input = _odbcCommand };
               break;
            case "FillSharedPoolQuery":
               _ExecuteTransaction = new Job(SendType.Self, 04) { Input = new OdbcDataAdapter(_odbcCommand) };
               break;
            case "ExecuteQuery":
               _ExecuteTransaction = new Job(SendType.Self, 05) { Input = new OdbcDataAdapter(_odbcCommand) };
               break;
         }
         Manager(_ExecuteTransaction);

         job.Output = _ExecuteTransaction.Output;
         job.Status = _ExecuteTransaction.Status;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteOdbcNonQuery(Job job)
      {
         OdbcCommand _odbcCommand = job.Input as OdbcCommand;
       
         try
         {
            _odbcCommand.Connection.Open();
            _odbcCommand.ExecuteNonQuery();
            _odbcCommand.Connection.Close();
            job.Status = StatusType.Successful;
         }
         catch (OdbcException odbcException)
         {
            job.Message = odbcException.Message;
            _odbcCommand.Connection.Close();
            job.Status = StatusType.Failed;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteOdbcScalar(Job job)
      {
         OdbcCommand _odbcCommand = job.Input as OdbcCommand;

         try
         {
            _odbcCommand.Connection.Open();
            job.Output = _odbcCommand.ExecuteScalar();
            _odbcCommand.Connection.Close();
            job.Status = StatusType.Successful;
         }
         catch (OdbcException odbcException)
         {
            job.Message = odbcException.Message;
            _odbcCommand.Connection.Close();
            job.Status = StatusType.Failed;
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteOdbcFillSharedPoolQuery(Job job)
      {
         OdbcDataAdapter _odbcDataAdapter = job.Input as OdbcDataAdapter;

         try
         {
            DataSet ds = new DataSet();
            _odbcDataAdapter.Fill(ds);

            #region Set TableName And Relations Into DataSet
            DataTable dt = ds.Tables[ds.Tables.Count - 1];
            DataRow row = dt.Rows[0];
            string[] TablesName = row["TablesName"].ToString().Split(':');
            ds.DataSetName = row["TableSpaceName"].ToString();

            int countTables = Convert.ToBoolean(row["hasRelation"]) ? ds.Tables.Count - 2 : ds.Tables.Count - 1;

            for (int i = 0; i < countTables; i++)
            {
               ds.Tables[i].TableName = TablesName[i];
            }

            // Set Relation Database Tables

            if (Convert.ToBoolean(row["hasRelation"]))
            {
               ds.Relations.Clear();
               foreach (DataRow item in ds.Tables[countTables].Rows)
               {
                  ds.Relations.Add(
                      item["RName"].ToString(),
                      ds.Tables[item["ParentTable"].ToString()].Columns[item["ParentKey"].ToString()],
                      ds.Tables[item["ForiegnTable"].ToString()].Columns[item["ForiegnKey"].ToString()],
                      false
                      );
               }
               ds.Tables.RemoveAt(ds.Tables.Count - 1);
            }

            ds.Tables.RemoveAt(ds.Tables.Count - 1);
            // Create Job for save run info in list for log and show user. :D ;)
            #endregion
            job.Output = ds;
            job.Status = StatusType.Successful;
         }
         catch (OdbcException odbcException)
         {
            job.Message = odbcException.Message;
            _odbcDataAdapter.SelectCommand.Connection.Close();
            job.Status = StatusType.Failed;
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteQuery(Job job)
      {
         OdbcDataAdapter _odbcDataAdapter = job.Input as OdbcDataAdapter;
         _odbcDataAdapter.SelectCommand.CommandType = CommandType.Text;

         try
         {
            DataSet ds = new DataSet();
            _odbcDataAdapter.Fill(ds);
            _odbcDataAdapter.SelectCommand.Connection.Close();
            
            job.Output = ds;
            job.Status = StatusType.Successful;
         }
         catch (OdbcException odbcException)
         {
            job.Message = odbcException.Message;
            _odbcDataAdapter.SelectCommand.Connection.Close();
            job.Status = StatusType.Failed;
         }
      }

      ////////////////////////////////////////////////////////////////////////////////

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteO2dbcTransaction(Job job)
      {
         /*
         string _executeType = (string)(job.Input as List<object>)[0];
         OracleCommand _o2dbcCommand = (job.Input as List<object>)[1] as OracleCommand;

         Job _ExecuteTransaction = null;
         switch (_executeType)
         {
            case "ExecuteNonQuery":
               _ExecuteTransaction = new Job(SendType.Self, 06) { Input = _o2dbcCommand };
               break;
            case "ExecuteScalar":
               _ExecuteTransaction = new Job(SendType.Self, 07) { Input = _o2dbcCommand };
               break;
            case "FillSharedPoolQuery":
               _ExecuteTransaction = new Job(SendType.Self, 08) { Input = new OracleDataAdapter(_o2dbcCommand) };
               break;
            default:
               break;
         }
         Manager(_ExecuteTransaction);

         job.Output = _ExecuteTransaction.Output;
         job.Status = _ExecuteTransaction.Status;
          * */
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteO2dbcNonQuery(Job job)
      {
         /*
         OracleCommand _o2dbcCommand = job.Input as OracleCommand;

         try
         {
            _o2dbcCommand.Connection.Open();
            _o2dbcCommand.ExecuteNonQuery();
            _o2dbcCommand.Connection.Close();
            job.Status = StatusType.Successful;
         }
         catch (OdbcException odbcException)
         {
            job.Message = odbcException.Message;
            job.Status = StatusType.Failed;
         }
          */
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteO2dbcScalar(Job job)
      {
         /*
         OracleCommand _o2dbcCommand = job.Input as OracleCommand;

         try
         {
            _o2dbcCommand.Connection.Open();
            job.Output = _o2dbcCommand.ExecuteScalar();
            _o2dbcCommand.Connection.Close();
            job.Status = StatusType.Successful;
         }
         catch (OdbcException odbcException)
         {
            job.Message = odbcException.Message;
            job.Status = StatusType.Failed;
         }
          */ 
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void ExecuteO2dbcFillSharedPoolQuery(Job job)
      {
         /*
         OracleDataAdapter _o2dbcDataAdapter = job.Input as OracleDataAdapter;

         try
         {
            DataSet ds = new DataSet();
            _o2dbcDataAdapter.Fill(ds);

            #region Set TableName And Relations Into DataSet
            DataTable dt = ds.Tables[ds.Tables.Count - 1];
            DataRow row = dt.Rows[0];
            string[] TablesName = row["TablesName"].ToString().Split(':');

            int countTables = Convert.ToBoolean(row["hasRelation"]) ? ds.Tables.Count - 2 : ds.Tables.Count - 1;

            for (int i = 0; i < countTables; i++)
            {
               ds.Tables[i].TableName = TablesName[i];
            }

            // Set Relation Database Tables

            if (Convert.ToBoolean(row["hasRelation"]))
            {
               ds.Relations.Clear();
               foreach (DataRow item in ds.Tables[countTables].Rows)
               {
                  ds.Relations.Add(
                      item["RName"].ToString(),
                      ds.Tables[item["ParentTable"].ToString()].Columns[item["ParentKey"].ToString()],
                      ds.Tables[item["ForiegnTable"].ToString()].Columns[item["ForiegnKey"].ToString()],
                      false
                      );
               }
               ds.Tables.RemoveAt(ds.Tables.Count - 1);
            }

            ds.Tables.RemoveAt(ds.Tables.Count - 1);
            // Create Job for save run info in list for log and show user. :D ;)
            #endregion
            job.Output = ds;
            job.Status = StatusType.Successful;
         }
         catch (OdbcException odbcException)
         {
            job.Message = odbcException.Message;
            job.Status = StatusType.Failed;
         }
          */
      }
   }
}
