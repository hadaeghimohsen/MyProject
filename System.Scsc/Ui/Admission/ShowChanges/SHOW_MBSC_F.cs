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

      private async void Execute_Query()
      {
         try
         {
            var result = await Task.Run(() =>
            {
               using (var iScsc = new Data.iScscDataContext(ConnectionString))
               {
                  var cbmtList = iScsc.Club_Methods.Where(cm => cm.MTOD_STAT == "002").ToList();

                  var mbsp004 = iScsc.Member_Ships.FirstOrDefault(mb => mb.RQRO_RQST_RQID == rqid && mb.RECT_CODE == "004" && mb.FIGH_FILE_NO == fileno);
                  var mbsp004Current = mbsp004;
                  var cbmt004 = mbsp004Current.Fighter_Public.Club_Method;

                  var cochName002 = cbmt004.Fighter.NAME_DNRM;
                  var mtodDesc002 = mbsp004Current.Fighter_Public.Method.MTOD_DESC;
                  var ctgyDesc002 = mbsp004Current.Fighter_Public.Category_Belt.CTGY_DESC;
                  var strtTime002 = mbsp004Current.STRT_DATE.Value.ToShortTimeString();
                  var endTime002 = mbsp004Current.END_DATE.Value.ToShortTimeString();

                  var mbsp002 = iScsc.Member_Ships.FirstOrDefault(mb => mb.RQRO_RQST_RQID == rqid && mb.RECT_CODE == "001" && mb.FIGH_FILE_NO == fileno);
                  var mbsp002Current = mbsp002;
                  var cbmt002 = mbsp002Current.Fighter_Public.Club_Method;

                  var cochName = cbmt002.Fighter.NAME_DNRM;
                  var mtodDesc = mbsp002.Method.MTOD_DESC;
                  var ctgyDesc = mbsp002.Category_Belt.CTGY_DESC;
                  var strtTime = mbsp002.STRT_DATE.Value.ToShortTimeString();
                  var endTime = mbsp002.END_DATE.Value.ToShortTimeString();

                  return new
                  {
                     cbmtList,
                     mbsp004, cochName002, mtodDesc002, ctgyDesc002, strtTime002, endTime002,
                     mbsp002, cochName, mtodDesc, ctgyDesc, strtTime, endTime
                  };
               }
            });

            iScsc = new Data.iScscDataContext(ConnectionString);
            CbmtBs1.DataSource = result.cbmtList;

            Mbsp004Bs.DataSource = result.mbsp004;
            CochName002_Txt.EditValue = result.cochName002;
            MtodDesc002_Txt.EditValue = result.mtodDesc002;
            CtgyDesc002_Txt.EditValue = result.ctgyDesc002;
            StrtTime002_Txt.EditValue = result.strtTime002;
            EndTime002_Txt.EditValue = result.endTime002;

            Mbsp002Bs.DataSource = result.mbsp002;
            CochName_Txt.EditValue = result.cochName;
            MtodDesc_Txt.EditValue = result.mtodDesc;
            CtgyDesc_Txt.EditValue = result.ctgyDesc;
            StrtTime_Txt.EditValue = result.strtTime;
            EndTime_Txt.EditValue = result.endTime;
         }
         catch (Exception ) { }
         requery = false;
      }
   }
}
