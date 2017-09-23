using System;
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
using System.CRM.ExceptionHandlings;
using System.IO;
using System.MaxUi;
using DevExpress.XtraEditors;
using System.Drawing.Imaging;

namespace System.CRM.Ui.PublicInformation
{
   public partial class RLAT_SINF_F : UserControl
   {
      public RLAT_SINF_F()
      {
         InitializeComponent();         
      }

      private XElement xinput;
      private bool requery = false;
      private long fileno;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);

         ServBs.DataSource = iCRM.Services.Where(s => s.CONF_STAT == "002");
         CompBs.DataSource = iCRM.Companies.Where(c => c.RECD_STAT == "002");
         
         RlatBs.DataSource = iCRM.Relation_Infos.Where(sc => sc.SERV_FILE_NO == fileno);         
         Serv_Lov.EditValue = fileno;

         Serv_Rb_CheckedChanged(null, null);

         ApbsBs.DataSource = iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "RELATION_INFO" && a.REF_CODE == null);

         requery = false;
      }      

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RlatBs.EndEdit();

            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }      

      private void AddNewAppBase_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 79 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "RELATION_INFO"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ApbsList_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  if (ApbsList_Lov.EditValue == null || ApbsList_Lov.EditValue.ToString() == "") { ApbsList_Lov.Focus(); return; }
                  if (ApbsList2_Lov.EditValue == null || ApbsList2_Lov.EditValue.ToString() == "") { ApbsList2_Lov.EditValue = null; }
                  if (Serv_Rb.Checked && (Servs_Lov.EditValue == null || Servs_Lov.EditValue.ToString() == "")) { Servs_Lov.Focus(); Comps_Lov.EditValue = null; return; }
                  if (Comp_Rb.Checked && (Comps_Lov.EditValue == null || Comps_Lov.EditValue.ToString() == "")) { Comps_Lov.Focus(); Servs_Lov.EditValue = null; return; }
                  if (Serv_Rb.Checked) { Comps_Lov.EditValue = null; }
                  if (Comp_Rb.Checked) { Servs_Lov.EditValue = null; }



                  /*RlatBs.AddNew();
                  var rlat = RlatBs.Current as Data.Relation_Info;
                  
                  rlat.SERV_FILE_NO = fileno;
                  if (Serv_Rb.Checked)
                     rlat.RLAT_SERV_FILE_NO = (long)Servs_Lov.EditValue;
                  else if (Comp_Rb.Checked)
                     rlat.RLAT_COMP_CODE = (long)Comps_Lov.EditValue;

                  rlat.APBS_CODE = (long)ApbsList_Lov.EditValue;                  
                  Rlat_Gv.PostEditor();
                  RlatBs.EndEdit();

                  iCRM.SubmitChanges();*/

                  iCRM.INS_RLAT_P(fileno, null, 
                     (Servs_Lov.EditValue == null ||Servs_Lov.EditValue.ToString() == "") ? null : (long?)Servs_Lov.EditValue, 
                     (Comps_Lov.EditValue == null || Comps_Lov.EditValue.ToString() == "") ? null : (long?)Comps_Lov.EditValue, 
                     (long?)ApbsList_Lov.EditValue, 
                     (ApbsList2_Lov.EditValue == null || ApbsList2_Lov.EditValue.ToString() == "") ? null : (long?)ApbsList2_Lov.EditValue
                  );
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
            }
         }
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  var rlat = RlatBs.Current as Data.Relation_Info;

                  iCRM.DEL_RLAT_P(rlat.CODE);
                  requery = true;
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void Serv_Rb_CheckedChanged(object sender, EventArgs e)
      {
         if (Serv_Rb.Checked)
         {
            Servs_Lov.Enabled = true;
            Comps_Lov.Enabled = false;
         }
         else
         {
            Servs_Lov.Enabled = false;
            Comps_Lov.Enabled = true;
         }
      }

      private void RlatBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rlat = RlatBs.Current as Data.Relation_Info;
            if (rlat == null) return;

            if(rlat.RLAT_SERV_FILE_NO != null)
            {
               var serv = rlat.Service;
               Name_Txt.Text = serv.NAME_DNRM;
               Address_Txt.Text = serv.POST_ADRS_DNRM;

               // show image profile

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
            else if(rlat.RLAT_COMP_CODE != null)
            {
               var comp = rlat.Company1;
               Name_Txt.Text = comp.NAME;
               Address_Txt.Text = comp.POST_ADRS;

               // show image profile

               var stream = new MemoryStream(comp.LOGO.ToArray());
               ImageProfile_Butn.ImageProfile = Image.FromStream(stream);               
            }
         }
         catch (Exception exc)
         {
            ImageProfile_Butn.ImageProfile = System.CRM.Properties.Resources.IMAGE_1115;
            //MessageBox.Show(exc.Message);            
         }
      }

      private void ApbsList2_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         if (e.Button.Index == 1)
            ApbsList2_Lov.EditValue = null;
      }

      private void SubmitChangeRelation_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Rlat_Gv.PostEditor();
            RlatBs.EndEdit();

            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void RadarRelation_Butn_Click(object sender, EventArgs e)
      {
         var serv = ServBs.List.OfType<Data.Service>().First(s => s.FILE_NO == fileno) as Data.Service;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
            {
               Input =
                  new XElement("GMapNets",
                     new XAttribute("requesttype", "showrelationmarks"),
                     new XAttribute("formcaller", "Program:CRM:" + GetType().Name),
                     new XAttribute("callback", 40 /* CordinateGetSet */),
                     new XAttribute("outputtype", ""),
                     new XAttribute("initalset", false),
                     new XElement("RelationMarks",
                        new XElement("Source",
                           new XAttribute("type", "001"),
                           new XAttribute("code", fileno),
                           new XAttribute("name", serv.NAME_DNRM),
                           new XAttribute("postadrs", serv.POST_ADRS_DNRM),
                           new XAttribute("cordx", serv.CORD_X_DNRM ?? 0),
                           new XAttribute("cordy", serv.CORD_Y_DNRM ?? 0)
                        ),
                        new XElement("Relations",
                           RlatBs.List.OfType<Data.Relation_Info>().Where(r => r.RLAT_SERV_FILE_NO != null)
                           .Select(
                              r => new XElement("Relation",
                                      new XAttribute("type", "001"),
                                      new XAttribute("code", r.RLAT_SERV_FILE_NO),
                                      new XAttribute("name", r.Service.NAME_DNRM),
                                      new XAttribute("postadrs", r.Service.POST_ADRS_DNRM),
                                      new XAttribute("cordx", r.Service.CORD_X_DNRM ?? 0),
                                      new XAttribute("cordy", r.Service.CORD_Y_DNRM ?? 0)
                                   )
                           ),
                           RlatBs.List.OfType<Data.Relation_Info>().Where(r => r.RLAT_COMP_CODE != null)
                           .Select(
                              r => new XElement("Relation",
                                      new XAttribute("type", "002"),
                                      new XAttribute("code", r.RLAT_COMP_CODE),
                                      new XAttribute("name", r.Company1.NAME),
                                      new XAttribute("postadrs", r.Company1.POST_ADRS),
                                      new XAttribute("billcordx", r.Company1.BILL_ADDR_X ?? 0),
                                      new XAttribute("billcordy", r.Company1.BILL_ADDR_Y ?? 0),
                                      new XAttribute("shipcordx", r.Company1.SHIP_ADDR_X ?? 0),
                                      new XAttribute("shipcordy", r.Company1.SHIP_ADDR_Y ?? 0)
                                   )
                           )
                        )
                     )
                  )
            }
         );
      }

      private void ImageProfile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rlat = RlatBs.Current as Data.Relation_Info;
            if (rlat == null) return;

            Btn_Back_Click(null, null);

            if(rlat.RLAT_SERV_FILE_NO != null)
            {
               var serv = rlat.Service;
               if(serv.SRPB_TYPE_DNRM == "001")
               {
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),                
                         new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
               }
               else if(serv.SRPB_TYPE_DNRM == "002")
               {
                  Job _InteractWithCRM =
                    new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                         new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                       });
                  _DefaultGateway.Gateway(_InteractWithCRM);
               }
            }
            else if(rlat.RLAT_COMP_CODE != null)
            {
               var comp = rlat.Company1;
               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, 39 /* Execute Inf_Acnt_F */),                
                      new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Company", new XAttribute("code", comp.CODE))},
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
