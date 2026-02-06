using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace System.MessageBroadcast.Code
{
   public partial class Msgb
   {
      public Msgb(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         // Set Method For Timers
         _SenderBgwk = new Timer(){Interval = 1000};
         _CustBgwk = new Timer() { Interval = 1000};

         _SenderBgwk.Tick += _SenderBgwk_Tick;
         _CustBgwk.Tick += _CustBgwk_Tick;

         _SenderBgwk.Enabled = true;
         _CustBgwk.Enabled = true;
      }

      public class FarazSms
      {
         public FarazSms(string userName, string password)
         {
            UserName = userName;
            Password = password;
         }

         public string UserName { get; set; }
         public string Password { get; set; }

         public void SendSms(string message, string formLine, string[] rcpts)
         {
            WebRequest request = WebRequest.Create("http://ippanel.com/services.jspd");
            //string[] rcpts = new string[] { "989100000009" };
            string json = JsonConvert.SerializeObject(rcpts);
            request.Method = "POST";
            string postData = string.Format("op=send&uname={0}&pass={1}&message={2}&to=" + json + "&from={3}", UserName, Password, message, formLine);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            System.Diagnostics.Debug.WriteLine(responseFromServer);
		
         }

         public void SendVoice(string fileUrl, string formLine, string[] rcpts)
         {            
	         WebRequest request = WebRequest.Create("http://ippanel.com/services.jspd");
	         //string[] rcpts = new string[] { "989100000009" };
	         string json = JsonConvert.SerializeObject(rcpts);
	         request.Method = "POST";
	         //string fileurl = "http://www.YourURL.com/voice.wav";
	         string postData = string.Format("op=sendvoice&uname={0}&pass={1}&fileUrl="+fileUrl+"&to="+json+"&repeat=1", UserName, Password);
	         byte[] byteArray = Encoding.UTF8.GetBytes(postData);
	         request.ContentType = "application/x-www-form-urlencoded";
	         request.ContentLength = byteArray.Length;
	         Stream dataStream = request.GetRequestStream();
	         dataStream.Write(byteArray, 0, byteArray.Length);
	         dataStream.Close();
	         WebResponse response = request.GetResponse();
	         Console.WriteLine(((HttpWebResponse)response).StatusDescription);
	         dataStream = response.GetResponseStream();
	         StreamReader reader = new StreamReader(dataStream);
	         string responseFromServer = reader.ReadToEnd();
	         Console.WriteLine(responseFromServer);
	         reader.Close();
	         dataStream.Close();
	         response.Close();
	         System.Diagnostics.Debug.WriteLine(responseFromServer);		
         }

         public void SendSmsPointToPoint(string[] messages, string fromLine, string[] rcpts)
         {
            WebRequest request = WebRequest.Create("http://ippanel.com/services.jspd");
	         request.Method = "POST";
	         string postData = string.Format("op=send&uname={0}&pass={1}&message={2}&to={3}=&from={4}", UserName, Password, JsonConvert.SerializeObject(messages), JsonConvert.SerializeObject(rcpts), fromLine);
	         byte[] byteArray = Encoding.UTF8.GetBytes(postData);
	         request.ContentType = "application/x-www-form-urlencoded";
	         request.ContentLength = byteArray.Length;
	         Stream dataStream = request.GetRequestStream();
	         dataStream.Write(byteArray, 0, byteArray.Length);
	         dataStream.Close();
	         WebResponse response = request.GetResponse();
	         Console.WriteLine(((HttpWebResponse)response).StatusDescription);
	         dataStream = response.GetResponseStream();
	         StreamReader reader = new StreamReader(dataStream);
	         string responseFromServer = reader.ReadToEnd();
	         Console.WriteLine(responseFromServer);
	         reader.Close();
	         dataStream.Close();
	         response.Close();
	         System.Diagnostics.Debug.WriteLine(responseFromServer);
         }

         public void GetDelivery(string uniqid)
         {            
		      WebRequest request = WebRequest.Create("http://ippanel.com/services.jspd");
		      request.Method = "POST";
		      string postData = string.Format("op=delivery&uname={0}&pass={1}&uinqid={2}", UserName, Password, uniqid);
		      byte[] byteArray = Encoding.UTF8.GetBytes(postData);
		      request.ContentType = "application/x-www-form-urlencoded";
		      request.ContentLength = byteArray.Length;
		      Stream dataStream = request.GetRequestStream();
		      dataStream.Write(byteArray, 0, byteArray.Length);
		      dataStream.Close();
		      WebResponse response = request.GetResponse();
		      Console.WriteLine(((HttpWebResponse)response).StatusDescription);
		      dataStream = response.GetResponseStream();
		      StreamReader reader = new StreamReader(dataStream);
		      string responseFromServer = reader.ReadToEnd();
		      Console.WriteLine(responseFromServer);
		      reader.Close();
		      dataStream.Close();
		      response.Close();
		      System.Diagnostics.Debug.WriteLine(responseFromServer);		
         }

         public void BookList()
         {
            WebRequest request = WebRequest.Create("http://ippanel.com/services.jspd");
            request.Method = "POST";
            string postData = string.Format("op=booklist&uname={0}&pass={1}", UserName, Password);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            System.Diagnostics.Debug.WriteLine(responseFromServer);
         }

         public double GetCredit()
         {
            WebRequest request = WebRequest.Create("http://ippanel.com/services.jspd");
            request.Method = "POST";
            string postData = string.Format("op=credit&uname={0}&pass={1}", UserName, Password);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            System.Diagnostics.Debug.WriteLine(responseFromServer);
            return 0;
         }
      }

      /// <summary>
      /// Client for IPPanel Edge API (synchronous, HttpWebRequest, compatible with .NET 4.5)
      /// Based on public docs at https://edge.ippanel.com/v1 and https://ippanelcom.github.io/Edge-Document/docs/
      /// </summary>
      public class IPPanelEdgeClient
      {
         private readonly string _baseUrl = "https://edge.ippanel.com/v1";
         private readonly string _authTokenOrApiKey;
         private readonly JavaScriptSerializer _js = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };

         public IPPanelEdgeClient(string tokenOrApiKey)
         {
            if (string.IsNullOrEmpty(tokenOrApiKey)) throw new ArgumentNullException("tokenOrApiKey");
            _authTokenOrApiKey = tokenOrApiKey;
         }

         public IPPanelEdgeClient(string username, string password) {
            //var tokenOrApiKey = 
            var _jsonRslt = JObject.Parse(Login(username, password));
            if (_jsonRslt["meta"]["status"].Value<bool>() == true)
               _authTokenOrApiKey = _jsonRslt["data"]["token"].Value<string>();
            //if (string.IsNullOrEmpty(tokenOrApiKey)) throw new ArgumentNullException("tokenOrApiKey");
            //_authTokenOrApiKey = tokenOrApiKey;
         }

         #region Internal HTTP helpers
         private HttpWebRequest CreateRequest(string relativePath, string method)
         {
            string url = _baseUrl.TrimEnd('/') + relativePath;
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = method;
            req.ContentType = "application/json";
            req.Accept = "application/json";
            // According to docs, use Authorization header with token or API key
            req.Headers["Authorization"] = _authTokenOrApiKey;
            return req;
         }

         private string SendRequest(string relativePath, string method = "GET", object body = null)
         {
            var req = CreateRequest(relativePath, method);

            if (body != null)
            {
               string json = _js.Serialize(body);
               byte[] bytes = Encoding.UTF8.GetBytes(json);
               req.ContentLength = bytes.Length;
               using (var rs = req.GetRequestStream())
               {
                  rs.Write(bytes, 0, bytes.Length);
               }
            }

            try
            {
               using (var resp = (HttpWebResponse)req.GetResponse())
               using (var sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
               {
                  return sr.ReadToEnd();
               }
            }
            catch (WebException ex)
            {
               if (ex.Response != null)
               {
                  using (var er = (HttpWebResponse)ex.Response)
                  using (var sr = new StreamReader(er.GetResponseStream(), Encoding.UTF8))
                  {
                     string err = sr.ReadToEnd();
                     // بازگرداندن خطای سرور برای دیباگ
                     return err;
                  }
               }
               throw;
            }
         }
         #endregion

         #region Authentication (acl/auth)
         // POST /api/acl/auth/login
         public string Login(string username, string password)
         {
            var body = new { username = username, password = password };
            return SendRequest("/api/acl/auth/login", "POST", body);
         }

         // POST /api/acl/auth/send_sms_otp
         public string SendSmsOtp(string token)
         {
            var body = new { token = token };
            return SendRequest("/api/acl/auth/send_sms_otp", "POST", body);
         }

         // POST /api/acl/auth/confirm_otp
         public string ConfirmOtp(string token, string otp)
         {
            var body = new { token = token, otp = otp };
            return SendRequest("/api/acl/auth/confirm_otp", "POST", body);
         }

         // POST /api/acl/auth/forgot_password
         public string ForgotPassword(string username, string mobile)
         {
            var body = new { username = username, mobile = mobile };
            return SendRequest("/api/acl/auth/forgot_password", "POST", body);
         }

         // POST /api/acl/auth/change_password_by_token
         public string ChangePasswordByToken(string token, string newPassword)
         {
            var body = new { token = token, password = newPassword };
            return SendRequest("/api/acl/auth/change_password_by_token", "POST", body);
         }

         // POST /api/acl/auth/logout
         public string Logout()
         {
            return SendRequest("/api/acl/auth/logout", "POST", null);
         }

         // GET /api/acl/auth/check
         public string CheckToken()
         {
            return SendRequest("/api/acl/auth/check", "GET", null);
         }
         #endregion

         #region Send SMS (single endpoint /api/send with sending_type)
         // Generic: POST /api/send
         // sending_type varies: "webservice", "peer_to_peer", "peer_to_peer_file", "postalcode", "country", "job", "keyword", "keyword_phonebook", "phonebook", "pattern", "file", "votp", "url"
         public string SendWebserviceSms(string fromNumber, IEnumerable<string> recipients, string message, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "webservice",
               from_number = fromNumber,
               message = message,
               @params = new { recipients = recipients },
               send_time = sendTimeUtcString // format "YYYY-MM-DD HH:MM:SS" in UTC per docs (optional)
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendPeerToPeer(string fromNumber, IEnumerable<object> messages, string sendTimeUtcString = null)
         {
            // messages: list of { recipients: [..], message: "..." }
            var body = new
            {
               sending_type = "peer_to_peer",
               from_number = fromNumber,
               @params = messages,
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendPeerToPeerByFile(string fromNumber, string filePathOnServerOrFileId, string sendTimeUtcString = null)
         {
            // docs mention a file-based variant - adjust 'params' as per actual expected key in your account
            var body = new
            {
               sending_type = "peer_to_peer_file",
               from_number = fromNumber,
               @params = new { file = filePathOnServerOrFileId },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendPostalCodeSms(string fromNumber, string postalCode, string message, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "postalcode",
               from_number = fromNumber,
               message = message,
               @params = new { postal_code = postalCode },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendCountrySms(string fromNumber, string countryOrProvinceCode, string message, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "country",
               from_number = fromNumber,
               message = message,
               @params = new { country = countryOrProvinceCode },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendJobSms(string fromNumber, string jobCategory, string message, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "job",
               from_number = fromNumber,
               message = message,
               @params = new { job = jobCategory },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendKeywordSms(string fromNumber, string keyword, string message, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "keyword",
               from_number = fromNumber,
               message = message,
               @params = new { keyword = keyword },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendKeywordPhonebookSms(string fromNumber, string keyword, string phonebookIdOrName, string message, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "keyword_phonebook",
               from_number = fromNumber,
               message = message,
               @params = new { keyword = keyword, phonebook = phonebookIdOrName },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendPhonebookSms(string fromNumber, IEnumerable<string> phonebookIds, string message, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "phonebook",
               from_number = fromNumber,
               message = message,
               @params = new { phonebooks = phonebookIds },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendPatternSms(string fromNumber, string patternCode, string recipient, Dictionary<string, object> values, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "pattern",
               from_number = fromNumber,
               @params = new
               {
                  code = patternCode,
                  recipient = recipient,
                  values = values
               },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         public string SendFileSms(string fromNumber, string fileIdOrPath, string sendTimeUtcString = null)
         {
            var body = new
            {
               sending_type = "file",
               from_number = fromNumber,
               @params = new { file = fileIdOrPath },
               send_time = sendTimeUtcString
            };
            return SendRequest("/api/send", "POST", body);
         }

         // VOTP (Voice OTP) - docs mention VOTP endpoint
         public string SendVotp(string fromNumber, string recipient, string message)
         {
            var body = new
            {
               sending_type = "votp",
               from_number = fromNumber,
               message = message,
               @params = new { recipients = new string[] { recipient } }
            };
            return SendRequest("/api/send", "POST", body);
         }

         // URL SMS: send by providing a URL
         public string SendSmsWithUrl(string fromNumber, string urlToFetchRecipientsOrContent)
         {
            var body = new
            {
               sending_type = "url",
               from_number = fromNumber,
               @params = new { url = urlToFetchRecipientsOrContent }
            };
            return SendRequest("/api/send", "POST", body);
         }

         // Cancel scheduled message - endpoint documented
         // docs mention Cancel Scheduled Message under send section
         public string CancelScheduledMessage(long messageOutboxId)
         {
            var body = new { messages_outbox_id = messageOutboxId };
            return SendRequest("/api/send/cancel", "POST", body);
         }

         // Calculate price: /api/send/price (docs mention "Calculate Price")
         public string CalculateSmsPrice(object calcRequest)
         {
            // calcRequest should match docs: includes numbers, message etc.
            return SendRequest("/api/send/price", "POST", calcRequest);
         }
         #endregion

         #region Reports
         // POST /api/report/new_list (Outbox Report)
         public string GetOutboxReport(int page = 1, int limit = 10, object filters = null)
         {
            var body = new { page = page, limit = limit, filters = filters ?? new { } };
            return SendRequest("/api/report/new_list", "POST", body);
         }

         // GET /api/report/by_bulk?messages_outbox_id={messages_outbox_id}
         public string GetOutboxReportById(string messagesOutboxId)
         {
            return SendRequest("/api/report/by_bulk?messages_outbox_id=" + Uri.EscapeDataString(messagesOutboxId), "GET", null);
         }

         // GET /api/report/bulk_stats?messages_outbox_id={id}
         public string GetBulkStats(string messagesOutboxId)
         {
            return SendRequest("/api/report/bulk_stats?messages_outbox_id=" + Uri.EscapeDataString(messagesOutboxId), "GET", null);
         }

         // GET /api/report/bulk_recipients?messages_outbox_id={id}&page={page}&limit={limit}
         public string GetBulkRecipients(string messagesOutboxId, int page = 1, int limit = 100)
         {
            string q = string.Format("/api/report/bulk_recipients?messages_outbox_id={0}&page={1}&limit={2}",
                Uri.EscapeDataString(messagesOutboxId), page, limit);
            return SendRequest(q, "GET", null);
         }

         // POST /api/report/inbox_list (Inbox Report) - docs show "Get Inbox Report"
         public string GetInboxReport(int page = 1, int limit = 50, object filters = null)
         {
            var body = new { page = page, limit = limit, filters = filters ?? new { } };
            return SendRequest("/api/report/inbox_list", "POST", body);
         }
         #endregion

         #region Numbers (assign/unassign/list)
         // POST /api/numbers/assign
         public string AssignNumber(object assignPayload)
         {
            // payload per docs: number_id, target_user_id OR number, target_user OR target_user_id etc.
            return SendRequest("/api/numbers/assign", "POST", assignPayload);
         }

         // POST /api/numbers/unassign
         public string UnassignNumber(object unassignPayload)
         {
            // payload: number_id OR number and target_user_id or target_user
            return SendRequest("/api/numbers/unassign", "POST", unassignPayload);
         }

         // GET /api/numbers/list
         public string ListNumbers()
         {
            return SendRequest("/api/numbers/list", "GET", null);
         }
         #endregion

         #region Users
         // GET /api/users (list)
         public string GetUsers(int page = 1, int limit = 50)
         {
            return SendRequest(string.Format("/api/users?page={0}&limit={1}", page, limit), "GET", null);
         }

         // GET /api/users/{userId}
         public string GetUser(string userId)
         {
            return SendRequest("/api/users/" + Uri.EscapeDataString(userId), "GET", null);
         }

         // POST /api/users/create
         public string CreateUser(object createPayload)
         {
            return SendRequest("/api/users/create", "POST", createPayload);
         }

         // PUT /api/users/{userId}/edit
         public string UpdateUser(string userId, object updatePayload)
         {
            return SendRequest("/api/users/" + Uri.EscapeDataString(userId) + "/edit", "PUT", updatePayload);
         }

         // DELETE /api/users/{userId}/delete
         public string DeleteUser(string userId)
         {
            return SendRequest("/api/users/" + Uri.EscapeDataString(userId) + "/delete", "DELETE", null);
         }
         #endregion

         #region Packages / Credit
         // GET /api/packages/credit  (or /api/credit)
         // Docs reference "packages/credit" earlier; provide both helpers.
         public string GetCredit()
         {
            // try packages/credit first
            string r = SendRequest("/api/packages/credit/mine", "GET", null);
            // If the remote returns an error because endpoint differs, user can call GetCreditAlternate
            return r;
         }

         public string GetCreditAlternate()
         {
            return SendRequest("/api/credit", "GET", null);
         }

         // POST /api/payment/purchase (purchase / charge)
         public string PurchaseCredit(object purchasePayload)
         {
            return SendRequest("/api/payment/purchase", "POST", purchasePayload);
         }
         #endregion

         #region Phonebook (contacts)
         // GET /api/phonebook (list)
         public string GetPhonebooks()
         {
            return SendRequest("/api/phonebook", "GET", null);
         }

         // POST /api/phonebook/create
         public string CreatePhonebook(object payload)
         {
            return SendRequest("/api/phonebook/create", "POST", payload);
         }

         // PUT /api/phonebook/{id}/edit
         public string UpdatePhonebook(string id, object payload)
         {
            return SendRequest("/api/phonebook/" + Uri.EscapeDataString(id) + "/edit", "PUT", payload);
         }

         // DELETE /api/phonebook/{id}/delete
         public string DeletePhonebook(string id)
         {
            return SendRequest("/api/phonebook/" + Uri.EscapeDataString(id) + "/delete", "DELETE", null);
         }

         // phonebook contacts endpoints - examples:
         public string AddPhonebookContact(string phonebookId, object contactPayload)
         {
            return SendRequest("/api/phonebook/" + Uri.EscapeDataString(phonebookId) + "/contact/create", "POST", contactPayload);
         }

         public string GetPhonebookContacts(string phonebookId, int page = 1, int limit = 100)
         {
            return SendRequest("/api/phonebook/" + Uri.EscapeDataString(phonebookId) + "/contacts?page=" + page + "&limit=" + limit, "GET", null);
         }
         #endregion

         #region Ticket (support)
         // POST /api/ticket/create
         public string CreateTicket(object payload)
         {
            return SendRequest("/api/ticket/create", "POST", payload);
         }

         // GET /api/ticket (list)
         public string GetTickets(int page = 1, int limit = 50)
         {
            return SendRequest("/api/ticket?page=" + page + "&limit=" + limit, "GET", null);
         }

         // POST /api/ticket/{id}/close
         public string CloseTicket(string ticketId)
         {
            return SendRequest("/api/ticket/" + Uri.EscapeDataString(ticketId) + "/close", "POST", null);
         }
         #endregion

         #region Drafts
         // POST /api/drafts/create
         public string CreateDraft(object payload)
         {
            return SendRequest("/api/drafts/create", "POST", payload);
         }

         // GET /api/drafts
         public string GetDrafts(int page = 1, int limit = 50)
         {
            return SendRequest("/api/drafts?page=" + page + "&limit=" + limit, "GET", null);
         }

         // PUT /api/drafts/{id}/edit
         public string EditDraft(string draftId, object payload)
         {
            return SendRequest("/api/drafts/" + Uri.EscapeDataString(draftId) + "/edit", "PUT", payload);
         }

         // DELETE /api/drafts/{id}/delete
         public string DeleteDraft(string draftId)
         {
            return SendRequest("/api/drafts/" + Uri.EscapeDataString(draftId) + "/delete", "DELETE", null);
         }
         #endregion

         #region Patterns (templates)
         // POST /api/patterns/create
         public string CreatePattern(object payload)
         {
            return SendRequest("/api/patterns/create", "POST", payload);
         }

         // GET /api/patterns
         public string GetPatterns(int page = 1, int limit = 50)
         {
            return SendRequest("/api/patterns?page=" + page + "&limit=" + limit, "GET", null);
         }

         // PUT /api/patterns/{code}/edit
         public string EditPattern(string patternCode, object payload)
         {
            return SendRequest("/api/patterns/" + Uri.EscapeDataString(patternCode) + "/edit", "PUT", payload);
         }

         // DELETE /api/patterns/{code}/delete
         public string DeletePattern(string patternCode)
         {
            return SendRequest("/api/patterns/" + Uri.EscapeDataString(patternCode) + "/delete", "DELETE", null);
         }
         #endregion

         #region Payment (info)
         // GET /api/payment/info
         public string GetPaymentInfo()
         {
            return SendRequest("/api/payment/info", "GET", null);
         }

         // GET /api/payment/invoices  (if exists)
         public string GetInvoices(int page = 1, int limit = 50)
         {
            return SendRequest("/api/payment/invoices?page=" + page + "&limit=" + limit, "GET", null);
         }
         #endregion

         #region Misc Helpers
         /// <summary>
         /// Helper to deserialize to a dynamic-like dictionary
         /// </summary>
         public Dictionary<string, object> DeserializeJsonToDict(string json)
         {
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
               var obj = _js.DeserializeObject(json) as Dictionary<string, object>;
               return obj;
            }
            catch { return null; }
         }
         #endregion
      }


   }
}
