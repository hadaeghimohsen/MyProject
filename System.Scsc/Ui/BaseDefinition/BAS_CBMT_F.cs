using System;
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
   public partial class BAS_CBMT_F : UserControl
   {
      public BAS_CBMT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long code;
      private string formCaller = "";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }      

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         int coch = CochBs1.Position;
         int mtod = MtodBs1.Position;
         int club = ClubBs1.Position;
         RequeryClubMethod_Butn_Click(null, null);
         CochBs1.Position = coch;
         MtodBs1.Position = mtod;
         ClubBs1.Position = club;
      }

      private void QWkdy00i_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SimpleButton sb = sender as SimpleButton;

            if (sb.Appearance.BackColor == Color.LightGray)
            {
               sb.Appearance.BackColor = Color.GreenYellow;
            }
            else
            {
               sb.Appearance.BackColor = Color.LightGray;
            }
         }
         catch { }
      }

      private void RequeryClubMethod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var weekdays = new List<string>();
            //CommandCbmt_Pnl.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.GreenYellow).ToList().ForEach(sb => weekdays.Add(string.Format("'{0}'", sb.Tag.ToString())));
            CommandCbmt_Pnl.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.GreenYellow).ToList().ForEach(sb => weekdays.Add(sb.Tag.ToString()));

            ClubWkdy2_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
            ClubWkdy1_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
            CtgyBs1.List.Clear();
            AttnBs1.List.Clear();
            VCochMbspBs1.List.Clear();

            if (weekdays.Count == 0)
            {
               CbmtBs1.DataSource =
                  iScsc.Club_Methods.Where(cm => true == false);               
               return;
            }
            else
            {
               var strttime = TimeSpan.Parse(QStrtTime_Tim.Text);
               var endtime = TimeSpan.Parse(QEndTime_Tim.Text);

               if (Tb_Master.SelectedTab == tp_001)
               {
                  CbmtBs1.DataSource =
                     iScsc.Club_Methods.Where(cm =>
                        cm.Club_Method_Weekdays.Any(cmw => cmw.STAT == "002" && weekdays.Contains(cmw.WEEK_DAY)) &&
                           //((cm.STRT_TIME >= strttime && cm.END_TIME <= endtime) ||
                           // (cm.STRT_TIME <= strttime && cm.END_TIME >= endtime) ||
                           // ((cm.STRT_TIME <= strttime && cm.END_TIME >= strttime) && cm.END_TIME <= endtime) ||
                           // ((cm.STRT_TIME >= strttime && cm.STRT_TIME <= endtime)))
                        ((cm.STRT_TIME.CompareTo(strttime) >= 0 && cm.END_TIME.CompareTo(endtime) <= 0) ||
                         (cm.STRT_TIME.CompareTo(strttime) <= 0 && cm.END_TIME.CompareTo(endtime) >= 0) ||
                         (cm.STRT_TIME.CompareTo(strttime) <= 0 && cm.END_TIME.CompareTo(strttime) >= 0 && cm.END_TIME.CompareTo(endtime) <= 0) ||
                         (cm.STRT_TIME.CompareTo(strttime) >= 0 && cm.STRT_TIME.CompareTo(endtime) <= 0))
                     );
               }
               else if(Tb_Master.SelectedTab == tp_002)
               {
                  var coch = CochBs1.Current as Data.Fighter;
                  var mtod = MtodBs1.Current as Data.Method;
                  var club = ClubBs1.Current as Data.Club;

                  CbmtBs1.DataSource =
                     iScsc.Club_Methods.Where(cm =>
                        cm.CLUB_CODE == club.CODE &&
                        cm.COCH_FILE_NO == coch.FILE_NO &&
                        cm.MTOD_CODE == mtod.CODE &&
                        cm.Club_Method_Weekdays.Any(cmw => cmw.STAT == "002" && weekdays.Contains(cmw.WEEK_DAY)) &&
                           //((cm.STRT_TIME >= strttime && cm.END_TIME <= endtime) ||
                           // (cm.STRT_TIME <= strttime && cm.END_TIME >= endtime) ||
                           // ((cm.STRT_TIME <= strttime && cm.END_TIME >= strttime) && cm.END_TIME <= endtime) ||
                           // ((cm.STRT_TIME >= strttime && cm.STRT_TIME <= endtime)))
                        ((cm.STRT_TIME.CompareTo(strttime) >= 0 && cm.END_TIME.CompareTo(endtime) <= 0) ||
                         (cm.STRT_TIME.CompareTo(strttime) <= 0 && cm.END_TIME.CompareTo(endtime) >= 0) ||
                         (cm.STRT_TIME.CompareTo(strttime) <= 0 && cm.END_TIME.CompareTo(strttime) >= 0 && cm.END_TIME.CompareTo(endtime) <= 0) ||
                         (cm.STRT_TIME.CompareTo(strttime) >= 0 && cm.STRT_TIME.CompareTo(endtime) <= 0))
                     );
               }
               else if(Tb_Master.SelectedTab == tp_003)
               {
                  var coch = CochBs1.Current as Data.Fighter;
                  if (coch == null) return;
                  
                  MtodBs3.DataSource = iScsc.Methods.Where(m => m.Club_Methods.Where(cm => cm.COCH_FILE_NO == coch.FILE_NO && cm.MTOD_STAT == "002").Count() > 0);
               }
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null)
               {
                  //CbmtBs2.List.Clear();
                  //ClubWkdy_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
                  return;
               }
            }
            tb_cbmt1_SelectedIndexChanged(null, null);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void CbmtBs1_CurrentChanged(object sender, EventArgs e)
      {
         var cbmt = CbmtBs1.Current as Data.Club_Method;
         if (cbmt == null) return;

         if (Tb_Master.SelectedTab == tp_001 || Tb_Master.SelectedTab == tp_002)
         {
            CochName2_Lb.Text = CochName1_Lb.Text = cbmt.Fighter.NAME_DNRM;
            //FngrPrnt1_Lb.Text = cbmt.Fighter.FNGR_PRNT_DNRM == "" ? "نامشخص" : cbmt.Fighter.FNGR_PRNT_DNRM;

            CbmtwkdyBs1.DataSource = cbmt.Club_Method_Weekdays.ToList();

            if (CbmtwkdyBs1.List.Count == 0)
            {
               ClubWkdy1_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               ClubWkdy2_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               return;
            }

            foreach (var wkdy in CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>())
            {
               var rslt = ClubWkdy1_Spn.Panel2.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
               rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;

               rslt = ClubWkdy2_Spn.Panel2.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
               rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;
            }

            CtgyBs1.DataSource = iScsc.Category_Belts.Where(cb => cb.MTOD_CODE == cbmt.MTOD_CODE && cb.CTGY_STAT == "002");

            var listMbsp =
               iScsc.Member_Ships
               .Where(ms =>
                  ms.RECT_CODE == "004" &&
                  ms.VALD_TYPE == "002" &&
                  ms.STRT_DATE.Value.Date <= DateTime.Now.Date &&
                  ms.END_DATE.Value.Date >= DateTime.Now.Date &&
                  (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM) &&
                  ms.Fighter_Public.CBMT_CODE == cbmt.CODE
               );

            ActvMembCount2_Lb.Text = ActvMembCount1_Lb.Text = listMbsp.Count().ToString();
            AgeMemb2_Lb.Text = AgeMemb1_Lb.Text = string.Join(", ", listMbsp.Select(ms => (DateTime.Now.Year - ms.Fighter.BRTH_DATE_DNRM.Value.Year).ToString()).Distinct().OrderBy(f => f).ToList());

            try
            {
               CochProFile2_Rb.ImageVisiable = CochProFile1_Rb.ImageVisiable = true;
               CochProFile1_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", cbmt.COCH_FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               //Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => CochProFile2_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = bm));
               else
                  CochProFile2_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = bm;
            }
            catch
            { 
               CochProFile2_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
            }
         }

         tb_cbmt1_SelectedIndexChanged(null, null);
      }

      private void Cbmt_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            if (e.Column.FieldName == "colUbStrtTime")
            {
               var row = e.Row as Data.Club_Method;
               if (e.IsGetData)
               {
                  if (row.STRT_TIME == null)
                     e.Value = new DateTime();
                  else
                     e.Value = new DateTime(((TimeSpan)row.STRT_TIME).Ticks);
               }
               if (e.IsSetData)
               {
                  if (e.Value is DateTime)
                     row.STRT_TIME = new TimeSpan(((DateTime)e.Value).Ticks);
               }
            }
            else if (e.Column.FieldName == "colUbEndTime")
            {
               var row = e.Row as Data.Club_Method;
               if (e.IsGetData)
               {
                  if (row.END_TIME == null)
                     e.Value = new DateTime();
                  else
                     e.Value = new DateTime(((TimeSpan)row.END_TIME).Ticks);
               }
               if (e.IsSetData)
               {
                  if (e.Value is DateTime)
                     row.END_TIME = new TimeSpan(((DateTime)e.Value).Ticks);
               }
            }
         }
         catch { }
      }

      private void tb_cbmt1_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (tb_cbmt1.SelectedTab == tp_0012 || tb_cbmt2.SelectedTab == tp_0022)
            {
               AttnBs1.List.Clear();
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               if (!ReloadAttn6_Cb.Checked) return;

               iScsc.CommandTimeout = 18000;

               var actvmbsp =
                  iScsc.VF_Coach_MemberShip(
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
                     )
                  );

               AttnBs1.DataSource =
                  iScsc.Attendances
                  .Where(a => actvmbsp.Any(am => am.FILE_NO == a.FIGH_FILE_NO && am.RWNO == a.MBSP_RWNO_DNRM));
            }
            else if (tb_cbmt1.SelectedTab == tp_0013 || tb_cbmt2.SelectedTab == tp_0023)
            {
               AttnBs1.List.Clear();
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               if (!ReloadAttn6_Cb.Checked) return;

               iScsc.CommandTimeout = 18000;

               VCochMbspBs1.DataSource =
                  iScsc.VF_Coach_MemberShip(
                     new XElement("Club_Method",
                        new XAttribute("code", cbmt.CODE)
                     )
                  );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NextCbmt1_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs1.MoveNext();
      }

      private void BackCbmt1_Butn_Click(object sender, EventArgs e)
      {
         CbmtBs1.MovePrevious();
      }

      private void AddNewMbsp_Butn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            long? cbmtcode = null, ctgycode = null;
            if (Tb_Master.SelectedTab == tp_001 || Tb_Master.SelectedTab == tp_002)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               var ctgy = CtgyBs1.Current as Data.Category_Belt;
               if (ctgy == null) return;

               cbmtcode = cbmt.CODE;
               ctgycode = ctgy.CODE;
            }


            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                     new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("type", "admcbmt"),
                              new XAttribute("cbmtcode", cbmtcode),
                              new XAttribute("ctgycode", ctgycode)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void GropMbsp_Butn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            long? cbmtcode = null;
            DateTime? date = null;
            if (Tb_Master.SelectedTab == tp_001 || Tb_Master.SelectedTab == tp_002)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if(Tb_Master.SelectedTab == tp_001)
                  if (!AttnDate1_Dt.Value.HasValue)
                     date = AttnDate1_Dt.Value = DateTime.Now;
                  else
                     date = AttnDate1_Dt.Value;
               else if(Tb_Master.SelectedTab == tp_002)
                  if (!AttnDate2_Dt.Value.HasValue)
                     date = AttnDate2_Dt.Value = DateTime.Now;
                  else
                     date = AttnDate2_Dt.Value;
            }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                    
                     new Job(SendType.Self, 121 /* Execute Aop_Mbsp_F */),
                     new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Member_Ship",
                              new XAttribute("cbmtcode", cbmtcode),
                              new XAttribute("attndate", date.Value.Date)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void GropAttn_Butn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            long? cbmtcode = null;
            DateTime? date = null;
            if (Tb_Master.SelectedTab == tp_001 || Tb_Master.SelectedTab == tp_002)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if (Tb_Master.SelectedTab == tp_001)
                  if (!AttnDate1_Dt.Value.HasValue)
                     date = AttnDate1_Dt.Value = DateTime.Now;
                  else
                     date = AttnDate1_Dt.Value;
               else if (Tb_Master.SelectedTab == tp_002)
                  if (!AttnDate2_Dt.Value.HasValue)
                     date = AttnDate2_Dt.Value = DateTime.Now;
                  else
                     date = AttnDate2_Dt.Value;
            }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                    
                     new Job(SendType.Self, 126 /* Execute Aop_Attn_F */),
                     new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Attendance",
                              new XAttribute("cbmtcode", cbmtcode),
                              new XAttribute("attndate", date.Value.Date)
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void Submit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            long? cbmtcode = null, ctgycode = null;
            if (Tb_Master.SelectedTab == tp_001 || Tb_Master.SelectedTab == tp_002)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               var ctgy = CtgyBs1.Current as Data.Category_Belt;
               if (ctgy == null) return;

               cbmtcode = cbmt.CODE;
               ctgycode = ctgy.CODE;
            }


            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     //new Job(SendType.Self, 123 /* Execute Adm_Figh_F */),
                     new Job(SendType.SelfToUserInterface, formCaller, 10 /* Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Request", 
                              new XAttribute("type", "admcbmt"),
                              new XAttribute("cbmtcode", cbmtcode),
                              new XAttribute("ctgycode", ctgycode)
                           ),
                        Next = (formCaller == "MBSP_CHNG_F" ? new Job(SendType.SelfToUserInterface, formCaller, 03 /* Execute Paint */) : null)
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            Back_Butn_Click(null, null);
         }
         catch { }
      }

      private void ClearParm_Butn_Click(object sender, EventArgs e)
      {
         CommandCbmt_Pnl.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.GreenYellow).ToList().ForEach(sb => sb.Appearance.BackColor = Color.LightGray);
      }

      private void SelectAllParm_Butn_Click(object sender, EventArgs e)
      {
         CommandCbmt_Pnl.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.LightGray).ToList().ForEach(sb => sb.Appearance.BackColor = Color.GreenYellow);
      }

      private void SaveWkdy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Data.Club_Method c = null;
            if (Tb_Master.SelectedTab == tp_001 || Tb_Master.SelectedTab == tp_002)
               c = CbmtBs1.Current as Data.Club_Method;

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                     new XElement("Update",
                        new XElement("Club_Method",
                           new XAttribute("code", c.CODE),
                           new XAttribute("clubcode", c.CLUB_CODE),
                           new XAttribute("mtodcode", c.MTOD_CODE),
                           new XAttribute("cochfileno", c.COCH_FILE_NO),
                           new XAttribute("daytype", c.DAY_TYPE),
                           new XAttribute("strttime", c.STRT_TIME.ToString()),
                           new XAttribute("endtime", c.END_TIME.ToString()),
                           new XAttribute("mtodstat", c.MTOD_STAT),
                           new XAttribute("sextype", c.SEX_TYPE),
                           new XAttribute("cbmtdesc", c.CBMT_DESC ?? ""),
                           new XAttribute("dfltstat", c.DFLT_STAT ?? "001"),
                           new XAttribute("cpctnumb", c.CPCT_NUMB ?? 0),
                           new XAttribute("cpctstat", c.CPCT_STAT ?? "001"),
                           new XAttribute("cbmttime", c.CBMT_TIME ?? 0),
                           new XAttribute("cbmttimestat", c.CBMT_TIME_STAT ?? "001"),
                           new XAttribute("clastime", c.CLAS_TIME ?? 90),
                           new XElement("Club_Method_Weekdays",
                              CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().Select(cbmw =>
                                 new XElement("Club_Method_Weekday",
                                    new XAttribute("code", cbmw.CODE),
                                    new XAttribute("weekday", cbmw.WEEK_DAY),
                                    new XAttribute("stat", cbmw.STAT)
                                 )
                              )
                           )
                        )
                     )
               )
            );

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
               Execute_Query();
            }
         }
      }

      private void FighMbsp5_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            dynamic vcochmbsp = null;
            if (tb_cbmt1.SelectedTab == tp_0013)
               vcochmbsp = VCochMbspBs1.Current as Data.VF_Coach_MemberShipResult;

            if (vcochmbsp == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", vcochmbsp.FILE_NO)) }
                  );
                  break;
               case 1:
                  Job _InteractWithScsc =
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
                                       "<Privilege>231</Privilege><Sub_Sys>5</Sub_Sys>", 
                                       "DataGuard"
                                    },
                                    AfterChangedOutput = new Action<object>((output) => {
                                       if ((bool)output)
                                          return;
                                       MessageBox.Show("خطا - عدم دسترسی به ردیف 231 سطوح امینتی", "عدم دسترسی");
                                    })
                                 },
                                 #endregion
                              }),
                           #region DoWork
                              new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
                              new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
                              {
                                 Input = 
                                    new XElement("Fighter",
                                       new XAttribute("fileno", vcochmbsp.FILE_NO),
                                       new XAttribute("mbsprwno", vcochmbsp.RWNO),
                                       new XAttribute("formcaller", GetType().Name)
                                    )
                              }
                           #endregion
                        });
                  _DefaultGateway.Gateway(_InteractWithScsc);
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

      private void Wkdy00i_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SimpleButton sb = sender as SimpleButton;

            if (CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT == "001")
            {
               CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT = "002";
               sb.Appearance.BackColor = Color.GreenYellow;
            }
            else
            {
               CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>().FirstOrDefault(w => w.WEEK_DAY == sb.Tag.ToString()).STAT = "001";
               sb.Appearance.BackColor = Color.LightGray;
            }
         }
         catch { }
      }

      private void RunInsTime_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var coch = CochBs1.Current as Data.Fighter;
            var mtod = MtodBs1.Current as Data.Method;
            var club = ClubBs1.Current as Data.Club;

            iScsc.IEXL_CBMT_P(
               new XElement("Club_Method",
                  new XAttribute("cochfileno", coch.FILE_NO),
                  new XAttribute("mtodcode", mtod.CODE),
                  new XAttribute("clubcode", club.CODE),
                  new XAttribute("strttime", StrtTime_Tspn.Time),
                  new XAttribute("endtime", EndTime_Tspn.Time),
                  new XAttribute("pridtime", PridTime_Tspn.EditValue ?? 0),
                  new XAttribute("satday", QWkdy007_Butn.Appearance.BackColor == Color.GreenYellow ? "002" : "001"),
                  new XAttribute("sunday", QWkdy001_Butn.Appearance.BackColor == Color.GreenYellow ? "002" : "001"),
                  new XAttribute("monday", QWkdy002_Butn.Appearance.BackColor == Color.GreenYellow ? "002" : "001"),
                  new XAttribute("tusday", QWkdy003_Butn.Appearance.BackColor == Color.GreenYellow ? "002" : "001"),
                  new XAttribute("wnsday", QWkdy004_Butn.Appearance.BackColor == Color.GreenYellow ? "002" : "001"),
                  new XAttribute("trsday", QWkdy005_Butn.Appearance.BackColor == Color.GreenYellow ? "002" : "001"),
                  new XAttribute("friday", QWkdy006_Butn.Appearance.BackColor == Color.GreenYellow ? "002" : "001")
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

      private void CochBs1_CurrentChanged(object sender, EventArgs e)
      {
         var coch = CochBs1.Current as Data.Fighter;
         CochName2_Lb.Text = coch.NAME_DNRM;
         try
         {
            CochProFile2_Rb.ImageVisiable = CochProFile1_Rb.ImageVisiable = true;
            CochProFile1_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", coch.FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            if (InvokeRequired)
               Invoke(new Action(() => CochProFile2_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = bm));
            else
               CochProFile2_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = bm;
         }
         catch
         {
            CochProFile2_Rb.ImageProfile = CochProFile1_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
         }
         ActvMembCount2_Lb.Text = "";
         AgeMemb2_Lb.Text = "";
         //ClubWkdy2_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
         Execute_Query();
      }

      private void MtodBs1_CurrentChanged(object sender, EventArgs e)
      {
         //ClubWkdy2_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
         ActvMembCount2_Lb.Text = "";
         AgeMemb2_Lb.Text = "";
         Execute_Query();
      }

      private void ClubBs1_CurrentChanged(object sender, EventArgs e)
      {
         var club = CochBs1.Current as Data.Club;
         CochName2_Lb.Text = "";
         CochProFile2_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
         ActvMembCount2_Lb.Text = "";
         AgeMemb2_Lb.Text = "";
         //ClubWkdy2_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
         Execute_Query();
      }

      private void DeleteTime_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rows = Cbmt2_Gv.GetSelectedRows();

            iScsc.STNG_SAVE_P(
               new XElement("Config",
                  new XAttribute("type", "005"),
                  new XElement("Delete",
                     rows.OfType<int>().Select(cm =>
                        new XElement("Club_Method",
                           new XAttribute("code", ((Data.Club_Method)Cbmt2_Gv.GetRow(cm)).CODE)
                        )
                     )
                  )
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

      private void MtodBs3_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var mtod = MtodBs3.Current as Data.Method;
            if (mtod == null) return;

            var coch = CochBs1.Current as Data.Fighter;
            if(coch == null) return;

            var club = ClubBs1.Current as Data.Club;
            if(club == null)return;

            FighsBs3.DataSource =
               iScsc.Fighters
               .Where(f => f.CONF_STAT == "002" &&
                           f.ACTV_TAG_DNRM.CompareTo("101") >= 0 &&
                           f.Member_Ships.Any(ms => ms.RECT_CODE == "004" &&
                                                    ms.VALD_TYPE == "002" &&
                                                    ms.Fighter_Public.CLUB_CODE == club.CODE &&
                                                    ms.Fighter_Public.MTOD_CODE == mtod.CODE &&
                                                    ms.Fighter_Public.COCH_FILE_NO == coch.FILE_NO ));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void HL_INVSFILENO_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var CrntFigh = FighsBs3.Current as Data.Fighter;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", CrntFigh.FILE_NO)) }
            );
         }
         catch { }
      }

      private void vF_Last_Info_FighterResultGridControl_DoubleClick(object sender, EventArgs e)
      {
         HL_INVSFILENO_ButtonClick(null, null);
      }

      private void colActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var index = FighsBs3.Position;
         var figh = FighsBs3.Current as Data.Fighter;
         switch (e.Button.Index)
         {
            case 0:
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                       new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                        })
               );
               break;
            case 1:
               if (iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == figh.FILE_NO && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

               // 1396/10/14 * بررسی اینکه آیا مشتری چند کلاس ثبت نام کرده است
               //if (iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == figh.FILE_NO && mb.RECT_CODE == "004" && mb.TYPE == "001" && mb.END_DATE.Value.Date >= DateTime.Now.Date && (mb.RWNO == 1 || mb.Request_Row.RQTT_CODE == "001") && (mb.NUMB_OF_ATTN_MONT > 0 && mb.NUMB_OF_ATTN_MONT > mb.SUM_ATTN_MONT_DNRM)).Count() >= 2)
               //{
               //   _DefaultGateway.Gateway(
               //      new Job(SendType.External, "localhost",
               //         new List<Job>
               //         {
               //            new Job(SendType.Self, 152 /* Execute Chos_Mbsp_F */),
               //            new Job(SendType.SelfToUserInterface, "CHOS_MBSP_F", 10 /* Execute Actn_CalF_F*/ )
               //            {
               //               Input = 
               //               new XElement("Fighter",
               //                  new XAttribute("fileno", figh.FILE_NO),
               //                  new XAttribute("namednrm", figh.NAME_DNRM),
               //                  new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM)
               //               )
               //            }
               //         }
               //      )
               //   );
               //}
               //else
               //   _DefaultGateway.Gateway(
               //      new Job(SendType.External, "Localhost",
               //         new List<Job>
               //         {
               //            new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
               //            new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM))}
               //         })
               //   );
               break;
            case 2:
               if (MessageBox.Show(this, "آیا با حذف مشتری موافق هستید؟", "عملیات حذف موقت مشتری", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),                        
                        new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))},
                        new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 07 /* Execute Load_Data */){Input = new XElement("LoadData", new XAttribute("requery", "1"))},
                     })
               );
               break;
            case 3:
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                        new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "changeinfo"), new XAttribute("fileno", figh.FILE_NO), new XAttribute("auto", "true"))}
                     })
               );
               break;
            case 4:
               if (figh.FNGR_PRNT_DNRM == "" && !(figh.FGPB_TYPE_DNRM == "002" || figh.FGPB_TYPE_DNRM == "003")) { MessageBox.Show(this, "برای عضو مورد نظر هیچ کد انگشتی وارد نشده، لطفا کد عضو را از طریق تغییرات مشخصات عمومی تغییر لازم را اعمال کنید"); return; }
               if (figh.COCH_FILE_NO_DNRM == null && !(figh.FGPB_TYPE_DNRM == "009" || figh.FGPB_TYPE_DNRM == "002" || figh.FGPB_TYPE_DNRM == "003" || figh.FGPB_TYPE_DNRM == "004")) { MessageBox.Show(this, "برای عضو شما مربی و ساعت کلاسی مشخصی وجود ندارد که مشخص کنیم در چه کلاس حضوری ثبت کنیم"); return; }
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                        
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "accesscontrol"), new XAttribute("fngrprnt", figh.FNGR_PRNT_DNRM), new XAttribute("attnsystype", "001"))}
                     })
               );
               break;
            case 5:
               if (figh.FNGR_PRNT_DNRM == "" && !(figh.FGPB_TYPE_DNRM == "002" || figh.FGPB_TYPE_DNRM == "003")) { MessageBox.Show(this, "برای عضو مورد نظر هیچ کد انگشتی وارد نشده، لطفا کد عضو را از طریق تغییرات مشخصات عمومی تغییر لازم را اعمال کنید"); return; }
               if (figh.COCH_FILE_NO_DNRM == null && !(figh.FGPB_TYPE_DNRM == "009" || figh.FGPB_TYPE_DNRM == "002" || figh.FGPB_TYPE_DNRM == "003" || figh.FGPB_TYPE_DNRM == "004")) { MessageBox.Show(this, "برای عضو شما مربی و ساعت کلاسی مشخصی وجود ندارد که مشخص کنیم در چه کلاس حضوری ثبت کنیم"); return; }

               /* 1395/03/15 * اگر سیستم بتواند حضوری را برای فرد ذخیره کند باید عملیات نمایش ورود فرد را آماده کنیم. */
               var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.ATTN_NOTF_STAT == "002").FirstOrDefault();
               if (attnNotfSetting != null && attnNotfSetting.ATTN_NOTF_STAT == "002" && figh.FILE_NO != 0 && iScsc.Attendances.Any(a => figh.FILE_NO == a.FIGH_FILE_NO && a.ATTN_DATE.Date == DateTime.Now.Date))
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),
                           new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", figh.FILE_NO),
                                 new XAttribute("attndate", DateTime.Now)
                              )
                           }
                        })
                  );
               }
               break;
            case 6:
               try
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                              Input = 
                                 new XElement("Fighter",
                                    new XAttribute("fileno", figh.FILE_NO)                               
                                 )
                           },
                           new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
                           {
                              Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", figh.FILE_NO),
                                 new XAttribute("type", "refresh"),
                                 new XAttribute("tabfocued", "tp_003")
                              )
                           }
                        })
                  );
               }
               catch { }
               break;
            default:
               break;
         }

         FighsBs3.Position = index;
      }
   }
}
