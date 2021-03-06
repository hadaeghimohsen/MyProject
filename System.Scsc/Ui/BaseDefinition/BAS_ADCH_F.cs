﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_ADCH_F : UserControl
   {
      public BAS_ADCH_F()
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

      private void GetMaxFngrPrint_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FNGR_PRNT_TextEdit.EditValue =
                iScsc.Fighters
                .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                .Select(f => f.FNGR_PRNT_DNRM)
                .ToList()
                .Where(f => f.All(char.IsDigit))
                .Max(f => Convert.ToInt64(f)) + 1;
         }
         catch
         {
            FNGR_PRNT_TextEdit.Text = "1";
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FrstName_Text.Text == "") { FrstName_Text.Focus(); return; }
            if (LastName_Text.Text == "") { LastName_Text.Focus(); return; }
            long mtod = 0;
            if (Mtod_Lov.EditValue == null || !long.TryParse(Mtod_Lov.EditValue.ToString(), out mtod)) { Mtod_Lov.Focus(); return; }
            int sex = 0;
            if (SexType_Lov.EditValue == null || !int.TryParse(SexType_Lov.EditValue.ToString(), out sex)) { SexType_Lov.Focus(); return; }

            iScsc.ADM_MSAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "001"),
                     new XAttribute("rqttcode", "003"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName_Text.Text),
                        new XElement("Last_Name", LastName_Text.Text),
                        new XElement("Fath_Name", ""),
                        new XElement("Coch_Deg", ""),
                        new XElement("Coch_Crtf_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("Gudg_Deg", ""),
                        new XElement("glob_Code", ""),
                        new XElement("Sex_Type", SexType_Lov.EditValue),
                        new XElement("Natl_Code", "0"),
                        new XElement("Brth_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("Cell_Phon", ""),
                        new XElement("Tell_Phon", ""),
                        new XElement("Type", "003"),
                        new XElement("Post_Adrs", ""),
                        new XElement("Emal_Adrs", ""),
                        new XElement("Insr_Numb", ""),
                        new XElement("Insr_Date", DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")),
                        new XElement("Educ_Deg", ""),
                        new XElement("Mtod_Code", Mtod_Lov.EditValue),
                        new XElement("Dise_Code", ""),
                        new XElement("Calc_Expn_Type", ""),
                        new XElement("Blod_grop", ""),
                        new XElement("Fngr_Prnt", FNGR_PRNT_TextEdit.Text),
                        new XElement("Dpst_Acnt_Slry_Bank", ""),
                        new XElement("Dpst_Acnt_Slry", ""),
                        new XElement("Chat_Id", Chat_Id_TextEdit.Text)
                     )
                  )
               )
            );

            // Save Card In Device
            if (CardNum_Txt.Text != "")
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
                  {
                     Input =
                     new XElement("User",
                        new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text),
                        new XAttribute("cardnumb", CardNum_Txt.Text),
                        new XAttribute("namednrm", FrstName_Text.Text + ", " + LastName_Text.Text)
                     )
                  }
               );

            CardNum_Txt.Text = "";

            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("TabPage",
                                 new XAttribute("showtabpage", "tp_005")
                              )
                        }
                     }
                  )
               );
               Back_Butn_Click(null, null);
               requery = false;
            }
         }
      }

      private void SaveNewCoch_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (FrstName_Text.Text == "") { FrstName_Text.Focus(); return; }
            if (LastName_Text.Text == "") { LastName_Text.Focus(); return; }
            long mtod = 0;
            if (Mtod_Lov.EditValue == null || !long.TryParse(Mtod_Lov.EditValue.ToString(), out mtod)) { Mtod_Lov.Focus(); return; }
            int sex = 0;
            if (SexType_Lov.EditValue == null || !int.TryParse(SexType_Lov.EditValue.ToString(), out sex)) { SexType_Lov.Focus(); return; }

            iScsc.ADM_MSAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "001"),
                     new XAttribute("rqttcode", "003"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName_Text.Text),
                        new XElement("Last_Name", LastName_Text.Text),
                        new XElement("Fath_Name", ""),
                        new XElement("Coch_Deg", ""),
                        new XElement("Coch_Crtf_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("Gudg_Deg", ""),
                        new XElement("glob_Code", ""),
                        new XElement("Sex_Type", SexType_Lov.EditValue),
                        new XElement("Natl_Code", "0"),
                        new XElement("Brth_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("Cell_Phon", ""),
                        new XElement("Tell_Phon", ""),
                        new XElement("Type", "003"),
                        new XElement("Post_Adrs", ""),
                        new XElement("Emal_Adrs", ""),
                        new XElement("Insr_Numb", ""),
                        new XElement("Insr_Date", DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")),
                        new XElement("Educ_Deg", ""),
                        new XElement("Mtod_Code", Mtod_Lov.EditValue),
                        new XElement("Dise_Code", ""),
                        new XElement("Calc_Expn_Type", ""),
                        new XElement("Blod_grop", ""),
                        new XElement("Fngr_Prnt", FNGR_PRNT_TextEdit.Text),
                        new XElement("Dpst_Acnt_Slry_Bank", ""),
                        new XElement("Dpst_Acnt_Slry", ""),
                        new XElement("Chat_Id", Chat_Id_TextEdit.Text)
                     )
                  )
               )
            );

            // Save Card In Device
            if (CardNum_Txt.Text != "")
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface)
                  {
                     Input =
                     new XElement("User",
                        new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text),
                        new XAttribute("cardnumb", CardNum_Txt.Text),
                        new XAttribute("namednrm", FrstName_Text.Text + ", " + LastName_Text.Text)
                     )
                  }
               );

            CardNum_Txt.Text = "";

            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("TabPage",
                                 new XAttribute("showtabpage", "tp_005")
                              )
                        }
                     }
                  )
               );
               FrstName_Text.Text = LastName_Text.Text = FNGR_PRNT_TextEdit.Text = Chat_Id_TextEdit.Text = "";
               FrstName_Text.Focus();
               requery = false;
            }
         }
      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("Command",
            //            new XAttribute("type", "fngrprntdev"),
            //            new XAttribute("fngractn", "enroll"),
            //            new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
            //         )
            //   }
            //);
         }
         catch{ }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
            //   {
            //      Input =
            //         new XElement("Command",
            //            new XAttribute("type", "fngrprntdev"),
            //            new XAttribute("fngractn", "enroll"),
            //            new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
            //         )
            //   }
            //);
         }
         catch{ }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch{ }
      }

      private void RqstBnDeleteFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch{ }
      }
      #endregion
   }
}
