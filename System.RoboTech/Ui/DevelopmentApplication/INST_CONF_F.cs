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
using System.RoboTech.ExceptionHandlings;
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.RoboTech.ExtCode;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using System.IO;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class INST_CONF_F : UserControl
   {
      public INST_CONF_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private IInstaApi _instaApi;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int inst = InstBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));         

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         InstBs.Position = inst;

         requery = false;
      }

      private void AddNewPage_Butn_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         if (InstBs.List.OfType<Data.Robot_Instagram>().Any(i => i.CODE == 0)) return;

         var inst = InstBs.AddNew() as Data.Robot_Instagram;
         inst.Robot = robo;
         inst.STAT = "002";
         inst.CELL_PHON = robo.CELL_PHON;

         iRoboTech.Robot_Instagrams.InsertOnSubmit(inst);
      }

      private void DelPage_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            if (MessageBox.Show(this, "آیا با حذف پیج اینستاگرام موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            if (inst.Robot_Instagram_Follows.Any() && (MessageBox.Show(this, "پیج دارای اعضا میباشد آیا مایل به حذف پیج هستید؟", "عملیات حذف", MessageBoxButtons.YesNo) != DialogResult.Yes)) return;
            if (inst.Robot_Instagram_DirectMessages.Any() && (MessageBox.Show(this, "پیج دارای فالوور میباشد آیا مایل به حذف پیج هستید؟", "عملیات حذف", MessageBoxButtons.YesNo) != DialogResult.Yes)) return;

            iRoboTech.Robot_Instagrams.DeleteOnSubmit(inst);
            iRoboTech.SubmitChanges();

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

      private void SaveInfoPage_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            InstGv.PostEditor();

            iRoboTech.SubmitChanges();
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

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "FRST_PAGE_F", 07 /* Execute LoadData */, SendType.SelfToUserInterface) 
               );
            }
         }
      }

      private void InstStat_Cbtn_CheckedChanged(object sender, EventArgs e)
      {
         var inst = InstBs.Current as Data.Robot_Instagram;
         if (inst == null) return;

         inst.STAT = InstStat_Cbtn.Checked ? "002" : "001";
         InstStat_Cbtn.Text = InstStat_Cbtn.Checked ? "فعال" : "غیرفعال";
      }

      private async Task<bool> LoginAsync()
      {
         try
         {
            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return false;

            if (inst.PAGE_OWNR_TYPE == "001")
               inst = InstBs.List.OfType<Data.Robot_Instagram>().FirstOrDefault(i => i.PAGE_OWNR_TYPE == "002" && i.STAT == "002");

            var userSession = new UserSessionData
            {
               UserName = inst.USER_NAME,
               Password = inst.PASS_WORD
            };

            _instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();
            string stateFile = string.Format("{0}_instagram_state.bin", inst.USER_NAME);
            try
            {
               // load session file if exists
               if (File.Exists(stateFile))
               {
                  using (var fs = File.OpenRead(stateFile))
                  {
                     _instaApi.LoadStateDataFromStream(fs);
                     // in .net core or uwp apps don't use LoadStateDataFromStream
                     // use this one:
                     // _instaApi.LoadStateDataFromString(new StreamReader(fs).ReadToEnd());
                     // you should pass json string as parameter to this function.
                  }
               }
            }
            catch { }

            if (!_instaApi.IsUserAuthenticated)
            {
               // login
               var logInResult = await _instaApi.LoginAsync();
               if (!logInResult.Succeeded)
               {
                  return false;
               }
            }
            // save session in file
            var state = _instaApi.GetStateDataAsStream();
            // in .net core or uwp apps don't use GetStateDataAsStream.
            // use this one:
            // var state = _instaApi.GetStateDataAsString();
            // this returns you session as json string.
            using (var fileStream = File.Create(stateFile))
            {
               state.Seek(0, SeekOrigin.Begin);
               state.CopyTo(fileStream);
            }
            return true;
         }
         catch { return false; }
      }

      private async void GetInstagramMembers_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            // Reset
            ProcNumbFollowers_Lb.Text = "Processed number: 0";
            ProcNumbFollowing_Lb.Text = "Processed number: 0";
            CountFollowers_Lb.Text = "Followers : 0";
            CountFollowing_Lb.Text = "Following : 0";
            SaveMembers_Pbc.Visible = true;

            var _login = await LoginAsync();
            if (!_login) return;

            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            var _getFollowers = await _instaApi.UserProcessor.GetUserFollowersAsync(inst.USER_NAME, InstagramApiSharp.PaginationParameters.Empty);
            var _getFollowing = await _instaApi.UserProcessor.GetUserFollowingAsync(inst.USER_NAME, InstagramApiSharp.PaginationParameters.Empty);

            CountFollowers_Lb.Text = string.Format("Followers : {0}", _getFollowers.Value.Count);
            CountFollowing_Lb.Text = string.Format("Following : {0}", _getFollowing.Value.Count);

            int i = 0, s = 0;
            // First Save Followers in database
            if (_getFollowers.Succeeded || _getFollowers.Value.Count > 0)
            {
               foreach (var f in _getFollowers.Value)
               {
                  ++i;
                  if (!IflrBs.List.OfType<Data.Robot_Instagram_Follow>().Any(fr => fr.Robot_Instagram == inst && fr.INST_PKID == f.Pk && fr.FOLW_TYPE == "001"))
                  {
                     ++s;
                     iRoboTech.ExecuteCommand(
                        string.Format("INSERT INTO dbo.Robot_Instagram_Follow(Rins_Code, Code, Inst_Pkid, User_Name, Full_Name, Folw_Type) VALUES({0}, 0, {1}, '{2}', N'{3}', '001');", inst.CODE, f.Pk, f.UserName, f.FullName.Replace("'", "''"))
                        //string.Format("INSERT INTO dbo.Robot_Instagram_Follow(Rins_Code, Code, Inst_Pkid, User_Name, Folw_Type) VALUES({0}, 0, {1}, '{2}', '001');", inst.CODE, f.Pk, f.UserName)
                     );

                     ProcNumbFollowers_Lb.Text = string.Format("Processed number: {0}", s);
                  }
                  // Set Step Progressbar
                  //if(i % SaveMembers_Pbc.Properties.Step == 0)
                  //   SaveMembers_Pbc.Position = SaveMembers_Pbc.Properties.Step;
               }
            }

            i = s = 0;
            // Second Save Following in database
            if (_getFollowing.Succeeded || _getFollowing.Value.Count > 0)
            {
               foreach (var f in _getFollowing.Value)
               {
                  ++i;
                  if (!IflnBs.List.OfType<Data.Robot_Instagram_Follow>().Any(fn => fn.Robot_Instagram == inst && fn.INST_PKID == f.Pk && fn.FOLW_TYPE == "002"))
                  {
                     ++s;
                     iRoboTech.ExecuteCommand(
                        string.Format("INSERT INTO dbo.Robot_Instagram_Follow(Rins_Code, Code, Inst_Pkid, User_Name, Full_Name, Folw_Type) VALUES({0}, 0, {1}, '{2}', N'{3}', '002');", inst.CODE, f.Pk, f.UserName, f.FullName.Replace("'", "''"))
                        //string.Format("INSERT INTO dbo.Robot_Instagram_Follow(Rins_Code, Code, Inst_Pkid, User_Name, Folw_Type) VALUES({0}, 0, {1}, '{2}', '002');", inst.CODE, f.Pk, f.UserName)
                     );

                     ProcNumbFollowing_Lb.Text = string.Format("Processed number: {0}", s);
                  }
                  // Set Step Progressbar
                  //SaveMembers_Pbc.Position = SaveMembers_Pbc.Properties.Step;
               }
            }
            SaveMembers_Pbc.Visible = false;
            //iRoboTech.SubmitChanges();

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

            SaveMembers_Pbc.Visible = false;
         }
      }

      private void InstBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            InstStat_Cbtn.Checked = inst.STAT == "002" ? true : false;
            InstCyclStat_Cbtn.Checked = inst.CYCL_STAT == "002" ? true : false;
            InstPageOwnrType_Cbtn.Checked = inst.PAGE_OWNR_TYPE == "002" ? true : false;

            IflrBs.DataSource = iRoboTech.Robot_Instagram_Follows.Where(f => f.Robot_Instagram == inst && f.FOLW_TYPE == "001");
            IflnBs.DataSource = iRoboTech.Robot_Instagram_Follows.Where(f => f.Robot_Instagram == inst && f.FOLW_TYPE == "002");
         }
         catch { }
      }

      private async void GetUpdateFollowerInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _login = await LoginAsync();
            if (!_login) return;

            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            var follower = IflrBs.Current as Data.Robot_Instagram_Follow;
            if (follower == null) return;

            var userFollower = await _instaApi.UserProcessor.GetFullUserInfoAsync(follower.INST_PKID);
            follower.BIOG_DESC = userFollower.Value.UserDetail.Biography;
            follower.URL = userFollower.Value.UserDetail.ExternalUrl;
            follower.CTGY_DESC = userFollower.Value.UserDetail.Category;
            follower.EMAL_ADRS = userFollower.Value.UserDetail.PublicEmail;

            iRoboTech.SubmitChanges();

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

      private async void GetUpdateFollowingInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _login = await LoginAsync();
            if (!_login) return;

            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            var following = IflnBs.Current as Data.Robot_Instagram_Follow;
            if (following == null) return;

            var userFollower = await _instaApi.UserProcessor.GetFullUserInfoAsync(following.INST_PKID);
            following.BIOG_DESC = userFollower.Value.UserDetail.Biography;
            following.URL = userFollower.Value.UserDetail.ExternalUrl;
            following.CTGY_DESC = userFollower.Value.UserDetail.Category;
            following.EMAL_ADRS = userFollower.Value.UserDetail.PublicEmail;

            iRoboTech.SubmitChanges();

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

      private void SendMesgToFollowers_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            if (inst.PAGE_OWNR_TYPE == "001")
               inst = InstBs.List.OfType<Data.Robot_Instagram>().FirstOrDefault(i => i.PAGE_OWNR_TYPE == "002" && i.STAT == "002");

            List<Data.Robot_Instagram_Follow> _members = new List<Data.Robot_Instagram_Follow>();

            switch (FollowersSelectOption_Fb.SelectedIndex)
            {
               case 0:
                  #region Single Direct Message
                  var follower = IflrBs.Current as Data.Robot_Instagram_Follow;
                  iRoboTech.INS_RIDM_P(
                     new XElement("Instagram",
                         new XAttribute("code", inst.CODE),
                         new XAttribute("rbid", inst.ROBO_RBID),
                         new XElement("Direct",
                             new XElement("Message", FlwrMesgText_Txt.Text),
                             new XElement("Users",
                                 new XElement("User",
                                     new XAttribute("pkid", follower.INST_PKID),
                                     new XAttribute("chatid", follower.CHAT_ID ?? 0)                                     
                                 )
                             )
                         )
                     )
                  );
                  FlwrProcNumb_Lb.Text = "Processed number: 1";
                  #endregion
                  break;
               case 1:
                  #region Sellected Direct Message
                  var rows = IflrGv.GetSelectedRows();
                  foreach (var r in rows)
                  {
                     _members.Add((Data.Robot_Instagram_Follow)IflrGv.GetRow(r));
                  }

                  iRoboTech.INS_RIDM_P(
                     new XElement("Instagram",
                         new XAttribute("code", inst.CODE),
                         new XAttribute("rbid", inst.ROBO_RBID),
                         new XElement("Direct",
                             new XElement("Message", FlwrMesgText_Txt.Text),
                             new XElement("Users",
                                 _members.Select(f => 
                                    new XElement("User",
                                        new XAttribute("pkid", f.INST_PKID)
                                    )
                                 )
                             )
                         )
                     )
                  );

                  FlwrProcNumb_Lb.Text = string.Format("Processed number: {0}", _members.Count);
                  #endregion
                  break;
               case 2:
                  #region All Direct Message
                  iRoboTech.INS_RIDM_P(
                     new XElement("Instagram",
                         new XAttribute("code", inst.CODE),
                         new XAttribute("rbid", inst.ROBO_RBID),
                         new XElement("Direct",
                             new XElement("Message", FlwrMesgText_Txt.Text),
                             new XElement("Users",
                                 IflrBs.List.OfType<Data.Robot_Instagram_Follow>()
                                 .Select(f =>
                                    new XElement("User",
                                        new XAttribute("pkid", f.INST_PKID)
                                    )
                                 )
                             )
                         )
                     )
                  );

                  FlwrProcNumb_Lb.Text = string.Format("Processed number: {0}", IflrBs.List.Count);
                  #endregion
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

      private void SendMesgToFollowing_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            if (inst.PAGE_OWNR_TYPE == "001")
               inst = InstBs.List.OfType<Data.Robot_Instagram>().FirstOrDefault(i => i.PAGE_OWNR_TYPE == "002" && i.STAT == "002");

            List<Data.Robot_Instagram_Follow> _members = new List<Data.Robot_Instagram_Follow>();

            switch (FollowingSelectOption_Fb.SelectedIndex)
            {
               case 0:
                  #region Single Direct Message
                  var follower = IflnBs.Current as Data.Robot_Instagram_Follow;
                  iRoboTech.INS_RIDM_P(
                     new XElement("Instagram",
                         new XAttribute("code", inst.CODE),
                         new XAttribute("rbid", inst.ROBO_RBID),
                         new XElement("Direct",
                             new XElement("Message", FlwnMesgText_Txt.Text),
                             new XElement("Users",
                                 new XElement("User",
                                     new XAttribute("pkid", follower.INST_PKID),
                                     new XAttribute("chatid", follower.CHAT_ID ?? 0)
                                 )
                             )
                         )
                     )
                  );
                  FlwnProcNumb_Lb.Text = "Processed number: 1";
                  #endregion
                  break;
               case 1:
                  #region Sellected Direct Message
                  var rows = IflnGv.GetSelectedRows();
                  foreach (var r in rows)
                  {
                     _members.Add((Data.Robot_Instagram_Follow)IflnGv.GetRow(r));
                  }

                  iRoboTech.INS_RIDM_P(
                     new XElement("Instagram",
                         new XAttribute("code", inst.CODE),
                         new XAttribute("rbid", inst.ROBO_RBID),
                         new XElement("Direct",
                             new XElement("Message", FlwnMesgText_Txt.Text),
                             new XElement("Users",
                                 _members.Select(f =>
                                    new XElement("User",
                                        new XAttribute("pkid", f.INST_PKID)
                                    )
                                 )
                             )
                         )
                     )
                  );

                  FlwnProcNumb_Lb.Text = string.Format("Processed number: {0}", _members.Count);
                  #endregion
                  break;
               case 2:
                  #region All Direct Message
                  iRoboTech.INS_RIDM_P(
                     new XElement("Instagram",
                         new XAttribute("code", inst.CODE),
                         new XAttribute("rbid", inst.ROBO_RBID),
                         new XElement("Direct",
                             new XElement("Message", FlwnMesgText_Txt.Text),
                             new XElement("Users",
                                 IflnBs.List.OfType<Data.Robot_Instagram_Follow>()
                                 .Select(f =>
                                    new XElement("User",
                                        new XAttribute("pkid", f.INST_PKID)
                                    )
                                 )
                             )
                         )
                     )
                  );

                  FlwnProcNumb_Lb.Text = string.Format("Processed number: {0}", IflnBs.List.Count);
                  #endregion
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

      private void FlwrAddPlacHldrMesg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var plachldr = TmpiBs.List.OfType<Data.Template_Item>().FirstOrDefault(t => t.CODE == (long)FlwrPlacHldr_Lov.EditValue);
            FlwrMesgText_Txt.Text = 
               FlwrMesgText_Txt.Text.Insert(FlwrMesgText_Txt.SelectionStart, 
                  FlwrInsAutoSpac_Fcb.Checked ? 
                     (FlwrInsAutoSpac_Fcb.SelectedIndex == 0) ? 
                        string.Format(" {0} ", plachldr.PLAC_HLDR) : 
                        (FlwrInsAutoSpac_Fcb.SelectedIndex == 1) ?
                           string.Format(" {0}", plachldr.PLAC_HLDR) : 
                           string.Format("{0} ", plachldr.PLAC_HLDR) 
                  : plachldr.PLAC_HLDR
               );
         }
         catch { }         
      }

      private void FlwnAddPlacHldrMesg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var plachldr = TmpiBs.List.OfType<Data.Template_Item>().FirstOrDefault(t => t.CODE == (long)FlwnPlacHldr_Lov.EditValue);
            FlwnMesgText_Txt.Text =
               FlwnMesgText_Txt.Text.Insert(FlwnMesgText_Txt.SelectionStart,
                  FlwnInsAutoSpac_Fcb.Checked ?
                     (FlwnInsAutoSpac_Fcb.SelectedIndex == 0) ?
                        string.Format(" {0} ", plachldr.PLAC_HLDR) :
                        (FlwnInsAutoSpac_Fcb.SelectedIndex == 1) ?
                           string.Format(" {0}", plachldr.PLAC_HLDR) :
                           string.Format("{0} ", plachldr.PLAC_HLDR) 
                  : plachldr.PLAC_HLDR
               );
         }
         catch { }
      }

      private void TmplStat_Cbtn_CheckedChanged(object sender, EventArgs e)
      {
         var tmpl = TmplBs.Current as Data.Template;
         if (tmpl == null) return;

         tmpl.STAT = tmpl.STAT == "002" ? "001" : "002";
         TmplStat_Cbtn.Checked = tmpl.STAT == "002" ? true : false;
      }

      private void AddNewTmpl_Butn_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         if (TmplBs.List.OfType<Data.Template>().Any(t => t.TMID == 0)) return;

         var tmpl = TmplBs.AddNew() as Data.Template;
         tmpl.Robot = robo;

         iRoboTech.Templates.InsertOnSubmit(tmpl);
      }

      private void DelTmpl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var tmpl = TmplBs.Current as Data.Template;
            if (tmpl == null) return;

            if (MessageBox.Show(this, "آیا با حذف قالب موافق هستید؟", "عملیات حذف", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iRoboTech.Templates.DeleteOnSubmit(tmpl);
            iRoboTech.SubmitChanges();

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

      private void SaveInfoTmpl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            TmplGv.PostEditor();

            iRoboTech.SubmitChanges();

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

      private void SlctTmpl_Butn_Click(object sender, EventArgs e)
      {
         var tmpl = TmplBs.Current as Data.Template;
         if (tmpl == null) return;

         if(Master_Tc.SelectedTabPage == Followers_Tp)
         {
            FlwrMesgText_Txt.Text = tmpl.TEMP_TEXT;
         }
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            TmpiBs.DataSource = iRoboTech.Template_Items.Where(t => t.Robot == robo && t.RECD_STAT == "002");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddPlacHldr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var plachldr = TmpiBs.List.OfType<Data.Template_Item>().FirstOrDefault(t => t.CODE == (long)PlacHldr_Lov.EditValue);
            MesgText_Txt.Text = 
               MesgText_Txt.Text.Insert(MesgText_Txt.SelectionStart, 
                  InsAutoSpac_Fcb.Checked ?
                     (InsAutoSpac_Fcb.SelectedIndex == 0) ?
                        string.Format(" {0} ", plachldr.PLAC_HLDR) :
                        (InsAutoSpac_Fcb.SelectedIndex == 1) ?
                           string.Format(" {0}", plachldr.PLAC_HLDR) :
                           string.Format("{0} ", plachldr.PLAC_HLDR) 
                  : plachldr.PLAC_HLDR
               );
         }
         catch { }
      }

      private async void GetUpdateSellerInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _login = await LoginAsync();
            if (!_login) return;

            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            var user = await _instaApi.UserProcessor.GetUserAsync(inst.USER_NAME);

            var userFollower = await _instaApi.UserProcessor.GetFullUserInfoAsync(user.Value.Pk);
            inst.BIOG_DESC = userFollower.Value.UserDetail.Biography;
            inst.URL = userFollower.Value.UserDetail.ExternalUrl;
            inst.CTGY_DESC = userFollower.Value.UserDetail.Category;
            inst.EMAL_ADRS = userFollower.Value.UserDetail.PublicEmail;
            inst.INST_PKID = user.Value.Pk;

            iRoboTech.SubmitChanges();

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

      private void IflrBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var flwr = IflrBs.Current as Data.Robot_Instagram_Follow;
            if (flwr == null) return;

            IdfrBs.DataSource = iRoboTech.Robot_Instagram_DirectMessages.Where(d => d.INST_PKID == flwr.INST_PKID);
         }
         catch { }
      }

      private void IflnBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var flwn = IflnBs.Current as Data.Robot_Instagram_Follow;
            if (flwn == null) return;

            IdfnBs.DataSource = iRoboTech.Robot_Instagram_DirectMessages.Where(d => d.INST_PKID == flwn.INST_PKID);
         }
         catch { }
      }

      private void InstCyclStat_Cbtn_CheckedChanged(object sender, EventArgs e)
      {
         var inst = InstBs.Current as Data.Robot_Instagram;
         if (inst == null) return;

         inst.CYCL_STAT = InstCyclStat_Cbtn.Checked ? "002" : "001";
         InstCyclStat_Cbtn.Text = InstCyclStat_Cbtn.Checked ? "فعال" : "غیرفعال";
      }

      private void InstPageOwnrType_Cbtn_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            inst.PAGE_OWNR_TYPE = InstPageOwnrType_Cbtn.Checked ? "002" : "001";
            InstPageOwnrType_Cbtn.Text = InstPageOwnrType_Cbtn.Checked ? "حساب مال من هست" : "حساب مال دیگری میباشد";

            Password_Txt.Enabled = true;

            if(!InstPageOwnrType_Cbtn.Checked)
            {
               inst.PASS_WORD = "";
               Password_Txt.Enabled = false;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FolwPage_Fb_ItemSelected(object sender, MaxUi.FlyoutEventArgs e)
      {
         switch (e.Index)
         {
            case 0:
               FolwPageNumb_Se.Visible = true;
               break;
            default:
               FolwPageNumb_Se.Visible = false;
               break;
         }
      }
   }
}
