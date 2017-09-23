using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Odbccfg
{
    partial class Odbc : IMP
    {
        protected override void ExternalAssistance(Job jobs)
        {
            switch (jobs.Gateway)
            {
                case "Command":
                    _Command.Gateway(jobs);
                    break;
                case "Transaction":
                    _Transaction.Gateway(jobs);
                    break;
                case "DefaultGateway":
                    _DefaultGateway.Gateway(jobs);
                    break;
            }
        }

        protected override void InternalAssistance(Job job)
        {
            switch (job.Method)
            {
                case 01:
                    BeginTransaction(job);
                    break;
                case 02:
                    CommandTypeValidation(job);
                    break;
                case 03:
                    InputParameterValidation(job);
                    break;
                case 04:
                    CommandNameValidation(job);
                    break;
                case 05:
                    DsnNameValidation(job);
                    break;
                case 06:
                    UserNameValidation(job);
                    break;
               case 07:
                    Import2Odbc(job);
                    break;
               default:
                    job.Status = StatusType.Failed;
                    break;
            }
        }

        protected override void RequestToUserInterface(Job job)
        {
            switch (job.Gateway)
            {
                case "OdbcSettings":
                    _OdbcSettings.RequestToUserInterface(job);
                    break;
                case "Build":
                    _Build.RequestToUserInterface(job);
                    break;
                case "ChooseCommandType":
                    _ChooseCommandType.RequestToUserInterface(job);
                    break;
                case "SetInputParameter":
                    _SetInputParameter.RequestToUserInterface(job);
                    break;
                case "ExecuteCommandName":
                    _ExecuteCommandName.RequestToUserInterface(job);
                    break;
                case "ExecuteDnsName":
                    _ExecureDsnName.RequestToUserInterface(job);
                    break;
                case "ExecuteUserName":
                    _ExecuteUserName.RequestToUserInterface(job);
                    break;
            }
        }
    }
}
