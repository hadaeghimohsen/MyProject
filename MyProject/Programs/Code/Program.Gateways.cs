using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using DataGuard = System.DataGuard;
using ServiceDef = System.ServiceDefinition;
using Reporting = System.Reporting;
using Gas = System.Gas;
using Scsc = System.Scsc;
using Sas = System.Emis.Sas;
using Msgb = System.MessageBroadcast;
using ISP = System.ISP;
using CRM = System.CRM;
using RoboTech = System.RoboTech;
using Accounting = System.Accounting;

namespace MyProject.Programs.Code
{
   partial class Program
   {
      internal Commons.Code.Commons _Commons { get; set; }

      internal DataGuard.Self.Code.DataGuard _DataGuard { get; set; }
      internal ServiceDef.Share.Code.Services _ServiceDefinition { get; set; }
      internal Reporting.Self.Code.Reporting _Reporting { get; set; }
      internal Gas.Self.Code.Gas _Gas { get; set; }
      internal Scsc.Code.Scsc _Scsc { get; set; }
      internal Sas.Controller.Sas _Sas { get; set; }
      internal Msgb.Code.Msgb _Msgb { get; set; }
      internal ISP.Code.ISP _ISP { get; set; }
      internal CRM.Code.CRM _CRM { get; set; }
      internal RoboTech.Code.RoboTech _RoboTech { get; set; }
      internal Accounting.Code.Accounting _Accounting { get; set; }
   }
}
