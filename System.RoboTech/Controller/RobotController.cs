using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.RoboTech.Controller
{
   public class RobotController
   {
      public XElement GetData(XElement x, string connectionString)
      {
         try
         {
            Data.iRoboTechDataContext iRobot = new Data.iRoboTechDataContext(connectionString);
            iRobot.CommandTimeout = 18000;
            XElement xResult = new XElement("Respons", "0");
            iRobot.Analisis_Message_P(x, ref xResult);
            return xResult;
         }
         catch (Exception exc)
         {
            return XDocument.Parse(string.Format("<Respons actncode=\"-1000\"><Message>{0}</Message></Respons>", exc.Message)).Root;
         }
      }
   }
}
