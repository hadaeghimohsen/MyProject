﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Setup.Code
{
   partial class Setup : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "DefaultGateway":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
               break;
            default:
               jobs.Status = StatusType.Failed;
               break;
         }
      }

      protected override void InternalAssistance(Job job)
      {
         switch (job.Method)
         {
            case 01:
               GetUi(job);
               break;
            case 02:
               Frst_Page_F(job);
               break;
            case 03:
               Chk_Licn_F(job);
               break;
            case 04:
               Sql_Conf_F(job);
               break;
            case 05:
               Chk_Tiny_F(job);
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
            case "Wall":
               _Wall.SendRequest(job);
               break;
            case "FRST_PAGE_F":
               _Frst_Page_F.SendRequest(job);
               break;
            case "CHK_LICN_F":
               _Chk_Licn_F.SendRequest(job);
               break;
            case "SQL_CONF_F":
               _Sql_Conf_F.SendRequest(job);
               break;
            case "CHK_TINY_F":
               _Chk_Tiny_F.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
