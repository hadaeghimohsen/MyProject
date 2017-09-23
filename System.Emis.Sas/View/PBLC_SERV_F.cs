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

namespace System.Emis.Sas.View
{
   public partial class PBLC_SERV_F : UserControl
   {
      public PBLC_SERV_F()
      {
         InitializeComponent();
      }

      private void INS_SERV_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         DataRowView Serv = Change_Sas_PublicBindingSource.Current as DataRowView;

         if (Serv == null) return;

         try
         {
            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  #region DoWork
                  new Job(SendType.Self, 06 /* Execute Serv_Info_F */){Input = new XElement("Sas_Public", new XAttribute("datasource", ConnectionString), new XAttribute("fileno", Serv["SERV_FILE_NO"].ToString())) }
                  #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_RunRqst_Click(object sender, EventArgs e)
      {
         try
         {
            if (Txt_LettNo.Text.Trim().Length == 0) { Txt_LettNo.Focus(); return; }
            if (Txt_LettDate.Text.Trim().Length == 0) { Txt_LettDate.Focus(); return; }
            if (Txt_LettOwnr.Text.Trim().Length == 0) { Txt_LettOwnr.Focus(); return; }
            if (Cmb_Rqtp.SelectedIndex == 0) { Cmb_Rqtp.Tag = "idty"; }
            access_entity.Run_Blok_U(string.Format(
               @"DECLARE
                  V_Rslt NUMBER;
                 BEGIN
                     V_Rslt := Pu$Sas_Public.Rqst_Run_F('<Request type=""{0}"" lettno=""{1}"" lettdate=""{2}"" lettownr=""{3}""/>');
                     IF v_Rslt = 0 THEN COMMIT; ELSE ROLLBACK; END IF;
                 END;
                ", Cmb_Rqtp.Tag, Txt_LettNo.Text, Txt_LettDate.Text, Txt_LettOwnr.Text));
            
            Change_Sas_PublicBindingSource.DataSource = access_entity.Run_Qury_U("SELECT * FROM Change_Sas_Public");
         }
         catch
         {
            MessageBox.Show("خطا در ثبت اطلاعات");
         }
      }
   }
}
