using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.RoboTech.ExceptionHandlings;
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.RoboTech.ExtCode;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class ALPK_DVLP_F : UserControl
   {
      public ALPK_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long? ordrcode;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         OrdrBs.DataSource =
            iRoboTech.Orders.Where(o => o.CODE == ordrcode);

         var ordr = OrdrBs.Current as Data.Order;

         JobBs.DataSource = iRoboTech.Jobs.Where(j => j.ROBO_RBID == ordr.SRBT_ROBO_RBID && j.ORDR_TYPE == "019");

         TotlAlpk_Lb.Text = PrjbBs.List.OfType<Data.Personal_Robot_Job>().Where(p => p.STAT == "002").Count().ToString();

         requery = false;
      }

      private void SaveAlpk_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if(ordr == null)return;

            var rows = Prjb_Gv.GetSelectedRows();
            List<long?> chatids = new List<long?>();
            foreach (var r in rows)
            {
               var row = (Data.Personal_Robot_Job)Prjb_Gv.GetRow(r);
               chatids.Add(row.Personal_Robot.CHAT_ID);
            }
            var xRet = new XElement("Result");

            iRoboTech.SAVE_ALPK_P(
               new XElement("RequestAlopeyk",
                  new XAttribute("rbid", ordr.SRBT_ROBO_RBID),
                  new XAttribute("ordrcode", ordr.CODE),
                  new XAttribute("actncode", "001"),
                  new XAttribute("expnamnt", ExpnAmnt_Txt.EditValue ?? 0),
                  new XAttribute("amnttype", ordr.AMNT_TYPE),
                  chatids.Select(
                     r =>
                        new XElement("Alopeyk", 
                           new XAttribute("chatid", r)
                        )
                  )
               ),
               ref xRet
            );

            // فراخوانی ربات برای ارسال پیام ثبت شده به سفیران انتخاب شده
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Robot", 
                              new XAttribute("runrobot", "start"),
                              new XAttribute("actntype", "sendordrs"),
                              //new XAttribute("keypad", "inline"),
                              //new XAttribute("command", "getalopyks"),
                              new XAttribute("rbid", ordr.SRBT_ROBO_RBID)
                           )
                     }                     
                  }
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Prjb_Gv_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
      {
         try
         {
            SlctAlpk_Lb.Text = Prjb_Gv.GetSelectedRows().Count().ToString();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowDestGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if (ordr == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
               {
                  Input =
                     new XElement("GMapNets",
                        new XAttribute("requesttype", "get"),
                        new XAttribute("formcaller", "Program:RoboTech:" + GetType().Name),
                        new XAttribute("callback", 40 /* CordinateGetSet */),
                        new XAttribute("outputtype", "destpostadrs"),
                        new XAttribute("initalset", true),
                        new XAttribute("cordx", ordr.CORD_X == null ? "29.610420210528" : ordr.CORD_X.ToString()),
                        new XAttribute("cordy", ordr.CORD_Y == null ? "52.5152599811554" : ordr.CORD_Y.ToString()),
                        new XAttribute("zoom", "1600")
                     )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveOrdrAdrs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            OrdrBs.EndEdit();

            iRoboTech.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }

      }

      private void ShowSorcGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = OrdrBs.Current as Data.Order;
            if (ordr == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
               {
                  Input =
                     new XElement("GMapNets",
                        new XAttribute("requesttype", "get"),
                        new XAttribute("formcaller", "Program:RoboTech:" + GetType().Name),
                        new XAttribute("callback", 40 /* CordinateGetSet */),
                        new XAttribute("outputtype", "sorcpostadrs"),
                        new XAttribute("initalset", true),
                        new XAttribute("cordx", ordr.SORC_CORD_X == null ? "29.610420210528" : ordr.SORC_CORD_X.ToString()),
                        new XAttribute("cordy", ordr.SORC_CORD_Y == null ? "52.5152599811554" : ordr.SORC_CORD_Y.ToString()),
                        new XAttribute("zoom", "1600")
                     )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SelectAll_Butn_Click(object sender, EventArgs e)
      {
         Prjb_Gv.SelectAll();
      }

      private void Deselect_Butn_Click(object sender, EventArgs e)
      {
         Prjb_Gv.ClearSelection();
      }

      private void ExpnAmnt_Txt_TextChanged(object sender, EventArgs e)
      {
         try
         {
            ExpnAmntDesc_Lb.Text = ExpnAmnt_Txt.Text.Replace(",", "").Num2Str() + " " + AmntType_Lov.Text;
         }
         catch(Exception exc)
         {
            ExpnAmntDesc_Lb.Text = exc.Message;
         }
      }
   }
}
