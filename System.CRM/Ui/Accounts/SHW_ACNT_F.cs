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
using System.Drawing.Imaging;

namespace System.CRM.Ui.Acounts
{
   public partial class SHW_ACNT_F : UserControl
   {
      public SHW_ACNT_F()
      {
         InitializeComponent();

         var path = new System.Drawing.Drawing2D.GraphicsPath();
         path.AddEllipse(0, 0, Lb_FilterCount.Width, Lb_FilterCount.Height);

         this.Lb_FilterCount.Region = new Region(path);
      }

      private bool requery = false;

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
            Actn_Clmn.Width = 25;
            if (CompActn_Butn.Buttons.OfType<EditorButton>().Any(b => b.Tag != null && b.Visible == false))
               CompActn_Butn.Buttons.OfType<EditorButton>().FirstOrDefault(b => b.Tag != null && b.Visible == false).Visible = true;

            CompActn_Butn.Buttons.OfType<EditorButton>().FirstOrDefault(b => b.Tag != null && b.Tag.ToString() == (onoftag == "on" ? "002" : "001")).Visible = false;

            iCRM = new Data.iCRMDataContext(ConnectionString);

            var Qxml = Filter_Butn.Tag as XElement;
            
            if (Qxml == null)
               Qxml =
                  new XElement("Company",
                     new XAttribute("recdstat", onoftag == "on" ? "002" : "001")
                  );
            else
               Qxml.Add(
                  new XElement("Company",
                     new XAttribute("recdstat", onoftag == "on" ? "002" : "001")
                  )
               );

            
            switch(AcntsSearch_Lov.Tag.ToString())
            {
               case "001": // نمایش کل شرکت ها                  
                  break;
               case "002": // نمایش شرکت های پیش فرض نواحی
                  Qxml.Add(
                     new XElement("Company",
                        new XAttribute("dfltstat", "002")
                     )
                  );
                  break;
               case "003": // نمایش شرکت های غیر از پیش فرض نواحی
                  Qxml.Add(
                     new XElement("Company",
                        new XAttribute("dfltstat", "001")
                     )
                  );
                  break;
               case "004": // شرکت هایی که مشتری به آن متصل می باشد
                  Qxml.Add(
                     new XElement("Company",
                        new XAttribute("empynumb", "002")
                     )
                  );
                  break;
               case "005": // شرکت هایی که مشتری با آن متصل نمی باشد
                  Qxml.Add(
                     new XElement("Company",
                        new XAttribute("empynumb", "001")
                     )
                  );
                  break;
            }
            CompBs.DataSource =
               iCRM.VF_Companies(Qxml);
            requery = false;
         }
         catch { }
         finally
         {
            Comp_Gv.BestFitColumns();
         }
      }

      private void ImageProfile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cont = CompBs.Current as Data.VF_CompaniesResult;
            if (cont == null) return;

            if(actntype == "join")
            {
               if (MessageBox.Show(this, "آیا با انتقال مشتری به شرکت مربوطه موافق هستید؟", "انتقال به شرکت", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               // مشتری انتقال داده شود
               var serv = iCRM.Services.FirstOrDefault(s => s.FILE_NO == fileno);
               iCRM.CHNG_SRPB_P(
                  new XElement("Service",
                     new XAttribute("fileno", serv.FILE_NO),
                     new XAttribute("actntype", "002"),
                     new XAttribute("compcode", cont.CODE)
                  )
               );
               // فرم بسته شود
               Back_Butn_Click(null, null);
               // اطلاعات مشتری بروز شود
               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, formcaller == "INF_LEAD_F" ? 24 : 34 /* Execute Inf_Lead_F */),                
                      new Job(SendType.SelfToUserInterface, formcaller, 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", fileno))},
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);

            }
            else if (actntype == "none")
            {
               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 39 /* Execute Inf_Acnt_F */),                
                      new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Company", new XAttribute("code", cont.CODE))},
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void LeadActn_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var comp = CompBs.Current as Data.VF_CompaniesResult;
            if (comp == null) return;

            Job _InteractWithCRM = null;
            switch (e.Button.Index)
            {
               case 0: // Call Log
                  #region Call Log
                  try
                  {
                     // ثبت تماس تلفنی باید به آخرین کسی که از پرسنل شرکت در ارتباط بوده ایم ثبت گردد
                     if (comp.LAST_SERV_FILE_NO_DNRM != null)
                     {
                        _InteractWithCRM =
                          new Job(SendType.External, "Localhost",
                             new List<Job>
                             {                  
                               new Job(SendType.Self, 25 /* Execute Opt_Logc_F */),
                               new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 10 /* Execute ACTN_CALF_P */)
                               {
                                  Input = 
                                    new XElement("Service", 
                                       new XAttribute("fileno", comp.LAST_SERV_FILE_NO_DNRM), 
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
               case 1: // Clone
                  #region Clone
                  try
                  {
                     _InteractWithCRM =
                       new Job(SendType.External, "Localhost",
                          new List<Job>
                          {                  
                            new Job(SendType.Self, 46 /* Execute Opt_Clnc_F */),
                            new Job(SendType.SelfToUserInterface, "OPT_CLNC_F", 10 /* Execute ACTN_CALF_P */)
                            {
                               Input = 
                                 new XElement("Company", 
                                    new XAttribute("code", comp.CODE), 
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                            },
                          });
                     _DefaultGateway.Gateway(_InteractWithCRM);
                  }
                  catch { }
                  #endregion
                  break;
               case 2: // Delete
                  #region Delete
                  try
                  {
                     if (comp.RECD_STAT == "002")
                     {
                        if (comp == null || MessageBox.Show(this, "آیا با حذف شرکت موافق هستید؟", "حذف شرکت", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                        iCRM.ONF_COMP_P(
                           new XElement("Company",
                              new XAttribute("Code", comp.CODE)
                           )
                        );
                     }
                     else
                     {
                        if (comp == null || MessageBox.Show(this, "آیا با بازیابی شرکت موافق هستید؟", "بازیابی شرکت", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                        iCRM.ONF_COMP_P(
                           new XElement("Company",
                              new XAttribute("code", comp.CODE)
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
                  _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 47 /* Execute Regn_Dfin_F */),                
                         new Job(SendType.SelfToUserInterface, "COMP_CHNG_F", 10 /* Execute Actn_Calf_F */)
                         {
                            Input = 
                              new XElement("Company", 
                                 new XAttribute("formtype", "edit"),
                                 new XAttribute("code", comp.CODE),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         }
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
                  #endregion
                  break;
               case 5: // Email
                  #region Email
                  try
                  {
                     if (comp.LAST_SERV_FILE_NO_DNRM != null)
                     {
                        _InteractWithCRM =
                          new Job(SendType.External, "Localhost",
                             new List<Job>
                             {                  
                               new Job(SendType.Self, 31 /* Execute Opt_Emal_F */),
                               new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 10 /* Execute ACTN_CALF_P */)
                               {
                                  Input = 
                                    new XElement("Service", 
                                       new XAttribute("fileno", comp.LAST_SERV_FILE_NO_DNRM), 
                                       new XAttribute("emid", 0),
                                       new XAttribute("formcaller", GetType().Name),
                                       new XAttribute("toemail", comp.EMAL_ADRS ?? "")
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

         }
      }

      private void CompBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {            
            var comp = CompBs.Current as Data.VF_CompaniesResult;
            if(comp == null)
               return;

            int i = 0;

            var relatedservice = iCRM.VF_Request_Changing(null, null, comp.CODE, null).OrderByDescending(r => r.SAVE_DATE).ToList().Select(s => new { s.FILE_NO, s.NAME_DNRM }).Distinct().Take(3);
            lb_1st.Visible = rb_1st.Visible = false;
            lb_2nd.Visible = rb_2nd.Visible = false;
            lb_3rd.Visible = rb_3rd.Visible = false;

            foreach (var serv in relatedservice)
            {
               if (i == 0)
               {
                  lb_1st.Visible = rb_1st.Visible = true;
                  rb_1st.ImageProfile = GetProfileImage(serv.FILE_NO);
                  lb_1st.Text = /*iCRM.Services.FirstOrDefault(s => s.FILE_NO == serv.FILE_NO).NAME_DNRM;*/ serv.NAME_DNRM;
                  rb_1st.Tag = serv.FILE_NO;

               }
               if (i == 1)
               {
                  lb_2nd.Visible = rb_2nd.Visible = true;
                  rb_2nd.ImageProfile = GetProfileImage(serv.FILE_NO);
                  lb_2nd.Text = /*iCRM.Services.FirstOrDefault(s => s.FILE_NO == serv.FILE_NO).NAME_DNRM;*/ serv.NAME_DNRM;
                  rb_2nd.Tag = serv.FILE_NO;
               }
               if (i == 2)
               {
                  lb_3rd.Visible = rb_3rd.Visible = true;
                  rb_3rd.ImageProfile = GetProfileImage(serv.FILE_NO);
                  lb_3rd.Text = /*iCRM.Services.FirstOrDefault(s => s.FILE_NO == serv.FILE_NO).NAME_DNRM;*/ serv.NAME_DNRM;
                  rb_3rd.Tag = serv.FILE_NO;
               }
               ++i;
            }

            if (comp.LOGO == null)
            {
               byte[] bytes = null;
               MemoryStream ms = new MemoryStream();
               Image img = Properties.Resources.IMAGE_1565;
               img.Save(ms, ImageFormat.Bmp);
               bytes = ms.ToArray();

               //comp.LOGO = bytes;
               CompanyLogo_Butn.ImageProfile = Properties.Resources.IMAGE_1565;
            }
            else
            {
               var stream = new MemoryStream(comp.LOGO.ToArray());
               CompanyLogo_Butn.ImageProfile = Image.FromStream(stream);
            }
            
         }
         catch
         {}
      }

      private Image GetProfileImage(long servfileno)
      {
         try
         {
            CompanyLogo_Butn.ImageProfile = null;
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

      private void rb_relatedservice_Click(object sender, EventArgs e)
      {
         try
         {
            var relatedservice = Convert.ToInt64((sender as System.MaxUi.RoundedButton).Tag);
            var serv = iCRM.Services.FirstOrDefault(s => s.FILE_NO  == relatedservice);

            if(serv.SRPB_TYPE_DNRM == "001")
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
            else if(serv.SRPB_TYPE_DNRM == "002")
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

      private void rb_4th_Click(object sender, EventArgs e)
      {
         try
         {
            var comp = CompBs.Current as Data.VF_CompaniesResult;
            if (comp == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 33 /* Execute Shw_Cont_F */),
                   new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 10 /* Execute Actn_CalF_P */)
                   {
                      Executive = ExecutiveType.Asynchronous,
                      Input = 
                        new XElement("Service", 
                           new XAttribute("onoftag", "on"),
                           new XAttribute("compcode", comp.CODE)
                        )
                   }
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);

         }
         catch
         { }
      }

      private void AddComp_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 47 /* Execute Regn_Dfin_F */),                
                new Job(SendType.SelfToUserInterface, "COMP_CHNG_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Company", 
                        new XAttribute("formtype", "add"),
                        new XAttribute("code", 0),
                        new XAttribute("formcaller", GetType().Name)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void AcntsSearch_Lov_EditValueChanging(object sender, ChangingEventArgs e)
      {
         AcntsSearch_Lov.Tag = e.NewValue;
         Execute_Query();
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
         Comp_Gv.OptionsFind.AlwaysVisible = ServInfoSearch_Pikb.PickChecked;
      }
   }
}
