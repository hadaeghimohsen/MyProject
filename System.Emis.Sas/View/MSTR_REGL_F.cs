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
   public partial class MSTR_REGL_F : UserControl
   {
      public MSTR_REGL_F()
      {
         InitializeComponent();
      }

      private void Lov_YEAR_EditValueChanged(object sender, EventArgs e)
      {
         if (bs_master.SelectedTab == bs_001)
         {
            if (!(Lov_YEAR1.EditValue.ToString() != "" && Convert.ToInt32(Lov_YEAR1.EditValue) > 0)) return;

            Reg07Bs1.DataSource = access_entity.Run_Qury_U(string.Format("SELECT * FROM Regulation WHERE Sub_Sys = 3 AND Regl_Type = '07' AND Cycl_Year = {0}", Lov_YEAR1.EditValue));
            Reg11Bs1.DataSource = access_entity.Run_Qury_U(string.Format("SELECT * FROM Regulation WHERE Sub_Sys = 3 AND Regl_Type = '11' AND Cycl_Year = {0}", Lov_YEAR1.EditValue));
         }
         else if (bs_master.SelectedTab == bs_002)
         {
            if (!(Lov_YEAR2.EditValue.ToString() != "" && Convert.ToInt32(Lov_YEAR2.EditValue) > 0)) return;

            Reg11Bs2.DataSource = access_entity.Run_Qury_U(string.Format("SELECT * FROM Regulation WHERE Sub_Sys = 3 AND Regl_Type = '11' AND Cycl_Year = {0}", Lov_YEAR2.EditValue));
         }
      }

      private void RegulationsBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         if (Reg07Bs1.Current == null)
         {
            Sas_Request_RequesterBindingSource.DataSource = null;
            Regulation_RowBindingSource.DataSource = null;
            return;
         }
         DataRowView Regl = Reg07Bs1.Current as DataRowView;

         Sas_Request_RequesterBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT * FROM Sas_Request_Requester WHERE Regl_Cycl_Year = {0} AND Regl_Code = {1} ORDER BY RQTP_CODE, RQTT_CODE", Regl["CYCL_YEAR"], Regl["Code"]));
         Regulation_RowBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT Rgro.Regl_Cycl_Year, Rgro.Regl_Code,Rgro.Row_No,Rgro.Rgro_Desc,Rgro.Trsp_Code,Ats.Use_Type,Pd.Netw_Type,Pd.Phas,Pd.From_Ampr,Pd.To_Ampr,Pd.From_Powr,Pd.To_Powr FROM Regulation_Row Rgro, Adm_Transfer_Spec Ats, Power_Domain Pd WHERE     Rgro.Trsp_Code = Ats.Code AND Ats.Podo_Code = Pd.Code AND Ats.Trns_Stat = '1' AND Pd.Sub_Sys = 3 AND Rgro.Regl_Cycl_Year = {0} AND Rgro.Regl_Code = {1}", Regl["CYCL_YEAR"], Regl["Code"]));
      }

      private void RQRQ_CurrentChange(object sender, EventArgs e)
      {
         if (Sas_Request_RequesterBindingSource.Current == null) return;

         DataRowView RQRQ = Sas_Request_RequesterBindingSource.Current as DataRowView;

         ExtpBs1.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT * FROM Expense_Type WHERE Rqrq_Regl_Cycl_Year = {0} AND Rqrq_Regl_Code = {1} AND Rqrq_Rqtp_Code = {2} AND Rqrq_Rqtt_Code = {3}", RQRQ["REGL_CYCL_YEAR"], RQRQ["REGL_CODE"], RQRQ["RQTP_CODE"], RQRQ["RQTT_CODE"]));

         RGRO_CurrentChanged(sender, e);
      }

      private void RGRO_CurrentChanged(object sender, EventArgs e)
      {
         if (Regulation_RowBindingSource.Current == null) return;
         if (ExtpBs1.Current == null) return;
         if (Reg11Bs1.Current == null) return;

         DataRowView RGRO = Regulation_RowBindingSource.Current as DataRowView;
         DataRowView EXTP = ExtpBs1.Current as DataRowView;

         Txt_CustType.Text = domain.RGRO(RGRO["PHAS"].ToString(), RGRO["NETW_TYPE"].ToString(), Convert.ToInt32(RGRO["FROM_AMPR"]), Convert.ToInt32(RGRO["TO_AMPR"]), Convert.ToInt32(RGRO["FROM_POWR"]), Convert.ToInt64(RGRO["TO_POWR"]));

         Expense_BindingSource.DataSource = access_entity.Run_Qury_U(string.Format("SELECT * FROM Expense WHERE RGRO_REGL_CYCL_YEAR = {0} AND RGRO_REGL_CODE = {1} AND RGRO_ROW_NO = {2} AND EXTP_CODE = {3}", RGRO["REGL_CYCL_YEAR"], RGRO["REGL_CODE"], RGRO["ROW_NO"], EXTP["CODE"]));
         
         ReacBs1.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT * FROM ADM_REGION_EXPENSE_ACOUNT WHERE REGL_CYCL_YEAR = {0} AND REGL_CODE = {1} AND EXTP_CODE = {2}", RGRO["REGL_CYCL_YEAR"], Code11_LookUpEdit.EditValue, EXTP["CODE"]));
      }

      private void Expense_TypeBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         RGRO_CurrentChanged(null, null);
      }

      private void Btn_SaveExpn_Click(object sender, EventArgs e)
      {
         DataRowView RGRO = Regulation_RowBindingSource.Current as DataRowView;
         DataRowView EXTP = ExtpBs1.Current as DataRowView;
         DataRowView EXPN = Expense_BindingSource.Current as DataRowView;

         if (RGRO == null) return;
         if (EXTP == null) return;
         if (EXPN == null)
         {
            access_entity.Run_Blok_U(string.Format(@"
            BEGIN
               INSERT INTO Expense (Pric, Extr_Prct, Rgro_Regl_Cycl_Year, Rgro_Regl_Code, Rgro_Row_No, Extp_Code, Actv)
               VALUES              ({0} , {1}      , {2}                , {3}           , {4}        , {5}      , 1);
   
               IF SQL%ROWCOUNT = 1 THEN
                  COMMIT;
               ELSE
                  ROLLBACK;
               END IF;

               EXCEPTION
                  WHEN OTHERS THEN
                     ROLLBACK;
            END;
            ", pRICTextBox.Text, eXTR_PRCTTextBox.Text, RGRO["REGL_CYCL_YEAR"], RGRO["REGL_CODE"], RGRO["ROW_NO"], EXTP["CODE"]));
            RGRO_CurrentChanged(null, null);
         }
         else
         {
            access_entity.Run_Blok_U(string.Format(@"
            BEGIN
               UPDATE Expense
                  SET Pric = {0}
                     ,Extr_Prct = {1}
                     ,Actv = 1
                  WHERE
                        RGRO_REGL_CYCL_YEAR = {2}
                  AND RGRO_REGL_CODE = {3}
                  AND RGRO_ROW_NO = {4}
                  AND EXTP_CODE = {5}
                  AND CODE = {6};
   
               IF SQL%ROWCOUNT = 1 THEN
                  COMMIT;
               ELSE
                  ROLLBACK;
               END IF;

               EXCEPTION
                  WHEN OTHERS THEN
                     ROLLBACK;
            END;
            ", pRICTextBox.Text, eXTR_PRCTTextBox.Text, RGRO["REGL_CYCL_YEAR"], RGRO["REGL_CODE"], RGRO["ROW_NO"], EXTP["CODE"], EXPN["CODE"]));
         }
      }

      private void Code11_LookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         if (Regulation_RowBindingSource.Current == null) return;
         if (ExtpBs1.Current == null) return;
         if (Reg11Bs1.Current == null) return;

         DataRowView RGRO = Regulation_RowBindingSource.Current as DataRowView;
         DataRowView EXTP = ExtpBs1.Current as DataRowView;

         ReacBs1.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT * FROM ADM_REGION_EXPENSE_ACOUNT WHERE REGL_CYCL_YEAR = {0} AND REGL_CODE = {1} AND EXTP_CODE = {2}", RGRO["REGL_CYCL_YEAR"], Code11_LookUpEdit.EditValue, EXTP["CODE"]));
      }

      private void BankBs2_CurrentChanged(object sender, EventArgs e)
      {
         var bank = BankBs2.Current as DataRowView;
         BnkbBs2.DataSource = access_entity.Run_Qury_U(string.Format("SELECT * FROM Bank_Branch WHERE Bank_Code = {0}", bank["CODE"]));
      }

      private void BnkbBs2_CurrentChanged(object sender, EventArgs e)
      {
         var bnkb = BnkbBs2.Current as DataRowView;
         BnkaBs2.DataSource = access_entity.Run_Qury_U(string.Format("SELECT * FROM Bank_Account WHERE Bnkb_Code = {0} AND Bnkb_Bank_Code = {1}", bnkb["Code"], bnkb["Bank_Code"]));
      }

      private void ReacFetchData(object sender, EventArgs e)
      {
         ExtpBs2.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT * FROM Expense_Type Extp  WHERE Rqrq_Regl_Cycl_Year = {0} AND Rqrq_Regl_Code = {1}", Tx7_CyclYear.Text, Tx7_Code.Text));

         if (BnkaBs2.Current == null) return;
         if (Reg11Bs2.Current == null) return;

         var bnka = BnkaBs2.Current as DataRowView;
         var reg11 = Reg11Bs2.Current as DataRowView;

         ReacBs2.DataSource = access_entity.Run_Qury_U(string.Format("SELECT Extp_Code, Regn_Code, (SELECT /*'هزینه ' || Epit.Epit_Desc ||*/ ' برای ' || Rqtp.Rqtp_Desc || ' به درخواست ' || Rqtt.Rqtt_Desc FROM Expense_Type Extp, Request_Type Rqtp, Requester_Type Rqtt, Sas_Expense_Item Epit WHERE Rqrq_Regl_Cycl_Year = {5} AND Rqrq_Regl_Code = {6} AND Extp.Code = Reac.Extp_Code AND Rqrq_Rqtp_Code = Rqtp.Code AND Rqrq_Rqtt_Code = Rqtt.Code AND Epit_Code = Epit.Code) AS Long_Extp_Desc FROM Adm_Region_Expense_Acount Reac WHERE REGL_CYCL_YEAR = {0} AND REGL_CODE = {1} AND BNKA_BNKB_BANK_CODE = {2} AND BNKA_BNKB_CODE = {3} AND BNKA_ACNT_NUMB = '{4}'", reg11["CYCL_YEAR"], reg11["CODE"], bnka["BNKB_BANK_CODE"], bnka["BNKB_CODE"], bnka["ACNT_NUMB"], Tx7_CyclYear.Text, Tx7_Code.Text));
      }

   }
}
