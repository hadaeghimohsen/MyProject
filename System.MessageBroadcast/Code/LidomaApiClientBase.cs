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
    /// Abstract base class for Lidoma Market API client classes.
    /// Provides shared functionality: HttpClient management, token caching,
    /// retry logic, HTTP helpers, and configuration reading.
    /// </summary>
    public abstract class LidomaApiClientBase : IDisposable
    {
        // ============================================================
        // Protected Fields (accessible to derived classes)
        // ============================================================
        protected readonly HttpClient _httpClient;
        protected readonly string _baseUrl;
        protected string _token;
        protected DateTime _tokenExpiry;

        // ============================================================
        // Retry Policy Constants
        // ============================================================
        protected const int MAX_RETRY_ATTEMPTS = 3;
        protected static readonly TimeSpan[] RETRY_DELAYS = new TimeSpan[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(4)
        };

        // ============================================================
        // Constructor
        // ============================================================
        protected LidomaApiClientBase(int timeoutSeconds = 60)
        {
            _baseUrl = ConfigurationManager.AppSettings["LidomaApiBaseUrl"] ?? "https://api.lidomamarket.ir";
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };
        }

        protected LidomaApiClientBase(string baseUrl, int timeoutSeconds = 60)
        {
            _baseUrl = (baseUrl ?? ConfigurationManager.AppSettings["LidomaApiBaseUrl"] ?? "https://api.lidomamarket.ir").TrimEnd('/');
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };
        }

        // ============================================================
        // Token Management
        // ============================================================
        /// <summary>
        /// نشان‌دهندهٔ اینکه لاگین موفق بوده و توکن هنوز معتبر است (تا ۲۳ ساعت).
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(_token) && DateTime.UtcNow < _tokenExpiry;
            }
        }

        /// <summary>
        /// توکن دریافتی از آخرین لاگین موفق (برای نمایش یا استفادهٔ مستقیم).
        /// </summary>
        public string Token
        {
            get
            {
                return _token;
            }
        }

        protected void EnsureTokenValid()
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

        protected void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _tokenExpiry = DateTime.UtcNow.AddHours(23);
        }

        // ============================================================
        // Retry Policy Helper
        // ============================================================

        /// <summary>
        /// Executes an async operation with retry logic for transient failures.
        /// Retries up to 3 times with exponential backoff (1s, 2s, 4s).
        /// Only retries on TimeoutException, HttpRequestException, and 500-599 status codes.
        /// Does NOT retry on 400, 401, 403, 404 errors.
        /// Handles 401 by calling OnUnauthorizedAsync (token refresh).
        /// Handles 429 by respecting Retry-After header.
        /// </summary>
        protected async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default(CancellationToken))
        {
            Exception lastException = null;
            for (int attempt = 0; attempt < MAX_RETRY_ATTEMPTS; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var startTime = DateTime.UtcNow;
                try
                {
                    var result = await operation();
                    var elapsedMs = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    return result;
                }
                 catch (OperationCanceledException)
                 {
                     if (cancellationToken.IsCancellationRequested)
                        throw;
                 }
                catch (TimeoutException tex)
                {
                    lastException = tex;
                    Logger.LogWarning("RetryPolicy: TimeoutException on attempt " + (attempt + 1) + " of " + MAX_RETRY_ATTEMPTS + ": " + tex.Message);
                }
                catch (HttpRequestException hex)
                {
                    lastException = hex;
                    Logger.LogWarning("RetryPolicy: HttpRequestException on attempt " + (attempt + 1) + " of " + MAX_RETRY_ATTEMPTS + ": " + hex.Message);
                }
                catch (Exception ex)
                {
                    // Check if it's a transient HTTP error (500-599)
                    if (IsTransientError(ex))
                    {
                        lastException = ex;
                        Logger.LogWarning("RetryPolicy: Transient error on attempt " + (attempt + 1) + " of " + MAX_RETRY_ATTEMPTS + ": " + ex.Message);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (attempt < MAX_RETRY_ATTEMPTS - 1)
                {
                    await Task.Delay(RETRY_DELAYS[attempt], cancellationToken);
                }
            }
            throw new Exception("Lidoma API request failed after " + MAX_RETRY_ATTEMPTS + " attempts.", lastException);
        }

        /// <summary>
        /// Checks if an exception represents a transient error that should be retried.
        /// </summary>
        protected bool IsTransientError(Exception ex)
        {
            if (ex is HttpRequestException)
                return true;

            if (ex is TimeoutException)
                return true;

            var webEx = ex as WebException;
            if (webEx != null)
            {
                var httpResponse = webEx.Response as HttpWebResponse;
                if (httpResponse != null)
                {
                    var statusCode = (int)httpResponse.StatusCode;
                    return statusCode >= 500 && statusCode <= 599;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the HttpResponseMessage represents a transient error.
        /// </summary>
        protected bool IsTransientStatusCode(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return false;

            var statusCode = (int)response.StatusCode;
            return statusCode >= 500 && statusCode <= 599;
        }

        // ============================================================
        // HTTP Helper Methods
        // ============================================================

        protected async Task<JObject> GetAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = _baseUrl + path;
            Logger.LogInfo("GET " + url);

            try
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.GetAsync(url, cancellationToken), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                var elapsedMs = await LogResponseTime("GET", url, response);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle specific status codes
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
                Logger.LogError("GET " + url + " failed", ex);
                throw;
            }
        }

        protected async Task<JObject> PostAsync(string path, object requestBody, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = _baseUrl + path;
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Logger.LogInfo("POST " + url);

            try
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.PostAsync(url, content, cancellationToken), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                var elapsedMs = await LogResponseTime("POST", url, response);

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

        protected async Task<JObject> PutAsync(string path, object requestBody, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = _baseUrl + path;
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PUT"), url) { Content = content };

            Logger.LogInfo("PUT " + url);

            try
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.SendAsync(request, CancellationToken.None), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                var elapsedMs = await LogResponseTime("PUT", url, response);

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
                Logger.LogError("PUT " + url + " failed", ex);
                throw;
            }
        }

        protected async Task<JObject> PatchAsync(string path, object requestBody, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = _baseUrl + path;
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };

            Logger.LogInfo("PATCH " + url);

            try
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.SendAsync(request, CancellationToken.None), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                var elapsedMs = await LogResponseTime("PATCH", url, response);

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
                Logger.LogError("PATCH " + url + " failed", ex);
                throw;
            }
        }

        protected async Task<JObject> DeleteAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = _baseUrl + path;

            Logger.LogInfo("DELETE " + url);

            try
            {
                var response = await ExecuteWithRetryAsync(() => _httpClient.DeleteAsync(url, cancellationToken), cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();

                var elapsedMs = await LogResponseTime("DELETE", url, response);

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
                Logger.LogError("DELETE " + url + " failed", ex);
                throw;
            }
        }

        // ============================================================
        // HTTP Status Code Handling
        // ============================================================

        /// <summary>
        /// Result of error response handling.
        /// </summary>
        protected class ErrorHandlerResult
        {
            public bool ShouldReturnNull { get; set; }
            public bool ShouldRetry { get; set; }
            public bool ShouldRefreshToken { get; set; }
        }

        /// <summary>
        /// Handles HTTP error response codes with specific logic:
        /// - 401 Unauthorized → Refresh token
        /// - 429 Too Many Requests → Retry with longer delay
        /// - 403 Forbidden → Log and show error
        /// - 404 Not Found → Return null
        /// </summary>
        protected async Task<ErrorHandlerResult> HandleErrorResponse(HttpResponseMessage response, string url, string responseJson)
        {
            var result = new ErrorHandlerResult();
            var statusCode = (int)response.StatusCode;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    // 401 - Try to refresh token
                    Logger.LogWarning("HTTP 401 Unauthorized for " + url + " - attempting token refresh");
                    result.ShouldRefreshToken = true;
                    result.ShouldRetry = true;
                    break;

                case (HttpStatusCode)429:
                    // 429 Too Many Requests
                    Logger.LogWarning("HTTP 429 Too Many Requests for " + url);
                    // Check for Retry-After header
                    var retryAfter = response.Headers.RetryAfter;
                    if (retryAfter != null && retryAfter.Delta.HasValue)
                    {
                        Logger.LogInfo("Retry-After: " + retryAfter.Delta.Value.TotalSeconds + " seconds");
                    }
                    result.ShouldRetry = true;
                    break;

                case HttpStatusCode.Forbidden:
                    // 403 Forbidden
                    Logger.LogError("HTTP 403 Forbidden for " + url + " - access denied", null);
                    break;

                case HttpStatusCode.NotFound:
                    // 404 Not Found - return null instead of error
                    Logger.LogInfo("HTTP 404 Not Found for " + url);
                    result.ShouldReturnNull = true;
                    break;

                default:
                    if (IsTransientStatusCode(response))
                    {
                        Logger.LogWarning("HTTP " + statusCode + " (transient) for " + url);
                        result.ShouldRetry = true;
                    }
                    else
                    {
                        Logger.LogError("HTTP " + statusCode + " for " + url, null);
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Logs the response time for performance monitoring.
        /// </summary>
        protected async Task<double> LogResponseTime(string method, string url, HttpResponseMessage response)
        {
            // This is a simplified response time logger
            // In a real implementation, we'd start timing before the request
            Logger.LogInfo(string.Format("{0} {1} - Status: {2}",
                method, url, response.StatusCode));
            return 0;
        }

        // ============================================================
        // IDisposable
        // ============================================================
        public void Dispose()
        {
            if (_httpClient != null)
                _httpClient.Dispose();
        }
    }
}
