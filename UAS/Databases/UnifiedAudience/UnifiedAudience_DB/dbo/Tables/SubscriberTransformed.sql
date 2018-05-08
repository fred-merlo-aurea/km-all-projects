CREATE TABLE [dbo].[SubscriberTransformed] (
    [SubscriberTransformedID] INT              IDENTITY (1, 1) NOT NULL,
    [SORecordIdentifier]      UNIQUEIDENTIFIER NOT NULL,
    [SourceFileID]            INT              NOT NULL,
    [PubCode]                 VARCHAR (100)    NULL,
    [Sequence]                INT              NOT NULL,
    [FName]                   VARCHAR (100)    NULL,
    [LName]                   VARCHAR (100)    NULL,
    [Title]                   VARCHAR (100)    NULL,
    [Company]                 VARCHAR (100)    NULL,
    [Address]                 VARCHAR (255)    NULL,
    [MailStop]                VARCHAR (255)    NULL,
    [City]                    VARCHAR (50)     NULL,
    [State]                   VARCHAR (50)     NULL,
    [Zip]                     VARCHAR (50)     NULL,
    [Plus4]                   VARCHAR (50)     NULL,
    [ForZip]                  VARCHAR (50)     NULL,
    [County]                  VARCHAR (100)    NULL,
    [Country]                 VARCHAR (100)    NULL,
    [CountryID]               INT              NULL,
    [Phone]                   VARCHAR (100)    NULL,
    [Fax]                     VARCHAR (100)    NULL,
    [Email]                   VARCHAR (100)    NULL,
    [CategoryID]              INT              NULL,
    [TransactionID]           INT              NULL,
    [TransactionDate]         DATETIME         NULL,
    [QDate]                   DATETIME         NULL,
    [QSourceID]               INT              NULL,
    [RegCode]                 VARCHAR (5)      NULL,
    [Verified]                VARCHAR (100)      NULL,
    [SubSrc]               VARCHAR (25)     NULL,
    [OrigsSrc]             VARCHAR (25)     NULL,
    [Par3C]                   VARCHAR (10)      NULL,    
    [Source]                  VARCHAR (50)     NULL,
    [Priority]                VARCHAR (4)      NULL,
    [Sic]                     VARCHAR (8)      NULL,
    [SicCode]                 VARCHAR (20)     NULL,
    [Gender]                  VARCHAR (1024)   NULL,
    [Address3]                VARCHAR (255)    NULL,
    [Home_Work_Address]       VARCHAR (10)     NULL,
    [Demo7]                   VARCHAR (1)      NULL,
    [Mobile]                  VARCHAR (30)     NULL,
    [Latitude]                DECIMAL (18, 15) NULL,
    [Longitude]               DECIMAL (18, 15) NULL,
    [IsLatLonValid]           BIT              NULL,
    [LatLonMsg]               NVARCHAR (500)   NULL,
    [EmailStatusID]           INT              NULL,
    [STRecordIdentifier]      UNIQUEIDENTIFIER NOT NULL,
    [DateCreated]             DATETIME         CONSTRAINT [DF_SubscriberTransformed_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]             DATETIME         NULL,
    [CreatedByUserID]         INT              NULL,
    [UpdatedByUserID]         INT              NULL,
    [ProcessCode]             VARCHAR (50)     NULL,
    [ImportRowNumber]         INT              NULL,
    [IsActive]                BIT              NULL,
	[ExternalKeyId]			  INT			   NULL,
	[AccountNumber]		      VARCHAR (50)	   NULL,
	[EmailID]			      INT			   NULL,
	[Copies]				  INT			   NULL,	
	[GraceIssues]             INT			   NULL,
	[IsComp]			      BIT			   NULL,
	[IsPaid]			      BIT			   NULL,
	[IsSubscribed]		      BIT			   NULL,	
	[Occupation]		      VARCHAR(50)	   NULL,
	[SubscriptionStatusID]    INT			   NULL,
	[SubsrcID]			      INT			   NULL,
	[Website]			      VARCHAR(255)	   NULL,
	[MailPermission]		  BIT			   NULL, 
    [FaxPermission]			  BIT			   NULL, 
    [PhonePermission]		  BIT			   NULL, 
    [OtherProductsPermission] BIT			   NULL, 
    [ThirdPartyPermission]	  BIT			   NULL, 
    [EmailRenewPermission]    BIT			   NULL, 
    [TextPermission]		  BIT			   NULL,
	[SubGenSubscriberID]	  INT			   NULL,
	[SubGenSubscriptionID]	  INT			   NULL,
	[SubGenPublicationID]	  INT			   NULL,
	[SubGenMailingAddressId]  INT			   NULL,
	[SubGenBillingAddressId]  INT			   NULL,
	[IssuesLeft]			  INT			   NULL,
	[UnearnedReveue]		  MONEY			   NULL,
	[SubGenIsLead] BIT NULL,
	[SubGenRenewalCode] VARCHAR(50) NULL,
	[SubGenSubscriptionRenewDate] DATE NULL,
	[SubGenSubscriptionExpireDate] DATE NULL,
	[SubGenSubscriptionLastQualifiedDate] DATE NULL,
    CONSTRAINT [PK_SubscriberTransformed] PRIMARY KEY CLUSTERED ([STRecordIdentifier] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IDX_Address]
    ON [dbo].[SubscriberTransformed]([Address] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Email]
    ON [dbo].[SubscriberTransformed]([Email] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Phone]
    ON [dbo].[SubscriberTransformed]([Phone] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_PubCode]
    ON [dbo].[SubscriberTransformed]([PubCode] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SourceFileID]
    ON [dbo].[SubscriberTransformed]([SourceFileID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberTransformed_ProcessCode]
    ON [dbo].[SubscriberTransformed]([ProcessCode] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberTransformed_ProcessCode_SORecordIdentifier]
    ON [dbo].[SubscriberTransformed]([ProcessCode] ASC, [SORecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberTransformed_SORecordIdentifier]
    ON [dbo].[SubscriberTransformed]([SORecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberTransformed_SourceFileID_ProcessCode_STRecordIdentifier_SORecordIdentifier]
    ON [dbo].[SubscriberTransformed]([SourceFileID] ASC, [ProcessCode] ASC, [STRecordIdentifier] ASC, [SORecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberTransformed_STRecordIdentifier_SourceFileID_ProcessCode_ImportRowNumber]
    ON [dbo].[SubscriberTransformed]([STRecordIdentifier] ASC, [SourceFileID] ASC, [ProcessCode] ASC, [ImportRowNumber] ASC) WITH (FILLFACTOR = 90);
GO

CREATE STATISTICS [STAT_SubscriberTransformed_PC_STRec_SFID_SORec]
    ON [dbo].[SubscriberTransformed]([ProcessCode], [STRecordIdentifier], [SourceFileID], [SORecordIdentifier]);
GO

CREATE STATISTICS [STAT_SubscriberTransformed_SFID_STRec]
    ON [dbo].[SubscriberTransformed]([SourceFileID], [STRecordIdentifier]);
GO

CREATE STATISTICS [STAT_SubscriberTransformed_SORec_PC_SFID]
    ON [dbo].[SubscriberTransformed]([SORecordIdentifier], [ProcessCode], [SourceFileID]);
GO

CREATE STATISTICS [STAT_SubscriberTransformed_SORec_STRec_SFID]
    ON [dbo].[SubscriberTransformed]([SORecordIdentifier], [STRecordIdentifier], [SourceFileID]);
GO

CREATE STATISTICS [STAT_SubscriberTransformed_SourceFileID_ProcessCode_ImportRowNumber]
    ON [dbo].[SubscriberTransformed]([ImportRowNumber], [ProcessCode], [SourceFileID]);
GO

CREATE STATISTICS [STAT_SubscriberTransformed_SourceFileID_STRecordIdentifier_ProcessCode_ImportRowNumber]
    ON [dbo].[SubscriberTransformed]([SourceFileID], [STRecordIdentifier], [ProcessCode], [ImportRowNumber]);
GO

CREATE STATISTICS [STAT_SubscriberTransformed_STRec_PC_SORec]
    ON [dbo].[SubscriberTransformed]([STRecordIdentifier], [ProcessCode], [SORecordIdentifier]);
GO