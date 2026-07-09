using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.MessageBroadcast.Code
{
    public class LidomaSmsClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private string _token;
        private DateTime _tokenExpiry;

        public LidomaSmsClient(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var request = new { username, password };
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl + "/ws/v1/login", content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return false;

                var result = JObject.Parse(responseJson);
                var entriesToken = result["entries"];
                _token = entriesToken != null && entriesToken["token"] != null ? entriesToken["token"].ToString() : null;

                if (string.IsNullOrEmpty(_token))
                    return false;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                _tokenExpiry = DateTime.UtcNow.AddHours(23);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LidomaSmsClient.LoginAsync: " + ex.ToString());
                return false;
            }
        }

        public async Task<JObject> SendSingleAsync(string storeId, int branchIndex, string sender, string receptor, string message)
        {
            EnsureTokenValid();
            var request = new
            {
                storeId,
                branchIndex,
                type = "one2one",
                messages = new[] { new { receptor, message } },
                sender
            };
            return await SendRequestAsync(request);
        }

        public async Task<JObject> SendBatchAsync(string storeId, int branchIndex, string sender, string[] receptors, string message)
        {
            EnsureTokenValid();
            var request = new
            {
                storeId,
                branchIndex,
                type = "one2many",
                receptors,
                message,
                sender
            };
            return await SendRequestAsync(request);
        }

        private async Task<JObject> SendRequestAsync(object request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl + "/ws/v1/sms/send", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return JObject.FromObject(new { success = false, error = responseJson });
            }

            return JObject.Parse(responseJson);
        }

        public async Task<JObject> GetStatusAsync(string messageIds)
        {
            EnsureTokenValid();
            var response = await _httpClient.GetAsync(_baseUrl + "/ws/v1/sms/status?messageid=" + messageIds);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JObject.FromObject(new { success = false, error = responseJson });

            return JObject.Parse(responseJson);
        }

        public async Task<long> GetCreditAsync()
        {
            EnsureTokenValid();
            var response = await _httpClient.GetAsync(_baseUrl + "/ws/v1/account/charge");
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return 0;

            var result = JObject.Parse(responseJson);
            var entriesCharge = result["entries"];
            var charge = entriesCharge != null && entriesCharge["charge"] != null ? entriesCharge["charge"].ToString() : null;
            long credit;
            return long.TryParse(charge, out credit) ? credit : 0;
        }

        public async Task<string> GetServerStatusAsync()
        {
            EnsureTokenValid();
            var response = await _httpClient.GetAsync(_baseUrl + "/ws/v1/server/status");
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return "unknown";

            var result = JObject.Parse(responseJson);
            var entriesStatus = result["entries"];
            var statusVal = entriesStatus != null && entriesStatus["status"] != null ? entriesStatus["status"].ToString() : null;
            return statusVal ?? "unknown";
        }

        private void EnsureTokenValid()
        {
            if (string.IsNullOrEmpty(_token) || DateTime.UtcNow >= _tokenExpiry)
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                if (!string.IsNullOrEmpty(_token))
                {
                    _token = null;
                    _tokenExpiry = DateTime.MinValue;
                }
            }
        }

        public void Dispose()
        {
            if (_httpClient != null)
                _httpClient.Dispose();
        }
    }
}
