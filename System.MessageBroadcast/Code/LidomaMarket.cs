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
    public class LidomaMarket : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private string _token;
        private DateTime _tokenExpiry;

        public LidomaMarket(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        }

        // ============================================================
        // 1. Authentication
        // ============================================================

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
                System.Diagnostics.Debug.WriteLine("LidomaMarket.LoginAsync: " + ex.ToString());
                return false;
            }
        }

        // ============================================================
        // 2. SMS Methods
        // ============================================================

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
            return await PostAsync("/ws/v1/sms/send", request);
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
            return await PostAsync("/ws/v1/sms/send", request);
        }

        public async Task<JObject> GetMessageStatusAsync(string messageIds)
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/sms/status?messageid=" + messageIds);
        }

        public async Task<JObject> GetServerStatusAsync()
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/server/status");
        }

        public async Task<JObject> GetAccountStatusAsync()
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/account/status");
        }

        public async Task<JObject> GetAccountChargeAsync()
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/account/charge");
        }

        // ============================================================
        // 3. Store Management (Platform account only)
        // ============================================================

        public async Task<JObject> GetStoresAsync(int page = 1, int limit = 20)
        {
            EnsureTokenValid();
            return await GetAsync(string.Format("/ws/v1/stores?page={0}&limit={1}", page, limit));
        }

        public async Task<JObject> GetStoreAsync(string storeSlug)
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/stores/" + storeSlug);
        }

        public async Task<JObject> CreateStoreAsync(object storeData)
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/stores", storeData);
        }

        public async Task<JObject> CreateStoresBulkAsync(object bulkData)
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/stores/bulk", bulkData);
        }

        public async Task<JObject> UpdateStoreAsync(string storeSlug, object updateData)
        {
            EnsureTokenValid();
            return await PatchAsync("/ws/v1/stores/" + storeSlug, updateData);
        }

        public async Task<JObject> DeleteStoreAsync(string storeSlug)
        {
            EnsureTokenValid();
            return await DeleteAsync("/ws/v1/stores/" + storeSlug);
        }

        // ============================================================
        // 4. Customer Management
        // ============================================================

        public async Task<JObject> CreateCustomerAsync(object customerData)
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/customers", customerData);
        }

        public async Task<JObject> CreateCustomersBulkAsync(object bulkData)
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/customers/bulk", bulkData);
        }

        // ============================================================
        // Private Helpers
        // ============================================================

        private async Task<JObject> GetAsync(string path)
        {
            var response = await _httpClient.GetAsync(_baseUrl + path);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JObject.FromObject(new { success = false, error = responseJson });

            return JObject.Parse(responseJson);
        }

        private async Task<JObject> PostAsync(string path, object requestBody)
        {
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl + path, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JObject.FromObject(new { success = false, error = responseJson });

            return JObject.Parse(responseJson);
        }

        private async Task<JObject> PatchAsync(string path, object requestBody)
        {
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), _baseUrl + path) { Content = content };
            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JObject.FromObject(new { success = false, error = responseJson });

            return JObject.Parse(responseJson);
        }

        private async Task<JObject> DeleteAsync(string path)
        {
            var response = await _httpClient.DeleteAsync(_baseUrl + path);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JObject.FromObject(new { success = false, error = responseJson });

            return JObject.Parse(responseJson);
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
