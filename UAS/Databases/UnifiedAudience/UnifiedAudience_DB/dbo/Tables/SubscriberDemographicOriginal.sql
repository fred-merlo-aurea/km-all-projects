CREATE TABLE [dbo].[SubscriberDemographicOriginal] (
    [SDOriginalID]       INT              IDENTITY (1, 1) NOT NULL,
    [PubID]              INT              NULL,
    [SORecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [MAFField]           VARCHAR (255)    NULL,
    [Value]              VARCHAR (MAX)    NULL,
    [NotExists]          BIT              NULL,
    [DateCreated]        DATETIME         CONSTRAINT [DF_SubscriberDemographicOriginal_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NOT NULL,
    [UpdatedByUserID]    INT              NULL,
	[DemographicUpdateCodeId] INT		  CONSTRAINT [DF_SubscriberDemographicOriginal_DemographicUpdateCodeId] DEFAULT (1928) NOT NULL,
	[IsAdhoc] BIT						  CONSTRAINT [DF_SubscriberDemographicOriginal_IsAdhoc] DEFAULT (0) NOT NULL,
	[ResponseOther] VARCHAR(256) NULL,
    CONSTRAINT [PK_SubscriberDemographicOriginal] PRIMARY KEY CLUSTERED ([SDOriginalID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberDemographicOriginal_SORecordIdentifier]
    ON [dbo].[SubscriberDemographicOriginal]([SORecordIdentifier] ASC) WITH (FILLFACTOR = 70);
GO

