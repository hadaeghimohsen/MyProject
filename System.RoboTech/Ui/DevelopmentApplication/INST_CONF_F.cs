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
               Execute_Query();
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

            var userSession = new UserSessionData
            {
               UserName = inst.USER_NAME,
               Password = inst.PASS_WORD
            };

            _instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();
            const string stateFile = "instagram_state.bin";
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
            ProcNumbFollowers_Lb.Text = "Processed number: {0}";
            ProcNumbFollowing_Lb.Text = "Processed number: {0}";
            CountFollowers_Lb.Text = "Followers : 0";
            CountFollowing_Lb.Text = "Following : 0";

            var _login = await LoginAsync();
            if (!_login) return;

            var inst = InstBs.Current as Data.Robot_Instagram;
            if (inst == null) return;

            var _getFollowers = await _instaApi.UserProcessor.GetCurrentUserFollowersAsync(InstagramApiSharp.PaginationParameters.Empty);
            var _getFollowing = await _instaApi.UserProcessor.GetUserFollowingAsync(inst.USER_NAME,InstagramApiSharp.PaginationParameters.Empty);

            CountFollowers_Lb.Text = string.Format("Followers : {0}", _getFollowers.Value.Count);
            CountFollowing_Lb.Text = string.Format("Following : {0}", _getFollowing.Value.Count);

            SaveMembers_Pbc.Position = 0;
            SaveMembers_Pbc.Properties.Step = _getFollowers.Value.Count / 100;
            int i = 0, s = 0;
            // First Save Followers in database
            foreach (var f in _getFollowers.Value)
            {
               ++i;
               if (!IflrBs.List.OfType<Data.Robot_Instagram_Follow>().Any(fr => fr.Robot_Instagram == inst && fr.INST_PKID == f.Pk && fr.FOLW_TYPE == "001"))
               {
                  ++s;
                  iRoboTech.ExecuteCommand(
                     string.Format("INSERT INTO dbo.Robot_Instagram_Follow(Rins_Code, Code, Inst_Pkid, User_Name, Full_Name, Folw_Type) VALUES({0}, 0, {1}, '{2}', N'{3}', '001');", inst.CODE, f.Pk, f.UserName, f.FullName)
                  );

                  ProcNumbFollowers_Lb.Text = string.Format("Processed number: {0}", s);
               }
               // Set Step Progressbar
               //if(i % SaveMembers_Pbc.Properties.Step == 0)
               //   SaveMembers_Pbc.Position = SaveMembers_Pbc.Properties.Step;
            }

            SaveMembers_Pbc.Position = 0;
            SaveMembers_Pbc.Properties.Step = _getFollowing.Value.Count / 100;
            i = s = 0;
            // Second Save Following in database
            foreach (var f in _getFollowing.Value)
            {
               ++i;
               if (!IflnBs.List.OfType<Data.Robot_Instagram_Follow>().Any(fn => fn.Robot_Instagram == inst && fn.INST_PKID == f.Pk && fn.FOLW_TYPE == "002"))
               {
                  ++s;
                  iRoboTech.ExecuteCommand(
                     string.Format("INSERT INTO dbo.Robot_Instagram_Follow(Rins_Code, Code, Inst_Pkid, User_Name, Full_Name, Folw_Type) VALUES({0}, 0, {1}, '{2}', N'{3}', '002');", inst.CODE, f.Pk, f.UserName, f.FullName)
                  );

                  ProcNumbFollowing_Lb.Text = string.Format("Processed number: {0}", s);
               }
               // Set Step Progressbar
               //SaveMembers_Pbc.Position = SaveMembers_Pbc.Properties.Step;
            }

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

      private void InstBs_CurrentChanged(object sender, EventArgs e)
      {
         var inst = InstBs.Current as Data.Robot_Instagram;

         IflrBs.DataSource = iRoboTech.Robot_Instagram_Follows.Where(f => f.FOLW_TYPE == "001");
         IflnBs.DataSource = iRoboTech.Robot_Instagram_Follows.Where(f => f.FOLW_TYPE == "002");
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
   }
}
