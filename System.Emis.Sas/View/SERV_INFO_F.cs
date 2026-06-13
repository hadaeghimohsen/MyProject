using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Emis.Sas.View
{
   public partial class SERV_INFO_F : UserControl
   {
      public SERV_INFO_F()
      {
         InitializeComponent();
      }

      private void SAS_PUBLICBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         if (SAS_PUBLICBindingSource.Position == SAS_PUBLICBindingSource.Count - 1)
            RWNOTextBox.BackColor = Color.GreenYellow;
         else
            RWNOTextBox.BackColor = SystemColors.Window;

         DataRowView saspublic = SAS_PUBLICBindingSource.Current as DataRowView;
         

         BASE_METER_SPECBindingSource.DataSource = access_entity.Run_Qury_U("SELECT * FROM BASE_METER_SPEC WHERE Serv_File_No = :p0 AND Rect_Code = 6 AND Rwno = :p1", new OracleParameter[] { new OracleParameter(":p0", FileNo), new OracleParameter(":p1", saspublic["BMSP_RWNO_DNRM"]) });
          BASE_METER_TARIFFBindingSource.DataSource = access_entity.Run_Qury_U("SELECT * FROM BASE_METER_TARIFF WHERE Bmsp_Serv_File_No = :p0 AND Bmsp_Rwno = :p1 AND Bmsp_Rect_Code = 6", new OracleParameter[] { new OracleParameter(":p0", FileNo), new OracleParameter(":p1", saspublic["Bmsp_Rwno_Dnrm"]) });
          METER_SPECBindingSource.DataSource = access_entity.Run_Qury_U("SELECT * FROM METER_SPEC WHERE Serv_File_No = :p0 AND Rect_Code = 6 AND Rwno = :p1", new OracleParameter[] { new OracleParameter(":p0", FileNo), new OracleParameter(":p1", saspublic["MSPC_RWNO_DNRM"]) });

          if (RQRO_RQST_RQIDTextBox.Text.Trim().Length == 0) { Pblc_RqtpDesc.Text = "";}
          else
          {
             Pblc_RqtpDesc.Text = access_entity.Run_Qury_U("SELECT Rt.* FROM Request R, Request_Type Rt WHERE R.Rqid = :p0 AND R.Rqtp_Code = Rt.Code", new OracleParameter[] { new OracleParameter(":p0", RQRO_RQST_RQIDTextBox.Text) }).Rows[0]["RQTP_DESC"].ToString();
          }

          if (RQRO_RQST_RQIDTextBox1.Text.Trim().Length == 0) { RQTP_DESCTextBox.Text = ""; }
          else
          {
             RQTP_DESCTextBox.Text = access_entity.Run_Qury_U("SELECT Rt.* FROM Request R, Request_Type Rt WHERE R.Rqid = :p0 AND R.Rqtp_Code = Rt.Code", new OracleParameter[] { new OracleParameter(":p0", RQRO_RQST_RQIDTextBox1.Text) }).Rows[0]["RQTP_DESC"].ToString();
          }

          if (RQRO_RQST_RQIDTextBox2.Text.Trim().Length == 0) { MSPC_RQTPDESC.Text = ""; }
          else
          {
             MSPC_RQTPDESC.Text = access_entity.Run_Qury_U("SELECT Rt.* FROM Request R, Request_Type Rt WHERE R.Rqid = :p0 AND R.Rqtp_Code = Rt.Code", new OracleParameter[] { new OracleParameter(":p0", RQRO_RQST_RQIDTextBox2.Text) }).Rows[0]["RQTP_DESC"].ToString();
          }

      }

      private void SERVICE_TARIFFBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         if (SERVICE_TARIFFBindingSource.Position == SERVICE_TARIFFBindingSource.Count - 1)
            RWNOTextBox3.BackColor = Color.GreenYellow;
         else
            RWNOTextBox3.BackColor = SystemColors.Window;

         DataRowView strf = SERVICE_TARIFFBindingSource.Current as DataRowView;
         if (strf != null)
             BIL_TARIFFBindingSource.DataSource = access_entity.Run_Qury_U("SELECT * FROM Bil_Tariff WHERE Tfid = :p0", new OracleParameter[] { new OracleParameter(":p0", strf["Tarf_Tfid"]) });
      }

   }
}
