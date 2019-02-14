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
         RequeryClubMethod_Butn_Click(null, null);
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

         if (Tb_Master.SelectedTab == tp_001)
         {  
            CochName_Lb.Text = cbmt.Fighter.NAME_DNRM;
            FngrPrnt_Lb.Text = cbmt.Fighter.FNGR_PRNT_DNRM == "" ? "نامشخص" : cbmt.Fighter.FNGR_PRNT_DNRM;

            CbmtwkdyBs1.DataSource = cbmt.Club_Method_Weekdays.ToList();

            if (CbmtwkdyBs1.List.Count == 0)
            {
               ClubWkdy1_Spn.Panel2.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               return;
            }

            foreach (var wkdy in CbmtwkdyBs1.List.OfType<Data.Club_Method_Weekday>())
            {
               var rslt = ClubWkdy1_Spn.Panel2.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
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

            ActvMembCount_Lb.Text = listMbsp.Count().ToString();
            AgeMemb_Lb.Text = string.Join(", ", listMbsp.Select(ms => (DateTime.Now.Year - ms.Fighter.BRTH_DATE_DNRM.Value.Year).ToString()).Distinct().OrderBy(f => f).ToList());

            try
            {
               CochProFile_Rb.ImageVisiable = true;
               CochProFile_Rb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", cbmt.COCH_FILE_NO))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               //Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => CochProFile_Rb.ImageProfile = bm));
               else
                  CochProFile_Rb.ImageProfile = bm;
            }
            catch
            { //Pb_FighImg.Visible = false;
               CochProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
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
            if (tb_cbmt2.SelectedTab == tp_0062)
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
            else if (tb_cbmt2.SelectedTab == tp_0063)
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
            if (Tb_Master.SelectedTab == tp_001)
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
            if (Tb_Master.SelectedTab == tp_001)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if (!AttnDate1_Dt.Value.HasValue)
                  date = AttnDate1_Dt.Value = DateTime.Now;
               else
                  date = AttnDate1_Dt.Value;
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
            if (Tb_Master.SelectedTab == tp_001)
            {
               var cbmt = CbmtBs1.Current as Data.Club_Method;
               if (cbmt == null) return;

               cbmtcode = cbmt.CODE;

               if (!AttnDate1_Dt.Value.HasValue)
                  date = AttnDate1_Dt.Value = DateTime.Now;
               else
                  date = AttnDate1_Dt.Value;
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
            if (Tb_Master.SelectedTab == tp_001)
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
                           )
                     }
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            Back_Butn_Click(null, null);
         }
         catch { }
      }

      private void ClearParm_Butn_Click(object sender, EventArgs e)
      {
         CommandCbmt_Pnl.Controls.OfType<SimpleButton>()/*.Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.GreenYellow)*/.ToList().ForEach(sb => sb.Appearance.BackColor = Color.LightGray);
      }

      private void SelectAllParm_Butn_Click(object sender, EventArgs e)
      {
         CommandCbmt_Pnl.Controls.OfType<SimpleButton>()/*.Where(sb => sb.Tag != null && sb.Appearance.BackColor == Color.GreenYellow)*/.ToList().ForEach(sb => sb.Appearance.BackColor = Color.GreenYellow);
      }

      private void SaveWkdy_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Data.Club_Method c = null;
            if (Tb_Master.SelectedTab == tp_001)
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
            if (tb_cbmt2.SelectedTab == tp_0063)
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

   }
}
