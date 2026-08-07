// ========== COMPLETED - DO NOT MODIFY WITHOUT REVIEW ==========
// STABLE VERSION 1.0 - 2026-08-07
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.MessageBroadcast.Code
{
    /// <summary>
    /// Lidoma Market API client for SMS sending operations.
    /// Inherits from LidomaApiClientBase for shared functionality.
    /// Only keeps SMS-related methods; all other methods removed as dead code.
    /// </summary>
    public class LidomaSmsClient : LidomaApiClientBase
    {
        // ============================================================
        // Singleton Pattern
        // ============================================================
        private static readonly Lazy<LidomaSmsClient> _lazyInstance =
            new Lazy<LidomaSmsClient>(() => new LidomaSmsClient());

        /// <summary>
        /// Default constructor - uses URL from app.config
        /// </summary>
        private LidomaSmsClient()
            : base(30)
        {
        }

        /// <summary>
        /// Parameterized constructor (for backward compatibility)
        /// </summary>
        public LidomaSmsClient(string baseUrl)
            : base(baseUrl, 30)
        {
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static LidomaSmsClient Instance
        {
            get { return _lazyInstance.Value; }
        }

        // ============================================================
        // Authentication
        // ============================================================

        public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var request = new { username, password };
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await ExecuteWithRetryAsync(() => _httpClient.PostAsync(_baseUrl + "/ws/v1/login", content), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var handlerResult = await HandleErrorResponse(response, _baseUrl + "/ws/v1/login", responseJson);
                    if (handlerResult.ShouldReturnNull || handlerResult.ShouldRefreshToken)
                        return false;
                    return false;
                }

                var result = JObject.Parse(responseJson);
                var entriesToken = result["entries"];
                if (entriesToken != null && entriesToken["token"] != null)
                {
                    SetToken(entriesToken["token"].ToString());
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError("LidomaSmsClient.LoginAsync: ", ex);
                return false;
            }
        }

        // ============================================================
        // SMS Methods
        // ============================================================

        public async Task<JObject> SendSingleAsync(string storeId, int branchIndex, string sender, string receptor, string message, CancellationToken cancellationToken = default(CancellationToken))
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
            return await SendRequestAsync(request, cancellationToken);
        }

        public async Task<JObject> SendBatchAsync(string storeId, int branchIndex, string sender, string[] receptors, string message, CancellationToken cancellationToken = default(CancellationToken))
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
            return await SendRequestAsync(request, cancellationToken);
        }

        private async Task<JObject> SendRequestAsync(object request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = _baseUrl + "/ws/v1/sms/send";
            Logger.LogInfo("POST " + url);

            try
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.PostAsync(url, content, cancellationToken), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                Logger.LogInfo("POST " + url + " - Status: " + response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var handlerResult = await HandleErrorResponse(response, url, responseJson);
                    if (handlerResult.ShouldReturnNull)
                        return null;
                    if (handlerResult.ShouldRetry)
                        throw new HttpRequestException("Retryable error: " + response.StatusCode);
                    return JObject.FromObject(new { success = false, error = responseJson });
                }

                return JObject.Parse(responseJson);
            }
            catch (Exception ex)
            {
                Logger.LogError("POST " + url + " failed", ex);
                throw;
            }
        }

        // ============================================================
        // Account Info
        // ============================================================

        public async Task<long> GetCreditAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            var url = _baseUrl + "/ws/v1/account/charge";
            Logger.LogInfo("GET " + url);

            try
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.GetAsync(url, cancellationToken), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                Logger.LogInfo("GET " + url + " - Status: " + response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var handlerResult = await HandleErrorResponse(response, url, responseJson);
                    if (handlerResult.ShouldReturnNull)
                        return 0;
                    if (handlerResult.ShouldRetry)
                        throw new HttpRequestException("Retryable error: " + response.StatusCode);
                    return 0;
                }

                var result = JObject.Parse(responseJson);
                var entriesCharge = result["entries"];
                var charge = entriesCharge != null && entriesCharge["remaincredit"] != null ? entriesCharge["remaincredit"].ToString() : null;
                long credit;
                return long.TryParse(charge, out credit) ? credit : 0;
            }
            catch (Exception ex)
            {
                Logger.LogError("GET " + url + " failed", ex);
                throw;
            }
        }

        // ============================================================
        // Dead Code Removed (use LidomaMarket class for these methods):
        //   - GetStatusAsync (use LidomaMarket.GetMessageStatusAsync instead)
        //   - GetServerStatusAsync (use LidomaMarket.GetServerStatusAsync instead)
        //   - GetStoreServicesAsync (use LidomaMarket.GetStoreServicesAsync instead)
        //   - SetStoreServicesAsync (use LidomaMarket.SetStoreServicesAsync instead)
        //   - GetStoreRevenuesAsync (use LidomaMarket.GetStoreRevenuesAsync instead)
        //   - SetStoreRevenuesAsync (use LidomaMarket.SetStoreRevenuesAsync instead)
        //   - GetStoreOrgansAsync (use LidomaMarket.GetStoreOrgansAsync instead)
        //   - SetStoreOrgansAsync (use LidomaMarket.SetStoreOrgansAsync instead)
        //   - UpdateCustomerAsync (use LidomaMarket.UpdateCustomerAsync instead)
        // ============================================================
    }
}
