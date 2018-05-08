CREATE TABLE [dbo].[SubscriberDemographicInvalid] (
    [SDInvalidID]        INT              IDENTITY (1, 1) NOT NULL,
    [PubID]              INT              NULL,
    [SORecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [SIRecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [MAFField]           VARCHAR (255)    NULL,
    [Value]              VARCHAR (MAX)    NULL,
    [NotExists]          BIT              NULL,
    [DateCreated]        DATETIME         CONSTRAINT [DF_SubscriberDemographicInvalid_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NOT NULL,
    [UpdatedByUserID]    INT              NULL,
	[DemographicUpdateCodeId] INT		  CONSTRAINT [DF_SubscriberDemographicInvalid_DemographicUpdateCodeId] DEFAULT (1928) NOT NULL,
	[IsAdhoc] BIT						  CONSTRAINT [DF_SubscriberDemographicInvalid_IsAdhoc] DEFAULT (0) NOT NULL,
	[ResponseOther] VARCHAR(256) NULL,
    CONSTRAINT [PK_SDInvalidID] PRIMARY KEY CLUSTERED ([SDInvalidID] ASC) WITH (FILLFACTOR = 90)
);


GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberDemographicInvalid_SORecordIdentifier]
    ON [dbo].[SubscriberDemographicInvalid]([SORecordIdentifier] ASC) WITH (FILLFACTOR = 70);
GO
