# تحلیل کامل Viewهای V_MKT_ در دیتابیس iScsc

**تاریخ استخراج:** 2026-08-07  
**دیتابیس:** iScsc  
**الگو:** Viewهای شروع شده با `V_MKT_`

---

## جدول خلاصه گزارش

| نام View | کاربرد | جداول اصلی | فیلدهای کلیدی (با ترجمه فارسی) | توضیحات تکمیلی |
|---|---|---|---|---|
| V_MKT_Customers | لیست مشتریان فعال + وضعیت عضویت/بدهی | Fighter | FILE_NO ← شناسه مشتری, CustomerName ← نام مشتری, Phone ← شماره موبایل, DebtAmount ← مقدار بدهی, HasPhone ← دارد شماره؟, HasAttended ← شرکت کرده؟, MemberStatus ← وضعیت عضویت | فقط مشتریان CONF_STAT='002' و ACTV_TAG_DNRM>='101' |
| V_MKT_Memberships | وضعیت اشتراکات عضویت باشگاه | Member_Ship, Fighter, Method, Category_Belt | CustomerFileNo ← شناسه مشتری, CustomerName ← نام مشتری, Phone ← تلفن, ServiceName ← نام سرویس, CourseName ← نام دوره, PlanPrice ← قیمت برنامه, STRT_DATE ← تاریخ شروع, END_DATE ← تاریخ پایان, DurationMonths ← مدت (ماه), SessionsPerMonth ← جلسات/ماه, TotalSessions ← کل جلسات, SessionsUsed ← جلسات استفاده شده, SessionsRemaining ← جلسات باقی‌مانده, DaysRemaining ← روزهای باقی‌مانده, SubscriptionStatus ← وضعیت اشتراک, ActionMessage ← پیام عملی, RECT_CODE ← نوع ثبت | فقط RECT_CODE='004' (اشتراکات فعال) |
| V_MKT_MonthlySummary | خلاصه مالی ماهانه فروش/سود/درآمد | Payment, Request | Year ← سال, Month ← ماه, RQTP_CODE ← نوع درخواست, InvoiceCount ← تعداد فاکتور, TotalSales ← کل فروش, TotalProfit ← کل سود, TotalCollected ← کل دریافتی, TotalOutstanding ← کل بدهی | گروه‌بندی بر اساس سال/ماه/نوع درخواست |
| V_MKT_PaymentChain | زنجیره پرداخت کامل: صورتحساب→ردیف→مشتری | Payment, Request, Request_Type, Request_Row, Fighter, Payment_Detail, Expense, Expense_Type, Expense_Item, Category_Belt, Method | CASH_CODE ← کد صورتحساب, PYMT_NO ← شماره پرداخت, RQST_RQID ← شناسه درخواست, RQTP_CODE ← نوع درخواست, RequestType ← نام نوع درخواست, RQST_DATE ← تاریخ درخواست, TotalAmount ← مبلغ کل, PYMT_STAT ← وضعیت پرداخت, RowNo ← شماره ردیف, EXPN_CODE ← کد هزینه, DirectDescription ← توضیح مستقیم, ItemPrice ← قیمت واحد, ItemQuantity ← تعداد, ISSU_DATE ← تاریخ صدور, EXPR_DATE ← تاریخ انقضا, ExpenseCode ← کد هزینه, ExpenseDescription ← نام هزینه, ExpenseTypeDescription ← نوع هزینه, ExpenseItemDescription ← آیتم هزینه, CategoryDesc ← دسته‌بندی, MethodDesc ← روش, ExpenseListPrice ← قیمت لیست هزینه, CustomerFileNo ← شناسه مشتری, CustomerName ← نام مشتری, CustomerPhone ← تلفن مشتری | اتصال کامل زنجیره مالی از سطح بالا به مشتری |
| V_MKT_PaymentDetail | جزئیات هر ردیف صورتحساب | Payment_Detail, Request, Expense, Expense_Type, Expense_Item | PYMT_CASH_CODE ← شناسه صورتحساب, PYMT_RQST_RQID ← شناسه درخواست, RQTP_CODE ← نوع درخواست, RQTT_CODE ← زیرنوع درخواست, RowNo ← شماره ردیف, EXPN_CODE ← کد هزینه, PAY_STAT ← وضعیت پرداخت, Description ← توضیح, UnitPrice ← قیمت واحد, Quantity ← تعداد, TotalPrice ← قیمت کل, IssueDate ← تاریخ صدور, ExpiryDate ← تاریخ انقضا, CustomerFileNo ← شناسه مشتری, ServiceCode ← کد سرویس, PricingPlanCode ← کد برنامه قیمت‌گذاری, ProfitAmount ← مقدار سود, CMNT ← کامنت, ExpenseDesc ← نام هزینه, ExpiryStatus ← وضعیت انقضاء (EXPIRED/ACTIVE/NO_EXPIRY) | تحلیل هزینه‌ها بر اساس تاریخ انقضاء |
| V_MKT_Priority | لیست اولویت‌دار مشتریان برای فالوآپ | Fighter, Member_Ship, Attendance | Priority ← اولویت ردیف, CustomerCode ← شناسه مشتری, FullName ← نام کامل, PhoneNumber ← شماره تلفن, RemainingDebt ← بدهی باقی‌مانده, AttendanceStatus ← وضعیت حضور (Attended/Never Attended), MembershipStatus ← وضعیت عضویت, ActionPriority ← اولویت اقدام, ActionPlan ← طرح عمل | مشتریان بدهکار/بدون حضور → اولویت بالا |
| V_MKT_ServiceExpiry | سرویسهای منقضی/به‌زودی منقضی | Payment_Detail, Request, Request_Row, Fighter, Expense, Expense_Type, Expense_Item | ExpiryDate ← تاریخ انقضا, ServiceName ← نام سرویس, ExpenseDesc ← هزینه, ExpiryItem ← آیتم هزینه, Amount ← مبلغ, CustomerFileNo ← شناسه مشتری, CustomerName ← نام مشتری, Phone ← تلفن, RQTP_CODE ← نوع درخواست, DaysOverdue ← روزهای تاخیر, ReminderStatus ← وضعیت یادآوری (OVERDUE/EXPIRING SOON/ACTIVE), RecommendedAction ← اقدام پیشنهادی | فقط ردیف‌های دارای EXPR_DATE |

---

## جزئیات کامل هر View

### 1. V_MKT_Customers

**کوئری تعریف:**
```sql
CREATE VIEW dbo.V_MKT_Customers
AS
SELECT 
    f.FILE_NO,
    f.NAME_DNRM AS CustomerName,
    f.CELL_PHON_DNRM AS Phone,
    ISNULL(f.DEBT_DNRM, 0) AS DebtAmount,
    CASE WHEN f.CELL_PHON_DNRM IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasPhone,
    CASE WHEN EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) THEN 'YES' ELSE 'NO' END AS HasAttended,
    CASE 
        WHEN f.DEBT_DNRM > 0 THEN 'Has Debt'
        WHEN EXISTS (SELECT 1 FROM Member_Ship ms WHERE ms.FIGH_FILE_NO = f.FILE_NO AND ms.VALD_TYPE = '002' AND ms.END_DATE >= GETDATE())
        THEN 'Active Member'
        ELSE 'Inactive/Expired'
    END AS MemberStatus
FROM Fighter f
WHERE f.CONF_STAT = '002'
  AND f.ACTV_TAG_DNRM >= '101';
```

**داده‌های نمایش‌دهی:** لیست مشتریان فعال (تایید شده با CONF_STAT='002' و وضعیت فعال ACTV_TAG_DNRM>='101') با نام، شماره موبایل، مقدار بدهی، وضعیت ورود به کلاس و وضعیت عضویتشان.

**کاربرد:** این View برای مدیریت ارتباط با مشتریان (CRM) و شناسایی مشتریان فعال/بدهکار/عضو استفاده میشود.

**جدول اصلی:** Fighter (همراه با بررسی‌های ساب‌کوئری روی Attendance و Member_Ship)

**فیلترها:** `CONF_STAT = '002'` (وضعیت تأیید) و `ACTV_TAG_DNRM >= '101'` (وضعیت فعال)

**ستون‌ها با ترجمه فارسی:**
- FILE_NO → شناسه مشتری
- CustomerName → نام مشتری
- Phone → شماره موبایل
- DebtAmount → مقدار بدهی
- HasPhone → دارد شماره موبایل؟
- HasAttended → شرکت در کلاس داشته؟
- MemberStatus → وضعیت عضویت

**کامنت:** هیچ

---

### 2. V_MKT_Memberships

**کوئری تعریف:**
```sql
CREATE VIEW dbo.V_MKT_Memberships
AS
SELECT 
    ms.FIGH_FILE_NO AS CustomerFileNo,
    f.NAME_DNRM AS CustomerName,
    f.CELL_PHON_DNRM AS Phone,
    mtd.MTOD_DESC AS ServiceName,
    cb.CTGY_DESC AS CourseName,
    cb.PRIC AS PlanPrice,
    ms.STRT_DATE,
    ms.END_DATE,
    ms.NUMB_OF_MONT_DNRM AS DurationMonths,
    ms.NUMB_OF_ATTN_MONT AS SessionsPerMonth,
    (ms.NUMB_OF_ATTN_MONT * ms.NUMB_OF_MONT_DNRM) AS TotalSessions,
    ISNULL((SELECT COUNT(*) FROM Attendance a WHERE a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND a.ATTN_DATE BETWEEN ms.STRT_DATE AND ms.END_DATE), 0) AS SessionsUsed,
    ((ms.NUMB_OF_ATTN_MONT * ms.NUMB_OF_MONT_DNRM) - ISNULL((SELECT COUNT(*) FROM Attendance a WHERE a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND a.ATTN_DATE BETWEEN ms.STRT_DATE AND ms.END_DATE), 0)) AS SessionsRemaining,
    DATEDIFF(DAY, GETDATE(), ms.END_DATE) AS DaysRemaining,
    CASE 
        WHEN ms.END_DATE < GETDATE() THEN 'EXPIRED'
        WHEN ms.END_DATE <= DATEADD(DAY, 7, GETDATE()) THEN 'EXPIRING SOON'
        ELSE 'ACTIVE'
    END AS SubscriptionStatus,
    CASE 
        WHEN ((ms.NUMB_OF_ATTN_MONT * ms.NUMB_OF_MONT_DNRM) - ISNULL((SELECT COUNT(*) FROM Attendance a WHERE a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND a.ATTN_DATE BETWEEN ms.STRT_DATE AND ms.END_DATE), 0)) > 0 
         AND ms.END_DATE >= GETDATE() THEN 'ACTIVE - Encourage attendance'
        WHEN ((ms.NUMB_OF_ATTN_MONT * ms.NUMB_OF_MONT_DNRM) - ISNULL((SELECT COUNT(*) FROM Attendance a WHERE a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND a.ATTN_DATE BETWEEN ms.STRT_DATE AND ms.END_DATE), 0)) > 0 
         AND ms.END_DATE < GETDATE() THEN 'EXPIRED WITH REMAINING SESSIONS - Offer extension'
        WHEN ((ms.NUMB_OF_ATTN_MONT * ms.NUMB_OF_MONT_DNRM) - ISNULL((SELECT COUNT(*) FROM Attendance a WHERE a.FIGH_FILE_NO = ms.FIGH_FILE_NO AND a.ATTN_DATE BETWEEN ms.STRT_DATE AND ms.END_DATE), 0)) = 0 
         THEN 'ALL SESSIONS USED'
        ELSE 'REVIEW'
    END AS ActionMessage,
    ms.RECT_CODE
FROM Member_Ship ms
JOIN Fighter f ON ms.FIGH_FILE_NO = f.FILE_NO
LEFT JOIN Method mtd ON ms.FGPB_MTOD_CODE_DNRM = mtd.CODE
LEFT JOIN Category_Belt cb ON ms.FGPB_CTGY_CODE_DNRM = cb.CODE
WHERE ms.RECT_CODE = '004';
```

**داده‌های نمایش‌دهی:** اطلاعات کامل اشتراکات عضویت فعال (RECT_CODE='004') شامایت تاریخ شروع/پایان، مدت زمان، جلسات کل/استفاده‌شده/باقی‌مانده، وضعیت و پیام عملیاتی.

**کاربرد:** مدیریت اشتراک‌ها، یادآوری انقضا، تشویق حضور و پیگیری جلسات باقی‌مانده.

**جداول اصلی:** Member_Ship (اصلی)، Fighter، Method، Category_Belt (JOINهای LEFT)

**فیلترها:** `RECT_CODE = '004'` — فقط ردیف‌های فعال/ثبت‌شده

**ستون‌ها با ترجمه فارسی:**
- CustomerFileNo → شناسه مشتری
- CustomerName → نام مشتری
- Phone → شماره تلفن
- ServiceName → نام سرویس (روش تمرین)
- CourseName → نام دوره (دسته‌بندی رنگ بدن)
- PlanPrice → قیمت برنامه
- STRT_DATE → تاریخ شروع
- END_DATE → تاریخ پایان
- DurationMonths → مدت (ماه)
- SessionsPerMonth → جلسات/ماه
- TotalSessions → کل جلسات
- SessionsUsed → جلسات استفاده شده
- SessionsRemaining → جلسات باقی‌مانده
- DaysRemaining → روزهای باقی‌مانده تا پایان
- SubscriptionStatus → وضعیت اشتراک (ACTIVE/EXPIRED/EXPIRING SOON)
- ActionMessage → پیام عملی
- RECT_CODE → نوع ثبت

**کامنت:** هیچ

---

### 3. V_MKT_MonthlySummary

**کوئری تعریف:**
```sql
CREATE VIEW dbo.V_MKT_MonthlySummary
AS
SELECT 
    YEAR(p.CASH_DATE) AS Year,
    MONTH(p.CASH_DATE) AS Month,
    r.RQTP_CODE,
    COUNT(DISTINCT p.CASH_CODE) AS InvoiceCount,
    ISNULL(SUM(p.SUM_EXPN_PRIC), 0) AS TotalSales,
    ISNULL(SUM(p.PROF_AMNT_DNRM), 0) AS TotalProfit,
    ISNULL(SUM(p.SUM_RCPT_EXPN_PRIC), 0) AS TotalCollected,
    ISNULL(SUM(p.SUM_EXPN_PRIC - ISNULL(p.SUM_RCPT_EXPN_PRIC, 0)), 0) AS TotalOutstanding
FROM Payment p
JOIN Request r ON p.RQST_RQID = r.RQID
GROUP BY YEAR(p.CASH_DATE), MONTH(p.CASH_DATE), r.RQTP_CODE;
```

**داده‌های نمایش‌دهی:** خلاصه مالی ماهانه شامایت تعداد فاکتور، کل فروش، سود، دریافتی و بدهی، گروه‌بندی بر اسامت سال، ماه و نوع درخواست.

**کاربرد:** گزارش‌گیری مالی و تحلیل فروش ماهانه بر اساس انواع درخواست.

**جداول اصلی:** Payment (اصلی)، Request (برای RQTP_CODE)

**فیلترها:** هیچ (WHERE ندارد، گروه‌بندی بر اساس GROUP BY)

**ستون‌ها با ترجمه فارسی:**
- Year → سال
- Month → ماه
- RQTP_CODE → کد نوع درخواست
- InvoiceCount → تعداد فاکتورها
- TotalSales → کل فروش
- TotalProfit → کل سود
- TotalCollected → کل دریافتی
- TotalOutstanding → بدهی باقی‌مانده

**کامنت:** هیچ

---

### 4. V_MKT_PaymentChain

**کوئری تعریف:**
```sql
CREATE VIEW dbo.V_MKT_PaymentChain
AS
SELECT 
    p.CASH_CODE,
    p.PYMT_NO,
    p.RQST_RQID,
    r.RQTP_CODE,
    rt.RQTP_DESC AS RequestType,
    r.RQST_DATE,
    p.SUM_EXPN_PRIC AS TotalAmount,
    p.PYMT_STAT,
    pd.RQRO_RWNO AS RowNo,
    pd.EXPN_CODE,
    pd.PYDT_DESC AS DirectDescription,
    pd.EXPN_PRIC AS ItemPrice,
    pd.QNTY AS ItemQuantity,
    pd.ISSU_DATE,
    pd.EXPR_DATE,
    e.CODE AS ExpenseCode,
    e.EXPN_DESC AS ExpenseDescription,
    et.EXTP_DESC AS ExpenseTypeDescription,
    ei.EPIT_DESC AS ExpenseItemDescription,
    cb.CTGY_DESC AS CategoryDesc,
    mtd.MTOD_DESC AS MethodDesc,
    e.PRIC AS ExpenseListPrice,
    rr.FIGH_FILE_NO AS CustomerFileNo,
    f.NAME_DNRM AS CustomerName,
    f.CELL_PHON_DNRM AS CustomerPhone
FROM Payment p
JOIN Request r ON p.RQST_RQID = r.RQID
JOIN Request_Type rt ON r.RQTP_CODE = rt.CODE AND rt.SUB_SYS = 1
JOIN Request_Row rr ON r.RQID = rr.RQST_RQID
JOIN Fighter f ON rr.FIGH_FILE_NO = f.FILE_NO
JOIN Payment_Detail pd ON p.CASH_CODE = pd.PYMT_CASH_CODE AND p.RQST_RQID = pd.PYMT_RQST_RQID
LEFT JOIN Expense e ON pd.EXPN_CODE = e.CODE
LEFT JOIN Expense_Type et ON e.EXTP_CODE = et.CODE
LEFT JOIN Expense_Item ei ON et.EPIT_CODE = ei.CODE
LEFT JOIN Category_Belt cb ON e.CTGY_CODE = cb.CODE
LEFT JOIN Method mtd ON e.MTOD_CODE = mtd.CODE;
```

**داده‌های نمایش‌دهی:** زنجیره کامل پرداخت از سطح صورتحساب (Payment) تا هر ردیف جزئیات (Payment_Detail) تا مشتری (Fighter)، همراه با تمام توضیحات هزینه، نوع هزینه، آیتم، دسته‌بندی و روش.

**کاربرد:** تحلیل کامل جزئیات هر صورتحساب/فاکتور، ردیابی هزینه‌ها و ارتباط به مشتری.

**جداول اصلی:** Payment, Request, Request_Type, Request_Row, Fighter, Payment_Detail, Expense, Expense_Type, Expense_Item, Category_Belt, Method

**فیلترها:** هیچ WHERE ندارد؛ شرط JOIN `rt.SUB_SYS = 1` روی Request_Type

**ستون‌ها با ترجمه فارسی:**
- CASH_CODE → کد صورتحساب/فاکتور
- PYMT_NO → شماره پرداخت
- RQST_RQID → شناسه درخواست
- RQTP_CODE → کد نوع درخواست
- RequestType → نوع درخواست
- RQST_DATE → تاریخ درخواست
- TotalAmount → مبلغ کل
- PYMT_STAT → وضعیت پرداخت
- RowNo → شماره ردیف
- EXPN_CODE → کد هزینه
- DirectDescription → توضیح مستقیم (PYDT_DESC)
- ItemPrice → قیمت واحد
- ItemQuantity → تعداد
- ISSU_DATE → تاریخ صدور
- EXPR_DATE → تاریخ انقضا
- ExpenseCode → کد هزینه (جدول Expense)
- ExpenseDescription → نام هزینه
- ExpenseTypeDescription → نوع هزینه
- ExpenseItemDescription → آیتم هزینه
- CategoryDesc → نام دسته‌بندی رنگ
- MethodDesc → نام روش
- ExpenseListPrice → قیمت لیست هزینه
- CustomerFileNo → شناسه مشتری
- CustomerName ← نام مشتری
- CustomerPhone ← تلفن مشتری

**کامنت:** هیچ

---

### 5. V_MKT_PaymentDetail

**کوئری تعریف:**
```sql
CREATE VIEW dbo.V_MKT_PaymentDetail
AS
SELECT 
    pd.PYMT_CASH_CODE,
    pd.PYMT_RQST_RQID,
    r.RQTP_CODE,
    r.RQTT_CODE,
    pd.RQRO_RWNO AS RowNo,
    pd.EXPN_CODE,
    pd.PAY_STAT,
    pd.PYDT_DESC AS Description,
    pd.EXPN_PRIC AS UnitPrice,
    pd.QNTY AS Quantity,
    (pd.EXPN_PRIC * ISNULL(pd.QNTY, 1)) AS TotalPrice,
    pd.ISSU_DATE AS IssueDate,
    pd.EXPR_DATE AS ExpiryDate,
    pd.FIGH_FILE_NO AS CustomerFileNo,
    pd.MTOD_CODE_DNRM AS ServiceCode,
    pd.CTGY_CODE_DNRM AS PricingPlanCode,
    pd.PROF_AMNT_DNRM AS ProfitAmount,
    pd.CMNT,
    e.EXPN_DESC AS ExpenseDesc,
    et.EXTP_DESC AS ExpenseTypeDesc,
    ei.EPIT_DESC AS ExpenseItemDesc,
    CASE 
        WHEN pd.EXPR_DATE IS NOT NULL AND pd.EXPR_DATE < GETDATE() THEN 'EXPIRED'
        WHEN pd.EXPR_DATE IS NOT NULL AND pd.EXPR_DATE >= GETDATE() THEN 'ACTIVE'
        ELSE 'NO_EXPIRY'
    END AS ExpiryStatus
FROM Payment_Detail pd
JOIN Request r ON pd.PYMT_RQST_RQID = r.RQID
LEFT JOIN Expense e ON pd.EXPN_CODE = e.CODE
LEFT JOIN Expense_Type et ON e.EXTP_CODE = et.CODE
LEFT JOIN Expense_Item ei ON et.EPIT_CODE = ei.CODE;
```

**داده‌های نمایش‌دهی:** جزئیات هر ردیف پرداخت (Payment_Detail) با محاسبه قیمت کل، وضعیت انقضاء (EXPIRED/ACTIVE/NO_EXPIRY) و توضیحات هزینه.

**کاربرد:** تحلیل هزینه‌ها و آیتم‌های خریداری شده توسط مشتریان، بررسی انقضاء سرویسها.

**جداول اصلی:** Payment_Detail (اصلی)، Request، Expense، Expense_Type، Expense_Item

**فیلترها:** هیچ WHERE ندارد

**ستون‌ها با ترجمه فارسی:**
- PYMT_CASH_CODE → شناسه صورتحساب
- PYMT_RQST_RQID → شناسه درخواست
- RQTP_CODE → کد نوع درخواست
- RQTT_CODE → کد زیرنوع درخواست
- RowNo → شماره ردیف
- EXPN_CODE → کد هزینه
- PAY_STAT → وضعیت پرداخت
- Description → توضیح
- UnitPrice → قیمت واحد
- Quantity → تعداد
- TotalPrice → قیمت کل
- IssueDate → تاریخ صدور
- ExpiryDate → تاریخ انقضا
- CustomerFileNo → شناسه مشتری
- ServiceCode → کد سرویس
- PricingPlanCode → کد برنامه قیمت‌گذاری
- ProfitAmount → مقدار سود
- CMNT → کامنت
- ExpenseDesc → نام هزینه
- ExpenseTypeDesc → نوع هزینه
- ExpenseItemDesc → آیتم هزینه
- ExpiryStatus → وضعیت انقضاء

**کامنت:** هیچ

---

### 6. V_MKT_Priority

**کوئری تعریف:**
```sql
CREATE VIEW dbo.V_MKT_Priority
AS
SELECT 
    ROW_NUMBER() OVER(ORDER BY 
        CASE WHEN f.CELL_PHON_DNRM IS NOT NULL AND f.DEBT_DNRM > 0 THEN 0
             WHEN f.CELL_PHON_DNRM IS NOT NULL THEN 1
             ELSE 2 END,
        ISNULL(f.DEBT_DNRM, 0) DESC) AS Priority,
    f.FILE_NO AS CustomerCode,
    f.NAME_DNRM AS FullName,
    f.CELL_PHON_DNRM AS PhoneNumber,
    f.DEBT_DNRM AS RemainingDebt,
    CASE WHEN EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) THEN 'Attended' ELSE 'Never Attended' END AS AttendanceStatus,
    CASE 
        WHEN EXISTS (SELECT 1 FROM Member_Ship ms WHERE ms.FIGH_FILE_NO = f.FILE_NO AND ms.VALD_TYPE = '002' AND ms.END_DATE >= GETDATE()) 
        THEN 'Active Until: ' + CONVERT(VARCHAR(10), (SELECT MAX(ms2.END_DATE) FROM Member_Ship ms2 WHERE ms2.FIGH_FILE_NO = f.FILE_NO AND ms2.VALD_TYPE = '002'), 111)
        ELSE 'No Active Membership'
    END AS MembershipStatus,
    CASE 
        WHEN f.CELL_PHON_DNRM IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) AND ISNULL(f.DEBT_DNRM, 0) > 0 THEN 1
        WHEN f.CELL_PHون_DNRM IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) THEN 2
        WHEN NOT EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) THEN 3
        ELSE 4
    END AS ActionPriority,
    CASE 
        WHEN f.CELL_PHON_DNRM IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) AND ISNULL(f.DEBT_DNRM, 0) > 0 
        THEN 'Send payment reminder + class invitation via SMS/Call'
        WHEN f.CELL_PHON_DNRM IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) 
        THEN 'Send welcome & class schedule via SMS/Call'
        WHEN NOT EXISTS (SELECT 1 FROM Attendance a WHERE a.FIGH_FILE_NO = f.FILE_NO) 
        THEN 'Find/Update contact information'
        ELSE 'Monitor'
    END AS ActionPlan
FROM Fighter f
WHERE f.CONF_STAT = '002' AND f.ACTV_TAG_DNRM >= '101';
```

**داده‌های نمایش‌دهی:** لیست مشتریان به ترتیب اولویت برای فالوآپ فروشی/بازاریابی، بر اساس وضعیت بدهی، وجود شماره تماس و سابقه حضور در کلاس.

**کاربرد:** اولویت‌بندی مشتریان برای تماس تلفنی/SMS/کال، پیگیری بدهی، دعوت کلاس و جمع‌آوری اطلاعات تماس.

**جدول اصلی:** Fighter (همراه با ساب‌کوئری روی Attendance و Member_Ship)

**فیلترها:** `CONF_STAT = '002'` و `ACTV_TAG_DNRM >= '101'`

**ستون‌ها با ترجمه فارسی:**
- Priority → اولویت ردیف
- CustomerCode → کد مشتری
- FullName → نام کامل
- PhoneNumber → شماره تلفن
- RemainingDebt → بدهی باقی‌مانده
- AttendanceStatus → وضعیت حضور (Attended/Never Attended)
- MembershipStatus → وضعیت عضویت
- ActionPriority → اولویت عمل
- ActionPlan → طرح/راهنمای اقدام

**کامنت:** هیچ

---

### 7. V_MKT_ServiceExpiry

**کوئری تعریف:**
```sql
CREATE VIEW dbo.V_MKT_ServiceExpiry
AS
SELECT 
    pd.EXPR_DATE AS ExpiryDate,
    pd.PYDT_DESC AS ServiceName,
    e.EXPN_DESC AS ExpenseDesc,
    ei.EPIT_DESC AS ExpenseItem,
    pd.EXPN_PRIC AS Amount,
    rr.FIGH_FILE_NO AS CustomerFileNo,
    f.NAME_DNRM AS CustomerName,
    f.CELL_PHON_DNRM AS Phone,
    r.RQTP_CODE,
    DATEDIFF(DAY, pd.EXPR_DATE, GETDATE()) AS DaysOverdue,
    CASE 
        WHEN pd.EXPR_DATE < GETDATE() THEN 'OVERDUE'
        WHEN pd.EXPR_DATE <= DATEADD(DAY, 7, GETDATE()) THEN 'EXPIRING SOON'
        ELSE 'ACTIVE'
    END AS ReminderStatus,
    CASE 
        WHEN pd.EXPR_DATE < GETDATE() AND r.RQTP_CODE = '016' THEN 'SEND RENEWAL OFFER'
        WHEN pd.EXPR_DATE < GETDATE() THEN 'CONTACT CUSTOMER'
        WHEN pd.EXPR_DATE <= DATEADD(DAY, 7, GETDATE()) THEN 'SEND REMINDER'
        ELSE 'NO ACTION'
    END AS RecommendedAction
FROM Payment_Detail pd
JOIN Request r ON pd.PYMT_RQST_RQID = r.RQID
JOIN Request_Row rr ON r.RQID = rr.RQST_RQID
JOIN Fighter f ON rr.FIGH_FILE_NO = f.FILE_NO
LEFT JOIN Expense e ON pd.EXPN_CODE = e.CODE
LEFT JOIN Expense_Type et ON e.EXTP_CODE = et.CODE
LEFT JOIN Expense_Item ei ON et.EPIT_CODE = ei.CODE
WHERE pd.EXPR_DATE IS NOT NULL;
```

**داده‌های نمایش‌دهی:** سرویسها و هزینه‌هایی که تاریخ انقضایشان ثبت شده، همراه با وضعیت یادآوری (OVERDUE/EXPIRING SOON/ACTIVE) و اقدام پیشنهادی.

**کاربرد:** ارسال یادآوری انقضای سرویس، پیشنهاد تمدید، تماس با مشتریان دیرتراز پرداخت.

**جداول اصلی:** Payment_Detail (اصلی)، Request، Request_Row، Fighter، Expense، Expense_Type، Expense_Item

**فیلترها:** `WHERE pd.EXPR_DATE IS NOT NULL` — فقط ردیف‌های دارای تاریخ انقضاء
- شرط درخواست: `r.RQTP_CODE = '016'` برای ارسال پیشنهاد تمدید

**ستون‌ها با ترجمه فارسی:**
- ExpiryDate → تاریخ انقضا
- ServiceName → نام سرویس
- ExpenseDesc → نام هزینه
- ExpenseItem → آیتم هزینه
- Amount → مبلغ
- CustomerFileNo → شناسه مشتری
- CustomerName → نام مشتری
- Phone → شماره تلفن
- RQTP_CODE → کد نوع درخواست
- DaysOverdue → روزهای تاخیر (مثبت = منقضی، منفی = آینده)
- ReminderStatus → وضعیت یادآوری (OVERDUE/EXPIRING SOON/ACTIVE)
- RecommendedAction → اقدام پیشنهادی (SEND RENEWAL OFFER/CONTACT CUSTOMER/SEND REMINDER/NO ACTION)

**کامنت:** هیچ

---

## جمع‌بندی

### بخش‌های سیستمی که Viewها مربوط می‌شوند

این ۷ View با پیشوند `V_MKT_` عمدتا به حوزه **بازاریابی و مشتریان** (Marketing & Customers) مرتبط‌اند اما شامل لایه‌های مختلف سیستم گیم مرکت/باشگاه طراوران می‌شوند:

| حوزه | Viewهای مرتبط | توصیف |
|---|---|---|
| **مدیریت مشتریان / CRM** | V_MKT_Customers، V_MKT_Priority | لیست مشتریان فعال، اولویت‌بندی برای فالوآپ فروشی/بازاریابی، شناسایی بدهکاران و بدون حضور |
| **مدیریت عضویت / اشتراک** | V_MKT_Memberships | وضعیت اشتراکات باشگاه (مدت، جلسات، انقضا، پیام عملیاتی) |
| **گزارش‌گیری مالی** | V_MKT_MonthlySummary، V_MKT_PaymentChain، V_MKT_PaymentDetail | خلاصه مالی ماهانه، زنجیره کامل صورتحساب، جزئیات هر ردیف هزینه |
| **یادآوری و پیگیری** | V_MKT_ServiceExpiry | سرویسهای منقضی یا به‌زودی منقضی — برای ارسال یادآوری، دعوت تمدید، تماس با مشتری |
| **تحلیل هزینه‌ها** | V_MKT_PaymentDetail، V_MKT_PaymentChain | تجزیه و تحلیل هزینه‌ها، انواع هزینه، دسته‌بندی رنگ و روش‌های آموزشی |

### نکات کلیدی عملی
1. **V_MKT_Priority** پیشنهاد می‌کند که مشتریان بدهکار بدون شماره تماس یا بدون حضور → اولویت ۱ (بیشترین اقدام) داشته باشند.
2. **V_MKT_ServiceExpiry** فقط روی `RQTP_CODE = '016'` یک پیام "SEND RENEWAL OFFER" می‌دهد — پیش فرضاً برای بقیه فقط "CONTACT CUSTOMER".
3. **V_MKT_Memberships** فقط RECT_CODE='004' را نشان می‌دهد — احتمالاً نوع "فعال" یا "ثبت‌شده" است.
4. هیچ‌کدام از Viewها کامنتی ندارند — پیشنهاد می‌شود Extended Properties اضافه شون.
5. تمام Viewها فقط بر روی دیتابیس iScsc تعریف شده‌اند (نه iProject).

### روش استفاده (اسکریپت RESET_LDMA_P باز هم قابل استفاده است برای ریست وضعیت‌ها اگر نیاز باشد)
```sql
-- مثال: دیدن مشتریان اولویت‌دار بدهکار
SELECT * FROM V_MKT_Priority WHERE RemainingDebt > 0 ORDER BY Priority;

-- مثال: سرویسهای منقضی شده‌ای که نیاز به یادآوری دارند
SELECT * FROM V_MKT_ServiceExpiry WHERE ReminderStatus = 'OVERDUE';
```

---
*تولید شده توسط opencode در تاریخ 2026-08-07 — بر اساس دیتابیس iScsc*
