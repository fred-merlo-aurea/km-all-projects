CREATE TABLE [dbo].[PubSubscriptions] (
    [PubSubscriptionID]   INT           IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]      INT           NOT NULL,
    [PubID]               INT           NOT NULL,
    [demo7]               VARCHAR (1)   NULL,
    [Qualificationdate]   DATE          NULL,
    [PubQSourceID]        INT           NULL,
    [PubCategoryID]       INT           NULL,
    [PubTransactionID]    INT           NULL,
    [EmailStatusID]       INT           NULL,
    [StatusUpdatedDate]   DATETIME      NULL,
    [StatusUpdatedReason] VARCHAR (200) NULL,
    [Email]               VARCHAR (100) NULL,
	[DateCreated]        DATETIME    NULL DEFAULT (getdate()),
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NULL,
    [UpdatedByUserID]    INT              NULL,
	[IsComp]			 BIT			  NULL DEFAULT ((0)),
	[SubscriptionStatusID] INT			  NULL DEFAULT ((1)),
	[AccountNumber]			varchar(50) 	NULL,
	[AddRemoveID]			INT			NULL DEFAULT ((0)),
	[Copies]				INT			NULL DEFAULT ((1)),
	[GraceIssues]			INT			NULL DEFAULT ((0)),
	[IMBSEQ]				VARCHAR(256) NULL,
	[IsActive]				BIT NULL,
	[IsPaid]				BIT NULL,
	[IsSubscribed]			BIT NULL,
	[MemberGroup]			VARCHAR(256) NULL,
	[OnBehalfOf]			VARCHAR(256) null,
	[OrigsSrc]				VARCHAR(256) null,
	[Par3CID]				INT null,
	[SequenceID]			INT NULL,
	[Status]				VARCHAR(50) NULL,
	[SubscriberSourceCode]		VARCHAR(100) NULL,
	[SubSrcID]				INT NULL,
	[Verify]				varchar(100) null,
	[ExternalKeyID]            INT              NULL,
    [FirstName]                VARCHAR (50)     NULL,
    [LastName]                 VARCHAR (50)     NULL,
    [Company]                  VARCHAR (100)    NULL,
    [Title]                    VARCHAR (255)    NULL,
    [Occupation]               VARCHAR (50)     NULL,
    [AddressTypeID]            INT              NULL,
    [Address1]                 VARCHAR (256)    NULL,
    [Address2]                 VARCHAR (256)    NULL,
	[Address3]                 VARCHAR (256)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [RegionCode]               VARCHAR (50)     NULL,
    [RegionID]                 INT              NULL,
    [ZipCode]                  VARCHAR (50)     NULL,
    [Plus4]                    VARCHAR (10)        NULL,
    [CarrierRoute]             VARCHAR (10)     NULL,
    [County]                   VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [CountryID]                INT              NULL,
    [Latitude]                 DECIMAL (18, 15) NULL,
    [Longitude]                DECIMAL (18, 15) NULL,
    [IsAddressValidated]       BIT              CONSTRAINT [DF_Subscriber_IsAddressValidated] DEFAULT ((0)) NULL,
    [AddressValidationDate]    DATETIME         NULL,
    [AddressValidationSource]  VARCHAR (50)     NULL,
    [AddressValidationMessage] VARCHAR (MAX)    NULL,
    [Phone]                    VARCHAR (100)     NULL,
    [Fax]                      VARCHAR (100)     NULL,
    [Mobile]                   VARCHAR (100)     NULL,
    [Website]                  VARCHAR (255)    NULL,
    [Birthdate]                DATE             NULL,
    [Age]                      INT              NULL,
    [Income]                   VARCHAR (50)     NULL,
    [Gender]                   VARCHAR (50)     NULL,
    [tmpSubscriptionID]        INT              NULL,
    [IsLocked]                 BIT              DEFAULT ((0)) NULL,
	[LockedByUserID]		   INT				NULL,
	[LockDate]				   DATETIME			NULL,
	[LockDateRelease]		   DATETIME         NULL,
    [PhoneExt]				   VARCHAR(25)	    NULL, 
    [IsInActiveWaveMailing]	   BIT				DEFAULT ((0)) NULL,
	AddressTypeCodeId int null,
	AddressLastUpdatedDate datetime null,
	AddressUpdatedSourceTypeCodeId int null,
    [WaveMailingID] INT NULL, 
    [IGrp_No] UNIQUEIDENTIFIER NULL, 
    [SFRecordIdentifier] UNIQUEIDENTIFIER NULL, 
    [ReqFlag] INT NULL DEFAULT ((0)), 
    [PubTransactionDate] DATETIME NULL, 
	[EmailID] INT NULL,
	[SubGenSubscriberID] INT NULL DEFAULT ((0)), 
    [MailPermission] BIT NULL, 
    [FaxPermission] BIT NULL, 
    [PhonePermission] BIT NULL, 
    [OtherProductsPermission] BIT NULL, 
    [ThirdPartyPermission] BIT NULL, 
    [EmailRenewPermission] BIT NULL, 
    [TextPermission] BIT NULL, 
	[SubGenSubscriptionID] INT NULL DEFAULT ((0)), 
	[SubGenPublicationID] INT NULL DEFAULT ((0)), 
	[SubGenMailingAddressId] INT NULL DEFAULT ((0)), 
	[SubGenBillingAddressId] INT NULL DEFAULT ((0)), 
	[IssuesLeft] INT NULL DEFAULT ((0)), 
	[UnearnedReveue] MONEY NULL DEFAULT ((0)), 
	[SubGenIsLead] BIT NULL,
	[SubGenRenewalCode] VARCHAR(50) NULL,
	[SubGenSubscriptionRenewDate] DATE NULL,
	[SubGenSubscriptionExpireDate] DATE NULL,
	[SubGenSubscriptionLastQualifiedDate] DATE NULL,
    CONSTRAINT [PK_PubSubscriptions] PRIMARY KEY CLUSTERED ([PubSubscriptionID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PubSubscriptions_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID]),
    CONSTRAINT [FK_PubSubscriptions_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID])
);
GO

CREATE NONCLUSTERED INDEX [idx_PubSubscriptions_AccountNumber]
    ON [dbo].[PubSubscriptions]([AccountNumber] ASC) WITH (FILLFACTOR = 90);
GO

-- Misspelled correction
--CREATE UNIQUE NONCLUSTERED INDEX [IDX_PubSubscriptions_AubscriptionID_PubID]
--    ON [dbo].[PubSubscriptions]([SubscriptionID] ASC, [PubID] ASC) WITH (FILLFACTOR = 90);
--GO

-- replaced with index with includes
--CREATE NONCLUSTERED INDEX [IDX_PubSubscriptions_Email]
--    ON [dbo].[PubSubscriptions]([Email] ASC) WITH (FILLFACTOR = 90);
--GO

CREATE NONCLUSTERED INDEX [IDX_PubSubscriptions_Email_includes] 
		ON [dbo].[PubSubscriptions] ([Email]) INCLUDE ([FirstName], [LastName], [Company], [Address1], [Phone], [IGrp_No])  WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [idx_PubSubscriptions_pubID_SubscriptionID]
    ON [dbo].[PubSubscriptions]([PubID] ASC)
    INCLUDE([SubscriptionID]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_PubSubscriptions_SequenceID_PubID]
    ON [dbo].[PubSubscriptions]([SequenceID] ASC, [PubID] ASC) WITH (FILLFACTOR = 90);
GO

-- replaced with index with includes
--CREATE NONCLUSTERED INDEX [IDX_PubSubscriptions_SubscriptionID]
--    ON [dbo].[PubSubscriptions]([SubscriptionID] ASC) WITH (FILLFACTOR = 90);
--GO

CREATE UNIQUE INDEX [IDX_PubSubscriptions_SubscriptionID_PubID] 
	ON [dbo].[PubSubscriptions] ([SubscriptionID], [PubID]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Pubsubscriptions_Address1_includes] 
	ON [dbo].[PubSubscriptions] ([Address1]) INCLUDE ([FirstName], [LastName], [Company], [Title], [IGrp_No]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Pubsubscriptions_FirstName_LastName_includes] 
	ON [dbo].[PubSubscriptions] ([FirstName], [LastName]) INCLUDE ([Email], [Company], [Address1], [Phone], [IGrp_No])  WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [IDX_Pubsubscriptions_IgrpNo] 
	ON [dbo].[PubSubscriptions] ([IGrp_No])  WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [IDX_Pubsubscriptions_Phone_includes] 
	ON [dbo].[PubSubscriptions] ([Phone]) INCLUDE ([FirstName], [LastName], [IGrp_No]) WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [IDX_Pubsubscriptions_qualificationDate_includes_EmailStatusID_SubscriptionID] 
	ON [dbo].[PubSubscriptions] ([Qualificationdate], [PubID], [EmailStatusID]) INCLUDE ([SubscriptionID], [PubSubscriptionID]) WITH (FILLFACTOR = 90);
GO

CREATE INDEX [IDX_Pubsubscriptions_EmailStatusID_includes] ON [dbo].[PubSubscriptions] ([EmailStatusID]) INCLUDE ([SubscriptionID])  WITH (FILLFACTOR = 90);
GO
CREATE STATISTICS [STAT_Pubsubscriptions_SubscriptionID_EmailStatusID] ON [dbo].[PubSubscriptions] ([SubscriptionID], [EmailStatusID])