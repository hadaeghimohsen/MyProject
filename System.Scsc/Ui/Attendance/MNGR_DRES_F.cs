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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.Attendance
{
   public partial class MNGR_DRES_F : UserControl
   {
      public MNGR_DRES_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         
         int dres = DresBs1.Position;
         
         ComaBs1.DataSource = iScsc.Computer_Actions;
         
         DresBs1.Position = dres;

         //DresBlnk_Gv.ActiveFilterString = "ORDR Is Null";

         requery = false;
      }

      private void SaveChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DresBs1.EndEdit();

            iScsc.SubmitChanges();

            requery = true;
         }
         catch { }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FromDate_Date.CommitChanges();
            ToDate_Date.CommitChanges();

            var dres = DresBs1.Current as Data.Dresser;
            if (dres == null) return;


            //FighBs1.DataSource = 
            //   iScsc.Attendances
            //   .Where(a => 
            //      a.ATTN_DATE.Date >= FromAttnDate_Date.Value.Value.Date &&
            //      a.ATTN_DATE.Date <= ToAttnDate_Date.Value.Value.Date &&
            //      a.Dresser_Attendances.Any(da => da.DRES_CODE == dres.CODE)
            //   ).Select(a => a.Fighter1).Distinct();

            V_DratBs1.DataSource =
               iScsc.V_Drats
               .Where(vd =>
                  vd.DRAT_DATE.Value.Date >= FromDate_Date.Value.Value.Date &&
                  vd.DRAT_DATE.Value.Date <= ToDate_Date.Value.Value.Date
               );
         }
         catch (Exception)
         {
            
         }
      }

      private void cMND_SENDTextEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var dres = DresBs1.Current as Data.Dresser;
            if (dres == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface) 
               { 
                  Input = 
                     new XElement("OprtDres", 
                           new XAttribute("type", "sendoprtdres"),
                           //new XAttribute("portname", dres.COMM_PORT),
                           new XAttribute("cmndname", dres.DRES_NUMB),
                           new XAttribute("devip", dres.IP_ADRS),
                           new XAttribute("cmndsend", dres.CMND_SEND ?? "")
                         )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CretDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _coma = ComaBs1.Current as Data.Computer_Action;
            if (_coma == null) return;

            for (int i = (int)FromNumb_Txt.Value; i <= ToNumb_Txt.Value; i++)
            {
               iScsc.ExecuteCommand(
                  string.Format(
                  "INSERT INTO dbo.Dresser(Coma_Code, Code, Dres_Numb, Rec_Stat, IP_Adrs, Ordr, Cmnd_Send)" +
                  "SELECT {0}, 0, {1}, '002', '{2}', {1}, dbo.GET_LPAD_U('{1}', 3, '0') WHERE NOT EXISTS (" + 
                  "SELECT * FROM dbo.Dresser d WHERE d.Coma_Code = {0} AND Dres_Numb = {1} );",
                  _coma.CODE, i,
                  IP_Txt.Text
                  )
               );
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

      private void SaveDres_Btn_Click(object sender, EventArgs e)
      {
         try
         {
            DresBs1.EndEdit();
            Dres_Gv.PostEditor();

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

      private void Test_Butn_Click(object sender, EventArgs e)
      {
         if (IPAdrs_Txt.Text == "") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
            {
               Input =
                  new XElement("OprtDres",
                      new XAttribute("type", "sendoprtdres"),                     
                      new XAttribute("cmndname", "test"),
                      new XAttribute("devip", IPAdrs_Txt.Text)
                  )
            }
         );
      }

      private void OpenAll_Butn_Click(object sender, EventArgs e)
      {
         if (IPAdrs_Txt.Text == "") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
            {
               Input =
                  new XElement("OprtDres",
                      new XAttribute("type", "sendoprtdres"),
                      new XAttribute("cmndname", "all"),
                      new XAttribute("devip", IPAdrs_Txt.Text)
                  )
            }
         );
      }

      private void RunTestLocker_Butn_Click(object sender, EventArgs e)
      {
         TestLocker_Tmr.Enabled = true;
         RunTestLocker_Butn.Enabled = false;
         TestLocker_Tmr.Interval = 500;
      }

      private void TestLocker_Tmr_Tick(object sender, EventArgs e)
      {         
         try
         {
            TestLocker_Tmr.Interval = 100000000;
            foreach (var dres in DresBs1.List.OfType<Data.Dresser>()/*.Where(d => d.ORDR < 13)*/.OrderBy(r => r.ORDR))
            {
               DresBs1.Position = DresBs1.IndexOf(dres);
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
               Threading.Thread.Sleep((int)(Wait_Nud.Value * 1000));
            }
            TestLocker_Tmr.Enabled = false;
            RunTestLocker_Butn.Enabled = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      List<Data.Dresser> _dressersList = new List<Data.Dresser>();
      Random _rndm = new Random();
      private void Start_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            TestLockerInLoop_Tmr.Enabled = false;
            TestLockerInLoop_Tmr.Interval = (int)(Wait_Nud.Value * 1000);

            if (_dressersList.Count == DresBs1.List.OfType<Data.Dresser>().Where(d => d.REC_STAT == "002" && (All_Rb.Checked || (Even_Rb.Checked && d.ORDR % 2 == 0) || (Odd_Rb.Checked && d.ORDR % 2 != 0))).Count() && InftLoop_Cbx.Checked)
               _dressersList.Clear();

            if (_dressersList.Count == DresBs1.List.OfType<Data.Dresser>().Where(d => d.REC_STAT == "002" && (All_Rb.Checked || (Even_Rb.Checked && d.ORDR % 2 == 0) || (Odd_Rb.Checked && d.ORDR % 2 != 0))).Count())
               return;

            int _slct = 0;
            var _validDressers = DresBs1.List.OfType<Data.Dresser>().Where(d => d.REC_STAT == "002" && !_dressersList.Any(dd => dd.CODE == d.CODE) && (All_Rb.Checked || (Even_Rb.Checked && d.ORDR % 2 == 0) || (Odd_Rb.Checked && d.ORDR % 2 != 0)));
            Data.Dresser _iRec = null;

            _slct = _rndm.Next(0, _validDressers.Count());
            _iRec = _validDressers.ToArray()[_slct];

            //if(All_Rb.Checked)
            //{
            //   _slct = _rndm.Next(0, _validDressers.Count());
            //   _iRec = _validDressers.ToArray()[_slct];
            //}
            //else if(Even_Rb.Checked)
            //{
            //   _slct = _rndm.Next(0, _validDressers.Count());
            //   _iRec = _validDressers.ToArray()[_slct];
            //}
            //else if (Odd_Rb.Checked)
            //{

            //}

            _dressersList.Add(_iRec);
            DresBs1.Position = DresBs1.IndexOf(_iRec);
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("OprtDres",
                           new XAttribute("type", "sendoprtdres"),
                           new XAttribute("cmndname", _iRec.DRES_NUMB),
                           new XAttribute("devip", _iRec.IP_ADRS),
                           new XAttribute("cmndsend", _iRec.CMND_SEND ?? "")
                         )
               }
            );

            TestLockerInLoop_Tmr.Enabled = true;
         }
         catch { }
      }

      private void Stop_Butn_Click(object sender, EventArgs e)
      {
         TestLockerInLoop_Tmr.Enabled = false;
      }

      private void TestLockerInLoop_Tmr_Tick(object sender, EventArgs e)
      {
         Start_Butn_Click(null, null);
      }

      private void All_Rb_CheckedChanged(object sender, EventArgs e)
      {
         _dressersList.Clear();
      }

      private void SetLockerGust_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _coma = ComaBs1.Current as Data.Computer_Action;
            if (_coma == null) return;

            XElement _xret = new XElement("Result");

            foreach (var _locker in _coma.Dressers.Where(l => !iScsc.Fighters.Any(f => f.FNGR_PRNT_DNRM == l.CMND_SEND)))
            {
               iScsc.RunnerdbCommand(
                  new XElement("Router_Command",
                      new XAttribute("crntuser", CurrentUser),
                      new XAttribute("subsys", 5),
                      new XAttribute("cmndcode", 100),
                      new XAttribute("frstname", _locker.DRES_NUMB),
                      new XAttribute("lastname", "شماره کمد"),
                      new XAttribute("fngrprnt", _locker.CMND_SEND),
                      new XAttribute("chatid", ""),
                      new XAttribute("cellphon", ""),
                      new XAttribute("natlcode", ""),
                      new XElement("Expense",                          
                          new XAttribute("rqtpcode", "025")
                      )
                  ),
                  ref _xret
               );
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

      private void SendCmnd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var dres = DresBs1.Current as Data.Dresser;
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
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void UpdtDstnNumb_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DresBs1.List.OfType<Data.Dresser>().ToList().ForEach(d => d.ORDR = null);
            //DresBlnk_Gv.ActiveFilterString = "ORDR Is Null";
            //int _indx = 0,
            //    _len = (int)DstnNumb_Nud.Value,
            //    _sec = (DresBs1.List.Count % _len == 0) ? DresBs1.List.Count / _len : (DresBs1.List.Count / _len) + 1,
            //    _ii = 0;
            
            //for (int i = 1; i <= DresBs1.List.Count; i++)
            //{
            //   if (_indx >= _len) _indx = 0;
            //   var _locker = DresBs1.List.OfType<Data.Dresser>().FirstOrDefault(d => d.DRES_NUMB == i);
            //   if (_indx == 0) { ++_ii; _locker.ORDR = _ii; }
            //   else _locker.ORDR = (_sec + _ii) + (_len * (_indx - 1));
            //   ++_indx;
            //}

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

      private void ActvDactv_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _dres = DresBs1.Current as Data.Dresser;
            if (_dres == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           #region Access Privilege
                           new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                           {
                              Input = new List<string> 
                              {
                                 "<Privilege>271</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                 {
                                    _dres.REC_STAT = _dres.REC_STAT == "002" ? "001" : "002";
                                    return;
                                 }
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 271 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),                           
                  })
            ); 
            //_dres.REC_STAT = _dres.REC_STAT == "002" ? "001" : "002";
            
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

      private void OrdrSet_Butn_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var _dres = DresBs1.Current as Data.Dresser;
            if (_dres == null) return;

            _dres.ORDR = DresBs1.List.OfType<Data.Dresser>().Where(d => d.ORDR != null).Count() + 1;

            //DresBlnk_Gv.ActiveFilterString = "ORDR Is Null";

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

      private void Ordr_Nud_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var _dres = DresBs1.Current as Data.Dresser;
            if (_dres == null) return;

            _dres.ORDR = null;

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

      private void DelDres_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _coma = ComaBs1.Current as Data.Computer_Action;
            if (_coma == null) return;

            if (MessageBox.Show(this, "آیا با حذف کمد ها موافق هستید؟", "حذف کمدها", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(string.Format("DELETE dbo.Dresser WHERE Coma_Code = {0};", _coma.CODE));
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

      private void SUNT_CODELookUpEdit_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 #region Access Privilege
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>171</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       #region Show Error
                                       MessageBox.Show("خطا: عدم دسترسی به کد 171");
                                       #endregion                           
                                    })
                                 },
                                 new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                                 {
                                    Input = new List<string> 
                                    {
                                       "<Privilege>175</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       #region Show Error
                                       MessageBox.Show("خطا: عدم دسترسی به کد 175");
                                       #endregion                           
                                    })
                                 }
                                 #endregion
                              }),
                           #region DoWork
                           new Job(SendType.Self, 108 /* Execute Orgn_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 10 /* Actn_CalF_P */)
                           #endregion
                           })
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

      private void FADratBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _fa = FADratBs.Current as Data.Dresser_Attendance;
            if (_fa == null) return;

            var _vdrat = V_DratBs1.Current as Data.V_Drat;
            if(_vdrat == null) return;

            if (!Fa_Rlt.RolloutStatus) return;
            //var _dres = DresBs1.Current as Data.Dresser;
            //if(_dres == null) return;

            DratBs.DataSource =
               iScsc.Dresser_Attendances
               .Where(da =>
                  da.CRET_DATE.Value.Date == _vdrat.DRAT_DATE.Value.Date &&
                  da.FIGH_FILE_NO == _fa.FIGH_FILE_NO &&
                  da.ATTN_CODE == _fa.ATTN_CODE
                  //da.DRES_CODE == _dres.CODE
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FRDratBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _fr = FRDratBs.Current as Data.Dresser_Attendance;
            if (_fr == null) return;

            var _vdrat = V_DratBs1.Current as Data.V_Drat;
            if (_vdrat == null) return;

            if (!Fr_Rlt.RolloutStatus) return;
            //var _dres = DresBs1.Current as Data.Dresser;
            //if(_dres == null) return;

            DratBs.DataSource =
               iScsc.Dresser_Attendances
               .Where(da =>
                  da.CRET_DATE.Value.Date == _vdrat.DRAT_DATE.Value.Date &&
                  da.FIGH_FILE_NO == _fr.FIGH_FILE_NO &&
                  da.RQST_RQID == _fr.RQST_RQID
               //da.DRES_CODE == _dres.CODE
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FMDratBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _fm = FMDratBs.Current as Data.Dresser_Attendance;
            if (_fm == null) return;

            var _vdrat = V_DratBs1.Current as Data.V_Drat;
            if (_vdrat == null) return;

            if (!Fm_Rlt.RolloutStatus) return;
            //var _dres = DresBs1.Current as Data.Dresser;
            //if(_dres == null) return;

            DratBs.DataSource =
               iScsc.Dresser_Attendances
               .Where(da =>
                  da.CRET_DATE.Value.Date == _vdrat.DRAT_DATE.Value.Date &&
                  da.FIGH_FILE_NO == _fm.FIGH_FILE_NO &&
                  da.MBSP_RWNO == _fm.MBSP_RWNO
               //da.DRES_CODE == _dres.CODE
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void V_DratBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _vdrat = V_DratBs1.Current as Data.V_Drat;
            if (_vdrat == null) return;

            var _dres = DresBs1.Current as Data.Dresser;
            if (_dres == null) return;

            if (Fa_Rlt.RolloutStatus)
               FADratBs.DataSource =
                  iScsc.Dresser_Attendances
                  .Where(da =>
                     da.ATTN_CODE != null &&
                     da.Attendance.ATTN_DATE.Date == _vdrat.DRAT_DATE.Value.Date &&
                     da.DRES_CODE == _dres.CODE
                  );

            if (Fm_Rlt.RolloutStatus)
               FMDratBs.DataSource =
                  iScsc.Dresser_Attendances
                  .Where(da =>
                     da.MBSP_RWNO != null &&
                     da.CRET_DATE.Value.Date == _vdrat.DRAT_DATE.Value.Date &&
                     da.DRES_CODE == _dres.CODE                     
                  );

            if (Fr_Rlt.RolloutStatus)
               FRDratBs.DataSource =
                  iScsc.Dresser_Attendances
                  .Where(da =>
                     da.RQST_RQID != null &&
                     da.Request.SAVE_DATE.Value.Date == _vdrat.DRAT_DATE.Value.Date &&
                     da.DRES_CODE == _dres.CODE
                  );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DresBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _dres = DresBs1.Current as Data.Dresser;
            if (_dres == null) return;

            V_DratBs1_CurrentChanged(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Fa_Rlt_Click(object sender, EventArgs e)
      {
         Fm_Rlt.RolloutStatus = Fr_Rlt.RolloutStatus = false;
      }

      private void Fr_Rlt_Click(object sender, EventArgs e)
      {
         Fa_Rlt.RolloutStatus = Fm_Rlt.RolloutStatus = false;
      }

      private void Fm_Rlt_Click(object sender, EventArgs e)
      {
         Fa_Rlt.RolloutStatus = Fr_Rlt.RolloutStatus = false;
      }
   }
}
