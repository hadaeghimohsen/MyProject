# Lidoma Sync — Memory & Context
# STABLE VERSION 1.0 - 2026-08-07 - All core systems COMPLETED and reviewed

## معماری کلی
WEBS_MESG_F.cs یک Windows Form در VS 2013 با C# 5.0 (بدون `?.` / `$""` / `nameof` / `using var`).
- از LINQ-to-SQL (DataContext) برای اتصال به دیتابیس iScsc و iProject استفاده می‌کند.
- از `SemaphoreSlim` برای قفل ارسال (یک عملیات در لحظه) استفاده می‌کند.
- API لیدوما: CreateStoreAsync, UpdateStoreAsync, CreateServiceAsync, CreateCustomersBulkAsync, LoginAsync, CheckHealthAsync.

## فایل‌های تغییر یافته
1. `System.MessageBroadcast/Ui/SmsApp/WEBS_MESG_F.cs` — متدهای اصلی همگام‌سازی
2. `System.MessageBroadcast/Data/iScsc.designer.cs` — انتیتی‌های Club, Fighter, Method, Category_Belt, Club_Method, Club_Method_Weekday, Regulation, D_ATYP
3. `C:\Users\Hadaegh\AppData\Local\Temp\opencode\add_ldma_code.sql` — اسکریپت SQL افودودن LDMA_CODE
4. `C:\Users\Hadaegh\AppData\Local\Temp\opencustom\restore_triggers_final.sql` — اسکریپت SQL تریگرهای cascade ادغام شده + الگوی جدید (OR)
5. `System.MessageBroadcast/Data/iScsc.dbml` + `.layout` — طراحی LINQ-to-SQL

## ستون‌های اضافه شده به SQL
- `LDMA_CODE VARCHAR(250) NULL` به تمام ۱۷۷ جدولی که ستون `LDMA_STAT` داشتند.
- تمام تریگرهای `CG$ASYNL_%` فعال: چک `IF UPDATE(LDMA_CODE) OR UPDATE(LDMA_STAT) OR UPDATE(LDMA_DATE) RETURN` بعد از NESTLEVEL.

## تریگرهای ASYNL و منطق Cascade (در 5 تریگر ادغام شده)
تریگرهای `CG$CLB_UPD_*` به‌صورت جداگانه حذف شده و منطق cascade به‌صورت خودکار در تریگرهای `CG$ASYNL_*` مربوطه ادغام شده است:

- **CG$ASYNL_MTOD** (روی `Method`): اگر `SHOW_STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
  - از طریق `Club_Method` (MTOD_CODE = Method.CODE) → `Club`
- **CG$ASYNL_CBMT** (روی `Club_Method`): اگر `MTOD_STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
  - مستقیم از `Club_Method.CLUB_CODE` → `Club.CODE`
- **CG$ASYNL_CBMW** (روی `Club_Method_Weekday`): اگر `STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
  - از طریق `Club_Method` (CODE = CBMT_CODE) → `Club_Method.CLUB_CODE` → `Club`
- **CG$ASYNL_CTGY** (روی `Category_Belt`): اگر `SHOW_STAT='002' AND CTGY_STAT='002'` تغییر کند → `Club.LDMA_STAT='003'`
  - از طریق `Club_Method` (MTOD_CODE = Category_Belt.MTOD_CODE) → `Club`
- **CG$ASYNL_FIGH** (روی `Fighter`): اگر `FGPB_TYPE_DNRM='003' AND ACTV_TAG_DNRM='101'` تغییر کند → `Club.LDMA_STAT='003'`
  - مستقیم از `Fighter.CLUB_CODE_DNRM` → `Club.CODE`
- همه این تریگرها فقط باشگاه‌هایی را آپدیت می‌کنند که LDMA_STATشون NULL، '001' یا '002' است (نه '003' که قبلاً به‌روزرسانی شده)

## الگوی جدید برای تمام تریگرهای ASYNL (168+ تریگر)
- قبل: 
  ```sql
  IF UPDATE(LDMA_CODE) RETURN;
  IF UPDATE(LDMA_STAT) AND UPDATE(LDMA_DATE) RETURN;
  ```
- بعد (تغییر یافته در تمام ۱۷۷ تریگر CG$ASYNL_%):
  ```sql
  IF UPDATE(LDMA_CODE) OR UPDATE(LDMA_STAT) OR UPDATE(LDMA_DATE) RETURN;
  ```
- تعداد تریگرهای به‌روز شده: 173 (168 با LDMA_CODE + 5 درهم + 3 بدون LDMA_CODE که الگوی جدید اعمال شد)

## گردش کار ارسال (RunSendAllAsync)
1. **Business** → `SendByTypeCoreAsync(Business)` → صف + `SyncClubsAsync()`
2. **HasAnyClubWithStoreIdAsync** → بررسی Club.LDMA_CODE != null
3. **Service** → `SendByTypeCoreAsync(Service)` → فقط صف (بدون متد همگام‌سازی مجزا)
4. **Customer** → `SendByTypeCoreAsync(Customer)` → صف + `SyncCustomersAsync()`

## متد SyncClubsAsync (WEBS_MESG_F.cs ~690)
- Local DataContext: `using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))`
- **فیلتر pending**: `(c.LDMA_STAT ?? "001") == "001" || c.LDMA_STAT == "003"`
- **Triggerهای DB**: اگر هر ردیف در جداول وابسته تغییر کند، Triggerهای `CG$ASYNL_*` به‌صورت خودکار `Club.LDMA_STAT='003'` می‌شود
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
- **فیلتر داخل حلقه**: `FGPB_TYPE_DNRM='001' یا '005'`, `ACTV_TAG_DNRM='101'`, `CONF_STAT='002'`, `CELL_PHON_DNRM` اجباری (11 رقم) برای FGPB_TYPE_DNRM='001'
- **بسته‌های ادغامی (Adaptive batch size)**: شروع با حجم 50، در صورت شکست نیمی می‌شود تا حداقل 1
- اگر درخواست جدید (CreateCustomersBulkAsync) شکست بخورد: تمام مشتریان بسته LDMA_STAT='004' می‌شوند، حجم بسته نصف شده و i را برمی‌گرداند تا مشتریان دوباره با حجم کوچکتر ارسال شوند
- اگر حجم بسته 1 باشد و هنوز شکست بخورد: مشتریان '004' می‌مانند و در چرخه همگام‌سازی آینده دوباره سعی می‌شود
- اگر درخواست موفق باشد: حجم بسته تا حداکثر 50 دو برابر می‌شود
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
- `"004"` = ارسال ناموفق (Failed، قابل ری‌تای)

## ریست ستون‌های LDMA (RESET_LDMA_P)
- **اگر کاربر خواست ستون‌های `LDMA_STAT` / `LDMA_DATE` / `LDMA_CODE` را در iScsc NULL کند** → فقط اجرا کن:
  ```sql
  EXEC RESET_LDMA_P;
  ```
- Stored Procedure `RESET_LDMA_P` در دیتابیس `iScsc` وجود دارد و:
  1. تمام تریگرها را DISABLE می‌کند (جلوگیری از خطای permission CG$AUPD_HLDY و خطای recursion 32-level CG$AINS_MSXD)
  2. برای هر جدولی که حداقل یکی از این ستون‌ها را دارد (۱۷۷ جدول / ۵۳۱ ستون): `UPDATE ... SET LDMA_STAT=NULL, LDMA_DATE=NULL, LDMA_CODE=NULL WHERE ... IS NOT NULL`
  3. تریگرها را دوباره ENABLE می‌کند
- فایل سورس اسکریپت: `C:\Users\Hadaegh\AppData\Local\Temp\opencustom\create_reset_ldma_proc.sql`
- اسکریپت یک‌بار مصرف قبلی (بدون SP): `C:\Users\Hadaegh\AppData\Local\Temp\opencustom\reset_ldma_columns.sql`
- تست: بعد از اجرا باید COUNT غیر-NULL = 0 باشد (Club, Sub_Unit, Basic_Calculate_Discount, Fighter, Method, Club_Method, Club_Method_Weekday, Category_Belt, Organ)

## همگام‌سازی ارگان‌ها (SyncOrgansAsync)
- فیلتر pending: `LDMA_STAT == "003"` (فقط ارگان‌های به‌روزرسانی شده)
- پس از موفقیت: `LDMA_STAT = "002"` برای Sub_Unit، Basic_Calculate_Discount (Rqtp_Code 001/009 و 016)
- پس از شکست: `LDMA_STAT = "004"` برای همان ردیف‌ها
- SubmitChanges در انتها برای ذخیره تمام تغییرات

## فیوزهای امنیتی
- `HasAnyClubWithStoreIdAsync()`: قبل از Services/Customers چک می‌کند حداقل یک Club LDMA_CODE داشته باشد
   - استفاده از `c.LDMA_CODE != null && c.LDMA_CODE != ""` به جای `string.IsNullOrEmpty` در LINQ-to-SQL (VS 2013)
- `RunSendAllAsync`: Business → چک `HasAnyClubWithStoreIdAsync` → Services → Customers → Expense → Organ
- `_sendLock.WaitAsync(0)`: فقط یک ارسال همزمان مجاز است
- `EnsureLoggedInAsync()`: قبل از هر ارسال لاگین را چک می‌کند

## فایل‌های COMPLETED (STABLE VERSION 1.0 - 2026-08-07)
تمام فایل‌های زیر با کامنت `// ========== COMPLETED - DO NOT MODIFY WITHOUT REVIEW ==========` علامت‌گذاری شده‌اند:
- WEBS_MESG_F.cs
- LidomaMarket.cs
- LidomaSmsClient.cs
- LidomaApiClientBase.cs
- Logger.cs
- Msgb.Properties.cs
- MSTR_PAGE_F.cs
- MSTR_PAGE_F.RequestRespons.cs
- App.config
- **NOT FOUND**: `frmStoreOrgans.cs` — این فایل در پروژه وجود ندارد
