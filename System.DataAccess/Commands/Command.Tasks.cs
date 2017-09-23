using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.JobRouting.Jobs;
using System.Linq;

namespace System.DataAccess.Commands
{
    partial class Command
    {
        /*
         * Index of method code is between 1 and 100
         */

        /// <summary>
        /// Code 01
        /// </summary>
        /// <param name="job"></param>
        /// Command Name
        /// Has Parameter
        /// Parameter Type
        /// Parameter Value
        private void CreateOdbcCommand(Job job)
        {
            string _dsnName = (string)(job.Input as List<object>)[0];
            string _userName = (string)(job.Input as List<object>)[1];
            string _password = (string)(job.Input as List<object>)[2];
            string _commandName = (string)(job.Input as List<object>)[3];
            bool   _hasParameter = (bool)(job.Input as List<object>)[4];
            string _parameterType = (string)(job.Input as List<object>)[5];
            object _parameterValue = (job.Input as List<object>)[6];

            // First step get create odbc connection string
            // Hossein Gholami, Mohsen Hadaeghi, Mohammad Ebrahim Keliddar Mohammadi, Hesam Forotan
            // HGMHMEKMHF
            // M3GFH3EK
            string _connectionString = string.Format("dsn={0};uid={1};pwd={2};", _dsnName, _userName, _password == "" ? "abcABC123!@#" : _password);
            //Job _CreateConnectionString = new Job(SendType.External, "ConnectionString",
            //    new List<Job>
            //    {
            //        new Job(SendType.Self, 01 /* Execute CreateConnectionString */)
            //        {
            //            AfterChangedOutput = new Action<object>((output) => _connectionString = output.ToString())
            //        }
            //    });
            //Manager(_CreateConnectionString);

            if (_hasParameter)
            {
                if (_parameterType == "dataset")
                {
                    Func<DataSet, string> _convertDataSet2XmlString = delegate(DataSet dataset)
                    {
                        StringWriter sw = new StringWriter();
                        dataset.WriteXml(sw);
                        return sw.ToString();
                    };

                    _parameterValue = _convertDataSet2XmlString.Invoke(_parameterValue as DataSet);
                }
            }

            OdbcCommand _odbcCommand = new OdbcCommand(_commandName, new OdbcConnection(_connectionString)) { CommandType = CommandType.StoredProcedure };
            if (_hasParameter)
            {
                OdbcParameter _odbcParameter = new OdbcParameter("@XmlArg", _parameterValue) { OdbcType = OdbcType.NText, Direction = ParameterDirection.Input };
                _odbcCommand.Parameters.Add(_odbcParameter);
            }
            job.Output = _odbcCommand;
            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 02
        /// </summary>
        /// <param name="job"></param>
        /// Command Name
        /// Has Parameter
        /// Parameter Type
        /// Parameter Value
        private void CreateO2dbcCommand(Job job)
        {
           /*
            string _tnsName = (string)(job.Input as List<object>)[0];
            string _userName = (string)(job.Input as List<object>)[1];
            string _commandName = (string)(job.Input as List<object>)[2];
            bool   _hasParameter = (bool)(job.Input as List<object>)[3];
            string _parameterType = (string)(job.Input as List<object>)[4];
            object _parameterValue = (job.Input as List<object>)[5];

            string _connectionString = string.Format("Data source={0};Persist Security Info=True;User ID={1};Password=abc123!@#;", _tnsName, _userName);

            if (_hasParameter)
            {
                if (_parameterType == "dataset")
                {
                    Func<DataSet, string> _convertDataSet2XmlString = delegate(DataSet dataset)
                    {
                        StringWriter sw = new StringWriter();
                        dataset.WriteXml(sw);
                        return sw.ToString();
                    };
                    _parameterValue = _convertDataSet2XmlString.Invoke(_parameterValue as DataSet);
                }
            }

            OracleCommand _oracleCommand = new OracleCommand(_commandName, new OracleConnection(_connectionString)) { CommandType = CommandType.StoredProcedure };
            //OracleParameter _oracleParameter = new OracleParameter("@XmlArg", _parameterValue) { OracleDbType = OracleDbType.XmlType, Direction = ParameterDirection.Input };
            //_oracleCommand.Parameters.Add(_oracleParameter);

            job.Output = _oracleCommand;
            job.Status = StatusType.Successful;
            */
        }
    }
}
