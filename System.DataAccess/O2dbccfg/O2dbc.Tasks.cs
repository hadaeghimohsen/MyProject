using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.O2dbccfg
{
    partial class O2dbc
    {
        /*
         * Index of method code is between 1 and 100
         */

        /// <summary>
        /// Code 1
        /// </summary>
        /// <param name="job"></param>
        /// <input>
        ///     <RequiresValidation value="(true | false)">
        ///     0- Command requires validation
        ///     </RequiresValidation>
        ///     <CommandType value="(procedure | func)">
        ///     1- Store procedure or function
        ///     </CommandType>
        ///     <SendInputParamater value="(true | false)">
        ///     2- object value input parameter
        ///     </SendInputParamater> 
        ///     <GetResultQuery value="(true | false)">
        ///     3- have result query
        ///     </GetResultQuery>
        ///     <ParameterType value="(dataset | xml | sqlquery)">
        ///     4- (DataSet) or (xml string) or (sql query)
        ///     </ParameterType>
        ///     <Parameter value="(object | string)">
        ///     5- Dataset or xml string or sql query
        ///     </Parameter>
        ///     <CommandName value="string">
        ///     6- Store Procedure or function name
        ///     </CommandName>
        ///     <DsnName>
        ///     7 - Dsn Name
        ///     </DsnName>
        ///     <UserName>
        ///     8 - User Name
        ///     </UserName>
        /// </input>
        private void BeginTransaction(Job job)
        {
            bool RequiresValidation = (bool)(job.Input as List<object>)[0];

            string _commandType = (string)(job.Input as List<object>)[1];
            bool _getResultQuery = (bool)(job.Input as List<object>)[3];

            #region Requiers Validation
            if (RequiresValidation)
            {
                #region Command Type Validation
                Job _CommandTypeValidation = new Job(SendType.Self, 02 /* Execute CommandTypeValidation */)
                {
                    Input = (job.Input as List<object>)[1].ToString(),
                    AfterChangedOutput = new Action<object>((output) => (job.Input as List<object>)[1] = output)
                };
                Manager(_CommandTypeValidation);
                #endregion

                #region Input Parameter Validation
                Job _InputParameterValidation = new Job(SendType.Self, 03 /* Execute InputParameterValidation */)
                {
                    Input = new List<object>
                        {
                            (job.Input as List<object>)[2], // Check has parameter
                            (job.Input as List<object>)[4], // input parameter type
                            (job.Input as List<object>)[5], // input parameter value
                        },
                    AfterChangedOutput = new Action<object>((output) =>
                    {
                        List<object> OUTPUT = output as List<object>;
                        (job.Input as List<object>)[2] = OUTPUT[0]; // Check has parameter
                        (job.Input as List<object>)[4] = OUTPUT[1]; // input parameter type
                        (job.Input as List<object>)[5] = OUTPUT[2]; // input parameter value
                    })
                };
                Manager(_InputParameterValidation);
                #endregion

                #region Command Name Validation
                Job _CommandNameValidation = new Job(SendType.Self, 04 /* Execute CommandNameValidation */)
                {
                    Input = (job.Input as List<object>)[6].ToString(),
                    AfterChangedOutput = new Action<object>((output) => (job.Input as List<object>)[6] = output)
                };
                Manager(_CommandNameValidation);
                #endregion

                #region TNS Name Validation
                Job _TnsNameValidation = new Job(SendType.Self, 05 /* Execute TnsNameValidation */)
                {
                    Input = (job.Input as List<object>)[7].ToString(),
                    AfterChangedOutput = new Action<object>((output) => (job.Input as List<object>)[7] = output)
                };
                Manager(_TnsNameValidation);
                #endregion

                #region User Name Validation
                Job _UserNameValidation = new Job(SendType.Self, 06 /* Execute UserNameValidation */)
                {
                    Input = (job.Input as List<object>)[8].ToString(),
                    AfterChangedOutput = new Action<object>((output) => (job.Input as List<object>)[8] = output)
                };
                Manager(_UserNameValidation);
                #endregion
            }
            #endregion

            #region Create Command
            Job _CreateCommand = new Job(SendType.External, "Command",
                new List<Job>
                {
                    new Job(SendType.Self, 02 /* Execute CreateO2dbcCommand */)
                    {
                        Input = new List<object>
                        {
                            (job.Input as List<object>)[7], // TNS Name
                            (job.Input as List<object>)[8], // User Name
                            (job.Input as List<object>)[6], // Command Name
                            (job.Input as List<object>)[2], // Has Parameter 
                            (job.Input as List<object>)[4], // Parameter Type
                            (job.Input as List<object>)[5]  // Parameter Value
                        }
                    }
                });
            Manager(_CreateCommand);
            #endregion

            #region Execute Transaction
            string _executeType = "ExecuteNonQuery";

            switch (_commandType)
            {
                case "procedure":
                    if (_getResultQuery)
                        _executeType = "FillSharedPoolQuery";
                    break;
                default:
                    _executeType = "ExecuteScalar";
                    break;
            }

            Job _ExecuteTransaction = new Job(SendType.External, "Transaction",
                new List<Job>
                {
                    new Job(SendType.Self, 05 /* Execute ExecuteO2dbcTransaction */)
                    {
                        Input = new List<object>
                        {
                            _executeType,
                            _CreateCommand.Output
                        }
                    }
                });
            Manager(_ExecuteTransaction);
            #endregion

            job.Output = _ExecuteTransaction.Output;
            job.Status = _ExecuteTransaction.Status;
        }

        /// <summary>
        /// Code 02
        /// </summary>
        /// <param name="job"></param>
        private void CommandTypeValidation(Job job)
        {
            if (job.Input.ToString().Trim().Length == 0)
            {
                /* Show Command type dialog for user */
                goto ShowDialog;
            }
            if (!(new List<string> { "procedure", "func" }).Contains(job.Input.ToString().ToLower()))
            {
                /* Show Command type dialog for user */
                goto ShowDialog;
            }
            // No syntax error
            job.Status = StatusType.Successful;
            return;

        ShowDialog:
            Job _ShowDialog = new Job(SendType.SelfToUserInterface, "ChooseCommandType", 01 /* Execute ShowDialog */)
            {
                AfterChangedOutput = new Action<object>((output) => job.Output = output)
            };
            Manager(_ShowDialog);
        }

        /// <summary>
        /// Code 03
        /// </summary>
        /// <param name="job"></param>
        private void InputParameterValidation(Job job)
        {
            bool val1 = (bool)(job.Input as List<object>)[0];
            string val2 = (string)(job.Input as List<object>)[1];
            object val3 = (job.Input as List<object>)[2];

            if (val1)
            {
                if (!(new List<string> { "xml", "sqlquery", "dataset" }).Contains(val2))
                {
                    /* Show Command type dialog for user */
                    goto ShowDialog;
                }
                if (val3.ToString().Trim().Length == 0 || val3 == null)
                {
                    /* Show Command type dialog for user */
                    goto ShowDialog;
                }
            }
            job.Status = StatusType.Successful;
            return;
        ShowDialog:
            Job _ShowDialog = new Job(SendType.SelfToUserInterface, "SetInputParameter", 01)
            {
                Input = job.Input,
                AfterChangedOutput = new Action<object>((output) => job.Output = output)
            };
            Manager(_ShowDialog);
        }

        /// <summary>
        /// Code 04
        /// </summary>
        /// <param name="job"></param>
        private void CommandNameValidation(Job job)
        {
            if (job.Input.ToString().Trim().Length == 0)
            {
                /* Show Command type dialog for user */
                goto ShowDialog;
            }

            // No syntax error
            job.Status = StatusType.Successful;
            return;

        ShowDialog:
            Job _ShowDialog = new Job(SendType.SelfToUserInterface, "ExecuteCommandName", 01 /* Execute ShowDialog */)
            {
                AfterChangedOutput = new Action<object>((output) => job.Output = output)
            };
            Manager(_ShowDialog);
        }

        /// <summary>
        /// Code 05
        /// </summary>
        /// <param name="job"></param>
        private void TnsNameValidation(Job job)
        {
            if (job.Input.ToString().Trim().Length == 0)
            {
                /* Show TNS Name dialog for user */
                goto ShowDialog;
            }

            // No syntax error
            job.Status = StatusType.Successful;
            return;

        ShowDialog:
            Job _ShowDialog = new Job(SendType.SelfToUserInterface, "ExecuteTNSName", 01 /* Execute ShowDialog */)
            {
                AfterChangedOutput = new Action<object>((output) => job.Output = output)
            };
            Manager(_ShowDialog);
        }

        /// <summary>
        /// Code 06
        /// </summary>
        /// <param name="job"></param>
        private void UserNameValidation(Job job)
        {
            if (job.Input.ToString().Trim().Length == 0)
            {
                /* Show User Name dialog for user */
                goto ShowDialog;
            }

            // No syntax error
            job.Status = StatusType.Successful;
            return;

        ShowDialog:
            Job _ShowDialog = new Job(SendType.SelfToUserInterface, "ExecuteUserName", 01 /* Execute ShowDialog */)
            {
                AfterChangedOutput = new Action<object>((output) => job.Output = output)
            };
            Manager(_ShowDialog);
        }
    }
}
