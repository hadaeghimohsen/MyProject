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
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;

namespace System.CRM.Ui.Contacts
{
   public partial class SHW_CONT_F : UserControl
   {
      public SHW_CONT_F()
      {
         InitializeComponent();

         var path = new System.Drawing.Drawing2D.GraphicsPath();
         path.AddEllipse(0, 0, Lb_FilterCount.Width, Lb_FilterCount.Height);

         this.Lb_FilterCount.Region = new Region(path);
         ImageProfile_Butn.ImageVisiable = Tag_Butn.ImageVisiable = ServCont_Butn.ImageVisiable = AddInfo_Butn.ImageVisiable = true;
      }

      private bool requery = false;
      private string onoftag;
      private long compcode;


      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            //Actn_Clmn.Width = 25;
            if (LeadActn_Butn.Buttons.OfType<EditorButton>().Any(b => b.Tag != null && b.Visible == false))
               LeadActn_Butn.Buttons.OfType<EditorButton>().FirstOrDefault(b => b.Tag != null && b.Visible == false).Visible = true;

            LeadActn_Butn.Buttons.OfType<EditorButton>().FirstOrDefault(b => b.Tag != null && b.Tag.ToString() == (onoftag == "on" ? "101" : "001")).Visible = false;

            iCRM = new Data.iCRMDataContext(ConnectionString);
            if (Filter_Butn.Tag != null)
            {
               var Qxml = Filter_Butn.Tag as XElement;

               Qxml.Add(
                     new XElement("Service",
                        new XAttribute("srpbtype", "002"),
                        new XAttribute("onoftag", onoftag),
                        new XAttribute("frstname", FrstName_Txt.Text),
                        new XAttribute("lastname", LastName_Txt.Text),
                        new XAttribute("sextype", BothSex_Rb.Checked ? "" : Men_Rb.Checked ? "001" : "002"),
                        new XAttribute("cellphon", CellPhon_Txt.Text),
                        new XAttribute("tellphon", TellPhon_Txt.Text),
                        new XAttribute("natlcode", NatlCode_Txt.Text),
                        new XAttribute("servno", ServNo_Txt.Text),
                        new XAttribute("postaddr", PostAddr_Txt.Text),
                        new XAttribute("cordx", CordX_Txt.Text),
                        new XAttribute("cordy", CordY_Txt.Text),
                        new XAttribute("radsnumb", Radius_Txt.Text),
                        new XAttribute("emaladdr", EmalAddr_Txt.Text),
                        new XAttribute("confdate", ConfDate_Dat.Value.HasValue ? ConfDate_Dat.Value.Value.Date.ToString("yyyy-MM-dd") : ""),
                        new XElement("Requests",
                           new XAttribute("cont", 1),
                           new XAttribute("allany", "any"),
                           new XElement("Request",
                              new XAttribute("mainstat", MainStat_Lov.EditValue ?? ""),
                              new XAttribute("substat", SubStat_Lov.EditValue ?? "")
                           )
                        )
                     ),
                     new XElement("Company",
                        new XAttribute("code", compcode)
                     )
               );

               ServBs.DataSource =
                  iCRM.VF_Services(
                     Qxml
                  );               
            }
            else
            {
               ServBs.DataSource =
                  iCRM.VF_Services(
                     new XElement("Query",
                        new XElement("Service",
                           new XAttribute("srpbtype", "002"),
                           new XAttribute("onoftag", onoftag),
                           new XAttribute("frstname", FrstName_Txt.Text),
                           new XAttribute("lastname", LastName_Txt.Text),
                           new XAttribute("sextype", BothSex_Rb.Checked ? "" : Men_Rb.Checked ? "001" : "002"),
                           new XAttribute("cellphon", CellPhon_Txt.Text),
                           new XAttribute("tellphon", TellPhon_Txt.Text),
                           new XAttribute("natlcode", NatlCode_Txt.Text),
                           new XAttribute("servno", ServNo_Txt.Text),
                           new XAttribute("postaddr", PostAddr_Txt.Text),
                           new XAttribute("cordx", CordX_Txt.Text),
                           new XAttribute("cordy", CordY_Txt.Text),
                           new XAttribute("radsnumb", Radius_Txt.Text),
                           new XAttribute("emaladdr", EmalAddr_Txt.Text),
                           new XAttribute("confdate", ConfDate_Dat.Value.HasValue ? ConfDate_Dat.Value.Value.Date.ToString("yyyy-MM-dd") : ""),
                           new XElement("Requests",
                              new XAttribute("cont", 1),
                              new XAttribute("allany", "any"),
                              new XElement("Request",
                                 new XAttribute("mainstat", MainStat_Lov.EditValue ?? ""),
                                 new XAttribute("substat", SubStat_Lov.EditValue ?? "")
                              )
                           )
                        ),
                        new XElement("Company",
                           new XAttribute("code", compcode)
                        )
                     )
                  );
            }
         }
         catch { }
         finally
         {            
            Serv_Gv.BestFitColumns();
            if (ServBs.List.Count >= 1)
               tc_master.SelectedTab = tp_001;
            Serv_Gc.Focus();
         }
      }

      private void AddLeads_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 11 /* Execute Adm_Cust_F */),                
                new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Lead", new XAttribute("srpbtype", "002"))},
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void ImageProfile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cont = ServBs.Current as Data.VF_ServicesResult;
            if (cont == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                   new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", cont.FILE_NO))},
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void LeadActn_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.VF_ServicesResult;
            if (serv == null) return;

            switch (e.Button.Index)
            {
               case 0: // Convert
                  #region Convert
                  try
                  {
                     if (serv == null || MessageBox.Show(this, "آیا با تغییر نوع مشتری موافق هستید؟", "تغییر نوع مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                     iCRM.CHNG_SRTP_P(
                        new XElement("Service",
                           new XAttribute("fileno", serv.FILE_NO),
                           new XAttribute("type", serv.SRPB_TYPE_DNRM == "001" ? "002" : "001")
                        )
                     );
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     iCRM.SaveException(exc);
                  }
                  finally
                  {
                     if (requery)
                     {
                        Execute_Query();
                     }
                  }
                  #endregion
                  break;
               case 1: // Call Log
                  #region Call Log
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                            new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                            {
                               Input = 
                                 new XElement("Service", 
                                    new XAttribute("fileno", serv.FILE_NO), 
                                    new XAttribute("lcid", 0),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                            },
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch { }
                  #endregion
                  break;
               case 2: // Clone
                  #region Clone
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 30 /* Execute Opt_Clon_F */),
                            new Job(SendType.SelfToUserInterface, "OPT_CLON_F", 10 /* Execute ACTN_CALF_P */)
                            {
                               Input = 
                                 new XElement("Service", 
                                    new XAttribute("fileno", serv.FILE_NO), 
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                            },
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch { }
                  #endregion
                  break;
               case 3: // Delete
                  #region Delete
                  try
                  {
                     if (Convert.ToInt32(serv.ONOF_TAG_DNRM) >= 101)
                     {
                        if (serv == null || MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "حذف مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                        iCRM.DEL_SERV_P(
                           new XElement("Service",
                              new XAttribute("fileno", serv.FILE_NO)
                           )
                        );
                     }
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     iCRM.SaveException(exc);
                  }
                  finally
                  {
                     if (requery)
                     {
                        Execute_Query();
                     }
                  }
                  #endregion
                  break;
               case 4: // Recovery
                  #region Recovery
                  try
                  {
                     if (Convert.ToInt32(serv.ONOF_TAG_DNRM) <= 100)
                     {
                        if (serv == null || MessageBox.Show(this, "آیا با بازیابی مشتری موافق هستید؟", "بازیابی مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                        iCRM.UND_SERV_P(
                           new XElement("Service",
                              new XAttribute("fileno", serv.FILE_NO)
                           )
                        );
                     }
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     iCRM.SaveException(exc);
                  }
                  finally
                  {
                     if (requery)
                     {
                        Execute_Query();
                     }
                  }
                  #endregion
                  break;
               case 5: // Appointment
                  #region Appointment
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                        {                  
                           new Job(SendType.Self, 27 /* Execute Opt_Apon_F */),
                           new Job(SendType.SelfToUserInterface, "OPT_APON_F", 10 /* Execute ACTN_CALF_P */)
                           {
                              Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", serv.FILE_NO), 
                                 new XAttribute("appointmenttype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                           },
                        });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch (Exception exc) { iCRM.SaveException(exc); }
                  #endregion
                  break;
               case 6: // Edit
                  #region Edit
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 13 /* Execute Adm_Chng_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Service", new XAttribute("type", "changeinfo"), new XAttribute("fileno", serv.FILE_NO), new XAttribute("auto", "true"), new XAttribute("srpbtype", serv.SRPB_TYPE_DNRM))}
                        })
                  );
                  #endregion
                  break;
               case 7: // Email
                  #region Email
                  try
                  {
                     Job _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                            new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                            {
                               Input = 
                                 new XElement("Service", 
                                    new XAttribute("fileno", serv.FILE_NO), 
                                    new XAttribute("emid", 0),
                                    new XAttribute("formcaller", GetType().Name),
                                    new XAttribute("toemail", serv.EMAL_ADRS_DNRM ?? "")
                                 )
                            },
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch (Exception exc) { iCRM.SaveException(exc); }
                  #endregion
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {

         }
      }

      private void ServBs_CurrentChanged(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.VF_ServicesResult;
         if (serv == null) return;
         try
         {
            ImageProfile_Butn.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", serv.FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            if (InvokeRequired)
               Invoke(new Action(() => ImageProfile_Butn.ImageProfile = bm));
            else
               ImageProfile_Butn.ImageProfile = bm;
         }
         catch {
            ImageProfile_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1149;
         }
         finally
         {
            TagBs.DataSource = iCRM.Tags.Where(t => t.SERV_FILE_NO == serv.FILE_NO);
            ExifBs.DataSource = iCRM.Extra_Infos.Where(ex => ex.SERV_FILE_NO == serv.FILE_NO && ex.EXIF_CODE == null);
            ContBs.DataSource = iCRM.Contact_Infos.Where(c => c.SERV_FILE_NO == serv.FILE_NO);
         }
      }

      private void Filter_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 78 /* Execute Hst_Fltr_F */),
                  new Job(SendType.SelfToUserInterface, "HST_FLTR_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Filter",
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               
               }
            )
         );
      }

      private void Lb_FilterCount_Click(object sender, EventArgs e)
      {
         // از بین بردن فیلتر
         Lb_FilterCount.Visible = false;
         Filter_Butn.Tag = null;
         Filter_Butn.ImageProfile = Properties.Resources.IMAGE_1597;
         Execute_Query();
      }

      private void Lb_FilterCount_MouseEnter(object sender, EventArgs e)
      {
         var lb = sender as Label;

         lb.Tag = lb.Text;
         lb.Text = "x";
      }

      private void Lb_FilterCount_MouseLeave(object sender, EventArgs e)
      {
         var lb = sender as Label;

         lb.Text = lb.Tag.ToString();
         lb.Tag = null;
      }

      private void ServInfoSearch_Pikb_PickCheckedChange(object sender)
      {
         Serv_Gv.OptionsFind.AlwaysVisible = ServInfoSearch_Pikb.PickChecked;
      }

      private void SendEmails_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var Qxml = Filter_Butn.Tag as XElement;
            if (Qxml == null)
               Qxml =
               new XElement("Filtering",
                  new XElement("Service",
                     new XAttribute("srpbtype", "001"),
                     new XAttribute("onoftag", onoftag)
                  ),
                  new XElement("Company",
                     new XAttribute("code", compcode)
                  )
               );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 82 /* Execute Opt_Aeml_F */),
                     new Job(SendType.SelfToUserInterface, "OPT_AEML_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("SendEmails",
                              new XAttribute("formcaller", GetType().Name),
                              Qxml,
                              new XElement("Services",
                                 new XAttribute("cont", ServBs.Count)                                 
                              )
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
         }
      }

      private void ShowServiceDetail_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 83 /* Execute Hst_Ssid_F */),
                     new Job(SendType.SelfToUserInterface, "HST_SSID_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Services",
                              new XAttribute("formcaller", GetType().Name),
                              ServBs.List.OfType<Data.VF_ServicesResult>()
                              .Select(s =>
                                 new XElement("Service", 
                                    new XAttribute("fileno", s.FILE_NO),
                                    new XAttribute("namednrm", s.NAME_DNRM),
                                    new XAttribute("cordx", s.CORD_X_DNRM ?? 0),
                                    new XAttribute("cordy", s.CORD_Y_DNRM ?? 0)
                                 )
                              )
                           )
                     }
               
                  }
               )
            );
         }
         catch { }
      }

      private void Tag_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.VF_ServicesResult;
         if (serv == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 50 /* Execute Tsk_Tag_F */),
                  new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", serv.FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void ServCont_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.VF_ServicesResult;
         if (serv == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 60 /* Execute Inf_Ctwk_F */),
                  new Job(SendType.SelfToUserInterface, "INF_CTWK_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", serv.FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void AddInfo_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.Current as Data.VF_ServicesResult;
         if (serv == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 71 /* Execute Add_Info_F */),
                  new Job(SendType.SelfToUserInterface, "ADD_INFO_F", 10 /* Execute Actn_CalF_P */) 
                  {
                     Input = 
                        new XElement("Service",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("fileno", serv.FILE_NO)
                        )
                  }
               }
            )
         );
      }

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void MainStat_Lov_EditValueChanging(object sender, ChangingEventArgs e)
      {
         try
         {
            SsttBs.DataSource = iCRM.Sub_States.Where(s => s.MSTT_CODE == (short)e.NewValue);
         }
         catch 
         {
         }
      }
   }
}
