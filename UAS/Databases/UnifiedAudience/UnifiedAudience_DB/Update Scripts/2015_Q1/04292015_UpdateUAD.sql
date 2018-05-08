/*
Deployment script for UAD

BACKUP DATABASE AdvanstarDeployTest to DISK = '\\10.10.41.250\Backups\MAF\AdvanstarDeployTest.bak' WITH COMPRESSION, INIT

use master
go
RESTORE DATABASE AdvanstarDeployTest FROM  DISK = '\\10.10.41.250\Backups\MAF\AdvanstarDeployTest.bak' WITH REPLACE, RECOVERY

*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;
GO

USE AdvanstarDeployTest;


GO
/*
The type for column LatLonMsg in table [dbo].[Subscriptions] is currently  NVARCHAR (500) NULL but is being changed to  VARCHAR (1000) NULL. Data loss could occur.
*/

--IF EXISTS (select top 1 1 from [dbo].[Subscriptions])  RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
PRINT N'Rename refactoring operation with key f6dec2be-2b90-4429-be07-ff4cc1f4b424 is skipped, element [dbo].[Pubs].[AcsMailerId] (SqlSimpleColumn) will not be renamed to AcsMailerInfoId';


GO
PRINT N'Dropping FK_PubSubscriptions_Pubs...';


GO
ALTER TABLE [dbo].[PubSubscriptions] DROP CONSTRAINT [FK_PubSubscriptions_Pubs];


GO
PRINT N'Dropping FK_PubMarkets_Pubs...';


GO
ALTER TABLE [dbo].[PubMarkets] DROP CONSTRAINT [FK_PubMarkets_Pubs];


GO
PRINT N'Dropping FK_PubGroups_Pubs...';


GO
ALTER TABLE [dbo].[PubGroups] DROP CONSTRAINT [FK_PubGroups_Pubs];


GO
PRINT N'Dropping FK_ResponseGroups_Pubs...';


GO
ALTER TABLE [dbo].[ResponseGroups] DROP CONSTRAINT [FK_ResponseGroups_Pubs];


GO
PRINT N'Dropping [dbo].[e_Subscription_Save_From_APIIncoming]...';


GO
DROP PROCEDURE [dbo].[e_Subscription_Save_From_APIIncoming];


GO
PRINT N'Altering [dbo].[Adhoc]...';


GO
ALTER TABLE [dbo].[Adhoc]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[AdhocCategory]...';


GO
ALTER TABLE [dbo].[AdhocCategory]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[Blast]...';


GO
ALTER TABLE [dbo].[Blast]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[CodeSheet]...';


GO
ALTER TABLE [dbo].[CodeSheet]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL,
        [DisplayOrder]    INT      NULL,
        [ReportGroupID]   INT      NULL,
        [IsActive]        BIT      NULL,
        [WQT_ResponseID]  INT      NULL,
        [IsOther]         BIT      NULL;


GO
PRINT N'Altering [dbo].[DomainTracking]...';


GO
ALTER TABLE [dbo].[DomainTracking]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO

PRINT N'ALTERING table [dbo].[ImportError]...';
ALTER TABLE ImportError ALTER COLUMN FormattedException VARCHAR(MAX)
ALTER TABLE ImportError ALTER COLUMN BadDataRow VARCHAR(MAX)

Declare @ImportErrorPK varchar(100)
select @ImportErrorPK = Name From sysobjects where name like 'pk%' and Parent_obj = OBJECT_ID('ImportError')

EXECUTE ('ALTER TABLE ImportError DROP CONSTRAINT ' +@ImportErrorPK)
ALTER TABLE ImportError ADD CONSTRAINT PK_ImportError PRIMARY KEY Clustered(ImportErrorID)


GO

GO
PRINT N'Altering [dbo].[Markets]...';


GO
ALTER TABLE [dbo].[Markets]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[Mastercodesheet]...';


GO
ALTER TABLE [dbo].[Mastercodesheet]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[MasterGroups]...';


GO
ALTER TABLE [dbo].[MasterGroups]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Starting rebuilding table [dbo].[Pubs]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Pubs] (
    [PubID]               INT           IDENTITY (1, 1) NOT NULL,
    [PubName]             VARCHAR (100) NOT NULL,
    [istradeshow]         BIT           NULL,
    [PubCode]             VARCHAR (50)  NULL,
    [PubTypeID]           INT           NULL,
    [GroupID]             INT           NULL,
    [EnableSearching]     BIT           NULL,
    [Score]               INT           NULL,
    [SortOrder]           INT           NULL,
    [DateCreated]         DATETIME      NULL,
    [DateUpdated]         DATETIME      NULL,
    [CreatedByUserID]     INT           NULL,
    [UpdatedByUserID]     INT           NULL,
    [ClientID]            INT           NULL,
    [YearStartDate]       VARCHAR (5)   NULL,
    [YearEndDate]         VARCHAR (5)   NULL,
    [IssueDate]           DATETIME      NULL,
    [IsImported]          BIT           NULL,
    [IsActive]            BIT           NULL,
    [AllowDataEntry]      BIT           NULL,
    [FrequencyID]         INT           NULL,
    [KMImportAllowed]     BIT           NULL,
    [ClientImportAllowed] BIT           NULL,
    [AddRemoveAllowed]    BIT           NULL,
    [AcsMailerInfoId]     INT           NULL,
    [IsUAD]               BIT           NULL,
    [IsCirc]              BIT           NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Pubs] PRIMARY KEY CLUSTERED ([PubID] ASC) WITH (FILLFACTOR = 90)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Pubs])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Pubs] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Pubs] ([PubID], [PubName], [PubCode], [PubTypeID], [GroupID], [istradeshow], [EnableSearching], [Score], [SortOrder])
        SELECT   [PubID],
                 [PubName],
                 [PubCode],
                 [PubTypeID],
                 [GroupID],
                 [istradeshow],
                 [EnableSearching],
                 [Score],
                 [SortOrder]
        FROM     [dbo].[Pubs]
        ORDER BY [PubID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Pubs] OFF;
    END

DROP TABLE [dbo].[Pubs];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Pubs]', N'Pubs';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Pubs]', N'PK_Pubs', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[Pubs].[IDX_Pubs_Pubcode]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_Pubs_Pubcode]
    ON [dbo].[Pubs]([PubCode] ASC) WITH (FILLFACTOR = 70);


GO
PRINT N'Altering [dbo].[PubSubscriptionDetail]...';


GO
ALTER TABLE [dbo].[PubSubscriptionDetail]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[PubSubscriptions]...';


GO
ALTER TABLE [dbo].[PubSubscriptions]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[PubTypes]...';


GO
ALTER TABLE [dbo].[PubTypes]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[QSource]...';


GO
ALTER TABLE [dbo].[QSource]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[QSourceGroup]...';


GO
ALTER TABLE [dbo].[QSourceGroup]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Altering [dbo].[ResponseGroups]...';


GO
ALTER TABLE [dbo].[ResponseGroups]
    ADD [DateCreated]         DATETIME NULL,
        [DateUpdated]         DATETIME NULL,
        [CreatedByUserID]     INT      NULL,
        [UpdatedByUserID]     INT      NULL,
        [DisplayOrder]        INT      NULL,
        [IsMultipleValue]     BIT      NULL,
        [IsRequired]          BIT      NULL,
        [IsActive]            BIT      NULL,
        [WQT_ResponseGroupID] INT      NULL,
        [ResponseGroupTypeId] INT      NULL;


GO
PRINT N'Altering [dbo].[Subscriptions]...';


GO
ALTER TABLE [dbo].[Subscriptions] ALTER COLUMN [LatLonMsg] VARCHAR (1000) NULL;


GO
ALTER TABLE [dbo].[Subscriptions]
    ADD [DateCreated]                    DATETIME NULL,
        [DateUpdated]                    DATETIME NULL,
        [CreatedByUserID]                INT      NULL,
        [UpdatedByUserID]                INT      NULL,
        [AddressTypeCodeId]              INT      NULL,
        [AddressLastUpdatedDate]         DATETIME NULL,
        [AddressUpdatedSourceTypeCodeId] INT      NULL;


GO
PRINT N'Altering [dbo].[SubscriptionsExtension]...';


GO
ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field1] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field10] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field100] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field11] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field12] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field13] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field14] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field15] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field16] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field17] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field18] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field19] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field2] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field20] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field21] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field22] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field23] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field24] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field25] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field26] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field27] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field28] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field29] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field3] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field30] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field31] ADD SPARSE;

ALTER TABLE [dbo].[SubscriptionsExtension] ALTER COLUMN [Field32] ADD SPARSE;


GO
PRINT N'Altering [dbo].[SubscriptionsExtensionMapper]...';


GO
ALTER TABLE [dbo].[SubscriptionsExtensionMapper]
    ADD [DateCreated]     DATETIME NULL,
        [DateUpdated]     DATETIME NULL,
        [CreatedByUserID] INT      NULL,
        [UpdatedByUserID] INT      NULL;


GO
PRINT N'Starting rebuilding table [dbo].[FileValidator_ImportError]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_FileValidator_ImportError] (
    [FV_ImportErrorID]   INT            IDENTITY (1, 1) NOT NULL,
    [SourceFileID]       INT            NOT NULL,
    [RowNumber]          INT            NULL,
    [FormattedException] VARCHAR (MAX)  NULL,
    [ClientMessage]      VARCHAR (4000) NULL,
    [MAFField]           VARCHAR (255)  NULL,
    [BadDataRow]         VARCHAR (MAX)  NULL,
    [ThreadID]           INT            NULL,
    [DateCreated]        DATETIME       NOT NULL,
    [ProcessCode]        VARCHAR (50)   NOT NULL,
    [IsDimensionError]   BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([FV_ImportErrorID] ASC) WITH (FILLFACTOR = 90)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[FileValidator_ImportError])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FileValidator_ImportError] ON;
        INSERT INTO [dbo].[tmp_ms_xx_FileValidator_ImportError] ([FV_ImportErrorID], [SourceFileID], [RowNumber], [FormattedException], [ClientMessage], [MAFField], [BadDataRow], [ThreadID], [DateCreated], [ProcessCode], [IsDimensionError])
        SELECT   [FV_ImportErrorID],
                 [SourceFileID],
                 [RowNumber],
                 [FormattedException],
                 [ClientMessage],
                 [MAFField],
                 [BadDataRow],
                 [ThreadID],
                 [DateCreated],
                 [ProcessCode],
                 [IsDimensionError]
        FROM     [dbo].[FileValidator_ImportError]
        ORDER BY [FV_ImportErrorID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_FileValidator_ImportError] OFF;
    END

DROP TABLE [dbo].[FileValidator_ImportError];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_FileValidator_ImportError]', N'FileValidator_ImportError';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[ProductAudit]...';


GO
CREATE TABLE [dbo].[ProductAudit] (
    [ProductAuditId]                 INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]                      INT           NOT NULL,
    [AuditField]                     VARCHAR (255) NOT NULL,
    [FieldMappingTypeId]             INT           NOT NULL,
    [ResponseGroupID]                INT           NULL,
    [SubscriptionsExtensionMapperID] INT           NULL,
    [IsActive]                       BIT           NOT NULL,
    [DateCreated]                    DATETIME      NOT NULL,
    [DateUpdated]                    DATETIME      NULL,
    [CreatedByUserID]                INT           NOT NULL,
    [UpdatedByUserID]                INT           NULL,
    PRIMARY KEY CLUSTERED ([ProductAuditId] ASC)
);


GO
PRINT N'Creating [dbo].[ReportGroups]...';


GO
CREATE TABLE [dbo].[ReportGroups] (
    [ReportGroupID]   INT          IDENTITY (1, 1) NOT NULL,
    [ResponseGroupID] INT          NULL,
    [DisplayName]     VARCHAR (50) NULL,
    [DisplayOrder]    INT          NULL,
    PRIMARY KEY CLUSTERED ([ReportGroupID] ASC)
);


GO
PRINT N'Creating [dbo].[Reports]...';


GO
CREATE TABLE [dbo].[Reports] (
    [ReportID]         INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]       VARCHAR (200) NOT NULL,
    [IsActive]         BIT           NOT NULL,
    [DateCreated]      DATETIME      NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT           NULL,
    [ProvideID]        BIT           NOT NULL,
    [ProductID]        INT           NULL,
    [URL]              VARCHAR (250) NULL,
    [IsCrossTabReport] BIT           NULL,
    [Row]              VARCHAR (50)  NULL,
    [Column]           VARCHAR (50)  NULL,
    [SuppressTotal]    BIT           NULL,
    [Status]           BIT           NULL,
    PRIMARY KEY CLUSTERED ([ReportID] ASC) WITH (FILLFACTOR = 80)
);


GO
PRINT N'Creating [dbo].[Response]...';


GO
CREATE TABLE [dbo].[Response] (
    [ResponseID]      INT           IDENTITY (1, 1) NOT NULL,
    [ResponseGroupID] INT           NULL,
    [PublicationID]   INT           NOT NULL,
    [ResponseName]    VARCHAR (250) NOT NULL,
    [ResponseCode]    VARCHAR (250) NOT NULL,
    [DisplayName]     VARCHAR (250) NOT NULL,
    [DisplayOrder]    INT           NOT NULL,
    [ReportGroupID]   INT           NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    [WQT_ResponseID]  INT           NULL,
    [IsOther]         BIT           NOT NULL,
    CONSTRAINT [PK_Response] PRIMARY KEY CLUSTERED ([ResponseID] ASC)
);


GO
PRINT N'Creating [dbo].[ResponseType]...';


GO
CREATE TABLE [dbo].[ResponseType] (
    [ResponseTypeID]      INT           IDENTITY (1, 1) NOT NULL,
    [PublicationID]       INT           NOT NULL,
    [ResponseTypeName]    VARCHAR (100) NOT NULL,
    [DisplayName]         VARCHAR (100) NOT NULL,
    [DisplayOrder]        INT           NOT NULL,
    [IsMultipleValue]     BIT           NOT NULL,
    [IsRequired]          BIT           NOT NULL,
    [IsActive]            BIT           NOT NULL,
    [DateCreated]         DATETIME      NOT NULL,
    [DateUpdated]         DATETIME      NULL,
    [CreatedByUserID]     INT           NOT NULL,
    [UpdatedByUserID]     INT           NULL,
    [WQT_ResponseGroupID] INT           NULL,
    CONSTRAINT [PK_ResponseType] PRIMARY KEY CLUSTERED ([ResponseTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[SubscriptionResponseMap]...';


GO
CREATE TABLE [dbo].[SubscriptionResponseMap] (
    [SubscriptionID]  INT           NOT NULL,
    [ResponseID]      INT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    [ResponseOther]   VARCHAR (300) NULL,
    CONSTRAINT [PK_SubscriptionResponseMap] PRIMARY KEY CLUSTERED ([SubscriptionID] ASC, [ResponseID] ASC) WITH (FILLFACTOR = 80)
);


GO
PRINT N'Creating Default Constraint on [dbo].[Reports]....';


GO
ALTER TABLE [dbo].[Reports]
    ADD DEFAULT ((1)) FOR [ProvideID];


GO
PRINT N'Creating DF_Response_IsOther...';


GO
ALTER TABLE [dbo].[Response]
    ADD CONSTRAINT [DF_Response_IsOther] DEFAULT ((0)) FOR [IsOther];


GO
PRINT N'Creating Default Constraint on [dbo].[SubscriptionResponseMap]....';


GO
ALTER TABLE [dbo].[SubscriptionResponseMap]
    ADD DEFAULT ('') FOR [ResponseOther];


GO
PRINT N'Creating DF_Pricing_UpdatedDate...';


GO
ALTER TABLE [dbo].[Pricing]
    ADD CONSTRAINT [DF_Pricing_UpdatedDate] DEFAULT (getdate()) FOR [UpdatedDate];


GO
PRINT N'Creating FK_PubSubscriptions_Pubs...';


GO
ALTER TABLE [dbo].[PubSubscriptions] WITH NOCHECK
    ADD CONSTRAINT [FK_PubSubscriptions_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID]);


GO
PRINT N'Creating FK_PubMarkets_Pubs...';


GO
ALTER TABLE [dbo].[PubMarkets] WITH NOCHECK
    ADD CONSTRAINT [FK_PubMarkets_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID]);


GO
PRINT N'Creating FK_PubGroups_Pubs...';


GO
ALTER TABLE [dbo].[PubGroups] WITH NOCHECK
    ADD CONSTRAINT [FK_PubGroups_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID]);


GO
PRINT N'Creating FK_ResponseGroups_Pubs...';


GO
ALTER TABLE [dbo].[ResponseGroups] WITH NOCHECK
    ADD CONSTRAINT [FK_ResponseGroups_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID]);


GO
PRINT N'Creating FK_Orders_ApplicationUsers...';


GO
ALTER TABLE [dbo].[Orders] WITH NOCHECK
    ADD CONSTRAINT [FK_Orders_ApplicationUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[ApplicationUsers] ([UserID]);


GO
PRINT N'Creating FK_Orders_Country...';


GO
ALTER TABLE [dbo].[Orders] WITH NOCHECK
    ADD CONSTRAINT [FK_Orders_Country] FOREIGN KEY ([CardHolderCountryID]) REFERENCES [dbo].[Country] ([CountryID]);


GO
PRINT N'Creating FK_Pricing_Brand...';


GO
ALTER TABLE [dbo].[Pricing] WITH NOCHECK
    ADD CONSTRAINT [FK_Pricing_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID]);


GO
PRINT N'Refreshing [dbo].[vw_BrandConsensus]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[vw_BrandConsensus]';


GO
PRINT N'Refreshing [dbo].[vw_Mapping]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[vw_Mapping]';


GO
PRINT N'Refreshing [dbo].[vw_Markets]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[vw_Markets]';


GO

PRINT N'Adding Default Constraints for DateCreated';
ALTER TABLE Adhoc ADD CONSTRAINT DF_Adhoc_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE AdhocCategory ADD CONSTRAINT DF_AdhocCategory_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Blast ADD CONSTRAINT DF_Blast_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE BrandProductMap ADD CONSTRAINT DF_BrandProductMap_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE CodeSheet ADD CONSTRAINT DF_CodeSheet_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE DomainTracking ADD CONSTRAINT DF_DomainTracking_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE FileValidator_DemographicTransformed ADD CONSTRAINT DF_FileValidator_DemographicTransformed_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE FileValidator_ImportError ADD CONSTRAINT DF_FileValidator_ImportError_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE FileValidator_Transformed ADD CONSTRAINT DF_FileValidator_Transformed_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE ImportError ADD CONSTRAINT DF_ImportError_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Markets ADD CONSTRAINT DF_Markets_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Mastercodesheet ADD CONSTRAINT DF_Mastercodesheet_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE MasterGroups ADD CONSTRAINT DF_MasterGroups_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE ProductAudit ADD CONSTRAINT DF_ProductAudit_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Pubs ADD CONSTRAINT DF_Pubs_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE PubSubscriptionDetail ADD CONSTRAINT DF_PubSubscriptionDetail_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE PubSubscriptions ADD CONSTRAINT DF_PubSubscriptions_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE PubTypes ADD CONSTRAINT DF_PubTypes_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE QSource ADD CONSTRAINT DF_QSource_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE QSourceGroup ADD CONSTRAINT DF_QSourceGroup_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Reports ADD CONSTRAINT DF_Reports_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Response ADD CONSTRAINT DF_Response_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE ResponseGroups ADD CONSTRAINT DF_ResponseGroups_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE ResponseType ADD CONSTRAINT DF_ResponseType_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SecurityGroupBrandMap ADD CONSTRAINT DF_SecurityGroupBrandMap_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SecurityGroupProductMap ADD CONSTRAINT DF_SecurityGroupProductMap_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberArchive ADD CONSTRAINT DF_SubscriberArchive_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberDemographicArchive ADD CONSTRAINT DF_SubscriberDemographicArchive_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberDemographicFinal ADD CONSTRAINT DF_SubscriberDemographicFinal_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberDemographicInvalid ADD CONSTRAINT DF_SubscriberDemographicInvalid_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberDemographicOriginal ADD CONSTRAINT DF_SubscriberDemographicOriginal_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberDemographicTransformed ADD CONSTRAINT DF_SubscriberDemographicTransformed_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberFinal ADD CONSTRAINT DF_SubscriberFinal_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberInvalid ADD CONSTRAINT DF_SubscriberInvalid_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberOriginal ADD CONSTRAINT DF_SubscriberOriginal_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriberTransformed ADD CONSTRAINT DF_SubscriberTransformed_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriptionResponseMap ADD CONSTRAINT DF_SubscriptionResponseMap_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Subscriptions ADD CONSTRAINT DF_Subscriptions_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE SubscriptionsExtensionMapper ADD CONSTRAINT DF_SubscriptionsExtensionMapper_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE Suppressed ADD CONSTRAINT DF_Suppressed_DateCreated DEFAULT GETDATE() FOR DateCreated
ALTER TABLE UADSuppression ADD CONSTRAINT DF_UADSuppression_DateCreated DEFAULT GETDATE() FOR DateCreated




PRINT N'Add IsActive to Subscriber tables...';


GO
if not exists(select * from sys.columns 
            where Name = N'IsActive' and Object_ID = Object_ID(N'SubscriberFinal'))
	begin
		ALTER TABLE SubscriberFinal
		Add IsActive bit null
	end
go
if not exists(select * from sys.columns 
            where Name = N'IsActive' and Object_ID = Object_ID(N'SubscriberOriginal'))
	begin
		ALTER TABLE SubscriberOriginal
		Add IsActive bit null
	end
go
if not exists(select * from sys.columns 
            where Name = N'IsActive' and Object_ID = Object_ID(N'SubscriberTransformed'))
	begin
		ALTER TABLE SubscriberTransformed
		Add IsActive bit null
	end
go
if not exists(select * from sys.columns 
            where Name = N'IsActive' and Object_ID = Object_ID(N'SubscriberArchive'))
	begin
		ALTER TABLE SubscriberArchive
		Add IsActive bit null
	end
go
if not exists(select * from sys.columns 
            where Name = N'IsActive' and Object_ID = Object_ID(N'SubscriberInvalid'))
	begin
		ALTER TABLE SubscriberInvalid
		Add IsActive bit null
	end
go

ALTER PROCEDURE [e_SubscriberFinal_SaveDQMClean]
@ProcessCode varchar(50)
AS
	exec e_SubscriberFinal_DisableIndexes
	exec e_SubscriberDemographicFinal_DisableIndexes

	INSERT INTO SubscriberFinal 
	(
		 STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,IsMailable,ProcessCode,ImportRowNumber,IsActive
	)  
	SELECT 
		 st.STRecordIdentifier,st.SourceFileID,st.PubCode,st.Sequence,st.FName,st.LName,st.Title,st.Company,st.Address,st.MailStop,st.City,st.State,st.Zip,st.Plus4,st.ForZip,st.County,st.Country,st.CountryID,st.Phone,st.PhoneExists,
		 st.Fax,st.FaxExists,st.Email,st.EmailExists,st.CategoryID,st.TransactionID,st.TransactionDate,st.QDate,st.QSourceID,st.RegCode,st.Verified,st.SubSrc,st.OrigsSrc,st.Par3C,st.Demo31,st.Demo32,st.Demo33,st.Demo34,st.Demo35,st.Demo36,st.Source,
		 st.Priority,st.IGrp_No,st.IGrp_Cnt,st.CGrp_No,st.CGrp_Cnt,st.StatList,st.Sic,st.SicCode,st.Gender,st.IGrp_Rank,st.CGrp_Rank,st.Address3,st.Home_Work_Address,st.PubIDs,st.Demo7,st.IsExcluded,st.Mobile,st.Latitude,st.Longitude,st.IsLatLonValid,
		 st.LatLonMsg,st.Score,st.EmailStatusID,st.StatusUpdatedDate,st.StatusUpdatedReason,st.Ignore,st.IsDQMProcessFinished,st.DQMProcessDate,st.IsUpdatedInLive,
		 st.UpdateInLiveDate, NEWID() AS SFRecordIdentifier, st.DateCreated,st.DateUpdated,st.CreatedByUserID,st.UpdatedByUserID,st.IsMailable,st.ProcessCode,st.ImportRowNumber,st.IsActive
	FROM SubscriberTransformed st With(NoLock)
	LEFT OUTER JOIN  SubscriberFinal sf with(NoLock) on st.STRecordIdentifier = sf.STRecordIdentifier
	WHERE st.ProcessCode = @ProcessCode 
	AND sf.SFRecordIdentifier is null

	--Insert non-duplicate records into subscriberDemographicFinal table
	INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	
	SELECT sdt.PubID,sf.SFRecordIdentifier,sdt.MAFField,sdt.Value,sdt.NotExists,sdt.DateCreated,sdt.DateUpdated,sdt.CreatedByUserID,sdt.UpdatedByUserID
	FROM SubscriberFinal sf  with(nolock)
	JOIN SubscriberDemographicTransformed sdt with(nolock) ON sdt.STRecordIdentifier = sf.STRecordIdentifier
	WHERE sf.ProcessCode = @ProcessCode 
			AND sdt.NotExists = 'false'

	exec e_SubscriberFinal_EnableIndexes
	exec e_SubscriberDemographicFinal_EnableIndexes

	UPDATE SubscriberFinal
	SET TransactionID = 10
	WHERE ProcessCode = @ProcessCode 
			AND (TransactionID = 0 OR TransactionID NOT IN (SELECT TransactionID FROM [Transaction]))
	
	UPDATE SubscriberFinal
	SET CategoryID = 10
	WHERE ProcessCode = @ProcessCode 
			AND (CategoryID = 0 OR CategoryID NOT IN (SELECT CategoryID FROM CATEGORY))

GO

CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select_ProductID_Count]
@ProductID int
AS
	Select COUNT(*) 
	FROM SubscriptionResponseMap srm with(nolock)
	JOIN Circulation..Subscription s  with(nolock) ON srm.SubscriptionID = s.SubscriptionID
	WHERE s.PublicationID = @ProductID 
go

CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select_ProductID_Paging]
@CurrentPage int,
@PageSize int,
@ProductID int
AS
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY srm.SubscriptionID) as 'RowNum', srm.* 
	FROM SubscriptionResponseMap srm  with(nolock)
	JOIN Circulation..Subscription s  with(nolock) ON srm.SubscriptionID = s.SubscriptionID
	WHERE s.PublicationID = @ProductID 
	)
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
go

CREATE proc [dbo].[rpt_QualificationBreakdown]
(
	@ProductID int,
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
	@years int,  
	@PrintColumns varchar(4000),
	@Download char(1)  
)
as
Begin 
	
	set nocount on
	
	declare @publicationID int
	declare @GetSubscriberIDs bit = 0
	
	set @publicationID = @ProductID
	
	declare @PublicationCode varchar(20)
	select @PublicationCode  = PubCode from Pubs where PubID = @ProductID
	
	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	create table #SubscriptionID (SubscriptionID int)  
	
	Insert into #SubscriptionID   
	exec rpt_GetSubscriptionIDs_From_Filter @ProductID, @CategoryIDs,
	@CategoryCodes,
	@TransactionIDs,
	@TransactionCodes,
	@QsourceIDs,
	@StateIDs,
	@Regions,
	@CountryIDs,
	@Email,
	@Phone,
	@Fax,
	@ResponseIDs,
	@Demo7,		
	@Year,
	@startDate,		
	@endDate,
	@AdHocXML,
	@GetSubscriberIDs

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	if @download = '0'
	Begin
		declare @yearTemp int,
				@i int,
				@y int,
				@sqlstring varchar(4000),
				@startperiod varchar(10),
				@endperiod varchar(10),
				@startdateTemp datetime,
				@enddateTemp datetime

		select @startperiod = p.YearStartDate , @endperiod = p.YearEndDate from Pubs p where PubID = @publicationID

		set @i = 0
		set @sqlstring = ''

		if getdate() > convert(datetime,@startperiod + '/' + convert(varchar,year(getdate())))
			set @yearTemp = year(getdate()) 
		else
			set @yearTemp = year(getdate()) - 1

		select @startdateTemp = @startperiod + '/' + convert(varchar,@yearTemp)
		select @endDateTemp =  dateadd(yy, 1, @startdateTemp) 

		while (@i < 5)
		Begin
			if @i < @years
			Begin
				set @y = @yearTemp-@i
				set @sqlstring = @sqlstring +  ' isnull(sum(Case when s.QSourceDate between ''' + convert(varchar(20), @startdateTemp, 120) +''' and ''' + convert(varchar(20),dateadd(ss, -1,@endDateTemp ), 120) +''' then s.copies end),0) AS Column'+ convert(varchar,@i)  + ','
				
				set @startdateTemp = dateadd(yy, -1, @startdateTemp) 
				set @endDateTemp =  dateadd(yy, -1, @endDateTemp) 
								
			end
			else if @i = 4
			Begin
				set @sqlstring = @sqlstring + ' isnull(sum(Case when s.QSourceDate < ''' + convert(varchar(20), @endDateTemp, 120) +''' then s.copies end),0) AS Column4' + ','
			end
			else
			Begin
				set @sqlstring = @sqlstring +  ' 0 AS Column' + convert(varchar,@i) + ','
			end
			select @i = @i + 1
		end 

		exec(' select	qg.displayname as QsourceGroup, q.displayname as Qsource, isnull(Column0,0) as OneYearCount, isnull(Column1,0) as TwoYearCount, isnull(Column2,0) as ThreeYearCount, isnull(Column3,0) as FourYearCount, isnull(Column4,0) as OtherYearCount, isnull(Qualified_Nonpaid,0) as NonQualifiedCount, isnull(Qualified_Paid,0) as QualifiedCount ' + 
			 ' from UAS..Code qg join Circulation..QualificationSource q  on qg.DisplayOrder = Q.QSourceTypeID left outer join ' + 
			'( ' + 
				'select s.QSourceID, ' + @sqlstring + 
						' sum(case when cct.CategoryCodeTypeName = ''Qualified Free'' then s.copies end) as Qualified_Nonpaid, ' + 
						' sum(case when cct.CategoryCodeTypeName = ''Qualified Paid'' then s.copies end) as Qualified_Paid ' + 
			 ' From ' +   
				' Circulation..Subscription s join ' +
				' #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  join  ' +
				' Circulation..Subscriber sb on s.SubscriberID = sb.subscriberID join ' +
				' Circulation..Action a on a.ActionID = s.ActionID_Current join ' +
				' Circulation..CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join ' +
				' Circulation..CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID join ' +
				' Circulation..TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID  ' +
				
			 ' Where ' +
				's.publicationID = ' + @publicationID + 
			  ' group by  s.QSourceID ) as temp1 on temp1.QSourceID = q.QSourceID Where qg.CodeTypeID = 19')			  	  
			 
	end
	else
	begin
		IF OBJECT_ID('tempdb..#tmpSubscriptionID') IS NOT NULL 
			BEGIN 
					DROP TABLE #tmpSubscriptionID;
			END 
			      
			CREATE TABLE #tmpSubscriptionID (subsID int); 

			insert into #tmpSubscriptionID
						exec sp_rpt_GetSubscriptionIDs @ProductID, @CategoryIDs,
							@CategoryCodes,
							@TransactionIDs,
							@TransactionCodes,
							@QsourceIDs,
							@StateIDs,
							@Regions,
							@CountryIDs,
							@Email,
							@Phone,
							@Fax,
							@ResponseIDs,
							@Demo7,		
							@Year,
							@startDate,		
							@endDate,
							@AdHocXML,
							@GetSubscriberIDs
		
			exec (	' select  distinct mg.PubCode as pubcode,s.SubscriberID, s.SubscriptionID, s.EMAILADDRESS, s.FNAME, s.LNAME, s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate, s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QSourceDate, s.Subsrc, s.copies, spt.PriceCode, spt.Term, spt.StartIssueDate, spt.ExpireIssueDate, spt.CPRate, spt.Amount, spt.AmountPaid, spt.BalanceDue, spt.PaidDate, spt.TotalIssues, p.paymenttype, ' +
					' case  when StartIssueDate > GETDATE() then TotalIssues
							when StartIssueDate < GETDATE() and expireissuedate > getdate() then  DATEDIFF(MM, dateadd(mm,1,convert(varchar,MONTH(getdate())) + ''/01/'' + convert(varchar,Year(getdate()))), expireissuedate) + 1   
							when  expireissuedate  < getdate()  then 0    end as  isssuestogo, spt.CheckNumber,spt.CCNumber,spt.CCExpirationMonth,spt.CCEXpirationYear,spt.CCHolderName   ' +
					@PrintColumns + 
					' from Circulation..Subscription s join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID left outer join QSource q on q.Qsource_ID = s.Qsource_ID join Category C on s.Category_ID = C.Category_ID join ' +
					' Categorygroup cg on cg.Categorygroup_ID = C.Categorygroup_ID join [Transaction] t on s.Transaction_ID = t.Transaction_ID join ' + 
					' Pubs mg on mg.PubID = s.PublicationID ' + 
					' join #tmpSubscriptionID tmp on s.subscriptionID = tmp.subsID ' +
					' left outer join SubscriberPaidTransaction spt on spt.SubscriptionID = s.subscriptionID ' +
					' left outer join PaymentsType p on  p.PaymentTypeID = spt.PaymentTypeID ' +
					' where s.magazineID = ' + @ProductID )
					
			DROP TABLE #tmpSubscriptionID
		end
			
	drop table #SubscriptionID 			
end		
go

CREATE proc [dbo].[rpt_CategorySummary]
(
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
	@PrintColumns varchar(4000),   
	@Download bit     
)
as
Begin

	set nocount on	
	declare @pubID int	
	set @pubID = @PublicationID
	declare @PublicationCode varchar(20)
	select @PublicationCode  = PubCode from Pubs with(nolock) where PubID = @pubID
	declare @GetSubscriberIDs bit = 0
	
	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	create table #SubscriptionID (SubscriptionID int)   
	
	Insert into #SubscriptionID   
	exec rpt_GetSubscriptionIDs_From_Filter @PublicationID, @CategoryIDs,
	@CategoryCodes,
	@TransactionIDs,
	@TransactionCodes,
	@QsourceIDs,
	@StateIDs,
	@Regions,
	@CountryIDs,
	@Email,
	@Phone,
	@Fax,
	@ResponseIDs,
	@Demo7,		
	@Year,
	@startDate,		
	@endDate,
	@AdHocXML,
	@GetSubscriberIDs
	
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	declare @cat table
		(
		CategoryCodeTypeID int,
		CategoryCodeID int,
		CategoryCodeTypeName varchar(100),
		CategoryCodeName varchar(100),
		CategoryCodeValue int
		)
	
		insert into @cat
		select	distinct cct.CategoryCodeTypeID, 
				cc.CategoryCodeID, 
				cct.CategoryCodeTypeName, 
				cc.CategoryCodeName,
				CategoryCodeValue
		from	
				Circulation..CategoryCodeType cct with(nolock) join 
				Circulation..CategoryCode cc with(nolock) on cc.CategoryCodeTypeID = cct.CategoryCodeTypeID 
				
	if @Download = 0
		Begin
			declare @sub table
			(
				scount int,
				CategoryCodeID int
			)
		
			insert into @sub
			select sum(s.copies), a.CategoryCodeID
			from	Circulation..Subscription  s with(nolock) join
					#SubscriptionID sf with(nolock) on sf.SubscriptionID = s.SubscriptionID join
					Circulation..Action a with(nolock) on a.ActionID = s.ActionID_Current
			where 
					PublicationID = @PublicationID
			group by 
					a.CategoryCodeID
		
			select 
				c.CategoryCodeTypeID categorygroup_ID,
				c.CategoryCodeValue category_ID,
				c.CategoryCodeTypeName categorygroup_name,
				c.CategoryCodeName category_name,
				isnull(sum(scount),0) as total
			from @cat c left outer join @sub s on c.CategoryCodeID = s.CategoryCodeID 
			group by c.CategoryCodeTypeID, c.CategoryCodeID,c.CategoryCodeValue, c.CategoryCodeTypeName, c.CategoryCodeName
		end
	else
		Begin
			exec ('select  distinct ''' + @PublicationCode + ''' as PubCode, s.SubscriberID, s.EMAILADDRESS, s.FNAME, s.LNAME, ' +
				' s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate, s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QualificationDate, s.Subsrc, s.copies ' +
				@PrintColumns + 
			' from subscriptions  s join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID join Category C on s.Category_ID = C.Category_ID left outer join QSource q on q.Qsource_ID = s.Qsource_ID where PublicationID = ' + @PublicationID )
		end
	drop table #SubscriptionID
end
go

CREATE Proc [dbo].[rpt_GetSubscriptionIDs_From_Filter]
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
	'Select	DISTINCT sp.SubscriptionID as Count
	FROM	Circulation..Subscriber s with(nolock) JOIN
			Circulation..Subscription sp with(nolock) ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
			Circulation..SubscriptionPaid spp with(nolock) ON sp.SubscriptionID = spp.SubscriptionID JOIN
			Circulation..Action a with(nolock) ON a.ActionID = sp.ActionID_Current JOIN
			Circulation..CategoryCode dc with(nolock) ON a.CategoryCodeID = dc.CategoryCodeID JOIN
			Circulation..TransactionCode dt with(nolock) on a.TransactionCodeID = dt.TransactionCodeID JOIN
			Circulation..TransactionCodeType tct with(nolock) ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
			Circulation..CategoryCodeType cct with(nolock) ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
			Circulation..QualificationSource dq with(nolock) on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
			UAS..Country c with(nolock) ON c.CountryID = s.CountryID
	Where	
			sp.PublicationID = ' + Convert(varchar,@PublicationID) 
else if(@GetSubscriberIDs = 1)
	set @executeString =
	'Select	DISTINCT sp.SubscriberID as Count
	FROM	Circulation..Subscriber s with(nolock) JOIN
			Circulation..Subscription sp with(nolock) ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
			Circulation..SubscriptionPaid spp with(nolock) ON sp.SubscriptionID = spp.SubscriptionID JOIN
			Circulation..Action a with(nolock) ON a.ActionID = sp.ActionID_Current JOIN
			Circulation..CategoryCode dc with(nolock) ON a.CategoryCodeID = dc.CategoryCodeID JOIN
			Circulation..TransactionCode dt with(nolock) on a.TransactionCodeID = dt.TransactionCodeID JOIN
			Circulation..TransactionCodeType tct with(nolock) ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
			Circulation..CategoryCodeType cct with(nolock) ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
			Circulation..QualificationSource dq with(nolock) on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
			UAS..Country c with(nolock) ON c.CountryID = s.CountryID
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
			
			select @tempStartDate = YearStartDate , @tempEndDate = YearEndDate from Pubs with(nolock) where PubID = @PublicationID	
			
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
			set @executeString = @executeString + ' and sp.DeliverabilityID in (SELECT DeliverabilityID from Circulation..Deliverability d with (NOLOCK) WHERE d.DeliverabilityID in (' + @Demo7 + '))'	
		
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
										WHEN 'Equal' THEN 's.' + @Column + ' = ' + @ValueFrom 
										WHEN 'Greater Than' THEN 's.' + @Column + ' >= ' + @ValueFrom 
										WHEN 'Lesser Than' THEN 's.' + @Column + ' <= ' + @ValueFrom 
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


PRINT N'Altering [dbo].[e_Subscription_Save_For_Delta]...';

GO

ALTER PROC [e_Subscription_Save_For_Delta]
(
	@IncomingReferenceID varchar(100),
	@FullFile bit = 0,
	@DeleteUpdateMissingSubscribers varchar(10) = '' --delete or update
)
as 
Begin
	set nocount on

	Create table #tmpIncomingDataDetails (cdid int IDENTITY(1,1), PubSubscriptionID int not null,  pubid int  not null, responsegroup varchar(200), responsevalue varchar(200), NotExists bit)

	Create table #tmpProductDemographics (PubSubscriptionID int not null, pubID int  not null, responsevalue varchar(max))

	declare 
		    @s varchar(100),
			@b varchar(max)
				

	update API_IncomingData set ctry='2' where IncomingReferenceID = @IncomingReferenceID and country= 'canada'
	update API_IncomingData set CTRY = null where IncomingReferenceID = @IncomingReferenceID and  CTRY = ','
	update API_IncomingData set XACT = 10 where IncomingReferenceID = @IncomingReferenceID and  XACT = 0
	update API_IncomingData set cat = 10 where IncomingReferenceID = @IncomingReferenceID and  cat = 0

	/* Delete or Update Missing Igrpno in LIVE */
	
	if (@FullFile = 1 and len(@DeleteUpdateMissingSubscribers) > 0)
	Begin
		create table #tblMissingIgrpno (SubscriptionID int, pubsubscriptionID int, Igrp_no uniqueidentifier)
		
		Insert into #tblMissingIgrpno		
		SELECT		s.SubscriptionID, ps.PubSubscriptionID, s.IGRP_NO   
		FROM  
					PubSubscriptions ps  WITH (nolock) join 
					Subscriptions s  WITH (nolock) on ps.subscriptionID = s.SubscriptionID join 
					Pubs p  WITH (nolock) on ps.PubID = p.pubID left outer join
					[API_IncomingData] a  WITH (nolock) on IncomingReferenceID = @IncomingReferenceID and a.PubCode = p.PUBCODE and a.IGRP_NO = s.IGRP_NO
		where 
					p.PubCode in 
					(
						select distinct PUBCODE from API_IncomingData a WITH (nolock) WHERE IncomingReferenceID = @IncomingReferenceID
					) and 
					a.IGRP_NO is null 


		select COUNT(*) from #tblMissingIgrpno
		
		if (@DeleteUpdateMissingSubscribers = 'delete')
		Begin
			
			delete from SubscriberClickActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from SubscriberOpenActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from SubscriberTopicActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from PubSubscriptionDetail where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from PubSubscriptions where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			
			declare @tblDeleteSubscriptionIDs table (SubscriptionID int PRIMARY KEY)
			
			insert into @tblDeleteSubscriptionIDs
			select distinct s.subscriptionID 
			from
					Subscriptions s  WITH (nolock) left outer join
					PubSubscriptions ps  WITH (nolock) on s.SubscriptionID = ps.SubscriptionID
			where
					ps.SubscriptionID is null

			if exists (select top 1 SubscriptionID from @tblDeleteSubscriptionIDs)	
			Begin					
				delete from BrandScore  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from CampaignFilterDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from SubscriberClickActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriberOpenActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriberTopicActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriberVisitActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from SubscriberMasterValues  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriptionsExtension  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from PubSubscriptionDetail  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from PubSubscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from SubscriptionDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from Subscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
			End
			
			-- repopulate concensus
			Delete from SubscriptionDetails
			Where 
					SubscriptionID in (select distinct SubscriptionID from #tblMissingIgrpno) and
					MasterID in (select MasterID from CodeSheet_Mastercodesheet_Bridge)

			insert into SubscriptionDetails (SubscriptionID, MasterID)
			select distinct psd.SubscriptionID, cmb.masterID 
			from 
					#tblMissingIgrpno t join
					PubSubscriptionDetail psd on t.subscriptionID  = psd.SubscriptionID
					join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
					left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
			where  
					sd.sdID is null	    

			Delete from SubscriberMasterValues
			Where 
					SubscriptionID in (select SubscriptionID from #tblMissingIgrpno) 

			insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
			SELECT 
			  MasterGroupID, [SubscriptionID] , 
			  STUFF((
				SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
				FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
				WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
				FOR XML PATH (''))
			  ,1,1,'') AS CombinedValues
			FROM 
				(
					SELECT distinct sd.SubscriptionID, mc.MasterGroupID
					FROM	
							#tblMissingIgrpno t 
							join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.subscriptionID
							join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID 			 
				)
			 Results
			GROUP BY [SubscriptionID] , MasterGroupID
			order by SubscriptionID    

		End
		else if (@DeleteUpdateMissingSubscribers = 'update')
		Begin
			update PubSubscriptions
			set PubTransactionID = 38
			where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
		End
		
		drop table #tblMissingIgrpno
	End
	


	Create table #tbl1 (IncomingDataID int, Igrp_No uniqueidentifier, Igrp_Rank varchar(10), PubID int, SubscriptionID int, PubSubscriptionID int)

	CREATE CLUSTERED INDEX IDX_C_tbl1_IncomingDataID ON #tbl1(IncomingDataID)
    
    CREATE INDEX IDX_tbl1_Igrp_No ON #tbl1(Igrp_No)
    CREATE INDEX IDX_tbl1_SubscriptionID ON #tbl1(SubscriptionID)
    CREATE INDEX IDX_tbl1_PubSubscriptionID ON #tbl1(PubSubscriptionID)
    CREATE INDEX IDX_tbl1_Igrp_Rank ON #tbl1(Igrp_Rank)

	insert into #tbl1 (IncomingDataID, IGRP_NO, Igrp_Rank, PubID, SubscriptionID)
	SELECT IncomingDataID, a.IGRP_NO, a.IGRP_RANK, p.PubID, s.SubscriptionID
	FROM 
		API_IncomingData a WITH (nolock)
		left outer join Subscriptions s WITH (nolock) on a.IGRP_NO = s.IGRP_NO
		JOIN Pubs p WITH (nolock) ON p.PubCode = a.PubCode
	WHERE 
		IncomingReferenceID = @IncomingReferenceID  --and a.email = 'ifynzeka@yahoo.com'

	Print (  'Insert into #tbl1 COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

	update t
	set t.PubSubscriptionID = ps.PubSubscriptionID
	from #tbl1 t join PubSubscriptions ps on t.PubID = ps.PubID and t.SubscriptionID = ps.SubscriptionID

	Print ('Update @tbl PubSubscriptionID COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

	BEGIN TRY
		BEGIN TRANSACTION;
    
	    delete	ps
	    from	PubSubscriptionDetail ps 
				join #tbl1 t on t.PubSubscriptionID = ps.PubSubscriptionID 
				join CodeSheet c on c.CodeSheetID = ps.CodesheetID 
				join ResponseGroups rg on rg.ResponseGroupID = c.ResponseGroupID 
	    Where 
				rg.ResponseGroupName <> 'TOPICS'
	    
	    Print ('DELETE PubSubscriptionDetail COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		delete	sd
		from	SubscriptionDetails sd
				join (select distinct subscriptionID from #tbl1) t  on sd.SubscriptionID = t.SubscriptionID 
				join CodeSheet_Mastercodesheet_Bridge cmb on sd.MasterID = cmb.MasterID 
				 
		Print ('Delete SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		delete	smv
		from	SubscriberMasterValues smv
				join (select distinct subscriptionID from #tbl1) t  on smv.SubscriptionID = t.SubscriptionID 
				 
		Print ('Delete SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    	    
		Update S1
		Set
			[SEQUENCE] = convert(int,isnull(a.[SEQUENCE], 0)),
			FNAME		= CASE WHEN ISNULL(a.FNAME, '') = '' and ISNULL(a.LNAME, '') = '' THEN s1.FNAME ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			LNAME		= CASE WHEN ISNULL(a.LNAME, '') = '' and ISNULL(a.LNAME, '') = '' THEN s1.LNAME ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100)  END,
			TITLE		= CASE WHEN ISNULL(a.TITLE, '') = '' THEN s1.TITLE ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100)  END,
			COMPANY		= CASE WHEN ISNULL(a.COMPANY, '') = '' THEN s1.COMPANY ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			ADDRESS     = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.ADDRESS END),
			MAILSTOP    = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.MAILSTOP END),
			CITY        = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.CITY END),
			STATE       = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.STATE END),
			ZIP         = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.ZIP END),
			PLUS4       = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.PLUS4 END),
			FORZIP      = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!='' AND ISNULL(a.FORZIP,'') !='' 
							  THEN REPLACE(REPLACE(REPLACE(a.FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.FORZIP END),
			COUNTY		= CASE WHEN ISNULL(a.COUNTY, '') = '' THEN s1.COUNTY ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 20) END,
			COUNTRY		= CASE WHEN ISNULL(a.COUNTRY, '') = '' THEN s1.COUNTRY ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			CountryID   = CASE WHEN ISNULL(a.CTRY,'') = '' THEN s1.CountryID ELSE CONVERT(int,a.CTRY) END,
			PHONE		= CASE WHEN ISNULL(a.PHONE, '') = '' THEN s1.PHONE ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			FAX			= CASE WHEN ISNULL(a.FAX, '') = '' THEN s1.FAX ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 30) END,
			MOBILE		= CASE WHEN ISNULL(a.MOBILE, '') = '' THEN s1.MOBILE ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			CategoryID	= case when CONVERT(INT,a.CAT) =0  then NULL else CONVERT(INT,a.CAT) end,
			TransactionID = CONVERT(INT,a.XACT),
			TransactionDate = CONVERT(VARCHAR(10),a.XACTDATE,101),
			QDate		=  CONVERT(VARCHAR(10),a.QDATE,101),
			Email		= SUBSTRING(a.Email, 1, 100),
			Demo31		= case when a.Demo31 is null then 1 when a.Demo31 ='Y' then 1 else 0 end,
			Demo32		= case when a.Demo32 is null then 1 when a.Demo32 ='Y' then 1 else 0 end,
			Demo33		= case when a.Demo33 is null then 1 when a.Demo33 ='Y' then 1 else 0 end,
			Demo34		= case when a.Demo34 is null then 1 when a.Demo34 ='Y' then 1 else 0 end,
			Demo35		= case when a.Demo35 is null then 1 when a.Demo35 ='Y' then 1 else 0 end,
			Demo36		= case when a.Demo36 is null then 1 when a.Demo36 ='Y' then 1 else 0 end,
			Demo7		=  a.Demo7,
			QSourceID	= DBO.FN_GetQSourceID(a.QSOURCE),
			PAR3C		= SUBSTRING(a.PAR3C, 1, 1),
			IGRP_NO		= a.IGRP_NO, 
			IGRP_CNT	= CONVERT(INT,a.IGRP_CNT),
			emailexists = (case when ltrim(rtrim(isnull(a.email,''))) <> '' then 1 else 0 end), 
			Faxexists	= (case when ltrim(rtrim(isnull(a.Fax,''))) <> '' then 1 else 0 end), 
			PhoneExists = (case when ltrim(rtrim(isnull(a.PHONE,''))) <> '' then 1 else 0 end)
		From 
				Subscriptions s1 
				join #tbl1 t on s1.SubscriptionID = t.SubscriptionID 
				join api_incomingdata a on a.IncomingDataID  = t.IncomingDataID
		where 
				t.Igrp_Rank = 'M'

		Print ('Update Subscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		Update [Subscriptions_DQM] 
		Set ZZ_PAR_ADDRESS_STD =  a.ZZ_PAR_ADDRESS_STD,
			ZZ_PAR_CITY_STD =  a.ZZ_PAR_CITY_STD,
			ZZ_PAR_COMPANY_MATCH1 =  a.ZZ_PAR_COMPANY_MATCH1,
			ZZ_PAR_COMPANY_MATCH2 =  a.ZZ_PAR_COMPANY_MATCH2,
			ZZ_PAR_COMPANY_STD =  a.ZZ_PAR_COMPANY_STD,
			ZZ_PAR_COMPANY2 =  a.ZZ_PAR_COMPANY2,
			ZZ_PAR_EMAIL_STD =  a.ZZ_PAR_EMAIL_STD,
			ZZ_PAR_FNAME_MATCH1 =  a.ZZ_PAR_FNAME_MATCH1,
			ZZ_PAR_FNAME_MATCH2 =  a.ZZ_PAR_FNAME_MATCH2,
			ZZ_PAR_FNAME_MATCH3 =  a.ZZ_PAR_FNAME_MATCH3,
			ZZ_PAR_FNAME_MATCH4 =  a.ZZ_PAR_FNAME_MATCH4,
			ZZ_PAR_FNAME_MATCH5 =  a.ZZ_PAR_FNAME_MATCH5,
			ZZ_PAR_FNAME_MATCH6 =  a.ZZ_PAR_FNAME_MATCH6,
			ZZ_PAR_FNAME_STD =  a.ZZ_PAR_FNAME_STD,
			ZZ_PAR_FORZIP_STD =  a.ZZ_PAR_FORZIP_STD,
			ZZ_PAR_INTL_PHONE =  a.ZZ_PAR_INTL_PHONE,
			ZZ_PAR_LNAME_STD =  a.ZZ_PAR_LNAME_STD,
			ZZ_PAR_MAILSTOP_STD = a.ZZ_PAR_MAILSTOP_STD,
			ZZ_PAR_PLUS4_STD =  a.ZZ_PAR_PLUS4_STD,
			ZZ_PAR_POBOX =  a.ZZ_PAR_POBOX,
			ZZ_PAR_POSTCODE =  a.ZZ_PAR_POSTCODE,
			ZZ_PAR_PRIMARY_NUMBER =  a.ZZ_PAR_PRIMARY_NUMBER,
			ZZ_PAR_PRIMARY_POSTFIX =  a.ZZ_PAR_PRIMARY_POSTFIX,
			ZZ_PAR_PRIMARY_PREFIX =  a.ZZ_PAR_PRIMARY_PREFIX,
			ZZ_PAR_PRIMARY_STREET =  a.ZZ_PAR_PRIMARY_STREET,
			ZZ_PAR_PRIMARY_TYPE =  a.ZZ_PAR_PRIMARY_TYPE,
			ZZ_PAR_RR_BOX =  a.ZZ_PAR_RR_BOX,
			ZZ_PAR_RR_NUMBER =  a.ZZ_PAR_RR_NUMBER,
			ZZ_PAR_STATE_STD =  a.ZZ_PAR_STATE_STD,
			ZZ_PAR_TITLE_STD =  a.ZZ_PAR_TITLE_STD,
			ZZ_PAR_UNIT_DESCRIPTION =  a.ZZ_PAR_UNIT_DESCRIPTION,
			ZZ_PAR_UNIT_NUMBER =  a.ZZ_PAR_UNIT_NUMBER,
			ZZ_PAR_USCAN_PHONE =  a.ZZ_PAR_USCAN_PHONE,
			ZZ_PAR_ZIP_STD = a.ZZ_PAR_ZIP_STD
		from 
				[Subscriptions_DQM]  s 
				join  #tbl1 t on s.SubscriptionID = t.SubscriptionID 
				join api_incomingdata a on a.IncomingDataID  = t.IncomingDataID 
		where 
				t.Igrp_Rank = 'M'
	    
	    Print ('Update [Subscriptions_DQM] COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
	    
	    Insert into [Subscriptions] 
			(	
				[SEQUENCE], FNAME, LNAME, TITLE, COMPANY, ADDRESS, MAILSTOP, CITY, STATE, ZIP, PLUS4,FORZIP,COUNTY,COUNTRY,CountryID,PHONE,MOBILE,FAX,EMAIL,
				CategoryID, TransactionID, TransactionDate,QDate, 
				QSourceID,PAR3C,
				Demo31,Demo32,Demo33,Demo34,Demo35,Demo36, 
				IGRP_NO, IGRP_CNT,
				emailexists, Faxexists, PhoneExists 
			)
		select
				convert(int,[SEQUENCE]) as sequence, 
				SUBSTRING(REPLACE(REPLACE(REPLACE(FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 255),
				SUBSTRING(REPLACE(REPLACE(REPLACE(MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 255),
				SUBSTRING(REPLACE(REPLACE(REPLACE(CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 50),
				SUBSTRING(REPLACE(REPLACE(REPLACE(STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1,50),
				SUBSTRING(REPLACE(REPLACE(REPLACE(ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1,10),
				SUBSTRING(REPLACE(REPLACE(REPLACE(PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 4),
				SUBSTRING(REPLACE(REPLACE(REPLACE(FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 50),
				SUBSTRING(REPLACE(REPLACE(REPLACE(COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 20),
				SUBSTRING(REPLACE(REPLACE(REPLACE(COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				CONVERT(int,CTRY),
				SUBSTRING(REPLACE(REPLACE(REPLACE(PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 30),
				SUBSTRING(REPLACE(REPLACE(REPLACE(FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(EMAIL, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				case when CONVERT(INT,CAT) =0  then NULL else CONVERT(INT,CAT) end as CategoryID, 
				CONVERT(INT,XACT) as TransactionID, 
				CONVERT(VARCHAR(10),XACTDATE,101) as TransactionDate, 
				CONVERT(VARCHAR(10),QDATE,101) as QDate, 
				DBO.FN_GetQSourceID(QSOURCE) as QSourceID, 
				SUBSTRING(PAR3C, 1, 1),
				case when Demo31 is null then 1 when Demo31 ='Y' then 1 else 0 end as Demo31, 
				case when DEMO32 is null then 1 when DEMO32 ='Y' then 1 else 0 end as Demo32, 
				case when DEMO33 is null then 1 when DEMO33 ='Y' then 1 else 0 end as Demo33, 
				case when DEMO34 is null then 1 when DEMO34 ='Y' then 1 else 0 end as Demo34, 
				case when DEMO35 is null then 1 when DEMO35 ='Y' then 1 else 0 end as Demo35, 
				case when DEMO36 is null then 1 when DEMO36 ='Y' then 1 else 0 end as Demo36, 
				a.Igrp_no, CONVERT(INT,IGRP_CNT) as IGRP_CNT,
				(case when ltrim(rtrim(isnull(email,''))) <> '' then 1 else 0 end),
				(case when ltrim(rtrim(isnull(Fax,''))) <> '' then 1 else 0 end),
				(case when ltrim(rtrim(isnull(PHONE,''))) <> '' then 1 else 0 end)
		from 
				#tbl1 t 
				join  api_incomingdata a with (NOLOCK) on t.IncomingDataID = a.IncomingDataID 
		where 
				t.SubscriptionID is null  and 
				t.Igrp_Rank = 'M' 

	    Print ('Insert [Subscriptions] COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
				
		Insert into [Subscriptions_DQM] 
		(	
			SubscriptionID, IGRP_NO, ZZ_PAR_ADDRESS_STD,ZZ_PAR_CITY_STD,ZZ_PAR_COMPANY_MATCH1,ZZ_PAR_COMPANY_MATCH2,ZZ_PAR_COMPANY_STD,ZZ_PAR_COMPANY2,
			ZZ_PAR_EMAIL_STD,ZZ_PAR_FNAME_MATCH1,ZZ_PAR_FNAME_MATCH2,ZZ_PAR_FNAME_MATCH3,ZZ_PAR_FNAME_MATCH4,ZZ_PAR_FNAME_MATCH5,ZZ_PAR_FNAME_MATCH6,
			ZZ_PAR_FNAME_STD,ZZ_PAR_FORZIP_STD,ZZ_PAR_INTL_PHONE,ZZ_PAR_LNAME_STD,ZZ_PAR_MAILSTOP_STD,ZZ_PAR_PLUS4_STD,ZZ_PAR_POBOX,ZZ_PAR_POSTCODE,
			ZZ_PAR_PRIMARY_NUMBER,ZZ_PAR_PRIMARY_POSTFIX,ZZ_PAR_PRIMARY_PREFIX,ZZ_PAR_PRIMARY_STREET,ZZ_PAR_PRIMARY_TYPE,ZZ_PAR_RR_BOX,ZZ_PAR_RR_NUMBER,
			ZZ_PAR_STATE_STD,ZZ_PAR_TITLE_STD,ZZ_PAR_UNIT_DESCRIPTION,ZZ_PAR_UNIT_NUMBER,ZZ_PAR_USCAN_PHONE,ZZ_PAR_ZIP_STD
		)
		select
			s.SubscriptionID, s.IGRP_NO, ZZ_PAR_ADDRESS_STD,ZZ_PAR_CITY_STD,ZZ_PAR_COMPANY_MATCH1,ZZ_PAR_COMPANY_MATCH2,ZZ_PAR_COMPANY_STD,ZZ_PAR_COMPANY2,
			ZZ_PAR_EMAIL_STD,ZZ_PAR_FNAME_MATCH1,ZZ_PAR_FNAME_MATCH2,ZZ_PAR_FNAME_MATCH3,ZZ_PAR_FNAME_MATCH4,ZZ_PAR_FNAME_MATCH5,ZZ_PAR_FNAME_MATCH6,
			ZZ_PAR_FNAME_STD,ZZ_PAR_FORZIP_STD,ZZ_PAR_INTL_PHONE,ZZ_PAR_LNAME_STD,ZZ_PAR_MAILSTOP_STD,ZZ_PAR_PLUS4_STD,ZZ_PAR_POBOX,ZZ_PAR_POSTCODE,
			ZZ_PAR_PRIMARY_NUMBER,ZZ_PAR_PRIMARY_POSTFIX,ZZ_PAR_PRIMARY_PREFIX,ZZ_PAR_PRIMARY_STREET,ZZ_PAR_PRIMARY_TYPE,ZZ_PAR_RR_BOX,ZZ_PAR_RR_NUMBER,
			ZZ_PAR_STATE_STD,ZZ_PAR_TITLE_STD,ZZ_PAR_UNIT_DESCRIPTION,ZZ_PAR_UNIT_NUMBER,ZZ_PAR_USCAN_PHONE,ZZ_PAR_ZIP_STD
		from
			#tbl1 t 
			join Subscriptions s  with (NOLOCK) on t.IGRP_NO = s.IGRP_NO 
			join api_incomingdata a  with (NOLOCK) on t.IncomingDataID = a.IncomingDataID 
		where 
			t.SubscriptionID is null and  
			t.Igrp_Rank = 'M'   
	    
	    Print ('Insert [Subscriptions_DQM] COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
	    
	    Update t
	    set t.SubscriptionID = s.subscriptionID
		FROM #tbl1 t join Subscriptions s WITH (nolock) on t.IGRP_NO = s.IGRP_NO
		WHERE t.SubscriptionID is null

		Print ('Update #tbl1 SubscriptioniID COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
	    
		UPDATE ps
		SET [demo7] = case when isnull(a.demo7,'') = '' then 'A' else a.demo7 end
			,[Qualificationdate] = a.QDate
			,[PubCategoryID] = case when CONVERT(INT,a.CAT) =0  then NULL else CONVERT(INT,a.CAT) end
			,[PubTransactionID] = CONVERT(INT,a.XACT)
			,[Email] = a.EMAIL
			,[EmailStatusID] =  case when es.EmailStatusID is null then 1 else  es.EmailStatusID end
			,[StatusUpdatedDate] = case when ps.emailstatusID = (case when es.EmailStatusID is null then 1 else  es.EmailStatusID end) then ps.StatusUpdatedDate else GETDATE() end
		from 
				PubSubscriptions ps 
				join #tbl1 t on ps.PubSubscriptionID = t.PubSubscriptionID 
				join API_IncomingData a on a.IncomingDataID = t.IncomingDataID
				left outer join EmailStatus es on es.Status = a.EMAILSTATUS
				
	    Print ('Update pubsubscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
	    
	    select *
		from
				#tbl1 t
		Where 
				t.SubscriptionID is null
	    
	    INSERT INTO pubsubscriptions 
	    (
			SubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID, PubCategoryID,PubTransactionID,Email,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason
		)
		select t.SubscriptionID , t.PubID, case when isnull(a.demo7,'') = '' then 'A' else a.demo7 end, a.QDATE, 
		DBO.FN_GetQSourceID(a.QSOURCE), 
		case when CONVERT(INT,a.CAT) =0  then NULL else CONVERT(INT,a.CAT) end, CONVERT(INT,a.XACT) as TransactionID,
		a.Email,
		case when es.EmailStatusID is null then 1 else es.EmailStatusID end, a.QDATE, ''
		from
				#tbl1 t
				join API_IncomingData a on t.IncomingDataID = a.IncomingDataID
				left outer join EmailStatus es on es.Status = a.EMAILSTATUS
		Where 
				t.PubSubscriptionID is null
	    
	    Print ('Insert pubsubscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
		    
		update t
		set t.PubSubscriptionID = ps.PubSubscriptionID
		from #tbl1 t join PubSubscriptions ps on t.PubID = ps.PubID and t.SubscriptionID = ps.SubscriptionID
		where t.PubSubscriptionID is null

		Print ('Update @tbl PubSubscriptionID COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
	    
	    declare @rgname varchar(50),
				@PubSubscriptionID int, 
				@pubID int, 
				@responsevalue varchar(max)
		
		DECLARE c_ProductDemographics CURSOR FOR 
		select distinct ResponseGroupName from responsegroups 

		OPEN c_ProductDemographics  
		FETCH NEXT FROM c_ProductDemographics INTO @rgname

		WHILE @@FETCH_STATUS = 0  
		BEGIN  	
		
			truncate table #tmpProductDemographics
			
			exec ('
			insert into #tmpIncomingDataDetails	 (PubSubscriptionID, pubid, responsegroup, responsevalue, notExists)
			select distinct t.PubSubscriptionID, p.pubID, ''' + @rgname + ''', a.[' + @rgname + '], 1 from #tbl1 t join API_IncomingData a on a.IncomingDataID = t.IncomingDataID
			join Pubs p on p.pubID = t.pubID join ResponseGroups rg on p.PubID = rg.PubID
			where rg.ResponseGroupName = ''' + @rgname + ''' and LTRIM(rtrim(a.[' + @rgname + '])) <> '''' and CHARINDEX('','', a.[' + @rgname + ']) = 0')
			 
			exec ('
			insert into #tmpProductDemographics 
			select t.PubSubscriptionID, p.pubID, a.[' + @rgname + '] from #tbl1 t join API_IncomingData a on a.IncomingDataID = t.IncomingDataID
			join Pubs p on p.pubID = t.pubID join ResponseGroups rg on p.PubID = rg.PubID
			where rg.ResponseGroupName = ''' + @rgname + ''' and LTRIM(rtrim(a.[' + @rgname + '])) <> '''' and CHARINDEX('','', a.[' + @rgname + ']) > 0')
			
			DECLARE c_ProductDemographicsData CURSOR FOR 
			select PubSubscriptionID , pubID , responsevalue  from  #tmpProductDemographics  
			
			OPEN c_ProductDemographicsData  
			FETCH NEXT FROM c_ProductDemographicsData INTO @PubSubscriptionID, @pubID, @responsevalue

			WHILE @@FETCH_STATUS = 0  
			BEGIN  					
				insert into #tmpIncomingDataDetails (PubSubscriptionID, pubid, responsegroup, responsevalue, NotExists)
				select @PubSubscriptionID, @pubId, @rgname, (case when PATINDEX('%[^0-9]%', items) = 0 and (Items not like '%$%' and Items not like '%.%') then CONVERT(varchar(100),CONVERT(int,items)) else items end), 1  
				from dbo.fn_split(@RESPONSEVALUE, ',')
			
				FETCH NEXT FROM c_ProductDemographicsData INTO  @PubSubscriptionID, @pubID, @responsevalue
			END

			CLOSE c_ProductDemographicsData  
			DEALLOCATE c_ProductDemographicsData  
			
			FETCH NEXT FROM c_ProductDemographics INTO  @rgname
		END

		CLOSE c_ProductDemographics  
		DEALLOCATE c_ProductDemographics  
		
		Print ('c_ProductDemographics complete COUNT : ' + ' / ' + convert(varchar(20), getdate(), 114))
		
		update #tmpIncomingDataDetails 
		set responsevalue = REPLACE(responsevalue, ' ' ,'')
		
		insert into PubSubscriptionDetail (PubSubscriptionID, SubscriptionID, CodeSheetID)
		select   ps.PubSubscriptionID, ps.SubscriptionID, cs.CodeSheetID--, idetail.responsegroup, idetail.responsevalue
		from 
			PubSubscriptions ps 
			join #tmpIncomingDataDetails idetail on ps.PubSubscriptionID = idetail.PubSubscriptionID 
			join ResponseGroups rg on idetail.responsegroup = rg.ResponseGroupName and idetail.pubid = rg.PubID
			join CodeSheet cs on rg.ResponseGroupID = cs.ResponseGroupID and rg.PubID = cs.PubID and 
				(case when LEN(idetail.responsevalue)= 1 and ISNUMERIC(idetail.responsevalue) = 1 then '0' + idetail.Responsevalue else idetail.responsevalue end) 
				= 
				(case when LEN(cs.responsevalue)= 1 and ISNUMERIC(cs.responsevalue) = 1 then '0' + cs.Responsevalue else cs.responsevalue end)
		order by ps.subscriptionID, cs.codesheetID	    
	    
	    Print ('Insert PubSubscriptionDetail COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		insert into SubscriptionDetails (SubscriptionID, MasterID)
		select distinct psd.SubscriptionID, cmb.masterID 
		from 
				(select distinct subscriptionID from #tbl1) t  
				join PubSubscriptionDetail psd on t.SubscriptionID = psd.SubscriptionID 
				join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
				left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
		where  
				sd.sdID is null	    
		
		Print ('Insert SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		/***** Final Step *****/
	    
		insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
		SELECT 
		  MasterGroupID, [SubscriptionID] , 
		  STUFF((
			SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
			FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
			WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
			FOR XML PATH (''))
		  ,1,1,'') AS CombinedValues
		FROM 
			(
				SELECT distinct sd.SubscriptionID, mc.MasterGroupID
				FROM	
						(select distinct subscriptionID from #tbl1) t 
						join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.SubscriptionID
						join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID 			 
			)
		 Results
		GROUP BY [SubscriptionID] , MasterGroupID
		order by SubscriptionID    
	    
		Print ('Insert into SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
	    
	    /*
		********** Insert/Update SubscriptionsExtension
		*/
		
		if exists (select top 1 * from SubscriptionsExtensionMapper where Active = 1)
		Begin
		
			DECLARE @ColumnName VARCHAR(255)
			DECLARE @FieldName VARCHAR(10)
			DECLARE @ColumnNamesCsv AS VARCHAR(MAX)
			DECLARE @FieldNamesCsv AS VARCHAR(MAX)
			DECLARE @WhereOneIsNotNull AS VARCHAR(MAX),
					@Updatestring AS VARCHAR(MAX)
						
			
			--copy data out of the old table into the new extenstion table
			DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT CustomField, StandardField FROM SubscriptionsExtensionMapper  where Active = 1
			OPEN c
			FETCH NEXT FROM c INTO @ColumnName, @FieldName
			WHILE @@FETCH_STATUS = 0  
			BEGIN 

--				set @Updatestring =  ISNULL(@Updatestring + ', ' + @FieldName + ' = [' + @ColumnName + ']', @FieldName + ' = [' + @ColumnName + ']') 		
--			    SET @Updatestring =  ISNULL(@Updatestring + ', ' + @FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') ='''' THEN [' + @fieldName + '] ELSE [' + @ColumnName + '] END ', @FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') = '''' THEN [' + @fieldName + '] ELSE [' + @ColumnName + '] END ')
                SET @Updatestring =  ISNULL(@Updatestring + ', ' + @FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') ='''' or ISNULL([' + @ColumnName + '],'''' ) = ISNULL([' + @fieldName + '], '''') THEN [' + @fieldName + '] WHEN ISNULL([' + @FieldName + '], '''') = '''' THEN CAST([' + @ColumnName + '] AS VARCHAR(2048)) ELSE CAST(LTRIM(RTRIM([' + @FieldName + '])) + '','' + LTRIM(RTRIM([' + @ColumnName + '])) AS VARCHAR(2048)) END',@FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') ='''' or ISNULL([' + @ColumnName + '], '''') = ISNULL([' + @fieldName + '],'''') THEN [' + @fieldName + '] WHEN ISNULL([' + @FieldName + '], '''') = '''' THEN CAST([' + @ColumnName + '] AS VARCHAR(2048)) ELSE CAST(LTRIM(RTRIM([' + @FieldName + '])) + '','' + LTRIM(RTRIM([' + @ColumnName + '])) AS VARCHAR(2048)) END')
				SET @ColumnNamesCsv = ISNULL(@ColumnNamesCsv + ', [' + @ColumnName + ']', '[' + @ColumnName + ']')
				SET @FieldNamesCsv = ISNULL(@FieldNamesCsv + ', ' + @FieldName, @FieldName)
				
				IF(@WhereOneIsNotNull IS NULL)
				BEGIN
					SET @WhereOneIsNotNull = '[' + @ColumnName + ']' + ' IS NOT NULL'
				END
				ELSE
				BEGIN
					SET @WhereOneIsNotNull = @WhereOneIsNotNull + ' OR ' + '[' + @ColumnName + ']' + ' IS NOT NULL'
				END
			
				FETCH NEXT FROM c INTO @ColumnName, @FieldName 
			END 
			CLOSE c
			DEALLOCATE c
			
			EXEC ('Update s1 Set ' + @Updatestring + '
			FROM SubscriptionsExtension s1 join #tbl1 t on s1.subscriptionID = t.subscriptionID join api_incomingdata a on t.incomingdataID = a.incomingdataID
			WHERE t.igrp_rank=''M''')
		
			Print ('Update SubscriptionsExtension COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
		
			EXEC ('INSERT INTO SubscriptionsExtension (SubscriptionID, ' + @FieldNamesCsv + ') 
			SELECT t.subscriptionID, ' + @ColumnNamesCsv + '
			FROM #tbl1 t join api_incomingdata a on t.incomingdataID = a.incomingdataID left outer join SubscriptionsExtension se on se.subscriptionID = t.subscriptionID
			WHERE t.igrp_rank=''M'' AND se.subscriptionID is null AND (' + @WhereOneIsNotNull + ')')

     		Print ('Insert SubscriptionsExtension COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
			
			update a
			set a.processed = 1
			from API_IncomingData a join #tbl1 t on a.IncomingDataID = t.IncomingDataID
			where t.PubSubscriptionID is not null
			
			Print ('Update API_IncomingData COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
			
		End
	    
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage;	
		
		Print 'ERROR'
			
		ROLLBACK TRANSACTION;
		
		SET @s = 'API Import Notification Failed ' + convert(varchar(100),DB_NAME());
		SELECT @b = ERROR_MESSAGE() 

  --  EXEC msdb..sp_send_dbmail 
		--@profile_name='SQLAdmin', 
		--@recipients='sunil@knowledgemarketing.com', 
		--@importance='High',
		--@body_format = 'HTML',
		--@subject= @s, 
		--@body=@b
    
		
	END CATCH;


	drop table #tmpIncomingDataDetails 

	drop table #tmpProductDemographics 
	
	drop table #tbl1

End

GO
PRINT N'Altering [dbo].[e_MasterGroup_Save]...';


GO
ALTER proc [dbo].[e_MasterGroup_Save]
	@MasterGroupID int,
	@DisplayName varchar(50), 
	@Name varchar(100), 
	@IsActive bit , 
	@EnableSubReporting bit, 
	@EnableSearching bit, 
	@EnableAdhocSearch bit,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
as
Begin
	SET NOCOUNT ON;
	if @MasterGroupID > 0
	begin
		UPDATE [MasterGroups] 
		SET [DisplayName] = @DisplayName, 
			[Name] = @Name, 
			[Description] = @Name,  
			[IsActive] = @IsActive, 
			[EnableSubReporting] = @EnableSubReporting, 
			[EnableSearching] = @EnableSearching, 
			[EnableAdhocSearch] = @EnableAdhocSearch 
		WHERE [MasterGroupID] = @MasterGroupID
		select @MasterGroupID;
	end
	else
	begin
		INSERT INTO 
		[MasterGroups] ([DisplayName], [Name], [Description], [IsActive],[EnableSubReporting], [EnableSearching], [EnableAdhocSearch], ColumnReference,DateCreated,CreatedByUserID) 
		VALUES ( @DisplayName, @Name, @Name, @IsActive, @EnableSubReporting, @EnableSearching, @EnableAdhocSearch, 'MASTER_' + REPLACE (@Name, ' ', '_'),@DateCreated,@CreatedByUserID)
		select @@IDENTITY;
    end
END
GO
PRINT N'Altering [dbo].[rpt_SourceFile_PubCodeSummary]...';


GO
ALTER PROCEDURE rpt_SourceFile_PubCodeSummary

	@SourceFileID int,
	@ProcessCode  varchar(50),
	@FileName varchar(100)

AS

SET NOCOUNT ON

DECLARE	@SourceFileID_Local int,
		@ProcessCode_Local  varchar(50),
		@FileName_Local varchar(100)

SET @SourceFileID_Local = @SourceFileID
SET @ProcessCode_Local = @ProcessCode
SET @FileName_Local = @FileName

SELECT	
	@SourceFileID_Local as SourceFileID, 
	@FileName_Local as FileName, 
	x.ProcessCode, 
	LastUploaded,  
	TransformedPubCode, 
	ISNULL(OriginalCount,0) as 'OriginalCount', 
	ISNULL(TransformedCount,0) as 'TransformedCount', 
	ISNULL(ArchivedCount,0) as 'ArchivedCount', 
	ISNULL(InvalidCount,0) as 'InvalidCount', 
	ISNULL(FinalCount,0) as 'FinalCount', 
	ISNULL(LiveCount,0) as 'LiveCount',
	ISNULL(MasterCount,0) as 'MasterCount', 
	ISNULL(SubordinateCount,0) as 'SubordinateCount'
FROM 
	(
		SELECT	so.SourceFileID, so.ProcessCode,  
				st.PubCode as 'TransformedPubCode', COUNT(SubscriberOriginalID) as 'OriginalCount',
				COUNT(SubscriberTransformedID) as 'TransformedCount',MAX(so.DateCreated) as 'LastUploaded'
		FROM	dbo.SubscriberOriginal so with (NOLOCK) 
				left outer join dbo.subscriberTransformed st with (NOLOCK) on st.SourceFileID = so.SourceFileID and st.ProcessCode = so.ProcessCode and st.SORecordIdentifier = so.SORecordIdentifier
		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
		GROUP BY so.SourceFileID, so.ProcessCode,st.PubCode
	)
	x
LEFT OUTER JOIN
	(
		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'FinalCount'
		FROM dbo.SubscriberFinal so with (NOLOCK)
		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
		)
	z on z.SourceFileID = x.SourceFileID and z.PubCode = x.TransformedPubCode and z.ProcessCode  = x.ProcessCode
LEFT OUTER JOIN
	(
		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberInvalidID),0) as 'InvalidCount'
		FROM dbo.SubscriberInvalid so with (NOLOCK)
		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
		)
	xx on xx.SourceFileID = x.SourceFileID and xx.PubCode = x.TransformedPubCode and xx.ProcessCode  = x.ProcessCode
LEFT OUTER JOIN
	(
		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
		FROM dbo.SubscriberArchive so with (NOLOCK)
		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
		)
	y on y.SourceFileID = x.SourceFileID and y.PubCode = x.TransformedPubCode and y.ProcessCode  = x.ProcessCode
LEFT OUTER JOIN
	(
		SELECT P.pubcode, ISNULL(COUNT(ps.subscriptionID),0) as 'LiveCount'
		FROM	dbo.PubSubscriptions ps with (NOLOCK) 
				join dbo.Pubs p on p.pubID = ps.pubID
		GROUP BY P.pubcode
	)
	a on a.PubCode = z.PubCode
LEFT OUTER JOIN
	(
		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'MasterCount'
		FROM dbo.SubscriberFinal so with (NOLOCK)
		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
		AND so.IGrp_Rank = 'M'
		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
		)
	m on m.SourceFileID = x.SourceFileID and m.PubCode = x.TransformedPubCode and m.ProcessCode  = x.ProcessCode
LEFT OUTER JOIN
	(
		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'SubordinateCount'
		FROM dbo.SubscriberFinal so with (NOLOCK)
		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
		AND so.IGrp_Rank = 'S'
		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
		)
	s on s.SourceFileID = x.SourceFileID and s.PubCode = x.TransformedPubCode and s.ProcessCode  = x.ProcessCode
ORDER BY 
	OriginalCount ASC
--CREATE PROCEDURE rpt_SourceFile_PubCodeSummary

--	@SourceFileID int,
--	@ProcessCode  varchar(50)

--AS

--SET NOCOUNT ON

--DECLARE	@SourceFileID_Local int,
--		@ProcessCode_Local  varchar(50)

--SET @SourceFileID_Local = @SourceFileID
--SET @ProcessCode_Local = @ProcessCode

--SELECT	
--	sf.SourceFileID, 
--	sf.FileName, 
--	x.ProcessCode, 
--	LastUploaded,  
--	TransformedPubCode, 
--	ISNULL(OriginalCount,0) as 'OriginalCount', 
--	ISNULL(TransformedCount,0) as 'TransformedCount', 
--	ISNULL(ArchivedCount,0) as 'ArchivedCount', 
--	ISNULL(InvalidCount,0) as 'InvalidCount', 
--	ISNULL(FinalCount,0) as 'FinalCount', 
--	ISNULL(LiveCount,0) as 'LiveCount',
--	ISNULL(MasterCount,0) as 'MasterCount', 
--	ISNULL(SubordinateCount,0) as 'SubordinateCount'
--FROM 
--	UAS.dbo.sourcefile sf WITH(NoLock)
--JOIN
--	(
--		SELECT	so.SourceFileID, so.ProcessCode,  
--				st.PubCode as 'TransformedPubCode', COUNT(SubscriberOriginalID) as 'OriginalCount',
--				COUNT(SubscriberTransformedID) as 'TransformedCount',MAX(so.DateCreated) as 'LastUploaded'
--		FROM	dbo.SubscriberOriginal so with (NOLOCK) 
--				left outer join dbo.subscriberTransformed st with (NOLOCK) on st.SourceFileID = so.SourceFileID and st.ProcessCode = so.ProcessCode and st.SORecordIdentifier = so.SORecordIdentifier
--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
--		GROUP BY so.SourceFileID, so.ProcessCode,st.PubCode
--	)
--	x on x.SourceFileID = sf.SourceFileID
--LEFT OUTER JOIN
--	(
--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'FinalCount'
--		FROM dbo.SubscriberFinal so with (NOLOCK)
--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
--		)
--	z on z.SourceFileID = x.SourceFileID and z.PubCode = x.TransformedPubCode and z.ProcessCode  = x.ProcessCode
--LEFT OUTER JOIN
--	(
--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberInvalidID),0) as 'InvalidCount'
--		FROM dbo.SubscriberInvalid so with (NOLOCK)
--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
--		)
--	xx on xx.SourceFileID = x.SourceFileID and xx.PubCode = x.TransformedPubCode and xx.ProcessCode  = x.ProcessCode
--LEFT OUTER JOIN
--	(
--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
--		FROM dbo.SubscriberArchive so with (NOLOCK)
--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
--		)
--	y on y.SourceFileID = x.SourceFileID and y.PubCode = x.TransformedPubCode and y.ProcessCode  = x.ProcessCode
--LEFT OUTER JOIN
--	(
--		SELECT P.pubcode, ISNULL(COUNT(ps.subscriptionID),0) as 'LiveCount'
--		FROM	dbo.PubSubscriptions ps with (NOLOCK) 
--				join dbo.Pubs p on p.pubID = ps.pubID
--		GROUP BY P.pubcode
--	)
--	a on a.PubCode = z.PubCode
--LEFT OUTER JOIN
--	(
--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'MasterCount'
--		FROM dbo.SubscriberFinal so with (NOLOCK)
--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
--		AND so.IGrp_Rank = 'M'
--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
--		)
--	m on m.SourceFileID = x.SourceFileID and m.PubCode = x.TransformedPubCode and m.ProcessCode  = x.ProcessCode
--LEFT OUTER JOIN
--	(
--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'SubordinateCount'
--		FROM dbo.SubscriberFinal so with (NOLOCK)
--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
--		AND so.IGrp_Rank = 'S'
--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
--		)
--	s on s.SourceFileID = x.SourceFileID and s.PubCode = x.TransformedPubCode and s.ProcessCode  = x.ProcessCode
--WHERE 
--	sf.SourceFileID = @SourceFileID_Local 
--ORDER BY 
--	OriginalCount ASC
GO
PRINT N'Altering [dbo].[e_ResponseGroup_Save]...';


GO
ALTER procedure e_ResponseGroup_Save
	@ResponseGroupID int,
	@PubID int,
	@ResponseGroupName varchar(100),
	@DisplayName varchar(100),
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int,
	@DisplayOrder int,
	@IsMultipleValue bit,
	@IsRequired bit,
	@IsActive bit,
	@WQT_ResponseGroupID int,
	@ResponseGroupTypeId int = null
as
	if(@ResponseGroupID > 0)
		begin
			update ResponseGroups
			set ResponseGroupName = @ResponseGroupName,
				DisplayName = @DisplayName,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DisplayOrder = @DisplayOrder,
				IsMultipleValue = @IsMultipleValue,
				IsRequired = @IsRequired,
				IsActive = @IsActive,
				WQT_ResponseGroupID = @WQT_ResponseGroupID,
				ResponseGroupTypeId = @ResponseGroupTypeId
			where ResponseGroupID = @ResponseGroupID
			
			select @ResponseGroupID;
		end
	else
		begin
			insert into ResponseGroups (PubID,ResponseGroupName,DisplayName, DateCreated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, IsActive, WQT_ResponseGroupID, ResponseGroupTypeId)
			values(@PubID,@ResponseGroupName,@DisplayName, @DateCreated, @CreatedByUserID, @DisplayOrder, @IsMultipleValue, @IsRequired, @IsActive, @WQT_ResponseGroupID, @ResponseGroupTypeId);select @@IDENTITY;
		end
GO
PRINT N'Altering [dbo].[ccp_NASFT_Company]...';


GO
ALTER PROCEDURE [dbo].[ccp_NASFT_Company]
@SourceFileID int
AS
	DECLARE @ClientID int = (SELECT ClientID FROM UAS..Client With(NoLock) WHERE ClientName = 'NASFT')
	DECLARE @PubID int = CASE WHEN EXISTS(SELECT ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							THEN(SELECT ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							ELSE 0
						END		
						   
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	SELECT * FROM
	(
	SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'BRNFLAG' as MAFField,
	(	SELECT CASE
			WHEN EXISTS(SELECT TOP 1 d.DimensionValue 
				FROM AdHocDimension d With(NoLock)
				WHERE d.ClientID = @ClientID 
				AND d.IsActive = 'true'
				AND d.StandardField = 'COMPANY'
				AND d.CreatedDimension = 'BRNFLAG'
				AND (dbo.fn_Levenshtein(st.Company, d.MatchValue) > 75))
			THEN 'Y'
			ELSE 'N'
			END
	) as Value,
	GETDATE() as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	) as A
	WHERE A.Value != ''
GO
PRINT N'Altering [dbo].[ccp_NASFT_KBCompany]...';


GO
ALTER PROCEDURE [dbo].[ccp_NASFT_KBCompany]
@SourceFileID int
AS
	DECLARE @ClientID int = (SELECT ClientID FROM UAS..Client With(NoLock) WHERE ClientName = 'NASFT')
	DECLARE @PubID int = CASE WHEN EXISTS(SELECT ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							THEN(SELECT ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							ELSE 0
						END		
						   
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	SELECT * FROM
	(
	SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'KBFLAG' as MAFField,
	(	SELECT CASE
			WHEN EXISTS(SELECT TOP 1 d.DimensionValue 
				FROM AdHocDimension d With(NoLock)
				WHERE d.ClientID = @ClientID
				AND d.IsActive = 'true'
				AND d.StandardField = 'COMPANY'
				AND d.CreatedDimension = 'KBFLAG'
				AND (dbo.fn_Levenshtein(st.Company, d.MatchValue) > 75))
			THEN 'Y'
			ELSE 'N'
			END
	)as VAlue,
	GETDATE() as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	) as A
	WHERE A.Value != ''
GO
PRINT N'Altering [dbo].[ccp_NASFT_SUM_TOTS]...';


GO
ALTER PROCEDURE [dbo].[ccp_NASFT_SUM_TOTS]
AS
/** NOTES **/
-----
---Process 1-5 will attach to record and if there are duplicates for one person
---EXAMPLE: Steve Jobs PUBCODE: "CCE12" IGRP_NO: "D32K..." has 2 records with TOTALSPENT’s of 50 and 100
---They will be summed to 150 and attached to both then when the duplicates are 
---removed the master? record will be left with a TOTALSPENT of 150
-----
-----
---Process 6 will attach to the “Master Record” for that specific IGRP_NO
---EXAMPLE: IGRP_NO: "D32K..." has 4 records. TOTALSPENT's of 50, 75, 100, 25 sum to 250 and
---attached in the TOTSPALL Field to the Master Record.
-----

/** Process 1: TOTALSPENT - IGRP_NO & Pubcode **/
IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_TotalSpent'))
BEGIN
	DROP TABLE #tmp_TotalSpent;
END

SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_TotalSpent
FROM SubscriberTransformed SF With(NoLock)
JOIN SubscriberDemographicTransformed SDF With(NoLock)
ON SF.SORecordIdentifier = SDF.SORecordIdentifier
WHERE SDF.MAFField = 'TOTALSPENT'
GROUP BY PubCode, IGrp_No

Update SubscriberDemographicTransformed
SET Value = t.SUMS
FROM SubscriberTransformed sf
JOIN #tmp_TotalSpent t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'TOTALSPENT'

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects s WHERE s.xtype in ('U') and s.id = object_id('tempdb..#tmp_TotalSpent'))
BEGIN
	DROP TABLE #tmp_TotalSpent;
END



/** Process 2: BOOTHS - IGRP_NO & Pubcode **/
IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Booths'))
BEGIN
	DROP TABLE #tmp_Booths;
END

SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_Booths
FROM SubscriberTransformed SF With(NoLock)
JOIN SubscriberDemographicTransformed SDF With(NoLock)
ON SF.SORecordIdentifier = SDF.SORecordIdentifier
WHERE SDF.MAFField = 'BOOTHS'
GROUP BY PubCode, IGrp_No

Update SubscriberDemographicTransformed
SET Value = t.SUMS
FROM SubscriberTransformed sf
JOIN #tmp_Booths t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'BOOTHS'

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Booths'))
BEGIN
	DROP TABLE #tmp_Booths;
END



/** Process 3: TOTWB - IGRP_NO & Pubcode **/
IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwb'))
BEGIN
	DROP TABLE #tmp_Totwb;
END

SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_Totwb
FROM SubscriberTransformed SF With(NoLock)
JOIN SubscriberDemographicTransformed SDF With(NoLock)
ON SF.SORecordIdentifier = SDF.SORecordIdentifier
WHERE SDF.MAFField = 'TOTWB'
GROUP BY PubCode, IGrp_No

Update SubscriberDemographicTransformed
SET Value = t.SUMS
FROM SubscriberTransformed sf
JOIN #tmp_Totwb t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'TOTWB'

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwb'))
BEGIN
	DROP TABLE #tmp_Totwb;
END



/** Process 4: TOTWP - IGRP_NO & Pubcode **/
IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwp'))
BEGIN
	DROP TABLE #tmp_Totwp;
END

SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_Totwp
FROM SubscriberTransformed SF With(NoLock)
JOIN SubscriberDemographicTransformed SDF With(NoLock)
ON SF.SORecordIdentifier = SDF.SORecordIdentifier
WHERE SDF.MAFField = 'TOTWP'
GROUP BY PubCode, IGrp_No

Update SubscriberDemographicTransformed
SET Value = t.SUMS
FROM SubscriberTransformed sf
JOIN #tmp_Totwp t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sf.ClientID = 15 AND sdf.MAFField = 'TOTWP'

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwp'))
BEGIN
	DROP TABLE #tmp_Totwp;
END



/** Process 5: RS - IGRP_NO & Pubcode **/
IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_rs'))
BEGIN
	DROP TABLE #tmp_rs;
END

SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_rs
FROM SubscriberTransformed SF With(NoLock)
JOIN SubscriberDemographicTransformed SDF With(NoLock)
ON SF.SORecordIdentifier = SDF.SORecordIdentifier
WHERE SDF.MAFField = 'RS'
GROUP BY PubCode, IGrp_No

Update SubscriberDemographicTransformed
SET Value = t.SUMS
FROM SubscriberTransformed sf
JOIN #tmp_rs t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'RS'

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_rs'))
BEGIN
	DROP TABLE #tmp_rs;
END



/** Process 6: TOTALSPENT - IGRP_NO **/
IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_TotalSpent2'))
BEGIN
	DROP TABLE #tmp_TotalSpent2;
END

SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.IGrp_No INTO #tmp_TotalSpent2
FROM SubscriberTransformed SF With(NoLock)
JOIN SubscriberDemographicTransformed SDF With(NoLock)
ON SF.SORecordIdentifier = SDF.SORecordIdentifier
WHERE SDF.MAFField = 'TOTALSPENT'
GROUP BY IGrp_No

Update SubscriberDemographicTransformed
SET Value = t.SUMS
FROM SubscriberTransformed sf
JOIN #tmp_TotalSpent2 t ON sf.IGrp_No = t.IGrp_No
JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
WHERE sf.IGrp_No = t.IGrp_No AND sdf.MAFField = 'TOTSPALL'

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_TotalSpent2'))
BEGIN
	DROP TABLE #tmp_TotalSpent2;
END
GO
PRINT N'Altering [dbo].[ccp_WATT_Process_MacMic]...';


GO
ALTER PROCEDURE [dbo].[ccp_WATT_Process_MacMic]
@Xml xml
AS
SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		Pubcode varchar(500),
		FoxColumnName varchar(500),		
		CodeSheetValue varchar(8000)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 Pubcode,FoxColumnName,CodeSheetValue
	)  
	
	SELECT 
		Pubcode,FoxColumnName,CodeSheetValue
	FROM OPENXML(@docHandle, N'/XML/WATT')  
	WITH   
	(
		Pubcode varchar(500) 'Pubcode',
		FoxColumnName varchar(500) 'FoxColumnName',
		CodeSheetValue varchar(8000) 'CodeSheetValue'
	)  
	EXEC sp_xml_removedocument @docHandle    

	Insert into tempWATTMicTable
	SELECT * FROM @import WHERE FoxColumnName = 'Micro'
	
	Insert into tempWATTMacTable
	SELECT * FROM @import WHERE FoxColumnName = 'Macro'

	INSERT INTO tempWattMicTableFinal (Pubcode,FoxColumnName,CodeSheetValue)	
		SELECT * FROM 
		(
			SELECT Pubcode,'MICRO' as FoxColumnName,
				[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.CodeSheetValue AS [text()] FROM tempWattMicTable SUB
				WHERE SUB.Pubcode = MAIN.Pubcode For Xml Path(''), type
				).value('.', 'nvarchar(max)')
				, 1, 1, ''), ',') As CodeSheetValue
			FROM tempWattMicTable MAIN With(NoLock)
			GROUP BY Pubcode,FoxColumnName
		) as A
		WHERE A.CodeSheetValue != '' AND A.CodeSheetValue is not NULL

		
	INSERT INTO tempWattMacTableFinal (Pubcode,FoxColumnName,CodeSheetValue)
		SELECT * FROM 
		(	
			SELECT Pubcode,'MACRO' as FoxColumnName,
				[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.CodeSheetValue AS [text()] FROM tempWattMacTable SUB
				WHERE SUB.Pubcode = MAIN.Pubcode For Xml Path(''), type
				).value('.', 'nvarchar(max)')
				, 1, 1, ''), ',') As CodeSheetValue
			FROM tempWattMacTable MAIN With(NoLock)
			GROUP BY Pubcode,FoxColumnName
		) as B
		WHERE B.CodeSheetValue != '' AND B.CodeSheetValue is not NULL
GO
PRINT N'Altering [dbo].[ccp_WATT_Sicalpha]...';


GO
ALTER PROCEDURE [dbo].[ccp_WATT_Sicalpha]
@SourceFileID int
AS
	
	DECLARE @ClientID int = (SELECT ClientID FROM UAS..Client With(NoLock) WHERE ClientName = 'Watt')
	DECLARE @PubID int = (SELECT TOP 1 ISNULL(PubID,-1)
						  FROM SubscriberDemographicTransformed d With(NoLock)
						  JOIN SubscriberTransformed st With(NoLock) ON d.STRecordIdentifier = st.STRecordIdentifier)
						   
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	SELECT * from
	(
		SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,sdt.MAFField,
		(
			SELECT TOP 1 d.DimensionValue 
			FROM AdHocDimension d With(NoLock)		
			WHERE d.ClientID = @ClientID
			AND d.IsActive = 'true'
			AND d.StandardField = 'SIC'
			AND d.CreatedDimension = 'SICALPHA'
			AND st.Title = d.MatchValue		
		) as Value,
		sdt.DateCreated,sdt.CreatedByUserId
		FROM SubscriberTransformed st With(NoLock)
		FULL JOIN SubscriberDemographicTransformed sdt With(NoLock) on st.SORecordIdentifier = sdt.SORecordIdentifier
		WHERE sdt.MAFField = 'SICALPHA' 
	) as A
GO
PRINT N'Altering [dbo].[e_ClientCustomProcedure_ExecuteSproc]...';


GO
ALTER PROCEDURE e_ClientCustomProcedure_ExecuteSproc
@sproc varchar(250),
@srcFile int,
@ProcessCode varchar(50) = ''
AS
	DECLARE @execsproc varchar(MAX)
	IF(LEN(@ProcessCode) > 0)
		begin
			SET @execsproc = 'EXEC '+@sproc + ' '+ CAST(@srcFile AS varchar(25)) + ', ' + '''' + @ProcessCode + ''''
		end
	else
		begin
			SET @execsproc = 'EXEC '+@sproc + ' '+ CAST(@srcFile AS varchar(25)) + ''
		end

	Exec(@execsproc)
GO
PRINT N'Altering [dbo].[e_DupeCleanUp]...';


GO

ALTER PROCEDURE e_DupeCleanUp
WITH EXECUTE as OWNER
AS

	--SET NOCOUNT ON;

	--declare     @i int,
	--			@KeepSubscriptionID int,
	--			@KeepPubsubscriptionID int,
	--			@pubID int,
	--			@pubsubscriptionID int,
	--			@codesheetID int

	--set @i = 1
	--print (' Process Dupes : ' +  convert(varchar,@i)  + ' / ' +  convert(varchar(20), getdate(), 114))

	--set nocount on

	--create table #curtable (KeepSubscriptionID int, DupeSubscriptionID int) 

	--CREATE INDEX IDX_Users_UserName ON #curtable(KeepSubscriptionID)
	
	--create table #NameSwapID(SubId int,DupeSubId int)
		
	--declare @tmp table (SubscriptionID int UNIQUE CLUSTERED (SubscriptionID))     

	--declare @loopcounter int = 1

	--while @loopcounter <= 15
	--Begin

	--	  delete from #curtable
	--	  delete from #NameSwapID

	--	  if @loopcounter = 1 -- MATCH ON COMLETE FNAME,LNAME,COMPANY, ADDRESS, CITY, STATE, ZIP, PHONE, EMAIL
	--	  Begin
	--			insert into #curtable
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select isnull(FNAME,'') FNAME, isnull(LNAME,'') LNAME, isnull(COMPANY,'') COMPANY, isnull(ADDRESS,'') ADDRESS, isnull(CITY,'')CITY, isnull(State,'')State, 
	--			isnull(ZIP,'') ZIP, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			group by
	--			isnull(FNAME,''), isnull(LNAME,''), isnull(COMPANY,''), isnull(ADDRESS,''), isnull(CITY,''), isnull(State,''), isnull(ZIP,''), isnull(PHONE,''), isnull(EMAIL,'')
	--			having COUNT(subscriptionID) > 1
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on isnull(s.FNAME,'') = x.FNAME and 
	--							  isnull(s.LNAME,'') = x.LNAME and 
	--							  isnull(s.COMPANY,'') = x.COMPANY and 
	--							  isnull(s.ADDRESS,'') = x.ADDRESS and 
	--							  isnull(s.CITY,'') = x.CITY and 
	--							  isnull(s.State,'') = x.State and 
	--							  isnull(s.ZIP,'') = x.ZIP and 
	--							  isnull(s.PHONE,'') = x.PHONE and 
	--							  isnull(s.EMAIL,'') = x.EMAIL        
	--			--where minsubscriptionID = 17840412      
	--			order by minsubscriptionID    
	                        
	--	  End
	--	  else if @loopcounter = 2 -- COMLETE FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #curtable
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname,ISNULL("ADDRESS",'') "ADDRESS",ZIP,CountryID, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
	--			group by FNAME,LNAME,"ADDRESS",ZIP,CountryID
	--			having COUNT(*) > 1 
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on 
	--							  isnull(s.FNAME,'') = isnull(x.FNAME,'') and 
	--							  isnull(s.LNAME,'') = isnull(x.LNAME,'') and 
	--							  isnull(s.ADDRESS,'') = isnull(x.ADDRESS,'') and 
	--							  s.ZIP = x.ZIP and 
	--							  ISNULL(x.CountryID,0) = ISNULL(S.CountryID,0)
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 3 -- MATCH FIELDS FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname,left(lname,6) lname,left("ADDRESS",15) "ADDRESS",ZIP, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
	--			group by left(fname,3),left(lname,6),left("ADDRESS",15),ZIP
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock)  on 
	--							  left(S.fname,3) = isnull(x.FNAME,'') and 
	--							  left(S.lname,6) = isnull(x.LNAME,'') and 
	--							  left(S.ADDRESS,15) = isnull(x.ADDRESS,'') and 
	--							  s.ZIP = x.ZIP 
	--			--where minsubscriptionID = 17840412      
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 4 -- COMPLETE FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(email,'') Email, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,EMAIL
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.FNAME = x.FNAME and 
	--							  s.LNAME = x.LNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 5 -- COMPLETE FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select fname,lname,company, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,company
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.FNAME = x.FNAME and 
	--							  s.LNAME = x.LNAME and 
	--							  s.company = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID                
	--	  End               
	--	  else if @loopcounter = 6 -- MATCH FIELDS FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname ,left(lname,6) lname,left(company,8) company, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by left(fname,3),left(lname,6),left(company,8)
	--			having COUNT(*) > 1

	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,3) = x.FNAME and 
	--							  LEFT(s.LNAME,6)  = x.LNAME and 
	--							  LEFT(s.company,8) = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 7 -- MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select LEFT(fname,3) fname,LEFT(lname,6) lname,Email, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by LEFT(FNAME,3),LEFT(LNAME,6),EMAIL
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,3) = x.FNAME and 
	--							  LEFT(s.LNAME,6)  = x.LNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 8 -- COMPLETE FNAME,LNAME,PHONE
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(phone,'') phone, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,phone
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  ISNULL(s.FNAME,'')  = x.FNAME and 
	--							  ISNULL(s.LNAME,'')   = x.LNAME and 
	--							  ISNULL(s.phone,'')  = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 9 -- MATCH FIELDS FNAME,LNAME,PHONE
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname,left(lname,6) lname,phone , COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by left(fname,3),left(lname,6),phone
	--			having COUNT(*) > 1 -- 16924 dupes
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  left(ISNULL(s.FNAME,''),3)  = x.FNAME and 
	--							  left(ISNULL(s.LNAME,''),6)  = x.LNAME and 
	--							  ISNULL(s.phone,'')  = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 10 -- EMAIL ONLY PROFILES MATCHING WITH ANOTHER PROFILE WITH FNAME/LNAME MASTER EMAIL ONLY
	--	  Begin
	--			insert into #curtable   
	--			select  minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select fname,lname,email, SubscriptionID as minsubscriptionID from Subscriptions with (NOLOCK) where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(email,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.EMAIL = x.email
	--			where ISNULL(s.fname,'') = '' and ISNULL(s.lname,'') = '' and ISNULL(s.email,'') <> ''
	--			order by minsubscriptionID     
	                 
	--	  End               
	--	  else if @loopcounter = 11 -- SAWP NAME MATCH ON COMLETE FNAME,LNAME,COMPANY, ADDRESS, CITY, STATE, ZIP, PHONE, EMAIL
	--	  Begin
				
	--			Insert Into #NameSwapID
	--			select x.SubscriptionID as SubId, s.subscriptionID as DupeSubId
	--			from
	--			(
	--			select isnull(FNAME,'') FNAME, isnull(LNAME,'') LNAME, isnull(COMPANY,'') COMPANY, isnull([ADDRESS],'') [ADDRESS], isnull(CITY,'')CITY, isnull([State],'')[State], 
	--			isnull(ZIP,'') ZIP, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, subscriptionID
	--			from Subscriptions with (NOLOCK)
	--			group by
	--			isnull(FNAME,''), isnull(LNAME,''), isnull(COMPANY,''), isnull([ADDRESS],''), isnull(CITY,''), isnull([State],''), isnull(ZIP,''), isnull(PHONE,''), isnull(EMAIL,''), subscriptionID
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on isnull(s.FNAME,'') = x.LNAME and 
	--							  isnull(s.LNAME,'') = x.FNAME  and 
	--							  isnull(s.COMPANY,'') = x.COMPANY and 
	--							  isnull(s.[ADDRESS],'') = x.[ADDRESS] and 
	--							  isnull(s.CITY,'') = x.CITY and 
	--							  isnull(s.[State],'') = x.[State] and 
	--							  isnull(s.ZIP,'') = x.ZIP and 
	--							  isnull(s.PHONE,'') = x.PHONE and 
	--							  isnull(s.EMAIL,'') = x.EMAIL        
	--			where x.SubscriptionID <> s.subscriptionID
	--			order by x.subscriptionID		

			    
	--	  End		  
	--	  else if @loopcounter = 12 -- SAWP NAME MATCH FIELDS FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,LEFT(Address,15) [address],LEFT(zip,5) zip,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(Address,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(zip,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  LEFT(s.ADDRESS,15) = x.address and
	--							  LEFT(s.Zip,5) = x.Zip
	--			where ISNULL(s.Address,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' and ISNULL(s.Zip,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID   		        
	--	  End
	--	  else if @loopcounter = 13 -- SAWP NAME MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,Email,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID           
	--	  End   
	--	  else if @loopcounter = 14 -- SAWP NAME MATCH FIELDS FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,LEFT(company,8) company,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  LEFT(s.company,8) = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID            
	--	  End 
	--	  else if @loopcounter = 15 -- SAWP NAME MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,phone,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  s.phone = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID          
	--	  End 
	    
	--	  -- clean up swap matches and insert id's into #curtable
	--	    Delete from #NameSwapID
	--	    Where DupeSubId in (select case when SubId > DupeSubId then DupeSubId else SubId end from #NameSwapID)

	--	    Insert into #curtable
	--	    Select SubId,DupeSubId from #NameSwapID	
		    
	--	  -- End SWAP Clean UP

	--	  DECLARE c_Subscriptions CURSOR FOR select distinct KeepSubscriptionID from #curtable

	--	  OPEN c_Subscriptions  
	--	  FETCH NEXT FROM c_Subscriptions INTO @KeepSubscriptionID

	--	  WHILE @@FETCH_STATUS = 0  
	--	  BEGIN  
	--			set @KeepPubsubscriptionID = 0
	--			set @codesheetID = 0
	--			set @pubID = 0
	--			set @pubsubscriptionID = 0
	            
	--			delete from @tmp

	--			insert into @tmp
	--			select DupeSubscriptionID from #curtable 
	--			where KeepSubscriptionID = @KeepSubscriptionID
	            
	--			if (not exists (select 1 from #curtable where KeepSubscriptionID = @KeepSubscriptionID and DupeSubscriptionID = @KeepSubscriptionID))
	--			Begin
	--				  insert into @tmp values (@KeepSubscriptionID)                 
	--			End
	                  
	--			print (convert(varchar,@i) + ' / ' + convert(varchar(20), getdate(), 114) + ' - SubscriptionID :' + convert(varchar(100),@KeepSubscriptionID) ) --+ ' - ' + @FNAME + ' - ' + @LNAME + ' - ' + @COMPANY + ' - ' + @ADDRESS + ' - ' + @CITY + ' - ' + @State + ' - ' + @ZIP + ' - ' + @PHONE + ' - ' + @EMAIL
	            
	--			if exists (select top 1 pubID from PubSubscriptions ps  WITH (NOLOCK) join @tmp t on ps.SubscriptionID = t.SubscriptionID group by PubID having COUNT(pubsubscriptionID) > 1)
	--			Begin
	                              
	--				  DECLARE c_PubSubscriptions CURSOR 
	--				  FOR select ps.pubID, ps.PubSubscriptionID 
	--				  from PubSubscriptions ps  with (NOLOCK) join  @tmp t on t.subscriptionID = ps.subscriptionID
	--				  where ps.SubscriptionID <> @KeepSubscriptionID and
	--				  PubID in (select pubID from PubSubscriptions ps    with (NOLOCK) where SubscriptionID in (select SubscriptionID from @tmp) group by pubID having COUNT(*) > 1)

	--				  OPEN c_PubSubscriptions  
	--				  FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
	                  
	--				  WHILE @@FETCH_STATUS = 0  
	--				  BEGIN  
	--						--print (' OPEN c_PubSubscriptions / ' + convert(varchar(20), getdate(), 114)   )           
	                        
	--						select @KeepPubsubscriptionID = PubSubscriptionID 
	--						from PubSubscriptions with (NOLOCK)
	--						where PubID = @pubID and SubscriptionID = @KeepSubscriptionID

	--						if (@KeepPubsubscriptionID = 0)
	--						Begin
	                        
	--							  print 'Insert KeepPubsubscriptionID : '
	                        
	--							  insert into PubSubscriptions (SubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email)
	--							  select @KeepSubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email
	--							  from PubSubscriptions  WITH (nolock) 
	--							  where pubsubscriptionID = @pubsubscriptionID
	                              
	--							  set @KeepPubsubscriptionID = @@IDENTITY
	--						End

	                        
	--						/********* pubSubscriptiondetail ***********/
	--						DECLARE c_PubSubscriptiondetail CURSOR 
	--						FOR 
	--						select CodesheetID from PubSubscriptionDetail  WITH (NOLOCK)
	--							  where PubSubscriptionID in (@pubsubscriptionID, @KeepPubsubscriptionID)
	--							  group by  CodesheetID
	--							  having COUNT(pubsubscriptiondetailID) > 1
	                        
	--						--print (' c_PubSubscriptiondetail / ' + convert(varchar(20), getdate(), 114)   )
	                              
	--						OPEN c_PubSubscriptiondetail  
	--						FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
	                        
	--						WHILE @@FETCH_STATUS = 0  
	--						BEGIN  
	                              
	--							  delete from PubSubscriptionDetail
	--							  where PubSubscriptionID = @pubsubscriptionID and CodesheetID = @codesheetID
	                        
	--							  FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
	--						END

	--						CLOSE c_PubSubscriptiondetail  
	--						DEALLOCATE c_PubSubscriptiondetail  
	--						/********* pubSubscriptiondetail ***********/

	--						update PubSubscriptionDetail
	--						set PubSubscriptionID = @KeepPubsubscriptionID
	--						where PubSubscriptionID = @pubsubscriptionID
	                        
	--						delete from PubSubscriptions 
	--						where  PubSubscriptionID = @pubsubscriptionID and PubID = @pubID
	                        
	--						FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
	                        
	--				  END

	--				  CLOSE c_PubSubscriptions  
	--				  DEALLOCATE c_PubSubscriptions  
	--			End
	                  
	--			update PubSubscriptionDetail
	--			set SubscriptionID = @KeepSubscriptionID
	--			where SubscriptionID in  (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			update PubSubscriptions
	--			set SubscriptionID = @KeepSubscriptionID
	--			where SubscriptionID in  (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			delete from SubscriberMasterValues where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from SubscriptionsExtension where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from SubscriptionDetails where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from Subscriptions where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			set @i = @i + 1
	                  
	--			FETCH NEXT FROM c_Subscriptions INTO @KeepSubscriptionID

	--	  END

	--	  CLOSE c_Subscriptions  
	--	  DEALLOCATE c_Subscriptions  

	--	  set @loopcounter = @loopcounter + 1      

	--	  truncate table subscriptiondetails

	--	  truncate table dbo.SubscriberMasterValues

	--	  insert into subscriptiondetails
	--		  select distinct subscriptionID, cb.masterID 
	--		  from  PubSubscriptionDetail psd with (NOLOCK) join 
	--					  CodeSheet_Mastercodesheet_Bridge cb  with (NOLOCK)  on psd.CodeSheetID = cb.CodeSheetID
		            
	--	  insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	--		  SELECT 
	--			MasterGroupID, [SubscriptionID] , 
	--			STUFF((
	--				SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
	--				FROM [dbo].[SubscriptionDetails] sd1   WITH (nolock) join Mastercodesheet mc1   WITH (nolock) on sd1.MasterID = mc1.MasterID  
	--				WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
	--				FOR XML PATH (''))
	--			,1,1,'') AS CombinedValues
	--		  FROM 
	--				(
	--				  SELECT DISTINCT sd.SubscriptionID, mg.MasterGroupID
	--				  FROM [dbo].[SubscriptionDetails] sd  WITH (nolock)  join Mastercodesheet mc  WITH (nolock)  on sd.MasterID = mc.MasterID join MasterGroups mg  WITH (nolock)  on mg.MasterGroupID = mc.MasterGroupID	            
	--				)
	--		  Results
	--		  GROUP BY [SubscriptionID] , MasterGroupID
	--		  ORDER BY SubscriptionID             
		      
	--END -- End while loop

	--drop table #curtable
	--drop table #NameSwapID
GO
PRINT N'Altering [dbo].[e_SubscriberFinal_SaveDQMClean]...';


GO
ALTER PROCEDURE [e_SubscriberFinal_SaveDQMClean]
@SourceFileID int,
@ProcessCode varchar(50)
AS
	exec e_SubscriberFinal_DisableIndexes
	exec e_SubscriberDemographicFinal_DisableIndexes

	INSERT INTO SubscriberFinal 
	(
		 STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,IsMailable,ProcessCode,ImportRowNumber
	)  
	SELECT 
		 st.STRecordIdentifier,st.SourceFileID,st.PubCode,st.Sequence,st.FName,st.LName,st.Title,st.Company,st.Address,st.MailStop,st.City,st.State,st.Zip,st.Plus4,st.ForZip,st.County,st.Country,st.CountryID,st.Phone,st.PhoneExists,
		 st.Fax,st.FaxExists,st.Email,st.EmailExists,st.CategoryID,st.TransactionID,st.TransactionDate,st.QDate,st.QSourceID,st.RegCode,st.Verified,st.SubSrc,st.OrigsSrc,st.Par3C,st.Demo31,st.Demo32,st.Demo33,st.Demo34,st.Demo35,st.Demo36,st.Source,
		 st.Priority,st.IGrp_No,st.IGrp_Cnt,st.CGrp_No,st.CGrp_Cnt,st.StatList,st.Sic,st.SicCode,st.Gender,st.IGrp_Rank,st.CGrp_Rank,st.Address3,st.Home_Work_Address,st.PubIDs,st.Demo7,st.IsExcluded,st.Mobile,st.Latitude,st.Longitude,st.IsLatLonValid,
		 st.LatLonMsg,st.Score,st.EmailStatusID,st.StatusUpdatedDate,st.StatusUpdatedReason,st.Ignore,st.IsDQMProcessFinished,st.DQMProcessDate,st.IsUpdatedInLive,
		 st.UpdateInLiveDate, NEWID() AS SFRecordIdentifier, st.DateCreated,st.DateUpdated,st.CreatedByUserID,st.UpdatedByUserID,st.IsMailable,st.ProcessCode,st.ImportRowNumber
	FROM SubscriberTransformed st With(NoLock)
	WHERE st.SourceFileID = @SourceFileID 
	AND st.ProcessCode = @ProcessCode 
	AND st.STRecordIdentifier NOT IN (SELECT sf.STRecordIdentifier FROM SubscriberFinal sf);

	--Insert non-duplicate records into subscriberDemographicFinal table
	INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	
	SELECT sdt.PubID,sf.SFRecordIdentifier,sdt.MAFField,sdt.Value,sdt.NotExists,sdt.DateCreated,sdt.DateUpdated,sdt.CreatedByUserID,sdt.UpdatedByUserID
	FROM SubscriberFinal sf 
	JOIN SubscriberDemographicTransformed sdt ON sdt.STRecordIdentifier = sf.STRecordIdentifier
	WHERE sf.SourceFileID = @SourceFileID 
			AND sf.ProcessCode = @ProcessCode 
			AND sdt.NotExists = 'false'

	exec e_SubscriberFinal_EnableIndexes
	exec e_SubscriberDemographicFinal_EnableIndexes

	UPDATE SubscriberFinal
	SET TransactionID = 10
	WHERE SourceFileID = @SourceFileID 
			AND ProcessCode = @ProcessCode 
			AND (TransactionID = 0 OR TransactionID NOT IN (SELECT TransactionID FROM [Transaction]))
	
	UPDATE SubscriberFinal
	SET CategoryID = 10
	WHERE SourceFileID = @SourceFileID 
			AND ProcessCode = @ProcessCode 
			AND (CategoryID = 0 OR CategoryID NOT IN (SELECT CategoryID FROM CATEGORY))
GO
PRINT N'Altering [dbo].[job_CountryRegionCleanse]...';


GO
ALTER PROCEDURE [job_CountryRegionCleanse]
@ProcessCode varchar(50),
@SourceFileID int
AS

UPDATE SubscriberTransformed
SET State = (Select RegionCode From UAS..Region With(NoLock) Where RegionID = RegionMap.RegionID),
DateUpdated = GETDATE(),
	UpdatedByUserID = 1
FROM UAS..RegionMap 
WHERE State = RegionMap.RegionDirty COLLATE SQL_Latin1_General_CP1_CI_AS
AND ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

UPDATE SubscriberTransformed
SET Country='UNITED STATES',
	CountryID = (Select CountryID From UAS..Country Where ShortName = 'United States'),
	DateUpdated = GETDATE(),
	UpdatedByUserID = 1
WHERE IsLatLonValid = 'true' 
AND State in (Select RegionCode from UAS..Region Where CountryID in (select CountryID from UAS..Country where ShortName = 'United States'))
AND Country = ''

UPDATE SubscriberTransformed
SET Country='UNITED STATES',
	CountryID = (Select CountryID From UAS..Country Where ShortName = 'United States'),
	DateUpdated = GETDATE(),
	UpdatedByUserID = 1
WHERE State in (Select RegionCode from UAS..Region Where CountryID in (select CountryID from UAS..Country where ShortName = 'United States'))
AND Country = ''

UPDATE SubscriberTransformed
SET Country='UNITED STATES',
	CountryID = (Select CountryID From UAS..Country Where ShortName = 'United States'),
	DateUpdated = GETDATE(),
	UpdatedByUserID = 1
WHERE Country in ('US','USA','United States','United States of America')
AND ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

UPDATE SubscriberTransformed
SET CountryID = CountryMap.CountryID,
DateUpdated = GETDATE(),
	UpdatedByUserID = 1
FROM UAS..CountryMap 
WHERE Country = CountryMap.CountryDirty COLLATE SQL_Latin1_General_CP1_CI_AS
AND ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

UPDATE SubscriberTransformed
SET Zip = Zip + ' ' + Plus4,
DateUpdated = GETDATE(),
	UpdatedByUserID = 1
WHERE Country = 'Canada' COLLATE SQL_Latin1_General_CP1_CI_AS
AND LEN(Plus4) > 0 AND LEN(Zip) > 0
AND ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

UPDATE SubscriberTransformed
SET State = 'FO',
DateUpdated = GETDATE(),
	UpdatedByUserID = 1
FROM UAS..CountryMap 
WHERE ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID
AND State IS NULL
GO
PRINT N'Altering [dbo].[job_MasterDB_Backup]...';


GO
ALTER PROCEDURE [job_MasterDB_Backup]
@DbName varchar(200),
@File varchar(500)
AS
BACKUP DATABASE @DbName 
TO DISK = @File
WITH STATS = 1, COMPRESSION
GO
PRINT N'Altering [dbo].[job_RefreshDB_Backup]...';


GO

ALTER PROCEDURE [job_RefreshDB_Backup]
@DbName varchar(200),
@File varchar(500)
AS
BACKUP DATABASE @DbName 
TO DISK = @File
WITH STATS = 1, COMPRESSION
GO
PRINT N'Altering [dbo].[o_CountAddressValidation_IsLatLonValid]...';


GO
ALTER PROCEDURE o_CountAddressValidation_IsLatLonValid
@IsLatLonValid bit
AS
	SELECT Count(*)
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = @IsLatLonValid
GO
PRINT N'Altering [dbo].[o_CountAddressValidation_ProcessCode_IsLatLonValid]...';


GO
ALTER PROCEDURE o_CountAddressValidation_ProcessCode_IsLatLonValid
@ProcessCode varchar(50),
@IsLatLonValid bit
AS
	SELECT Count(*)
	FROM SubscriberTransformed With(NoLock)
	WHERE ProcessCode = @ProcessCode
	AND IsLatLonValid = @IsLatLonValid
GO
PRINT N'Altering [dbo].[o_CountAddressValidation_SourceFileID_IsLatLonValid]...';


GO
ALTER PROCEDURE o_CountAddressValidation_SourceFileID_IsLatLonValid
@SourceFileID int,
@IsLatLonValid bit
AS
	SELECT Count(*)
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID
	AND IsLatLonValid = @IsLatLonValid
GO
PRINT N'Altering [dbo].[o_CountForGeoCoding]...';


GO
ALTER PROCEDURE o_CountForGeoCoding
AS
	SELECT Count(*)
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = 'false'
GO
PRINT N'Altering [dbo].[o_CountForGeoCoding_SourceFileID]...';


GO
ALTER PROCEDURE o_CountForGeoCoding_SourceFileID
@SourceFileID int
AS
	SELECT Count(*)
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID AND IsLatLonValid = 'false'
GO
PRINT N'Altering [dbo].[rpt_SourceFile_Summary]...';


GO
ALTER PROCEDURE rpt_SourceFile_Summary
@SourceFileID int,
@ProcessCode  varchar(50),
@FileName varchar(100)
AS
	SELECT  o.ProcessCode, @SourceFileID as SourceFileID, @FileName as FileName, LastUploaded,OriginalCount, 
		ISNULL(TransformedCount,0) as 'TransformedCount', ISNULL(ArchivedCount,0) as 'ArchivedCount', ISNULL(InvalidCount,0) as 'InvalidCount', 
		ISNULL(FinalCount,0) as 'FinalCount', ISNULL(MasterCount,0) as 'MasterCount', ISNULL(SubordinateCount,0) as 'SubordinateCount'
	FROM
		(
			SELECT so.SourceFileID, so.ProcessCode, COUNT(SubscriberOriginalID) as 'OriginalCount', MIN(so.DateCreated) as 'LastUploaded'
			FROM	subscriberoriginal so WITH(NoLock)
			GROUP BY so.sourcefileID, so.ProcessCode
		)
		o
	LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberTransformedID) as 'TransformedCount'
			FROM SubscriberTransformed so WITH(NoLock)
			GROUP BY so.sourcefileID, so.ProcessCode
			)
		t on t.SourceFileID = o.SourceFileID  and t.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
			FROM SubscriberArchive so with (NOLOCK)
			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
			)
		y on y.SourceFileID = o.SourceFileID and y.ProcessCode  = o.ProcessCode
	LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberFinalID) as 'FinalCount'
			FROM SubscriberFinal so WITH(NoLock)
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		z on z.SourceFileID = o.SourceFileID and z.ProcessCode  = o.ProcessCode
	LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberInvalidID) as 'InvalidCount'
			FROM SubscriberInvalid so WITH(NoLock)
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		i on i.SourceFileID = o.SourceFileID  and i.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'MasterCount'
			FROM SubscriberFinal so WITH(NoLock)
			WHERE so.IGrp_Rank = 'M'
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		r on r.SourceFileID = o.SourceFileID and r.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'SubordinateCount'
			FROM SubscriberFinal so WITH(NoLock)
			WHERE so.IGrp_Rank = 'S'
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		s on s.SourceFileID = o.SourceFileID and s.ProcessCode  = o.ProcessCode
	WHERE o.ProcessCode = @ProcessCode 
	ORDER BY OriginalCount asc
--CREATE PROCEDURE rpt_SourceFile_Summary
--@SourceFileID int,
--@ProcessCode  varchar(50)
--AS
--	SELECT  o.ProcessCode, sf.SourceFileID, sf.FileName, LastUploaded,OriginalCount, 
--		ISNULL(TransformedCount,0) as 'TransformedCount', ISNULL(ArchivedCount,0) as 'ArchivedCount', ISNULL(InvalidCount,0) as 'InvalidCount', 
--		ISNULL(FinalCount,0) as 'FinalCount', ISNULL(MasterCount,0) as 'MasterCount', ISNULL(SubordinateCount,0) as 'SubordinateCount'
--	FROM UAS..sourcefile sf WITH(NoLock)  
--	JOIN
--		(
--			SELECT so.SourceFileID, so.ProcessCode, COUNT(SubscriberOriginalID) as 'OriginalCount', MIN(so.DateCreated) as 'LastUploaded'
--			FROM	subscriberoriginal so WITH(NoLock)
--			GROUP BY so.sourcefileID, so.ProcessCode
--		)
--		o on o.SourceFileID = sf.SourceFileID
--	LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberTransformedID) as 'TransformedCount'
--			FROM SubscriberTransformed so WITH(NoLock)
--			GROUP BY so.sourcefileID, so.ProcessCode
--			)
--		t on t.SourceFileID = o.SourceFileID  and t.ProcessCode  = o.ProcessCode
--		LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
--			FROM SubscriberArchive so with (NOLOCK)
--			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
--			)
--		y on y.SourceFileID = o.SourceFileID and y.ProcessCode  = o.ProcessCode
--	LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberFinalID) as 'FinalCount'
--			FROM SubscriberFinal so WITH(NoLock)
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		z on z.SourceFileID = o.SourceFileID and z.ProcessCode  = o.ProcessCode
--	LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberInvalidID) as 'InvalidCount'
--			FROM SubscriberInvalid so WITH(NoLock)
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		i on i.SourceFileID = o.SourceFileID  and i.ProcessCode  = o.ProcessCode
--		LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'MasterCount'
--			FROM SubscriberFinal so WITH(NoLock)
--			WHERE so.IGrp_Rank = 'M'
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		r on r.SourceFileID = o.SourceFileID and r.ProcessCode  = o.ProcessCode
--		LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'SubordinateCount'
--			FROM SubscriberFinal so WITH(NoLock)
--			WHERE so.IGrp_Rank = 'S'
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		s on s.SourceFileID = o.SourceFileID and s.ProcessCode  = o.ProcessCode
--	WHERE sf.SourceFileID = @SourceFileID AND o.ProcessCode = @ProcessCode 
--	ORDER BY OriginalCount asc
--GO
GO
PRINT N'Creating [dbo].[ccp_TenMissions_DONO_Suppression]...';


GO
create procedure ccp_TenMissions_DONO_Suppression
@SourceFileID int,
@ProcessCode varchar(50)
AS

BEGIN
	declare @Xml xml = 
'
<XML>
	<Entity>
		<FName>ROBBIE</FName>
		<LName>CHEATHAM</LName>
		<Company>CHEATHAM TOWING SVC &amp; REPAIR</Company>
		<Address>17490 HIGHWAY 19 S</Address>
		<City>PHILADELPHIA</City>
		<State>MS</State>
		<Zip>39350 4696</Zip>
		<Phone>601-656-8492</Phone>
		<Fax></Fax>
		<Email></Email>
	</Entity>
</XML>'

	--Save the xml to the UAS.FileLog table
	declare @xmlString varchar(max) = cast(@Xml as varchar(max))
	declare @FSCodeId int = (Select CodeId From UAS..Code with(nolock) where CodeTypeId = 12 and CodeName = 'Suppression')
	INSERT INTO UAS..FileLog (SourceFileID, FileStatusTypeID, Message, LogDate, LogTime, ProcessCode)
	VALUES(@SourceFileID, @FSCodeId, @xmlString, GETDATE(), GETDATE(), @ProcessCode);
	
	--Create temp table for xml
	CREATE TABLE #tmp_SuppFile
		(
			FName varchar(100),
            LName varchar(100),
            Company varchar(255),
            Address varchar(255),
            City varchar(50),
            State varchar(50),
            Zip varchar(50),
            Phone varchar(100),
            Fax varchar(100),
            Email varchar(100)
        )
    --CREATE INDEX idx_SuppFile ON #tmp_SuppFile(FName,LName,Company,[Address],City,State,Zip,Phone,Fax,Email)
    
                                                
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @docHandle int
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml
	
	insert into #tmp_SuppFile 
	(
		 FName,LName,Company,Address,City,State,Zip,Phone,Fax,Email
	)  
	
	SELECT 
		FName,LName,Company,Address,City,State,Zip,Phone,Fax,Email
	FROM OPENXML(@docHandle, N'/XML/Entity') 
	WITH   
	(  
		FName varchar(100) 'FName',
        LName varchar(100) 'LName',
        Company varchar(255) 'Company',
        Address varchar(255) 'Address',
        City varchar(50) 'City',
        State varchar(50) 'State',
        Zip varchar(50) 'Zip',
        Phone varchar(100) 'Phone',
        Fax varchar(100) 'Fax',
        Email varchar(100) 'Email'
	)  

	EXEC sp_xml_removedocument @docHandle   

	declare @AddrSupp int
	declare @AddrIgrpNo int
	declare @CompanySupp int
	declare @CompanyIgrpNo int
	declare @EmailSupp int
	declare @EmailIgrpNo int
	declare @PhoneSupp int
	declare @PhoneIgrpNo int
	
	--CREATE TABLE #tmp_IgrpNoSupp(IGRP_NO uniqueidentifier,SuppFileName varchar(100))

	--
	-- Match on FName,LName,Address,Zip
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,DateCreated,DateUpdated,CreatedByUserID)

	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,1,0,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on left(sf.FName,3) = left(ts.FName,3) and left(sf.LName,6) = left(ts.LName,6) and left(sf.Address,15) = left(ts.Address,15) and left(sf.Zip,5) = left(ts.Zip,5)
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.Address,'')!='' AND ISNULL(sf.Zip,'')!='' AND SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @AddrSupp = @@ROWCOUNT

	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,1,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @AddrIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp

	--
	-- Match on Fname,LName,Company
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,0,1,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on left(sf.FName,3) = left(ts.FName,3) and left(sf.LName,6) = left(ts.LName,6) and left(sf.Company,8) = left(ts.Company,8)
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.Company,'')!=''AND SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed) 
	
	SET @CompanySupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,0,1,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @CompanyIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp

	--
	-- Any email that matches, set Demo33 to false
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,1,0,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on sf.Email = ts.Email
	WHERE ISNULL(sf.Email,'')!='' AND SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode 
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @EmailSupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,1,0,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @EmailIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp

	--
	-- Any phone that matches, set Demo34 and Demo35 to false
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,1,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on sf.Phone = ts.Phone
	WHERE ISNULL(sf.Phone,'')!='' AND SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode  AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @PhoneSupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,1,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @PhoneIgrpNo = @@ROWCOUNT
	
	DROP TABLE #tmp_SuppFile
	--DROP TABLE #tmp_IgrpNoSupp
		
	-- Update subFinal
	UPDATE sf
	SET Demo31 = 'false', Demo32 = 'false', Demo33 = 'false', Demo34 = 'false', Demo35 = 'false', Demo36 = 'false'
	FROM SubscriberFinal sf INNER JOIN Suppressed sp on sf.SFRecordIdentifier = sp.SFRecordIdentifier
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode


	-- Total up affected rows
	DECLARE @SuppCount int
	SET @SuppCount = @AddrSupp + @CompanySupp + @EmailSupp + @PhoneSupp + @AddrIgrpNo + @CompanyIgrpNo + @EmailIgrpNo + @PhoneIgrpNo
	
	SELECT @SuppCount
END
GO
PRINT N'Creating [dbo].[e_Adhoc_Delete_CategoryID]...';


GO
CREATE PROCEDURE [dbo].[e_Adhoc_Delete_CategoryID]
	@CategoryID int
AS
	DELETE Adhoc WHERE CategoryID = @CategoryID
GO
PRINT N'Creating [dbo].[e_Adhoc_Save]...';


GO
CREATE PROCEDURE [dbo].[e_Adhoc_Save]
	@AdhocID int,
	@AdhocName varchar(50),    
	@CategoryID int,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
	IF @AdhocID > 0
	BEGIN						
		UPDATE Adhoc
		SET AdhocName = @AdhocName,
			CategoryID = @CategoryID,
			SortOrder = @SortOrder
		WHERE AdhocID = @AdhocID;
		
		SELECT @AdhocID;
	END
	ELSE
		BEGIN
			INSERT INTO Adhoc (AdhocName,CategoryID,SortOrder)
			VALUES(@AdhocName,@CategoryID,@SortOrder);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_Adhoc_Select_All]...';


GO
CREATE PROCEDURE [dbo].[e_Adhoc_Select_All]
AS
	SELECT * FROM Adhoc With(NoLock)
GO
PRINT N'Creating [dbo].[e_Adhoc_Select_CategoryID]...';


GO
CREATE PROCEDURE [dbo].[e_Adhoc_Select_CategoryID]
	@CategoryID int,
	@BrandID int,
	@PubID int
AS
BEGIN
	if @CategoryID = 0
	begin	

		if(@PubID != 0)
		Begin
			--standardfields, SubscriptionsExtensionMapper
			select 
				case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
				when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
				when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
				when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
				upper(c.name) as DisplayName, 
				t.name as ColumnType, 
				c.name as ColumnName
			from 
				sysobjects s join syscolumns c on s.ID = c.ID  inner join
				systypes t on c.xtype = t.xtype 
			where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4','COUNTY', 'FORZIP', 'PUBIDs', 'NOTES') and 
				c.name not in (select AdhocName from Adhoc)  
			union
				select 
					'e|' + StandardField  + '|' +
					case when CustomFieldDataType like '%date%' then 'd' 
						 when CustomFieldDataType like '%bit%' then 'b' 
						 when CustomFieldDataType like '%int%' then 'i' 
						 when CustomFieldDataType like '%float%' then 'f'
						 else '' 
					end
					as ColumnValue, 
					upper(CustomField) as displayName,
					CustomFieldDataType as columnType,
					StandardField as columnName  
				from 
					SubscriptionsExtensionMapper map 
				where 
					map.Active = 1 and map.CustomField not in (select AdhocName from Adhoc)  
			union
				select 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT'
			order by 
				columnName
		End
		else
		Begin
			--standardfields, SubscriptionsExtensionMapper, MasterGroups
		   if @BrandID = 0
		   begin
 				select 
 					case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
					when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
					when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
					when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
					upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName
				from sysobjects s join syscolumns c on s.ID = c.ID inner join 
					systypes t on c.xtype = t.xtype 
				where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4','COUNTY', 'FORZIP', 'PUBIDs', 'NOTES') and 
					c.name not in (select AdhocName from Adhoc)  
				union
					select 
						'm|' + CONVERT(varchar(3),m.MasterGroupID)  as columnValue ,
						upper(m.Displayname) as displayName, 'varchar' as columnType, ColumnReference as columnName  
					from 
						MasterGroups m 
					where 
						m.EnableAdhocSearch = 1 and m.Displayname not in (select AdhocName from Adhoc)   
				union
					select 'e|' + StandardField  + '|' +
						case when CustomFieldDataType like '%date%' then 'd' 
							 when CustomFieldDataType like '%bit%' then 'b' 
							 when CustomFieldDataType like '%int%' then 'i' 
							 when CustomFieldDataType like '%float%' then 'f'
							 else '' 
						end
						as ColumnValue, 
						upper(CustomField) as displayName,
						CustomFieldDataType as columnType,
						StandardField as columnName  
					from 
						SubscriptionsExtensionMapper map 
					where 
						map.Active = 1 and map.CustomField not in (select AdhocName from Adhoc)  
				union
					select 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT'
				order by 
					columnName
			end
			else
			begin
				select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
					when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
					when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
					when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
					upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName
				from sysobjects s join syscolumns c on s.ID = c.ID inner join 
					systypes t on c.xtype = t.xtype 
				where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4','COUNTY', 'FORZIP', 'PUBIDs', 'NOTES') and 
					c.name not in (select AdhocName from Adhoc)  
				union
					SELECT distinct 'm|' + CONVERT(varchar(3),mg.MasterGroupID)  as columnValue,
							upper(mg.Displayname) as displayName, 'varchar' as columnType, 
							mg.ColumnReference as columnName  
					From vw_Mapping v join 
					MasterGroups mg on mg.MasterGroupID = v.MasterGroupID join
					branddetails bd on bd.pubid = v.pubid  join
					Brand b on b.BrandID = bd.BrandID
					WHERE mg.EnableAdhocSearch = 1 and mg.Displayname not in (select AdhocName from Adhoc)and bd.brandid = @BrandID and b.IsDeleted = 0
				union
					select 'e|' + StandardField  + '|' +
					case when CustomFieldDataType like '%date%' then 'd' 
						 when CustomFieldDataType like '%bit%' then 'b' 
						 when CustomFieldDataType like '%int%' then 'i' 
						 when CustomFieldDataType like '%float%' then 'f'
						 else '' 
					end
					as ColumnValue 
						,upper(CustomField) as displayName
						,CustomFieldDataType as columnType
						,StandardField as columnName  
					from SubscriptionsExtensionMapper map 
					where map.Active = 1
					and map.CustomField not in (select AdhocName from Adhoc)  
				union
					select 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT'
				order by columnName	
			end
		end
	end
	else
	begin
		if(@PubID != 0)
		Begin
			--standardfields, SubscriptionsExtensionMapper
			select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
				when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
				when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
				when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
				upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName, a.AdhocName,  a.SortOrder 
			from Adhoc a  left outer join   syscolumns c   on  a.AdhocName = c.name
				join sysobjects s on s.ID = c.ID inner join 
				systypes t on c.xtype = t.xtype 
			where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4','COUNTY', 'FORZIP', 'PUBIDs', 'NOTES') and 
			a.CategoryID = @CategoryID
			union
				select 'e|' + StandardField  + '|' +
				case when CustomFieldDataType like '%date%' then 'd' 
					 when CustomFieldDataType like '%bit%' then 'b' 
					 when CustomFieldDataType like '%int%' then 'i' 
					 when CustomFieldDataType like '%float%' then 'f'
					 else '' 
				end
				as ColumnValue 
					,upper(CustomField) as displayName
					,CustomFieldDataType as columnType
					,StandardField as columnName  
					,a.AdhocName, a.SortOrder
				from Adhoc a  
				join SubscriptionsExtensionMapper sem 
					on a.AdhocName = sem.CustomField
					and sem.Active = 1
				where a.CategoryID = @CategoryID   
			order by SortOrder		
		end
		else
		begin
			--standardfields, SubscriptionsExtensionMapper, MasterGroups
			if @BrandID = 0
			begin
				select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
					when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
					when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
					when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
					upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName, a.AdhocName,  a.SortOrder 
				from Adhoc a  left outer join   syscolumns c   on  a.AdhocName = c.name
					join sysobjects s on s.ID = c.ID inner join 
					systypes t on c.xtype = t.xtype 
				where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4','COUNTY', 'FORZIP', 'PUBIDs', 'NOTES') and 
				a.CategoryID = @CategoryID
				union
					select 'm|' + CONVERT(varchar(3),m.MasterGroupID)  as columnValue ,
					upper(m.Displayname) as displayName, 'varchar' as columnType, ColumnReference as columnName, a.AdhocName, a.SortOrder  
					from Adhoc a  left outer join MasterGroups m on a.AdhocName = m.Displayname
					where m.EnableAdhocSearch = 1  and a.CategoryID = @CategoryID
				union
					select 'e|' + StandardField  + '|' +
					case when CustomFieldDataType like '%date%' then 'd' 
						 when CustomFieldDataType like '%bit%' then 'b' 
						 when CustomFieldDataType like '%int%' then 'i' 
						 when CustomFieldDataType like '%float%' then 'f'
						 else '' 
					end
					as ColumnValue 
						,upper(CustomField) as displayName
						,CustomFieldDataType as columnType
						,StandardField as columnName  
						,a.AdhocName, a.SortOrder
					from Adhoc a  
					join SubscriptionsExtensionMapper sem 
						on a.AdhocName = sem.CustomField
						and sem.Active = 1
					where a.CategoryID = @CategoryID   
				order by SortOrder		
			end
			else
			begin
				select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
					when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
					when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
					when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
					upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName, a.AdhocName,  a.SortOrder 
				from Adhoc a  left outer join   syscolumns c   on  a.AdhocName = c.name
					join sysobjects s on s.ID = c.ID inner join 
					systypes t on c.xtype = t.xtype 
				where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4','COUNTY', 'FORZIP', 'PUBIDs', 'NOTES')
				and a.CategoryID = @CategoryID
				union
					select distinct 'm|' + CONVERT(varchar(3), mg.MasterGroupID)  as columnValue ,
						upper(mg.Displayname) as displayName, 'varchar' as columnType, 
						mg.ColumnReference as columnName, a.AdhocName, a.SortOrder  
					From vw_Mapping v join 
					MasterGroups mg on mg.MasterGroupID = v.MasterGroupID join
					branddetails bd on bd.pubid = v.pubid  join
					Brand b on b.BrandID = bd.BrandID join
					Adhoc a  on a.AdhocName = mg.Displayname
					where mg.EnableAdhocSearch = 1  and a.CategoryID = @CategoryID and bd.BrandID = @BrandID and b.Isdeleted = 0
				union
					select 'e|' + StandardField  + '|' +
					case when CustomFieldDataType like '%date%' then 'd' 
						 when CustomFieldDataType like '%bit%' then 'b' 
						 when CustomFieldDataType like '%int%' then 'i' 
						 when CustomFieldDataType like '%float%' then 'f'
						 else '' 
					end
					as ColumnValue 
						,upper(CustomField) as displayName
						,CustomFieldDataType as columnType
						,StandardField as columnName  
						,a.AdhocName, a.SortOrder
					from Adhoc a  
					join SubscriptionsExtensionMapper sem 
						on a.AdhocName = sem.CustomField
						and sem.Active = 1
					where a.CategoryID = @CategoryID   
				order by SortOrder	
		end
		end
	end				
End
GO

PRINT N'Altering [dbo].[e_DupeCleanUp]...';


GO
ALTER PROCEDURE [e_DupeCleanUp]
WITH EXECUTE as OWNER
AS

	--SET NOCOUNT ON;

	--declare     @i int,
	--			@KeepSubscriptionID int,
	--			@KeepPubsubscriptionID int,
	--			@pubID int,
	--			@pubsubscriptionID int,
	--			@codesheetID int

	--set @i = 1
	--print (' Process Dupes : ' +  convert(varchar,@i)  + ' / ' +  convert(varchar(20), getdate(), 114))

	--set nocount on

	--create table #curtable (KeepSubscriptionID int, DupeSubscriptionID int) 

	--CREATE INDEX IDX_Users_UserName ON #curtable(KeepSubscriptionID)
	
	--create table #NameSwapID(SubId int,DupeSubId int)
		
	--declare @tmp table (SubscriptionID int UNIQUE CLUSTERED (SubscriptionID))     

	--declare @loopcounter int = 1

	--while @loopcounter <= 15
	--Begin

	--	  delete from #curtable
	--	  delete from #NameSwapID

	--	  if @loopcounter = 1 -- MATCH ON COMLETE FNAME,LNAME,COMPANY, ADDRESS, CITY, STATE, ZIP, PHONE, EMAIL
	--	  Begin
	--			insert into #curtable
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select isnull(FNAME,'') FNAME, isnull(LNAME,'') LNAME, isnull(COMPANY,'') COMPANY, isnull(ADDRESS,'') ADDRESS, isnull(CITY,'')CITY, isnull(State,'')State, 
	--			isnull(ZIP,'') ZIP, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			group by
	--			isnull(FNAME,''), isnull(LNAME,''), isnull(COMPANY,''), isnull(ADDRESS,''), isnull(CITY,''), isnull(State,''), isnull(ZIP,''), isnull(PHONE,''), isnull(EMAIL,'')
	--			having COUNT(subscriptionID) > 1
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on isnull(s.FNAME,'') = x.FNAME and 
	--							  isnull(s.LNAME,'') = x.LNAME and 
	--							  isnull(s.COMPANY,'') = x.COMPANY and 
	--							  isnull(s.ADDRESS,'') = x.ADDRESS and 
	--							  isnull(s.CITY,'') = x.CITY and 
	--							  isnull(s.State,'') = x.State and 
	--							  isnull(s.ZIP,'') = x.ZIP and 
	--							  isnull(s.PHONE,'') = x.PHONE and 
	--							  isnull(s.EMAIL,'') = x.EMAIL        
	--			--where minsubscriptionID = 17840412      
	--			order by minsubscriptionID    
	                        
	--	  End
	--	  else if @loopcounter = 2 -- COMLETE FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #curtable
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname,ISNULL("ADDRESS",'') "ADDRESS",ZIP,CountryID, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
	--			group by FNAME,LNAME,"ADDRESS",ZIP,CountryID
	--			having COUNT(*) > 1 
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on 
	--							  isnull(s.FNAME,'') = isnull(x.FNAME,'') and 
	--							  isnull(s.LNAME,'') = isnull(x.LNAME,'') and 
	--							  isnull(s.ADDRESS,'') = isnull(x.ADDRESS,'') and 
	--							  s.ZIP = x.ZIP and 
	--							  ISNULL(x.CountryID,0) = ISNULL(S.CountryID,0)
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 3 -- MATCH FIELDS FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname,left(lname,6) lname,left("ADDRESS",15) "ADDRESS",ZIP, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
	--			group by left(fname,3),left(lname,6),left("ADDRESS",15),ZIP
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock)  on 
	--							  left(S.fname,3) = isnull(x.FNAME,'') and 
	--							  left(S.lname,6) = isnull(x.LNAME,'') and 
	--							  left(S.ADDRESS,15) = isnull(x.ADDRESS,'') and 
	--							  s.ZIP = x.ZIP 
	--			--where minsubscriptionID = 17840412      
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 4 -- COMPLETE FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(email,'') Email, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,EMAIL
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.FNAME = x.FNAME and 
	--							  s.LNAME = x.LNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 5 -- COMPLETE FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select fname,lname,company, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,company
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.FNAME = x.FNAME and 
	--							  s.LNAME = x.LNAME and 
	--							  s.company = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID                
	--	  End               
	--	  else if @loopcounter = 6 -- MATCH FIELDS FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname ,left(lname,6) lname,left(company,8) company, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by left(fname,3),left(lname,6),left(company,8)
	--			having COUNT(*) > 1

	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,3) = x.FNAME and 
	--							  LEFT(s.LNAME,6)  = x.LNAME and 
	--							  LEFT(s.company,8) = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 7 -- MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select LEFT(fname,3) fname,LEFT(lname,6) lname,Email, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by LEFT(FNAME,3),LEFT(LNAME,6),EMAIL
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,3) = x.FNAME and 
	--							  LEFT(s.LNAME,6)  = x.LNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 8 -- COMPLETE FNAME,LNAME,PHONE
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(phone,'') phone, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,phone
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  ISNULL(s.FNAME,'')  = x.FNAME and 
	--							  ISNULL(s.LNAME,'')   = x.LNAME and 
	--							  ISNULL(s.phone,'')  = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 9 -- MATCH FIELDS FNAME,LNAME,PHONE
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname,left(lname,6) lname,phone , COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by left(fname,3),left(lname,6),phone
	--			having COUNT(*) > 1 -- 16924 dupes
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  left(ISNULL(s.FNAME,''),3)  = x.FNAME and 
	--							  left(ISNULL(s.LNAME,''),6)  = x.LNAME and 
	--							  ISNULL(s.phone,'')  = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 10 -- EMAIL ONLY PROFILES MATCHING WITH ANOTHER PROFILE WITH FNAME/LNAME MASTER EMAIL ONLY
	--	  Begin
	--			insert into #curtable   
	--			select  minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select fname,lname,email, SubscriptionID as minsubscriptionID from Subscriptions with (NOLOCK) where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(email,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.EMAIL = x.email
	--			where ISNULL(s.fname,'') = '' and ISNULL(s.lname,'') = '' and ISNULL(s.email,'') <> ''
	--			order by minsubscriptionID     
	                 
	--	  End               
	--	  else if @loopcounter = 11 -- SAWP NAME MATCH ON COMLETE FNAME,LNAME,COMPANY, ADDRESS, CITY, STATE, ZIP, PHONE, EMAIL
	--	  Begin
				
	--			Insert Into #NameSwapID
	--			select x.SubscriptionID as SubId, s.subscriptionID as DupeSubId
	--			from
	--			(
	--			select isnull(FNAME,'') FNAME, isnull(LNAME,'') LNAME, isnull(COMPANY,'') COMPANY, isnull([ADDRESS],'') [ADDRESS], isnull(CITY,'')CITY, isnull([State],'')[State], 
	--			isnull(ZIP,'') ZIP, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, subscriptionID
	--			from Subscriptions with (NOLOCK)
	--			group by
	--			isnull(FNAME,''), isnull(LNAME,''), isnull(COMPANY,''), isnull([ADDRESS],''), isnull(CITY,''), isnull([State],''), isnull(ZIP,''), isnull(PHONE,''), isnull(EMAIL,''), subscriptionID
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on isnull(s.FNAME,'') = x.LNAME and 
	--							  isnull(s.LNAME,'') = x.FNAME  and 
	--							  isnull(s.COMPANY,'') = x.COMPANY and 
	--							  isnull(s.[ADDRESS],'') = x.[ADDRESS] and 
	--							  isnull(s.CITY,'') = x.CITY and 
	--							  isnull(s.[State],'') = x.[State] and 
	--							  isnull(s.ZIP,'') = x.ZIP and 
	--							  isnull(s.PHONE,'') = x.PHONE and 
	--							  isnull(s.EMAIL,'') = x.EMAIL        
	--			where x.SubscriptionID <> s.subscriptionID
	--			order by x.subscriptionID		

			    
	--	  End		  
	--	  else if @loopcounter = 12 -- SAWP NAME MATCH FIELDS FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,LEFT(Address,15) [address],LEFT(zip,5) zip,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(Address,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(zip,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  LEFT(s.ADDRESS,15) = x.address and
	--							  LEFT(s.Zip,5) = x.Zip
	--			where ISNULL(s.Address,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' and ISNULL(s.Zip,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID   		        
	--	  End
	--	  else if @loopcounter = 13 -- SAWP NAME MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,Email,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID           
	--	  End   
	--	  else if @loopcounter = 14 -- SAWP NAME MATCH FIELDS FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,LEFT(company,8) company,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  LEFT(s.company,8) = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID            
	--	  End 
	--	  else if @loopcounter = 15 -- SAWP NAME MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,phone,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  s.phone = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID          
	--	  End 
	    
	--	  -- clean up swap matches and insert id's into #curtable
	--	    Delete from #NameSwapID
	--	    Where DupeSubId in (select case when SubId > DupeSubId then DupeSubId else SubId end from #NameSwapID)

	--	    Insert into #curtable
	--	    Select SubId,DupeSubId from #NameSwapID	
		    
	--	  -- End SWAP Clean UP

	--	  DECLARE c_Subscriptions CURSOR FOR select distinct KeepSubscriptionID from #curtable

	--	  OPEN c_Subscriptions  
	--	  FETCH NEXT FROM c_Subscriptions INTO @KeepSubscriptionID

	--	  WHILE @@FETCH_STATUS = 0  
	--	  BEGIN  
	--			set @KeepPubsubscriptionID = 0
	--			set @codesheetID = 0
	--			set @pubID = 0
	--			set @pubsubscriptionID = 0
	            
	--			delete from @tmp

	--			insert into @tmp
	--			select DupeSubscriptionID from #curtable 
	--			where KeepSubscriptionID = @KeepSubscriptionID
	            
	--			if (not exists (select 1 from #curtable where KeepSubscriptionID = @KeepSubscriptionID and DupeSubscriptionID = @KeepSubscriptionID))
	--			Begin
	--				  insert into @tmp values (@KeepSubscriptionID)                 
	--			End
	                  
	--			print (convert(varchar,@i) + ' / ' + convert(varchar(20), getdate(), 114) + ' - SubscriptionID :' + convert(varchar(100),@KeepSubscriptionID) ) --+ ' - ' + @FNAME + ' - ' + @LNAME + ' - ' + @COMPANY + ' - ' + @ADDRESS + ' - ' + @CITY + ' - ' + @State + ' - ' + @ZIP + ' - ' + @PHONE + ' - ' + @EMAIL
	            
	--			if exists (select top 1 pubID from PubSubscriptions ps  WITH (NOLOCK) join @tmp t on ps.SubscriptionID = t.SubscriptionID group by PubID having COUNT(pubsubscriptionID) > 1)
	--			Begin
	                              
	--				  DECLARE c_PubSubscriptions CURSOR 
	--				  FOR select ps.pubID, ps.PubSubscriptionID 
	--				  from PubSubscriptions ps  with (NOLOCK) join  @tmp t on t.subscriptionID = ps.subscriptionID
	--				  where ps.SubscriptionID <> @KeepSubscriptionID and
	--				  PubID in (select pubID from PubSubscriptions ps    with (NOLOCK) where SubscriptionID in (select SubscriptionID from @tmp) group by pubID having COUNT(*) > 1)

	--				  OPEN c_PubSubscriptions  
	--				  FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
	                  
	--				  WHILE @@FETCH_STATUS = 0  
	--				  BEGIN  
	--						--print (' OPEN c_PubSubscriptions / ' + convert(varchar(20), getdate(), 114)   )           
	                        
	--						select @KeepPubsubscriptionID = PubSubscriptionID 
	--						from PubSubscriptions with (NOLOCK)
	--						where PubID = @pubID and SubscriptionID = @KeepSubscriptionID

	--						if (@KeepPubsubscriptionID = 0)
	--						Begin
	                        
	--							  print 'Insert KeepPubsubscriptionID : '
	                        
	--							  insert into PubSubscriptions (SubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email)
	--							  select @KeepSubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email
	--							  from PubSubscriptions  WITH (nolock) 
	--							  where pubsubscriptionID = @pubsubscriptionID
	                              
	--							  set @KeepPubsubscriptionID = @@IDENTITY
	--						End

	                        
	--						/********* pubSubscriptiondetail ***********/
	--						DECLARE c_PubSubscriptiondetail CURSOR 
	--						FOR 
	--						select CodesheetID from PubSubscriptionDetail  WITH (NOLOCK)
	--							  where PubSubscriptionID in (@pubsubscriptionID, @KeepPubsubscriptionID)
	--							  group by  CodesheetID
	--							  having COUNT(pubsubscriptiondetailID) > 1
	                        
	--						--print (' c_PubSubscriptiondetail / ' + convert(varchar(20), getdate(), 114)   )
	                              
	--						OPEN c_PubSubscriptiondetail  
	--						FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
	                        
	--						WHILE @@FETCH_STATUS = 0  
	--						BEGIN  
	                              
	--							  delete from PubSubscriptionDetail
	--							  where PubSubscriptionID = @pubsubscriptionID and CodesheetID = @codesheetID
	                        
	--							  FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
	--						END

	--						CLOSE c_PubSubscriptiondetail  
	--						DEALLOCATE c_PubSubscriptiondetail  
	--						/********* pubSubscriptiondetail ***********/

	--						update PubSubscriptionDetail
	--						set PubSubscriptionID = @KeepPubsubscriptionID
	--						where PubSubscriptionID = @pubsubscriptionID
	                        
	--						delete from PubSubscriptions 
	--						where  PubSubscriptionID = @pubsubscriptionID and PubID = @pubID
	                        
	--						FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
	                        
	--				  END

	--				  CLOSE c_PubSubscriptions  
	--				  DEALLOCATE c_PubSubscriptions  
	--			End
	                  
	--			update PubSubscriptionDetail
	--			set SubscriptionID = @KeepSubscriptionID
	--			where SubscriptionID in  (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			update PubSubscriptions
	--			set SubscriptionID = @KeepSubscriptionID
	--			where SubscriptionID in  (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			delete from SubscriberMasterValues where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from SubscriptionsExtension where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from SubscriptionDetails where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from Subscriptions where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			set @i = @i + 1
	                  
	--			FETCH NEXT FROM c_Subscriptions INTO @KeepSubscriptionID

	--	  END

	--	  CLOSE c_Subscriptions  
	--	  DEALLOCATE c_Subscriptions  

	--	  set @loopcounter = @loopcounter + 1      

	--	  truncate table subscriptiondetails

	--	  truncate table dbo.SubscriberMasterValues

	--	  insert into subscriptiondetails
	--		  select distinct subscriptionID, cb.masterID 
	--		  from  PubSubscriptionDetail psd with (NOLOCK) join 
	--					  CodeSheet_Mastercodesheet_Bridge cb  with (NOLOCK)  on psd.CodeSheetID = cb.CodeSheetID
		            
	--	  insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	--		  SELECT 
	--			MasterGroupID, [SubscriptionID] , 
	--			STUFF((
	--				SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
	--				FROM [dbo].[SubscriptionDetails] sd1   WITH (nolock) join Mastercodesheet mc1   WITH (nolock) on sd1.MasterID = mc1.MasterID  
	--				WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
	--				FOR XML PATH (''))
	--			,1,1,'') AS CombinedValues
	--		  FROM 
	--				(
	--				  SELECT DISTINCT sd.SubscriptionID, mg.MasterGroupID
	--				  FROM [dbo].[SubscriptionDetails] sd  WITH (nolock)  join Mastercodesheet mc  WITH (nolock)  on sd.MasterID = mc.MasterID join MasterGroups mg  WITH (nolock)  on mg.MasterGroupID = mc.MasterGroupID	            
	--				)
	--		  Results
	--		  GROUP BY [SubscriptionID] , MasterGroupID
	--		  ORDER BY SubscriptionID             
		      
	--END -- End while loop

	--drop table #curtable
	--drop table #NameSwapID

GO


PRINT N'Creating [dbo].[e_AdhocCategory_Save]...';


GO
CREATE PROCEDURE [dbo].[e_AdhocCategory_Save]
	@CategoryID int,
	@CategoryName varchar(50),
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
	IF @CategoryID > 0
		BEGIN						
			UPDATE AdhocCategory
			SET CategoryName = @CategoryName,
				SortOrder = @SortOrder
			WHERE CategoryID = @CategoryID;
		
			SELECT @CategoryID;
		END
	ELSE
		BEGIN
			INSERT INTO AdhocCategory (CategoryName,SortOrder)
			VALUES(@CategoryName,@SortOrder);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_AdhocCategory_Select_All]...';


GO
CREATE PROCEDURE [dbo].[e_AdhocCategory_Select_All]
AS
	SELECT * FROM AdhocCategory With(NoLock)
GO
PRINT N'Creating [dbo].[e_ClientCustomProcedure_ExecuteSproc_Xml]...';


GO
CREATE PROCEDURE e_ClientCustomProcedure_ExecuteSproc_Xml
@sproc varchar(250),
@xml xml
as
	DECLARE @execsproc varchar(MAX)
	SET @execsproc = 'EXEC '+@sproc + '''' + cast(@xml as varchar(max)) + '''' 
	Exec(@execsproc)
GO
PRINT N'Creating [dbo].[e_CodeSheet_Delete_CodeSheetID]...';


GO
CREATE PROCEDURE [dbo].[e_CodeSheet_Delete_CodeSheetID]
	@CodeSheetID int
AS
	DELETE FROM CodeSheet WHERE CodeSheetID = @CodeSheetID
GO
PRINT N'Creating [dbo].[e_CodeSheet_Save]...';


GO
CREATE PROCEDURE [dbo].[e_CodeSheet_Save]
	@CodeSheetID int, 
	@PubID int, 
	@ResponseGroupID int, 
	@ResponseGroup varchar(255), 
	@ResponseValue varchar(255), 
	@ResponseDesc varchar(255),
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int,	
	@DisplayOrder int,	
	@ReportGroupID int,
	@IsActive bit,
	@WQT_ResponseID int,
	@IsOther bit
AS
	IF @CodeSheetID > 0
	BEGIN						
		UPDATE CodeSheet
			SET ResponseGroupID = @ResponseGroupID, 
				ResponseGroup = @ResponseGroup, 
				ResponseValue = @ResponseValue, 
				ResponseDesc = @ResponseDesc,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DisplayOrder = @DisplayOrder,	
				ReportGroupID = @ReportGroupID,
				IsActive = @IsActive,
				WQT_ResponseID = @WQT_ResponseID,
				IsOther = @IsOther
			WHERE CodeSheetID = @CodeSheetID
		
		SELECT @CodeSheetID;
	END
	ELSE
		BEGIN			
			INSERT INTO CodeSheet ([PubID], [ResponseGroupID], [ResponseGroup], [ResponseValue], [ResponseDesc], [DateCreated], [CreatedByUserID], [DisplayOrder], [ReportGroupID], [IsActive], [WQT_ResponseID], [IsOther])
			VALUES(@PubID, @ResponseGroupID, @ResponseGroup, @ResponseValue, @ResponseDesc, @DateCreated, @CreatedByUserID, @DisplayOrder, @ReportGroupID, @IsActive, @WQT_ResponseID, @IsOther);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_CodeSheetMasterCodeSheetBridge_Delete_CodeSheetID]...';


GO
CREATE PROCEDURE [dbo].[e_CodeSheetMasterCodeSheetBridge_Delete_CodeSheetID]
	@CodeSheetID int
AS
	DELETE FROM CodeSheet_Mastercodesheet_Bridge WHERE CodeSheetID = @CodeSheetID
GO
PRINT N'Creating [dbo].[e_CodeSheetMasterCodeSheetBridge_Delete_MasterID]...';


GO
CREATE PROCEDURE [dbo].[e_CodeSheetMasterCodeSheetBridge_Delete_MasterID]
	@MasterID int
AS	
	DELETE FROM CodeSheet_Mastercodesheet_Bridge WHERE MasterID = @MasterID
GO
PRINT N'Creating [dbo].[e_CodeSheetMasterCodeSheetBridge_Save]...';


GO
CREATE PROCEDURE [dbo].[e_CodeSheetMasterCodeSheetBridge_Save]
	@CodeSheetID int,
	@MasterID int
AS
	INSERT INTO CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID) 
	VALUES (@CodeSheetID, @MasterID)
GO
PRINT N'Creating [dbo].[e_Database_Select]...';


GO
CREATE PROCEDURE [dbo].[e_Database_Select]	
AS
	SELECT name as 'DatabaseName' 
	FROM sys.databases With(NoLock) 
	where database_id not in (1,2,3,4)
GO
PRINT N'Creating [dbo].[e_MasterCodeSheet_Delete_MasterID]...';


GO
CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Delete_MasterID]
	@MasterID int
AS	
	DELETE FROM Mastercodesheet WHERE MasterID = @MasterID
GO
PRINT N'Creating [dbo].[e_MasterCodeSheet_Import_Subscriber]...';


GO
CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Import_Subscriber]
	@MasterGroupID INT,
	@importXML TEXT
AS
BEGIN	
		
	DECLARE @docHandle INT		

	SET NOCOUNT ON

	CREATE TABLE #tmpData (
		IGROUPNO UNIQUEIDENTIFIER, 
		mastervalue VARCHAR(100),
		masterdesc VARCHAR(255), 
		CONSTRAINT [PK_TMPDATA] PRIMARY KEY (IGROUPNO, MASTERVALUE, MASTERDESC))

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @importXML  

	INSERT INTO #tmpData (
		IGROUPNO, 
		MASTERVALUE, 
		MASTERDESC
		)  
	SELECT 	DISTINCT 
		IGROUPNO, 
		MASTERVALUE,
		MASTERDESC
	FROM 
		OPENXML(@docHandle, N'/mastergrouplist/mastergroup')   
	WITH   
		(  
		IGROUPNO VARCHAR(255) 'igroupno', 
		MASTERVALUE VARCHAR(50) 'mastervalue', 
		MASTERDESC VARCHAR(255) 'masterdesc'
		) 

	EXEC sp_xml_removedocument @docHandle  

	SELECT * FROM #tmpData

	INSERT INTO Mastercodesheet
	SELECT DISTINCT 
		MASTERVALUE, 
		MASTERDESC, 
		@MasterGroupID, 
		NULL, 
		0, 
		NULL 
	FROM 
		#tmpData
	WHERE 
		MASTERVALUE NOT IN (SELECT MC.MASTERVALUE FROM Mastercodesheet MC WHERE mastergroupID = @MasterGroupID)

	DECLARE @Subscriber table (SUBSCRIPTIONID int, IGROUPNO UNIQUEIDENTIFIER, MASTERID INT)

	INSERT INTO @Subscriber
	SELECT DISTINCT  
		s.SubscriptionID, 
		s.IGRP_NO, 
		masterID 
	FROM
		subscriptions s 
		JOIN #tmpData t ON t.IGROUPNO = s.igrp_no 
		JOIN Mastercodesheet ms ON t.MASTERVALUE = ms.MASTERVALUE
	WHERE 
		MasterGroupID = @MasterGroupID

	INSERT INTO SubscriptionDetails
	SELECT 
		S.subscriptionID, 
		S.masterID 
	FROM 
		@Subscriber S 
		LEFT OUTER JOIN SubscriptionDetails SD ON S.subscriptionID = SD.subscriptionID AND S.MASTERID = SD.MASTERID
	WHERE 
		SD.MASTERID IS NULL

	DROP TABLE #tmpData

	DECLARE @SubscriptionID int,
			@mvalue VARCHAR(1000)
				
	DECLARE c_subs CURSOR FOR SELECT DISTINCT SUBSCRIPTIONID FROM @Subscriber

	OPEN c_subs  
	FETCH NEXT FROM c_subs INTO @SubscriptionID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		SET @mvalue = ''

		SELECT @mvalue = STUFF((
					SELECT 
						DISTINCT ',' + mastervalue 
					FROM 
						SubscriptionDetails sd 
						JOIN mastercodesheet m ON sd.masterID = m.masterID 
					WHERE 
						sd.SubscriptionID = @SubscriptionID 
						AND m.MasterGroupID = @MasterGroupID
					ORDER BY 
						',' + mastervalue
					FOR XML PATH('')),1, 1, '')
	                		
		IF EXISTS (SELECT TOP 1 subscriptionID FROM SubscriberMasterValues WHERE SubscriptionID = @SubscriptionID AND MasterGroupID = @MasterGroupID )
		BEGIN
			UPDATE 
				SubscriberMasterValues
			SET 
				[MastercodesheetValues] = @mvalue
			WHERE 
				SubscriptionID = @SubscriptionID 
				AND MasterGroupID = @MasterGroupID
		END
		ELSE
		BEGIN
			INSERT INTO SubscriberMasterValues (
				MasterGroupID,
				SubscriptionID,
				[MastercodesheetValues]) 
			VALUES (
				@MasterGroupID, 
				@SubscriptionID, 
				@mvalue	)
		END		
		FETCH NEXT FROM c_subs INTO @SubscriptionID

	END
	
	CLOSE c_subs  
	DEALLOCATE c_subs 
		
END
GO
PRINT N'Creating [dbo].[e_MasterCodeSheet_Save]...';


GO
CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Save]
	@MasterID int, 
	@MasterGroupID int, 
	@MasterValue varchar(100), 
	@MasterDesc varchar(255),
	@MasterDesc1 varchar(255),
	@EnableSearching bit,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
	BEGIN
	IF @MasterID > 0
		BEGIN
			UPDATE Mastercodesheet
			SET MasterGroupID = @MasterGroupID, 
				MasterValue = @MasterValue, 
				MasterDesc = @MasterDesc,
				MasterDesc1 = @MasterDesc1,
				EnableSearching = @EnableSearching,
				SortOrder = @SortOrder
			WHERE MasterID = @MasterID	

			SELECT @MasterID;					
		END			
	ELSE
		BEGIN
			INSERT INTO [Mastercodesheet]([MasterGroupID], [MasterValue], [MasterDesc], [MasterDesc1], [EnableSearching], [SortOrder])
				VALUES (@MasterGroupID, @MasterValue, @MasterDesc, @MasterDesc1, @EnableSearching, @SortOrder);SELECT @@IDENTITY;
		END
	END
GO
PRINT N'Creating [dbo].[e_MasterCodeSheet_Select]...';


GO
CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Select]
AS
	SELECT * FROM MasterCodeSheet With(NoLock)
GO
PRINT N'Creating [dbo].[e_MasterCodeSheet_Select_MasterGroupID]...';


GO
CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Select_MasterGroupID]
	@MasterGroupID int
AS
	SELECT * FROM MasterCodeSheet With(NoLock) WHERE MasterGroupID = @MasterGroupID
GO
PRINT N'Creating [dbo].[e_MasterGroup_Delete]...';


GO
CREATE PROCEDURE [dbo].[e_MasterGroup_Delete]
	@MasterGroupID int
AS
	DELETE FROM MasterGroup WHERE MasterGroupID = @MasterGroupID
GO
PRINT N'Creating [dbo].[e_Product_Copy]...';


GO
CREATE PROCEDURE [dbo].[e_Product_Copy]
	@FromPubID int, 
	@ToPubID int
AS
BEGIN
	if exists (select top 1 * from CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (select CodeSheetID from CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID)))
			delete CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (select CodeSheetID from CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID))

	if exists (select top 1 * from CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID))
			delete CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID)
			
	if exists (select top 1 * from ResponseGroups where PubID = @ToPubID)
			delete ResponseGroups where PubID = @ToPubID
	
	--insert ResponseGroups		
	DECLARE @ReponseGroupIDNew int
	DECLARE @ReponseGroupID int
	DECLARE @ResponseGroupName varchar(100)
	DECLARE @DisplayName varchar(100)
	DECLARE c_ResponseGroups CURSOR FOR select ResponseGroupID, ResponseGroupName, DisplayName from ResponseGroups where PubID = @FromPubID
	OPEN c_ResponseGroups  
	FETCH NEXT FROM c_ResponseGroups INTO @ReponseGroupID, @ResponseGroupName, @DisplayName
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		SET @ReponseGroupIDNew = 0 
		Insert into ResponseGroups ( PubID, ResponseGroupName, DisplayName ) values ( @ToPubID, @ResponseGroupName, @DisplayName )
		SELECT @ReponseGroupIDNew = SCOPE_IDENTITY() 
		
		--insert CodeSheet
		DECLARE @CodeSheetIDNew int
		DECLARE @CodeSheetID int
		DECLARE @ResponseValue varchar(255)
		DECLARE @ResponseDesc varchar(255)
		DECLARE c_CodeSheet CURSOR FOR select CodeSheetID, ResponseValue, ResponseDesc from CodeSheet where ResponseGroupID = @ReponseGroupID
		OPEN c_CodeSheet
		FETCH NEXT FROM c_CodeSheet INTO @CodeSheetID, @ResponseValue, @ResponseDesc
		WHILE @@FETCH_STATUS = 0  
		BEGIN 
			SET @CodeSheetIDNew = 0 
			Insert into CodeSheet ( PubID, ResponseGroup, Responsevalue, Responsedesc, ResponseGroupID ) values ( @ToPubID, @ResponseGroupName, @ResponseValue, @ResponseDesc, @ReponseGroupIDNew )
			SELECT @CodeSheetIDNew = SCOPE_IDENTITY()
			
			--insert bridge
			DECLARE @MasterID int
			DECLARE c_Bridge CURSOR FOR select MasterID from CodeSheet_Mastercodesheet_Bridge where CodeSheetID = @CodeSheetID
			OPEN c_Bridge
			FETCH NEXT FROM c_Bridge INTO @MasterID
			WHILE @@FETCH_STATUS = 0  
			BEGIN
				Insert into CodeSheet_Mastercodesheet_Bridge ( CodeSheetID, MasterID ) values ( @CodeSheetIDNew, @MasterID )
				
				FETCH NEXT FROM c_Bridge INTO @MasterID
			END
			CLOSE c_Bridge  
			DEALLOCATE c_Bridge			
			
			FETCH NEXT FROM c_CodeSheet INTO @CodeSheetID, @ResponseValue, @ResponseDesc
		END
		CLOSE c_CodeSheet  
		DEALLOCATE c_CodeSheet		
		
		FETCH NEXT FROM c_ResponseGroups INTO @ReponseGroupID, @ResponseGroupName, @DisplayName
	END
	CLOSE c_ResponseGroups  
	DEALLOCATE c_ResponseGroups  		
	
	---------------------------------------------------------------------------------------------------------------
END
GO
PRINT N'Creating [dbo].[e_Product_Save]...';


GO
CREATE PROCEDURE [dbo].[e_Product_Save]
	@PubID int,
	@PubName varchar(100),
	@istradeshow bit,
	@PubCode varchar(50),
	@PubTypeID int,
	@GroupID int,
	@EnableSearching bit,
	@score int,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int,
	@ClientID int,
	@YearStartDate varchar(5),
	@YearEndDate varchar(5),
	@IssueDate datetime,
	@IsImported bit,
	@IsActive bit,
	@AllowDataEntry bit,
	@FrequencyID int,
	@KMImportAllowed bit,
	@ClientImportAllowed bit,
	@AddRemoveAllowed bit,
	@AcsMailerInfoId int,
	@IsUAD bit,
	@IsCirc bit
AS
	IF @PubID > 0
	BEGIN
		UPDATE Pubs
			SET PubName = @PubName,
				istradeshow = @istradeshow, 
				PubCode = @PubCode, 
				PubTypeID = @PubTypeID, 
				GroupID = @GroupID, 
				EnableSearching = @EnableSearching, 
				score = @score, 
				SortOrder = @SortOrder,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				ClientID = @ClientID,
				YearStartDate = @YearStartDate,
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
				IsUAD = @IsUAD,
				IsCirc = @IsCirc
		WHERE PubID = @PubID
		SELECT @PubID;
	END
	ELSE
	BEGIN
		INSERT INTO Pubs (PubName, istradeshow, PubCode, PubTypeID, GroupID, EnableSearching, score, SortOrder, DateCreated, CreatedByUserID, ClientID, YearStartDate, YearEndDate, IssueDate, IsImported, IsActive, AllowDataEntry, FrequencyID, KMImportAllowed, ClientImportAllowed, AddRemoveAllowed, AcsMailerInfoId, IsUAD, IsCirc)
		VALUES (@PubName, @istradeshow, @PubCode, @PubTypeID, @GroupID, @EnableSearching, @score, @SortOrder, @DateCreated, @CreatedByUserID, @ClientID, @YearStartDate, @YearEndDate, @IssueDate, @IsImported, @IsActive, @AllowDataEntry, @FrequencyID, @KMImportAllowed, @ClientImportAllowed, @AddRemoveAllowed, @AcsMailerInfoId, @IsUAD, @IsCirc)
	END
GO
PRINT N'Creating [dbo].[e_ProductAudit_Save]...';


GO
create procedure e_ProductAudit_Save
@ProductAuditId int,
@ProductId int,
@AuditField varchar(255),
@FieldMappingTypeId int,
@ResponseGroupID int,
@SubscriptionsExtensionMapperID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
	
	IF @ProductAuditId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
					
			UPDATE ProductAudit
				SET ProductId = @ProductId,
					AuditField = @AuditField,
					FieldMappingTypeId = @FieldMappingTypeId,
					ResponseGroupID = @ResponseGroupID,
					SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperID,
					IsActive = @IsActive,
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID
			WHERE ProductAuditId = @ProductAuditId
			SELECT @ProductAuditId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO ProductAudit (ProductId,AuditField,FieldMappingTypeId,ResponseGroupID,SubscriptionsExtensionMapperID,IsActive,DateCreated,CreatedByUserID)
			VALUES (@ProductId,@AuditField,@FieldMappingTypeId,@ResponseGroupID,@SubscriptionsExtensionMapperID,@IsActive,@DateCreated,@CreatedByUserID);Select @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_ProductAudit_Select_ProductId]...';


GO
create procedure e_ProductAudit_Select_ProductId
@ProductId int
as
	select *
	from ProductAudit with(nolock)
	where ProductId = @ProductId
	order by AuditField
GO
PRINT N'Creating [dbo].[e_ProductGroups_Delete]...';


GO
CREATE PROCEDURE [dbo].[e_ProductGroups_Delete]
	@PubID int
AS
	DELETE PubGroups WHERE PubID = @PubID
GO
PRINT N'Creating [dbo].[e_ProductGroups_Save]...';


GO
CREATE PROCEDURE [dbo].[e_ProductGroups_Save]
	@PubID int,
	@GroupID int
AS
	INSERT INTO PubGroups (PubID, GroupID)
	VALUES (@PubID, @GroupID)
GO
PRINT N'Creating [dbo].[e_ProductGroups_Select]...';


GO
CREATE PROCEDURE [dbo].[e_ProductGroups_Select]
AS
	SELECT * FROM PubGroups With(NoLock)
GO
PRINT N'Creating [dbo].[e_ProductSubscription_Save]...';


GO
create procedure e_ProductSubscription_Save
@PubSubscriptionID int,
@SubscriptionID	int,
@PubID int,
@Demo7 varchar(1),
@QualificationDate date,
@PubQSourceID int,
@PubCategoryID int,
@PubTransactionID int,
@EmailStatusID int,
@StatusUpdatedDate datetime,
@StatusUpdatedReason varchar(200),
@Email varchar(100),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
	IF @PubSubscriptionID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE PubSubscriptions
			SET SubscriptionID = @SubscriptionID,
				PubID = @PubID,
				Demo7 = @Demo7,
				QualificationDate = @QualificationDate,
				PubQSourceID = @PubQSourceID,
				PubCategoryID = @PubCategoryID,
				PubTransactionID = @PubTransactionID,
				EmailStatusID = @EmailStatusID,
				StatusUpdatedDate = @StatusUpdatedDate,
				StatusUpdatedReason = @StatusUpdatedReason,
				Email = @Email,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE PubSubscriptionID = @PubSubscriptionID;

			SELECT @PubSubscriptionID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO PubSubscriptions (SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,
											 StatusUpdatedReason,Email,DateCreated,CreatedByUserID)
			VALUES(@SubscriptionID,@PubID,@Demo7,@QualificationDate,@PubQSourceID,@PubCategoryID,@PubTransactionID,@EmailStatusID,@StatusUpdatedDate,
				   @StatusUpdatedReason,@Email,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_ProductTypes_Delete]...';


GO
CREATE PROCEDURE [dbo].[e_ProductTypes_Delete]
	@PubTypeID int
AS
	DELETE PubTypes WHERE PubTypeID = @PubTypeID
GO
PRINT N'Creating [dbo].[e_ProductTypes_Save]...';


GO
CREATE PROCEDURE [dbo].[e_ProductTypes_Save]
	@PubTypeID int, 
	@PubTypeDisplayName varchar(50),
	@ColumnReference  varchar(50),
	@IsActive  bit,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
as
BEGIN
	Declare @CurrentSortOrder int

	IF @PubTypeID > 0
		BEGIN						
			SELECT @CurrentSortOrder = SortOrder from  PubTypes where PubTypeID = @PubTypeID;

			UPDATE PubTypes 
				SET PubTypeDisplayName =  @PubTypeDisplayName, 
					ColumnReference =  @ColumnReference,
					IsActive =  @IsActive,
					SortOrder = @SortOrder  
			WHERE PubTypeID = @PubTypeID			
			
			IF (@SortOrder < @SortOrder)
			BEGIN
				 UPDATE PubTypes set SortOrder = SortOrder - 1 WHERE SortOrder >=  @CurrentSortOrder and  SortOrder <=   @SortOrder and PubTypeID <> @PubTypeID
			END
			ELSE
			BEGIN			  
				UPDATE PubTypes set SortOrder = SortOrder + 1 WHERE SortOrder >=  @SortOrder and  SortOrder <=  @CurrentSortOrder and PubTypeID <> @PubTypeID
			END
			
			SELECT @PubTypeID
			
		END
	ELSE
		BEGIN
			
  			SELECT @CurrentSortOrder = ISNULL(MAX(sortorder),0)+1 from PubTypes
		
			INSERT INTO PubTypes (PubTypeDisplayName, ColumnReference, IsActive, SortOrder) 
			VALUES (@PubTypeDisplayName, @ColumnReference, @IsActive, @SortOrder) 
					
			IF (@CurrentSortOrder < @SortOrder)
			BEGIN
				 UPDATE PubTypes set SortOrder = SortOrder - 1 WHERE SortOrder >= @CurrentSortOrder and  SortOrder <= @SortOrder and PubTypeID <> @@IDENTITY
			END
			ELSE
			BEGIN			  
				UPDATE PubTypes set SortOrder = SortOrder + 1 WHERE SortOrder >= @SortOrder and  SortOrder <= @CurrentSortOrder and PubTypeID <> @@IDENTITY
			END					
					
			SELECT @@IDENTITY;
		END	
END
GO
PRINT N'Creating [dbo].[e_ProductTypes_Select]...';


GO
CREATE PROCEDURE [dbo].[e_ProductTypes_Select]
AS
	SELECT * FROM PubTypes With(NoLock)
GO
PRINT N'Creating [dbo].[e_PubCode_Select]...';


GO
CREATE PROCEDURE [dbo].[e_PubCode_Select]	
AS
	SELECT pubcode From pubs With(NoLock)
GO
PRINT N'Creating [dbo].[e_PubSubscriptionDetail_Delete_CodeSheetID]...';


GO
CREATE PROCEDURE [dbo].[e_PubSubscriptionDetail_Delete_CodeSheetID]
	@CodeSheetID int
AS
	DELETE FROM PubSubscriptionDetail WHERE CodeSheetID = @CodeSheetID
GO
PRINT N'Creating [dbo].[e_QSource_Save]...';


GO
create procedure e_QSource_Save
@QSourceID int,
@QSourceValue varchar(2),
@QSourceName varchar(100),
@QsourceGroupID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
	IF @QSourceID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE QSource
			SET QSourceValue = @QSourceValue,
				QSourceName = @QSourceName,
				QsourceGroupID = @QsourceGroupID,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE QSourceID = @QSourceID;

			SELECT @QSourceID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO QSource (QSourceValue,QSourceName,QsourceGroupID,DateCreated,CreatedByUserID)
			VALUES(@QSourceValue,@QSourceName,@QsourceGroupID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_QSourceGroup_Save]...';


GO
create procedure e_QSourceGroup_Save
@QSourceGroupID int,
@QSourceGroupName varchar(100),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
	IF @QSourceGroupID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE QSourceGroup
			SET QSourceGroupName = @QSourceGroupName,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE QSourceGroupID = @QSourceGroupID;

			SELECT @QSourceGroupID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO QSourceGroup (QSourceGroupName,DateCreated,CreatedByUserID)
			VALUES(@QSourceGroupName,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_ReportGroups_Save]...';


GO
CREATE PROCEDURE [dbo].[e_ReportGroups_Save]
	@ReportGroupID int,
	@ResponseGroupID int,
	@DisplayName varchar(50),
	@DisplayOrder int
AS
	IF @ReportGroupID > 0
		BEGIN
			UPDATE ReportGroups
			SET 				
				ResponseGroupID = @ResponseGroupID,
				DisplayName = @DisplayName,
				DisplayOrder = @DisplayOrder
			WHERE ReportGroupID = @ReportGroupID 
		
			SELECT @ReportGroupID;
		END
	ELSE
		BEGIN
			INSERT INTO ReportGroups (ResponseGroupID,DisplayName,DisplayOrder)
			VALUES(@ResponseGroupID,@DisplayName,@DisplayOrder);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_ReportGroups_Select]...';


GO
CREATE PROCEDURE [dbo].[e_ReportGroups_Select]
AS
	SELECT * FROM ReportGroups With(NoLock)
GO
PRINT N'Creating [dbo].[e_Reports_Save]...';


GO

CREATE PROCEDURE [dbo].[e_Reports_Save]
@ReportID int,
@ReportName varchar(200),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@ProvideID bit,
@ProductID int,
@URL varchar(250),
@IsCrossTabReport bit,
@Row varchar(50),
@Column varchar(50),
@SuppressTotal bit,
@Status bit
AS

IF @ReportID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Reports
		SET ReportName = @ReportName,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			ProvideID = @ProvideID,
			ProductID = @ProductID,
			URL = @URL,
			IsCrossTabReport = @IsCrossTabReport,
			[Row] = @Row,
			[Column] = @Column,
			SuppressTotal = @SuppressTotal,
			[Status] = @Status
		WHERE ReportID = @ReportID;
		
		SELECT @ReportID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Reports (ReportName,IsActive,DateCreated,CreatedByUserID,ProvideID,ProductID,URL,IsCrossTabReport,[Row],[Column],SuppressTotal,[Status])
		VALUES(@ReportName,@IsActive,@DateCreated,@CreatedByUserID,@ProvideID,@ProductID,@URL,@IsCrossTabReport,@Row,@Column,@SuppressTotal,@Status);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_Reports_Select]...';


GO

CREATE PROCEDURE [dbo].[e_Reports_Select]
AS
	SELECT *
	FROM Reports With(NoLock)
GO
PRINT N'Creating [dbo].[e_Response_Delete]...';


GO
CREATE PROCEDURE [dbo].[e_Response_Delete]
	@ResponseID int
AS
	DELETE Response WHERE ResponseID = @ResponseID
GO
PRINT N'Creating [dbo].[e_Response_Save]...';


GO
CREATE PROCEDURE e_Response_Save
@ResponseID int,
@ResponseGroupID int,
@PublicationID int,
@ResponseName varchar(250),
@ResponseCode nchar(10),
@DisplayName varchar(250),
@DisplayOrder int,
@ReportGroupID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@WQT_ResponseID int,
@IsOther bit
AS

IF @ResponseID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Response
		SET 
			ResponseGroupID = @ResponseGroupID,
			PublicationID = @PublicationID,
			ResponseName = @ResponseName,
			ResponseCode = @ResponseCode,
			DisplayName = @DisplayName,
			DisplayOrder = @DisplayOrder,
			ReportGroupID = @ReportGroupID,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			WQT_ResponseID = @WQT_ResponseID,
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
		INSERT INTO Response (ResponseGroupID,PublicationID,ResponseName,ResponseCode,DisplayName,DisplayOrder,ReportGroupID,IsActive,DateCreated,CreatedByUserID,WQT_ResponseID,IsOther)
		VALUES(@ResponseGroupID,@PublicationID,@ResponseName,@ResponseCode,@DisplayName,@DisplayOrder,@ReportGroupID,@IsActive,@DateCreated,@CreatedByUserID,@WQT_ResponseID,@IsOther);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_Response_Select]...';


GO

CREATE PROCEDURE e_Response_Select
AS
	SELECT * FROM Response With(NoLock)
GO
PRINT N'Creating [dbo].[e_Response_Select_PublicationID]...';


GO
CREATE PROCEDURE e_Response_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM Response With(NoLock) WHERE PublicationID = @PublicationID
GO
PRINT N'Creating [dbo].[e_ResponseGroup_Copy]...';


GO
CREATE PROCEDURE [dbo].[e_ResponseGroup_Copy]	
	@srcResponseGroupID int,
	@destPubsXML TEXT  -- 	set @destPubsXML = '<XML><Pub ID="1" /> <Pub ID="2" /> <Pub ID="3" /> </XML>'
as
BEGIN		
	set nocount on
	
	DECLARE @ResponseGroupName varchar(50),
			@docHandle int,
			@destPubID int,
			@destResponseGroupID int
			
	SELECT @ResponseGroupName = responsegroupname from ResponseGroups where ResponseGroupID = @srcResponseGroupID
	
	DECLARE @tbl_Destinationpub Table (PubID int)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @destPubsXML  

	INSERT into @tbl_Destinationpub
	SELECT PubID FROM OPENXML(@docHandle, N'/XML/Pub') WITH ( PubID int '@ID' )  

	EXEC sp_xml_removedocument @docHandle 

	DECLARE c_Emails CURSOR FOR SELECT PubID from @tbl_Destinationpub

	OPEN c_Emails  
	FETCH NEXT FROM c_Emails INTO @destPubID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		set @destResponseGroupID = 0
		SELECT @destResponseGroupID = responsegroupID from ResponseGroups where PubID = @destPubID and ResponseGroupName = @ResponseGroupName
		
		IF (@destResponseGroupID = 0)
		BEGIN
			INSERT into ResponseGroups (PubID,ResponseGroupName,DisplayName)
			SELECT @destPubID, ResponseGroupName, DisplayName from ResponseGroups where ResponseGroupID = @srcResponseGroupID
			
			SELECT @destResponseGroupID = @@IDENTITY
		End

		IF ( @destResponseGroupID > 0)
		BEGIN
			DELETE from CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (SELECT CodeSheetID from CodeSheet where ResponseGroupID = @destResponseGroupID )
			DELETE from CodeSheet where ResponseGroupID = @destResponseGroupID

			INSERT into codesheet (pubID, responsegroup, ResponseValue, Responsedesc, responsegroupID)
			SELECT @destPubID, @ResponseGroupName, ResponseValue, Responsedesc, @destResponseGroupID   FROM codesheet where responsegroupID = @srcResponseGroupID   
			
			INSERT into CodeSheet_Mastercodesheet_Bridge (CodesheetID , MasterID)
			SELECT c2.codesheetID, cmb.masterID 
			FROM	CodeSheet_Mastercodesheet_Bridge cmb join 
					codesheet c on cmb.codesheetID = c.codesheetID join 
					codesheet c2 on c2.responsevalue = c.responsevalue  
			where 
					c.responsegroupID = @srcResponseGroupID and 
					c2.responsegroupID = @destResponseGroupID
		End

		FETCH NEXT FROM c_Emails INTO @destPubID
	END

	CLOSE c_Emails  
	DEALLOCATE c_Emails  

End
GO
PRINT N'Creating [dbo].[e_ResponseGroup_Delete]...';


GO
CREATE PROCEDURE [dbo].[e_ResponseGroup_Delete]
	@ResponseGroupID int
AS
BEGIN
	DELETE from CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (select c.CodeSheetID from CodeSheet c where ResponseGroupID = @ResponseGroupID)
	DELETE from PubSubscriptionDetail where CodeSheetID in (select c.CodeSheetID from CodeSheet c where ResponseGroupID = @ResponseGroupID)
	DELETE from CodeSheet where ResponseGroupID = @ResponseGroupID
	DELETE from ResponseGroups where ResponseGroupID =  @ResponseGroupID
END
GO
PRINT N'Creating [dbo].[e_ResponseType_Delete_PublicationID]...';


GO
CREATE PROCEDURE [dbo].[e_ResponseType_Delete_PublicationID]
	@PublicationID int
AS
	DELETE ResponseType WHERE PublicationID = @PublicationID
GO
PRINT N'Creating [dbo].[e_ResponseType_Save]...';


GO
CREATE PROCEDURE e_ResponseType_Save
@ResponeTypeID int,
@PublicationID int,
@ResponseTypeName varchar(100),
@DisplayName varchar(100),
@DisplayOrder int,
@IsMultipleValue bit,
@IsRequired bit,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ResponeTypeID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ResponseType
		SET 
			PublicationID = @PublicationID,
			ResponseTypeName = @ResponseTypeName,
			DisplayName = @DisplayName,
			DisplayOrder = @DisplayOrder,
			IsMultipleValue = @IsMultipleValue,
			IsRequired = @IsRequired,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ResponseTypeID = @ResponeTypeID;
		
		SELECT @ResponeTypeID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ResponseType (PublicationID,ResponseTypeName,DisplayName,DisplayOrder,IsMultipleValue,IsRequired,IsActive,DateCreated,CreatedByUserID)
		VALUES(@PublicationID,@ResponseTypeName,@DisplayName,@DisplayOrder,@IsMultipleValue,@IsRequired,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_ResponseType_Select]...';


GO
CREATE PROCEDURE e_ResponseType_Select
AS
	SELECT * FROM ResponseType With(NoLock)
GO
PRINT N'Creating [dbo].[e_ResponseType_Select_PublicationID]...';


GO
CREATE PROCEDURE e_ResponseType_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM ResponseType With(NoLock) WHERE PublicationID = @PublicationID
GO
PRINT N'Creating [dbo].[e_SubscriberMasterValues_Delete_MasterID]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriberMasterValues_Delete_MasterID]
	@MasterID int
AS	
	DELETE FROM SubscriberMasterValues WHERE MasterGroupID in 
	(
		SELECT mcs.MasterGroupID FROM SubscriberMasterValues smv
		JOIN Mastercodesheet mcs ON smv.MasterGroupID = mcs.MasterGroupID
		WHERE mcs.MasterID = @MasterID
	)
GO
PRINT N'Creating [dbo].[e_Subscription_Save]...';


GO
create procedure e_Subscription_Save
@SubscriptionID int,
@Sequence int,
@FName varchar(100),
@LName varchar(100),
@Title varchar(100),
@Company	varchar(100),
@Address	varchar(255),
@MailStop varchar(255),
@City varchar(50),
@State varchar(50),
@Zip	varchar(10),
@Plus4 varchar(4),
@ForZip varchar(50),
@County varchar(20),
@Country varchar(100),
@CountryID int,
@Phone varchar(100),
@PhoneExists	bit,
@Fax	varchar(100),
@FaxExists bit,
@Email varchar(100),
@EmailExists	bit,
@CategoryID int,
@TransactionID int,
@TransactionDate smalldatetime,
@QDate datetime,
@QSourceID int,
@RegCode	varchar(5),
@Verified varchar(1),
@SubSrc varchar(8),
@OrigsSrc varchar(8),
@Par3C varchar(1),
@Demo31 bit,
@Demo32 bit,
@Demo33 bit,
@Demo34 bit,
@Demo35 bit,
@Demo36 bit,
@Source varchar(50),
@Priority varchar(4),
@IGrp_Cnt int,
@CGrp_No int,
@CGrp_Cnt int,
@StatList bit,
@Sic varchar(8),
@SicCode varchar(20),
@Gender varchar(1024),
@IGrp_Rank varchar(2),
@CGrp_Rank varchar(2),
@Address3 varchar(255),
@Home_Work_Address varchar(10),
@PubIDs varchar(2000),
@Demo7 varchar(1),
@IsExcluded bit,
@Mobile varchar(30),
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsLatLonValid bit,
@LatLonMsg nvarchar(1000),
@Score int,
@IGrp_No uniqueidentifier,
@Notes varchar(2000),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@AddressTypeCodeId int,
@AddressLastUpdatedDate datetime,
@AddressUpdatedSourceTypeCodeId int
as
	IF @SubscriptionID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE Subscriptions
			SET Sequence = @Sequence,
				FName = @FName,
				LName = @LName,
				Title = @Title,
				Company = @Company,
				Address = @Address,
				MailStop = @MailStop,
				City = @City,
				State = @State,
				Zip = @Zip,
				Plus4 = @Plus4,
				ForZip = @ForZip,
				County = @County,
				Country = @Country,
				CountryID = @CountryID,
				Phone = @Phone,
				PhoneExists = @PhoneExists,
				Fax = @Fax,
				FaxExists = @FaxExists,
				Email = @Email,
				EmailExists = @EmailExists,
				CategoryID = @CategoryID,
				TransactionID = @TransactionID,
				TransactionDate = @TransactionDate,
				QDate = @QDate,
				QSourceID = @QSourceID,
				RegCode = @RegCode,
				Verified = @Verified,
				SubSrc = @SubSrc,
				OrigsSrc = @OrigsSrc,
				Par3C = @Par3C,
				Demo31 = @Demo31,
				Demo32 = @Demo32,
				Demo33 = @Demo33,
				Demo34 = @Demo34,
				Demo35 = @Demo35,
				Demo36 = @Demo36,
				Source = @Source,
				Priority = @Priority,
				IGrp_Cnt = @IGrp_Cnt,
				CGrp_No = @CGrp_No,
				CGrp_Cnt = @CGrp_Cnt,
				StatList = @StatList,
				Sic = @Sic,
				SicCode = @SicCode,
				Gender = @Gender,
				IGrp_Rank = @IGrp_Rank,
				CGrp_Rank = @CGrp_Rank,
				Address3 = @Address3,
				Home_Work_Address = @Home_Work_Address,
				PubIDs = @PubIDs,
				Demo7 = @Demo7,
				IsExcluded = @IsExcluded,
				Mobile = @Mobile,
				Latitude = @Latitude,
				Longitude = @Longitude,
				IsLatLonValid = @IsLatLonValid,
				LatLonMsg = @LatLonMsg,
				Score = @Score,
				IGrp_No = @IGrp_No,
				Notes = @Notes,
				AddressTypeCodeId = @AddressTypeCodeId,
				AddressLastUpdatedDate = @AddressLastUpdatedDate,
				AddressUpdatedSourceTypeCodeId = @AddressUpdatedSourceTypeCodeId
			WHERE SubscriptionID = @SubscriptionID;

			SELECT @SubscriptionID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Subscriptions (Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,Fax,
									   FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,
									   Demo34,Demo35,Demo36,Source,Priority,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,
									   Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,IGrp_No,Notes,DateCreated,CreatedByUserID,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId)
			VALUES(@Sequence,@FName,@LName,@Title,@Company,@Address,@MailStop,@City,@State,@Zip,@Plus4,@ForZip,@County,@Country,@CountryID,@Phone,@PhoneExists,@Fax,
				   @FaxExists,@Email,@EmailExists,@CategoryID,@TransactionID,@TransactionDate,@QDate,@QSourceID,@RegCode,@Verified,@SubSrc,@OrigsSrc,@Par3C,@Demo31,@Demo32,@Demo33,
				   @Demo34,@Demo35,@Demo36,@Source,@Priority,@IGrp_Cnt,@CGrp_No,@CGrp_Cnt,@StatList,@Sic,@SicCode,@Gender,@IGrp_Rank,@CGrp_Rank,@Address3,@Home_Work_Address,@PubIDs,
				   @Demo7,@IsExcluded,@Mobile,@Latitude,@Longitude,@IsLatLonValid,@LatLonMsg,@Score,@IGrp_No,@Notes,@DateCreated,@CreatedByUserID,@AddressTypeCodeId,@AddressLastUpdatedDate,@AddressUpdatedSourceTypeCodeId);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_SubscriptionDetails_Delete_MasterID]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriptionDetails_Delete_MasterID]
	@MasterID int
AS	
	DELETE FROM SubscriptionDetails WHERE MasterID = @MasterID
GO
PRINT N'Creating [dbo].[e_SubscriptionResponseMap_BulkUpdate]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_BulkUpdate]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		SubscriptionID int,
		ResponseID int,
		IsActive bit,
		DateCreated datetime,
		DateUpdated datetime,
		CreatedByUserID int,
		UpdatedByUserID int,
		ResponseOther varchar(300)
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 SubscriptionID, ResponseID, IsActive, DateCreated, DateUpdated,CreatedByUserID, UpdatedByUserID, ResponseOther
	)  
	
	SELECT SubscriptionID,ResponseID,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ResponseOther
	FROM OPENXML(@docHandle,N'/XML/SubscriptionResponseMap')
	WITH
	(
		SubscriptionID int 'SubscriptionID',
		ResponseID int 'ResponseID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ResponseOther varchar(300) 'ResponseOther'
	)
	
	EXEC sp_xml_removedocument @docHandle

	-- If the record joins, do updates
	UPDATE SubscriptionResponseMap
	SET 
		IsActive = i.IsActive,
		DateUpdated = CASE WHEN ISNULL(i.DateUpdated,'')='' THEN GETDATE() ELSE i.DateUpdated END,
		UpdatedByUserID = i.UpdatedByUserID,
		ResponseOther = i.ResponseOther
	FROM #import i
	WHERE SubscriptionResponseMap.SubscriptionID = i.SubscriptionID AND SubscriptionResponseMap.ResponseID = i.ResponseID;
	
	INSERT INTO SubscriptionResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
	SELECT DISTINCT i.SubscriptionID,i.ResponseID,i.IsActive,i.DateCreated,i.CreatedByUserID,i.ResponseOther 
	FROM #import i INNER JOIN SubscriptionResponseMap srm ON srm.SubscriptionID = i.SubscriptionID
	WHERE i.ResponseID NOT IN (Select srm.ResponseID FROM SubscriptionResponseMap srm INNER JOIN #import i on srm.SubscriptionID = i.SubscriptionID)

	INSERT INTO SubscriptionResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
	SELECT DISTINCT i.SubscriptionID,i.ResponseID,i.IsActive,i.DateCreated,i.CreatedByUserID,i.ResponseOther 
	FROM #import i
	WHERE i.SubscriptionID NOT IN (SELECT SubscriptionID FROM SubscriptionResponseMap GROUP BY SubscriptionID)
	
	-- Removes records that were marked inactive
	DELETE FROM SubscriptionResponseMap WHERE IsActive = 0 and SubscriptionID IN (SELECT SubscriptionID FROM #import GROUP BY SubscriptionID)

	DROP TABLE #import
GO
PRINT N'Creating [dbo].[e_SubscriptionResponseMap_Save]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Save]
@SubscriptionID int,
@ResponseID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@ResponseOther varchar(300)
AS

IF EXISTS(Select SubscriptionID From SubscriptionResponseMap With(NoLock) Where SubscriptionID = @SubscriptionID AND ResponseID = @ResponseID) 
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE SubscriptionResponseMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			ResponseOther = @ResponseOther
		WHERE SubscriptionID = @SubscriptionID AND ResponseID = @ResponseID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO SubscriptionResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
		VALUES(@SubscriptionID,@ResponseID,@IsActive,@DateCreated,@CreatedByUserID,@ResponseOther);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_SubscriptionResponseMap_Select]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select]
AS
	SELECT *
	FROM SubscriptionResponseMap With(NoLock)
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
PRINT N'Creating [dbo].[e_SubscriptionResponseMap_Select_SubscriberID]...';


GO
CREATE PROCEDURE e_SubscriptionResponseMap_Select_SubscriberID
@SubscriberID int
AS
	SELECT rm.* 
	FROM SubscriptionResponseMap rm With(NoLock) 
	JOIN Subscription s With(NoLock) ON s.SubscriptionID = rm.SubscriptionID
	WHERE s.SubscriberID = @SubscriberID
GO
PRINT N'Creating [dbo].[e_SubscriptionResponseMap_Select_SubscriptionID]...';


GO
CREATE PROCEDURE e_SubscriptionResponseMap_Select_SubscriptionID
@SubscriptionID int
AS
	SELECT * FROM SubscriptionResponseMap With(NoLock) WHERE SubscriptionID = @SubscriptionID
GO
PRINT N'Creating [dbo].[e_SubscriptionsExtensionMapper_Save]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Save]
	@SubscriptionsExtensionMapperId int,
	@StandardField varchar(255),
	@CustomField varchar(255),
	@CustomFieldDataType varchar(25),
	@Active bit,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
	IF @SubscriptionsExtensionMapperId > 0
		BEGIN
			UPDATE SubscriptionsExtensionMapper
			SET
				StandardField = @StandardField,
				CustomField = @CustomField,
				CustomFieldDataType = @CustomFieldDataType,
				Active = @Active
			WHERE SubscriptionsExtensionMapperId = @SubscriptionsExtensionMapperId
			Select @SubscriptionsExtensionMapperId
		END 
	ELSE
		BEGIN
			INSERT INTO SubscriptionsExtensionMapper (StandardField,CustomField,CustomFieldDataType,Active)           
			VALUES(@StandardField,@CustomField,@CustomFieldDataType,@Active);SELECT @@IDENTITY;
		END
GO
PRINT N'Creating [dbo].[e_SubscriptionsExtensionMapper_Select_All]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Select_All]
AS
	SELECT * FROM SubscriptionsExtensionMapper With(NoLock)
GO
PRINT N'Creating [dbo].[e_Table_Select]...';


GO
CREATE PROCEDURE [dbo].[e_Table_Select]	
AS
	SELECT TABLE_NAME as 'TableName'
    FROM INFORMATION_SCHEMA.TABLES With(NoLock)
    WHERE TABLE_TYPE = 'BASE TABLE'
GO
PRINT N'Creating [dbo].[e_UADTable_ExportData]...';


GO
CREATE PROCEDURE [dbo].[e_UADTable_ExportData]
	@Table varchar(100),
	@PubCode varchar(50)
AS	
    IF EXISTS(SELECT * FROM sys.columns 
            WHERE (UPPER([name]) = 'PUBCODE' OR UPPER([name]) = 'PUBID') AND [object_id] = OBJECT_ID(@Table))
    BEGIN    
    IF (@PubCode != '')
	    BEGIN
		    DECLARE @PubID int = (SELECT PubID FROM Pubs With(NoLock) WHERE PubCode = @PubCode)
		    IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubID%')
		    BEGIN
			    EXEC('SELECT * FROM ' + @Table + ' WHERE PubID = ' + @PubID);
		    END
		    ELSE
		    BEGIN
			    IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubCode%')
			    BEGIN
				    EXEC('SELECT * FROM ' + @Table + ' WHERE PubCode = ' + @PubCode);
			    END
			    ELSE
			    BEGIN
				    EXEC('SELECT * FROM ' + @Table);
			    END
		    END
	    END
	    ELSE
	    BEGIN
		    EXEC('SELECT * FROM ' + @Table);
	    END
    END
    ELSE
    BEGIN
	    BEGIN
		    EXEC('SELECT * FROM ' + @Table);
	    END
    END
GO
PRINT N'Creating [dbo].[job__ADMS_Remove_By_ProcessCode]...';


GO
CREATE PROCEDURE [dbo].[job__ADMS_Remove_By_ProcessCode]
	@ProcessCode varchar(50)	
AS	

	declare @subs Table (subscriptionID int, pubID int)
	declare @ps Table (PubSubscriptionID int)

	insert into @subs
	select distinct  s.SubscriptionID, p.pubID
	from subscriberfinal sf join Subscriptions s on sf.igrp_no = s.igrp_no join pubs p on p.pubcode = sf.pubcode
	where ProcessCode = @processCode and isupdatedinlive = 1

	insert into @ps
	select distinct ps.PubSubscriptionID from PubSubscriptions ps join @subs s on ps.SubscriptionID = s.subscriptionID and ps.PubID = s.pubID

	delete from SubscriberClickActivity where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)
	delete from SubscriberOpenActivity where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)
	delete from SubscriberTopicActivity where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)
	delete from PubSubscriptionDetail where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)
	delete from PubSubscriptions where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)

	declare @tblDeleteSubscriptionIDs table (SubscriptionID int PRIMARY KEY)
                  
	insert into @tblDeleteSubscriptionIDs
	select distinct s.subscriptionID 
	from
				Subscriptions s  WITH (nolock) left outer join
				PubSubscriptions ps  WITH (nolock) on s.SubscriptionID = ps.SubscriptionID
	where
				ps.SubscriptionID is null
            
	if exists (select top 1 SubscriptionID from @tblDeleteSubscriptionIDs)  
	Begin                         
		  delete from BrandScore  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from CampaignFilterDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from SubscriberClickActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriberOpenActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriberTopicActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriberVisitActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from SubscriberMasterValues  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriptionsExtension  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from PubSubscriptionDetail  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from PubSubscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from SubscriptionDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from Subscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
	End
                  
	-- repopulate concensus
	Delete from SubscriptionDetails
	Where 
				SubscriptionID in (select distinct s.SubscriptionID from @subs s) and
				MasterID in (select MasterID from CodeSheet_Mastercodesheet_Bridge)

	insert into SubscriptionDetails (SubscriptionID, MasterID)
	select distinct psd.SubscriptionID, cmb.masterID 
	from 
				@subs t join
				PubSubscriptionDetail psd on t.subscriptionID  = psd.SubscriptionID
				join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
				left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
	where  
				sd.sdID is null       

	Delete from SubscriberMasterValues
	Where 
				SubscriptionID in (select SubscriptionID from @subs s) 

	insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	SELECT 
	  MasterGroupID, [SubscriptionID] , 
	  STUFF((
		  SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
		  FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
		  WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
		  FOR XML PATH (''))
	  ,1,1,'') AS CombinedValues
	FROM 
		  (
				SELECT distinct sd.SubscriptionID, mc.MasterGroupID
				FROM  
							@subs t 
							join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.subscriptionID
							join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID                    
		  )
	Results
	GROUP BY [SubscriptionID] , MasterGroupID
	order by SubscriptionID   

	delete SubscriberDemographicInvalid  where SORecordIdentifier in (select so.SORecordIdentifier from SubscriberOriginal so where ProcessCode = @processCode)
	delete SubscriberDemographicArchive where SARecordIdentifier in (select sa.SARecordIdentifier from  SubscriberArchive sa where ProcessCode = @processCode)
	delete SubscriberDemographicFinal where SFRecordIdentifier in (select sf.SFRecordIdentifier from  SubscriberFinal sf where ProcessCode = @processCode)
	delete SubscriberDemographicTransformed where STRecordIdentifier in (select st.STRecordIdentifier from SubscriberTransformed st where ProcessCode = @processCode)
	delete SubscriberDemographicOriginal where SORecordIdentifier in (select so.SORecordIdentifier from SubscriberOriginal so where ProcessCode = @processCode)

	delete SubscriberInvalid where ProcessCode = @processCode
	delete SubscriberArchive where ProcessCode = @processCode
	delete SubscriberFinal where ProcessCode = @processCode
	delete SubscriberTransformed where  ProcessCode = @processCode
	delete SubscriberOriginal where  ProcessCode = @processCode

	delete uas..FileLog where ProcessCode = @processCode
GO
PRINT N'Creating [dbo].[job_ACS_Update_UAS_SubscriberAddress]...';


GO
create procedure job_ACS_Update_UAS_SubscriberAddress
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

	declare @userID int = (select UserID from UAS..[User] where EmailAddress = 'AcsImport@TeamKM.com')
	declare @addressUpdateSourceTypeCodeId int = (Select CodeId From uas..Code with(nolock) where CodeTypeId = 31 and CodeName='ACS')
       
	declare @userLogTypeId int = (select CodeId from uas..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
	declare @appId int = (select ApplicationID from uas..Application with(nolock) where ApplicationName='UAD')
	declare @userLogId int

	Insert Into UAS..UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
	Values(@appId,@userLogTypeId,@userID,'Subscriber','job_ACS_UpdateSubscriberAddress','',GETDATE());
	set @userLogId = (select @@IDENTITY);

	--Now actually update the address
	Update s
	set Address = i.NewAddress1,
		MailStop = i.NewAddress2,
		Address3 = i.NewAddress3,
		City = i.NewCity,
		State = i.NewStateAbbreviation,
		Zip = i.NewZipCode,
		Plus4 = i.NewPlus4Code,
		IsLatLonValid = 'false',
		Latitude = 0,
		Longitude = 0,
		LatLonMsg = '',
		DateUpdated = GETDATE(),
		UpdatedByUserID = @userID,
		AddressLastUpdatedDate = GETDATE(),
		AddressUpdatedSourceTypeCodeId = @addressUpdateSourceTypeCodeId
	from Subscriptions s
	join @import i on i.SequenceID = s.Sequence
	where s.Address = i.OldAddress1
	and s.City = i.OldCity
	and s.State = i.OldStateAbbreviation 
	and s.Zip = i.OldZipCode
GO
PRINT N'Creating [dbo].[job_ADMS_Remove_By_PubCode]...';


GO
CREATE PROCEDURE [dbo].[job_ADMS_Remove_By_PubCode]
	@PubCode varchar(100)	
AS	 

	create table #tblMissingIgrpno (SubscriptionID int, pubsubscriptionID int, Igrp_no uniqueidentifier)

	Insert into #tblMissingIgrpno       
	SELECT            s.SubscriptionID, ps.PubSubscriptionID, s.IGRP_NO   
	FROM  
				PubSubscriptions ps  WITH (nolock) join 
				Subscriptions s  WITH (nolock) on ps.subscriptionID = s.SubscriptionID join 
				Pubs p  WITH (nolock) on ps.PubID = p.pubID 
	where 
				p.PubCode = @pubcode


	select COUNT(*) from #tblMissingIgrpno


	delete from SubscriberClickActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
	delete from SubscriberOpenActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
	delete from SubscriberTopicActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
	delete from PubSubscriptionDetail where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
	delete from PubSubscriptions where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

	declare @tblDeleteSubscriptionIDs table (SubscriptionID int PRIMARY KEY)

	insert into @tblDeleteSubscriptionIDs
	select distinct s.subscriptionID 
	from
				Subscriptions s  WITH (nolock) left outer join
				PubSubscriptions ps  WITH (nolock) on s.SubscriptionID = ps.SubscriptionID
	where
				ps.SubscriptionID is null

	if exists (select top 1 SubscriptionID from @tblDeleteSubscriptionIDs)  
	Begin                         
		  delete from BrandScore  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from CampaignFilterDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from SubscriberClickActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriberOpenActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriberTopicActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriberVisitActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from SubscriberMasterValues  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from SubscriptionsExtension  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from PubSubscriptionDetail  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from PubSubscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

		  delete from SubscriptionDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
		  delete from Subscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
	End

	-- repopulate concensus
	Delete from SubscriptionDetails
	Where 
				SubscriptionID in (select distinct SubscriptionID from #tblMissingIgrpno) and
				MasterID in (select MasterID from CodeSheet_Mastercodesheet_Bridge)

	insert into SubscriptionDetails (SubscriptionID, MasterID)
	select distinct psd.SubscriptionID, cmb.masterID 
	from 
				#tblMissingIgrpno t join
				PubSubscriptionDetail psd on t.subscriptionID  = psd.SubscriptionID
				join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
				left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
	where  
				sd.sdID is null       

	Delete from SubscriberMasterValues
	Where 
				SubscriptionID in (select SubscriptionID from #tblMissingIgrpno) 

	insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	SELECT 
	  MasterGroupID, [SubscriptionID] , 
	  STUFF((
		  SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
		  FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
		  WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
		  FOR XML PATH (''))
	  ,1,1,'') AS CombinedValues
	FROM 
		  (
				SELECT distinct sd.SubscriptionID, mc.MasterGroupID
				FROM  
							#tblMissingIgrpno t 
							join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.subscriptionID
							join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID                    
		  )
	Results
	GROUP BY [SubscriptionID] , MasterGroupID
	order by SubscriptionID    

	drop table #tblMissingIgrpno
GO
PRINT N'Creating [dbo].[job_CircSync_ClientID_ProcessCode]...';


GO
CREATE PROCEDURE [dbo].[job_CircSync_ClientID_ProcessCode]
	@ClientID int,
      @ProcessCode varchar(50)      
AS
BEGIN 
/*---- CREATE BATCH FOR RECORDS TO UPDATE (Written in case file has mulitple pubcodes which it shouldn't)-----*/
DECLARE @distPubs table ( PubCode varchar(50), PubID int, batchCount int )
DECLARE @pubidBatch table ( BatchID int, PubID int )

INSERT INTO @distPubs (PubCode, PubID, batchCount)
      Select A.PubCode, A.PubID, A.BatchCount from ( 
                  Select SF.PubCode, UP.PubID, Count(*) as BatchCount from SubscriberFinal SF With(NoLock)
                        join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
                        join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
                        join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
                        join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
                        join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
                        join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
                  where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1
                  GROUP BY SF.PubCode, UP.PubID ) as A

DECLARE @userID int = 1
DECLARE @batchID int
INSERT INTO Circulation..Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
      OUTPUT INSERTED.BatchID, INSERTED.PublicationID INTO @pubidBatch
      Select PubID, @userID, batchCount, 'false', getdate(), getdate()
            from @distPubs where batchCount > 0

DECLARE @HistSubIDs Table
( HistorySubscriptionID int );

/* ---- UPDATE HISTORY OF CURRENT START (HistorySubscription, History, UserLog, HistoryToUserLog) ----*/
BEGIN
      INSERT INTO Circulation..HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
            IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
            SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
            OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
            Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
            AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
            SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
            LockDate,LockDateRelease,LockedByUserID,PhoneExt)     
      OUTPUT Inserted.HistorySubscriptionID into @HistSubIDs(HistorySubscriptionID)        
      SELECT 
            CSS.SubscriptionID,CSS.PublisherID,CSS.SubscriberID,CSS.PublicationID,CSS.ActionID_Current,CSS.ActionID_Previous,CSS.SubscriptionStatusID,
            CSS.IsPaid,CSS.QSourceID,CSS.QSourceDate,CSS.DeliverabilityID,CSS.IsSubscribed,CSS.SubscriberSourceCode,CSS.Copies,CSS.OriginalSubscriberSourceCode,CSS.DateCreated,
            CSS.DateUpdated,CSS.CreatedByUserID,CSS.UpdatedByUserID,CSS.AccountNumber,CSS.GraceIssues,CSS.IsNewSubscription,CSS.MemberGroup,
            CSS.OnBehalfOf,CSS.Par3cID,CSS.SequenceID,CSS.SubsrcTypeID,CSS.Verify,CS.ExternalKeyID,CS.FirstName,CS.LastName,CS.Company,CS.Title,CS.Occupation,CS.AddressTypeID,CS.Address1,
            CS.Address2,CS.Address3,CS.City,CS.RegionCode,CS.RegionID,CS.ZipCode,CS.Plus4,CS.CarrierRoute,CS.County,CS.Country,CS.CountryID,CS.Latitude,CS.Longitude,CS.IsAddressValidated,
            CS.AddressValidationDate,CS.AddressValidationSource,CS.AddressValidationMessage,CS.Email,CS.Phone,CS.Fax,CS.Mobile,CS.Website,CS.Birthdate,CS.Age,CS.Income,CS.Gender,
            CS.DateCreated,CS.DateUpdated,CS.CreatedByUserID,CS.UpdatedByUserID,CS.DateCreated,CS.CreatedByUserID,CS.IsLocked,
            CS.LockDate,CS.LockDateRelease,CS.LockedByUserID,CS.PhoneExt
      FROM SubscriberFinal SF With(NoLock)
            join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
            join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
            join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
            join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
            join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
            join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
                  where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1

      DECLARE @HSubID int     
      DECLARE c CURSOR
      FOR
            Select HistorySubscriptionID from @HistSubIDs
      OPEN c
      FETCH NEXT FROM c INTO @HSubID
      WHILE @@FETCH_STATUS = 0
      BEGIN
            DECLARE @historyId int
            INSERT INTO Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID
                                          ,HistoryPaidID,HistoryPaidBillToID,DateCreated,CreatedByUserID)
            Select PB.BatchID, DP.batchCount, HS.PublisherID, HS.PublicationID, HS.SubscriberID, HS.SubscriptionID, @HSubID, 0, 0, GETDATE(), @userID
                  from Circulation..HistorySubscription HS 
                        join @pubidBatch PB on HS.PublicationID = PB.PubID 
                        join @distPubs DP on PB.PubID = DP.PubID
                              where HistorySubscriptionID = @HSubID
            set @historyId = (select @@IDENTITY);

            declare @userLogTypeId int = (Select CodeId from UAS..Code C join UAS..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'User Log' and C.CodeName = 'Edit')
            declare @appId int = (select ApplicationID from UAS..Application with(nolock) where ApplicationName='Circulation')
            declare @userLogId int

            Insert Into UAS..UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
            Values(@appId,@userLogTypeId,@userID,'Subscriber','job_CircSync_ClientID_ProcessCode','',GETDATE());
            set @userLogId = (select @@IDENTITY);
                                          
            Insert Into Circulation..HistoryToUserLog (HistoryID,UserLogID)
            Values(@historyId,@userLogId);

            Insert Into UAS..UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
            Values(@appId,@userLogTypeId,@userID,'Subscription','job_CircSync_ClientID_ProcessCode','',GETDATE());
            set @userLogId = (select @@IDENTITY);
                                          
            Insert Into Circulation..HistoryToUserLog (HistoryID,UserLogID)
            Values(@historyId,@userLogId);
      END
END
/*---- UPDATE HISTORY OF CURRENT END ----*/


/*---- UPDATE SUBSCRIBER AND SUBSCRIPTION START ----*/
DECLARE @CurSubscriberID int, @CurSubscriberFinalID int
DECLARE c CURSOR
FOR   
      Select CS.SubscriberID, SF.SubscriberFinalID 
      from SubscriberFinal SF With(NoLock)
            join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
            join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
            join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
            join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
            join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
            join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
            where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1
OPEN c
FETCH NEXT FROM c INTO @CurSubscriberID, @CurSubscriberFinalID
WHILE @@FETCH_STATUS = 0
BEGIN 
      Update Circulation..Subscriber
      set 
            FirstName = US.FNAME, LastName = US.LNAME, Company = US.Company, Title = US.Title, Address1 = US.[Address], Address2 = US.MailStop, Address3 = US.Address3,
            City = US.City, RegionCode = US.[State], RegionID = (Select RegionID from UAS..Region where RegionCode = US.[State]), ZipCode = US.Zip, Plus4 = US.Plus4, County = US.COUNTY, Country = US.COUNTRY, 
            CountryID = US.CountryID, Latitude = US.Latitude, Longitude = US.Longitude, IsAddressValidated = US.IsLatLonValid, 
            AddressValidationDate = US.AddressLastUpdatedDate, AddressValidationMessage = US.LatLonMsg, Email = US.Email, Phone = US.Phone, Fax = US.Fax, 
            Mobile = US.Mobile, DateUpdated = GETDATE(), UpdatedByUserID = 1
            --ExternalKeyID, Occupation, AddressTypeID, CarrierRoute, AddressValidationSource, Website, Birthdate, Age, Income, Gender, PhoneExt
            from SubscriberFinal SF With(NoLock)
                  join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
                  join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
                  join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
                  join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
                  join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
                  join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
                  where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1 and CS.SubscriberID = @CurSubscriberID

      Update Circulation..Subscription
      set 
            ActionID_Current = (Select a.ActionID from Circulation..Action a where a.CategoryCodeID = US.CategoryID and a.TransactionCodeID = US.TransactionID and a.ActionTypeID = (Select CodeId from UAS..Code C With(NoLock) join UAS..CodeType CT With(NoLock) on C.CodeTypeId = CT.CodeTypeId where CodeName = 'System Generated' and CodeTypeName = 'Action')), 
            ActionID_Previous = CSS.ActionID_Current, QSourceID = US.QSourceID, QSourceDate = US.QDate, DeliverabilityID = US.Demo7,
            OriginalSubscriberSourceCode = US.Origssrc, Par3cID = US.Par3c
            --SubscriptionStatusID, IsPaid, IsSubscribed, SubscriberSourceCode, Copies, AccountNumber, GraceIssues, IsNewSubscription, MemberGroup, OnBehalfOf, 
            --SequenceID, SubsrcTypeID, Verify
            from SubscriberFinal SF With(NoLock)
                  join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
                  join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
                  join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
                  join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
                  join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
                  join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
                  where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1 and CS.SubscriberID = @CurSubscriberID
END
/*---- UPDATE SUBSCRIBER AND SUBSCRIPTION END ----*/


/*---- INSERT SUBSCRIBER AND SUBSCRIPTION START ----*/
DECLARE @InsertSubs table ( SubscriberID int, SFRecordIdentifier uniqueidentifier )
INSERT INTO Circulation..Subscriber (FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City
                              ,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated
                              ,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate
                              ,Age,Income,Gender,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,tmpSubscriptionID,IsLocked
                              ,LockedByUserID,LockDate,LockDateRelease,PhoneExt,IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate
                              ,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier)
OUTPUT Inserted.SubscriberID, Inserted.SFRecordIdentifier INTO @InsertSubs
Select S.FName,S.LName,S.Company,S.Title,NULL,NULL,S.[Address],S.MailStop,S.Address3,S.City,S.[State]
            ,(Select RegionID from UAS..Region where RegionCode = S.[State]),S.Zip,S.Plus4,NULL,S.County,S.Country,S.CountryID,S.Latitude,S.Longitude
            ,S.IsLatLonValid,NULL,NULL,S.LatLonMsg,S.Email,S.Phone,S.Fax,S.Mobile,NULL
            ,NULL,NULL,NULL,S.Gender,S.DateCreated,S.DateUpdated,S.CreatedByUserID,S.UpdatedByUserID,NULL,'false'
            ,NULL,NULL,NULL,NULL,'false',0,NULL
            ,0,0,S.IGrp_No,SF.SFRecordIdentifier
      from SubscriberFinal SF With(NoLock) 
            join Subscriptions S With(NoLock) on SF.igrp_no = S.igrp_no 
            join Pubs P on P.pubcode = SF.pubcode           
                  where (ProcessCode = @ProcessCode and isupdatedinlive = 1)        
and (not S.SEQUENCE > 0 OR NOT EXISTS (
      Select * from Circulation..Subscriber CS
            join Circulation..Subscription CSS on CS.SubscriberID = CSS.SubscriberID
            join Circulation..Publication CP on CP.PublicationID = CSS.PublicationID
            join Circulation..Publisher CPP on CPP.PublisherID = CP.PublisherID     
            WHERE CSS.SequenceID = S.SEQUENCE AND CP.PublicationCode = P.PubCode ))

DECLARE @InsertSubscription table ( SubscriptionID int, SubscriberID int )          
INSERT INTO Circulation..Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID
                                                ,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,DateCreated,DateUpdated
                                                ,CreatedByUserID,UpdatedByUserID,Par3cID,SubsrcTypeID,AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify)
OUTPUT Inserted.SubscriptionID, Inserted.SubscriberID INTO @InsertSubscription
Select (Select MAX(SequenceID) + 1 FROM Circulation..Subscription WHERE PublicationID = P.PubID),P.ClientID,INS.SubscriberID,P.PubID
                                                ,(Select a.ActionID from Circulation..Action a where a.CategoryCodeID = S.CategoryID and a.TransactionCodeID = S.TransactionID and a.ActionTypeID = (Select CodeId from UAS..Code C With(NoLock) join UAS..CodeType CT With(NoLock) on C.CodeTypeId = CT.CodeTypeId where CodeName = 'System Generated' and CodeTypeName = 'Action'))
                                                ,0,NULL,0,S.QSourceID,S.QDate,S.Demo7,'false','',1,'',GETDATE(),NULL,1,NULL,S.Par3c,NULL,'',0,'','','' 
      from SubscriberFinal SF With(NoLock) 
      join Subscriptions S With(NoLock) on SF.igrp_no = S.igrp_no 
      join Pubs P With(NoLock) on P.pubcode = SF.pubcode
      join @InsertSubs INS on SF.SFRecordIdentifier = INS.SFRecordIdentifier

/*---- History Inserts ----**/
DECLARE @HistSubIDs2 table ( HistorySubscriptionID int )
INSERT INTO Circulation..HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
            IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
            SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
            OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
            Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
            AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
            SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
            LockDate,LockDateRelease,LockedByUserID,PhoneExt)     
      OUTPUT Inserted.HistorySubscriptionID into @HistSubIDs2(HistorySubscriptionID)        
      SELECT 
            CSS.SubscriptionID,CSS.PublisherID,CSS.SubscriberID,CSS.PublicationID,CSS.ActionID_Current,CSS.ActionID_Previous,CSS.SubscriptionStatusID,
            CSS.IsPaid,CSS.QSourceID,CSS.QSourceDate,CSS.DeliverabilityID,CSS.IsSubscribed,CSS.SubscriberSourceCode,CSS.Copies,CSS.OriginalSubscriberSourceCode,CSS.DateCreated,
            CSS.DateUpdated,CSS.CreatedByUserID,CSS.UpdatedByUserID,CSS.AccountNumber,CSS.GraceIssues,CSS.IsNewSubscription,CSS.MemberGroup,
            CSS.OnBehalfOf,CSS.Par3cID,CSS.SequenceID,CSS.SubsrcTypeID,CSS.Verify,CS.ExternalKeyID,CS.FirstName,CS.LastName,CS.Company,CS.Title,CS.Occupation,CS.AddressTypeID,CS.Address1,
            CS.Address2,CS.Address3,CS.City,CS.RegionCode,CS.RegionID,CS.ZipCode,CS.Plus4,CS.CarrierRoute,CS.County,CS.Country,CS.CountryID,CS.Latitude,CS.Longitude,CS.IsAddressValidated,
            CS.AddressValidationDate,CS.AddressValidationSource,CS.AddressValidationMessage,CS.Email,CS.Phone,CS.Fax,CS.Mobile,CS.Website,CS.Birthdate,CS.Age,CS.Income,CS.Gender,
            CS.DateCreated,CS.DateUpdated,CS.CreatedByUserID,CS.UpdatedByUserID,CS.DateCreated,CS.CreatedByUserID,CS.IsLocked,
            CS.LockDate,CS.LockDateRelease,CS.LockedByUserID,CS.PhoneExt
      FROM Circulation..Subscription CSS With(NoLock)
            join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
            join @InsertSubscription INS on INS.SubscriberID = CS.SubscriberID            
            
DECLARE @distPubs2 table ( PubCode varchar(50), PubID int, batchCount int )
DECLARE @pubidBatch2 table ( BatchID int, PubID int )

INSERT INTO @distPubs2 (PubCode, PubID, batchCount)
      Select A.PubCode, A.PublicationID, A.BatchCount from ( 
                  Select SF.PubCode, CPP.PublicationID, Count(*) as BatchCount 
                        from SubscriberFinal SF With(NoLock)                        
                        join Circulation..Subscription CSS With(NoLock) on SF.Sequence = CSS.SequenceID
                        join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
                        join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and SF.PubCode = CPP.PublicationCode
                        where CS.SubscriberID in (Select SubscriberID from @InsertSubs)
                  GROUP BY SF.PubCode, CPP.PublicationID ) as A

INSERT INTO Circulation..Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
OUTPUT INSERTED.BatchID, INSERTED.PublicationID INTO @pubidBatch2
Select PubID, @userID, batchCount, 'false', getdate(), getdate()
      from @distPubs2 where batchCount > 0

DECLARE @HistIDs table ( HistoryID int, SubscriberID int )
INSERT INTO Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID
                                    ,HistoryPaidID,HistoryPaidBillToID,DateCreated,CreatedByUserID)
      OUTPUT Inserted.HistoryID, Inserted.SubscriberID INTO @HistIDs
      Select PB.BatchID, DP.batchCount, HS.PublisherID, HS.PublicationID, HS.SubscriberID, HS.SubscriptionID, @HSubID, 0, 0, GETDATE(), @userID
            from Circulation..HistorySubscription HS 
                  join @pubidBatch2 PB on HS.PublicationID = PB.PubID 
                  join @distPubs2 DP on PB.PubID = DP.PubID
                        where HistorySubscriptionID in (Select HistorySubscriptionID from @HistSubIDs2)

DECLARE @InsertMarket table ( MarketingID int, SubscriberID int, PublicationID int, IsActive bit, DateCreated datetime, CreatedByUserID int )
INSERT INTO Circulation..MarketingMap (SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID)
OUTPUT Inserted.MarketingID, Inserted.SubscriberID, Inserted.PublicationID, Inserted.IsActive, Inserted.DateCreated, Inserted.CreatedByUserID INTO HistoryMarketingMap
Select I.SubscriberID, P.PubID, 1, GetDate(), 1
      from @InsertSubs I
            join SubscriberFinal SF on SF.SFRecordIdentifier = I.SFRecordIdentifier
            join Pubs P on SF.PubCode = P.PubCode
            where SF.ProcessCode = @ProcessCode

DECLARE @InsertHistMarket table ( HistoryMarketingMapID int, SubscriberID int )
INSERT INTO Circulation..HistoryMarketingMap (MarketingID, SubscriberID, PublicationID, IsActive, DateCreated, CreatedByUserID)
      OUTPUT Inserted.HistoryMarketingMapID, Inserted.SubscriberID INTO @InsertHistMarket
      Select MarketingID, SubscriberID, PublicationID, IsActive, DateCreated, CreatedByUserID from @InsertMarket

INSERT INTO Circulation..HistoryToHistoryMarketingMap (HistoryID, HistoryMarketingMapID)
      Select HID.HistoryID, IHM.HistoryMarketingMapID from @InsertHistMarket IHM
            join @HistIDs HID on IHM.SubscriberID = HID.SubscriberID
/*---- INSERT SUBSCRIBER AND SUBSCRIPTION END ----*/
END
GO
PRINT N'Creating [dbo].[job_IssueComp_IssueCompDetail_Add]...';


GO
CREATE PROCEDURE [dbo].[job_IssueComp_IssueCompDetail_Add]
	@ProcessCode varchar(50),
	@PublicationID int,
	@SourceFileId int
AS
BEGIN	
	DECLARE @distinctComps TABLE (CompName VARCHAR(200), CompCount int, SFRecordIdentifier UNIQUEIDENTIFIER, IssueID int)
	DECLARE @PublisherID INT = (SELECT PublisherID FROM Circulation..Publication WHERE PublicationID = @PublicationID)
	DECLARE @PubCode varchar(100) = (Select PubCode FROM Pubs where PubID = @PublicationID)

	--
	-- Temporary patch for records with ascii characters in email field, need to remove when fix is implemented
	--
	UPDATE SubscriberFinal
	SET Email =  ''  
	WHERE LEN(LTRIM(rtrim(email))) > 0 and LEN(LTRIM(RTRIM(email))) <= 4 and SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode 

	PRINT ('UPDATE SubscriberFinal / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	--
	-- Begin CREATE TABLE
	--
	CREATE TABLE #MatchCirc(
		SFRecordIdentifier UNIQUEIDENTIFIER,indNameAddress UNIQUEIDENTIFIER,indNameEmail UNIQUEIDENTIFIER,indNameCompany UNIQUEIDENTIFIER,indNamePhone UNIQUEIDENTIFIER
		,indNameNotBlankEmail UNIQUEIDENTIFIER,indDistEmail UNIQUEIDENTIFIER,grpNameAddress UNIQUEIDENTIFIER,grpNameEmail UNIQUEIDENTIFIER,grpNameCompany UNIQUEIDENTIFIER
		,grpNamePhone UNIQUEIDENTIFIER,grpDistEmail UNIQUEIDENTIFIER,IsOriginal bit,Igrp_no UNIQUEIDENTIFIER,FName varchar(100),LName varchar(100),Company varchar(100),Address varchar(255)
		,State varchar(50),zip varchar(50),Phone varchar(100),Email varchar(100),FName3 varchar(3),LName6 varchar(6),Address15 varchar(15),Company8 varchar(8),Title8 varchar(8))

	CREATE CLUSTERED INDEX idx_MatchGroups_SFRecordIdentifier ON #MatchCirc(SFRecordIdentifier)

	CREATE INDEX idx_MatchGroups ON #MatchCirc(SFRecordIdentifier,indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,
												grpNamePhone,grpDistEmail,Igrp_no)


	INSERT INTO #MatchCirc(SFRecordIdentifier,FName,LName,Company,Address	,State ,zip ,Phone ,Email ,FName3 ,LName6 ,Address15,Company8,Title8,IsOriginal)
	SELECT distinct SFRecordIdentifier, isnull(FName,''), isnull(LName,''), isnull(Company,''), isnull(Address,'')	, isnull(State,'') , left(isnull(zip,''),5) , isnull(Phone,'') , isnull(Email,'') , 
					left(isnull(FName,''),3) , left(isnull(LName,''),6) , left(isnull(Address,''),15), left(isnull(Company,''),8 ), LEFT(isnull(Title,''),8),1
	FROM SubscriberFinal WITH(NOLOCK) 
	WHERE SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode 
	
	PRINT ('INSERT INTO #MatchCirc / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	--
	-- Split Names with special characters
	--	
		
	CREATE TABLE #NameSplit (SFRecordIdentifier uniqueidentifier, SplitChar varchar(256))
		
	INSERT INTO #NameSplit
	SELECT SFRecordIdentifier,f.items AS fnameSplit
	FROM SubscriberFinal
	CROSS APPLY MASTER.dbo.fn_GetSpecialChar(fname) AS f
	UNION
	SELECT SFRecordIdentifier,l.items AS lnameSplit
	FROM SubscriberFinal		
	CROSS APPLY MASTER.dbo.fn_GetSpecialChar(lname) AS l
		
	INSERT INTO #MatchCirc(SFRecordIdentifier,FName,LName,Company,Address,zip,Phone,Email,FName3,LName6,Address15,Company8,Title8,IsOriginal)
	SELECT DISTINCT sf.SFRecordIdentifier,MASTER.dbo.fn_StripNonAlphaNumerics(a.items),MASTER.dbo.fn_StripNonAlphaNumerics(b.items),Company,Address,Zip,Phone,Email,MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(a.items,3)),
					MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(b.items,6)),LEFT(sf.Address,15),LEFT(sf.Company,8),LEFT(sf.Title,8),0
	FROM SubscriberFinal sf INNER JOIN #NameSplit ns ON sf.SFRecordIdentifier = ns.SFRecordIdentifier
	CROSS APPLY MASTER.dbo.fn_split(sf.FName,ns.SplitChar) AS a
	CROSS APPLY MASTER.dbo.fn_split(sf.LName,ns.SplitChar) AS b
	WHERE ISNULL(master.dbo.fn_StripNonAlphaNumerics(a.items),'')!='' AND ISNULL(MASTER.dbo.fn_StripNonAlphaNumerics(b.items),'')!=''
	ORDER BY sf.SFRecordIdentifier
		
	PRINT ('INSERT NAME SPLIT INTO #MatchCirc / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	-- Insert into #MatchCirc with characters stripped out
	INSERT INTO #MatchCirc(SFRecordIdentifier,FName,LName,Company,Address,zip,Phone,Email,FName3,LName6,Address15,Company8,Title8,IsOriginal)
	SELECT DISTINCT sf.SFRecordIdentifier,MASTER.dbo.fn_StripNonAlphaNumerics(sf.fname),MASTER.dbo.fn_StripNonAlphaNumerics(sf.lname),Company,Address,Zip,Phone,Email,MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(sf.fname,3)),
			MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(sf.Lname,6)),LEFT(sf.Address,15),LEFT(sf.Company,8),LEFT(sf.Title,8),0
	FROM SubscriberFinal sf INNER JOIN #NameSplit ns ON sf.SFRecordIdentifier = ns.SFRecordIdentifier
	WHERE ISNULL(sf.Fname,'')!='' AND ISNULL(sf.LName,'')!=''	
		
	PRINT ('INSERT SPECIAL CHAR REMOVED INTO #MatchCirc / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #NameSplit
		
	--
	-------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Begin Data Match
		
	--
	-- Fname,Lname and Address match on individual level
	--
	UPDATE mg
	SET indNameAddress = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND Address15 = LEFT(s.Address1,15) AND mg.Zip = left(s.ZipCode,5)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.address15 != '' AND mg.zip != '' and ss.PublicationID = @PublicationID
		
	--Swap
	UPDATE mg
	SET indNameAddress = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON mg.FName3 = LEFT(s.LastName,3) AND mg.LName6 = LEFT(s.FirstName,6) AND mg.Address15 = LEFT(s.Address1,15) AND mg.Zip = left(s.ZipCode,5)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.address15 != '' AND mg.zip != '' AND mg.indNameAddress is null and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 1 : Fname,Lname and Address match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
	
  	--
	-- Fname,Lname and Email match on individual level
	--
	UPDATE mg
	SET indNameEmail = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON mg.Email = s.EMAIL AND FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Email != '' and ss.PublicationID = @PublicationID 

	-- Swap
	UPDATE mg
	SET indNameEmail = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON mg.Email = s.EMAIL AND FName3 = LEFT(s.Lastname,3) AND LName6 = LEFT(s.FirstName,6)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Email != '' and mg.indNameEmail is null and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 2 : Fname,Lname and Email match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
					  
  	--
	-- Fname,Lname and Company match on individual level
	--
	UPDATE mg
	SET indNameCompany = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND  Company8 = LEFT(s.Company,8)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND Company8 != '' and ss.PublicationID = @PublicationID
		
	-- Swap
	UPDATE mg
	SET indNameCompany = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.Lastname,3) AND LName6 = LEFT(s.FirstName,6) AND  Company8 = LEFT(s.Company,8)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND Company8 != '' and mg.indNameCompany is null and ss.PublicationID = @PublicationID

	PRINT ('After Step 3 : Fname,Lname and Company match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  	--
	-- Fname,Lname and Phone match on individual level
	--
	UPDATE mg
	SET indNamePhone = s.igrp_no
	FROM #MatchCirc mg 
	INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND mg.PHone = s.Phone
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Phone != '' and ss.PublicationID = @PublicationID
		
	-- Swap
	UPDATE mg
	SET indNamePhone = s.igrp_no
	FROM #MatchCirc mg 
	INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND mg.PHone = s.Phone
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Phone != '' AND mg.indNamePhone is null and ss.PublicationID = @PublicationID

	PRINT ('After Step 4 : Fname,Lname and Phone match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
			  
	--
	-- Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank
	--
	UPDATE mg
	SET indNameNotBlankEmail = s.Igrp_no
	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Email = s.email		
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID					 
	WHERE mg.Email != '' AND (mg.FName != '' OR mg.lname != '')
	AND ISNULL(s.Email,'') != '' AND ISNULL(s.firstname,'') = '' AND ISNULL(s.lastname,'') = '' and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 5 : Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	--
	-- Email not blank and Fname OR Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above
	--
	UPDATE mg
	SET indNameNotBlankEmail = s.Igrp_no
	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Email = s.email INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE  mg.Email != '' AND mg.FName = '' AND mg.lname = ''
	AND ISNULL(s.Email,'') != '' AND (ISNULL(s.firstname,'') != '' OR ISNULL(s.lastname,'') != '') and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 6 : Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	--
	-- Distinct Email Source file does not have Fname Or LName match on Subscriptions
	--
	UPDATE mg
	SET indDistEmail = s.Igrp_no
	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Email = s.Email INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID			 
	WHERE mg.Email != '' AND mg.fname = '' AND mg.lname = '' and ss.PublicationID = @PublicationID

 	PRINT ('After Step 7a : Distinct Email Source file does not have Fname Or LName match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
 		
 	-- We are using indDistEmail for this update because the match criteria assumes that this field should not have any values
 	-- FName, LName and Email is Blank. Title and Company not Blank match on Subscriptions
 	UPDATE mg
 	SET indDistEmail = s.Igrp_no
 	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Title8 = LEFT(s.Title,8) AND mg.Company8 = LEFT(s.Company,8)
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
 	WHERE ISNULL(mg.FName,'') = '' AND ISNULL(mg.LName,'') = '' AND ISNULL(mg.EMAIL,'') = '' AND ISNULL(mg.TITLE8,'') != '' AND ISNULL(mg.Company8,'') != '' and ss.PublicationID = @PublicationID
 		
 	PRINT ('After Step 7b : FName, LName and Email is Blank. Title and Company not Blank match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

    --END INDIVIDUAL MATCHING
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    --START GROUP MATCHING
	--
	-- Name Addres Group Match
	--
	CREATE TABLE #tmpNameAddressGroup (fname VARCHAR(3), lname  VARCHAR(6), address  VARCHAR(15), zip  VARCHAR(5), igrp_no UNIQUEIDENTIFIER)
		
	INSERT INTO #tmpNameAddressGroup
	SELECT Fname3 AS fname,lname6 AS lname,address15 AS 'address',LEFT(ZIP,5) AS ZIP,NEWID() AS igrp_no
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(address15,'') != '' AND ISNULL(zip,'') != '' 
	GROUP BY fname3,lname6,address15,LEFT(ZIP,5)
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpNameAddress = ng.igrp_no
	FROM #MatchCirc mg INNER JOIN #tmpNameAddressGroup ng ON FName3 = ng.fname AND LName6 = ng.lname AND Address15 = ng.[address] and LEFT(mg.Zip,5) = ng.ZIP
	WHERE (FName3 != '' OR LName6 != '') AND Address15 != '' AND ISNULL(mg.zip,'') != ''
	
	PRINT ('After Step 8 : Name Addres Group Match / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNameAddressGroup
	--
	-- Name Email Group Match
	--
	CREATE TABLE #tmpNameEmailGroup (fname VARCHAR(3), lname  VARCHAR(6), Email  VARCHAR(100), igrp_no UNIQUEIDENTIFIER)
		
	INSERT INTO #tmpNameEmailGroup
	SELECT fname3 AS fname,lname6 AS lname,Email,NEWID() AS Igrp_no
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(Email,'') != '' 
	GROUP BY fname3,lname6,Email
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpNameEmail = ng.Igrp_no
	FROM #MatchCirc mg INNER JOIN #tmpNameEmailGroup AS ng ON FName3 = ng.fname AND LName6 = ng.lname AND mg.Email = ng.Email
	WHERE (mg.FName3 != '' OR mg.lname6 != '') AND mg.Email != ''
			
	PRINT ('After Step 9 : Name Email Group Match / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNameEmailGroup
		
	--	
	-- Name Company Group Match	
	--
	CREATE TABLE #tmpNameCompanyGroup (fname VARCHAR(3), lname  VARCHAR(6), Company  VARCHAR(8), igrp_no UNIQUEIDENTIFIER)

	INSERT INTO #tmpNameCompanyGroup		
	SELECT fname3 AS fname,lname6 AS lname,Company8 AS Company,NEWID() AS Igrp_No
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(sf.Company8,'') != ''  
	GROUP BY fname3,lname6,Company8
	HAVING COUNT(*) > 1
										
	UPDATE mg
	SET grpNameCompany = ng.Igrp_No
	FROM #MatchCirc mg INNER JOIN #tmpNameCompanyGroup AS ng ON FName3 = ng.fname AND LName6 = ng.lname AND LEFT(mg.Company,8) = ng.Company
	WHERE (mg.FName3 != '' OR mg.lname6 != '') AND mg.Company8 != ''
				
	PRINT ('After Step 10 : Name Company Group Match	 / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNameCompanyGroup

	--
	-- Name Phone Group Match
	--
		
	CREATE TABLE #tmpNamePhoneGroup (fname VARCHAR(3), lname  VARCHAR(6), Phone  VARCHAR(100), igrp_no UNIQUEIDENTIFIER)
		
	INSERT INTO #tmpNamePhoneGroup		
	SELECT fname3 AS fname ,lname6 AS lname,Phone,NEWID() AS Igrp_No
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(Phone,'') != '' 			  
	GROUP BY fname3,lname6,Phone
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpNamePhone = ng.Igrp_no
	FROM #MatchCirc mg INNER JOIN #tmpNamePhoneGroup AS ng
									ON FName3 = ng.fname AND LName6 = ng.lname AND mg.Phone = ng.Phone
	WHERE (mg.FName3 != '' OR mg.lname6 != '') AND mg.Phone != ''
		
	PRINT ('After Step 11 : Name Phone Group Match / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNamePhoneGroup

	--
	-- No Name Distinct Email Match on its own sourcefile
	--
	CREATE TABLE #NoNameEmail(
		SFRecordIdentifier UNIQUEIDENTIFIER
		,Email VARCHAR(255)
		,EmailIgrp_No UNIQUEIDENTIFIER)

	CREATE INDEX idx_NoNameEMail ON #NoNameEmail(SFRecordIdentifier,Email)

	INSERT INTO #NoNameEmail
	SELECT sf1.SFRecordIdentifier,sf1.Email,null
	FROM #MatchCirc sf1 WITH(NoLock) INNER JOIN #MatchCirc sf2 WITH(NoLock) on sf1.Email = sf2.Email
	WHERE ISNULL(sf1.FName,'') = '' AND ISNULL(sf1.lname,'') = '' AND ISNULL(sf1.Email,'') != ''
		AND (ISNULL(sf2.FName,'') = '' OR ISNULL(sf2.lname,'') = '' OR ISNULL(sf2.FName,'') != '' OR ISNULL(sf2.lname,'') != '') AND ISNULL(sf2.Email,'') != ''
	--GROUP BY sf1.SFRecordIdentifier,sf1.Email
	UNION
	SELECT sf2.SFRecordIdentifier, sf2.Email,null
	FROM #MatchCirc sf1 WITH(NoLock) INNER JOIN #MatchCirc sf2 WITH(NoLock) on sf1.Email = sf2.Email
	WHERE ISNULL(sf1.FName,'') = '' AND ISNULL(sf1.lname,'') = '' AND ISNULL(sf1.Email,'') != ''
		AND (ISNULL(sf2.FName,'') = '' OR ISNULL(sf2.lname,'') = '' OR ISNULL(sf2.FName,'') != '' OR ISNULL(sf2.lname,'') != '') AND ISNULL(sf2.Email,'') != ''

		
	-- Create new temp table and group by email so we can assign a newid()
	CREATE TABLE #EmailGroups (EMAIL VARCHAR(100), groupCount int, EmailIGrp_No UNIQUEIDENTIFIER)
		
	INSERT INTO #EmailGroups
	SELECT EMAIL,COUNT(*) as groupCount,NEWID() as EmailIGrp_No FROM #NoNameEmail GROUP BY Email

	-- Join #EmailGroups back to #NoNameEmail on Email equals Email and update EmailIgrp_No
	UPDATE ne
	SET EmailIGrp_No = e.EmailIGrp_No
	FROM #NoNameEmail ne INNER JOIN #EmailGroups e on ne.Email = e.Email
	WHERE ne.EmailIgrp_No IS NULL and e.groupCount > 1
		
	-- Update #MatchCirc with EmailIgrp_No id
	UPDATE mg
	SET grpDistEmail = ne.EmailIgrp_No
	FROM #MatchCirc mg INNER JOIN #NoNameEmail ne on mg.SFRecordIdentifier = ne.SFRecordIdentifier
	WHERE mg.grpDistEmail IS NULL AND ne.EmailIgrp_No IS NOT NULL
		
	PRINT ('After Step 12a : No Name Distinct Email Match on its own sourcefile / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #NoNameEmail
	DROP TABLE #EmailGroups
		
	-- Match on Title and Company where firstname, lastname and EMAIL is blank
		
	CREATE TABLE #TitleCompany(
			Title VARCHAR(8)
		,Company VARCHAR(8)
		,TitleCompanyIgrp_No UNIQUEIDENTIFIER)
		
	CREATE INDEX #TitleCompany ON #TitleCompany(Title,Company)
		
	INSERT INTO #TitleCompany
	SELECT sf.Title8,sf.Company8,NEWID() 
	FROM #MatchCirc sf WITH(NOLOCK)
	WHERE ISNULL(FName,'') = '' AND ISNULL(LName,'') = '' AND ISNULL(EMAIL,'') = '' AND ISNULL(TITLE8,'') != '' AND ISNULL(Company8,'') != ''
	GROUP BY sf.Title8,sf.Company8
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpDistEmail = tc.TitleCompanyIgrp_No
	FROM #MatchCirc mg INNER JOIN #TitleCompany tc ON mg.Title8 = tc.Title AND mg.Company8 = tc.Company
	WHERE ISNULL(mg.FName,'') = '' AND ISNULL(mg.LName,'') = '' AND ISNULL(mg.EMAIL,'') = '' AND ISNULL(mg.TITLE8,'') != '' AND ISNULL(mg.Company8,'') != ''

	PRINT ('After Step 12b : Match on Title and Company where firstname, lastname and EMAIL is blank / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	DROP TABLE #TitleCompany
		
    --END GROUP MATCH
    ----------------------------------------------------------------------------------------------------------------------------
    --BEGIN ASSIGN IGRP_NO

	--
	-- Create temp table and store grp[ColumnName] fields going down instead of going accross
	--
	CREATE TABLE #Groups(
		SFRecordIdentifier UNIQUEIDENTIFIER,
		GroupMatch UNIQUEIDENTIFIER,
		ColumnMatch VARCHAR(255))

	CREATE INDEX idx_groups ON #groups(SFRecordIdentifier,GroupMatch,ColumnMatch)
	CREATE INDEX idx_groups_GroupMatch ON #groups(GroupMatch)
	CREATE INDEX idx_groups_SFRecordIdentifier ON #groups(SFRecordIdentifier)

	INSERT INTO #Groups

	SELECT SFRecordIdentifier,grpNameAddress AS Groupmatch,'grpNameAddress' AS ColumnMatch FROM #MatchCirc WHERE grpNameAddress IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpNameEmail,'grpNameEmail'AS ColumnMatch FROM #MatchCirc WHERE grpNameEmail IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpNameCompany,'grpNameCompany'AS ColumnMatch FROM #MatchCirc WHERE grpNameCompany IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpNamePhone,'grpNamePhone'AS ColumnMatch FROM #MatchCirc WHERE grpNamePhone IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpDistEmail,'grpDistEmail'AS ColumnMatch FROM #MatchCirc WHERE grpDistEmail IS NOT NULL
	ORDER BY SFRecordIdentifier,groupmatch
		
		
	-- Find combinations of SFRecordIdentifier and grp[ColumnName] and assign igrp_no to SFRecordIdentifiers
	--
	DECLARE @SFRecordIdentifier UNIQUEIDENTIFIER
	DECLARE @Sqlstmt VARCHAR(MAX)
	DECLARE @uniqueID UNIQUEIDENTIFIER

	CREATE TABLE #cur_inner_Q1 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)
		
	CREATE INDEX idx_cur_inner_Q1_SFRecordIdentifier ON #cur_inner_Q1(SFRecordIdentifier)
		
	CREATE TABLE #cur_inner_Q2 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)

	CREATE INDEX idx_cur_inner_Q2_GroupMatch ON #cur_inner_Q2(GroupMatch)

	CREATE TABLE #cur_inner_Q3 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)

	CREATE INDEX idx_cur_inner_Q3_SFRecordIdentifier ON #cur_inner_Q3(SFRecordIdentifier)
		
	CREATE TABLE #cur_inner_Q4 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)

	CREATE INDEX idx_cur_inner_Q4_GroupMatch ON #cur_inner_Q4(GroupMatch)

	PRINT ('CURSOR Start / '  +  CONVERT(VARCHAR(20), GETDATE(), 114))

	declare @curRowcount int
		
	select @curRowcount  = count(SFRecordIdentifier)
	FROM #MatchCirc 
	WHERE igrp_no IS NULL AND (grpNameAddress IS NOT NULL OR grpNameEmail IS NOT NULL OR grpNameCompany IS NOT NULL OR grpNamePhone IS NOT NULL OR grpDistEmail IS NOT NULL )

	PRINT ('Cursor ROWCOUNT : '  + Convert(VARCHAR,@curRowcount) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))


	DECLARE c CURSOR
	FOR 
		SELECT SFRecordIdentifier 
		FROM #MatchCirc 
		WHERE igrp_no IS NULL AND (grpNameAddress IS NOT NULL OR grpNameEmail IS NOT NULL OR grpNameCompany IS NOT NULL OR grpNamePhone IS NOT NULL OR grpDistEmail IS NOT NULL )
	OPEN c
	FETCH NEXT FROM c INTO @SFRecordIdentifier

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @uniqueID = NEWID()
			
		Truncate table #cur_inner_Q1
		Truncate table #cur_inner_Q2
		Truncate table #cur_inner_Q3
		Truncate table #cur_inner_Q4
			
		INSERT INTO #cur_inner_Q4
		SELECT SFRecordIdentifier,GroupMatch 
		FROM	#Groups 
		WHERE SFRecordIdentifier = CAST(@SFRecordIdentifier AS VARCHAR(50))

		INSERT INTO 
			#cur_inner_Q3
		SELECT 
			g.SFRecordIdentifier,
			g.GroupMatch
		FROM 
			#Groups g 
			INNER JOIN #cur_inner_Q4 AS g2 ON g.GroupMatch = g2.GroupMatch
		GROUP BY 
			g.SFRecordIdentifier,
			g.GroupMatch

		INSERT INTO 
			#cur_inner_Q2
		SELECT 
			g1.SFRecordIdentifier,
			g1.GroupMatch 
		FROM
			#Groups g1 
			INNER JOIN #cur_inner_Q3 AS dg1 on g1.SFRecordIdentifier = dg1.SFRecordIdentifier
		GROUP BY 
			g1.SFRecordIdentifier,
			g1.GroupMatch

		INSERT INTO 
			#cur_inner_Q1
		SELECT 
			ge.SFRecordIdentifier,
			ge.groupMatch 
		FROM
			#Groups ge 
			INNER JOIN #cur_inner_Q2 dg on ge.GroupMatch = dg.GroupMatch
		GROUP BY 
			ge.SFRecordIdentifier,
			ge.GroupMatch

		UPDATE 
			mg
		SET 
			igrp_no =  CAST(@uniqueID AS VARCHAR(50)) 		
		FROM 
			#MatchCirc mg 
			INNER JOIN #cur_inner_Q1 AS dg3	ON mg.SFRecordIdentifier = DG3.SFRecordIdentifier
		WHERE 
			mg.Igrp_no is null 
			
		FETCH NEXT FROM c INTO @SFRecordIdentifier
	END
	CLOSE c
	DEALLOCATE c
		
	DROP TABLE #cur_inner_Q1
	DROP TABLE #cur_inner_Q2
	DROP TABLE #cur_inner_Q3
	DROP TABLE #cur_inner_Q4
		
	PRINT ('CURSOR END / '   + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	PRINT ('Begin Assign IGRP_NO to matched Subscriptions and matched within its own sourcefile/ '  + CONVERT(VARCHAR(20), GETDATE(), 114))	
	--
	-- Assign Igrp_no to records that matched Subscriptions and matched within it's own sourcefile.  These record will get their igrp_no from the record it matched in Subscriptions
	--
	CREATE TABLE #indGrpIgrpNo_Assign(
			indNameAddress UNIQUEIDENTIFIER
			,indNameEmail UNIQUEIDENTIFIER
			,indNameCompany UNIQUEIDENTIFIER
			,indNamePhone UNIQUEIDENTIFIER
			,indNameNotBlankEmail UNIQUEIDENTIFIER
			,indDistEmail UNIQUEIDENTIFIER
			,grpNameAddress UNIQUEIDENTIFIER
			,grpNameEmail UNIQUEIDENTIFIER
			,grpNameCompany UNIQUEIDENTIFIER
			,grpNamePhone UNIQUEIDENTIFIER
			,grpDistEmail UNIQUEIDENTIFIER
			,Igrp_No UNIQUEIDENTIFIER)
	          
	CREATE INDEX idx_indGrpIgrpNo_Assign ON #indGrpIgrpNo_Assign(indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,grpNamePhone,grpDistEmail,Igrp_no)
	    
	CREATE INDEX idx_Igrp_no ON #indGrpIgrpNo_Assign(Igrp_No)

	--
	-- The only records with a guid in the igrp_no column in #MatchCirc are records that have group matches
	--
	INSERT INTO #indGrpIgrpNo_Assign
	SELECT indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,grpNamePhone,grpDistEmail,Igrp_no 
	FROM #MatchCirc
	WHERE Igrp_no IS NOT NULL AND (indNameEmail IS NOT NULL OR indNameAddress IS NOT NULL OR indNameCompany IS NOT NULL OR indNamePhone IS NOT NULL OR indNameNotBlankEmail IS NOT NULL OR indDistEmail IS NOT NULL)
	GROUP BY indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,grpNamePhone,grpDistEmail,Igrp_no
	    

	UPDATE mg
	SET Igrp_no = CASE WHEN tmg.indNameEmail IS NOT NULL THEN tmg.indNameEmail
						WHEN tmg.indNameAddress IS NOT NULL THEN tmg.indNameAddress
						WHEN tmg.indNameCompany IS NOT NULL THEN tmg.indNameCompany
						WHEN tmg.indNamePhone IS NOT NULL THEN tmg.indNamePhone
						WHEN tmg.indNameNotBlankEmail IS NOT NULL THEN tmg.indNameNotBlankEmail
						WHEN tmg.indDistEmail IS NOT NULL THEN tmg.indDistEmail
						ELSE tmg.Igrp_no END
	FROM #MatchCirc mg INNER JOIN #indGrpIgrpNo_Assign tmg ON mg.Igrp_no = tmg.igrp_no
		
	DROP TABLE #indGrpIgrpNo_Assign
		        
	PRINT ('END Assign IGRP_NO to matched Subscriptions and matched within its own sourcefile/ '  + CONVERT(VARCHAR(20), GETDATE(), 114))	       
	--
	-- Assign igrp_no to records without any group match but did match against SubscriberFina
	--
	PRINT ('Begin Assign IGRP_NO / '  + CONVERT(VARCHAR(20), GETDATE(), 114))		
		
	UPDATE #MatchCirc
	SET Igrp_NO = CASE WHEN indNameEmail IS NOT NULL AND Igrp_no IS NULL THEN indNameEmail
						WHEN indNameAddress IS NOT NULL AND Igrp_no IS NULL  THEN indNameAddress
						WHEN indNameCompany IS NOT NULL AND Igrp_no IS NULL  THEN indNameCompany
						WHEN indNamePhone IS NOT NULL AND Igrp_no IS NULL  THEN indNamePhone
						WHEN indNameNotBlankEmail IS NOT NULL AND Igrp_no IS NULL  THEN indNameNotBlankEmail
						WHEN indDistEmail IS NOT NULL AND Igrp_no IS NULL  THEN indDistEmail ELSE Igrp_NO END        
		        
	--
	-- Create igrp_no for any remaining records without an igrp_no
	--
	UPDATE #MatchCirc
	SET Igrp_no = NEWID()
	WHERE Igrp_no is null

	--
	-- Update SubscriberFinal igrp_no
	--
	UPDATE sf
	SET IGRP_NO = mg.Igrp_no
	FROM SubscriberFinal sf INNER JOIN #MatchCirc mg on sf.SFRecordIdentifier = mg.SFRecordIdentifier
	WHERE sf.SourceFileID = @SourceFileID AND sf.ProcessCode = @ProcessCode 

	--
	-- Drop temp tables as they are no longer needed
	--
	DROP TABLE #MatchCirc
	DROP TABLE #Groups

	PRINT ('END ASSIGN IGRP_NO / '  + CONVERT(VARCHAR(20), GETDATE(), 114))


    --END ASSIGN IGRP_NO
    -------------------------------------------------------------------------------------------------------------
    --BEGIN REMOVE INTERNAL DUPLICATES

	PRINT ('BEGIN REMOVE INTERNAL DUPLICATES / '  + CONVERT(VARCHAR(20), GETDATE(), 114))

	CREATE TABLE #tmp_pubcodeGroupDupe_inner(pubcode varchar(100), igrp_no UNIQUEIDENTIFIER)

	CREATE CLUSTERED INDEX IDX_C_tmp_pubcodeGroupDupe_inner ON #tmp_pubcodeGroupDupe_inner(pubcode, igrp_no)

	Insert into #tmp_pubcodeGroupDupe_inner
	SELECT pubcode,igrp_no
			FROM SubscriberFinal sf1 WITH(NoLock) 
			WHERE sf1.ProcessCode = @ProcessCode AND sf1.SourceFileID = @SourceFileId
			GROUP BY PubCode,IGrp_No HAVING COUNT(1) > 1

	PRINT ('Insert into #tmp_pubcodeGroupDupe_inner  / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
				
	-- Remove duplicate within pubcode and igrp_no 	
	CREATE TABLE #tmp_pubcodeGroupDupe(
		SFRecordIdentifier UNIQUEIDENTIFIER
		,IsLatLonValid bit
		,QDate datetime
		,IGrp_No UNIQUEIDENTIFIER
		,pubcode varchar(255)
		,StatusUPDATEdReason VARCHAR(200))

	CREATE INDEX idx_pubcodeGroup ON #tmp_pubcodeGroupDupe(SFRecordIdentifier,IsLatLonValid,QDate,IGrp_No,StatusUPDATEdReason)

	INSERT INTO #tmp_pubcodeGroupDupe
	SELECT SFRecordIdentifier,sf.IsLatLonValid,sf.QDate,sf.IGrp_No,sf.pubcode,NULL as StatusUPDATEdReason
	FROM SubscriberFinal sf WITH(NoLock) 
		INNER JOIN #tmp_pubcodeGroupDupe_inner pub 
		ON sf.IGrp_No = pub.IGrp_No
	WHERE sf.ProcessCode = @ProcessCode AND SourceFileID = @SourceFileId
	GROUP BY SFRecordIdentifier,sf.IsLatLonValid,sf.QDate,sf.IGrp_No,sf.Pubcode,StatusUPDATEdReason
	ORDER BY sf.IGrp_No
		
	PRINT ('INSERT INTO #tmp_pubcodeGroupDupe  / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	drop table #tmp_pubcodeGroupDupe_inner 

	CREATE TABLE #tmp_TopOneMasterPubGroup ( SFRecordIdentifier UNIQUEIDENTIFIER)

	;WITH pubDupes 
			AS (SELECT SFRecordIdentifier, 
					(CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END) as IsLatLonValid
					,igrp_no 
					,pubcode
					,qdate
					,Row_number() 
						OVER ( 
						PARTITION BY igrp_no,pubcode 
						ORDER BY (CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END),qdate DESC) rn 
				FROM   #tmp_pubcodeGroupDupe)
	INSERT INTO #tmp_TopOneMasterPubGroup
	SELECT SFRecordIdentifier
	FROM   pubDupes 
	WHERE  rn = 1 
	ORDER BY SFRecordIdentifier

	PRINT ('WITH pubDupes  / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	-- Anything that is not a master will be removed
	UPDATE pgd
	SET StatusUPDATEdReason = 'Master within pubcode'
	FROM #tmp_pubcodeGroupDupe pgd inner join #tmp_TopOneMasterPubGroup tom ON pgd.SFRecordIdentifier = tom.SFRecordIdentifier

	PRINT ('StatusUPDATEdReason : Master within pubcode / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	UPDATE #tmp_pubcodeGroupDupe
	SET StatusUPDATEdReason = 'Dupe within pubcode'
	WHERE isnull(StatusUPDATEdReason,'')=''

	PRINT ('StatusUPDATEdReason : Dupe within pubcode / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	-- Change subordinate SFRecordIdentifer in SubscriberDemographicFinal to the master's SFRecordIdentifier, then archive the profile record from SubscriberFinal into SubscriberArchive
	CREATE TABLE #MasterIgrpNo(
		MSFRecordIdentifier uniqueidentifier,
		Igrp_no uniqueidentifier)

	INSERT INTO #MasterIgrpNo
	SELECT tom.SFRecordIdentifier,pgd.IGrp_No 
	FROM #tmp_TopOneMasterPubGroup tom INNER JOIN #tmp_pubcodeGroupDupe pgd on tom.SFRecordIdentifier = pgd.SFRecordIdentifier
	WHERE pubcode in (Select pubcode FROM #tmp_pubcodeGroupDupe Group by pubcode having COUNT(*) > 1)
		
	UPDATE sdf
	SET SFRecordIdentifier = x.MSFRecordIdentifier
	FROM SubscriberDemographicFinal sdf INNER JOIN (SELECT pgd.SFRecordIdentifier AS DupeSFRecordIdentifier,m.MSFRecordIdentifier,pgd.StatusUPDATEdReason 
													FROM #tmp_pubcodeGroupDupe pgd JOIN #MasterIgrpNo m ON pgd.IGrp_No = m.Igrp_no
													WHERE pgd.StatusUPDATEdReason = 'Dupe within pubcode') AS x
										ON sdf.SFRecordIdentifier = x.DupeSFRecordIdentifier

	-- Insert duplicate records into SubscriberArchive	
	DECLARE @satIDs table (SARecordIdentifier UNIQUEIDENTIFIER, SFRecordIdentifier UNIQUEIDENTIFIER, UNIQUE CLUSTERED (SFRecordIdentifier))

	INSERT INTO SubscriberArchive
	(
			SFRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
			Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
			Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
			LatLonMsg,Score,EmailStatusID,StatusUPDATEdDate,StatusUPDATEdReason,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUPDATEdInLive,
			UPDATEInLiveDate,SARecordIdentifier,DateCreated,DateUPDATEd,CreatedByUserID,UPDATEdByUserID,IsMailable,ProcessCode,ImportRowNumber
	)  
	OUTPUT Inserted.SARecordIdentifier, Inserted.SFRecordIdentifier
			INTO @satIDs
	SELECT 
			sf.SFRecordIdentifier,sf.SourceFileID,sf.PubCode,sf.Sequence,sf.FName,sf.LName,sf.Title,sf.Company,sf.Address,sf.MailStop,sf.City,sf.State,sf.Zip,sf.Plus4,sf.ForZip,sf.County,sf.Country,CountryID,sf.Phone,sf.PhoneExists,
			sf.Fax,sf.FaxExists,sf.Email,sf.EmailExists,sf.CategoryID,sf.TransactionID,sf.TransactionDate,sf.QDate,sf.QSourceID,sf.RegCode,sf.Verified,sf.SubSrc,sf.OrigsSrc,sf.Par3C,sf.Demo31,sf.Demo32,sf.Demo33,sf.Demo34,sf.Demo35,sf.Demo36,sf.Source,
			sf.Priority,sf.IGrp_No,sf.IGrp_Cnt,sf.CGrp_No,sf.CGrp_Cnt,sf.StatList,sf.Sic,sf.SicCode,sf.Gender,sf.IGrp_Rank,sf.CGrp_Rank,sf.Address3,sf.Home_Work_Address,sf.PubIDs,sf.Demo7,sf.IsExcluded,sf.Mobile,sf.Latitude,sf.Longitude,sf.IsLatLonValid,
			sf.LatLonMsg,sf.Score,sf.EmailStatusID,sf.StatusUPDATEdDate,'Duplicate within pubcode' as StatusUPDATEdReason,sf.Ignore,sf.IsDQMProcessFinished,sf.DQMProcessDate,sf.IsUPDATEdInLive,
			sf.UPDATEInLiveDate,NEWID(),sf.DateCreated,sf.DateUPDATEd,sf.CreatedByUserID,sf.UPDATEdByUserID,sf.IsMailable,sf.ProcessCode,sf.ImportRowNumber
	FROM SubscriberFinal sf With(NoLock)
			INNER JOIN #tmp_pubcodeGroupDupe pg ON sf.SFRecordIdentifier = pg.SFRecordIdentifier
	WHERE pg.StatusUPDATEdReason = 'Dupe within pubcode'
	AND sf.SourceFileID = @SourceFileId 
	AND sf.ProcessCode = @ProcessCode 
	AND sf.SFRecordIdentifier NOT IN (SELECT sa.SFRecordIdentifier FROM SubscriberArchive sa where sa.SourceFileID = @SourceFileId  and sa.ProcessCode = @ProcessCode );
		
	PRINT ('Insert Into SubscriberArchive / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DELETE sf FROM SubscriberFinal sf join @satIDs t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	WHERE sf.SourceFileID = @sourceFileID and sf.ProcessCode = @Processcode

	DROP TABLE #tmp_pubcodeGroupDupe
	DROP TABLE #tmp_TopOneMasterPubGroup
	DROP TABLE #MasterIgrpNo
	

    -------------------------------------------------------------------------------------------------------------------------------------------------
    -- There may be files with multiple pubcodes in an igrp_no from the current processing file and igrp_no is not in Subscriptions
    -- Assign M records for SubscriberFinal
	CREATE TABLE #tmp_SubAssignMaster(
				SFRecordIdentifier UNIQUEIDENTIFIER
			,IsLatLonValid BIT
			,QDate DATETIME
			,IGrp_No UNIQUEIDENTIFIER
			,igrp_rank VARCHAR(1))

	CREATE INDEX idx_SubAssignMaster ON #tmp_SubAssignMaster(SFRecordIdentifier,IsLatLonValid,QDate,IGrp_No,igrp_rank)

	CREATE TABLE #tmp_SubAssignMaster_innerquery (igrp_no UNIQUEIDENTIFIER)

	CREATE CLUSTERED INDEX IDX_tmp_SubAssignMaster_innerquery_igrp_no ON #tmp_SubAssignMaster_innerquery(igrp_no)

	INSERT INTO #tmp_SubAssignMaster_innerquery
	SELECT IGRP_NO 
	FROM SubscriberFinal
	WHERE ProcessCode = @ProcessCode AND SourceFileID = @SourceFileId
	GROUP BY IGrp_No HAVING Count(SubscriberFinalID) > 1

	PRINT ('INSERT INTO #tmp_SubAssignMaster_innerquery / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	INSERT INTO #tmp_SubAssignMaster
		SELECT sf1.SFRecordIdentifier,sf1.IsLatLonValid,sf1.QDate,sf2.igrp_no,null
		FROM SubscriberFinal sf1 WITH(NOLOCK) INNER JOIN #tmp_SubAssignMaster_innerquery as sf2 ON sf1.IGrp_No = sf2.IGrp_No
		WHERE sf1.ProcessCode = @ProcessCode AND sf1.SourceFileID = @SourceFileId AND ISNULL(Igrp_rank,'')=''
		GROUP BY sf1.SFRecordIdentifier,sf1.IsLatLonValid,sf1.QDate,sf2.IGrp_No,sf1.igrp_rank,sf1.StatusUpdatedReason

	PRINT ('INSERT INTO #tmp_SubAssignMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmp_SubAssignMaster_innerquery

	CREATE TABLE #tmp_InternalMaster ( SFRecordIdentifier UNIQUEIDENTIFIER)

	;WITH assignMaster 
	AS (SELECT SFRecordIdentifier, 
			(CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END) AS IsLatLonValid
			,igrp_no 
			,igrp_rank
			,qdate 
			,Row_number() 
			OVER ( 
			PARTITION BY igrp_no 
			ORDER BY (CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END),qdate DESC) rn 
		FROM   #tmp_SubAssignMaster)
	INSERT INTO #tmp_InternalMaster
	SELECT SFRecordIdentifier
	FROM   assignMaster 
	WHERE  rn = 1 
	ORDER BY SFRecordIdentifier

	PRINT ('INSERT INTO #tmp_InternalMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	UPDATE #tmp_SubAssignMaster
	SET igrp_rank = 'M'
	FROM #tmp_SubAssignMaster sm inner join #tmp_InternalMaster im ON sm.SFRecordIdentifier = im.SFRecordIdentifier

	PRINT ('UPDATE #tmp_SubAssignMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	drop table #tmp_InternalMaster

	UPDATE #tmp_SubAssignMaster
	SET igrp_rank = 'S'
	WHERE ISNULL(igrp_rank,'')=''

	PRINT ('UPDATE #tmp_SubAssignMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	UPDATE sf
	SET Igrp_Rank = sm.Igrp_Rank
	FROM SubscriberFinal sf INNER JOIN #tmp_SubAssignMaster sm ON sf.SFRecordIdentifier = sm.SFRecordIdentifier
		
	PRINT ('UPDATE SubscriberFinal / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
	
	Drop table #tmp_SubAssignMaster

	---------------------------------------------------------------------------------------------------------------------------------
    -- Assign M to igrp_rank because igrp_no may be a new record and is the only record with that igrp_no
    UPDATE SubscriberFinal
    SET IGrp_Rank = 'M'
    WHERE ISNULL(igrp_rank,'')='' AND @SourceFileID = @SourceFileID AND ProcessCode = @ProcessCode

	PRINT ('UPDATE SubscriberFinal TO M / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

    --END ASSIGN IGRP_RANK

	Create table #tbl1 (SubscriberFinalID int, Igrp_No uniqueidentifier, Igrp_Rank varchar(2), PubID int, SubscriptionID int, SubscriberID int)

	CREATE CLUSTERED INDEX IDX_C_tbl1_IncomingDataID ON #tbl1(SubscriberFinalID)
    
	CREATE INDEX IDX_tbl1_Igrp_No ON #tbl1(Igrp_No)
	CREATE INDEX IDX_tbl1_SubscriptionID ON #tbl1(SubscriptionID)
	CREATE INDEX IDX_tbl1_SubscriberID ON #tbl1(SubscriberID)
	CREATE INDEX IDX_tbl1_Igrp_Rank ON #tbl1(Igrp_Rank)

	insert into #tbl1 (SubscriberFinalID, IGRP_NO, Igrp_Rank, PubID, SubscriberID)
	select MAX(sf.SubscriberFinalID), sf.IGrp_No, min(sf.IGrp_Rank), p.PubID, s.SubscriberID
	from 
			(select sf1.IGrp_No, min(sf1.IGrp_Rank) as IGrp_Rank, PubCode from SubscriberFinal sf1 With(NoLock) where sf1.ProcessCode = @ProcessCode and Ignore = 0 and isUpdatedinLIVE = 0  group by sf1.IGrp_No, PubCode) x 
			join SubscriberFinal sf on sf.IGrp_No = x.IGrp_No and sf.IGrp_Rank = x.IGrp_Rank and sf.PubCode = x.PubCode 
			JOIN Pubs p WITH (nolock) ON p.PubCode = sf.PubCode
			left outer join Circulation..Subscriber s WITH (nolock) on sf.IGRP_NO = s.IGRP_NO 
			INNER JOIN Circulation..Subscription ss With(NoLock) on s.SubscriberID = ss.SubscriberID
	where Ignore = 0 and isUpdatedinLIVE = 0 and sf.ProcessCode = @ProcessCode and ss.PublicationID = @PublicationID
	GROUP BY sf.IGrp_No, p.PubID, s.SubscriberID
	order by 2,3

	--SELECT * FROM #tbl1		

	--DECLARE @distinctComps TABLE (CompName VARCHAR(200), CompCount int, SFRecordIdentifier UNIQUEIDENTIFIER, IssueID int)
	--DECLARE @PublisherID INT = (SELECT PublisherID FROM Circulation..Publication WHERE PublicationID = @PublicationID)
	--DECLARE @PubCode varchar(100) = (Select PubCode FROM Pubs where PubID = @PublicationID)

	/* GET LIST OF DISTINCT COMPS AND COUNT OF ALL TOTAL WITH WHO IS IN COMP */
	INSERT INTO @distinctComps (CompName, CompCount, SFRecordIdentifier)
		Select PubCode,COUNT(PubCode),SF.SFRecordIdentifier
		FROM SubscriberFinal SF join #tbl1 t on SF.SubscriberFinalID = t.SubscriberFinalID
			WHERE SF.ProcessCode = @ProcessCode and t.SubscriberID is null and PubCode = @PubCode
			GROUP BY SF.PubCode, SF.SFRecordIdentifier

	/* APPEND THE ISSUEID TO THE LIST */
	UPDATE DC
		SET IssueID = (SELECT ISNULL(IssueID,0) FROM Circulation..Issue I join Circulation..Publication P ON I.PublicationId = P.PublicationID WHERE IsComplete = 0 and P.PublicationCode = DC.CompName)
		FROM @distinctComps DC JOIN SubscriberFinal SF ON DC.SFRecordIdentifier = SF.SFRecordIdentifier
		WHERE SF.ProcessCode = @ProcessCode 

	/* ANY ISSUEID OF ZERO WILL BE CONSIDERED ERRORS AND SHOULD BE MESSAGED */
	INSERT INTO Circulation..IssueCompError (CompName, SFRecordIdentifier, ProcessCode, DateCreated, CreatedByUserID)
		SELECT CompName, SFRecordIdentifier, @ProcessCode, GETDATE(), 1 FROM @distinctComps where IssueID = 0

	/* INSERT ISSUEID'S THAT AREN'T ZERO */
	INSERT INTO Circulation..IssueComp (IssueId, ImportedDate, IssueCompCount, DateCreated, CreatedByUserID)
		SELECT 
			IssueID,
			GETDATE(),
			CompCount,
			GETDATE(),
			1
		FROM @distinctComps WHERE IssueID > 0
				
	/* INSERT RECORD INTO DETAIL FOR THE SUBSCRIBER */
	INSERT INTO Circulation..IssueCompDetail (IssueCompID,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City
												,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate
												,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,SequenceID
												,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID
												,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify
												,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
		SELECT (SELECT IssueCompID FROM Circulation..IssueComp WHERE IssueID = DC.IssueID),NULL,SF.FName,SF.LName,SF.Company,SF.Title,NULL,NULL,SF.Address,SF.MailStop,NULL,SF.City,
				SF.State,NULL,SF.Zip,SF.Plus4,NULL,SF.County,SF.Country,SF.CountryID,SF.Latitude,SF.Longitude,SF.IsLatLonValid,SF.StatusUpdatedDate,
				NULL,SF.LatLonMsg,SF.Email,SF.Phone,SF.Fax,SF.Mobile,NULL,NULL,NULL,NULL,NULL,0,NULL,0,			   
				@PublisherID,0,@PublicationID,(Select ISNULL((Select ActionID from Circulation..Action where CategoryCodeID = SF.CategoryID and TransactionCodeID = SF.TransactionID and ActionTypeID = 1), 0)),0,0,0,SF.QSourceID,SF.QDate,SF.Demo7,
				0,NULL,0,NULL,SF.Par3C,0,NULL,NULL,NULL,SF.Verified,
				SF.DateCreated,SF.DateUpdated,SF.CreatedByUserID,SF.UpdatedByUserID
		FROM SubscriberFinal SF			
			JOIN @distinctComps DC ON SF.SFRecordIdentifier = DC.SFRecordIdentifier
			WHERE SF.ProcessCode = @ProcessCode

	drop table #tbl1
END


--			/*------ OLD WITH DATA MATCHING AND CHECKING FOR DUPES ------*/
--	/* GET LIST OF DISTINCT COMPS AND COUNT OF ALL TOTAL WITH WHO IS IN COMP */
--	INSERT INTO @distinctComps (CompName, CompCount, SFRecordIdentifier)
--		Select PubCode,COUNT(PubCode),SF.SFRecordIdentifier
--		FROM SubscriberFinal SF
--			WHERE SF.ProcessCode = @ProcessCode GROUP BY SF.PubCode, SF.SFRecordIdentifier


--	/* APPEND THE ISSUEID TO THE LIST */
--	UPDATE DC
--		SET IssueID = (SELECT ISNULL(IssueID,0) FROM Circulation..Issue I join Circulation..Publication P ON I.PublicationId = P.PublicationID WHERE IsComplete = 0 and P.PublicationCode = DC.CompName)
--		FROM @distinctComps DC JOIN SubscriberFinal SF ON DC.SFRecordIdentifier = SF.SFRecordIdentifier
--		WHERE SF.ProcessCode = @ProcessCode 


--	/* ANY ISSUEID OF ZERO WILL BE CONSIDERED ERRORS AND SHOULD BE MESSAGED */
--	INSERT INTO Circulation..IssueCompError (CompName, SFRecordIdentifier, ProcessCode, DateCreated, CreatedByUserID)
--		SELECT CompName, SFRecordIdentifier, @ProcessCode, GETDATE(), 1 FROM @distinctComps where IssueID = 0


--	/* INSERT ISSUEID'S THAT AREN'T ZERO */
--	INSERT INTO Circulation..IssueComp (IssueId, ImportedDate, IssueCompCount, DateCreated, CreatedByUserID)
--		SELECT 
--			IssueID,
--			GETDATE(),
--			CompCount,
--			GETDATE(),
--			1
--		FROM @distinctComps WHERE IssueID > 0
				

--	/* INSERT RECORD INTO DETAIL FOR THE SUBSCRIBER */
--	INSERT INTO Circulation..IssueCompDetail (IssueCompID,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City
--											 ,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate
--											 ,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,SequenceID
--											 ,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID
--											 ,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify
--											 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
--		SELECT (SELECT IssueCompID FROM Circulation..IssueComp WHERE IssueID = DC.IssueID),NULL,SF.FName,SF.LName,SF.Company,SF.Title,NULL,NULL,SF.Address,SF.MailStop,NULL,SF.City,
--			   SF.State,NULL,SF.Zip,SF.Plus4,NULL,SF.County,SF.Country,SF.CountryID,SF.Latitude,SF.Longitude,SF.IsLatLonValid,SF.StatusUpdatedDate,
--			   NULL,SF.LatLonMsg,SF.Email,SF.Phone,SF.Fax,SF.Mobile,NULL,NULL,NULL,NULL,NULL,0,NULL,0,			   
--			   @PublisherID,0,@PublicationID,0,0,0,0,SF.QSourceID,SF.QDate,SF.Demo7,
--			   0,NULL,0,NULL,SF.Par3C,0,NULL,NULL,NULL,SF.Verified,
--			   SF.DateCreated,SF.DateUpdated,SF.CreatedByUserID,SF.UpdatedByUserID
--		FROM SubscriberFinal SF			
--			JOIN @distinctComps DC ON SF.SFRecordIdentifier = DC.SFRecordIdentifier
--			WHERE SF.ProcessCode = @ProcessCode
--END
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
					ProcessCode varchar(50) not null
	)
	CREATE NONCLUSTERED INDEX [IDX_SequenceID] ON #NCOAimport (SequenceID ASC)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into #NCOAimport 
	(
		SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,ProcessCode 
	)  

	SELECT SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,ProcessCode 
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
		ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle

    delete #NCOAimport where SequenceID not in (Select Sequence from Subscriptions with(nolock))

	declare @userID int = (select UserID from UAS..[User] where EmailAddress = 'NcoaImport@TeamKM.com')
    declare @addressUpdateSourceTypeCodeId int = (Select CodeId From uas..Code with(nolock) where CodeTypeId = 31 and CodeName='NCOA')
	declare @userLogTypeId int = (select CodeId from uas..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
	declare @appId int = (select ApplicationID from uas..Application with(nolock) where ApplicationName='UAD')
	
	Insert Into UAS..UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
	Values(@appId,@userLogTypeId,@userID,'Subscriptions','job_NCOA_AddressUpdate','',GETDATE());

			--Now actually update the address
				Update s
	set Address = i.Address1,
		MailStop = i.Address2,
		Address3 = '',
		City = i.City,
		State = i.RegionCode,
		Zip = i.ZipCode,
		Plus4 = i.Plus4,
		IsLatLonValid = 'false',
		Latitude = 0,
		Longitude = 0,
		LatLonMsg = '',
		DateUpdated = GETDATE(),
		UpdatedByUserID = @userID,
		AddressLastUpdatedDate = GETDATE(),
		AddressUpdatedSourceTypeCodeId = @addressUpdateSourceTypeCodeId
	from Subscriptions s
	join #NCOAimport i on i.SequenceID = s.Sequence
			
	
	DROP TABLE #NCOAimport
GO
PRINT N'Creating [dbo].[job_QSourceValidation]...';


GO
create procedure job_QSourceValidation
@SourceFileID int,
@ProcessCode varchar(50)
as
	CREATE TABLE #tmpInvalidQSourceID
	(
		SubscriberTransformedID INT,
		RowNumber INT,
		PubCode varchar(100),
		QSourceID int
	)
	CREATE CLUSTERED INDEX PK_tmpInvalidQSourceID_SubscriberTransformedID ON #tmpInvalidQSourceID(SubscriberTransformedID)
	
	INSERT INTO 
		#tmpInvalidQSourceID
	SELECT DISTINCT 
		SubscriberTransformedID,
		st.ImportRowNumber, 
		st.PubCode, 
		st.QSourceID
	FROM 
		dbo.SubscriberTransformed st WITH (NOLOCK)
		LEFT OUTER JOIN QSource q WITH (NOLOCK) ON st.QSourceID = q.QSourceID
	WHERE	
		st.SourceFileID = @sourceFileID 
		AND st.ProcessCode = @ProcessCode 
		AND q.QSourceID IS NULL
	---------------------------------------------------------------
	
	INSERT INTO ImportError (SourceFileID,RowNumber,FormattedException,ClientMessage,MAFField,BadDataRow,ThreadID,DateCreated,ProcessCode,IsDimensionError)
	
	SELECT 
		@sourcefileID,t.RowNumber,'QSourceID:' + CAST(t.QSourceID as varchar(20)) + ' does not exist',
		'QSourceID:' + CAST(t.QSourceID as varchar(20)) + ' does not exist','QSourceID','',-1,GETDATE(),@ProcessCode,'false'
	FROM 
		#tmpInvalidQSourceID t WITH(NOLOCK)
		
	DROP TABLE #tmpInvalidQSourceID
GO
PRINT N'Creating [dbo].[job_TelemarketingRules_ProcessCode]...';


GO
create procedure job_TelemarketingRules_ProcessCode
@ProcessCode varchar(50),
@FileType varchar(50)
as
 --   2 code paths - Long Form or Short Form - Short Form can have audit questions
 --   much like NCOA - match on Seq# then update data
 --   Shared rules
 --   - Updates existing active qualified record with new information from a pre-approved telemarketing script.
 --   - Cat 10  Xact 27
 --   - use qdate on incoming record 
--REPLACE ALL PAR3C WITH '1' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #' ') AND (FNAME #'  ' OR LNAME #'  ')
--REPLACE ALL PAR3C WITH '2' FOR ((function ='  ' or function = 'ZZ') AND TITLE ='  ') AND (FNAME#'  ' OR LNAME #'  ')
--REPLACE ALL PAR3C WITH '3' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #'  ') AND (FNAME ='  ' AND LNAME ='  ')
--REPLACE ALL PAR3C WITH '4' FOR ((function =' ' or function = 'ZZ') AND TITLE ='  ' AND FNAME ='  ' AND LNAME ='  ' AND COMPANY #'  ')
--REPLACE ALL PAR3C WITH '5' FOR COPIES >1

 --   - keep existing SequenceNumber
 --   - Assign a new batch. No limit to batch size.
 --   - Update any record within the existing file as long as the incoming qdate is greater than the current date on the record or the qsource is a (H,I,J,K,L,M,N).
 --   - if seqNumber is blank send through DQM Matching process to determin if subscriber already exist for this product or to create a new record
    
 --   Long Form rules
 --   - Overwrite existing demos from incoming  telemarketing file. 
 --   - If the incoming list does not have a demo7(media) field replace demo7 with "A" for print. If demo7 is on incoming file replace existing demo7 with demo7 incoming data.
    
 --   Short Form rules
 --   - Keep Demos from existing record when not provided on short form.
 --   - If the incoming list does not have a demo7(media) field  or the field is there but is blank replace demo7 with "A" for print.
 
	-- Assign a new batch. No limit to batch size.
	--how the hell can I batch all this to track before and after???
	-- will create a temp table to store all the before records, will do batching at the end.
	
	--create a temp copy of Circulation.Subscriptions that are about to be updated
	select cs.*
	into #tmpOriginalCS
	from Circulation..Subscription cs
	join SubscriberTransformed st on cs.SequenceID = st.Sequence
	and st.ProcessCode = @ProcessCode
	and st.Sequence > 0

	-- Cat 10  Xact 27
	-- use qdate on incoming record
   update cs
   set ActionID_Previous = ActionID_Current, 
	   ActionID_Current = (Select ActionID
						   From Circulation..Action a
						   join Circulation..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
						   join Circulation..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 10 and tc.TransactionCodeValue = 27
						   and a.IsActive = 'true'
						   and a.ActionTypeID = (select CodeId from UAS..Code where CodeName = 'System Generated')),
		QSourceDate = st.QDate
	from Circulation..Subscription cs
	join SubscriberTransformed st on cs.SequenceID = st.Sequence
	and st.ProcessCode = @ProcessCode
	and st.Sequence > 0
	
	--REPLACE ALL PAR3C WITH '1' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #' ') AND (FNAME #'  ' OR LNAME #'  ')
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdt.Value != '' and sdt.Value != 'zz') or t.Title != '') and (t.FName != '' or t.LName != '')
	and t.ProcessCode = @ProcessCode 
	and t.Sequence > 0
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 1
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	where ((sdt.Value != '' and sdt.Value != 'zz') or t.Title != '') and (t.FName != '' or t.LName != '')
	and t.ProcessCode = @ProcessCode 
	and t.Sequence > 0
	
	--REPLACE ALL PAR3C WITH '2' FOR ((function ='  ' or function = 'ZZ') AND TITLE ='  ') AND (FNAME#'  ' OR LNAME #'  ')
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdt.Value = '' or sdt.Value = 'zz') and t.Title = '') and (t.FName != '' or t.LName != '') 
	and t.ProcessCode = @ProcessCode 
	and t.Sequence > 0
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 2
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	where ((sdt.Value = '' or sdt.Value = 'zz') and t.Title = '') and (t.FName != '' or t.LName != '') 
	and t.ProcessCode = @ProcessCode
	and t.Sequence > 0
	
	--REPLACE ALL PAR3C WITH '3' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #'  ') AND (FNAME ='  ' AND LNAME ='  ')
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdt.Value != '' and sdt.Value != 'zz') or t.Title != '') and (t.FName = '' or t.LName = '') 
	and t.ProcessCode = @ProcessCode 
	and t.Sequence > 0
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 3
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	where ((sdt.Value != '' and sdt.Value != 'zz') or t.Title != '') and (t.FName = '' or t.LName = '') 
	and t.ProcessCode = @ProcessCode
	and t.Sequence > 0

	--REPLACE ALL PAR3C WITH '4' FOR ((function =' ' or function = 'ZZ') AND TITLE ='  ' AND FNAME ='  ' AND LNAME ='  ' AND COMPANY #'  ')
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdt.Value = '' or sdt.Value = 'zz') and t.Title = '' and t.FName = '' and t.LName = '' and t.Company != '')
	and t.ProcessCode = @ProcessCode 
	and t.Sequence > 0
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 4
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	where ((sdt.Value = '' or sdt.Value = 'zz') and t.Title = '' and t.FName = '' and t.LName = '' and t.Company != '')
	and t.ProcessCode = @ProcessCode
	and t.Sequence > 0

	--REPLACE ALL PAR3C WITH '5' FOR COPIES >1
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where cs.Copies > 1
	and t.ProcessCode = @ProcessCode 
	and t.Sequence > 0
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 5
	from Circulation..Subscription cs
	join SubscriberTransformed t on cs.SequenceID = t.Sequence
	join SubscriberDemographicTransformed sdt on sdt.STRecordIdentifier = t.STRecordIdentifier and sdt.MAFField = 'Function'
	where cs.Copies > 1
	and t.ProcessCode = @ProcessCode
	and t.Sequence > 0
				   
	--Update any record within the existing file as long as the incoming qdate is greater than the current date on the record or the qsource is a (H,I,J,K,L,M,N).
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber s with(nolock) on cs.SubscriberID = s.SubscriberID 
	join SubscriberTransformed t with(nolock) on cs.SequenceID = t.Sequence
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where cs.SequenceID = t.Sequence
	and t.Sequence > 0
	and t.ProcessCode = @ProcessCode
	and (t.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	and tcs.SubscriptionID is null
	
	update cs
    set cs.DateUpdated = GETDATE(),
    cs.QSourceDate = t.QDate,
    cs.SubscriberSourceCode = t.SubSrc
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber s with(nolock) on cs.SubscriberID = s.SubscriberID 
	join SubscriberTransformed t with(nolock) on cs.SequenceID = t.Sequence
	where cs.SequenceID = t.Sequence
	and t.Sequence > 0
	and t.ProcessCode = @ProcessCode
	and (t.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	
	--------------everything above this point updates Circulation..Subscription table
	
	---------------now get a temp Subscriber table to store those original values
	select s.*
	into #tmpOriginalCSubscriber
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber s with(nolock) on cs.SubscriberID = s.SubscriberID 
	join SubscriberTransformed t with(nolock) on cs.SequenceID = t.Sequence
	where cs.SequenceID = t.Sequence
	and t.Sequence > 0
	and t.ProcessCode = @ProcessCode
	and (t.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	
	update s
	set	s.FirstName = t.FName,
		s.LastName = t.LName,
		s.Company = t.Company, 
		s.Title = t.Title,
		s.Address1 = t.Address,
		s.Address2 = t.MailStop,
		s.Address3 = t.Address3,
		s.City = t.City,
		s.RegionCode = t.State,
		s.RegionID = (select isnull(RegionID,0) from uas..Region where CountryID = t.CountryID and RegionCode = t.State),
		s.ZipCode = t.Zip,
		s.Plus4 = t.Plus4,
		s.County = t.County,
		s.Country = t.Country,
		s.CountryID = t.CountryID,
		s.Latitude = 0,
		s.Longitude = 0,
		s.IsAddressValidated = 'false',
		s.AddressValidationDate = null,
		s.AddressValidationSource = null,
		s.AddressValidationMessage = null,
		s.Email = t.Email,
		s.Phone = t.Phone,
		s.Fax = t.Fax,
		s.Mobile = t.Mobile,
		s.Gender = t.Gender
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber s with(nolock) on cs.SubscriberID = s.SubscriberID 
	join SubscriberTransformed t with(nolock) on cs.SequenceID = t.Sequence
	where cs.SequenceID = t.Sequence
	and t.Sequence > 0
	and t.ProcessCode = @ProcessCode
	and (t.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	
	
	------------both long and short update Demo7
	--If the incoming list does not have a demo7(media) field replace demo7 with "A" for print. If demo7 is on incoming file replace existing demo7 with demo7 incoming data.
	select st.Sequence,st.Demo7,st.PubCode
	into #tmpDemo7Update
	from SubscriberTransformed st with(nolock) 
	where st.ProcessCode = @ProcessCode
	
	select srm.*
	into #tmpOriginalCSRM
	from Circulation..SubscriptionResponseMap srm
	join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
	join SubscriberTransformed st on s.SequenceID = st.Sequence
	join Pubs pub on st.PubCode = pub.PubCode
	join Circulation..Response r with(nolock) on srm.ResponseID = r.ResponseID and pub.PubID = r.PublicationID and r.ResponseName = 'DEMO7'
	where st.ProcessCode = @ProcessCode
	
	update srm
	set srm.DateUpdated = GETDATE(),
	srm.UpdatedByUserID = 1,
	srm.ResponseID = (select ResponseID 
					  from Circulation..Response r with(nolock) 
					  join Circulation..Publication p with(nolock) on r.PublicationID = p.PublicationID
					  where r.ResponseCode = isnull(nullif(st.Demo7,''),'A') and r.PublicationID = pub.PubID and r.ResponseName = 'DEMO7')
	from Circulation..SubscriptionResponseMap srm
	join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
	join #tmpDemo7Update st on s.SequenceID = st.Sequence
	join Pubs pub on st.PubCode = pub.PubCode
	join Circulation..Response r with(nolock) on srm.ResponseID = r.ResponseID and pub.PubID = r.PublicationID and r.ResponseName = 'DEMO7'
	 
	drop table #tmpDemo7Update
			
	if(@FileType = 'Telemarketing_Long_Form')
		begin
			--Overwrite existing demos from incoming  telemarketing file. 
			select sdt.MAFField,sdt.Value,st.Sequence,sdt.PubID
			into #tmpDemoUpdate
			from SubscriberDemographicTransformed sdt with(nolock)
			join SubscriberTransformed st with(nolock) on sdt.STRecordIdentifier = st.STRecordIdentifier
			where st.ProcessCode = @ProcessCode

			insert into #tmpOriginalCSRM
			select srm.*
			from Circulation..SubscriptionResponseMap srm
			join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
			join SubscriberTransformed st on s.SequenceID = st.Sequence
			join Pubs pub on st.PubCode = pub.PubCode
			join Circulation..Response r with(nolock) on srm.ResponseID = r.ResponseID and pub.PubID = r.PublicationID and r.ResponseName != 'DEMO7'
			where st.ProcessCode = @ProcessCode
	
			update srm
			set srm.DateUpdated = GETDATE(),
			srm.UpdatedByUserID = 1,
			srm.ResponseID = (select ResponseID 
							  from Circulation..Response r with(nolock) 
							  where r.ResponseCode = d.Value and r.PublicationID = d.PubID and r.ResponseName = d.MAFField and r.ResponseName != 'DEMO7')
			from Circulation..SubscriptionResponseMap srm
			join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
			join #tmpDemoUpdate d on s.SequenceID = d.Sequence
			join Circulation..Response r with(nolock) on srm.ResponseID = r.ResponseID and d.PubID = r.PublicationID and r.ResponseName != 'DEMO7'
			
			drop table #tmpDemoUpdate
			
			
		end
	--if(@FileType = 'Telemarketing_Short_Form')
	--	begin
	--		 --Keep Demos from existing record when not provided on short form.
	--		 --Do Nothing to demos
	--	end


--can't assume this will be only for one pubcode so need to pull multiple batches if needed.

	--temp tables with original values #tmpOriginalCS, #tmpOriginalCSubscriber, #tmpOriginalCSRM
	declare @PublisherID int = (select top 1 PublisherID
								from Circulation..Publisher p with(nolock)
								join UAS..SourceFile sf with(nolock) on p.ClientID = sf.ClientID
								join SubscriberTransformed st with(nolock) on st.SourceFileID = sf.SourceFileID 
								where st.ProcessCode = @ProcessCode)
								
	declare @pubCount int = (select count(distinct p.PubID)
								  from Pubs p with(nolock)
								  join SubscriberTransformed st with(nolock) on st.PubCode = p.PubCode 
								  where st.ProcessCode = @ProcessCode)
	if(@pubCount = 1)
		begin
			declare @tocsCount   int = (select COUNT(*) from #tmpOriginalCS)
			declare @tocSubCount int = (select COUNT(*) from #tmpOriginalCSubscriber)
			declare @tocsrmCount int = (select COUNT(*) from #tmpOriginalCSRM)
			declare @BatchCount int = (select @tocSubCount + @tocsCount + @tocsrmCount)
			
			declare @PublicationID int = (select distinct p.PubID
								  from Pubs p with(nolock)
								  join SubscriberTransformed st with(nolock) on st.PubCode = p.PubCode 
								  where st.ProcessCode = @ProcessCode)
			--pull a BatchID
			declare @BatchID int
			insert into Circulation..Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated)
			values(@PublicationID,1,@BatchCount,'true',GETDATE());set @BatchID = @@IDENTITY;  		
			
			Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
														  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
														  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
														  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription)


			select SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
				  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
				  DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,
				  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription
			from #tmpOriginalCS with(nolock)

			------------#tmpOriginalCSubscriber
			Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
														  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
														  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
														  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,
														  
														  ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,
														  RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
														  AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,
														  Age,Income,Gender,SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,IsLocked,LockDate,LockDateRelease,
														  LockedByUserID,PhoneExt)


			select cs.SubscriptionID,cs.SequenceID,cs.PublisherID,cs.SubscriberID,cs.PublicationID,cs.ActionID_Current,cs.ActionID_Previous,cs.SubscriptionStatusID,
				   cs.IsPaid,cs.QSourceID,cs.QSourceDate,cs.DeliverabilityID,cs.IsSubscribed,cs.SubscriberSourceCode,cs.Copies,cs.OriginalSubscriberSourceCode,
				   cs.DateCreated,cs.DateUpdated,cs.CreatedByUserID,cs.UpdatedByUserID,cs.AccountNumber,cs.GraceIssues,cs.OnBehalfOf,cs.MemberGroup,cs.Verify,cs.IsNewSubscription,

			ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,
					County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,
					Fax,Mobile,Website,Birthdate,Age,Income,Gender,tocs.DateCreated,tocs.DateUpdated,tocs.CreatedByUserID,tocs.UpdatedByUserID,IsLocked,LockDate,LockDateRelease,
					LockedByUserID,PhoneExt
			from #tmpOriginalCSubscriber tocs with(nolock)
			join Circulation..Subscription cs with(nolock) on tocs.SubscriberID = cs.SubscriberID 
			
			
			Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
			select @BatchID,@@ROWCOUNT,t.PublisherID,t.PublicationID,t.SubscriberID,t.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
			from #tmpOriginalCS t with(nolock)
			join Circulation..HistorySubscription hs with(nolock) on t.SubscriptionID = hs.SubscriptionID 
			
			Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
			select @BatchID,@@ROWCOUNT,cs.PublisherID,cs.PublicationID,cs.SubscriberID,cs.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
			from #tmpOriginalCSubscriber sub with(nolock)
			join Circulation..Subscription cs with(nolock) on sub.SubscriberID = cs.SubscriberID 
			join Circulation..HistorySubscription hs with(nolock) on cs.SubscriptionID = hs.SubscriptionID 
			
			drop table #tmpOriginalCS
			drop table #tmpOriginalCSubscriber
		end	
	else
		begin
			--do same thing as above but by pubcode
			--lets do a cursor for each distinct PubID
			declare @PubID int
			declare @PubCode varchar(50)

			declare c cursor
			for 
				select distinct PublisherID, PubCode 
				from Circulation..Publisher p with(nolock)
				join UAS..SourceFile sf with(nolock) on p.ClientID = sf.ClientID
				join SubscriberTransformed st with(nolock) on st.SourceFileID = sf.SourceFileID 
				where st.ProcessCode = @ProcessCode

			open c
			fetch next from c into @PubID,@PubCode
			while @@FETCH_STATUS = 0
			begin
					
				declare @tocsCount2   int = (select COUNT(*) from #tmpOriginalCS where PublicationID = @PubID)
				declare @tocSubCount2 int = (select COUNT(*) 
											 from #tmpOriginalCSubscriber t
											 join Circulation..Subscription s on t.SubscriberID = s.SubscriberID 
											 where s.PublicationID = @PubID)
				declare @tocsrmCount2 int = (select COUNT(*) 
											 from #tmpOriginalCSRM m
											 join Circulation..Subscription s on m.SubscriptionID = s.SubscriptionID 
											 where s.PublicationID = @PubID)
				declare @BatchCount2 int = (select @tocSubCount2 + @tocsCount2 + @tocsrmCount2)

				--pull a BatchID
				declare @BatchID2 int
				insert into Circulation..Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated)
				values(@PublicationID,1,@BatchCount2,'true',GETDATE());set @BatchID = @@IDENTITY;  		
				
				Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
															  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
															  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
															  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription)


				select SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
					  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
					  DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,
					  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription
				from #tmpOriginalCS with(nolock)
				where PublicationID = @PubID
				
				------------#tmpOriginalCSubscriber
				Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
															  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
															  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
															  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,
															  
															  ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,
															  RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
															  AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,
															  Age,Income,Gender,SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,IsLocked,LockDate,LockDateRelease,
															  LockedByUserID,PhoneExt)


				select cs.SubscriptionID,cs.SequenceID,cs.PublisherID,cs.SubscriberID,cs.PublicationID,cs.ActionID_Current,cs.ActionID_Previous,cs.SubscriptionStatusID,
					   cs.IsPaid,cs.QSourceID,cs.QSourceDate,cs.DeliverabilityID,cs.IsSubscribed,cs.SubscriberSourceCode,cs.Copies,cs.OriginalSubscriberSourceCode,
					   cs.DateCreated,cs.DateUpdated,cs.CreatedByUserID,cs.UpdatedByUserID,cs.AccountNumber,cs.GraceIssues,cs.OnBehalfOf,cs.MemberGroup,cs.Verify,cs.IsNewSubscription,

				ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,
						County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,
						Fax,Mobile,Website,Birthdate,Age,Income,Gender,tocs.DateCreated,tocs.DateUpdated,tocs.CreatedByUserID,tocs.UpdatedByUserID,IsLocked,LockDate,LockDateRelease,
						LockedByUserID,PhoneExt
				from #tmpOriginalCSubscriber tocs with(nolock)
				join Circulation..Subscription cs with(nolock) on tocs.SubscriberID = cs.SubscriberID 
				where PublicationID = @PubID
				
				Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
				select @BatchID2,@@ROWCOUNT,t.PublisherID,t.PublicationID,t.SubscriberID,t.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
				from #tmpOriginalCS t with(nolock)
				join Circulation..HistorySubscription hs with(nolock) on t.SubscriptionID = hs.SubscriptionID 
				where PublicationID = @PubID
				
				Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
				select @BatchID,@@ROWCOUNT,cs.PublisherID,cs.PublicationID,cs.SubscriberID,cs.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
				from #tmpOriginalCSubscriber sub with(nolock)
				join Circulation..Subscription cs with(nolock) on sub.SubscriberID = cs.SubscriberID 
				join Circulation..HistorySubscription hs with(nolock) on cs.SubscriptionID = hs.SubscriptionID 
				where cs.PublicationID = @PubID
				
				fetch next from c into @PubID,@PubCode
			end
			close c
			deallocate c
		end
GO
PRINT N'Creating [dbo].[job_UAD_SubscriberFinal_To_Circ_Subscriber_Subscription]...';


GO
CREATE PROCEDURE [dbo].[job_UAD_SubscriberFinal_To_Circ_Subscriber_Subscription]
	@ProcessCode varchar(50),
	@PublicationID int
AS
BEGIN	
	DECLARE @ids table (ID int, SFRecordIdentifier uniqueidentifier)
	DECLARE @PublisherID int = (Select PublisherID from Circulation..Publication where PublicationID = @PublicationID)

	INSERT INTO Circulation..Subscriber (ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3
										,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated
										,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income
										,Gender,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease
										,PhoneExt,IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier)
			OUTPUT INSERTED.SubscriberID, INSERTED.SFRecordIdentifier INTO @ids(ID, SFRecordIdentifier)

	SELECT NULL,FName,LName,Company,Title,NULL,NULL,Address,MailStop,NULL,City,State,NULL,Zip,Plus4,NULL,County,Country,CountryID,Latitude,Longitude,
		   IsLatLonValid,StatusUpdatedDate,NULL,LatLonMsg,Email,Phone,Fax,Mobile,NULL,NULL,NULL,NULL,NULL,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,NULL,0,
		   NULL,GETDATE(),NULL,NULL,0,NULL,NULL,NULL,NULL,IGrp_No,SFRecordIdentifier
	FROM SubscriberFinal sf where ProcessCode = @ProcessCode
	
	Insert into Circulation..Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous
										  ,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode
										  ,Copies,OriginalSubscriberSourceCode,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,Par3cID
										  ,SubsrcTypeID,AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,AddRemoveID)										  
		Select
		(Select isnull(MAX(SequenceID),0) + 1 From Circulation..Subscription where PublicationID = @PublicationID)
		,@PublisherID,S.SubscriberID,@PublicationID,(Select ActionID from Circulation..Action where CategoryCodeID = SF.CategoryID and TransactionCodeID = SF.TransactionID and ActionTypeID = 2),NULL
		,NULL,0,SF.QSourceID,SF.QDate,NULL,1,NULL
		,1,NULL,SF.DateCreated,SF.DateUpdated,SF.CreatedByUserID,SF.UpdatedByUserID,SF.Par3C
		,SF.SubSrc,NULL,NULL,NULL,NULL,SF.Verified,NULL,NULL	
		from SubscriberFinal SF 
		join @ids IDS on SF.SFRecordIdentifier = IDS.SFRecordIdentifier 
		join Circulation..Subscriber S on IDS.ID = S.SubscriberID
		where SF.ProcessCode = @ProcessCode	
END
GO
PRINT N'Creating [dbo].[o_MasterData_Select]...';


GO
CREATE PROCEDURE [dbo].[o_MasterData_Select]
AS
SELECT cmb.CodeSheetID, cmb.MasterID, mc.MasterValue, mc.MasterDesc, mg.DisplayName
	FROM CodeSheet_Mastercodesheet_Bridge cmb With(NoLock)
	inner join Mastercodesheet mc With(NoLock) on cmb.MasterID = mc.MasterID
	inner join MasterGroups mg With(NoLock) on mc.MasterGroupID = mg.MasterGroupID
GO
PRINT N'Creating [dbo].[o_Product_Select]...';


GO
CREATE procedure [dbo].[o_Product_Select]
AS
BEGIN

SET NOCOUNT ON;

	Select PubID as ProductID,PubName as ProductName,PubCode as ProductCode,ClientID,IsActive,IsUAD,IsCirc,AllowDataEntry from Pubs

END
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
	FROM	Circulation..Subscriber s JOIN
			Circulation..Subscription sp ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
			Circulation..SubscriptionPaid spp ON sp.SubscriptionID = spp.SubscriptionID JOIN
			Circulation..Action a ON a.ActionID = sp.ActionID_Current JOIN
			Circulation..CategoryCode dc ON a.CategoryCodeID = dc.CategoryCodeID JOIN
			Circulation..TransactionCode dt on a.TransactionCodeID = dt.TransactionCodeID JOIN
			Circulation..TransactionCodeType tct ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
			Circulation..CategoryCodeType cct ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
			Circulation..QualificationSource dq on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
			UAS..Country c ON c.CountryID = s.CountryID
	Where	
			sp.PublicationID = ' + Convert(varchar,@PublicationID) 
else if(@GetSubscriberIDs = 1)
	set @executeString =
	'Select	sp.SubscriberID as Count
	FROM	Circulation..Subscriber s JOIN
			Circulation..Subscription sp ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
			Circulation..SubscriptionPaid spp ON sp.SubscriptionID = spp.SubscriptionID JOIN
			Circulation..Action a ON a.ActionID = sp.ActionID_Current JOIN
			Circulation..CategoryCode dc ON a.CategoryCodeID = dc.CategoryCodeID JOIN
			Circulation..TransactionCode dt on a.TransactionCodeID = dt.TransactionCodeID JOIN
			Circulation..TransactionCodeType tct ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
			Circulation..CategoryCodeType cct ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
			Circulation..QualificationSource dq on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
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
			set @executeString = @executeString + ' and sp.DeliverabilityID in (SELECT DeliverabilityID from Circulation..Deliverability d with (NOLOCK) WHERE d.DeliverabilityID in (' + @Demo7 + '))'	
		
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
PRINT N'Creating [dbo].[sp_rpt_CategorySummary]...';


GO

CREATE proc [dbo].[sp_rpt_CategorySummary]
(
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
	@PrintColumns varchar(4000),   
	@Download bit     
)
as
Begin

	set nocount on	
	declare @pubID int	
	set @pubID = @PublicationID
	declare @PublicationCode varchar(20)
	select @PublicationCode  = PubCode from Pubs where PubID = @pubID
	declare @GetSubscriberIDs bit = 0
	
	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	create table #SubscriptionID (SubscriptionID int)   
	
	Insert into #SubscriptionID   
	exec sp_rpt_GetSubscriptionIDs @PublicationID, @CategoryIDs,
	@CategoryCodes,
	@TransactionIDs,
	@TransactionCodes,
	@QsourceIDs,
	@StateIDs,
	@Regions,
	@CountryIDs,
	@Email,
	@Phone,
	@Fax,
	@ResponseIDs,
	@Demo7,		
	@Year,
	@startDate,		
	@endDate,
	@AdHocXML,
	@GetSubscriberIDs
	
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	declare @cat table
		(
		CategoryCodeTypeID int,
		CategoryCodeID int,
		CategoryCodeTypeName varchar(100),
		CategoryCodeName varchar(100),
		CategoryCodeValue int
		)
	
		insert into @cat
		select	distinct cct.CategoryCodeTypeID, 
				cc.CategoryCodeID, 
				cct.CategoryCodeTypeName, 
				cc.CategoryCodeName,
				CategoryCodeValue
		from	
				Circulation..CategoryCodeType cct join 
				Circulation..CategoryCode cc on cc.CategoryCodeTypeID = cct.CategoryCodeTypeID 
				
	if @Download = 0
		Begin
			declare @sub table
			(
				scount int,
				CategoryCodeID int
			)
		
			insert into @sub
			select sum(s.copies), a.CategoryCodeID
			from	Circulation..Subscription  s join
					#SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID join
					Circulation..Action a on a.ActionID = s.ActionID_Current
			where 
					PublicationID = @PublicationID
			group by 
					a.CategoryCodeID
		
			select 
				c.CategoryCodeTypeID categorygroup_ID,
				c.CategoryCodeValue category_ID,
				c.CategoryCodeTypeName categorygroup_name,
				c.CategoryCodeName category_name,
				isnull(sum(scount),0) as total
			from @cat c left outer join @sub s on c.CategoryCodeID = s.CategoryCodeID 
			group by c.CategoryCodeTypeID, c.CategoryCodeID,c.CategoryCodeValue, c.CategoryCodeTypeName, c.CategoryCodeName
		end
	else
		Begin
			exec ('select  distinct ''' + @PublicationCode + ''' as PubCode, s.SubscriberID, s.EMAILADDRESS, s.FNAME, s.LNAME, ' +
				' s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate, s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QualificationDate, s.Subsrc, s.copies ' +
				@PrintColumns + 
			' from subscriptions  s join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID join Category C on s.Category_ID = C.Category_ID left outer join QSource q on q.Qsource_ID = s.Qsource_ID where PublicationID = ' + @PublicationID )
		end
	drop table #SubscriptionID
end
GO
PRINT N'Refreshing [dbo].[sp_Adhoc_Delete_CategoryID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Adhoc_Delete_CategoryID]';


GO
PRINT N'Refreshing [dbo].[sp_Adhoc_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Adhoc_Save]';


GO
PRINT N'Refreshing [dbo].[sp_Adhoc_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Adhoc_Select]';


GO
PRINT N'Refreshing [dbo].[sp_Adhoc_Select_CategoryID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Adhoc_Select_CategoryID]';


GO
PRINT N'Refreshing [dbo].[sp_Adhoc_SelectAll]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Adhoc_SelectAll]';


GO
PRINT N'Refreshing [dbo].[sp_AdhocCategory_Exists_ByName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_AdhocCategory_Exists_ByName]';


GO
PRINT N'Refreshing [dbo].[sp_AdhocCategory_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_AdhocCategory_Save]';


GO
PRINT N'Refreshing [dbo].[sp_AdhocCategory_Select_All]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_AdhocCategory_Select_All]';


GO
PRINT N'Refreshing [dbo].[job_ImportSubscriberClickActivity_XML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_ImportSubscriberClickActivity_XML]';


GO
PRINT N'Refreshing [dbo].[job_ImportSubscriberOpenActivity_XML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_ImportSubscriberOpenActivity_XML]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriberActivity]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriberActivity]';


GO
PRINT N'Refreshing [dbo].[sp_GetProductDimensionSubscriberData]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetProductDimensionSubscriberData]';


GO
PRINT N'Refreshing [dbo].[sp_GetConcensusViewData]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetConcensusViewData]';


GO
PRINT N'Refreshing [dbo].[job_CodesheetValidation]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_CodesheetValidation]';


GO
PRINT N'Refreshing [dbo].[job_FileValidator_CodesheetValidation]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_FileValidator_CodesheetValidation]';


GO
PRINT N'Refreshing [dbo].[e_Codesheet_Exists_ByResponseGroupIDValue]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Codesheet_Exists_ByResponseGroupIDValue]';


GO
PRINT N'Refreshing [dbo].[e_CodeSheet_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_CodeSheet_Select]';


GO
PRINT N'Refreshing [dbo].[e_CodeSheet_Select_PubID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_CodeSheet_Select_PubID]';


GO
PRINT N'Refreshing [dbo].[e_ImportFromUAS]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ImportFromUAS]';


GO
PRINT N'Refreshing [dbo].[job_Subscriptions_ImportSubscribe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_Subscriptions_ImportSubscribe]';


GO
PRINT N'Refreshing [dbo].[job_Subscriptions_ImportUnsubscribe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_Subscriptions_ImportUnsubscribe]';


GO
PRINT N'Refreshing [dbo].[o_PubSubscriptionDetailKVP_Select_SubscriptionID_PubCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_PubSubscriptionDetailKVP_Select_SubscriptionID_PubCode]';


GO
PRINT N'Refreshing [dbo].[o_SubscriberProductDemographic_Select_SubscriptionID_PubCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_SubscriberProductDemographic_Select_SubscriptionID_PubCode]';


GO
PRINT N'Refreshing [dbo].[sp_CodeSheet_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_CodeSheet_Delete]';


GO
PRINT N'Refreshing [dbo].[sp_Pubs_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Pubs_Delete]';


GO
PRINT N'Refreshing [dbo].[sp_ResponseGroups_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_ResponseGroups_Delete]';


GO
PRINT N'Refreshing [dbo].[sp_Subscriber_Codesheet_counts]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Subscriber_Codesheet_counts]';


GO
PRINT N'Refreshing [dbo].[spCopyPubCodes]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spCopyPubCodes]';


GO
PRINT N'Refreshing [dbo].[spCopyResponseGroups]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spCopyResponseGroups]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart2]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart2]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart3]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart3]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart4]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart4]';


GO
PRINT N'Refreshing [dbo].[spGetAdminReport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spGetAdminReport]';


GO
PRINT N'Refreshing [dbo].[spSaveCodeSheet]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spSaveCodeSheet]';


GO
PRINT N'Refreshing [dbo].[up_insertResponses_old]...';


GO
--EXECUTE sp_refreshsqlmodule N'[dbo].[up_insertResponses_old]';
--??

GO
PRINT N'Refreshing [dbo].[Usp_MergeSubscriberMasterValues]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[Usp_MergeSubscriberMasterValues]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberData]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberData]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberDimension]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberDimension]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberDimensionForExport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberDimensionForExport]';


GO
PRINT N'Refreshing [dbo].[sp_Subscriber_MasterCodesheet_counts]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Subscriber_MasterCodesheet_counts]';


GO
PRINT N'Refreshing [dbo].[sp_SummaryReport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SummaryReport]';


GO
PRINT N'Refreshing [dbo].[e_BrandDimension_Select_BrandID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_BrandDimension_Select_BrandID]';


GO
PRINT N'Refreshing [dbo].[e_MasterCodeSheet_Select_ByBrandID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_MasterCodeSheet_Select_ByBrandID]';


GO
PRINT N'Refreshing [dbo].[e_MasterGroup_Select_ByBrandID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_MasterGroup_Select_ByBrandID]';


GO
PRINT N'Refreshing [dbo].[e_SubscriberVisitActivity_Select_BySubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SubscriberVisitActivity_Select_BySubscriptionID]';


GO
PRINT N'Refreshing [dbo].[job_ImportSubscriberVisitActivity_XML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_ImportSubscriberVisitActivity_XML]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriberVisitActivity_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriberVisitActivity_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriberVistActivity]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriberVistActivity]';


GO
PRINT N'Refreshing [dbo].[e_ImportError_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ImportError_Save]';


GO
PRINT N'Refreshing [dbo].[e_ImportError_Select_ProcessCode_SourceFileID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ImportError_Select_ProcessCode_SourceFileID]';


GO
PRINT N'Refreshing [dbo].[job_CodesheetValidation_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_CodesheetValidation_Delete]';


GO
PRINT N'Refreshing [dbo].[o_ImportErrorSummary_Select_SourceFileID_ProcessCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ImportErrorSummary_Select_SourceFileID_ProcessCode]';


GO
PRINT N'Refreshing [dbo].[sp_SaveMarkets]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SaveMarkets]';


GO
PRINT N'Refreshing [dbo].[sp_SaveMarketsWithXML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SaveMarketsWithXML]';


GO
PRINT N'Refreshing [dbo].[o_SubscriberConsensusDemographic_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_SubscriberConsensusDemographic_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[o_SubscriptionDetailKVP_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_SubscriptionDetailKVP_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[sp_getOpportunity]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getOpportunity]';


GO
PRINT N'Refreshing [dbo].[SP_IMPORT_SUBSCRIBER_MASTERCODESHEET]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[SP_IMPORT_SUBSCRIBER_MASTERCODESHEET]';


GO
PRINT N'Refreshing [dbo].[sp_Mastercodesheet_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Mastercodesheet_Delete]';


GO
PRINT N'Refreshing [dbo].[sp_MasterGroup_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_MasterGroup_Delete]';


GO
PRINT N'Refreshing [dbo].[sp_Taxonomys]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Taxonomys]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart5]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart5]';


GO
PRINT N'Refreshing [dbo].[spSaveMasterCodeSheet]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spSaveMasterCodeSheet]';


GO
PRINT N'Refreshing [dbo].[spUpdateTopicsDimension]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateTopicsDimension]';


GO
PRINT N'Refreshing [dbo].[ccp_Canon_ConsensusDim_EventSwipe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[ccp_Canon_ConsensusDim_EventSwipe]';


GO
PRINT N'Refreshing [dbo].[ccp_SpecialityFoods_ConsensusDim_EventSwipe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[ccp_SpecialityFoods_ConsensusDim_EventSwipe]';


GO
PRINT N'Refreshing [dbo].[e_FilterExportField_GetDisplayName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FilterExportField_GetDisplayName]';


GO
PRINT N'Refreshing [dbo].[e_MasterGroup_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_MasterGroup_Select]';


GO
PRINT N'Refreshing [dbo].[spDownloaddetailsResponseCount]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDownloaddetailsResponseCount]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart1]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart1]';


GO
PRINT N'Refreshing [dbo].[e_Filters_Select_All]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Filters_Select_All]';


GO
PRINT N'Refreshing [dbo].[e_Filters_Select_UserID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Filters_Select_UserID]';


GO
PRINT N'Refreshing [dbo].[e_Product_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Product_Select]';


GO
PRINT N'Refreshing [dbo].[e_Product_Select_PubID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Product_Select_PubID]';


GO
PRINT N'Refreshing [dbo].[e_ProductSubscription_Select_PubCode_EmailAddress]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ProductSubscription_Select_PubCode_EmailAddress]';


GO
PRINT N'Refreshing [dbo].[e_ProductSubscription_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ProductSubscription_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_Pubs_Save_SortOrder]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Pubs_Save_SortOrder]';


GO
PRINT N'Refreshing [dbo].[job_GetBrandWelcomeLetter]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_GetBrandWelcomeLetter]';


GO
PRINT N'Refreshing [dbo].[job_GetWebsiteSubscriberRequest]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_GetWebsiteSubscriberRequest]';


GO
PRINT N'Refreshing [dbo].[o_PubSubscriptions_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_PubSubscriptions_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[rptProductSubscriberCounts]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[rptProductSubscriberCounts]';


GO
PRINT N'Refreshing [dbo].[sp_ClientProd]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_ClientProd]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberPubs]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberPubs]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberPubsForExport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberPubsForExport]';


GO
PRINT N'Refreshing [dbo].[sp_GetTop5BlastComparision]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetTop5BlastComparision]';


GO
PRINT N'Refreshing [dbo].[sp_SavePubs]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SavePubs]';


GO
PRINT N'Refreshing [dbo].[spUpdateBrandScore]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateBrandScore]';


GO
PRINT N'Refreshing [dbo].[spUpdateScore]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateScore]';


GO
PRINT N'Refreshing [dbo].[job_DataMatching]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_DataMatching]';


GO
PRINT N'Refreshing [dbo].[e_ProductSubscription_Insert]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ProductSubscription_Insert]';


GO
PRINT N'Refreshing [dbo].[e_ProductSubscription_Select_PubID_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ProductSubscription_Select_PubID_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_Email]';


GO
PRINT N'Refreshing [dbo].[job_PTNThirdPartyBTNExport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_PTNThirdPartyBTNExport]';


GO
PRINT N'Refreshing [dbo].[job_SyncEmailStatus]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_SyncEmailStatus]';


GO
PRINT N'Refreshing [dbo].[job_UpdateEmailStatus]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_UpdateEmailStatus]';


GO
PRINT N'Refreshing [dbo].[job_UpdateSendDimension]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_UpdateSendDimension]';


GO
PRINT N'Refreshing [dbo].[o_Subscriber_Select_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_Subscriber_Select_Email]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberByGL]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberByGL]';


GO
PRINT N'Refreshing [dbo].[sp_OpensClicks]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_OpensClicks]';


GO
PRINT N'Refreshing [dbo].[sp_PubSubscriptions_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_PubSubscriptions_Save]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart6]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart6]';


GO
PRINT N'Refreshing [dbo].[spUpdateSuppression]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateSuppression]';


GO
PRINT N'Refreshing [dbo].[Usp_UpdateSuppressedEmailFromSubRecord]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[Usp_UpdateSuppressedEmailFromSubRecord]';


GO
PRINT N'Refreshing [dbo].[e_PubTypes_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_PubTypes_Delete]';


GO
PRINT N'Refreshing [dbo].[sp_PubTypes_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_PubTypes_Save]';


GO
PRINT N'Refreshing [dbo].[sp_displayFilterValues]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_displayFilterValues]';


GO
PRINT N'Refreshing [dbo].[sp_getFilterValues]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getFilterValues]';


GO
PRINT N'Refreshing [dbo].[e_ResponseGroup_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ResponseGroup_Select]';


GO
PRINT N'Refreshing [dbo].[e_ResponseGroup_Select_PubID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ResponseGroup_Select_PubID]';


GO
PRINT N'Refreshing [dbo].[o_Get_FileMappingColumns]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_Get_FileMappingColumns]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart2_Bulk]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart2_Bulk]';


GO
PRINT N'Refreshing [dbo].[sp_GetSelectedSubscriberCount]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSelectedSubscriberCount]';


GO
PRINT N'Refreshing [dbo].[spUpdateSuppression_XML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateSuppression_XML]';


GO
PRINT N'Refreshing [dbo].[e_OrderDetails_Select_UserID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_OrderDetails_Select_UserID]';


GO
PRINT N'Refreshing [dbo].[e_ShoppingCarts_Select_UserID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ShoppingCarts_Select_UserID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_AddressUpdate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_AddressUpdate]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Insert_Profile]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Insert_Profile]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_GetInValidLatLon]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_GetInValidLatLon]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_GetInValidLatLon_Range]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_GetInValidLatLon_Range]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_Insert_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_Insert_Email]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_Select_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_Select_Email]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_UpdateLatLon]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_UpdateLatLon]';


GO
PRINT N'Refreshing [dbo].[o_Subscriber_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_Subscriber_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[sp_getFullSubscribers_using_ReportFilters]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getFullSubscribers_using_ReportFilters]';


GO
PRINT N'Refreshing [dbo].[sp_GetRecordsDownload]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetRecordsDownload]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberCountByRadius]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberCountByRadius]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberGLByIGRPNO]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberGLByIGRPNO]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberGLBySubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberGLBySubscriptionID]';


GO
PRINT N'Refreshing [dbo].[sp_getSubscribers_using_ReportFiltersXml]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getSubscribers_using_ReportFiltersXml]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_by_country]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_by_country]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_Canada]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_Canada]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_domestic]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_domestic]';


GO
PRINT N'Refreshing [dbo].[sp_SaveSubscriptions]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SaveSubscriptions]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriberScores]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriberScores]';


GO
PRINT N'Refreshing [dbo].[e_SubscriberExtenstion_getbySubscriberID_forRecordView]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SubscriberExtenstion_getbySubscriberID_forRecordView]';


GO
PRINT N'Refreshing [dbo].[e_SubscriptionsExtensionMapper_Exists_ByIDName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SubscriptionsExtensionMapper_Exists_ByIDName]';


GO
PRINT N'Refreshing [dbo].[e_SubscriptionsExtensionMapper_Exists_ByName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SubscriptionsExtensionMapper_Exists_ByName]';



GO
PRINT N'Refreshing [dbo].[sp_SubscriptionsExtensionMapper_Save]...';


GO
--??
--EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriptionsExtensionMapper_Save]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriptionsExtensionMapper_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriptionsExtensionMapper_Select]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriptionsExtensionMapper_Select_All]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriptionsExtensionMapper_Select_All]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart2A_FillSubscriptionExtension]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart2A_FillSubscriptionExtension]';


GO
PRINT N'Refreshing [dbo].[o_ImportErrorSummary_FileValidatorSelect_SourceFileID_ProcessCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ImportErrorSummary_FileValidatorSelect_SourceFileID_ProcessCode]';


GO
PRINT N'Refreshing [dbo].[job_ImportTopics_XML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_ImportTopics_XML]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart7]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart7]';


GO
PRINT N'Refreshing [dbo].[o_ConsensusDimension_SaveXML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ConsensusDimension_SaveXML]';


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'f6dec2be-2b90-4429-be07-ff4cc1f4b424')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('f6dec2be-2b90-4429-be07-ff4cc1f4b424')

GO

GO
PRINT N'Checking existing data against newly created constraints';

GO
ALTER TABLE [dbo].[PubSubscriptions] WITH CHECK CHECK CONSTRAINT [FK_PubSubscriptions_Pubs];

ALTER TABLE [dbo].[PubMarkets] WITH CHECK CHECK CONSTRAINT [FK_PubMarkets_Pubs];

ALTER TABLE [dbo].[PubGroups] WITH CHECK CHECK CONSTRAINT [FK_PubGroups_Pubs];

ALTER TABLE [dbo].[ResponseGroups] WITH CHECK CHECK CONSTRAINT [FK_ResponseGroups_Pubs];

ALTER TABLE [dbo].[Orders] WITH CHECK CHECK CONSTRAINT [FK_Orders_ApplicationUsers];

ALTER TABLE [dbo].[Orders] WITH CHECK CHECK CONSTRAINT [FK_Orders_Country];

ALTER TABLE [dbo].[Pricing] WITH CHECK CHECK CONSTRAINT [FK_Pricing_Brand];


GO
PRINT N'Update complete.';


GO


UPDATE ResponseGroups
SET ResponseGroupTypeId = 184
WHERE DisplayName LIKE 'FUNCTION' OR DisplayName LIKE 'BUSINESS'
go

alter proc [dbo].[e_MasterGroup_Save]
	@MasterGroupID int,
	@DisplayName varchar(50), 
	@Name varchar(100), 
	@IsActive bit , 
	@EnableSubReporting bit, 
	@EnableSearching bit = 'true', 
	@EnableAdhocSearch bit = 'true',
	@SortOrder int = 0,
	@DateCreated datetime = null,
	@DateUpdated datetime = null,
	@CreatedByUserID int = 0,
	@UpdatedByUserID int = 0
as
Begin
	SET NOCOUNT ON;
	if @MasterGroupID > 0
	begin
		if(@DateUpdated is null)
			begin
				set @DateUpdated = getdate()
			end

		UPDATE [MasterGroups] 
		SET [DisplayName] = @DisplayName, 
			[Name] = @Name, 
			[Description] = @Name,  
			[IsActive] = @IsActive, 
			[SortOrder] = @SortOrder,
			[EnableSubReporting] = @EnableSubReporting, 
			[EnableSearching] = @EnableSearching, 
			[EnableAdhocSearch] = @EnableAdhocSearch ,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE [MasterGroupID] = @MasterGroupID
		select @MasterGroupID;
	end
	else
	begin
		if(@DateCreated is null)
			begin
				set @DateCreated = getdate()
			end
		INSERT INTO 
		[MasterGroups] ([DisplayName], [Name], [Description], [IsActive],[EnableSubReporting], [EnableSearching], [EnableAdhocSearch], ColumnReference) 
		VALUES ( @DisplayName, @Name, @Name, @IsActive, @EnableSubReporting, @EnableSearching, @EnableAdhocSearch, 'MASTER_' + REPLACE (@Name, ' ', '_'))
		select @@IDENTITY;
    end
END
go

alter procedure e_ResponseGroup_Save
	@ResponseGroupID int,
	@PubID int,
	@ResponseGroupName varchar(100),
	@DisplayName varchar(100) = '',
	@DateCreated datetime = null,
	@DateUpdated datetime = null,
	@CreatedByUserID int = 0,
	@UpdatedByUserID int = 0,
	@DisplayOrder int = 0,
	@IsMultipleValue bit = 'false',
	@IsRequired bit = 'false',
	@IsActive bit = 'true',
	@WQT_ResponseGroupID int = 0,
	@ResponseGroupTypeId int = null
as
	if(@ResponseGroupID > 0)
		begin
			if(@DateUpdated is null)
			begin
				set @DateUpdated = getdate()
			end

			update ResponseGroups
			set ResponseGroupName = @ResponseGroupName,
				DisplayName = @DisplayName,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DisplayOrder = @DisplayOrder,
				IsMultipleValue = @IsMultipleValue,
				IsRequired = @IsRequired,
				IsActive = @IsActive,
				WQT_ResponseGroupID = @WQT_ResponseGroupID,
				ResponseGroupTypeId = @ResponseGroupTypeId
			where ResponseGroupID = @ResponseGroupID
			
			select @ResponseGroupID;
		end
	else
		begin
			if(@DateCreated is null)
			begin
				set @DateCreated = getdate()
			end
			insert into ResponseGroups (PubID,ResponseGroupName,DisplayName, DateCreated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, IsActive, WQT_ResponseGroupID, ResponseGroupTypeId)
			values(@PubID,@ResponseGroupName,@DisplayName, @DateCreated, @CreatedByUserID, @DisplayOrder, @IsMultipleValue, @IsRequired, @IsActive, @WQT_ResponseGroupID, @ResponseGroupTypeId);select @@IDENTITY;
		end
go


PRINT N'Altering [dbo].[Subscriptions]...';


GO
ALTER TABLE [dbo].[Subscriptions]
    ADD [IsActive]   BIT NULL,
        [IsMailable] BIT NULL;


GO
PRINT N'Altering [dbo].[e_Subscription_AddressUpdate]...';


GO
ALTER PROCEDURE e_Subscription_AddressUpdate
@xml xml

AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriptionID int, [Address] varchar(255) NULL, MailStop varchar(255) NULL, City varchar(50) NULL, [State] varchar(50) NULL,
		Zip varchar(10) NULL, Plus4 varchar(50) NULL, Latitude decimal(18, 15) NULL, Longitude decimal(18, 15) NULL, IsLatLonValid bit NULL,  
		LatLongMsg nvarchar(500) NULL, Country varchar(100) NULL, County varchar(100) NULL
	)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	insert into @import 
	(
		 SubscriptionID, [Address], MailStop, City, [State],Zip, Plus4, Latitude, Longitude, IsLatLonValid, LatLongMsg, Country, County
	)  
	
	SELECT 
		SubscriptionID, [Address], MailStop, City, [State],Zip, Plus4, Latitude, Longitude, IsLatLonValid, LatLongMsg, Country, County
	FROM OPENXML(@docHandle, N'/XML/Subscriber') 
	WITH   
	(  
		SubscriptionID int 'SubscriptionID',
		[Address] varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City', 
		[State] varchar(50) 'State',
		Zip varchar(10) 'Zip', 
		Plus4 varchar(10) 'Plus4', 
		Latitude decimal(18, 15) 'Latitude', 
		Longitude decimal(18, 15) 'Longitude', 
		IsLatLonValid bit 'IsLatLonValid', 
		LatLongMsg nvarchar(500) 'LatLongMsg',
		Country varchar(100) 'Country',
		County varchar(100) 'County'
	)

	EXEC sp_xml_removedocument @docHandle  

	UPDATE Subscriptions
	SET	Address = i.Address,
		MailStop = i.Mailstop,
		City = i.City,
		State = i.State,
		Zip = i.Zip,
		Plus4 = case when len(i.Plus4) > 0 and Subscriptions.Plus4 != i.Plus4 then i.Plus4 else Subscriptions.Plus4 end,
		Latitude = i.Latitude,
		Longitude = i.Longitude,
		IsLatLonValid = i.IsLatLonValid,
		LatLonMsg = i.LatLongMsg,
		Country = case when len(i.Country) > 0 and Subscriptions.Country != i.Country then i.Country else Subscriptions.Country end,
		County = case when len(i.County) > 0 and Subscriptions.County != i.County then Substring(i.County,0,20) else Subscriptions.County end
	FROM @import i
	WHERE Subscriptions.SubscriptionID = i.SubscriptionID
GO
PRINT N'Altering [dbo].[e_Subscription_Save]...';


GO
ALTER procedure e_Subscription_Save
@SubscriptionID int,
@Sequence int,
@FName varchar(100),
@LName varchar(100),
@Title varchar(100),
@Company	varchar(100),
@Address	varchar(255),
@MailStop varchar(255),
@City varchar(50),
@State varchar(50),
@Zip	varchar(10),
@Plus4 varchar(4),
@ForZip varchar(50),
@County varchar(20),
@Country varchar(100),
@CountryID int,
@Phone varchar(100),
@PhoneExists	bit,
@Fax	varchar(100),
@FaxExists bit,
@Email varchar(100),
@EmailExists	bit,
@CategoryID int,
@TransactionID int,
@TransactionDate smalldatetime,
@QDate datetime,
@QSourceID int,
@RegCode	varchar(5),
@Verified varchar(1),
@SubSrc varchar(8),
@OrigsSrc varchar(8),
@Par3C varchar(1),
@Demo31 bit,
@Demo32 bit,
@Demo33 bit,
@Demo34 bit,
@Demo35 bit,
@Demo36 bit,
@Source varchar(50),
@Priority varchar(4),
@IGrp_Cnt int,
@CGrp_No int,
@CGrp_Cnt int,
@StatList bit,
@Sic varchar(8),
@SicCode varchar(20),
@Gender varchar(1024),
@IGrp_Rank varchar(2),
@CGrp_Rank varchar(2),
@Address3 varchar(255),
@Home_Work_Address varchar(10),
@PubIDs varchar(2000),
@Demo7 varchar(1),
@IsExcluded bit,
@Mobile varchar(30),
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsLatLonValid bit,
@LatLonMsg nvarchar(1000),
@Score int,
@IGrp_No uniqueidentifier,
@Notes varchar(2000),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@AddressTypeCodeId int,
@AddressLastUpdatedDate datetime,
@AddressUpdatedSourceTypeCodeId int,
@IsActive BIT = 'true',
@IsMailable BIT = 'true'
as
	IF @SubscriptionID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE Subscriptions
			SET Sequence = @Sequence,
				FName = @FName,
				LName = @LName,
				Title = @Title,
				Company = @Company,
				Address = @Address,
				MailStop = @MailStop,
				City = @City,
				State = @State,
				Zip = @Zip,
				Plus4 = @Plus4,
				ForZip = @ForZip,
				County = @County,
				Country = @Country,
				CountryID = @CountryID,
				Phone = @Phone,
				PhoneExists = @PhoneExists,
				Fax = @Fax,
				FaxExists = @FaxExists,
				Email = @Email,
				EmailExists = @EmailExists,
				CategoryID = @CategoryID,
				TransactionID = @TransactionID,
				TransactionDate = @TransactionDate,
				QDate = @QDate,
				QSourceID = @QSourceID,
				RegCode = @RegCode,
				Verified = @Verified,
				SubSrc = @SubSrc,
				OrigsSrc = @OrigsSrc,
				Par3C = @Par3C,
				Demo31 = @Demo31,
				Demo32 = @Demo32,
				Demo33 = @Demo33,
				Demo34 = @Demo34,
				Demo35 = @Demo35,
				Demo36 = @Demo36,
				Source = @Source,
				Priority = @Priority,
				IGrp_Cnt = @IGrp_Cnt,
				CGrp_No = @CGrp_No,
				CGrp_Cnt = @CGrp_Cnt,
				StatList = @StatList,
				Sic = @Sic,
				SicCode = @SicCode,
				Gender = @Gender,
				IGrp_Rank = @IGrp_Rank,
				CGrp_Rank = @CGrp_Rank,
				Address3 = @Address3,
				Home_Work_Address = @Home_Work_Address,
				PubIDs = @PubIDs,
				Demo7 = @Demo7,
				IsExcluded = @IsExcluded,
				Mobile = @Mobile,
				Latitude = @Latitude,
				Longitude = @Longitude,
				IsLatLonValid = @IsLatLonValid,
				LatLonMsg = @LatLonMsg,
				Score = @Score,
				IGrp_No = @IGrp_No,
				Notes = @Notes,
				AddressTypeCodeId = @AddressTypeCodeId,
				AddressLastUpdatedDate = @AddressLastUpdatedDate,
				AddressUpdatedSourceTypeCodeId = @AddressUpdatedSourceTypeCodeId,
				IsActive = @IsActive,
				IsMailable = @IsMailable
			WHERE SubscriptionID = @SubscriptionID;

			SELECT @SubscriptionID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Subscriptions (Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,Fax,
									   FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,
									   Demo34,Demo35,Demo36,Source,Priority,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,
									   Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,IGrp_No,Notes,DateCreated,CreatedByUserID,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,IsActive,IsMailable)
			VALUES(@Sequence,@FName,@LName,@Title,@Company,@Address,@MailStop,@City,@State,@Zip,@Plus4,@ForZip,@County,@Country,@CountryID,@Phone,@PhoneExists,@Fax,
				   @FaxExists,@Email,@EmailExists,@CategoryID,@TransactionID,@TransactionDate,@QDate,@QSourceID,@RegCode,@Verified,@SubSrc,@OrigsSrc,@Par3C,@Demo31,@Demo32,@Demo33,
				   @Demo34,@Demo35,@Demo36,@Source,@Priority,@IGrp_Cnt,@CGrp_No,@CGrp_Cnt,@StatList,@Sic,@SicCode,@Gender,@IGrp_Rank,@CGrp_Rank,@Address3,@Home_Work_Address,@PubIDs,
				   @Demo7,@IsExcluded,@Mobile,@Latitude,@Longitude,@IsLatLonValid,@LatLonMsg,@Score,@IGrp_No,@Notes,@DateCreated,@CreatedByUserID,@AddressTypeCodeId,@AddressLastUpdatedDate,@AddressUpdatedSourceTypeCodeId,@IsActive,@IsMailable);SELECT @@IDENTITY;
		END
GO
PRINT N'Altering [dbo].[e_Subscriptions_GetInValidLatLon_Range]...';


GO
ALTER PROCEDURE [dbo].[e_Subscriptions_GetInValidLatLon_Range]
@Start int,
@End int
AS

SELECT	SubscriptionID,
		ADDRESS,
		CITY,
		State,
		ltrim(rtrim(ZIP)) as zip,
		--substring(ltrim(rtrim(ZIP)),1,6)  as  zip,
		COUNTRY,
		Latitude,
		Longitude,
		IsLatLonValid,
		LatLonMsg 
FROM Subscriptions WITH(NOLOCK)
WHERE isnull(IsLatLonValid,0) = 0
AND (LatLonMsg='not done' OR LatLonMsg = '' OR LatLonMsg IS NULL) 
and ((STATE <> 'FO' OR LEN(isnull(State,'')) > 0) OR LEN(isnull(zip,'')) > 0)
and LEN(ADDRESS) > 0
and ADDRESS NOT LIKE 'PO BOX%'
and CountryID in (1,2)
AND SubscriptionID BETWEEN @Start AND @End
GO
PRINT N'Altering [dbo].[e_ApplicationUsers_Save_FailedPasswordAttemptCount]...';


GO
ALTER proc [dbo].[e_ApplicationUsers_Save_FailedPasswordAttemptCount]
@UserName varchar(100)
as
		if exists(select username from ApplicationUsers with(nolock)  where username = @UserName)
		begin
		
			declare @FailedPasswordAttemptCount int
			
			select @FailedPasswordAttemptCount = ISNULL(FailedPasswordAttemptCount,0) from ApplicationUsers where UserName = @UserName
			
			if @FailedPasswordAttemptCount = 4 
			begin
				update ApplicationUsers 
				set 
					FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1,
					IsLockedOut = 1,
					LastLockOutDate = GETDATE()					 
				where 
					UserName = @UserName		
				
				select @FailedPasswordAttemptCount+1	
			end
			else if @FailedPasswordAttemptCount > 4
			begin
				select @FailedPasswordAttemptCount	
			end  
			else
			begin
				update ApplicationUsers 
				set 
					FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1 
				where 
					UserName = @UserName
					
				select @FailedPasswordAttemptCount+1	
			end	
		end	
		else
		Begin
			select 0	
		End
GO
PRINT N'Altering [dbo].[e_ApplicationUsers_Search]...';


GO
--INSERT
ALTER PROCEDURE [dbo].[e_ApplicationUsers_Search]
	@startdate varchar(25),
	@enddate varchar(25),
	@email varchar(50),
	@company varchar(100),
	@SalesForceID varchar(15),
	@SalesRepID uniqueidentifier,
	@UserID uniqueidentifier
AS
	if (@UserID = cast(cast(0 as binary) as uniqueidentifier))
	begin
		 select au.UserID, 
				au.Email,
				au.username,
				al.LastLoginDate,
				au.IsLockedOut,
				au.CreatedDate as DateAdded, 
				au.FullName as FullName,
				au.CompanyName as CompanyName,
				au.SalesForceID as SalesForceID,
				au.PackageLevel as PackageLevel,
				au.TrialExpireDate as TrialExpireDate,
				au.IsApproved,
				case when au.IsApproved = 0 then 'Inactive' else
				(case 
					when au.TrialExpireDate is null then 'Active'
					when au.TrialExpireDate < GETDATE() then 'Expired' else 'Active' end) 
				End as Status,
				r.RoleName,
				au1.FullName as SalesRepName
		 from ApplicationUsers au with(nolock) join  
			  Roles r with(nolock) on r.RoleId = au.RoleId left outer join
			  ApplicationUsers au1 with(nolock) on au.SalesRepID = au1.userID left outer join
			  (select userID, MAX(ActivityDate) as LastLoginDate from ActivityLog with(nolock) Group by userID) al on  au.userID = al.userID
		 where 
			 (len(@company)=0 or au.CompanyName LIKE @company + '%')  and
			 (len(@SalesForceID)=0 or au.SalesForceID = @SalesForceID ) and
			 (len(@email)=0 or au.Email LIKE '%' + @email + '%') and
			 (len(@startdate)=0  or au.TrialExpireDate  >= @startdate )and 
			 (len(@enddate)=0  or au.TrialExpireDate <= @enddate + ' 23:59:59') and
			 (@SalesRepID = cast(cast(0 as binary) as uniqueidentifier) or au.SalesRepID = @SalesRepID)  
		 order by au.CreatedDate
	end 
	else
	begin
		 select au.UserID, 
				au.Email,
				au.username,
				al.LastLoginDate,
				au.IsLockedOut,
				au.CreatedDate as DateAdded, 
				au.FullName as FullName,
				au.CompanyName as CompanyName,
				au.SalesForceID as SalesForceID,
				au.PackageLevel as PackageLevel,
				au.TrialExpireDate as TrialExpireDate,
				au.IsApproved,
				case when au.IsApproved = 0 then 'Inactive' else
				(case 
					when au.TrialExpireDate is null then 'Active'
					when au.TrialExpireDate < GETDATE() then 'Expired' else 'Active' end) 
				End as Status,
				r.RoleName,
				au1.FullName as SalesRepName
		 from ApplicationUsers au with(nolock) join  
			  Roles r with(nolock) on r.RoleId = au.RoleId left outer join
			  ApplicationUsers au1 with(nolock) on au.SalesRepID = au1.userID left outer join
			  (select userID, MAX(ActivityDate) as LastLoginDate from ActivityLog with(nolock) Group by userID) al on  au.userID = al.userID
		 where 
			 (len(@company)=0 or au.CompanyName LIKE @company + '%')  and
			 (len(@SalesForceID)=0 or au.SalesForceID = @SalesForceID ) and
			 (len(@email)=0 or au.Email LIKE '%' + @email + '%') and
			 (len(@startdate)=0  or au.TrialExpireDate  >= @startdate )and 
			 (len(@enddate)=0  or au.TrialExpireDate <= @enddate + ' 23:59:59') and
			 (@SalesRepID = cast(cast(0 as binary) as uniqueidentifier)  or au.SalesRepID = @SalesRepID) and
			 au.SalesRepID = @UserID
			 
		 order by au.CreatedDate
	end
GO
PRINT N'Altering [dbo].[e_ApplicationUsers_Select_UserName_Password]...';


GO
ALTER PROCEDURE [dbo].[e_ApplicationUsers_Select_UserName_Password]   
@UserName  varchar(100),
@Password  varchar(50)
AS
	if exists(select username from ApplicationUsers with(nolock)  where username = @UserName and password = @Password)
	begin
		update ApplicationUsers 
		set 
			FailedPasswordAttemptCount = null 
		where 
			UserName = @UserName	
	end

	Select * from ApplicationUsers With(NoLock) 
	where 
		UserName = @UserName and [Password] = @Password
GO
PRINT N'Altering [dbo].[e_BillingHistory_Select_UserID]...';


GO
ALTER PROCEDURE [dbo].[e_BillingHistory_Select_UserID]
	@UserID uniqueidentifier
	
AS
		select 
			RegID as PaymentID, 
			DateAdded as Paymentdate, 
			SubscriptionFee as amount,   
			('xxxxxxxx' + CardNo) as CardNo, 
			CardType, CardHolderName, 
			'Subscription Fee ' + CONVERT(varchar(10), DateAdded, 101) + ' - ' +  CONVERT(varchar(10),(DateAdd(Month, 1, DateAdded)-1),101) as description, 
			'Subscription' as ptype
		from UsersRegistration with(nolock)  
		where UserID = @UserID union all
		select 
			PaymentID as PaymentID, 
			DateAdded as Paymentdate, 
			SubscriptionFee as amount,   
			('xxxxxxxx' + CardNo) as CardNo, 
			CardType, CardHolderName, 
			'Subscription Fee ' as description, 
			'SubMonthly' as ptype
		from BillingHistory with(nolock)   
		where UserID = @UserID union all	
		select 
			OrderID as PaymentID, 
			OrderDate as Paymentdate, 
			OrderTotal as amount,   
			('xxxxxxxx' + CardNo) as CardNo, 
			CardType, 
			CardHolderName, 
			'Download Charges'  as description, 
			'Order' as ptype
		from Orders with(nolock)
		where orderTotal > 0 and  UserID = @UserID order by 2 desc
GO
PRINT N'Altering [dbo].[e_Orders_Save]...';


GO
ALTER PROCEDURE [dbo].[e_Orders_Save]
	@UserID  uniqueidentifier,
	@OrderSubTotal decimal(10,2),
	@PromotionCode varchar(10),
	@OrderTotal decimal(10,2),
	@CardHolderName varchar(50),
	@CardHolderAddress1 varchar(100),
	@CardHolderAddress2 varchar(100),
	@CardHolderCity varchar(50),
	@CardHolderState varchar(50),
	@CardHolderZip varchar(10),
	@CardHolderCountryID int,
	@CardHolderPhone varchar(25),
	@CardNo varchar(4),
	@CardExpirationMonth int,
	@CardExpirationYear int,
	@CardType varchar(25),
	@CardCVV varchar(10),
	@IsProcessed bit,
	@PaymentTransactionID varchar(50),
	@CartType int,
	@DownloadLockDays int = null,
	@CartIDs varchar(4000)
AS
	declare @OrderID int
	
	declare @shoppingcart Table (subscriptionID int)
	
	insert into @shoppingcart
	select  distinct s.SubscriptionID 
	from	ShoppingCarts s with(nolock) left outer join			
			OrderDetails od with(nolock) on s.SubscriptionID = od.SubscriptionID left outer join 
			Orders o with(nolock) on od.OrderID = o.OrderID 
	where  (o.OrderDate < (GETDATE() - 30) or o.OrderDate is null) and 
			s.UserID = @UserID  and (s.SearchTypeID = @CartType or @CartType=0)
			--and (ShoppingCartID in (select ITEMS from dbo.fn_Split(@CartIDs,','))) 
			
	if exists (select top 1 subscriptionID from @shoppingcart)
	Begin

		--OPEN SYMMETRIC KEY CDMKey DECRYPTION BY CERTIFICATE CDMCert
		
		INSERT INTO [Orders]
			  ([UserID]
			  ,[OrderDate]
			  ,[OrderSubTotal]
			  ,[PromotionCode]
			  ,[OrderTotal]
			  ,[CardHolderName]
			  ,[CardHolderAddress1]
			  ,[CardHolderAddress2]
			  ,[CardHolderCity]
			  ,[CardHolderState]
			  ,[CardHolderZip]
			  ,[CardHolderCountryID]
			  ,[CardHolderPhone]
			  ,[CardNo]
			  ,[CardExpirationMonth]
			  ,[CardExpirationYear]
			  ,[CardType]
			  ,[CardCVV]
			  ,[IsProcessed]
			  ,[PaymentTransactionID])
		      
		VALUES
			(@UserID, GETDATE(), @OrderSubTotal, @PromotionCode,@OrderTotal,@CardHolderName, @CardHolderAddress1, @CardHolderAddress2,
			 @CardHolderCity, @CardHolderState, @CardHolderZip, (case when @CardHolderCountryID = 0 then null else @CardHolderCountryID end), @CardHolderPhone, @CardNo, @CardExpirationMonth,
			 @CardExpirationYear, @CardType, @CardCVV, @IsProcessed,	@PaymentTransactionID)   

		if @CartType = 0
		begin
			if exists(select * from ShoppingCarts with(nolock) where UserID = @UserID)
			begin
				set @OrderID = @@IDENTITY

				insert into  OrderDetails (OrderID, UserID, SearchTypeID, SubscriptionID, Price, IsFreeDownload) 
				select @OrderID, UserID, SearchTypeID, s.SubscriptionID, case when IsFreeDownload = 1 then 0 else ISNULL(Price,0) end , IsFreeDownload  
				from ShoppingCarts s with(nolock) join @shoppingcart ts on s.SubscriptionID = ts.subscriptionID
				where UserID = @UserID
				
				--select  @OrderID, od.userID, o.OrderDate, s.UserID, s.SearchTypeID, s.SubscriptionID, ISNULL(s.Price,0), s.IsFreeDownload  
				--from ShoppingCarts s left outer join			
				--OrderDetails od on s.SubscriptionID = od.SubscriptionID left outer join 
				--Orders o on od.OrderID = o.OrderID 
				--where  (o.OrderDate < (GETDATE() - 30) or o.OrderDate is null) and 
				--s.UserID = @UserID 
				
				--select @OrderID, UserID, SearchTypeID, SubscriptionID, ISNULL(Price,0), IsFreeDownload  
				--from ShoppingCarts  
				--where UserID = @UserID 
				
				delete from ShoppingCarts where UserID = @UserID and SubscriptionID in (select SubscriptionID from @shoppingcart)
				
				SELECT @OrderID
			end
			else
			Begin
				RAISERROR('There is no items added to the cart',16,1)                   
				RETURN 
			End			
		end	
		else 
		begin
			if exists(select * from ShoppingCarts with(nolock) where UserID = @UserID and SearchTypeID = @CartType)
			begin
				set @OrderID = @@IDENTITY
				 
				insert into  OrderDetails (OrderID, UserID, SearchTypeID, SubscriptionID, Price, IsFreeDownload) 
				select @OrderID, UserID, SearchTypeID, s.SubscriptionID, case when IsFreeDownload = 1 then 0 else ISNULL(Price,0) end , IsFreeDownload  
				from ShoppingCarts s with(nolock) join @shoppingcart ts on s.SubscriptionID = ts.subscriptionID
				where UserID = @UserID and SearchTypeID = @CartType 
				
				delete from ShoppingCarts where UserID = @UserID and SubscriptionID in (select SubscriptionID from @shoppingcart)
				 
				--insert into  OrderDetails (OrderID, UserID, SearchTypeID, SubscriptionID, Price, IsFreeDownload) 
				--select @OrderID, UserID, SearchTypeID, SubscriptionID, ISNULL(Price,0), IsFreeDownload  
				--from ShoppingCarts  
				--where UserID = @UserID and SearchTypeID = @CartType
				
				--delete from ShoppingCarts where UserID = @UserID and SearchTypeID = @CartType
				
				SELECT @OrderID
			end	
			else
			Begin
				RAISERROR('There is no items added to the cart',16,1)                   
				RETURN 
			End				
		end
	end
GO
PRINT N'Altering [dbo].[e_Product_Save]...';


GO
ALTER PROCEDURE [dbo].[e_Product_Save]
	@PubID int,
	@PubName varchar(100),
	@istradeshow bit,
	@PubCode varchar(50),
	@PubTypeID int,
	@GroupID int,
	@EnableSearching bit,
	@score int,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int,
	@ClientID int,
	@YearStartDate varchar(5),
	@YearEndDate varchar(5),
	@IssueDate datetime,
	@IsImported bit,
	@IsActive bit,
	@AllowDataEntry bit,
	@FrequencyID int,
	@KMImportAllowed bit,
	@ClientImportAllowed bit,
	@AddRemoveAllowed bit,
	@AcsMailerInfoId int,
	@IsUAD bit,
	@IsCirc bit
AS
	IF @PubID > 0
	BEGIN
		UPDATE Pubs
			SET PubName = @PubName,
				istradeshow = @istradeshow, 
				PubCode = @PubCode, 
				PubTypeID = @PubTypeID, 
				GroupID = @GroupID, 
				EnableSearching = @EnableSearching, 
				score = @score, 
				SortOrder = @SortOrder,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				ClientID = @ClientID,
				YearStartDate = @YearStartDate,
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
				IsUAD = @IsUAD,
				IsCirc = @IsCirc
		WHERE PubID = @PubID
		SELECT @PubID;
	END
	ELSE
	BEGIN
		INSERT INTO Pubs (PubName, istradeshow, PubCode, PubTypeID, GroupID, EnableSearching, score, SortOrder, DateCreated, CreatedByUserID, ClientID, YearStartDate, YearEndDate, IssueDate, IsImported, IsActive, AllowDataEntry, FrequencyID, KMImportAllowed, ClientImportAllowed, AddRemoveAllowed, AcsMailerInfoId, IsUAD, IsCirc)
		VALUES (@PubName, @istradeshow, @PubCode, @PubTypeID, @GroupID, @EnableSearching, @score, @SortOrder, @DateCreated, @CreatedByUserID, @ClientID, @YearStartDate, @YearEndDate, @IssueDate, @IsImported, @IsActive, @AllowDataEntry, @FrequencyID, @KMImportAllowed, @ClientImportAllowed, @AddRemoveAllowed, @AcsMailerInfoId, @IsUAD, @IsCirc)
		SELECT @@IDENTITY
	END
GO
PRINT N'Altering [dbo].[e_SubscriberArchive_SaveBulkInsert]...';


GO

ALTER PROCEDURE [dbo].[e_SubscriberArchive_SaveBulkInsert]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriberArchiveID int,[SFRecordIdentifier] [uniqueidentifier] NOT NULL, [SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
	[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
	[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
	[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [datetime] NULL,[QDate] [datetime] NULL,
	[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,
	[Demo31] [bit] NULL,[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
	[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
	[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
	[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
	[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
	[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
	[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,SARecordIdentifier [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50) NOT NULL, ImportRowNumber int,IsActive BIT NULL
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberArchiveID,SFRecordIdentifier, SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SARecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	)  
	
	SELECT 
		SubscriberArchiveID,SFRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SARecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM OPENXML(@docHandle, N'/XML/SubscriberArchive')  
	WITH   
	(
	    SubscriberArchiveID int 'SubscriberArchiveID',
	    SFRecordIdentifier uniqueidentifier 'SFRecordIdentifier',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate datetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(1) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(1) 'Par3C',
		Demo31 bit 'Demo31',
		Demo32 bit 'Demo32',
		Demo33 bit 'Demo33',
		Demo34 bit 'Demo34',
		Demo35 bit 'Demo35',
		Demo36 bit 'Demo36',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No int 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		SARecordIdentifier uniqueidentifier 'SARecordIdentifier',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		ImportRowNumber int 'ImportRowNumber',
		IsActive BIT 'IsActive'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    

--------------------SubscriberDemographicArchive
	DECLARE @sdtDocHandle int

    declare @sdtInsertcount int
    
	DECLARE @sdaImport TABLE    
	(  
		SDArchiveID int, PubID int, SARecordIdentifier uniqueidentifier, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberArchiveRecordIdentifier [uniqueidentifier]
	)  
	--CREATE NONCLUSTERED INDEX [IDX_RecordIdentifier] ON #sdtImport (SubscriberInvalidRecordIdentifier ASC)
	
	EXEC sp_xml_preparedocument @sdtDocHandle OUTPUT, @xml
	
	insert into @sdaImport 
	(
		 SDArchiveID,PubID,SARecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberArchiveRecordIdentifier
	)  
	
	SELECT 
		SDArchiveID,PubID,SARecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberArchiveRecordIdentifier
	FROM OPENXML(@sdtDocHandle, N'/XML/SubscriberArchive/DemographicArchiveList/SubscriberDemographicArchive')  
	WITH   
	(  
		SDArchiveID int 'SDArchiveID',
		PubID int 'PubID',
		SARecordIdentifier uniqueidentifier 'SARecordIdentifier',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberArchiveRecordIdentifier uniqueidentifier 'SubscriberArchiveRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdtDocHandle   


-------do inserts
	DECLARE @saIDs table (SARecordIdentifier uniqueidentifier, SFRecordIdentifier uniqueidentifier)
	
	INSERT INTO SubscriberArchive (SFRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SARecordIdentifier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive)
			OUTPUT Inserted.SARecordIdentifier, Inserted.SFRecordIdentifier 
			INTO @saIDs
	SELECT SFRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SARecordIdentifier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM @import
	
	UPDATE @sdaImport 
	SET SARecordIdentifier = x.SARecordIdentifier
	FROM @saIDs x WHERE SubscriberArchiveRecordIdentifier = x.SARecordIdentifier
	
	INSERT INTO SubscriberDemographicArchive (PubID,SARecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	SELECT PubID,SARecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID
	FROM @sdaImport
GO
PRINT N'Altering [dbo].[e_SubscriberFinal_Save]...';


GO
ALTER PROCEDURE [e_SubscriberFinal_Save]
@SubscriberFinalID int,
@STRecordIdentifier uniqueidentifier,
@SourceFileID int,
@PubCode varchar(100),
@Sequence int,
@FName varchar(100),
@LName varchar(100),
@Title varchar(100),
@Company varchar(100),
@Address varchar(255),
@MailStop varchar(255),
@City varchar(50),
@State varchar(50),
@Zip varchar(10),
@Plus4 varchar(4),
@ForZip varchar(50),
@County varchar(20),
@Country varchar(100),
@CountryID int,
@Phone varchar(100),
@PhoneExists bit,
@Fax varchar(100),
@FaxExists bit,
@Email varchar(100),
@EmailExists bit,
@CategoryID int,
@TransactionID int,
@TransactionDate smalldatetime,
@QDate datetime,
@QSourceID int,
@RegCode varchar(5),
@Verified varchar(1),
@SubSrc varchar(8),
@OrigsSrc varchar(8),
@Par3C varchar(1),
@Demo31 bit,
@Demo32 bit,
@Demo33 bit,
@Demo34 bit,
@Demo35 bit,
@Demo36 bit,
@Source varchar(50),
@Priority varchar(4),
@IGrp_No uniqueidentifier,
@IGrp_Cnt int,
@CGrp_No int,
@CGrp_Cnt int,
@StatList bit,
@Sic varchar(8),
@SicCode varchar(20),
@Gender varchar(1024),
@IGrp_Rank varchar(2),
@CGrp_Rank varchar(2),
@Address3 varchar(255),
@Home_Work_Address varchar(10),
@PubIDs varchar(2000),
@Demo7 varchar(1),
@IsExcluded bit,
@Mobile varchar(30),
@Latitude decimal(18, 15),
@Longitude decimal(18, 15),
@IsLatLonValid bit,
@LatLonMsg nvarchar(500),
@Score int,
@EmailStatusID int,
@StatusUpdatedDate datetime,
@StatusUpdatedReason nvarchar(200),
@IsMailable bit,
@Ignore bit,
@IsDQMProcessFinished bit,
@DQMProcessDate datetime,
@IsUpdatedInLive bit,
@UpdateInLiveDate datetime,
@SFRecordIdentifier uniqueidentifier,
@ProcessCode varchar(50),
@ImportRowNumber int,
@IsActive bit = 'true'
AS
	IF EXISTS(SELECT SubscriberFinalID FROM SubscriberFinal with(nolock) WHERE SubscriberFinalID = @SubscriberFinalID)
		BEGIN
			IF @CategoryID = 0
			BEGIN
				SET @CategoryID = 10
			END
			IF @TransactionID = 0
			BEGIN
				SET @TransactionID = 10
			END

			UPDATE SubscriberFinal
			SET
				STRecordIdentifier = @STRecordIdentifier,
				PubCode = @PubCode,
				Sequence = @Sequence,
				FName = @FName,
				LName = @LName,
				Title = @Title,
				Company = @Company,
				Address = @Address,
				MailStop = @MailStop,
				City = @City,
				State = @State,
				Zip = @Zip,
				Plus4 = @Plus4,
				ForZip = @ForZip,
				County = @County,
				Country = @Country,
				CountryID = @CountryID,
				Phone = @Phone,
				PhoneExists = @PhoneExists,
				Fax = @Fax,
				FaxExists = @FaxExists,
				Email = @Email,
				EmailExists = @EmailExists,
				CategoryID = @CategoryID,
				TransactionID = @TransactionID,
				TransactionDate = @TransactionDate,
				QDate = @QDate,
				QSourceID = @QSourceID,
				RegCode = @RegCode,
				Verified = @Verified,
				SubSrc = @SubSrc,
				OrigsSrc = @OrigsSrc,
				Par3C = @Par3C,
				Demo31 = @Demo31,
				Demo32 = @Demo32,
				Demo33 = @Demo33,
				Demo34 = @Demo34,
				Demo35 = @Demo35,
				Demo36 = @Demo36,
				Source = @Source,
				Priority = @Priority,
				IGrp_No = @IGrp_No,
				IGrp_Cnt = @IGrp_Cnt,
				CGrp_No = @CGrp_No,
				CGrp_Cnt = @CGrp_Cnt,
				StatList = @StatList,
				Sic = @Sic,
				SicCode = @SicCode,
				Gender = @Gender,
				IGrp_Rank = @IGrp_Rank,
				CGrp_Rank = @CGrp_Rank,
				Address3 = @Address3,
				Home_Work_Address = @Home_Work_Address,
				PubIDs = @PubIDs,
				Demo7 = @Demo7,
				IsExcluded = @IsExcluded,
				Mobile = @Mobile,
				Latitude = @Latitude,
				Longitude = @Longitude,
				IsLatLonValid = @IsLatLonValid,
				LatLonMsg = @LatLonMsg,
				Score = @Score,
				EmailStatusID = @EmailStatusID,
				StatusUpdatedDate = @StatusUpdatedDate,
				StatusUpdatedReason = @StatusUpdatedReason,
				IsMailable = @IsMailable,
				Ignore = @Ignore,
				IsDQMProcessFinished = @IsDQMProcessFinished,
				DQMProcessDate = @DQMProcessDate,
				IsUpdatedInLive = @IsUpdatedInLive,
				UpdateInLiveDate = @UpdateInLiveDate,
				SFRecordIdentifier = @SFRecordIdentifier,
				ProcessCode = @ProcessCode,
				ImportRowNumber = @ImportRowNumber,
				DateUpdated = GETDATE(),
				IsActive = @IsActive
			WHERE SubscriberFinalID = @SubscriberFinalID
		END 
	ELSE
		BEGIN
			INSERT INTO SubscriberFinal (STRecordIdentifier,[SourceFileID],[PubCode],[Sequence],[FName],[LName],[Title],[Company],[Address],[MailStop],[City],[State],[Zip],[Plus4]
           ,[ForZip],[County],[Country],[CountryID],[Phone],[PhoneExists],[Fax],[FaxExists],[Email],[EmailExists],[CategoryID],[TransactionID],[TransactionDate],[QDate]
           ,[QSourceID],[RegCode],[Verified],[SubSrc],[OrigsSrc],[Par3C],[Demo31],[Demo32],[Demo33],[Demo34],[Demo35],[Demo36],[Source],[Priority],[IGrp_No],[IGrp_Cnt]
           ,[CGrp_No],[CGrp_Cnt],[StatList],[Sic],[SicCode],[Gender],[IGrp_Rank],[CGrp_Rank],[Address3],[Home_Work_Address],[PubIDs],[Demo7],[IsExcluded],[Mobile],[Latitude]
           ,[Longitude],[IsLatLonValid],[LatLonMsg],[Score],[EmailStatusID],[StatusUpdatedDate],[StatusUpdatedReason],[IsMailable]
           ,[Ignore],[IsDQMProcessFinished],[DQMProcessDate],[IsUpdatedInLive],[UpdateInLiveDate],SFRecordIdentifier,ProcessCode,ImportRowNumber,IsActive)
           
			VALUES(@STRecordIdentifier,@SourceFileID,@PubCode,@Sequence,@FName,@LName,@Title,@Company,@Address,@MailStop,@City,@State,@Zip,@Plus4,@ForZip,@County,@Country,@CountryID,@Phone,
					@PhoneExists,@Fax,@FaxExists,@Email,@EmailExists,@CategoryID,@TransactionID,@TransactionDate,@QDate,@QSourceID,@RegCode,@Verified,@SubSrc,@OrigsSrc,@Par3C,@Demo31,@Demo32,@Demo33,
					@Demo34,@Demo35,@Demo36,@Source,@Priority,@IGrp_No,@IGrp_Cnt,@CGrp_No,@CGrp_Cnt,@StatList,@Sic,@SicCode,@Gender,@IGrp_Rank,@CGrp_Rank,@Address3,@Home_Work_Address,@PubIDs,@Demo7,
					@IsExcluded,@Mobile,@Latitude,@Longitude,@IsLatLonValid,@LatLonMsg,@Score,@EmailStatusID,@StatusUpdatedDate,@StatusUpdatedReason,@IsMailable,@Ignore,
					@IsDQMProcessFinished,@DQMProcessDate,@IsUpdatedInLive,@UpdateInLiveDate,@SFRecordIdentifier,@ProcessCode,@ImportRowNumber,@IsActive);SELECT @@IDENTITY;
		END
GO
PRINT N'Altering [dbo].[e_SubscriberFinal_SaveBulkInsert]...';


GO
ALTER PROCEDURE [e_SubscriberFinal_SaveBulkInsert]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriberFinalID int,STRecordIdentifier uniqueidentifier, [SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
	[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
	[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
	[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [smalldatetime] NULL,[QDate] [datetime] NULL,
	[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,
	[Demo31] [bit] NULL,[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
	[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
	[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
	[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
	[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
	[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
	[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,[SFRecordIndentfier] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50) NOT NULL, ImportRowNumber int NULL, IsActive bit NULL
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberFinalID, STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIndentfier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	)  
	
	SELECT 
		 SubscriberFinalID, STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIndentfier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM OPENXML(@docHandle, N'/XML/SubscriberFinal')   
	WITH   
	(  
		SubscriberFinalID int 'SubscriberFinalID',
		STRecordIdentifier uniqueidentifier 'STRecordIdentifier',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate smalldatetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(1) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(1) 'Par3C',
		Demo31 bit 'Demo31',
		Demo32 bit 'Demo32',
		Demo33 bit 'Demo33',
		Demo34 bit 'Demo34',
		Demo35 bit 'Demo35',
		Demo36 bit 'Demo36',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No int 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		SFRecordIndentfier uniqueidentifier 'SFRecordIndentfier',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		ImportRowNumber int 'ImportRowNumber',
		IsActive BIT 'IsActive'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    
	
	--------------------SubscriberDemographicFinal
	DECLARE @sdfDocHandle int

    declare @sdfInsertcount int
    
	DECLARE @sdfImport TABLE    
	(  
		SDFinalID int,SFRecordIdentifier uniqueidentifier, PubID int, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberFinalRecordIdentifier [uniqueidentifier]
	)  
	--CREATE NONCLUSTERED INDEX [IDX_RecordIdentifier] ON #sdfImport (SubscriberFinalRecordIdentifier ASC)

	EXEC sp_xml_preparedocument @sdfDocHandle OUTPUT, @xml
	
	insert into @sdfImport 
	(
		 SDFinalID,SFRecordIdentifier,PubID,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberFinalRecordIdentifier
	)  
	
	SELECT 
		SDFinalID,SFRecordIdentifier,PubID,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberFinalRecordIdentifier
	FROM OPENXML(@sdfDocHandle, N'/XML/SubscriberFinal/DemographicFinalList/SubscriberDemographicFinal')  
	WITH   
	(
		SDFinalID int 'SDFinalID', 
		SFRecordIdentifier uniqueidentifier 'SFRecordIdentifier', 
		PubID int 'PubID',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberFinalRecordIdentifier uniqueidentifier 'SubscriberFinalRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdfDocHandle   


-------do inserts
	DECLARE @sfIDs table (SFRecordIdentifier uniqueidentifier, STRecordIdentifier uniqueidentifier)
	
	INSERT INTO SubscriberFinal (STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SFRecordIdentifier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive)
			OUTPUT Inserted.SFRecordIdentifier, Inserted.STRecordIdentifier 
			INTO @sfIDs
	SELECT STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SFRecordIndentfier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM @import
	
	UPDATE @sdfImport 
	SET SFRecordIdentifier = x.SFRecordIdentifier
	FROM @sfIDs x WHERE SubscriberFinalRecordIdentifier = x.SFRecordIdentifier
	
	INSERT INTO SubscriberDemographicFinal (SFRecordIdentifier,PubID,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	SELECT SFRecordIdentifier,PubID,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID
	FROM @sdfImport
	
	--DROP table #import
	--DROP table #sdfImport

	UPDATE SubscriberFinal
	SET TransactionID = 10
	WHERE TransactionID = 0
	
	UPDATE SubscriberFinal
	SET CategoryID = 10
	WHERE CategoryID = 0
GO
PRINT N'Altering [dbo].[e_SubscriberFinal_SaveBulkUpdate]...';


GO

ALTER PROCEDURE [e_SubscriberFinal_SaveBulkUpdate]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		tmpImportID int IDENTITY(1,1), 
		SubscriberFinalID int,
		[STRecordIdentifier] [uniqueidentifier] NOT NULL,[SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
	[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
	[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
	[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [smalldatetime] NULL,[QDate] [datetime] NULL,
	[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,
	[Demo31] [bit] NULL,[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
	[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
	[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
	[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
	[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
	[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
	[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,SFRecordIdentifier [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50) NOT NULL,ImportRowNumber int NULL,IsActive BIT NULL
	)  

	--CREATE INDEX EA_1 on #import (SubscriberFinalID)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberFinalID,STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	)  
	
	SELECT 
		 SubscriberFinalID,STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM OPENXML(@docHandle, N'/XML/SubscriberFinal')   
	WITH   
	(  
		SubscriberFinalID int 'SubscriberFinalID',
		STRecordIdentifier uniqueidentifier 'STRecordIdentifier',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate smalldatetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(1) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(1) 'Par3C',
		Demo31 bit 'Demo31',
		Demo32 bit 'Demo32',
		Demo33 bit 'Demo33',
		Demo34 bit 'Demo34',
		Demo35 bit 'Demo35',
		Demo36 bit 'Demo36',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No int 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		SFRecordIdentifier uniqueidentifier 'SFRecordIdentifier',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		ImportRowNumber int 'ImportRowNumber',
		IsActive BIT 'IsActive'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    
	
	--------------------SubscriberDemographicFinal
	DECLARE @sdfDocHandle int

    declare @sdfInsertcount int
    
	DECLARE @sdfImport TABLE    
	(  
		SDFinalID int,PubID int, SFRecordIdentifier uniqueidentifier, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberOriginalRecordIdentifier [uniqueidentifier]
	)  

	EXEC sp_xml_preparedocument @sdfDocHandle OUTPUT, @xml
	
	insert into @sdfImport 
	(
		 SDFinalID,PubID,SFRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	)  
	
	SELECT 
		SDFinalID,PubID,SFRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	FROM OPENXML(@sdfDocHandle, N'/XML/SubscriberFinal/DemographicFinalList/SubscriberDemographicFinal')  
	WITH   
	(
		SDFinalID int 'SDFinalID',  
		PubID int 'PubID',
		SFRecordIdentifier uniqueidentifier 'SFRecordIdentifier',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberOriginalRecordIdentifier uniqueidentifier 'SubscriberOriginalRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdfDocHandle
	
	
	
	UPDATE SubscriberFinal
	SET
		STRecordIdentifier = i.STRecordIdentifier,
		SourceFileID = i.SourceFileID,
		PubCode = i.PubCode,
		Sequence = i.Sequence,
		FName = i.FName,
		LName = i.LName,
		Title = i.Title,
		Company = i.Company,
		Address = i.Address,
		MailStop = i.MailStop,
		City = i.City,
		State = i.State,
		Zip = i.Zip,
		Plus4 = i.Plus4,
		ForZip = i.ForZip,
		County = i.County,
		Country = i.Country,
		CountryID = i.CountryID,
		Phone = i.Phone,
		PhoneExists = i.PhoneExists,
		Fax = i.Fax,
		FaxExists = i.FaxExists,
		Email = i.Email,
		EmailExists = i.EmailExists,
		CategoryID = i.CategoryID,
		TransactionID = i.TransactionID,
		TransactionDate = i.TransactionDate,
		QDate = i.QDate,
		QSourceID = i.QSourceID,
		RegCode = i.RegCode,
		Verified = i.Verified,
		SubSrc = i.SubSrc,
		OrigsSrc = i.OrigsSrc,
		Par3C = i.Par3C,
		Demo31 = i.Demo31,
		Demo32 = i.Demo32,
		Demo33 = i.Demo33,
		Demo34= i.Demo34,
		Demo35 = i.Demo35,
		Demo36 = i.Demo36,
		Source = i.Source,
		Priority = i.Priority,
		IGrp_No = i.IGrp_No,
		IGrp_Cnt = i.IGrp_Cnt,
		CGrp_No = i.CGrp_No,
		CGrp_Cnt = i.CGrp_Cnt,
		StatList = i.StatList,
		Sic = i.Sic,
		SicCode = i.SicCode,
		Gender = i.Gender,
		IGrp_Rank = i.IGrp_Rank,
		CGrp_Rank = i.CGrp_Rank,
		Address3 = i.Address3,
		Home_Work_Address = i.Home_Work_Address,
		PubIDs = i.PubIDs,
		Demo7 = i.Demo7,
		IsExcluded = i.IsExcluded,
		Mobile = i.Mobile,
		Latitude = i.Latitude,
		Longitude = i.Longitude,
		IsLatLonValid = i.IsLatLonValid,
		LatLonMsg = i.LatLonMsg,
		Score = i.Score,
		EmailStatusID = i.EmailStatusID,
		StatusUpdatedDate = i.StatusUpdatedDate,
		StatusUpdatedReason = i.StatusUpdatedReason,
		IsMailable = i.IsMailable,
		Ignore = i.Ignore,
		IsDQMProcessFinished = i.IsDQMProcessFinished,
		DQMProcessDate = i.DQMProcessDate,
		IsUpdatedInLive = i.IsUpdatedInLive,
		UpdateInLiveDate = i.UpdateInLiveDate,
		SFRecordIdentifier = i.SFRecordIdentifier,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1,
		ImportRowNumber = i.ImportRowNumber,
		ProcessCode = i.ProcessCode,
		IsActive = i.IsActive
	FROM @import i
	WHERE SubscriberFinal.SubscriberFinalID = i.SubscriberFinalID


	UPDATE SubscriberDemographicFinal
	SET 
		PubID = i.PubID,
		MAFField = i.MAFField,
		Value = i.Value,
		NotExists = i.NotExists,
		DateCreated = i.DateCreated,
		DateUpdated = i.DateUpdated,
		CreatedByUserID = i.CreatedByUserID,
		UpdatedByUserID = i.UpdatedByUserID
	FROM @sdfImport i
	WHERE i.SDFinalID = SubscriberDemographicFinal.SDFinalID
	
	--DROP table #import
	--DROP table #sdfImport

	UPDATE SubscriberFinal
	SET TransactionID = 10
	WHERE TransactionID = 0
	
	UPDATE SubscriberFinal
	SET CategoryID = 10
	WHERE CategoryID = 0
GO
PRINT N'Altering [dbo].[e_SubscriberFinal_SaveDQMClean]...';


GO
ALTER PROCEDURE [e_SubscriberFinal_SaveDQMClean]
@ProcessCode varchar(50)
AS
	exec e_SubscriberFinal_DisableIndexes
	exec e_SubscriberDemographicFinal_DisableIndexes

	INSERT INTO SubscriberFinal 
	(
		 STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,IsMailable,ProcessCode,ImportRowNumber,IsActive
	)  
	SELECT 
		 st.STRecordIdentifier,st.SourceFileID,st.PubCode,st.Sequence,st.FName,st.LName,st.Title,st.Company,st.Address,st.MailStop,st.City,st.State,st.Zip,st.Plus4,st.ForZip,st.County,st.Country,st.CountryID,st.Phone,st.PhoneExists,
		 st.Fax,st.FaxExists,st.Email,st.EmailExists,st.CategoryID,st.TransactionID,st.TransactionDate,st.QDate,st.QSourceID,st.RegCode,st.Verified,st.SubSrc,st.OrigsSrc,st.Par3C,st.Demo31,st.Demo32,st.Demo33,st.Demo34,st.Demo35,st.Demo36,st.Source,
		 st.Priority,st.IGrp_No,st.IGrp_Cnt,st.CGrp_No,st.CGrp_Cnt,st.StatList,st.Sic,st.SicCode,st.Gender,st.IGrp_Rank,st.CGrp_Rank,st.Address3,st.Home_Work_Address,st.PubIDs,st.Demo7,st.IsExcluded,st.Mobile,st.Latitude,st.Longitude,st.IsLatLonValid,
		 st.LatLonMsg,st.Score,st.EmailStatusID,st.StatusUpdatedDate,st.StatusUpdatedReason,st.Ignore,st.IsDQMProcessFinished,st.DQMProcessDate,st.IsUpdatedInLive,
		 st.UpdateInLiveDate, NEWID() AS SFRecordIdentifier, st.DateCreated,st.DateUpdated,st.CreatedByUserID,st.UpdatedByUserID,st.IsMailable,st.ProcessCode,st.ImportRowNumber,st.IsActive
	FROM SubscriberTransformed st With(NoLock)
	LEFT OUTER JOIN  SubscriberFinal sf with(NoLock) on st.STRecordIdentifier = sf.STRecordIdentifier
	WHERE st.ProcessCode = @ProcessCode 
	AND sf.SFRecordIdentifier is null

	--Insert non-duplicate records into subscriberDemographicFinal table
	INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	
	SELECT sdt.PubID,sf.SFRecordIdentifier,sdt.MAFField,sdt.Value,sdt.NotExists,sdt.DateCreated,sdt.DateUpdated,sdt.CreatedByUserID,sdt.UpdatedByUserID
	FROM SubscriberFinal sf  with(nolock)
	JOIN SubscriberDemographicTransformed sdt with(nolock) ON sdt.STRecordIdentifier = sf.STRecordIdentifier
	WHERE sf.ProcessCode = @ProcessCode 
			AND sdt.NotExists = 'false'

	exec e_SubscriberFinal_EnableIndexes
	exec e_SubscriberDemographicFinal_EnableIndexes

	UPDATE SubscriberFinal
	SET TransactionID = 10
	WHERE ProcessCode = @ProcessCode 
			AND (TransactionID = 0 OR TransactionID NOT IN (SELECT TransactionID FROM [Transaction]))
	
	UPDATE SubscriberFinal
	SET CategoryID = 10
	WHERE ProcessCode = @ProcessCode 
			AND (CategoryID = 0 OR CategoryID NOT IN (SELECT CategoryID FROM CATEGORY))
GO
PRINT N'Altering [dbo].[e_SubscriberInvalid_SaveBulkInsert]...';


GO
ALTER PROCEDURE e_SubscriberInvalid_SaveBulkInsert
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriberInvalidID int,[SORecordIdentifier] [uniqueidentifier] NOT NULL, [SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
	[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
	[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
	[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [datetime] NULL,[QDate] [datetime] NULL,
	[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,
	[Demo31] [bit] NULL,[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
	[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
	[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
	[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
	[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
	[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
	[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,SIRecordIdentifier [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50) NOT NULL, ImportRowNumber int NULL,IsActive BIT NULL
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberInvalidID,SORecordIdentifier, SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SIRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	)  
	
	SELECT 
		SubscriberInvalidID,SORecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SIRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM OPENXML(@docHandle, N'/XML/SubscriberInvalid')  
	WITH   
	(
	    SubscriberInvalidID int 'SubscriberInvalidID',
	    SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate datetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(1) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(1) 'Par3C',
		Demo31 bit 'Demo31',
		Demo32 bit 'Demo32',
		Demo33 bit 'Demo33',
		Demo34 bit 'Demo34',
		Demo35 bit 'Demo35',
		Demo36 bit 'Demo36',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No int 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		SIRecordIdentifier uniqueidentifier 'SIRecordIdentifier',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		ImportRowNumber int 'ImportRowNumber',
		IsActive BIT 'IsActive'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    

--------------------SubscriberDemographicOriginal
	DECLARE @sdtDocHandle int

    declare @sdtInsertcount int
    
	DECLARE @sdtImport TABLE   
	(  
		SDInvalidID int, PubID int, SORecordIdentifier uniqueidentifier, SIRecordIdentifier uniqueidentifier, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberInvalidRecordIdentifier [uniqueidentifier]
	)  
	--CREATE NONCLUSTERED INDEX [IDX_RecordIdentifier] ON #sdtImport (SubscriberInvalidRecordIdentifier ASC)

	EXEC sp_xml_preparedocument @sdtDocHandle OUTPUT, @xml
	
	insert into @sdtImport 
	(
		 SDInvalidID,PubID,SORecordIdentifier,SIRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberInvalidRecordIdentifier
	)  
	
	SELECT 
		SDInvalidID,PubID,SORecordIdentifier,SIRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberInvalidRecordIdentifier
	FROM OPENXML(@sdtDocHandle, N'/XML/SubscriberInvalid/DemographicInvalidList/SubscriberDemographicInvalid')  
	WITH   
	(  
		SDInvalidID int 'SDInvalidID',
		PubID int 'PubID',
		SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		SIRecordIdentifier uniqueidentifier 'SIRecordIdentifier',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberInvalidRecordIdentifier uniqueidentifier 'SubscriberInvalidRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdtDocHandle   


-------do inserts
	DECLARE @stIDs table (SIRecordIdentifier uniqueidentifier, SORecordIdentifier uniqueidentifier)
	
	INSERT INTO SubscriberInvalid (SORecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SIRecordIdentifier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive)
			OUTPUT Inserted.SIRecordIdentifier, Inserted.SORecordIdentifier 
			INTO @stIDs
	SELECT SORecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SIRecordIdentifier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM @import
	
	UPDATE @sdtImport 
	SET SIRecordIdentifier = x.SIRecordIdentifier
	FROM @stIDs x WHERE SubscriberInvalidRecordIdentifier = x.SIRecordIdentifier
	
	INSERT INTO SubscriberDemographicInvalid (PubID,SORecordIdentifier,SIRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	SELECT PubID,SORecordIdentifier,SIRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID
	FROM @sdtImport
GO
PRINT N'Altering [dbo].[e_SubscriberOriginal_Save]...';


GO
ALTER PROCEDURE [e_SubscriberOriginal_Save]
@SubscriberOriginalID int,
@SourceFileID int,
@PubCode varchar(100),
@Sequence int,
@FName varchar(100),
@LName varchar(100),
@Title varchar(100),
@Company varchar(100),
@Address varchar(255),
@MailStop varchar(255),
@City varchar(50),
@State varchar(50),
@Zip varchar(10),
@Plus4 varchar(4),
@ForZip varchar(50),
@County varchar(20),
@Country varchar(100),
@CountryID int,
@Phone varchar(100),
@PhoneExists bit,
@Fax varchar(100),
@FaxExists bit,
@Email varchar(100),
@EmailExists bit,
@CategoryID int,
@TransactionID int,
@TransactionDate smalldatetime,
@QDate datetime,
@QSourceID int,
@RegCode varchar(5),
@Verified varchar(1),
@SubSrc varchar(8),
@OrigsSrc varchar(8),
@Par3C varchar(1),
@Demo31 bit,
@Demo32 bit,
@Demo33 bit,
@Demo34 bit,
@Demo35 bit,
@Demo36 bit,
@Source varchar(50),
@Priority varchar(4),
@IGrp_No uniqueidentifier,
@IGrp_Cnt int,
@CGrp_No int,
@CGrp_Cnt int,
@StatList bit,
@Sic varchar(8),
@SicCode varchar(20),
@Gender varchar(1024),
@IGrp_Rank varchar(2),
@CGrp_Rank varchar(2),
@Address3 varchar(255),
@Home_Work_Address varchar(10),
@PubIDs varchar(2000),
@Demo7 varchar(1),
@IsExcluded bit,
@Mobile varchar(30),
@Latitude decimal(18, 15),
@Longitude decimal(18, 15),
@IsLatLonValid bit,
@LatLonMsg nvarchar(500),
@Score int,
@EmailStatusID int,
@StatusUpdatedDate datetime,
@StatusUpdatedReason nvarchar(200),
@IsMailable bit,
@Ignore bit,
@IsDQMProcessFinished bit,
@DQMProcessDate datetime,
@IsUpdatedInLive bit,
@UpdateInLiveDate datetime,
@SORecordIdentifier uniqueidentifier,
@ImportRowNumber int,
@ProcessCode varchar(50),
@IsActive BIT = 'true'
AS
	IF EXISTS(SELECT SORecordIdentifier FROM SubscriberOriginal with(nolock) WHERE SORecordIdentifier = @SORecordIdentifier)
		BEGIN
			UPDATE SubscriberOriginal
			SET
				SourceFileID = @SourceFileID,
				PubCode = @PubCode,
				Sequence = @Sequence,
				FName = @FName,
				LName = @LName,
				Title = @Title,
				Company = @Company,
				Address = @Address,
				MailStop = @MailStop,
				City = @City,
				State = @State,
				Zip = @Zip,
				Plus4 = @Plus4,
				ForZip = @ForZip,
				County = @County,
				Country = @Country,
				CountryID = @CountryID,
				Phone = @Phone,
				PhoneExists = @PhoneExists,
				Fax = @Fax,
				FaxExists = @FaxExists,
				Email = @Email,
				EmailExists = @EmailExists,
				CategoryID = @CategoryID,
				TransactionID = @TransactionID,
				TransactionDate = @TransactionDate,
				QDate = @QDate,
				QSourceID = @QSourceID,
				RegCode = @RegCode,
				Verified = @Verified,
				SubSrc = @SubSrc,
				OrigsSrc = @OrigsSrc,
				Par3C = @Par3C,
				Demo31 = @Demo31,
				Demo32 = @Demo32,
				Demo33 = @Demo33,
				Demo34 = @Demo34,
				Demo35 = @Demo35,
				Demo36 = @Demo36,
				Source = @Source,
				Priority = @Priority,
				IGrp_No = @IGrp_No,
				IGrp_Cnt = @IGrp_Cnt,
				CGrp_No = @CGrp_No,
				CGrp_Cnt = @CGrp_Cnt,
				StatList = @StatList,
				Sic = @Sic,
				SicCode = @SicCode,
				Gender = @Gender,
				IGrp_Rank = @IGrp_Rank,
				CGrp_Rank = @CGrp_Rank,
				Address3 = @Address3,
				Home_Work_Address = @Home_Work_Address,
				PubIDs = @PubIDs,
				Demo7 = @Demo7,
				IsExcluded = @IsExcluded,
				Mobile = @Mobile,
				Latitude = @Latitude,
				Longitude = @Longitude,
				IsLatLonValid = @IsLatLonValid,
				LatLonMsg = @LatLonMsg,
				Score = @Score,
				EmailStatusID = @EmailStatusID,
				StatusUpdatedDate = @StatusUpdatedDate,
				StatusUpdatedReason = @StatusUpdatedReason,
				IsMailable = @IsMailable,
				Ignore = @Ignore,
				IsDQMProcessFinished = @IsDQMProcessFinished,
				DQMProcessDate = @DQMProcessDate,
				IsUpdatedInLive = @IsUpdatedInLive,
				UpdateInLiveDate = @UpdateInLiveDate,
				ImportRowNumber = @ImportRowNumber,
				ProcessCode = @ProcessCode,
				DateUpdated = GETDATE(),
				IsActive = @IsActive
			WHERE SORecordIdentifier = @SORecordIdentifier
		END 
	ELSE
		BEGIN
			INSERT INTO SubscriberOriginal ([SourceFileID],[PubCode],[Sequence],[FName],[LName],[Title],[Company],[Address],[MailStop],[City],[State],[Zip],[Plus4]
           ,[ForZip],[County],[Country],[CountryID],[Phone],[PhoneExists],[Fax],[FaxExists],[Email],[EmailExists],[CategoryID],[TransactionID],[TransactionDate],[QDate]
           ,[QSourceID],[RegCode],[Verified],[SubSrc],[OrigsSrc],[Par3C],[Demo31],[Demo32],[Demo33],[Demo34],[Demo35],[Demo36],[Source],[Priority],[IGrp_No],[IGrp_Cnt]
           ,[CGrp_No],[CGrp_Cnt],[StatList],[Sic],[SicCode],[Gender],[IGrp_Rank],[CGrp_Rank],[Address3],[Home_Work_Address],[PubIDs],[Demo7],[IsExcluded],[Mobile],[Latitude]
           ,[Longitude],[IsLatLonValid],[LatLonMsg],[Score],[EmailStatusID],[StatusUpdatedDate],[StatusUpdatedReason],[IsMailable]
           ,[Ignore],[IsDQMProcessFinished],[DQMProcessDate],[IsUpdatedInLive],[UpdateInLiveDate],SORecordIdentifier,ImportRowNumber,ProcessCode,IsActive)
           
			VALUES(@SourceFileID,@PubCode,@Sequence,@FName,@LName,@Title,@Company,@Address,@MailStop,@City,@State,@Zip,@Plus4,@ForZip,@County,@Country,@CountryID,@Phone,
					@PhoneExists,@Fax,@FaxExists,@Email,@EmailExists,@CategoryID,@TransactionID,@TransactionDate,@QDate,@QSourceID,@RegCode,@Verified,@SubSrc,@OrigsSrc,@Par3C,@Demo31,@Demo32,@Demo33,
					@Demo34,@Demo35,@Demo36,@Source,@Priority,@IGrp_No,@IGrp_Cnt,@CGrp_No,@CGrp_Cnt,@StatList,@Sic,@SicCode,@Gender,@IGrp_Rank,@CGrp_Rank,@Address3,@Home_Work_Address,@PubIDs,@Demo7,
					@IsExcluded,@Mobile,@Latitude,@Longitude,@IsLatLonValid,@LatLonMsg,@Score,@EmailStatusID,@StatusUpdatedDate,@StatusUpdatedReason,@IsMailable,@Ignore,
					@IsDQMProcessFinished,@DQMProcessDate,@IsUpdatedInLive,@UpdateInLiveDate,@SORecordIdentifier,@ImportRowNumber,@ProcessCode,@IsActive);SELECT @@IDENTITY;
		END
GO
PRINT N'Altering [dbo].[e_SubscriberOriginal_SaveBulkInsert]...';


GO

ALTER PROCEDURE [e_SubscriberOriginal_SaveBulkInsert]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriberOriginalID int, [SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
	[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
	[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
	[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [datetime] NULL,[QDate] [datetime] NULL,
	[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,
	[Demo31] [bit] NULL,[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
	[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
	[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
	[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
	[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
	[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
	[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,SORecordIdentifier [uniqueidentifier] NOT NULL, ImportRowNumber int NULL,
	[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50),IsActive BIT null
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberOriginalID,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SORecordIdentifier,ImportRowNumber,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,IsActive
	)  
	
	SELECT 
		SubscriberOriginalID,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SORecordIdentifier,ImportRowNumber,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,IsActive
	FROM OPENXML(@docHandle, N'/XML/SubscriberOriginal') --SubscriberOriginal  
	WITH   
	(  
		SubscriberOriginalID int 'SubscriberOriginalID',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate datetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(1) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(1) 'Par3C',
		Demo31 bit 'Demo31',
		Demo32 bit 'Demo32',
		Demo33 bit 'Demo33',
		Demo34 bit 'Demo34',
		Demo35 bit 'Demo35',
		Demo36 bit 'Demo36',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No int 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		ImportRowNumber int 'ImportRowNumber',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		IsActive BIT 'IsActive'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    

--------------------SubscriberDemographicOriginal
	DECLARE @sdoDocHandle int

    declare @sdoInsertcount int
    
	DECLARE @sdoImport TABLE    
	(  
		SDOriginalID int, PubID int, SORecordIdentifier uniqueidentifier, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberOriginalRecordIdentifier [uniqueidentifier]
	)  
	--CREATE NONCLUSTERED INDEX [IDX_RecordIdentifier] ON #sdoImport (SubscriberOriginalRecordIdentifier ASC)

	EXEC sp_xml_preparedocument @sdoDocHandle OUTPUT, @xml
	
	insert into @sdoImport 
	(
		 SDOriginalID,PubID,SORecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	)  
	
	SELECT 
		SDOriginalID,PubID,SORecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	FROM OPENXML(@sdoDocHandle, N'/XML/SubscriberOriginal/DemographicOriginalList/SubscriberDemographicOriginal') --SubscriberOriginal  
	WITH   
	(  
		SDOriginalID int 'SDOriginalID',
		PubID int 'PubID',
		SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberOriginalRecordIdentifier uniqueidentifier 'SubscriberOriginalRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdoDocHandle   


-------do inserts
	DECLARE @soIDs TABLE (SubscriberOriginalID int, SORecordIdentifier uniqueidentifier)
	--CREATE UNIQUE NONCLUSTERED INDEX [IDX_RecordIdentifier] ON #soIDs (RecordIdentifier ASC)

	INSERT INTO SubscriberOriginal (SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SORecordIdentifier,ImportRowNumber,DateCreated,CreatedByUserID,ProcessCode,IsActive)
			OUTPUT Inserted.SubscriberOriginalID, Inserted.SORecordIdentifier 
			INTO @soIDs
	SELECT SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									SORecordIdentifier,ImportRowNumber,DateCreated,CreatedByUserID,ProcessCode,IsActive
	FROM @import
	
	UPDATE @sdoImport 
	SET SORecordIdentifier = x.SORecordIdentifier
	FROM @soIDs x WHERE SubscriberOriginalRecordIdentifier = x.SORecordIdentifier
	
	INSERT INTO SubscriberDemographicOriginal (PubID,SORecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	SELECT PubID,SORecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID
	FROM @sdoImport
GO
PRINT N'Altering [dbo].[e_SubscriberOriginal_SaveBulkUpdate]...';


GO
ALTER PROCEDURE [e_SubscriberOriginal_SaveBulkUpdate]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		tmpImportID int IDENTITY(1,1), 
		[SubscriberOriginalID] [int] NOT NULL,[SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
	[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
	[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
	[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [smalldatetime] NULL,[QDate] [datetime] NULL,
	[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,
	[Demo31] [bit] NULL,[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
	[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
	[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
	[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
	[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
	[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
	[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,SORecordIdentifier [uniqueidentifier] NOT NULL,ImportRowNumber int null,
	[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50),IsActive BIT NULL
	)  

	--CREATE INDEX EA_1 on #import (SubscriberOriginalID)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberOriginalID,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SORecordIdentifier,ImportRowNumber,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,IsActive
	)  
	
	SELECT 
		SubscriberOriginalID,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SORecordIdentifier,ImportRowNumber,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,IsActive
	FROM OPENXML(@docHandle, N'/XML/SubscriberOrginal')   
	WITH   
	(  
		SubscriberOriginalID int 'SubscriberOriginalID',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate smalldatetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(1) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(1) 'Par3C',
		Demo31 bit 'Demo31',
		Demo32 bit 'Demo32',
		Demo33 bit 'Demo33',
		Demo34 bit 'Demo34',
		Demo35 bit 'Demo35',
		Demo36 bit 'Demo36',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No int 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		ImportRowNumber int 'ImportRowNumber',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		IsActive BIT 'IsActive'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    
	
	
	--------------------SubscriberDemographicOriginal
	DECLARE @sdoDocHandle int

    declare @sdoInsertcount int
    
	DECLARE @sdoImport TABLE   
	(  
		SDOriginalID int, PubID int, SORecordIdentifier uniqueidentifier, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberOriginalRecordIdentifier [uniqueidentifier]
	)  

	EXEC sp_xml_preparedocument @sdoDocHandle OUTPUT, @xml
	
	insert into @sdoImport 
	(
		 SDOriginalID,PubID,SORecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	)  
	
	SELECT 
		SDOriginalID,PubID,SORecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	FROM OPENXML(@sdoDocHandle, N'/XML/SubscriberOriginal/DemographicOriginalList/SubscriberDemographicOriginal') --SubscriberOriginal  
	WITH   
	(  
		SDOriginalID int 'SDOriginalID',
		PubID int 'PubID',
		SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberOriginalRecordIdentifier uniqueidentifier 'SubscriberOriginalRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdoDocHandle
	
	UPDATE SubscriberOriginal
	SET
		SourceFileID = i.SourceFileID,
		PubCode = i.PubCode,
		Sequence = i.Sequence,
		FName = i.FName,
		LName = i.LName,
		Title = i.Title,
		Company = i.Company,
		Address = i.Address,
		MailStop = i.MailStop,
		City = i.City,
		State = i.State,
		Zip = i.Zip,
		Plus4 = i.Plus4,
		ForZip = i.ForZip,
		County = i.County,
		Country = i.Country,
		CountryID = i.CountryID,
		Phone = i.Phone,
		PhoneExists = i.PhoneExists,
		Fax = i.Fax,
		FaxExists = i.FaxExists,
		Email = i.Email,
		EmailExists = i.EmailExists,
		CategoryID = i.CategoryID,
		TransactionID = i.TransactionID,
		TransactionDate = i.TransactionDate,
		QDate = i.QDate,
		QSourceID = i.QSourceID,
		RegCode = i.RegCode,
		Verified = i.Verified,
		SubSrc = i.SubSrc,
		OrigsSrc = i.OrigsSrc,
		Par3C = i.Par3C,
		Demo31 = i.Demo31,
		Demo32 = i.Demo32,
		Demo33 = i.Demo33,
		Demo34= i.Demo34,
		Demo35 = i.Demo35,
		Demo36 = i.Demo36,
		Source = i.Source,
		Priority = i.Priority,
		IGrp_No = i.IGrp_No,
		IGrp_Cnt = i.IGrp_Cnt,
		CGrp_No = i.CGrp_No,
		CGrp_Cnt = i.CGrp_Cnt,
		StatList = i.StatList,
		Sic = i.Sic,
		SicCode = i.SicCode,
		Gender = i.Gender,
		IGrp_Rank = i.IGrp_Rank,
		CGrp_Rank = i.CGrp_Rank,
		Address3 = i.Address3,
		Home_Work_Address = i.Home_Work_Address,
		PubIDs = i.PubIDs,
		Demo7 = i.Demo7,
		IsExcluded = i.IsExcluded,
		Mobile = i.Mobile,
		Latitude = i.Latitude,
		Longitude = i.Longitude,
		IsLatLonValid = i.IsLatLonValid,
		LatLonMsg = i.LatLonMsg,
		Score = i.Score,
		EmailStatusID = i.EmailStatusID,
		StatusUpdatedDate = i.StatusUpdatedDate,
		StatusUpdatedReason = i.StatusUpdatedReason,
		IsMailable = i.IsMailable,
		Ignore = i.Ignore,
		IsDQMProcessFinished = i.IsDQMProcessFinished,
		DQMProcessDate = i.DQMProcessDate,
		IsUpdatedInLive = i.IsUpdatedInLive,
		UpdateInLiveDate = i.UpdateInLiveDate,
		SORecordIdentifier = i.SORecordIdentifier,
		ImportRowNumber = i.ImportRowNumber,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1,
		ProcessCode = i.ProcessCode,
		IsActive = i.IsActive
	FROM @import i
	WHERE SubscriberOriginal.SubscriberOriginalID = i.SubscriberOriginalID
	
	UPDATE SubscriberDemographicOriginal
	SET PubID = i.PubID,
		SORecordIdentifier = i.SORecordIdentifier,
		MAFField = i.MAFField,
		Value = i.Value,
		NotExists = i.NotExists,
		DateCreated = i.DateCreated,
		DateUpdated = i.DateUpdated,
		CreatedByUserID = i.CreatedByUserID,
		UpdatedByUserID = i.UpdatedByUserID
	FROM @sdoImport i
	WHERE i.SDOriginalID = SubscriberDemographicOriginal.SDOriginalID
GO
PRINT N'Altering [dbo].[e_SubscriberTransformed_AddressValidation_Paging]...';


GO

ALTER PROCEDURE e_SubscriberTransformed_AddressValidation_Paging
@CurrentPage int, 
@PageSize int,
@ProcessCode varchar(50) = '',
@SourceFileID int = 0,
@IsLatLonValid bit = 'false'
AS
	-- The number of rows affected by the different commands-- does not interest the application, so turn NOCOUNT ON
	SET NOCOUNT ON
	-- Determine the first record and last record 
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);

	CREATE TABLE #TempResult 
	(
		RowNum int,
		[SubscriberTransformedID] [int] NOT NULL, [SORecordIdentifier] [uniqueidentifier] NOT NULL, [SourceFileID] [int] NOT NULL, 
		[PubCode] [varchar](100) NULL, [Sequence] [int] NOT NULL, [FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,
		[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](50) NULL,[Plus4] [varchar](50) NULL,
		[ForZip] [varchar](50) NULL,[County] [varchar](100) NULL,[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,
		[FaxExists] [bit] NULL,[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [datetime] NULL,[QDate] [datetime] NULL,
		[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,[Demo31] [bit] NULL,
		[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,[IGrp_No] [uniqueidentifier] NULL,
		[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,[Gender] [varchar](1024) NULL,
		[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,
		[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,
		[Score] [int] NULL,[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,
		[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,
		[UpdateInLiveDate] [datetime] NULL,[STRecordIdentifier] [uniqueidentifier] NOT NULL,[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,
		[UpdatedByUserID] [datetime] NULL,ProcessCode varchar(50) NOT NULL, ImportRowNumber int NULL, IsActive bit null
	)

	IF LEN(@ProcessCode) > 0
		BEGIN

			INSERT INTO #TempResult
			SELECT ROW_NUMBER() OVER(ORDER BY st.SubscriberTransformedID ASC) as 'RowNum'
			  ,st.*
			FROM SubscriberTransformed st WITH (NOLOCK)
			WHERE ProcessCode = @ProcessCode
			AND SourceFileID = @SourceFileID
			AND IsLatLonValid = @IsLatLonValid
		END
	ELSE
		BEGIN
			IF @SourceFileID > 0
				BEGIN
					INSERT INTO #TempResult
					SELECT ROW_NUMBER() OVER(ORDER BY st.SubscriberTransformedID ASC) as 'RowNum'
					  ,st.*
					FROM SubscriberTransformed st WITH (NOLOCK)
					WHERE SourceFileID = @SourceFileID
					AND IsLatLonValid = @IsLatLonValid
				END
			ELSE
				BEGIN
					INSERT INTO #TempResult
					SELECT ROW_NUMBER() OVER(ORDER BY st.SubscriberTransformedID ASC) as 'RowNum'
					  ,st.*
					FROM SubscriberTransformed st WITH (NOLOCK)
					WHERE IsLatLonValid = @IsLatLonValid
				END
		END

	------------------Return results
	SELECT top (@LastRec-1) *
	FROM #TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
	-- Turn NOCOUNT back OFF
	SET NOCOUNT OFF
	DROP TABLE #TempResult
GO
PRINT N'Altering [dbo].[e_SubscriberTransformed_Save]...';


GO
ALTER PROCEDURE [dbo].[e_SubscriberTransformed_Save]
@SubscriberTransformedID int,
@SORecordIdentifier uniqueidentifier,
@SourceFileID int,
@PubCode varchar(100),
@Sequence int,
@FName varchar(100),
@LName varchar(100),
@Title varchar(100),
@Company varchar(100),
@Address varchar(255),
@MailStop varchar(255),
@City varchar(50),
@State varchar(50),
@Zip varchar(10),
@Plus4 varchar(4),
@ForZip varchar(50),
@County varchar(20),
@Country varchar(100),
@CountryID int,
@Phone varchar(100),
@PhoneExists bit,
@Fax varchar(100),
@FaxExists bit,
@Email varchar(100),
@EmailExists bit,
@CategoryID int,
@TransactionID int,
@TransactionDate smalldatetime,
@QDate datetime,
@QSourceID int,
@RegCode varchar(5),
@Verified varchar(1),
@SubSrc varchar(8),
@OrigsSrc varchar(8),
@Par3C varchar(1),
@Demo31 bit,
@Demo32 bit,
@Demo33 bit,
@Demo34 bit,
@Demo35 bit,
@Demo36 bit,
@Source varchar(50),
@Priority varchar(4),
@IGrp_No uniqueidentifier,
@IGrp_Cnt int,
@CGrp_No int,
@CGrp_Cnt int,
@StatList bit,
@Sic varchar(8),
@SicCode varchar(20),
@Gender varchar(1024),
@IGrp_Rank varchar(2),
@CGrp_Rank varchar(2),
@Address3 varchar(255),
@Home_Work_Address varchar(10),
@PubIDs varchar(2000),
@Demo7 varchar(1),
@IsExcluded bit,
@Mobile varchar(30),
@Latitude decimal(18, 15),
@Longitude decimal(18, 15),
@IsLatLonValid bit,
@LatLonMsg nvarchar(500),
@Score int,
@EmailStatusID int,
@StatusUpdatedDate datetime,
@StatusUpdatedReason nvarchar(200),
@IsMailable bit,
@Ignore bit,
@IsDQMProcessFinished bit,
@DQMProcessDate datetime,
@IsUpdatedInLive bit,
@UpdateInLiveDate datetime,
@STRecordIdentifier uniqueidentifier,
@ProcessCode varchar(50),
@ImportRowNumber int,
@IsActive BIT = 'true'
AS
	IF EXISTS(SELECT SubscriberTransformedID FROM SubscriberTransformed with(nolock) WHERE SubscriberTransformedID = @SubscriberTransformedID)
		BEGIN
			UPDATE SubscriberTransformed
			SET
				SORecordIdentifier = @SORecordIdentifier,
				SourceFileID = @SourceFileID,
				PubCode = @PubCode,
				Sequence = @Sequence,
				FName = @FName,
				LName = @LName,
				Title = @Title,
				Company = @Company,
				Address = @Address,
				MailStop = @MailStop,
				City = @City,
				State = @State,
				Zip = @Zip,
				Plus4 = @Plus4,
				ForZip = @ForZip,
				County = @County,
				Country = @Country,
				CountryID = @CountryID,
				Phone = @Phone,
				PhoneExists = @PhoneExists,
				Fax = @Fax,
				FaxExists = @FaxExists,
				Email = @Email,
				EmailExists = @EmailExists,
				CategoryID = @CategoryID,
				TransactionID = @TransactionID,
				TransactionDate = @TransactionDate,
				QDate = @QDate,
				QSourceID = @QSourceID,
				RegCode = @RegCode,
				Verified = @Verified,
				SubSrc = @SubSrc,
				OrigsSrc = @OrigsSrc,
				Par3C = @Par3C,
				Demo31 = @Demo31,
				Demo32 = @Demo32,
				Demo33 = @Demo33,
				Demo34 = @Demo34,
				Demo35 = @Demo35,
				Demo36 = @Demo36,
				Source = @Source,
				Priority = @Priority,
				IGrp_No = @IGrp_No,
				IGrp_Cnt = @IGrp_Cnt,
				CGrp_No = @CGrp_No,
				CGrp_Cnt = @CGrp_Cnt,
				StatList = @StatList,
				Sic = @Sic,
				SicCode = @SicCode,
				Gender = @Gender,
				IGrp_Rank = @IGrp_Rank,
				CGrp_Rank = @CGrp_Rank,
				Address3 = @Address3,
				Home_Work_Address = @Home_Work_Address,
				PubIDs = @PubIDs,
				Demo7 = @Demo7,
				IsExcluded = @IsExcluded,
				Mobile = @Mobile,
				Latitude = @Latitude,
				Longitude = @Longitude,
				IsLatLonValid = @IsLatLonValid,
				LatLonMsg = @LatLonMsg,
				Score = @Score,
				EmailStatusID = @EmailStatusID,
				StatusUpdatedDate = @StatusUpdatedDate,
				StatusUpdatedReason = @StatusUpdatedReason,
				IsMailable = @IsMailable,
				Ignore = @Ignore,
				IsDQMProcessFinished = @IsDQMProcessFinished,
				DQMProcessDate = @DQMProcessDate,
				IsUpdatedInLive = @IsUpdatedInLive,
				UpdateInLiveDate = @UpdateInLiveDate,
				STRecordIdentifier = @STRecordIdentifier,
				ProcessCode = @ProcessCode,
				ImportRowNumber = @ImportRowNumber,
				DateUpdated = GETDATE(),
				IsActive = @IsActive
			WHERE SubscriberTransformedID = @SubscriberTransformedID
		END 
	ELSE
		BEGIN
			INSERT INTO SubscriberTransformed ([SORecordIdentifier],[SourceFileID],[PubCode],[Sequence],[FName],[LName],[Title],[Company],[Address],[MailStop],[City],[State],[Zip],[Plus4]
           ,[ForZip],[County],[Country],[CountryID],[Phone],[PhoneExists],[Fax],[FaxExists],[Email],[EmailExists],[CategoryID],[TransactionID],[TransactionDate],[QDate]
           ,[QSourceID],[RegCode],[Verified],[SubSrc],[OrigsSrc],[Par3C],[Demo31],[Demo32],[Demo33],[Demo34],[Demo35],[Demo36],[Source],[Priority],[IGrp_No],[IGrp_Cnt]
           ,[CGrp_No],[CGrp_Cnt],[StatList],[Sic],[SicCode],[Gender],[IGrp_Rank],[CGrp_Rank],[Address3],[Home_Work_Address],[PubIDs],[Demo7],[IsExcluded],[Mobile],[Latitude]
           ,[Longitude],[IsLatLonValid],[LatLonMsg],[Score],[EmailStatusID],[StatusUpdatedDate],[StatusUpdatedReason],[IsMailable]
           ,[Ignore],[IsDQMProcessFinished],[DQMProcessDate],[IsUpdatedInLive],[UpdateInLiveDate],STRecordIdentifier,ProcessCode,ImportRowNumber,IsActive)
           
			VALUES(@SORecordIdentifier,@SourceFileID,@PubCode,@Sequence,@FName,@LName,@Title,@Company,@Address,@MailStop,@City,@State,@Zip,@Plus4,@ForZip,@County,@Country,@CountryID,@Phone,
					@PhoneExists,@Fax,@FaxExists,@Email,@EmailExists,@CategoryID,@TransactionID,@TransactionDate,@QDate,@QSourceID,@RegCode,@Verified,@SubSrc,@OrigsSrc,@Par3C,@Demo31,@Demo32,@Demo33,
					@Demo34,@Demo35,@Demo36,@Source,@Priority,@IGrp_No,@IGrp_Cnt,@CGrp_No,@CGrp_Cnt,@StatList,@Sic,@SicCode,@Gender,@IGrp_Rank,@CGrp_Rank,@Address3,@Home_Work_Address,@PubIDs,@Demo7,
					@IsExcluded,@Mobile,@Latitude,@Longitude,@IsLatLonValid,@LatLonMsg,@Score,@EmailStatusID,@StatusUpdatedDate,@StatusUpdatedReason,(case when isnull(@Address,'')!='' then 'true' else @IsMailable end),@Ignore,
					@IsDQMProcessFinished,@DQMProcessDate,@IsUpdatedInLive,@UpdateInLiveDate,@STRecordIdentifier,@ProcessCode,@ImportRowNumber,@IsActive);SELECT @@IDENTITY;
		END
GO
PRINT N'Altering [dbo].[e_SubscriberTransformed_SaveBulkInsert]...';


GO
ALTER PROCEDURE [e_SubscriberTransformed_SaveBulkInsert]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriberTransformedID int,[SORecordIdentifier] [uniqueidentifier] NOT NULL, [SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
	[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
	[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
	[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [datetime] NULL,[QDate] [datetime] NULL,
	[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](1) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,
	[Demo31] [bit] NULL,[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
	[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
	[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
	[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
	[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
	[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
	[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,STRecordIdentifier [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50), ImportRowNumber int,IsActive BIT NULL
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberTransformedID,SORecordIdentifier, SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,STRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	)  
	
	SELECT 
		SubscriberTransformedID,SORecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,STRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM OPENXML(@docHandle, N'/XML/SubscriberTransformed') --SubscriberTransformed  
	WITH   
	(
	    SubscriberTransformedID int 'SubscriberTransformedID',
	    SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate datetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(1) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(1) 'Par3C',
		Demo31 bit 'Demo31',
		Demo32 bit 'Demo32',
		Demo33 bit 'Demo33',
		Demo34 bit 'Demo34',
		Demo35 bit 'Demo35',
		Demo36 bit 'Demo36',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No int 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		STRecordIdentifier uniqueidentifier 'STRecordIdentifier',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		ImportRowNumber int 'ImportRowNumber',
		IsActive BIT 'IsActive'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    

--------------------SubscriberDemographicOriginal
	DECLARE @sdtDocHandle int

    declare @sdtInsertcount int
    
	DECLARE @sdtImport TABLE    
	(  
		SubscriberDemographicTransformedID int, PubID int, SORecordIdentifier uniqueidentifier, STRecordIdentifier uniqueidentifier, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberTransformedRecordIdentifier [uniqueidentifier]
	)  
	--CREATE NONCLUSTERED INDEX [IDX_RecordIdentifier] ON #sdtImport (SubscriberTransformedRecordIdentifier ASC)

	EXEC sp_xml_preparedocument @sdtDocHandle OUTPUT, @xml
	
	insert into @sdtImport 
	(
		 SubscriberDemographicTransformedID,PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberTransformedRecordIdentifier
	)  
	
	SELECT 
		SubscriberDemographicTransformedID,PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberTransformedRecordIdentifier
	FROM OPENXML(@sdtDocHandle, N'/XML/SubscriberTransformed/DemographicTransformedList/SubscriberDemographicTransformed') --SubscriberTransformed  
	WITH   
	(  
		SubscriberDemographicTransformedID int 'SubscriberDemographicTransformedID',
		PubID int 'PubID',
		SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		STRecordIdentifier uniqueidentifier 'STRecordIdentifier',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberTransformedRecordIdentifier uniqueidentifier 'SubscriberTransformedRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdtDocHandle   


-------do inserts
	DECLARE @stIDs TABLE (STRecordIdentifier uniqueidentifier, SORecordIdentifier uniqueidentifier)
	--CREATE UNIQUE NONCLUSTERED INDEX [IDX_RecordIdentifier] ON #stIDs (RecordIdentifier ASC)

	INSERT INTO SubscriberTransformed (SORecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									STRecordIdentifier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive)
			OUTPUT Inserted.STRecordIdentifier, Inserted.SORecordIdentifier 
			INTO @stIDs
	SELECT SORecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,
									Phone,PhoneExists,Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,
									Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,
									CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,Score,
									EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,(case when isnull(Address,'')!='' then 'true' else IsMailable end) as IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,UpdateInLiveDate,
									STRecordIdentifier,DateCreated,CreatedByUserID,ProcessCode,ImportRowNumber,IsActive
	FROM @import
	
	UPDATE @sdtImport 
	SET STRecordIdentifier = x.STRecordIdentifier
	FROM @stIDs x WHERE SubscriberTransformedRecordIdentifier = x.STRecordIdentifier
	
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	SELECT PubID,STRecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID
	FROM @sdtImport
GO
PRINT N'Altering [dbo].[job_IssueComp_IssueCompDetail_Add]...';


GO
ALTER PROCEDURE [dbo].[job_IssueComp_IssueCompDetail_Add]
	@ProcessCode varchar(50),
	@PublicationID int,
	@SourceFileId int
AS
BEGIN	
	DECLARE @distinctComps TABLE (CompName VARCHAR(200), CompCount int, IssueID int)
	DECLARE @PublisherID INT = (SELECT PublisherID FROM Circulation..Publication WHERE PublicationID = @PublicationID)
	DECLARE @PubCode varchar(100) = (Select PubCode FROM Pubs where PubID = @PublicationID)

	--
	-- Temporary patch for records with ascii characters in email field, need to remove when fix is implemented
	--
	UPDATE SubscriberFinal
	SET Email =  ''  
	WHERE LEN(LTRIM(rtrim(email))) > 0 and LEN(LTRIM(RTRIM(email))) <= 4 and SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode 

	PRINT ('UPDATE SubscriberFinal / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	--
	-- Begin CREATE TABLE
	--
	CREATE TABLE #MatchCirc(
		SFRecordIdentifier UNIQUEIDENTIFIER,indNameAddress UNIQUEIDENTIFIER,indNameEmail UNIQUEIDENTIFIER,indNameCompany UNIQUEIDENTIFIER,indNamePhone UNIQUEIDENTIFIER
		,indNameNotBlankEmail UNIQUEIDENTIFIER,indDistEmail UNIQUEIDENTIFIER,grpNameAddress UNIQUEIDENTIFIER,grpNameEmail UNIQUEIDENTIFIER,grpNameCompany UNIQUEIDENTIFIER
		,grpNamePhone UNIQUEIDENTIFIER,grpDistEmail UNIQUEIDENTIFIER,IsOriginal bit,Igrp_no UNIQUEIDENTIFIER,FName varchar(100),LName varchar(100),Company varchar(100),Address varchar(255)
		,State varchar(50),zip varchar(50),Phone varchar(100),Email varchar(100),FName3 varchar(3),LName6 varchar(6),Address15 varchar(15),Company8 varchar(8),Title8 varchar(8))

	CREATE CLUSTERED INDEX idx_MatchGroups_SFRecordIdentifier ON #MatchCirc(SFRecordIdentifier)

	CREATE INDEX idx_MatchGroups ON #MatchCirc(SFRecordIdentifier,indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,
												grpNamePhone,grpDistEmail,Igrp_no)


	INSERT INTO #MatchCirc(SFRecordIdentifier,FName,LName,Company,Address	,State ,zip ,Phone ,Email ,FName3 ,LName6 ,Address15,Company8,Title8,IsOriginal)
	SELECT distinct SFRecordIdentifier, isnull(FName,''), isnull(LName,''), isnull(Company,''), isnull(Address,'')	, isnull(State,'') , left(isnull(zip,''),5) , isnull(Phone,'') , isnull(Email,'') , 
					left(isnull(FName,''),3) , left(isnull(LName,''),6) , left(isnull(Address,''),15), left(isnull(Company,''),8 ), LEFT(isnull(Title,''),8),1
	FROM SubscriberFinal WITH(NOLOCK) 
	WHERE SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode 
	
	PRINT ('INSERT INTO #MatchCirc / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	--
	-- Split Names with special characters
	--	
		
	CREATE TABLE #NameSplit (SFRecordIdentifier uniqueidentifier, SplitChar varchar(256))
		
	INSERT INTO #NameSplit
	SELECT SFRecordIdentifier,f.items AS fnameSplit
	FROM SubscriberFinal
	CROSS APPLY MASTER.dbo.fn_GetSpecialChar(fname) AS f
	UNION
	SELECT SFRecordIdentifier,l.items AS lnameSplit
	FROM SubscriberFinal		
	CROSS APPLY MASTER.dbo.fn_GetSpecialChar(lname) AS l
		
	INSERT INTO #MatchCirc(SFRecordIdentifier,FName,LName,Company,Address,zip,Phone,Email,FName3,LName6,Address15,Company8,Title8,IsOriginal)
	SELECT DISTINCT sf.SFRecordIdentifier,MASTER.dbo.fn_StripNonAlphaNumerics(a.items),MASTER.dbo.fn_StripNonAlphaNumerics(b.items),Company,Address,Zip,Phone,Email,MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(a.items,3)),
					MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(b.items,6)),LEFT(sf.Address,15),LEFT(sf.Company,8),LEFT(sf.Title,8),0
	FROM SubscriberFinal sf INNER JOIN #NameSplit ns ON sf.SFRecordIdentifier = ns.SFRecordIdentifier
	CROSS APPLY MASTER.dbo.fn_split(sf.FName,ns.SplitChar) AS a
	CROSS APPLY MASTER.dbo.fn_split(sf.LName,ns.SplitChar) AS b
	WHERE ISNULL(master.dbo.fn_StripNonAlphaNumerics(a.items),'')!='' AND ISNULL(MASTER.dbo.fn_StripNonAlphaNumerics(b.items),'')!=''
	ORDER BY sf.SFRecordIdentifier
		
	PRINT ('INSERT NAME SPLIT INTO #MatchCirc / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	-- Insert into #MatchCirc with characters stripped out
	INSERT INTO #MatchCirc(SFRecordIdentifier,FName,LName,Company,Address,zip,Phone,Email,FName3,LName6,Address15,Company8,Title8,IsOriginal)
	SELECT DISTINCT sf.SFRecordIdentifier,MASTER.dbo.fn_StripNonAlphaNumerics(sf.fname),MASTER.dbo.fn_StripNonAlphaNumerics(sf.lname),Company,Address,Zip,Phone,Email,MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(sf.fname,3)),
			MASTER.dbo.fn_StripNonAlphaNumerics(LEFT(sf.Lname,6)),LEFT(sf.Address,15),LEFT(sf.Company,8),LEFT(sf.Title,8),0
	FROM SubscriberFinal sf INNER JOIN #NameSplit ns ON sf.SFRecordIdentifier = ns.SFRecordIdentifier
	WHERE ISNULL(sf.Fname,'')!='' AND ISNULL(sf.LName,'')!=''	
		
	PRINT ('INSERT SPECIAL CHAR REMOVED INTO #MatchCirc / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #NameSplit
		
	--
	-------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Begin Data Match
		
	--
	-- Fname,Lname and Address match on individual level
	--
	UPDATE mg
	SET indNameAddress = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND Address15 = LEFT(s.Address1,15) AND mg.Zip = left(s.ZipCode,5)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.address15 != '' AND mg.zip != '' and ss.PublicationID = @PublicationID
		
	--Swap
	UPDATE mg
	SET indNameAddress = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON mg.FName3 = LEFT(s.LastName,3) AND mg.LName6 = LEFT(s.FirstName,6) AND mg.Address15 = LEFT(s.Address1,15) AND mg.Zip = left(s.ZipCode,5)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.address15 != '' AND mg.zip != '' AND mg.indNameAddress is null and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 1 : Fname,Lname and Address match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
	
  	--
	-- Fname,Lname and Email match on individual level
	--
	UPDATE mg
	SET indNameEmail = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON mg.Email = s.EMAIL AND FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Email != '' and ss.PublicationID = @PublicationID 

	-- Swap
	UPDATE mg
	SET indNameEmail = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON mg.Email = s.EMAIL AND FName3 = LEFT(s.Lastname,3) AND LName6 = LEFT(s.FirstName,6)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Email != '' and mg.indNameEmail is null and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 2 : Fname,Lname and Email match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
					  
  	--
	-- Fname,Lname and Company match on individual level
	--
	UPDATE mg
	SET indNameCompany = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND  Company8 = LEFT(s.Company,8)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND Company8 != '' and ss.PublicationID = @PublicationID
		
	-- Swap
	UPDATE mg
	SET indNameCompany = s.igrp_no
	FROM #MatchCirc mg 
		INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.Lastname,3) AND LName6 = LEFT(s.FirstName,6) AND  Company8 = LEFT(s.Company,8)
		INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND Company8 != '' and mg.indNameCompany is null and ss.PublicationID = @PublicationID

	PRINT ('After Step 3 : Fname,Lname and Company match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  	--
	-- Fname,Lname and Phone match on individual level
	--
	UPDATE mg
	SET indNamePhone = s.igrp_no
	FROM #MatchCirc mg 
	INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND mg.PHone = s.Phone
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Phone != '' and ss.PublicationID = @PublicationID
		
	-- Swap
	UPDATE mg
	SET indNamePhone = s.igrp_no
	FROM #MatchCirc mg 
	INNER JOIN Circulation..Subscriber s ON FName3 = LEFT(s.firstname,3) AND LName6 = LEFT(s.LastName,6) AND mg.PHone = s.Phone
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE (FName3 != '' OR LName6 != '') AND mg.Phone != '' AND mg.indNamePhone is null and ss.PublicationID = @PublicationID

	PRINT ('After Step 4 : Fname,Lname and Phone match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
			  
	--
	-- Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank
	--
	UPDATE mg
	SET indNameNotBlankEmail = s.Igrp_no
	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Email = s.email		
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID					 
	WHERE mg.Email != '' AND (mg.FName != '' OR mg.lname != '')
	AND ISNULL(s.Email,'') != '' AND ISNULL(s.firstname,'') = '' AND ISNULL(s.lastname,'') = '' and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 5 : Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	--
	-- Email not blank and Fname OR Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above
	--
	UPDATE mg
	SET indNameNotBlankEmail = s.Igrp_no
	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Email = s.email INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
	WHERE  mg.Email != '' AND mg.FName = '' AND mg.lname = ''
	AND ISNULL(s.Email,'') != '' AND (ISNULL(s.firstname,'') != '' OR ISNULL(s.lastname,'') != '') and ss.PublicationID = @PublicationID
		
	PRINT ('After Step 6 : Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	--
	-- Distinct Email Source file does not have Fname Or LName match on Subscriptions
	--
	UPDATE mg
	SET indDistEmail = s.Igrp_no
	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Email = s.Email INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID			 
	WHERE mg.Email != '' AND mg.fname = '' AND mg.lname = '' and ss.PublicationID = @PublicationID

 	PRINT ('After Step 7a : Distinct Email Source file does not have Fname Or LName match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
 		
 	-- We are using indDistEmail for this update because the match criteria assumes that this field should not have any values
 	-- FName, LName and Email is Blank. Title and Company not Blank match on Subscriptions
 	UPDATE mg
 	SET indDistEmail = s.Igrp_no
 	FROM #MatchCirc mg INNER JOIN Circulation..Subscriber s on mg.Title8 = LEFT(s.Title,8) AND mg.Company8 = LEFT(s.Company,8)
	INNER JOIN Circulation..Subscription ss on s.SubscriberID = ss.SubscriberID
 	WHERE ISNULL(mg.FName,'') = '' AND ISNULL(mg.LName,'') = '' AND ISNULL(mg.EMAIL,'') = '' AND ISNULL(mg.TITLE8,'') != '' AND ISNULL(mg.Company8,'') != '' and ss.PublicationID = @PublicationID
 		
 	PRINT ('After Step 7b : FName, LName and Email is Blank. Title and Company not Blank match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

    --END INDIVIDUAL MATCHING
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    --START GROUP MATCHING
	--
	-- Name Addres Group Match
	--
	CREATE TABLE #tmpNameAddressGroup (fname VARCHAR(3), lname  VARCHAR(6), address  VARCHAR(15), zip  VARCHAR(5), igrp_no UNIQUEIDENTIFIER)
		
	INSERT INTO #tmpNameAddressGroup
	SELECT Fname3 AS fname,lname6 AS lname,address15 AS 'address',LEFT(ZIP,5) AS ZIP,NEWID() AS igrp_no
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(address15,'') != '' AND ISNULL(zip,'') != '' 
	GROUP BY fname3,lname6,address15,LEFT(ZIP,5)
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpNameAddress = ng.igrp_no
	FROM #MatchCirc mg INNER JOIN #tmpNameAddressGroup ng ON FName3 = ng.fname AND LName6 = ng.lname AND Address15 = ng.[address] and LEFT(mg.Zip,5) = ng.ZIP
	WHERE (FName3 != '' OR LName6 != '') AND Address15 != '' AND ISNULL(mg.zip,'') != ''
	
	PRINT ('After Step 8 : Name Addres Group Match / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNameAddressGroup
	--
	-- Name Email Group Match
	--
	CREATE TABLE #tmpNameEmailGroup (fname VARCHAR(3), lname  VARCHAR(6), Email  VARCHAR(100), igrp_no UNIQUEIDENTIFIER)
		
	INSERT INTO #tmpNameEmailGroup
	SELECT fname3 AS fname,lname6 AS lname,Email,NEWID() AS Igrp_no
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(Email,'') != '' 
	GROUP BY fname3,lname6,Email
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpNameEmail = ng.Igrp_no
	FROM #MatchCirc mg INNER JOIN #tmpNameEmailGroup AS ng ON FName3 = ng.fname AND LName6 = ng.lname AND mg.Email = ng.Email
	WHERE (mg.FName3 != '' OR mg.lname6 != '') AND mg.Email != ''
			
	PRINT ('After Step 9 : Name Email Group Match / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNameEmailGroup
		
	--	
	-- Name Company Group Match	
	--
	CREATE TABLE #tmpNameCompanyGroup (fname VARCHAR(3), lname  VARCHAR(6), Company  VARCHAR(8), igrp_no UNIQUEIDENTIFIER)

	INSERT INTO #tmpNameCompanyGroup		
	SELECT fname3 AS fname,lname6 AS lname,Company8 AS Company,NEWID() AS Igrp_No
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(sf.Company8,'') != ''  
	GROUP BY fname3,lname6,Company8
	HAVING COUNT(*) > 1
										
	UPDATE mg
	SET grpNameCompany = ng.Igrp_No
	FROM #MatchCirc mg INNER JOIN #tmpNameCompanyGroup AS ng ON FName3 = ng.fname AND LName6 = ng.lname AND LEFT(mg.Company,8) = ng.Company
	WHERE (mg.FName3 != '' OR mg.lname6 != '') AND mg.Company8 != ''
				
	PRINT ('After Step 10 : Name Company Group Match	 / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNameCompanyGroup

	--
	-- Name Phone Group Match
	--
		
	CREATE TABLE #tmpNamePhoneGroup (fname VARCHAR(3), lname  VARCHAR(6), Phone  VARCHAR(100), igrp_no UNIQUEIDENTIFIER)
		
	INSERT INTO #tmpNamePhoneGroup		
	SELECT fname3 AS fname ,lname6 AS lname,Phone,NEWID() AS Igrp_No
	FROM #MatchCirc sf WITH(NoLock)
	WHERE (ISNULL(sf.FName3,'') != '' OR ISNULL(sf.lname6,'') != '') AND ISNULL(Phone,'') != '' 			  
	GROUP BY fname3,lname6,Phone
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpNamePhone = ng.Igrp_no
	FROM #MatchCirc mg INNER JOIN #tmpNamePhoneGroup AS ng
									ON FName3 = ng.fname AND LName6 = ng.lname AND mg.Phone = ng.Phone
	WHERE (mg.FName3 != '' OR mg.lname6 != '') AND mg.Phone != ''
		
	PRINT ('After Step 11 : Name Phone Group Match / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmpNamePhoneGroup

	--
	-- No Name Distinct Email Match on its own sourcefile
	--
	CREATE TABLE #NoNameEmail(
		SFRecordIdentifier UNIQUEIDENTIFIER
		,Email VARCHAR(255)
		,EmailIgrp_No UNIQUEIDENTIFIER)

	CREATE INDEX idx_NoNameEMail ON #NoNameEmail(SFRecordIdentifier,Email)

	INSERT INTO #NoNameEmail
	SELECT sf1.SFRecordIdentifier,sf1.Email,null
	FROM #MatchCirc sf1 WITH(NoLock) INNER JOIN #MatchCirc sf2 WITH(NoLock) on sf1.Email = sf2.Email
	WHERE ISNULL(sf1.FName,'') = '' AND ISNULL(sf1.lname,'') = '' AND ISNULL(sf1.Email,'') != ''
		AND (ISNULL(sf2.FName,'') = '' OR ISNULL(sf2.lname,'') = '' OR ISNULL(sf2.FName,'') != '' OR ISNULL(sf2.lname,'') != '') AND ISNULL(sf2.Email,'') != ''
	--GROUP BY sf1.SFRecordIdentifier,sf1.Email
	UNION
	SELECT sf2.SFRecordIdentifier, sf2.Email,null
	FROM #MatchCirc sf1 WITH(NoLock) INNER JOIN #MatchCirc sf2 WITH(NoLock) on sf1.Email = sf2.Email
	WHERE ISNULL(sf1.FName,'') = '' AND ISNULL(sf1.lname,'') = '' AND ISNULL(sf1.Email,'') != ''
		AND (ISNULL(sf2.FName,'') = '' OR ISNULL(sf2.lname,'') = '' OR ISNULL(sf2.FName,'') != '' OR ISNULL(sf2.lname,'') != '') AND ISNULL(sf2.Email,'') != ''

		
	-- Create new temp table and group by email so we can assign a newid()
	CREATE TABLE #EmailGroups (EMAIL VARCHAR(100), groupCount int, EmailIGrp_No UNIQUEIDENTIFIER)
		
	INSERT INTO #EmailGroups
	SELECT EMAIL,COUNT(*) as groupCount,NEWID() as EmailIGrp_No FROM #NoNameEmail GROUP BY Email

	-- Join #EmailGroups back to #NoNameEmail on Email equals Email and update EmailIgrp_No
	UPDATE ne
	SET EmailIGrp_No = e.EmailIGrp_No
	FROM #NoNameEmail ne INNER JOIN #EmailGroups e on ne.Email = e.Email
	WHERE ne.EmailIgrp_No IS NULL and e.groupCount > 1
		
	-- Update #MatchCirc with EmailIgrp_No id
	UPDATE mg
	SET grpDistEmail = ne.EmailIgrp_No
	FROM #MatchCirc mg INNER JOIN #NoNameEmail ne on mg.SFRecordIdentifier = ne.SFRecordIdentifier
	WHERE mg.grpDistEmail IS NULL AND ne.EmailIgrp_No IS NOT NULL
		
	PRINT ('After Step 12a : No Name Distinct Email Match on its own sourcefile / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #NoNameEmail
	DROP TABLE #EmailGroups
		
	-- Match on Title and Company where firstname, lastname and EMAIL is blank
		
	CREATE TABLE #TitleCompany(
			Title VARCHAR(8)
		,Company VARCHAR(8)
		,TitleCompanyIgrp_No UNIQUEIDENTIFIER)
		
	CREATE INDEX #TitleCompany ON #TitleCompany(Title,Company)
		
	INSERT INTO #TitleCompany
	SELECT sf.Title8,sf.Company8,NEWID() 
	FROM #MatchCirc sf WITH(NOLOCK)
	WHERE ISNULL(FName,'') = '' AND ISNULL(LName,'') = '' AND ISNULL(EMAIL,'') = '' AND ISNULL(TITLE8,'') != '' AND ISNULL(Company8,'') != ''
	GROUP BY sf.Title8,sf.Company8
	HAVING COUNT(*) > 1
		
	UPDATE mg
	SET grpDistEmail = tc.TitleCompanyIgrp_No
	FROM #MatchCirc mg INNER JOIN #TitleCompany tc ON mg.Title8 = tc.Title AND mg.Company8 = tc.Company
	WHERE ISNULL(mg.FName,'') = '' AND ISNULL(mg.LName,'') = '' AND ISNULL(mg.EMAIL,'') = '' AND ISNULL(mg.TITLE8,'') != '' AND ISNULL(mg.Company8,'') != ''

	PRINT ('After Step 12b : Match on Title and Company where firstname, lastname and EMAIL is blank / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	DROP TABLE #TitleCompany
		
    --END GROUP MATCH
    ----------------------------------------------------------------------------------------------------------------------------
    --BEGIN ASSIGN IGRP_NO

	--
	-- Create temp table and store grp[ColumnName] fields going down instead of going accross
	--
	CREATE TABLE #Groups(
		SFRecordIdentifier UNIQUEIDENTIFIER,
		GroupMatch UNIQUEIDENTIFIER,
		ColumnMatch VARCHAR(255))

	CREATE INDEX idx_groups ON #groups(SFRecordIdentifier,GroupMatch,ColumnMatch)
	CREATE INDEX idx_groups_GroupMatch ON #groups(GroupMatch)
	CREATE INDEX idx_groups_SFRecordIdentifier ON #groups(SFRecordIdentifier)

	INSERT INTO #Groups

	SELECT SFRecordIdentifier,grpNameAddress AS Groupmatch,'grpNameAddress' AS ColumnMatch FROM #MatchCirc WHERE grpNameAddress IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpNameEmail,'grpNameEmail'AS ColumnMatch FROM #MatchCirc WHERE grpNameEmail IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpNameCompany,'grpNameCompany'AS ColumnMatch FROM #MatchCirc WHERE grpNameCompany IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpNamePhone,'grpNamePhone'AS ColumnMatch FROM #MatchCirc WHERE grpNamePhone IS NOT NULL
	UNION
	SELECT SFRecordIdentifier,grpDistEmail,'grpDistEmail'AS ColumnMatch FROM #MatchCirc WHERE grpDistEmail IS NOT NULL
	ORDER BY SFRecordIdentifier,groupmatch
		
		
	-- Find combinations of SFRecordIdentifier and grp[ColumnName] and assign igrp_no to SFRecordIdentifiers
	--
	DECLARE @SFRecordIdentifier UNIQUEIDENTIFIER
	DECLARE @Sqlstmt VARCHAR(MAX)
	DECLARE @uniqueID UNIQUEIDENTIFIER

	CREATE TABLE #cur_inner_Q1 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)
		
	CREATE INDEX idx_cur_inner_Q1_SFRecordIdentifier ON #cur_inner_Q1(SFRecordIdentifier)
		
	CREATE TABLE #cur_inner_Q2 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)

	CREATE INDEX idx_cur_inner_Q2_GroupMatch ON #cur_inner_Q2(GroupMatch)

	CREATE TABLE #cur_inner_Q3 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)

	CREATE INDEX idx_cur_inner_Q3_SFRecordIdentifier ON #cur_inner_Q3(SFRecordIdentifier)
		
	CREATE TABLE #cur_inner_Q4 (SFRecordIdentifier UNIQUEIDENTIFIER,GroupMatch UNIQUEIDENTIFIER)

	CREATE INDEX idx_cur_inner_Q4_GroupMatch ON #cur_inner_Q4(GroupMatch)

	PRINT ('CURSOR Start / '  +  CONVERT(VARCHAR(20), GETDATE(), 114))

	declare @curRowcount int
		
	select @curRowcount  = count(SFRecordIdentifier)
	FROM #MatchCirc 
	WHERE igrp_no IS NULL AND (grpNameAddress IS NOT NULL OR grpNameEmail IS NOT NULL OR grpNameCompany IS NOT NULL OR grpNamePhone IS NOT NULL OR grpDistEmail IS NOT NULL )

	PRINT ('Cursor ROWCOUNT : '  + Convert(VARCHAR,@curRowcount) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))


	DECLARE c CURSOR
	FOR 
		SELECT SFRecordIdentifier 
		FROM #MatchCirc 
		WHERE igrp_no IS NULL AND (grpNameAddress IS NOT NULL OR grpNameEmail IS NOT NULL OR grpNameCompany IS NOT NULL OR grpNamePhone IS NOT NULL OR grpDistEmail IS NOT NULL )
	OPEN c
	FETCH NEXT FROM c INTO @SFRecordIdentifier

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @uniqueID = NEWID()
			
		Truncate table #cur_inner_Q1
		Truncate table #cur_inner_Q2
		Truncate table #cur_inner_Q3
		Truncate table #cur_inner_Q4
			
		INSERT INTO #cur_inner_Q4
		SELECT SFRecordIdentifier,GroupMatch 
		FROM	#Groups 
		WHERE SFRecordIdentifier = CAST(@SFRecordIdentifier AS VARCHAR(50))

		INSERT INTO 
			#cur_inner_Q3
		SELECT 
			g.SFRecordIdentifier,
			g.GroupMatch
		FROM 
			#Groups g 
			INNER JOIN #cur_inner_Q4 AS g2 ON g.GroupMatch = g2.GroupMatch
		GROUP BY 
			g.SFRecordIdentifier,
			g.GroupMatch

		INSERT INTO 
			#cur_inner_Q2
		SELECT 
			g1.SFRecordIdentifier,
			g1.GroupMatch 
		FROM
			#Groups g1 
			INNER JOIN #cur_inner_Q3 AS dg1 on g1.SFRecordIdentifier = dg1.SFRecordIdentifier
		GROUP BY 
			g1.SFRecordIdentifier,
			g1.GroupMatch

		INSERT INTO 
			#cur_inner_Q1
		SELECT 
			ge.SFRecordIdentifier,
			ge.groupMatch 
		FROM
			#Groups ge 
			INNER JOIN #cur_inner_Q2 dg on ge.GroupMatch = dg.GroupMatch
		GROUP BY 
			ge.SFRecordIdentifier,
			ge.GroupMatch

		UPDATE 
			mg
		SET 
			igrp_no =  CAST(@uniqueID AS VARCHAR(50)) 		
		FROM 
			#MatchCirc mg 
			INNER JOIN #cur_inner_Q1 AS dg3	ON mg.SFRecordIdentifier = DG3.SFRecordIdentifier
		WHERE 
			mg.Igrp_no is null 
			
		FETCH NEXT FROM c INTO @SFRecordIdentifier
	END
	CLOSE c
	DEALLOCATE c
		
	DROP TABLE #cur_inner_Q1
	DROP TABLE #cur_inner_Q2
	DROP TABLE #cur_inner_Q3
	DROP TABLE #cur_inner_Q4
		
	PRINT ('CURSOR END / '   + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	PRINT ('Begin Assign IGRP_NO to matched Subscriptions and matched within its own sourcefile/ '  + CONVERT(VARCHAR(20), GETDATE(), 114))	
	--
	-- Assign Igrp_no to records that matched Subscriptions and matched within it's own sourcefile.  These record will get their igrp_no from the record it matched in Subscriptions
	--
	CREATE TABLE #indGrpIgrpNo_Assign(
			indNameAddress UNIQUEIDENTIFIER
			,indNameEmail UNIQUEIDENTIFIER
			,indNameCompany UNIQUEIDENTIFIER
			,indNamePhone UNIQUEIDENTIFIER
			,indNameNotBlankEmail UNIQUEIDENTIFIER
			,indDistEmail UNIQUEIDENTIFIER
			,grpNameAddress UNIQUEIDENTIFIER
			,grpNameEmail UNIQUEIDENTIFIER
			,grpNameCompany UNIQUEIDENTIFIER
			,grpNamePhone UNIQUEIDENTIFIER
			,grpDistEmail UNIQUEIDENTIFIER
			,Igrp_No UNIQUEIDENTIFIER)
	          
	CREATE INDEX idx_indGrpIgrpNo_Assign ON #indGrpIgrpNo_Assign(indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,grpNamePhone,grpDistEmail,Igrp_no)
	    
	CREATE INDEX idx_Igrp_no ON #indGrpIgrpNo_Assign(Igrp_No)

	--
	-- The only records with a guid in the igrp_no column in #MatchCirc are records that have group matches
	--
	INSERT INTO #indGrpIgrpNo_Assign
	SELECT indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,grpNamePhone,grpDistEmail,Igrp_no 
	FROM #MatchCirc
	WHERE Igrp_no IS NOT NULL AND (indNameEmail IS NOT NULL OR indNameAddress IS NOT NULL OR indNameCompany IS NOT NULL OR indNamePhone IS NOT NULL OR indNameNotBlankEmail IS NOT NULL OR indDistEmail IS NOT NULL)
	GROUP BY indNameAddress,indNameEmail,indNameCompany,indNamePhone,indNameNotBlankEmail,indDistEmail,grpNameAddress,grpNameEmail,grpNameCompany,grpNamePhone,grpDistEmail,Igrp_no
	    

	UPDATE mg
	SET Igrp_no = CASE WHEN tmg.indNameEmail IS NOT NULL THEN tmg.indNameEmail
						WHEN tmg.indNameAddress IS NOT NULL THEN tmg.indNameAddress
						WHEN tmg.indNameCompany IS NOT NULL THEN tmg.indNameCompany
						WHEN tmg.indNamePhone IS NOT NULL THEN tmg.indNamePhone
						WHEN tmg.indNameNotBlankEmail IS NOT NULL THEN tmg.indNameNotBlankEmail
						WHEN tmg.indDistEmail IS NOT NULL THEN tmg.indDistEmail
						ELSE tmg.Igrp_no END
	FROM #MatchCirc mg INNER JOIN #indGrpIgrpNo_Assign tmg ON mg.Igrp_no = tmg.igrp_no
		
	DROP TABLE #indGrpIgrpNo_Assign
		        
	PRINT ('END Assign IGRP_NO to matched Subscriptions and matched within its own sourcefile/ '  + CONVERT(VARCHAR(20), GETDATE(), 114))	       
	--
	-- Assign igrp_no to records without any group match but did match against SubscriberFina
	--
	PRINT ('Begin Assign IGRP_NO / '  + CONVERT(VARCHAR(20), GETDATE(), 114))		
		
	UPDATE #MatchCirc
	SET Igrp_NO = CASE WHEN indNameEmail IS NOT NULL AND Igrp_no IS NULL THEN indNameEmail
						WHEN indNameAddress IS NOT NULL AND Igrp_no IS NULL  THEN indNameAddress
						WHEN indNameCompany IS NOT NULL AND Igrp_no IS NULL  THEN indNameCompany
						WHEN indNamePhone IS NOT NULL AND Igrp_no IS NULL  THEN indNamePhone
						WHEN indNameNotBlankEmail IS NOT NULL AND Igrp_no IS NULL  THEN indNameNotBlankEmail
						WHEN indDistEmail IS NOT NULL AND Igrp_no IS NULL  THEN indDistEmail ELSE Igrp_NO END        
		        
	--
	-- Create igrp_no for any remaining records without an igrp_no
	--
	UPDATE #MatchCirc
	SET Igrp_no = NEWID()
	WHERE Igrp_no is null

	--
	-- Update SubscriberFinal igrp_no
	--
	UPDATE sf
	SET IGRP_NO = mg.Igrp_no
	FROM SubscriberFinal sf INNER JOIN #MatchCirc mg on sf.SFRecordIdentifier = mg.SFRecordIdentifier
	WHERE sf.SourceFileID = @SourceFileID AND sf.ProcessCode = @ProcessCode 

	--
	-- Drop temp tables as they are no longer needed
	--
	DROP TABLE #MatchCirc
	DROP TABLE #Groups

	PRINT ('END ASSIGN IGRP_NO / '  + CONVERT(VARCHAR(20), GETDATE(), 114))


    --END ASSIGN IGRP_NO
    -------------------------------------------------------------------------------------------------------------
    --BEGIN REMOVE INTERNAL DUPLICATES

	PRINT ('BEGIN REMOVE INTERNAL DUPLICATES / '  + CONVERT(VARCHAR(20), GETDATE(), 114))

	CREATE TABLE #tmp_pubcodeGroupDupe_inner(pubcode varchar(100), igrp_no UNIQUEIDENTIFIER)

	CREATE CLUSTERED INDEX IDX_C_tmp_pubcodeGroupDupe_inner ON #tmp_pubcodeGroupDupe_inner(pubcode, igrp_no)

	Insert into #tmp_pubcodeGroupDupe_inner
	SELECT pubcode,igrp_no
			FROM SubscriberFinal sf1 WITH(NoLock) 
			WHERE sf1.ProcessCode = @ProcessCode AND sf1.SourceFileID = @SourceFileId
			GROUP BY PubCode,IGrp_No HAVING COUNT(1) > 1

	PRINT ('Insert into #tmp_pubcodeGroupDupe_inner  / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
				
	-- Remove duplicate within pubcode and igrp_no 	
	CREATE TABLE #tmp_pubcodeGroupDupe(
		SFRecordIdentifier UNIQUEIDENTIFIER
		,IsLatLonValid bit
		,QDate datetime
		,IGrp_No UNIQUEIDENTIFIER
		,pubcode varchar(255)
		,StatusUPDATEdReason VARCHAR(200))

	CREATE INDEX idx_pubcodeGroup ON #tmp_pubcodeGroupDupe(SFRecordIdentifier,IsLatLonValid,QDate,IGrp_No,StatusUPDATEdReason)

	INSERT INTO #tmp_pubcodeGroupDupe
	SELECT SFRecordIdentifier,sf.IsLatLonValid,sf.QDate,sf.IGrp_No,sf.pubcode,NULL as StatusUPDATEdReason
	FROM SubscriberFinal sf WITH(NoLock) 
		INNER JOIN #tmp_pubcodeGroupDupe_inner pub 
		ON sf.IGrp_No = pub.IGrp_No
	WHERE sf.ProcessCode = @ProcessCode AND SourceFileID = @SourceFileId
	GROUP BY SFRecordIdentifier,sf.IsLatLonValid,sf.QDate,sf.IGrp_No,sf.Pubcode,StatusUPDATEdReason
	ORDER BY sf.IGrp_No
		
	PRINT ('INSERT INTO #tmp_pubcodeGroupDupe  / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	drop table #tmp_pubcodeGroupDupe_inner 

	CREATE TABLE #tmp_TopOneMasterPubGroup ( SFRecordIdentifier UNIQUEIDENTIFIER)

	;WITH pubDupes 
			AS (SELECT SFRecordIdentifier, 
					(CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END) as IsLatLonValid
					,igrp_no 
					,pubcode
					,qdate
					,Row_number() 
						OVER ( 
						PARTITION BY igrp_no,pubcode 
						ORDER BY (CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END),qdate DESC) rn 
				FROM   #tmp_pubcodeGroupDupe)
	INSERT INTO #tmp_TopOneMasterPubGroup
	SELECT SFRecordIdentifier
	FROM   pubDupes 
	WHERE  rn = 1 
	ORDER BY SFRecordIdentifier

	PRINT ('WITH pubDupes  / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	-- Anything that is not a master will be removed
	UPDATE pgd
	SET StatusUPDATEdReason = 'Master within pubcode'
	FROM #tmp_pubcodeGroupDupe pgd inner join #tmp_TopOneMasterPubGroup tom ON pgd.SFRecordIdentifier = tom.SFRecordIdentifier

	PRINT ('StatusUPDATEdReason : Master within pubcode / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	UPDATE #tmp_pubcodeGroupDupe
	SET StatusUPDATEdReason = 'Dupe within pubcode'
	WHERE isnull(StatusUPDATEdReason,'')=''

	PRINT ('StatusUPDATEdReason : Dupe within pubcode / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	-- Change subordinate SFRecordIdentifer in SubscriberDemographicFinal to the master's SFRecordIdentifier, then archive the profile record from SubscriberFinal into SubscriberArchive
	CREATE TABLE #MasterIgrpNo(
		MSFRecordIdentifier uniqueidentifier,
		Igrp_no uniqueidentifier)

	INSERT INTO #MasterIgrpNo
	SELECT tom.SFRecordIdentifier,pgd.IGrp_No 
	FROM #tmp_TopOneMasterPubGroup tom INNER JOIN #tmp_pubcodeGroupDupe pgd on tom.SFRecordIdentifier = pgd.SFRecordIdentifier
	WHERE pubcode in (Select pubcode FROM #tmp_pubcodeGroupDupe Group by pubcode having COUNT(*) > 1)
		
	UPDATE sdf
	SET SFRecordIdentifier = x.MSFRecordIdentifier
	FROM SubscriberDemographicFinal sdf INNER JOIN (SELECT pgd.SFRecordIdentifier AS DupeSFRecordIdentifier,m.MSFRecordIdentifier,pgd.StatusUPDATEdReason 
													FROM #tmp_pubcodeGroupDupe pgd JOIN #MasterIgrpNo m ON pgd.IGrp_No = m.Igrp_no
													WHERE pgd.StatusUPDATEdReason = 'Dupe within pubcode') AS x
										ON sdf.SFRecordIdentifier = x.DupeSFRecordIdentifier

	-- Insert duplicate records into SubscriberArchive	
	DECLARE @satIDs table (SARecordIdentifier UNIQUEIDENTIFIER, SFRecordIdentifier UNIQUEIDENTIFIER, UNIQUE CLUSTERED (SFRecordIdentifier))

	INSERT INTO SubscriberArchive
	(
			SFRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
			Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,Source,
			Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
			LatLonMsg,Score,EmailStatusID,StatusUPDATEdDate,StatusUPDATEdReason,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUPDATEdInLive,
			UPDATEInLiveDate,SARecordIdentifier,DateCreated,DateUPDATEd,CreatedByUserID,UPDATEdByUserID,IsMailable,ProcessCode,ImportRowNumber
	)  
	OUTPUT Inserted.SARecordIdentifier, Inserted.SFRecordIdentifier
			INTO @satIDs
	SELECT 
			sf.SFRecordIdentifier,sf.SourceFileID,sf.PubCode,sf.Sequence,sf.FName,sf.LName,sf.Title,sf.Company,sf.Address,sf.MailStop,sf.City,sf.State,sf.Zip,sf.Plus4,sf.ForZip,sf.County,sf.Country,CountryID,sf.Phone,sf.PhoneExists,
			sf.Fax,sf.FaxExists,sf.Email,sf.EmailExists,sf.CategoryID,sf.TransactionID,sf.TransactionDate,sf.QDate,sf.QSourceID,sf.RegCode,sf.Verified,sf.SubSrc,sf.OrigsSrc,sf.Par3C,sf.Demo31,sf.Demo32,sf.Demo33,sf.Demo34,sf.Demo35,sf.Demo36,sf.Source,
			sf.Priority,sf.IGrp_No,sf.IGrp_Cnt,sf.CGrp_No,sf.CGrp_Cnt,sf.StatList,sf.Sic,sf.SicCode,sf.Gender,sf.IGrp_Rank,sf.CGrp_Rank,sf.Address3,sf.Home_Work_Address,sf.PubIDs,sf.Demo7,sf.IsExcluded,sf.Mobile,sf.Latitude,sf.Longitude,sf.IsLatLonValid,
			sf.LatLonMsg,sf.Score,sf.EmailStatusID,sf.StatusUPDATEdDate,'Duplicate within pubcode' as StatusUPDATEdReason,sf.Ignore,sf.IsDQMProcessFinished,sf.DQMProcessDate,sf.IsUPDATEdInLive,
			sf.UPDATEInLiveDate,NEWID(),sf.DateCreated,sf.DateUPDATEd,sf.CreatedByUserID,sf.UPDATEdByUserID,sf.IsMailable,sf.ProcessCode,sf.ImportRowNumber
	FROM SubscriberFinal sf With(NoLock)
			INNER JOIN #tmp_pubcodeGroupDupe pg ON sf.SFRecordIdentifier = pg.SFRecordIdentifier
	WHERE pg.StatusUPDATEdReason = 'Dupe within pubcode'
	AND sf.SourceFileID = @SourceFileId 
	AND sf.ProcessCode = @ProcessCode 
	AND sf.SFRecordIdentifier NOT IN (SELECT sa.SFRecordIdentifier FROM SubscriberArchive sa where sa.SourceFileID = @SourceFileId  and sa.ProcessCode = @ProcessCode );
		
	PRINT ('Insert Into SubscriberArchive / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DELETE sf FROM SubscriberFinal sf join @satIDs t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	WHERE sf.SourceFileID = @sourceFileID and sf.ProcessCode = @Processcode

	DROP TABLE #tmp_pubcodeGroupDupe
	DROP TABLE #tmp_TopOneMasterPubGroup
	DROP TABLE #MasterIgrpNo
	

    -------------------------------------------------------------------------------------------------------------------------------------------------
    -- There may be files with multiple pubcodes in an igrp_no from the current processing file and igrp_no is not in Subscriptions
    -- Assign M records for SubscriberFinal
	CREATE TABLE #tmp_SubAssignMaster(
				SFRecordIdentifier UNIQUEIDENTIFIER
			,IsLatLonValid BIT
			,QDate DATETIME
			,IGrp_No UNIQUEIDENTIFIER
			,igrp_rank VARCHAR(1))

	CREATE INDEX idx_SubAssignMaster ON #tmp_SubAssignMaster(SFRecordIdentifier,IsLatLonValid,QDate,IGrp_No,igrp_rank)

	CREATE TABLE #tmp_SubAssignMaster_innerquery (igrp_no UNIQUEIDENTIFIER)

	CREATE CLUSTERED INDEX IDX_tmp_SubAssignMaster_innerquery_igrp_no ON #tmp_SubAssignMaster_innerquery(igrp_no)

	INSERT INTO #tmp_SubAssignMaster_innerquery
	SELECT IGRP_NO 
	FROM SubscriberFinal
	WHERE ProcessCode = @ProcessCode AND SourceFileID = @SourceFileId
	GROUP BY IGrp_No HAVING Count(SubscriberFinalID) > 1

	PRINT ('INSERT INTO #tmp_SubAssignMaster_innerquery / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	INSERT INTO #tmp_SubAssignMaster
		SELECT sf1.SFRecordIdentifier,sf1.IsLatLonValid,sf1.QDate,sf2.igrp_no,null
		FROM SubscriberFinal sf1 WITH(NOLOCK) INNER JOIN #tmp_SubAssignMaster_innerquery as sf2 ON sf1.IGrp_No = sf2.IGrp_No
		WHERE sf1.ProcessCode = @ProcessCode AND sf1.SourceFileID = @SourceFileId AND ISNULL(Igrp_rank,'')=''
		GROUP BY sf1.SFRecordIdentifier,sf1.IsLatLonValid,sf1.QDate,sf2.IGrp_No,sf1.igrp_rank,sf1.StatusUpdatedReason

	PRINT ('INSERT INTO #tmp_SubAssignMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	DROP TABLE #tmp_SubAssignMaster_innerquery

	CREATE TABLE #tmp_InternalMaster ( SFRecordIdentifier UNIQUEIDENTIFIER)

	;WITH assignMaster 
	AS (SELECT SFRecordIdentifier, 
			(CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END) AS IsLatLonValid
			,igrp_no 
			,igrp_rank
			,qdate 
			,Row_number() 
			OVER ( 
			PARTITION BY igrp_no 
			ORDER BY (CASE WHEN IsLatLonValid = 0 THEN 2 ELSE 1 END),qdate DESC) rn 
		FROM   #tmp_SubAssignMaster)
	INSERT INTO #tmp_InternalMaster
	SELECT SFRecordIdentifier
	FROM   assignMaster 
	WHERE  rn = 1 
	ORDER BY SFRecordIdentifier

	PRINT ('INSERT INTO #tmp_InternalMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	UPDATE #tmp_SubAssignMaster
	SET igrp_rank = 'M'
	FROM #tmp_SubAssignMaster sm inner join #tmp_InternalMaster im ON sm.SFRecordIdentifier = im.SFRecordIdentifier

	PRINT ('UPDATE #tmp_SubAssignMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

	drop table #tmp_InternalMaster

	UPDATE #tmp_SubAssignMaster
	SET igrp_rank = 'S'
	WHERE ISNULL(igrp_rank,'')=''

	PRINT ('UPDATE #tmp_SubAssignMaster / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
	UPDATE sf
	SET Igrp_Rank = sm.Igrp_Rank
	FROM SubscriberFinal sf INNER JOIN #tmp_SubAssignMaster sm ON sf.SFRecordIdentifier = sm.SFRecordIdentifier
		
	PRINT ('UPDATE SubscriberFinal / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
	
	Drop table #tmp_SubAssignMaster

	---------------------------------------------------------------------------------------------------------------------------------
    -- Assign M to igrp_rank because igrp_no may be a new record and is the only record with that igrp_no
    UPDATE SubscriberFinal
    SET IGrp_Rank = 'M'
    WHERE ISNULL(igrp_rank,'')='' AND @SourceFileID = @SourceFileID AND ProcessCode = @ProcessCode

	PRINT ('UPDATE SubscriberFinal TO M / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

    --END ASSIGN IGRP_RANK

	Create table #tbl1 (SubscriberFinalID int, Igrp_No uniqueidentifier, Igrp_Rank varchar(2), PubID int, SubscriptionID int)--, SubscriberID int)

	CREATE CLUSTERED INDEX IDX_C_tbl1_IncomingDataID ON #tbl1(SubscriberFinalID)
    
	CREATE INDEX IDX_tbl1_Igrp_No ON #tbl1(Igrp_No)
	CREATE INDEX IDX_tbl1_SubscriptionID ON #tbl1(SubscriptionID)
	--CREATE INDEX IDX_tbl1_SubscriberID ON #tbl1(SubscriberID)
	CREATE INDEX IDX_tbl1_Igrp_Rank ON #tbl1(Igrp_Rank)

	insert into #tbl1 (SubscriberFinalID, IGRP_NO, Igrp_Rank, PubID)
	select MAX(sf.SubscriberFinalID), sf.IGrp_No, min(sf.IGrp_Rank), p.PubID
	from 
		(select sf1.IGrp_No, min(sf1.IGrp_Rank) as IGrp_Rank, PubCode 
		from SubscriberFinal sf1 With(NoLock) 
		where sf1.ProcessCode = @ProcessCode and Ignore = 0 and isUpdatedinLIVE = 0  
		group by sf1.IGrp_No, PubCode) x
		join SubscriberFinal sf on sf.IGrp_No = x.IGrp_No and sf.IGrp_Rank = x.IGrp_Rank and sf.PubCode = x.PubCode 
		JOIN Pubs p WITH (nolock) ON p.PubCode = sf.PubCode	
		where Ignore = 0 and isUpdatedinLIVE = 0 and sf.ProcessCode = @ProcessCode 
		and not sf.IGrp_No in 
		(	
			Select IGrp_No 
			from Circulation..Subscriber S 
			join Circulation..Subscription SS on S.SubscriberID = SS.SubscriberID 
			where SS.PublicationID = @PublicationID and not IGrp_No is null
		)
	GROUP BY sf.IGrp_No, p.PubID
	order by 2,3

	--SELECT * FROM #tbl1		

	--DECLARE @distinctComps TABLE (CompName VARCHAR(200), CompCount int, SFRecordIdentifier UNIQUEIDENTIFIER, IssueID int)
	--DECLARE @PublisherID INT = (SELECT PublisherID FROM Circulation..Publication WHERE PublicationID = @PublicationID)
	--DECLARE @PubCode varchar(100) = (Select PubCode FROM Pubs where PubID = @PublicationID)

	--DECLARE @distinctCompSFRecordIdentifier table (CompName Varchar(200), SFRecordIdentifier Uniqueidentifier)

	/* GET LIST OF DISTINCT COMPS AND COUNT OF ALL TOTAL WITH WHO IS IN COMP */
	INSERT INTO @distinctComps (CompName, CompCount)
		Select PubCode,COUNT(PubCode)--,SF.SFRecordIdentifier
		FROM SubscriberFinal SF join #tbl1 t on SF.SubscriberFinalID = t.SubscriberFinalID
			WHERE SF.ProcessCode = @ProcessCode 
			--and t.SubscriberID is null 
			and PubCode = @PubCode
			GROUP BY SF.PubCode	

	UPDATE IC
		set IsActive = 'false'
		from Circulation..IssueComp IC where IC.IssueId = 
		(SELECT ISNULL(IssueID,0) FROM Circulation..Issue I join Pubs P ON I.PublicationId = P.PubID WHERE IsComplete = 0 and P.PubID = @PublicationID)

	/* APPEND THE ISSUEID TO THE LIST */
	UPDATE DC
		SET IssueID = (SELECT ISNULL(IssueID,0) FROM Circulation..Issue I join Pubs P ON I.PublicationId = P.PubID WHERE IsComplete = 0 and P.PubCode = DC.CompName)
		FROM @distinctComps DC JOIN SubscriberFinal SF ON DC.CompName = SF.PubCode--DC.SFRecordIdentifier = SF.SFRecordIdentifier
		WHERE SF.ProcessCode = @ProcessCode 

	/* ANY ISSUEID OF ZERO WILL BE CONSIDERED ERRORS AND SHOULD BE MESSAGED */
	INSERT INTO Circulation..IssueCompError (CompName, SFRecordIdentifier, ProcessCode, DateCreated, CreatedByUserID)
		SELECT CompName, SFRecordIdentifier, @ProcessCode, GETDATE(), 1 FROM @distinctComps DC join SubscriberFinal SF on DC.CompName = SF.PubCode where IssueID = 0 and SF.ProcessCode = @ProcessCode

	/* INSERT ISSUEID'S THAT AREN'T ZERO */
	INSERT INTO Circulation..IssueComp (IssueId, ImportedDate, IssueCompCount, DateCreated, CreatedByUserID, IsActive)
		SELECT 
			IssueID,
			GETDATE(),
			CompCount,
			GETDATE(),
			1,
			'true'
		FROM @distinctComps WHERE IssueID > 0					

	Delete from Circulation..IssueCompDetail where IssueCompID in (Select IssueCompID from Circulation..IssueComp where IsActive = 'false')

	/* INSERT RECORD INTO DETAIL FOR THE SUBSCRIBER */
	INSERT INTO Circulation..IssueCompDetail (IssueCompID,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City
												,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate
												,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,SequenceID
												,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID
												,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify
												,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
		SELECT (SELECT ISNULL(IssueCompID,0) FROM Circulation..IssueComp WHERE IssueID = DC.IssueID and IsActive = 'true'),NULL,SF.FName,SF.LName,SF.Company,SF.Title,NULL,NULL,SF.Address,SF.MailStop,NULL,SF.City,
				SF.State,NULL,SF.Zip,SF.Plus4,NULL,SF.County,SF.Country,SF.CountryID,SF.Latitude,SF.Longitude,SF.IsLatLonValid,SF.StatusUpdatedDate,
				NULL,SF.LatLonMsg,SF.Email,SF.Phone,SF.Fax,SF.Mobile,NULL,NULL,NULL,NULL,NULL,0,NULL,0,			   
				@PublisherID,0,@PublicationID,0,0,0,0,SF.QSourceID,SF.QDate,SF.Demo7,
				0,NULL,0,NULL,SF.Par3C,0,NULL,NULL,NULL,SF.Verified,
				SF.DateCreated,SF.DateUpdated,SF.CreatedByUserID,SF.UpdatedByUserID
		FROM SubscriberFinal SF			
			JOIN @distinctComps DC ON SF.PubCode = DC.CompName --.SFRecordIdentifier = DC.SFRecordIdentifier
			WHERE SF.ProcessCode = @ProcessCode

	drop table #tbl1
END


--			/*------ OLD WITH DATA MATCHING AND CHECKING FOR DUPES ------*/
--	/* GET LIST OF DISTINCT COMPS AND COUNT OF ALL TOTAL WITH WHO IS IN COMP */
--	INSERT INTO @distinctComps (CompName, CompCount, SFRecordIdentifier)
--		Select PubCode,COUNT(PubCode),SF.SFRecordIdentifier
--		FROM SubscriberFinal SF
--			WHERE SF.ProcessCode = @ProcessCode GROUP BY SF.PubCode, SF.SFRecordIdentifier


--	/* APPEND THE ISSUEID TO THE LIST */
--	UPDATE DC
--		SET IssueID = (SELECT ISNULL(IssueID,0) FROM Circulation..Issue I join Circulation..Publication P ON I.PublicationId = P.PublicationID WHERE IsComplete = 0 and P.PublicationCode = DC.CompName)
--		FROM @distinctComps DC JOIN SubscriberFinal SF ON DC.SFRecordIdentifier = SF.SFRecordIdentifier
--		WHERE SF.ProcessCode = @ProcessCode 


--	/* ANY ISSUEID OF ZERO WILL BE CONSIDERED ERRORS AND SHOULD BE MESSAGED */
--	INSERT INTO Circulation..IssueCompError (CompName, SFRecordIdentifier, ProcessCode, DateCreated, CreatedByUserID)
--		SELECT CompName, SFRecordIdentifier, @ProcessCode, GETDATE(), 1 FROM @distinctComps where IssueID = 0


--	/* INSERT ISSUEID'S THAT AREN'T ZERO */
--	INSERT INTO Circulation..IssueComp (IssueId, ImportedDate, IssueCompCount, DateCreated, CreatedByUserID)
--		SELECT 
--			IssueID,
--			GETDATE(),
--			CompCount,
--			GETDATE(),
--			1
--		FROM @distinctComps WHERE IssueID > 0
				

--	/* INSERT RECORD INTO DETAIL FOR THE SUBSCRIBER */
--	INSERT INTO Circulation..IssueCompDetail (IssueCompID,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City
--											 ,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate
--											 ,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,SequenceID
--											 ,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID
--											 ,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify
--											 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
--		SELECT (SELECT IssueCompID FROM Circulation..IssueComp WHERE IssueID = DC.IssueID),NULL,SF.FName,SF.LName,SF.Company,SF.Title,NULL,NULL,SF.Address,SF.MailStop,NULL,SF.City,
--			   SF.State,NULL,SF.Zip,SF.Plus4,NULL,SF.County,SF.Country,SF.CountryID,SF.Latitude,SF.Longitude,SF.IsLatLonValid,SF.StatusUpdatedDate,
--			   NULL,SF.LatLonMsg,SF.Email,SF.Phone,SF.Fax,SF.Mobile,NULL,NULL,NULL,NULL,NULL,0,NULL,0,			   
--			   @PublisherID,0,@PublicationID,0,0,0,0,SF.QSourceID,SF.QDate,SF.Demo7,
--			   0,NULL,0,NULL,SF.Par3C,0,NULL,NULL,NULL,SF.Verified,
--			   SF.DateCreated,SF.DateUpdated,SF.CreatedByUserID,SF.UpdatedByUserID
--		FROM SubscriberFinal SF			
--			JOIN @distinctComps DC ON SF.SFRecordIdentifier = DC.SFRecordIdentifier
--			WHERE SF.ProcessCode = @ProcessCode
--END
GO
PRINT N'Altering [dbo].[job_TelemarketingRules_ProcessCode]...';


GO
ALTER procedure job_TelemarketingRules_ProcessCode
@ProcessCode varchar(50),
@FileType varchar(50)
as
	declare @pubCount int = (select count(distinct p.PubID)
							  from Pubs p with(nolock)
							  join SubscriberFinal sf with(nolock) on sf.PubCode = p.PubCode 
							  where sf.ProcessCode = @ProcessCode)
							  					  
	----------------NOW INSERT NEW RECORDS
	---- Cat 70  Xact 10
	-------this should get us all the new records
	select sf.*,0 as 'SubscriptionID', 0 as 'SubscriberID'
	into #tmpNewRecords
	from SubscriberFinal sf with(nolock)
	left join Circulation..Subscriber csub with(nolock) on csub.IGrp_No = sf.IGrp_No
	where sf.ProcessCode = @ProcessCode
	and csub.SubscriberID is null
			
	declare @NewBatchCount int = (select COUNT(*) from #tmpNewRecords)
	
	if(@NewBatchCount > 0)
		begin		
	---pull a batch number
		if(@pubCount = 1)
			begin
				declare @NewPublicationID int = (select distinct p.PubID
												  from Pubs p with(nolock)
												  join #tmpNewRecords sf with(nolock) on sf.PubCode = p.PubCode)											  
				--pull a BatchID
				declare @NewBatchID int
				insert into Circulation..Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated)
				values(@NewPublicationID,1,@NewBatchCount,'true',GETDATE());set @NewBatchID = @@IDENTITY;  

				DECLARE @subID TABLE (SubscriberID int, SFRecordIdentifier uniqueidentifier)
				--Insert to Subscriber
				INSERT INTO Circulation..Subscriber (FirstName,LastName,Company,Title,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
													County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationMessage,
													Email,Phone,Fax,Mobile,DateCreated,CreatedByUserID,IsInActiveWaveMailing,IGrp_No, SFRecordIdentifier)
				OUTPUT Inserted.SubscriberID, Inserted.SFRecordIdentifier
				INTO @subID
				select n.FName,n.LName,n.Company,n.Title,n.Address,n.MailStop,n.Address3,n.City,n.State,
				(select RegionID from UAS..Region where RegionCode = n.State and CountryID = n.CountryID),n.Zip,n.Plus4,
				n.County,n.Country,n.CountryID,n.Latitude,n.Longitude,n.IsLatLonValid,n.LatLonMsg,n.Email,n.Phone,n.Fax,n.Mobile,GETDATE(),1,'false',n.IGrp_No,n.SFRecordIdentifier
				from #tmpNewRecords n
				
				UPDATE #tmpNewRecords 
				SET #tmpNewRecords.SubscriberID = x.SubscriberID
				FROM @subID x 
				WHERE #tmpNewRecords.SFRecordIdentifier = x.SFRecordIdentifier

				DECLARE @sID TABLE (SubscriptionID int, SubscriberID int)
				--Insert to Subscription
				declare @sfID int = (select distinct SourceFileID from #tmpNewRecords)
				declare @ClientID int = (select ClientID from UAS..SourceFile with(nolock) where SourceFileID = @sfID)
				declare @pCode varchar(50) = (select distinct PubCode from #tmpNewRecords)
				declare @NewPubID int = (Select PubID From Pubs with(nolock) where PubCode = @pCode)
				declare @ActionID int = (select ActionID
										 from Circulation..Action a
										 join Circulation..CategoryCode c on a.CategoryCodeID = c.CategoryCodeID
										 join Circulation..TransactionCode t on a.TransactionCodeID = t.TransactionCodeID 
										 where c.CategoryCodeValue = 70 
										 and t.TransactionCodeValue = 10
										 and a.ActionTypeID = 2)
				declare @SubStatusID int = (select SubscriptionStatusID from Circulation..SubscriptionStatus where StatusName = 'Active Free')
				declare @DeliverID int = (select DeliverabilityID from Circulation..Deliverability where DeliverabilityName = 'Print')
				INSERT INTO Circulation..Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,SubscriptionStatusID,IsPaid,QSourceID,
														QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,DateCreated,CreatedByUserID,
														Par3cID,IsActive)
				OUTPUT Inserted.SubscriptionID, Inserted.SubscriberID
				INTO @sID						
				select n.Sequence,@ClientID,n.SubscriberID,@NewPubID,@ActionID,@SubStatusID,'false',n.QSourceID,n.QDate,@DeliverID,'true',n.SubSrc,1,n.Source,GETDATE(),1,
				n.Par3C,'true'
				from #tmpNewRecords n
				
				UPDATE #tmpNewRecords 
				SET #tmpNewRecords.SubscriptionID = x.SubscriptionID
				FROM @sID x 
				WHERE #tmpNewRecords.SubscriberID = x.SubscriberID
				
				--Insert SubscriberDemographicFinal to SubscriptionResponseMap
				select sdf.MAFField,sdf.Value,sf.IGrp_No,sdf.PubID,sf.SFRecordIdentifier,sf.SubscriptionID
				into #tmpNewDemos
				from SubscriberDemographicFinal sdf with(nolock)
				join #tmpNewRecords sf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier
				where sf.ProcessCode = @ProcessCode
				
				insert into Circulation..SubscriptionResponseMap (SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID)
				select distinct d.SubscriptionID,
				r.CodeSheetID,
				'true',GETDATE(),1
				from #tmpNewDemos d
				join dbo.CodeSheet r with(nolock) on r.Responsevalue = d.Value 
													 and r.PubID = d.PubID 
													 and r.ResponseGroup = d.MAFField
													 and r.ResponseGroup != 'DEMO7'
				left join Circulation..SubscriptionResponseMap srm on srm.SubscriptionID = d.SubscriptionID and srm.CodeSheetID = r.CodeSheetID
				where srm.SubscriptionID is null and srm.CodeSheetID is null
															 
				--Insert to History table
				Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,DateCreated,CreatedByUserID)
				select @NewBatchID,@@ROWCOUNT,@ClientID,@NewPublicationID,t.SubscriberID,t.SubscriptionID,GETDATE(),1
				from #tmpNewRecords t with(nolock)

				drop table #tmpNewDemos
				
			end
		else
			begin
				--Multiple PubCodes do same thing but in a cursor on PubCode
				DECLARE @PubId int
				DECLARE @PubCode varchar(50)

				DECLARE c CURSOR
				FOR 
					select distinct p.PubID,sf.PubCode
					from Pubs p with(nolock)
					join #tmpNewRecords sf with(nolock) on sf.PubCode = p.PubCode
				OPEN c
				FETCH NEXT FROM c INTO @PubId,@PubCode
				WHILE @@FETCH_STATUS = 0
				BEGIN
					select *
					into #tmpPubRecords
					from #tmpNewRecords
					where PubCode = @PubCode
					
					declare @PubBatchCount int = (select COUNT(*) from #tmpPubRecords)
				
					declare @PubPublicationID int = (select distinct p.PubID
													  from Pubs p with(nolock)
													  join #tmpPubRecords sf with(nolock) on sf.PubCode = p.PubCode)
													  
					--pull a BatchID
					declare @PubBatchID int
					insert into Circulation..Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated)
					values(@NewPublicationID,1,@NewBatchCount,'true',GETDATE());set @PubBatchID = @@IDENTITY;  

					DECLARE @PubsubID TABLE (SubscriberID int, SFRecordIdentifier uniqueidentifier)
					--Insert to Subscriber
					INSERT INTO Circulation..Subscriber (FirstName,LastName,Company,Title,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
														County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationMessage,
														Email,Phone,Fax,Mobile,DateCreated,CreatedByUserID,IsInActiveWaveMailing,IGrp_No, SFRecordIdentifier)
					OUTPUT Inserted.SubscriberID, Inserted.SFRecordIdentifier
					INTO @PubsubID
					select n.FName,n.LName,n.Company,n.Title,n.Address,n.MailStop,n.Address3,n.City,n.State,
					(select RegionID from UAS..Region where RegionCode = n.State and CountryID = n.CountryID),n.Zip,n.Plus4,
					n.County,n.Country,n.CountryID,n.Latitude,n.Longitude,n.IsLatLonValid,n.LatLonMsg,n.Email,n.Phone,n.Fax,n.Mobile,GETDATE(),1,'false',n.IGrp_No,n.SFRecordIdentifier
					from #tmpPubRecords n
					
					UPDATE #tmpPubRecords 
					SET #tmpPubRecords.SubscriberID = x.SubscriberID
					FROM @subID x 
					WHERE #tmpPubRecords.SFRecordIdentifier = x.SFRecordIdentifier
					
					DECLARE @PubsID TABLE (SubscriptionID int, SubscriberID int)
					--Insert to Subscription
					declare @PubsfID int = (select distinct SourceFileID from #tmpPubRecords)
					declare @PubClientID int = (select ClientID from UAS..SourceFile with(nolock) where SourceFileID = @PubsfID)
					declare @PubpCode varchar(50) = (select distinct PubCode from #tmpPubRecords)
					declare @PubPubID int = (Select PubID From Pubs with(nolock) where PubCode = @PubpCode)
					declare @PubActionID int = (select ActionID
											 from Circulation..Action a
											 join Circulation..CategoryCode c on a.CategoryCodeID = c.CategoryCodeID
											 join Circulation..TransactionCode t on a.TransactionCodeID = t.TransactionCodeID 
											 where c.CategoryCodeValue = 70 
											 and t.TransactionCodeValue = 10)
					declare @PubSubStatusID int = (select SubscriptionStatusID from Circulation..SubscriptionStatus where StatusName = 'Active Free')
					declare @PubDeliverID int = (select DeliverabilityID from Circulation..Deliverability where DeliverabilityName = 'Print')
					INSERT INTO Circulation..Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,SubscriptionStatusID,IsPaid,QSourceID,
															QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,DateCreated,CreatedByUserID,
															Par3cID,IsActive)
					OUTPUT Inserted.SubscriptionID, Inserted.SubscriberID
					INTO @PubsID						
					select n.Sequence,@ClientID,n.SubscriberID,@NewPubID,@ActionID,@SubStatusID,'false',n.QSourceID,n.QDate,@DeliverID,'true',n.SubSrc,1,n.Source,GETDATE(),1,
					ISNUMERIC(n.Par3C),'true'
					from #tmpPubRecords n
					
					UPDATE #tmpPubRecords 
					SET #tmpPubRecords.SubscriptionID = x.SubscriptionID
					FROM @PubsID x 
					WHERE #tmpPubRecords.SubscriberID = x.SubscriberID
					
					--Insert SubscriberDemographicFinal to SubscriptionResponseMap
					select sdf.MAFField,sdf.Value,sf.IGrp_No,sdf.PubID,sf.SFRecordIdentifier,sf.SubscriptionID
					into #tmpPubDemos
					from SubscriberDemographicFinal sdf with(nolock)
					join #tmpPubRecords sf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier
					where sf.ProcessCode = @ProcessCode
					
					insert into Circulation..SubscriptionResponseMap (SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID)
					select distinct d.SubscriptionID,
					r.CodeSheetID,
					'true',GETDATE(),1
					from #tmpPubDemos d
					join dbo.CodeSheet r with(nolock) on r.Responsevalue = d.Value 
														 and r.PubID = d.PubID 
														 and r.ResponseGroup = d.MAFField
														 and r.ResponseGroup != 'DEMO7'
					left join Circulation..SubscriptionResponseMap srm on srm.SubscriptionID = d.SubscriptionID and srm.CodeSheetID = r.CodeSheetID
					where srm.SubscriptionID is null and srm.CodeSheetID is null

					--Insert to History table
					Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,DateCreated,CreatedByUserID)
					select @PubBatchID,@@ROWCOUNT,@PubClientID,@PubPublicationID,t.SubscriberID,t.SubscriptionID,GETDATE(),1
					from #tmpPubRecords t with(nolock)

					drop table #tmpPubDemos		
					drop table #tmpPubRecords
					
					FETCH NEXT FROM c INTO @PubId,@PubCode
				END
				CLOSE c
				DEALLOCATE c
			end
		end

	drop table #tmpNewRecords

----------NOW DO update and apply rules

 --   2 code paths - Long Form or Short Form - Short Form can have audit questions
 --   much like NCOA - match on Seq# then update data
 --   Shared rules
 --   - Updates existing active qualified record with new information from a pre-approved telemarketing script.
 --   - Cat 10  Xact 27
 --   - use qdate on incoming record 
--REPLACE ALL PAR3C WITH '1' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #' ') AND (FNAME #'  ' OR LNAME #'  ')
--REPLACE ALL PAR3C WITH '2' FOR ((function ='  ' or function = 'ZZ') AND TITLE ='  ') AND (FNAME#'  ' OR LNAME #'  ')
--REPLACE ALL PAR3C WITH '3' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #'  ') AND (FNAME ='  ' AND LNAME ='  ')
--REPLACE ALL PAR3C WITH '4' FOR ((function =' ' or function = 'ZZ') AND TITLE ='  ' AND FNAME ='  ' AND LNAME ='  ' AND COMPANY #'  ')
--REPLACE ALL PAR3C WITH '5' FOR COPIES >1

 --   - keep existing SequenceNumber
 --   - Assign a new batch. No limit to batch size.
 --   - Update any record within the existing file as long as the incoming qdate is greater than the current date on the record or the qsource is a (H,I,J,K,L,M,N).
 --   - if seqNumber is blank send through DQM Matching process to determin if subscriber already exist for this product or to create a new record
    
 --   Long Form rules
 --   - Overwrite existing demos from incoming  telemarketing file. 
 --   - If the incoming list does not have a demo7(media) field replace demo7 with "A" for print. If demo7 is on incoming file replace existing demo7 with demo7 incoming data.
    
 --   Short Form rules
 --   - Keep Demos from existing record when not provided on short form.
 --   - If the incoming list does not have a demo7(media) field  or the field is there but is blank replace demo7 with "A" for print.
 
	-- Assign a new batch. No limit to batch size.
	--how the hell can I batch all this to track before and after???
	-- will create a temp table to store all the before records, will do batching at the end.
	
	--create a temp copy of Circulation.Subscriptions that are about to be updated
	
	select cs.*
	into #tmpOriginalCS
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf on csub.IGrp_No = sf.IGrp_No
	and sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)

	-- Cat 10  Xact 27
	-- use qdate on incoming record
	
   update cs
   set ActionID_Previous = ActionID_Current, 
	   ActionID_Current = (Select ActionID
						   From Circulation..Action a
						   join Circulation..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
						   join Circulation..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 10 and tc.TransactionCodeValue = 27
						   and a.IsActive = 'true'
						   and a.ActionTypeID = (select CodeId from UAS..Code where CodeName = 'System Generated')),
		QSourceDate = sf.QDate
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf on csub.IGrp_No = sf.IGrp_No
	and sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	
	--REPLACE ALL PAR3C WITH '1' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #' ') AND (FNAME #'  ' OR LNAME #'  ')
	
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	left join #tmpOriginalCS tcs with(nolock) on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdf.Value != '' and sdf.Value != 'zz') or sf.Title != '') and (sf.FName != '' or sf.LName != '')
	and sf.ProcessCode = @ProcessCode 
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and tcs.SubscriptionID is null

	update cs
    set cs.Par3cID = 1
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value != '' and sdf.Value != 'zz') or sf.Title != '') and (sf.FName != '' or sf.LName != '')
	and sf.ProcessCode = @ProcessCode 
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	
	--REPLACE ALL PAR3C WITH '2' FOR ((function ='  ' or function = 'ZZ') AND TITLE ='  ') AND (FNAME#'  ' OR LNAME #'  ')
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdf.Value = '' or sdf.Value = 'zz') and sf.Title = '') and (sf.FName != '' or sf.LName != '') 
	and sf.ProcessCode = @ProcessCode 
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and tcs.SubscriptionID is null

	update cs
    set cs.Par3cID = 2
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value = '' or sdf.Value = 'zz') and sf.Title = '') and (sf.FName != '' or sf.LName != '') 
	and sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	
	--REPLACE ALL PAR3C WITH '3' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #'  ') AND (FNAME ='  ' AND LNAME ='  ')
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdf.Value != '' and sdf.Value != 'zz') or sf.Title != '') and (sf.FName = '' or sf.LName = '') 
	and sf.ProcessCode = @ProcessCode 
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 3
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value != '' and sdf.Value != 'zz') or sf.Title != '') and (sf.FName = '' or sf.LName = '') 
	and sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)

	--REPLACE ALL PAR3C WITH '4' FOR ((function =' ' or function = 'ZZ') AND TITLE ='  ' AND FNAME ='  ' AND LNAME ='  ' AND COMPANY #'  ')
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where ((sdf.Value = '' or sdf.Value = 'zz') and sf.Title = '' and sf.FName = '' and sf.LName = '' and sf.Company != '')
	and sf.ProcessCode = @ProcessCode 
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 4
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value = '' or sdf.Value = 'zz') and sf.Title = '' and sf.FName = '' and sf.LName = '' and sf.Company != '')
	and sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)

	--REPLACE ALL PAR3C WITH '5' FOR COPIES >1
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where cs.Copies > 1
	and sf.ProcessCode = @ProcessCode 
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and tcs.SubscriptionID is null
	
	update cs
    set cs.Par3cID = 5
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID  
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where cs.Copies > 1
	and sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
				   
	--Update any record within the existing file as long as the incoming qdate is greater than the current date on the record or the qsource is a (H,I,J,K,L,M,N).
	insert into #tmpOriginalCS
	select cs.*
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID 
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	left join #tmpOriginalCS tcs on tcs.SubscriptionID = cs.SubscriptionID
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and (sf.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	and tcs.SubscriptionID is null
	
	update cs
    set cs.DateUpdated = GETDATE(),
    cs.QSourceDate = sf.QDate,
    cs.SubscriberSourceCode = sf.SubSrc
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID 
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and (sf.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	
	--------------everything above this point updates Circulation..Subscription table
	
	---------------now get a temp Subscriber table to store those original values
	select csub.*
	into #tmpOriginalCSubscriber
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber csub with(nolock) on cs.SubscriberID = csub.SubscriberID 
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and (sf.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	
	update s
	set	s.FirstName = sf.FName,
		s.LastName = sf.LName,
		s.Company = sf.Company, 
		s.Title = sf.Title,
		s.Address1 = sf.Address,
		s.Address2 = sf.MailStop,
		s.Address3 = sf.Address3,
		s.City = sf.City,
		s.RegionCode = sf.State,
		s.RegionID = (select isnull(RegionID,0) from uas..Region where CountryID = sf.CountryID and RegionCode = sf.State),
		s.ZipCode = sf.Zip,
		s.Plus4 = sf.Plus4,
		s.County = sf.County,
		s.Country = sf.Country,
		s.CountryID = sf.CountryID,
		s.Latitude = 0,
		s.Longitude = 0,
		s.IsAddressValidated = 'false',
		s.AddressValidationDate = null,
		s.AddressValidationSource = null,
		s.AddressValidationMessage = null,
		s.Email = sf.Email,
		s.Phone = sf.Phone,
		s.Fax = sf.Fax,
		s.Mobile = sf.Mobile,
		s.Gender = sf.Gender
	from Circulation..Subscription cs with(nolock)
	join Circulation..Subscriber s with(nolock) on cs.SubscriberID = s.SubscriberID 
	join SubscriberFinal sf with(nolock) on s.IGrp_No = sf.IGrp_No
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Circulation..Publication where PublicationID = cs.PublicationID)
	and (sf.QDate > cs.QSourceDate 
	or cs.QSourceID in (select QSourceID from Circulation..QualificationSource where QSourceCode in ('H','I','J','K','L','M','N')))
	
	
	------------both long and short update Demo7
	--If the incoming list does not have a demo7(media) field replace demo7 with "A" for print. If demo7 is on incoming file replace existing demo7 with demo7 incoming data.
	select sf.Sequence,sf.Demo7,sf.PubCode
	into #tmpDemo7Update
	from SubscriberFinal sf with(nolock) 
	where sf.ProcessCode = @ProcessCode
	
	select srm.*
	into #tmpOriginalCSRM
	from Circulation..SubscriptionResponseMap srm
	join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
	join Circulation..Subscriber csub with(nolock) on csub.SubscriberID = s.SubscriberID 
	join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
	join Pubs pub on sf.PubCode = pub.PubCode
	join CodeSheet r with(nolock) on srm.ResponseID = r.CodeSheetID and pub.PubID = r.PubID and r.ResponseGroup = 'DEMO7'
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = (select PubCode from Pubs where PubID = s.PublicationID)
	
	update srm
	set srm.DateUpdated = GETDATE(),
	srm.UpdatedByUserID = 1,
	srm.CodeSheetID = r.CodeSheetID
	from Circulation..SubscriptionResponseMap srm
	join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
	join #tmpDemo7Update st on s.SequenceID = st.Sequence
	join Pubs pub on st.PubCode = pub.PubCode
	join CodeSheet r with(nolock) on srm.CodeSheetID = r.CodeSheetID and pub.PubID = r.PubID and r.ResponseGroup = 'DEMO7'
	where r.Responsevalue = isnull(nullif(st.Demo7,''),'A') 
	 
	drop table #tmpDemo7Update
	
	-----need to insert new Demos that don't exist in SubscriptionResponseMap
	select sdf.MAFField,sdf.Value,sf.IGrp_No,sdf.PubID
	into #tmpDemoInsert
	from SubscriberDemographicFinal sdf with(nolock)
	join Subscriberfinal sf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode

	Insert Into Circulation..SubscriptionResponseMap (SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID)
	select distinct s.SubscriptionID,r.CodeSheetID,'true',GETDATE(),1
	from Circulation..Subscription s 
	join Circulation..Subscriber csub with(nolock) on csub.SubscriberID = s.SubscriberID 
	join #tmpDemoInsert d on csub.IGrp_No = d.IGrp_No
	join CodeSheet r with(nolock) on  d.PubID = r.PubID and d.MAFField = r.ResponseGroup and d.Value = r.Responsevalue
	left join Circulation..SubscriptionResponseMap srm on srm.SubscriptionID = s.SubscriptionID and srm.CodeSheetID = r.CodeSheetID
	where srm.CodeSheetID is null and srm.SubscriptionID is null
	
	drop table #tmpDemoInsert
		
	if(@FileType = 'Telemarketing_Long_Form')
		begin
			--Overwrite existing demos from incoming  telemarketing file. 
			select sdf.MAFField,sdf.Value,sf.IGrp_No,sdf.PubID
			into #tmpDemoUpdate
			from SubscriberDemographicFinal sdf with(nolock)
			join Subscriberfinal sf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier
			where sf.ProcessCode = @ProcessCode

			insert into #tmpOriginalCSRM
			select srm.*
			from Circulation..SubscriptionResponseMap srm
			join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
			join Circulation..Subscriber csub with(nolock) on csub.SubscriberID = s.SubscriberID 
			join SubscriberFinal sf with(nolock) on csub.IGrp_No = sf.IGrp_No
			join Pubs pub on sf.PubCode = pub.PubCode
			join CodeSheet r with(nolock) on srm.ResponseID = r.CodeSheetID and pub.PubID = r.PubID and r.ResponseGroup != 'DEMO7'
			where sf.ProcessCode = @ProcessCode
			and sf.PubCode = (select PubCode from Pubs where PubID = s.PublicationID)
	
			update srm
			set srm.DateUpdated = GETDATE(),
			srm.UpdatedByUserID = 1,
			srm.CodeSheetID = r.CodeSheetID
			from Circulation..SubscriptionResponseMap srm
			join Circulation..Subscription s on srm.SubscriptionID = s.SubscriptionID
			join Circulation..Subscriber csub with(nolock) on csub.SubscriberID = s.SubscriberID 
			join #tmpDemoUpdate d on csub.IGrp_No = d.IGrp_No
			join CodeSheet r with(nolock) on srm.CodeSheetID = r.CodeSheetID and d.PubID = r.PubID and r.ResponseGroup != 'DEMO7'

			drop table #tmpDemoUpdate
		end
	--if(@FileType = 'Telemarketing_Short_Form')
	--	begin
	--		 --Keep Demos from existing record when not provided on short form.
	--		 --Do Nothing to demos
	--	end


--can't assume this will be only for one pubcode so need to pull multiple batches if needed.

	--temp tables with original values #tmpOriginalCS, #tmpOriginalCSubscriber, #tmpOriginalCSRM
	declare @PublisherID int = (select top 1 PublisherID
								from Circulation..Publisher p with(nolock)
								join UAS..SourceFile sf with(nolock) on p.ClientID = sf.ClientID
								join SubscriberFinal st with(nolock) on st.SourceFileID = sf.SourceFileID 
								where st.ProcessCode = @ProcessCode)
	if(@pubCount = 1)
		begin
			declare @tocsCount   int = (select COUNT(*) from #tmpOriginalCS)
			declare @tocSubCount int = (select COUNT(*) from #tmpOriginalCSubscriber)
			declare @tocsrmCount int = (select COUNT(*) from #tmpOriginalCSRM)
			declare @BatchCount int = (select @tocSubCount + @tocsCount + @tocsrmCount)
			
			declare @PublicationID int = (select distinct p.PubID
								  from Pubs p with(nolock)
								  join SubscriberFinal sf with(nolock) on sf.PubCode = p.PubCode 
								  where sf.ProcessCode = @ProcessCode)
			--pull a BatchID
			declare @BatchID int
			insert into Circulation..Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated)
			values(@PublicationID,1,@BatchCount,'true',GETDATE());set @BatchID = @@IDENTITY;  		
			
			Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
														  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
														  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
														  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,IsAddressValidated,DateCreated,SubscriberDateCreated,SubscriberCreatedByUserID,CreatedByUserID)


			select SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
				  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
				  DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,
				  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,'false',GETDATE(),GETDATE(),1,1
			from #tmpOriginalCS with(nolock)

			------------#tmpOriginalCSubscriber
			Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
														  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
														  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
														  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,
														  
														  ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,
														  RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
														  AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,
														  Age,Income,Gender,SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,IsLocked,LockDate,LockDateRelease,
														  LockedByUserID,PhoneExt,DateCreated,CreatedByUserID)


			select cs.SubscriptionID,cs.SequenceID,cs.PublisherID,cs.SubscriberID,cs.PublicationID,cs.ActionID_Current,cs.ActionID_Previous,cs.SubscriptionStatusID,
				   cs.IsPaid,cs.QSourceID,cs.QSourceDate,cs.DeliverabilityID,cs.IsSubscribed,cs.SubscriberSourceCode,cs.Copies,cs.OriginalSubscriberSourceCode,
				   cs.DateCreated,cs.DateUpdated,cs.CreatedByUserID,cs.UpdatedByUserID,cs.AccountNumber,cs.GraceIssues,cs.OnBehalfOf,cs.MemberGroup,cs.Verify,cs.IsNewSubscription,

			ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,
					County,Country,CountryID,Latitude,Longitude,ISNULL(IsAddressValidated,'false'),AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,
					Fax,Mobile,Website,Birthdate,Age,Income,Gender,tocs.DateCreated,tocs.DateUpdated,tocs.CreatedByUserID,tocs.UpdatedByUserID,IsLocked,LockDate,LockDateRelease,
					LockedByUserID,PhoneExt,GETDATE(),1
			from #tmpOriginalCSubscriber tocs with(nolock)
			join Circulation..Subscription cs with(nolock) on tocs.SubscriberID = cs.SubscriberID 
			
			
			Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
			select @BatchID,@@ROWCOUNT,t.PublisherID,t.PublicationID,t.SubscriberID,t.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
			from #tmpOriginalCS t with(nolock)
			join Circulation..HistorySubscription hs with(nolock) on t.SubscriptionID = hs.SubscriptionID 
			
			Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
			select @BatchID,@@ROWCOUNT,cs.PublisherID,cs.PublicationID,cs.SubscriberID,cs.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
			from #tmpOriginalCSubscriber sub with(nolock)
			join Circulation..Subscription cs with(nolock) on sub.SubscriberID = cs.SubscriberID 
			join Circulation..HistorySubscription hs with(nolock) on cs.SubscriptionID = hs.SubscriptionID 
			
			drop table #tmpOriginalCS
			drop table #tmpOriginalCSubscriber
		end	
	else
		begin
			--do same thing as above but by pubcode
			--lets do a cursor for each distinct PubID
			declare @PubID1 int
			declare @PubCode1 varchar(50)

			declare c cursor
			for 
				select distinct PublisherID, PubCode 
				from Circulation..Publisher p with(nolock)
				join UAS..SourceFile sf with(nolock) on p.ClientID = sf.ClientID
				join SubscriberFinal st with(nolock) on st.SourceFileID = sf.SourceFileID 
				where st.ProcessCode = @ProcessCode

			open c
			fetch next from c into @PubID1,@PubCode1
			while @@FETCH_STATUS = 0
			begin
					
				declare @tocsCount2   int = (select COUNT(*) from #tmpOriginalCS where PublicationID = @PubID1)
				declare @tocSubCount2 int = (select COUNT(*) 
											 from #tmpOriginalCSubscriber t
											 join Circulation..Subscription s on t.SubscriberID = s.SubscriberID 
											 where s.PublicationID = @PubID1)
				declare @tocsrmCount2 int = (select COUNT(*) 
											 from #tmpOriginalCSRM m
											 join Circulation..Subscription s on m.SubscriptionID = s.SubscriptionID 
											 where s.PublicationID = @PubID1)
				declare @BatchCount2 int = (select @tocSubCount2 + @tocsCount2 + @tocsrmCount2)

				--pull a BatchID
				declare @BatchID2 int
				insert into Circulation..Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated)
				values(@PublicationID,1,@BatchCount2,'true',GETDATE());set @BatchID = @@IDENTITY;  		
				
				Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
															  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
															  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
															  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,IsAddressValidated,DateCreated,SubscriberDateCreated,SubscriberCreatedByUserID,CreatedByUserID)


				select SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
					  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
					  DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,
					  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,'false',GETDATE(),GETDATE(),1,1
				from #tmpOriginalCS with(nolock)
				where PublicationID = @PubID1
				
				------------#tmpOriginalCSubscriber
				Insert Into Circulation..HistorySubscription (SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
															  IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,
															  SubscriptionDateCreated,SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,
															  AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,
															  
															  ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,
															  RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
															  AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,
															  Age,Income,Gender,SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,IsLocked,LockDate,LockDateRelease,
															  LockedByUserID,PhoneExt,DateCreated,CreatedByUserID)


				select cs.SubscriptionID,cs.SequenceID,cs.PublisherID,cs.SubscriberID,cs.PublicationID,cs.ActionID_Current,cs.ActionID_Previous,cs.SubscriptionStatusID,
					   cs.IsPaid,cs.QSourceID,cs.QSourceDate,cs.DeliverabilityID,cs.IsSubscribed,cs.SubscriberSourceCode,cs.Copies,cs.OriginalSubscriberSourceCode,
					   cs.DateCreated,cs.DateUpdated,cs.CreatedByUserID,cs.UpdatedByUserID,cs.AccountNumber,cs.GraceIssues,cs.OnBehalfOf,cs.MemberGroup,cs.Verify,cs.IsNewSubscription,

				ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,
						County,Country,CountryID,Latitude,Longitude,ISNULL(IsAddressValidated,'false'),AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,
						Fax,Mobile,Website,Birthdate,Age,Income,Gender,tocs.DateCreated,tocs.DateUpdated,tocs.CreatedByUserID,tocs.UpdatedByUserID,IsLocked,LockDate,LockDateRelease,
						LockedByUserID,PhoneExt,GETDATE(),1
				from #tmpOriginalCSubscriber tocs with(nolock)
				join Circulation..Subscription cs with(nolock) on tocs.SubscriberID = cs.SubscriberID 
				where PublicationID = @PubID1
				
				Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
				select @BatchID2,@@ROWCOUNT,t.PublisherID,t.PublicationID,t.SubscriberID,t.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
				from #tmpOriginalCS t with(nolock)
				join Circulation..HistorySubscription hs with(nolock) on t.SubscriptionID = hs.SubscriptionID 
				where PublicationID = @PubID1
				
				Insert Into Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
				select @BatchID,@@ROWCOUNT,cs.PublisherID,cs.PublicationID,cs.SubscriberID,cs.SubscriptionID,hs.HistorySubscriptionID,GETDATE(),1
				from #tmpOriginalCSubscriber sub with(nolock)
				join Circulation..Subscription cs with(nolock) on sub.SubscriberID = cs.SubscriberID 
				join Circulation..HistorySubscription hs with(nolock) on cs.SubscriptionID = hs.SubscriptionID 
				where cs.PublicationID = @PubID1
				
				fetch next from c into @PubID1,@PubCode1
			end
			close c
			deallocate c
			
			drop table #tmpOriginalCS
			drop table #tmpOriginalCSubscriber
		end
GO
PRINT N'Altering [dbo].[o_CountAddressValidation_IsLatLonValid]...';


GO
ALTER PROCEDURE o_CountAddressValidation_IsLatLonValid
@IsLatLonValid bit
AS
	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = @IsLatLonValid
GO
PRINT N'Altering [dbo].[o_CountAddressValidation_ProcessCode_IsLatLonValid]...';


GO
ALTER PROCEDURE o_CountAddressValidation_ProcessCode_IsLatLonValid
@ProcessCode varchar(50),
@IsLatLonValid bit
AS
	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE ProcessCode = @ProcessCode
	AND IsLatLonValid = @IsLatLonValid
GO
PRINT N'Altering [dbo].[o_CountAddressValidation_SourceFileID_IsLatLonValid]...';


GO
ALTER PROCEDURE o_CountAddressValidation_SourceFileID_IsLatLonValid
@SourceFileID int,
@IsLatLonValid bit
AS
	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID
	AND IsLatLonValid = @IsLatLonValid
GO
PRINT N'Altering [dbo].[o_CountForGeoCoding]...';


GO
ALTER PROCEDURE o_CountForGeoCoding
AS
	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE IsLatLonValid = 'false'
GO
PRINT N'Altering [dbo].[o_CountForGeoCoding_SourceFileID]...';


GO
ALTER PROCEDURE o_CountForGeoCoding_SourceFileID
@SourceFileID int
AS
	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID AND IsLatLonValid = 'false'
GO
PRINT N'Altering [dbo].[o_Get_FileMappingColumns]...';


GO
ALTER PROCEDURE [dbo].[o_Get_FileMappingColumns]
WITH EXECUTE AS OWNER
AS

SET NOCOUNT ON

SELECT 'DEMO' AS 'ColumnName', 'VARCHAR' AS 'DataType', CAST('true' AS BIT) AS 'IsDemographic'
UNION
SELECT 'PUBCODE', 'VARCHAR',CAST('false' AS BIT)
UNION
SELECT 'DEMO31', 'VARCHAR',CAST('false' AS BIT)
UNION
SELECT 'DEMO32', 'VARCHAR',CAST('false' AS BIT)
UNION
SELECT 'DEMO33', 'VARCHAR',CAST('false' AS BIT)
UNION
SELECT 'DEMO34', 'VARCHAR',CAST('false' AS BIT)
UNION
SELECT 'DEMO35', 'VARCHAR',CAST('false' AS BIT)
UNION
SELECT 'DEMO36', 'VARCHAR',CAST('false' AS BIT)
UNION
SELECT 'FULLNAME', 'VARCHAR',CAST('false' AS BIT)
UNION 
SELECT 'EMAILSTATUS' , 'VARCHAR',CAST('false' AS BIT)
UNION 
SELECT 'CTRY', 'INT',CAST('false' AS BIT)
UNION           
SELECT
	UPPER(sc.name), 
	UPPER(t.name),
	CAST('false' AS BIT)
FROM 
	SysObjects so
	INNER JOIN SysColumns sc ON so.id = sc.id
	INNER JOIN Sys.Types t ON t.system_type_id = sc.xtype
WHERE
	so.name = 'Subscriptions'
	AND LOWER(sc.name) NOT IN (
	'pubcode',
	'subscriptionid',
	'subscriberid', 
	'sourcefileid', 
	'clientid',
	'score',
	'ignore',
	'isdqmprocessfinished',
	'dqmprocessdate',
	'isupdatedinlive',
	'updateinlivedate', 
	'recordindentfier',
	--'sequence', 
	'countryid',
	'isexcluded',
	'pubid',
	'statlist',
	'igrp_no',
	'igrp_cnt',
	'cgrp_no',
	'cgrp_cnt',
	'priority',
	'sales',
	'source',
	'insuppression',
	'suppresseddate',
	'suppressedemail',
	'latitude',
	'longitude',
	'islatlonvalid',
	'latlonmsg',
	'verified',
	'employ', 
	'sales', 
	'statusupdateddate', 
	'statusupdatedreason',
	'igrp_rank', 
	'cgrp_rank', 
	'name', 
	'demo31', 
	'demo32',
	'demo33',
	'demo34',
	'demo35',
	'demo36')
UNION     
SELECT DISTINCT 
	UPPER(CustomField), 
	UPPER(CustomFieldDataType),
	CAST('true' AS BIT) 
FROM 
	SubscriptionsExtensionMapper
UNION           
SELECT DISTINCT 
	UPPER(ResponseGroupName), 
	UPPER(isc.DATA_TYPE),
	CAST('true' AS BIT) 
FROM 
	ResponseGroups
	JOIN INFORMATION_SCHEMA.COLUMNS isc ON UPPER(isc.COLUMN_NAME) = 'RESPONSEGROUPNAME' AND UPPER(isc.TABLE_NAME) = 'RESPONSEGROUPS' AND UPPER(ResponseGroupName) != 'PUBCODE'
ORDER BY 
	1
GO
PRINT N'Creating [dbo].[e_Adhoc_Delete]...';


GO
CREATE PROCEDURE [dbo].[e_Adhoc_Delete]
	@AdhocID int
AS
	DELETE Adhoc WHERE AdhocID = @AdhocID
GO
PRINT N'Creating [dbo].[e_Brand_Select]...';


GO
create procedure e_Brand_Select
as
	select *
	from Brand with(nolock)
	order by BrandName
GO
PRINT N'Creating [dbo].[e_Market_Select]...';


GO
create procedure e_Market_Select
as
	select *
	from Market with(nolock)
	order by MarketName
GO
PRINT N'Creating [dbo].[e_SubscriberTransformed_Select_ProcessCode_TopOne]...';


GO
CREATE PROCEDURE [dbo].[e_SubscriberTransformed_Select_ProcessCode_TopOne]
	@ProcessCode varchar(50)
AS
	SET NOCOUNT ON;
	select top 1 * from SubscriberTransformed where ProcessCode = @ProcessCode
GO
PRINT N'Creating [dbo].[job_CircSync_ClientID_ProcessCode]...';


GO
CREATE PROCEDURE [dbo].[job_CircSync_ClientID_ProcessCode]
	@ClientID int,
	@ProcessCode varchar(50)	
AS
BEGIN	
/*---- CREATE BATCH FOR RECORDS TO UPDATE (Written in case file has mulitple pubcodes which it shouldn't)-----*/
DECLARE @distPubs table ( PubCode varchar(50), PubID int, batchCount int )
DECLARE @pubidBatch table ( BatchID int, PubID int )

INSERT INTO @distPubs (PubCode, PubID, batchCount)
	Select A.PubCode, A.PubID, A.BatchCount from ( 
			Select SF.PubCode, UP.PubID, Count(*) as BatchCount from SubscriberFinal SF With(NoLock)
				join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
				join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
				join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
				join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
				join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
				join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
			where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1
			GROUP BY SF.PubCode, UP.PubID ) as A

DECLARE @userID int = 1
DECLARE @batchID int
INSERT INTO Circulation..Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
	OUTPUT INSERTED.BatchID, INSERTED.PublicationID INTO @pubidBatch
	Select PubID, @userID, batchCount, 'false', getdate(), getdate()
		from @distPubs where batchCount > 0

DECLARE @HistSubIDs Table
( HistorySubscriptionID int );

/* ---- UPDATE HISTORY OF CURRENT START (HistorySubscription, History, UserLog, HistoryToUserLog) ----*/
BEGIN
	INSERT INTO Circulation..HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
		IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
		SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
		OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
		Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
		AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
		SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
		LockDate,LockDateRelease,LockedByUserID,PhoneExt)	
	OUTPUT Inserted.HistorySubscriptionID into @HistSubIDs(HistorySubscriptionID)        
	SELECT 
		CSS.SubscriptionID,UP.ClientID,CSS.SubscriberID,UP.PubID,CSS.ActionID_Current,CSS.ActionID_Previous,CSS.SubscriptionStatusID,
		CSS.IsPaid,CSS.QSourceID,CSS.QSourceDate,CSS.DeliverabilityID,CSS.IsSubscribed,CSS.SubscriberSourceCode,CSS.Copies,CSS.OriginalSubscriberSourceCode,CSS.DateCreated,
		CSS.DateUpdated,CSS.CreatedByUserID,CSS.UpdatedByUserID,CSS.AccountNumber,CSS.GraceIssues,CSS.IsNewSubscription,CSS.MemberGroup,
		CSS.OnBehalfOf,CSS.Par3cID,CSS.SequenceID,CSS.SubsrcTypeID,CSS.Verify,CS.ExternalKeyID,CS.FirstName,CS.LastName,CS.Company,CS.Title,CS.Occupation,CS.AddressTypeID,CS.Address1,
		CS.Address2,CS.Address3,CS.City,CS.RegionCode,CS.RegionID,CS.ZipCode,CS.Plus4,CS.CarrierRoute,CS.County,CS.Country,CS.CountryID,CS.Latitude,CS.Longitude,CS.IsAddressValidated,
		CS.AddressValidationDate,CS.AddressValidationSource,CS.AddressValidationMessage,CS.Email,CS.Phone,CS.Fax,CS.Mobile,CS.Website,CS.Birthdate,CS.Age,CS.Income,CS.Gender,
		CS.DateCreated,CS.DateUpdated,CS.CreatedByUserID,CS.UpdatedByUserID,CS.DateCreated,CS.CreatedByUserID,CS.IsLocked,
		CS.LockDate,CS.LockDateRelease,CS.LockedByUserID,CS.PhoneExt
	FROM SubscriberFinal SF With(NoLock)
		join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
		join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
		join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
		join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
		join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
		join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
			where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1

	DECLARE @HSubID int	
	DECLARE c CURSOR
	FOR
		Select HistorySubscriptionID from @HistSubIDs
	OPEN c
	FETCH NEXT FROM c INTO @HSubID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @historyId int
		INSERT INTO Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID
							,HistoryPaidID,HistoryPaidBillToID,DateCreated,CreatedByUserID)
		Select PB.BatchID, DP.batchCount, HS.PublisherID, HS.PublicationID, HS.SubscriberID, HS.SubscriptionID, @HSubID, 0, 0, GETDATE(), @userID
			from Circulation..HistorySubscription HS 
				join @pubidBatch PB on HS.PublicationID = PB.PubID 
				join @distPubs DP on PB.PubID = DP.PubID
					where HistorySubscriptionID = @HSubID
		set @historyId = (select @@IDENTITY);

		declare @userLogTypeId int = (select CodeId from UAS..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
		declare @appId int = (select ApplicationID from UAS..Application with(nolock) where ApplicationName='Circulation')
		declare @userLogId int

		Insert Into UAS..UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
		Values(@appId,@userLogTypeId,@userID,'Subscriber','job_CircSync_ClientID_ProcessCode','',GETDATE());
		set @userLogId = (select @@IDENTITY);
							
		Insert Into Circulation..HistoryToUserLog (HistoryID,UserLogID)
		Values(@historyId,@userLogId);

		Insert Into UAS..UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
		Values(@appId,@userLogTypeId,@userID,'Subscription','job_CircSync_ClientID_ProcessCode','',GETDATE());
		set @userLogId = (select @@IDENTITY);
							
		Insert Into Circulation..HistoryToUserLog (HistoryID,UserLogID)
		Values(@historyId,@userLogId);
		FETCH NEXT FROM c INTO @HSubID
	END
	CLOSE c
	DEALLOCATE c
END
/*---- UPDATE HISTORY OF CURRENT END ----*/


/*---- UPDATE SUBSCRIBER AND SUBSCRIPTION START ----*/
DECLARE @CurSubscriberID int, @CurSubscriberFinalID int
DECLARE c CURSOR
FOR	
	Select CS.SubscriberID, SF.SubscriberFinalID 
	from SubscriberFinal SF With(NoLock)
		join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
		join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
		join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
		join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
		join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
		join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
		where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1
OPEN c
FETCH NEXT FROM c INTO @CurSubscriberID, @CurSubscriberFinalID
WHILE @@FETCH_STATUS = 0
BEGIN	
	Update Circulation..Subscriber
	set 
		FirstName = US.FNAME, LastName = US.LNAME, Company = US.Company, Title = US.Title, Address1 = US.[Address], Address2 = US.MailStop, Address3 = US.Address3,
		City = US.City, RegionCode = US.[State], RegionID = (Select RegionID from UAS..Region where RegionCode = US.[State]), ZipCode = US.Zip, Plus4 = US.Plus4, County = US.COUNTY, Country = US.COUNTRY, 
		CountryID = US.CountryID, Latitude = US.Latitude, Longitude = US.Longitude, IsAddressValidated = US.IsLatLonValid, 
		AddressValidationDate = US.AddressLastUpdatedDate, AddressValidationMessage = US.LatLonMsg, Email = US.Email, Phone = US.Phone, Fax = US.Fax, 
		Mobile = US.Mobile, DateUpdated = GETDATE(), UpdatedByUserID = 1
		--ExternalKeyID, Occupation, AddressTypeID, CarrierRoute, AddressValidationSource, Website, Birthdate, Age, Income, Gender, PhoneExt
		from SubscriberFinal SF With(NoLock)
			join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
			join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
			join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
			join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
			join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
			join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
			where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1 and CS.SubscriberID = @CurSubscriberID

	Update Circulation..Subscription
	set 
		ActionID_Current = (Select ActionID from Circulation..Action A where A.CategoryCodeID = US.CategoryID and A.TransactionCodeID = US.TransactionID and ActionTypeID = 1), 
		ActionID_Previous = CSS.ActionID_Current, QSourceID = US.QSourceID, QSourceDate = US.QDate, DeliverabilityID = US.Demo7,
		OriginalSubscriberSourceCode = US.Origssrc, Par3cID = US.Par3c
		--SubscriptionStatusID, IsPaid, IsSubscribed, SubscriberSourceCode, Copies, AccountNumber, GraceIssues, IsNewSubscription, MemberGroup, OnBehalfOf, 
		--SequenceID, SubsrcTypeID, Verify
		from SubscriberFinal SF With(NoLock)
			join Subscriptions US With(NoLock) on SF.IGrp_No = US.IGrp_No
			join Pubs UP With(NoLock) on SF.PubCode = UP.PubCode
			join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
			join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
			join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
			join Circulation..Publisher CP With(NoLock) on CPP.PublisherID = CP.PublisherID
			where SF.ProcessCode = @ProcessCode and SF.IsUpdatedInLive = 1 and CS.SubscriberID = @CurSubscriberID
FETCH NEXT FROM c INTO @CurSubscriberID, @CurSubscriberFinalID
END
CLOSE c
DEALLOCATE c
/*---- UPDATE SUBSCRIBER AND SUBSCRIPTION END ----*/


/*---- INSERT SUBSCRIBER AND SUBSCRIPTION START ----*/
DECLARE @InsertSubs table ( SubscriberID int, SFRecordIdentifier uniqueidentifier )
INSERT INTO Circulation..Subscriber (FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City
					,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated
					,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate
					,Age,Income,Gender,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,tmpSubscriptionID,IsLocked
					,LockedByUserID,LockDate,LockDateRelease,PhoneExt,IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate
					,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier)
OUTPUT Inserted.SubscriberID, Inserted.SFRecordIdentifier INTO @InsertSubs
Select S.FName,S.LName,S.Company,S.Title,NULL,NULL,S.[Address],S.MailStop,S.Address3,S.City,S.[State]
		,(Select RegionID from UAS..Region where RegionCode = S.[State]),S.Zip,S.Plus4,NULL,S.County,S.Country,S.CountryID,S.Latitude,S.Longitude
		,S.IsLatLonValid,NULL,NULL,S.LatLonMsg,S.Email,S.Phone,S.Fax,S.Mobile,NULL
		,NULL,NULL,NULL,S.Gender,ISNULL(S.DateCreated, GETDATE()),S.DateUpdated,ISNULL(S.CreatedByUserID, 1),S.UpdatedByUserID,NULL,'false'
		,NULL,NULL,NULL,NULL,'false',0,NULL
		,0,0,S.IGrp_No,SF.SFRecordIdentifier
	from SubscriberFinal SF With(NoLock) 
		join Subscriptions S With(NoLock) on SF.igrp_no = S.igrp_no 
		join Pubs P on P.pubcode = SF.pubcode		
			where (ProcessCode = @ProcessCode and isupdatedinlive = 1)		
and (not S.SEQUENCE > 0 OR NOT EXISTS (
	Select * from Circulation..Subscriber CS
		join Circulation..Subscription CSS on CS.SubscriberID = CSS.SubscriberID
		join Circulation..Publication CP on CP.PublicationID = CSS.PublicationID
		join Circulation..Publisher CPP on CPP.PublisherID = CP.PublisherID	
		WHERE CSS.SequenceID = S.SEQUENCE AND CP.PublicationCode = P.PubCode ))

DECLARE @InsertSubscription table ( SubscriptionID int, SubscriberID int )		
INSERT INTO Circulation..Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID
								,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,DateCreated,DateUpdated
								,CreatedByUserID,UpdatedByUserID,Par3cID,SubsrcTypeID,AccountNumber,GraceIssues,OnBehalfOf,MemberGroup,Verify)
OUTPUT Inserted.SubscriptionID, Inserted.SubscriberID INTO @InsertSubscription
Select (Select MAX(SequenceID) + 1 FROM Circulation..Subscription WHERE PublicationID = P.PubID),P.ClientID,INS.SubscriberID,P.PubID
								,(Select ActionID from Circulation..Action where CategoryCodeID = S.CategoryID and TransactionCodeID = S.TransactionID and ActionTypeID = 1)
								,0,NULL,0,S.QSourceID,S.QDate,S.Demo7,'false','',1,'',GETDATE(),NULL,1,NULL,S.Par3c,NULL,'',0,'','','' 
	from SubscriberFinal SF With(NoLock) 
	join Subscriptions S With(NoLock) on SF.igrp_no = S.igrp_no 
	join Pubs P With(NoLock) on P.pubcode = SF.pubcode
	join @InsertSubs INS on SF.SFRecordIdentifier = INS.SFRecordIdentifier

/*---- History Inserts ----**/
DECLARE @HistSubIDs2 table ( HistorySubscriptionID int )
INSERT INTO Circulation..HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
		IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
		SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
		OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
		Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
		AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
		SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
		LockDate,LockDateRelease,LockedByUserID,PhoneExt)	
	OUTPUT Inserted.HistorySubscriptionID into @HistSubIDs2(HistorySubscriptionID)        
	SELECT 
		CSS.SubscriptionID,UP.ClientID,CSS.SubscriberID,UP.PubID,CSS.ActionID_Current,CSS.ActionID_Previous,ISNULL(CSS.SubscriptionStatusID, 0),
		CSS.IsPaid,CSS.QSourceID,CSS.QSourceDate,CSS.DeliverabilityID,CSS.IsSubscribed,CSS.SubscriberSourceCode,CSS.Copies,CSS.OriginalSubscriberSourceCode,CSS.DateCreated,
		CSS.DateUpdated,CSS.CreatedByUserID,CSS.UpdatedByUserID,CSS.AccountNumber,CSS.GraceIssues,CSS.IsNewSubscription,CSS.MemberGroup,
		CSS.OnBehalfOf,CSS.Par3cID,CSS.SequenceID,CSS.SubsrcTypeID,CSS.Verify,CS.ExternalKeyID,CS.FirstName,CS.LastName,CS.Company,CS.Title,CS.Occupation,CS.AddressTypeID,CS.Address1,
		CS.Address2,CS.Address3,CS.City,CS.RegionCode,CS.RegionID,CS.ZipCode,CS.Plus4,CS.CarrierRoute,CS.County,CS.Country,CS.CountryID,CS.Latitude,CS.Longitude,CS.IsAddressValidated,
		CS.AddressValidationDate,CS.AddressValidationSource,CS.AddressValidationMessage,CS.Email,CS.Phone,CS.Fax,CS.Mobile,CS.Website,CS.Birthdate,CS.Age,CS.Income,CS.Gender,
		CS.DateCreated,CS.DateUpdated,CS.CreatedByUserID,CS.UpdatedByUserID,CS.DateCreated,CS.CreatedByUserID,CS.IsLocked,
		CS.LockDate,CS.LockDateRelease,CS.LockedByUserID,CS.PhoneExt
	FROM Circulation..Subscription CSS With(NoLock)
		join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID
		join Circulation..Publication CP With(NoLock) on CSS.PublicationID = CP.PublicationID
		join Pubs UP With(NoLock) on CP.PublicationCode = UP.PubCode
		join @InsertSubscription INS on INS.SubscriberID = CS.SubscriberID		
		
DECLARE @distPubs2 table ( PubCode varchar(50), PubID int, batchCount int )
DECLARE @pubidBatch2 table ( BatchID int, PubID int )

INSERT INTO @distPubs2 (PubCode, PubID, batchCount)
	Select A.PubCode, A.PubID, A.BatchCount from ( 
			Select SF.PubCode, UP.PubID, Count(*) as BatchCount from SubscriberFinal SF With(NoLock)	
				join Subscriptions US on US.IGRP_NO = SF.IGrp_No
				join Pubs UP on SF.PubCode = UP.PubCode
				join Circulation..Subscription CSS With(NoLock) on US.Sequence = CSS.SequenceID
				join Circulation..Subscriber CS With(NoLock) on CSS.SubscriberID = CS.SubscriberID				
				join Circulation..Publication CPP With(NoLock) on CPP.PublicationID = CSS.PublicationID and UP.PubCode = CPP.PublicationCode
				where CS.SubscriberID in (Select SubscriberID from @InsertSubs)
			GROUP BY SF.PubCode, UP.PubID ) as A

INSERT INTO Circulation..Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
OUTPUT INSERTED.BatchID, INSERTED.PublicationID INTO @pubidBatch2
Select PubID, @userID, batchCount, 'false', getdate(), getdate()
	from @distPubs2 where batchCount > 0

DECLARE @HistIDs table ( HistoryID int, SubscriberID int )
INSERT INTO Circulation..History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID
						,HistoryPaidID,HistoryPaidBillToID,DateCreated,CreatedByUserID)
	OUTPUT Inserted.HistoryID, Inserted.SubscriberID INTO @HistIDs
	Select PB.BatchID, DP.batchCount, HS.PublisherID, HS.PublicationID, HS.SubscriberID, HS.SubscriptionID, @HSubID, 0, 0, GETDATE(), @userID
		from Circulation..HistorySubscription HS 
			join @pubidBatch2 PB on HS.PublicationID = PB.PubID 
			join @distPubs2 DP on PB.PubID = DP.PubID
				where HistorySubscriptionID in (Select HistorySubscriptionID from @HistSubIDs2)


/*---- INSERT SUBSCRIBER AND SUBSCRIPTION END ----*/
END




/*---- Marketing Inserts ----*/
--DECLARE @InsertMarket table ( MarketingID int, SubscriberID int, PublicationID int, IsActive bit, DateCreated datetime, CreatedByUserID int )
--INSERT INTO Circulation..MarketingMap (SubscriberID,PublicationID,IsActive,DateCreated,CreatedByUserID)
--OUTPUT Inserted.MarketingID, Inserted.SubscriberID, Inserted.PublicationID, Inserted.IsActive, Inserted.DateCreated, Inserted.CreatedByUserID INTO Circulation..HistoryMarketingMap
--Select I.SubscriberID, P.PubID, 1, GetDate(), 1
--	from @InsertSubs I
--		join SubscriberFinal SF on SF.SFRecordIdentifier = I.SFRecordIdentifier
--		join Pubs P on SF.PubCode = P.PubCode
--		where SF.ProcessCode = @ProcessCode

--DECLARE @InsertHistMarket table ( HistoryMarketingMapID int, SubscriberID int )
--INSERT INTO Circulation..HistoryMarketingMap (MarketingID, SubscriberID, PublicationID, IsActive, DateCreated, CreatedByUserID)
--	OUTPUT Inserted.HistoryMarketingMapID, Inserted.SubscriberID INTO @InsertHistMarket
--	Select MarketingID, SubscriberID, PublicationID, IsActive, DateCreated, CreatedByUserID from @InsertMarket

--INSERT INTO Circulation..HistoryToHistoryMarketingMap (HistoryID, HistoryMarketingMapID)
--	Select HID.HistoryID, IHM.HistoryMarketingMapID from @InsertHistMarket IHM
--		join @HistIDs HID on IHM.SubscriberID = HID.SubscriberID
GO
PRINT N'Creating [dbo].[o_SubscriberConsensus_Select_Email]...';


GO
create procedure o_SubscriberConsensus_Select_Email
@Email varchar(100)
as
	select Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,Phone,Fax,Demo31,Demo32,Demo33,Demo34,Demo35,Demo36,
		   Gender,Address3,Home_Work_Address,Mobile,Score,Latitude,Longitude,Demo7,IGrp_No,Par3C,TransactionDate,
		   QDate,Email,SubscriptionID,IsActive
	from Subscriptions with(nolock)
	where Email = @Email
GO
PRINT N'Creating [dbo].[o_SubscriberProduct_Select_Email_ProductCode]...';


GO
create procedure o_SubscriberProduct_Select_Email_ProductCode
@Email varchar(100),
@ProductCode varchar(50) 
as
	select s.Sequence,s.FName,s.LName,s.Title,s.Company,s.Address,s.MailStop,s.City,s.State,s.Zip,s.Plus4,s.ForZip,s.County,s.Country,s.Phone,s.Fax,
		   s.Demo31,s.Demo32,s.Demo33,s.Demo34,s.Demo35,s.Demo36,s.Gender,s.Address3,Home_Work_Address,s.Mobile,s.Score,
		   s.Latitude,s.Longitude,s.Demo7,s.IGrp_No,s.Par3C,s.TransactionDate,s.QDate,s.Email,s.SubscriptionID,s.IsActive
	from Subscriptions s with(nolock)
	join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID 
	join Pubs p with(nolock) on ps.PubID = p.PubID
	where ps.Email = @Email
	and p.PubCode = @ProductCode
GO
PRINT N'Refreshing [dbo].[spUpdateSuppression_XML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateSuppression_XML]';


GO
PRINT N'Refreshing [dbo].[sp_GetSelectedSubscriberCount]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSelectedSubscriberCount]';


GO
PRINT N'Refreshing [dbo].[e_ImportFromUAS]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ImportFromUAS]';


GO
PRINT N'Refreshing [dbo].[e_MasterCodeSheet_Import_Subscriber]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_MasterCodeSheet_Import_Subscriber]';


GO
PRINT N'Refreshing [dbo].[e_OrderDetails_Select_UserID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_OrderDetails_Select_UserID]';


GO
PRINT N'Refreshing [dbo].[e_ProductSubscription_Select_PubCode_EmailAddress]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ProductSubscription_Select_PubCode_EmailAddress]';


GO
PRINT N'Refreshing [dbo].[e_ShoppingCarts_Select_UserID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ShoppingCarts_Select_UserID]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Insert_Profile]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Insert_Profile]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_Email]';


GO
PRINT N'Refreshing [dbo].[e_Subscription_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscription_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_GetInValidLatLon]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_GetInValidLatLon]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_Insert_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_Insert_Email]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_Select_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_Select_Email]';


GO
PRINT N'Refreshing [dbo].[e_Subscriptions_UpdateLatLon]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Subscriptions_UpdateLatLon]';


GO
PRINT N'Refreshing [dbo].[job__ADMS_Remove_By_ProcessCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job__ADMS_Remove_By_ProcessCode]';


GO
PRINT N'Refreshing [dbo].[job_ACS_Update_UAS_SubscriberAddress]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_ACS_Update_UAS_SubscriberAddress]';


GO
PRINT N'Refreshing [dbo].[job_ADMS_Remove_By_PubCode]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_ADMS_Remove_By_PubCode]';


GO
PRINT N'Refreshing [dbo].[job_NCOA_AddressUpdate]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_NCOA_AddressUpdate]';


GO
PRINT N'Refreshing [dbo].[job_PTNThirdPartyBTNExport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_PTNThirdPartyBTNExport]';


GO
PRINT N'Refreshing [dbo].[job_Subscriptions_ImportSubscribe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_Subscriptions_ImportSubscribe]';


GO
PRINT N'Refreshing [dbo].[job_Subscriptions_ImportUnsubscribe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[job_Subscriptions_ImportUnsubscribe]';


GO
PRINT N'Refreshing [dbo].[o_Subscriber_Select_Email]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_Subscriber_Select_Email]';


GO
PRINT N'Refreshing [dbo].[o_Subscriber_Select_SubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_Subscriber_Select_SubscriptionID]';


GO
PRINT N'Refreshing [dbo].[sp_getFullSubscribers_using_ReportFilters]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getFullSubscribers_using_ReportFilters]';


GO
PRINT N'Refreshing [dbo].[sp_getOpportunity]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getOpportunity]';


GO
PRINT N'Refreshing [dbo].[sp_GetRecordsDownload]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetRecordsDownload]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberByGL]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberByGL]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberCountByRadius]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberCountByRadius]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberDimension]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberDimension]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberDimensionForExport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberDimensionForExport]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberGLByIGRPNO]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberGLByIGRPNO]';


GO
PRINT N'Refreshing [dbo].[sp_GetSubscriberGLBySubscriptionID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_GetSubscriberGLBySubscriptionID]';


GO
PRINT N'Refreshing [dbo].[sp_getSubscribers_using_ReportFiltersXml]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_getSubscribers_using_ReportFiltersXml]';


GO
PRINT N'Refreshing [dbo].[SP_IMPORT_SUBSCRIBER_MASTERCODESHEET]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[SP_IMPORT_SUBSCRIBER_MASTERCODESHEET]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_by_country]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_by_country]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_Canada]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_Canada]';


GO
PRINT N'Refreshing [dbo].[sp_rpt_Qualified_Breakdown_domestic]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_rpt_Qualified_Breakdown_domestic]';


GO
PRINT N'Refreshing [dbo].[sp_SaveSubscriptions]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SaveSubscriptions]';


GO
PRINT N'Refreshing [dbo].[sp_Subscriber_Codesheet_counts]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Subscriber_Codesheet_counts]';


GO
PRINT N'Refreshing [dbo].[sp_Subscriber_MasterCodesheet_counts]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_Subscriber_MasterCodesheet_counts]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriberActivity]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriberActivity]';


GO
PRINT N'Refreshing [dbo].[sp_SubscriberScores]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SubscriberScores]';


GO
PRINT N'Refreshing [dbo].[sp_SummaryReport]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[sp_SummaryReport]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart5]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart5]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart6]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart6]';


GO
PRINT N'Refreshing [dbo].[spDownloaddetailsResponseCount]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDownloaddetailsResponseCount]';


GO
PRINT N'Refreshing [dbo].[spUpdateBrandScore]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateBrandScore]';


GO
PRINT N'Refreshing [dbo].[spUpdateSuppression]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spUpdateSuppression]';


GO
PRINT N'Refreshing [dbo].[Usp_UpdateSuppressedEmailFromSubRecord]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[Usp_UpdateSuppressedEmailFromSubRecord]';


GO
PRINT N'Refreshing [dbo].[spDataRefreshPart7]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[spDataRefreshPart7]';


GO
PRINT N'Refreshing [dbo].[ccp_Canon_ConsensusDim_EventSwipe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[ccp_Canon_ConsensusDim_EventSwipe]';


GO
PRINT N'Refreshing [dbo].[ccp_SpecialityFoods_ConsensusDim_EventSwipe]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[ccp_SpecialityFoods_ConsensusDim_EventSwipe]';


GO
PRINT N'Refreshing [dbo].[o_ConsensusDimension_SaveXML]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_ConsensusDimension_SaveXML]';


GO
PRINT N'Update complete.';


GO


