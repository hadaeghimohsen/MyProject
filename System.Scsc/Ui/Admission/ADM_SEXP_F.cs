﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.Admission
{
   public partial class ADM_SEXP_F : UserControl
   {
      public ADM_SEXP_F()
      {
         InitializeComponent();
      }

      private void Btn_AutoExpn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RQID_TextBox.Text == "") return;

            Validate();
            requestBindingSource.EndEdit();

            iScsc.INS_SEXP_P(new XElement( "Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("rqtpcode", P_RqtpCode.Text),
                     new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue),
                     new XAttribute("prvncode", REGN_PRVN_CODE_ComboBox.SelectedValue),
                     new XAttribute("regncode", REGN_CODE_ComboBox.SelectedValue)
                  )
               )
            );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var crntRqid = requestBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            requestBindingSource.DataSource = 
               iScsc.Requests
               .Where(
               rqst =>
                  rqst.RQTP_CODE == P_RqtpCode.Text &&
                  (
                     (rqst.SSTT_MSTT_CODE == 4 || rqst.SSTT_MSTT_CODE == 1 || rqst.SSTT_MSTT_CODE == 2) // && rqst.SSTT_CODE == 1
                  ) &&
                  rqst.SEND_EXPN == "002" &&
                  rqst.SUB_SYS == 1
            ); ;
            requestBindingSource.Position = crntRqid;
         }
      }

      private void Btn_ApprExpn_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = requestBindingSource.Current as Data.Request;
            if (RQID_TextBox.Text == "" || (Rqst.SSTT_MSTT_CODE == 2 && Rqst.SSTT_CODE == 2)) return;

            Validate();
            requestBindingSource.EndEdit();

            iScsc.NEXT_LEVL_F(new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("msttcode", 2),
                     new XAttribute("ssttcode", 2)
                  )
               )
            );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var crntRqid = requestBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            requestBindingSource.DataSource =
               iScsc.Requests
               .Where(
               rqst =>
                  rqst.RQTP_CODE == P_RqtpCode.Text &&
                  (
                     (rqst.SSTT_MSTT_CODE == 4 || rqst.SSTT_MSTT_CODE == 1 || rqst.SSTT_MSTT_CODE == 2) // && rqst.SSTT_CODE == 1
                  ) &&
                  rqst.SEND_EXPN == "002" &&
                  rqst.SUB_SYS == 1
            ); ;
            requestBindingSource.Position = crntRqid;
         }
      }

      private void Btn_CnclRqst_Click(object sender, EventArgs e)
      {

      }

      private void Btn_Next_Click(object sender, EventArgs e)
      {
         XElement x =
            new XElement(
               "Process",
               new XElement("Request",
                  new XAttribute("rqid", RQID_TextBox.Text),
                  new XAttribute("rqtpcode", P_RqtpCode.Text),
                  new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue),
                  new XAttribute("msttcode", SSTT_MSTT_CODE_ComboBox.SelectedValue),
                  new XAttribute("ssttcode", SSTT_CODE_ComboBox.SelectedValue)
               ),
               new XElement("Form",
                  new XAttribute("mtodnumb", 17)
               )
            );
         x = iScsc.GOTO_NEXT_F(x);

         /* Check Condition For Go Next Form */
         if (x.Descendants("NextForm").Attributes("mtodnumb").First().Value == "-1") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "MSTR_PAGE_F", 08 /* Execute Goto_NextForm */, SendType.SelfToUserInterface) { Input = x }
         );
      }
   }
}
