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
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace System.Scsc.Ui.Advertising
{
   public partial class ADV_BASE_F : UserControl
   {
      public ADV_BASE_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int index = default(int);

      private void Execute_Query()
      {         
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);

            int _ixAdvp = AdvpBs.Position;
            AdvpBs.DataSource = iScsc.Advertising_Parameters;
            AdvpBs.Position = _ixAdvp;

            Advc2_Gv.ActiveFilterString = string.Format("RECD_STAT = '002' AND CELL_PHON != ''");
            Advc3_Gv.ActiveFilterString = string.Format("RECD_STAT = '003' AND CELL_PHON != ''");
            requery = false;
         }
         catch { }
         finally { requery = false; }
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void AddAdvp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (AdvpBs.List.OfType<Data.Advertising_Parameter>().Any(a => a.CODE == 0)) return;

            var _advp = AdvpBs.AddNew() as Data.Advertising_Parameter;
            if (_advp == null) return;

            _advp.ADVP_NAME = "عنوان تخفیف";
            _advp.RECD_TYPE = "001";
            _advp.DSCT_TYPE = "001";
            _advp.STAT = "002";
            _advp.DSCT_AMNT = 10;
            _advp.DISC_CODE = Guid.NewGuid().ToString().Split('-')[0].ToUpper();
            _advp.NUMB_LAST_DAY = 0;
            _advp.EXPR_DATE = DateTime.Now.AddDays(7);

            iScsc.Advertising_Parameters.InsertOnSubmit(_advp);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelAdvp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Advertising_Parameters.DeleteOnSubmit(_advp);
            iScsc.SubmitChanges();
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

      private void SaveAdvp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            advp_gv.PostEditor();

            iScsc.SubmitChanges();
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

      private void Rqtp_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  _advp.RQTP_CODE = null;
                  break;
               default:
                  break;
            }

            advp_gv.PostEditor();
            iScsc.SubmitChanges();
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

      private void Ctgy_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  _advp.CTGY_CODE = null;
                  break;
               default:
                  break;
            }

            advp_gv.PostEditor();
            iScsc.SubmitChanges();
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

      private void SrchAdvp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            var _ctgys = 
               new XElement("Categories", 
                   new XAttribute("isctgy", CtgyFltr_Cbx.Checked) 
               );
            if(CtgyFltr_Cbx.Checked)
               foreach (var i in Ctgy_Gv.GetSelectedRows())
               {
                  var _item = Ctgy_Gv.GetRow(i) as Data.Category_Belt;
                  _ctgys.Add(
                     new XElement("Category",
                         new XAttribute("code", _item.CODE)
                     )
                  );                  
               }

            var _cochs =
               new XElement("Coaches",
                   new XAttribute("iscoch", CochFltr_Cbx.Checked)
               );
            if (CochFltr_Cbx.Checked)
               foreach (var i in Coch_Gv.GetSelectedRows())
               {
                  var _item = Coch_Gv.GetRow(i) as Data.Fighter;
                  _ctgys.Add(
                     new XElement("Coach",
                         new XAttribute("fileno", _item.FILE_NO)
                     )
                  );
               }

            var _orgns = 
               new XElement("Organs",
                   new XAttribute("isorgn", SuntFltr_Cbx.Checked) 
               );
            if(SuntFltr_Cbx.Checked)
               foreach (var i in Sunt_Gv.GetSelectedRows())
               {
                  var _item = Sunt_Gv.GetRow(i) as Data.Sub_Unit;
                  _orgns.Add(
                     new XElement("SubUnit",
                         new XAttribute("code", _item.BUNT_DEPT_ORGN_CODE + _item.BUNT_DEPT_CODE + _item.BUNT_CODE + _item.CODE)
                     )
                  );
               }

            var _grops = 
               new XElement("Grouping",
                   new XAttribute("isgrop", SGrpFltr_Cbx.Checked) 
               );
            if(SGrpFltr_Cbx.Checked)
               foreach (var i in SGrp_Gv.GetSelectedRows())
               {
                  var _item = SGrp_Gv.GetRow(i) as Data.App_Base_Define;
                  _grops.Add(
                     new XElement("Group",
                         new XAttribute("code", _item.CODE)
                     )
                  );
               }

            var _calls = 
               new XElement("Calling", 
                   new XAttribute("iscall", RCalFltr_Cbx.Checked) ,
                   new XAttribute("isfromcall", FromCall_Cbx.Checked),
                   new XAttribute("fromcall", FromCall_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : FromCall_Dt.Value.Value.ToString("yyyy-MM-dd"))
               );
            if(RCalFltr_Cbx.Checked)
               foreach (var i in RCal_Gv.GetSelectedRows())
               {
                  var _item = RCal_Gv.GetRow(i) as Data.App_Base_Define;               
                  _calls.Add(
                     new XElement("Call",
                         new XAttribute("code", _item.CODE)
                     )
                  );
               }

            var _survs = 
               new XElement("Call_Survey", 
                   new XAttribute("issurvey", RSurFltr_Cbx.Checked),
                   new XAttribute("isfromsurvey", FromSurvey_Cbx.Checked),
                   new XAttribute("fromsurvey", FromSurvey_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : FromSurvey_Dt.Value.Value.ToString("yyyy-MM-dd"))
               );
            if(RSurFltr_Cbx.Checked)
               foreach (var i in RSur_Gv.GetSelectedRows())
               {
                  var _item = RSur_Gv.GetRow(i) as Data.App_Base_Define;
                  _calls.Add(
                     new XElement("Survey",
                         new XAttribute("code", _item.CODE)
                     )
                  );
               }

            // 1401/07/19 * روزهای سرنگونی نظام کثیف آخوندی
            if(StrtCyclDate_Cbx.Checked)
            {
               if (!FromStrtCyclDate_Dt.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromStrtCyclDate_Dt.Focus(); return; }
               if (!ToStrtCyclDate_Dt.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToStrtCyclDate_Dt.Focus(); return; }
            }

            if (EndCyclDate_Cbx.Checked)
            {
               if (!FromEndCyclDate_Dt.Value.HasValue) { MessageBox.Show("تاریخ شروع را مشخص کنید"); FromEndCyclDate_Dt.Focus(); return; }
               if (!ToEndCyclDate_Dt.Value.HasValue) { MessageBox.Show("تاریخ پایان را مشخص کنید"); ToEndCyclDate_Dt.Focus(); return; }
            }

            iScsc.CRET_ADVP_P(
               new XElement("Advertising_Parameter",
                   new XAttribute("code", _advp.CODE),
                   new XElement("Sex",
                       new XAttribute("ismen", Men_Cbx.Checked),
                       new XAttribute("iswomen", Women_Cbx.Checked)
                   ),
                   new XElement("BirthDate",
                       new XAttribute("isfrombd", FromBd_Cbx.Checked),
                       new XAttribute("frombd", FromBd_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : FromBd_Dt.Value.Value.ToString("yyyy-MM-dd")),
                       new XAttribute("istobd", ToBd_Cbx.Checked),
                       new XAttribute("tobd", ToBd_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : ToBd_Dt.Value.Value.ToString("yyyy-MM-dd"))
                   ),
                   new XElement("EndCycle",
                       new XAttribute("isfromnumblastday", FromNumbLastDay_Cbx.Checked),
                       new XAttribute("fromnumblastday", FromNumbLastDay_Txt.EditValue ?? 0),
                       new XAttribute("istonumblastday", ToNumbLastDay_Cbx.Checked),
                       new XAttribute("tonumblastday", ToNumbLastDay_Txt.EditValue ?? 0)
                   ),
                   new XElement("Cycle",
                       new XAttribute("isstrtcycldate", StrtCyclDate_Cbx.Checked),
                       new XAttribute("fromstrtcycldate", FromStrtCyclDate_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : FromStrtCyclDate_Dt.Value.Value.ToString("yyyy-MM-dd")),
                       new XAttribute("tostrtcycldate", ToStrtCyclDate_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : ToStrtCyclDate_Dt.Value.Value.ToString("yyyy-MM-dd")),
                       
                       new XAttribute("isendcycldate", EndCyclDate_Cbx.Checked),
                       new XAttribute("fromendcycldate", FromEndCyclDate_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : FromEndCyclDate_Dt.Value.Value.ToString("yyyy-MM-dd")),
                       new XAttribute("toendcycldate", ToEndCyclDate_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : ToEndCyclDate_Dt.Value.Value.ToString("yyyy-MM-dd"))
                   ),
                   new XElement("Inviting",
                       new XAttribute("isnumbinvdir", NumbInvDir_Cbx.Checked),
                       new XAttribute("numbinvdir", NumbInvDir_Txt.EditValue ?? 0),
                       new XAttribute("isnumbinvndir", NumbInvNDir_Cbx.Checked),
                       new XAttribute("numbinvndir", NumbInvNDir_Txt.EditValue ?? 0)
                   ),
                   new XElement("Deposit",
                       new XAttribute("isnumbdpst", NumbDpst_Cbx.Checked),
                       new XAttribute("numbdpst", NumbDpst_Txt.EditValue ?? 0),
                       new XAttribute("issumdpst", SumDpst_Cbx.Checked),
                       new XAttribute("sumdpst", SumDpst_Txt.EditValue ?? 0),
                       new XAttribute("isfromdpst", FromDpst_Cbx.Checked),
                       new XAttribute("fromdpst", FromDpst_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : FromDpst_Dt.Value.Value.ToString("yyyy-MM-dd"))
                   ),
                   new XElement("Payment",
                       new XAttribute("isnumbpymt", NumbPymt_Cbx.Checked),
                       new XAttribute("numbpymt", NumbPymt_Txt.EditValue ?? 0),
                       new XAttribute("issumpymt", SumPymt_Cbx.Checked),
                       new XAttribute("sumpymt", SumPymt_Txt.EditValue ?? 0),
                       new XAttribute("isfrompymt", FromPymt_Cbx.Checked),
                       new XAttribute("frompymt", FromPymt_Dt.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : FromPymt_Dt.Value.Value.ToString("yyyy-MM-dd"))
                   ),
                   _ctgys,
                   _cochs,
                   _orgns,
                   _grops,
                   _calls,
                   _survs
               )               
            );

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

      private void AdvpBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            //ServBs.List.Clear();

            ServBs.DataSource =
               from f in iScsc.Fighters
               join s in iScsc.Advertising_Campaigns on f.FILE_NO equals s.FIGH_FILE_NO
               where s.ADVP_CODE == _advp.CODE &&
               (
                  (AdvcRecd002_Tsm.Checked && s.RECD_STAT == "002") || 
                  (AdvcRecd003_Tsm.Checked && s.RECD_STAT == "003" && s.RQST_RQID == null) || 
                  (AdvcRecd003Rqst_Tsm.Checked && s.RECD_STAT == "003" && s.RQST_RQID != null)
               )
               select f;

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ClerItem_Butn_Click(object sender, EventArgs e)
      {
         Param_Ro.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = false);
         Param_Ro.Controls.OfType<GroupBox>().ToList().ForEach(g => g.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = false));
      }

      private void ShowProfInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _serv = ServBs.Current as Data.Fighter;
            if (_serv == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", _serv.FILE_NO)
                           )
                     }
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SendDsct4Serv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            if (MessageBox.Show(this, "آیا با ثبت کدهای تخفیف برای مشتریان موافق هستید؟", "ارسال کد تخفیف", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            if (_advp.TEMP_TMID == null)
            {
               if (TempDsct_Lov.EditValue == null || TempDsct_Lov.EditValue.ToString() == "")
               {
                  Param_Ro.RolloutStatus = false;
                  Conf_Ro.RolloutStatus = true;
                  TempDsct_Lov.Focus();
                  return;
               }
            }
            else
               TempDsct_Lov.EditValue = _advp.TEMP_TMID;

            iScsc.SEND_DSCT_P(
               new XElement("Advertising_Parameter",
                   new XAttribute("code", _advp.CODE),
                   new XAttribute("tmid", TempDsct_Lov.EditValue ?? ""),
                   new XAttribute("sendsmsstat", SendSms_Cbx.Checked),
                   new XAttribute("sendappstat", SendApp_Cbx.Checked)
               )
            );

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

               var _advp = AdvpBs.Current as Data.Advertising_Parameter;              

               // نمایش تعداد کارت تخفیف های ثبت شده
               var _dscts =
                  iScsc.Fighter_Discount_Cards
                  .Where(d => d.ADVP_CODE == _advp.CODE
                     && d.EXPR_DATE.Value.Date >= DateTime.Now.Date
                     && d.STAT == "002"
                     && d.RQST_RQID == null);

               if (_dscts.Count() > 0)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new List<object>
                           {
                              ToolTipIcon.Info,
                              string.Format("تعداد {0} کد تخفیف برای مشتریان ثبت شد", _dscts.Count()),
                              "ثبت کد تخفیف",
                              2000
                           }
                     }
                  );
               }
            }
         }
      }

      private void OpenFile_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SlctFile_Ofd.ShowDialog() != Windows.Forms.DialogResult.OK)
               return;

            FilePath_Txt.Text = SlctFile_Ofd.FileName;            

            StreamReader sr = new StreamReader(SlctFile_Ofd.FileName);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
               if (line != "")
               {
                  if(CellList_Cxl.FindString(line) < 0)
                  {
                     CellList_Cxl.Items.Add(line);
                  }
               }
            }

            sr.Close();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelSlctCell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (CellList_Cxl.CheckedItems.Count == 0)
               return;

            if (MessageBox.Show(this, "آیا با حذف آیتم های انتخاب شده جدول موافق هستید؟", "حذف آیتم", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            foreach (var item in CellList_Cxl.CheckedItems.OfType<string>().ToList())
            {
               CellList_Cxl.Items.Remove(item);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelAllCell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با حذف همه آیتم های جدول موافق هستید؟", "حذف آیتم", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            CellList_Cxl.Items.Clear();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SendSlctCamp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            if (_advp.RECD_TYPE != "007")
            {
               _advp = AdvpBs.List.OfType<Data.Advertising_Parameter>().FirstOrDefault(a => a.RECD_TYPE == "007" && a.STAT == "002");
               if (_advp == null) return;
            }

            if (CellList_Cxl.CheckedItems.Count == 0 ||
                CellList_Cxl.CheckedItems.OfType<string>().Where(c => !_advp.Advertising_Campaigns.Any(ct => ct.CELL_PHON == c && ct.RECD_STAT == "002")).Count() == 0) return;

            iScsc.ExecuteCommand(
               string.Format("INSERT INTO dbo.Advertising_Campaign(Advp_Code, Code, Cell_Phon) VALUES {0};",
                  string.Join(",", CellList_Cxl.CheckedItems.OfType<string>().Where(c => !_advp.Advertising_Campaigns.Any(ct => ct.CELL_PHON == c && ct.RECD_STAT == "002")).Select(c => string.Format("({0}, dbo.Gnrt_Nvid_U(), '{1}')", _advp.CODE, c)))
               )
            );
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

      private void SendAllCamp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            for (int i = 0; i < CellList_Cxl.Items.Count; i++)
            {
               CellList_Cxl.SetItemChecked(i, true);
            }

            SendSlctCamp_Butn_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SlctAllCell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            for (int i = 0; i < CellList_Cxl.Items.Count; i++)
            {
               CellList_Cxl.SetItemChecked(i, true);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SlctInvrCell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            for (int i = 0; i < CellList_Cxl.Items.Count; i++)
            {
               CellList_Cxl.SetItemChecked(i, !CellList_Cxl.GetItemChecked(i));
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SlctNoneCell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            for (int i = 0; i < CellList_Cxl.Items.Count; i++)
            {
               CellList_Cxl.SetItemChecked(i, false);
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ShowAdvcProfInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advc = AdvcBs.Current as Data.Advertising_Campaign;
            if (_advc == null || _advc.FIGH_FILE_NO == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", _advc.FIGH_FILE_NO)
                           )
                     }
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SendDsct4Cell_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            if (MessageBox.Show(this, "آیا با ثبت کدهای تخفیف برای شماره تلفنها موافق هستید؟", "ارسال کد تخفیف", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            if (TempDsct_Lov.EditValue == null || TempDsct_Lov.EditValue.ToString() == "")
            {
               Param_Ro.RolloutStatus = false;
               Conf_Ro.RolloutStatus = true;
               TempDsct_Lov.Focus();
               return;
            }

            iScsc.SEND_DSCT_P(
               new XElement("Advertising_Parameter",
                   new XAttribute("code", _advp.CODE),
                   new XAttribute("tmid", TempDsct_Lov.EditValue ?? ""),
                   new XAttribute("sendsmsstat", SendSms_Cbx.Checked),
                   new XAttribute("sendappstat", SendApp_Cbx.Checked)
               )
            );

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

               var _advp = AdvpBs.Current as Data.Advertising_Parameter;

               // نمایش تعداد کارت تخفیف های ثبت شده
               var _dscts =
                  iScsc.Advertising_Campaigns
                  .Where(d => d.ADVP_CODE == _advp.CODE
                     && d.ACTN_DATE.Value.Date == DateTime.Now.Date
                     && d.RECD_STAT == "003"
                     && d.RQST_RQID == null);

               if (_dscts.Count() > 0)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new List<object>
                           {
                              ToolTipIcon.Info,
                              string.Format("تعداد {0} کد تخفیف برای شماره موبایل ثبت شد", _dscts.Count()),
                              "ثبت کد تخفیف",
                              2000
                           }
                     }
                  );
               }
            }
         }
      }

      private void TempDsct_Lov_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            if (TempDsct_Lov.EditValue == null || TempDsct_Lov.EditValue.ToString() == "") { TempDsct_Lov.Focus(); return; }

            switch (e.Button.Index)
            {
               case 1:
                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Advertising_Parameter SET TEMP_TMID = {0} WHERE CODE = {1};", TempDsct_Lov.EditValue, _advp.CODE));                  
                  break;
            }
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

      private void AdvcRecd00i_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var _advcRecd00i = sender as ToolStripMenuItem;
            if (_advcRecd00i == null) return;

            switch (_advcRecd00i.Tag.ToString())
            {
               case "002":
                  AdvcRecd002_Tsm.Checked = true;
                  AdvcRecd003_Tsm.Checked = AdvcRecd003Rqst_Tsm.Checked = false;
                  break;
               case "003":
                  AdvcRecd003_Tsm.Checked = true;
                  AdvcRecd002_Tsm.Checked = AdvcRecd003Rqst_Tsm.Checked = false;
                  break;
               case "003Rqst":
                  AdvcRecd003Rqst_Tsm.Checked = true;
                  AdvcRecd002_Tsm.Checked = AdvcRecd003_Tsm.Checked = false;
                  break;
               default:
                  break;
            }
            AdvpBs_CurrentChanged(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelAdvc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advc = AdvcBs.Current as Data.Advertising_Campaign;
            if (_advc == null) return;

            iScsc.ExecuteCommand(
               string.Format("DELETE dbo.Advertising_Campaign WHERE Code = {0};", _advc.CODE)
            );
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
   }
}
