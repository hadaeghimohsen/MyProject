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
   }
}
