using InstagramApiSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.RoboTech.Data;
using InstagramApiSharp.Classes;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using System.IO;

namespace System.RoboTech.Controller
{
   public class Instagram
   {
      private IInstaApi _instaApi;
      private iRoboTechDataContext iRoboTech;

      public Instagram(iRoboTechDataContext _iRoboTech)
      {
         iRoboTech = _iRoboTech;
      }

      private async Task<bool> LoginAsync()
      {
         try
         {
            var inst = iRoboTech.Robot_Instagrams.FirstOrDefault(i => i.STAT == "002" || i.Robot_Instagram_DirectMessages.Any(d => d.SEND_STAT == "005"));
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

      public async Task<bool> SendDirectMessageAsync(Data.Robot_Instagram_DirectMessage dir)
      {
         try
         {
            var _login = await LoginAsync();
            if (!_login) return false;

            if (dir.MESG_TYPE == "001")
            {
               var result = await _instaApi.MessagingProcessor.SendDirectTextAsync(dir.INST_PKID.ToString(), string.Empty, dir.MESG_TEXT);
               return result.Succeeded;
            }
            else if (dir.MESG_TYPE == "002")
            {
               var result = await _instaApi.MessagingProcessor.SendDirectPhotoAsync(null, new InstagramApiSharp.Classes.Models.InstaImage(@"D:\happynewyear.jpg", 1230, 1600), "");
               return result.Succeeded;
            }

            return false;
         }
         catch 
         {
            return false;
         }
      }
   }
}
