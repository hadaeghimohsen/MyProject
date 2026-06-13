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

             Reg07Bs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Regulation WHERE Sub_Sys = 3 AND Regl_Type = '07' AND Cycl_Year = :p0", new OracleParameter[] { new OracleParameter(":p0", Lov_YEAR1.EditValue) });
             Reg11Bs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Regulation WHERE Sub_Sys = 3 AND Regl_Type = '11' AND Cycl_Year = :p0", new OracleParameter[] { new OracleParameter(":p0", Lov_YEAR1.EditValue) });
         }
         else if (bs_master.SelectedTab == bs_002)
         {
            if (!(Lov_YEAR2.EditValue.ToString() != "" && Convert.ToInt32(Lov_YEAR2.EditValue) > 0)) return;

             Reg11Bs2.DataSource = access_entity.Run_Qury_U("SELECT * FROM Regulation WHERE Sub_Sys = 3 AND Regl_Type = '11' AND Cycl_Year = :p0", new OracleParameter[] { new OracleParameter(":p0", Lov_YEAR2.EditValue) });
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

         Sas_Request_RequesterBindingSource.DataSource = access_entity.Run_Qury_U("SELECT * FROM Sas_Request_Requester WHERE Regl_Cycl_Year = :p0 AND Regl_Code = :p1 ORDER BY RQTP_CODE, RQTT_CODE", new OracleParameter[] { new OracleParameter(":p0", Regl["CYCL_YEAR"]), new OracleParameter(":p1", Regl["Code"]) });
          Regulation_RowBindingSource.DataSource = access_entity.Run_Qury_U("SELECT Rgro.Regl_Cycl_Year, Rgro.Regl_Code,Rgro.Row_No,Rgro.Rgro_Desc,Rgro.Trsp_Code,Ats.Use_Type,Pd.Netw_Type,Pd.Phas,Pd.From_Ampr,Pd.To_Ampr,Pd.From_Powr,Pd.To_Powr FROM Regulation_Row Rgro, Adm_Transfer_Spec Ats, Power_Domain Pd WHERE     Rgro.Trsp_Code = Ats.Code AND Ats.Podo_Code = Pd.Code AND Ats.Trns_Stat = '1' AND Pd.Sub_Sys = 3 AND Rgro.Regl_Cycl_Year = :p0 AND Rgro.Regl_Code = :p1", new OracleParameter[] { new OracleParameter(":p0", Regl["CYCL_YEAR"]), new OracleParameter(":p1", Regl["Code"]) });
      }

      private void RQRQ_CurrentChange(object sender, EventArgs e)
      {
         if (Sas_Request_RequesterBindingSource.Current == null) return;

         DataRowView RQRQ = Sas_Request_RequesterBindingSource.Current as DataRowView;

         ExtpBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Expense_Type WHERE Rqrq_Regl_Cycl_Year = :p0 AND Rqrq_Regl_Code = :p1 AND Rqrq_Rqtp_Code = :p2 AND Rqrq_Rqtt_Code = :p3", new OracleParameter[] { new OracleParameter(":p0", RQRQ["REGL_CYCL_YEAR"]), new OracleParameter(":p1", RQRQ["REGL_CODE"]), new OracleParameter(":p2", RQRQ["RQTP_CODE"]), new OracleParameter(":p3", RQRQ["RQTT_CODE"]) });

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

         Expense_BindingSource.DataSource = access_entity.Run_Qury_U("SELECT * FROM Expense WHERE RGRO_REGL_CYCL_YEAR = :p0 AND RGRO_REGL_CODE = :p1 AND RGRO_ROW_NO = :p2 AND EXTP_CODE = :p3", new OracleParameter[] { new OracleParameter(":p0", RGRO["REGL_CYCL_YEAR"]), new OracleParameter(":p1", RGRO["REGL_CODE"]), new OracleParameter(":p2", RGRO["ROW_NO"]), new OracleParameter(":p3", EXTP["CODE"]) });
          
          ReacBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM ADM_REGION_EXPENSE_ACOUNT WHERE REGL_CYCL_YEAR = :p0 AND REGL_CODE = :p1 AND EXTP_CODE = :p2", new OracleParameter[] { new OracleParameter(":p0", RGRO["REGL_CYCL_YEAR"]), new OracleParameter(":p1", Code11_LookUpEdit.EditValue), new OracleParameter(":p2", EXTP["CODE"]) });
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
             access_entity.Run_Blok_U(@"
             BEGIN
                INSERT INTO Expense (Pric, Extr_Prct, Rgro_Regl_Cycl_Year, Rgro_Regl_Code, Rgro_Row_No, Extp_Code, Actv)
                VALUES              (:p0 , :p1      , :p2                , :p3           , :p4        , :p5      , 3);
    
                IF SQL%ROWCOUNT = 1 THEN
                   COMMIT;
                ELSE
                   ROLLBACK;
                END IF;

                EXCEPTION
                   WHEN OTHERS THEN
                      ROLLBACK;
             END;
             ", new OracleParameter[] {
                new OracleParameter(":p0", pRICTextBox.Text),
                new OracleParameter(":p1", eXTR_PRCTTextBox.Text),
                new OracleParameter(":p2", RGRO["REGL_CYCL_YEAR"]),
                new OracleParameter(":p3", RGRO["REGL_CODE"]),
                new OracleParameter(":p4", RGRO["ROW_NO"]),
                new OracleParameter(":p5", EXTP["CODE"])
             });
             RGRO_CurrentChanged(null, null);
          }
          else
          {
             access_entity.Run_Blok_U(@"
             BEGIN
                UPDATE Expense
                   SET Pric = :p0
                      ,Extr_Prct = :p1
                      ,Actv = :p7
                   WHERE
                         RGRO_REGL_CYCL_YEAR = :p2
                   AND RGRO_REGL_CODE = :p3
                   AND RGRO_ROW_NO = :p4
                   AND EXTP_CODE = :p5
                   AND CODE = :p6;
    
                IF SQL%ROWCOUNT = 1 THEN
                   COMMIT;
                ELSE
                   ROLLBACK;
                END IF;

                EXCEPTION
                   WHEN OTHERS THEN
                      ROLLBACK;
             END;
             ", new OracleParameter[] {
                new OracleParameter(":p0", pRICTextBox.Text),
                new OracleParameter(":p1", eXTR_PRCTTextBox.Text),
                new OracleParameter(":p2", RGRO["REGL_CYCL_YEAR"]),
                new OracleParameter(":p3", RGRO["REGL_CODE"]),
                new OracleParameter(":p4", RGRO["ROW_NO"]),
                new OracleParameter(":p5", EXTP["CODE"]),
                new OracleParameter(":p6", EXPN["CODE"]),
                new OracleParameter(":p7", EXPN["ACTV"])
             });
          }

         Totl_Expn_Txt.Text = (Convert.ToInt64(pRICTextBox.Text) + Convert.ToInt64(eXTR_PRCTTextBox.Text)).ToString("n0");
      }

      private void Code11_LookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         if (Regulation_RowBindingSource.Current == null) return;
         if (ExtpBs1.Current == null) return;
         if (Reg11Bs1.Current == null) return;

         DataRowView RGRO = Regulation_RowBindingSource.Current as DataRowView;
         DataRowView EXTP = ExtpBs1.Current as DataRowView;

         ReacBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM ADM_REGION_EXPENSE_ACOUNT WHERE REGL_CYCL_YEAR = :p0 AND REGL_CODE = :p1 AND EXTP_CODE = :p2", new OracleParameter[] { new OracleParameter(":p0", RGRO["REGL_CYCL_YEAR"]), new OracleParameter(":p1", Code11_LookUpEdit.EditValue), new OracleParameter(":p2", EXTP["CODE"]) });
      }

      private void BankBs2_CurrentChanged(object sender, EventArgs e)
      {
         var bank = BankBs2.Current as DataRowView;
          BnkbBs2.DataSource = access_entity.Run_Qury_U("SELECT * FROM Bank_Branch WHERE Bank_Code = :p0", new OracleParameter[] { new OracleParameter(":p0", bank["CODE"]) });
      }

      private void BnkbBs2_CurrentChanged(object sender, EventArgs e)
      {
         var bnkb = BnkbBs2.Current as DataRowView;
          BnkaBs2.DataSource = access_entity.Run_Qury_U("SELECT * FROM Bank_Account WHERE Bnkb_Code = :p0 AND Bnkb_Bank_Code = :p1", new OracleParameter[] { new OracleParameter(":p0", bnkb["Code"]), new OracleParameter(":p1", bnkb["Bank_Code"]) });
      }

      private void ReacFetchData(object sender, EventArgs e)
      {
         ExtpBs2.DataSource = access_entity.Run_Qury_U("SELECT * FROM Expense_Type Extp  WHERE Rqrq_Regl_Cycl_Year = :p0 AND Rqrq_Regl_Code = :p1", new OracleParameter[] { new OracleParameter(":p0", Tx7_CyclYear.Text), new OracleParameter(":p1", Tx7_Code.Text) });

         if (BnkaBs2.Current == null) return;
         if (Reg11Bs2.Current == null) return;

         var bnka = BnkaBs2.Current as DataRowView;
         var reg11 = Reg11Bs2.Current as DataRowView;

         ReacBs2.DataSource = access_entity.Run_Qury_U("SELECT Extp_Code, Regn_Code, (SELECT /*'هزینه ' || Epit.Epit_Desc ||*/ ' برای ' || Rqtp.Rqtp_Desc || ' به درخواست ' || Rqtt.Rqtt_Desc FROM Expense_Type Extp, Request_Type Rqtp, Requester_Type Rqtt, Sas_Expense_Item Epit WHERE Rqrq_Regl_Cycl_Year = :p5 AND Rqrq_Regl_Code = :p6 AND Extp.Code = Reac.Extp_Code AND Rqrq_Rqtp_Code = Rqtp.Code AND Rqrq_Rqtt_Code = Rqtt.Code AND Epit_Code = Epit.Code) AS Long_Extp_Desc FROM Adm_Region_Expense_Acount Reac WHERE REGL_CYCL_YEAR = :p0 AND REGL_CODE = :p1 AND BNKA_BNKB_BANK_CODE = :p2 AND BNKA_BNKB_CODE = :p3 AND BNKA_ACNT_NUMB = :p4", new OracleParameter[] { new OracleParameter(":p0", reg11["CYCL_YEAR"]), new OracleParameter(":p1", reg11["CODE"]), new OracleParameter(":p2", bnka["BNKB_BANK_CODE"]), new OracleParameter(":p3", bnka["BNKB_CODE"]), new OracleParameter(":p4", bnka["ACNT_NUMB"]), new OracleParameter(":p5", Tx7_CyclYear.Text), new OracleParameter(":p6", Tx7_Code.Text) });
      }

      private void Expense_BindingSource_CurrentChanged(object sender, EventArgs e)
      {
         var expn = Expense_BindingSource.Current as DataRowView;
         if (expn == null) return;

         Totl_Expn_Txt.Text = (Convert.ToInt64(expn["PRIC"]) + Convert.ToInt64(expn["EXTR_PRCT"])).ToString("n0");
      }

      private void CopyExpn_Butn_Click(object sender, EventArgs e)
      {
         var servtype = Txt_CustType.Text;
         var expn = Expense_BindingSource.Current as DataRowView;

         Regulation_RowBindingSource.MoveFirst();
         for (int i = 0; i < Regulation_RowBindingSource.Count; ++i )
         {
            Regulation_RowBindingSource.MoveNext();
            RGRO_CurrentChanged(null, null);
            var RGRO = Regulation_RowBindingSource.Current as Data.DataRowView;
            if (servtype == domain.RGRO(RGRO["PHAS"].ToString(), RGRO["NETW_TYPE"].ToString(), Convert.ToInt32(RGRO["FROM_AMPR"]), Convert.ToInt32(RGRO["TO_AMPR"]), Convert.ToInt32(RGRO["FROM_POWR"]), Convert.ToInt64(RGRO["TO_POWR"])))
            {
               var newexpn = Expense_BindingSource.Current as DataRowView;
               newexpn["PRIC"] = expn["PRIC"];
               newexpn["EXTR_PRCT"] = expn["EXTR_PRCT"];

               pRICTextBox.Text = expn["PRIC"].ToString();
               eXTR_PRCTTextBox.Text = expn["EXTR_PRCT"].ToString();

               Btn_SaveExpn_Click(null, null);
            }
         }
      }

   }
}
