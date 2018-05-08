CREATE TABLE [dbo].[DataCompareProfile]
(
	[SubscriberFinalId]    INT              NOT NULL,
    [SFRecordIdentifier]   UNIQUEIDENTIFIER NOT NULL,
    [SourceFileId]         INT              NOT NULL,
    [ProcessCode]          VARCHAR (50)     NOT NULL,
    [ExternalKeyId]		   INT			    NULL,
	[PubCode]              VARCHAR (100)    NULL,
    [FName]                VARCHAR (100)    NULL,
    [LName]                VARCHAR (100)    NULL,
    [Title]                VARCHAR (100)    NULL,
	[Occupation]		   VARCHAR(50)		NULL,
    [Company]              VARCHAR (100)    NULL,
    [Address]              VARCHAR (255)    NULL,
    [MailStop]             VARCHAR (255)    NULL,
    [Address3]             VARCHAR (255)    NULL,
    [City]                 VARCHAR (50)     NULL,
    [State]                VARCHAR (50)     NULL,
    [Zip]                  VARCHAR (50)     NULL,
    [Plus4]                VARCHAR (50)     NULL,
    [ForZip]               VARCHAR (50)     NULL,
    [County]               VARCHAR (100)    NULL,
    [Country]              VARCHAR (100)    NULL,
    [CountryId]            INT              NULL,
    [Phone]                VARCHAR (100)    NULL,
    [Mobile]               VARCHAR (30)     NULL,
    [Fax]                  VARCHAR (100)    NULL,
    [Email]                VARCHAR (100)    NULL,
    [EmailStatusId]        INT              NULL,
    [Gender]               VARCHAR (1024)   NULL,
	[Website]			   VARCHAR(255)		NULL,
    [DateCreated]          DATETIME         CONSTRAINT [DF_DataCompareProfile_DateCreated] DEFAULT (getdate()) NULL,
    [ImportRowNumber]      INT              NULL,
    [IGrp_No]              UNIQUEIDENTIFIER NULL,
	[IsNew]				   BIT				NULL,
    CONSTRAINT [PK_DataCompareProfile] PRIMARY KEY CLUSTERED ([SFRecordIdentifier] ASC) WITH (FILLFACTOR = 90)
)
GO

CREATE NONCLUSTERED INDEX [IX_DataCompareProfile_ProcessCode_IGrp_No]
    ON [dbo].[DataCompareProfile]([ProcessCode] ASC, [IGrp_No] ASC) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO
