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
            FromAttnDate_Date.CommitChanges();
            ToAttnDate_Date.CommitChanges();
            var dres = DresBs1.Current as Data.Dresser;
            if (dres == null) return;


            FighBs1.DataSource = 
               iScsc.Attendances
               .Where(a => 
                  a.ATTN_DATE.Date >= FromAttnDate_Date.Value.Value.Date &&
                  a.ATTN_DATE.Date <= ToAttnDate_Date.Value.Value.Date &&
                  a.Dresser_Attendances.Any(da => da.DRES_CODE == dres.CODE)
               ).Select(a => a.Fighter1).Distinct();

         }
         catch (Exception)
         {
            
         }
      }

      private void DresBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            Search_Butn_Click(null, null);
         }catch{}
      }

      private void FighBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var figh = FighBs1.Current as Data.Fighter;
            if (figh == null) return;

            UserProFile_Rb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            //Pb_FighImg.Visible = true;

            if (InvokeRequired)
               Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
            else
               UserProFile_Rb.ImageProfile = bm;
         }
         catch
         { //Pb_FighImg.Visible = false;
            UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
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
   }
}
