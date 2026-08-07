# STABLE VERSION 1.0 - 2026-08-07

## COMPLETED FEATURES

### 1. SMS Sending System
- **Bulk Send**: Business, Service, Customer, Expense, Organ queue items
- **Single Send**: Manual SMS send with configurable provider (SmsCall, iNoti, IPPanel, Lidoma)
- **SemaphoreSlim** lock (`_sendLock`): Only one send operation at a time
- **Queue persistence**: Queue saved/loaded from file (`_queueFile`)

### 2. Lidoma Market API Client (LidomaMarket.cs)
- **Singleton** pattern via `Lazy<T>`
- **MemoryCache** (5-minute cache) for server status, account status, account charge, store data
- **Authentication**: LoginAsync with token caching (23-hour expiry)
- **Store Management**: CreateStoreAsync, UpdateStoreAsync, CreateStoresBulkAsync, GetStoresAsync, GetStoreAsync, DeleteStoreAsync
- **Customer Management**: CreateCustomerAsync, CreateCustomersBulkAsync, UpdateCustomerAsync
- **Service Management**: CreateServiceAsync, CreateServicesBulkAsync
- **Organ Management**: SetStoreOrgansAsync (PUT), GetStoreOrgansAsync (GET)
- **Revenue Management**: SetStoreRevenuesAsync, GetStoreRevenuesAsync
- **SMS Methods**: SendSingleAsync, SendBatchAsync
- **Status Methods**: GetMessageStatusAsync

### 3. Lidoma SMS Client (LidomaSmsClient.cs)
- **Singleton** pattern via `Lazy<T>`
- **Authentication**: LoginAsync with token caching
- **SMS Sending**: SendSingleAsync, SendBatchAsync
- **Credit Check**: GetCreditAsync

### 4. Base API Client (LidomaApiClientBase.cs)
- **Retry Logic**: 3 attempts, exponential backoff (1s, 2s, 4s)
- **Retry Triggers**: TimeoutException, HttpRequestException, HTTP 500-599
- **No Retry**: HTTP 400, 401, 403, 404 (handled specifically)
- **HTTP 401**: Token refresh logic
- **HTTP 429**: Respects Retry-After header
- **HTTP Helpers**: GetAsync, PostAsync, PutAsync, PatchAsync, DeleteAsync
- **IDisposable**: Properly disposes HttpClient

### 5. Logging System (Logger.cs)
- **Thread-safe** with lock object
- **File-based** logging to ApplicationData/LidomaSync
- **Log Rotation**: Max 5MB per file, 5 archived files
- **Levels**: INFO, WARN, ERROR, DEBUG
- **Config**: Enable/disable via AppSettings

### 6. Database Sync (WEBS_MESG_F.cs)
- **SyncClubsAsync**: Business sync to Lidoma Market API
  - Filters: `LDMA_STAT` NULL/"001" (pending) or "003" (updated)
  - Success: Sets LDMA_STAT="002", creates LDMA_CODE
  - Uses `CreateStoreAsync` or `UpdateStoreAsync` based on LDMA_CODE
- **SyncServicesAsync**: Service sync (queue only, no direct sync)
- **SyncCustomersAsync**: Customer sync in batches of 50
  - Filters: `LDMA_CODE != null`, `FGPB_TYPE_DNRM='001'`, `ACTV_TAG_DNRM='101'`, `CONF_STAT='002'`, `CELL_PHON_DNRM` required
- **SyncExpensesAsync**: Expense sync (incomplete - marked TODO)
- **SyncOrgansAsync**: Organ sync with status tracking
  - Filters: `LDMA_STAT == "003"` (pending)
  - Success: Sets `LDMA_STAT="002"` for Sub_Unit, Basic_Calculate_Discount (subscriptions + product sales)
  - Failure: Sets `LDMA_STAT="004"` for same records
  - SubmitChanges persists all updates

### 7. Query Methods (WEBS_MESG_F.cs)
- **GetOrgansFromDatabaseAsync**: LINQ to SQL query on Sub_Unit, filters LDMA_STAT="003"
- **GetSubscriptionDiscountsFromDatabaseAsync**: LINQ to SQL on Basic_Calculate_Discount (Rqtp_Code "001"/"009"), filters LDMA_STAT="003"
- **GetProductSalesDiscountsFromDatabaseAsync**: LINQ to SQL on Basic_Calculate_Discount (Rqtp_Code "016"), filters LDMA_STAT="003" + Expense.EXPN_STAT="002"
- **GetDiscountKind**: Helper for ACTN_TYPE CASE translation
- **HasAnyClubWithStoreIdAsync**: Checks if any Club has LDMA_CODE

### 8. Master Page (MSTR_PAGE_F.cs)
- **Timer-based polling**: `_pollTimer` (3s), `_creditTimer` (configurable)
- **SMS provider support**: SmsCall, iNoti, FarazSms, IPPanel, Lidoma
- **Thread-safe UI updates**: SafeInvoke helper
- **Network detection**: Ping check before credit queries

### 9. SMS Sending (Msgb.Properties.cs)
- **Single SMS**: Multi-provider support with type-specific formatting
- **Bulk SMS**: Background timer with configurable intervals
- **Fixed**: ObjectDisposedException from deferred LINQ query in async callback

## CONFIGURATION SETTINGS (App.config)

### Connection Strings
- `iProjectConnectionString`: `(localdb)\v11.0` / `iProject`
- `iScscConnectionString`: `(localdb)\v11.0` / `iScsc`
- `LidomaDB`: `.\sqlexpress` / `iScsc`

### App Settings
| Key | Value | Purpose |
|---|---|---|
| `LidomaApiBaseUrl` | `https://api.lidomamarket.ir` | Lidoma API endpoint |
| `LidomaLoggingEnabled` | `true` | Enable/disable logging |
| `LidomaCacheDurationMinutes` | `5` | API cache duration |
| `LidomaMaxRetryAttempts` | `3` | HTTP retry attempts |
| `ServerStatusIntervalMinutes` | `5` | Server status check timer |
| `AccountStatusIntervalMinutes` | `10` | Account status check timer |
| `CreditCheckIntervalMinutes` | `5` | Credit check timer |

## FILES

| File | Path | Purpose |
|---|---|---|
| WEBS_MESG_F.cs | `System.MessageBroadcast/Ui/SmsApp/` | Main form, all sync methods |
| LidomaMarket.cs | `System.MessageBroadcast/Code/` | Lidoma Market API client (Singleton) |
| LidomaSmsClient.cs | `System.MessageBroadcast/Code/` | Lidoma SMS client (Singleton) |
| LidomaApiClientBase.cs | `System.MessageBroadcast/Code/` | Base API client (retry, HTTP helpers) |
| Logger.cs | `System.MessageBroadcast/Code/` | File-based logging system |
| Msgb.Properties.cs | `System.MessageBroadcast/Code/` | SMS sending logic (bulk + single) |
| MSTR_PAGE_F.cs | `System.MessageBroadcast/Ui/MasterPage/` | Master page with timers |
| MSTR_PAGE_F.RequestRespons.cs | `System.MessageBroadcast/Ui/MasterPage/` | Job routing requests |
| App.config | `System.MessageBroadcast/` | Configuration |

## LDMA_STAT Lifecycle
| Value | Meaning |
|---|---|
| `NULL` or `"001"` | Pending (not yet sent) |
| `"002"` | Completed (synced successfully) |
| `"003"` | Updated (changed, needs re-sync) |
| `"004"` | Failed (sync error, retry possible) |

## GUIDELINES FOR FUTURE UPDATES

### DO NOT CHANGE (Core Infrastructure)
- Singleton pattern implementation (`Lazy<T>`) in LidomaMarket.cs and LidomaSmsClient.cs
- Retry logic in LidomaApiClientBase.cs (3 attempts, exponential backoff 1s/2s/4s)
- Cache logic (MemoryCache with 5-minute duration)
- Logging system (Logger.cs - file rotation, thread safety)
- Base class HTTP methods (GetAsync, PostAsync, PutAsync, PatchAsync, DeleteAsync)
- HTTP status code handling (401 token refresh, 429 rate limit, 404 return null)
- Token refresh logic (23-hour expiry)
- SemaphoreSlim send lock pattern
- DataContext using-block pattern with SubmitChanges
- LDMA_STAT lifecycle values ("001"/"002"/"003"/"004")

### CAN BE EXTENDED (Additive Changes Only)
- Add new methods to LidomaMarket.cs (new API endpoints)
- Add new sync methods to WEBS_MESG_F.cs (new QueueItemType values)
- Add new configuration keys to App.config (under `<appSettings>`)
- Add new query methods to WEBS_MESG_F.cs (new database queries)
- Add new SMS provider types to Msgb.Properties.cs

### DO NOT TOUCH
- All existing method signatures
- Database update logic pattern (query entities → modify → SubmitChanges)
- UI controls and designer files (.Designer.cs)
- Connection string names in App.config
- The `using var` pattern is NOT available - use traditional `using` blocks

### C# 5.0 COMPATIBILITY RULES
- Use `String.Format()` instead of `$""`
- Use `x != null ? x : null` instead of `?.`
- Use `string.IsNullOrEmpty` for regular strings (but NOT in LINQ-to-SQL queries)
- Use `x != null && x != ""` instead of `string.IsNullOrEmpty` in LINQ-to-SQL Where clauses
- All async methods must use `await` (no `.Result` or `.GetAwaiter().GetResult()`)
- Use explicit types in `var` declarations when the type is not obvious

## FILE STATUS

| File | Status |
|---|---|
| WEBS_MESG_F.cs | COMPLETED |
| LidomaMarket.cs | COMPLETED |
| LidomaSmsClient.cs | COMPLETED |
| LidomaApiClientBase.cs | COMPLETED |
| Logger.cs | COMPLETED |
| Msgb.Properties.cs | COMPLETED |
| MSTR_PAGE_F.cs | COMPLETED |
| MSTR_PAGE_F.RequestRespons.cs | COMPLETED |
| App.config | COMPLETED |
| frmStoreOrgans.cs | NOT FOUND - does not exist in project |
