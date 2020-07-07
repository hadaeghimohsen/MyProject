using Bale.Bot;
using Bale.Bot.Args;
using Bale.Bot.Types;
using Bale.Bot.Types.Enums;
using Bale.Bot.Types.InlineQueryResults;
using Bale.Bot.Types.InputFiles;
using Bale.Bot.Types.Payments;
using Bale.Bot.Types.ReplyMarkups;
using DevExpress.XtraEditors;
using System.Collections.Generic;
using System.IO;
using System.JobRouting.Jobs;
using System.Linq;
using System.RoboTech.ExtCode;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.RoboTech.Controller
{
   public class BaleApiBot : IApiBot
   {
      //private static readonly TelegramBotClient Bot = new TelegramBotClient("273587585:AAFdrJate4ED0ccT4kGubo14xse4ZU81g7E");
      private BaleBotClient Bot;
      private string Token;
      internal User Me;
      private string connectionString;
      private Data.Robot robot;
      private bool started = false;
      private Ui.Action.STRT_ROBO_F _Strt_Robo_F;

      public Data.Robot Robot
      {
         get
         {
            return robot;
         }
      }

      public bool Started
      {
         get
         {
            return started;
         }
      }

      public string BotName
      {
         get
         {
            return Me.Username;
         }
      }

      public BaleApiBot(string token, string connectionString, MemoEdit ConsoleOutLog_MemTxt, bool activeRobot = true, Data.Robot robot = null, Ui.Action.STRT_ROBO_F strt_robo_f = null)
      {
         try
         {
            this.ConsoleOutLog_MemTxt = ConsoleOutLog_MemTxt;

            Bot = new BaleBotClient(token);
            Token = token;
            this.robot = robot;
            Chats = new List<ChatInfo>();
            RobotHandle = new RobotController();
            this.connectionString = connectionString;
            _Strt_Robo_F = strt_robo_f;

            started = true;
            main(activeRobot);
         }
         catch (Exception exc) { this.ConsoleOutLog_MemTxt.Text = exc.Message; }
      }

      void main(bool activeRobot = true)
      {
         if (activeRobot)
         {
            Bot.OnUpdate += BotOnUpdateReceived;
            //Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
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

      public void StopReceiving()
      {
         Bot.StopReceiving();
         started = false;
      }

      public async void SendAction(XElement x)
      {
         try
         {
            Data.iRoboTechDataContext iRobotTech = new Data.iRoboTechDataContext(connectionString);
            var chatid = x.Attribute("chatid").Value.ToInt64();
            var chatinfo = Chats.FirstOrDefault(c => c.Message.Chat.Id == chatid);
            switch (x.Attribute("actntype").Value)
            {
               case "sendordrs":
                  await Send_Order(iRobotTech, null);
                  break;
               case "savepymt":
                  await FireEventResultOpration(chatinfo, null, x.Element("Result"));
                  await Send_Order(iRobotTech, null);
                  await Send_Replay_Message(GetToken(), chatinfo);
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            if (ConsoleOutLog_MemTxt.InvokeRequired)
               ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = exc.Message + ConsoleOutLog_MemTxt.Text));
            else
               ConsoleOutLog_MemTxt.Text = exc.Message + ConsoleOutLog_MemTxt.Text;
         }
      }

      private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
      {
         try
         {
            var callBackQuery = e.CallbackQuery;
            string data = callBackQuery.Data;
            #region HoldHistory
            ChatInfo chat = null;
            if (Chats.All(c => c.Message.Chat.Id != e.CallbackQuery.Message.Chat.Id))
               Chats.Add(new ChatInfo() { Message = e.CallbackQuery.Message, LastVisitDate = DateTime.Now, Runed = false });
            chat = Chats.FirstOrDefault(c => c.Message.Chat.Id == e.CallbackQuery.Message.Chat.Id);
            chat.Message = e.CallbackQuery.Message;
            chat.LastVisitDate = DateTime.Now;
            chat.ForceReply = false;
            var removeRam = Chats.Where(c => c.LastVisitDate.AddMinutes(5) < DateTime.Now);
            removeRam.ToList().Clear();
            #endregion
            try
            {
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("{1} , {2} , {0}, ({3})\r\n", e.CallbackQuery.Message.Chat.Id, Me.Username, DateTime.Now.ToString("HH:mm:ss"), e.CallbackQuery.Data) + ConsoleOutLog_MemTxt.Text));
               else
                  ConsoleOutLog_MemTxt.Text += string.Format("{1} , {2} , {0}, ({3})\r\n", e.CallbackQuery.Message.Chat.Id, Me.Username, DateTime.Now.ToString("HH:mm:ss"), e.CallbackQuery.Data) + ConsoleOutLog_MemTxt.Text;
            }
            catch { }

            /*await Bot.AnswerCallbackQueryAsync(
                  callbackQueryId: callbackQuery.Id,
                  text: string.Format(@"Received {0}", e.CallbackQuery.Data)
            );*/

            // Data : [ "!" ] [ "@" | "." ] "/" UserInterfacePaths | UssdCode ";" Command "-" Params
            // ForceReply ::= "!"
            // UserInterfacePaths ::= Ui {":" Ui}
            // UssdCode ::= "*" [0-9]+ "#"
            // Params ::= Param {"," Param}
            // e.g. : @/DefaultGateway:Scsc:Mstr_Page_F;Attn-1398655458,1
            

            bool forceReply = false;
            if (data.StartsWith("!"))
            {
               forceReply = true;
               data = data.Substring(1);
            }

            string target = data.Substring(0, 1);
            if(target == "@")
            {
               #region SubSystem Service
               data = data.Substring(target.Length + 1); // data.Split('/')[1];
               string uis = data.Split(';')[0];
               data = data.Substring(uis.Length + 1); // data.Split(';')[1];
               string cmnd = data.Split('-')[0];
               data = data.Substring(cmnd.Length + 1); // data.Split('-')[1];
               string param = data.Split('$')[0];
               data = data.Substring(param.Length + 1); // data.Split('$')[1];
               string postexecs = data.Split('#')[0];
               data = data.Substring(postexecs.Length + 1); // data.Split('#')[1];
               string triggers = data.Split('\0')[0];
               string aftrbfor = "";
               if (triggers != "" && (triggers.Substring(0, 2) == ">>" || triggers.Substring(0, 2) == "<<"))
               {
                  aftrbfor = triggers.Substring(0, 2);
                  triggers = triggers.Substring(2);
               }
               else
                  aftrbfor = ">>";


               // ⏳ Please wait...
               await Bot.SendTextMessageAsync(
                  e.CallbackQuery.Message.Chat.Id,
                  "⏳ لطفا چند لحظه صبر کنید...",
                  replyMarkup:
                  null);

               _Strt_Robo_F.SendRequest(
                  new Job(SendType.SelfToUserInterface, 1000 /* Execute Call_SystemService_F */ )
                  {
                     Input =
                        new XElement("Input",
                           new XAttribute("chatid", e.CallbackQuery.Message.Chat.Id),
                           new XAttribute("ussdcode", ""),
                           new XAttribute("subsystarget", uis),
                           new XAttribute("cmnd", cmnd),
                           new XAttribute("param", param)
                        ),
                     AfterChangedOutput =
                        new Action<object>(
                           (output) =>
                           {
                              var xoutput = output as XElement;

                              var resultcode = xoutput.Attribute("resultcode").Value.ToInt64();
                              var resultdesc = xoutput.Attribute("resultdesc").Value;
                              var mesgtype = (MessageType)xoutput.Attribute("mesgtype").Value.ToInt32();

                              switch (mesgtype)
                              {
                                 case MessageType.Text:
                                    Bot.SendTextMessageAsync(
                                       e.CallbackQuery.Message.Chat.Id,
                                       string.Format("👈 {0} ",
                                          resultdesc
                                       ),
                                       replyMarkup:
                                       null).Wait();
                                    break;

                              }
                           }
                        )
                  }
               );
               #endregion
            }
            else if (target == ".")
            {
               #region Database Service
               data = data.Substring(target.Length + 1); // data.Split('/')[1];
               string ussdcode = data.Split(';')[0];
               data = data.Substring(ussdcode.Length + 1); // data.Split(';')[1];
               string cmnd = data.Split('-')[0];
               data = data.Substring(cmnd.Length + 1); // data.Split('-')[1];
               string param = data.Split('$')[0];
               data = data.Substring(param.Length + 1); // data.Split('$')[1];
               string postexecs = data.Split('#')[0];
               data = data.Substring(postexecs.Length + 1); // data.Split('#')[1];
               string triggers = data.Split('\0')[0];
               string triggersteprun = ""; // >> after; << before; <> None;

               // تنظیم کردن گزینه هایی که برای وارد کردن اطلاعات از قیمت ورودی کاربر می تواند دریافت کند
               if(forceReply)
               {
                  chat.UisUssd = ussdcode;
                  chat.CommandText = cmnd;
                  chat.Params = param;
                  chat.PostExecs = postexecs;
                  chat.Triggers = triggers;
                  chat.TriggerStepRun = triggersteprun;
                  chat.Target = target;
                  chat.ForceReply = true;
               }

               if (triggers != "" && (triggers.Substring(0, 2) == ">>" || triggers.Substring(0, 2) == "<<" || triggers.Substring(0, 2) == "<>"))
               {
                  triggersteprun = triggers.Substring(0, 2);
                  triggers = triggers.Substring(2);
               }
               else
                  triggersteprun = ">>";

               //var chat = new ChatInfo() { Message = e.CallbackQuery.Message, LastVisitDate = DateTime.Now, Runed = false };
               var iRobotTech = new Data.iRoboTechDataContext(connectionString);
               var xResult = new XElement("Result", "No Message");

               // POST Execution
               #region Post Execution
               foreach (var postexec in postexecs.Split(','))
               {
                  switch (postexec)
                  {
                     case "del":
                        // اگر لازم به این باشد که پیامی که منوی آن انتخاب شده حذف شود
                        // چون ممکن است منوهای آن پیام تغییراتی داشته باشند
                        TryExtension.Try(async () => await Bot.DeleteMessageAsync(chat.Message.Chat.Id, chat.Message.MessageId)).Catch(x => { if (ConsoleOutLog_MemTxt.InvokeRequired) ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = x.Message + ConsoleOutLog_MemTxt.Text)); else ConsoleOutLog_MemTxt.Text = x.Message + ConsoleOutLog_MemTxt.Text; });
                        postexecs = postexecs.Replace("del", "");
                        if(postexecs != "")
                           postexecs = postexecs.Substring(1);
                        break;
                     case "pay":
                        // Create Payment Invoice
                        #region Order Checkout Payment Process
                        var ordrinvc =
                           iRobotTech.Orders
                           .Where(
                              o => /*o.CHAT_ID == e.CallbackQuery.Message.Chat.Id
                                && o.Robot.TKON_CODE == GetToken()
                                && o.ORDR_STAT == "001"
                                && (o.ORDR_TYPE == "004" || o.ORDR_TYPE == "013" || o.ORDR_TYPE == "014" || o.ORDR_TYPE == "016")
                                && o.STRT_DATE.Value.Date == DateTime.Now.Date
                                && */o.CODE == param.ToInt64() // ordrcode
                           ).FirstOrDefault();

                        // اگر فاکتور تسویه حساب نباشد به مرحله کارت به کارت میرویم
                        if (ordrinvc.DEBT_DNRM != 0)
                        {
                           // Process SendInvoice
                           var price = new List<LabeledPrice>();
                           if (ordrinvc.AMNT_TYPE == "001")
                           {
                              if (ordrinvc.EXTR_PRCT != null && ordrinvc.EXTR_PRCT > 0)
                              {
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrinvc.DEBT_DNRM - ordrinvc.EXTR_PRCT /*- ordrinvc.SUM_FEE_AMNT_DNRM*/)));
                                 price.Add(new LabeledPrice("ارزش افزوده", (int)ordrinvc.EXTR_PRCT));
                              }
                              else
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrinvc.DEBT_DNRM /*- ordrinvc.SUM_FEE_AMNT_DNRM*/)));

                              // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                              if (ordrinvc.TXFE_AMNT_DNRM != null && ordrinvc.TXFE_AMNT_DNRM > 0)
                                 price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordrinvc.TXFE_AMNT_DNRM));
                           }
                           else if (ordrinvc.AMNT_TYPE == "002")
                           {
                              if (ordrinvc.EXTR_PRCT != null && ordrinvc.EXTR_PRCT > 0)
                              {
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrinvc.DEBT_DNRM - ordrinvc.EXTR_PRCT /*- ordrinvc.SUM_FEE_AMNT_DNRM*/) * 10));
                                 price.Add(new LabeledPrice("ارزش افزوده", (int)ordrinvc.EXTR_PRCT * 10));
                              }
                              else
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrinvc.DEBT_DNRM /*- ordrinvc.SUM_FEE_AMNT_DNRM*/) * 10));

                              // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                              if (ordrinvc.TXFE_AMNT_DNRM != null && ordrinvc.TXFE_AMNT_DNRM > 0)
                                 price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordrinvc.TXFE_AMNT_DNRM * 10));
                           }

                           if (ordrinvc.AMNT_TYPE == "001")
                           {
                              // پرداخت به صورت کارت به کارت
                              // مبلغ کارت به کارت 10 میلیون تومان
                              if (price.Sum(p => p.Amount) <= 100000000)
                              {
                                 await Bot.SendInvoiceAsync(
                                    (int)e.CallbackQuery.Message.Chat.Id,
                                    string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordrinvc.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordrinvc.STRT_DATE)),
                                    "سبد خرید شما",
                                    ordrinvc.CODE.ToString(),
                                    ordrinvc.DEST_CARD_NUMB_DNRM,
                                    "",
                                    "IRR",
                                    price/*,
                                    photoUrl: "https://devbale.ir/sites/default/files/styles/large/public/1397-12/404733-PCXHHU-813.jpg?itok=3WLQI4eW"*/
                                    );
                              }
                              else
                              {
                                 // پرداخت از طریق درگاه پرداخت
                              }
                           }
                           else if (ordrinvc.AMNT_TYPE == "002")
                           {
                              // پرداخت به صورت کارت به کارت
                              // مبلغ کارت به کارت 10 میلیون تومان
                              if (price.Sum(p => p.Amount) <= 10000000)
                              {
                                 await Bot.SendInvoiceAsync(
                                    (int)e.CallbackQuery.Message.Chat.Id,
                                    string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordrinvc.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordrinvc.STRT_DATE)),
                                    "سبد خرید شما",
                                    ordrinvc.CODE.ToString(),
                                    ordrinvc.DEST_CARD_NUMB_DNRM,
                                    "",
                                    "IRR",
                                    price
                                    );
                              }
                              else
                              {
                                 // پرداخت از طریق درگاه پرداخت
                              }
                           }
                        }
                        #endregion
                        postexecs = postexecs.Replace("pay", "");
                        if(postexecs != "")
                           postexecs = postexecs.Substring(1);
                        break;
                     case "withdraw":
                        // Create Payment Withdraw
                        #region Order Checkout Payment Process
                        var ordrwithdraw =
                           iRobotTech.Orders
                           .Where(
                              o => /*o.CHAT_ID == e.CallbackQuery.Message.Chat.Id
                                && o.Robot.TKON_CODE == GetToken()
                                && o.ORDR_STAT == "001"
                                && (o.ORDR_TYPE == "004" || o.ORDR_TYPE == "013" || o.ORDR_TYPE == "014" || o.ORDR_TYPE == "016")
                                && o.STRT_DATE.Value.Date == DateTime.Now.Date
                                && */o.CODE == param.ToInt64() // ordrcode
                           ).FirstOrDefault();

                        // اگر فاکتور تسویه حساب نباشد به مرحله کارت به کارت میرویم
                        if (ordrwithdraw.Order1.DEBT_DNRM != 0)
                        {
                           // Process SendInvoice
                           var price = new List<LabeledPrice>();
                           if (ordrwithdraw.Order1.AMNT_TYPE == "001")
                           {
                              if (ordrwithdraw.Order1.EXTR_PRCT != null && ordrwithdraw.Order1.EXTR_PRCT > 0)
                              {
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrwithdraw.Order1.DEBT_DNRM - ordrwithdraw.Order1.EXTR_PRCT /*- ordrwithdraw.Order1.SUM_FEE_AMNT_DNRM*/)));
                                 price.Add(new LabeledPrice("ارزش افزوده", (int)ordrwithdraw.Order1.EXTR_PRCT));
                              }
                              else
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrwithdraw.Order1.DEBT_DNRM /*- ordrwithdraw.Order1.SUM_FEE_AMNT_DNRM*/)));

                              // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                              //if (ordrwithdraw.Order1.TXFE_AMNT_DNRM != null && ordrwithdraw.Order1.TXFE_AMNT_DNRM > 0)
                              //   price.Add(new LabeledPrice("کسر کارمزد خدمات واریز به حساب مشتری", (int)ordrwithdraw.Order1.TXFE_AMNT_DNRM));
                           }
                           else if (ordrwithdraw.Order1.AMNT_TYPE == "002")
                           {
                              if (ordrwithdraw.Order1.EXTR_PRCT != null && ordrwithdraw.Order1.EXTR_PRCT > 0)
                              {
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrwithdraw.Order1.DEBT_DNRM - ordrwithdraw.Order1.EXTR_PRCT /*- ordrwithdraw.SUM_FEE_AMNT_DNRM*/) * 10));
                                 price.Add(new LabeledPrice("ارزش افزوده", (int)ordrwithdraw.Order1.EXTR_PRCT * 10));
                              }
                              else
                                 //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                                 price.Add(new LabeledPrice("قیمت کل", (int)(ordrwithdraw.DEBT_DNRM - ordrwithdraw.SUM_FEE_AMNT_DNRM) * 10));

                              // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                              //if (ordrwithdraw.Order1.TXFE_AMNT_DNRM != null && ordrwithdraw.Order1.TXFE_AMNT_DNRM > 0)
                              //   price.Add(new LabeledPrice("کسر کارمزد خدمات واریز به حساب مشتری", (int)ordrwithdraw.Order1.TXFE_AMNT_DNRM * 10));
                           }

                           if (ordrwithdraw.Order1.AMNT_TYPE == "001")
                           {
                              // پرداخت به صورت کارت به کارت
                              // مبلغ کارت به کارت 10 میلیون تومان
                              if (price.Sum(p => p.Amount) <= 100000000)
                              {
                                 await Bot.SendInvoiceAsync(
                                    (int)e.CallbackQuery.Message.Chat.Id,
                                    string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "سند شما", "شماره", ordrwithdraw.Order1.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordrwithdraw.STRT_DATE)),
                                    "سند واریز به حساب مشتری",
                                    ordrwithdraw.Order1.CODE.ToString(),
                                    ordrwithdraw.DEST_CARD_NUMB_DNRM,
                                    "",
                                    "IRR",
                                    price/*,
                                    photoUrl: "https://devbale.ir/sites/default/files/styles/large/public/1397-12/404733-PCXHHU-813.jpg?itok=3WLQI4eW"*/
                                    );
                              }
                              else
                              {
                                 // پرداخت از طریق درگاه پرداخت
                              }
                           }
                           else if (ordrwithdraw.Order1.AMNT_TYPE == "002")
                           {
                              // پرداخت به صورت کارت به کارت
                              // مبلغ کارت به کارت 10 میلیون تومان
                              if (price.Sum(p => p.Amount) <= 10000000)
                              {
                                 await Bot.SendInvoiceAsync(
                                    (int)e.CallbackQuery.Message.Chat.Id,
                                    string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "سند شما", "شماره", ordrwithdraw.Order1.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordrwithdraw.STRT_DATE)),
                                    "سند واریز به حساب مشتری",
                                    ordrwithdraw.Order1.CODE.ToString(),
                                    ordrwithdraw.DEST_CARD_NUMB_DNRM,
                                    "",
                                    "IRR",
                                    price
                                    );
                              }
                              else
                              {
                                 // پرداخت از طریق درگاه پرداخت
                              }
                           }
                        }
                        #endregion
                        postexecs = postexecs.Replace("withdraw", "");
                        if(postexecs != "")
                           postexecs = postexecs.Substring(1);
                        break;
                     default:
                        break;
                  }
               }
               #endregion

               #region Command Run
               // IF NO COMMAND TO RUN GOTO END
               if (cmnd != "")
               {
                  #region Run Main Command Text
                  // ⏳ Please wait...
                  var waitmesg =
                     await Bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        "⏳ لطفا چند لحظه صبر کنید...",
                        replyMarkup: null
                     );

                  var xmlmsg = RobotHandle.GetData(
                     new XElement("Robot",
                        new XAttribute("token", GetToken()),
                        new XElement("Message",
                           new XAttribute("cbq", "002"),
                           new XAttribute("ussd", ussdcode),
                           new XAttribute("chatid", e.CallbackQuery.Message.Chat.Id),
                           new XAttribute("mesgid", e.CallbackQuery.Message.MessageId),
                           new XElement("Text",
                              new XAttribute("param", param),
                              new XAttribute("postexec", postexecs),
                              new XAttribute("trigger", triggers),
                              cmnd
                           )
                        )
                     ), connectionString);

                  await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, waitmesg.MessageId);

                  var rmessage = xmlmsg.Descendants("Message").FirstOrDefault().Value;
                  #endregion

                  #region "Found Menu"
                  iRobotTech.Proccess_Message_P(
                     new XElement("Robot",
                        new XAttribute("token", GetToken()),
                        new XElement("Message",
                           new XAttribute("ussd", ussdcode),
                           new XAttribute("mesgid", chat.Message.MessageId),
                           new XAttribute("chatid", chat.Message.Chat.Id),
                           new XElement("Text", ussdcode),
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
                              new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber.Replace(" ", "") ?? "" : "")
                           )
                        )
                     ),
                     ref xResult
                  );

                  KeyboardButton[][] keyBoardMarkup = null;
                  if (xResult != null)
                     keyBoardMarkup = CreateKeyboardButton(xResult.Descendants("Text")/*.Select(x => x.Value)*/.ToList(), Convert.ToInt32(xResult.Descendants("Row").FirstOrDefault().Value), Convert.ToInt32(xResult.Descendants("Column").FirstOrDefault().Value));

                  // تنظیم کردن متغییرهای لازمه
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

                  bool visited = false;

                  // اگر بخواهیم کاری را انجام بدهیم که خروجی تابع اصلی برای ما مهم نیست که نمایش داده شود
                  // ولی میخواهیم که رخداد ها اجرا شوند
                  if (triggersteprun != "<>")
                  {
                     #region Before Trigger Execute
                     if (triggersteprun == "<<" && triggers != "")
                     {
                        // Do It
                        foreach (var trigger in triggers.Split(','))
                        {
                           #region Trigger Run
                           string tparam = "";
                           if (trigger.Contains("^"))
                              tparam = trigger.Split('^')[1];
                           string tcmnd = trigger.Split('^')[0];
                           triggers = triggers.Replace(trigger, "");
                           if (triggers != "")
                              triggers = triggers.Substring(1);
                           var xmlmsgtrgr = 
                              RobotHandle.GetData(
                                 new XElement("Robot",
                                    new XAttribute("token", GetToken()),
                                    new XElement("Message",
                                       new XAttribute("cbq", "002"),
                                       new XAttribute("ussd", ussdcode),
                                       new XAttribute("chatid", e.CallbackQuery.Message.Chat.Id),
                                       new XAttribute("mesgid", e.CallbackQuery.Message.MessageId),
                                       new XElement("Text",
                                          new XAttribute("param", tparam),
                                          new XAttribute("postexec", postexecs),
                                          new XAttribute("trigger", triggers),
                                          tcmnd
                                       )
                                    )
                                 ), connectionString);
                           var trmessage = xmlmsgtrgr.Descendants("Message").FirstOrDefault().Value;

                           visited = false;
                           try
                           {
                              var tdata = XDocument.Parse(trmessage).Elements().First();
                              var xdata = xmlmsgtrgr;//XDocument.Parse(message).Elements().First();
                              await FireEventResultOpration(chat, keyBoardMarkup, xdata);
                              visited = true;
                           }
                           catch { visited = false; }

                           try
                           {
                              if (!visited)
                                 await MessagePaging(chat, trmessage, keyBoardMarkup);
                           }
                           catch { }
                           #endregion
                        }
                     }
                     #endregion

                     #region Main Function Execution AND SHOW Result
                     visited = false;
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
                     #endregion

                     #region After Trigger Execute
                     if (triggersteprun == ">>" && triggers != "")
                     {
                        // Do It
                        foreach (var trigger in triggers.Split(','))
                        {
                           #region Trigger Run
                           string tparam = "";
                           if (trigger.Contains("^"))
                              tparam = trigger.Split('^')[1];
                           string tcmnd = trigger.Split('^')[0];
                           triggers = triggers.Replace(trigger, "");
                           if (triggers != "")
                              triggers = triggers.Substring(1);
                           var xmlmsgtrgr = 
                              RobotHandle.GetData(
                                 new XElement("Robot",
                                    new XAttribute("token", GetToken()),
                                    new XElement("Message",
                                       new XAttribute("cbq", "002"),
                                       new XAttribute("ussd", ussdcode),
                                       new XAttribute("chatid", e.CallbackQuery.Message.Chat.Id),
                                       new XAttribute("mesgid", e.CallbackQuery.Message.MessageId),
                                       new XElement("Text",
                                          new XAttribute("param", tparam),
                                          new XAttribute("postexec", postexecs),
                                          new XAttribute("trigger", triggers),
                                          tcmnd
                                       )
                                    )
                                 ), connectionString);
                           var trmessage = xmlmsgtrgr.Descendants("Message").FirstOrDefault().Value;

                           visited = false;
                           try
                           {
                              var tdata = XDocument.Parse(trmessage).Elements().First();
                              var xdata = xmlmsgtrgr;//XDocument.Parse(message).Elements().First();
                              await FireEventResultOpration(chat, keyBoardMarkup, xdata);
                              visited = true;
                           }
                           catch { visited = false; }

                           try
                           {
                              if (!visited)
                                 await MessagePaging(chat, trmessage, keyBoardMarkup);
                           }
                           catch { }
                           #endregion
                        }
                     }
                     #endregion
                  }
                  else
                  {
                     #region Trigger Execute
                     if (triggers != "")
                     {
                        // Do It
                        foreach (var trigger in triggers.Split(','))
                        {
                           #region Trigger Run
                           string tparam = trigger.Split('^')[1];
                           string tcmnd = trigger.Split('^')[0];
                           xmlmsg = RobotHandle.GetData(
                              new XElement("Robot",
                                 new XAttribute("token", GetToken()),
                                 new XElement("Message",
                                    new XAttribute("cbq", "002"),
                                    new XAttribute("ussd", ussdcode),
                                    new XAttribute("chatid", e.CallbackQuery.Message.Chat.Id),
                                    new XAttribute("mesgid", e.CallbackQuery.Message.MessageId),
                                    new XElement("Text",
                                       new XAttribute("param", tparam),
                                       new XAttribute("postexec", postexecs),
                                       new XAttribute("trigger", triggers),
                                       tcmnd
                                    )
                                 )
                              ), connectionString);
                           var trmessage = xmlmsg.Descendants("Message").FirstOrDefault().Value;

                           visited = false;
                           try
                           {
                              var tdata = XDocument.Parse(trmessage).Elements().First();
                              var xdata = xmlmsg;//XDocument.Parse(message).Elements().First();
                              await FireEventResultOpration(chat, keyBoardMarkup, xdata);
                              visited = true;
                           }
                           catch { visited = false; }

                           try
                           {
                              if (!visited)
                                 await MessagePaging(chat, trmessage, keyBoardMarkup);
                           }
                           catch { }
                           #endregion
                        }
                     }
                     #endregion
                  }

                  await Send_Order(iRobotTech, keyBoardMarkup);
                  await Send_Replay_Message(GetToken(), chat);
               }
               #endregion
               #endregion
            }
         }
         catch(Exception exc)
         {
            if (ConsoleOutLog_MemTxt.InvokeRequired)
               ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = exc.Message + ConsoleOutLog_MemTxt.Text));
            else
               ConsoleOutLog_MemTxt.Text = exc.Message + ConsoleOutLog_MemTxt.Text;
         }
      }

      private async void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
      {
         //throw new NotImplementedException();         
         return;
      }

      private async void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs e)
      {         
         if (ConsoleOutLog_MemTxt.InvokeRequired)
            ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += e.ChosenInlineResult.ResultId));
         else
            ConsoleOutLog_MemTxt.Text += e.ChosenInlineResult.ResultId;
      }

      private async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs e)
      {
         try
         {         
            InlineQueryResultBase[] results = {
                   // displayed result
                   new InlineQueryResultArticle(
                       id: "3",
                       title: "TgBots",
                       inputMessageContent: new InputTextMessageContent(
                           "hello"
                       )
                   )
               };
            await Bot.AnswerInlineQueryAsync(
                inlineQueryId: e.InlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0
            );
         }
         catch (Exception)
         {

            throw;
         }
      }

      private async void BotOnUpdateReceived(object sender, UpdateEventArgs e)
      {
         if (e.Update.Type.In(UpdateType.Message))
            if (e.Update.Message.Chat.Id > 0)
               await Robot_Interact(e);
            else
               await Robot_Spy(e);

         try
         {
            await Download_Media(GetToken(), e.Update.Message);
         }
         catch
         { }
      }

      private async Task Robot_Interact(MessageEventArgs e)
      {
         ChatInfo chat = null;
         try
         {            
            var message = e.Message;
            if (message == null) return;

            var iRobotTech = new Data.iRoboTechDataContext(connectionString);
            var xResult = new XElement("Result", "No Message");
            KeyboardButton[][] keyBoardMarkup = null;            

            #region ForceReply
            // اگر اطلاعات ورودی قبلی شامل دریافت ورودی از کاربر تنظیم شده باشد باید اول به آن ها رسیدیگی شود
            if (Chats != null && Chats.Any(c => c.Message.Chat.Id == message.Chat.Id) && Chats.FirstOrDefault(c => c.Message.Chat.Id == message.Chat.Id).ForceReply)
            {
               chat = Chats.FirstOrDefault(c => c.Message.Chat.Id == message.Chat.Id);
               chat.Message = message;
               chat.LastVisitDate = DateTime.Now;               
               if (e.Message.Text == "بازگشت به منوی اصلی" || e.Message.Text == "🔺 بازگشت")
               {
                  chat.ReadyToFire = false;
                  chat.Runed = false;
                  chat.ForceReply = false;
               }
               else
               {
                  chat = Chats.FirstOrDefault(c => c.Message.Chat.Id == message.Chat.Id);
                  string target = chat.Target;
                  if (target == "@")
                  {
                     #region SubSystem Service
                     string uis = chat.UisUssd;
                     string cmnd = chat.CommandText;
                     string param = string.Format("{0},{1}", chat.Params, e.Message.Text);
                     string postexecs = chat.PostExecs;
                     string triggers = chat.Triggers;
                     string triggersteprun = chat.TriggerStepRun;

                     // ⏳ Please wait...
                     await Bot.SendTextMessageAsync(
                        chat.Message.Chat.Id,
                        "⏳ لطفا چند لحظه صبر کنید...",
                        replyMarkup:
                        null);

                     _Strt_Robo_F.SendRequest(
                        new Job(SendType.SelfToUserInterface, 1000 /* Execute Call_SystemService_F */ )
                        {
                           Input =
                              new XElement("Input",
                                 new XAttribute("chatid", chat.Message.Chat.Id),
                                 new XAttribute("ussdcode", ""),
                                 new XAttribute("subsystarget", uis),
                                 new XAttribute("cmnd", cmnd),
                                 new XAttribute("param", param)
                              ),
                           AfterChangedOutput =
                              new Action<object>(
                                 (output) =>
                                 {
                                    var xoutput = output as XElement;

                                    var resultcode = xoutput.Attribute("resultcode").Value.ToInt64();
                                    var resultdesc = xoutput.Attribute("resultdesc").Value;
                                    var mesgtype = (MessageType)xoutput.Attribute("mesgtype").Value.ToInt32();

                                    switch (mesgtype)
                                    {
                                       case MessageType.Text:
                                          Bot.SendTextMessageAsync(
                                             chat.Message.Chat.Id,
                                             string.Format("👈 {0} ",
                                                resultdesc
                                             ),
                                             replyMarkup:
                                             null).Wait();
                                          break;

                                    }
                                 }
                              )
                        }
                     );
                     #endregion
                  }
                  else if (target == ".")
                  {
                     #region Database Service
                     string ussdcode = chat.UisUssd;
                     string cmnd = chat.CommandText;
                     string param = chat.Params == "" ? e.Message.Text : string.Format("{0},{1}", chat.Params, e.Message.Text);
                     string postexecs = chat.PostExecs;
                     string triggers = chat.Triggers;
                     string triggersteprun = chat.TriggerStepRun;

                     //var xResult = new XElement("Result", "No Message");

                     // POST Execution
                     #region Post Execution
                     foreach (var postexec in postexecs.Split(','))
                     {
                        switch (postexec)
                        {
                           case "del":
                              // اگر لازم به این باشد که پیامی که منوی آن انتخاب شده حذف شود
                              // چون ممکن است منوهای آن پیام تغییراتی داشته باشند
                              TryExtension.Try(async () => await Bot.DeleteMessageAsync(chat.Message.Chat.Id, chat.Message.MessageId)).Catch(x => { if (ConsoleOutLog_MemTxt.InvokeRequired) ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = x.Message + ConsoleOutLog_MemTxt.Text)); else ConsoleOutLog_MemTxt.Text = x.Message + ConsoleOutLog_MemTxt.Text; });
                              postexecs = postexecs.Replace("del", "");
                              if (postexecs != "")
                                 postexecs = postexecs.Substring(1);
                              break;
                           case "pay":
                              // Create Payment Invoice
                              #region Order Checkout Payment Process
                              var ordr =
                                 iRobotTech.Orders
                                 .Where(
                                    o => /*o.CHAT_ID == e.CallbackQuery.Message.Chat.Id
                                && o.Robot.TKON_CODE == GetToken()
                                && o.ORDR_STAT == "001"
                                && (o.ORDR_TYPE == "004" || o.ORDR_TYPE == "013" || o.ORDR_TYPE == "014" || o.ORDR_TYPE == "016")
                                && o.STRT_DATE.Value.Date == DateTime.Now.Date
                                && */o.CODE == param.ToInt64() // ordrcode
                                 ).FirstOrDefault();

                              // اگر فاکتور تسویه حساب نباشد به مرحله کارت به کارت میرویم
                              if (ordr.DEBT_DNRM != 0)
                              {
                                 // Process SendInvoice
                                 var price = new List<LabeledPrice>();
                                 if (ordr.AMNT_TYPE == "001")
                                 {
                                    if (ordr.EXTR_PRCT != null && ordr.EXTR_PRCT > 0)
                                    {
                                       //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                                       price.Add(new LabeledPrice("قیمت کل", (int)(ordr.DEBT_DNRM - ordr.EXTR_PRCT - ordr.SUM_FEE_AMNT_DNRM)));
                                       price.Add(new LabeledPrice("ارزش افزوده", (int)ordr.EXTR_PRCT));
                                    }
                                    else
                                       //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                                       price.Add(new LabeledPrice("قیمت کل", (int)(ordr.DEBT_DNRM - ordr.SUM_FEE_AMNT_DNRM)));

                                    // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                                    if (ordr.TXFE_AMNT_DNRM != null && ordr.TXFE_AMNT_DNRM > 0)
                                       price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordr.TXFE_AMNT_DNRM));
                                 }
                                 else if (ordr.AMNT_TYPE == "002")
                                 {
                                    if (ordr.EXTR_PRCT != null && ordr.EXTR_PRCT > 0)
                                    {
                                       //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                                       price.Add(new LabeledPrice("قیمت کل", (int)(ordr.DEBT_DNRM - ordr.EXTR_PRCT - ordr.SUM_FEE_AMNT_DNRM) * 10));
                                       price.Add(new LabeledPrice("ارزش افزوده", (int)ordr.EXTR_PRCT * 10));
                                    }
                                    else
                                       //price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                                       price.Add(new LabeledPrice("قیمت کل", (int)(ordr.DEBT_DNRM - ordr.SUM_FEE_AMNT_DNRM) * 10));

                                    // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                                    if (ordr.TXFE_AMNT_DNRM != null && ordr.TXFE_AMNT_DNRM > 0)
                                       price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordr.TXFE_AMNT_DNRM * 10));
                                 }

                                 if (ordr.AMNT_TYPE == "001")
                                 {
                                    // پرداخت به صورت کارت به کارت
                                    // مبلغ کارت به کارت 10 میلیون تومان
                                    if (price.Sum(p => p.Amount) <= 100000000)
                                    {
                                       await Bot.SendInvoiceAsync(
                                          (int)chat.Message.Chat.Id,
                                          string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordr.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordr.STRT_DATE)),
                                          "سبد خرید شما",
                                          ordr.CODE.ToString(),
                                          ordr.DEST_CARD_NUMB_DNRM,
                                          "",
                                          "IRR",
                                          price/*,
                                    photoUrl: "https://devbale.ir/sites/default/files/styles/large/public/1397-12/404733-PCXHHU-813.jpg?itok=3WLQI4eW"*/
                                          );
                                    }
                                    else
                                    {
                                       // پرداخت از طریق درگاه پرداخت
                                    }
                                 }
                                 else if (ordr.AMNT_TYPE == "002")
                                 {
                                    // پرداخت به صورت کارت به کارت
                                    // مبلغ کارت به کارت 10 میلیون تومان
                                    if (price.Sum(p => p.Amount) <= 10000000)
                                    {
                                       await Bot.SendInvoiceAsync(
                                          (int)chat.Message.Chat.Id,
                                          string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordr.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordr.STRT_DATE)),
                                          "سبد خرید شما",
                                          ordr.CODE.ToString(),
                                          ordr.DEST_CARD_NUMB_DNRM,
                                          "",
                                          "IRR",
                                          price
                                          );
                                    }
                                    else
                                    {
                                       // پرداخت از طریق درگاه پرداخت
                                    }
                                 }
                              }
                              #endregion
                              postexecs = postexecs.Replace("pay", "");
                              if (postexecs != "")
                                 postexecs = postexecs.Substring(1);
                              break;
                           default:
                              break;
                        }
                     }
                     #endregion

                     #region Command Run
                     // IF NO COMMAND TO RUN GOTO END
                     if (cmnd != "")
                     {
                        #region Run Main Command Text
                        // ⏳ Please wait...
                        var waitmesg =
                           await Bot.SendTextMessageAsync(
                              chat.Message.Chat.Id,
                              "⏳ لطفا چند لحظه صبر کنید...",
                              replyMarkup: null
                           );
                        string elmntype = "001",
                               mimetype = "",
                               filename = "",
                               fileext = "";

                        if (chat.Message.Photo != null)
                        {
                           elmntype = "002";
                           mimetype = "application/jpg";
                           filename = e.Message.Photo.Reverse().FirstOrDefault().FileId + ".jpg";
                           fileext = "jpg";
                        }
                        else if (chat.Message.Video != null)
                        {
                           elmntype = "003";
                           mimetype = chat.Message.Video.MimeType;
                        }
                        else if (chat.Message.Document != null)
                        {
                           elmntype = "004";
                           mimetype = chat.Message.Document.MimeType;
                           filename = e.Message.Document.FileName;
                           fileext = e.Message.Document.FileName.Substring(e.Message.Document.FileName.LastIndexOf('.') + 1);
                        }
                        else if (chat.Message.Location != null)
                        {
                           elmntype = "005";
                        }
                        else if (chat.Message.Audio != null)
                        {
                           elmntype = "006";
                           mimetype = chat.Message.Audio.MimeType;
                           filename = e.Message.Audio.FileId;
                        }
                        var xmlmsg = RobotHandle.GetData(
                           new XElement("Robot",
                              new XAttribute("token", GetToken()),
                              new XElement("Message",
                                 new XAttribute("cbq", "002"),
                                 new XAttribute("ussd", ussdcode),
                                 new XAttribute("chatid", chat.Message.Chat.Id),
                                 new XAttribute("elmntype", elmntype),
                                 new XAttribute("mimetype", mimetype),
                                 new XAttribute("filename", filename),
                                 new XAttribute("fileext", fileext),
                                 new XAttribute("mesgid", chat.Message.MessageId),
                                 new XElement("Text",
                                    new XAttribute("param", param),
                                    new XAttribute("postexec", postexecs),
                                    new XAttribute("trigger", triggers),
                                    cmnd
                                 ),
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
                                 ),
                                 new XElement("Contact",
                                    new XAttribute("frstname", chat.Message.Contact != null ? chat.Message.Contact.FirstName ?? "" : ""),
                                    new XAttribute("lastname", chat.Message.Contact != null ? chat.Message.Contact.LastName ?? "" : ""),
                                    new XAttribute("id", chat.Message.Contact != null ? chat.Message.Contact.UserId : 0),
                                    new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber.Replace(" ", "") ?? "" : "")
                                 )
                              )
                           ), connectionString);

                        await Bot.DeleteMessageAsync(chat.Message.Chat.Id, waitmesg.MessageId);

                        var rmessage = xmlmsg.Descendants("Message").FirstOrDefault().Value;
                        #endregion

                        #region "Found Menu"
                        iRobotTech.Proccess_Message_P(
                           new XElement("Robot",
                              new XAttribute("token", GetToken()),
                              new XElement("Message",
                                 new XAttribute("ussd", ussdcode),
                                 new XAttribute("mesgid", chat.Message.MessageId),
                                 new XAttribute("chatid", chat.Message.Chat.Id),
                                 new XElement("Text", ussdcode),
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
                                    new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber.Replace(" ", "") ?? "" : "")
                                 )
                              )
                           ),
                           ref xResult
                        );

                        if (xResult != null)
                           keyBoardMarkup = CreateKeyboardButton(xResult.Descendants("Text")/*.Select(x => x.Value)*/.ToList(), Convert.ToInt32(xResult.Descendants("Row").FirstOrDefault().Value), Convert.ToInt32(xResult.Descendants("Column").FirstOrDefault().Value));

                        // تنظیم کردن متغییرهای لازمه
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

                        bool visited = false;

                        // اگر بخواهیم کاری را انجام بدهیم که خروجی تابع اصلی برای ما مهم نیست که نمایش داده شود
                        // ولی میخواهیم که رخداد ها اجرا شوند
                        if (triggersteprun != "<>")
                        {
                           #region Before Trigger Execute
                           if (triggersteprun == "<<" && triggers != "")
                           {
                              // Do It
                              foreach (var trigger in triggers.Split(','))
                              {
                                 #region Trigger Run
                                 string tparam = "";
                                 if (trigger.Contains("^"))
                                    tparam = trigger.Split('^')[1];
                                 string tcmnd = trigger.Split('^')[0];
                                 triggers = triggers.Replace(trigger, "");
                                 if (triggers != "")
                                    triggers = triggers.Substring(1);
                                 var xmlmsgtrgr =
                                    RobotHandle.GetData(
                                       new XElement("Robot",
                                          new XAttribute("token", GetToken()),
                                          new XElement("Message",
                                             new XAttribute("cbq", "002"),
                                             new XAttribute("ussd", ussdcode),
                                             new XAttribute("chatid", chat.Message.Chat.Id),
                                             new XAttribute("mesgid", chat.Message.MessageId),
                                             new XElement("Text",
                                                new XAttribute("param", tparam),
                                                new XAttribute("postexec", postexecs),
                                                new XAttribute("trigger", triggers),
                                                tcmnd
                                             )
                                          )
                                       ), connectionString);
                                 var trmessage = xmlmsgtrgr.Descendants("Message").FirstOrDefault().Value;

                                 visited = false;
                                 try
                                 {
                                    var tdata = XDocument.Parse(trmessage).Elements().First();
                                    var xdata = xmlmsgtrgr;//XDocument.Parse(message).Elements().First();
                                    await FireEventResultOpration(chat, keyBoardMarkup, xdata);
                                    visited = true;
                                 }
                                 catch { visited = false; }

                                 try
                                 {
                                    if (!visited)
                                       await MessagePaging(chat, trmessage, keyBoardMarkup);
                                 }
                                 catch { }
                                 #endregion
                              }
                           }
                           #endregion

                           #region Main Function Execution AND SHOW Result
                           visited = false;
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
                           #endregion

                           #region After Trigger Execute
                           if (triggersteprun == ">>" && triggers != "")
                           {
                              // Do It
                              foreach (var trigger in triggers.Split(','))
                              {
                                 #region Trigger Run
                                 string tparam = "";
                                 if (trigger.Contains("^"))
                                    tparam = trigger.Split('^')[1];
                                 string tcmnd = trigger.Split('^')[0];
                                 triggers = triggers.Replace(trigger, "");
                                 if (triggers != "")
                                    triggers = triggers.Substring(1);
                                 var xmlmsgtrgr =
                                    RobotHandle.GetData(
                                       new XElement("Robot",
                                          new XAttribute("token", GetToken()),
                                          new XElement("Message",
                                             new XAttribute("cbq", "002"),
                                             new XAttribute("ussd", ussdcode),
                                             new XAttribute("chatid", chat.Message.Chat.Id),
                                             new XAttribute("mesgid", chat.Message.MessageId),
                                             new XElement("Text",
                                                new XAttribute("param", tparam),
                                                new XAttribute("postexec", postexecs),
                                                new XAttribute("trigger", triggers),
                                                tcmnd
                                             )
                                          )
                                       ), connectionString);
                                 var trmessage = xmlmsgtrgr.Descendants("Message").FirstOrDefault().Value;

                                 visited = false;
                                 try
                                 {
                                    var tdata = XDocument.Parse(trmessage).Elements().First();
                                    var xdata = xmlmsgtrgr;//XDocument.Parse(message).Elements().First();
                                    await FireEventResultOpration(chat, keyBoardMarkup, xdata);
                                    visited = true;
                                 }
                                 catch { visited = false; }

                                 try
                                 {
                                    if (!visited)
                                       await MessagePaging(chat, trmessage, keyBoardMarkup);
                                 }
                                 catch { }
                                 #endregion
                              }
                           }
                           #endregion
                        }
                        else
                        {
                           #region Trigger Execute
                           if (triggers != "")
                           {
                              // Do It
                              foreach (var trigger in triggers.Split(','))
                              {
                                 #region Trigger Run
                                 string tparam = trigger.Split('^')[1];
                                 string tcmnd = trigger.Split('^')[0];
                                 xmlmsg = RobotHandle.GetData(
                                    new XElement("Robot",
                                       new XAttribute("token", GetToken()),
                                       new XElement("Message",
                                          new XAttribute("cbq", "002"),
                                          new XAttribute("ussd", ussdcode),
                                          new XAttribute("chatid", chat.Message.Chat.Id),
                                          new XAttribute("mesgid", chat.Message.MessageId),
                                          new XElement("Text",
                                             new XAttribute("param", tparam),
                                             new XAttribute("postexec", postexecs),
                                             new XAttribute("trigger", triggers),
                                             tcmnd
                                          )
                                       )
                                    ), connectionString);
                                 var trmessage = xmlmsg.Descendants("Message").FirstOrDefault().Value;

                                 visited = false;
                                 try
                                 {
                                    var tdata = XDocument.Parse(trmessage).Elements().First();
                                    var xdata = xmlmsg;//XDocument.Parse(message).Elements().First();
                                    await FireEventResultOpration(chat, keyBoardMarkup, xdata);
                                    visited = true;
                                 }
                                 catch { visited = false; }

                                 try
                                 {
                                    if (!visited)
                                       await MessagePaging(chat, trmessage, keyBoardMarkup);
                                 }
                                 catch { }
                                 #endregion
                              }
                           }
                           #endregion
                        }

                        await Send_Order(iRobotTech, keyBoardMarkup);
                        await Send_Replay_Message(GetToken(), chat);
                     }
                     #endregion
                     #endregion
                  }
                  return;
               }
            }
            #endregion

            //ChatInfo chat = null;
            #region HoldHistory
            if (Chats.All(c => c.Message.Chat.Id != e.Message.Chat.Id))
               Chats.Add(new ChatInfo() { Message = message, LastVisitDate = DateTime.Now, Runed = false });
            chat = Chats.FirstOrDefault(c => c.Message.Chat.Id == message.Chat.Id);
            chat.Message = message;
            chat.LastVisitDate = DateTime.Now;
            chat.ForceReply = false;
            var removeRam = Chats.Where(c => c.LastVisitDate.AddMinutes(5) < DateTime.Now);
            removeRam.ToList().Clear();
            #endregion

            if (chat.Message.Chat.Id < 0)
               return;

            try
            {
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", message.Chat.Id, message.From.FirstName + ", " + message.From.LastName, message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text));
               else
                  ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", message.Chat.Id, message.From.FirstName + ", " + message.From.LastName, message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
            }
            catch { }

            xResult = new XElement("Respons", "No Message");

            // 1398/11/20 * ثبت وصولی بدست آماده
            if (e.Message.Type == MessageType.SuccessfulPayment && e.Message.From.Username == "receipt")
            {
               // ⏳ Please wait...
               var waitmesg = 
                  await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        "⏳ لطفا چند لحظه صبر کنید...",
                        replyMarkup:
                        null);

               // ثبت وصولی پرداخت شده درون سیستم و صدور سند مالی
               // شماره فاکتور سیستم ما : e.Message.SuccessfulPayment.InvoicePayload
               // شماره پیگیری از سمت بانک : e.Message.SuccessfulPayment.ProviderPaymentChargeId
               // کل مبلغ پرداخت شده : e.Message.SuccessfulPayment.TotalAmount               
               iRobotTech.SAVE_PYMT_P(
                  new XElement("Payment",
                     new XAttribute("ordrcode", e.Message.SuccessfulPayment.InvoicePayload),
                     new XAttribute("txid", e.Message.SuccessfulPayment.ProviderPaymentChargeId),
                     new XAttribute("totlamnt", e.Message.SuccessfulPayment.TotalAmount),
                     new XAttribute("dircall", "002"),
                     new XAttribute("rcptmtod", "009")
                  ),
                  ref xResult
               );

               await Bot.DeleteMessageAsync(e.Message.Chat.Id, waitmesg.MessageId);

               await FireEventResultOpration(chat, null, xResult);

               await Send_Order(iRobotTech, null);

               await Send_Replay_Message(GetToken(), chat);

               return;
            }

            // 1398/12/04 * اگر دستوراتی مخفی ربات دریافت شد
            if(e.Message.Type == MessageType.Text && 
               e.Message.Text.Length >= 2 &&
               e.Message.Text.Substring(0, 2).In("*%" /* این گزینه برای پاسخگویی به خدمات پیک موتوری هست */))
            {
               // اجرای دستورات ربات برای خدمات مخفی
               if (e.Message.Text.Substring(0, 2).In("*%" /* این گزینه برای پاسخگویی به خدمات پیک موتوری هست */))
               {
                  // خدمات پیک موتوری و اسنپ
                  iRobotTech.SAVE_ALPK_P(
                     new XElement("RequestAlopeyk",
                        new XAttribute("token", GetToken()),
                        new XAttribute("actncode", "000"),
                        new XAttribute("cmnd", e.Message.Text),
                        new XElement("Alopeyk",
                           new XAttribute("chatid", e.Message.Chat.Id)
                        )
                        
                     ),
                     ref xResult
                  );
               }

               await FireEventResultOpration(chat, null, xResult);               

               await Send_Order(iRobotTech, null);

               await Send_Replay_Message(GetToken(), chat);

               return;
            }

            // ارسال تبلیغات
            try
            {
               await Send_Advertising(GetToken(), chat);
            }
            catch (Exception exc)
            {
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += exc.Message));
               else
                  ConsoleOutLog_MemTxt.Text += exc.Message;

               //Bot.SendTextMessageAsync(
               //      chat.Message.Chat.Id,
               //      exc.Message
               //   ).Wait();
            }
            try
            {
               await Send_Replay_Message(GetToken(), chat);
            }
            catch (Exception exc)
            {
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = exc.Message + ConsoleOutLog_MemTxt.Text));
               else
                  ConsoleOutLog_MemTxt.Text = exc.Message + ConsoleOutLog_MemTxt.Text;

               //Bot.SendTextMessageAsync(
               //   chat.Message.Chat.Id,
               //   exc.Message
               //).Wait();
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
                                 && r.TKON_CODE == GetToken()
                                 && p.SHOW_STRT == "002"
                                 && p.IMAG_DESC != null
                           orderby p.ORDR descending
                           select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.ORDR }).ToList();

               if (iRobotTech.Robots.Any(r => r.TKON_CODE == GetToken() && r.BULD_STAT != "006"))
               {
                  try
                  {
                     ///***FileToSend fts = new FileToSend(iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == Token).BULD_FILE_ID);
                     InputOnlineFile fts = new InputOnlineFile(iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == GetToken()).BULD_FILE_ID);
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
                     ///***photo = new FileToSend()
                     ///***{
                     ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                     ///***   Filename = pic.FILE_NAME
                     ///***};
                     photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                  }
                  else
                  {
                     ///***photo = new FileToSend(pic.FILE_ID);
                     photo = new InputOnlineFile(pic.FILE_ID);
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
                                 && r.TKON_CODE == GetToken()
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
                  string.Format("کد دستگاه شما {0} می باشد", chat.Message.Chat.Id)
               );
            }
            #endregion

            #region "Check Menu on Request Contact OR Request Location"
            if (chat.UssdCode != null)
            {
               var menu = iRobotTech.Menu_Ussds.FirstOrDefault(m => m.ROBO_RBID == robot.RBID && m.Menu_Ussd1.USSD_CODE == chat.UssdCode);
               if(menu != null)
                  switch (message.Type)
                  {
                     case MessageType.Contact:
                        if (menu.CMND_TYPE == "015")                           
                           message.Text = menu.MENU_TEXT;
                        break;
                     case MessageType.Location:
                        if (menu.CMND_TYPE == "016")                           
                           message.Text = menu.MENU_TEXT;
                        break;
                  }
            }
            #endregion

            #region "Found Menu"
            iRobotTech.Proccess_Message_P(
               new XElement("Robot",
                  new XAttribute("token", GetToken()),
                  new XElement("Message",
                     new XAttribute("ussd", chat.UssdCode ?? ""),
                     new XAttribute("mesgid", chat.Message.MessageId),
                     new XAttribute("chatid", chat.Message.Chat.Id),
                     new XAttribute("refchatid", chat.Message.Text != null && chat.Message.Text.Length > 6 ? chat.Message.Text.ToLower().Substring(0, 6) == "/start" ? (chat.Message.Text.Split(' ').Count() > 1 ? (chat.Message.Text.Split(' ')[1]) : "") : "" : ""),
                     new XElement("Text", chat.Message.Text == null ? (chat.Message.Contact != null ? string.Format("{0}*{1}*{2}", chat.Message.Contact.FirstName + " , " + chat.Message.Contact.LastName, chat.Message.Contact.PhoneNumber.Replace(" ", ""), "آدرس نا مشخص") : "No Text") : chat.Message.Text),
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
                        new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber.Replace(" ", "") ?? "" : "")
                     )
                  )
               ),
               ref xResult
            );
            #endregion
            keyBoardMarkup = null;
            #region Create Menu Array            
            if (xResult != null)
               keyBoardMarkup = CreateKeyboardButton(xResult.Descendants("Text")/*.Select(x => x.Value)*/.ToList(), Convert.ToInt32(xResult.Descendants("Row").FirstOrDefault().Value), Convert.ToInt32(xResult.Descendants("Column").FirstOrDefault().Value));
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
                                 && r.TKON_CODE == GetToken()
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
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                  where r.TKON_CODE == GetToken()                               
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

            #region Developer Monitor
            if ((e.Message.Caption != null && e.Message.Caption.Length >= 2 && ( e.Message.Caption == "*#" || e.Message.Caption.Substring(0, 2) == "*#")) ||
               (e.Message.Text != null && e.Message.Text.Length >= 2 && (e.Message.Text == "*#" || e.Message.Text.Substring(0, 2) == "*#")) ||
               (e.Message.Sticker != null))
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
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Photo :\n\r\n\r" + e.Message.Photo.Reverse().FirstOrDefault().FileId);
                  fileid = e.Message.Photo.Reverse().FirstOrDefault().FileId;
                  filetype = "002";
                  //await Bot.GetFileAsync(chat.Message.Photo.Reverse().FirstOrDefault().FileId, new FileStream(@"C:\Image\MyFile.jpg", FileMode.OpenOrCreate));
               }
               else if (e.Message.Video != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Video :\n\r\n\r" + e.Message.Video.FileId);
                  fileid = e.Message.Video.FileId;
                  filetype = "003";
               }
               else if (e.Message.Document != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Document :\n\r\n\r" + e.Message.Document.FileId);
                  fileid = e.Message.Document.FileId;
                  filetype = "004";
               }
               else if (e.Message.Audio != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Audio :\n\r\n\r" + e.Message.Audio.FileId);
                  fileid = e.Message.Audio.FileId;
                  filetype = "006";
               }
               else if (e.Message.Location != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, string.Format("X : {0}\n\rY : {1}", e.Message.Location.Latitude, e.Message.Location.Longitude));
                  filetype = "005";
               }
               else if (e.Message.Sticker != null)
               {
                  await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Sticker :\n\r\n\r" + e.Message.Sticker.FileId);
                  await Bot.SendStickerAsync(e.Message.Chat.Id, /*"BQADBAADOwMAAgXhMAewhyhhPvCl1QI"*/new InputOnlineFile(e.Message.Sticker.FileId), false, e.Message.MessageId);
                  fileid = e.Message.Sticker.FileId;
                  filetype = "007";
               }
               try
               {
                  //if (menucmndtype.CMND_TYPE != "018" && fileid != "")
                  if (e.Message.Caption != null && e.Message.Caption != "*#")
                  {
                     iRobotTech.INS_OGMD_P(
                        new XElement("Robot",
                           new XAttribute("tokencode", GetToken()),
                           new XAttribute("chatid", e.Message.Chat.Id),
                           new XElement("File",
                              new XAttribute("id", fileid),                              
                              new XAttribute("ussdcode", e.Message.Caption.Substring(e.Message.Caption.IndexOf('*', 2))),
                              new XAttribute("cmndtype", e.Message.Caption.Substring(2, e.Message.Caption.IndexOf('*', 2) - 2)),
                              new XAttribute("filetype", filetype)
                           )
                        )
                     );
                  }
                  else if (e.Message.Text != null && e.Message.Text != "*#")
                     iRobotTech.INS_OGMD_P(
                        new XElement("Robot",
                           new XAttribute("tokencode", GetToken()),
                           new XAttribute("chatid", e.Message.Chat.Id),
                           new XElement("File",                              
                              new XAttribute("ussdcode", e.Message.Text.Substring(e.Message.Text.IndexOf('*', 2))),
                              new XAttribute("cmndtype", e.Message.Text.Substring(2, e.Message.Text.IndexOf('*', 2) - 2))
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
                                 where r.TKON_CODE == GetToken()
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
                                     sr.Robot.TKON_CODE == GetToken()
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

                           ///***await Bot.GetFileAsync(e.Message.Photo.Reverse().FirstOrDefault().FileId, System.IO.File.Create(fileupload + "\\" + filename + ".jpg"));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(e.Message.Photo.Reverse().FirstOrDefault().FileId);
                           file.FilePath = fileupload + "\\" + filename + ".jpg";
                           await Bot.SendTextMessageAsync(e.Message.Chat.Id, "فایل عکس شما با موفقیت ذخیره گردید 💾", ParseMode.Default, false, false, e.Message.MessageId, null);
                        }
                        catch (Exception ex)
                        {
                           //Bot.SendTextMessageAsync(e.Message.Chat.Id, ex.Message, ParseMode.Default, false, false, e.Message.MessageId, null).Wait();
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

                           ///***await Bot.GetFileAsync(e.Message.Video.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Video.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(e.Message.Video.FileId);
                           file.FilePath = fileupload + "\\" + filename;
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
                           ///***await Bot.GetFileAsync(e.Message.Document.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Document.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(e.Message.Document.FileId);
                           file.FilePath = fileupload + "\\" + filename;
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
                           ///***await Bot.GetFileAsync(e.Message.Audio.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Audio.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(e.Message.Audio.FileId);
                           file.FilePath = fileupload + "\\" + filename;
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
                           ///***await Bot.GetFileAsync(e.Message.Sticker.FileId, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Sticker.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(e.Message.Sticker.FileId);
                           file.FilePath = fileupload + "\\" + filename;
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
                  else if (chat.Message.Video != null)
                  {
                     elmntype = "003";
                     mimetype = chat.Message.Video.MimeType;
                  }
                  else if (chat.Message.Document != null)
                  {
                     elmntype = "004";
                     mimetype = chat.Message.Document.MimeType;
                     filename = e.Message.Document.FileName;
                     fileext = e.Message.Document.FileName.Substring(e.Message.Document.FileName.LastIndexOf('.') + 1);
                  }
                  else if (chat.Message.Location != null)
                  {
                     elmntype = "005";
                  }
                  else if (chat.Message.Audio != null)
                  {
                     elmntype = "006";
                     mimetype = chat.Message.Audio.MimeType;
                     filename = e.Message.Audio.FileId;
                  }
                  try
                  {
                     // ⏳ Please wait...
                     var waitmesg =
                        await Bot.SendTextMessageAsync(
                              e.Message.Chat.Id,
                              "⏳ لطفا چند لحظه صبر کنید...",
                              replyMarkup:
                              null);

                     var xmlmsg = RobotHandle.GetData(
                        new XElement("Robot",
                           new XAttribute("token", GetToken()),
                           new XElement("Message",
                              new XAttribute("ussd", chat.UssdCode ?? ""),
                              new XAttribute("childussd", menucmndtype != null ? menucmndtype.USSD_CODE ?? "" : ""),
                              new XAttribute("chatid", chat.Message.Chat.Id),
                              new XAttribute("elmntype", elmntype),
                              new XAttribute("mimetype", mimetype),
                              new XAttribute("filename", filename),
                              new XAttribute("fileext", fileext),
                              new XAttribute("mesgid", chat.Message.MessageId),
                              new XElement("Text", chat.Message.Text == null ? (chat.Message.Contact != null ? string.Format("{0}*{1}*{2}", chat.Message.Contact.FirstName + " , " + chat.Message.Contact.LastName, chat.Message.Contact.PhoneNumber.Replace(" ", ""), "متن نا مشخص") : (chat.Message.Caption == null ? "No Text" : chat.Message.Caption)) : chat.Message.Text),
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
                              ),
                              new XElement("Contact",
                                 new XAttribute("frstname", chat.Message.Contact != null ? chat.Message.Contact.FirstName ?? "" : ""),
                                 new XAttribute("lastname", chat.Message.Contact != null ? chat.Message.Contact.LastName ?? "" : ""),
                                 new XAttribute("id", chat.Message.Contact != null ? chat.Message.Contact.UserId : 0),
                                 new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber.Replace(" ", "") ?? "" : "")
                              )
                           )
                        ), connectionString);

                     await Bot.DeleteMessageAsync(e.Message.Chat.Id, waitmesg.MessageId);

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
                  catch { }                  
               }
            }
            else if (menucmndtype != null && menucmndtype.CMND_TYPE != null && new List<string> { "001", "002", "003", "004", "005", "006", "007", "008", "009", "010", "011", "012", "013", "014", "015", "016", "017", "018", "019", "020", "021", "022", "023", "024", "025", "026", "027", "028" }.Contains(menucmndtype.CMND_TYPE))
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
                * 021 - Download
                * 022 - Stickers
                * 023 - Stickers & Info Text
                * 024 - Stickers & Image
                * 025 - Stickers & Image & Info Text
                * 026 - Direct Payment Process
                * 027 - Order Checkout Payment Process
                * 028 - SubSystem Service
                */
               if (menucmndtype.CMND_TYPE == "001")
               {
                  #region Location
                  // 001 - Location
                  var loc = iRobotTech.Organs
                  .Join(iRobotTech.Robots, organ => organ.OGID, robot => robot.ORGN_OGID,
                  (organ, robot) => new { organ.CORD_X, organ.CORD_Y, robot.TKON_CODE }).ToList().FirstOrDefault(or => or.TKON_CODE == GetToken());

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
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
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
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                      && r.TKON_CODE == GetToken()
                                //&& p.IMAG_DESC != null
                                orderby p.ORDR
                                select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var video in videos)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(video.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(video.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = video.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(video.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), video.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(video.FILE_ID);
                        photo = new InputOnlineFile(video.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                      && r.TKON_CODE == GetToken()
                                //&& p.IMAG_DESC != null
                                orderby p.ORDR
                                select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var video in videos)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(video.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(video.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = video.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(video.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), video.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(video.FILE_ID);
                        photo = new InputOnlineFile(video.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
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

                  // ⏳ Please wait...
                  var waitmesg = 
                     await Bot.SendTextMessageAsync(
                           e.Message.Chat.Id,
                           "⏳ لطفا چند لحظه صبر کنید...",
                           replyMarkup:
                           null);

                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", GetToken()),
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
                          ),
                          new XElement("Contact",
                             new XAttribute("frstname", chat.Message.Contact != null ? chat.Message.Contact.FirstName ?? "" : ""),
                             new XAttribute("lastname", chat.Message.Contact != null ? chat.Message.Contact.LastName ?? "" : ""),
                             new XAttribute("id", chat.Message.Contact != null ? chat.Message.Contact.UserId : 0),
                             new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber.Replace(" ", "") ?? "" : "")
                          )
                       )
                    ), connectionString);

                  await Bot.DeleteMessageAsync(e.Message.Chat.Id, waitmesg.MessageId);

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
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
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
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                  (organ, robot) => new { organ.CORD_X, organ.CORD_Y, robot.TKON_CODE }).ToList().FirstOrDefault(or => or.TKON_CODE == GetToken());

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

                  // ⏳ Please wait...
                  var waitmesg = 
                     await Bot.SendTextMessageAsync(
                           e.Message.Chat.Id,
                           "⏳ لطفا چند لحظه صبر کنید...",
                           replyMarkup:
                           null);

                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", GetToken()),
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
                          ),
                          new XElement("Contact",
                             new XAttribute("frstname", chat.Message.Contact != null ? chat.Message.Contact.FirstName ?? "" : ""),
                             new XAttribute("lastname", chat.Message.Contact != null ? chat.Message.Contact.LastName ?? "" : ""),
                             new XAttribute("id", chat.Message.Contact != null ? chat.Message.Contact.UserId : 0),
                             new XAttribute("phonnumb", chat.Message.Contact != null ? chat.Message.Contact.PhoneNumber.Replace(" ", "") ?? "" : "")
                          )
                       )
                    ), connectionString);

                  await Bot.DeleteMessageAsync(e.Message.Chat.Id, waitmesg.MessageId);

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
                  var mesg = 
                     await Bot.SendTextMessageAsync(
                        chat.Message.Chat.Id,
                        "Typing",//xResult.Descendants("Description").FirstOrDefault().Value,
                        replyToMessageId:
                        chat.Message.MessageId,
                        replyMarkup:
                        new ReplyKeyboardMarkup()
                        {
                           Keyboard = keyBoardMarkup,
                           ResizeKeyboard = true,
                           Selective = true
                        });                  
                  
                  await Bot.DeleteMessageAsync(chat.Message.Chat.Id, mesg.MessageId);                  
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
                                    && r.TKON_CODE == GetToken()
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
                                    && r.TKON_CODE == GetToken()
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
                                    && r.TKON_CODE == GetToken()
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var doc in docs)
                  {
                     dynamic document;
                     if (string.IsNullOrEmpty(doc.FILE_ID))
                     {
                        ///***document = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(doc.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = doc.FILE_NAME
                        ///***};
                        document = new InputOnlineFile(new FileStream(doc.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), doc.FILE_NAME);
                     }
                     else
                     {
                        ///***document = new FileToSend(doc.FILE_ID);
                        document = new InputOnlineFile(doc.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
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
                                      && r.TKON_CODE == GetToken()
                                //&& p.IMAG_DESC != null
                                orderby p.ORDR
                                select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var sound in sounds)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(sound.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(sound.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = sound.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(sound.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), sound.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(sound.FILE_ID);
                        photo = new InputOnlineFile(sound.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendAudioAsync(chat.Message.Chat.Id, photo, "*", 0, 0, sound.IMAG_DESC, "#",
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
               else if (menucmndtype.CMND_TYPE == "015")
               {
                  #region Send Request Contact
                  // ⏳ Please wait...
                  await Bot.SendTextMessageAsync(
                           e.Message.Chat.Id,
                           "⏳ لطفا چند لحظه صبر کنید...",
                           replyMarkup:
                           null);

                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", GetToken()),
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
                  #endregion
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
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                                    && r.TKON_CODE == GetToken()
                              //&& p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var doc in docs)
                  {
                     dynamic document;
                     if (string.IsNullOrEmpty(doc.FILE_ID))
                     {
                        ///***document = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(doc.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = doc.FILE_NAME
                        ///***};
                        document = new InputOnlineFile(new FileStream(doc.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), doc.FILE_NAME);
                     }
                     else
                     {
                        ///***document = new FileToSend(doc.FILE_ID);
                        document = new InputOnlineFile(doc.FILE_ID);
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
                                     && r.TKON_CODE == GetToken()
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
                                    && r.TKON_CODE == GetToken()
                                    && u.FILE_ID != null
                                    && u.CHAT_ID == chat.Message.Chat.Id
                              orderby u.RWNO
                              select new { u.FILE_NAME, u.FILE_PATH, IMAG_DESC = u.FILE_PATH.Substring(u.FILE_PATH.LastIndexOf('\\') + 1), u.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic photo;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        photo = new InputOnlineFile(pic.FILE_ID);
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
                           string.Format("{0}\n\rhttps://ble.ir/{1}?start={2}\n\r{3}", robot.INVT_FRND ?? "از اینکه دوستان خود را به ما معرفی میکنید بسیار ممنون و خرسندیم، لینک شما برای دعوت کردن دوستان. با بله حساب کتابت با همه درسته 👉 😀 👈", robot.NAME.Substring(1), e.Message.Chat.Id, robot.HASH_TAG ?? "#انارسافت"),
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
               else if (menucmndtype.CMND_TYPE == "021")
               {
                  #region Download
                  // ارسال فایل
                  foreach (var file in Directory.GetFiles(string.Format(@"{0}\{1}\", robot.UP_LOAD_FILE_PATH, e.Message.Chat.Id)))
                  {
                     await Bot.SendDocumentAsync(e.Message.Chat.Id,
                        new InputOnlineFile(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read), /*e.Message.Chat.Id.ToString()*/file)
                     );

                     //await Bot.SendDocumentAsync(e.Message.Chat.Id,
                     //   new InputOnlineFile(new FileStream(string.Format(@"{0}\{1}\{1}.pdf", robot.UP_LOAD_FILE_PATH, e.Message.Chat.Id), FileMode.Open, FileAccess.Read, FileShare.Read), e.Message.Chat.Id.ToString())
                     //);
                  }

                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "022")
               {
                  #region Stickers
                  // 022 - Stickers
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_TYPE == "007"
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic sticker;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        sticker = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        sticker = new InputOnlineFile(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendStickerAsync(chat.Message.Chat.Id, sticker,
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
               else if (menucmndtype.CMND_TYPE == "023")
               {
                  #region Stickers
                  // 023 - Stickers & Text
                  chat.Runed = false;
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_TYPE == "007"
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic sticker;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        sticker = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        sticker = new InputOnlineFile(pic.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendStickerAsync(chat.Message.Chat.Id, sticker,
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
                                    && r.TKON_CODE == GetToken()
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
               else if (menucmndtype.CMND_TYPE == "024")
               {
                  #region Stickers
                  // 024 - Stickers & Image
                  chat.Runed = false;
                  var stickers = (from o in iRobotTech.Organs
                                  join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                                  join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                                  where o.STAT == "002"
                                        && r.STAT == "002"
                                        && p.STAT == "002"
                                        && p.USSD_CODE == menucmndtype.USSD_CODE
                                        && r.TKON_CODE == GetToken()
                                        && p.IMAG_TYPE == "007"
                                  orderby p.ORDR
                                  select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in stickers)
                  {
                     dynamic sticker;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        sticker = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        sticker = new InputOnlineFile(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendStickerAsync(chat.Message.Chat.Id, sticker,
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
                  #region Image
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic sticker;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        sticker = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        sticker = new InputOnlineFile(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, sticker, pic.IMAG_DESC,
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
               else if (menucmndtype.CMND_TYPE == "025")
               {
                  #region Stickers
                  // 025 - Stickers & Image & Text
                  chat.Runed = false;
                  var stickers = (from o in iRobotTech.Organs
                                  join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                                  join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                                  where o.STAT == "002"
                                        && r.STAT == "002"
                                        && p.STAT == "002"
                                        && p.USSD_CODE == menucmndtype.USSD_CODE
                                        && r.TKON_CODE == GetToken()
                                        && p.IMAG_TYPE == "007"
                                  orderby p.ORDR
                                  select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID }).ToList();

                  foreach (var pic in stickers)
                  {
                     dynamic sticker;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        sticker = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        sticker = new InputOnlineFile(pic.FILE_ID);
                     }

                     try
                     {
                        await Bot.SendStickerAsync(chat.Message.Chat.Id, sticker,
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
                  #region Image
                  var pics = (from o in iRobotTech.Organs
                              join r in iRobotTech.Robots on o.OGID equals r.ORGN_OGID
                              join p in iRobotTech.Organ_Medias on o.OGID equals p.ORGN_OGID
                              where o.STAT == "002"
                                    && r.STAT == "002"
                                    && p.STAT == "002"
                                    && p.USSD_CODE == menucmndtype.USSD_CODE
                                    && r.TKON_CODE == GetToken()
                                    && p.IMAG_DESC != null
                              orderby p.ORDR
                              select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.OPID }).ToList();

                  foreach (var pic in pics)
                  {
                     dynamic sticker;
                     if (string.IsNullOrEmpty(pic.FILE_ID))
                     {
                        ///***photo = new FileToSend()
                        ///***{
                        ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                        ///***   Filename = pic.FILE_NAME
                        ///***};
                        sticker = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
                     }
                     else
                     {
                        ///***photo = new FileToSend(pic.FILE_ID);
                        sticker = new InputOnlineFile(pic.FILE_ID);
                     }

                     try
                     {
                        //RobotClient.SendChatAction(chat.Message.Chat.Id, ChatActions.Upload_photo);
                        await Bot.SendPhotoAsync(chat.Message.Chat.Id, sticker, pic.IMAG_DESC,
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
                                    && r.TKON_CODE == GetToken()
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
               else if(menucmndtype.CMND_TYPE == "026")
               {
                  #region Direct Payment Process
                  var _getdirectpaymentprocess =
                     from ghi in iRobotTech.Group_Header_Items
                     join gm in iRobotTech.Group_Menu_Ussds on ghi.Group_Menu_Ussd equals gm
                     join g in iRobotTech.Groups on gm.Group equals g
                     join srg in iRobotTech.Service_Robot_Groups on g equals srg.Group
                     join sr in iRobotTech.Service_Robots on srg.Service_Robot equals sr
                     join r in iRobotTech.Robots on sr.Robot equals r
                     where r.TKON_CODE == GetToken()
                        && sr.CHAT_ID == chat.Message.Chat.Id
                        //&& srg.DFLT_STAT == "002"
                        && ghi.STAT == "002"
                        && g.STAT == "002"
                        && gm.STAT == "002"
                        && srg.STAT == "002"
                        && gm.MNUS_MUID == menucmndtype.MUID
                     select new
                     {
                        Price = ghi.PRIC,
                        Ussd_Code = ghi.USSD_CODE_DNRM,
                        Muid = ghi.GRMU_MNUS_MUID,
                        Gpid = ghi.GRMU_GROP_GPID,
                        Auto_Join = g.AUTO_JOIN,
                        Default = srg.DFLT_STAT,
                        Title = ghi.GHDT_DESC,
                        Description = ghi.Group_Header.GRPH_DESC,
                        Tax = ghi.TAX_PRCT,
                        Off_Percentage = g.OFF_PRCT
                     };

                  var _getdefaultgroupaccess =
                     from gm in iRobotTech.Group_Menu_Ussds
                     join g in iRobotTech.Groups on gm.Group equals g
                     join srg in iRobotTech.Service_Robot_Groups on g equals srg.Group
                     join sr in iRobotTech.Service_Robots on srg.Service_Robot equals sr
                     join r in iRobotTech.Robots on sr.Robot equals r
                     where r.TKON_CODE == GetToken()
                        && sr.CHAT_ID == chat.Message.Chat.Id
                        && srg.DFLT_STAT == "002"
                        && g.STAT == "002"
                        && gm.STAT == "002"
                        && srg.STAT == "002"
                        && gm.MNUS_MUID == menucmndtype.MUID
                     select new
                     {
                        Off_Percentage = g.OFF_PRCT
                     };

                  var _dfltgropamnt = _getdirectpaymentprocess.Where(a => a.Default == "002");
                  long? amnt = null;
                  string title = "", description = "", ussdcode = "";
                  int? tax = 0, off_prct = 0;
                  if (_dfltgropamnt.Any())
                  {
                     // اگر گروه دسترسی پیش فرض برای محاسبه هزینه وجود داشته باشد                     
                     amnt = _dfltgropamnt.Max(v => v.Price);
                     tax = _dfltgropamnt.FirstOrDefault(v => v.Price == amnt).Tax;
                     //off_prct = _dfltgropamnt.FirstOrDefault(v => v.Price == amnt).Off_Percentage;
                     title = _dfltgropamnt.FirstOrDefault(v => v.Price == amnt).Title;
                     description = _dfltgropamnt.FirstOrDefault(v => v.Price == amnt).Description;
                     ussdcode = _dfltgropamnt.FirstOrDefault(v => v.Price == amnt).Ussd_Code;
                  }
                  else
                  {
                     // اگر گروه دسترسی پیش فرض برای محاسبه هزینه وجود نداشته باشد
                     amnt = _getdirectpaymentprocess.Where(dpp => dpp.Auto_Join == "002").Max(v => v.Price);
                     tax = _getdirectpaymentprocess.FirstOrDefault(dpp => dpp.Auto_Join == "002" && dpp.Price == amnt).Tax;
                     // اگر در خروجی بدست آمده تخفیف گروه پیش فرض وحود داشته باشد
                     if (!_getdefaultgroupaccess.Any())
                        off_prct = _getdirectpaymentprocess.FirstOrDefault(dpp => dpp.Auto_Join == "002" && dpp.Price == amnt).Off_Percentage;
                     else
                        off_prct = _getdefaultgroupaccess.Max(dpp => dpp.Off_Percentage);

                     title = _getdirectpaymentprocess.FirstOrDefault(dpp => dpp.Auto_Join == "002" && dpp.Price == amnt).Title;
                     description = _getdirectpaymentprocess.FirstOrDefault(dpp => dpp.Auto_Join == "002" && dpp.Price == amnt).Description;
                     ussdcode = _getdirectpaymentprocess.FirstOrDefault(dpp => dpp.Auto_Join == "002" && dpp.Price == amnt).Ussd_Code;
                  }

                  #region Create Order with Order Type

                  // ⏳ Please wait...
                  var waitmesg = 
                     await Bot.SendTextMessageAsync(
                           e.Message.Chat.Id,
                           "⏳ لطفا چند لحظه صبر کنید...",
                           replyMarkup:
                           null);

                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", GetToken()),
                       new XElement("Message",
                          new XAttribute("ussd", chat.UssdCode ?? ""),
                          new XAttribute("childussd", menucmndtype != null ? menucmndtype.USSD_CODE ?? "" : ""),
                          new XAttribute("chatid", chat.Message.Chat.Id)
                       )
                    ), connectionString);

                  await Bot.DeleteMessageAsync(e.Message.Chat.Id, waitmesg.MessageId);

                  var ordr =
                     iRobotTech.Orders
                     .Where(
                        o => o.CHAT_ID == e.Message.Chat.Id
                          && o.Robot.TKON_CODE == GetToken()
                          && o.ORDR_STAT == "001"
                          && (o.ORDR_TYPE == "004" || o.ORDR_TYPE == "013" || o.ORDR_TYPE == "014" || o.ORDR_TYPE == "016")
                          && o.STRT_DATE.Value.Date == DateTime.Now.Date 
                     ).OrderByDescending(o => o.STRT_DATE).Take(1).FirstOrDefault();

                  // Process SendInvoice
                  var price = new List<LabeledPrice>();
                  /*if (off_prct > 0)
                     amnt -= (amnt * off_prct) / 100;*/

                  if (ordr.AMNT_TYPE == "001")
                  {
                     if (ordr.EXTR_PRCT != null && ordr.EXTR_PRCT > 0)
                     {
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                        price.Add(new LabeledPrice("ارزش افزوده", (int)ordr.EXTR_PRCT));
                     }
                     else
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));

                     // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                     if (ordr.TXFE_AMNT_DNRM != null && ordr.TXFE_AMNT_DNRM > 0)
                        price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordr.TXFE_AMNT_DNRM));
                  }
                  else if (ordr.AMNT_TYPE == "002")
                  {
                     if (ordr.EXTR_PRCT != null && ordr.EXTR_PRCT > 0)
                     {
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                        price.Add(new LabeledPrice("ارزش افزوده", (int)ordr.EXTR_PRCT * 10));
                     }
                     else
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));

                     // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                     if (ordr.TXFE_AMNT_DNRM != null && ordr.TXFE_AMNT_DNRM > 0)
                        price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordr.TXFE_AMNT_DNRM * 10));
                  }

                  await FireEventResultOpration(chat, keyBoardMarkup, xdata);

                  await Send_Order(iRobotTech, keyBoardMarkup);

                  if (ordr.AMNT_TYPE == "001")
                  {
                     // پرداخت به صورت کارت به کارت
                     if (price.Sum(p => p.Amount) <= 30000000)
                     {
                        await Bot.SendInvoiceAsync(
                           (int)e.Message.Chat.Id,
                           string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordr.CODE, "تاریخ" , iRobotTech.GET_MTOS_U(ordr.STRT_DATE)),
                           description,
                           ordr.CODE.ToString(),
                           ordr.DEST_CARD_NUMB_DNRM,
                           "",
                           "IRR",
                           price
                           );
                     }
                     else
                     {
                        // پرداخت از طریق درگاه پرداخت
                     }
                  }
                  else if(ordr.AMNT_TYPE == "002")
                  {
                     // پرداخت به صورت کارت به کارت
                     if (price.Sum(p => p.Amount) <= 3000000)
                     {
                        await Bot.SendInvoiceAsync(
                           (int)e.Message.Chat.Id,
                           string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordr.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordr.STRT_DATE)),
                           description,
                           ordr.CODE.ToString(),
                           ordr.DEST_CARD_NUMB_DNRM,
                           "",
                           "IRR",
                           price
                           );
                     }
                     else
                     {
                        // پرداخت از طریق درگاه پرداخت
                     }
                  }
                  #endregion
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "027")
               {
                  #region Order Checkout Payment Process
                  // ⏳ Please wait...
                  var waitmesg = 
                     await Bot.SendTextMessageAsync(
                           e.Message.Chat.Id,
                           "⏳ لطفا چند لحظه صبر کنید...",
                           replyMarkup:
                           null);

                  var xdata = RobotHandle.GetData(
                    new XElement("Robot",
                       new XAttribute("token", GetToken()),
                       new XElement("Message",
                          new XAttribute("ussd", chat.UssdCode ?? ""),
                          new XAttribute("childussd", menucmndtype != null ? menucmndtype.USSD_CODE ?? "" : ""),
                          new XAttribute("chatid", chat.Message.Chat.Id)
                       )
                    ), connectionString);

                  await Bot.DeleteMessageAsync(e.Message.Chat.Id, waitmesg.MessageId);

                  var ordr =
                     iRobotTech.Orders
                     .Where(
                        o => o.CHAT_ID == e.Message.Chat.Id
                          && o.Robot.TKON_CODE == GetToken()
                          && o.ORDR_STAT == "001"
                          && (o.ORDR_TYPE == "004" || o.ORDR_TYPE == "013" || o.ORDR_TYPE == "014" || o.ORDR_TYPE == "016")
                          && o.STRT_DATE.Value.Date == DateTime.Now.Date
                     ).OrderByDescending(o => o.STRT_DATE).Take(1).FirstOrDefault();

                  // Process SendInvoice
                  var price = new List<LabeledPrice>();
                  /*if (off_prct > 0)
                     amnt -= (amnt * off_prct) / 100;*/
                  if (ordr.AMNT_TYPE == "001")
                  {
                     if (ordr.EXTR_PRCT != null && ordr.EXTR_PRCT > 0)
                     {
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));
                        price.Add(new LabeledPrice("ارزش افزوده", (int)ordr.EXTR_PRCT));
                     }
                     else
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT));

                     // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                     if (ordr.TXFE_AMNT_DNRM != null && ordr.TXFE_AMNT_DNRM > 0)
                        price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordr.TXFE_AMNT_DNRM));
                  }
                  else if (ordr.AMNT_TYPE == "002")
                  {
                     if (ordr.EXTR_PRCT != null && ordr.EXTR_PRCT > 0)
                     {
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));
                        price.Add(new LabeledPrice("ارزش افزوده", (int)ordr.EXTR_PRCT * 10));
                     }
                     else
                        price.Add(new LabeledPrice("قیمت کل", (int)ordr.EXPN_AMNT * 10));

                     // اگر بخواهیم از مشتری کارمزد دریافت کنیم
                     if (ordr.TXFE_AMNT_DNRM != null && ordr.TXFE_AMNT_DNRM > 0)
                        price.Add(new LabeledPrice("کارمزد خدمات غیر حضوری", (int)ordr.TXFE_AMNT_DNRM * 10));
                  }                  

                  await FireEventResultOpration(chat, keyBoardMarkup, xdata);

                  await Send_Order(iRobotTech, keyBoardMarkup);

                  if (ordr.AMNT_TYPE == "001")
                  {
                     // پرداخت به صورت کارت به کارت
                     if (price.Sum(p => p.Amount) <= 30000000)
                     {
                        await Bot.SendInvoiceAsync(
                           (int)e.Message.Chat.Id,
                           string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordr.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordr.STRT_DATE)),
                           "سبد خرید شما",
                           ordr.CODE.ToString(),
                           ordr.DEST_CARD_NUMB_DNRM,
                           "",
                           "IRR",
                           price
                           );
                     }
                     else
                     {
                        // پرداخت از طریق درگاه پرداخت
                     }
                  }
                  else if (ordr.AMNT_TYPE == "002")
                  {
                     // پرداخت به صورت کارت به کارت
                     if (price.Sum(p => p.Amount) <= 3000000)
                     {
                        await Bot.SendInvoiceAsync(
                           (int)e.Message.Chat.Id,
                           string.Format("{0}\n\r{1} : {2}\n\r{3} : {4}", "فاکتور شما", "شماره", ordr.CODE, "تاریخ", iRobotTech.GET_MTOS_U(ordr.STRT_DATE)),
                           "سبد خرید شما",
                           ordr.CODE.ToString(),
                           ordr.DEST_CARD_NUMB_DNRM,
                           "",
                           "IRR",
                           price
                           );
                     }
                     else
                     {
                        // پرداخت از طریق درگاه پرداخت
                     }
                  }
                  #endregion
               }
               else if (menucmndtype.CMND_TYPE == "028")
               {
                  #region SubSystem Service
                  // ⏳ Please wait...
                  var waitmesg = 
                     await Bot.SendTextMessageAsync(
                           e.Message.Chat.Id,
                           "⏳ لطفا چند لحظه صبر کنید...",
                           replyMarkup:
                           null);

                  _Strt_Robo_F.SendRequest(
                     new Job(SendType.SelfToUserInterface, 1000 /* Execute Call_SystemService_F */ )
                     {
                        Input =
                           new XElement("Input",
                              new XAttribute("chatid", e.Message.Chat.Id),
                              new XAttribute("ussdcode", menucmndtype.USSD_CODE),
                              new XAttribute("subsystarget", menucmndtype.MNUS_DESC.Split(';')[0]),
                              new XAttribute("cmnd", menucmndtype.MNUS_DESC.Split(';')[1]),
                              new XAttribute("param", "")
                           ),
                        AfterChangedOutput = 
                           new Action<object>(
                              (output) =>
                                 {
                                    var xoutput = output as XElement;

                                    var resultcode = xoutput.Attribute("resultcode").Value.ToInt64();
                                    var resultdesc = xoutput.Attribute("resultdesc").Value;
                                    var mesgtype = (MessageType)xoutput.Attribute("mesgtype").Value.ToInt32();

                                    switch (mesgtype)
                                    {
                                       case MessageType.Text:
                                          Bot.SendTextMessageAsync(
                                             e.Message.Chat.Id,
                                             string.Format("👈 {0} ",
                                                resultdesc
                                             ),
                                             replyMarkup:
                                             null).Wait();
                                          break;
                                       
                                    }
                                 }
                              )
                     }
                  );

                  await Bot.DeleteMessageAsync(e.Message.Chat.Id, waitmesg.MessageId);
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
               ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, exc.Message, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text));
            else
               ConsoleOutLog_MemTxt.Text = string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, exc.Message, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
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
                        ConsoleOutLog_MemTxt.Text =
                        string.Format("Robot Id : {3} * {5}, DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n",
                        e.Message.Chat.Id,
                        e.Message.From.FirstName + ", " + e.Message.From.LastName,
                        e.Message.Text,
                        Me.Username,
                        DateTime.Now.ToString(),
                        e.Message.Chat.Username) + ConsoleOutLog_MemTxt.Text));
            else
               ConsoleOutLog_MemTxt.Text = string.Format("Robot Id : {3} , DateTime : {4} , Chat Id : {0}, From : {1}, Message Text : {2}\r\n", e.Message.Chat.Id, e.Message.From.FirstName + ", " + e.Message.From.LastName, e.Message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
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
            case MessageType.Audio:
               xret =
                  new XElement("AudioMessage",
                     new XAttribute("mimetype", e.Message.Audio.MimeType ?? ""),
                     new XAttribute("fileid", e.Message.Audio.FileId),
                     new XAttribute("performer", e.Message.Audio.Performer),
                     new XAttribute("titl", e.Message.Audio.Title),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.Contact:
               xret =
                  new XElement("ContactMessage",
                     new XAttribute("frstname", e.Message.Contact.FirstName ?? ""),
                     new XAttribute("lastname", e.Message.Contact.LastName ?? ""),
                     new XAttribute("phonnumb", e.Message.Contact.PhoneNumber)
                  );
               break;
            case MessageType.Document:
               xret =
                  new XElement("DocumentMessage",
                     new XAttribute("mimetype", e.Message.Document.MimeType ?? ""),
                     new XAttribute("fileid", e.Message.Document.FileId),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.Location:
               xret =
                  new XElement("LocationMessage",
                     new XAttribute("latitude", e.Message.Location.Latitude),
                     new XAttribute("longitude", e.Message.Location.Longitude),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.Photo:
               xret =
                  new XElement("PhotoMessage",
                     new XAttribute("caption", e.Message.Caption ?? ""),
                     new XAttribute("fileid", e.Message.Photo.Reverse().FirstOrDefault().FileId),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            //case MessageType.Service:
            //   if(e.Message.NewChatMembers != null)
            //      xret =
            //         new XElement("ServiceMessage",                       
            //            new XAttribute("joinleft", "join"),
            //            new XAttribute("frstname", e.Message.NewChatMember.FirstName ?? ""),
            //            new XAttribute("lastname", e.Message.NewChatMember.LastName ?? ""),
            //            new XAttribute("id", e.Message.NewChatMember.Id),
            //            new XAttribute("username", e.Message.NewChatMember.Username ?? "")
            //         );
            //   else
            //      xret =
            //         new XElement("ServiceMessage",
            //            new XAttribute("joinleft", "left"),
            //            new XAttribute("frstname", e.Message.LeftChatMember.FirstName ?? ""),
            //            new XAttribute("lastname", e.Message.LeftChatMember.LastName ?? ""),
            //            new XAttribute("id", e.Message.LeftChatMember.Id),
            //            new XAttribute("username", e.Message.LeftChatMember.Username ?? "")
            //         );
            //   break;
            case MessageType.Sticker:
               xret =
                  new XElement("StickerMessage",
                     new XAttribute("emoji", e.Message.Sticker.Emoji ?? ""),
                     new XAttribute("fileid", e.Message.Sticker.FileId)
                  );
               break;
            case MessageType.Text:
               xret =
                  new XElement("TextMessage",
                     new XAttribute("text", e.Message.Text ?? "")
                  );
               break;
            case MessageType.Unknown:
               break;
            case MessageType.Venue:
               break;
            case MessageType.Video:
               xret =
                  new XElement("VideoMessage",
                     new XAttribute("mimetype", e.Message.Video.MimeType ?? ""),
                     new XAttribute("fileid", e.Message.Video.FileId),
                     new XAttribute("caption", e.Message.Caption)
                  );
               break;
            case MessageType.Voice:
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
                              && r.TKON_CODE == GetToken()
                              && p.SHOW_STRT == "002"
                              && p.IMAG_DESC != null
                        orderby p.ORDR descending
                        select new { p.FILE_NAME, p.FILE_PATH, p.IMAG_DESC, p.FILE_ID, p.ORDR }).ToList();

            if (iRobotTech.Robots.Any(r => r.TKON_CODE == GetToken() && r.BULD_STAT != "006"))
            {
               try
               {
                  //string fileid = iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == GetToken()).BULD_FILE_ID;
                  ///***await Bot.SendPhotoAsync(chat.Message.Chat.Id, new FileToSend(iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == GetToken()).BULD_FILE_ID), "کاربر گرامی نرم افزار در حال آماده سازی می باشد و هنوز یه مرحله نهایی نرسیده. بعداز اتمام از همین سامانه به شما اطلاع رسانی می شود");
                  await Bot.SendPhotoAsync(chat.Message.Chat.Id, new InputOnlineFile(iRobotTech.Robots.FirstOrDefault(r => r.TKON_CODE == GetToken()).BULD_FILE_ID), "کاربر گرامی نرم افزار در حال آماده سازی می باشد و هنوز یه مرحله نهایی نرسیده. بعداز اتمام از همین سامانه به شما اطلاع رسانی می شود");
               }
               catch { }
            }

            foreach (var pic in pics.Take(1))
            {
               dynamic photo;
               if (string.IsNullOrEmpty(pic.FILE_ID))
               {
                  ///***photo = new FileToSend()
                  ///***{
                  ///***   Content = new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                  ///***   Filename = pic.FILE_NAME
                  ///***};
                  photo = new InputOnlineFile(new FileStream(pic.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), pic.FILE_NAME);
               }
               else
               {
                  ///***photo = new FileToSend(pic.FILE_ID);
                  photo = new InputOnlineFile(pic.FILE_ID);
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
                              && r.TKON_CODE == GetToken()
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
               ///***await Bot.GetFileAsync(fileid, System.IO.File.Create(destpath));
               Bale.Bot.Types.File file = await Bot.GetFileAsync(fileid);
               file.FilePath = destpath;
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
            var photos = await Bot.GetUserProfilePhotosAsync(userid, 0, 100);
            for (int i = 0; i < photos.TotalCount; i++)
            {
               var photo = photos.Photos[i].Reverse().FirstOrDefault();
               ///***await Bot.GetFileAsync(photo.FileId, System.IO.File.Create(destpath + "_" + i.ToString() +  ".jpg"));
               Bale.Bot.Types.File file = await Bot.GetFileAsync(photo.FileId);
               file.FilePath = destpath + "_" + i.ToString() + ".jpg";
            }
         }
         catch (Exception exc)
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
         public bool ForceReply { get; set; }
         public string Target { get; set; }
         public string UisUssd { get; set; }
         public string CommandText { get; set; }
         public string Params { get; set; }
         public string PostExecs { get; set; }
         public string Triggers { get; set; }
         public string TriggerStepRun { get; set; }
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
               ///***file = new FileToSend()
               ///***{
               ///***   Content = new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
               ///***   Filename = send.FILE_PATH
               ///***};
               file = new InputOnlineFile(new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), send.FILE_PATH);
            }
            else if (send.PAKT_TYPE != "001")
            {
               ///***file = new FileToSend(send.FILE_ID);
               file = new InputOnlineFile(send.FILE_ID);
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
                              ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text));
                           else
                              ConsoleOutLog_MemTxt.Text = string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
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
                              ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text));
                           else
                              ConsoleOutLog_MemTxt.Text = string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Failed\r\n", s.CHAT_ID, s.Service.FRST_NAME + ", " + s.Service.LAST_NAME, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
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
                              Bot.SendAudioAsync((long)s.CHAT_ID, file, send.TEXT_MESG ?? "No Audio Caption", 0, 0, send.TEXT_MESG, "#");
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
                     ///***photo = new FileToSend()
                     ///***{
                     ///***   Content = new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read),
                     ///***   Filename = send.FILE_PATH.Substring(send.FILE_PATH.LastIndexOf('\\') + 1)
                     ///***};
                     photo = new InputOnlineFile(new FileStream(send.FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read), send.FILE_PATH.Substring(send.FILE_PATH.LastIndexOf('\\') + 1));
                  }
                  else
                  {
                     ///***photo = new FileToSend(send.FILE_ID);
                     photo = new InputOnlineFile(send.FILE_ID);
                  }

                  if (send.MESG_TYPE == "002")
                     await Bot.SendPhotoAsync((long)send.CHAT_ID, photo, send.MESG_TEXT ?? "😊", ParseMode.Default, false, (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "003")
                     await Bot.SendVideoAsync((long)send.CHAT_ID, photo, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "004")
                     await Bot.SendDocumentAsync((long)send.CHAT_ID, photo, send.MESG_TEXT ?? "😊", replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "006")
                     await Bot.SendAudioAsync((long)send.CHAT_ID, photo, "*", 0, 0, "*", "#", replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "007")
                     await Bot.SendStickerAsync((long)send.CHAT_ID, photo, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
                  else if (send.MESG_TYPE == "009")
                     await Bot.SendVoiceAsync((long)send.CHAT_ID, photo, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
               }
               else if (send.MESG_TYPE == "005")
               {
                  await Bot.SendLocationAsync((long)send.CHAT_ID, (float)send.LAT, (float)send.LON, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
               }
               else if (send.MESG_TYPE == "008")
               {
                  await Bot.SendContactAsync((long)send.CHAT_ID, send.CONT_CELL_PHON, send.MESG_TEXT, replyToMessageId: (int)(send.SRMG_MESG_ID_DNRM ?? 0));
               }

               send.SEND_STAT = "004";
               if (ConsoleOutLog_MemTxt.InvokeRequired)
                  ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", send.CHAT_ID, send.Service_Robot.Service.FRST_NAME + ", " + send.Service_Robot.Service.LAST_NAME, Me.Username, DateTime.Now.ToString())));
               else
                  ConsoleOutLog_MemTxt.Text += string.Format("Robot Id : {2} , DateTime : {3} , Chat Id : {0}, From : {1} => Successful\r\n", send.CHAT_ID, send.Service_Robot.Service.FRST_NAME + ", " + send.Service_Robot.Service.LAST_NAME, Me.Username, DateTime.Now.ToString());
            }
            catch
            {
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
                           ///***await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + filename + ".jpg"));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(ordt.ORDR_DESC);
                           file.FilePath = fileupload + "\\" + filename + ".jpg";
                           ordt.IMAG_PATH = fileupload + "\\" + filename + ".jpg";
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "003")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Video.FileId*/ filename + "." + ordt.FILE_EXT));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(ordt.ORDR_DESC);
                           file.FilePath = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                           ordt.IMAG_PATH = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "004")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Document.FileId*/ filename+"." + ordt.FILE_EXT));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(ordt.ORDR_DESC);
                           file.FilePath = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                           ordt.IMAG_PATH = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "006")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Audio.FileId*/ filename + "." + ordt.FILE_EXT));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(ordt.ORDR_DESC);
                           file.FilePath = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                           ordt.IMAG_PATH = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
                        }
                        catch { }
                     }
                     else if (ordt.ELMN_TYPE == "009")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(ordt.ORDR_DESC, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Sticker.FileId*/ filename + "." + ordt.FILE_EXT));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(ordt.ORDR_DESC);
                           file.FilePath = fileupload + "\\" + filename + "." + ordt.FILE_EXT;
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
         foreach (var rsgm in iRobotTech.Robot_Spy_Group_Messages.Where(r => r.FILE_ID != null && r.FILE_PATH == null))
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
                           ///***await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + filename + ".jpg"));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(rsgm.FILE_ID);
                           file.FilePath = fileupload + "\\" + filename + ".jpg";
                           rsgm.FILE_PATH = fileupload + "\\" + filename + ".jpg";
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "003")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Video.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(rsgm.FILE_ID);
                           file.FilePath = fileupload + "\\" + filename;
                           rsgm.FILE_PATH = fileupload + "\\" + filename;
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "004")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Document.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(rsgm.FILE_ID);
                           file.FilePath = fileupload + "\\" + filename;
                           rsgm.FILE_PATH = fileupload + "\\" + filename;
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "006")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Audio.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(rsgm.FILE_ID);
                           file.FilePath = fileupload + "\\" + filename;
                           rsgm.FILE_PATH = fileupload + "\\" + filename;
                        }
                        catch { }
                     }
                     else if (rsgm.MESG_TYPE == "009")
                     {
                        try
                        {
                           ///***await Bot.GetFileAsync(rsgm.FILE_ID, System.IO.File.Create(fileupload + "\\" + /*chat.Message.Sticker.FileId*/ filename));
                           Bale.Bot.Types.File file = await Bot.GetFileAsync(rsgm.FILE_ID);
                           file.FilePath = fileupload + "\\" + filename;
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
            var ordts = iRobotTech.Order_Details.Where(o => o.ORDR_CODE == prjo.ORDR_CODE && o.SEND_STAT == "001").ToList();

            //prjo.ORDR_STAT = "002";
            iRobotTech.Set_Personal_Robot_Job_Order(
               new XElement("PJBO",
                  new XAttribute("prjbcode", prjo.PRJB_CODE),
                  new XAttribute("ordrcode", prjo.ORDR_CODE),
                  new XAttribute("ordrstat", "002")
               )
            );            

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
                  case "017": // واحد حسابداری
                  case "018": // واحد انبارداری
                  case "019": // واحد پیک و ارسال بسته
                     #region پیام های ارسالی
                     switch (ordt.ELMN_TYPE)
                     {
                        case "001":
                           if (ordt.INLN_KEYB_DNRM == null)
                              await Bot.SendTextMessageAsync(
                                 (int)prjo.PRBT_CHAT_ID,
                                 iRobotTech.CRET_PMSG_U(new XElement("Message",
                                    new XAttribute("prjbcode", prjo.PRJB_CODE),
                                    new XAttribute("ordrcode", prjo.ORDR_CODE),
                                    new XAttribute("ordtrwno", ordt.RWNO)
                                 )),
                                 replyMarkup:                              
                                 keyBoardMarkup == null ? null : 
                                 new ReplyKeyboardMarkup()
                                 {
                                    Keyboard = keyBoardMarkup,
                                    ResizeKeyboard = true,
                                    Selective = true
                                 });
                           else 
                              await Bot.SendTextMessageAsync(
                                 (int)prjo.PRBT_CHAT_ID,
                                 iRobotTech.CRET_PMSG_U(new XElement("Message",
                                    new XAttribute("prjbcode", prjo.PRJB_CODE),
                                    new XAttribute("ordrcode", prjo.ORDR_CODE),
                                    new XAttribute("ordtrwno", ordt.RWNO)
                                 )),
                                 replyMarkup:
                                 CreateInLineKeyboard(ordt.INLN_KEYB_DNRM.Descendants("InlineKeyboardButton").ToList(), 3)
                                 );
                              
                           break;
                        case "002":
                           if (ordt.INLN_KEYB_DNRM == null)
                              await Bot.SendPhotoAsync(
                                 (int)prjo.PRBT_CHAT_ID,
                                 ///***new FileToSend(ordt.ORDR_DESC),
                                 new InputOnlineFile(ordt.IMAG_PATH),
                                 //caption: ordt.ORDR_CMNT ?? "",
                                 caption:iRobotTech.CRET_PMSG_U(new XElement("Message",
                                       new XAttribute("prjbcode", prjo.PRJB_CODE),
                                       new XAttribute("ordrcode", prjo.ORDR_CODE),
                                       new XAttribute("ordtrwno", ordt.RWNO)
                                    )),
                                 replyMarkup:
                                 keyBoardMarkup == null ? null : 
                                 new ReplyKeyboardMarkup()
                                 {
                                    Keyboard = keyBoardMarkup,
                                    ResizeKeyboard = true,
                                    Selective = true
                                 });
                           else
                              await Bot.SendPhotoAsync(
                                 (int)prjo.PRBT_CHAT_ID,
                                 ///***new FileToSend(ordt.ORDR_DESC),
                                 new InputOnlineFile(ordt.IMAG_PATH),
                                 //caption: ordt.ORDR_CMNT ?? "",
                                 caption: iRobotTech.CRET_PMSG_U(new XElement("Message",
                                       new XAttribute("prjbcode", prjo.PRJB_CODE),
                                       new XAttribute("ordrcode", prjo.ORDR_CODE),
                                       new XAttribute("ordtrwno", ordt.RWNO)
                                    )),
                                 replyMarkup:
                                 CreateInLineKeyboard(ordt.INLN_KEYB_DNRM.Descendants("InlineKeyboardButton").ToList(), 3));
                           break;
                        case "003":
                           await Bot.SendVideoAsync(
                              (int)prjo.PRBT_CHAT_ID,
                              ///***new FileToSend(ordt.ORDR_DESC),
                              new InputOnlineFile(ordt.ORDR_DESC),
                              replyMarkup:
                              keyBoardMarkup == null ? null : 
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
                              ///***new FileToSend(ordt.ORDR_DESC),
                              new InputOnlineFile(ordt.ORDR_DESC),
                              replyMarkup:
                              keyBoardMarkup == null ? null : 
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
                              keyBoardMarkup == null ? null : 
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
         //iRobotTech.SubmitChanges();
      }
      //private async Task Send_Order(Data.iRoboTechDataContext iRobotTech, string command, string param)
      //{
      //   // بررسی اینکه آیا درخواستی برای پرسنلی ثبت شده یا خیر
      //   var prjos = iRobotTech.Personal_Robot_Job_Orders.Where(po => po.ORDR_STAT == "001");

      //   foreach (var prjo in prjos)
      //   {
      //      var ordts = iRobotTech.Order_Details.Where(o => o.ORDR_CODE == prjo.ORDR_CODE && o.SEND_STAT == "001").ToList();

      //      //prjo.ORDR_STAT = "002";
      //      iRobotTech.Set_Personal_Robot_Job_Order(
      //         new XElement("PJBO",
      //            new XAttribute("prjbcode", prjo.PRJB_CODE),
      //            new XAttribute("ordrcode", prjo.ORDR_CODE),
      //            new XAttribute("ordrstat", "002")
      //         )
      //      );

      //      foreach (var ordt in ordts)
      //      {
      //         // 1398/12/29 * در این قسمت باید مشخص کنیم که صفحه کلید هر درخواست به چه صورت می باشد
      //         InlineKeyboardMarkup inlineKeyboardMarkup = null;
      //         // فراخوانی تابع 
      //         // getdata
      //         // برای اینکه درخواست اگر نیاز به صفحه کلیدی دارد متن را برای مشتری ارسال کند
      //         var xmlmsg = RobotHandle.GetData(
      //            new XElement("Robot",
      //               new XAttribute("token", GetToken()),
      //               new XElement("Message",
      //                  new XAttribute("cbq", "002"),
      //                  new XAttribute("lacbq", "002"),
      //                  new XAttribute("ordrcode", ordt.Order.CODE),
      //                  new XElement("Text", 
      //                      new XAttribute("param", param),
      //                      command
      //                  )
      //               )
      //            ), connectionString);
               
      //         var query = XDocument.Parse(string.Format("<Message>{0}</Message>", xmlmsg.Element("Message").Value));
      //         //inlineKeyboardMarkup = CreateInlineKeyboardMarkup(query.Descendants("InlineKeyboardMarkup").First());
      //         inlineKeyboardMarkup = CreateInLineKeyboard(query.Descendants("InlineKeyboardMarkup").ToList(), 3);

      //         switch (ordt.Order.ORDR_TYPE)
      //         {
      //            case "001": // پیشنهادات                              
      //            case "002": // نظرسنجی
      //            case "003": // شکایات
      //            case "004": // سفارشات
      //            case "005": // Like
      //            case "006": // پرسش
      //            case "007": // پاسخ
      //            case "008": // تجربیات
      //            case "009": // Upload
      //            case "010": // معرفی
      //            case "011": // اخطار
      //            case "012": // اعلام ها
      //            case "017": // واحد حسابداری
      //            case "018": // واحد انبارداری
      //            case "019": // واحد پیک و ارسال بسته
      //               #region پیام های ارسالی
      //               switch (ordt.ELMN_TYPE)
      //               {
      //                  case "001":
      //                     await Bot.SendTextMessageAsync(
      //                        (int)prjo.PRBT_CHAT_ID,
      //                        iRobotTech.CRET_PMSG_U(new XElement("Message",
      //                           new XAttribute("prjbcode", prjo.PRJB_CODE),
      //                           new XAttribute("ordrcode", prjo.ORDR_CODE),
      //                           new XAttribute("ordtrwno", ordt.RWNO)
      //                        )),
      //                        replyMarkup:
      //                        inlineKeyboardMarkup);
      //                     break;
      //                  case "002":
      //                     await Bot.SendPhotoAsync(
      //                        (int)prjo.PRBT_CHAT_ID,
      //                        ///***new FileToSend(ordt.ORDR_DESC),
      //                        new InputOnlineFile(ordt.ORDR_DESC),
      //                        caption: ordt.ORDR_CMNT ?? "",
      //                        replyMarkup:
      //                        inlineKeyboardMarkup);
      //                     break;
      //                  case "003":
      //                     await Bot.SendVideoAsync(
      //                        (int)prjo.PRBT_CHAT_ID,
      //                        ///***new FileToSend(ordt.ORDR_DESC),
      //                        new InputOnlineFile(ordt.ORDR_DESC),
      //                        replyMarkup:
      //                        inlineKeyboardMarkup);
      //                     break;
      //                  case "004":
      //                     await Bot.SendDocumentAsync(
      //                        (int)prjo.PRBT_CHAT_ID,
      //                        ///***new FileToSend(ordt.ORDR_DESC),
      //                        new InputOnlineFile(ordt.ORDR_DESC),
      //                        replyMarkup:
      //                        inlineKeyboardMarkup);
      //                     break;
      //                  case "005":
      //                     float cordx = Convert.ToSingle(ordt.ORDR_DESC.Split(',')[0], System.Globalization.CultureInfo.InvariantCulture);
      //                     float cordy = Convert.ToSingle(ordt.ORDR_DESC.Split(',')[1], System.Globalization.CultureInfo.InvariantCulture);

      //                     await Bot.SendLocationAsync(
      //                        (long)prjo.PRBT_CHAT_ID,
      //                        Convert.ToSingle(ordt.ORDR_DESC.Split(',')[0], System.Globalization.CultureInfo.InvariantCulture),
      //                        Convert.ToSingle(ordt.ORDR_DESC.Split(',')[1], System.Globalization.CultureInfo.InvariantCulture),
      //                        replyMarkup:
      //                        inlineKeyboardMarkup);

      //                     break;
      //                  default:
      //                     break;
      //               }
      //               #endregion
      //               break;
      //         }
      //      }
      //   }
      //}
      public KeyboardButton[][] CreateKeyboardButton(List<XElement> list, int rows, int cols)
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
      public InlineKeyboardMarkup CreateInlineKeyboardMarkup(XElement xlist)
      {
         return 
            new InlineKeyboardMarkup(
               xlist.Descendants().Select(
                  l => InlineKeyboardButton.WithCallbackData(l.Value, l.Attribute("data").Value)
               ).ToList()
            );
      }
      public InlineKeyboardMarkup CreateInLineKeyboard(List<XElement> list, int columns)
      {
         int rows = (int)Math.Ceiling((double)list.Count / (double)columns);
         InlineKeyboardButton[][] buttons = new InlineKeyboardButton[rows][];

         for (int i = 0; i < buttons.Length; i++)
         {
            buttons[i] = list
                .Skip(i * columns)
                .Take(columns)
                .Select(direction => InlineKeyboardButton.WithCallbackData(direction.Value, direction.Attribute("data").Value))
                .ToArray();
         }
         return new InlineKeyboardMarkup(buttons);
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
               0, //chat.Message.MessageId,
               replyMarkup: keyBoardMarkup != null ?
               new ReplyKeyboardMarkup()
               {
                  Keyboard = keyBoardMarkup,
                  ResizeKeyboard = true,
                  Selective = true
               } : 
               null
            );
         }
         await Task.Yield();
      }
      private async Task FireEventResultOpration(ChatInfo chat, KeyboardButton[][] keyBoardMarkup, XElement xdata)
      {
         if (xdata.Elements("Message").Count() > 0)
         {            
            var query = XDocument.Parse(string.Format("<Message>{0}</Message>", xdata.Element("Message").Value));
            if (query.Element("Message").Element("Message") != null)
               query = new XDocument(query.Element("Message").LastNode);
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

                     try
                     {
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
                     }
                     catch { }

                     try
                     {
                        if (xinnerelement.Attribute("caption") != null)
                        {
                           await Bot.SendTextMessageAsync(
                             chat.Message.Chat.Id,
                             xinnerelement.Attribute("caption").Value,
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
                     }
                     catch { }
                  }
                  #endregion
               }
               else if (xelement.Name == "Images")
               {
                  #region Image
                  foreach (string innerorder in xelement.Elements().Attributes("order").OrderBy(o => o.Value))
                  {
                     var xinnerelement = xelement.Elements().Where(x => x.Attribute("order").Value == innerorder).First();

                     await Bot.SendPhotoAsync(chat.Message.Chat.Id, 
                        new InputOnlineFile(xinnerelement.Attribute("fileid").Value), 
                        xinnerelement.Attribute("caption").Value,
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
               else if (xelement.Name == "InlineKeyboardMarkup")
               {
                  #region Inline Keyboard
                  string caption = "";
                  if(xelement.Attribute("caption") != null)
                     caption = xelement.Attribute("caption").Value;
                  else
                     caption = "لطفا گزینه مورد نظر خود را انتخاب کنید";

                  try
                  {
                     await
                        Bot.SendTextMessageAsync(
                           chatId: chat.Message.Chat.Id,
                           text: caption,
                           replyMarkup: CreateInLineKeyboard(xelement.Descendants("InlineKeyboardButton").ToList(), 3)
                        );
                  }
                  catch { }

                  try
                  {
                     var chosmesg =
                        await Bot.SendTextMessageAsync(
                         chatId: chat.Message.Chat.Id,
                         text: "لطفا گزینه مورد نظر خود را انتخاب کنید",
                         replyMarkup: new ReplyKeyboardMarkup()
                         {
                            Keyboard = keyBoardMarkup,
                            ResizeKeyboard = true,
                            Selective = true
                         }
                     );

                     await Bot.DeleteMessageAsync(chat.Message.Chat.Id, chosmesg.MessageId);
                  }
                  catch
                  {
                     if (ConsoleOutLog_MemTxt.InvokeRequired)
                        ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, chat.Message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text));
                     else
                        ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, chat.Message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
                  }
                  #endregion
               }
               else if (xelement.Name == "Complex_InLineKeyboardMarkup")
               {
                  #region Complex_InLineKeyboardMarkup
                  string caption = "";
                  if (xelement.Attribute("caption") != null)
                     caption = xelement.Attribute("caption").Value;
                  else
                     caption = "لطفا گزینه مورد نظر خود را انتخاب کنید";

                  string fileid = "";
                  if (xelement.Attribute("fileid") != null)
                     fileid = xelement.Attribute("fileid").Value;                  

                  switch (xelement.Attribute("filetype").Value)
                  {
                     case "002":
                        try
                        {
                           await Bot.SendPhotoAsync(
                               chatId: chat.Message.Chat.Id,
                               photo: fileid,
                               caption: caption,
                               replyMarkup: CreateInLineKeyboard(xelement.Descendants("InlineKeyboardButton").ToList(), 3)
                           );
                        }
                        catch
                        {
                           if (ConsoleOutLog_MemTxt.InvokeRequired)
                              ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, chat.Message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text));
                           else
                              ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, chat.Message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
                        }
                        break;
                  }

                  try
                  {
                     var chosmesg =
                        await Bot.SendTextMessageAsync(
                         chatId: chat.Message.Chat.Id,
                         text: "لطفا گزینه مورد نظر خود را انتخاب کنید",
                         replyMarkup: new ReplyKeyboardMarkup()
                         {
                            Keyboard = keyBoardMarkup,
                            ResizeKeyboard = true,
                            Selective = true
                         }
                     );

                     await Bot.DeleteMessageAsync(chat.Message.Chat.Id, chosmesg.MessageId);
                  }
                  catch
                  {
                     if (ConsoleOutLog_MemTxt.InvokeRequired)
                        ConsoleOutLog_MemTxt.Invoke(new Action(() => ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, chat.Message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text));
                     else
                        ConsoleOutLog_MemTxt.Text = string.Format("{3} , {4} , {0}, {1}, {2}\r\n", chat.Message.Chat.Id, chat.Message.From.FirstName + ", " + chat.Message.From.LastName, chat.Message.Text, Me.Username, DateTime.Now.ToString()) + ConsoleOutLog_MemTxt.Text;
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
      private string GetToken()
      {
         string token = "";
         if(robot.COPY_TYPE == "002" && robot.ROBO_RBID != null)
         {
            token = robot.Robot1.TKON_CODE;
         }
         else
         {
            token = Token;
         }
         return token;
      }
   }
}
