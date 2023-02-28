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
using System.Xml.Linq;
using System.IO;
using System.Scsc.ExtCode;

namespace System.Scsc.Ui.Admission.ShowChanges
{
   public partial class SHOW_MBSC_F : UserControl
   {
      public SHOW_MBSC_F()
      {
         InitializeComponent();
      }

      bool requery = false;
      long fileno, rqid;

      private void Back_Btn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            Mbsp001Bs.DataSource = iScsc.Member_Ships.FirstOrDefault(mb => mb.RQRO_RQST_RQID == rqid && mb.RECT_CODE == "001" && mb.FIGH_FILE_NO == fileno);
            var mbsp = Mbsp001Bs.Current as Data.Member_Ship;

            var cbmt = mbsp.Fighter_Public.Club_Method;
            CochName_Txt.EditValue = cbmt.Fighter.NAME_DNRM;
            MtodDesc_Txt.EditValue = mbsp.Fighter_Public.Method.MTOD_DESC;
            CtgyDesc_Txt.EditValue = mbsp.Fighter_Public.Category_Belt.CTGY_DESC;
            StrtTime_Txt.EditValue = cbmt.STRT_TIME.ToString().Substring(0, 5);
            EndTime_Txt.EditValue = cbmt.END_TIME.ToString().Substring(0, 5);

            CbmtBs1.DataSource = iScsc.Club_Methods.Where(cm => cm.MTOD_STAT == "002" /*&& cm.MTOD_CODE == mbsp.Fighter_Public.MTOD_CODE*/);

            Mbsp002Bs.DataSource = iScsc.Member_Ships.FirstOrDefault(mb => mb.RQRO_RQST_RQID == rqid && mb.RECT_CODE == "002" && mb.FIGH_FILE_NO == fileno);
            mbsp = Mbsp002Bs.Current as Data.Member_Ship;

            cbmt = mbsp.Fighter_Public.Club_Method;
            CochName002_Txt.EditValue = cbmt.Fighter.NAME_DNRM;
            MtodDesc002_Txt.EditValue = mbsp.Fighter_Public.Method.MTOD_DESC;
            CtgyDesc002_Txt.EditValue = mbsp.Fighter_Public.Category_Belt.CTGY_DESC;
            StrtTime002_Txt.EditValue = cbmt.STRT_TIME.ToString().Substring(0, 5);
            EndTime002_Txt.EditValue = cbmt.END_TIME.ToString().Substring(0, 5);
         }
         catch (Exception ) { }
         requery = false;
      }

   }
}
