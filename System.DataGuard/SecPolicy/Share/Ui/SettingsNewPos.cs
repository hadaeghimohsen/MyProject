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
using DevExpress.XtraEditors;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsNewPos : UserControl
   {
      public SettingsNewPos()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }            
         );
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         if (Pos_Device != null)
         {
            PosBs.DataSource = iProject.Pos_Devices.FirstOrDefault(p => p.PSID == Pos_Device.PSID);
         }
         else
         {
            PosBs.List.Clear();
            PosBs.AddNew();
         }
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;           

            iProject.SavePosDevice(
               new XElement("Pos",
                  new XAttribute("psid", pos == null ? 0 : pos.PSID),
                  new XAttribute("banktype", BankType_Lov.EditValue),
                  new XAttribute("bnkbcode", BnkbCode_Txt.Text),
                  new XAttribute("bnkaacntnumb", BnkaAcntNumb_Txt.Text),
                  new XAttribute("shbacode", ShbaCode_Txt.Text),
                  new XAttribute("posdesc", PosDesc_Txt.Text),
                  new XAttribute("posstat", PosStat_Lov.EditValue),
                  new XAttribute("posdflt", PosDfltStat_Lov.EditValue),
                  new XAttribute("poscncttype", PosCnctType_Lov.EditValue),
                  new XAttribute("ipadrs", IPAdrs_Txt.Text),
                  new XAttribute("commport", ComPortName_Txt.Text),
                  new XAttribute("bandrate", BandRate_Txt.Text),
                  new XAttribute("prntsale", PrntSale_Txt.Text),
                  new XAttribute("prntcust", PrntCust_Txt.Text),
                  new XAttribute("autocomm", AutoComm_Lov.EditValue),
                  new XAttribute("gtwymacadrs", Comp_Lov.EditValue),
                  new XAttribute("billno", BillNo_Txt.EditValue),
                  new XAttribute("actntype", ActnType_Lov.EditValue),
                  new XAttribute("billfindtype", BillFindType_Lov.EditValue)
               )
            );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "SettingsDevice", 10 /* Execute ActionCallWindows */){Input = "Pos_Butn"}
                  }
               )
            );
            Back_Butn_Click(null, null);

            UserAccessPos_Gv.PostEditor();
            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ComPortName_Lov_SelectedIndexChanged(object sender, EventArgs e)
      {
         ComPortName_Txt.Text = ComPortName_Lov.Text;
      }

      private void GetHost_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self)
            {
               AfterChangedOutput =
                  new Action<object>(
                     (output) =>
                     {
                        var hostinfo = output as XElement;
                        Comp_Lov.EditValue = hostinfo.Attribute("cpu").Value;
                     }
                  )
            }
         );
      }
   }
}
