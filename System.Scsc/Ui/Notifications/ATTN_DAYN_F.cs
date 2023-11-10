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
using System.MaxUi;
using System.Xml.Linq;
using System.Scsc.ExtCode;
using System.IO;

namespace System.Scsc.Ui.Notifications
{
   public partial class ATTN_DAYN_F : UserControl
   {
      public ATTN_DAYN_F()
      {
         InitializeComponent();
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
            iScsc = new Data.iScscDataContext(ConnectionString);

            FromAttnDate_Date.Value = FromAttnDate_Date.Value.HasValue ? FromAttnDate_Date.Value.Value : DateTime.Now;
            if (!ToAttnDate_Date.Value.HasValue)
               ToAttnDate_Date.Value = FromAttnDate_Date.Value;

            if (Tb_Master.SelectedTab == tp_001)
            {

               if (CBMT_CODE_GridLookUpEdit.EditValue == null || CBMT_CODE_GridLookUpEdit.EditValue.ToString() == "")
                  AttnBs1.DataSource =
                     iScsc.Attendances
                     .Where(a =>
                        a.ATTN_DATE.Date >= FromAttnDate_Date.Value.Value.Date &&
                        a.ATTN_DATE.Date <= ToAttnDate_Date.Value.Value.Date &&
                        a.ATTN_STAT == "002" &&
                        Fga_Uclb_U.Contains(a.CLUB_CODE)
                     );
               else
               {
                  var cbmtcode = (long?)CBMT_CODE_GridLookUpEdit.EditValue;
                  var cbmtobj = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.CODE == cbmtcode);

                  AttnBs1.DataSource =
                     iScsc.Attendances
                     .Where(a =>
                        a.ATTN_DATE.Date >= FromAttnDate_Date.Value.Value.Date &&
                        a.ATTN_DATE.Date <= ToAttnDate_Date.Value.Value.Date &&
                        (Coch_Pkb.PickChecked == false || a.COCH_FILE_NO == cbmtobj.COCH_FILE_NO) &&
                        (Mtod_Pkb.PickChecked == false || a.MTOD_CODE_DNRM == cbmtobj.MTOD_CODE) &&
                        (Cbmt_Pkb.PickChecked == false || a.CBMT_CODE_DNRM == cbmtobj.CODE) &&
                        a.ATTN_STAT == "002" &&
                        Fga_Uclb_U.Contains(a.CLUB_CODE)
                     );
               }
            }
            else if (Tb_Master.SelectedTab == tp_002)
            {
               VSTMbspBs.DataSource =
                  iScsc.V_Total_Member_Ships
                  .Where(h =>
                     FromAttnDate_Date.Value.Value.Date >= h.STRT_DATE.Value.Date &&
                     ToAttnDate_Date.Value.Value.Date <= h.END_DATE.Value.Date &&
                     !iScsc.Attendances
                     .Any(a =>
                        a.FIGH_FILE_NO == h.FILE_NO &&
                        a.CBMT_CODE_DNRM == h.CBMT_CODE &&
                        a.MBSP_RWNO_DNRM == h.MBSP_RWNO &&
                        a.COCH_FILE_NO == h.COCH_FILE_NO &&
                        a.ATTN_STAT == "002"
                     )
                  );
            }
         }catch(Exception exc)
         {
            CBMT_CODE_GridLookUpEdit.EditValue = null;
            Execute_Query();
         }
      }

      private void Reload_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){ Input = Keys.Escape },
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", _attn.FIGH_FILE_NO),
                                 new XAttribute("attndate",_attn.ATTN_DATE.Date),
                                 new XAttribute("attncode", _attn.CODE),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                           }
                        })
                  );
                  break;
               case 1:
                  if(_attn.EXIT_TIME != null && _attn.Club.Settings.Any(a => a.DRES_AUTO == "002"))
                  {
                     var dres = _attn.Dresser_Attendances.FirstOrDefault().Dresser as Data.Dresser;
                     if (dres == null) return;

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
                        {
                           Input =
                              new XElement("OprtDres",
                                    new XAttribute("type", "sendoprtdres"),
                                    new XAttribute("cmndname", dres.DRES_NUMB),
                                    new XAttribute("devip", dres.IP_ADRS),
                                    new XAttribute("cmndsend", dres.CMND_SEND ?? "")
                                  )
                        }
                     );
                  }
                  _attn.MDFY_DATE = DateTime.Now;
                  iScsc.SubmitChanges();
                  requery = true;
                  break;
               case 2:
                  Back_Butn_Click(null, null);
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _attn.FIGH_FILE_NO)) }
                  );
                  break;
               case 3:
                  if (_attn.EXIT_TIME == null)
                  {
                     if (MessageBox.Show(this, "با خروج دستی مشتری موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                     iScsc.INS_ATTN_P(_attn.CLUB_CODE, _attn.FIGH_FILE_NO, null, null, "003", _attn.MBSP_RWNO_DNRM, "001", "002");
                     requery = true;
                  }
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            //MsgBox.Show(exc.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, exc.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Btn_AutoExitAttn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "با خروج دستی همه مشترییان موافق هستید؟", "خروجی دستی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.AUTO_AEXT_P(new XElement("Process"));
            requery = true;
         }
         catch (Exception ex)
         {
            //MessageBox.Show(ex.Message);
            //var result = MsgBox.Show(ex.Message, "خطا", MsgBox.Buttons.OK, MsgBox.Icon.Error, MsgBox.AnimateStyle.FadeIn);
            MessageBox.Show(this, ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            if (requery)
            {
               requery = false;
               Execute_Query();
            }
         }
      }

      private void PrintDefault_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (!FromAttnDate_Date.Value.HasValue)
            {
               FromAttnDate_Date.Focus();
               return;
            }

            Back_Butn_Click(null, null);
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Attn_Date BETWEEN '{0}' AND '{1}'", FromAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd")))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void Print_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Back_Butn_Click(null, null);
            Job _InteractWithScsc =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {
                       new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Attn_Date BETWEEN '{0}' AND '{1}'", FromAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd"), ToAttnDate_Date.Value.Value.Date.ToString("yyyy-MM-dd")))}
                    });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void PrintSetting_Butn_Click(object sender, EventArgs e)
      {
         Back_Butn_Click(null, null);
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void ClearCbmt_Butn_Click(object sender, EventArgs e)
      {
         CBMT_CODE_GridLookUpEdit.EditValue = null;
      }

      private void AttnBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _attn = AttnBs1.Current as Data.Attendance;
            if (_attn == null) return;

            if(ServProFile_Rb.Tag == null || ServProFile_Rb.Tag.ToString().ToInt64() != _attn.FIGH_FILE_NO)
            {
               if (_attn.IMAG_RCDC_RCID_DNRM != null)
               {
                  try
                  {
                     ServProFile_Rb.ImageProfile = null;
                     MemoryStream mStream = new MemoryStream();
                     byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _attn.FIGH_FILE_NO))).ToArray();
                     mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                     Bitmap bm = new Bitmap(mStream, false);
                     mStream.Dispose();

                     if (InvokeRequired)
                        Invoke(new Action(() => ServProFile_Rb.ImageProfile = bm));
                     else
                        ServProFile_Rb.ImageProfile = bm;

                     ServProFile_Rb.Tag = _attn.FIGH_FILE_NO;
                  }
                  catch { }
               }
               else
               {
                  ServProFile_Rb.ImageProfile = null;
                  ServProFile_Rb.Tag = null;
               }
            }

            if(CochProFile_Rb.Tag == null || CochProFile_Rb.Tag.ToString().ToInt64() != _attn.COCH_FILE_NO)
            {
               if (_attn.Fighter != null && _attn.Fighter.IMAG_RCDC_RCID_DNRM != null)
               {
                  try
                  {
                     CochProFile_Rb.ImageProfile = null;
                     MemoryStream mStream = new MemoryStream();
                     byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", _attn.COCH_FILE_NO))).ToArray();
                     mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                     Bitmap bm = new Bitmap(mStream, false);
                     mStream.Dispose();

                     if (InvokeRequired)
                        Invoke(new Action(() => CochProFile_Rb.ImageProfile = bm));
                     else
                        CochProFile_Rb.ImageProfile = bm;

                     CochProFile_Rb.Tag = _attn.COCH_FILE_NO;
                  }
                  catch { }
               }
               else
               {
                  CochProFile_Rb.ImageProfile = null;
                  CochProFile_Rb.Tag = null;
               }
            }

            if (ServProFile_Rb.ImageProfile == null && _attn.Fighter1.SEX_TYPE_DNRM == "002")
               ServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1148;
            else if (ServProFile_Rb.ImageProfile == null)
               ServProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1149;

            if (_attn.Fighter != null)
            {
               CochProFile_Rb.Visible = true;
               if (CochProFile_Rb.ImageProfile == null && _attn.Fighter.SEX_TYPE_DNRM == "002")
                  CochProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1507;
               else if (CochProFile_Rb.ImageProfile == null)
                  CochProFile_Rb.ImageProfile = System.Scsc.Properties.Resources.IMAGE_1076;
            }
            else
               CochProFile_Rb.Visible = false;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AttnActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _HAttn = VSTMbspBs.Current as Data.V_Total_Member_Ship;
            if (_HAttn == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  Back_Butn_Click(null, null);
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _HAttn.FILE_NO)) }
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
