
CREATE TABLE [dbo].[SubscriberArchive] (
    [SubscriberArchiveID]  INT              IDENTITY (1, 1) NOT NULL,
    [SFRecordIdentifier]   UNIQUEIDENTIFIER NOT NULL,
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
    [PhoneExists]          BIT              NULL,
    [Fax]                  VARCHAR (100)    NULL,
    [FaxExists]            BIT              NULL,
    [Email]                VARCHAR (100)    NULL,
    [EmailExists]          BIT              NULL,
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
    [EmailStatusID]        INT              NULL,
    [SARecordIdentifier]   UNIQUEIDENTIFIER NOT NULL,
    [DateCreated]          DATETIME         CONSTRAINT [DF_SubscriberArchive_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]          DATETIME         NULL,
    [CreatedByUserID]      INT              NULL,
    [UpdatedByUserID]      INT              NULL,
    [IsMailable]           BIT              NULL,
    [SuppressedTime]       TIME (7)         NULL,
    [SuppressedSource]     VARCHAR (250)    NULL,
    [ProcessCode]          VARCHAR (50)     NULL,
    [ImportRowNumber]      INT              NULL,
    [IsActive]             BIT              NULL,
	[ExternalKeyId]		   INT			    NULL,
	[AccountNumber]		   VARCHAR (50)	    NULL,
	[EmailID]			   INT			    NULL,
	[Copies]			   INT				NULL,	
	[GraceIssues]          INT				NULL,
	[IsComp]			   BIT				NULL,
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
    [TextPermission]	   BIT				NULL,
    CONSTRAINT [PK_SubscriberArchive] PRIMARY KEY CLUSTERED ([SARecordIdentifier] ASC) WITH (FILLFACTOR = 70)
);


GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberArchive_IGrp_No]
    ON [dbo].[SubscriberArchive]([IGrp_No] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberArchive_IGrp_Rank]
    ON [dbo].[SubscriberArchive]([IGrp_Rank] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberArchive_SORecordIdentifier]
    ON [dbo].[SubscriberArchive]([SARecordIdentifier] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberArchive_ProcessCode_SourceFileID_SFRecordIdentifier] ON [dbo].[SubscriberArchive] 
(
	[ProcessCode] ASC,
	[SourceFileID] ASC,
	[SFRecordIdentifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO