
-- This script adds KM.Menu entries for KMWeb (Form Designer)

declare @pMenuID int
 

insert into Menu( ApplicationID,    IsServiceFeature, ServiceFeatureID,      MenuName,Description,   IsParent,   ParentMenuID,     URL   ,IsActive      ,MenuOrder, HasFeatures,      ImagePath,  DateCreated,      DateUpdated,      CreatedByUserID,  UpdatedByUserID,  ServiceID,  IsClientGroupService,      IsSysAdmin, IsChannelAdmin,   IsCustomerAdmin)

VALUES(12,  0,    -1,   'Home',     NULL, 1,    0,    '/ecn.accounts/main/default.aspx',      1,    0,    0,    '',   GETDATE(),  NULL, 1,    NULL  ,0    ,NULL,      NULL,      NULL, NULL)

select @pMenuID = @@IDENTITY;

 

insert into Menu( ApplicationID,    IsServiceFeature, ServiceFeatureID,      MenuName,Description,   IsParent,   ParentMenuID,     URL   ,IsActive      ,MenuOrder, HasFeatures,      ImagePath,  DateCreated,      DateUpdated,      CreatedByUserID,  UpdatedByUserID,  ServiceID,  IsClientGroupService,      IsSysAdmin, IsChannelAdmin,   IsCustomerAdmin)

VALUES(12,  1,    24,   'Data Compare',   NULL, 0,    @pMenuID,   '/uad.datacompare/',      1,    9,    0,    '',   GETDATE(),  NULL  ,1    ,NULL ,NULL ,NULL ,NULL ,NULL      ,NULL),

(     12,   0,    -1,   'Audience Management System (AMS)', NULL, 0,    @pMenuID,      '/UAS.Web', 1,    4,    0,    '',   GETDATE(),  NULL, 9869, NULL, 1,    NULL,      NULL, NULL  ,NULL),

(     12,   0,    -1,   'Audience Management System (AMS)'  ,NULL,      0,    @pMenuID,      '/UAS.Web', 1,    4,    0     ,'',  GetDATE(),NULL    ,9869 ,NULL ,2    ,NULL      ,NULL ,NULL ,NULL),

(12,  0,    -1,   'Email Marketing',      NULL, 0,    @pMenuID,      '/ecn.communicator/main/default.aspx',    1,    7,    0,    '',   GETDATE(),      NULL, 1,    NULL, 3,    NULL, NULL, NULL  ,NULL),

(     12,   0,    -1,   'Unified Audience Database',  NULL, 0,    @pMenuID,   '',   1,      2,    0,    '',   GETDATE(),  NULL, 1,    NULL, 2     ,NULL ,NULL,      NULL,      NULL),

(     12,   0,    -1,   'Surveys',  NULL, 0,    @pMenuID,      '/ecn.collector/main/survey/',      1,    3,    0     ,'',  GETDATE(),  NULL, 1,      NULL, 6     ,NULL,      NULL, NULL  ,NULL),

(     12,   0,    -1,   'Product Reporting',    NULL, 0,    @pMenuID,      'http://wqttest.kmpsgroup.com/login.aspx?',     0,    4,    0     ,'',  GETDATE(),      NULL, 1,    NULL, 11    ,NULL,      NULL, NULL, NULL),

(     12,   0,    -1,   'Digital Editions',     NULL, 0,    @pMenuID,      '/ecn.publisher/main/edition/default.aspx',     0,    5,    0,    '',   GETDATE(),      NULL, 1     ,NULL ,10   ,NULL,      NULL, NULL, NULL),

(     12,   0,    -1,   'Domain Tracking',      NULL, 0,    @pMenuID,      '/ecn.domaintracking/Main/Index',   1,    6,    0,    '',   GETDATE(),  NULL, 1,      NULL, 7,    1,    NULL, NULL, NULL),

(     12,   0,    -1,   'Form Designer'   ,NULL,      0,    @pMenuID    ,'/KMWeb/Forms',      1,    0     ,0,   '',   GETDATE(),  NULL, 1     ,NULL ,4,   NULL, NULL  ,NULL,      NULL),

(     12,   0,    -1,   'Marketing Automation', NULL, 0,    @pMenuID,      '/ecn.marketingautomation/',  1     ,8,   0     ,'',  GETDATE(),  NULL  ,1      ,NULL,      15,   1,    NULL, NULL  ,NULL),

(12,  0,    -1,   'Platform Home',  NULL, 0,    @pMenuID,      '/ecn.accounts/main/default.aspx',  1,    1,    0,    '',   GETDATE(),  NULL, 1,      NULL  ,0    ,NULL,      NULL, NULL, NULL)

 

--below is for adding MA application and MA menu items

 

declare @MAAppID int

insert into Application(ApplicationName,  Description,      ApplicationCode,      DefaultView,      IsActive,   IconFullName,     DateCreated,      DateUpdated,      CreatedByUserID   ,UpdatedByUserID, FromEmailAddress, ErrorEmailAddress)

VALUES('Marketing Automation',      'Marketing Automation', 'MA','', 1, NULL, GETDATE(),      NULL, 1,    NULL, 'ma_PROD@teamkm.com',   'platform-services@TeamKM.com')

select @MAAppID = @@IDENTITY;

 

declare @maHomeMenuID int

 

insert into Menu( ApplicationID,    IsServiceFeature, ServiceFeatureID,      MenuName,Description,   IsParent,   ParentMenuID,     URL   ,IsActive      ,MenuOrder, HasFeatures,      ImagePath,  DateCreated,      DateUpdated,      CreatedByUserID,  UpdatedByUserID,  ServiceID,  IsClientGroupService,      IsSysAdmin, IsChannelAdmin,   IsCustomerAdmin)

VALUES(@MAAppID,  0,    -1,   'Home',     NULL, 1,    0,      '/ecn.accounts/main/default.aspx',  1,    0,    0,    '',   GETDATE(),  NULL, 1,      NULL  ,0    ,NULL,      NULL, NULL, NULL)

select @maHomeMenuID = @@IDENTITY;

 

insert into Menu( ApplicationID,    IsServiceFeature, ServiceFeatureID,      MenuName,Description,   IsParent,   ParentMenuID,     URL   ,IsActive      ,MenuOrder, HasFeatures,      ImagePath,  DateCreated,      DateUpdated,      CreatedByUserID,  UpdatedByUserID,  ServiceID,  IsClientGroupService,      IsSysAdmin, IsChannelAdmin,   IsCustomerAdmin)

VALUES(@MAAppID,  1,    24,   'Data Compare',   NULL, 0,    @maHomeMenuID,      '/uad.datacompare/',    1,    9,    0,    '',   GETDATE(),  NULL  ,1    ,NULL      ,NULL ,NULL ,NULL ,NULL ,NULL),

(     @MAAppID,   0,    -1,   'Audience Management System (AMS)', NULL, 0,      @maHomeMenuID,    '/UAS.Web', 1,    4,    0,    '',   GETDATE(),  NULL, 9869,      NULL, 1,    NULL, NULL, NULL  ,NULL),

(     @MAAppID,   0,    -1,   'Audience Management System (AMS)'  ,NULL,      0,      @maHomeMenuID,    '/UAS.Web', 1,    4,    0     ,'',  GetDATE(),NULL    ,9869      ,NULL ,2    ,NULL ,NULL ,NULL ,NULL),

(@MAAppID,  0,    -1,   'Email Marketing',      NULL, 0,    @maHomeMenuID,      '/ecn.communicator/main/default.aspx',    1,    8,    0,    '',   GETDATE(),      NULL, 1,    NULL, 3,    NULL, NULL, NULL  ,NULL),

(     @MAAppID,   0,    -1,   'Unified Audience Database',  NULL, 0,    @maHomeMenuID,      '',   1,    2,    0,    '',   GETDATE(),  NULL, 1,    NULL, 2     ,NULL ,NULL,      NULL, NULL),

(     @MAAppID,   0,    -1,   'Surveys',  NULL, 0,    @maHomeMenuID,      '/ecn.collector/main/survey/',      1,    3,    0     ,'',  GETDATE(),  NULL, 1,      NULL, 6     ,NULL,      NULL, NULL  ,NULL),

(     @MAAppID,   0,    -1,   'Product Reporting',    NULL, 0,    @maHomeMenuID,      'http://wqttest.kmpsgroup.com/login.aspx?',     0,    4,    0     ,'',  GETDATE(),      NULL, 1,    NULL, 11    ,NULL,      NULL, NULL, NULL),

(     @MAAppID,   0,    -1,   'Digital Editions',     NULL, 0,    @maHomeMenuID,      '/ecn.publisher/main/edition/default.aspx',     0,    5,    0,    '',   GETDATE(),      NULL, 1     ,NULL ,10   ,NULL,      NULL, NULL, NULL),

(     @MAAppID,   0,    -1,   'Domain Tracking',      NULL, 0,    @maHomeMenuID,      '/ecn.domaintracking/Main/Index',   1,    6,    0,    '',   GETDATE(),  NULL, 1,      NULL, 7,    1,    NULL, NULL, NULL),

(     @MAAppID,   0,    -1,   'Form Designer'   ,NULL,      0,    @maHomeMenuID      ,'/KMWeb/Forms',  1,    7     ,0,   '',   GETDATE(),  NULL, 1     ,NULL ,4,      NULL, NULL  ,NULL,      NULL),

(     @MAAppID,   0,    -1,   'Marketing Automation', NULL, 0,    @maHomeMenuID,      '/ecn.marketingautomation/',  1     ,0,   0     ,'',  GETDATE(),  NULL  ,1      ,NULL,      15,   1,    NULL, NULL  ,NULL),

(@MAAppID,  0,    -1,   'Platform Home',  NULL, 0,    @maHomeMenuID,      '/ecn.accounts/main/default.aspx',  1,    0,    1,    '',   GETDATE(),  NULL, 1,      NULL  ,0    ,NULL,      NULL, NULL, NULL)

 