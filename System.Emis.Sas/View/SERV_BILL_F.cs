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
   public partial class SERV_BILL_F : UserControl
   {
      public SERV_BILL_F()
      {
         InitializeComponent();
      }

      private DataSet Parallel_Table = new DataSet();
      private void BILL_BindingSource_CurrentChanged(object sender, EventArgs e)
      {
         if (BILL_BindingSource.Current == null) return;
         DataRowView bill = BILL_BindingSource.Current as DataRowView;

         if (Parallel_Table.Tables.Count > 0)
            Parallel_Table.Tables.Clear();

         OraDA.SelectCommand.CommandText = string.Format("SELECT * FROM Bil_Consumption WHERE Bill_Serv_File_No = {0} AND Bill_Bill_No = {1} ORDER BY BMTR_METR_TARF_TYPE, BMTR_DAY_TYPE", bill.Row["SERV_FILE_NO"], bill.Row["BILL_NO"]);
         OraDA.Fill(Parallel_Table);
         Parallel_Table.Tables[0].TableName = "BIL_CONSUMPTION";

         BIL_CONSUMPTION_BindingSource.DataSource = Parallel_Table.Tables["BIL_CONSUMPTION"];

         /**/         

         OraDA.SelectCommand.CommandText = string.Format("SELECT * FROM BIL_BILL_DEBIT WHERE Bill_Serv_File_No = {0} AND Bill_Bill_No = {1}", bill.Row["SERV_FILE_NO"], bill.Row["BILL_NO"]);
         OraDA.Fill(Parallel_Table);
         Parallel_Table.Tables[1].TableName = "BIL_BILL_DEBIT";

         BIL_BILL_DEBIT_BindingSource.DataSource = Parallel_Table.Tables["BIL_BILL_DEBIT"];

         /**/

         OraDA.SelectCommand.CommandText = string.Format("SELECT * FROM BIL_BILL_AMOUNT WHERE Bill_Serv_File_No = {0} AND Bill_Bill_No = {1}", bill.Row["SERV_FILE_NO"], bill.Row["BILL_NO"]);
         OraDA.Fill(Parallel_Table);
         Parallel_Table.Tables[2].TableName = "BIL_BILL_AMOUNT";

         BIL_BILL_AMOUNT_BindingSource.DataSource = Parallel_Table.Tables["BIL_BILL_AMOUNT"];
      }

      private DataSet Request_Type = new DataSet();
      private void RQRO_RQST_RQIDTextBox_TextChanged(object sender, EventArgs e)
      {
         if (RQRO_RQST_RQIDTextBox.Text.Trim().Length == 0) { RQTP_DESCTextBox.Text = ""; return; }

         if (Request_Type.Tables.Count > 0)
            Request_Type.Tables.Clear(); 

         OraDA.SelectCommand.CommandText = string.Format("SELECT Rt.* FROM Request R, Request_Type Rt WHERE R.Rqid = {0} AND R.Rqtp_Code = Rt.Code", RQRO_RQST_RQIDTextBox.Text);
         OraDA.Fill(Request_Type);
         Request_Type.Tables[0].TableName = "REQUEST_TYPE";

         //REQUEST_TYPE_BindingSource.DataSource = Request_Type.Tables["REQUEST_TYPE"];

         RQTP_DESCTextBox.Text = Request_Type.Tables["REQUEST_TYPE"].Rows[0]["RQTP_DESC"].ToString();
      }

      private DataSet Bil_Consumption_Detail = new DataSet();
      private void BIL_CONSUMPTION_BindingSource_CurrentChanged(object sender, EventArgs e)
      {
         if (BIL_CONSUMPTION_BindingSource.Current == null) return;
         DataRowView bill_cons = BIL_CONSUMPTION_BindingSource.Current as DataRowView;

         if (Bil_Consumption_Detail.Tables.Count > 0)
            Bil_Consumption_Detail.Tables.Clear();         

         OraDA.SelectCommand.CommandText = string.Format("SELECT * FROM Bil_Consumption_Detail WHERE Cons_Bill_Serv_File_No = {0} AND Cons_Bill_Bill_No = {1} AND CONS_BMTR_METR_TARF_TYPE = {2} AND CONS_BMTR_DAY_TYPE = {3} ORDER BY CONS_BMTR_METR_TARF_TYPE, CONS_BMTR_DAY_TYPE", bill_cons.Row["BILL_SERV_FILE_NO"], bill_cons.Row["BILL_BILL_NO"], bill_cons.Row["BMTR_METR_TARF_TYPE"], bill_cons.Row["BMTR_DAY_TYPE"]);
         OraDA.Fill(Bil_Consumption_Detail);
         Bil_Consumption_Detail.Tables[0].TableName = "BIL_CONSUMPTION_DETAIL";

         BIL_CONSUMPTION_DETAILBindingSource.DataSource = Bil_Consumption_Detail.Tables["BIL_CONSUMPTION_DETAIL"];
      }
   }
}
