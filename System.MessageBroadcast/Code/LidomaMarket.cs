// ========== COMPLETED - DO NOT MODIFY WITHOUT REVIEW ==========
// STABLE VERSION 1.0 - 2026-08-07
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.MessageBroadcast.Code
{
    /// <summary>
    /// Lidoma Market API client for SYNC operations.
    /// Inherits from LidomaApiClientBase for shared functionality.
    /// </summary>
    public class LidomaMarket : LidomaApiClientBase
    {
        // ============================================================
        // Singleton Pattern
        // ============================================================
        private static readonly Lazy<LidomaMarket> _lazyInstance =
            new Lazy<LidomaMarket>(() => new LidomaMarket());

        /// <summary>
        /// Default constructor - uses URL from app.config
        /// </summary>
        private LidomaMarket()
            : base(60)
        {
        }

        /// <summary>
        /// Parameterized constructor (for backward compatibility)
        /// </summary>
        public LidomaMarket(string baseUrl)
            : base(baseUrl, 60)
        {
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static LidomaMarket Instance
        {
            get { return _lazyInstance.Value; }
        }

        // ============================================================
        // Cache Configuration
        // ============================================================
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private const int CACHE_DURATION_MINUTES = 5;

        /// <summary>
        /// Cache key prefix for store-related data
        /// </summary>
        private const string CACHE_PREFIX = "LidomaMarket.";

        /// <summary>
        /// Cache key prefix for static status data
        /// </summary>
        private const string CACHE_KEY_SERVER_STATUS = CACHE_PREFIX + "ServerStatus";
        private const string CACHE_KEY_ACCOUNT_STATUS = CACHE_PREFIX + "AccountStatus";
        private const string CACHE_KEY_ACCOUNT_CHARGE = CACHE_PREFIX + "AccountCharge";
        private const string CACHE_KEY_STORE_PREFIX = CACHE_PREFIX + "Store_";

        /// <summary>
        /// Clears all cached data for this client.
        /// </summary>
        public void ClearCache()
        {
            var keysToRemove = new List<string>();
            foreach (var item in _cache)
            {
                if (item.Key.StartsWith(CACHE_PREFIX))
                    keysToRemove.Add(item.Key);
            }
            foreach (var key in keysToRemove)
                _cache.Remove(key);

            Logger.LogInfo("Cache cleared for LidomaMarket");
        }

        // ============================================================
        // 1. Authentication
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
                Logger.LogError("LidomaMarket.LoginAsync: ", ex);
                return false;
            }
        }

        // ============================================================
        // 2. SMS Methods
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
            return await PostAsync("/ws/v1/sms/send", request, cancellationToken);
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
            return await PostAsync("/ws/v1/sms/send", request, cancellationToken);
        }

        // ============================================================
        // 3. Cached Status Methods (5-minute cache)
        // ============================================================

        public async Task<JObject> GetMessageStatusAsync(string messageIds, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/sms/status?messageid=" + messageIds, cancellationToken);
        }

        /// <summary>
        /// Gets server status with 5-minute caching.
        /// </summary>
        public async Task<JObject> GetServerStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();

            // Check cache first
            var cached = _cache.Get(CACHE_KEY_SERVER_STATUS) as JObject;
            if (cached != null)
            {
                Logger.LogInfo("Returning cached server status");
                return cached;
            }

            var result = await GetAsync("/ws/v1/server/status", cancellationToken);

            if (result != null)
            {
                _cache.Set(CACHE_KEY_SERVER_STATUS, result, DateTimeOffset.Now.AddMinutes(CACHE_DURATION_MINUTES));
            }

            return result;
        }

        /// <summary>
        /// Gets account status with 5-minute caching.
        /// </summary>
        public async Task<JObject> GetAccountStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();

            var cached = _cache.Get(CACHE_KEY_ACCOUNT_STATUS) as JObject;
            if (cached != null)
            {
                Logger.LogInfo("Returning cached account status");
                return cached;
            }

            var result = await GetAsync("/ws/v1/account/status", cancellationToken);

            if (result != null)
            {
                _cache.Set(CACHE_KEY_ACCOUNT_STATUS, result, DateTimeOffset.Now.AddMinutes(CACHE_DURATION_MINUTES));
            }

            return result;
        }

        /// <summary>
        /// Gets account charge with 5-minute caching.
        /// </summary>
        public async Task<JObject> GetAccountChargeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();

            var cached = _cache.Get(CACHE_KEY_ACCOUNT_CHARGE) as JObject;
            if (cached != null)
            {
                Logger.LogInfo("Returning cached account charge");
                return cached;
            }

            var result = await GetAsync("/ws/v1/account/charge", cancellationToken);

            if (result != null)
            {
                _cache.Set(CACHE_KEY_ACCOUNT_CHARGE, result, DateTimeOffset.Now.AddMinutes(CACHE_DURATION_MINUTES));
            }

            return result;
        }

        // ============================================================
        // 4. Store Management (Platform account only)
        // ============================================================

        public async Task<JObject> GetStoresAsync(int page = 1, int limit = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await GetAsync(string.Format("/ws/v1/stores?page={0}&limit={1}", page, limit), cancellationToken);
        }

        /// <summary>
        /// Gets a specific store with 5-minute caching.
        /// </summary>
        public async Task<JObject> GetStoreAsync(string storeSlug, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();

            var cacheKey = CACHE_KEY_STORE_PREFIX + storeSlug;
            var cached = _cache.Get(cacheKey) as JObject;
            if (cached != null)
            {
                Logger.LogInfo("Returning cached store: " + storeSlug);
                return cached;
            }

            var result = await GetAsync("/ws/v1/stores/" + storeSlug, cancellationToken);

            if (result != null)
            {
                _cache.Set(cacheKey, result, DateTimeOffset.Now.AddMinutes(CACHE_DURATION_MINUTES));
            }

            return result;
        }

        public async Task<JObject> CreateStoreAsync(object storeData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/stores", storeData, cancellationToken);
        }

        public async Task<JObject> CreateStoresBulkAsync(object bulkData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/stores/bulk", bulkData, cancellationToken);
        }

        public async Task<JObject> UpdateStoreAsync(string storeSlug, object updateData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PatchAsync("/ws/v1/stores/" + storeSlug, updateData, cancellationToken);
        }

        public async Task<JObject> DeleteStoreAsync(string storeSlug, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await DeleteAsync("/ws/v1/stores/" + storeSlug, cancellationToken);
        }

        // ============================================================
        // 5. Customer Management
        // ============================================================

        public async Task<JObject> CreateCustomerAsync(object customerData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/customers", customerData, cancellationToken);
        }

        public async Task<JObject> CreateCustomersBulkAsync(object bulkData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/customers/bulk", bulkData, cancellationToken);
        }

        // ============================================================
        // 6. Service Management (endpoint فرضی - در صورت تغییر مسیر، اینجا اصلاح شود)
        // ============================================================

        public async Task<JObject> CreateServiceAsync(object serviceData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/services", serviceData, cancellationToken);
        }

        // ============================================================
        // 7. Revenue Management
        // ============================================================

        public async Task<JObject> SetStoreRevenuesAsync(string storeId, object revenuesData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PutAsync("/ws/v1/stores/" + storeId + "/revenues", revenuesData, cancellationToken);
        }

        public async Task<JObject> GetStoreRevenuesAsync(string storeSlug, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/stores/" + storeSlug + "/revenues", cancellationToken);
        }

        // ============================================================
        // 8. Services & Organs Management
        // ============================================================

        public async Task<JObject> GetStoreServicesAsync(string storeSlug, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/stores/" + storeSlug + "/services", cancellationToken);
        }

        // FIX: Changed PostAsync to PutAsync to match Postman specification
        public async Task<JObject> SetStoreServicesAsync(string storeSlug, object servicesData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PutAsync("/ws/v1/stores/" + storeSlug + "/services", servicesData, cancellationToken);
        }

        // FIX: Added missing GetStoreOrgans method
        public async Task<JObject> GetStoreOrgansAsync(string storeId, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await GetAsync("/ws/v1/stores/" + storeId + "/organs", cancellationToken);
        }

        // FIX: Added missing SetStoreOrgans method
        public async Task<JObject> SetStoreOrgansAsync(string storeId, object organsData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PutAsync("/ws/v1/stores/" + storeId + "/organs", organsData, cancellationToken);
        }

        public async Task<JObject> UpdateCustomerAsync(string customerId, object customerData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            // FIX: Changed PatchAsync to PutAsync to match Postman specification
            return await PutAsync("/ws/v1/customers/" + customerId, customerData, cancellationToken);
        }

        // ============================================================
        // 9. Bulk Services Management
        // ============================================================

        public async Task<JObject> CreateServicesBulkAsync(object bulkData, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureTokenValid();
            return await PostAsync("/ws/v1/services/bulk", bulkData, cancellationToken);
        }
    }
}
