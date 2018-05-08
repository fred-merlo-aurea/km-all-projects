/*
Deployment script for Circulation

BACKUP DATABASE CirculationDeployTest to DISK = '\\10.10.41.250\Backups\MAF\CirculationDeployTest.bak' WITH COMPRESSION, INIT

use master
go
RESTORE DATABASE CirculationDeployTest FROM  DISK = '\\10.10.41.250\Backups\MAF\CirculationDeployTest.bak' WITH REPLACE, RECOVERY

*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;


GO
USE CirculationDeployTest;


GO
/*
The column [dbo].[PriceCode].[Code] is being dropped, data loss could occur.
The column [dbo].[PriceCode].[NumberOfIssues] is being dropped, data loss could occur.
The column [dbo].[PriceCode].[Price] is being dropped, data loss could occur.
*/

--IF EXISTS (select top 1 1 from [dbo].[PriceCode])  RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
/*
The column [dbo].[SubscriberAddKill].[Name] is being dropped, data loss could occur.
The column [dbo].[SubscriberAddKill].[UpdatedUserID] is being dropped, data loss could occur.
The column FilterID on table [dbo].[SubscriberAddKill] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
The type for column FilterID in table [dbo].[SubscriberAddKill] is currently  XML NULL but is being changed to  INT NOT NULL. There is no implicit or explicit conversion.


IF EXISTS (select top 1 1 from [dbo].[SubscriberAddKill])   RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[ActionType])			RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[AddressType])			RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Application])			RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[ApplicationSecurityGroupMap])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Country])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[CreditCardType])		RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[dbfSub])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[dbfsub_MTG])			RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Filters])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[MarketingMap_03232015])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Menu])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[MenuFeature])			RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[MenuFeatureMap])		RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[MenuMap])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[PaymentType])			RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[QualificationSourceType])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Region])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[SecurityGroup])		RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Subscriber_03232015]) RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[SubscriberSourceType]) RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Subscription_03232015]) RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[SubscriptionPaid_03232015])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[SubscriptionResponseMap_03232015])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[User])					RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[UserLogType])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[zzzzzTask])				RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
*/
GO

ALTER TABLE Publication
ADD IsOpenCloseLocked bit Default('false') NULL
go

ALTER TABLE IssueComp
ADD IsActive bit NOT NULL Default 0
go


PRINT N'Rename refactoring operation with key 6d248142-af49-4d2b-b3a6-e13e64babf2f is skipped, element [dbo].[PriceCode].[PriceCode] (SqlSimpleColumn) will not be renamed to PriceCodes';


GO
PRINT N'The following operation was generated from a refactoring log file 72ba3f6f-8648-4185-bc5a-df517a5f4e84';

PRINT N'Rename [dbo].[SubscriberAddKill].[FilterXML] to FilterID';


GO
EXECUTE sp_rename @objname = N'[dbo].[SubscriberAddKill].[FilterXML]', @newname = N'FilterID', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 4cab03aa-3245-4432-abea-3abca15a431f';

PRINT N'Rename [dbo].[SubscriberAddKill].[CreatedUserID] to CreatedByUserID';


GO
EXECUTE sp_rename @objname = N'[dbo].[SubscriberAddKill].[CreatedUserID]', @newname = N'CreatedByUserID', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file d2c2db74-c25e-412f-ae2d-bcf107c7b8e2';

PRINT N'Rename [dbo].[SubscriberAddKill].[CreatedDate] to DateCreated';


GO
EXECUTE sp_rename @objname = N'[dbo].[SubscriberAddKill].[CreatedDate]', @newname = N'DateCreated', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file c7667e3f-7a2c-4616-8f28-0a3fdcea64b9, c8d95739-5879-4671-89db-3f8382537ff8, 8e3b3f7f-cb5c-4d5f-bb6f-77d9aaa28515';

PRINT N'Rename [dbo].[SubscriberAddKill].[UpdatedDate] to DateUpdated';


GO
EXECUTE sp_rename @objname = N'[dbo].[SubscriberAddKill].[UpdatedDate]', @newname = N'DateUpdated', @objtype = N'COLUMN';


GO
PRINT N'Rename refactoring operation with key 137d877f-8d59-4bb9-a4fd-276ab251bc41, 8e3b3f7f-cb5c-4d5f-bb6f-77d9aaa28515 is skipped, element [dbo].[SubscriberAddKill].[UpdatedUserID] (SqlSimpleColumn) will not be renamed to DateUpdated';


GO
PRINT N'Rename refactoring operation with key 215f394a-1bcd-4cd0-9820-f34ccb33aae5 is skipped, element [dbo].[WaveMailing].[ActionID] (SqlSimpleColumn) will not be renamed to WaveMailingID';


GO
PRINT N'Rename refactoring operation with key 9adfd068-e51c-440d-a6bc-760a6b956248 is skipped, element [dbo].[WaveMailing].[IsSubmitted] (SqlSimpleColumn) will not be renamed to DateCreated';


GO
PRINT N'Rename refactoring operation with key 3e08d834-9beb-459e-ad8b-6e4b3c8a7085, 6753563b-be56-480b-bc21-f1f0998a2f06 is skipped, element [dbo].[WaveMailing].[CurrentWave] (SqlSimpleColumn) will not be renamed to WaveNumber';


GO
PRINT N'Rename refactoring operation with key 30edf91b-8731-4e9b-9579-7fd009b747db is skipped, element [dbo].[Subscription].[IMB] (SqlSimpleColumn) will not be renamed to IMBSeq';


GO
PRINT N'Rename refactoring operation with key 24b680ac-0421-4e0b-86bf-0f26fe09bedf is skipped, element [dbo].[Publication].[AcsMailerId] (SqlSimpleColumn) will not be renamed to AcsMailerInfoId';


GO
PRINT N'Rename refactoring operation with key ded35f1c-5747-42cd-bd4b-5f97eb01101d is skipped, element [dbo].[AcsMailerInfo].[Id] (SqlSimpleColumn) will not be renamed to AcsMailerInfoId';


GO
PRINT N'Dropping DF_SubscriptionHistory_Copies...';


GO
ALTER TABLE [dbo].[HistorySubscription] DROP CONSTRAINT [DF_SubscriptionHistory_Copies];


GO
PRINT N'Dropping DF__PriceCode__Numbe__77B1E307...';


GO
ALTER TABLE [dbo].[PriceCode] DROP CONSTRAINT [DF__PriceCode__Numbe__77B1E307];


GO
PRINT N'Dropping DF_Subscriber_IsAddressValidated...';


GO
ALTER TABLE [dbo].[Subscriber] DROP CONSTRAINT [DF_Subscriber_IsAddressValidated];


GO
PRINT N'Dropping DF__Subscribe__IsLoc__49080A04...';


GO
ALTER TABLE [dbo].[Subscriber] DROP CONSTRAINT [DF__Subscribe__IsLoc__49080A04];


GO
PRINT N'Dropping DF_Subscription_IsSubscribed...';


GO
ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT [DF_Subscription_IsSubscribed];


GO
PRINT N'Dropping DF_Subscription_Copies...';


GO
ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT [DF_Subscription_Copies];


GO
PRINT N'Dropping DF_ActionType_IsActive...';


GO
ALTER TABLE [dbo].[ActionType] DROP CONSTRAINT [DF_ActionType_IsActive];


GO
PRINT N'Dropping DF_AddressType_IsActive...';


GO
ALTER TABLE [dbo].[AddressType] DROP CONSTRAINT [DF_AddressType_IsActive];


GO
PRINT N'Dropping DF_Application_IsActive...';


GO
ALTER TABLE [dbo].[Application] DROP CONSTRAINT [DF_Application_IsActive];


GO
PRINT N'Dropping DF_Menu_IsParent...';


GO
ALTER TABLE [dbo].[Menu] DROP CONSTRAINT [DF_Menu_IsParent];


GO
PRINT N'Dropping DF_Menu_IsActive...';


GO
ALTER TABLE [dbo].[Menu] DROP CONSTRAINT [DF_Menu_IsActive];


GO
PRINT N'Dropping DF_Menu_MenuOrder...';


GO
ALTER TABLE [dbo].[Menu] DROP CONSTRAINT [DF_Menu_MenuOrder];


GO
PRINT N'Dropping DF_Menu_HasFeatures...';


GO
ALTER TABLE [dbo].[Menu] DROP CONSTRAINT [DF_Menu_HasFeatures];


GO
PRINT N'Dropping DF_MenuFeatu_IsActive...';


GO
ALTER TABLE [dbo].[MenuFeature] DROP CONSTRAINT [DF_MenuFeatu_IsActive];


GO
PRINT N'Dropping DF_MenuMap_HasAccess...';


GO
ALTER TABLE [dbo].[MenuMap] DROP CONSTRAINT [DF_MenuMap_HasAccess];


GO
PRINT N'Dropping DF_MenuMap_IsActive...';


GO
ALTER TABLE [dbo].[MenuMap] DROP CONSTRAINT [DF_MenuMap_IsActive];


GO
PRINT N'Dropping DF_SecurityGroup_IsActive...';


GO
ALTER TABLE [dbo].[SecurityGroup] DROP CONSTRAINT [DF_SecurityGroup_IsActive];


GO
PRINT N'Dropping DF_SubscriberSourceType_IsActive...';


GO
ALTER TABLE [dbo].[SubscriberSourceType] DROP CONSTRAINT [DF_SubscriberSourceType_IsActive];


GO
PRINT N'Dropping DF_User_DateAdded...';


GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_DateAdded];


GO
PRINT N'Dropping DF_Task_HasSubTask...';


GO
ALTER TABLE [dbo].[zzzzzTask] DROP CONSTRAINT [DF_Task_HasSubTask];


GO
PRINT N'Dropping DF_Task_IsActive...';


GO
ALTER TABLE [dbo].[zzzzzTask] DROP CONSTRAINT [DF_Task_IsActive];


GO
PRINT N'Dropping DF_Task_IsSubTask...';


GO
ALTER TABLE [dbo].[zzzzzTask] DROP CONSTRAINT [DF_Task_IsSubTask];


GO
PRINT N'Dropping DF_Task_ParentTaskID...';


GO
ALTER TABLE [dbo].[zzzzzTask] DROP CONSTRAINT [DF_Task_ParentTaskID];


GO
PRINT N'Dropping DF_Task_IsAdminTask...';


GO
ALTER TABLE [dbo].[zzzzzTask] DROP CONSTRAINT [DF_Task_IsAdminTask];


GO
PRINT N'Dropping FK_SubscriberAddKillDetail_SubscriberAddKill...';


GO
ALTER TABLE [dbo].[SubscriberAddKillDetail] DROP CONSTRAINT [FK_SubscriberAddKillDetail_SubscriberAddKill];


GO
PRINT N'Dropping FK_SubscriberAddKillDetail_Subscription...';


GO
ALTER TABLE [dbo].[SubscriberAddKillDetail] DROP CONSTRAINT [FK_SubscriberAddKillDetail_Subscription];


GO
PRINT N'Dropping [dbo].[dbfSub]...';


GO
DROP TABLE [dbo].[dbfSub];


GO
PRINT N'Dropping [dbo].[dbfsub_MTG]...';


GO
DROP TABLE [dbo].[dbfsub_MTG];


GO


GO
PRINT N'Dropping [dbo].[MarketingMap_03232015]...';


GO
DROP TABLE [dbo].[MarketingMap_03232015];


GO
PRINT N'Dropping [dbo].[Subscriber_03232015]...';


GO
DROP TABLE [dbo].[Subscriber_03232015];


GO
PRINT N'Dropping [dbo].[Subscription_03232015]...';


GO
DROP TABLE [dbo].[Subscription_03232015];


GO
PRINT N'Dropping [dbo].[SubscriptionPaid_03232015]...';


GO
DROP TABLE [dbo].[SubscriptionPaid_03232015];


GO
PRINT N'Dropping [dbo].[SubscriptionResponseMap_03232015]...';


GO
DROP TABLE [dbo].[SubscriptionResponseMap_03232015];


GO
PRINT N'Dropping [dbo].[zzzzzTask]...';


GO
DROP TABLE [dbo].[zzzzzTask];


GO
PRINT N'Dropping [dbo].[e_ActionType_Save]...';


GO
DROP PROCEDURE [dbo].[e_ActionType_Save];


GO
PRINT N'Dropping [dbo].[e_ActionType_Select]...';


GO
DROP PROCEDURE [dbo].[e_ActionType_Select];


GO
PRINT N'Dropping [dbo].[e_AddressType_Save]...';


GO
DROP PROCEDURE [dbo].[e_AddressType_Save];


GO
PRINT N'Dropping [dbo].[e_AddressType_Select]...';


GO
DROP PROCEDURE [dbo].[e_AddressType_Select];


GO
PRINT N'Dropping [dbo].[e_Application_Save]...';


GO
DROP PROCEDURE [dbo].[e_Application_Save];


GO
PRINT N'Dropping [dbo].[e_Application_Select]...';


GO
DROP PROCEDURE [dbo].[e_Application_Select];


GO
PRINT N'Dropping [dbo].[e_Application_Select_UserID]...';


GO
DROP PROCEDURE [dbo].[e_Application_Select_UserID];


GO
PRINT N'Dropping [dbo].[e_ApplicationSecurityGroupMap_Save]...';


GO
DROP PROCEDURE [dbo].[e_ApplicationSecurityGroupMap_Save];


GO
PRINT N'Dropping [dbo].[e_ApplicationSecurityGroupMap_Select]...';


GO
DROP PROCEDURE [dbo].[e_ApplicationSecurityGroupMap_Select];


GO
PRINT N'Dropping [dbo].[e_ApplicationSecurityGroupMap_Select_SecurityGroupID]...';


GO
DROP PROCEDURE [dbo].[e_ApplicationSecurityGroupMap_Select_SecurityGroupID];


GO
PRINT N'Dropping [dbo].[e_Country_Select]...';


GO
DROP PROCEDURE [dbo].[e_Country_Select];


GO
PRINT N'Dropping [dbo].[e_Country_Select_CountryName]...';


GO
DROP PROCEDURE [dbo].[e_Country_Select_CountryName];


GO
PRINT N'Dropping [dbo].[e_CreditCardType_Save]...';


GO
DROP PROCEDURE [dbo].[e_CreditCardType_Save];


GO
PRINT N'Dropping [dbo].[e_CreditCardType_Select]...';


GO
DROP PROCEDURE [dbo].[e_CreditCardType_Select];


GO
PRINT N'Dropping [dbo].[e_Menu_Save]...';


GO
DROP PROCEDURE [dbo].[e_Menu_Save];


GO
PRINT N'Dropping [dbo].[e_Menu_Select]...';


GO
DROP PROCEDURE [dbo].[e_Menu_Select];


GO
PRINT N'Dropping [dbo].[e_Menu_Select_SecurityGroupID]...';


GO
DROP PROCEDURE [dbo].[e_Menu_Select_SecurityGroupID];


GO
PRINT N'Dropping [dbo].[e_MenuFeature_Save]...';


GO
DROP PROCEDURE [dbo].[e_MenuFeature_Save];


GO
PRINT N'Dropping [dbo].[e_MenuFeature_Select]...';


GO
DROP PROCEDURE [dbo].[e_MenuFeature_Select];


GO
PRINT N'Dropping [dbo].[e_MenuFeatureMap_Save]...';


GO
DROP PROCEDURE [dbo].[e_MenuFeatureMap_Save];


GO
PRINT N'Dropping [dbo].[e_MenuFeatureMap_Select]...';


GO
DROP PROCEDURE [dbo].[e_MenuFeatureMap_Select];


GO
PRINT N'Dropping [dbo].[e_MenuMap_Save]...';


GO
DROP PROCEDURE [dbo].[e_MenuMap_Save];


GO
PRINT N'Dropping [dbo].[e_MenuMap_Select]...';


GO
DROP PROCEDURE [dbo].[e_MenuMap_Select];


GO
PRINT N'Dropping [dbo].[e_MenuMap_Select_SecurityGroupID]...';


GO
DROP PROCEDURE [dbo].[e_MenuMap_Select_SecurityGroupID];


GO
PRINT N'Dropping [dbo].[e_PaymentType_Save]...';


GO
DROP PROCEDURE [dbo].[e_PaymentType_Save];


GO
PRINT N'Dropping [dbo].[e_PaymentType_Select]...';


GO
DROP PROCEDURE [dbo].[e_PaymentType_Select];


GO
PRINT N'Dropping [dbo].[e_QualificationSourceType_Save]...';


GO
DROP PROCEDURE [dbo].[e_QualificationSourceType_Save];


GO
PRINT N'Dropping [dbo].[e_QualificationSourceType_Select]...';


GO
DROP PROCEDURE [dbo].[e_QualificationSourceType_Select];


GO
PRINT N'Dropping [dbo].[e_Region_Select]...';


GO
DROP PROCEDURE [dbo].[e_Region_Select];


GO
PRINT N'Dropping [dbo].[e_SecurityGroup_Save]...';


GO
DROP PROCEDURE [dbo].[e_SecurityGroup_Save];


GO
PRINT N'Dropping [dbo].[e_SecurityGroup_Select]...';


GO
DROP PROCEDURE [dbo].[e_SecurityGroup_Select];


GO
PRINT N'Dropping [dbo].[e_Subscriber_Search_Params_2]...';


GO
DROP PROCEDURE [dbo].[e_Subscriber_Search_Params_2];


GO
PRINT N'Dropping [dbo].[e_SubscriberSourceType_Select]...';


GO
DROP PROCEDURE [dbo].[e_SubscriberSourceType_Select];


GO
PRINT N'Dropping [dbo].[e_User_LogIn]...';


GO
DROP PROCEDURE [dbo].[e_User_LogIn];


GO
PRINT N'Dropping [dbo].[e_User_Save]...';


GO
DROP PROCEDURE [dbo].[e_User_Save];


GO
PRINT N'Dropping [dbo].[e_User_Search_Email]...';


GO
DROP PROCEDURE [dbo].[e_User_Search_Email];


GO
PRINT N'Dropping [dbo].[e_User_Search_UserName]...';


GO
DROP PROCEDURE [dbo].[e_User_Search_UserName];


GO
PRINT N'Dropping [dbo].[e_User_Select]...';


GO
DROP PROCEDURE [dbo].[e_User_Select];


GO
PRINT N'Dropping [dbo].[e_User_Select_UserID]...';


GO
DROP PROCEDURE [dbo].[e_User_Select_UserID];


GO
PRINT N'Dropping [dbo].[e_UserLogType_Save]...';


GO
DROP PROCEDURE [dbo].[e_UserLogType_Save];


GO
PRINT N'Dropping [dbo].[e_UserLogType_Select]...';


GO
DROP PROCEDURE [dbo].[e_UserLogType_Select];


GO
PRINT N'Dropping [dbo].[ActionType]...';


GO
DROP TABLE [dbo].[ActionType];


GO
PRINT N'Dropping [dbo].[AddressType]...';


GO
DROP TABLE [dbo].[AddressType];


GO
PRINT N'Dropping [dbo].[Application]...';


GO
DROP TABLE [dbo].[Application];


GO
PRINT N'Dropping [dbo].[ApplicationSecurityGroupMap]...';


GO
DROP TABLE [dbo].[ApplicationSecurityGroupMap];


GO
PRINT N'Dropping [dbo].[Country]...';


GO
DROP TABLE [dbo].[Country];


GO
PRINT N'Dropping [dbo].[CreditCardType]...';


GO
DROP TABLE [dbo].[CreditCardType];


GO
PRINT N'Dropping [dbo].[Menu]...';


GO
DROP TABLE [dbo].[Menu];


GO
PRINT N'Dropping [dbo].[MenuFeature]...';


GO
DROP TABLE [dbo].[MenuFeature];


GO
PRINT N'Dropping [dbo].[MenuFeatureMap]...';


GO
DROP TABLE [dbo].[MenuFeatureMap];


GO
PRINT N'Dropping [dbo].[MenuMap]...';


GO
DROP TABLE [dbo].[MenuMap];


GO
PRINT N'Dropping [dbo].[PaymentType]...';


GO
DROP TABLE [dbo].[PaymentType];


GO

GO
PRINT N'Dropping [dbo].[Region]...';


GO
DROP TABLE [dbo].[Region];


GO
PRINT N'Dropping [dbo].[SecurityGroup]...';


GO
DROP TABLE [dbo].[SecurityGroup];


GO
PRINT N'Dropping [dbo].[SubscriberSourceType]...';


GO
DROP TABLE [dbo].[SubscriberSourceType];


GO
PRINT N'Dropping [dbo].[User]...';


GO
DROP TABLE [dbo].[User];


GO
PRINT N'Dropping [dbo].[UserLogType]...';


GO
DROP TABLE [dbo].[UserLogType];


GO
PRINT N'Altering [dbo].[Batch]...';


GO
ALTER TABLE [dbo].[Batch]
    ADD [BatchNumber] INT NULL;


GO
PRINT N'Starting rebuilding table [dbo].[HistoryPaidBillTo]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_HistoryPaidBillTo] (
    [HistoryPaidBillToID]      INT              IDENTITY (1, 1) NOT NULL,
    [PaidBillToID]             INT              NOT NULL,
    [SubscriptionPaidID]       INT              NOT NULL,
    [SubscriptionID]           INT              NOT NULL,
    [FirstName]                VARCHAR (50)     NULL,
    [LastName]                 VARCHAR (50)     NULL,
    [Company]                  VARCHAR (100)    NULL,
    [Title]                    VARCHAR (255)    NULL,
    [AddressTypeID]            INT              NULL,
    [Address1]                 VARCHAR (100)    NULL,
    [Address2]                 VARCHAR (100)    NULL,
    [Address3]                 VARCHAR (100)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [RegionCode]               VARCHAR (50)     NULL,
    [RegionID]                 INT              NULL,
    [ZipCode]                  VARCHAR (50)     NULL,
    [Plus4]                    CHAR (10)        NULL,
    [CarrierRoute]             VARCHAR (10)     NULL,
    [County]                   VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [CountryID]                INT              NULL,
    [Latitude]                 DECIMAL (18, 15) NULL,
    [Longitude]                DECIMAL (18, 15) NULL,
    [IsAddressValidated]       BIT              NOT NULL,
    [AddressValidationDate]    DATETIME         NULL,
    [AddressValidationSource]  VARCHAR (50)     NULL,
    [AddressValidationMessage] VARCHAR (MAX)    NULL,
    [Email]                    VARCHAR (255)    NULL,
    [Phone]                    CHAR (50)        NULL,
    [Fax]                      CHAR (50)        NULL,
    [Mobile]                   CHAR (50)        NULL,
    [Website]                  VARCHAR (255)    NULL,
    [DateCreated]              DATETIME         NOT NULL,
    [CreatedByUserID]          INT              NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_HistoryPaidBillTo] PRIMARY KEY CLUSTERED ([HistoryPaidBillToID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[HistoryPaidBillTo])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_HistoryPaidBillTo] ON;
        INSERT INTO [dbo].[tmp_ms_xx_HistoryPaidBillTo] ([HistoryPaidBillToID], [PaidBillToID], [SubscriptionPaidID], [SubscriptionID], [FirstName], [LastName], [Company], [Title], [AddressTypeID], [Address1], [Address2], [City], [RegionCode], [RegionID], [ZipCode], [Plus4], [CarrierRoute], [County], [Country], [CountryID], [Latitude], [Longitude], [IsAddressValidated], [AddressValidationDate], [AddressValidationSource], [AddressValidationMessage], [Email], [Phone], [Fax], [Mobile], [Website], [DateCreated], [CreatedByUserID])
        SELECT   [HistoryPaidBillToID],
                 [PaidBillToID],
                 [SubscriptionPaidID],
                 [SubscriptionID],
                 [FirstName],
                 [LastName],
                 [Company],
                 [Title],
                 [AddressTypeID],
                 [Address1],
                 [Address2],
                 [City],
                 [RegionCode],
                 [RegionID],
                 [ZipCode],
                 [Plus4],
                 [CarrierRoute],
                 [County],
                 [Country],
                 [CountryID],
                 [Latitude],
                 [Longitude],
                 [IsAddressValidated],
                 [AddressValidationDate],
                 [AddressValidationSource],
                 [AddressValidationMessage],
                 [Email],
                 [Phone],
                 [Fax],
                 [Mobile],
                 [Website],
                 [DateCreated],
                 [CreatedByUserID]
        FROM     [dbo].[HistoryPaidBillTo]
        ORDER BY [HistoryPaidBillToID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_HistoryPaidBillTo] OFF;
    END

DROP TABLE [dbo].[HistoryPaidBillTo];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_HistoryPaidBillTo]', N'HistoryPaidBillTo';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_HistoryPaidBillTo]', N'PK_HistoryPaidBillTo', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[HistorySubscription]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_HistorySubscription] (
    [HistorySubscriptionID]        INT              IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]               INT              NOT NULL,
    [PublisherID]                  INT              NOT NULL,
    [SubscriberID]                 INT              NOT NULL,
    [PublicationID]                INT              NOT NULL,
    [ActionID_Current]             INT              NOT NULL,
    [ActionID_Previous]            INT              NULL,
    [SubscriptionStatusID]         INT              NOT NULL,
    [IsPaid]                       BIT              NOT NULL,
    [QSourceID]                    INT              NULL,
    [QSourceDate]                  DATE             NULL,
    [DeliverabilityID]             INT              NULL,
    [IsSubscribed]                 BIT              NOT NULL,
    [SubscriberSourceCode]         VARCHAR (256)    NULL,
    [Copies]                       INT              CONSTRAINT [DF_SubscriptionHistory_Copies] DEFAULT ((1)) NOT NULL,
    [OriginalSubscriberSourceCode] VARCHAR (256)    NULL,
    [SubscriptionDateCreated]      DATETIME         NOT NULL,
    [SubscriptionDateUpdated]      DATETIME         NULL,
    [SubscriptionCreatedByUserID]  INT              NOT NULL,
    [SubscriptionUpdatedByUserID]  INT              NULL,
    [AccountNumber]                VARCHAR (50)     NULL,
    [GraceIssues]                  INT              NULL,
    [IsNewSubscription]            INT              NULL,
    [MemberGroup]                  VARCHAR (256)    NULL,
    [OnBehalfOf]                   VARCHAR (256)    NULL,
    [Par3cID]                      INT              NULL,
    [SequenceID]                   INT              NULL,
    [SubsrcTypeID]                 INT              NULL,
    [Verify]                       VARCHAR (256)    NULL,
    [IsActive]                     BIT              NULL,
    [ExternalKeyID]                INT              NULL,
    [FirstName]                    VARCHAR (50)     NULL,
    [LastName]                     VARCHAR (50)     NULL,
    [Company]                      VARCHAR (100)    NULL,
    [Title]                        VARCHAR (255)    NULL,
    [Occupation]                   VARCHAR (50)     NULL,
    [AddressTypeID]                INT              NULL,
    [Address1]                     VARCHAR (100)    NULL,
    [Address2]                     VARCHAR (100)    NULL,
    [Address3]                     VARCHAR (100)    NULL,
    [City]                         VARCHAR (50)     NULL,
    [RegionCode]                   VARCHAR (50)     NULL,
    [RegionID]                     INT              NULL,
    [ZipCode]                      VARCHAR (50)     NULL,
    [Plus4]                        CHAR (10)        NULL,
    [CarrierRoute]                 VARCHAR (10)     NULL,
    [County]                       VARCHAR (50)     NULL,
    [Country]                      VARCHAR (50)     NULL,
    [CountryID]                    INT              NULL,
    [Latitude]                     DECIMAL (18, 15) NULL,
    [Longitude]                    DECIMAL (18, 15) NULL,
    [IsAddressValidated]           BIT              NOT NULL,
    [AddressValidationDate]        DATETIME         NULL,
    [AddressValidationSource]      VARCHAR (50)     NULL,
    [AddressValidationMessage]     VARCHAR (MAX)    NULL,
    [Email]                        VARCHAR (255)    NULL,
    [Phone]                        VARCHAR (25)     NULL,
    [Fax]                          VARCHAR (25)     NULL,
    [Mobile]                       VARCHAR (25)     NULL,
    [Website]                      VARCHAR (255)    NULL,
    [Birthdate]                    DATE             NULL,
    [Age]                          INT              NULL,
    [Income]                       VARCHAR (50)     NULL,
    [Gender]                       VARCHAR (50)     NULL,
    [SubscriberDateCreated]        DATETIME         NOT NULL,
    [SubscriberDateUpdated]        DATETIME         NULL,
    [SubscriberCreatedByUserID]    INT              NOT NULL,
    [SubscriberUpdatedByUserID]    INT              NULL,
    [DateCreated]                  DATETIME         NOT NULL,
    [CreatedByUserID]              INT              NOT NULL,
    [IsLocked]                     BIT              NULL,
    [LockDate]                     DATETIME         NULL,
    [LockDateRelease]              DATETIME         NULL,
    [LockedByUserID]               INT              NULL,
    [PhoneExt]                     VARCHAR (25)     NULL,
    [IsUadUpdated]                 BIT              CONSTRAINT [DF_HistorySubscription_IsUadUpdated] DEFAULT ((0)) NOT NULL,
    [UadUpdatedDate]               DATETIME         NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_HistorySubscription] PRIMARY KEY CLUSTERED ([HistorySubscriptionID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[HistorySubscription])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_HistorySubscription] ON;
        INSERT INTO [dbo].[tmp_ms_xx_HistorySubscription] ([HistorySubscriptionID], [SubscriptionID], [PublisherID], [SubscriberID], [PublicationID], [ActionID_Current], [ActionID_Previous], [SubscriptionStatusID], [IsPaid], [QSourceID], [QSourceDate], [DeliverabilityID], [IsSubscribed], [SubscriberSourceCode], [Copies], [OriginalSubscriberSourceCode], [SubscriptionDateCreated], [SubscriptionDateUpdated], [SubscriptionCreatedByUserID], [SubscriptionUpdatedByUserID], [ExternalKeyID], [FirstName], [LastName], [Company], [Title], [Occupation], [AddressTypeID], [Address1], [Address2], [City], [RegionCode], [RegionID], [ZipCode], [Plus4], [CarrierRoute], [County], [Country], [CountryID], [Latitude], [Longitude], [IsAddressValidated], [AddressValidationDate], [AddressValidationSource], [AddressValidationMessage], [Email], [Phone], [Fax], [Mobile], [Website], [Birthdate], [Age], [Income], [Gender], [SubscriberDateCreated], [SubscriberDateUpdated], [SubscriberCreatedByUserID], [SubscriberUpdatedByUserID], [DateCreated], [CreatedByUserID])
        SELECT   [HistorySubscriptionID],
                 [SubscriptionID],
                 [PublisherID],
                 [SubscriberID],
                 [PublicationID],
                 [ActionID_Current],
                 [ActionID_Previous],
                 [SubscriptionStatusID],
                 [IsPaid],
                 [QSourceID],
                 [QSourceDate],
                 [DeliverabilityID],
                 [IsSubscribed],
                 [SubscriberSourceCode],
                 [Copies],
                 [OriginalSubscriberSourceCode],
                 [SubscriptionDateCreated],
                 [SubscriptionDateUpdated],
                 [SubscriptionCreatedByUserID],
                 [SubscriptionUpdatedByUserID],
                 [ExternalKeyID],
                 [FirstName],
                 [LastName],
                 [Company],
                 [Title],
                 [Occupation],
                 [AddressTypeID],
                 [Address1],
                 [Address2],
                 [City],
                 [RegionCode],
                 [RegionID],
                 [ZipCode],
                 [Plus4],
                 [CarrierRoute],
                 [County],
                 [Country],
                 [CountryID],
                 [Latitude],
                 [Longitude],
                 [IsAddressValidated],
                 [AddressValidationDate],
                 [AddressValidationSource],
                 [AddressValidationMessage],
                 [Email],
                 [Phone],
                 [Fax],
                 [Mobile],
                 [Website],
                 [Birthdate],
                 [Age],
                 [Income],
                 [Gender],
                 [SubscriberDateCreated],
                 [SubscriberDateUpdated],
                 [SubscriberCreatedByUserID],
                 [SubscriberUpdatedByUserID],
                 [DateCreated],
                 [CreatedByUserID]
        FROM     [dbo].[HistorySubscription]
        ORDER BY [HistorySubscriptionID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_HistorySubscription] OFF;
    END

DROP TABLE [dbo].[HistorySubscription];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_HistorySubscription]', N'HistorySubscription';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_HistorySubscription]', N'PK_HistorySubscription', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PaidBillTo]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PaidBillTo] (
    [PaidBillToID]             INT              IDENTITY (1, 1) NOT NULL,
    [SubscriptionPaidID]       INT              NOT NULL,
    [SubscriptionID]           INT              NOT NULL,
    [FirstName]                VARCHAR (50)     NULL,
    [LastName]                 VARCHAR (50)     NULL,
    [Company]                  VARCHAR (100)    NULL,
    [Title]                    VARCHAR (255)    NULL,
    [AddressTypeID]            INT              NULL,
    [Address1]                 VARCHAR (100)    NULL,
    [Address2]                 VARCHAR (100)    NULL,
    [Address3]                 VARCHAR (100)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [RegionCode]               VARCHAR (50)     NULL,
    [RegionID]                 INT              NULL,
    [ZipCode]                  VARCHAR (50)     NULL,
    [Plus4]                    VARCHAR (10)     NULL,
    [CarrierRoute]             VARCHAR (10)     NULL,
    [County]                   VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [CountryID]                INT              NULL,
    [Latitude]                 DECIMAL (18, 15) NULL,
    [Longitude]                DECIMAL (18, 15) NULL,
    [IsAddressValidated]       BIT              NOT NULL,
    [AddressValidationDate]    DATETIME         NULL,
    [AddressValidationSource]  VARCHAR (50)     NULL,
    [AddressValidationMessage] VARCHAR (MAX)    NULL,
    [Email]                    VARCHAR (255)    NULL,
    [Phone]                    VARCHAR (50)     NULL,
    [PhoneExt]                 VARCHAR (25)     NULL,
    [Fax]                      VARCHAR (50)     NULL,
    [Mobile]                   VARCHAR (50)     NULL,
    [Website]                  VARCHAR (255)    NULL,
    [DateCreated]              DATETIME         NOT NULL,
    [DateUpdated]              DATETIME         NULL,
    [CreatedByUserID]          INT              NOT NULL,
    [UpdatedByUserID]          INT              NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PaidBillTo] PRIMARY KEY CLUSTERED ([PaidBillToID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PaidBillTo])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PaidBillTo] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PaidBillTo] ([PaidBillToID], [SubscriptionPaidID], [SubscriptionID], [FirstName], [LastName], [Company], [Title], [AddressTypeID], [Address1], [Address2], [City], [RegionCode], [RegionID], [ZipCode], [Plus4], [CarrierRoute], [County], [Country], [CountryID], [Latitude], [Longitude], [IsAddressValidated], [AddressValidationDate], [AddressValidationSource], [AddressValidationMessage], [Email], [Phone], [PhoneExt], [Fax], [Mobile], [Website], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID])
        SELECT   [PaidBillToID],
                 [SubscriptionPaidID],
                 [SubscriptionID],
                 [FirstName],
                 [LastName],
                 [Company],
                 [Title],
                 [AddressTypeID],
                 [Address1],
                 [Address2],
                 [City],
                 [RegionCode],
                 [RegionID],
                 [ZipCode],
                 [Plus4],
                 [CarrierRoute],
                 [County],
                 [Country],
                 [CountryID],
                 [Latitude],
                 [Longitude],
                 [IsAddressValidated],
                 [AddressValidationDate],
                 [AddressValidationSource],
                 [AddressValidationMessage],
                 [Email],
                 [Phone],
                 [PhoneExt],
                 [Fax],
                 [Mobile],
                 [Website],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID]
        FROM     [dbo].[PaidBillTo]
        ORDER BY [PaidBillToID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PaidBillTo] OFF;
    END

DROP TABLE [dbo].[PaidBillTo];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PaidBillTo]', N'PaidBillTo';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PaidBillTo]', N'PK_PaidBillTo', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PriceCode]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PriceCode] (
    [PriceCodeID]      INT             IDENTITY (1, 1) NOT NULL,
    [PublicationID]    INT             NOT NULL,
    [PriceCodes]       VARCHAR (50)    NULL,
    [Term]             INT             NULL,
    [US_CopyRate]      DECIMAL (18, 2) NULL,
    [CAN_CopyRate]     DECIMAL (18, 2) NULL,
    [FOR_CopyRate]     DECIMAL (18, 2) NULL,
    [US_Price]         DECIMAL (18, 2) NULL,
    [CAN_Price]        DECIMAL (18, 2) NULL,
    [FOR_Price]        DECIMAL (18, 2) NULL,
    [QFOfferCode]      VARCHAR (256)   NULL,
    [FoxProPriceCode]  VARCHAR (256)   NULL,
    [Description]      VARCHAR (256)   NULL,
    [DeliverabilityID] INT             NULL,
    [TotalIssues]      INT             DEFAULT ((0)) NULL,
    [IsActive]         BIT             NOT NULL,
    [DateCreated]      DATETIME        NOT NULL,
    [DateUpdated]      DATETIME        NULL,
    [CreatedByUserID]  INT             NOT NULL,
    [UpdatedByUserID]  INT             NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PriceCode] PRIMARY KEY CLUSTERED ([PriceCodeID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PriceCode])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PriceCode] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PriceCode] ([PriceCodeID], [PublicationID], [Term], [IsActive], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID])
        SELECT   [PriceCodeID],
                 [PublicationID],
                 [Term],
                 [IsActive],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID]
        FROM     [dbo].[PriceCode]
        ORDER BY [PriceCodeID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PriceCode] OFF;
    END

DROP TABLE [dbo].[PriceCode];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PriceCode]', N'PriceCode';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PriceCode]', N'PK_PriceCode', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Publication]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Publication] (
    [PublicationID]       INT          IDENTITY (1, 1) NOT NULL,
    [PublicationName]     VARCHAR (50) NOT NULL,
    [PublicationCode]     VARCHAR (50) NOT NULL,
    [PublisherID]         INT          NOT NULL,
    [YearStartDate]       CHAR (5)     NOT NULL,
    [YearEndDate]         CHAR (5)     NOT NULL,
    [IssueDate]           DATE         NULL,
    [IsImported]          BIT          NOT NULL,
    [IsActive]            BIT          NOT NULL,
    [AllowDataEntry]      BIT          NOT NULL,
    [FrequencyID]         INT          NULL,
    [KMImportAllowed]     BIT          DEFAULT ('false') NOT NULL,
    [ClientImportAllowed] BIT          DEFAULT ('false') NOT NULL,
    [AddRemoveAllowed]    BIT          DEFAULT ('false') NOT NULL,
    [AcsMailerInfoId]     INT          NULL,
    [DateCreated]         DATETIME     NOT NULL,
    [DateUpdated]         DATETIME     NULL,
    [CreatedByUserID]     INT          NOT NULL,
    [UpdatedByUserID]     INT          NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Publication] PRIMARY KEY CLUSTERED ([PublicationCode] ASC, [PublisherID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Publication])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Publication] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Publication] ([PublicationCode], [PublisherID], [PublicationID], [PublicationName], [YearStartDate], [YearEndDate], [IssueDate], [IsImported], [IsActive], [AllowDataEntry], [FrequencyID], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID])
        SELECT   [PublicationCode],
                 [PublisherID],
                 [PublicationID],
                 [PublicationName],
                 [YearStartDate],
                 [YearEndDate],
                 [IssueDate],
                 [IsImported],
                 [IsActive],
                 [AllowDataEntry],
                 [FrequencyID],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID]
        FROM     [dbo].[Publication]
        ORDER BY [PublicationCode] ASC, [PublisherID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Publication] OFF;
    END

DROP TABLE [dbo].[Publication];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Publication]', N'Publication';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Publication]', N'PK_Publication', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Altering [dbo].[Response]...';


GO
ALTER TABLE [dbo].[Response]
    ADD [IsOther] BIT NULL;


GO
PRINT N'Starting rebuilding table [dbo].[Subscriber]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Subscriber] (
    [SubscriberID]                   INT              IDENTITY (1, 1) NOT NULL,
    [ExternalKeyID]                  INT              NULL,
    [FirstName]                      VARCHAR (50)     NULL,
    [LastName]                       VARCHAR (50)     NULL,
    [Company]                        VARCHAR (100)    NULL,
    [Title]                          VARCHAR (255)    NULL,
    [Occupation]                     VARCHAR (50)     NULL,
    [AddressTypeID]                  INT              NULL,
    [Address1]                       VARCHAR (100)    NULL,
    [Address2]                       VARCHAR (100)    NULL,
    [Address3]                       VARCHAR (100)    NULL,
    [City]                           VARCHAR (50)     NULL,
    [RegionCode]                     VARCHAR (50)     NULL,
    [RegionID]                       INT              NULL,
    [ZipCode]                        VARCHAR (50)     NULL,
    [Plus4]                          VARCHAR (10)     NULL,
    [CarrierRoute]                   VARCHAR (10)     NULL,
    [County]                         VARCHAR (50)     NULL,
    [Country]                        VARCHAR (50)     NULL,
    [CountryID]                      INT              NULL,
    [Latitude]                       DECIMAL (18, 15) NULL,
    [Longitude]                      DECIMAL (18, 15) NULL,
    [IsAddressValidated]             BIT              CONSTRAINT [DF_Subscriber_IsAddressValidated] DEFAULT ((0)) NOT NULL,
    [AddressValidationDate]          DATETIME         NULL,
    [AddressValidationSource]        VARCHAR (50)     NULL,
    [AddressValidationMessage]       VARCHAR (MAX)    NULL,
    [Email]                          VARCHAR (255)    NULL,
    [Phone]                          VARCHAR (25)     NULL,
    [Fax]                            VARCHAR (25)     NULL,
    [Mobile]                         VARCHAR (25)     NULL,
    [Website]                        VARCHAR (255)    NULL,
    [Birthdate]                      DATE             NULL,
    [Age]                            INT              NULL,
    [Income]                         VARCHAR (50)     NULL,
    [Gender]                         VARCHAR (50)     NULL,
    [DateCreated]                    DATETIME         NOT NULL,
    [DateUpdated]                    DATETIME         NULL,
    [CreatedByUserID]                INT              NOT NULL,
    [UpdatedByUserID]                INT              NULL,
    [tmpSubscriptionID]              INT              NULL,
    [IsLocked]                       BIT              DEFAULT ((0)) NOT NULL,
    [LockedByUserID]                 INT              NULL,
    [LockDate]                       DATETIME         NULL,
    [LockDateRelease]                DATETIME         NULL,
    [PhoneExt]                       VARCHAR (25)     NULL,
    [IsInActiveWaveMailing]          BIT              DEFAULT ((0)) NOT NULL,
    [AddressTypeCodeId]              INT              NULL,
    [AddressLastUpdatedDate]         DATETIME         NULL,
    [AddressUpdatedSourceTypeCodeId] INT              NULL,
    [WaveMailingID]                  INT              NULL,
    [IGrp_No]                        UNIQUEIDENTIFIER NULL,
    [SFRecordIdentifier]             UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Subscriber] PRIMARY KEY CLUSTERED ([SubscriberID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Subscriber])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Subscriber] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Subscriber] ([SubscriberID], [ExternalKeyID], [FirstName], [LastName], [Company], [Title], [Occupation], [AddressTypeID], [Address1], [Address2], [City], [RegionCode], [RegionID], [ZipCode], [Plus4], [CarrierRoute], [County], [Country], [CountryID], [Latitude], [Longitude], [IsAddressValidated], [AddressValidationDate], [AddressValidationSource], [AddressValidationMessage], [Email], [Phone], [Fax], [Mobile], [Website], [Birthdate], [Age], [Income], [Gender], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID], [tmpSubscriptionID], [IsLocked], [PhoneExt])
        SELECT   [SubscriberID],
                 [ExternalKeyID],
                 [FirstName],
                 [LastName],
                 [Company],
                 [Title],
                 [Occupation],
                 [AddressTypeID],
                 [Address1],
                 [Address2],
                 [City],
                 [RegionCode],
                 [RegionID],
                 [ZipCode],
                 [Plus4],
                 [CarrierRoute],
                 [County],
                 [Country],
                 [CountryID],
                 [Latitude],
                 [Longitude],
                 [IsAddressValidated],
                 [AddressValidationDate],
                 [AddressValidationSource],
                 [AddressValidationMessage],
                 [Email],
                 [Phone],
                 [Fax],
                 [Mobile],
                 [Website],
                 [Birthdate],
                 [Age],
                 [Income],
                 [Gender],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID],
                 [tmpSubscriptionID],
                 [IsLocked],
                 [PhoneExt]
        FROM     [dbo].[Subscriber]
        ORDER BY [SubscriberID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Subscriber] OFF;
    END

DROP TABLE [dbo].[Subscriber];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Subscriber]', N'Subscriber';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Subscriber]', N'PK_Subscriber', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[Subscriber].[IDX_Address1]...';


GO
CREATE NONCLUSTERED INDEX [IDX_Address1]
    ON [dbo].[Subscriber]([Address1] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Creating [dbo].[Subscriber].[IDX_City]...';


GO
CREATE NONCLUSTERED INDEX [IDX_City]
    ON [dbo].[Subscriber]([City] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Creating [dbo].[Subscriber].[IDX_Company]...';


GO
CREATE NONCLUSTERED INDEX [IDX_Company]
    ON [dbo].[Subscriber]([Company] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Creating [dbo].[Subscriber].[IDX_Email]...';


GO
CREATE NONCLUSTERED INDEX [IDX_Email]
    ON [dbo].[Subscriber]([Email] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Creating [dbo].[Subscriber].[IDX_FirstName]...';


GO
CREATE NONCLUSTERED INDEX [IDX_FirstName]
    ON [dbo].[Subscriber]([FirstName] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Creating [dbo].[Subscriber].[IDX_LastName]...';


GO
CREATE NONCLUSTERED INDEX [IDX_LastName]
    ON [dbo].[Subscriber]([LastName] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Creating [dbo].[Subscriber].[IDX_ZipCode]...';


GO
CREATE NONCLUSTERED INDEX [IDX_ZipCode]
    ON [dbo].[Subscriber]([ZipCode] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Starting rebuilding table [dbo].[SubscriberAddKill]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_SubscriberAddKill] (
    [AddKillID]       INT          IDENTITY (1, 1) NOT NULL,
    [PublicationID]   INT          NULL,
    [FilterID]        INT          NOT NULL,
    [Count]           INT          NULL,
    [AddKillCount]    INT          NULL,
    [Type]            VARCHAR (50) NULL,
    [IsActive]        BIT          DEFAULT 1 NOT NULL,
    [CreatedByUserID] INT          NULL,
    [DateCreated]     DATETIME     NULL,
    [UpdatedByUserID] INT          NULL,
    [DateUpdated]     DATETIME     NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_SubscriberAddKill] PRIMARY KEY CLUSTERED ([AddKillID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SubscriberAddKill])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SubscriberAddKill] ON;
        INSERT INTO [dbo].[tmp_ms_xx_SubscriberAddKill] ([AddKillID], [PublicationID], [FilterID], [Count], [AddKillCount], [Type], [CreatedByUserID], [DateCreated], [DateUpdated])
        SELECT   [AddKillID],
                 [PublicationID],
                 [FilterID],
                 [Count],
                 [AddKillCount],
                 [Type],
                 [CreatedByUserID],
                 [DateCreated],
                 [DateUpdated]
        FROM     [dbo].[SubscriberAddKill]
        ORDER BY [AddKillID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SubscriberAddKill] OFF;
    END

DROP TABLE [dbo].[SubscriberAddKill];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SubscriberAddKill]', N'SubscriberAddKill';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_SubscriberAddKill]', N'PK_SubscriberAddKill', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[Subscription]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Subscription] (
    [SubscriptionID]               INT           IDENTITY (1, 1) NOT NULL,
    [SequenceID]                   INT           NULL,
    [PublisherID]                  INT           NOT NULL,
    [SubscriberID]                 INT           NOT NULL,
    [PublicationID]                INT           NOT NULL,
    [ActionID_Current]             INT           NOT NULL,
    [ActionID_Previous]            INT           NULL,
    [SubscriptionStatusID]         INT           NULL,
    [IsPaid]                       BIT           NOT NULL,
    [QSourceID]                    INT           NULL,
    [QSourceDate]                  DATE          NULL,
    [DeliverabilityID]             INT           NULL,
    [IsSubscribed]                 BIT           CONSTRAINT [DF_Subscription_IsSubscribed] DEFAULT ((1)) NOT NULL,
    [SubscriberSourceCode]         VARCHAR (256) NULL,
    [Copies]                       INT           CONSTRAINT [DF_Subscription_Copies] DEFAULT ((1)) NOT NULL,
    [OriginalSubscriberSourceCode] VARCHAR (256) NULL,
    [DateCreated]                  DATETIME      NOT NULL,
    [DateUpdated]                  DATETIME      NULL,
    [CreatedByUserID]              INT           NOT NULL,
    [UpdatedByUserID]              INT           NULL,
    [Par3cID]                      INT           NULL,
    [SubsrcTypeID]                 INT           NULL,
    [AccountNumber]                VARCHAR (50)  NULL,
    [GraceIssues]                  INT           NULL,
    [OnBehalfOf]                   VARCHAR (256) NULL,
    [MemberGroup]                  VARCHAR (256) NULL,
    [Verify]                       VARCHAR (256) NULL,
    [IsNewSubscription]            BIT           NULL,
    [AddRemoveID]                  INT           NULL,
    [IMBSeq]                       VARCHAR (10)  NULL,
    [IsActive]                     BIT           NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Subscription] PRIMARY KEY CLUSTERED ([SubscriberID] ASC, [PublicationID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Subscription])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Subscription] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Subscription] ([SubscriberID], [PublicationID], [SubscriptionID], [SequenceID], [PublisherID], [ActionID_Current], [ActionID_Previous], [SubscriptionStatusID], [IsPaid], [QSourceID], [QSourceDate], [DeliverabilityID], [IsSubscribed], [SubscriberSourceCode], [Copies], [OriginalSubscriberSourceCode], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID], [Par3cID], [SubsrcTypeID], [AccountNumber], [OnBehalfOf], [MemberGroup], [Verify])
        SELECT   [SubscriberID],
                 [PublicationID],
                 [SubscriptionID],
                 [SequenceID],
                 [PublisherID],
                 [ActionID_Current],
                 [ActionID_Previous],
                 [SubscriptionStatusID],
                 [IsPaid],
                 [QSourceID],
                 [QSourceDate],
                 [DeliverabilityID],
                 [IsSubscribed],
                 [SubscriberSourceCode],
                 [Copies],
                 [OriginalSubscriberSourceCode],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID],
                 [Par3cID],
                 [SubsrcTypeID],
                 [AccountNumber],
                 [OnBehalfOf],
                 [MemberGroup],
                 [Verify]
        FROM     [dbo].[Subscription]
        ORDER BY [SubscriberID] ASC, [PublicationID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Subscription] OFF;
    END

DROP TABLE [dbo].[Subscription];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Subscription]', N'Subscription';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Subscription]', N'PK_Subscription', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[Subscription].[IDX_SubscriptionID]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_SubscriptionID]
    ON [dbo].[Subscription]([SubscriptionID] ASC) WITH (FILLFACTOR = 80);


GO
PRINT N'Starting rebuilding table [dbo].[SubscriptionPaid]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_SubscriptionPaid] (
    [SubscriptionPaidID] INT             IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]     INT             NOT NULL,
    [PriceCodeID]        INT             NOT NULL,
    [StartIssueDate]     DATE            NOT NULL,
    [ExpireIssueDate]    DATE            NOT NULL,
    [CPRate]             DECIMAL (10, 2) NULL,
    [Amount]             DECIMAL (10, 2) NULL,
    [AmountPaid]         DECIMAL (10, 2) NULL,
    [BalanceDue]         DECIMAL (10, 2) NULL,
    [PaidDate]           DATETIME        NULL,
    [TotalIssues]        INT             NOT NULL,
    [CheckNumber]        CHAR (20)       NULL,
    [CCNumber]           CHAR (16)       NULL,
    [CCExpirationMonth]  CHAR (2)        NULL,
    [CCExpirationYear]   CHAR (4)        NULL,
    [CCHolderName]       VARCHAR (100)   NULL,
    [CreditCardTypeID]   INT             NULL,
    [PaymentTypeID]      INT             NOT NULL,
    [DeliverID]          INT             NULL,
    [GraceIssues]        INT             NULL,
    [WriteOffAmount]     DECIMAL (10, 2) NULL,
    [OtherType]          VARCHAR (256)   NULL,
    [DateCreated]        DATETIME        NOT NULL,
    [DateUpdated]        DATETIME        NULL,
    [CreatedByUserID]    INT             NOT NULL,
    [UpdatedByUserID]    INT             NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_SubscriptionPaid] PRIMARY KEY CLUSTERED ([SubscriptionPaidID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SubscriptionPaid])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SubscriptionPaid] ON;
        INSERT INTO [dbo].[tmp_ms_xx_SubscriptionPaid] ([SubscriptionPaidID], [SubscriptionID], [PriceCodeID], [StartIssueDate], [ExpireIssueDate], [CPRate], [Amount], [AmountPaid], [BalanceDue], [PaidDate], [TotalIssues], [CheckNumber], [CCNumber], [CCExpirationMonth], [CCExpirationYear], [CCHolderName], [CreditCardTypeID], [PaymentTypeID], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID], [DeliverID], [GraceIssues])
        SELECT   [SubscriptionPaidID],
                 [SubscriptionID],
                 [PriceCodeID],
                 [StartIssueDate],
                 [ExpireIssueDate],
                 [CPRate],
                 [Amount],
                 [AmountPaid],
                 [BalanceDue],
                 [PaidDate],
                 [TotalIssues],
                 [CheckNumber],
                 [CCNumber],
                 [CCExpirationMonth],
                 [CCExpirationYear],
                 [CCHolderName],
                 [CreditCardTypeID],
                 [PaymentTypeID],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID],
                 [DeliverID],
                 [GraceIssues]
        FROM     [dbo].[SubscriptionPaid]
        ORDER BY [SubscriptionPaidID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SubscriptionPaid] OFF;
    END

DROP TABLE [dbo].[SubscriptionPaid];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SubscriptionPaid]', N'SubscriptionPaid';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_SubscriptionPaid]', N'PK_SubscriptionPaid', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[AcsFileDetail]...';


GO
CREATE TABLE [dbo].[AcsFileDetail] (
    [AcsFileDetailId]               INT            IDENTITY (1, 1) NOT NULL,
    [ClientId]                      INT            NULL,
    [RecordType]                    VARCHAR (1)    NULL,
    [FileVersion]                   VARCHAR (2)    NULL,
    [SequenceNumber]                INT            NULL,
    [AcsMailerId]                   VARCHAR (9)    NULL,
    [KeylineSequenceSerialNumber]   VARCHAR (16)   NULL,
    [MoveEffectiveDate]             DATE           NULL,
    [MoveType]                      VARCHAR (1)    NULL,
    [DeliverabilityCode]            VARCHAR (1)    NULL,
    [UspsSiteID]                    INT            NULL,
    [LastName]                      VARCHAR (20)   NULL,
    [FirstName]                     VARCHAR (15)   NULL,
    [Prefix]                        VARCHAR (6)    NULL,
    [Suffix]                        VARCHAR (6)    NULL,
    [OldAddressType]                VARCHAR (1)    NULL,
    [OldUrb]                        VARCHAR (28)   NULL,
    [OldPrimaryNumber]              VARCHAR (10)   NULL,
    [OldPreDirectional]             VARCHAR (2)    NULL,
    [OldStreetName]                 VARCHAR (28)   NULL,
    [OldSuffix]                     VARCHAR (4)    NULL,
    [OldPostDirectional]            VARCHAR (2)    NULL,
    [OldUnitDesignator]             VARCHAR (4)    NULL,
    [OldSecondaryNumber]            VARCHAR (10)   NULL,
    [OldCity]                       VARCHAR (28)   NULL,
    [OldStateAbbreviation]          VARCHAR (2)    NULL,
    [OldZipCode]                    VARCHAR (5)    NULL,
    [NewAddressType]                VARCHAR (1)    NULL,
    [NewPmb]                        VARCHAR (8)    NULL,
    [NewUrb]                        VARCHAR (28)   NULL,
    [NewPrimaryNumber]              VARCHAR (10)   NULL,
    [NewPreDirectional]             VARCHAR (2)    NULL,
    [NewStreetName]                 VARCHAR (28)   NULL,
    [NewSuffix]                     VARCHAR (4)    NULL,
    [NewPostDirectional]            VARCHAR (2)    NULL,
    [NewUnitDesignator]             VARCHAR (4)    NULL,
    [NewSecondaryNumber]            VARCHAR (10)   NULL,
    [NewCity]                       VARCHAR (28)   NULL,
    [NewStateAbbreviation]          VARCHAR (2)    NULL,
    [NewZipCode]                    VARCHAR (5)    NULL,
    [Hyphen]                        VARCHAR (1)    NULL,
    [NewPlus4Code]                  VARCHAR (4)    NULL,
    [NewDeliveryPoint]              VARCHAR (2)    NULL,
    [NewAbbreviatedCityName]        VARCHAR (13)   NULL,
    [NewAddressLabel]               VARCHAR (66)   NULL,
    [FeeNotification]               VARCHAR (1)    NULL,
    [NotificationType]              VARCHAR (1)    NULL,
    [IntelligentMailBarcode]        VARCHAR (31)   NULL,
    [IntelligentMailPackageBarcode] VARCHAR (35)   NULL,
    [IdTag]                         VARCHAR (16)   NULL,
    [HardcopyToElectronicFlag]      VARCHAR (1)    NULL,
    [TypeOfAcs]                     VARCHAR (1)    NULL,
    [FulfillmentDate]               DATE           NULL,
    [ProcessingType]                VARCHAR (1)    NULL,
    [CaptureType]                   VARCHAR (1)    NULL,
    [MadeAvailableDate]             DATE           NULL,
    [ShapeOfMail]                   VARCHAR (1)    NULL,
    [MailActionCode]                VARCHAR (1)    NULL,
    [NixieFlag]                     VARCHAR (1)    NULL,
    [ProductCode1]                  INT            NULL,
    [ProductCodeFee1]               DECIMAL (4, 2) NULL,
    [ProductCode2]                  INT            NULL,
    [ProductCodeFee2]               DECIMAL (4, 2) NULL,
    [ProductCode3]                  INT            NULL,
    [ProductCodeFee3]               DECIMAL (4, 2) NULL,
    [ProductCode4]                  INT            NULL,
    [ProductCodeFee4]               DECIMAL (4, 2) NULL,
    [ProductCode5]                  INT            NULL,
    [ProductCodeFee5]               DECIMAL (4, 2) NULL,
    [ProductCode6]                  INT            NULL,
    [ProductCodeFee6]               DECIMAL (4, 2) NULL,
    [Filler]                        VARCHAR (405)  NULL,
    [EndMarker]                     VARCHAR (1)    NULL,
    [ProductCode]                   VARCHAR (50)   NULL,
    [OldAddress1]                   VARCHAR (100)  NULL,
    [OldAddress2]                   VARCHAR (100)  NULL,
    [OldAddress3]                   VARCHAR (100)  NULL,
    [NewAddress1]                   VARCHAR (100)  NULL,
    [NewAddress2]                   VARCHAR (100)  NULL,
    [NewAddress3]                   VARCHAR (100)  NULL,
    [SequenceID]                    INT            NULL,
    [TransactionCodeValue]          INT            NULL,
    [CategoryCodeValue]             INT            NULL,
    [IsIgnored]                     BIT            NULL,
    [AcsActionId]                   INT            NOT NULL,
    [CreatedDate]                   DATE           NULL,
    [CreatedTime]                   TIME (7)       NULL,
    [ProcessCode]                   VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([AcsFileDetailId] ASC)
);


GO
PRINT N'Creating [dbo].[AcsFileHeader]...';


GO
CREATE TABLE [dbo].[AcsFileHeader] (
    [AcsFileHeaderId]       INT            IDENTITY (1, 1) NOT NULL,
    [ClientId]              INT            NULL,
    [RecordType]            VARCHAR (1)    NULL,
    [FileVersion]           VARCHAR (2)    NULL,
    [CustomerID]            INT            NULL,
    [CreateDate]            DATE           NULL,
    [ShipmentNumber]        BIGINT         NULL,
    [TotalAcsRecordCount]   INT            NULL,
    [TotalCoaCount]         INT            NULL,
    [TotalNixieCount]       INT            NULL,
    [TrdRecordCount]        INT            NULL,
    [TrdAcsFeeAmount]       DECIMAL (9, 2) NULL,
    [TrdCoaCount]           INT            NULL,
    [TrdCoaAcsFeeAmount]    DECIMAL (9, 2) NULL,
    [TrdNixieCount]         INT            NULL,
    [TrdNixieAcsFeeAmount]  DECIMAL (9, 2) NULL,
    [OcdRecordCount]        INT            NULL,
    [OcdAcsFeeAmount]       DECIMAL (9, 2) NULL,
    [OcdCoaCount]           INT            NULL,
    [OcdCoaAcsFeeAmount]    DECIMAL (9, 2) NULL,
    [OcdNixieCount]         INT            NULL,
    [OcdNixieAcsFeeAmount]  DECIMAL (9, 2) NULL,
    [FsRecordCount]         INT            NULL,
    [FsAcsFeeAmount]        DECIMAL (9, 2) NULL,
    [FsCoaCount]            INT            NULL,
    [FsCoaAcsFeeAmount]     DECIMAL (9, 2) NULL,
    [FsNixieCount]          INT            NULL,
    [FsNixieAcsFeeAmount]   DECIMAL (9, 2) NULL,
    [ImpbRecordCount]       INT            NULL,
    [ImpbAcsFeeAmount]      DECIMAL (9, 2) NULL,
    [ImpbCoaCount]          INT            NULL,
    [ImpbCoaAcsFeeAmount]   DECIMAL (9, 2) NULL,
    [ImpbNixieCount]        INT            NULL,
    [ImpbNixieAcsFeeAmount] DECIMAL (9, 2) NULL,
    [Filler]                VARCHAR (405)  NULL,
    [EndMarker]             VARCHAR (1)    NULL,
    [ProcessCode]           VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([AcsFileHeaderId] ASC)
);


GO
PRINT N'Creating [dbo].[AcsMailerInfo]...';


GO
CREATE TABLE [dbo].[AcsMailerInfo] (
    [AcsMailerInfoId] INT         IDENTITY (1, 1) NOT NULL,
    [AcsMailerId]     VARCHAR (9) NOT NULL,
    [ImbSeqCounter]   INT         NULL,
    [DateCreated]     DATETIME    NULL,
    [DateUpdated]     DATETIME    NULL,
    [CreatedByUserID] INT         NULL,
    [UpdatedByUserID] INT         NULL,
    PRIMARY KEY CLUSTERED ([AcsMailerInfoId] ASC)
);


GO
PRINT N'Creating [dbo].[AcsShippingDetail]...';


GO
CREATE TABLE [dbo].[AcsShippingDetail] (
    [AcsShippingDetailId] INT             IDENTITY (1, 1) NOT NULL,
    [ClientId]            INT             NOT NULL,
    [CustomerNumber]      INT             NOT NULL,
    [AcsDate]             DATE            NOT NULL,
    [ShipmentNumber]      BIGINT          NOT NULL,
    [AcsTypeId]           INT             NOT NULL,
    [AcsId]               INT             NOT NULL,
    [AcsName]             VARCHAR (250)   NOT NULL,
    [ProductCode]         VARCHAR (100)   NOT NULL,
    [Description]         VARCHAR (250)   NOT NULL,
    [Quantity]            INT             NOT NULL,
    [UnitCost]            DECIMAL (8, 2)  NOT NULL,
    [TotalCost]           DECIMAL (12, 2) NOT NULL,
    [DateCreated]         DATETIME        NOT NULL,
    [IsBilled]            BIT             NOT NULL,
    [BilledDate]          DATETIME        NULL,
    [BilledByUserID]      INT             NULL,
    [ProcessCode]         VARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([AcsShippingDetailId] ASC)
);


GO
PRINT N'Creating [dbo].[ClientProductMap]...';


GO
CREATE TABLE [dbo].[ClientProductMap] (
    [ProductID]   INT          NOT NULL,
    [ClientID]    INT          NOT NULL,
    [ProductCode] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC, [ClientID] ASC)
);


GO
PRINT N'Creating [dbo].[Issue]...';


GO
CREATE TABLE [dbo].[Issue] (
    [IssueId]          INT           IDENTITY (1, 1) NOT NULL,
    [PublicationId]    INT           NOT NULL,
    [IssueName]        VARCHAR (100) NOT NULL,
    [IssueCode]        VARCHAR (50)  NOT NULL,
    [DateOpened]       DATETIME      NOT NULL,
    [OpenedByUserID]   INT           NOT NULL,
    [IsClosed]         BIT           NOT NULL,
    [DateClosed]       DATETIME      NULL,
    [ClosedByUserID]   INT           NULL,
    [IsComplete]       BIT           NOT NULL,
    [DateComplete]     DATETIME      NULL,
    [CompleteByUserID] INT           NULL,
    [DateCreated]      DATETIME      NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT           NULL,
    CONSTRAINT [PK_Issue] PRIMARY KEY CLUSTERED ([IssueId] ASC)
);


GO
PRINT N'Creating [dbo].[IssueArchiveSubscriber]...';


GO
CREATE TABLE [dbo].[IssueArchiveSubscriber] (
    [IssueArchiveSubscriberId] INT              IDENTITY (1, 1) NOT NULL,
    [IssueId]                  INT              NOT NULL,
    [IsComp]                   BIT              NOT NULL,
    [CompId]                   INT              NULL,
    [SubscriberID]             INT              NULL,
    [ExternalKeyID]            INT              NULL,
    [FirstName]                VARCHAR (50)     NULL,
    [LastName]                 VARCHAR (50)     NULL,
    [Company]                  VARCHAR (100)    NULL,
    [Title]                    VARCHAR (255)    NULL,
    [Occupation]               VARCHAR (50)     NULL,
    [AddressTypeID]            INT              NULL,
    [Address1]                 VARCHAR (100)    NULL,
    [Address2]                 VARCHAR (100)    NULL,
    [Address3]                 VARCHAR (100)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [RegionCode]               VARCHAR (50)     NULL,
    [RegionID]                 INT              NULL,
    [ZipCode]                  VARCHAR (50)     NULL,
    [Plus4]                    VARCHAR (10)     NULL,
    [CarrierRoute]             VARCHAR (10)     NULL,
    [County]                   VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [CountryID]                INT              NULL,
    [Latitude]                 DECIMAL (18, 15) NULL,
    [Longitude]                DECIMAL (18, 15) NULL,
    [IsAddressValidated]       BIT              NOT NULL,
    [AddressValidationDate]    DATETIME         NULL,
    [AddressValidationSource]  VARCHAR (50)     NULL,
    [AddressValidationMessage] VARCHAR (MAX)    NULL,
    [Email]                    VARCHAR (255)    NULL,
    [Phone]                    VARCHAR (25)     NULL,
    [Fax]                      VARCHAR (25)     NULL,
    [Mobile]                   VARCHAR (25)     NULL,
    [Website]                  VARCHAR (255)    NULL,
    [Birthdate]                DATE             NULL,
    [Age]                      INT              NULL,
    [Income]                   VARCHAR (50)     NULL,
    [Gender]                   VARCHAR (50)     NULL,
    [IsLocked]                 BIT              NOT NULL,
    [PhoneExt]                 VARCHAR (25)     NULL,
    [DateCreated]              DATETIME         NOT NULL,
    [DateUpdated]              DATETIME         NULL,
    [CreatedByUserID]          INT              NOT NULL,
    [UpdatedByUserID]          INT              NULL,
    CONSTRAINT [PK_IssueArchiveSubscriber] PRIMARY KEY CLUSTERED ([IssueArchiveSubscriberId] ASC)
);


GO
PRINT N'Creating [dbo].[IssueArchiveSubscription]...';


GO
CREATE TABLE [dbo].[IssueArchiveSubscription] (
    [IssueArchiveSubscriptionId]   INT           IDENTITY (1, 1) NOT NULL,
    [IssueArchiveSubscriberId]     INT           NOT NULL,
    [IsComp]                       BIT           NOT NULL,
    [CompId]                       INT           NULL,
    [SubscriptionID]               INT           NULL,
    [SequenceID]                   INT           NULL,
    [PublisherID]                  INT           NOT NULL,
    [SubscriberID]                 INT           NOT NULL,
    [PublicationID]                INT           NOT NULL,
    [ActionID_Current]             INT           NOT NULL,
    [ActionID_Previous]            INT           NULL,
    [SubscriptionStatusID]         INT           NULL,
    [IsPaid]                       BIT           NOT NULL,
    [QSourceID]                    INT           NULL,
    [QSourceDate]                  DATE          NULL,
    [DeliverabilityID]             INT           NULL,
    [IsSubscribed]                 BIT           NOT NULL,
    [SubscriberSourceCode]         VARCHAR (256) NULL,
    [Copies]                       INT           NOT NULL,
    [OriginalSubscriberSourceCode] VARCHAR (256) NULL,
    [Par3cID]                      INT           NULL,
    [SubsrcTypeID]                 INT           NULL,
    [AccountNumber]                VARCHAR (50)  NULL,
    [GraceIssues]                  INT           NULL,
    [OnBehalfOf]                   VARCHAR (256) NULL,
    [MemberGroup]                  VARCHAR (256) NULL,
    [Verify]                       VARCHAR (256) NULL,
    [IsNewSubscription]            BIT           NULL,
    [DateCreated]                  DATETIME      NOT NULL,
    [DateUpdated]                  DATETIME      NULL,
    [CreatedByUserID]              INT           NOT NULL,
    [UpdatedByUserID]              INT           NULL,
    CONSTRAINT [PK_IssueArchiveSubscription] PRIMARY KEY CLUSTERED ([IssueArchiveSubscriptionId] ASC)
);


GO
PRINT N'Creating [dbo].[IssueArchiveSubscriptonResponseMap]...';


GO
CREATE TABLE [dbo].[IssueArchiveSubscriptonResponseMap] (
    [IssueArchiveSubscriptionId] INT           IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]             INT           NOT NULL,
    [ResponseID]                 INT           NOT NULL,
    [IsActive]                   BIT           NOT NULL,
    [ResponseOther]              VARCHAR (300) NULL,
    [DateCreated]                DATETIME      NOT NULL,
    [DateUpdated]                DATETIME      NULL,
    [CreatedByUserID]            INT           NOT NULL,
    [UpdatedByUserID]            INT           NULL,
    CONSTRAINT [PK_IssueArchiveSubscriptonResponseMap] PRIMARY KEY CLUSTERED ([IssueArchiveSubscriptionId] ASC)
);


GO
PRINT N'Creating [dbo].[IssueComp]...';


GO
CREATE TABLE [dbo].[IssueComp] (
    [IssueCompId]     INT      IDENTITY (1, 1) NOT NULL,
    [IssueId]         INT      NOT NULL,
    [ImportedDate]    DATETIME NOT NULL,
    [IssueCompCount]  INT      NOT NULL,
    [DateCreated]     DATETIME NOT NULL,
    [DateUpdated]     DATETIME NULL,
    [CreatedByUserID] INT      NOT NULL,
    [UpdatedByUserID] INT      NULL,
    CONSTRAINT [PK_IssueComp] PRIMARY KEY CLUSTERED ([IssueCompId] ASC)
);


GO
PRINT N'Creating [dbo].[IssueCompDetail]...';


GO
CREATE TABLE [dbo].[IssueCompDetail] (
    [IssueCompDetailId]            INT              IDENTITY (1, 1) NOT NULL,
    [IssueCompID]                  INT              NOT NULL,
    [ExternalKeyID]                INT              NULL,
    [FirstName]                    VARCHAR (50)     NULL,
    [LastName]                     VARCHAR (50)     NULL,
    [Company]                      VARCHAR (100)    NULL,
    [Title]                        VARCHAR (255)    NULL,
    [Occupation]                   VARCHAR (50)     NULL,
    [AddressTypeID]                INT              NULL,
    [Address1]                     VARCHAR (100)    NULL,
    [Address2]                     VARCHAR (100)    NULL,
    [Address3]                     VARCHAR (100)    NULL,
    [City]                         VARCHAR (50)     NULL,
    [RegionCode]                   VARCHAR (50)     NULL,
    [RegionID]                     INT              NULL,
    [ZipCode]                      VARCHAR (50)     NULL,
    [Plus4]                        VARCHAR (10)     NULL,
    [CarrierRoute]                 VARCHAR (10)     NULL,
    [County]                       VARCHAR (50)     NULL,
    [Country]                      VARCHAR (50)     NULL,
    [CountryID]                    INT              NULL,
    [Latitude]                     DECIMAL (18, 15) NULL,
    [Longitude]                    DECIMAL (18, 15) NULL,
    [IsAddressValidated]           BIT              NOT NULL,
    [AddressValidationDate]        DATETIME         NULL,
    [AddressValidationSource]      VARCHAR (50)     NULL,
    [AddressValidationMessage]     VARCHAR (MAX)    NULL,
    [Email]                        VARCHAR (255)    NULL,
    [Phone]                        VARCHAR (25)     NULL,
    [Fax]                          VARCHAR (25)     NULL,
    [Mobile]                       VARCHAR (25)     NULL,
    [Website]                      VARCHAR (255)    NULL,
    [Birthdate]                    DATE             NULL,
    [Age]                          INT              NULL,
    [Income]                       VARCHAR (50)     NULL,
    [Gender]                       VARCHAR (50)     NULL,
    [IsLocked]                     BIT              NOT NULL,
    [PhoneExt]                     VARCHAR (25)     NULL,
    [SequenceID]                   INT              NULL,
    [PublisherID]                  INT              NOT NULL,
    [SubscriberID]                 INT              NOT NULL,
    [PublicationID]                INT              NOT NULL,
    [ActionID_Current]             INT              NOT NULL,
    [ActionID_Previous]            INT              NULL,
    [SubscriptionStatusID]         INT              NULL,
    [IsPaid]                       BIT              NOT NULL,
    [QSourceID]                    INT              NULL,
    [QSourceDate]                  DATE             NULL,
    [DeliverabilityID]             INT              NULL,
    [IsSubscribed]                 BIT              NOT NULL,
    [SubscriberSourceCode]         VARCHAR (256)    NULL,
    [Copies]                       INT              NOT NULL,
    [OriginalSubscriberSourceCode] VARCHAR (256)    NULL,
    [Par3cID]                      INT              NULL,
    [SubsrcTypeID]                 INT              NULL,
    [AccountNumber]                VARCHAR (50)     NULL,
    [OnBehalfOf]                   VARCHAR (256)    NULL,
    [MemberGroup]                  VARCHAR (256)    NULL,
    [Verify]                       VARCHAR (256)    NULL,
    [DateCreated]                  DATETIME         NOT NULL,
    [DateUpdated]                  DATETIME         NULL,
    [CreatedByUserID]              INT              NOT NULL,
    [UpdatedByUserID]              INT              NULL,
    CONSTRAINT [PK_IssueCompDetail] PRIMARY KEY CLUSTERED ([IssueCompDetailId] ASC)
);


GO
PRINT N'Creating [dbo].[IssueCompError]...';


GO
CREATE TABLE [dbo].[IssueCompError] (
    [IssueCompErrorID]   INT              IDENTITY (1, 1) NOT NULL,
    [CompName]           VARCHAR (200)    NOT NULL,
    [SFRecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [ProcessCode]        VARCHAR (50)     NOT NULL,
    [DateCreated]        DATETIME         NOT NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NOT NULL,
    [UpdatedByUserID]    INT              NULL,
    PRIMARY KEY CLUSTERED ([IssueCompErrorID] ASC)
);


GO
PRINT N'Creating [dbo].[IssueSplit]...';


GO
CREATE TABLE [dbo].[IssueSplit] (
    [IssueSplitId]    INT           IDENTITY (1, 1) NOT NULL,
    [IssueId]         INT           NOT NULL,
    [IssueSplitCode]  VARCHAR (250) NOT NULL,
    [IssueSplitName]  VARCHAR (250) NOT NULL,
    [IssueSplitCount] INT           NOT NULL,
    [FilterId]        INT           NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    [IsActive]        BIT           NOT NULL,
    [KeyCode]         VARCHAR (250) NULL,
    CONSTRAINT [PK_IssueSplit] PRIMARY KEY CLUSTERED ([IssueSplitId] ASC)
);


GO
PRINT N'Creating [dbo].[WaveMailing]...';


GO
CREATE TABLE [dbo].[WaveMailing] (
    [WaveMailingID]              INT           IDENTITY (1, 1) NOT NULL,
    [IssueID]                    INT           NOT NULL,
    [WaveMailingName]            VARCHAR (100) NOT NULL,
    [PublicationID]              INT           NOT NULL,
    [WaveNumber]                 INT           NOT NULL,
    [DateSubmittedToPrinter]     DATETIME      NULL,
    [DateCreated]                DATETIME      NOT NULL,
    [DateUpdated]                DATETIME      NULL,
    [SubmittedToPrinterByUserID] INT           NULL,
    [CreatedByUserID]            INT           NOT NULL,
    [UpdatedByUserID]            INT           NULL
);


GO
PRINT N'Creating [dbo].[WaveMailingDetail]...';


GO
CREATE TABLE [dbo].[WaveMailingDetail] (
    [WaveMailingDetailID] INT           IDENTITY (1, 1) NOT NULL,
    [WaveMailingID]       INT           NOT NULL,
    [SubscriberID]        INT           NOT NULL,
    [SubscriptionID]      INT           NOT NULL,
    [DeliverabilityID]    INT           NULL,
    [ActionID_Current]    INT           NULL,
    [ActionID_Previous]   INT           NULL,
    [Copies]              INT           NULL,
    [FirstName]           VARCHAR (50)  NULL,
    [LastName]            VARCHAR (50)  NULL,
    [Title]               VARCHAR (255) NULL,
    [Company]             VARCHAR (100) NULL,
    [AddressTypeID]       INT           NULL,
    [Address1]            VARCHAR (100) NULL,
    [Address2]            VARCHAR (100) NULL,
    [Address3]            VARCHAR (100) NULL,
    [City]                VARCHAR (50)  NULL,
    [RegionCode]          VARCHAR (50)  NULL,
    [RegionID]            INT           NULL,
    [ZipCode]             VARCHAR (50)  NULL,
    [Plus4]               VARCHAR (10)  NULL,
    [County]              VARCHAR (50)  NULL,
    [Country]             VARCHAR (50)  NULL,
    [CountryID]           INT           NULL,
    [Email]               VARCHAR (255) NULL,
    [Phone]               VARCHAR (25)  NULL,
    [Fax]                 VARCHAR (25)  NULL,
    [Mobile]              VARCHAR (25)  NULL,
    [DateCreated]         DATETIME      NOT NULL,
    [DateUpdated]         DATETIME      NULL,
    [CreatedByUserID]     INT           NOT NULL,
    [UpdatedByUserID]     INT           NULL
);


GO
PRINT N'Creating Default Constraint on [dbo].[AcsFileDetail]....';


GO
ALTER TABLE [dbo].[AcsFileDetail]
    ADD DEFAULT ('false') FOR [IsIgnored];


GO
PRINT N'Creating Default Constraint on [dbo].[AcsMailerInfo]....';


GO
ALTER TABLE [dbo].[AcsMailerInfo]
    ADD DEFAULT (1) FOR [ImbSeqCounter];


GO
PRINT N'Creating Default Constraint on [dbo].[AcsShippingDetail]....';


GO
ALTER TABLE [dbo].[AcsShippingDetail]
    ADD DEFAULT ('false') FOR [IsBilled];


GO
PRINT N'Creating Default Constraint on [dbo].[IssueSplit]....';


GO
ALTER TABLE [dbo].[IssueSplit]
    ADD DEFAULT 1 FOR [IsActive];


GO
PRINT N'Creating Full-text Index on [dbo].[Subscription]...';


GO
CREATE FULLTEXT INDEX ON [dbo].[Subscription]
    KEY INDEX [IDX_SubscriptionID]
    ON [Subscriber Catalog];


GO
ALTER FULLTEXT INDEX ON [dbo].[Subscription] DISABLE;


GO
PRINT N'Creating FK_SubscriberAddKillDetail_SubscriberAddKill...';


GO
ALTER TABLE [dbo].[SubscriberAddKillDetail] WITH NOCHECK
    ADD CONSTRAINT [FK_SubscriberAddKillDetail_SubscriberAddKill] FOREIGN KEY ([AddKillID]) REFERENCES [dbo].[SubscriberAddKill] ([AddKillID]);


GO
PRINT N'Creating FK_SubscriberAddKillDetail_Subscription...';


GO
ALTER TABLE [dbo].[SubscriberAddKillDetail] WITH NOCHECK
    ADD CONSTRAINT [FK_SubscriberAddKillDetail_Subscription] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscription] ([SubscriptionID]);


GO
PRINT N'Altering [dbo].[e_Batch_Create]...';


GO
ALTER PROCEDURE [dbo].[e_Batch_Create]
@UserID int,
@PublicationID int
AS

	declare @BatchNumber int = (select MAX(batchNumber) from Batch where PublicationID = @PublicationID) + 1
	 
	if(@BatchNumber) is null
	begin
		set	@BatchNumber = 1
	end


	INSERT INTO Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,BatchNumber)
	VALUES(@PublicationID,@UserID,0,'true',GetDate(),@BatchNumber);SELECT @@IDENTITY;
GO
PRINT N'Altering [dbo].[e_Batch_Save]...';


GO
ALTER PROCEDURE [dbo].[e_Batch_Save]
@BatchID int,
@PublicationID int,
@UserID int,
@BatchCount int,
@IsActive bit,
@DateCreated datetime,
@DateFinalized datetime,
@BatchNumber int
AS

IF @BatchID > 0
	BEGIN
		IF @DateFinalized IS NULL
			BEGIN
				SET @DateFinalized = GETDATE();
			END
			
		UPDATE Batch
		SET 
			PublicationID = @PublicationID,
			UserID = @UserID,
			BatchCount = @BatchCount,
			IsActive = @IsActive,
			DateFinalized = @DateFinalized,
			BatchNumber = @BatchNumber
		WHERE BatchID = @BatchID;
		
		SELECT @BatchID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END		
		
		declare @bn int = (select MAX(batchNumber) from Batch where PublicationID = @PublicationID) + 1
		 
		if(@bn) is null
		begin
			set	@bn = 1
		end	
			
		INSERT INTO Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,BatchNumber)
		VALUES(@PublicationID,@UserID,@BatchCount,@IsActive,@DateCreated,@bn);SELECT @@IDENTITY;
		
	END
GO
PRINT N'Altering [dbo].[o_BatchHistoryDetail_Select]...';


GO

ALTER PROCEDURE [dbo].[o_BatchHistoryDetail_Select]
AS
	SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   p.PublicationID,p.PublicationName,p.PublicationCode,
		   pub.PublisherID,pub.PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   --ult.UserLogTypeID,ult.UserLogTypeName,
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	--LEFT JOIN UAS..UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID
GO
PRINT N'Altering [dbo].[o_BatchHistoryDetail_Select_BatchID]...';


GO

ALTER PROCEDURE [dbo].[o_BatchHistoryDetail_Select_BatchID]
@BatchID int
AS
	SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   p.PublicationID,p.PublicationName,p.PublicationCode,
		   pub.PublisherID,pub.PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   --ult.UserLogTypeID,ult.UserLogTypeName,
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	--LEFT JOIN UAS..UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID
	WHERE b.BatchID = @BatchID
GO
PRINT N'Altering [dbo].[o_BatchHistoryDetail_Select_SubscriberID]...';


GO


ALTER PROCEDURE [dbo].[o_BatchHistoryDetail_Select_SubscriberID]
@SubscriberID int
AS
	SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   h.PublicationID,'' as PublicationName, '' as PublicationCode,
		   h.PublisherID,'' as PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   c.CodeID,c.DisplayName AS UserLogTypeName,
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	--JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	--JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	LEFT JOIN UAS..Code c on c.CodeId = ul.UserLogTypeID
	LEFT JOIN UAS..CodeType ct on c.CodeTypeId = ct.CodeTypeId
	WHERE s.SubscriberID = @SubscriberID and ul.UserLogID != h.HistorySubscriptionID
GO
PRINT N'Altering [dbo].[o_BatchHistoryDetail_Select_SubscriptionID]...';


GO


ALTER PROCEDURE [dbo].[o_BatchHistoryDetail_Select_SubscriptionID]
@SubscriptionID int
AS
	SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   p.PublicationID,p.PublicationName,p.PublicationCode,
		   pub.PublisherID,pub.PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   --ult.UserLogTypeID,ult.UserLogTypeName,
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	--LEFT JOIN UAS..UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID
	WHERE h.SubscriptionID = @SubscriptionID
GO
PRINT N'Altering [dbo].[o_BatchHistoryDetail_Select_UserID_IsActive]...';


GO

ALTER PROCEDURE [dbo].[o_BatchHistoryDetail_Select_UserID_IsActive]
@UserID int,
@IsActive bit
AS

	--SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',b.DateFinalized as 'BatchDateFinalized',
	--	   p.PublicationID,p.PublicationName,p.PublicationCode,
	--	   pub.PublisherID,pub.PublisherName,
	--	   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,h.SubscriptionHistoryID,h.DateCreated as 'HistoryDateCreated',
	--	   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
	--	   t.TaskID,t.TaskName,
	--	   ult.UserLogTypeID,ult.UserLogTypeName,
	--	   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	--FROM Batch b With(NoLock)
	--JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	--JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	--JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	--JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	--LEFT JOIN HistoryMap hm With(NoLock) ON h.HistoryID = hm.HistoryID
	--LEFT JOIN UserLog ul With(NoLock) ON hm.UserLogID = ul.UserLogID
	--LEFT JOIN UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID
	--LEFT JOIN Task t With(NoLock) ON ul.TaskID = t.TaskID 
	--WHERE b.UserID = @UserID AND b.IsActive = @IsActive

	SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   p.PublicationID,p.PublicationName,p.PublicationCode,
		   pub.PublisherID,pub.PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   sub.SequenceID,
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	JOIN Subscription sub With(NoLock) On s.SubscriberID = sub.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	WHERE b.UserID = @UserID AND b.IsActive = @IsActive and ul.UserLogID != h.HistorySubscriptionID
GO
PRINT N'Altering [dbo].[o_FinalizeBatch_Select]...';


GO


ALTER PROCEDURE [dbo].[o_FinalizeBatch_Select]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,cpm.ProductID,p.PublicationName,Max(h.BatchCountItem) as LastCount, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN ClientProductMap cpm ON cpm.ProductID = b.PublicationID
		JOIN Publication p ON cpm.ProductCode = p.PublicationCode
		JOIN UAS..[Client] pub With(NoLock) ON cpm.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		--LEFT JOIN UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID
		WHERE b.IsActive = 1 and b.UserID = @UserID
	Group By b.BatchID, pub.ClientID,pub.ClientName, cpm.ProductID, p.PublicationName, b.DateFinalized,b.BatchNumber
GO
PRINT N'Altering [dbo].[o_FinalizeBatch_SelectAll]...';


GO

ALTER PROCEDURE [dbo].[o_FinalizeBatch_SelectAll]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,cpm.ProductID,p.PublicationName,Max(h.BatchCountItem) as LastCount, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN ClientProductMap cpm ON cpm.ProductID = b.PublicationID
		JOIN Publication p ON cpm.ProductCode = p.PublicationCode
		JOIN UAS..[Client] pub With(NoLock) ON cpm.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		--LEFT JOIN UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID
		WHERE b.UserID = @UserID
	Group By b.BatchID, pub.ClientID,pub.ClientName, cpm.ProductID, p.PublicationName, b.DateFinalized,b.BatchNumber
GO
PRINT N'Altering [dbo].[o_FinalizeBatch_SelectAll_NoUser]...';


GO
ALTER PROCEDURE [dbo].[o_FinalizeBatch_SelectAll_NoUser]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,cpm.ProductID,p.PublicationName, p.PublicationCode, Max(h.BatchCountItem) as LastCount, u.FirstName + ' ' + u.LastName as UserName, b.DateCreated, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN ClientProductMap cpm ON cpm.ProductID = b.PublicationID
		JOIN Publication p ON cpm.ProductCode = p.PublicationCode
		JOIN UAS..[Client] pub With(NoLock) ON cpm.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		JOIN UAS..[User] u With(NoLock) ON u.UserID = b.UserID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		--LEFT JOIN UAS..CodeType ct ON ct.CodeTypeName = 'User Log'
		--LEFT JOIN UAS..Code c ON c.CodeTypeId = ct.CodeTypeDescription 
	Group By b.BatchID, pub.ClientID, pub.ClientName, cpm.ProductID, p.PublicationName, p.PublicationCode, u.FirstName, u.LastName, b.DateCreated, b.DateFinalized,b.BatchNumber
GO
PRINT N'Altering [dbo].[e_HistoryPaidBillTo_Save]...';


GO
ALTER PROCEDURE [dbo].[e_HistoryPaidBillTo_Save]
@PaidBillToID int,
@SubscriptionPaidID int,
@SubscriptionID int,
@FirstName nvarchar(50),
@LastName nvarchar(50),
@Company nvarchar(100),
@Title nvarchar(50),
@Occupation nvarchar(50)='',
@AddressTypeID int,
@Address1 nvarchar(100),
@Address2 nvarchar(100),
@Address3 nvarchar(100),
@City nvarchar(50),
@RegionCode nvarchar(50),
@RegionID int,
@ZipCode nchar(10),
@Plus4 nchar(10),
@CarrierRoute varchar(10),
@County nvarchar(50),
@Country nvarchar(50),
@CountryID int,
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource nvarchar(50),
@AddressValidationMessage nvarchar(max),
@Email nvarchar(100),
@Phone nchar(10),
@Fax nchar(10),
@Mobile nchar(10),
@Website nvarchar(100),
@Birthdate date='',
@Age int='',
@Income nvarchar(50)='',
@Gender nvarchar(50)='',
@DateCreated datetime,
@CreatedByUserID int
AS
IF @DateCreated IS NULL
	BEGIN
		SET @DateCreated = GETDATE();
	END
	
INSERT INTO HistoryPaidBillTo (PaidBillToID,SubscriptionPaidID,SubscriptionID,FirstName,LastName,Company,Title,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
							   CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
							   Email,Phone,Fax,Mobile,Website,DateCreated,CreatedByUserID)
		VALUES(@PaidBillToID,@SubscriptionPaidID,@SubscriptionID,@FirstName,@LastName,@Company,@Title,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,
				@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Email,@Phone,@Fax,@Mobile,
				@Website,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
GO
PRINT N'Altering [dbo].[e_HistorySubscription_Save]...';


GO
ALTER PROCEDURE [dbo].[e_HistorySubscription_Save]
@HistorySubscriptionID int,
@SubscriptionID int,
@PublisherID int,
@SubscriberID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate datetime,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256)='',
@Copies int,
@OriginalSubscriberSourceCode varchar(256)='',
@SubscriptionDateCreated datetime,
@SubscriptionDateUpdated datetime,
@SubscriptionCreatedByUserID int,
@SubscriptionUpdatedByUserID int,
@AccountNumber int = '',
@GraceIssues int = '',
@IsNewSubscription int = '',
@MemberGroup varchar(256) = '',
@OnBehalfOf varchar(256) = '',
@Par3cID int = '',
@SequenceID int = '',
@SubsrcTypeID int = '',
@Verify varchar(256),
@IsActive bit = 'true',
@ExternalKeyID int,
@FirstName nvarchar(50),
@LastName nvarchar(50),
@Company nvarchar(100),
@Title nvarchar(50),
@Occupation nvarchar(50),
@AddressTypeID int,
@Address1 nvarchar(100),
@Address2 nvarchar(100),
@Address3 nvarchar(100),
@City nvarchar(50),
@RegionCode nvarchar(50),
@RegionID int,
@ZipCode nvarchar(50),
@Plus4 nchar(10),
@CarrierRoute varchar(10),
@County nvarchar(50),
@Country nvarchar(50),
@CountryID int,
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource nvarchar(50),
@AddressValidationMessage nvarchar(max),
@Email nvarchar(100),
@Phone nchar(10),
@Fax nchar(10),
@Mobile nchar(10),
@Website nvarchar(100),
@Birthdate date,
@Age int,
@Income nvarchar(50),
@Gender nvarchar(50),
@SubscriberDateCreated datetime,
@SubscriberDateUpdated datetime,
@SubscriberCreatedByUserID int,
@SubscriberUpdatedByUserID int,
@DateCreated datetime,
@CreatedByUserID int,
@IsLocked bit,
@LockDate DATETIME = '',
@LockDateRelease datetime = '',
@LockedByUserID	int = '',
@PhoneExt varchar(25)='',
@IsUadUpdated bit,
@UadUpdatedDate datetime =''
AS

IF @HistorySubscriptionID > 0
	BEGIN		
		UPDATE HistorySubscription
		SET 
			SubscriptionID = @SubscriptionID,
			PublisherID = @PublisherID,
			SubscriberID = @SubscriberID,
			PublicationID = @PublicationID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			SubscriptionStatusID = @SubscriptionStatusID,
			IsPaid = @IsPaid,
			QSourceID = @QSourceID,
			QSourceDate = @QSourceDate,
			DeliverabilityID = @DeliverabilityID,
			IsSubscribed = @IsSubscribed,
			SubscriberSourceCode = @SubscriberSourceCode,
			Copies = @Copies,
			OriginalSubscriberSourceCode = @OriginalSubscriberSourceCode,
			SubscriptionDateCreated = @SubscriptionDateCreated,
			SubscriptionDateUpdated = @SubscriptionDateUpdated,
			SubscriptionCreatedByUserID = @SubscriptionCreatedByUserID,
			SubscriptionUpdatedByUserID = @SubscriptionUpdatedByUserID,
			AccountNumber =	AccountNumber,
			GraceIssues = GraceIssues,
			IsNewSubscription = IsNewSubscription, 
			MemberGroup = MemberGroup,
			OnBehalfOf = OnBehalfOf,
			Par3cID = @Par3cID,
			SequenceID = SequenceID,
			SubsrcTypeID = SubsrcTypeID,
			Verify = Verify,
			IsActive = @IsActive,
			ExternalKeyID = @ExternalKeyID,
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
			Occupation = @Occupation,
			AddressTypeID = @AddressTypeID,
			Address1 = @Address1,
			Address2 = @Address2,
			Address3 = @Address3,
			City = @City,
			RegionCode = @RegionCode,
			RegionID = @RegionID,
			ZipCode = @ZipCode,
			Plus4 = @Plus4,
			CarrierRoute = @CarrierRoute,
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Latitude = @Latitude,
			Longitude = @Longitude,
			IsAddressValidated = @IsAddressValidated,
			AddressValidationDate = @AddressValidationDate,
			AddressValidationSource = @AddressValidationSource,
			AddressValidationMessage = @AddressValidationMessage,
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			Birthdate = @Birthdate, 
			Age = @Age,
			Income = @Income,
			Gender = @Gender,
			SubscriberDateCreated = @SubscriberDateCreated,
			SubscriberDateUpdated = @SubscriberDateUpdated,
			SubscriberCreatedByUserID = @SubscriberCreatedByUserID,
			SubscriberUpdatedByUserID = @SubscriberUpdatedByUserID,
			IsLocked = IsLocked,
			LockDate  = @LockDate,
			LockDateRelease = LockDateRelease,
			LockedByUserID= @LockedByUserID,
			PhoneExt = @PhoneExt,
			IsUadUpdated = @IsUadUpdated,
			UadUpdatedDate = @UadUpdatedDate
		WHERE HistorySubscriptionID = @HistorySubscriptionID;
		
		SELECT @HistorySubscriptionID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
										 SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,IsActive,ExternalKeyID,FirstName,LastName,Company,
										 Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
										 AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,SubscriberDateCreated,
										 SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,LockDate,LockDateRelease,LockedByUserID,PhoneExt,IsUadUpdated,UadUpdatedDate)
										 
		VALUES(@SubscriptionID,@PublisherID,@SubscriberID,@PublicationID,@ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,@IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@SubscriptionDateCreated,
			   @SubscriptionDateUpdated,@SubscriptionCreatedByUserID,@SubscriptionUpdatedByUserID,@AccountNumber,@GraceIssues,@IsNewSubscription,@MemberGroup,@OnBehalfOf,@Par3cID,@SequenceID,@SubsrcTypeID,@Verify,@IsActive,@ExternalKeyID,@FirstName,@LastName,@Company,
			   @Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,
			   @AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Email,@Phone,@Fax,@Mobile,@Website,@Birthdate,@Age,@Income,@Gender,@SubscriberDateCreated,
			   @SubscriberDateUpdated,@SubscriberCreatedByUserID,@SubscriberUpdatedByUserID,@DateCreated,@CreatedByUserID,@IsLocked,@LockDate,@LockDateRelease,@LockedByUserID,@PhoneExt,@IsUadUpdated,@UadUpdatedDate);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_PaidBillTo_Save]...';


GO
ALTER PROCEDURE [dbo].[e_PaidBillTo_Save]
@PaidBillToID int,
@SubscriptionPaidID int,
@SubscriptionID int,
@FirstName varchar(50)='',
@LastName varchar(50)='',
@Company varchar(100)='',
@Title varchar(255)='',
@AddressTypeID int='',
@Address1 varchar(100)='',
@Address2 varchar(100)='',
@Address3 varchar(100)='',
@City varchar(50)='',
@RegionCode varchar(50)='',
@RegionID int='',
@ZipCode varchar(50)='',
@Plus4 varchar(10)='',
@CarrierRoute varchar(10)='',
@County varchar(50)='',
@Country varchar(50)='',
@CountryID int='',
@Latitude decimal(18,15)='',
@Longitude decimal(18,15)='',
@IsAddressValidated bit='',
@AddressValidationDate datetime='',
@AddressValidationSource varchar(50)='',
@AddressValidationMessage varchar(max)='',
@Email varchar(255)='',
@Phone varchar(50)='',
@PhoneExt varchar(25)='',
@Fax varchar(50)='',
@Mobile varchar(50)='',
@Website varchar(255)='',
@DateCreated datetime='',
@DateUpdated datetime='',
@CreatedByUserID int='',
@UpdatedByUserID int=''
AS

IF @PaidBillToID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE PaidBillTo
		SET 
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
			AddressTypeID = @AddressTypeID,
			Address1 = @Address1,
			Address2 = @Address2,
			Address3 = @Address3,
			City = @City,
			RegionCode = @RegionCode,
			RegionID = @RegionID,
			ZipCode = @ZipCode,
			Plus4 = @Plus4,
			CarrierRoute = @CarrierRoute,
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Latitude = @Latitude,
			Longitude = @Longitude,
			IsAddressValidated = @IsAddressValidated,
			AddressValidationDate = @AddressValidationDate,
			AddressValidationSource = @AddressValidationSource,
			AddressValidationMessage = @AddressValidationMessage,
			Email = @Email,
			Phone = @Phone,
			PhoneExt = @PhoneExt,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PaidBillToID = @PaidBillToID;
		
		SELECT @PaidBillToID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO PaidBillTo (SubscriptionPaidID,SubscriptionID,FirstName,LastName,Company,Title,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,
								Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,PhoneExt,Fax,Mobile,
								Website,DateCreated,CreatedByUserID)
		VALUES(@SubscriptionPaidID,@SubscriptionID,@FirstName,@LastName,@Company,@Title,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,
				@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Email,@Phone,@PhoneExt,@Fax,@Mobile,
				@Website,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_PriceCode_Save]...';


GO
ALTER PROCEDURE e_PriceCode_Save
@PriceCodeID int,
@PublicationID int,
@PriceCode varchar(50),
@Term int,
@USCopyRate decimal,
@CANCopyRate decimal,
@FORCopyRate decimal,
@USPrice decimal(18,2),
@CANPrice decimal(18,2),
@FORPrice decimal(18,2),
@QFOfferCode varchar(255),
@FoxProPriceCode varchar(255),
@Description varchar(255),
@DeliverabilityID varchar(50),
@TotalIssues int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @PriceCodeID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE PriceCode
		SET 
			PublicationID = @PublicationID,
			PriceCodes = @PriceCode,
			Term = @Term,
			US_CopyRate = @USCopyRate,
			CAN_CopyRate = @CANCopyRate,
			FOR_CopyRate = @FORCopyRate,
			US_Price= @USPrice,
			CAN_Price= @CANPrice,
			FOR_Price= @FORPrice,
			QFOfferCode = @QFOfferCode,
			FoxProPriceCode = @FoxProPriceCode,
			[Description] = @Description,
			DeliverabilityID = @DeliverabilityID,
			TotalIssues = @TotalIssues,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PriceCodeID = @PriceCodeID;
		
		SELECT @PriceCodeID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO PriceCode (PublicationID,PriceCodes,Term,US_CopyRate,CAN_CopyRate,FOR_CopyRate,US_Price,CAN_Price,FOR_Price,QFOfferCode,FoxProPriceCode,[Description],DeliverabilityID,TotalIssues,IsActive,DateCreated,CreatedByUserID)
		VALUES(@PublicationID,@PriceCode,@Term,@USCopyRate,@CANCopyRate,@FORCopyRate,@USPrice,@CANPrice,@FORPrice,@QFOfferCode,@FoxProPriceCode,@Description,@DeliverabilityID,@TotalIssues,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_PriceCode_Select_PriceCode_PublicationID]...';


GO
ALTER PROCEDURE e_PriceCode_Select_PriceCode_PublicationID
@PriceCode varchar(50),
@PublicationID int
AS
	SELECT *
	FROM PriceCode With(NoLock)
	WHERE PriceCodes = @PriceCode AND PublicationID = @PublicationID AND IsActive = 'true'
GO
PRINT N'Altering [dbo].[e_Publication_Save]...';


GO
ALTER PROCEDURE e_Publication_Save
@PublicationID int,
@PublicationName varchar(50),
@PublicationCode varchar(50),
@PublisherID int,
@YearStartDate char(5),
@YearEndDate char(5),
@IssueDate date,
@IsImported bit,
@IsActive bit,
@AllowDataEntry bit,
@FrequencyID int,
@KMImportAllowed bit,
@ClientImportAllowed bit,
@AddRemoveAllowed bit,
@AcsMailerInfoId int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @PublicationID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Publication
		SET 
			PublicationName = @PublicationName,
			YearStartDate= @YearStartDate,
			YearEndDate = @YearEndDate,
			IssueDate = @IssueDate,
			IsImported = @IsImported,
			IsActive = @IsActive,
			AllowDataEntry = @AllowDataEntry,
			FrequencyID = @FrequencyID,
			KMImportAllowed = @KMImportAllowed, 
			ClientImportAllowed = @ClientImportAllowed, 
			AddRemoveAllowed = @AddRemoveAllowed,
			AcsMailerInfoId = @AcsMailerInfoId,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PublicationID = @PublicationID;
		
		SELECT @PublicationID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Publication (PublicationName,PublicationCode,PublisherID,YearStartDate,YearEndDate,IssueDate,IsImported,IsActive,AllowDataEntry,FrequencyID,KMImportAllowed, ClientImportAllowed, AddRemoveAllowed,AcsMailerInfoId,DateCreated,CreatedByUserID)
		VALUES(@PublicationName,@PublicationCode,@PublisherID,@YearStartDate,@YearEndDate,@IssueDate,@IsImported,@IsActive,@AllowDataEntry,@FrequencyID,@KMImportAllowed,@ClientImportAllowed,@AddRemoveAllowed,@AcsMailerInfoId,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[o_SubscriptionSearchResult_Select_SubscriberID]...';


GO
ALTER PROCEDURE [dbo].[o_SubscriptionSearchResult_Select_SubscriberID]
@SubscriberID int
AS
	
	SELECT s.SubscriptionID,s.SequenceID,RTRIM(LTRIM(ISNULL(sr.Address1, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(sr.Address2,''))) + ', ' + RTRIM(LTRIM(ISNULL(sr.City, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(sr.RegionCode,''))) + ', ' + 
		(case when sr.CountryID = 1 and ISNULL(sr.ZipCode,'')!='' AND ISNULL(sr.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + '-' + sr.Plus4 
			  when sr.CountryID = 2 and ISNULL(sr.ZipCode,'')!='' AND ISNULL(sr.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + ' ' + sr.Plus4 
			  else RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + RTRIM(LTRIM(ISNULL(sr.Plus4,''))) end)+ ', ' + 
		RTRIM(LTRIM(ISNULL(sr.Country, ''))) as 'FullAddress',
		   '' as ProductCode,sr.Phone,sr.Email,s.PublicationID as ProductID,s.PublisherID as ClientID,s.SubscriberID,
		   '' as ClientName,sr.Company,s.IsSubscribed,s.AccountNumber,ISNULL(uc.PhonePrefix,'0') AS PhoneCode,
		   s.SubscriptionStatusID
	FROM Subscription s With(NoLock)
	JOIN Subscriber sr With(NoLock) ON sr.SubscriberID = s.SubscriberID
	--JOIN Publication cation With(NoLock) ON s.PublicationID = cation.PublicationID
	--JOIN Publisher p With(NoLock) ON cation.PublisherID = p.PublisherID
	LEFT JOIN UAS..Country uc WITH(Nolock) ON sr.CountryID = uc.CountryID
	WHERE s.SubscriberID = @SubscriberID
GO
PRINT N'Altering [dbo].[e_Response_Save]...';


GO
ALTER PROCEDURE e_Response_Save
@ResponseID int,
@ResponseTypeID int,
@PublicationID int,
@ResponseName varchar(250),
@ResponseCode nchar(10),
@DisplayName varchar(250),
@DisplayOrder int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@IsOther BIT
AS

IF @ResponseID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Response
		SET 
			ResponseTypeID = @ResponseTypeID,
			PublicationID = @PublicationID,
			ResponseName = @ResponseName,
			ResponseCode = @ResponseCode,
			DisplayName = @DisplayName,
			DisplayOrder = @DisplayOrder,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			IsOther = @IsOther
		WHERE ResponseID = @ResponseID;
		
		SELECT @ResponseID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Response (ResponseTypeID,PublicationID,ResponseName,ResponseCode,DisplayName,DisplayOrder,IsActive,DateCreated,CreatedByUserID,IsOther)
		VALUES(@ResponseTypeID,@PublicationID,@ResponseName,@ResponseCode,@DisplayName,@DisplayOrder,@IsActive,@DateCreated,@CreatedByUserID,@IsOther);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_Subscriber_Delete_SubscriberID]...';


GO
ALTER PROCEDURE e_Subscriber_Delete_SubscriberID
	@SubscriberID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM Subscription WHERE SubscriberID = @SubscriberID
	DELETE FROM Subscriber WHERE SubscriberID = @SubscriberID
    
END
GO
PRINT N'Altering [dbo].[e_Subscriber_Save]...';


GO

ALTER PROCEDURE [dbo].[e_Subscriber_Save]
@SubscriberID int,
@ExternalKeyID int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar(50),
@Occupation varchar(50),
@AddressTypeID int,
@Address1 varchar(100),
@Address2 varchar(100),
@Address3 varchar(100),
@City varchar(50),
@RegionCode varchar(50),
@RegionID int,
@ZipCode varchar(10),
@Plus4 varchar(10),
@CarrierRoute varchar(10),
@County varchar(50),
@Country varchar(50),
@CountryID int,
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource varchar(50),
@AddressValidationMessage varchar(max),
@Email varchar(100),
@Phone varchar(50),
@Fax varchar(50),
@Mobile varchar(50),
@Website varchar(100),
@Birthdate date,
@Age int,
@Income varchar(50),
@Gender varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
--@IsLocked bit,
--@LockedByUserID int,
--@LockDate int,
@PhoneExt varchar(25),
@IsInActiveWaveMailing bit,
@AddressTypeCodeId int,
@AddressLastUpdatedDate datetime,
@AddressUpdatedSourceTypeCodeId int,
@WaveMailingID int,
@IGrp_No uniqueidentifier,
@SFRecordIdentifier uniqueidentifier
AS

IF @SubscriberID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Subscriber
		SET 
			ExternalKeyID = @ExternalKeyID,
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
			Occupation = @Occupation,
			AddressTypeID = @AddressTypeID,
			Address1 = @Address1,
			Address2 = @Address2,
			Address3 = @Address3,
			City = @City,
			RegionCode = @RegionCode,
			RegionID = @RegionID,
			ZipCode = @ZipCode,
			Plus4 = @Plus4,
			CarrierRoute = @CarrierRoute,
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Latitude = @Latitude,
			Longitude = @Longitude,
			IsAddressValidated = @IsAddressValidated,
			AddressValidationDate = @AddressValidationDate,
			AddressValidationSource = @AddressValidationSource,
			AddressValidationMessage = @AddressValidationMessage,
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			Birthdate = @Birthdate,
			Age = @Age,
			Income = @Income,
			Gender = @Gender, 
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			--IsLocked = @IsLocked,
			--LockedByUserID = @LockedByUserID,
			--LockDate = @LockDate,
			PhoneExt = @PhoneExt,
			IsInActiveWaveMailing = @IsInActiveWaveMailing,
			AddressTypeCodeId = @AddressTypeCodeId,
			AddressLastUpdatedDate = @AddressLastUpdatedDate,
			AddressUpdatedSourceTypeCodeId = @AddressUpdatedSourceTypeCodeId,
			WaveMailingID = @WaveMailingID,
			IGrp_No = @IGrp_No,
			SFRecordIdentifier = @SFRecordIdentifier
 
		WHERE SubscriberID = @SubscriberID;
		
		SELECT @SubscriberID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Subscriber (ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,
								Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,
								Website,Birthdate,Age,Income,Gender,DateCreated,CreatedByUserID,PhoneExt,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId, 
								IsInActiveWaveMailing, WaveMailingID, IGrp_No, SFRecordIdentifier)
		VALUES(@ExternalKeyID,@FirstName,@LastName,@Company,@Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,
				@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Email,@Phone,@Fax,@Mobile,
				@Website,@Birthdate,@Age,@Income,@Gender,@DateCreated,@CreatedByUserID,@PhoneExt,@AddressTypeCodeId,@AddressLastUpdatedDate,@AddressUpdatedSourceTypeCodeId, 
				@IsInActiveWaveMailing, @WaveMailingID, @IGrp_No, @SFRecordIdentifier);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_Subscriber_Update]...';


GO

ALTER PROCEDURE [dbo].[e_Subscriber_Update]
@SubscriberID int,
@IsLocked bit,
@UserID int
AS

IF (@SubscriberID) > 0
BEGIN

	IF(@IsLocked) = 1
	BEGIN
		UPDATE Subscriber
		SET IsLocked = @IsLocked,
			LockedByUserID = @UserID,
			LockDate = getdate(),
			LockDateRelease = null
		WHERE SubscriberID = @SubscriberID;
	END
	ELSE
	BEGIN
		UPDATE Subscriber
		SET IsLocked = @IsLocked,
			LockDateRelease = getdate()
		WHERE SubscriberID = @SubscriberID;
	END
		SELECT @SubscriberID;

END
ELSE
BEGIN
	-- This part of the proc should only happen if the application crashes.  If there is a crash, all subscribers
	-- that was locked by the user will get unlocked
	IF(@UserID) > 0 AND @IsLocked = 0
	BEGIN

		UPDATE Subscriber
		SET IsLocked = @IsLocked,
			LockDateRelease = getdate()
		WHERE LockedByUserID = @UserID AND IsLocked = 1

	END
END
GO
PRINT N'Altering [dbo].[e_Subscription_Save]...';


GO

ALTER PROCEDURE [dbo].[e_Subscription_Save]
@SubscriptionID int,
@SequenceID int = 0,
@SubscriberID int,
@PublisherID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate datetime,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256)='',
@Copies int,
@OriginalSubscriberSourceCode varchar(256)='',
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@Par3cID int,
@SubsrcTypeID int,
@AccountNumber varchar(50),
@OnBehalfOf varchar(256),
@MemberGroup varchar(256),
@Verify varchar(256),
@AddRemoveID int,
@IMBSeq varchar(10),
@IsActive BIT = 'true'

AS

IF @SubscriptionID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		
		--IF @SequenceID = 0
		--	BEGIN
		--		SET @SequenceID = (Select MAX(SequenceID) + 1 FROM Subscription WHERE PublicationID = @PublicationID)
		--	END
			
		UPDATE Subscription
		SET 
			--SequenceID = @SequenceID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			SubscriptionStatusID = @SubscriptionStatusID,
			IsPaid = @IsPaid,
			QSourceID = @QSourceID,
			QSourceDate = @QSourceDate,
			DeliverabilityID = @DeliverabilityID,
			IsSubscribed = @IsSubscribed,
			SubscriberSourceCode = @SubscriberSourceCode,
			Copies = @Copies,
			OriginalSubscriberSourceCode = @OriginalSubscriberSourceCode,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			Par3cID = @Par3cID,
			SubsrcTypeID = @SubsrcTypeID,
			AccountNumber = @AccountNumber,
			OnBehalfOf = @OnBehalfOf,
			MemberGroup = @MemberGroup,
			Verify = @Verify,
			IMBSeq = @IMBSeq,
			IsActive = @IsActive
		WHERE SubscriptionID = @SubscriptionID;
		
		SELECT @SubscriptionID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		IF @SequenceID = 0
			BEGIN
				SET @SequenceID = (Select MAX(SequenceID) + 1 FROM Subscription WHERE PublicationID = @PublicationID)
				IF(@SequenceID) IS NULL
				BEGIN
					SET @SequenceID = 1
				END
			END	
		--INSERT INTO PublicationSequence (PublicationID,SequenceID, DateCreated, CreatedByUserID)
		--	VALUES(@PublicationID,@SequenceID, @DateCreated, @CreatedByUserID)
			
		INSERT INTO Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,
									IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,DateCreated,CreatedByUserID,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify, AddRemoveID, IMBSeq,IsActive)
		VALUES(@SequenceID,@PublisherID,@SubscriberID,@PublicationID,@ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,
				@IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@DateCreated,@CreatedByUserID,@Par3cID,@SubsrcTypeID,@AccountNumber,@OnBehalfOf,@MemberGroup,@Verify, @AddRemoveID, @IMBSeq,@IsActive);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_SubscriptionPaid_Save]...';


GO

ALTER PROCEDURE [dbo].[e_SubscriptionPaid_Save]
@SubscriptionPaidID int,
@SubscriptionID int,
@PriceCodeID int,
@StartIssueDate date,
@ExpireIssueDate date,
@CPRate decimal(10,2),
@Amount decimal(10,2),
@AmountPaid decimal(10,2),
@BalanceDue decimal(10,2),
@PaidDate datetime,
@TotalIssues int,
@CheckNumber char(20)='',
@CCNumber char(16)='',
@CCExpirationMonth char(2)='',
@CCExpirationYear char(4)='',
@CCHolderName varchar(100)='',
@CreditCardTypeID int='',
@PaymentTypeID int,
@DateCreated datetime='',
@DateUpdated datetime='',
@CreatedByUserID int='',
@UpdatedByUserID int='',
@DeliverID int='',
@GraceIssues int='',
@WriteOffAmount decimal(10,2) = '',
@OtherType varchar(256) = ''
AS

IF @SubscriptionPaidID > 0
	BEGIN
		UPDATE SubscriptionPaid
		SET 
			SubscriptionID = @SubscriptionID,
			PriceCodeID = @PriceCodeID,
			StartIssueDate = @StartIssueDate,
			ExpireIssueDate = @ExpireIssueDate,
			CPRate = @CPRate,
			Amount = @Amount,
			AmountPaid = @AmountPaid,
			BalanceDue = @BalanceDue,
			PaidDate = @PaidDate,
			TotalIssues = @TotalIssues,
			CheckNumber = @CheckNumber,
			CCNumber = @CCNumber,
			CCExpirationMonth = @CCExpirationMonth,
			CCExpirationYear = @CCExpirationYear,
			CCHolderName = @CCHolderName,
			CreditCardTypeID = @CreditCardTypeID,
			PaymentTypeID = @PaymentTypeID,
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID,
			DeliverID = @DeliverID,
			GraceIssues = @GraceIssues,
			WriteOffAmount = @WriteOffAmount,
			OtherType = @OtherType
		WHERE SubscriptionPaidID = @SubscriptionPaidID 
		
		SELECT @SubscriptionPaidID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO SubscriptionPaid (SubscriptionID,PriceCodeID,StartIssueDate,ExpireIssueDate,CPRate,Amount,AmountPaid,
									  BalanceDue,PaidDate,TotalIssues,CheckNumber,CCNumber,CCExpirationMonth,CCExpirationYear,CCHolderName,
									  CreditCardTypeID,PaymentTypeID,DateCreated,CreatedByUserID,DeliverID,GraceIssues,WriteOffAmount,OtherType)

		VALUES(@SubscriptionID,@PriceCodeID,@StartIssueDate,@ExpireIssueDate,@CPRate,@Amount,@AmountPaid,
			   @BalanceDue,@PaidDate,@TotalIssues,@CheckNumber,@CCNumber,@CCExpirationMonth,@CCExpirationYear,@CCHolderName,
			   @CreditCardTypeID,@PaymentTypeID,@DateCreated,@CreatedByUserID,@DeliverID,@GraceIssues,@WriteOffAmount,@OtherType);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_MarketingMap_Save]...';


GO

ALTER PROCEDURE e_MarketingMap_Save
@MarketingID int,
@SubscriberID int,
@PublicationID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF NOT EXISTS(SELECT MarketingID FROM MarketingMap With(NoLock) WHERE MarketingID = @MarketingID AND SubscriberID = @SubscriberID AND PublicationID = @PublicationID)
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO MarketingMap (MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@MarketingID,@SubscriberID,@PublicationID,@IsActive,@DateCreated,@CreatedByUserID);
	END
ELSE
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE MarketingMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		 WHERE MarketingID = @MarketingID AND SubscriberID = @SubscriberID AND PublicationID = @PublicationID;
	END

	DELETE FROM MarketingMap WHERE SubscriberID = @SubscriberID AND IsActive = 0
GO
PRINT N'Altering [dbo].[e_Subscriber_Search]...';


GO

ALTER PROCEDURE [dbo].[e_Subscriber_Search]
@Search nvarchar(4000),
@SearchFields nvarchar(4000) = '',
@OrderBy nvarchar(2000) = ''
AS 
DECLARE @FindWord nvarchar(4000)
Declare @individual nvarchar(100) = null

CREATE table #Indiv  
(
	[SubscriberID] [int] NOT NULL,
	[ExternalKeyID] [int] NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Company] [nvarchar](100) NULL,
	[Title] [nvarchar](255) NULL,
	Occupation [nvarchar](255) NULL,
	[AddressTypeID] [int] NULL,
	[Address1] [nvarchar](100) NULL,
	[Address2] [nvarchar](100) NULL,
	[Address3] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
	RegionCode [nchar](50) NULL,
	RegionID [int] NULL,
	[ZipCode] [nchar](10) NULL,
	[Plus4] [nchar](10) NULL,
	[CarrierRoute] [varchar](10) NULL,
	[County] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[CountryID] [int] NULL,
	Latitude decimal(18,15) null,
	Longitude decimal(18,15) null,
	[IsAddressValidated] [bit] NOT NULL,
	[AddressValidationDate] [datetime] NULL,
	[AddressValidationSource] [nvarchar](50) NULL,
	[AddressValidationMessage] [nvarchar](max) NULL,
	[Email] [nvarchar](255) NULL,
	[Phone] [nchar](50) NULL,
	[Fax] [nchar](50) NULL,
	[Mobile] [nchar](50) NULL,
	[Website] [nvarchar](255) NULL,
	Birthdate date NULL,
	Age int NULL,
	Income [nvarchar](50) NULL,
	Gender [nvarchar](50) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedByUserID] [int] NOT NULL,
	[UpdatedByUserID] [int] NULL,
	[IsLocked] [bit] NOT NULL
)

IF LEN(@SearchFields) = 0
	BEGIN
		WHILE LEN(@Search) > 0
		BEGIN
			IF PATINDEX('% %',@Search) > 0
			BEGIN
				SET @individual = SUBSTRING(@Search, 0, PATINDEX('% %',@Search))
				SET @FindWord = '"' +@individual + '*"'
				-- do query here
				Insert Into #Indiv
				SELECT *
				FROM Individual WITH (NOLOCK)
				WHERE (CONTAINS(FirstName,@FindWord)) OR
						(CONTAINS(LastName,@FindWord)) OR
						(CONTAINS(Company,@FindWord)) OR
						(CONTAINS(Title,@FindWord)) OR
						(CONTAINS(Address1,@FindWord)) OR
						(CONTAINS(City,@FindWord)) OR
						(CONTAINS(State,@FindWord)) OR
						(CONTAINS(ZipCode,@FindWord)) OR
						(CONTAINS(Email,@FindWord)) OR
						(CONTAINS(Phone,@FindWord)) OR
						(CONTAINS(Mobile,@FindWord)) 
				
				----------------------
				SET @Search = REPLACE(@Search, @individual + ' ','')
			END
			ELSE
			BEGIN
				SET @individual = @Search
				SET @FindWord = '"' +@individual + '*"'
				-- do query here
				Insert Into #Indiv
				SELECT *
				FROM Individual WITH (NOLOCK)
				WHERE (CONTAINS(FirstName,@FindWord)) OR
						(CONTAINS(LastName,@FindWord)) OR
						(CONTAINS(Company,@FindWord)) OR
						(CONTAINS(Title,@FindWord)) OR
						(CONTAINS(Address1,@FindWord)) OR
						(CONTAINS(City,@FindWord)) OR
						(CONTAINS(State,@FindWord)) OR
						(CONTAINS(ZipCode,@FindWord)) OR
						(CONTAINS(Email,@FindWord)) OR
						(CONTAINS(Phone,@FindWord)) OR
						(CONTAINS(Mobile,@FindWord)) 
				
				----------------------
				SET @Search = NULL
			END
		END
	END
ELSE
	BEGIN
		DECLARE @Column nvarchar(50)
		DECLARE @OriginalSearch nvarchar(4000) = @Search
		
		DECLARE c Cursor
		FOR 
			SELECT Items FROM dbo.fn_Split(@SearchFields,',')
		OPEN c
		FETCH NEXT FROM c INTO @Column
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @Search = @OriginalSearch
			WHILE LEN(@Search) > 0
			BEGIN
				IF PATINDEX('% %',@Search) > 0
				BEGIN
					SET @individual = SUBSTRING(@Search, 0, PATINDEX('% %',@Search))
					SET @FindWord = '"' +@individual + '*"'

					IF @Column = 'FirstName'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(FirstName,@FindWord))
					END
					
					IF @Column = 'LastName'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(LastName,@FindWord))
					END
					
					IF @Column = 'Company'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Company,@FindWord))
					END
					
					IF @Column = 'Title'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Title,@FindWord))
					END
					
					IF @Column = 'Address1'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Address1,@FindWord))
					END
					
					IF @Column = 'City'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(City,@FindWord))
					END
					
					IF @Column = 'State'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(State,@FindWord))
					END
					
					IF @Column = 'ZipCode'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(ZipCode,@FindWord))
					END
					
					IF @Column = 'Email'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Email,@FindWord))
					END
					
					--IF @Column = 'Phone'
					--BEGIN
					--	INSERT INTO #Indiv
					--		SELECT *
					--		FROM Individual WITH (NOLOCK)
					--		WHERE (CONTAINS(Phone,@FindWord))
					--END
					
					IF (@Column = 'Mobile') OR (@Column = 'Phone')
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Mobile,@FindWord)) OR (CONTAINS(Phone,@FindWord))
					END
										
					----------------------
					SET @Search = REPLACE(@Search, @individual + ' ','')
				END
				ELSE
				BEGIN
					SET @individual = @Search
					SET @FindWord = '"' +@individual + '*"'
					-- do query here
					IF @Column = 'FirstName'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(FirstName,@FindWord))
					END
					
					IF @Column = 'LastName'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(LastName,@FindWord))
					END
					IF @Column = 'Company'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Company,@FindWord))
					END
					
					IF @Column = 'Title'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Title,@FindWord))
					END
					
					IF @Column = 'Address1'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Address1,@FindWord))
					END
					
					IF @Column = 'City'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(City,@FindWord))
					END
					
					IF @Column = 'State'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(State,@FindWord))
					END
					
					IF @Column = 'ZipCode'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(ZipCode,@FindWord))
					END
					
					IF @Column = 'Email'
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Email,@FindWord))
					END
					
					--IF @Column = 'Phone'
					--BEGIN
					--	INSERT INTO #Indiv
					--		SELECT *
					--		FROM Individual WITH (NOLOCK)
					--		WHERE (CONTAINS(Phone,@FindWord))
					--END
					
					IF (@Column = 'Mobile') OR (@Column = 'Phone')
					BEGIN
						INSERT INTO #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(Mobile,@FindWord)) OR (CONTAINS(Phone,@FindWord))
					END
					
					----------------------
					SET @Search = NULL
				END
			END
			
			FETCH NEXT FROM c INTO @Column
		END
		CLOSE c
		DEALLOCATE c
	END

IF LEN(@OrderBy) > 0
	BEGIN
		DECLARE @SqlCode nvarchar(500) = 'SELECT * FROM #Indiv ORDER BY ' + @OrderBy
		EXEC(@SqlCode)
	END
ELSE
	BEGIN
		SELECT * FROM #Indiv
	END
DROP TABLE #Indiv
GO
PRINT N'Altering [dbo].[e_Subscriber_Search_Params]...';


GO
ALTER PROCEDURE [dbo].[e_Subscriber_Search_Params]
@fName varchar(100) = '',
@lName varchar(100) = '',
@Company varchar(100) = '',
@Title varchar(255) = '',
@Add1 varchar(100) = '',
@City varchar(50) = '',
@RegionCode varchar(50) = '',
@Zip varchar(50) = '',
@Country varchar(50) = '',
@Email varchar(255) = '',
@Phone varchar(50) = '',
@SequenceID int,
@AccountID varchar(50),
@PublisherID int,
@PublicationID int

AS

	CREATE table #Indiv  
	(
		[SubscriberID] [int] NOT NULL,
		[ExternalKeyID] [int] NULL,
		[FirstName] [varchar](50) NULL,
		[LastName] [varchar](50) NULL,
		[Company] [varchar](100) NULL,
		[Title] [varchar](255) NULL,
		Occupation [varchar](255) NULL,
		[AddressTypeID] [int] NULL,
		[Address1] [varchar](100) NULL,
		[Address2] [varchar](100) NULL,
		[Address3] [varchar](100) NULL,
		[City] [varchar](50) NULL,
		RegionCode [varchar](50) NULL,
		RegionID [int] NULL,
		[ZipCode] [varchar](10) NULL,
		[Plus4] [varchar](10) NULL,
		[CarrierRoute] [varchar](10) NULL,
		[County] [varchar](50) NULL,
		[Country] [varchar](50) NULL,
		[CountryID] [int] NULL,
		Latitude decimal(18,15) null,
		Longitude decimal(18,15) null,
		[IsAddressValidated] [bit] NOT NULL,
		[AddressValidationDate] [datetime] NULL,
		[AddressValidationSource] [varchar](50) NULL,
		[AddressValidationMessage] [varchar](max) NULL,
		[Email] [varchar](255) NULL,
		[Phone] [varchar](50) NULL,
		[Fax] [varchar](50) NULL,
		[Mobile] [varchar](50) NULL,
		[Website] [varchar](255) NULL,
		Birthdate date NULL,
		Age int NULL,
		Income [varchar](50) NULL,
		Gender [varchar](50) NULL,
		[DateCreated] [datetime] NOT NULL,
		[DateUpdated] [datetime] NULL,
		[CreatedByUserID] [int] NOT NULL,
		[UpdatedByUserID] [int] NULL,
		tmpSubscriptionID int null,
		[IsLocked] bit NOT NULL,
		LockedByUserID int NULL,
		LockDate datetime NULL,
		LockDateRelease datetime NULL,
		PhoneExt [varchar](25) NULL,
		IsInActiveWaveMailing bit,
		AddressTypeCodeId int null,
		AddressLastUpdatedDate datetime null,
		AddressUpdatedSourceTypeCodeId int null,
		WaveMailingID int NULL,
		Igrp_No uniqueidentifier,
		SFRecordIdentifier uniqueidentifier,
		PublisherID int null,
		PublicationID int null,
		PublisherName varchar(256),
		PublicationCode varchar(50)
	)

		declare @FirstName varchar(50)
		declare @LastName varchar(50)
		declare @WhereSqlStmt varchar(max)
		declare @WhereLeftSqlStmt varchar(max)
		declare @SqlStmt varchar(max)

		
		--IF LEN(@FullName) > 0
		--BEGIN
		--	IF(@nameCount) <= 2
		--	BEGIN
		--		IF PATINDEX('% %',@FullName) > 0
		--		BEGIN
		--			SET @FirstName = SUBSTRING(@FullName, 0, PATINDEX('% %',@FullName))	
					
		--			DECLARE @FirstNameLength int = LEN(@FirstName)
					
		--			SET @WhereSqlStmt = 'dbo.fn_Levenshtein(LEFT(FirstName,'+CAST(@FirstNameLength as VARCHAR(256))+'),'''+@FirstName+''') > 75'
		--			SET @WhereLeftSqlStmt = 'Left(FirstName,1) = Left('''+@FirstName+''',1)'
					
		--			SET @LastName = REPLACE(@FullName, @FirstName + ' ','')
					
		--			IF LEN(@LastName) > 0
		--			DECLARE @LastNameLength int = LEN(@LastName)
		--			BEGIN
		--				SET @WhereSqlStmt += ' and dbo.fn_Levenshtein(LEFT(LastName,'+CAST(@LastNameLength as VARCHAR(256))+'),'''+@LastName+''') > 75'
		--				SET @WhereLeftSqlStmt += ' and Left(LastName,1) = Left('''+@LastName+''',1)'
		--			END
		--		END
		--		ELSE IF PATINDEX('% %',@FullName) = 0
		--		BEGIN
		--			SET @WhereSqlStmt = '(dbo.fn_Levenshtein(FirstName,'''+@FullName+''') > 75 or dbo.fn_Levenshtein(LastName,'''+@FullName+''') > 75)'
		--			SET @WhereLeftSqlStmt = '(Left(FirstName,1) = Left('''+@FullName+''',1) or Left(LastName,1) = Left('''+@FullName+''',1))'
		--		END
		--	END
		--	ELSE IF(@nameCount) >= 3
		--	BEGIN
		--		SET @WhereSqlStmt = 'dbo.fn_Levenshtein(LEFT(FirstName + '' '' + lastname,'+CAST((select LEN(@FullName)) as VARCHAR(256))+'),'''+@FullName+''') > 75'
		--	END
		--END

		--IF LEN(@FullName) > 0
		--BEGIN
		--		IF PATINDEX('% %',@FullName) = 0
		--		BEGIN
		--			SET @WhereSqlStmt = '(dbo.fn_Levenshtein(FirstName,'''+@FullName+''') > 75 or dbo.fn_Levenshtein(LastName,'''+@FullName+''') > 75)'
		--			SET @WhereLeftSqlStmt = '(Left(FirstName,1) = Left('''+@FullName+''',1) or Left(LastName,1) = Left('''+@FullName+''',1))'
		--		END
		--		ELSE
		--		BEGIN
		--			SET @WhereSqlStmt = 'dbo.fn_Levenshtein(REPLACE(LEFT(FirstName + lastname,'+CAST((select LEN(@FullName)) as VARCHAR(256))+'),'' '',''''),'''+@FullName+''') > 80 '
		--			SET @WhereLeftSqlStmt = '(LEFT(FirstName + LastName,1) = LEFT('''+@FullName+''',1))'
		--		END
		--END
		
		if LEN(@fname) > 0
		begin
			set @WhereSqlStmt = ' firstName like ''' + @fName + '%'''
		end
		
		if LEN(@lName) > 0
		begin
			if LEN(@WhereSqlStmt) > 0
			begin
				set @WhereSqlStmt += ' and lastName like ''' + @lName + '%'''
			end
			else
			begin
				set @WhereSqlStmt = ' lastName like ''' + @lName + '%'''
			end
		end

		IF LEN(@Company) > 0
		BEGIN

			IF LEN(@WhereSqlStmt) > 0			
			BEGIN
				SET @WhereSqlStmt += ' and Company like ''' + @Company + '%'''
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = ' Company like ''' + @Company + '%'''
			END
		END

		IF LEN(@Title) > 0
		BEGIN

			IF LEN(@WhereSqlStmt) > 0			
			BEGIN
				SET @WhereSqlStmt += ' and Title like ''' + @Title + '%'''
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = ' Title like ''' + @Title + '%'''
			END
		END
		
		IF LEN(@Add1) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0			
			BEGIN
				SET @WhereSqlStmt += ' and Address1 like ''' + @Add1 + '%'''
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = ' Address1 like ''' + @Add1 + '%'''
			END
		END
		
		IF LEN(@City) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0			
			BEGIN
				SET @WhereSqlStmt += ' and City like ''' + @City + '%'''
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = ' City like ''' + @City + '%'''
			END
		END
		
		IF LEN(@RegionCode) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and RegionCode = '''+@RegionCode+''''		
			ELSE
				SET @WhereSqlStmt = 'RegionCode = '''+@RegionCode+''''		
		END	

		IF LEN(@Zip) > 0
		BEGIN
			DECLARE @Zip5 varchar(25)
			DECLARE @Zip4 varchar(25)
		
			IF LEN(@Zip) = 10
			BEGIN
				SET @Zip5 = SUBSTRING(@Zip,1,5)
				SET @Zip4 = SUBSTRING(@Zip,7,4)
			END
			ELSE IF LEN(@Zip) = 7
			BEGIN
				SET @Zip5 = SUBSTRING(@Zip,1,3)
				SET @Zip4 = SUBSTRING(@Zip,5,3)
			END
			ELSE
			BEGIN
				SET @Zip5 = @Zip
			END
			
			IF LEN(@Zip4) > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and ZipCode = '''+@Zip5+''' and Plus4 = '''+@Zip4+''''		
				ELSE
					SET @WhereSqlStmt = 'ZipCode = '''+@Zip5+''' and Plus4 = '''+@Zip4+''''	
			END
			ELSE
			BEGIN
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and ZipCode like '''+@Zip5+'%'''		
				ELSE
					SET @WhereSqlStmt = 'ZipCode like '''+@Zip5+'%'''	
			END	
		END
		
		IF LEN(@Country) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0			
			BEGIN
				SET @WhereSqlStmt += ' and Country like ''' + @Country + '%'''
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = ' Country like ''' + @Country + '%'''
			END
		END
		
		IF LEN(@Email) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0			
			BEGIN
				SET @WhereSqlStmt += ' and Email like ''' + @Email + '%'''
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = ' Email like ''' + @Email + '%'''
			END
		END
		
		IF LEN(@Phone) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and (Phone like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'		
			ELSE
				SET @WhereSqlStmt = '(Phone like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'
		END	
				
		IF @SequenceID > 0
		BEGIN		
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.SequenceID = '''+cast(@SequenceID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.SequenceID = '''+cast(@SequenceID as varchar(25))+''''
				
		END
		
		IF LEN(@AccountID) > 0 
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.AccountNumber = '''+cast(@AccountID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.AccountNumber = '''+cast(@AccountID as varchar(25))+''''
				
		END
		
		IF @PublisherID > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''
		END
		
		IF @PublicationID > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.PublicationID = '''+cast(@PublicationID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.PublicationID = '''+cast(@PublicationID as varchar(25))+''''
		END
		
		
		--IF LEN(@WhereLeftSqlStmt) > 0
		--BEGIN
		--	SET @SqlStmt = 'Insert into #Indiv
		--					Select top 100 s.*,ss.PublisherID,ss.PublicationId,p.PublisherName,pp.PublicationCode
		--					FROM Subscriber s WITH (NOLOCK) INNER JOIN Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID
		--													INNER JOIN Publisher p with(nolock) on ss.PublisherId = p.PublisherId
		--													INNER JOIN Publication pp with(nolock) on ss.PublicationID = pp.PublicationId
		--					WHERE  ('+@WhereLeftSqlStmt+') and ('+ @WhereSqlStmt+')'
		--END
		--ELSE
		--BEGIN
		SET @SqlStmt = 'Insert into #Indiv
			Select top 100 s.*,ss.PublisherID,ss.PublicationID ,'''' as PublisherName,'''' as PublicationCode
			FROM Subscriber s WITH (NOLOCK) INNER JOIN Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID
			WHERE ' + @WhereSqlStmt				
		--END
		
		--PRINT(@SqlStmt)
		Exec(@SqlStmt)

	SELECT TOP 100 [SubscriberID],[ExternalKeyID],[FirstName],[LastName],[Company],[Title],Occupation,[AddressTypeID],[Address1],[Address2],[Address3],[City],RegionCode,RegionID,[ZipCode],
		[Plus4],[CarrierRoute],[County],[Country],[CountryID],Latitude,Longitude,[IsAddressValidated],[AddressValidationDate],[AddressValidationSource],[AddressValidationMessage],
		[Email],[Phone],[Fax],[Mobile],[Website],Birthdate,Age,Income,Gender,[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID],tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,PublisherID,PublicationID,
		PublisherName,PublicationCode, WaveMailingID, IsInActiveWaveMailing
	FROM #Indiv 
	GROUP BY [SubscriberID],[ExternalKeyID],[FirstName],[LastName],[Company],[Title],Occupation,[AddressTypeID],[Address1],[Address2],[Address3],[City],RegionCode,RegionID,[ZipCode],
		[Plus4],[CarrierRoute],[County],[Country],[CountryID],Latitude,Longitude,[IsAddressValidated],[AddressValidationDate],[AddressValidationSource],[AddressValidationMessage],
		[Email],[Phone],[Fax],[Mobile],[Website],Birthdate,Age,Income,Gender,[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID],tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,PublisherID,PublicationID,
		PublisherName,PublicationCode, WaveMailingID, IsInActiveWaveMailing
	Order By PublisherName,PublicationCode,[FirstName],[LastName],[Company],[Title],[Address1],[City],RegionCode,[ZipCode],[Email],[Phone] DESC

	drop table #Indiv
GO
PRINT N'Altering [dbo].[e_Subscriber_SuggestMatch]...';


GO

ALTER PROCEDURE [dbo].[e_Subscriber_SuggestMatch] 
	@FirstName varchar(50) = '',
	@LastName varchar(50) = '',
	@Email varchar(50) = '',
	@PublisherID int = '',
	@PublicationID int = ''

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @WhereSqlStmt varchar(max)
	DECLARE @WhereLeftSqlStmt varchar(max)
	DECLARE @SqlStmt varchar(max)

		IF LEN(@FirstName) > 0
		BEGIN
			SET @WhereSqlStmt = 'dbo.fn_Levenshtein(FirstName,'''+@FirstName+''') > 85'
			SET @WhereLeftSqlStmt = 'Left(FirstName,2) = Left('''+@FirstName+''',2)'
		END
		
		IF LEN(@LastName) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
			BEGIN
				SET @WhereSqlStmt += ' and dbo.fn_Levenshtein(LastName,'''+@LastName+''') > 85' 
				SET @WhereLeftSqlStmt += ' and Left(LastName,2) = Left('''+@LastName+''',2)'
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = 'dbo.fn_Levenshtein(LastName,'''+@LastName+''') > 85'
				SET @WhereLeftSqlStmt = 'Left(LastName,2) = Left('''+@LastName+''',2)'
			END
		END
		
		IF LEN(@Email) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
			BEGIN
				SET @WhereSqlStmt += ' and Email = '''+@Email+'''' 
				--SET @WhereLeftSqlStmt += ' and Left(Email,2) = Left('''+@Email+''',2)'
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = 'Email = '''+@Email+'''' 
				--SET @WhereLeftSqlStmt = 'Left(Email,2) = Left('''+@Email+''',2)'
			END
		END
			
	-- If @WhereLeftSqlStmt is 0 then there was no statements generated for that variable, therefore the @Sqlstmt should be the ELSE statement
	IF LEN(@WhereLeftSqlStmt) > 0
	BEGIN
		SET @SqlStmt = 'Select top 25 s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County 
								,Phone,Mobile,Fax,Email,Website 
						FROM Subscriber s WITH (NOLOCK) inner join Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID 
						WHERE ('+@WhereLeftSqlStmt+') and ('+ @WhereSqlStmt+') 
						and ss.PublisherID = '+ cast(@PublisherID as varchar(10)) +' and SS.PublicationID = '+ cast(@PublicationID as varchar(10)) +' 
						GROUP BY s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County
						 ,Phone,Mobile,Fax,Email,Website'
	END
	ELSE
	BEGIN
		SET @SqlStmt = 'Select top 25 s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County 
							,Phone,Mobile,Fax,Email,Website 
					FROM Subscriber s WITH (NOLOCK) inner join Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID 
					WHERE ('+ @WhereSqlStmt+') 
					and ss.PublisherID = '+ cast(@PublisherID as varchar(10)) +' and SS.PublicationID = '+ cast(@PublicationID as varchar(10)) +' 
					GROUP BY s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County
					 ,Phone,Mobile,Fax,Email,Website'
	END
	

	EXEC(@SqlStmt)
	
END
GO
PRINT N'Altering [dbo].[o_BatchHistoryDetail_Select_BatchID_Name_Sequence]...';


GO


ALTER PROCEDURE [dbo].[o_BatchHistoryDetail_Select_BatchID_Name_Sequence]
@BatchID int,
@Name varchar(500),
@SequenceID int
AS

	DECLARE @Sqlstmt VARCHAR(max)
	DECLARE @WhereStmt VARCHAR(max)
	
	-- Dynamically build the WHERE statement for the query below
	IF (@BatchID) > 0
	BEGIN
		SET @WhereStmt = ' b.BatchID = ' + CAST(@BatchID AS VARCHAR(25)) + ''
	END

	IF LEN(@Name) > 0
	BEGIN
		IF LEN(@WhereStmt) > 0
		BEGIN
			SET @WhereStmt += ' AND (ISNULL(s.FirstName, '''') + '' '' + ISNULL(s.LastName, '''') LIKE ''%'' + '''+ NULLIF(@Name, '') +''' +''%'')'
		END
		ELSE
		BEGIN
			SET @WhereStmt = ' ISNULL(s.FirstName, '''') + '' '' + ISNULL(s.LastName, '''') LIKE ''%'' + '''+ NULLIF(@Name, '') +''' +''%'' '
		END
	END

	IF (@SequenceID) > 0
	BEGIN
		IF LEN(@WhereStmt) > 0
		BEGIN
			SET @WhereStmt += ' AND sub.SequenceID = ' + CAST(@SequenceID AS VARCHAR(25)) + ' '
		END
		ELSE
		BEGIN
			SET @WhereStmt = ' sub.SequenceID = ' + CAST(@SequenceID AS VARCHAR(25)) + ' '
		END
	END
	-- Execute query for results, add @WhereStmt if the LEN is longer than zero
	IF LEN(@WhereStmt) > 0
	BEGIN
	SET @Sqlstmt = 'SELECT DISTINCT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as ''BatchDateCreated'',
					   b.DateFinalized as ''BatchDateFinalized'',
					   p.PublicationID,p.PublicationName,p.PublicationCode,
					   pub.PublisherID,pub.PublisherName,
					   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
					   h.HistorySubscriptionID,
					   h.DateCreated as ''HistoryDateCreated'',
					   s.FirstName, s.LastName, s.FirstName + '' '' + s.LastName as ''FullName'',
					   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated ''UserLogDateCreated'',
					   sub.SequenceID
					FROM Batch b With(NoLock)
					JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
					JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
					JOIN History h With(NoLock) ON b.BatchID = h.BatchID
					JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
					LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
					LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
					LEFT JOIN Subscription sub With(NoLock) ON s.SubscriberID = sub.SubscriberID
					WHERE ' + @WhereStmt
	END

	EXEC(@Sqlstmt)
GO
PRINT N'Creating [dbo].[e_AcsFileDetail_Save]...';


GO
create procedure e_AcsFileDetail_Save
@AcsFileDetailId int,
@ClientId int,
@RecordType varchar(1),
@FileVersion varchar(2),
@SequenceNumber int,
@AcsMailerId varchar(9), 
@KeylineSequenceSerialNumber varchar(16),
@MoveEffectiveDate date, 
@MoveType varchar(1), 
@DeliverabilityCode varchar(1), 
@UspsSiteID int,
@LastName varchar(20),
@FirstName varchar(15),
@Prefix varchar(6),
@Suffix varchar(6),
@OldAddressType varchar(1),
@OldUrb varchar(28),
@OldPrimaryNumber varchar(10),
@OldPreDirectional varchar(2),
@OldStreetName varchar(28),
@OldSuffix varchar(4),
@OldPostDirectional varchar(2),
@OldUnitDesignator varchar(4),
@OldSecondaryNumber varchar(10),
@OldCity varchar(28),
@OldStateAbbreviation varchar(2),
@OldZipCode varchar(5),
@NewAddressType varchar(1), 
@NewPmb varchar(8),
@NewUrb varchar(28),
@NewPrimaryNumber varchar(10),
@NewPreDirectional varchar(2),
@NewStreetName varchar(28),
@NewSuffix varchar(4),
@NewPostDirectional varchar(2),
@NewUnitDesignator varchar(4),
@NewSecondaryNumber varchar(10),
@NewCity varchar(28),
@NewStateAbbreviation varchar(2),
@NewZipCode varchar(5),
@Hyphen varchar(1),
@NewPlus4Code varchar(4),
@NewDeliveryPoint varchar(2),
@NewAbbreviatedCityName varchar(13),
@NewAddressLabel varchar(66),
@FeeNotification varchar(1), 
@NotificationType varchar(1), 
@IntelligentMailBarcode varchar(31), 
@IntelligentMailPackageBarcode varchar(35), 
@IdTag varchar(16), 
@HardcopyToElectronicFlag varchar(1),
@TypeOfAcs varchar(1), 
@FulfillmentDate date,
@ProcessingType varchar(1), 
@CaptureType varchar(1),
@MadeAvailableDate date,
@ShapeOfMail varchar(1), 
@MailActionCode varchar(1),
@NixieFlag varchar(1), 
@ProductCode1 int,
@ProductCodeFee1 decimal(4,2),
@ProductCode2 int,
@ProductCodeFee2 decimal(4,2),
@ProductCode3 int,
@ProductCodeFee3 decimal(4,2),
@ProductCode4 int,
@ProductCodeFee4 decimal(4,2),
@ProductCode5 int,
@ProductCodeFee5 decimal(4,2),
@ProductCode6 int,
@ProductCodeFee6 decimal(4,2),
@Filler varchar(405),
@EndMarker varchar(1),
@ProductCode varchar(50),
@OldAddress1 varchar(100),
@OldAddress2 varchar(100),
@OldAddress3 varchar(100),
@NewAddress1 varchar(100),
@NewAddress2 varchar(100),
@NewAddress3 varchar(100),
@SequenceID int,
@TransactionCodeValue int,
@CategoryCodeValue int,
@IsIgnored bit,
@AcsActionId int,
@CreatedDate date,
@CreatedTime time(7),
@ProcessCode varchar(50)
as
	IF @AcsFileDetailId > 0
		BEGIN
			UPDATE AcsFileDetail
			SET ClientId = @ClientId,
				RecordType = @RecordType,
				FileVersion = @FileVersion,
				SequenceNumber = @SequenceNumber,
				AcsMailerId = @AcsMailerId,
				KeylineSequenceSerialNumber = @KeylineSequenceSerialNumber,
				MoveEffectiveDate = @MoveEffectiveDate,
				MoveType = @MoveType,
				DeliverabilityCode = @DeliverabilityCode,
				UspsSiteID = @UspsSiteID,
				LastName = @LastName,
				FirstName = @FirstName,
				Prefix = @Prefix,
				Suffix = @Suffix,
				OldAddressType = @OldAddressType,
				OldUrb = @OldUrb,
				OldPrimaryNumber = @OldPrimaryNumber,
				OldPreDirectional = @OldPreDirectional,
				OldStreetName = @OldStreetName,
				OldSuffix = @OldSuffix,
				OldPostDirectional = @OldPostDirectional,
				OldUnitDesignator = @OldUnitDesignator,
				OldSecondaryNumber = @OldSecondaryNumber,
				OldCity = @OldCity,
				OldStateAbbreviation = @OldStateAbbreviation,
				OldZipCode = @OldZipCode,
				NewAddressType = @NewAddressType,
				NewPmb = @NewPmb,
				NewUrb = @NewUrb,
				NewPrimaryNumber = @NewPrimaryNumber,
				NewPreDirectional = @NewPreDirectional,
				NewStreetName = @NewStreetName,
				NewSuffix = @NewSuffix,
				NewPostDirectional = @NewPostDirectional,
				NewUnitDesignator = @NewUnitDesignator,
				NewSecondaryNumber = @NewSecondaryNumber,
				NewCity = @NewCity,
				NewStateAbbreviation = @NewStateAbbreviation,
				NewZipCode = @NewZipCode,
				Hyphen = @Hyphen,
				NewPlus4Code = @NewPlus4Code,
				NewDeliveryPoint = @NewDeliveryPoint,
				NewAbbreviatedCityName = @NewAbbreviatedCityName,
				NewAddressLabel = @NewAddressLabel,
				FeeNotification = @FeeNotification,
				NotificationType = @NotificationType,
				IntelligentMailBarcode = @IntelligentMailBarcode,
				IntelligentMailPackageBarcode = @IntelligentMailPackageBarcode,
				IdTag = @IdTag,
				HardcopyToElectronicFlag = @HardcopyToElectronicFlag,
				TypeOfAcs = @TypeOfAcs,
				FulfillmentDate = @FulfillmentDate,
				ProcessingType = @ProcessingType,
				CaptureType = @CaptureType,
				MadeAvailableDate = @MadeAvailableDate,
				ShapeOfMail = @ShapeOfMail,
				MailActionCode = @MailActionCode,
				NixieFlag = @NixieFlag,
				ProductCode1 = @ProductCode1,
				ProductCodeFee1 = @ProductCodeFee1,
				ProductCode2 = @ProductCode2,
				ProductCodeFee2 = @ProductCodeFee2,
				ProductCode3 = @ProductCode3,
				ProductCodeFee3 = @ProductCodeFee3,
				ProductCode4 = @ProductCode4,
				ProductCodeFee4 = @ProductCodeFee4,
				ProductCode5 = @ProductCode5,
				ProductCodeFee5 = @ProductCodeFee5,
				ProductCode6 = @ProductCode6,
				ProductCodeFee6 = @ProductCodeFee6,
				Filler = @Filler,
				EndMarker = @EndMarker,
				ProductCode = @ProductCode,
				OldAddress1 = @OldAddress1,
				OldAddress2 = @OldAddress2,
				OldAddress3 = @OldAddress3,
				NewAddress1 = @NewAddress1,
				NewAddress2 = @NewAddress2,
				NewAddress3 = @NewAddress3,
				SequenceID = @SequenceID,
				TransactionCodeValue = @TransactionCodeValue,
				CategoryCodeValue = @CategoryCodeValue,
				IsIgnored = @IsIgnored,
				AcsActionId = @AcsActionId,
				ProcessCode = @ProcessCode
			WHERE AcsFileDetailId = @AcsFileDetailId;

			SELECT @AcsFileDetailId;
		END
	ELSE
		BEGIN
			INSERT INTO AcsFileDetail (ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,
									   UspsSiteID,LastName,FirstName,Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,
									   OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,
									   NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,NewZipCode,Hyphen,NewPlus4Code,
									   NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,
									   IdTag,HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,
									   ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,
									   ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,
									   ProductCode,OldAddress1,OldAddress2,OldAddress3,NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,AcsActionId,CreatedDate,CreatedTime,ProcessCode)
			VALUES(@ClientId,@RecordType,@FileVersion,@SequenceNumber,@AcsMailerId,@KeylineSequenceSerialNumber,@MoveEffectiveDate,@MoveType,@DeliverabilityCode,
				   @UspsSiteID,@LastName,@FirstName,@Prefix,@Suffix,@OldAddressType,@OldUrb,@OldPrimaryNumber,@OldPreDirectional,@OldStreetName,@OldSuffix,@OldPostDirectional,
				   @OldUnitDesignator,@OldSecondaryNumber,@OldCity,@OldStateAbbreviation,@OldZipCode,@NewAddressType,@NewPmb,@NewUrb,@NewPrimaryNumber,@NewPreDirectional,
				   @NewStreetName,@NewSuffix,@NewPostDirectional,@NewUnitDesignator,@NewSecondaryNumber,@NewCity,@NewStateAbbreviation,@NewZipCode,@Hyphen,@NewPlus4Code,
				   @NewDeliveryPoint,@NewAbbreviatedCityName,@NewAddressLabel,@FeeNotification,@NotificationType,@IntelligentMailBarcode,@IntelligentMailPackageBarcode,
				   @IdTag,@HardcopyToElectronicFlag,@TypeOfAcs,@FulfillmentDate,@ProcessingType,@CaptureType,@MadeAvailableDate,@ShapeOfMail,@MailActionCode,@NixieFlag,
				   @ProductCode1,@ProductCodeFee1,@ProductCode2,@ProductCodeFee2,@ProductCode3,@ProductCodeFee3,@ProductCode4,@ProductCodeFee4,@ProductCode5,
				   @ProductCodeFee5,@ProductCode6,@ProductCodeFee6,@Filler,@EndMarker,
				   @ProductCode,@OldAddress1,@OldAddress2,@OldAddress3,@NewAddress1,@NewAddress2,@NewAddress3,@SequenceID,@TransactionCodeValue,@CategoryCodeValue,@IsIgnored,@AcsActionId,@CreatedDate,@CreatedTime,@ProcessCode);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_AcsFileDetail_Select_ProcessCode]...';


GO
create procedure e_AcsFileDetail_Select_ProcessCode
@ProcessCode varchar(50)
as
	select *
	from AcsFileDetail with(nolock)
	where ProcessCode = @ProcessCode
GO
PRINT N'Creating [dbo].[e_AcsFileDetail_Update_Xml]...';


GO
create procedure e_AcsFileDetail_Update_Xml
@xml xml
as
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int

	DECLARE @import TABLE
	(
		AcsFileDetailId int NOT NULL,
		ClientId int NULL,
		RecordType varchar(1) NULL,
		FileVersion varchar(2) NULL,
		SequenceNumber int NULL,
		AcsMailerId varchar(9) NULL,
		KeylineSequenceSerialNumber varchar(16) NULL,
		MoveEffectiveDate date NULL,
		MoveType varchar(1) NULL,
		DeliverabilityCode varchar(1) NULL,
		UspsSiteID int NULL,
		LastName varchar(20) NULL,
		FirstName varchar(15) NULL,
		Prefix varchar(6) NULL,
		Suffix varchar(6) NULL,
		OldAddressType varchar(1) NULL,
		OldUrb varchar(28) NULL,
		OldPrimaryNumber varchar(10) NULL,
		OldPreDirectional varchar(2) NULL,
		OldStreetName varchar(28) NULL,
		OldSuffix varchar(4) NULL,
		OldPostDirectional varchar(2) NULL,
		OldUnitDesignator varchar(4) NULL,
		OldSecondaryNumber varchar(10) NULL,
		OldCity varchar(28) NULL,
		OldStateAbbreviation varchar(2) NULL,
		OldZipCode varchar(5) NULL,
		NewAddressType varchar(1) NULL,
		NewPmb varchar(8) NULL,
		NewUrb varchar(28) NULL,
		NewPrimaryNumber varchar(10) NULL,
		NewPreDirectional varchar(2) NULL,
		NewStreetName varchar(28) NULL,
		NewSuffix varchar(4) NULL,
		NewPostDirectional varchar(2) NULL,
		NewUnitDesignator varchar(4) NULL,
		NewSecondaryNumber varchar(10) NULL,
		NewCity varchar(28) NULL,
		NewStateAbbreviation varchar(2) NULL,
		NewZipCode varchar(5) NULL,
		Hyphen varchar(1) NULL,
		NewPlus4Code varchar(4) NULL,
		NewDeliveryPoint varchar(2) NULL,
		NewAbbreviatedCityName varchar(13) NULL,
		NewAddressLabel varchar(66) NULL,
		FeeNotification varchar(1) NULL,
		NotificationType varchar(1) NULL,
		IntelligentMailBarcode varchar(31) NULL,
		IntelligentMailPackageBarcode varchar(35) NULL,
		IdTag varchar(16) NULL,
		HardcopyToElectronicFlag varchar(1) NULL,
		TypeOfAcs varchar(1) NULL,
		FulfillmentDate date NULL,
		ProcessingType varchar(1) NULL,
		CaptureType varchar(1) NULL,
		MadeAvailableDate date NULL,
		ShapeOfMail varchar(1) NULL,
		MailActionCode varchar(1) NULL,
		NixieFlag varchar(1) NULL,
		ProductCode1 int NULL,
		ProductCodeFee1 decimal(4, 2) NULL,
		ProductCode2 int NULL,
		ProductCodeFee2 decimal(4, 2) NULL,
		ProductCode3 int NULL,
		ProductCodeFee3 decimal(4, 2) NULL,
		ProductCode4 int NULL,
		ProductCodeFee4 decimal(4, 2) NULL,
		ProductCode5 int NULL,
		ProductCodeFee5 decimal(4, 2) NULL,
		ProductCode6 int NULL,
		ProductCodeFee6 decimal(4, 2) NULL,
		Filler varchar(405) NULL,
		EndMarker varchar(1) NULL,
		ProductCode varchar(50) NULL,
		OldAddress1 varchar(100) NULL,
		OldAddress2 varchar(100) NULL,
		OldAddress3 varchar(100) NULL,
		NewAddress1 varchar(100) NULL,
		NewAddress2 varchar(100) NULL,
		NewAddress3 varchar(100) NULL,
		SequenceID int NULL,
		TransactionCodeValue int NULL,
		CategoryCodeValue int NULL,
		IsIgnored bit NULL,
		AcsActionId int NULL,
		ProcessCode varchar(50) NULL
	)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into @import 
	(
		 AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
		Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
		OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
		NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
		HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
		ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
		NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,AcsActionId,ProcessCode
	)  
	
	SELECT 
		AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
		Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
		OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
		NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
		HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
		ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
		NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,AcsActionId,ProcessCode
	FROM OPENXML(@docHandle, N'/XML/AcsFileDetail') 
	WITH   
	(  
		AcsFileDetailId int 'AcsFileDetailId',
		ClientId int 'ClientId',
		RecordType varchar(1) 'RecordType',
		FileVersion varchar(2) 'FileVersion',
		SequenceNumber int 'SequenceNumber',
		AcsMailerId varchar(9) 'AcsMailerId',
		KeylineSequenceSerialNumber varchar(16) 'KeylineSequenceSerialNumber',
		MoveEffectiveDate date 'MoveEffectiveDate',
		MoveType varchar(1) 'MoveType',
		DeliverabilityCode varchar(1) 'DeliverabilityCode',
		UspsSiteID int 'UspsSiteID',
		LastName varchar(20) 'LastName',
		FirstName varchar(15) 'FirstName',
		Prefix varchar(6) 'Prefix',
		Suffix varchar(6) 'Suffix',
		OldAddressType varchar(1) 'OldAddressType',
		OldUrb varchar(28) 'OldUrb',
		OldPrimaryNumber varchar(10) 'OldPrimaryNumber',
		OldPreDirectional varchar(2) 'OldPreDirectional',
		OldStreetName varchar(28) 'OldStreetName',
		OldSuffix varchar(4) 'OldSuffix',
		OldPostDirectional varchar(2) 'OldPostDirectional',
		OldUnitDesignator varchar(4) 'OldUnitDesignator',
		OldSecondaryNumber varchar(10) 'OldSecondaryNumber',
		OldCity varchar(28) 'OldCity',
		OldStateAbbreviation varchar(2) 'OldStateAbbreviation',
		OldZipCode varchar(5) 'OldZipCode',
		NewAddressType varchar(1) 'NewAddressType',
		NewPmb varchar(8) 'NewPmb',
		NewUrb varchar(28) 'NewUrb',
		NewPrimaryNumber varchar(10) 'NewPrimaryNumber',
		NewPreDirectional varchar(2) 'NewPreDirectional',
		NewStreetName varchar(28) 'NewStreetName',
		NewSuffix varchar(4) 'NewSuffix',
		NewPostDirectional varchar(2) 'NewPostDirectional',
		NewUnitDesignator varchar(4) 'NewUnitDesignator',
		NewSecondaryNumber varchar(10) 'NewSecondaryNumber',
		NewCity varchar(28) 'NewCity',
		NewStateAbbreviation varchar(2) 'NewStateAbbreviation',
		NewZipCode varchar(5) 'NewZipCode',
		Hyphen varchar(1) 'Hyphen',
		NewPlus4Code varchar(4) 'NewPlus4Code',
		NewDeliveryPoint varchar(2) 'NewDeliveryPoint',
		NewAbbreviatedCityName varchar(13) 'NewAbbreviatedCityName',
		NewAddressLabel varchar(66) 'NewAddressLabel',
		FeeNotification varchar(1) 'FeeNotification',
		NotificationType varchar(1) 'NotificationType',
		IntelligentMailBarcode varchar(31) 'IntelligentMailBarcode',
		IntelligentMailPackageBarcode varchar(35) 'IntelligentMailPackageBarcode',
		IdTag varchar(16) 'IdTag',
		HardcopyToElectronicFlag varchar(1) 'HardcopyToElectronicFlag',
		TypeOfAcs varchar(1) 'TypeOfAcs',
		FulfillmentDate date 'FulfillmentDate',
		ProcessingType varchar(1) 'ProcessingType',
		CaptureType varchar(1) 'CaptureType',
		MadeAvailableDate date 'MadeAvailableDate',
		ShapeOfMail varchar(1) 'ShapeOfMail',
		MailActionCode varchar(1) 'MailActionCode',
		NixieFlag varchar(1) 'NixieFlag',
		ProductCode1 int 'ProductCode1',
		ProductCodeFee1 decimal(4, 2) 'ProductCodeFee1',
		ProductCode2 int 'ProductCode2',
		ProductCodeFee2 decimal(4, 2) 'ProductCodeFee2',
		ProductCode3 int 'ProductCode3',
		ProductCodeFee3 decimal(4, 2) 'ProductCodeFee3',
		ProductCode4 int 'ProductCode4',
		ProductCodeFee4 decimal(4, 2) 'ProductCodeFee4',
		ProductCode5 int 'ProductCode5',
		ProductCodeFee5 decimal(4, 2) 'ProductCodeFee5',
		ProductCode6 int 'ProductCode6',
		ProductCodeFee6 decimal(4, 2) 'ProductCodeFee6',
		Filler varchar(405) 'Filler',
		EndMarker varchar(1) 'EndMarker',
		ProductCode varchar(50) 'ProductCode',
		OldAddress1 varchar(100) 'OldAddress1',
		OldAddress2 varchar(100) 'OldAddress2',
		OldAddress3 varchar(100) 'OldAddress3',
		NewAddress1 varchar(100) 'NewAddress1',
		NewAddress2 varchar(100) 'NewAddress2',
		NewAddress3 varchar(100) 'NewAddress3',
		SequenceID int 'SequenceID',
		TransactionCodeValue int 'TransactionCodeValue',
		CategoryCodeValue int 'CategoryCodeValue',
		IsIgnored bit 'IsIgnored',
		AcsActionId int 'AcsActionId',
		ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle

	update AcsFileDetail
	set ClientId = i.ClientId,
		RecordType = i.RecordType,
		FileVersion = i.FileVersion,
		SequenceNumber = i.SequenceNumber,
		AcsMailerId = i.AcsMailerId,
		KeylineSequenceSerialNumber = i.KeylineSequenceSerialNumber,
		MoveEffectiveDate = i.MoveEffectiveDate,
		MoveType = i.MoveType,
		DeliverabilityCode = i.DeliverabilityCode,
		UspsSiteID = i.UspsSiteID,
		LastName = i.LastName,
		FirstName = i.FirstName,
		Prefix = i.Prefix,
		Suffix = i.Suffix,
		OldAddressType = i.OldAddressType,
		OldUrb = i.OldUrb,
		OldPrimaryNumber = i.OldPrimaryNumber,
		OldPreDirectional = i.OldPreDirectional,
		OldStreetName = i.OldStreetName,
		OldSuffix = i.OldSuffix,
		OldPostDirectional = i.OldPostDirectional,
		OldUnitDesignator = i.OldUnitDesignator,
		OldSecondaryNumber = i.OldSecondaryNumber,
		OldCity = i.OldCity,
		OldStateAbbreviation = i.OldStateAbbreviation,
		OldZipCode = i.OldZipCode,
		NewAddressType = i.NewAddressType,
		NewPmb = i.NewPmb,
		NewUrb = i.NewUrb,
		NewPrimaryNumber = i.NewPrimaryNumber,
		NewPreDirectional = i.NewPreDirectional,
		NewStreetName = i.NewStreetName,
		NewSuffix = i.NewSuffix,
		NewPostDirectional = i.NewPostDirectional,
		NewUnitDesignator = i.NewUnitDesignator,
		NewSecondaryNumber = i.NewSecondaryNumber,
		NewCity = i.NewCity,
		NewStateAbbreviation = i.NewStateAbbreviation,
		NewZipCode = i.NewZipCode,
		Hyphen = i.Hyphen,
		NewPlus4Code = i.NewPlus4Code,
		NewDeliveryPoint = i.NewDeliveryPoint,
		NewAbbreviatedCityName = i.NewAbbreviatedCityName,
		NewAddressLabel = i.NewAddressLabel,
		FeeNotification = i.FeeNotification,
		NotificationType = i.NotificationType,
		IntelligentMailBarcode = i.IntelligentMailBarcode,
		IntelligentMailPackageBarcode = i.IntelligentMailPackageBarcode,
		IdTag = i.IdTag,
		HardcopyToElectronicFlag = i.HardcopyToElectronicFlag,
		TypeOfAcs = i.TypeOfAcs,	
		FulfillmentDate = i.FulfillmentDate,
		ProcessingType = i.ProcessingType,
		CaptureType = i.CaptureType,
		MadeAvailableDate = i.MadeAvailableDate,
		ShapeOfMail = i.ShapeOfMail,
		MailActionCode = i.MailActionCode,
		NixieFlag = i.NixieFlag,
		ProductCode1 = i.ProductCode1,
		ProductCodeFee1 = i.ProductCodeFee1,
		ProductCode2 = i.ProductCode2,
		ProductCodeFee2 = i.ProductCodeFee2,
		ProductCode3 = i.ProductCode3,
		ProductCodeFee3 = i.ProductCodeFee3,
		ProductCode4 = i.ProductCode4,
		ProductCodeFee4 = i.ProductCodeFee4,
		ProductCode5 = i.ProductCode5,
		ProductCodeFee5 = i.ProductCodeFee5,
		ProductCode6 = i.ProductCode6,
		ProductCodeFee6 = i.ProductCodeFee6,
		Filler = i.Filler,
		EndMarker = i.EndMarker,
		ProductCode = i.ProductCode,
		OldAddress1 = i.OldAddress1,
		OldAddress2 = i.OldAddress2,
		OldAddress3 = i.OldAddress3,
		NewAddress1 = i.NewAddress1,
		NewAddress2 = i.NewAddress2,
		NewAddress3 = i.NewAddress3,
		SequenceID = i.SequenceID,
		TransactionCodeValue = i.TransactionCodeValue,
		CategoryCodeValue = i.CategoryCodeValue,
		IsIgnored = i.IsIgnored,
		AcsActionId = i.AcsActionId,
		ProcessCode = i.ProcessCode
	from @import i
	where i.AcsFileDetailId = AcsFileDetail.AcsFileDetailId
GO
PRINT N'Creating [dbo].[e_AcsFileHeader_Save]...';


GO
create procedure e_AcsFileHeader_Save
@AcsFileHeaderId int,
@ClientId int,
@RecordType varchar(1),
@FileVersion varchar(2),
@CustomerID int,
@CreateDate date,
@ShipmentNumber bigint,
@TotalAcsRecordCount int,
@TotalCoaCount int,
@TotalNixieCount int,
@TrdRecordCount int,
@TrdAcsFeeAmount decimal(9,2),
@TrdCoaCount int,
@TrdCoaAcsFeeAmount decimal(9,2),
@TrdNixieCount int,
@TrdNixieAcsFeeAmount decimal(9,2),
@OcdRecordCount int,
@OcdAcsFeeAmount decimal(9,2),
@OcdCoaCount int,
@OcdCoaAcsFeeAmount decimal(9,2),
@OcdNixieCount int,
@OcdNixieAcsFeeAmount decimal(9,2),
@FsRecordCount int,
@FsAcsFeeAmount decimal(9,2),
@FsCoaCount int,
@FsCoaAcsFeeAmount decimal(9,2),
@FsNixieCount int,
@FsNixieAcsFeeAmount decimal(9,2),
@ImpbRecordCount int,
@ImpbAcsFeeAmount decimal(9,2),
@ImpbCoaCount int,
@ImpbCoaAcsFeeAmount decimal(9,2),
@ImpbNixieCount int,
@ImpbNixieAcsFeeAmount decimal(9,2),
@Filler varchar(405),
@EndMarker varchar(1),
@ProcessCode varchar(50)
as
	IF @AcsFileHeaderId > 0
		BEGIN
			UPDATE AcsFileHeader
			SET ClientId = @ClientId,
				RecordType = @RecordType,
				FileVersion = @FileVersion,
				CustomerID = @CustomerID,
				CreateDate = @CreateDate,
				ShipmentNumber = @ShipmentNumber,
				TotalAcsRecordCount = @TotalAcsRecordCount,
				TotalCoaCount = @TotalCoaCount,
				TotalNixieCount = @TotalNixieCount,
				TrdRecordCount = @TrdRecordCount,
				TrdAcsFeeAmount = @TrdAcsFeeAmount,
				TrdCoaCount = @TrdCoaCount,
				TrdCoaAcsFeeAmount = @TrdCoaAcsFeeAmount,
				TrdNixieCount = @TrdNixieCount,
				TrdNixieAcsFeeAmount = @TrdNixieAcsFeeAmount,
				OcdRecordCount = @OcdRecordCount,
				OcdAcsFeeAmount = @OcdAcsFeeAmount,
				OcdCoaCount = @OcdCoaCount,
				OcdCoaAcsFeeAmount = @OcdCoaAcsFeeAmount,
				OcdNixieCount = @OcdNixieCount,
				OcdNixieAcsFeeAmount = @OcdNixieAcsFeeAmount,
				FsRecordCount = @FsRecordCount,
				FsAcsFeeAmount = @FsAcsFeeAmount,
				FsCoaCount = @FsCoaCount,
				FsCoaAcsFeeAmount = @FsCoaAcsFeeAmount,
				FsNixieCount = @FsNixieCount,
				FsNixieAcsFeeAmount = @FsNixieAcsFeeAmount,
				ImpbRecordCount = @ImpbRecordCount,
				ImpbAcsFeeAmount = @ImpbAcsFeeAmount,
				ImpbCoaCount = @ImpbCoaCount,
				ImpbCoaAcsFeeAmount = @ImpbCoaAcsFeeAmount,
				ImpbNixieCount = @ImpbNixieCount,
				ImpbNixieAcsFeeAmount = @ImpbNixieAcsFeeAmount,
				Filler = @Filler,
				EndMarker = @EndMarker,
				ProcessCode = @ProcessCode
			WHERE AcsFileHeaderId = @AcsFileHeaderId;

			SELECT @AcsFileHeaderId;
		END
	ELSE
		BEGIN
			INSERT INTO AcsFileHeader (ClientId,RecordType,FileVersion,CustomerID,CreateDate,ShipmentNumber,TotalAcsRecordCount,TotalCoaCount,TotalNixieCount,TrdRecordCount,
									   TrdAcsFeeAmount,TrdCoaCount,TrdCoaAcsFeeAmount,TrdNixieCount,TrdNixieAcsFeeAmount,OcdRecordCount,OcdAcsFeeAmount,OcdCoaCount,
									   OcdCoaAcsFeeAmount,OcdNixieCount,OcdNixieAcsFeeAmount,FsRecordCount,FsAcsFeeAmount,FsCoaCount,FsCoaAcsFeeAmount,FsNixieCount,
									   FsNixieAcsFeeAmount,ImpbRecordCount,ImpbAcsFeeAmount,ImpbCoaCount,ImpbCoaAcsFeeAmount,ImpbNixieCount,ImpbNixieAcsFeeAmount,
									   Filler,EndMarker,ProcessCode)
			VALUES(@ClientId,@RecordType,@FileVersion,@CustomerID,@CreateDate,@ShipmentNumber,@TotalAcsRecordCount,@TotalCoaCount,@TotalNixieCount,@TrdRecordCount,
				   @TrdAcsFeeAmount,@TrdCoaCount,@TrdCoaAcsFeeAmount,@TrdNixieCount,@TrdNixieAcsFeeAmount,@OcdRecordCount,@OcdAcsFeeAmount,@OcdCoaCount,
				   @OcdCoaAcsFeeAmount,@OcdNixieCount,@OcdNixieAcsFeeAmount,@FsRecordCount,@FsAcsFeeAmount,@FsCoaCount,@FsCoaAcsFeeAmount,@FsNixieCount,
				   @FsNixieAcsFeeAmount,@ImpbRecordCount,@ImpbAcsFeeAmount,@ImpbCoaCount,@ImpbCoaAcsFeeAmount,@ImpbNixieCount,@ImpbNixieAcsFeeAmount,
				   @Filler,@EndMarker,@ProcessCode);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_AcsMailerInfo_Save]...';


GO
CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Save]
@AcsMailerInfoId int,
@AcsMailerId varchar(9),
@ImbSeqCounter int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @AcsMailerInfoId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE AcsMailerInfo
		SET AcsMailerId = @AcsMailerId,
			ImbSeqCounter = @ImbSeqCounter,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE AcsMailerInfoId = @AcsMailerInfoId;

		SELECT @AcsMailerInfoId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into AcsMailerInfo(AcsMailerId,ImbSeqCounter,DateCreated,CreatedByUserID)
		VALUES(@AcsMailerId,@ImbSeqCounter,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_AcsMailerInfo_Select]...';


GO
CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Select]
AS
	SELECT * FROM AcsMailerInfo With(NoLock)
GO
PRINT N'Creating [dbo].[e_AcsMailerInfo_Select_ID]...';


GO
CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Select_ID]
@AcsMailerInfoId int
AS
	SELECT *
	FROM AcsMailerInfo With(NoLock)
	WHERE AcsMailerInfoId = @AcsMailerInfoId
GO
PRINT N'Creating [dbo].[e_AcsShippingDetail_Save]...';


GO
CREATE PROCEDURE e_AcsShippingDetail_Save
@AcsShippingDetailId int,
@ClientId int,
@CustomerNumber int,
@AcsDate date,
@ShipmentNumber bigint,
@AcsTypeId int,
@AcsId int,
@AcsName varchar(250),
@ProductCode varchar(100),
@Description varchar(250),
@Quantity int,
@UnitCost decimal(8,2),
@TotalCost decimal(12,2),
@DateCreated datetime,
@IsBilled bit,
@BilledDate datetime,
@BilledByUserID int,
@ProcessCode varchar(50)
AS

IF @AcsShippingDetailId > 0
	BEGIN		
		UPDATE AcsShippingDetail
		SET ClientId = @ClientId,
			CustomerNumber = @CustomerNumber,
			AcsDate = @AcsDate,
			ShipmentNumber = @ShipmentNumber,
			AcsTypeId = @AcsTypeId,
			AcsId = @AcsId,
			AcsName = @AcsName,
			ProductCode = @ProductCode,
			Description = @Description,
			Quantity = @Quantity,
			UnitCost = @UnitCost,
			TotalCost = @TotalCost,
			IsBilled = @IsBilled,
			BilledDate = @BilledDate,
			BilledByUserID = @BilledByUserID,
			ProcessCode = @ProcessCode
		WHERE AcsShippingDetailId = @AcsShippingDetailId;
		
		SELECT @AcsShippingDetailId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO AcsShippingDetail (ClientId,CustomerNumber,AcsDate,ShipmentNumber,AcsTypeId,AcsId,AcsName,ProductCode,Description,Quantity,UnitCost,TotalCost,DateCreated,IsBilled,BilledDate,BilledByUserID,ProcessCode)
		VALUES(@ClientId,@CustomerNumber,@AcsDate,@ShipmentNumber,@AcsTypeId,@AcsId,@AcsName,@ProductCode,@Description,@Quantity,@UnitCost,@TotalCost,@DateCreated,@IsBilled,@BilledDate,@BilledByUserID,@ProcessCode);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_Frequency_Select]...';


GO
CREATE PROCEDURE [dbo].[e_Frequency_Select]
AS
	SELECT * FROM Frequency With(NoLock)
GO
PRINT N'Creating [dbo].[e_HistoryMarketingMap_BulkSave]...';


GO

CREATE PROCEDURE [dbo].[e_HistoryMarketingMap_BulkSave]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		MarketingID int,
		SubscriberID int,
		PublicationID int,
		IsActive bit,
		DateCreated datetime,
		CreatedByUserID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID
	)  
	
	SELECT MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID
	FROM OPENXML(@docHandle,N'/XML/HistoryMarketingMap')
	WITH
	(
		MarketingID int 'MarketingID',
		SubscriberID int 'SubscriberID',
		PublicationID int 'PublicationID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		CreatedByUserID int 'CreatedByUserID'
	)
	
	EXEC sp_xml_removedocument @docHandle
	
	DECLARE @userLogId TABLE (HistoryMarketingMapID int)
	
	INSERT INTO HistoryMarketingMap(MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID)
	OUTPUT inserted.HistoryMarketingMapID INTO @userLogId
	SELECT MarketingID,SubscriberID,PublicationID,IsActive,(CASE WHEN ISNULL(DateCreated,'')='' THEN GETDATE() ELSE DateCreated END) AS DateCreated,CreatedByUserID
	FROM @import

	SELECT hmm.HistoryMarketingMapID,MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID
	FROM HistoryMarketingMap hmm INNER JOIN @userLogId u on hmm.HistoryMarketingMapID = u.HistoryMarketingMapID
GO
PRINT N'Creating [dbo].[e_HistorySubscription_BulkUpdate_IsUadUpdated]...';


GO

CREATE PROCEDURE [dbo].[e_HistorySubscription_BulkUpdate_IsUadUpdated]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		HistorySubscriptionID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 HistorySubscriptionID
	)  
	
	SELECT HistorySubscriptionID
	
	FROM OPENXML(@docHandle,N'/XML/HistorySubscriptionID')
	WITH
	(
		HistorySubscriptionID  int 'HistorySubscriptionID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	Update HistorySubscription
	Set IsUadUpdated = 1, UadUpdatedDate = GETDATE()
	Where HistorySubscriptionID in (Select HistorySubscriptionID from #import)

	DROP TABLE #import
GO
PRINT N'Creating [dbo].[e_HistorySubscription_Select_IsUadUpdated]...';


GO
CREATE PROCEDURE [dbo].[e_HistorySubscription_Select_IsUadUpdated]
	@IsUadUpdated bit
AS
	SET NOCOUNT ON
	
	Select * from HistorySubscription where IsUadUpdated = @IsUadUpdated
GO
PRINT N'Creating [dbo].[e_HistoryToHistoryMarketingMap_BulkSave]...';


GO
CREATE PROCEDURE [dbo].[e_HistoryToHistoryMarketingMap_BulkSave]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		HistoryID int,
		HistoryMarketingMapID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 HistoryID,HistoryMarketingMapID
	)  
	
	SELECT HistoryID,HistoryMarketingMapID
	FROM OPENXML(@docHandle,N'/XML/History')
	WITH
	(
		HistoryID int 'HistoryID' ,
		HistoryMarketingMapID int 'HistoryMarketingMapID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	INSERT INTO HistoryToHistoryMarketingMap (HistoryID,HistoryMarketingMapID)
	SELECT HistoryID,HistoryMarketingMapID FROM @import
GO
PRINT N'Creating [dbo].[e_Issue_Save]...';


GO
CREATE PROCEDURE e_Issue_Save
@IssueId int,
@PublicationId int,
@IssueName varchar(100),
@IssueCode varchar(50),
@DateOpened datetime,
@OpenedByUserID int,
@IsClosed bit,
@DateClosed datetime,
@ClosedByUserID int,
@IsComplete bit,
@DateComplete datetime,
@CompleteByUserID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE Issue
		SET PublicationId = @PublicationId,
			IssueName = @IssueName,
			IssueCode = @IssueCode,
			DateOpened = @DateOpened,
			OpenedByUserID = @OpenedByUserID,
			IsClosed = @IsClosed,
			DateClosed = @DateClosed,
			ClosedByUserID = @ClosedByUserID,
			IsComplete = @IsComplete,
			DateComplete = @DateComplete,
			CompleteByUserID = @CompleteByUserID,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueId = @IssueId;

		SELECT @IssueId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into Issue (PublicationId,IssueName,IssueCode,DateOpened,OpenedByUserID,IsClosed,DateClosed,ClosedByUserID,IsComplete,DateComplete,CompleteByUserID,DateCreated,CreatedByUserID)
		VALUES(@PublicationId,@IssueName,@IssueCode,@DateOpened,@OpenedByUserID,@IsClosed,@DateClosed,@ClosedByUserID,@IsComplete,@DateComplete,@CompleteByUserID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_Issue_Select]...';


GO
CREATE PROCEDURE [dbo].[e_Issue_Select]
AS
	SELECT * FROM Issue With(NoLock)
GO
PRINT N'Creating [dbo].[e_Issue_Select_PublicationID]...';


GO
CREATE PROCEDURE [dbo].[e_Issue_Select_PublicationID]
@PublicationID int
AS
	SELECT * FROM Issue With(NoLock)
	WHERE PublicationId = @PublicationID
	AND Issue.IsComplete = 0
GO
PRINT N'Creating [dbo].[e_Issue_Select_PublisherID]...';


GO
CREATE PROCEDURE [dbo].[e_Issue_Select_PublisherID]
@PublisherID int
AS
	SELECT * FROM Issue With(NoLock)
	JOIN ClientProductMap map ON map.ProductID = Issue.PublicationId
	JOIN UAS..Client c ON map.ClientID = c.ClientID
	WHERE c.ClientID = @PublisherID
	AND Issue.IsComplete = 0
GO
PRINT N'Creating [dbo].[e_IssueArchiveSubscriber_Save]...';


GO
CREATE PROCEDURE e_IssueArchiveSubscriber_Save
@IssueArchiveSubscriberId int,
@IssueId int,
@IsComp bit,
@CompId int ,
@SubscriberID int,
@ExternalKeyID int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar (255),
@Occupation varchar (50),
@AddressTypeID  int,
@Address1  varchar (100),
@Address2  varchar (100),
@Address3  varchar (100),
@City varchar (50),
@RegionCode varchar (50),
@RegionID  int,
@ZipCode   varchar (50),
@Plus4 varchar (10),
@CarrierRoute varchar (10),
@County varchar (50),
@Country varchar (50),
@CountryID int,
@Latitude decimal(18, 15),
@Longitude decimal(18, 15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource varchar(50),
@AddressValidationMessage varchar(max),
@Email varchar(255),
@Phone varchar(25),
@Fax varchar(25),
@Mobile varchar(25),
@Website varchar(255),
@Birthdate date,
@Age int,
@Income varchar(50),
@Gender varchar(50),
@IsLocked bit,
@PhoneExt varchar(25), 
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueArchiveSubscriber
		SET IssueId = @IssueId,
			IsComp = @IsComp,
			CompId = @CompId,
			SubscriberID = @SubscriberID,
			ExternalKeyID = @ExternalKeyID,
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
			Occupation = @Occupation,
			AddressTypeID = @AddressTypeID,
			Address1 = @Address1,
			Address2 = @Address2,
			Address3 = @Address3,
			City = @City,
			RegionCode = @RegionCode,
			RegionID = @RegionID,
			ZipCode = @ZipCode,
			Plus4 = @Plus4,
			CarrierRoute = @CarrierRoute,
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Latitude = @Latitude,
			Longitude = @Longitude,
			IsAddressValidated = @IsAddressValidated,
			AddressValidationDate = @AddressValidationDate,
			AddressValidationSource = @AddressValidationSource,
			AddressValidationMessage = @AddressValidationMessage,
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			Birthdate = @Birthdate,
			Age = @Age,
			Income = @Income,
			Gender = @Gender,
			IsLocked = @IsLocked,
			PhoneExt = @PhoneExt, 
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueArchiveSubscriberId = @IssueArchiveSubscriberId;

		SELECT @IssueArchiveSubscriberId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueArchiveSubscriber (IssueArchiveSubscriberId,IssueId,IsComp,CompId,SubscriberID,ExternalKeyID,FirstName,LastName,Company,Title,
											Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,
											CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,
											AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,DateCreated,CreatedByUserID)
		VALUES(@IssueArchiveSubscriberId,@IssueId,@IsComp,@CompId,@SubscriberID,@ExternalKeyID,@FirstName,@LastName,@Company,@Title,
			   @Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,@Country,
			   @CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,
			   @Email,@Phone,@Fax,@Mobile,@Website,@Birthdate,@Age,@Income,@Gender,@IsLocked,@PhoneExt,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_IssueArchiveSubscriber_Select_IssueID]...';


GO
CREATE PROCEDURE [dbo].[e_IssueArchiveSubscriber_Select_IssueID]
@IssueID int
AS
	SELECT s.* FROM IssueArchiveSubscriber s With(NoLock)	
	WHERE s.IssueId = @IssueID
GO
PRINT N'Creating [dbo].[e_IssueArchiveSubscription_Save]...';


GO
CREATE PROCEDURE e_IssueArchiveSubscription_Save
@IssueArchiveSubscriptionId int,
@IssueArchiveSubscriberId int,
@IsComp bit,
@CompId int,
@SubscriptionID int,
@SequenceID int,
@PublisherID int,
@SubscriberID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate date,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256),
@Copies int,
@OriginalSubscriberSourceCode varchar(256),
@Par3cID int,
@SubsrcTypeID int,
@AccountNumber varchar(50),
@GraceIssues	int,
@OnBehalfOf varchar(256), 
@MemberGroup varchar(256), 
@Verify varchar(256), 
@IsNewSubscription bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueArchiveSubscriptionId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueArchiveSubscription
		SET IssueArchiveSubscriberId = @IssueArchiveSubscriberId,
			IsComp = @IsComp,
			CompId = @CompId,
			SubscriptionID = @SubscriptionID,
			SequenceID = @SequenceID,
			PublisherID = @PublisherID,
			SubscriberID = @SubscriberID,
			PublicationID = @PublicationID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			SubscriptionStatusID = @SubscriptionStatusID,
			IsPaid = @IsPaid,
			QSourceID = @QSourceID,
			QSourceDate = @QSourceDate,
			DeliverabilityID = @DeliverabilityID,
			IsSubscribed = @IsSubscribed,
			SubscriberSourceCode = @SubscriberSourceCode,
			Copies = @Copies,
			OriginalSubscriberSourceCode = @OriginalSubscriberSourceCode,
			Par3cID = @Par3cID,
			SubsrcTypeID = @SubsrcTypeID,
			AccountNumber = @AccountNumber,
			GraceIssues = @GraceIssues,
			OnBehalfOf = @OnBehalfOf, 
			MemberGroup = @MemberGroup, 
			Verify = @Verify, 
			IsNewSubscription = @IsNewSubscription,
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueArchiveSubscriptionId = @IssueArchiveSubscriptionId;

		SELECT @IssueArchiveSubscriptionId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueArchiveSubscription (IssueArchiveSubscriberId,IsComp,CompId,SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,
											  ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,
											  IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,
											  GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,DateCreated,CreatedByUserID)
		VALUES(@IssueArchiveSubscriberId,@IsComp,@CompId,@SubscriptionID,@SequenceID,@PublisherID,@SubscriberID,@PublicationID,
			   @ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,
			   @IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@Par3cID,@SubsrcTypeID,@AccountNumber,
			   @GraceIssues,@OnBehalfOf,@MemberGroup,@Verify,@IsNewSubscription,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_IssueArchiveSubscription_Select_IssueID]...';


GO
CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_Select_IssueID]
@IssueID int
AS
	SELECT s.* FROM IssueArchiveSubscription s With(NoLock)	
	JOIN IssueArchiveSubscriber ias ON ias.IssueArchiveSubscriberId = s.IssueArchiveSubscriberId
	WHERE ias.IssueId = @IssueID
GO
PRINT N'Creating [dbo].[e_IssueArchiveSubscriptonResponseMap_Save]...';


GO
CREATE PROCEDURE e_IssueArchiveSubscriptonResponseMap_Save
@IssueArchiveSubscriptionId int,
@SubscriptionID  int,
@ResponseID int,
@IsActive bit,
@ResponseOther varchar(300),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueArchiveSubscriptionId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueArchiveSubscriptonResponseMap
		SET SubscriptionID = @SubscriptionID,
			ResponseID = @ResponseID,
			IsActive = @IsActive,
			ResponseOther = @ResponseOther,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueArchiveSubscriptionId = @IssueArchiveSubscriptionId;

		SELECT @IssueArchiveSubscriptionId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueArchiveSubscriptonResponseMap (SubscriptionID,ResponseID,IsActive,ResponseOther,DateCreated,CreatedByUserID)
		VALUES(@SubscriptionID,@ResponseID,@IsActive,@ResponseOther,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_IssueComp_Save]...';


GO
CREATE PROCEDURE e_IssueComp_Save
@IssueCompId int,
@IssueId int,
@ImportedDate datetime,
@IssueCompCount int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueCompId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueComp
		SET IssueId = @IssueId,
			ImportedDate = @ImportedDate,
			IssueCompCount = @IssueCompCount,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueCompId = @IssueCompId;

		SELECT @IssueCompId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueComp (IssueCompId,IssueId,ImportedDate,IssueCompCount,DateCreated,CreatedByUserID)
		VALUES(@IssueCompId,@IssueId,@ImportedDate,@IssueCompCount,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_IssueCompDetail_Save]...';


GO
CREATE PROCEDURE e_IssueCompDetail_Save
@IssueCompDetailId int,
@IssueCompID int,
@ExternalKeyID int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar (255),
@Occupation varchar (50),
@AddressTypeID  int,
@Address1  varchar (100),
@Address2  varchar (100),
@Address3  varchar (100),
@City varchar (50),
@RegionCode varchar (50),
@RegionID  int,
@ZipCode   varchar (50),
@Plus4 varchar (10),
@CarrierRoute varchar (10),
@County varchar (50),
@Country varchar (50),
@CountryID int,
@Latitude decimal(18, 15),
@Longitude decimal(18, 15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource varchar(50),
@AddressValidationMessage varchar(max),
@Email varchar(255),
@Phone varchar(25),
@Fax varchar(25),
@Mobile varchar(25),
@Website varchar(255),
@Birthdate date,
@Age int,
@Income varchar(50),
@Gender varchar(50),
@IsLocked bit,
@PhoneExt varchar(25), 
@SequenceID int,
@PublisherID int,
@SubscriberID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate date,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256),
@Copies int,
@OriginalSubscriberSourceCode varchar(256),
@Par3cID int,
@SubsrcTypeID int,
@AccountNumber varchar(50),
@OnBehalfOf varchar(256), 
@MemberGroup varchar(256), 
@Verify varchar(256), 
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueCompDetailId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueCompDetail
		SET IssueCompID = @IssueCompID,
			ExternalKeyID = @ExternalKeyID,
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
			Occupation = @Occupation,
			AddressTypeID = @AddressTypeID,
			Address1 = @Address1,
			Address2 = @Address2,
			Address3 = @Address3,
			City = @City,
			RegionCode = @RegionCode,
			RegionID = @RegionID,
			ZipCode = @ZipCode,
			Plus4 = @Plus4,
			CarrierRoute = @CarrierRoute,
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Latitude = @Latitude,
			Longitude = @Longitude,
			IsAddressValidated = @IsAddressValidated,
			AddressValidationDate = @AddressValidationDate,
			AddressValidationSource = @AddressValidationSource,
			AddressValidationMessage = @AddressValidationMessage,
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			Birthdate = @Birthdate,
			Age = @Age,
			Income = @Income,
			Gender = @Gender,
			IsLocked = @IsLocked,
			PhoneExt = @PhoneExt, 
			SequenceID = @SequenceID,
			PublisherID = @PublisherID,
			SubscriberID = @SubscriberID,
			PublicationID = @PublicationID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			SubscriptionStatusID = @SubscriptionStatusID,
			IsPaid = @IsPaid,
			QSourceID = @QSourceID,
			QSourceDate = @QSourceDate,
			DeliverabilityID = @DeliverabilityID,
			IsSubscribed = @IsSubscribed,
			SubscriberSourceCode = @SubscriberSourceCode,
			Copies = @Copies,
			OriginalSubscriberSourceCode = @OriginalSubscriberSourceCode,
			Par3cID = @Par3cID,
			SubsrcTypeID = @SubsrcTypeID,
			AccountNumber = @AccountNumber,
			OnBehalfOf = @OnBehalfOf, 
			MemberGroup = @MemberGroup, 
			Verify = @Verify, 
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueCompDetailId = @IssueCompDetailId;

		SELECT @IssueCompDetailId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueCompDetail (IssueCompID,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,
									 County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
									 Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,SequenceID,PublisherID,SubscriberID,PublicationID,
									 ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,
									 Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify,DateCreated,CreatedByUserID)
		VALUES(@IssueCompID,@ExternalKeyID,@FirstName,@LastName,@Company,@Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,
			   @County,@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,
			   @Email,@Phone,@Fax,@Mobile,@Website,@Birthdate,@Age,@Income,@Gender,@IsLocked,@PhoneExt,@SequenceID,@PublisherID,@SubscriberID,@PublicationID,
			   @ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,
			   @IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@Par3cID,@SubsrcTypeID,@AccountNumber,
			   @OnBehalfOf,@MemberGroup,@Verify,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_IssueCompError_Select]...';


GO
CREATE PROCEDURE [dbo].[e_IssueCompError_Select]
AS
	SELECT * FROM IssueCompError With(NoLock)
GO
PRINT N'Creating [dbo].[e_IssueCompError_Select_ProcessCode]...';


GO
CREATE PROCEDURE [dbo].[e_IssueCompError_Select_ProcessCode]
	@ProcessCode VARCHAR(50)
AS
	SELECT * FROM IssueCompError With(NoLock) WHERE ProcessCode = @ProcessCode
GO
PRINT N'Creating [dbo].[e_IssueSplit_Save]...';


GO
CREATE PROCEDURE e_IssueSplit_Save
@IssueSplitId int,
@IssueId int,
@IssueSplitCode varchar(250),
@IssueSplitName varchar(250),
@IssueSplitCount int,
@FilterId int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@IsActive bit,
@KeyCode varchar(250)
AS
IF @IssueSplitId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueSplit
		SET IssueId = @IssueId,
			IssueSplitCode = @IssueSplitCode,
			IssueSplitName = @IssueSplitName,
			IssueSplitCount = @IssueSplitCount,
			FilterId = @FilterId,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			IsActive = @IsActive,
			KeyCode = @KeyCode
		WHERE IssueSplitId = @IssueSplitId;

		SELECT @IssueSplitId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT intO IssueSplit (IssueId,IssueSplitCode,IssueSplitName,IssueSplitCount,FilterId,DateCreated,CreatedByUserID, IsActive, KeyCode)
		VALUES(@IssueId,@IssueSplitCode,@IssueSplitName,@IssueSplitCount,@FilterId,@DateCreated,@CreatedByUserID, @IsActive, @KeyCode);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_IssueSplit_Select_IssueID]...';


GO
CREATE PROCEDURE [dbo].[e_IssueSplit_Select_IssueID]
	@IssueID int
AS
SELECT * FROM IssueSplit i With(NoLock)
WHERE i.IssueId = @IssueID
GO
PRINT N'Creating [dbo].[e_MarketingMap_BulkUpdate]...';


GO
CREATE PROCEDURE [dbo].[e_MarketingMap_BulkUpdate]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		MarketingID int,
		SubscriberID int,
		PublicationID int,
		IsActive bit,
		DateCreated datetime,
		DateUpdated datetime,
		CreatedByUserID int,
		UpdatedByUserID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 MarketingID,SubscriberID, PublicationID, IsActive, DateCreated, DateUpdated,CreatedByUserID, UpdatedByUserID
	)  
	
	SELECT MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID
	FROM OPENXML(@docHandle,N'/XML/MarketingMap')
	WITH
	(
		MarketingID  int 'MarketingID',
		SubscriberID int 'SubscriberID',
		PublicationID int 'PublicationID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	-- If the record joins, do updates
	UPDATE MarketingMap
	SET 
		IsActive = i.IsActive,
		DateUpdated = CASE WHEN ISNULL(i.DateUpdated,'')='' THEN GETDATE() ELSE i.DateUpdated END,
		UpdatedByUserID = i.UpdatedByUserID
	FROM #import i
	WHERE MarketingMap.SubscriberID = i.SubscriberID and MarketingMap.SubscriberID = i.MarketingID
	
	INSERT INTO MarketingMap (MarketingID,SubscriberID, PublicationID, IsActive, DateCreated,CreatedByUserID)
	SELECT DISTINCT i.MarketingID,i.SubscriberID,i.PublicationID,i.IsActive,i.DateCreated,i.CreatedByUserID
	FROM #import i INNER JOIN MarketingMap srm ON srm.SubscriberID = i.SubscriberID
	WHERE i.MarketingID NOT IN (Select mm.MarketingID FROM MarketingMap mm INNER JOIN #import i on mm.SubscriberID = i.SubscriberID)

	INSERT INTO MarketingMap (MarketingID,SubscriberID, PublicationID, IsActive, DateCreated, CreatedByUserID)
	SELECT DISTINCT i.MarketingID,i.SubscriberID,i.PublicationID,i.IsActive,i.DateCreated,i.CreatedByUserID
	FROM #import i
	WHERE i.SubscriberID NOT IN (SELECT SubscriberID FROM MarketingMap GROUP BY SubscriberID)
	
	-- Removes records that were marked inactive
	DELETE FROM MarketingMap WHERE IsActive = 0 and SubscriberID IN (SELECT SubscriberID FROM #import GROUP BY SubscriberID)

	DROP TABLE #import
GO
PRINT N'Creating [dbo].[e_PublicationReport_Select_PublicationID]...';


GO
CREATE PROCEDURE [dbo].[e_PublicationReport_Select_PublicationID]
@PublicationID int
AS
	SELECT r.* FROM Publicationreports r 
	JOIN Publication p ON r.PublicationID = p.PublicationID 
	WHERE isnull(r.status,1) = 1 and p.PublicationID = @PublicationID 
	ORDER BY ReportName
GO
PRINT N'Creating [dbo].[e_ReportGroups_Save]...';


GO
CREATE PROCEDURE [dbo].[e_ReportGroups_Save]
	@ReportGroupID int,
	@ResponseTypeID int,
	@DisplayName varchar(50),
	@DisplayOrder int
AS
	IF @ReportGroupID > 0
		BEGIN
			UPDATE ReportGroups
			SET 				
				ResponseTypeID = @ResponseTypeID,
				DisplayName = @DisplayName,
				DisplayOrder = @DisplayOrder
			WHERE ReportGroupID = @ReportGroupID 
		
			SELECT @ReportGroupID;
		END
	ELSE
		BEGIN
			INSERT INTO ReportGroups (ResponseTypeID,DisplayName,DisplayOrder)
			VALUES(@ResponseTypeID,@DisplayName,@DisplayOrder);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_ReportGroups_Select]...';


GO
CREATE PROCEDURE [dbo].[e_ReportGroups_Select]
AS
	SELECT * FROM ReportGroups With(NoLock)
GO
PRINT N'Creating [dbo].[e_Response_Delete]...';


GO
CREATE PROCEDURE [dbo].[e_Response_Delete]
	@ResponseID int
AS
	DELETE Response WHERE ResponseID = @ResponseID
GO
PRINT N'Creating [dbo].[e_ResponseType_Delete_PublicationID]...';


GO
CREATE PROCEDURE [dbo].[e_ResponseType_Delete_PublicationID]
	@PublicationID int
AS
	DELETE ResponseType WHERE PublicationID = @PublicationID
GO
PRINT N'Creating [dbo].[e_Subscriber_BulkUpdate_WaveMailing]...';


GO
CREATE Proc [dbo].[e_Subscriber_BulkUpdate_WaveMailing]
(  
	@SubscriberXML TEXT,
	@WaveMailingID int
) AS

CREATE TABLE #Subs
(  
	RowID int IDENTITY(1, 1)
  ,[SubscriberID] int
)

DECLARE @docHandle int

EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriberXML  
INSERT INTO #Subs 
(
	 SubscriberID
)  
SELECT [SubscriberID]
FROM OPENXML(@docHandle,N'/XML/Subscriber')
WITH
(
	[SubscriberID] int 'SubID'
)

UPDATE Subscriber
SET IsInActiveWaveMailing = 1, WaveMailingID = @WaveMailingID
FROM Subscriber s
INNER JOIN #Subs s2
ON s.SubscriberID = s2.SubscriberID

DROP TABLE #Subs
GO
PRINT N'Creating [dbo].[e_Subscriber_ClearWaveMailingInfo]...';


GO
CREATE PROCEDURE [dbo].[e_Subscriber_ClearWaveMailingInfo]
@WaveMailingID int
AS
	UPDATE Subscriber
	SET WaveMailingID = 0, IsInActiveWaveMailing = 0
	WHERE WaveMailingID = @WaveMailingID
GO
PRINT N'Creating [dbo].[e_SubscriberAddKill_Save]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriberAddKill_Save]
	@AddKillID int,
	@PublicationID int,
	@FilterID int,
	@Count int,
	@AddKillCount int,
	@Type varchar(10),
	@IsActive bit,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
	IF @AddKillID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE SubscriberAddKill
		SET 
			PublicationID = @PublicationID,
			FilterID = @FilterID,
			[Count] = @Count,
			AddKillCount = @AddKillCount,
			[Type] = @Type,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
			
		WHERE AddKillID = @AddKillID;
		
		SELECT @AddKillID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO SubscriberAddKill (PublicationID, FilterID, [Count], AddKillCount, [Type], IsActive, DateCreated, CreatedByUserID)
		VALUES(@PublicationID, @FilterID, @Count, @AddKillCount, @Type, @IsActive, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_SubscriberAddKill_Select]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriberAddKill_Select]
AS
SELECT *
FROM SubscriberAddKill s With(NoLock)
GO
PRINT N'Creating [dbo].[e_SubscriberAddKill_UpdateSubscription]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriberAddKill_UpdateSubscription]
	@SubscriptionIDs TEXT,
	@ProductID int,
	@AddRemoveID int,
	@DeleteAddRemoveID BIT
AS
	Declare  @docHandle int
	--set @SubscriptionIDs = '<XML><SUBSCRIBERS><ID>5</ID><ID>4</ID></SUBSCRIBERS></XML>'
	DECLARE @Subscriptions  Table (SubscriptionID varchar(20))	
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriptionIDs   
 
	Insert into @Subscriptions
	Select  ID
			FROM OPENXML(@docHandle, N'/XML/SUBSCRIBERS/ID')   
			WITH (ID int '.')  
	
	EXEC sp_xml_removedocument @docHandle  
	
	IF @DeleteAddRemoveID = 0
		UPDATE Subscription
		SET AddRemoveID = @AddRemoveID
		WHERE SubscriptionID IN (Select * FROM @Subscriptions)
		AND PublicationID = @ProductID
		
	ELSE
		UPDATE Subscription
		SET AddRemoveID = 0
		WHERE SubscriptionID IN (Select * FROM @Subscriptions)
		AND PublicationID = @ProductID
		AND AddRemoveID = @AddRemoveID
	
Return 0
GO
PRINT N'Creating [dbo].[e_Subscription_BulkUpdate_ActionIDs]...';


GO
CREATE Proc [dbo].[e_Subscription_BulkUpdate_ActionIDs]
(  
	@SubscriptionXML TEXT
) AS

CREATE TABLE #Subs
(  
	RowID int IDENTITY(1, 1)
  ,[SubscriptionID] int
  ,[CurrentAction] int
  ,[PreviousAction] int
)

DECLARE @docHandle int
--DECLARE @SubscriptionXML varchar(8000) --FOR TESTING PURPOSES

--set @SubscriptionXML = '<XML><Subscription><SubID>8366623</SubID><CurrentAction>72</CurrentAction><PreviousAction>72</PreviousAction></Subscription></XML>'

EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriptionXML  
INSERT INTO #Subs 
(
	 SubscriptionID, CurrentAction, PreviousAction
)  
SELECT [SubscriptionID], [CurrentAction], [PreviousAction]
FROM OPENXML(@docHandle,N'/XML/Subscription')
WITH
(
	[SubscriptionID] int 'SubID',
	[CurrentAction] int 'CurrentAction',
	[PreviousAction] int 'PreviousAction'
)

UPDATE Subscription
SET ActionID_Current = s2.CurrentAction, ActionID_Previous = s2.PreviousAction
FROM Subscription s
INNER JOIN #Subs s2
ON s.SubscriptionID = s2.SubscriptionID

DROP TABLE #Subs
GO
PRINT N'Creating [dbo].[e_SubscriptionResponseMap_Select_ProductID]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select_ProductID]
@ProductID int
AS
	Select s.* FROM SubscriptionResponseMap s With(NoLock)
	JOIN Subscription sp With(NoLock) ON sp.SubscriptionID = s.SubscriptionID
	WHERE sp.PublicationID = @ProductID
GO
PRINT N'Creating [dbo].[e_WaveMailing_Save]...';


GO
CREATE PROCEDURE [e_WaveMailing_Save]
@WaveMailingID int,
@IssueID int,
@WaveMailingName varchar(100),
@WaveNumber int,
@PublicationID int,
@DateSubmittedToPrinter datetime,
@DateCreated datetime,
@DateUpdated datetime,
@SubmittedToPrinterByUserID int,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @WaveMailingID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE WaveMailing
		SET IssueID = @IssueID,
			WaveMailingName = @WaveMailingName,
			WaveNumber = @WaveNumber,
			PublicationID = @PublicationID,
			DateSubmittedToPrinter = @DateSubmittedToPrinter,
			DateUpdated = @DateUpdated,
			SubmittedToPrinterByUserID = @SubmittedToPrinterByUserID,
			UpdatedByUserID = @UpdatedByUserID
		WHERE WaveMailingID = @WaveMailingID;

		SELECT @WaveMailingID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT intO WaveMailing(IssueID,WaveMailingName,WaveNumber,PublicationID,DateCreated,CreatedByUserID)
		VALUES(@IssueID,@WaveMailingName,@WaveNumber,@PublicationID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_WaveMailing_Select]...';


GO
CREATE PROCEDURE [dbo].[e_WaveMailing_Select]
AS
	SELECT * FROM WaveMailing With(NoLock)
GO
PRINT N'Creating [dbo].[e_WaveMailingDetail_Save]...';


GO
CREATE PROCEDURE [dbo].[e_WaveMailingDetail_Save]
@WaveMailingDetailID int,
@WaveMailingID int,
@SubscriberID int,
@SubscriptionID int,
@DeliverabilityID int,
@ActionID_Current int,
@ActionID_Previous int,
@Copies int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar(50),
@AddressTypeID int,
@Address1 varchar(100),
@Address2 varchar(100),
@Address3 varchar(100),
@City varchar(50),
@RegionCode varchar(50),
@RegionID int,
@ZipCode varchar(10),
@Plus4 varchar(10),
@County varchar(50),
@Country varchar(50),
@CountryID int,
@Email varchar(100),
@Phone varchar(50),
@Fax varchar(50),
@Mobile varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @WaveMailingDetailID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE WaveMailingDetail
		SET 
			WaveMailingID = @WaveMailingID,
			DeliverabilityID = @DeliverabilityID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			Copies = @Copies,
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
			AddressTypeID = @AddressTypeID,
			Address1 = @Address1,
			Address2 = @Address2,
			Address3 = @Address3,
			City = @City,
			RegionCode = @RegionCode,
			RegionID = @RegionID,
			ZipCode = @ZipCode,
			Plus4 = @Plus4,
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE WaveMailingDetailID = @WaveMailingDetailID;
		
		SELECT @WaveMailingDetailID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO WaveMailingDetail(WaveMailingID, SubscriberID, SubscriptionID,DeliverabilityID,ActionID_Current,ActionID_Previous,Copies,FirstName,LastName,Company,Title,
					AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,County,Country,CountryID,Email,Phone,Fax,Mobile,DateCreated,CreatedByUserID)
		VALUES(@WaveMailingID,@SubscriberID,@SubscriptionID,@DeliverabilityID,@ActionID_Current,@ActionID_Previous,@Copies,@FirstName,@LastName,@Company,@Title,@AddressTypeID,
				@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@County,@Country,@CountryID,@Email,@Phone,@Fax,@Mobile,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_WaveMailingDetail_Select_IssueID]...';


GO
CREATE PROCEDURE [dbo].[e_WaveMailingDetail_Select_IssueID]
@IssueID int
AS
	SELECT * FROM WaveMailingDetail wmd With(NoLock)
	JOIN WaveMailing wm With(NoLock) ON wmd.WaveMailingID = wm.WaveMailingID
	WHERE wm.IssueID = @IssueID
GO
PRINT N'Creating [dbo].[e_WaveMailingDetail_UpdateOriginal]...';


GO
CREATE PROCEDURE [dbo].[e_WaveMailingDetail_UpdateOriginal]
(
@WaveMailingDetailID int,
@WaveMailingID int,
@SubscriberID int,
@SubscriptionID int,
@DeliverabilityID int,
@ActionID_Current int,
@ActionID_Previous int,
@Copies int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar(50),
@AddressTypeID int,
@Address1 varchar(100),
@Address2 varchar(100),
@Address3 varchar(100),
@City varchar(50),
@RegionCode varchar(50),
@RegionID int,
@ZipCode varchar(10),
@Plus4 varchar(10),
@County varchar(50),
@Country varchar(50),
@CountryID int,
@Email varchar(100),
@Phone varchar(50),
@Fax varchar(50),
@Mobile varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
)
AS
BEGIN

	DECLARE	@executeString varchar(8000)
	DECLARE @subscriberString varchar(8000)
	DECLARE @subscriptionString varchar(8000)
	DECLARE @subscriberFinalString varchar(8000)
	DECLARE @subscriptionFinalString varchar(8000)	

	SET @subscriberFinalString = 'UPDATE Subscriber SET'
							
	SET @subscriptionFinalString = 'UPDATE Subscription SET'

	SET @subscriberString = ''
	SET @subscriptionString = ''

	if(@DeliverabilityID > 0)
		set @subscriptionString = @subscriptionString + ' DeliverabilityID = ' + ''''+ CONVERT(varchar(10),@DeliverabilityID) + '''' +  ','
		
	if(@ActionID_Current > 0)
		set @subscriptionString = @subscriptionString + ' ActionID_Current = ' + CONVERT(varchar(10),@ActionID_Current) + ','
		
	if(@ActionID_Previous > 0)
		set @subscriptionString = @subscriptionString + ' ActionID_Previous = ' + CONVERT(varchar(10),@ActionID_Previous) + ','
		
	if(@Copies > 0)
		set @subscriptionString = @subscriptionString + ' Copies = ' + ''''+ CONVERT(varchar(10),@Copies) + '''' + ','
		
	if(LEN(@FirstName) > 0)
		set @subscriberString = @subscriberString + ' FirstName = ' + '''' + @FirstName + '''' + ','
		
	if(LEN(@LastName) > 0)
		set @subscriberString = @subscriberString + ' LastName = ' + '''' + @LastName + '''' + ','
		
	if(LEN(@Company) > 0)
		set @subscriberString = @subscriberString + ' Company = ' + ''''+ @Company + '''' + ','
		
	if(LEN(@Title) > 0)
		set @subscriberString = @subscriberString + ' Title = ' + '''' + @Title + '''' + ','

	if(@AddressTypeID > 0)
		set @subscriberString = @subscriberString + ' AddressTypeID = ' + CONVERT(varchar(10),@AddressTypeID) + ','

	if(LEN(@Address1) > 0)
		set @subscriberString = @subscriberString + ' Address1 = ' + '''' + @Address1 + ''''+ ','

	if(LEN(@Address2) > 0)
		set @subscriberString = @subscriberString + ' Address2 = ' + '''' + @Address2 + '''' + ','
		
	if(LEN(@Address3) > 0)
		set @subscriberString = @subscriberString + ' Address3 = ' + '''' + @Address3 + '''' + ','
		
	if(LEN(@City) > 0)
		set @subscriberString = @subscriberString + ' City = ' + '''' + @City + '''' + ','
		
	if(LEN(@RegionCode) > 0)
		set @subscriberString = @subscriberString + ' RegionCode = ' + '''' + @RegionCode + '''' + ','
		
	if(@RegionID > 0)
		set @subscriberString = @subscriberString + ' RegionID = ' + CONVERT(varchar(10),@RegionID) + ','

	if(LEN(@ZipCode) > 0)
		set @subscriberString = @subscriberString + ' ZipCode = ' + '''' + @ZipCode + '''' + ','

	if(LEN(@Plus4) > 0)
		set @subscriberString = @subscriberString + ' Plus4 = ' + '''' + @Plus4 + '''' + ','
		
	if(LEN(@County) > 0)
		set @subscriberString = @subscriberString + ' County = ' + '''' + @County + '''' + ','
		
	if(LEN(@Country) > 0)
		set @subscriberString = @subscriberString + ' Country = ' + '''' + @Country + '''' + ','
		
	if(@CountryID > 0)
		set @subscriberString = @subscriberString + ' CountryID = ' + CONVERT(varchar(10),@CountryID) + ','

	if(LEN(@Email) > 0)
		set @subscriberString = @subscriberString + ' Email = ' + '''' + @Email + '''' + ','
		
	if(LEN(@Phone) > 0)
		set @subscriberString = @subscriberString + ' Phone = ' + '''' + @Phone + '''' + ','
		
	if(LEN(@Mobile) > 0)
		set @subscriberString = @subscriberString + ' Mobile = ' + '''' + @Mobile + '''' + ','
		
	if(LEN(@Fax) > 0)
		set @subscriberString = @subscriberString + ' Fax = ' + '''' + @Fax + '''' + ','
	
	if(LEN(@subscriberString) > 0)
		BEGIN
			set @subscriberString =	LEFT(@subscriberString, LEN(@subscriberString) - 1)
			set @subscriberFinalString = @subscriberFinalString + @subscriberString + ' WHERE SubscriberID = ' + CONVERT(varchar(255),@SubscriberID)
			EXEC(@subscriberFinalString)
		END
	
	if(LEN(@subscriptionString) > 0)
		BEGIN
			set @subscriptionString = LEFT(@subscriptionString, LEN(@subscriptionString) - 1)
			set @subscriptionFinalString = @subscriptionFinalString + @subscriptionString + ' WHERE SubscriptionID = ' + CONVERT(varchar(255),@SubscriptionID)	
			EXEC(@subscriptionFinalString)
		END
						
END
GO
PRINT N'Creating [dbo].[job_ACS_KillSubscriber]...';


GO
create procedure job_ACS_KillSubscriber
@xml xml,
@publicationID int,
@publisherID int
as
	SET NOCOUNT ON           
	DECLARE @docHandle int
    declare @insertcount int

	DECLARE @import TABLE
	(
					AcsFileDetailId int NOT NULL,
					ClientId int NULL,
					RecordType varchar(1) NULL,
					FileVersion varchar(2) NULL,
					SequenceNumber int NULL,
					AcsMailerId varchar(9) NULL,
					KeylineSequenceSerialNumber varchar(16) NULL,
					MoveEffectiveDate date NULL,
					MoveType varchar(1) NULL,
					DeliverabilityCode varchar(1) NULL,
					UspsSiteID int NULL,
					LastName varchar(20) NULL,
					FirstName varchar(15) NULL,
					Prefix varchar(6) NULL,
					Suffix varchar(6) NULL,
					OldAddressType varchar(1) NULL,
					OldUrb varchar(28) NULL,
					OldPrimaryNumber varchar(10) NULL,
					OldPreDirectional varchar(2) NULL,
					OldStreetName varchar(28) NULL,
					OldSuffix varchar(4) NULL,
					OldPostDirectional varchar(2) NULL,
					OldUnitDesignator varchar(4) NULL,
					OldSecondaryNumber varchar(10) NULL,
					OldCity varchar(28) NULL,
					OldStateAbbreviation varchar(2) NULL,
					OldZipCode varchar(5) NULL,
					NewAddressType varchar(1) NULL,
					NewPmb varchar(8) NULL,
					NewUrb varchar(28) NULL,
					NewPrimaryNumber varchar(10) NULL,
					NewPreDirectional varchar(2) NULL,
					NewStreetName varchar(28) NULL,
					NewSuffix varchar(4) NULL,
					NewPostDirectional varchar(2) NULL,
					NewUnitDesignator varchar(4) NULL,
					NewSecondaryNumber varchar(10) NULL,
					NewCity varchar(28) NULL,
					NewStateAbbreviation varchar(2) NULL,
					NewZipCode varchar(5) NULL,
					Hyphen varchar(1) NULL,
					NewPlus4Code varchar(4) NULL,
					NewDeliveryPoint varchar(2) NULL,
					NewAbbreviatedCityName varchar(13) NULL,
					NewAddressLabel varchar(66) NULL,
					FeeNotification varchar(1) NULL,
					NotificationType varchar(1) NULL,
					IntelligentMailBarcode varchar(31) NULL,
					IntelligentMailPackageBarcode varchar(35) NULL,
					IdTag varchar(16) NULL,
					HardcopyToElectronicFlag varchar(1) NULL,
					TypeOfAcs varchar(1) NULL,
					FulfillmentDate date NULL,
					ProcessingType varchar(1) NULL,
					CaptureType varchar(1) NULL,
					MadeAvailableDate date NULL,
					ShapeOfMail varchar(1) NULL,
					MailActionCode varchar(1) NULL,
					NixieFlag varchar(1) NULL,
					ProductCode1 int NULL,
					ProductCodeFee1 decimal(4, 2) NULL,
					ProductCode2 int NULL,
					ProductCodeFee2 decimal(4, 2) NULL,
					ProductCode3 int NULL,
					ProductCodeFee3 decimal(4, 2) NULL,
					ProductCode4 int NULL,
					ProductCodeFee4 decimal(4, 2) NULL,
					ProductCode5 int NULL,
					ProductCodeFee5 decimal(4, 2) NULL,
					ProductCode6 int NULL,
					ProductCodeFee6 decimal(4, 2) NULL,
					Filler varchar(405) NULL,
					EndMarker varchar(1) NULL,
					ProductCode varchar(50) NULL,
					OldAddress1 varchar(100) NULL,
					OldAddress2 varchar(100) NULL,
					OldAddress3 varchar(100) NULL,
					NewAddress1 varchar(100) NULL,
					NewAddress2 varchar(100) NULL,
					NewAddress3 varchar(100) NULL,
					SequenceID int NULL,
					TransactionCodeValue int NULL,
					CategoryCodeValue int NULL,
					IsIgnored bit NULL,
					ProcessCode varchar(50) NULL
	)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into @import 
	(
					AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
					Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
					OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
					NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
					HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
					ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
					NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,ProcessCode
	)  

	SELECT 
					AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
					Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
					OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
					NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
					HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
					ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
					NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,ProcessCode
	FROM OPENXML(@docHandle, N'/XML/AcsFileDetail') 
	WITH   
	(  
					AcsFileDetailId int 'AcsFileDetailId',
					ClientId int 'ClientId',
					RecordType varchar(1) 'RecordType',
					FileVersion varchar(2) 'FileVersion',
					SequenceNumber int 'SequenceNumber',
					AcsMailerId varchar(9) 'AcsMailerId',
					KeylineSequenceSerialNumber varchar(16) 'KeylineSequenceSerialNumber',
					MoveEffectiveDate date 'MoveEffectiveDate',
					MoveType varchar(1) 'MoveType',
					DeliverabilityCode varchar(1) 'DeliverabilityCode',
					UspsSiteID int 'UspsSiteID',
					LastName varchar(20) 'LastName',
					FirstName varchar(15) 'FirstName',
					Prefix varchar(6) 'Prefix',
					Suffix varchar(6) 'Suffix',
					OldAddressType varchar(1) 'OldAddressType',
					OldUrb varchar(28) 'OldUrb',
					OldPrimaryNumber varchar(10) 'OldPrimaryNumber',
					OldPreDirectional varchar(2) 'OldPreDirectional',
					OldStreetName varchar(28) 'OldStreetName',
					OldSuffix varchar(4) 'OldSuffix',
					OldPostDirectional varchar(2) 'OldPostDirectional',
					OldUnitDesignator varchar(4) 'OldUnitDesignator',
					OldSecondaryNumber varchar(10) 'OldSecondaryNumber',
					OldCity varchar(28) 'OldCity',
					OldStateAbbreviation varchar(2) 'OldStateAbbreviation',
					OldZipCode varchar(5) 'OldZipCode',
					NewAddressType varchar(1) 'NewAddressType',
					NewPmb varchar(8) 'NewPmb',
					NewUrb varchar(28) 'NewUrb',
					NewPrimaryNumber varchar(10) 'NewPrimaryNumber',
					NewPreDirectional varchar(2) 'NewPreDirectional',
					NewStreetName varchar(28) 'NewStreetName',
					NewSuffix varchar(4) 'NewSuffix',
					NewPostDirectional varchar(2) 'NewPostDirectional',
					NewUnitDesignator varchar(4) 'NewUnitDesignator',
					NewSecondaryNumber varchar(10) 'NewSecondaryNumber',
					NewCity varchar(28) 'NewCity',
					NewStateAbbreviation varchar(2) 'NewStateAbbreviation',
					NewZipCode varchar(5) 'NewZipCode',
					Hyphen varchar(1) 'Hyphen',
					NewPlus4Code varchar(4) 'NewPlus4Code',
					NewDeliveryPoint varchar(2) 'NewDeliveryPoint',
					NewAbbreviatedCityName varchar(13) 'NewAbbreviatedCityName',
					NewAddressLabel varchar(66) 'NewAddressLabel',
					FeeNotification varchar(1) 'FeeNotification',
					NotificationType varchar(1) 'NotificationType',
					IntelligentMailBarcode varchar(31) 'IntelligentMailBarcode',
					IntelligentMailPackageBarcode varchar(35) 'IntelligentMailPackageBarcode',
					IdTag varchar(16) 'IdTag',
					HardcopyToElectronicFlag varchar(1) 'HardcopyToElectronicFlag',
					TypeOfAcs varchar(1) 'TypeOfAcs',
					FulfillmentDate date 'FulfillmentDate',
					ProcessingType varchar(1) 'ProcessingType',
					CaptureType varchar(1) 'CaptureType',
					MadeAvailableDate date 'MadeAvailableDate',
					ShapeOfMail varchar(1) 'ShapeOfMail',
					MailActionCode varchar(1) 'MailActionCode',
					NixieFlag varchar(1) 'NixieFlag',
					ProductCode1 int 'ProductCode1',
					ProductCodeFee1 decimal(4, 2) 'ProductCodeFee1',
					ProductCode2 int 'ProductCode2',
					ProductCodeFee2 decimal(4, 2) 'ProductCodeFee2',
					ProductCode3 int 'ProductCode3',
					ProductCodeFee3 decimal(4, 2) 'ProductCodeFee3',
					ProductCode4 int 'ProductCode4',
					ProductCodeFee4 decimal(4, 2) 'ProductCodeFee4',
					ProductCode5 int 'ProductCode5',
					ProductCodeFee5 decimal(4, 2) 'ProductCodeFee5',
					ProductCode6 int 'ProductCode6',
					ProductCodeFee6 decimal(4, 2) 'ProductCodeFee6',
					Filler varchar(405) 'Filler',
					EndMarker varchar(1) 'EndMarker',
					ProductCode varchar(50) 'ProductCode',
					OldAddress1 varchar(100) 'OldAddress1',
					OldAddress2 varchar(100) 'OldAddress2',
					OldAddress3 varchar(100) 'OldAddress3',
					NewAddress1 varchar(100) 'NewAddress1',
					NewAddress2 varchar(100) 'NewAddress2',
					NewAddress3 varchar(100) 'NewAddress3',
					SequenceID int 'SequenceID',
					TransactionCodeValue int 'TransactionCodeValue',
					CategoryCodeValue int 'CategoryCodeValue',
					IsIgnored bit 'IsIgnored',
					ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle

    --Get a BatchID
	declare @batchCount int =	(
								select count(i.AcsFileDetailId)
								from @import i
								join Subscription s with(nolock) on i.SequenceID = s.SequenceID
								join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
							)
	declare @userID int = (select UserID from UAS..[User] where EmailAddress = 'AcsImport@TeamKM.com')
	declare @batchID int
	if(@batchCount > 0)
		begin
			insert into Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
			values(@publicationID,@userID,@batchCount,'false',getdate(),getdate());
			set @batchID = (select @@IDENTITY);
		end

	--foreach address in import where oldAddress = KmAddress update - log before/after in userlog table
	declare @sequenceID int
	declare @subscriptionID int
	declare @subscriberID int
	declare @acsFileDetailId int
	declare @tranCodeValue int
	declare @actionID_Current int
	
	DECLARE c CURSOR
	FOR 
					select s.SequenceID,s.SubscriptionID,s.SubscriberID,i.AcsFileDetailId,i.TransactionCodeValue,s.ActionID_Current
					from @import i
					join Subscription s with(nolock) on i.SequenceID = s.SequenceID
					join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
	OPEN c
	FETCH NEXT FROM c INTO @sequenceID,@subscriptionID,@subscriberID,@acsFileDetailId,@tranCodeValue,@actionID_Current
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--Get current TranCode value - if 34 just skip
		declare @tranCodeValueCurrent int = (select TransactionCodeValue
											from TransactionCode tc with(nolock)
											join Action a with(nolock) on tc.TransactionCodeID = a.TransactionCodeID
											where a.ActionID = @actionID_Current)
		IF(@tranCodeValueCurrent  != 34)
			Begin							
				declare @historySubscriptionID int
				--create HistorySubscription record
				insert into HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
																								IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
																								SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
																								OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
																								Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
																								AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
																								SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
																								LockDate,LockDateRelease,LockedByUserID,PhoneExt)
			    
				 select 
								SubscriptionID,PublisherID,s.SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
								IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,s.DateCreated,
								s.DateUpdated,s.CreatedByUserID,s.UpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
								OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
								Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
								AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
								sub.DateCreated,sub.DateUpdated,sub.CreatedByUserID,sub.UpdatedByUserID,sub.DateCreated,sub.CreatedByUserID,IsLocked,
								LockDate,LockDateRelease,LockedByUserID,PhoneExt
				from Subscription s with(nolock)
				join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
				where s.SubscriptionID = @subscriptionID;
				set @historySubscriptionID = (select @@IDENTITY);

				declare @historyId int
				Insert Into History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,HistoryPaidID,
																												HistoryPaidBillToID,DateCreated,CreatedByUserID)
				Values(@batchID,@batchCount,@publisherID,@publicationID,@subscriberID,@subscriptionID,@historySubscriptionID,0,0,GETDATE(),@userID);
				set @historyId = (select @@IDENTITY);
				
				declare @userLogTypeId int = (select CodeId from uas..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
				declare @appId int = (select ApplicationID from uas..Application with(nolock) where ApplicationName='Circulation')
				declare @userLogId int
				
				Insert Into UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
				Values(@appId,@userLogTypeId,@userID,'Subscription','job_ACS_KillSubscriber','',GETDATE());
				set @userLogId = (select @@IDENTITY);
				
				Insert Into HistoryToUserLog (HistoryID,UserLogID)
				Values(@historyId,@userLogId);
				
				--Now update the ActionID_Current and Previous
				declare @isPaid bit = (select IsPaid from Subscription with(nolock) where SubscriptionID = @subscriptionID)
				declare @currentActionID int = (select ActionID_Current from Subscription with(nolock) where SubscriptionID = @subscriptionID)
				declare @catCodeID int = (select CategoryCodeID from Action with(nolock) where ActionID = @currentActionID)
				declare @tranCodeID int
				declare @newActionID int
				IF(@isPaid = 'true')
					begin
						set @tranCodeID = (select TransactionCodeID from TransactionCode with(nolock) where TransactionCodeValue = 61)
						set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where CategoryCodeID = @catCodeID and TransactionCodeID = @tranCodeID and ActionTypeID = 2)

						Update Subscription
						set ActionID_Previous = @currentActionID,
							ActionID_Current = @newActionID 
						where SubscriptionID = @subscriptionID 
					end
				else
					begin
						set @tranCodeID = (select TransactionCodeID from TransactionCode with(nolock) where TransactionCodeValue = @tranCodeValue)
						set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = @tranCodeID and CategoryCodeID = @catCodeID and ActionTypeID = 2)

						Update Subscription
						set ActionID_Previous = @currentActionID,
							ActionID_Current = @newActionID 
						where SubscriptionID = @subscriptionID 
					end
			End
		FETCH NEXT FROM c INTO @sequenceID,@subscriptionID,@subscriberID,@acsFileDetailId,@tranCodeValue,@actionID_Current
	END
	CLOSE c
	DEALLOCATE c
GO
PRINT N'Creating [dbo].[job_ACS_UpdateSubscriberAddress]...';


GO
create procedure job_ACS_UpdateSubscriberAddress
@xml xml,
@publicationID int,
@publisherID int
as
	SET NOCOUNT ON           
	DECLARE @docHandle int
    declare @insertcount int

	DECLARE @import TABLE
	(
					AcsFileDetailId int NOT NULL,
					ClientId int NULL,
					RecordType varchar(1) NULL,
					FileVersion varchar(2) NULL,
					SequenceNumber int NULL,
					AcsMailerId varchar(9) NULL,
					KeylineSequenceSerialNumber varchar(16) NULL,
					MoveEffectiveDate date NULL,
					MoveType varchar(1) NULL,
					DeliverabilityCode varchar(1) NULL,
					UspsSiteID int NULL,
					LastName varchar(20) NULL,
					FirstName varchar(15) NULL,
					Prefix varchar(6) NULL,
					Suffix varchar(6) NULL,
					OldAddressType varchar(1) NULL,
					OldUrb varchar(28) NULL,
					OldPrimaryNumber varchar(10) NULL,
					OldPreDirectional varchar(2) NULL,
					OldStreetName varchar(28) NULL,
					OldSuffix varchar(4) NULL,
					OldPostDirectional varchar(2) NULL,
					OldUnitDesignator varchar(4) NULL,
					OldSecondaryNumber varchar(10) NULL,
					OldCity varchar(28) NULL,
					OldStateAbbreviation varchar(2) NULL,
					OldZipCode varchar(5) NULL,
					NewAddressType varchar(1) NULL,
					NewPmb varchar(8) NULL,
					NewUrb varchar(28) NULL,
					NewPrimaryNumber varchar(10) NULL,
					NewPreDirectional varchar(2) NULL,
					NewStreetName varchar(28) NULL,
					NewSuffix varchar(4) NULL,
					NewPostDirectional varchar(2) NULL,
					NewUnitDesignator varchar(4) NULL,
					NewSecondaryNumber varchar(10) NULL,
					NewCity varchar(28) NULL,
					NewStateAbbreviation varchar(2) NULL,
					NewZipCode varchar(5) NULL,
					Hyphen varchar(1) NULL,
					NewPlus4Code varchar(4) NULL,
					NewDeliveryPoint varchar(2) NULL,
					NewAbbreviatedCityName varchar(13) NULL,
					NewAddressLabel varchar(66) NULL,
					FeeNotification varchar(1) NULL,
					NotificationType varchar(1) NULL,
					IntelligentMailBarcode varchar(31) NULL,
					IntelligentMailPackageBarcode varchar(35) NULL,
					IdTag varchar(16) NULL,
					HardcopyToElectronicFlag varchar(1) NULL,
					TypeOfAcs varchar(1) NULL,
					FulfillmentDate date NULL,
					ProcessingType varchar(1) NULL,
					CaptureType varchar(1) NULL,
					MadeAvailableDate date NULL,
					ShapeOfMail varchar(1) NULL,
					MailActionCode varchar(1) NULL,
					NixieFlag varchar(1) NULL,
					ProductCode1 int NULL,
					ProductCodeFee1 decimal(4, 2) NULL,
					ProductCode2 int NULL,
					ProductCodeFee2 decimal(4, 2) NULL,
					ProductCode3 int NULL,
					ProductCodeFee3 decimal(4, 2) NULL,
					ProductCode4 int NULL,
					ProductCodeFee4 decimal(4, 2) NULL,
					ProductCode5 int NULL,
					ProductCodeFee5 decimal(4, 2) NULL,
					ProductCode6 int NULL,
					ProductCodeFee6 decimal(4, 2) NULL,
					Filler varchar(405) NULL,
					EndMarker varchar(1) NULL,
					ProductCode varchar(50) NULL,
					OldAddress1 varchar(100) NULL,
					OldAddress2 varchar(100) NULL,
					OldAddress3 varchar(100) NULL,
					NewAddress1 varchar(100) NULL,
					NewAddress2 varchar(100) NULL,
					NewAddress3 varchar(100) NULL,
					SequenceID int NULL,
					TransactionCodeValue int NULL,
					CategoryCodeValue int NULL,
					IsIgnored bit NULL,
					ProcessCode varchar(50) NULL
	)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into @import 
	(
					AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
					Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
					OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
					NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
					HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
					ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
					NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,ProcessCode
	)  

	SELECT 
					AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
					Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
					OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
					NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
					HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
					ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
					NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,ProcessCode
	FROM OPENXML(@docHandle, N'/XML/AcsFileDetail') 
	WITH   
	(  
					AcsFileDetailId int 'AcsFileDetailId',
					ClientId int 'ClientId',
					RecordType varchar(1) 'RecordType',
					FileVersion varchar(2) 'FileVersion',
					SequenceNumber int 'SequenceNumber',
					AcsMailerId varchar(9) 'AcsMailerId',
					KeylineSequenceSerialNumber varchar(16) 'KeylineSequenceSerialNumber',
					MoveEffectiveDate date 'MoveEffectiveDate',
					MoveType varchar(1) 'MoveType',
					DeliverabilityCode varchar(1) 'DeliverabilityCode',
					UspsSiteID int 'UspsSiteID',
					LastName varchar(20) 'LastName',
					FirstName varchar(15) 'FirstName',
					Prefix varchar(6) 'Prefix',
					Suffix varchar(6) 'Suffix',
					OldAddressType varchar(1) 'OldAddressType',
					OldUrb varchar(28) 'OldUrb',
					OldPrimaryNumber varchar(10) 'OldPrimaryNumber',
					OldPreDirectional varchar(2) 'OldPreDirectional',
					OldStreetName varchar(28) 'OldStreetName',
					OldSuffix varchar(4) 'OldSuffix',
					OldPostDirectional varchar(2) 'OldPostDirectional',
					OldUnitDesignator varchar(4) 'OldUnitDesignator',
					OldSecondaryNumber varchar(10) 'OldSecondaryNumber',
					OldCity varchar(28) 'OldCity',
					OldStateAbbreviation varchar(2) 'OldStateAbbreviation',
					OldZipCode varchar(5) 'OldZipCode',
					NewAddressType varchar(1) 'NewAddressType',
					NewPmb varchar(8) 'NewPmb',
					NewUrb varchar(28) 'NewUrb',
					NewPrimaryNumber varchar(10) 'NewPrimaryNumber',
					NewPreDirectional varchar(2) 'NewPreDirectional',
					NewStreetName varchar(28) 'NewStreetName',
					NewSuffix varchar(4) 'NewSuffix',
					NewPostDirectional varchar(2) 'NewPostDirectional',
					NewUnitDesignator varchar(4) 'NewUnitDesignator',
					NewSecondaryNumber varchar(10) 'NewSecondaryNumber',
					NewCity varchar(28) 'NewCity',
					NewStateAbbreviation varchar(2) 'NewStateAbbreviation',
					NewZipCode varchar(5) 'NewZipCode',
					Hyphen varchar(1) 'Hyphen',
					NewPlus4Code varchar(4) 'NewPlus4Code',
					NewDeliveryPoint varchar(2) 'NewDeliveryPoint',
					NewAbbreviatedCityName varchar(13) 'NewAbbreviatedCityName',
					NewAddressLabel varchar(66) 'NewAddressLabel',
					FeeNotification varchar(1) 'FeeNotification',
					NotificationType varchar(1) 'NotificationType',
					IntelligentMailBarcode varchar(31) 'IntelligentMailBarcode',
					IntelligentMailPackageBarcode varchar(35) 'IntelligentMailPackageBarcode',
					IdTag varchar(16) 'IdTag',
					HardcopyToElectronicFlag varchar(1) 'HardcopyToElectronicFlag',
					TypeOfAcs varchar(1) 'TypeOfAcs',
					FulfillmentDate date 'FulfillmentDate',
					ProcessingType varchar(1) 'ProcessingType',
					CaptureType varchar(1) 'CaptureType',
					MadeAvailableDate date 'MadeAvailableDate',
					ShapeOfMail varchar(1) 'ShapeOfMail',
					MailActionCode varchar(1) 'MailActionCode',
					NixieFlag varchar(1) 'NixieFlag',
					ProductCode1 int 'ProductCode1',
					ProductCodeFee1 decimal(4, 2) 'ProductCodeFee1',
					ProductCode2 int 'ProductCode2',
					ProductCodeFee2 decimal(4, 2) 'ProductCodeFee2',
					ProductCode3 int 'ProductCode3',
					ProductCodeFee3 decimal(4, 2) 'ProductCodeFee3',
					ProductCode4 int 'ProductCode4',
					ProductCodeFee4 decimal(4, 2) 'ProductCodeFee4',
					ProductCode5 int 'ProductCode5',
					ProductCodeFee5 decimal(4, 2) 'ProductCodeFee5',
					ProductCode6 int 'ProductCode6',
					ProductCodeFee6 decimal(4, 2) 'ProductCodeFee6',
					Filler varchar(405) 'Filler',
					EndMarker varchar(1) 'EndMarker',
					ProductCode varchar(50) 'ProductCode',
					OldAddress1 varchar(100) 'OldAddress1',
					OldAddress2 varchar(100) 'OldAddress2',
					OldAddress3 varchar(100) 'OldAddress3',
					NewAddress1 varchar(100) 'NewAddress1',
					NewAddress2 varchar(100) 'NewAddress2',
					NewAddress3 varchar(100) 'NewAddress3',
					SequenceID int 'SequenceID',
					TransactionCodeValue int 'TransactionCodeValue',
					CategoryCodeValue int 'CategoryCodeValue',
					IsIgnored bit 'IsIgnored',
					ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle

    --Get a BatchID
	declare @batchCount int =	(
								select count(i.AcsFileDetailId)
								from @import i
								join Subscription s with(nolock) on i.SequenceID = s.SequenceID
								join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
								where sub.Address1 = i.OldAddress1
								and sub.City = i.OldCity
								and sub.RegionCode = i.OldStateAbbreviation 
								and sub.ZipCode = i.OldZipCode
							)
	declare @userID int = (select UserID from UAS..[User] where EmailAddress = 'AcsImport@TeamKM.com')
	declare @batchID int
	if(@batchCount > 0)
		begin
			insert into Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
			values(@publicationID,@userID,@batchCount,'false',getdate(),getdate());
			set @batchID = (select @@IDENTITY);
		end

	--foreach address in import where oldAddress = KmAddress update - log before/after in userlog table
	declare @sequenceID int
	declare @subscriptionID int
	declare @subscriberID int
	declare @acsFileDetailId int

	DECLARE c CURSOR
	FOR 
					select s.SequenceID,s.SubscriptionID,s.SubscriberID,i.AcsFileDetailId
					from @import i
					join Subscription s with(nolock) on i.SequenceID = s.SequenceID
					join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
					where sub.Address1 = i.OldAddress1
					and sub.City = i.OldCity
					and sub.RegionCode = i.OldStateAbbreviation 
					and sub.ZipCode = i.OldZipCode
	OPEN c
	FETCH NEXT FROM c INTO @sequenceID,@subscriptionID,@subscriberID,@acsFileDetailId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	                
					declare @historySubscriptionID int
					--create HistorySubscription record
					insert into HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
																									IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
																									SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
																									OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
																									Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
																									AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
																									SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
																									LockDate,LockDateRelease,LockedByUserID,PhoneExt)
	                
					 select 
									SubscriptionID,PublisherID,s.SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
									IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,s.DateCreated,
									s.DateUpdated,s.CreatedByUserID,s.UpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
									OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
									Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
									AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
									sub.DateCreated,sub.DateUpdated,sub.CreatedByUserID,sub.UpdatedByUserID,sub.DateCreated,sub.CreatedByUserID,IsLocked,
									LockDate,LockDateRelease,LockedByUserID,PhoneExt
					from Subscription s with(nolock)
					join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
					where s.SubscriptionID = @subscriptionID;
					set @historySubscriptionID = (select @@IDENTITY);

					declare @historyId int
					Insert Into History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,HistoryPaidID,
																													HistoryPaidBillToID,DateCreated,CreatedByUserID)
					Values(@batchID,@batchCount,@publisherID,@publicationID,@subscriberID,@subscriptionID,@historySubscriptionID,0,0,GETDATE(),@userID);
					set @historyId = (select @@IDENTITY);
					
					declare @userLogTypeId int = (select CodeId from uas..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
					declare @appId int = (select ApplicationID from uas..Application with(nolock) where ApplicationName='Circulation')
					declare @userLogId int
					
					Insert Into UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
					Values(@appId,@userLogTypeId,@userID,'Subscriber','job_ACS_UpdateSubscriberAddress','',GETDATE());
					set @userLogId = (select @@IDENTITY);
					
					Insert Into HistoryToUserLog (HistoryID,UserLogID)
					Values(@historyId,@userLogId);
					
					
					--Now actually update the address
					Update Subscriber
					set Address1 = i.NewAddress1,
						Address2 = i.NewAddress2,
						Address3 = i.NewAddress3,
						City = i.NewCity,
						RegionCode = i.NewStateAbbreviation,
						RegionID = (select RegionID from UAS..Region with(nolock) where RegionCode = i.NewStateAbbreviation),
						ZipCode = i.NewZipCode,
						Plus4 = i.NewPlus4Code,
						IsAddressValidated = 'false',
						Latitude = 0,
						Longitude = 0,
						AddressValidationDate = null,
						AddressValidationMessage = '',
						DateUpdated = GETDATE(),
						UpdatedByUserID = @userID
					from @import i
					where i.AcsFileDetailId = @acsFileDetailId
					and Subscriber.SubscriberID = @subscriberID
					
					--Now update the ActionID_Current and Previous
					declare @isPaid bit = (select IsPaid from Subscription with(nolock) where SubscriptionID = @subscriptionID)
					declare @currentActionID int = (select ActionID_Current from Subscription with(nolock) where SubscriptionID = @subscriptionID)
					declare @catCodeID int = (select CategoryCodeID from Action where ActionID = @currentActionID)
					declare @newActionID int = 0
					IF(@isPaid = 'true')
						begin
							set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 2 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

							Update Subscription
							set ActionID_Previous = @currentActionID,
								ActionID_Current = @newActionID --TransactionCodeID=2 TransactionCodeValue = 21
							where SubscriptionID = @subscriptionID 
						end
					else
						begin
							set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 17 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

							Update Subscription
							set ActionID_Previous = @currentActionID,
								ActionID_Current = @newActionID --TransactionCodeID=17 TransactionCodeValue = 21
							where SubscriptionID = @subscriptionID 
						end
						
			FETCH NEXT FROM c INTO @sequenceID,@subscriptionID,@subscriberID,@acsFileDetailId
	END
	CLOSE c
	DEALLOCATE c
GO
PRINT N'Creating [dbo].[job_NCOA_AddressUpdate]...';


GO
create procedure job_NCOA_AddressUpdate
@xml xml
as
	SET NOCOUNT ON           
	DECLARE @docHandle int
    declare @insertcount int

	CREATE TABLE #NCOAimport 
	(
					SequenceID int NOT NULL,
					Address1 varchar(100) null,
					Address2 varchar(100) null,
					City varchar(50) null,
					RegionCode varchar(50) null,
					ZipCode varchar(50) null,
					Plus4 varchar(10) null,
					PublisherID int not null,
					PublicationID int not null,
					ProcessCode varchar(50) not null
	)
	CREATE NONCLUSTERED INDEX [IDX_SequenceID] ON #NCOAimport (SequenceID ASC)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into #NCOAimport 
	(
		SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,PublisherID,PublicationID,ProcessCode 
	)  

	SELECT SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,PublisherID,PublicationID,ProcessCode 
	FROM OPENXML(@docHandle, N'/XML/NCOA') 
	WITH   
	(  
		SequenceID int 'SequenceID',
		Address1 varchar(100) 'Address1',
		Address2 varchar(100) 'Address2',
		City varchar(50) 'City',
		RegionCode varchar(50) 'RegionCode',
		ZipCode varchar(50) 'ZipCode',
		Plus4 varchar(10) 'Plus4',
		PublisherID int 'PublisherID',
		PublicationID int 'PublicationID',
		ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle

	declare @publisherID int = (select distinct PublisherID from #NCOAimport)
    declare @publicationID int = (select distinct PublicationID from #NCOAimport)
    
    delete #NCOAimport where SequenceID not in (Select SequenceID from Subscription with(nolock))
    
	declare @batchCount int = (select count(i.SequenceID)
								from #NCOAimport i)
								--join Subscription s with(nolock) on i.SequenceID = s.SequenceID)

	declare @userID int = (select UserID from UAS..[User] where EmailAddress = 'NcoaImport@TeamKM.com')
	declare @batchID int
	if(@batchCount > 0)
			begin
				insert into Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
				values(@publicationID,@userID,@batchCount,'false',getdate(),getdate());
				set @batchID = (select @@IDENTITY);
			end
		--select * from Batch where PublicationID=7
		--select * from UserLog where FromObjectValues='job_NCOA_AddressUpdate'
		
	--foreach address in import where oldAddress = KmAddress update - log before/after in userlog table
	declare @SequenceID int
	declare @Address1 varchar(100)
	declare @Address2 varchar(100)
	declare @City varchar(50)
	declare @RegionCode varchar(50)
	declare @ZipCode varchar(50)
	declare @Plus4 varchar(10)
	declare @ProcessCode varchar(50)
	declare @SubscriptionID int
	declare @SubscriberID int
	
	
	DECLARE c CURSOR
	FOR 
					select i.SequenceID,i.Address1,i.Address2,i.City,i.RegionCode,i.ZipCode,i.Plus4,i.ProcessCode,s.SubscriptionID,s.SubscriberID
					from #NCOAimport i
					join Subscription s with(nolock) on i.SequenceID = s.SequenceID
					join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
					where i.PublisherID = @publisherID
	OPEN c
	FETCH NEXT FROM c INTO @SequenceID,@Address1,@Address2,@City,@RegionCode,@ZipCode,@Plus4,@ProcessCode,@SubscriptionID,@SubscriberID
	WHILE @@FETCH_STATUS = 0
	BEGIN
	                
			declare @historySubscriptionID int
			--create HistorySubscription record
			insert into HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
											IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
											SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
											OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
											Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
											AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
											SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
											LockDate,LockDateRelease,LockedByUserID,PhoneExt)
            
			 select 
					SubscriptionID,PublisherID,s.SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
					IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,s.DateCreated,
					s.DateUpdated,s.CreatedByUserID,s.UpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
					OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
					Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
					AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
					sub.DateCreated,sub.DateUpdated,sub.CreatedByUserID,sub.UpdatedByUserID,sub.DateCreated,sub.CreatedByUserID,IsLocked,
					LockDate,LockDateRelease,LockedByUserID,PhoneExt
			from Subscription s with(nolock)
			join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
			where s.SubscriptionID = @subscriptionID;
			set @historySubscriptionID = (select @@IDENTITY);

			declare @historyId int
			Insert Into History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,HistoryPaidID,
																											HistoryPaidBillToID,DateCreated,CreatedByUserID)
			Values(@batchID,@batchCount,@publisherID,@publicationID,@subscriberID,@subscriptionID,@historySubscriptionID,0,0,GETDATE(),@userID);
			set @historyId = (select @@IDENTITY);
			
			declare @userLogTypeId int = (select CodeId from uas..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
			declare @appId int = (select ApplicationID from uas..Application with(nolock) where ApplicationName='Circulation')
			declare @userLogId int
			
			Insert Into UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
			Values(@appId,@userLogTypeId,@userID,'Subscriber','job_NCOA_AddressUpdate','',GETDATE());
			set @userLogId = (select @@IDENTITY);
			
			Insert Into HistoryToUserLog (HistoryID,UserLogID)
			Values(@historyId,@userLogId);

			--Now actually update the address
			Update Subscriber
			set Address1 = @Address1,
				Address2 = @Address2,
				City = @City,
				RegionCode = @RegionCode,
				RegionID = (select RegionID from UAS..Region with(nolock) where RegionCode = @RegionCode),
				ZipCode = @ZipCode,
				Plus4 = @Plus4,
				IsAddressValidated = 'false',
				Latitude = 0,
				Longitude = 0,
				AddressValidationDate = null,
				AddressValidationMessage = '',
				DateUpdated = GETDATE(),
				UpdatedByUserID = @userID
			where Subscriber.SubscriberID = @SubscriberID
			
			--Now update the ActionID_Current and Previous
			declare @isPaid bit = (select IsPaid from Subscription with(nolock) where SubscriptionID = @SubscriptionID)
			declare @currentActionID int = (select ActionID_Current from Subscription with(nolock) where SubscriptionID = @SubscriptionID)
			declare @catCodeID int = (select CategoryCodeID from Action where ActionID = @currentActionID)
			declare @newActionID int = 0
			IF(@isPaid = 'true')
				begin
					set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 2 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

					Update Subscription
					set ActionID_Previous = @currentActionID,
						ActionID_Current = @newActionID --TransactionCodeID=2 TransactionCodeValue = 21
					where SubscriptionID = @subscriptionID 
				end
			else
				begin
					set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 17 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

					Update Subscription
					set ActionID_Previous = @currentActionID,
						ActionID_Current = @newActionID --TransactionCodeID=17 TransactionCodeValue = 21
					where SubscriptionID = @subscriptionID 
				end
						
		FETCH NEXT FROM c INTO @SequenceID,@Address1,@Address2,@City,@RegionCode,@ZipCode,@Plus4,@ProcessCode,@SubscriptionID,@SubscriberID
	END
	CLOSE c
	DEALLOCATE c
	
	DROP TABLE #NCOAimport
GO
PRINT N'Creating [dbo].[job_Subscriber_Clean_Up]...';


GO
CREATE PROCEDURE job_Subscriber_Clean_Up

AS
BEGIN

	SET NOCOUNT ON;
	-- This proc is used to delete subscriber and subscription records that are NEWLY created but a system shut down occured.
	-- System shut down as in, the users computer froze or the users computer lost power

	-- Create temp table
	create table #SubscriberForDelete(SubscriberID Int, SubscriptionID int)
	
	-- Insert ID's that are to be deleted
	insert into #SubscriberForDelete
	select SubscriberID,SubscriptionID 
	from Circulation..Subscription
	where ActionID_Current = -1 and CreatedByUserID in (
		select distinct ual.UserID
		from UAS..[Service] s with(nolock)
				join UAS..ClientGroupServiceMap cgsm with(nolock) on s.ServiceID = cgsm.ServiceID
				join UAS..ClientGroupUserMap cgum with(nolock) on cgsm.ClientGroupID = cgum.ClientGroupID
				join UAS..UserAuthorizationLog ual with(nolock) on cgum.UserID = ual.UserID
		where s.ServiceID = 1 
				and s.ServiceCode = 'FUL' 
				and CONVERT(Date,logoutdate) = CONVERT(Date,GETDATE())
				and CONVERT(TIME,LogOutTime) >= Convert(TIME,DATEADD(hour, -1, SYSDATETIMEOFFSET())) )
	
	-- Begin Delete
	delete s from Subscriber s join #SubscriberForDelete sd on s.SubscriberID = sd.SubscriberID
	delete ss from Subscription ss join #SubscriberForDelete sd on ss.SubscriptionID = sd.SubscriptionID
	
	drop table #SubscriberForDelete
	
END
GO
PRINT N'Creating [dbo].[o_FinalizeBatch_SelectAll_UserName]...';


GO
CREATE PROCEDURE [dbo].[o_FinalizeBatch_SelectAll_UserName]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,cpm.ProductID,p.PublicationName, p.PublicationCode, Max(h.BatchCountItem) as LastCount, u.FirstName + ' ' + u.LastName as UserName, b.DateCreated, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN ClientProductMap cpm ON cpm.ProductID = b.PublicationID
		JOIN Publication p ON cpm.ProductCode = p.PublicationCode
		JOIN UAS..[Client] pub With(NoLock) ON cpm.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		JOIN UAS..[User] u With(NoLock) ON u.UserID = b.UserID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		LEFT JOIN UAS..CodeType ct ON ct.CodeTypeName = 'User Log'
		LEFT JOIN UAS..Code c ON c.CodeTypeId = ct.CodeTypeDescription 
		WHERE b.UserID = @UserID
	Group By b.BatchID, pub.ClientID, pub.ClientName, cpm.ProductID, p.PublicationName, p.PublicationCode, u.FirstName, u.LastName, b.DateCreated, b.DateFinalized,b.BatchNumber
GO
PRINT N'Creating [dbo].[o_SubscriptionSearchResult_Select_SubscriberID_Multiple]...';


GO
CREATE PROCEDURE [dbo].[o_SubscriptionSearchResult_Select_SubscriberID_Multiple]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int
    
	CREATE TABLE #import
	(  
      [SubscriberID] int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 [SubscriberID]
	)  
	
	SELECT [SubscriberID]
	FROM OPENXML(@docHandle,N'/XML/SubscriptionSearchResult')
	WITH
	(
		[SubscriberID] int 'SubscriberID'
	)
	
	EXEC sp_xml_removedocument @docHandle


	SELECT s.SubscriptionID,s.SequenceID,RTRIM(LTRIM(ISNULL(sr.Address1, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(sr.Address2,''))) + ', ' + RTRIM(LTRIM(ISNULL(sr.City, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(sr.RegionCode,''))) + ', ' + 
		(case when sr.CountryID = 1 and ISNULL(sr.ZipCode,'')!='' AND ISNULL(sr.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + '-' + sr.Plus4 
			  when sr.CountryID = 2 and ISNULL(sr.ZipCode,'')!='' AND ISNULL(sr.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + ' ' + sr.Plus4 
			  else RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + RTRIM(LTRIM(ISNULL(sr.Plus4,''))) end)+ ', ' + 
		RTRIM(LTRIM(ISNULL(sr.Country, ''))) as 'FullAddress',
		   '' as ProductCode,sr.Phone,sr.Email,s.PublicationID as ProductID,s.PublisherID as ClientID,s.SubscriberID,
		   '' as ClientName,sr.Company,s.IsSubscribed,s.AccountNumber,ISNULL(uc.PhonePrefix,'0') AS PhoneCode,
		   s.SubscriptionStatusID
	FROM Subscription s With(NoLock)
	JOIN Subscriber sr With(NoLock) ON sr.SubscriberID = s.SubscriberID
	--JOIN Publication cation With(NoLock) ON s.PublicationID = cation.PublicationID
	--JOIN Publisher p With(NoLock) ON cation.PublisherID = p.PublisherID
	LEFT JOIN UAS..Country uc WITH(Nolock) ON sr.CountryID = uc.CountryID
	WHERE s.SubscriberID in (Select [SubscriberID] from #import)
GO
PRINT N'Creating [dbo].[rpt_AcsImportCount]...';


GO
create procedure rpt_AcsImportCount
@StartDate date,
@EndDate date
as
	select c.ClientID,c.ClientName,p.PublicationCode, 
		(select count(TransactionCodeValue) 
		 from AcsFileDetail x21
		 where TransactionCodeValue = 21
		 and x21.ProductCode = d.ProductCode) as 'xact21',
		 (select count(TransactionCodeValue) 
		 from AcsFileDetail x31
		 where TransactionCodeValue = 31
		 and x31.ProductCode = d.ProductCode) as 'xact31',
		  (select count(TransactionCodeValue) 
		 from AcsFileDetail x32
		 where TransactionCodeValue = 32
		 and x32.ProductCode = d.ProductCode) as 'xact32',
		 (select count(TransactionCodeValue) 
		 from AcsFileDetail t
		 where t.ProductCode = d.ProductCode) as 'Total'
	from AcsFileDetail d with(nolock)
	join AcsMailerInfo a with(nolock) on d.AcsMailerId = a.AcsMailerId
	join Publication p with(nolock) on a.AcsMailerInfoId = p.AcsMailerInfoId
	join Publisher pub with(nolock) on p.PublisherID = pub.PublisherID
	join uas..Client c with(nolock) on pub.ClientID = c.ClientID 
	where d.CreatedDate between @StartDate and @EndDate
	group by c.ClientID,c.ClientName,p.PublicationCode,d.ProductCode
	order by p.PublicationCode
GO
PRINT N'Creating [dbo].[sp_rpt_GetSubscriberDetails]...';


GO
CREATE PROCEDURE [dbo].[sp_rpt_GetSubscriberDetails]
	@ProductID int,
	@AddKillID int
AS
	 SELECT s.SubscriberID, s.Email, s.FirstName, s.LastName, s.COMPANY, s.TITLE, s.Address1, s.Address2, s.CITY, s.RegionCode, s.ZipCode, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, a.CategoryCodeID as CAT,
	 c.CategoryCodeValue as CategoryName, a.TransactionCodeID as XACT, s.Plus4, s.County, sub.QSourceID, q.QSourceName + '' + q.QSourceCode + '' as Qsource, 
	 sub.QSourceDate, sub.SubsrcTypeID, p.PublicationCode as pubcode, sub.Copies
	from Subscriber s JOIN 
	Subscription sub ON s.SubscriberID = sub.SubscriberID JOIN
	Action a ON a.ActionID = sub.ActionID_Current JOIN
	CategoryCode c ON c.CategoryCodeID = a.CategoryCodeID LEFT outer join 
	QualificationSource q on q.QSourceID = sub.QSourceID join 
	Publication p ON p.PublicationID = sub.PublicationID
	WHERE p.PublicationID = @ProductID AND
	sub.AddRemoveID > 0
GO
PRINT N'Creating [dbo].[sp_rpt_GetSubscriptionIDs]...';


GO
CREATE Proc [dbo].[sp_rpt_GetSubscriptionIDs]
(  
	--@Test int
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(800),
	@Email varchar(10),
	@Phone varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(800),
	@Demo7 varchar(10),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@GetSubscriberIDs bit
) AS

BEGIN

DECLARE	@executeString varchar(8000)
DECLARE @currentYear int
DECLARE @tempStartDate varchar(10)
DECLARE @tempEndDate varchar(10)
DECLARE @years table (value varchar(4))
DECLARE @tempYear varchar(20)

CREATE TABLE #Areas
(
	[Value] varchar(40)
)

CREATE TABLE #AdHoc
(  
	RowID int IDENTITY(1, 1)
  ,[FilterObject] nvarchar(256)
  ,[SelectedCondition] nvarchar(256)
  ,[Type] nvarchar(256)
  ,[Value] nvarchar(256)
  ,[ToValue] nvarchar(256)
  ,[FromValue] nvarchar(256)
)

DECLARE @docHandle int
--DECLARE @AdHocXML varchar(8000)

--set @AdHocXML = '<XML><AdHocFilters.AdHocFilterField><FilterObject>FirstName</FilterObject><FromValue></FromValue><SelectedCondition>CONTAINS</SelectedCondition><ToValue></ToValue><Type>Standard</Type><Value>Gabriel</Value></AdHocFilters.AdHocFilterField>\r\n<AdHocFilters.AdHocFilterField  ><FilterObject>LastName</FilterObject><FromValue></FromValue><SelectedCondition>CONTAINS</SelectedCondition><ToValue></ToValue><Type>Standard</Type><Value>Santiago</Value></AdHocFilters.AdHocFilterField>\r\n<AdHocFilters.AdHocFilterField><FilterObject>StartIssueDate</FilterObject><FromValue>11/01/2011</FromValue><SelectedCondition>DateRange</SelectedCondition><ToValue>11/01/2014</ToValue><Type>DateRange</Type><Value></Value></AdHocFilters.AdHocFilterField></XML>'

EXEC sp_xml_preparedocument @docHandle OUTPUT, @AdHocXML  
INSERT INTO #AdHoc 
(
	 [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
)  
SELECT [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
FROM OPENXML(@docHandle,N'/XML/FilterDetail')
WITH
(
	[FilterObject] nvarchar(256) 'FilterField',
	[SelectedCondition] nvarchar(256) 'SearchCondition',
	[Type] nvarchar(256) 'FilterObjectType',
	[Value] nvarchar(265) 'AdHocFieldValue',
	[ToValue] nvarchar(256) 'AdHocToField',
	[FromValue] nvarchar(256) 'AdHocFromField'
)

EXEC sp_xml_removedocument @docHandle
	
--DECLARE @PublicationID int
--DECLARE @CategoryIDs varchar(800)
--DECLARE @CategoryCodes varchar(800)
--DECLARE @TransactionIDs varchar(800)
--DECLARE @TransactionCodes varchar(800)
--DECLARE @QsourceIDs varchar(800)
--DECLARE @StateIDs varchar(800)
--DECLARE @Regions varchar(max)
--DECLARE @CountryIDs varchar(800)
--DECLARE @Email varchar(10)
--DECLARE @Phone varchar(10)
--DECLARE @Fax varchar(10)
--DECLARE @Year varchar(800)
--DECLARE @startDate varchar(10)	
--DECLARE @endDate varchar(10)
--DECLARE @ResponseIDs varchar(800)
--DECLARE @DEMO7 varchar(800)
--DECLARE @GetSubscriberIDs bit
--set @PublicationID = 113
--set @TransactionCodes = '9'
--set @GetSubscriberIDs = 0
----set @Regions = 'America Central, Asia Pacific'
----set @CountryIDs = '1,2'
----set @CategoryIDs = '1,3'
----set @CategoryCodes = '1,2,3,4,5,6'
----set @TransactionCodes = '1,2,3,4,5,6,7,10,11,12'
----set @TransactionIDs = '1,3'
----set @QsourceIDs = '1,3,4'
----set @StateIDs = '1,2,3,5,6'
----set @Email = 'Yes'
----set @Phone = 'Yes'
----set @Fax = 'No'
----set @Year = '1,2,3,4,5'
----set @ResponseIDs = '35819'

if(@GetSubscriberIDs = 0)
	set @executeString =
	'Select	sp.SubscriptionID as Count
	FROM	Subscriber s JOIN
			Subscription sp ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
			SubscriptionPaid spp ON sp.SubscriptionID = spp.SubscriptionID JOIN
			Action a ON a.ActionID = sp.ActionID_Current JOIN
			CategoryCode dc ON a.CategoryCodeID = dc.CategoryCodeID JOIN
			TransactionCode dt on a.TransactionCodeID = dt.TransactionCodeID JOIN
			TransactionCodeType tct ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
			CategoryCodeType cct ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
			QualificationSource dq on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
			UAS..Country c ON c.CountryID = s.CountryID
	Where	
			sp.PublicationID = ' + Convert(varchar,@PublicationID) 
else if(@GetSubscriberIDs = 1)
	set @executeString =
	'Select	sp.SubscriberID as Count
	FROM	Subscriber s JOIN
			Subscription sp ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
			SubscriptionPaid spp ON sp.SubscriptionID = spp.SubscriptionID JOIN
			Action a ON a.ActionID = sp.ActionID_Current JOIN
			CategoryCode dc ON a.CategoryCodeID = dc.CategoryCodeID JOIN
			TransactionCode dt on a.TransactionCodeID = dt.TransactionCodeID JOIN
			TransactionCodeType tct ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
			CategoryCodeType cct ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
			QualificationSource dq on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
			UAS..Country c ON c.CountryID = s.CountryID
	Where	
			sp.PublicationID = ' + Convert(varchar,@PublicationID) 
		
Begin --Check passed Parameters

	if len(@CategoryIDs) > 0
				set @executeString = @executeString + ' and cct.CategoryCodeTypeID in (' + @CategoryIDs +')'
				
	if len(@CategoryCodes) > 0
				set @executeString = @executeString + ' and a.CategoryCodeID in (' + @CategoryCodes +')'
				
	if len(@TransactionCodes) > 0
				set @executeString = @executeString + ' and a.TransactionCodeID in (' + @TransactionCodes +')'
				
	if len(@TransactionIDs) > 0
				set @executeString = @executeString + ' and tct.TransactionCodeTypeID in (' + @TransactionIDs +')'
				
	if len(@QsourceIDs) > 0
				set @executeString = @executeString + ' and sp.QSourceID in (' + @QsourceIDs +')'
				
	if len(@StateIDs) > 0
				set @executeString = @executeString + ' and s.RegionID in (' + @StateIDs +')'
				
	if LEN(@Regions) > 0
		BEGIN
				INSERT INTO #Areas
				select * FROM dbo.fn_Split(@Regions, ',');
				set @executeString = @executeString + ' and s.CountryID in ( Select CountryID FROM UAS..Country WHERE Area IN ( select * from #areas ))'
		END
				
	if LEN(@CountryIDs) > 0
				set @executeString = @executeString + ' and s.CountryID in (' + @CountryIDs + ')'
				
	if len(@Email) > 0
		Begin
			set @Email = (CASE WHEN @Email='0' THEN 1 ELSE 0 END)
			if @Email = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Email)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Email)),'''') = ''''' 
		End

	if len(@Phone) > 0
		Begin
			set @Phone = (CASE WHEN @Phone='0' THEN 1 ELSE 0 END)
			if @Phone = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Phone)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Phone)),'''') = ''''' 
		End

	if len(@Fax) > 0
		Begin
			set @Fax = (CASE WHEN @Fax='0' THEN 1 ELSE 0 END)
			if @Fax = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Fax)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Fax)),'''') = ''''' 
		End
	
	if len(@startDate) > 0
			set @executeString = @executeString + ' and sp.QSourceDate >= ''' + @startDate + ''''

	if len(@endDate) > 0
			set @executeString = @executeString + ' and sp.QSourceDate <= ''' + @endDate + ''''	
				
	if len(@Year) > 0
		Begin
			insert into @years
			select * FROM dbo.fn_Split(@Year, ',');
			
			set @tempYear = (SELECT TOP 1 * FROM @years)
			
			select @tempStartDate = YearStartDate , @tempEndDate = YearEndDate from Publication where PublicationID = @PublicationID	
			
			DECLARE My_Cursor CURSOR FOR
			select value FROM ( Select y.value, row_number() over (order by value ASC) rn FROM @years y) src WHERE rn > 1

			if getdate() > convert(datetime,@tempStartDate + '/' + convert(varchar,year(getdate())))
				set @currentYear = year(getdate()) 
			else
				set @currentYear = year(getdate()) - 1

			set @executeString = @executeString + ' and (sp.QSourceDate >= ''' + @tempStartDate + '/' + Convert(varchar(10),(convert(int,@currentYear-@tempYear)))  + ''''
			set @executeString = @executeString + ' and sp.QSourceDate <= ''' + @tempEndDate + '/' +  Convert(varchar(10),(convert(int,@currentYear-@tempYear+1))) + ''''
			
			OPEN My_Cursor
			
			FETCH NEXT FROM My_Cursor
			INTO @tempYear
			WHILE @@FETCH_STATUS = 0
			BEGIN
				set @executeString = @executeString + ' or (sp.QSourceDate >= ''' + @tempStartDate + '/' + Convert(varchar(10),(convert(int,@currentYear-@tempYear)))  + ''''
				set @executeString = @executeString + ' and sp.QSourceDate <= ''' + @tempEndDate + '/' +  Convert(varchar(10),(convert(int,@currentYear-@tempYear+1))) + '''' + ')'
				FETCH NEXT FROM My_Cursor
				INTO @tempYear
			END
			CLOSE My_Cursor
			DEALLOCATE My_Cursor
			set @executeString = @executeString + ')'
		End
	
	if LEN(@Demo7) > 0
			set @executeString = @executeString + ' and sp.DeliverabilityID in (SELECT DeliverabilityID from Deliverability d with (NOLOCK) WHERE d.DeliverabilityID in (' + @Demo7 + '))'	
		
	if LEN(@ResponseIDs) > 0
		set @executeString = @executeString + ' and sp.SubscriptionID in (SELECT SubscriptionID from SubscriptionResponseMap srm with (NOLOCK) WHERE  srm.ResponseID in (' + @ResponseIDs +'))'
	
	--ADHOC PROCESSING--
	if LEN(@AdHocXML) > 0
		BEGIN
			declare @Column varchar(100),
						@Value varchar(100), 
						@ValueFrom varchar(100), 
						@ValueTo varchar(100), 
						@DataType varchar(100), 
						@Condition varchar(100)
				 
			DECLARE @NumberRecords int, @RowCount int, @AdHocString varchar(100), @AdHocFinal varchar(8000)
			set @NumberRecords = 0
			set @AdHocString = ''
			set @AdHocFinal = ''
				
			SELECT @NumberRecords = COUNT(*) from #Adhoc
				SET @RowCount = 1
				WHILE @RowCount <= @NumberRecords
				BEGIN
					 set @AdhocString = '';
					 SELECT @Column = FilterObject,
							@Value = Value,
							@ValueFrom = FromValue,
							@ValueTo = ToValue,
							@DataType = Type,
							@Condition = SelectedCondition
					FROM #AdHoc
					WHERE RowID = @RowCount
					
					 if(@DataType = 'Standard') 
						begin
							while(PATINDEX ('% ,%',@Value)>0 or PATINDEX ('%, %',@Value)>0 )
								set @Value = LTRIM(RTRIM(REPLACE(replace(@Value,' ,',','),', ',',')))
						 
							set @AdhocString =  
								CASE  @Condition
									WHEN 'Equal' THEN '( s.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or s.' + @Column + ' =  ''')+ ''') ' 
									WHEN 'Contains' THEN '( s.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' like  ''%')+ '%'') ' 
									WHEN 'Start With' THEN '( s.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' like  ''')+ '%'') '
									WHEN 'End With' THEN '( s.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or s.' + @Column + ' like  ''%')+ ''') '
									WHEN 'Does Not Contain' THEN '( s.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
								END 
						 end			 

					 if(@DataType = 'DateRange') 
						 begin
							 if(@Column = 'STARTISSUEDATE' or  @Column = 'EXPIREISSUEDATE' or @Column = 'PAIDDATE')
								 Begin
									 set @AdhocString = 
										 CASE  @Condition
												WHEN 'DateRange' THEN case when @ValueTo = null then 'spp.' + @Column + ' >= ''' + @ValueFrom + '''' else 'spp.' + @Column + ' >= ''' + @ValueFrom + ''' and spp.' + @Column + ' <= ''' + @ValueTo + ''''  END
												WHEN 'Year' THEN case when @ValueTo = null then 'year(spp.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(spp.' + @Column + ') >= ''' + @ValueFrom + ''' and year(spp.' + @Column + ') <= ''' + @ValueTo + ''''  END
												WHEN 'Month' THEN case when @ValueTo = null then 'month(spp.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(spp.' + @Column + ') >= ''' + @ValueFrom + ''' and month(spp.' + @Column + ') <= ''' + @ValueTo + ''''  END
										 END 
								 end	 
							 else	 
								 Begin
									 set @AdhocString = 
										 CASE  @Condition
												WHEN 'DateRange' THEN case when @ValueTo = null then 's.' + @Column + ' >= ''' + @ValueFrom + '''' else 's.' + @Column + ' >= ''' + @ValueFrom + ''' and s.' + @Column + ' <= ''' + @ValueTo + ''''  END
												WHEN 'Year' THEN case when @ValueTo = null then 'year(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(s.' + @Column + ') >= ''' + @ValueFrom + ''' and year(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
												WHEN 'Month' THEN case when @ValueTo = null then 'month(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(s.' + @Column + ') >= ''' + @ValueFrom + ''' and month(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
										 END 
								 end				 
						 end		
				 
					if(@DataType = 'Range') 
						begin
							 set @AdhocString = 
								 CASE  @Condition
										WHEN 'Range' THEN case when @ValueTo = null then 'year(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(s.' + @Column + ') >= ''' + @ValueFrom + ''' and year(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
										WHEN 'Equal' THEN 's.' + @Column + ' = ' + @Value 
										WHEN 'Greater Than' THEN 's.' + @Column + ' >= ' + @Value 
										WHEN 'Lesser Than' THEN 's.' + @Column + ' <= ' + @Value 
								 END 
						end	 
				 
				if @AdhocString != ''
					set @AdHocFinal = @AdHocFinal + ' and ' + @AdhocString
					
				SET @RowCount = @RowCount + 1
				
				END
				
			set @executeString = @executeString + @AdHocFinal
		END
			
END

PRINT(@executeString)
		
EXEC(@executeString)

DROP TABLE #AdHoc
DROP TABLE #Areas

END
GO

CREATE PROCEDURE [dbo].[e_Publication_Select_Client]
	@ClientID int
AS
	Select PUB.*
	from Publication PUB With(NoLock) join Publisher P With(NoLock) on PUB.PublisherID = P.PublisherID
	where P.ClientID = @ClientID
go

CREATE PROCEDURE [dbo].[o_CircProduct_Select]
	@ClientID int
AS
	Select PublicationID as ProductID, PublicationName as ProductName, PublicationCode as ProductCode, P.ClientID, PUB.IsActive, CAST('false' AS BIT) as IsUAD, CAST('true' AS BIT) as IsCirc, AllowDataEntry 
	from Publication PUB With(NoLock) join Publisher P With(NoLock) on PUB.PublisherID = P.PublisherID
	where P.ClientID = @ClientID
go

CREATE PROCEDURE [dbo].[e_Subscription_Select_ProductID_Count]
@ProductID int
AS
	Select COUNT(*) 
	FROM Subscription With(NoLock)
	WHERE PublicationID = @ProductID
	
GO

CREATE PROCEDURE [dbo].[e_Subscription_Select_ProductID_Paging]
@CurrentPage int,
@PageSize int,
@ProductID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY SubscriptionID) as 'RowNum', * 
	FROM Subscription With(NoLock)
	WHERE PublicationID = @ProductID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
	
GO

CREATE PROCEDURE [dbo].[e_Subscriber_Select_ProductID_Count]
@ProductID int
AS
	Select COUNT(*) 
	FROM Subscriber s With(NoLock)
	JOIN Subscription sp With(NoLock) ON s.SubscriberID = sp.SubscriberID
	WHERE sp.PublicationID = @ProductID
go

CREATE PROCEDURE [dbo].[e_Subscriber_Select_ProductID_Paging]
@CurrentPage int,
@PageSize int,
@ProductID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY s.SubscriberID) as 'RowNum', s.* 
	FROM Subscriber s With(NoLock)
	JOIN Subscription sp With(NoLock) ON s.SubscriberID = sp.SubscriberID 
	WHERE sp.PublicationID = @ProductID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
go

CREATE PROCEDURE [dbo].[e_IssueArchiveSubscriber_Select_Count]
@IssueID int
AS
	Select COUNT(*) 
	FROM IssueArchiveSubscriber s With(NoLock)
	WHERE s.IssueId = @IssueID
go

CREATE PROCEDURE [dbo].[e_IssueArchiveSubscriber_Select_Paging]
@CurrentPage int,
@PageSize int,
@IssueID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY s.IssueArchiveSubscriberID) as 'RowNum', s.* 
	FROM IssueArchiveSubscriber s With(NoLock)
	WHERE s.IssueId = @IssueID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
go

CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_Select_Count]
@IssueID int
AS
	Select COUNT(*) 
	FROM IssueArchiveSubscription sp With(NoLock)
	JOIN IssueArchiveSubscriber s With(NoLock) ON s.IssueArchiveSubscriberId = sp.IssueArchiveSubscriberId
	WHERE s.IssueId = @IssueID
go

CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_Select_Paging]
@CurrentPage int,
@PageSize int,
@IssueID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY s.IssueArchiveSubscriptionID) as 'RowNum', s.* FROM
	IssueArchiveSubscription s With(NoLock)
	JOIN IssueArchiveSubscriber ias With(NoLock) ON ias.IssueArchiveSubscriberId = s.IssueArchiveSubscriberId
	WHERE ias.IssueId = @IssueID
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
go

CREATE TABLE [dbo].[ActionBackUp]
(
	[ActionBackUpID] INT IDENTITY (1, 1) NOT NULL, 
    [ProductID] INT NOT NULL, 
    [SubscriptionID] INT NOT NULL, 
    [ActionID_Current] INT NOT NULL, 
    [ActionID_Previous] INT NULL
)
go

CREATE PROCEDURE [dbo].[e_ActionBackUp_Bulk_Insert]
	@ProductID int
AS
	DELETE FROM ActionBackUp
	WHERE ProductID = @ProductID

	INSERT INTO ActionBackUp
	SELECT PublicationID, SubscriptionID, ActionID_Current, ActionID_Previous
	FROM Subscription s With(NoLock)
	WHERE s.PublicationID = @ProductID
go

CREATE PROCEDURE [dbo].[e_ActionBackUp_Restore]
	@ProductID int
AS
	UPDATE Subscription
	SET ActionID_Current = a.ActionID_Current, ActionID_Previous = a.ActionID_Previous
	FROM Subscription s
	INNER JOIN ActionBackUp a
	ON a.SubscriptionID = s.SubscriptionID
	WHERE s.PublicationID = @ProductID
go

CREATE PROCEDURE [dbo].[e_IssueCompError_Select]
AS
	SELECT * FROM IssueCompError With(NoLock)
go

CREATE PROCEDURE [dbo].[e_IssueCompError_Select_ProcessCode]
	@ProcessCode VARCHAR(50)
AS
	SELECT * FROM IssueCompError With(NoLock) WHERE ProcessCode = @ProcessCode
go

CREATE PROCEDURE [dbo].[e_IssueCompDetail_Select]
	@IssueCompID int
AS
	SELECT * FROM IssueCompDetail icd With(NoLock)
	JOIN IssueComp ic With(NoLock) ON ic.IssueCompId = icd.IssueCompID
	WHERE ic.IssueCompId = @IssueCompID
go

CREATE PROCEDURE e_IssueCompDetail_Save
@IssueCompDetailId int,
@IssueCompID int,
@ExternalKeyID int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar (255),
@Occupation varchar (50),
@AddressTypeID  int,
@Address1  varchar (100),
@Address2  varchar (100),
@Address3  varchar (100),
@City varchar (50),
@RegionCode varchar (50),
@RegionID  int,
@ZipCode   varchar (50),
@Plus4 varchar (10),
@CarrierRoute varchar (10),
@County varchar (50),
@Country varchar (50),
@CountryID int,
@Latitude decimal(18, 15),
@Longitude decimal(18, 15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource varchar(50),
@AddressValidationMessage varchar(max),
@Email varchar(255),
@Phone varchar(25),
@Fax varchar(25),
@Mobile varchar(25),
@Website varchar(255),
@Birthdate date,
@Age int,
@Income varchar(50),
@Gender varchar(50),
@IsLocked bit,
@PhoneExt varchar(25), 
@SequenceID int,
@PublisherID int,
@SubscriberID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate date,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256),
@Copies int,
@OriginalSubscriberSourceCode varchar(256),
@Par3cID int,
@SubsrcTypeID int,
@AccountNumber varchar(50),
@OnBehalfOf varchar(256), 
@MemberGroup varchar(256), 
@Verify varchar(256), 
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueCompDetailId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueCompDetail
		SET IssueCompID = @IssueCompID,
			ExternalKeyID = @ExternalKeyID,
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
			Occupation = @Occupation,
			AddressTypeID = @AddressTypeID,
			Address1 = @Address1,
			Address2 = @Address2,
			Address3 = @Address3,
			City = @City,
			RegionCode = @RegionCode,
			RegionID = @RegionID,
			ZipCode = @ZipCode,
			Plus4 = @Plus4,
			CarrierRoute = @CarrierRoute,
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Latitude = @Latitude,
			Longitude = @Longitude,
			IsAddressValidated = @IsAddressValidated,
			AddressValidationDate = @AddressValidationDate,
			AddressValidationSource = @AddressValidationSource,
			AddressValidationMessage = @AddressValidationMessage,
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			Birthdate = @Birthdate,
			Age = @Age,
			Income = @Income,
			Gender = @Gender,
			IsLocked = @IsLocked,
			PhoneExt = @PhoneExt, 
			SequenceID = @SequenceID,
			PublisherID = @PublisherID,
			SubscriberID = @SubscriberID,
			PublicationID = @PublicationID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			SubscriptionStatusID = @SubscriptionStatusID,
			IsPaid = @IsPaid,
			QSourceID = @QSourceID,
			QSourceDate = @QSourceDate,
			DeliverabilityID = @DeliverabilityID,
			IsSubscribed = @IsSubscribed,
			SubscriberSourceCode = @SubscriberSourceCode,
			Copies = @Copies,
			OriginalSubscriberSourceCode = @OriginalSubscriberSourceCode,
			Par3cID = @Par3cID,
			SubsrcTypeID = @SubsrcTypeID,
			AccountNumber = @AccountNumber,
			OnBehalfOf = @OnBehalfOf, 
			MemberGroup = @MemberGroup, 
			Verify = @Verify, 
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueCompDetailId = @IssueCompDetailId;

		SELECT @IssueCompDetailId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueCompDetail (IssueCompID,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,
									 County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
									 Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,SequenceID,PublisherID,SubscriberID,PublicationID,
									 ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,
									 Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify,DateCreated,CreatedByUserID)
		VALUES(@IssueCompID,@ExternalKeyID,@FirstName,@LastName,@Company,@Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,
			   @County,@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,
			   @Email,@Phone,@Fax,@Mobile,@Website,@Birthdate,@Age,@Income,@Gender,@IsLocked,@PhoneExt,@SequenceID,@PublisherID,@SubscriberID,@PublicationID,
			   @ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,
			   @IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@Par3cID,@SubsrcTypeID,@AccountNumber,
			   @OnBehalfOf,@MemberGroup,@Verify,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO

CREATE Proc [dbo].[e_IssueCompDetail_GetFromFilter]
(  
	--@Test int
	@IssueCompID int,
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(800),
	@Email varchar(10),
	@Phone varchar(10),
	@Fax varchar(10),
	@Demo7 varchar(10),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000)
) AS

BEGIN

DECLARE	@executeString varchar(8000)
DECLARE @currentYear int
DECLARE @tempStartDate varchar(10)
DECLARE @tempEndDate varchar(10)
DECLARE @years table (value varchar(4))
DECLARE @tempYear varchar(20)

CREATE TABLE #Areas
(
	[Value] varchar(40)
)

CREATE TABLE #AdHoc
(  
	RowID int IDENTITY(1, 1)
  ,[FilterObject] nvarchar(256)
  ,[SelectedCondition] nvarchar(256)
  ,[Type] nvarchar(256)
  ,[Value] nvarchar(256)
  ,[ToValue] nvarchar(256)
  ,[FromValue] nvarchar(256)
)

DECLARE @docHandle int
--DECLARE @AdHocXML varchar(8000)

--set @AdHocXML = '<XML><AdHocFilters.AdHocFilterField><FilterObject>FirstName</FilterObject><FromValue></FromValue><SelectedCondition>CONTAINS</SelectedCondition><ToValue></ToValue><Type>Standard</Type><Value>Gabriel</Value></AdHocFilters.AdHocFilterField>\r\n<AdHocFilters.AdHocFilterField  ><FilterObject>LastName</FilterObject><FromValue></FromValue><SelectedCondition>CONTAINS</SelectedCondition><ToValue></ToValue><Type>Standard</Type><Value>Santiago</Value></AdHocFilters.AdHocFilterField>\r\n<AdHocFilters.AdHocFilterField><FilterObject>StartIssueDate</FilterObject><FromValue>11/01/2011</FromValue><SelectedCondition>DateRange</SelectedCondition><ToValue>11/01/2014</ToValue><Type>DateRange</Type><Value></Value></AdHocFilters.AdHocFilterField></XML>'

EXEC sp_xml_preparedocument @docHandle OUTPUT, @AdHocXML  
INSERT INTO #AdHoc 
(
	 [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
)  
SELECT [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
FROM OPENXML(@docHandle,N'/XML/FilterDetail')
WITH
(
	[FilterObject] nvarchar(256) 'FilterField',
	[SelectedCondition] nvarchar(256) 'SearchCondition',
	[Type] nvarchar(256) 'FilterObjectType',
	[Value] nvarchar(265) 'AdHocFieldValue',
	[ToValue] nvarchar(256) 'AdHocToField',
	[FromValue] nvarchar(256) 'AdHocFromField'
)

EXEC sp_xml_removedocument @docHandle
	
--DECLARE @PublicationID int
--DECLARE @CategoryIDs varchar(800)
--DECLARE @CategoryCodes varchar(800)
--DECLARE @TransactionIDs varchar(800)
--DECLARE @TransactionCodes varchar(800)
--DECLARE @QsourceIDs varchar(800)
--DECLARE @StateIDs varchar(800)
--DECLARE @Regions varchar(max)
--DECLARE @CountryIDs varchar(800)
--DECLARE @Email varchar(10)
--DECLARE @Phone varchar(10)
--DECLARE @Fax varchar(10)
--DECLARE @Year varchar(800)
--DECLARE @startDate varchar(10)	
--DECLARE @endDate varchar(10)
--DECLARE @ResponseIDs varchar(800)
--DECLARE @DEMO7 varchar(800)
--DECLARE @GetSubscriberIDs bit
--set @PublicationID = 113
--set @TransactionCodes = '9'
--set @GetSubscriberIDs = 0
----set @Regions = 'America Central, Asia Pacific'
----set @CountryIDs = '1,2'
----set @CategoryIDs = '1,3'
----set @CategoryCodes = '1,2,3,4,5,6'
----set @TransactionCodes = '1,2,3,4,5,6,7,10,11,12'
----set @TransactionIDs = '1,3'
----set @QsourceIDs = '1,3,4'
----set @StateIDs = '1,2,3,5,6'
----set @Email = 'Yes'
----set @Phone = 'Yes'
----set @Fax = 'No'
----set @Year = '1,2,3,4,5'
----set @ResponseIDs = '35819'

set @executeString =
'Select	* FROM
		IssueCompDetail s JOIN
		Action a ON a.ActionID = s.ActionID_Current JOIN
		CategoryCode dc ON a.CategoryCodeID = dc.CategoryCodeID JOIN
		TransactionCode dt on a.TransactionCodeID = dt.TransactionCodeID JOIN
		TransactionCodeType tct ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
		CategoryCodeType cct ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
		QualificationSource dq on s.QSourceID = dq.QSourceID LEFT OUTER JOIN
		UAS..Country c ON c.CountryID = s.CountryID
Where	
		s.IssueCompID = ' + Convert(varchar,@IssueCompID) 
		
Begin --Check passed Parameters
				
	if len(@CategoryIDs) > 0
				set @executeString = @executeString + ' and cct.CategoryCodeTypeID in (' + @CategoryIDs +')'
				
	if len(@CategoryCodes) > 0
				set @executeString = @executeString + ' and a.CategoryCodeID in (' + @CategoryCodes +')'
				
	if len(@TransactionCodes) > 0
				set @executeString = @executeString + ' and a.TransactionCodeID in (' + @TransactionCodes +')'
				
	if len(@TransactionIDs) > 0
				set @executeString = @executeString + ' and tct.TransactionCodeTypeID in (' + @TransactionIDs +')'
				
	if len(@QsourceIDs) > 0
				set @executeString = @executeString + ' and s.QSourceID in (' + @QsourceIDs +')'
				
	if len(@StateIDs) > 0
				set @executeString = @executeString + ' and s.RegionID in (' + @StateIDs +')'
				
	if LEN(@Regions) > 0
		BEGIN
				INSERT INTO #Areas
				select * FROM dbo.fn_Split(@Regions, ',');
				set @executeString = @executeString + ' and s.CountryID in ( Select CountryID FROM UAS..Country WHERE Area IN ( select * from #areas ))'
		END
				
	if LEN(@CountryIDs) > 0
				set @executeString = @executeString + ' and s.CountryID in (' + @CountryIDs + ')'
				
	if len(@Email) > 0
		Begin
			set @Email = (CASE WHEN @Email='0' THEN 1 ELSE 0 END)
			if @Email = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Email)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Email)),'''') = ''''' 
		End

	if len(@Phone) > 0
		Begin
			set @Phone = (CASE WHEN @Phone='0' THEN 1 ELSE 0 END)
			if @Phone = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Phone)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Phone)),'''') = ''''' 
		End

	if len(@Fax) > 0
		Begin
			set @Fax = (CASE WHEN @Fax='0' THEN 1 ELSE 0 END)
			if @Fax = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Fax)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Fax)),'''') = ''''' 
		End
	
	if len(@startDate) > 0
			set @executeString = @executeString + ' and s.QSourceDate >= ''' + @startDate + ''''

	if len(@endDate) > 0
			set @executeString = @executeString + ' and s.QSourceDate <= ''' + @endDate + ''''	
				
	if len(@Year) > 0
		Begin
			insert into @years
			select * FROM dbo.fn_Split(@Year, ',');
			
			set @tempYear = (SELECT TOP 1 * FROM @years)
			
			select @tempStartDate = YearStartDate , @tempEndDate = YearEndDate from Publication where PublicationID = @PublicationID	
			
			DECLARE My_Cursor CURSOR FOR
			select value FROM ( Select y.value, row_number() over (order by value ASC) rn FROM @years y) src WHERE rn > 1

			if getdate() > convert(datetime,@tempStartDate + '/' + convert(varchar,year(getdate())))
				set @currentYear = year(getdate()) 
			else
				set @currentYear = year(getdate()) - 1

			set @executeString = @executeString + ' and (s.QSourceDate >= ''' + @tempStartDate + '/' + Convert(varchar(10),(convert(int,@currentYear-@tempYear)))  + ''''
			set @executeString = @executeString + ' and s.QSourceDate <= ''' + @tempEndDate + '/' +  Convert(varchar(10),(convert(int,@currentYear-@tempYear+1))) + ''''
			
			OPEN My_Cursor
			
			FETCH NEXT FROM My_Cursor
			INTO @tempYear
			WHILE @@FETCH_STATUS = 0
			BEGIN
				set @executeString = @executeString + ' or (s.QSourceDate >= ''' + @tempStartDate + '/' + Convert(varchar(10),(convert(int,@currentYear-@tempYear)))  + ''''
				set @executeString = @executeString + ' and s.QSourceDate <= ''' + @tempEndDate + '/' +  Convert(varchar(10),(convert(int,@currentYear-@tempYear+1))) + '''' + ')'
				FETCH NEXT FROM My_Cursor
				INTO @tempYear
			END
			CLOSE My_Cursor
			DEALLOCATE My_Cursor
			set @executeString = @executeString + ')'
		End
	
	if LEN(@Demo7) > 0
			set @executeString = @executeString + ' and s.DeliverabilityID in (SELECT DeliverabilityID from Deliverability d with (NOLOCK) WHERE d.DeliverabilityID in (' + @Demo7 + '))'	
	
	--ADHOC PROCESSING--
	if LEN(@AdHocXML) > 0
		BEGIN
			declare @Column varchar(100),
						@Value varchar(100), 
						@ValueFrom varchar(100), 
						@ValueTo varchar(100), 
						@DataType varchar(100), 
						@Condition varchar(100)
				 
			DECLARE @NumberRecords int, @RowCount int, @AdHocString varchar(100), @AdHocFinal varchar(8000)
			set @NumberRecords = 0
			set @AdHocString = ''
			set @AdHocFinal = ''
				
			SELECT @NumberRecords = COUNT(*) from #Adhoc
				SET @RowCount = 1
				WHILE @RowCount <= @NumberRecords
				BEGIN
					 set @AdhocString = '';
					 SELECT @Column = FilterObject,
							@Value = Value,
							@ValueFrom = FromValue,
							@ValueTo = ToValue,
							@DataType = Type,
							@Condition = SelectedCondition
					FROM #AdHoc
					WHERE RowID = @RowCount
					
					if(@DataType = 'Standard') 
						begin
							while(PATINDEX ('% ,%',@Value)>0 or PATINDEX ('%, %',@Value)>0 )
								set @Value = LTRIM(RTRIM(REPLACE(replace(@Value,' ,',','),', ',',')))
						 
							set @AdhocString =  
								CASE  @Condition
									WHEN 'Equal' THEN '( s.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or s.' + @Column + ' =  ''')+ ''') ' 
									WHEN 'Contains' THEN '( s.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' like  ''%')+ '%'') ' 
									WHEN 'Start With' THEN '( s.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' like  ''')+ '%'') '
									WHEN 'End With' THEN '( s.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or s.' + @Column + ' like  ''%')+ ''') '
									WHEN 'Does Not Contain' THEN '( s.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
								END 
						 end			 

					if(@DataType = 'DateRange') 
						begin 
								 Begin
									 set @AdhocString = 
										 CASE  @Condition
												WHEN 'DateRange' THEN case when @ValueTo = null then 's.' + @Column + ' >= ''' + @ValueFrom + '''' else 's.' + @Column + ' >= ''' + @ValueFrom + ''' and s.' + @Column + ' <= ''' + @ValueTo + ''''  END
												WHEN 'Year' THEN case when @ValueTo = null then 'year(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(s.' + @Column + ') >= ''' + @ValueFrom + ''' and year(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
												WHEN 'Month' THEN case when @ValueTo = null then 'month(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(s.' + @Column + ') >= ''' + @ValueFrom + ''' and month(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
										 END 
								 end				 
						 end		
				 
					if(@DataType = 'Range') 
						begin
							 set @AdhocString = 
								 CASE  @Condition
										WHEN 'Range' THEN case when @ValueTo = null then 'year(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(s.' + @Column + ') >= ''' + @ValueFrom + ''' and year(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
										WHEN 'Equal' THEN 's.' + @Column + ' = ' + @Value 
										WHEN 'Greater Than' THEN 's.' + @Column + ' >= ' + @Value 
										WHEN 'Lesser Than' THEN 's.' + @Column + ' <= ' + @Value 
								 END 
						end	 
				 
				if @AdhocString != ''
					set @AdHocFinal = @AdHocFinal + ' and ' + @AdhocString
					
				SET @RowCount = @RowCount + 1
				
				END
				
			set @executeString = @executeString + @AdHocFinal
		END
			
END

PRINT(@executeString)
		
EXEC(@executeString)

DROP TABLE #AdHoc
DROP TABLE #Areas

END
go

CREATE PROCEDURE [dbo].[e_IssueCompDetail_Clear]
@IssueCompID int
AS
DELETE FROM IssueCompDetail
WHERE IssueCompID = @IssueCompID
go

CREATE PROCEDURE [dbo].[e_IssueComp_Select_Issue]
	@IssueID int
AS
	SELECT * FROM IssueComp With(NoLock)
	WHERE IssueId = @IssueID
go

































PRINT N'Refreshing [dbo].[e_Batch_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Batch_Select]';


GO
PRINT N'Refreshing [dbo].[e_Batch_Select_UserID_IsActive]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Batch_Select_UserID_IsActive]';


GO
PRINT N'Refreshing [dbo].[e_History_Select_Active_User_BatchID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_History_Select_Active_User_BatchID]';


GO
PRINT N'Refreshing [dbo].[e_HistoryPaidBillTo_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_HistoryPaidBillTo_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_HistoryPaidBillTo_Select_SubscriptionPaidID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_HistoryPaidBillTo_Select_SubscriptionPaidID]';


GO
PRINT N'Refreshing [dbo].[e_HistorySubscription_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_HistorySubscription_Select]';


GO
PRINT N'Refreshing [dbo].[e_PaidBillTo_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_PaidBillTo_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_PaidBillTo_Select_SubscriptionPaidID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_PaidBillTo_Select_SubscriptionPaidID]';


GO
PRINT N'Refreshing [dbo].[o_ExportData_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ExportData_Select]';


GO
PRINT N'Refreshing [dbo].[o_ExportData_Select_Publisher_Publication]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ExportData_Select_Publisher_Publication]';


GO
PRINT N'Refreshing [dbo].[e_PriceCode_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_PriceCode_Select]';


GO
PRINT N'Refreshing [dbo].[e_PriceCode_Select_PublicationID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_PriceCode_Select_PublicationID]';


GO
PRINT N'Refreshing [dbo].[e_Publication_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Publication_Select]';


GO
PRINT N'Refreshing [dbo].[e_Publication_Select_PublicationID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Publication_Select_PublicationID]';


GO
PRINT N'Refreshing [dbo].[e_Publication_Select_PublisherID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Publication_Select_PublisherID]';


GO
PRINT N'Refreshing [dbo].[e_Publication_Select_Subscriber]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Publication_Select_Subscriber]';


GO
PRINT N'Refreshing [dbo].[e_PublicationSequence_Select_PublisherID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_PublicationSequence_Select_PublisherID]';


GO
PRINT N'Refreshing [dbo].[e_SearchResult_Select_IndividualID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SearchResult_Select_IndividualID]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Select]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Select_Prospect_PublicationID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Select_Prospect_PublicationID]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Select_PublicationID_IsProspect]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Select_PublicationID_IsProspect]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Select_PublicationID_IsSubscribed]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Select_PublicationID_IsSubscribed]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Select_PublicationID_IsSubscribed_IsProspect]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Select_PublicationID_IsSubscribed_IsProspect]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Select_PublisherID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Select_PublisherID]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Select_SubscriberID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Select_SubscriberID]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_SelectPublication]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_SelectPublication]';


GO
PRINT N'Refreshing [dbo].[sp_getSubscribers_using_XMLFilters]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getSubscribers_using_XMLFilters]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_by_country]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_by_country]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_ByCountry_ByState]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_ByCountry_ByState]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_Canada]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_Canada]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_domestic]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_domestic]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_SubsrcReport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_SubsrcReport]';


GO
PRINT N'Refreshing [dbo].[sp_rptCategorySummary]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rptCategorySummary]';


GO
PRINT N'Refreshing [dbo].[sp_rptCrossTabwithQualBreakdown]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rptCrossTabwithQualBreakdown]';


GO
PRINT N'Refreshing [dbo].[sp_rptPAR3C]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rptPAR3C]';


GO
PRINT N'Refreshing [dbo].[sp_rptSubFields]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rptSubFields]';


GO
PRINT N'Refreshing [dbo].[sp_rptViewResponseTotals]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rptViewResponseTotals]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscribers]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscribers]';


GO
PRINT N'Refreshing [dbo].[o_ExportData_UpdateResponse]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ExportData_UpdateResponse]';


GO
PRINT N'Refreshing [dbo].[e_Response_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Response_Select]';


GO
PRINT N'Refreshing [dbo].[e_Response_Select_PublicationID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Response_Select_PublicationID]';


GO
PRINT N'Refreshing [dbo].[o_ExportData_UpdateResponseOther]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ExportData_UpdateResponseOther]';


GO
PRINT N'Refreshing [dbo].[sp_GetColumnLegend]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetColumnLegend]';


GO
PRINT N'Refreshing [dbo].[e_Subscriber_Delete_SubscriberByID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriber_Delete_SubscriberByID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_PublicationID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_PublicationID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_SequenceID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_SequenceID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_SubscriberID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_SubscriberID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_SubscriberID_PublicationID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_SubscriberID_PublicationID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Update_QDate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Update_QDate]';


GO
PRINT N'Refreshing [dbo].[e_SubscriptionResponseMap_Select_SubscriberID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SubscriptionResponseMap_Select_SubscriberID]';


GO
PRINT N'Refreshing [dbo].[e_SubscriptionPaid_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SubscriptionPaid_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_DeliverSubscriptionPaid_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_DeliverSubscriptionPaid_Save]';


GO
PRINT N'Refreshing [dbo].[sp_getFilterValues]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getFilterValues]';


GO
PRINT N'Refreshing [dbo].[sp_rptQualificationBreakdown]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rptQualificationBreakdown]';


GO
PRINT N'Refreshing [dbo].[o_ExportData_RunImportSave]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ExportData_RunImportSave]';


GO
PRINT N'Refreshing [dbo].[sp_crosstab]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_crosstab]';


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6d248142-af49-4d2b-b3a6-e13e64babf2f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6d248142-af49-4d2b-b3a6-e13e64babf2f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '72ba3f6f-8648-4185-bc5a-df517a5f4e84')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('72ba3f6f-8648-4185-bc5a-df517a5f4e84')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4cab03aa-3245-4432-abea-3abca15a431f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4cab03aa-3245-4432-abea-3abca15a431f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'd2c2db74-c25e-412f-ae2d-bcf107c7b8e2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('d2c2db74-c25e-412f-ae2d-bcf107c7b8e2')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c7667e3f-7a2c-4616-8f28-0a3fdcea64b9')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c7667e3f-7a2c-4616-8f28-0a3fdcea64b9')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c8d95739-5879-4671-89db-3f8382537ff8')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c8d95739-5879-4671-89db-3f8382537ff8')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '137d877f-8d59-4bb9-a4fd-276ab251bc41')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('137d877f-8d59-4bb9-a4fd-276ab251bc41')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8e3b3f7f-cb5c-4d5f-bb6f-77d9aaa28515')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8e3b3f7f-cb5c-4d5f-bb6f-77d9aaa28515')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '215f394a-1bcd-4cd0-9820-f34ccb33aae5')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('215f394a-1bcd-4cd0-9820-f34ccb33aae5')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '9adfd068-e51c-440d-a6bc-760a6b956248')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('9adfd068-e51c-440d-a6bc-760a6b956248')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '3e08d834-9beb-459e-ad8b-6e4b3c8a7085')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('3e08d834-9beb-459e-ad8b-6e4b3c8a7085')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6753563b-be56-480b-bc21-f1f0998a2f06')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6753563b-be56-480b-bc21-f1f0998a2f06')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '30edf91b-8731-4e9b-9579-7fd009b747db')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('30edf91b-8731-4e9b-9579-7fd009b747db')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '24b680ac-0421-4e0b-86bf-0f26fe09bedf')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('24b680ac-0421-4e0b-86bf-0f26fe09bedf')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'ded35f1c-5747-42cd-bd4b-5f97eb01101d')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('ded35f1c-5747-42cd-bd4b-5f97eb01101d')

GO

GO
PRINT N'Checking existing data against newly created constraints';
GO
ALTER TABLE [dbo].[SubscriberAddKillDetail] WITH CHECK CHECK CONSTRAINT [FK_SubscriberAddKillDetail_SubscriberAddKill];
ALTER TABLE [dbo].[SubscriberAddKillDetail] WITH CHECK CHECK CONSTRAINT [FK_SubscriberAddKillDetail_Subscription];


GO
PRINT N'Update complete.';


GO

alter PROCEDURE e_Publication_Save
@PublicationID int,
@PublicationName varchar(50),
@PublicationCode varchar(50),
@PublisherID int,
@YearStartDate char(5),
@YearEndDate char(5),
@IssueDate date,
@IsImported bit,
@IsActive bit,
@AllowDataEntry bit,
@FrequencyID int,
@KMImportAllowed bit,
@ClientImportAllowed bit,
@AddRemoveAllowed bit,
@AcsMailerInfoId int,
@IsOpenCloseLocked int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @PublicationID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Publication
		SET 
			PublicationName = @PublicationName,
			YearStartDate= @YearStartDate,
			YearEndDate = @YearEndDate,
			IssueDate = @IssueDate,
			IsImported = @IsImported,
			IsActive = @IsActive,
			AllowDataEntry = @AllowDataEntry,
			FrequencyID = @FrequencyID,
			KMImportAllowed = @KMImportAllowed, 
			ClientImportAllowed = @ClientImportAllowed, 
			AddRemoveAllowed = @AddRemoveAllowed,
			AcsMailerInfoId = @AcsMailerInfoId,
			IsOpenCloseLocked = @IsOpenCloseLocked,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PublicationID = @PublicationID;
		
		SELECT @PublicationID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Publication (PublicationName,PublicationCode,PublisherID,YearStartDate,YearEndDate,IssueDate,IsImported,IsActive,AllowDataEntry,FrequencyID,KMImportAllowed, ClientImportAllowed, AddRemoveAllowed,AcsMailerInfoId,IsOpenCloseLocked,DateCreated,CreatedByUserID)
		VALUES(@PublicationName,@PublicationCode,@PublisherID,@YearStartDate,@YearEndDate,@IssueDate,@IsImported,@IsActive,@AllowDataEntry,@FrequencyID,@KMImportAllowed,@ClientImportAllowed,@AddRemoveAllowed,@AcsMailerInfoId,@IsOpenCloseLocked,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
go

alter PROCEDURE [dbo].[o_FinalizeBatch_Select]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,b.PublicationID as ProductID,p.PublicationName,Max(h.BatchCountItem) as LastCount, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN Publication p ON b.PublicationID = p.PublicationID
		JOIN [Publisher] pp With(NoLock) ON p.PublisherID = pp.PublisherID
        JOIN UAS..Client pub with(nolock) on pp.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		LEFT JOIN UAS..CodeType ct ON ct.CodeTypeName = 'User Log'
		LEFT JOIN UAS..Code c ON c.CodeTypeId = ct.CodeTypeDescription 
		WHERE b.IsActive = 1 and b.UserID = @UserID
	Group By b.BatchID, pub.ClientID,pub.ClientName, b.PublicationID, p.PublicationName, b.DateFinalized,b.BatchNumber
go

alter PROCEDURE [dbo].[o_FinalizeBatch_SelectAll]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,b.PublicationID as ProductID,p.PublicationName,Max(h.BatchCountItem) as LastCount, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN Publication p ON b.PublicationID = p.PublicationID
		JOIN [Publisher] pp With(NoLock) ON p.PublisherID = pp.PublisherID
        JOIN UAS..Client pub with(nolock) on pp.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		LEFT JOIN UAS..CodeType ct ON ct.CodeTypeName = 'User Log'
		LEFT JOIN UAS..Code c ON c.CodeTypeId = ct.CodeTypeDescription 
		WHERE b.UserID = @UserID
	Group By b.BatchID, pub.ClientID,pub.ClientName, b.PublicationID, p.PublicationName, b.DateFinalized,b.BatchNumber
go

alter PROCEDURE [dbo].[o_FinalizeBatch_SelectAll_NoUser]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,b.PublicationID as ProductID,p.PublicationName, p.PublicationCode, Max(h.BatchCountItem) as LastCount, u.FirstName + ' ' + u.LastName as UserName, b.DateCreated, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN Publication p ON b.PublicationID = p.PublicationID
		JOIN [Publisher] pp With(NoLock) ON p.PublisherID = pp.PublisherID
        JOIN UAS..Client pub with(nolock) on pp.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		JOIN UAS..[User] u With(NoLock) ON u.UserID = b.UserID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		LEFT JOIN UAS..CodeType ct ON ct.CodeTypeName = 'User Log'
		LEFT JOIN UAS..Code c ON c.CodeTypeId = ct.CodeTypeDescription 
	Group By b.BatchID, pub.ClientID, pub.ClientName, b.PublicationID, p.PublicationName, p.PublicationCode, u.FirstName, u.LastName, b.DateCreated, b.DateFinalized,b.BatchNumber
go

alter PROCEDURE [dbo].[o_FinalizeBatch_SelectAll_UserName]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,b.PublicationID as ProductID,p.PublicationName, p.PublicationCode, Max(h.BatchCountItem) as LastCount, u.FirstName + ' ' + u.LastName as UserName, b.DateCreated, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN Publication p ON b.PublicationID = p.PublicationID
		JOIN [Publisher] pp With(NoLock) ON p.PublisherID = pp.PublisherID
        JOIN UAS..Client pub with(nolock) on pp.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		JOIN UAS..[User] u With(NoLock) ON u.UserID = b.UserID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		LEFT JOIN UAS..CodeType ct ON ct.CodeTypeName = 'User Log'
		LEFT JOIN UAS..Code c ON c.CodeTypeId = ct.CodeTypeDescription 
		WHERE b.UserID = @UserID
	Group By b.BatchID, pub.ClientID, pub.ClientName, b.PublicationID, p.PublicationName, p.PublicationCode, u.FirstName, u.LastName, b.DateCreated, b.DateFinalized,b.BatchNumber

go

alter PROCEDURE e_IssueComp_Save
@IssueCompId int,
@IssueId int,
@ImportedDate datetime,
@IssueCompCount int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@IsActive bit
AS
IF @IssueCompId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueComp
		SET IssueId = @IssueId,
			ImportedDate = @ImportedDate,
			IssueCompCount = @IssueCompCount,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			IsActive = @IsActive
		WHERE IssueCompId = @IssueCompId;

		SELECT @IssueCompId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueComp (IssueCompId,IssueId,ImportedDate,IssueCompCount,DateCreated,CreatedByUserID,IsActive)
		VALUES(@IssueCompId,@IssueId,@ImportedDate,@IssueCompCount,@DateCreated,@CreatedByUserID,@IsActive);SELECT @@IDENTITY;
	END
GO




