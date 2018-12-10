using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
//using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

using System.CodeDom;
using System.Text;
using System.Xml.Serialization;
using System.Threading;
using DevExpress.XtraEditors;


namespace System.RoboTech.Controller
{
   public class iRobot
   {
      //private static readonly TelegramBotClient Bot = new TelegramBotClient("273587585:AAFdrJate4ED0ccT4kGubo14xse4ZU81g7E");
      private TelegramBotClient Bot;
      private string Token;
      internal User Me;
      private string connectionString;
      private Data.Robot robot;
      public bool Started = false;

      public iRobot(string token, string connectionString, MemoEdit ConsoleOutLog_MemTxt, bool activeRobot = true, Data.Robot robot = null)
      {
         try
         {
            this.ConsoleOutLog_MemTxt = ConsoleOutLog_MemTxt;

            Bot = new TelegramBotClient(token);
            Token = token;
            this.robot = robot;
            Chats = new List<ChatInfo>();
            RobotHandle = new RobotController();
            this.connectionString = connectionString;

            Started = true;
            main(activeRobot);
         }
         catch (Exception exc) { this.ConsoleOutLog_MemTxt.Text = exc.Message; }
      }

      void main(bool activeRobot = true)
      {
         if (activeRobot)
         {
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;
         }
         Me = Bot.GetMeAsync().Result;

         //System.Diagnostics.Debug.WriteLine(Token);

         Bot.StartReceiving();
         //Console.ReadLine();
         //Bot.StopReceiving();
      }

      internal void StopReceiving()
      {
         Bot.StopReceiving();
         Started = false;
      }

      private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
      {
         throw new NotImplementedException();
      }

      private async void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
      {
         //throw new NotImplementedException();         
         return;
      }

      private async void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs e)
      {
         throw new NotImplementedException();
      }

      private async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
      {
         throw new NotImplementedException();
      }

      private async void BotOnMessageReceived(object sender, MessageEventArgs e)
      {         
         if (/*robot.SPY_TYPE == "001"*/ e.Message.Chat.Id > 0)
            await Robot_Interact(e);
         else
            await Robot_Spy(e);

         try
         {
            await Download_Media(Token, e.Message);
         }
         catch
         { }
      }

      private async Task Robot_Interact(MessageEventArgs e)
      {
         ChatInfo chat = null;
         try
         {
            //return;
            var message = e.Message;

            if (message == null) return;

            Data.iRoboTechDataContext iRobotTech = new Data.iRoboTechDataContext(connectionString);
            //ChatInfo chat = null;
            if (Chats.All(c => c.Message.Chat.Id != e.Message.Chat.Id))
               Chats.Add(new ChatInfo() { Message = message, LastVisitDate = DateTime.Now, Runed = false });
            chat = Chats.FirstOrDefault(c => c.Message.Chat.Id == message.Chat.Id);
            chat.Message = message;
            chat.LastVisitDate = DateTime.Now;
            var removeRam = Chats.Where(c => c.LastVisitDate.AddMinutes(5) < DateTime.Now);
            removeRam.ToList().Clear();

            if (chat.Message.Chat.Id < 0)
               return;

#if DEBUG
            try
            {
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", message.Chat.Id, message.From.FirstName + ", " + message.From.LastName, message.Text, Me.Username, DateTime.Now.ToString())));
               else
                  ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", message.Chat.Id, message.From.FirstName + ", " + message.From.LastName, message.Text, Me.Username, DateTime.Now.ToString());
               //Debug.WriteLine(string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}", message.Chat.Id, message.From.FirstName + ", " + message.From.LastName, message.Text, Me.Username, DateTime.Now.ToString()));
               //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Typing);
               //RobotClient.SendMessage(214695989, string.Format("Robot Id : {3} ,Chat Id : {0}, From : {1}, Message Text : {2}", newMsg.Message.Chat.Id, newMsg.Message.From.FirstName + ", " + newMsg.Message.From.LastName, newMsg.Message.Text, RobotClient.GetMe().UserName), null, null, null);
            }
            catch { }
#endif
            try
            {
               await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            }
            catch { return; }

            XElement xResult = new XElement("Respons", "No Message");

            // ارسال تبلیغات
            try
            {
               await Send_Advertising(Token, chat);
            }
            catch(Exception exc) {
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += exc.Message));
               else
                  ConsoleOutLog_MemTxt.Text += exc.Message;

               Bot.SendTextMessageAsync(
                  chat.Message.Chat.Id,
                  exc.Message
               );
            }
            try
            {
               await Send_Replay_Message(Token, chat);
            }
            catch(Exception exc) {
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += exc.Message));
               else
                  ConsoleOutLog_MemTxt.Text += exc.Message;

               Bot.SendTextMessageAsync(
                  chat.Message.Chat.Id,
                  exc.Message
               );
            }
            

            /* اگر برای اولین بار داره ربات رو اجرا می کنه می تونیم چک کنیم آیا متن مقدماتی داریم به بخوایم بهش نشون بدیم
             */
            #region "/Start"
            if (chat.Message.Text != null && chat.Message.Text.Length >= 6 && chat.Message.Text.ToLower().Substring(0, 6) == "/start")
            {
               var pics = (from o in iRobotTech.Organs
                           join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                           join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                           where o.STAT == "002"
                                 && r.STAT == "002"
                                 && p.STAT == "002"
                                 && r.TKON_CODE == Token
                                 && p.SHOW_STRT == "002"
                                 && p.IMAG_DESC != null
                           orderby p.ORDR descending
                           select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.ORDR }).ToList();

               if (iRobotTech.Robots.Any(r => r.TKON_CODE == Token && r.BULD_STAT != "006"))
               {
                  try
                  {
                     FileToSend fts = new FileToSend(iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == Token).BULD_FILE_ID);
                     //string fileid = iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == Token).BULD_FILE_ID;
                     await Bot.SendPhotoAsync(chat.Message.Chat.Id, fts, "کاربر گرامی نرم افزار در حال آماده سازی می باشد و هنوز یه مرحله نهایی نرسیده. بعداز اتمام از همین سامانه به شما اطلاع رسانی می شود");
                  }
                  catch { }
               }

               foreach (var pic in pics.Take(1))
               {
                  dynamic photo;
                  if (string.IsNullOrEmpty(pic.FILE_ID))
                  {
                     photo = new FileToSend()
                     {
                        Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        Filename = pic.FILE_NAME
                     };
                  }
                  else
                  {
                     photo = new FileToSend(pic.FILE_ID);
                  }
                  try
                  {
                     await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC);
                  }
                  catch { }

               }

               var desc = (from o in iRobotTech.Organs
                           join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                           join p in iRobotTech.Organ_Descriptions on o.OGID equals p.ORGN_OGID
                           where o.STAT == "002"
                                 && r.STAT == "002"
                                 && p.STAT == "002"
                                 && r.TKON_CODE == Token
                                 && p.SHOW_STRT == "002"
                                 && p.ITEM_DESC != null
                           orderby p.ORDR
                           select new { p.ITEM_VALU, p.ITEM_DESC }).ToList();
               string textmsg = "";
               foreach (var d in desc)
               {
                  textmsg += string.Format("{1}  {0} \n\r", d.ITEM_VALU, d.ITEM_DESC);
                  if (!string.IsNullOrEmpty(textmsg))
                     await Bot.SendTextMessageAsync(
                        chat.Message.Chat.Id,
                        textmsg
                        );
                  textmsg = "";
               }

               await Bot.SendTextMessageAsync(
                  chat.Message.Chat.Id,
                  string.Format("کد تلگرامی شما {0} می باشد", chat.Message.Chat.Id)
               );
            }
            #endregion

            #region "Check Menu on Request Contact OR Request Location"
            if(chat.UssdCode != null)
               switch (message.Type)
               {
                  case MessageType.ContactMessage:
                     message.Text = iRobotTech.Menu_Ussds.FirstOrDefault(m => m.ROBO_RBID == robot.RBID && m.Menu_Ussd1.USSD_CODE == chat.UssdCode && m.CMND_TYPE == "015").MENU_TEXT;
                     break;
                  case MessageType.LocationMessage:
                     message.Text = iRobotTech.Menu_Ussds.FirstOrDefault(m => m.ROBO_RBID == robot.RBID && m.Menu_Ussd1.USSD_CODE == chat.UssdCode && m.CMND_TYPE == "016").MENU_TEXT;
                     break;                  
               }
            #endregion

            #region "Found Menu"
            iRobotTech.Proccess_Message_P(
               new XElement("Robot",
                  new XAttribute("token", Token),
                  new XElement("Message",
                     new XAttribute("ussd", chat.UssdCode ?? ""),
                     new XAttribute("mesgid", chat.Message.MessageId),
                     new XAttribute("chatid", chat.Message.Chat.Id),
                     new XAttribute("refchatid", chat.Message.Text != null && chat.Message.Text.Length > 6 ? chat.Message.Text.ToLower().Substring(0, 6) == "/start" ? (chat.Message.Text.Split(' ').Count() > 1 ? (chat.Message.Text.Split(' ')[1]) : "") : "" : ""),
                     new XElement("Text", chat.Message.Text == null ? (chat.Message.Contact != null ? string.Format("{0}*{1}*{2}", chat.Message.Contact.FirstName + " , " + chat.Message.Contact.LastName, chat.Message.Contact.PhoneNumber, "آدرس نا مشخص") : "No Text") : chat.Message.Text),
                     new XElement("From",
                           new XAttribute("frstname", chat.Message.From.FirstName ?? ""),
                           new XAttribute("lastname", chat.Message.From.LastName ?? ""),
                           new XAttribute("username", chat.Message.From.Username ?? ""),
                           new XAttribute("id", chat.Message.From.Id)
                     ),
                     new XElement("Location",
                        new XAttribute("latitude", chat.Message.Location != null ? chat.Message.Location.Latitude : 0),
                        new XAttribute("longitude", chat.Message.Location != null ? chat.Message.Location.Longitude : 0)
                     ),
                     new XElement("Contact",
                        new XAttribute("frstname", chat.Message.Contact != null ? chat.Message.Contact.FirstName ?? "" : ""),
                        new XAttribute("lastname", chat.Message.Contact != null ? chat.Message.Contact.LastName ?? "" : ""),
                        new XAttribute("id", chat.Message.Contact != null ? chat.Message.Contact.UserId : 0),
                        new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber ?? "" : "")
                     )
                  )
               ),
               ref xResult
            );
            #endregion

            #region Create Menu Array
            KeyboardButton[][] keyBoardMarkup = null;
            if (xResult != null)
               keyBoardMarkup = CreateArray(xResult.Descendants("Text")/*.Select(x => x.Value)*/.ToList(), Convert.ToInt32(xResult.Descendants("Row").FirstOrDefault().Value), Convert.ToInt32(xResult.Descendants("Column").FirstOrDefault().Value));
            #endregion

            if (chat.Message.Text == "بازگشت به منوی اصلی" || chat.Message.Text == "🔺 بازگشت")
            {
               chat.ReadyToFire = false;
               chat.Runed = false;
            }

            #region "Go To First Menu"
            if (chat.Message.Text == "بازگشت به منوی اصلی")
            {
               var pics = (from o in iRobotTech.Organs
                           join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                           join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                           where o.STAT == "002"
                                 && r.STAT == "002"
                                 && p.STAT == "002"
                                 && r.TKON_CODE == Token
                                 && p.SHOW_STRT == "002"
                                 && p.IMAG_DESC != null
                           orderby p.ORDR
                           select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

               if (pics.Count() >= 1)
               {
                  var picindex = new Random().Next(0, pics.Count());
                  var pic = pics.ToList()[picindex];
                  //foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }
                     try
                     {
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC);
                     }
                     catch { }
                  }
               }
            }
            #endregion

            string messageText = chat.Message.Text ?? "No Menu";
            string ussdParentCode = chat.UssdCode ?? "";

            #region Find Menu
            var menucmndtype =
                           (from r in iRobotTech.Robots
                            join m in iRobotTech.Menu_Ussds on r.RBID equals m.ROBO_RBID
                            where r.TKON_CODE == Token
                               /*&& ((_messageText.Trim().StartsWith("*") && m.USSD_CODE == _messageText)
                                  || m.MENU_TEXT == (_messageText.Trim().StartsWith("*") ? m.MENU_TEXT : _messageText))
                               && (_messageText.Trim().StartsWith("*") ||
                                     (_ussdCode == ""
                                     || iRobotTech.Menu_Ussds
                                        .Where(mt => mt.ROBO_RBID == m.ROBO_RBID
                                                  && mt.USSD_CODE == _ussdCode
                                                  && mt.MUID      == m.MNUS_MUID).Any()))*/
                               && (
                                 (messageText.Trim().StartsWith("*") && m.USSD_CODE == messageText) ||

                                 ((ussdParentCode != "" && messageText != "") &&
                                  (m.MENU_TEXT == messageText &&
                                   iRobotTech.Menu_Ussds.Any(mt => mt.ROBO_RBID == m.ROBO_RBID
                                                  && mt.USSD_CODE == ussdParentCode
                                                  && mt.MUID == m.MNUS_MUID)
                                  )
                                 ) ||

                                 ((ussdParentCode == "" && messageText != "") &&
                                  (m.MENU_TEXT == messageText && m.ROOT_MENU == "002")
                                 )
                               )
                            select m).ToList().FirstOrDefault();

            /*
             * Temp Code
             */

            #region Developer Monitor
            if ((chat.Message.Caption != null && (chat.Message.Caption == "*#" || chat.Message.Caption.Substring(0, 2) == "*#")) ||
               (chat.Message.Text != null && chat.Message.Text.Length >= 2 && (chat.Message.Text == "*#" || chat.Message.Text.Substring(0, 2) == "*#")))
            {
               string fileid = "";
               string filetype = "";
               /*
                  VALU	DOMN_DESC
                  001	Message
                  002	Picture
                  003	Video
                  004	File & Document
                  005	Location
                  006	Audio
                  007	Sticker
                */
               //if (menucmndtype != null)
               //   await Bot.SendTextMessageAsync(chat.Message.Chat.Id, menucmndtype.USSD_CODE + ", " + (chat.UssdCode ?? "No Parent"));
               if (e.Message.Photo != null)
               {
                  await Bot.SendTextMessageAsync(chat.Message.Chat.Id, "Photo :\n\r\n\r" + e.Message.Photo.Reverse().FirstOrDefault().FileId);
                  fileid = e.Message.Photo.Reverse().FirstOrDefault().FileId;
                  filetype = "002";
                  //await Bot.GetFileAsync(chat.Message.Photo.Reverse().FirstOrDefault().FileId, new FileStream(@"C:\Image\MyFile.jpg", FileMode.OpenOrCreate));
               }
               if (e.Message.Video != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Video :\n\r\n\r" + e.Message.Video.FileId);
                  fileid = e.Message.Video.FileId;
                  filetype = "003";
               }
               if (e.Message.Document != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Document :\n\r\n\r" + e.Message.Document.FileId);
                  fileid = e.Message.Document.FileId;
                  filetype = "004";
               }
               if (e.Message.Audio != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Audio :\n\r\n\r" + e.Message.Audio.FileId);
                  fileid = e.Message.Audio.FileId;
                  filetype = "006";
               }
               if (e.Message.Location != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, string.Format("X : {0}\n\rY : {1}", e.Message.Location.Latitude, e.Message.Location.Longitude));
                  filetype = "005";
               }
               if (e.Message.Sticker != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Sticker :\n\r\n\r" + e.Message.Sticker.FileId);
                  await Bot.SendStickerAsync(e.Message.Chat.Id, /*"BQADBAADOwMAAgXhMAewhyhhPvCl1QI"*/new FileToSend(e.Message.Sticker.FileId), false, e.Message.MessageId);
                  fileid = e.Message.Sticker.FileId;
                  filetype = "007";
               }
               try
               {
                  //if (menucmndtype.CMND_TYPE != "018" && fileid != "")
                  if (chat.Message.Caption != null && chat.Message.Caption != "*#")
                  {
                     iRobotTech.INS_OGMD_P(
                        new XElement("Robot",
                           new XAttribute("tokencode", Token),
                           new XElement("File",
                              new XAttribute("id", fileid),
                              new XAttribute("ussdcode", chat.Message.Caption.Substring(chat.Message.Caption.IndexOf('*', 2))),
                              new XAttribute("cmndtype", chat.Message.Caption.Substring(2, chat.Message.Caption.IndexOf('*', 2) - 2)),
                              new XAttribute("filetype", filetype)
                           )
                        )
                     );
                  }
                  else if (chat.Message.Text != null && chat.Message.Text != "*#")
                     iRobotTech.INS_OGMD_P(
                        new XElement("Robot",
                           new XAttribute("tokencode", Token),
                           new XElement("File",
                              new XAttribute("ussdcode", chat.Message.Text.Substring(chat.Message.Text.IndexOf('*', 2))),
                              new XAttribute("cmndtype", chat.Message.Text.Substring(2, chat.Message.Text.IndexOf('*', 2) - 2))
                           )
                        )
                     );
               }
               catch { }
            }
            #endregion
            #region Service Uploading
            if (ussdParentCode != "" && (e.Message.Photo != null || e.Message.Video != null || e.Message.Document != null || e.Message.Audio != null || e.Message.Sticker != null))
            {
               var parentmenu = (from r in iRobotTech.Robots
                                 join m in iRobotTech.Menu_Ussds on r.RBID equals m.ROBO_RBID
                                 where r.TKON_CODE == Token
                                    && m.USSD_CODE == ussdParentCode
                                    && m.CMND_TYPE == "018"
                                 select m).ToList().FirstOrDefault();

               // 1396/07/22 * بدست آوردن مرجعی برای ذخیره سازی اطلاعات فایل های دریافتی برای ربات
               var filestorage = "";
               if (robot.DOWN_LOAD_FILE_PATH != "" && robot.DOWN_LOAD_FILE_PATH.Length >= 10)
                  filestorage = robot.DOWN_LOAD_FILE_PATH;
               else filestorage = null;

               if (parentmenu != null && filestorage != null)
                  parentmenu.UPLD_FILE_PATH = filestorage;


               if (parentmenu != null)
               {
                  var datenow = iRobotTech.GET_MTOS_U(DateTime.Now).Replace("/", "_");
                  if ((parentmenu.UPLD_FILE_PATH ?? @"D:\") != "")
                  {
                     var fileupload = (parentmenu.UPLD_FILE_PATH ?? @"D:") + "\\" + Me.Username + "\\" + datenow + "\\" + chat.Message.Chat.Id.ToString() + "\\" + parentmenu.MENU_TEXT;
                     var filename = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString();
                     if (!Directory.Exists(fileupload))
                     {
                        DirectoryInfo di = Directory.CreateDirectory(fileupload);
                     }

                     var serviceRobot =
                        iRobotTech.Service_Robots
                        .Where(sr => sr.CHAT_ID == chat.Message.Chat.Id &&
                                     sr.Robot.TKON_CODE == Token
                        ).ToList().FirstOrDefault();

                     if (e.Message.Photo != null)
                     {
                        try
                        {
                           lock (Bot)
                           {
                              iRobotTech.INS_SRUL_P(
                                 serviceRobot.SERV_FILE_NO,
                                 serviceRobot.ROBO_RBID,
                                 e.Message.Chat.Id,
                                 fileupload,
                                 e.Message.Photo.Reverse().FirstOrDefault().FileId,
                                 "001",
                                 DateTime.Now,
                                 parentmenu.USSD_CODE,
                                 filename
                              );                             
                              // 1396/07/21 * After Update Telegram
                              //Bot.GetFileAsync(e.Message.Photo.Reverse().FirstOrDefault().FileId, System.IO.File.Create(fileupload + "\\" + filename + ".jpg"));
                           }

                           await Bot.GetFileAsync(e.Message.Photo.Reverse().FirstOrDefault().FileId, System.IO.File.Create(fileupload + "\\" + filename + ".jpg"));
                           await Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل عکس شما با موفقیت ذخیره گردید 💾", ParseMode.Default, false, false, e.Message.MessageId, null);
                        }
                        catch(Exception ex) {
                           Bot.SendTextMessageAsync(e.Message.Chat.Id, ex.Message, ParseMode.Default, false, false, e.Message.MessageId, null);
                        }

                     }
                     else if (e.Message.Video != null)
                     {
                        try
                        {
                           lock (Bot)
                           {
                              iRobotTech.INS_SRUL_P(
                                 serviceRobot.SERV_FILE_NO,
                                 serviceRobot.ROBO_RBID,
                                 e.Message.Chat.Id,
                                 fileupload,
                                 e.Message.Video.FileId,
                                 "002",
                                 DateTime.Now,
                                 parentmenu.USSD_CODE,
                                 filename
                              );

                              // 1396/07/21 * After Update Telegram
                              //Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل تصویری شما با موفقیت ذخیره گردید 💾", false, false, e.Message.MessageId, null, ParseMode.Default);
                              //Bot.GetFile(e.Message.Video.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Video.FileId*/ filename));
                           }
                           
                           await Bot.GetFileAsync(e.Message.Video.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Video.FileId*/ filename));
                           await Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل تصویری شما با موفقیت ذخیره گردید 💾", ParseMode.Default, false, false, e.Message.MessageId, null);
                        }
                        catch { }
                     }
                     else if (e.Message.Document != null)
                     {
                        try
                        {
                           lock (Bot)
                           {
                              iRobotTech.INS_SRUL_P(
                                 serviceRobot.SERV_FILE_NO,
                                 serviceRobot.ROBO_RBID,
                                 e.Message.Chat.Id,
                                 fileupload,
                                 e.Message.Document.FileId,
                                 "003",
                                 DateTime.Now,
                                 parentmenu.USSD_CODE,
                                 filename
                              );
                              // 1396/07/21 * After Telegram Update
                              //Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل مستند شما با موفقیت ذخیره گردید 💾", false, false, e.Message.MessageId, null, ParseMode.Default);
                              //Bot.GetFile(e.Message.Document.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Document.FileId*/ filename));
                           }
                           await Bot.GetFileAsync(e.Message.Document.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Document.FileId*/ filename));
                           await Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل مستند شما با موفقیت ذخیره گردید 💾", ParseMode.Default, false, false, e.Message.MessageId, null);                           
                        }
                        catch { }
                     }
                     else if (e.Message.Audio != null)
                     {
                        try
                        {
                           lock (Bot)
                           {
                              iRobotTech.INS_SRUL_P(
                                 serviceRobot.SERV_FILE_NO,
                                 serviceRobot.ROBO_RBID,
                                 e.Message.Chat.Id,
                                 fileupload,
                                 e.Message.Audio.FileId,
                                 "004",
                                 DateTime.Now,
                                 parentmenu.USSD_CODE,
                                 filename
                              );
                              
                              // 1396/07/21 * After Telegram Update
                              //Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل صوتی شما با موفقیت ذخیره گردید 💾", false, false, e.Message.MessageId, null, ParseMode.Default);
                              //Bot.GetFile(e.Message.Audio.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Audio.FileId*/ filename));
                           }                           
                           await Bot.GetFileAsync(e.Message.Audio.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Audio.FileId*/ filename));
                           await Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل صوتی شما با موفقیت ذخیره گردید 💾", ParseMode.Default, false, false, e.Message.MessageId, null);                                                      
                        }
                        catch { }
                     }
                     else if (e.Message.Sticker != null)
                     {
                        try
                        {
                           lock (Bot)
                           {
                              iRobotTech.INS_SRUL_P(
                                 serviceRobot.SERV_FILE_NO,
                                 serviceRobot.ROBO_RBID,
                                 e.Message.Chat.Id,
                                 fileupload,
                                 e.Message.Sticker.FileId,
                                 "005",
                                 DateTime.Now,
                                 parentmenu.USSD_CODE,
                                 filename
                              );

                              // 1396/07/21 * After Telegram Update
                              //Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل استیکر شما با موفقیت ذخیره گردید 💾", false, false, e.Message.MessageId, null, ParseMode.Default);
                              //Bot.GetFile(e.Message.Sticker.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Sticker.FileId*/ filename));
                           }
                           
                           await Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل استیکر شما با موفقیت ذخیره گردید 💾", ParseMode.Default, false, false, e.Message.MessageId, null);
                           await Bot.GetFileAsync(e.Message.Sticker.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Sticker.FileId*/ filename));
                        }
                        catch { }
                     }
                     //iRobotTech.SubmitChanges();
                  }
               }
            }
            #endregion
            #endregion

            #region Operation
            if (chat.ReadyToFire && !chat.Runed)
            {
               chat.ReadyToFire = false;
               if (chat.CommandRunPlace == "001")
                  chat.Runed = true;

               if (!chat.Runed && keyBoardMarkup != null)
               {
                  string elmntype = "001"; // متن ساده   
                  string mimetype = ""; // پسوند فایل
                  string filename = "";
                  string fileext = "";
                  if (chat.Message.Photo != null)
                  {
                     elmntype = "002";
                     mimetype = "application/jpg";
                     filename = e.Message.Photo.Reverse().FirstOrDefault().FileId + ".jpg";
                     fileext = "jpg";
                  }
                  if (chat.Message.Video != null)
                  {
                     elmntype = "003";
                     mimetype = chat.Message.Video.MimeType;
                  }
                  if (chat.Message.Document != null)
                  {
                     elmntype = "004";
                     mimetype = chat.Message.Document.MimeType;
                     filename = e.Message.Document.FileName;
                     fileext = e.Message.Document.FileName.Substring(e.Message.Document.FileName.LastIndexOf('.') + 1);
                  }
                  if (chat.Message.Location != null)
                  {
                     elmntype = "005";                     
                  }
                  if(chat.Message.Audio != null)
                  {
                     elmntype = "006";
                     mimetype = chat.Message.Audio.MimeType;
                     filename = e.Message.Audio.FileId;
                  }
                  try                  
                  {
                     var xmlmsg = RobotHandle.GetData(
                        new XElement("Robot",
                           new XAttribute("token", Token),
                           new XElement("Message",
                              new XAttribute("ussd", chat.UssdCode ?? ""),
                              new XAttribute("childussd", menucmndtype != null ? menucmndtype.USSD_CODE ?? "" : ""),
                              new XAttribute("chatid", chat.Message.Chat.Id),
                              new XAttribute("elmntype", elmntype),
                              new XAttribute("mimetype", mimetype),
                              new XAttribute("filename", filename),
                              new XAttribute("fileext", fileext),
                              new XAttribute("mesgid", chat.Message.MessageId),
                              new XElement("Text", chat.Message.Text == null ? (chat.Message.Contact != null ? string.Format("{0}*{1}*{2}", chat.Message.Contact.FirstName + " , " + chat.Message.Contact.LastName, chat.Message.Contact.PhoneNumber, "متن نا مشخص") : (chat.Message.Caption == null ? "No Text" : chat.Message.Caption)) : chat.Message.Text),
                              new XElement("Location",
                                 new XAttribute("latitude", chat.Message.Location != null ? chat.Message.Location.Latitude : 0),
                                 new XAttribute("longitude", chat.Message.Location != null ? chat.Message.Location.Longitude : 0)
                              ),
                              new XElement("Photo",
                                 new XAttribute("fileid", chat.Message.Photo != null ? chat.Message.Photo.Reverse().FirstOrDefault().FileId : "")
                              ),
                              new XElement("Video",
                                 new XAttribute("fileid", chat.Message.Video != null ? chat.Message.Video.FileId : "")
                              ),
                              new XElement("Document",
                                 new XAttribute("fileid", chat.Message.Document != null ? chat.Message.Document.FileId : "")
                              ),
                              new XElement("Audio",
                                 new XAttribute("fileid", chat.Message.Audio != null ? chat.Message.Audio.FileId : "")
                              )
                           )
                        ), connectionString);

                     var rmessage = xmlmsg.Descendants("Message").FirstOrDefault().Value;

                     bool visited = false;
                     try
                     {
                        var tdata = XDocument.Parse(rmessage).Elements().First();
                        var xdata = xmlmsg;//XDocument.Parse(message).Elements().First();
                        await FireEventResultOpration(chat, keyBoardMarkup, xdata);
                        visited = true;
                     }
                     catch { visited = false; }

                     try
                     {
                        if (!visited)
                           await MessagePaging(chat, rmessage, keyBoardMarkup);
                     }
                     catch { }

                     await Send_Order(iRobotTech, keyBoardMarkup);
                  }
                  catch
                  {
                     //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال اطلاعات مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId, null);
                  }
                  //chat.Runed = true;
               }
            }
            else if (menucmndtype != null && menucmndtype.CMND_TYPE != null && new List<string> { "001", "002", "003", "004", "005", "006", "007", "008", "009", "010", "011", "012", "013", "014", "015", "016", "017","018", "019", "020" }.Contains(menucmndtype.CMND_TYPE))
            {
               /*
                * 001 - Location
                * 002 - Image & Text
                * 003 - Video
                * 004 - Document & File
                * 005 - Info Text
                * 006 - Image & Text, Video
                * 007 - Image & Text, Video, Text
                * 008 - Fire Event
                * 009 - Image && Info text
                * 010 - Image & Location
                * 011 - Fire Event & Continue
                * 012 - Branch Location
                * 013 - Document & File & Info Text
                * 014 - Imgae & Audio & Info Text
                * 015 - Request Contact
                * 016 - Request Location
                * 017 - Image & Document
                * 018 - Upload
                * 019 - Show Upload
                * 020 - Invite Friend
                */
               if (menucmndtype.CMND_TYPE == "001")
               {
                  #region Location
                  // 001 - Location
                  var loc = iRobotTech.Organs
                  .Join(iRobotTech.Robots, organ => organ.OGID, robot => robot.ORGN_OGID,
                  (organ, robot) => new { organ.CORD_X, organ.CORD_Y, robot.TKON_CODE }).ToList().FirstOrDefault(or => or.TKON_CODE == Token);

                  chat.Runed = false;

                  if (keyBoardMarkup != null)
                  {
                     float cordx = Convert.ToSingle(loc.CORD_X.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                     float cordy = Convert.ToSingle(loc.CORD_Y.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                     await Bot.SendLocationAsync(
                        chat.Message.Chat.Id,
                        cordx,
                        cordy,
                        replyToMessageId: chat.Message.MessageId,
                        replyMarkup:
                        new ReplyKeyboardMarkup()
                        {
                           Keyboard = keyBoardMarkup,
                           ResizeKeyboard = true,
                           Selective = true
                        });
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "002")
               {
                  #region Image & Text
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId: chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        /*Bot.SendTextMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", 
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });*/
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "003")
               {
                  #region Video
                  // 003 - Video
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_video);
                        await Bot.SendVideoAsync(chat.Message.Chat.Id, photo,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال فیلم مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "004")
               {
                  #region Document & File
                  // 004 - Document & File
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_document);
                        await Bot.SendDocumentAsync(chat.Message.Chat.Id, photo,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال مستندات مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "005")
               {
                  #region Info Text
                  // 005 - Info Text
                  chat.Runed = false;
                  var desc = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Descriptions on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.ITEM_DESC != null
                              orderby p.ORDR
                              select new { p.ITEM_VALU, p.ITEM_DESC }).ToList();
                  string textmsg = "";
                  foreach (var d in desc)
                  {
                     textmsg += string.Format("{1} : {0} \n\r", d.ITEM_VALU, d.ITEM_DESC);
                     if (keyBoardMarkup != null)
                        await Bot.SendTextMessageAsync(
                           chat.Message.Chat.Id,
                           textmsg,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     textmsg = "";
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "006")
               {
                  #region Image & Text
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
                  #region Video
                  // 003 - Video
                  chat.Runed = false;
                  var videos = (from o in iRobotTech.Organs
                                join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                                join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                                where o.STAT == "002"
                                      && r.STAT == "002"
                                      && p.STAT == "002"
                                      && p.USSD_CODE == menucmndtype.USSD_CODE
                                      && r.TKON_CODE == Token
                                //&& p.IMAG_DESC != null
                                orderby p.ORDR
                                select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var video in videos)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(video.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(video.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = video.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(video.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_video);
                        await Bot.SendVideoAsync(chat.Message.Chat.Id, photo,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال فیلم مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "007")
               {
                  #region Image & Text
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
                  #region Video
                  // 003 - Video
                  chat.Runed = false;
                  var videos = (from o in iRobotTech.Organs
                                join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                                join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                                where o.STAT == "002"
                                      && r.STAT == "002"
                                      && p.STAT == "002"
                                      && p.USSD_CODE == menucmndtype.USSD_CODE
                                      && r.TKON_CODE == Token
                                //&& p.IMAG_DESC != null
                                orderby p.ORDR
                                select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var video in videos)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(video.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(video.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = video.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(video.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendVideoAsync(chat.Message.Chat.Id, photo,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال فیلم مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
                  #region Info Text
                  // 005 - Info Text
                  chat.Runed = false;
                  var desc = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Descriptions on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.ITEM_DESC != null
                              orderby p.ORDR
                              select new { p.ITEM_VALU, p.ITEM_DESC }).ToList();
                  string textmsg = "";
                  foreach (var d in desc)
                  {
                     textmsg += string.Format("{1} : {0} \n\r", d.ITEM_VALU, d.ITEM_DESC);
                     if (keyBoardMarkup != null)
                        await Bot.SendTextMessageAsync(
                           chat.Message.Chat.Id,
                           textmsg,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     textmsg = "";
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "008")
               {
                  #region Fire Event

                  string elmntype = "001"; // متن ساده   

                  if (chat.Message.Photo != null)
                     elmntype = "002";
                  if (chat.Message.Video != null)
                     elmntype = "003";
                  if (chat.Message.Document != null)
                     elmntype = "004";
                  if (chat.Message.Location != null)
                     elmntype = "005";

                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", Token),
                       new XElement("Message",
                          new XAttribute("ussd", chat.UssdCode ?? ""),
                          new XAttribute("childussd", menucmndtype != null ? menucmndtype.USSD_CODE ?? "" : ""),
                          new XAttribute("chatid", chat.Message.Chat.Id),
                          new XAttribute("elmntype", elmntype),
                          new XElement("Text", chat.Message.Text),
                          new XElement("Location",
                             new XAttribute("latitude", chat.Message.Location != null ? chat.Message.Location.Latitude : 0),
                             new XAttribute("longitude", chat.Message.Location != null ? chat.Message.Location.Longitude : 0)
                          ),
                          new XElement("Photo",
                             new XAttribute("fileid", chat.Message.Photo != null ? chat.Message.Photo.Reverse().FirstOrDefault().FileId : "")
                          )
                       )
                    ), connectionString);

                  /*
                   * 001 - Location
                   * 002 - Image & Text
                   * 003 - Video
                   * 004 - Document & File
                   * 005 - Info Text
                   * 006 - Image & Text, Video
                   * 007 - Image & Text, Video, Text
                   * 008 - Fire Event
                   */
                  await FireEventResultOpration(chat, keyBoardMarkup, xdata);

                  await Send_Order(iRobotTech, keyBoardMarkup);
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "009")
               {
                  #region Image & Text
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId: chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
                  #region Info Text
                  // 005 - Info Text
                  chat.Runed = false;
                  var desc = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Descriptions on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.ITEM_DESC != null
                              orderby p.ORDR
                              select new { p.ITEM_VALU, p.ITEM_DESC }).ToList();
                  string textmsg = "";
                  foreach (var d in desc)
                  {
                     textmsg += string.Format("{1} : {0} \n\r", d.ITEM_VALU, d.ITEM_DESC);
                     if (keyBoardMarkup != null)
                        await Bot.SendTextMessageAsync(
                           chat.Message.Chat.Id,
                           textmsg,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     textmsg = "";
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "010")
               {
                  #region Image & Text
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId: chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
                  #region Location
                  // 001 - Location
                  var loc = iRobotTech.Organs
                  .Join(iRobotTech.Robots, organ => organ.OGID, robot => robot.ORGN_OGID,
                  (organ, robot) => new { organ.CORD_X, organ.CORD_Y, robot.TKON_CODE }).ToList().FirstOrDefault(or => or.TKON_CODE == Token);

                  chat.Runed = false;

                  if (keyBoardMarkup != null)
                  {
                     float cordx = Convert.ToSingle(loc.CORD_X.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                     float cordy = Convert.ToSingle(loc.CORD_Y.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                     await Bot.SendLocationAsync(
                        chat.Message.Chat.Id,
                        cordx,
                        cordy,
                        replyToMessageId:
                        chat.Message.MessageId,
                        replyMarkup:
                        new ReplyKeyboardMarkup()
                        {
                           Keyboard = keyBoardMarkup,
                           ResizeKeyboard = true,
                           Selective = true
                        });
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "011")
               {
                  #region Fire Event & Continue

                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", Token),
                       new XElement("Message",
                          new XAttribute("ussd", chat.UssdCode ?? ""),
                          new XAttribute("childussd", menucmndtype != null ? menucmndtype.USSD_CODE ?? "" : ""),
                          new XAttribute("chatid", chat.Message.Chat.Id),
                          new XElement("Text", chat.Message.Text),
                          new XElement("Location",
                             new XAttribute("latitude", chat.Message.Location != null ? chat.Message.Location.Latitude : 0),
                             new XAttribute("longitude", chat.Message.Location != null ? chat.Message.Location.Longitude : 0)
                          ),
                          new XElement("Photo",
                             new XAttribute("fileid", chat.Message.Photo != null ? chat.Message.Photo.Reverse().FirstOrDefault().FileId : "")
                          )
                       )
                    ), connectionString);

                  /*
                   * 001 - Location
                   * 002 - Image & Text
                   * 003 - Video
                   * 004 - Document & File
                   * 005 - Info Text
                   * 006 - Image & Text, Video
                   * 007 - Image & Text, Video, Text
                   * 008 - Fire Event
                   */
                  await FireEventResultOpration(chat, keyBoardMarkup, xdata);

                  await Send_Order(iRobotTech, keyBoardMarkup);
                  #region Execute Fire Event

                  #endregion
                  #region Show Menu
                  await Bot.SendTextMessageAsync(
                     chat.Message.Chat.Id,
                     xResult.Descendants("Description").FirstOrDefault().Value,
                     replyToMessageId:
                     chat.Message.MessageId,
                     replyMarkup:
                     new ReplyKeyboardMarkup()
                     {
                        Keyboard = keyBoardMarkup,
                        ResizeKeyboard = true,
                        Selective = true
                     });
                  #endregion
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "012")
               {
                  #region Branch Location
                  // 012 - Branch Location 
                  var locs = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Representations on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && r.TKON_CODE == Token
                              select new { p.PLAC_ADRS, p.CORD_X, p.CORD_Y }).ToList();

                  chat.Runed = false;

                  if (keyBoardMarkup != null)
                  {
                     foreach (var loc in locs)
                     {
                        await Bot.SendLocationAsync(
                        chat.Message.Chat.Id,
                        (float)loc.CORD_X.Value,
                        (float)loc.CORD_Y.Value,
                        replyToMessageId:
                        chat.Message.MessageId,
                        replyMarkup:
                        new ReplyKeyboardMarkup()
                        {
                           Keyboard = keyBoardMarkup,
                           ResizeKeyboard = true,
                           Selective = true
                        });
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "013")
               {
                  #region Info Text
                  // 005 - Info Text
                  chat.Runed = false;
                  var desc = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Descriptions on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.ITEM_DESC != null
                              orderby p.ORDR
                              select new { p.ITEM_VALU, p.ITEM_DESC }).ToList();
                  string textmsg = "";
                  foreach (var d in desc)
                  {
                     textmsg += string.Format("{1} : {0} \n\r", d.ITEM_VALU, d.ITEM_DESC);
                     if (keyBoardMarkup != null)
                        await Bot.SendTextMessageAsync(
                           chat.Message.Chat.Id,
                           textmsg,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     textmsg = "";
                  }
                  #endregion
                  #region Document & File
                  // 004 - Document & File
                  chat.Runed = false;
                  var docs = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var doc in docs)
                  {
                     dynamic document;
                     if (string.IsNullOrEmpty(doc.FILE_ID))
                     {
                        document = new FileToSend()
                        {
                           Content = new FileStream(doc.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = doc.FILE_NAME
                        };
                     }
                     else
                     {
                        document = new FileToSend(doc.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_document);
                        await Bot.SendDocumentAsync(chat.Message.Chat.Id, document,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال مستندات مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "014")
               {
                  #region Image & Text
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        /*RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        new ReplyKeyboardMarkup()
                        {
                           Keyboard = keyBoardMarkup,
                           ResizeKeyboard = true,
                           Selective = true
                        });*/
                     }
                  }
                  #endregion
                  #region Info Text
                  // 005 - Info Text
                  chat.Runed = false;
                  var desc = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Descriptions on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.ITEM_DESC != null
                              orderby p.ORDR
                              select new { p.ITEM_VALU, p.ITEM_DESC }).ToList();
                  string textmsg = "";
                  foreach (var d in desc)
                  {
                     textmsg += string.Format("{1} : {0} \n\r", d.ITEM_VALU, d.ITEM_DESC);
                     if (keyBoardMarkup != null)
                        await Bot.SendTextMessageAsync(
                           chat.Message.Chat.Id,
                           textmsg,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     textmsg = "";
                  }
                  #endregion
                  #region Audio
                  // 004 - Document & File
                  chat.Runed = false;
                  var sounds = (from o in iRobotTech.Organs
                                join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                                join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                                where o.STAT == "002"
                                      && r.STAT == "002"
                                      && p.STAT == "002"
                                      && p.USSD_CODE == menucmndtype.USSD_CODE
                                      && r.TKON_CODE == Token
                                //&& p.IMAG_DESC != null
                                orderby p.ORDR
                                select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var sound in sounds)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(sound.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(sound.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = sound.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(sound.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendAudioAsync(chat.Message.Chat.Id, photo, "*", 0, sound.IMAG_DESC,"#",
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        /*RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال مستندات مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        new ReplyKeyboardMarkup()
                        {
                           Keyboard = keyBoardMarkup,
                           ResizeKeyboard = true,
                           Selective = true
                        });*/
                     }
                  }
                  #endregion
               }
               else if(menucmndtype.CMND_TYPE == "015")
               {
                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", Token),
                       new XElement("Message",
                          new XAttribute("ussd", chat.UssdCode ?? ""),
                          new XAttribute("childussd", menucmndtype != null ? menucmndtype.USSD_CODE ?? "" : ""),
                          new XAttribute("chatid", chat.Message.Chat.Id),
                          new XAttribute("elmntype", "008"),
                          new XElement("Text", chat.Message.Text),
                          new XElement("Contact",
                             new XAttribute("phonnumb", chat.Message.Contact.PhoneNumber)
                          )
                       )
                    ), connectionString);

                  await FireEventResultOpration(chat, keyBoardMarkup, xdata);

                  await Send_Order(iRobotTech, keyBoardMarkup);
               }
               else if (menucmndtype.CMND_TYPE == "017")
               {
                  #region Image & Text
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        /*RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        new ReplyKeyboardMarkup()
                        {
                           Keyboard = keyBoardMarkup,
                           ResizeKeyboard = true,
                           Selective = true
                        });*/
                     }
                  }
                  #endregion
                  #region Document & File
                  // 004 - Document & File
                  chat.Runed = false;
                  var docs = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var doc in docs)
                  {
                     dynamic document;
                     if (string.IsNullOrEmpty(doc.FILE_ID))
                     {
                        document = new FileToSend()
                        {
                           Content = new FileStream(doc.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = doc.FILE_NAME
                        };
                     }
                     else
                     {
                        document = new FileToSend(doc.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_document);
                        await Bot.SendDocumentAsync(chat.Message.Chat.Id, document,
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        //RobotClient.SendMessage(chat.Message.Chat.Id, "ارسال مستندات مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", null, chat.Message.MessageId,
                        //new ReplyKeyboardMarkup()
                        //{
                        //   Keyboard = keyBoardMarkup,
                        //   ResizeKeyboard = true,
                        //   Selective = true
                        //});
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "018")
               {
                  #region Upload
                  // 002 - Image & Text
                  chat.Runed = false;
                  var files = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join u in iRobotTech.Service_Robot_Uploads on r.RBID equals u.SRBT_ROBO_RBID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                 //&& u.STAT == "002"
                                    && u.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == Token
                                    && u.FILE_ID != null
                                    && u.CHAT_ID == e.Message.Chat.Id
                              orderby u.RWNO
                              select new { u.FILE_NAME, u.FILE_PATH, IMAG_DESC = u.FILE_PATH.Substring(u.FILE_PATH.LastIndexOf('\\') + 1), u.FILE_ID }).ToList();

                  await Bot.SendTextMessageAsync(
                     e.Message.Chat.Id,
                     string.Format("🗂 تعداد فایل های ذخیره شده : {0}", files.Count), 
                     ParseMode.Default,
                     false, false,
                     e.Message.MessageId, 
                     new ReplyKeyboardMarkup()
                     {
                        Keyboard = keyBoardMarkup,
                        ResizeKeyboard = true,
                        Selective = true
                     });
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "019")
               {
                  #region Show Upload
                  // 002 - Image & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join u in iRobotTech.Service_Robot_Uploads on r.RBID equals u.SRBT_ROBO_RBID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    //&& u.STAT == "002"
                                    && u.USSD_CODE == menucmndtype.MNUS_USSD_CODE
                                    && r.TKON_CODE == Token
                                    && u.FILE_ID != null
                                    && u.CHAT_ID == chat.Message.Chat.Id
                              orderby u.RWNO
                              select new { u.FILE_NAME, u.FILE_PATH, IMAG_DESC = u.FILE_PATH.Substring(u.FILE_PATH.LastIndexOf('\\') + 1), u.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        photo = new FileToSend()
                        {
                           Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                           Filename = pic.FILE_NAME
                        };
                     }
                     else
                     {
                        photo = new FileToSend(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC,
                           replyToMessageId: chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                     }
                     catch
                     {
                        /*Bot.SendTextMessage(chat.Message.Chat.Id, "ارسال عکس مورد نظر با اشکال مواجه شد. لطفا به بخش پشتیبانی با شماره 09333617031 تماس بگیرید", 
                           replyToMessageId:
                           chat.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });*/
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "020")
               {
                  #region Invite Friend
                  chat.Runed = false;
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, 
                           string.Format("{0}\n\rhttps://telegram.me/{1}?start={2}", robot.INVT_FRND ?? "از اینکه دوستان خود را به ما معرفی میکنید بسیار ممنون و خرسندیم، لینک شما برای دعوت کردن دوستان", robot.NAME.Substring(1), e.Message.Chat.Id),
                           ParseMode.Default,
                           replyToMessageId:
                           e.Message.MessageId,
                           replyMarkup:
                           new ReplyKeyboardMarkup()
                           {
                              Keyboard = keyBoardMarkup,
                              ResizeKeyboard = true,
                              Selective = true
                           });
                  #endregion
               }
            }
            else
            {
               chat.Runed = false;
               if (keyBoardMarkup != null)
               {
                  try
                  {
                     await MessagePaging(chat, xResult.Descendants("Description").FirstOrDefault().Value, keyBoardMarkup);
                  }
                  catch { return; }
               }
            }
            try
            {
               chat.UssdCode = xResult.Descendants("UssdCode").FirstOrDefault().Value;
               chat.ReadyToFire = xResult.Descendants("ReadyToFire").FirstOrDefault().Value == "002" ? true : false;
               chat.CommandRunPlace = xResult.Descendants("CommandRunPlace").FirstOrDefault().Value;
            }
            catch
            {
               chat.UssdCode = "";
               chat.ReadyToFire = false;
               chat.CommandRunPlace = "001";
            }
            #endregion
         }
         catch (Exception exc)
         {
            if (ConsoleOutLog_MemTxt.InvokeRequired)
               ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, exc.Message, Me.Username, DateTime.Now.ToString())));
            else
               ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, exc.Message, Me.Username, DateTime.Now.ToString());
            BotOnStarted(chat);
         }
      }

      private async Task Robot_Spy(MessageEventArgs e)
      {
#if DEBUG
         try
         {
            if (ConsoleOutLog_MemTxt.InvokeRequired)
               ConsoleOutLog_MemTxt.Invoke(
                  new Action(
                     () => 
                        ConsoleOutLog_MemTxt.Text += 
                        string.Format("Robot Id : {3} * {5}, DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", 
                        e.Message.Chat.Id, 
                        e.Message.From.FirstName + ", " + e.Message.From.LastName, 
                        e.Message.Text, 
                        Me.Username,                         
                        DateTime.Now.ToString(),
                        e.Message.Chat.Username)));
            else
               ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", e.Message.Chat.Id, e.Message.From.FirstName + ", " + e.Message.From.LastName, e.Message.Text, Me.Username, DateTime.Now.ToString());
            //Debug.WriteLine(string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}", message.Chat.Id, message.From.FirstName + ", " + message.From.LastName, message.Text, Me.Username, DateTime.Now.ToString()));
            //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Typing);
            //RobotClient.SendMessage(214695989, string.Format("Robot Id : {3} ,Chat Id : {0}, From : {1}, Message Text : {2}", newMsg.Message.Chat.Id, newMsg.Message.From.FirstName + ", " + newMsg.Message.From.LastName, newMsg.Message.Text, RobotClient.GetMe().UserName), null, null, null);

            var x =
               new XElement("Robot_Spy_Message_Group",
                  new XAttribute("rbid", robot.RBID),
                  new XElement("Group",
                     new XAttribute("code", e.Message.Chat.Id),
                     new XAttribute("titl", e.Message.Chat.Title),
                     new XAttribute("type", e.Message.Chat.Type)
                  ),
                  new XElement("Service",
                     new XAttribute("frstname", e.Message.From.FirstName ?? ""),
                     new XAttribute("lastname", e.Message.From.LastName ?? ""),
                     new XAttribute("id", e.Message.From.Id),
                     new XAttribute("username", e.Message.From.Username ?? "")
                  ),
                  new XElement("Message",
                     new XAttribute("id", e.Message.MessageId),
                     new XAttribute("date", e.Message.Date),
                     new XAttribute("type", e.Message.Type),
                     GetXmlMessage(e)
                  )
               );

            Data.iRoboTechDataContext iRobotTech = new Data.iRoboTechDataContext(connectionString);
            iRobotTech.SAVE_RSMG_P(x);            
         }
         catch { }
#endif
      }

      private XElement GetXmlMessage(MessageEventArgs e)
      {
         XElement xret = null;
         switch (e.Message.Type)
         {
            case MessageType.AudioMessage:
               xret =
                  new XElement("AudioMessage",
                     new XAttribute("mimetype", e.Message.Audio.MimeType ?? ""),
                     new XAttribute("fileid", e.Message.Audio.FileId),
                     new XAttribute("performer", e.Message.Audio.Performer),
                     new XAttribute("titl", e.Message.Audio.Title),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.ContactMessage:
               xret =
                  new XElement("ContactMessage",
                     new XAttribute("frstname", e.Message.Contact.FirstName ?? ""),
                     new XAttribute("lastname", e.Message.Contact.LastName ?? ""),
                     new XAttribute("phonnumb", e.Message.Contact.PhoneNumber)
                  );
               break;
            case MessageType.DocumentMessage:
               xret =
                  new XElement("DocumentMessage",
                     new XAttribute("mimetype", e.Message.Document.MimeType ?? ""),
                     new XAttribute("fileid", e.Message.Document.FileId),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.LocationMessage:
               xret =
                  new XElement("LocationMessage",
                     new XAttribute("latitude", e.Message.Location.Latitude),
                     new XAttribute("longitude", e.Message.Location.Longitude),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.PhotoMessage:
               xret =
                  new XElement("PhotoMessage",
                     new XAttribute("caption", e.Message.Caption ?? ""),
                     new XAttribute("fileid", e.Message.Photo.Reverse().FirstOrDefault().FileId),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.ServiceMessage:
               if(e.Message.NewChatMember != null)
                  xret =
                     new XElement("ServiceMessage",                       
                        new XAttribute("joinleft", "join"),
                        new XAttribute("frstname", e.Message.NewChatMember.FirstName ?? ""),
                        new XAttribute("lastname", e.Message.NewChatMember.LastName ?? ""),
                        new XAttribute("id", e.Message.NewChatMember.Id),
                        new XAttribute("username", e.Message.NewChatMember.Username ?? "")
                     );
               else
                  xret =
                     new XElement("ServiceMessage",
                        new XAttribute("joinleft", "left"),
                        new XAttribute("frstname", e.Message.LeftChatMember.FirstName ?? ""),
                        new XAttribute("lastname", e.Message.LeftChatMember.LastName ?? ""),
                        new XAttribute("id", e.Message.LeftChatMember.Id),
                        new XAttribute("username", e.Message.LeftChatMember.Username ?? "")
                     );
               break;
            case MessageType.StickerMessage:
               xret =
                  new XElement("StickerMessage",
                     new XAttribute("emoji", e.Message.Sticker.Emoji ?? ""),
                     new XAttribute("fileid", e.Message.Sticker.FileId)
                  );
               break;
            case MessageType.TextMessage:
               xret =
                  new XElement("TextMessage",
                     new XAttribute("text", e.Message.Text ?? "")
                  );
               break;
            case MessageType.UnknownMessage:
               break;
            case MessageType.VenueMessage:
               break;
            case MessageType.VideoMessage:
               xret =
                  new XElement("VideoMessage",
                     new XAttribute("mimetype", e.Message.Video.MimeType ?? ""),
                     new XAttribute("fileid", e.Message.Video.FileId),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.VoiceMessage:
               xret =
                  new XElement("VoiceMessage",
                     new XAttribute("mimetype", e.Message.Voice.MimeType ?? ""),
                     new XAttribute("fileid", e.Message.Voice.FileId),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            default:
               break;
         }

         return xret;
      }

      private async void BotOnStarted(ChatInfo chat)
      {
         Data.iRoboTechDataContext iRobotTech = new Data.iRoboTechDataContext(connectionString);
         #region "/Start"
         if (true)
         {
            var pics = (from o in iRobotTech.Organs
                        join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                        join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                        where o.STAT == "002"
                              && r.STAT == "002"
                              && p.STAT == "002"
                              && r.TKON_CODE == Token
                              && p.SHOW_STRT == "002"
                              && p.IMAG_DESC != null
                        orderby p.ORDR descending
                        select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.ORDR }).ToList();

            if (iRobotTech.Robots.Any(r => r.TKON_CODE == Token && r.BULD_STAT != "006"))
            {
               try
               {
                  //string fileid = iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == Token).BULD_FILE_ID;
                  await Bot.SendPhotoAsync(chat.Message.Chat.Id, new FileToSend(iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == Token).BULD_FILE_ID), "کاربر گرامی نرم افزار در حال آماده سازی می باشد و هنوز یه مرحله نهایی نرسیده. بعداز اتمام از همین سامانه به شما اطلاع رسانی می شود");
               }
               catch { }
            }

            foreach (var pic in pics.Take(1))
            {
               dynamic photo;
               if (string.IsNullOrEmpty(pic.FILE_ID))
               {
                  photo = new FileToSend()
                  {
                     Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                     Filename = pic.FILE_NAME
                  };
               }
               else
               {
                  photo = new FileToSend(pic.FILE_ID);
               }
               try
               {
                  await Bot.SendPhotoAsync(chat.Message.Chat.Id, photo, pic.IMAG_DESC);
               }
               catch { }

            }

            var desc = (from o in iRobotTech.Organs
                        join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                        join p in iRobotTech.Organ_Descriptions on o.OGID equals p.ORGN_OGID
                        where o.STAT == "002"
                              && r.STAT == "002"
                              && p.STAT == "002"
                              && r.TKON_CODE == Token
                              && p.SHOW_STRT == "002"
                              && p.ITEM_DESC != null
                        orderby p.ORDR
                        select new { p.ITEM_VALU, p.ITEM_DESC }).ToList();
            string textmsg = "";
            foreach (var d in desc)
            {
               textmsg += string.Format("{1}  {0} \n\r", d.ITEM_VALU, d.ITEM_DESC);
               if (!string.IsNullOrEmpty(textmsg))
                  await Bot.SendTextMessageAsync(
                     chat.Message.Chat.Id,
                     textmsg
                     );
               textmsg = "";
            }
         }
         #endregion
      }

      internal async void GetFile(string ordrtype, string fileid, string destpath)
      {
         //var getguid = Guid.NewGuid();
         switch (ordrtype)
         {
            case "002":
               await Bot.GetFileAsync(fileid, System.IO.File.Create(destpath));
               break;
            case "003":
               break;
            case "004":
               break;
            default:
               break;
         }
      }

      internal async void GetUserProfilePhotos(int userid, string destpath)
      {
         try
         {
            var photos = await Bot.GetUserProfilePhotosAsync(userid, null, 100);
            for (int i = 0; i < photos.TotalCount; i++)
            {
               var photo = photos.Photos[i].Reverse().FirstOrDefault();
               await Bot.GetFileAsync(photo.FileId, System.IO.File.Create(destpath + "_" + i.ToString() +  ".jpg"));
            }
         }
         catch(Exception exc)
         {
            System.Windows.Forms.MessageBox.Show("در ذخیره سازی اطلاعات پروفایل مشتری اشکالی به وجود آماده، لطفا بررسی کنید");
         }
      }

      readonly List<ChatInfo> Chats;
      private MemoEdit ConsoleOutLog_MemTxt;
      public RobotController RobotHandle { get; set; }

      class ChatInfo
      {
         public string UssdCode { get; set; }
         public Message Message { get; set; }
         public object Data { get; set; }
         public DateTime LastVisitDate { get; set; }
         public bool ReadyToFire { get; set; }
         public bool Runed { get; set; }
         public string CommandRunPlace { get; set; }
      }
      private async Task Send_Advertising(string robotToken, ChatInfo chat)
      {
         Data.iRoboTechDataContext iRobotTech = new Data.iRoboTechDataContext(connectionString);
         var sends = (from o in iRobotTech.Organs
                      join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                      join s in iRobotTech.Send_Advertisings on r.RBID equals s.ROBO_RBID
                      where o.STAT == "002"
                         && r.STAT == "002"
                         && s.STAT == "005"
                         && r.TKON_CODE == robotToken
                      orderby s.ORDR
                      select s).ToList();

         foreach (Data.Send_Advertising send in sends)
         {
            dynamic file = null;
            if (send.PAKT_TYPE != "001" && string.IsNullOrEmpty(send.FILE_ID))
            {
               file = new FileToSend()
               {
                  Content = new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                  Filename = send.FILE_PATH
               };
            }
            else if (send.PAKT_TYPE != "001")
            {
               file = new FileToSend(send.FILE_ID);
            }

            switch (send.PAKT_TYPE)
            {
               case "001":
                  iRobotTech.Service_Robots
                     .Where(sr => sr.Robot.TKON_CODE == robotToken && sr.STAT == "002")
                     .ToList()
                     .ForEach(s =>
                     {
                        try
                        {
                           Bot.SendTextMessageAsync((long)s.CHAT_ID, send.TEXT_MESG ?? "😊");
                           var srsa = new Data.Service_Robot_Send_Advertising()
                           {
                              Service_Robot = s,
                              SDAD_ID = send.ID,
                              SEND_STAT = "004"
                           };
                           iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                           iRobotTech.SubmitChanges();
                           
                           if (ConsoleOutLog_MemTxt.InvokeRequired)
                              ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                           else
                              ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
                        }
                        catch
                        {
                           var srsa = new Data.Service_Robot_Send_Advertising()
                           {
                              Service_Robot = s,
                              SDAD_ID = send.ID,
                              SEND_STAT = "003"
                           };
                           iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                           iRobotTech.SubmitChanges();

                           if (ConsoleOutLog_MemTxt.InvokeRequired)
                              ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                           else
                              ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
                        }
                     });
                  break;
               case "002":
                  try
                  {
                     iRobotTech.Service_Robots
                        .Where(sr => sr.Robot.TKON_CODE == robotToken && sr.STAT == "002")
                        .ToList()
                        .ForEach(s =>
                        {
                           try
                           {
                              Bot.SendPhotoAsync((long)s.CHAT_ID, file, send.TEXT_MESG);
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "004"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());

                           }
                           catch
                           {
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "003"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
                           }
                        });
                  }
                  catch { }
                  break;
               case "003":
                  try
                  {
                     iRobotTech.Service_Robots
                        .Where(sr => sr.Robot.TKON_CODE == robotToken && sr.STAT == "002")
                        .ToList()
                        .ForEach(s =>
                        {
                           try
                           {
                              Bot.SendVideoAsync((long)s.CHAT_ID, file, 0, 0, 0, send.TEXT_MESG);
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "004"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());

                           }
                           catch
                           {
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "003"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
                           }
                        });
                  }
                  catch { }
                  break;
               case "004":
                  try
                  {
                     iRobotTech.Service_Robots
                        .Where(sr => sr.Robot.TKON_CODE == robotToken && sr.STAT == "002")
                        .ToList()
                        .ForEach(s =>
                        {
                           try
                           {
                              Bot.SendDocumentAsync((long)s.CHAT_ID, file, send.TEXT_MESG);
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "004"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());

                           }
                           catch
                           {
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "003"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
                           }
                        });
                  }
                  catch { }
                  break;
               case "006":
                  try
                  {
                     iRobotTech.Service_Robots
                        .Where(sr => sr.Robot.TKON_CODE == robotToken && sr.STAT == "002")
                        .ToList()
                        .ForEach(s =>
                        {
                           try
                           {
                              Bot.SendAudioAsync((long)s.CHAT_ID, file,send.TEXT_MESG ?? "No Audio Caption", 0, send.TEXT_MESG, "#");
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "004"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());

                           }
                           catch
                           {
                              var srsa = new Data.Service_Robot_Send_Advertising()
                              {
                                 Service_Robot = s,
                                 SDAD_ID = send.ID,
                                 SEND_STAT = "003"
                              };
                              iRobotTech.Service_Robot_Send_Advertisings.InsertOnSubmit(srsa);
                              iRobotTech.SubmitChanges();

                              if (ConsoleOutLog_MemTxt.InvokeRequired)
                                 ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
                              else
                                 ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
                           }
                        });
                  }
                  catch { }
                  break;
               
               default:
                  break;
            }
            send.STAT = "004";
         }
         iRobotTech.SubmitChanges();
         await Task.Yield();
      }
      private async Task Send_Replay_Message(string robotToken, ChatInfo chat)
      {
         Data.iRoboTechDataContext iRobotTech = new Data.iRoboTechDataContext(connectionString);
         var sends = (from o in iRobotTech.Organs
                      join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                      join sr in iRobotTech.Service_Robots on r.RBID equals sr.ROBO_RBID
                      join rm in iRobotTech.Service_Robot_Replay_Messages on new { S = sr.SERV_FILE_NO, R = sr.ROBO_RBID } equals new { S = (long)rm.SRBT_SERV_FILE_NO, R = (long)rm.SRBT_ROBO_RBID }
                      where o.STAT == "002"
                         && r.STAT == "002"
                         && rm.SEND_STAT == "005"
                         && r.TKON_CODE == robotToken
                      orderby rm.RPLY_DATE
                      select rm).ToList();

         foreach (Data.Service_Robot_Replay_Message send in sends)
         {
            try
            {
               if (send.MESG_TYPE == "001")
               {
                  await Bot.SendTextMessageAsync((long)send.CHAT_ID, send.MESG_TEXT ?? "😊", replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
               }
               else if (send.MESG_TYPE == "002" || send.MESG_TYPE == "003" || send.MESG_TYPE == "004" || send.MESG_TYPE == "006" || send.MESG_TYPE == "007" || send.MESG_TYPE == "009")
               {
                  dynamic photo;
                  if (string.IsNullOrEmpty(send.FILE_ID))
                  {
                     photo = new FileToSend()
                     {
                        Content = new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        Filename = send.FILE_PATH.Substring(send.FILE_PATH.LastIndexOf('\\') + 1)
                     };
                  }
                  else
                  {
                     photo = new FileToSend(send.FILE_ID);
                  }

                  if (send.MESG_TYPE == "002")
                     await Bot.SendPhotoAsync((long)send.CHAT_ID, photo, send.MESG_TEXT ?? "😊", false, (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if(send.MESG_TYPE == "003")
                     await Bot.SendVideoAsync((long)send.CHAT_ID, photo, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "004")
                     await Bot.SendDocumentAsync((long)send.CHAT_ID, photo, send.MESG_TEXT ?? "😊", replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "006")
                     await Bot.SendAudioAsync((long)send.CHAT_ID, photo, "*", 0, "*", "#", replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "007")
                     await Bot.SendStickerAsync((long)send.CHAT_ID, photo, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if(send.MESG_TYPE == "009")
                     await Bot.SendVoiceAsync((long)send.CHAT_ID, photo, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
               }
               else if (send.MESG_TYPE == "005")
               {
                  await Bot.SendLocationAsync((long)send.CHAT_ID, (float)send.LAT, (float)send.LON, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
               }
               else if(send.MESG_TYPE == "008")
               {
                  await Bot.SendContactAsync((long)send.CHAT_ID, send.CONT_CELL_PHON, send.MESG_TEXT, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
               }               

               send.SEND_STAT = "004";
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", send.CHAT_ID, send.Service_Robot.Service.FRST_NAME + ", " + send.Service_Robot.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
               else
                  ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", send.CHAT_ID, send.Service_Robot.Service.FRST_NAME + ", " + send.Service_Robot.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
            }
            catch { 
               send.SEND_STAT = "001";
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", send.CHAT_ID, send.Service_Robot.Service.FRST_NAME + ", " + send.Service_Robot.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
               else
                  ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", send.CHAT_ID, send.Service_Robot.Service.FRST_NAME + ", " + send.Service_Robot.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
            }
         }
         iRobotTech.SubmitChanges();
      }
      private async Task Download_Media(string robotToken, Message m)
      {
         // برای ذخیره سازی فایل های وارد شده به سیستم برای نگهداری درون سیستم
         // مرحله اول ذخیره کردن اطلاعات از جدول Order_Detail
         Data.iRoboTechDataContext iRobotTech = new Data.iRoboTechDataContext(connectionString);
         #region Order_Detail
         foreach (var ordt in iRobotTech.Order_Details.Where(od => (od.IMAG_PATH == null || od.IMAG_PATH.Trim().Length == 0) && (od.ELMN_TYPE == "002" || od.ELMN_TYPE == "003" || od.ELMN_TYPE == "004" || od.ELMN_TYPE == "006" || od.ELMN_TYPE == "009")))
         {
            #region Order_Detail Download
            {
               // 1396/07/22 * بدست آوردن مرجعی برای ذخیره سازی اطلاعات فایل های دریافتی برای ربات
               var filestorage = "";
               if (robot.DOWN_LOAD_FILE_PATH != "" && robot.DOWN_LOAD_FILE_PATH.Length >= 10)
                  filestorage = robot.DOWN_LOAD_FILE_PATH;

               //if (parentmenu != null)
               {
                  var datenow = iRobotTech.GET_MTOS_U(DateTime.Now).Replace("/", "_");
                  //if ((parentmenu.UPLD_FILE_PATH ?? @"D:\") != "")
                  {
                     var fileupload = (filestorage ?? @"D:") + "\\" + Me.Username + "\\" + datenow + "\\" + m.Chat.Id.ToString() + "\\" + ordt.RWNO.ToString();
                     var filename = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString();
                     if (!Directory.Exists(fileupload))
                     {
                        DirectoryInfo di = Directory.CreateDirectory(fileupload);
                     }

                     if (ordt.ELMN_TYPE == "002")
                     {
                        try
                        {
                           await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + filename + ".jpg"));
                           ordt.IMAG_PATH = fileupload + "\\" + filename + ".jpg";
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "003")
                     {
                        try
                        {
                           await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Video.FileId*/ filename + "." + ordt.FILE_EXT));
                           ordt.IMAG_PATH = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "004")
                     {
                        try
                        {
                           await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Document.FileId*/ filename+"." + ordt.FILE_EXT));
                           ordt.IMAG_PATH = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "006")
                     {
                        try
                        {
                           await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Audio.FileId*/ filename + "." + ordt.FILE_EXT));
                           ordt.IMAG_PATH = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "009")
                     {
                        try
                        {
                           await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Sticker.FileId*/ filename + "." + ordt.FILE_EXT));
                           ordt.IMAG_PATH = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                        }
                        catch { }
                     }                     
                  }
               }
            }
            #endregion
         }
         #endregion

         #region Robot_Spy_Group_Message
         foreach (var rsgm in iRobotTech.Robot_Spy_Group_Messages.Where(r => r.FILE_ID != null && r .FILE_PATH == null))
         {
            #region Robot_Spy_Group_Message Download
            {
               // 1396/07/22 * بدست آوردن مرجعی برای ذخیره سازی اطلاعات فایل های دریافتی برای ربات
               var filestorage = "";
               if (robot.DOWN_LOAD_FILE_PATH != "" && robot.DOWN_LOAD_FILE_PATH.Length >= 10)
                  filestorage = robot.DOWN_LOAD_FILE_PATH;

               //if (parentmenu != null)
               {
                  var datenow = iRobotTech.GET_MTOS_U(DateTime.Now).Replace("/", "_");
                  //if ((parentmenu.UPLD_FILE_PATH ?? @"D:\") != "")
                  {
                     var fileupload = (filestorage ?? @"D:") + "\\" + Me.Username + "\\" + datenow + "\\" + m.Chat.Id.ToString() + "\\" + rsgm.CODE.ToString();
                     var filename = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString();
                     if (!Directory.Exists(fileupload))
                     {
                        DirectoryInfo di = Directory.CreateDirectory(fileupload);
                     }

                     if (rsgm.MESG_TYPE == "002")
                     {
                        try
                        {
                           await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + filename + ".jpg"));
                           rsgm.FILE_PATH = fileupload + "\\" + filename + ".jpg";
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "003")
                     {
                        try
                        {
                           await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Video.FileId*/ filename));
                           rsgm.FILE_PATH = fileupload + "\\" + filename;
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "004")
                     {
                        try
                        {
                           await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Document.FileId*/ filename));
                           rsgm.FILE_PATH = fileupload + "\\" + filename;
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "006")
                     {
                        try
                        {
                           await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Audio.FileId*/ filename));
                           rsgm.FILE_PATH = fileupload + "\\" + filename;
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "009")
                     {
                        try
                        {
                           await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Sticker.FileId*/ filename));
                           rsgm.FILE_PATH = fileupload + "\\" + filename;
                        }
                        catch { }
                     }
                  }
               }
            }
            #endregion
         }
         #endregion

         iRobotTech.SubmitChanges();
      }
      private async Task Send_Order(Data.iRoboTechDataContext iRobotTech, KeyboardButton[][] keyBoardMarkup)
      {
         // بررسی اینکه آیا درخواستی برای پرسنلی ثبت شده یا خیر
         var prjos = iRobotTech.Personal_Robot_Job_Orders.Where(po => po.ORDR_STAT == "001");

         foreach (var prjo in prjos)
         {
            prjo.ORDR_STAT = "002";
            iRobotTech.Set_Personal_Robot_Job_Order(
               new XElement("PJBO",
                  new XAttribute("prjbcode", prjo.PRJB_CODE),
                  new XAttribute("ordrcode", prjo.ORDR_CODE),
                  new XAttribute("ordrstat", "002")
               )
            );

            var ordts = iRobotTech.Order_Details.Where(o => o.ORDR_CODE == prjo.ORDR_CODE);

            foreach (var ordt in ordts)
            {
               switch (ordt.Order.ORDR_TYPE)
               {
                  case "001": // پیشنهادات                              
                  case "002": // نظرسنجی
                  case "003": // شکایات
                  case "004": // سفارشات
                  case "005": // Like
                  case "006": // پرسش
                  case "007": // پاسخ
                  case "008": // تجربیات
                  case "009": // Upload
                  case "010": // معرفی
                  case "011": // اخطار
                  case "012": // اعلام ها
                     #region پیام های ارسالی
                     switch (ordt.ELMN_TYPE)
                     {
                        case "001":
                           await Bot.SendTextMessageAsync(
                              (int)prjo.PRBT_CHAT_ID,
                              iRobotTech.CRET_PMSG_U(new XElement("Message",
                                 new XAttribute("prjbcode", prjo.PRJB_CODE),
                                 new XAttribute("ordrcode", prjo.ORDR_CODE),
                                 new XAttribute("ordtrwno", ordt.RWNO)
                              )),
                              replyMarkup:
                              new ReplyKeyboardMarkup()
                              {
                                 Keyboard = keyBoardMarkup,
                                 ResizeKeyboard = true,
                                 Selective = true
                              });
                           break;
                        case "002":
                           await Bot.SendPhotoAsync(
                              (int)prjo.PRBT_CHAT_ID,
                              new FileToSend(ordt.ORDR_DESC),
                              caption: ordt.ORDR_CMNT ?? "",
                              replyMarkup:
                              new ReplyKeyboardMarkup()
                              {
                                 Keyboard = keyBoardMarkup,
                                 ResizeKeyboard = true,
                                 Selective = true
                              });
                           break;
                        case "003":
                           await Bot.SendVideoAsync(
                              (int)prjo.PRBT_CHAT_ID,
                              new FileToSend(ordt.ORDR_DESC),
                              replyMarkup:
                              new ReplyKeyboardMarkup()
                              {
                                 Keyboard = keyBoardMarkup,
                                 ResizeKeyboard = true,
                                 Selective = true
                              });
                           break;
                        case "004":
                           await Bot.SendDocumentAsync(
                              (int)prjo.PRBT_CHAT_ID,
                              new FileToSend(ordt.ORDR_DESC),
                              replyMarkup:
                              new ReplyKeyboardMarkup()
                              {
                                 Keyboard = keyBoardMarkup,
                                 ResizeKeyboard = true,
                                 Selective = true
                              });
                           break;
                        case "005":
                           float cordx = Convert.ToSingle(ordt.ORDR_DESC.Split(',')[0], System.Globalization.CultureInfo.InvariantCulture);
                           float cordy = Convert.ToSingle(ordt.ORDR_DESC.Split(',')[1], System.Globalization.CultureInfo.InvariantCulture);

                           await Bot.SendLocationAsync(
                              (long)prjo.PRBT_CHAT_ID,
                              Convert.ToSingle(ordt.ORDR_DESC.Split(',')[0], System.Globalization.CultureInfo.InvariantCulture),
                              Convert.ToSingle(ordt.ORDR_DESC.Split(',')[1], System.Globalization.CultureInfo.InvariantCulture),
                              replyMarkup:
                              new ReplyKeyboardMarkup()
                              {
                                 Keyboard = keyBoardMarkup,
                                 ResizeKeyboard = true,
                                 Selective = true
                              });

                           break;
                        default:
                           break;
                     }
                     #endregion
                     break;
               }
            }
         }
         iRobotTech.SubmitChanges();
      }
      public KeyboardButton[][] CreateArray(List<XElement> list, int rows, int cols)
      {
         int index = 0;
         if (rows == 0)
         {
            rows = list.Count / cols;
            rows = list.Count % cols == 0 ? rows : rows + 1;
         }

         KeyboardButton[][] array = new KeyboardButton[rows][];
         for (int i = 0; i < rows; i++)
         {
            array[i] = new KeyboardButton[cols];
            for (int j = 0; j < cols; j++)
            {
               if (list.Count > index)
               {
                  array[i][j] = list.ToArray()[index].Value;
                  if (list.ToArray()[index].Attribute("type").Value == "016")
                  {
                     array[i][j].RequestLocation = true;
                  }
                  else if (list.ToArray()[index].Attribute("type").Value == "015")
                  {
                     array[i][j].RequestContact = true;
                  }
                  ++index;
               }
               else if (list.Count % 2 != 0 || (list.Count % 2 == 0 && array[i][j] == null))
               {
                  array[i][j] = "";
               }
            }
         }

         return array;
      }
      private async Task MessagePaging(ChatInfo chat, string message, KeyboardButton[][] keyBoardMarkup)
      {
         for (int i = 0; i < message.Length; i += 4096)
         {
            var m = message.Substring(i, Math.Min(4096, message.Length - i));
            await Bot.SendTextMessageAsync(
            chat.Message.Chat.Id,
            m,
            ParseMode.Default,
            false,
            false,
            chat.Message.MessageId,
            new ReplyKeyboardMarkup()
            {
               Keyboard = keyBoardMarkup,
               ResizeKeyboard = true,
               Selective = true
            });
         }
         await Task.Yield();
      }
      private async Task FireEventResultOpration(ChatInfo chat, KeyboardButton[][] keyBoardMarkup, XElement xdata)
      {
         if (xdata.Elements("Message").Count() > 0)
         {
            var query = XDocument.Parse(string.Format("<Message>{0}</Message>", xdata.Element("Message").Value));
            foreach (string order in query.Element("Message").Elements().Attributes("order").OrderBy(o => o.Value))
            {
               var xelement = query.Element("Message").Elements().Where(x => x.Attribute("order").Value == order).First();
               if (xelement.Name == "Texts")
               {
                  #region Info
                  foreach (string innerorder in xelement.Elements().Attributes("order").OrderBy(o => o.Value))
                  {
                     var xinnerelement = xelement.Elements().Where(x => x.Attribute("order").Value == innerorder).First();

                     await Bot.SendTextMessageAsync(
                       chat.Message.Chat.Id,
                       xinnerelement.Value ?? "...",
                       ParseMode.Default,
                       false,
                       false,
                       chat.Message.MessageId,
                       new ReplyKeyboardMarkup()
                       {
                          Keyboard = keyBoardMarkup,
                          ResizeKeyboard = true,
                          Selective = true
                       });
                  }
                  #endregion
               }
               else if (xelement.Name == "Locations")
               {
                  #region Location
                  foreach (string innerorder in xelement.Elements().Attributes("order").OrderBy(o => o.Value))
                  {
                     var xinnerelement = xelement.Elements().Where(x => x.Attribute("order").Value == innerorder).First();

                     float cordx = Convert.ToSingle(xinnerelement.Attribute("cordx").Value, System.Globalization.CultureInfo.InvariantCulture);
                     float cordy = Convert.ToSingle(xinnerelement.Attribute("cordy").Value, System.Globalization.CultureInfo.InvariantCulture);

                     await Bot.SendLocationAsync(
                      chat.Message.Chat.Id,
                      cordx,
                      cordy,
                      0,
                      false,
                      chat.Message.MessageId,
                      new ReplyKeyboardMarkup()
                      {
                         Keyboard = keyBoardMarkup,
                         ResizeKeyboard = true,
                         Selective = true
                      });

                     await Bot.SendTextMessageAsync(
                       chat.Message.Chat.Id,
                       xinnerelement.Attribute("cellphon").Value,
                       ParseMode.Default,
                       false,
                       false,
                       chat.Message.MessageId,
                       new ReplyKeyboardMarkup()
                       {
                          Keyboard = keyBoardMarkup,
                          ResizeKeyboard = true,
                          Selective = true
                       });
                  }
                  #endregion
               }
               else if (xelement.Name == "Images")
               {
                  #region Image
                  foreach (string innerorder in xelement.Elements().Attributes("order").OrderBy(o => o.Value))
                  {
                  }
                  #endregion
               }
            }
            // اگر پیام حاوی خروجی عدم ثبت کد اشتراک باشه باشد
            if (query.Element("Message").Elements().Attributes("order").Count() == 0)
            {
               var xelement = query.Element("Message").Value;
               #region Info
               // Info
               await MessagePaging(chat, xelement, keyBoardMarkup);
               //await Bot.SendTextMessageAsync(
               //     chat.Message.Chat.Id,
               //     xelement ?? "...",
               //     ParseMode.Default,
               //     false,
               //     false,
               //     chat.Message.MessageId,
               //     new ReplyKeyboardMarkup()
               //     {
               //        Keyboard = keyBoardMarkup,
               //        ResizeKeyboard = true,
               //        Selective = true
               //     });
               #endregion
            }
         }
      }
      /*private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
      {
         Debugger.Break();
      }

      private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
      {
         //Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
      }

      private async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
      {
         InlineQueryResult[] results = {
               new InlineQueryResultLocation
               {
                  Id = "1",
                  Latitude = 40.7058316f, // displayed result
                  Longitude = -74.2581888f,
                  Title = "New York",
                  InputMessageContent = new InputLocationMessageContent // message if result is selected
                  {
                     Latitude = 40.7058316f,
                     Longitude = -74.2581888f,
                  }
               },

               new InlineQueryResultLocation
               {
                  Id = "2",
                  Longitude = 52.507629f, // displayed result
                  Latitude = 13.1449577f,
                  Title = "Berlin",
                  InputMessageContent = new InputLocationMessageContent // message if result is selected
                  {
                     Longitude = 52.507629f,
                     Latitude = 13.1449577f
                  }
               }
         };

         await Bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
      }

      private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
      {
         var message = messageEventArgs.Message;

         if (message == null || message.Type != MessageType.TextMessage) return;

         if (message.Text.StartsWith("/inline")) // send inline keyboard
         {
               await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

               var keyboard = new InlineKeyboardMarkup(new[]
               {
                  new[] // first row
                  {
                     new InlineKeyboardButton("1.1"),
                     new InlineKeyboardButton("1.2"),
                  },
                  new[] // second row
                  {
                     new InlineKeyboardButton("2.1"),
                     new InlineKeyboardButton("2.2"),
                  }
               });

               await Task.Delay(500); // simulate longer running task

               await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
                  replyMarkup: keyboard);
         }
         else if (message.Text.StartsWith("/keyboard")) // send custom keyboard
         {
               var keyboard = new ReplyKeyboardMarkup(new[]
               {
                  new [] // first row
                  {
                     new KeyboardButton("1.1"),
                     new KeyboardButton("1.2"),  
                  },
                  new [] // last row
                  {
                     new KeyboardButton("2.1"),
                     new KeyboardButton("2.2"),  
                  }
               });

               await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
                  replyMarkup: keyboard);
         }
         else if (message.Text.StartsWith("/photo")) // send a photo
         {
               await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

               const string file = @"C:\\Users\\mohsen\\Pictures\\800t.png";

               var fileName = file.Split('\\').Last();

               using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
               {
                  var fts = new FileToSend(fileName, fileStream);

                  await Bot.SendPhotoAsync(message.Chat.Id, fts, "Nice Picture");
               }
         }
         else if (message.Text.StartsWith("/request")) // request location or contact
         {
               var keyboard = new ReplyKeyboardMarkup(new []
               {
                  new KeyboardButton("Location")
                  {
                     RequestLocation = true
                  },
                  new KeyboardButton("Contact")
                  {
                     RequestContact = true
                  }, 
               });

               await Bot.SendTextMessageAsync(message.Chat.Id, "Who or Where are you?", replyMarkup: keyboard);
         }
         else
         {
               var usage = @"Usage:
/inline   - send inline keyboard
/keyboard - send custom keyboard
/photo    - send a photo
/request  - request location or contact
";

               await Bot.SendTextMessageAsync(message.Chat.Id, usage,
                  replyMarkup: new ReplyKeyboardHide());
         }
      }

      private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
      {
         await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
               "Received {callbackQueryEventArgs.CallbackQuery.Data}");
      }
      */
   }
}
