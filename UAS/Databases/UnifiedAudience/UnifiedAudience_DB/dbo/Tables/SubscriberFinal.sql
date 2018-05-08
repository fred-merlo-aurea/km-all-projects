CREATE TABLE [dbo].[SubscriberFinal] (
    [SubscriberFinalID]    INT              IDENTITY (1, 1) NOT NULL,
    [STRecordIdentifier]   UNIQUEIDENTIFIER NOT NULL,
    [SourceFileID]         INT              NULL,
    [PubCode]              VARCHAR (100)    NULL,
    [Sequence]             INT              NOT NULL,
    [FName]                VARCHAR (100)    NULL,
    [LName]                VARCHAR (100)    NULL,
    [Title]                VARCHAR (100)    NULL,
    [Company]              VARCHAR (100)    NULL,
    [Address]              VARCHAR (255)    NULL,
    [MailStop]             VARCHAR (255)    NULL,
    [City]                 VARCHAR (50)     NULL,
    [State]                VARCHAR (50)     NULL,
    [Zip]                  VARCHAR (50)     NULL,
    [Plus4]                VARCHAR (50)     NULL,
    [ForZip]               VARCHAR (50)     NULL,
    [County]               VARCHAR (100)    NULL,
    [Country]              VARCHAR (100)    NULL,
    [CountryID]            INT              NULL,
    [Phone]                VARCHAR (100)    NULL,
    [Fax]                  VARCHAR (100)    NULL,
    [Email]                VARCHAR (100)    NULL,
    [CategoryID]           INT              NULL,
    [TransactionID]        INT              NULL,
    [TransactionDate]      SMALLDATETIME    NULL,
    [QDate]                DATETIME         NULL,
    [QSourceID]            INT              NULL,
    [RegCode]              VARCHAR (5)      NULL,
    [Verified]             VARCHAR (100)      NULL,
    [SubSrc]               VARCHAR (25)     NULL,
    [OrigsSrc]             VARCHAR (25)     NULL,
    [Par3C]                VARCHAR (10)      NULL,    
    [Source]               VARCHAR (50)     NULL,
    [Priority]             VARCHAR (4)      NULL,
    [IGrp_No]              UNIQUEIDENTIFIER NULL,
    [IGrp_Cnt]             INT              NULL,
    [CGrp_No]              UNIQUEIDENTIFIER NULL,
    [CGrp_Cnt]             INT              NULL,
    [StatList]             BIT              NULL,
    [Sic]                  VARCHAR (8)      NULL,
    [SicCode]              VARCHAR (20)     NULL,
    [Gender]               VARCHAR (1024)   NULL,
    [IGrp_Rank]            VARCHAR (2)      NULL,
    [CGrp_Rank]            VARCHAR (2)      NULL,
    [Address3]             VARCHAR (255)    NULL,
    [Home_Work_Address]    VARCHAR (10)     NULL,
    [Demo7]                VARCHAR (1)      NULL,
    [Mobile]               VARCHAR (30)     NULL,
    [Latitude]             DECIMAL (18, 15) NULL,
    [Longitude]            DECIMAL (18, 15) NULL,
    [IsLatLonValid]        BIT              NULL,
    [LatLonMsg]            NVARCHAR (500)   NULL,
    [EmailStatusID]        INT              NULL,
    [Ignore]               BIT              NOT NULL,
    [IsDQMProcessFinished] BIT              NOT NULL,
    [DQMProcessDate]       DATETIME         NULL,
    [IsUpdatedInLive]      BIT              NOT NULL,
    [UpdateInLiveDate]     DATETIME         NULL,
    [SFRecordIdentifier]   UNIQUEIDENTIFIER NOT NULL,
    [DateCreated]          DATETIME         CONSTRAINT [DF_SubscriberFinal_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]          DATETIME         NULL,
    [CreatedByUserID]      INT              NULL,
    [UpdatedByUserID]      INT              NULL,
    [IsMailable]           BIT              NULL,
    [ProcessCode]          VARCHAR (50)     NULL,
    [ImportRowNumber]      INT              NULL,
    [IsActive]             BIT              NULL,
	[ExternalKeyId]		   INT			    NULL,
	[AccountNumber]		   VARCHAR (50)	    NULL,
	[EmailID]		       INT			    NULL,
	[Copies]			   INT				NULL,	
	[GraceIssues]          INT				NULL,
	[IsComp]			   BIT				NULL DEFAULT ((0)),
	[IsPaid]			   BIT				NULL,
	[IsSubscribed]		   BIT				NULL,	
	[Occupation]		   VARCHAR(50)		NULL,
	[SubscriptionStatusID] INT				NULL,
	[SubsrcID]			   INT				NULL,
	[Website]			   VARCHAR(255)		NULL,
	[MailPermission]	   BIT				NULL, 
    [FaxPermission]		   BIT				NULL, 
    [PhonePermission]	   BIT				NULL, 
    [OtherProductsPermission] BIT			NULL, 
    [ThirdPartyPermission]	  BIT			NULL, 
    [EmailRenewPermission]    BIT			NULL, 
    [TextPermission]		  BIT			NULL,
	[SubGenSubscriberID]	  INT			NULL,
	[SubGenSubscriptionID]	  INT			NULL,
	[SubGenPublicationID]	  INT			NULL,
	[SubGenMailingAddressId]  INT			NULL,
	[SubGenBillingAddressId]  INT			NULL,
	[IssuesLeft]			  INT			NULL,
	[UnearnedReveue]		  MONEY			NULL,
	[SubGenIsLead] BIT NULL,
	[SubGenRenewalCode] VARCHAR(50) NULL,
	[SubGenSubscriptionRenewDate] DATE NULL,
	[SubGenSubscriptionExpireDate] DATE NULL,
	[SubGenSubscriptionLastQualifiedDate] DATE NULL,
	[ConditionApplied]	BIT	default((0)) NULL,
	[IsNewRecord]		BIT default((0)) NULL,
    CONSTRAINT [PK_SubscriberFinal] PRIMARY KEY CLUSTERED ([SFRecordIdentifier] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_AccountNumber]
    ON [dbo].[SubscriberFinal]([AccountNumber] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_Address]
    ON [dbo].[SubscriberFinal]([Address] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_Email]
    ON [dbo].[SubscriberFinal]([Email] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_IGrp_No]
    ON [dbo].[SubscriberFinal]([IGrp_No] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_Phone]
    ON [dbo].[SubscriberFinal]([Phone] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_ProcessCode]
    ON [dbo].[SubscriberFinal]([ProcessCode] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_ProcessCode_SourceFileID_SFRecordIdentifier]
    ON [dbo].[SubscriberFinal]([ProcessCode] ASC, [SourceFileID] ASC, [SFRecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_PubCode]
    ON [dbo].[SubscriberFinal]([PubCode] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_SourceFileId]
    ON [dbo].[SubscriberFinal]([SourceFileID] ASC)
    INCLUDE([MailPermission], [FaxPermission], [PhonePermission], [OtherProductsPermission], [ThirdPartyPermission], [EmailRenewPermission], [TextPermission]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberFinal_STRecordIdentifier]
    ON [dbo].[SubscriberFinal]([STRecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_SubscriberFinal_SubscriberFinalID]
    ON [dbo].[SubscriberFinal]([SubscriberFinalID] ASC) WITH (FILLFACTOR = 90);
GO
