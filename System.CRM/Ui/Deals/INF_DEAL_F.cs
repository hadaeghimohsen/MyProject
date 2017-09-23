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

namespace System.CRM.Ui.Deals
{
   public partial class INF_DEAL_F : UserControl
   {
      public INF_DEAL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string pymtstag;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }      

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         PymtBs.DataSource = iCRM.VF_Save_Payments(null, null, null).Where(p => p.PYMT_STAG == pymtstag);
      }

      private void DealActn_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.VF_Save_PaymentsResult;
            if (pymt == null) return;

            switch (e.Button.Index)
            {
               case 1: // Call Log
                  #region Call Log
                  try
                  {
                     // ثبت تماس تلفنی باید به آخرین کسی که از پرسنل شرکت در ارتباط بوده ایم ثبت گردد
                     if (pymt.SERV_FILE_NO != null)
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
                                       new XAttribute("fileno", pymt.SERV_FILE_NO), 
                                       new XAttribute("srpbtype", "002"), 
                                       new XAttribute("islock", true), 
                                       new XAttribute("lcid", 0),
                                       new XAttribute("formcaller", GetType().Name)
                                    )
                               },
                             });
                        _DefaultGateway.Gateway(_InteractWithCRM);
                     }
                  }
                  catch { }
                  #endregion
                  break;
               case 5: // Clone
                  #region Clone
                  //try
                  //{
                  //   Job _InteractWithCRM =
                  //     new Job(SendType.External, "Localhost",
                  //        new List<Job>
                  //        {                  
                  //          new Job(SendType.Self, 30 /* Execute Opt_Clon_F */),
                  //          new Job(SendType.SelfToUserInterface, "OPT_CLON_F", 10 /* Execute ACTN_CALF_P */)
                  //          {
                  //             Input = 
                  //               new XElement("Service", 
                  //                  new XAttribute("fileno", comp.FILE_NO), 
                  //                  new XAttribute("formcaller", GetType().Name)
                  //               )
                  //          },
                  //        });
                  //   _DefaultGateway.Gateway(_InteractWithCRM);
                  //}
                  //catch { }
                  #endregion
                  break;
               case 2: // Delete
                  #region Delete
                  //try
                  //{
                  //   if (comp.RECD_STAT == "002")
                  //   {
                  //      if (comp == null || MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "حذف مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  //      iCRM.DEL_SERV_P(
                  //         new XElement("Service",
                  //            new XAttribute("fileno", comp.FILE_NO)
                  //         )
                  //      );
                  //   }
                  //   requery = true;
                  //}
                  //catch (Exception exc)
                  //{
                  //   iCRM.SaveException(exc);
                  //}
                  //finally
                  //{
                  //   if (requery)
                  //   {
                  //      Execute_Query();
                  //   }
                  //}
                  #endregion
                  break;
               case 3: // Recovery
                  #region Recovery
                  //try
                  //{
                  //   if (comp.RECD_STAT == "001")
                  //   {
                  //      if (comp == null || MessageBox.Show(this, "آیا با بازیابی مشتری موافق هستید؟", "بازیابی مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  //      iCRM.UND_SERV_P(
                  //         new XElement("Service",
                  //            new XAttribute("fileno", comp.FILE_NO)
                  //         )
                  //      );
                  //   }
                  //   requery = true;
                  //}
                  //catch (Exception exc)
                  //{
                  //   iCRM.SaveException(exc);
                  //}
                  //finally
                  //{
                  //   if (requery)
                  //   {
                  //      Execute_Query();
                  //   }
                  //}
                  #endregion
                  break;
               case 4: // Edit
                  #region Edit
                  //_DefaultGateway.Gateway(
                  //   new Job(SendType.External, "Localhost",
                  //      new List<Job>
                  //      {
                  //         new Job(SendType.Self, 13 /* Execute Adm_Chng_F */),
                  //         new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Service", new XAttribute("type", "changeinfo"), new XAttribute("fileno", comp.FILE_NO), new XAttribute("auto", "true"))}
                  //      })
                  //);
                  #endregion
                  break;
               case 0: // Email
                  #region Email
                  try
                  {
                     if (pymt.SERV_FILE_NO != null)
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
                                       new XAttribute("fileno", pymt.SERV_FILE_NO), 
                                       new XAttribute("srpbtype", "002"), 
                                       new XAttribute("islock", true), 
                                       new XAttribute("emid", 0),
                                       new XAttribute("formcaller", GetType().Name),
                                       new XAttribute("toemail", iCRM.Services.FirstOrDefault(s => s.FILE_NO == pymt.SERV_FILE_NO).EMAL_ADRS_DNRM ?? "")
                                    )
                               },
                             });
                        _DefaultGateway.Gateway(_InteractWithCRM);
                     }
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
            iCRM.SaveException(exc);
         }
      }

      private Image GetProfileImage(long servfileno)
      {
         try
         {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iCRM.GET_PIMG_U(new XElement("Service", new XAttribute("fileno", servfileno))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            return bm;
         }
         catch
         {
            return System.CRM.Properties.Resources.IMAGE_1149;
         }
      }

      private void PymtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtBs.Current as Data.VF_Save_PaymentsResult;
            rb_servicerelatedpayment.ImageProfile = GetProfileImage((long)pymt.SERV_FILE_NO);
            rb_servicerelatedpayment.Tag = pymt.SERV_FILE_NO;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void rb_servicerelatedpayment_Click(object sender, EventArgs e)
      {
         try
         {
            var relatedservice = Convert.ToInt64((sender as System.MaxUi.RoundedButton).Tag);
            var serv = iCRM.Services.FirstOrDefault(s => s.FILE_NO == relatedservice);

            if (serv.SRPB_TYPE_DNRM == "001")
            {
               // Lead Info
               try
               {
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                          new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                          new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", relatedservice))},
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
               }
               catch { }
            }
            else if (serv.SRPB_TYPE_DNRM == "002")
            {
               // Contact
               try
               {
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                         new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", relatedservice))},
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
               }
               catch { }
            }
         }
         catch { }
      }
   }
}
