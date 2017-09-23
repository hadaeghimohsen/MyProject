using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_WKDY_F : UserControl
   {
      public BAS_WKDY_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long code;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }      

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         CbmtBs1.DataSource = iScsc.Club_Methods.FirstOrDefault(cm => cm.CODE == code);
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var c = CbmtBs1.Current as Data.Club_Method;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                     new XElement("Update",
                        new XElement("Club_Method",
                           new XAttribute("code", c.CODE),
                           new XAttribute("clubcode", c.CLUB_CODE),
                           new XAttribute("mtodcode", c.MTOD_CODE),
                           new XAttribute("cochfileno", c.COCH_FILE_NO),
                           new XAttribute("daytype", c.DAY_TYPE),
                           new XAttribute("strttime", c.STRT_TIME.ToString()),
                           new XAttribute("endtime", c.END_TIME.ToString()),
                           new XAttribute("mtodstat", c.MTOD_STAT),
                           new XAttribute("sextype", c.SEX_TYPE),
                           new XElement("Club_Method_Weekdays",
                              CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().Select(cbmw =>
                                 new XElement("Club_Method_Weekday",
                                    new XAttribute("code", cbmw.CODE),
                                    new XAttribute("weekday", cbmw.WEEK_DAY),
                                    new XAttribute("stat", cbmw.STAT)
                                 )
                              )
                           )
                        )
                     )
               )
            );

            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("TabPage",
                                 new XAttribute("showtabpage", "tp_006")
                              )
                        }
                     }
                  )
               );
               Back_Butn_Click(null, null);
               requery = false;
            }
         }
      }

      private void CbmtBs1_CurrentChanged(object sender, EventArgs e)
      {
         var cbmt = CbmtBs1.Current as Data.Club_Method;
         if(cbmt == null)return;

         CbmtwkdyBs1.DataSource = cbmt.Club_Method_Weekdays.ToList();

         foreach (var wkdy in CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>())
         {
            var rslt = WeekDays_Flp.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag.ToString() == wkdy.WEEK_DAY);
            rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;
         }
      }

      private void Wkdy00i_Butn_Click(object sender, EventArgs e)
      {
         SimpleButton sb = sender as SimpleButton;         
         
         if(CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT == "001")
         {
            CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT = "002";
            sb.Appearance.BackColor = Color.GreenYellow;
         }
         else
         {
            CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT = "001";
            sb.Appearance.BackColor = Color.LightGray;
         }
      }
   }
}
