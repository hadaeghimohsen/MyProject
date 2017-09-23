using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Data;

namespace System.Emis.Sas.Model
{
   public class Access_Entity
   {
      private string ConnectionString;
      private OracleDataAdapter OraDA;
      private DataSet OraPool;
      private  OracleCommand OraCmd;
      public Access_Entity(string connectionString)
      {
         ConnectionString = connectionString;
         OraDA =
               new OracleDataAdapter(
                  new OracleCommand("SELECT SYSDATE FROM DUAL",
                     new OracleConnection(ConnectionString)
                     ) { CommandType = CommandType.Text }
               );
         OraPool = new DataSet();
      }

      public DataTable Run_Qury_U(string qury)
      {
         try
         {
            OraPool.Tables.Clear();
            OraDA.SelectCommand.CommandText = qury;
            OraDA.Fill(OraPool);
            return OraPool.Tables[0];
         }
         catch
         {
            return null;
         }
      }

      public int Run_Blok_U(string blokstoreprocedure)
      {
         try
         {
            OraCmd = OraDA.SelectCommand;
            OraCmd.CommandText = blokstoreprocedure;
            OraCmd.CommandType = CommandType.Text;
            OraCmd.Connection.Open();
            int x = OraCmd.ExecuteNonQuery();
            OraCmd.Connection.Close();
            return x;
         }
         catch (OracleException oe)
         {
            System.Windows.Forms.MessageBox.Show(oe.Message);
            return -1;
         }
      }
   }
}
