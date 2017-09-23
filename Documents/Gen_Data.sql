USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [AppDecision] ( [ID], [ShortCut], [TitleFa], [TitleEN], [IsVisible], [IsActive])
VALUES ( 13921211150538, 1, N'کریستال ریپورت', N'Crystal Report', 1, 1)
INSERT INTO [AppDecision] ( [ID], [ShortCut], [TitleFa], [TitleEN], [IsVisible], [IsActive])
VALUES ( 13921211150601, 2, N'جسپر', N'Jasper iReport', 1, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [DataSource] ( [ID], [ShortCut], [DatabaseServer], [IPAddress], [Port], [Database], [UserID], [Password], [TitleFa], [IsActive], [IsVisible])
VALUES ( 13921130183438, 1, 0, '192.168.1.27', 1521, 'esfahan', 'dbasale', 'dbasale', N'سرور تستی اصفهان', 1, 1)
INSERT INTO [DataSource] ( [ID], [ShortCut], [DatabaseServer], [IPAddress], [Port], [Database], [UserID], [Password], [TitleFa], [IsActive], [IsVisible])
VALUES ( 13921130193147, 2, 1, 'localhost', 1521, 'iProject', 'scott', 'abc123!@#', N'سرور تستی خونه', 1, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [GroupHeader] ( [ID], [ShortCut], [TitleFa], [STitleFa], [TitleEn], [STitleEn], [IsVisible], [IsActive])
VALUES ( 13921115223611, 1, N'گزارشات', N'گزارشات', N'Report', N'Report', 1, 1)
INSERT INTO [GroupHeader] ( [ID], [ShortCut], [TitleFa], [STitleFa], [TitleEn], [STitleEn], [IsVisible], [IsActive])
VALUES ( 13921116213623, 2, N'مالی', N'مالی', N'payment', N'payment', 1, 1)
INSERT INTO [GroupHeader] ( [ID], [ShortCut], [TitleFa], [STitleFa], [TitleEn], [STitleEn], [IsVisible], [IsActive])
VALUES ( 13921117211232, 3, N'پرسنلی', N'پرسنلی', N'personal', N'personal', 1, 1)
INSERT INTO [GroupHeader] ( [ID], [ShortCut], [TitleFa], [STitleFa], [TitleEn], [STitleEn], [IsVisible], [IsActive])
VALUES ( 13921213080833, 4, N'گزارشات فروش انرژی', N'گزارشاتفروشانرژی', N'', N'', 1, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [PrgGroup] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13920927211901, 1, N'امنیتی', N'DataGuard', 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13920927202839, 1, N'ورود به سیستم', N'Create Session', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13920927202840, 2, N'نمایش سیاست های امنیتی', N'Interact With Security Policy', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13920930195352, 3, N'ایجاد گروه دسترسی', N'Create New Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921001214617, 4, N'حذف گروه دسترسی', N'Remove Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921002114152, 5, N'تغییر نام گروه دسترسی', N'Change Role Name', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921002120617, 6, N'غیرفعال کردن گروه دسترسی', N'Deactive Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921002121141, 7, N'فعال کردن گروه دسترسی', N'Active Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921002154918, 8, N'نمونه سازی از گروه دسترسی', N'Duplicate Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921003224622, 9, N'اضافه کردن سطح دسترسی به گروه دسترسی', N'Grant Privilege to Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921004014551, 10, N'حذف سطح دسترسی از گروه دسترسی', N'Revoke Privilege from Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921004015329, 11, N'غیرفعال کردن سطح دسترسی در گروه دسترسی', N'Deactive Privilege in Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921004082735, 12, N'فعال کردن سطح دسترسی در گروه دسترسی', N'Active Privilege in Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921024190547, 13, N'اضافه کردن سطح دسترسی به کاربر', N'Grant Privilege to User', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921024190628, 14, N'حذف سطح دسترسی از کاربر', N'Revoke Privilege from User', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921024190719, 15, N'غیرفعال کردن سطح دسترسی از کاربر', N'Deactive Privilege in User', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921024190810, 16, N'فعال کردن سطح دسترسی به کاریر', N'Active Privilege in User', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921025184905, 17, N'ملحق کردن کاربر به گروه دسترسی', N'Grant User to Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921025185811, 18, N'خارج کردن کاربر از گروه دسترسی', N'Revoke User from Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921025190004, 19, N'غیرفعال کردن کاربر در گروه دسترسی', N'Deactive User to Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921025190157, 20, N'فعال کردن کاربر غیرفعال در گروه دسترسی', N'Active User to Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921030210110, 21, N'ویرایش اطلاعات کاربری', N'Update User profile', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921030210129, 22, N'اضافه کردن کاربر جدید', N'Create New User', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921030210213, 23, N'نمونه سازی از کاربر', N'Duplicate User', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921113221314, 24, N'ملحق شدن کاربر جاری به گروه های دسترسی', N'Join Current User to Roles', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921113233508, 25, N'خارج شدن کاربر جاری از گروه های دسترسی', N'Leave Current User From Roles', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921114080432, 26, N'غیر فعال کردن گروه دسترسی کاربر جاری', N'Deactive Roles For Current User', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921114081907, 27, N'فعال کردن گروه دسترسی کاربر جاری', N'Active Roles For Current User', 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921128190555, 1, N'ورود به سیستم', N'Create Session', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921129220254, 2, N'نمایش سرور پایگاه داده ها', N'Show Database Server', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921129234518, 3, N'ایجاد پل ارتباطی با سرور پایگاه داده', N'Create Connection String', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921130185336, 4, N'ویرایش کردن پل ارتباطی با سرور پایگاه داده', N'Update Connection String', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921130203438, 5, N'برگرداندن سرور پایگاه داده به لیست فعال', N'Restore Connection String', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921130210135, 6, N'حذف سرور پایگاه داده از لیست فعال', N'Remove Connection String', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921130210954, 7, N'فعال کردن سرور پایگاه داده', N'Active Connection String', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921130211014, 8, N'غیرفعال کردن سرور پایگاه داده', N'Deactive Connection String', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921202090027, 9, N'انتقال پل های ارتباطی به برنامه ODBC', N'Import Connection String To ODBC', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921211160805, 10, N'ثبت تنظیمات فایل های گزارش', N'Submit Change Of Specify Report File', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921212151301, 11, N'ثبت تنظیمات نوع فایل های گزارش', N'Submit Change Of Specify AppDecision File Type', 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921109204848, 1, N'نمایش خدمات سیستم', N'Interact With Service Def', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921109204924, 2, N'نمایش جزئیات خدمت', N'Show Service Detail', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921115220415, 3, N'ایجاد کردن سرفصل جدید', N'Create New Group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921116201640, 4, N'ویرایش اطلاعات سرفصل', N'Update Group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921116210403, 5, N'ایجاد کردن نمونه از سرفصل', N'Create Duplicate from Group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921117192051, 6, N'فعال کردن سرفصل', N'Active group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921117192131, 7, N'غیر فعال کردن سرفصل', N'Deactive Group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921117201733, 8, N'خارج کردن سرفصل از گروه دسترسی', N'Leave Group Header From Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921117201957, 9, N'عضو کردن سرفصل در گروه دسترسی', N'Join Group Header To Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921117231149, 10, N'فعال کردن سرفصل در گروه دسترسی', N'Active Group Header To Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921117231228, 11, N'غیر فعال کردن سرفصل در گروه دسترسی', N'Deactive Group Header to Role', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921119201444, 12, N'ایجاد کردن گروه خدمت', N'Add New Group Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921120214008, 13, N'ویرایش کردن گروه خدمت', N'Update Group Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921121183205, 14, N'غیر فعال کردن گروه خدمت', N'Deactive Group Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921121212624, 15, N'فعال کردن گروه خدمت', N'Active Group Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921122002422, 16, N'ملحق کردن گروه خدمت به سرفصل', N'Join Group Service Into group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921122114124, 17, N'خارج کردن گروه خدمت از سرفصل', N'leave Group Service From Group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921122120824, 18, N'فعال کردن گروه خدمت در سرفصل', N'Active Group Service In Group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921122124155, 19, N'غیرفعال کردن گروه خدمت از سرفصل', N'Deactive Group Service Of Group Header', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921122220546, 20, N'ایجاد کردن رسته خدماتی', N'Create UnitType', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921123001838, 21, N'ویرایش کردن رسته خدماتی', N'Update UnitType', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921123212008, 22, N'فعال کردن رسته خدماتی', N'Active UnitType', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921123213019, 23, N'غیرفعال کردن رسته خدماتی', N'Deactive UnitType', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921123215322, 24, N'برگرداندن رسته حذف شده', N'Enabled UnitType', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921123215349, 25, N'حذف رسته خدماتی', N'Diabled UnitType', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124094547, 26, N'ایجاد کردن خدمت', N'Create Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124105705, 27, N'ویرایش کردن خدمت', N'Update Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124111813, 28, N'حذف کردن خدمت', N'Disabled Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124111833, 29, N'برگرداندن خدمت', N'Enabled Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124120623, 30, N'ملحق کردن خدمت به گروه خدمت', N'Join Service Into Group Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124122710, 31, N'خارج کردن خدمت از گروه خدمت', N'Leave Service From Group Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124125752, 32, N'فعال کردن خدمت', N'Active Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921124125817, 33, N'غیرفعال کردن خدمت', N'Deactive Service', 1)
INSERT INTO [Privilege] ( [ID], [ShortCut], [TitleFa], [TitleEn], [IsVisible])
VALUES ( 13921212161937, 34, N'تغییر مولفه پدر گروه خدمات', N'Change Parent iD for Parent Group', 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
SET IDENTITY_INSERT [Role] ON
GO
BEGIN TRANSACTION
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13920927203149, 1, N'ارتباطباسیستم', N'ارتباط با سیستم', N'Connect', 1, 1, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13920930164748, 2, N'گروه موقت', N'گروه موقت', N'Test Role', 1, 1, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13920930215248, 3, N'مدیرانارشد', N'مدیران ارشد', N'dba', 0, 1, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13920930220012, 4, N'کاربرانسیستم', N'کاربران سیستم', N'users', 0, 0, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13921002174123, 21, N'کاربرانارشدبخشچشم', N'کاربران ارشد بخش چشم', N'', 0, 0, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13921002174154, 22, N'کاربرانبخشاتاقعمل', N'کاربران بخش اتاق عمل', N'', 0, 0, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13921213080009, 23, N'انشعاب', N'انشعاب', N'ADM', 0, 1, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13921213080027, 24, N'فروش انرژی', N'فروش انرژی', N'Bill', 0, 1, 1)
INSERT INTO [Role] ( [ID], [ShortCut], [STitleFa], [TitleFa], [TitleEn], [IsDefualt], [IsVisible], [IsActive])
VALUES ( 13921213080057, 25, N'خدمات پس از فروش', N'خدمات پس از فروش', N'SAS', 0, 1, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO
SET IDENTITY_INSERT [Role] OFF
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920927203149, 13921115223611, 1, 1, 1)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920927203149, 13921116213623, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920927203149, 13921117211232, 1, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920927203149, 13921213080833, 1, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920930164748, 13921115223611, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920930164748, 13921116213623, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920930164748, 13921117211232, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920930215248, 13921115223611, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920930215248, 13921116213623, 1, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13920930215248, 13921117211232, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13921002174154, 13921115223611, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13921002174154, 13921116213623, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13921002174154, 13921117211232, 0, 1, 0)
INSERT INTO [Role_GroupHeader] ( [RoleID], [GroupHeaderID], [IsActive], [IsVisible], [IsReporting])
VALUES ( 13921213080027, 13921213080833, 1, 1, 0)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921003224622, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13920927202839, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13920930195352, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921001214617, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921002114152, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921002120617, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921002121141, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921002154918, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921004014551, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921004015329, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921004082735, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921024190547, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921024190628, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921024190719, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921024190810, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921025184905, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921025185811, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921025190004, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921025190157, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921030210110, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921030210129, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921030210213, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921113221314, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921113233508, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921114080432, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921114081907, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921109204848, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921109204924, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921115220415, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921116201640, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921116210403, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921117192051, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921117192131, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921117201733, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921117201957, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921117231149, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921117231228, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921119201444, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921120214008, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921121183205, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921121212624, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921122002422, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921122114124, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921122120824, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921122124155, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921122220546, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921123001838, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921123212008, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921123213019, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921123215322, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921123215349, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124094547, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124105705, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124111813, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124111833, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124120623, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124122710, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124125752, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921124125817, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13921128190555, 1, 0, 1, 2)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13920927202839, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13920927202840, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13920930195352, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921001214617, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921002114152, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921002120617, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921002121141, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921002154918, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921003224622, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921004014551, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921004015329, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921004082735, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921024190547, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921024190628, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921024190719, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921024190810, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921025184905, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921025185811, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921025190004, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921025190157, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921030210110, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921030210129, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921030210213, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921113221314, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921113233508, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921114080432, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921114081907, 1, 0, 1, 0)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921109204848, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921109204924, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921115220415, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921116201640, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921116210403, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921117192051, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921117192131, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921117201733, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921117201957, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921117231149, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921117231228, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921119201444, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921120214008, 1, 0, 1, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO
BEGIN TRANSACTION
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921121183205, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921121212624, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921122002422, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921122114124, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921122120824, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921122124155, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921122220546, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921123001838, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921123212008, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921123213019, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921123215322, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921123215349, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124094547, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124105705, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124111813, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124111833, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124120623, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124122710, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124125752, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921124125817, 1, 0, 1, 1)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920930164748, 13921128190555, 1, 0, 1, 2)
INSERT INTO [Role_Privilege] ( [RoleID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927203149, 13920927202840, 1, 0, 1, 0)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Role_UnitType] ( [RoleID], [UnitTypeID], [IsActive], [IsReporting], [ADID])
VALUES ( 13920927203149, 13921122234205, 1, 1, NULL)
INSERT INTO [Role_UnitType] ( [RoleID], [UnitTypeID], [IsActive], [IsReporting], [ADID])
VALUES ( 13920930215248, 13921122234205, 0, 0, NULL)
INSERT INTO [Role_UnitType] ( [RoleID], [UnitTypeID], [IsActive], [IsReporting], [ADID])
VALUES ( 13920927203149, 13921205230916, 1, 0, NULL)
INSERT INTO [Role_UnitType] ( [RoleID], [UnitTypeID], [IsActive], [IsReporting], [ADID])
VALUES ( 13920927203149, 13921210170637, 1, 1, 13921211150538)
INSERT INTO [Role_UnitType] ( [RoleID], [UnitTypeID], [IsActive], [IsReporting], [ADID])
VALUES ( 13920927203149, 13921210170658, 1, 1, 13921211150601)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13920927203149, 1, 1, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921025195800, 13920927203149, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921030213132, 13920927203149, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921101191809, 13920927203149, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13920930164748, 0, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13921002174123, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13920930220012, 1, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13920930220012, 1, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13920930220012, 1, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921101202659, 13920927203149, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13921213080009, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13921213080027, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13921213080057, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921101202659, 13921213080027, 1, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921101191913, 13920927203149, 1, 0, 0)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13921002174154, 0, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921101191913, 13921002174154, 1, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13921025195800, 13921002174154, 1, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13920930220012, 1, 0, 1)
INSERT INTO [Role_User] ( [UserID], [RoleID], [IsActive], [IsDefault], [IsVisible])
VALUES ( 13920927204910, 13920930215248, 0, 0, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921124131859, 0, N'انشعاب', N'ADM', 13921120211002, 13921120210940, N'1392/11/24 13:18', NULL, 1, 13921204184644, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921124131927, 0, N'فروش انرژی', N'BIL', 13921120211002, 13921120210940, N'1392/11/24 13:19', NULL, 1, 13921204184644, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921124131942, 0, N'خدمات پس از فروش', N'SAS', 13921120211002, 13921120210940, N'1392/11/24 13:19', NULL, 1, 13921204184644, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921124132016, 0, N'عملکرد', N'SPR', 13921120211002, 13921120210940, N'1392/11/24 13:20', NULL, 1, 13921124131942, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921124132824, 1, N'پرداختی های وصولی', N'\\ReportServer\SAS\SPR_AEFS.P.RPT', 13921120211002, 13921122234205, N'1392/11/24 13:28', NULL, 0, 13921124132016, 1, 2000, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921204184644, 0, N'سیستم جامع مشترکین اداره برق', N'Public Power System', 13921120211002, 13921120210940, N'1392/12/04 18:46', NULL, 1, 0, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921205214156, 0, N'پرداختی نهایی', N'\\ReportServer\SPR\Report.Rpt', 13921210170637, 13921205230916, N'1392/12/05 21:41', NULL, 0, 13921124132016, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921213080919, 0, N'گزارشات الف', N'1st Report', 13921120211002, 13921120210940, N'1392/12/13 08:09', NULL, 1, 0, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921213080941, 0, N'گزارشات ب', N'2nd Report', 13921120211002, 13921120210940, N'1392/12/13 08:09', NULL, 1, 0, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921213081018, 0, N'گزارش اول', N'1st Report File', 13921210170658, 13921205230916, N'1392/12/13 08:10', NULL, 0, 13921213080941, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921213081043, 0, N'گزارش اول', N'1st Report File', 13921210170637, 13921122234205, N'1392/12/13 08:10', NULL, 0, 13921213080919, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921213081059, 0, N'گزارش دوم', N'2nd Report File', 13921210170637, 13921122234205, N'1392/12/13 08:10', NULL, 0, 13921213080919, 0, 0, 1, 1, 1)
INSERT INTO [Service] ( [ID], [ShortCut], [TitleFa], [TitleEn], [ServiceUnitID], [ServiceTypeID], [StartDate], [ExpDate], [Level], [ParentID], [PriceType], [Price], [RectCode], [IsActive], [IsVisible])
VALUES ( 13921213081114, 0, N'گزارش دوم', N'2nd Report File', 13921210170658, 13921205230916, N'1392/12/13 08:11', NULL, 0, 13921213080941, 0, 0, 1, 1, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921124132824, 13921115223611, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921204184644, 13921115223611, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921205214156, 13921115223611, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921213080919, 13921213080833, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921213080941, 13921213080833, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921213081018, 13921213080833, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921213081043, 13921213080833, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921213081059, 13921213080833, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921213081114, 13921213080833, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921124131859, 13921115223611, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921124131927, 13921115223611, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921124131942, 13921115223611, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921124132016, 13921115223611, 1, 1)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921124131942, 13921213080833, 0, 0)
INSERT INTO [Service_GroupHeader] ( [ServiceID], [GroupHeaderID], [IsActive], [IsVisible])
VALUES ( 13921124132016, 13921213080833, 0, 0)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [UnitType] ( [ID], [ShortCut], [TitleFa], [TitleEn], [UnitType], [IsVisible], [IsActive])
VALUES ( 13921120210940, 1, N'بدون نوع', N'No Type', 1, 1, 1)
INSERT INTO [UnitType] ( [ID], [ShortCut], [TitleFa], [TitleEn], [UnitType], [IsVisible], [IsActive])
VALUES ( 13921120211002, 1, N'بدون واحد', N'No Unit', 0, 1, 1)
INSERT INTO [UnitType] ( [ID], [ShortCut], [TitleFa], [TitleEn], [UnitType], [IsVisible], [IsActive])
VALUES ( 13921122234205, 2, N'درآمدی', N'', 1, 1, 1)
INSERT INTO [UnitType] ( [ID], [ShortCut], [TitleFa], [TitleEn], [UnitType], [IsVisible], [IsActive])
VALUES ( 13921205230916, 3, N'غیر درآمدی', N'', 1, 1, 1)
INSERT INTO [UnitType] ( [ID], [ShortCut], [TitleFa], [TitleEn], [UnitType], [IsVisible], [IsActive])
VALUES ( 13921210170637, 2, N'Crystal Report', N'', 0, 1, 1)
INSERT INTO [UnitType] ( [ID], [ShortCut], [TitleFa], [TitleEn], [UnitType], [IsVisible], [IsActive])
VALUES ( 13921210170658, 3, N'Jasper iReport', N'', 0, 1, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [User] ( [ID], [ShortCut], [TitleFa], [TitleEn], [STitleEn], [Password], [IsLock], [IsVisible])
VALUES ( 13920927204910, 1, N'عارف حدائقی', N'aref', N'aref', N'922013', 0, 1)
INSERT INTO [User] ( [ID], [ShortCut], [TitleFa], [TitleEn], [STitleEn], [Password], [IsLock], [IsVisible])
VALUES ( 13921025195800, 2, N'کاربر تستی', N'test', N'test', N'test', 0, 1)
INSERT INTO [User] ( [ID], [ShortCut], [TitleFa], [TitleEn], [STitleEn], [Password], [IsLock], [IsVisible])
VALUES ( 13921030213132, 3, N'اسماعیل سرور', N'soror', N'esmaeilsoror', N'123456789', 0, 1)
INSERT INTO [User] ( [ID], [ShortCut], [TitleFa], [TitleEn], [STitleEn], [Password], [IsLock], [IsVisible])
VALUES ( 13921101191809, 4, N'سعید یزدانی', N'yandani', N'SaiedYazdany', N'123456', 0, 1)
INSERT INTO [User] ( [ID], [ShortCut], [TitleFa], [TitleEn], [STitleEn], [Password], [IsLock], [IsVisible])
VALUES ( 13921101191913, 5, N'حمیدرضا خونکاری', N'khoonkari', N'MohammadKhonkary', N'123', 1, 1)
INSERT INTO [User] ( [ID], [ShortCut], [TitleFa], [TitleEn], [STitleEn], [Password], [IsLock], [IsVisible])
VALUES ( 13921101202659, 6, N'محسن ملیپور', N'melipour', N'MohsenMellipour', N'456', 0, 1)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


USE [iProject];
GO
SET DATEFORMAT ymd
GO
BEGIN TRANSACTION
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13920927202839, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13920930195352, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921109204848, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921001214617, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921002114152, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921002120617, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921109204924, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13920927202839, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13920930195352, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921001214617, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921002114152, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921002120617, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921002121141, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921002154918, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921003224622, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921004014551, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921025190157, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921024190810, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921025184905, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921025185811, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921025190004, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921025195800, 13921025190157, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921030210129, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921030210110, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921030210213, 0, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921122002422, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921122124155, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921122120824, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921122220546, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921123001838, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921123212008, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921123213019, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921123215322, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921123215349, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921124094547, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921124094547, 1, 0, 0, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921124105705, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921128190555, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921109204848, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921129220254, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921129234518, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921130185336, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921130203438, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13920927202839, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13920930195352, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921130210135, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921001214617, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921002114152, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921002120617, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921130210954, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921025190157, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921030210129, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921030210110, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921030210213, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921202090027, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921211160805, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921025185811, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921025190004, 1, 0, 1, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921212151301, 1, 0, 1, 2)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921212161937, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921115220415, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921116201640, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921116210403, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921117192051, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921117192131, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921117201733, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921117201957, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921117231149, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921117231228, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921119201444, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921120214008, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921121183205, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921121212624, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921122002422, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921122114124, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921122120824, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13921101202659, 13921122124155, 1, 0, 1, 1)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921025185811, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921025190004, 1, 0, 0, 0)
INSERT INTO [User_Privilege] ( [UserID], [PrivilegeID], [IsActive], [IsDefault], [IsVisible], [Sub_Sys])
VALUES ( 13920927204910, 13921130211014, 1, 0, 1, 2)
IF @@ERROR <> 0
ROLLBACK TRAN
ELSE
COMMIT TRANSACTION
GO


