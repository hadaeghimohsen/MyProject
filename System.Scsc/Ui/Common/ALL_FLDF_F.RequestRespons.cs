using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Common
{
   partial class ALL_FLDF_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private long fileno;
      private bool isFirstLoaded = false;

      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         try
         {
            if (job.Input == null) return;

            /*Tc_Info.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 50, Screen.PrimaryScreen.WorkingArea.Height - 50);
            Tc_Info.Left = 30;
            Tc_Info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            */
            tb_master.SelectedTab = tp_001;

            fileno = Convert.ToInt64((job.Input as XElement).Attributes("fileno").First().Value);
            try
            {
               UserProFile_Rb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", fileno))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               //Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
               else
                  UserProFile_Rb.ImageProfile = bm;
            }
            catch { //Pb_FighImg.Visible = false;
               UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482; 
            }
            // 1395/11/26 * اگر مشترک غیرفعال باشد باید از لیست مربوط به مشترکین غیرفعال استفاده کرد
            var crntinfo = iScsc.Fighters.First(f => f.FILE_NO == fileno);
            if (Convert.ToInt32(crntinfo.ACTV_TAG_DNRM) <= 100)
               vF_Last_Info_FighterBs.DataSource = iScsc.VF_Last_Info_Deleted_Fighter(fileno, null, null, null, null, null, null, null, null, null);
            else
               vF_Last_Info_FighterBs.DataSource = iScsc.VF_Last_Info_Fighter(fileno, null, null, null, null, null, null, null, null, null);

            vF_All_Info_FightersBs.DataSource = iScsc.VF_All_Info_Fighters(fileno).OrderByDescending(f => f.RWNO);
            //vF_SavePaymentsBs.DataSource = iScsc.VF_Payments(null, null, fileno, null, null, null, null).OrderByDescending(p => p.ISSU_DATE);
            
            vF_SavePaymentsBs.DataSource = iScsc.VF_Save_Payments(null, fileno).OrderByDescending(p => p.PYMT_CRET_DATE);
            ShowCrntReglYear_Butn_Click(null, null);

            vF_Request_ChangingBs.DataSource = iScsc.VF_Request_Changing(fileno).OrderBy(r => r.RQST_DATE);
            vF_Request_DocumentBs.DataSource = iScsc.VF_Request_Document(fileno);
            AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
            
            CochBs1.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003");

            //var crnt = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;

            if(Convert.ToInt32(crntinfo.ACTV_TAG_DNRM) <= 100)
            {
               // غیر فعال
               aCTV_TAGTextBox.BackColor = Color.PaleVioletRed;
               aCTV_TAGTextBox.ForeColor = Color.White;
            }
            else
            {
               // فعال
               aCTV_TAGTextBox.BackColor = Color.YellowGreen;
               aCTV_TAGTextBox.ForeColor = Color.Black;
            }

            //var mbcofigh = iScsc.Fighters.First(f => f.FILE_NO == fileno);
            if(crntinfo.MBCO_RWNO_DNRM != null)
            {
               Mbco_Pn.Visible = true;
               var mbco = iScsc.Member_Ships.First(m => m.FIGH_FILE_NO == crntinfo.FILE_NO && m.RWNO == crntinfo.MBCO_RWNO_DNRM && m.RECT_CODE == "004");
               StrtPrivSesn_Date.Value = mbco.STRT_DATE;
               EndPrivSesn_Date.Value = mbco.END_DATE;
               MbcoRwno_Txt.Text = mbco.RWNO.ToString();
               MbcoMont_Txt.Text = mbco.NUMB_OF_MONT_DNRM.ToString();
               MbcoDays_Txt.Text = mbco.NUMB_OF_DAYS_DNRM.ToString();
            }
            else
            {
               Mbco_Pn.Visible = false;
            }

            if (crntinfo.MBFZ_RWNO_DNRM != null)
            {
               var mbfz = iScsc.Member_Ships.First(m => m.FIGH_FILE_NO == crntinfo.FILE_NO && m.RWNO == crntinfo.MBFZ_RWNO_DNRM && m.RECT_CODE == "004");

               Mbfz_pn.Visible = mbfz.END_DATE.Value.Date >= DateTime.Now.Date ? true : false;
               StrtBlok_Date.Value = mbfz.STRT_DATE;
               EndBlok_Date.Value = mbfz.END_DATE;
               MbfzRwno_Txt.Text = mbfz.RWNO.ToString();
               MbfzDays_Txt.Text = mbfz.NUMB_OF_DAYS_DNRM.ToString();
            }
            else
            {
               Mbfz_pn.Visible = false;
            }

            // 1396/10/13 * نمایش لیست دوره های ثبت نام شده
            MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == fileno && mb.RECT_CODE == "004" && mb.TYPE == "001");

            if(isFirstLoaded) goto commandfinished;

            DDebtBs.DataSource = iScsc.D_DEBTs;
            DAttpBs.DataSource = iScsc.D_ATTPs;
            DActvBs.DataSource = iScsc.D_ACTVs;
            DRcmtBs.DataSource = iScsc.D_RCMTs;
            DAtypBs.DataSource = iScsc.D_ATYPs;

            /*vF_TestBs.DataSource = iScsc.VF_Test(fileno);
            vF_CampititionBs.DataSource = iScsc.VF_Campitition(fileno);
            vF_Physical_FitnessBs.DataSource = iScsc.VF_Physical_Fitness(fileno).OrderByDescending(p => p.RWNO);
            vF_Calculate_CalorieBs.DataSource = iScsc.VF_Calculate_Calorie(fileno).OrderByDescending(c => c.RWNO);
            vF_Heart_ZoneBs.DataSource = iScsc.VF_Heart_Zone(fileno).OrderByDescending(h => h.RWNO);
            vF_ExamBs.DataSource = iScsc.VF_Exam(fileno).OrderByDescending(e => e.RWNO);
            */
            isFirstLoaded = true;
            commandfinished:
            ;
         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput.Attribute("type").Value == "refresh")
         {
            Execute_Query();
            if(xinput.Attribute("tabfocued").Value == "tp_003")
            {
               tb_master.SelectedTab = tp_003;
            }
         }
      }
   }
}
