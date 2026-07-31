# Lidoma Sync — Memory & Context

## معماری کلی
WEBS_MESG_F.cs یک Windows Form در VS 2013 با C# 5.0 (بدون `?.` / `$""` / `nameof` / `using var`).
- از LINQ-to-SQL (DataContext) برای اتصال به دیتابیس iScsc و iProject استفاده می‌کند.
- از `SemaphoreSlim` برای قفل ارسال (یک عملیات در لحظه) استفاده می‌کند.
- API لیدوما: CreateStoreAsync, UpdateStoreAsync, CreateServiceAsync, CreateCustomersBulkAsync, LoginAsync, CheckHealthAsync.

## فایل‌های تغییر یافته
1. `System.MessageBroadcast/Ui/SmsApp/WEBS_MESG_F.cs` — متدهای اصلی همگام‌سازی
2. `System.MessageBroadcast/Data/iScsc.designer.cs` — انتیتی‌های Club, Fighter, Method, Category_Belt, Club_Method, Club_Method_Weekday, Regulation, D_ATYP
3. `C:\Users\Hadaegh\AppData\Local\Temp\opencode\add_ldma_code.sql` — اسکریپت SQL افزودن LDMA_CODE
4. `C:\Users\Hadaegh\AppData\Local\Temp\opencode\add_cascade_triggers.sql` — اسکریپت SQL تریگرهای cascade
5. `System.MessageBroadcast/Data/iScsc.dbml` + `.layout` — طراحی LINQ-to-SQL

## ستون‌های اضافه شده به SQL
- `LDMA_CODE VARCHAR(250) NULL` به تمام ۱۷۷ جدولی که ستون `LDMA_STAT` داشتند.
- تمام تریگرهای `CG$ASYNL_%` فعال: اضافه شدن `IF UPDATE(LDMA_CODE) RETURN` بعد از NESTLEVEL.

## Triggerهای کascade برای جداول وابسته (add_cascade_triggers.sql)
- `CG$CLB_UPD_CLUB_METHOD`: روی `Club_Method` — اگر ردیفی با `MTOD_STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
- `CG$CLB_UPD_CLBMT_WDAY`: روی `Club_Method_Weekday` — اگر ردیفی با `STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
- `CG$CLB_UPD_METHOD`: روی `Method` — اگر ردیفی با `SHOW_STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
- `CG$CLB_UPD_CTGY_BELT`: روی `Category_Belt` — اگر ردیفی با `SHOW_STAT='002' AND CTGY_STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
- `CG$CLB_UPD_FIGHTER`: روی `Fighter` — اگر مربی (FGPB_TYPE_DNRM='003', ACTV_TAG_DNRM='101') تغییر کند → `Club.LDMA_STAT='003'`
- همه این تریگرها فقط باشگاه‌هایی را آپدیت می‌کنند که LDMA_STATشون NULL، '001' یا '002' است (نه '003' که قبلاً به‌روزرسانی شده)

## گردش کار ارسال (RunSendAllAsync)
1. **Business** → `SendByTypeCoreAsync(Business)` → صف + `SyncClubsAsync()`
2. **HasAnyClubWithStoreIdAsync** → بررسی Club.LDMA_CODE != null
3. **Service** → `SendByTypeCoreAsync(Service)` → فقط صف (بدون متد همگام‌سازی مجزا)
4. **Customer** → `SendByTypeCoreAsync(Customer)` → صف + `SyncCustomersAsync()`

## متد SyncClubsAsync (WEBS_MESG_F.cs ~690)
- Local DataContext: `using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))`
- **فیلتر pending**: `(c.LDMA_STAT ?? "001") == "001" || c.LDMA_STAT == "003"`
- **Triggerهای DB**: اگر هر ردیف در جداول وابسته تغییر کند، Triggerهای `CG$CLB_UPD_*` به‌صورت خودکار `Club.LDMA_STAT='003'` می‌شود
- **Update vs Create**: اگر `c.LDMA_CODE` مقدار دارد → `UpdateStoreAsync(LDMA_CODE, storeData)` وگرنه `CreateStoreAsync(storeData)`
- **بعد از ارسال موفق**: `LDMA_STAT='002'` برای خود باشگاه + تمام ردیف‌های وابسته:
  - **اولین بار** (CreateStoreAsync): همه ردیف‌های وابسته به '002' (با شرایط JSON building)
  - **بارهای بعدی** (UpdateStoreAsync): فقط ردیف‌هایی که LDMA_STATشون '001' یا '003' است
  - Club_Method (CLUB_CODE=c.CODE, MTOD_STAT='002')
  - Club_Method_Weekday (از طریق Club_Method، STAT='002')
  - Fighter/مربی (CLUB_CODE_DNRM=c.CODE، FGPB_TYPE_DNRM='003'، ACTV_TAG_DNRM='101')
  - Method (از طریق Club_Method، SHOW_STAT='002'، MTOD_STAT='002')
  - Category_Belt (از طریق Method، SHOW_STAT='002'، CTGY_STAT='002')
- **ذخیره storeId** بعد ارسال: `c.LDMA_CODE = (string)res["entries"]["storeId"]`
- **services JSON section**:
  - `methods[]`: متدهای یکتا از Club_Method (MTOD_STAT='002') + Method (SHOW_STAT='002', MTOD_STAT='002')
  - `categories[]` درون هر method: Category_Belt (SHOW_STAT='002', CTGY_STAT='002')
  - `personnels[]`: مربیان (Fighter با FGPB_TYPE_DNRM='003', ACTV_TAG_DNRM='101')
  - `serviceMethods[]` درون هر personnel: Club_Method مربی با MTOD_STAT='002' + Method معتبر
  - `weekDays[]` درون هر serviceMethod: Club_Method_Weekday (STAT='002')
- اگر هیچ متد معتبری نداشته باشد → `continue` (رد کردن باشگاه)
- **فیلدهای storeData**: name, template, about, billingType="percentage", ownerPhone, ownerPassword, city, regulation, location, branches, contacts, modules, services

## متد SyncCustomersAsync (WEBS_MESG_F.cs ~997)
- ابتدا Clubهای دارای `LDMA_CODE != null && != ""` را می‌گیرد
- Fighterهایی که `CLUB_CODE_DNRM` در باشگاه‌های بالا داشته باشند
- **فیلتر pending**: `(c.LDMA_STAT ?? "001") == "001" || c.LDMA_STAT == "003"` (مهم: NULL معادل 001)
- **فیلتر داخل حلقه**: `FGPB_TYPE_DNRM='001'`, `ACTV_TAG_DNRM='101'`, `CONF_STAT='002'`, `CELL_PHON_DNRM` اجباری
- **بسته‌های ۵۰ تایی** ارسال می‌شوند
- **storeId** از `Club.LDMA_CODE` (متعلق به باشگاه Fighter اول در بسته)
- **پاسخ API**: `entries[].phone` و `entries[].userId` → مطابقت با `CELL_PHON_DNRM` و ذخیره `userId` به عنوان `Fighter.LDMA_CODE`
- اگر `entries` نباشد → fallback: همه fighters batch را LDMA_STAT='002' کن
- **قالب JSON مشتری**: fileNo, frstName, lastName, fathName, debtDnrm, dpstDnrm, confDate, sexType, brthDate, cellPhon, tellPhon, insrNumb, insrDate, fngrPrnt, orgnCode, servNo, natlCode, dadCellPhon, dadTellPhon, momCellPhon, momTellPhon, dpstAcntSlryBank, dpstAcntSlry

## قالب JSON درخواست مشتریان (با تطابق API)
- درخواست: `{ "storeId": "...", "customers": [ { fileNo, ..., dpstAcntSlry } ] }`
- پاسخ موفق: شامل `entries[{ phone, userId }]`
- کلید تطابق: `entry["phone"]` با `fighter.CELL_PHON_DNRM`

## نکات C# 5.0 / VS 2013
- هیچ‌جا از `?.` استفاده نکن → `(token != null) ? token.ToString() : null`
- هیچ‌جا از `$""` استفاده نکن → `String.Format(...)`
- هیچ‌جا از `nameof` استفاده نکن
- `string.IsNullOrEmpty` در LINQ-to-SQL خطا می‌دهد → بجای آن از `x != null && x != ""` استفاده کن
- همه `using` باید با `{ }` block باشند

## LDMA_STAT lifecycle
- `NULL` یا `"001"` = منتظر ارسال (نفرستاده شده)
- `"003"` = به‌روزرسانی (قبلاً فرستاده شده، حالا تغییر کرده)
- `"002"` = همگام‌سازی شده

## فیوزهای امنیتی
- `HasAnyClubWithStoreIdAsync()`: قبل از Services/Customers چک می‌کند حداقل یک Club LDMA_CODE داشته باشد
  - استفاده از `c.LDMA_CODE != null && c.LDMA_CODE != ""` به جای `string.IsNullOrEmpty` در LINQ-to-SQL (VS 2013)
- `RunSendAllAsync`: Business → چک `HasAnyClubWithStoreIdAsync` → Services → Customers
- `_sendLock.WaitAsync(0)`: فقط یک ارسال همزمان مجاز است
- `EnsureLoggedInAsync()`: قبل از هر ارسال لاگین را چک می‌کند
