

CREATE TABLE [dbo].[SubscriberDemographicArchive] (
    [SDArchiveID]        INT              IDENTITY (1, 1) NOT NULL,
    [PubID]              INT              NULL,
    [SARecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [MAFField]           VARCHAR (255)    NULL,
    [Value]              VARCHAR (MAX)    NULL,
    [NotExists]          BIT              NULL,
    [DateCreated]        DATETIME         CONSTRAINT [DF_SubscriberDemographicArchive_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NOT NULL,
    [UpdatedByUserID]    INT              NULL,
	[DemographicUpdateCodeId] INT		  CONSTRAINT [DF_SubscriberDemographicArchive_DemographicUpdateCodeId] DEFAULT (1928) NOT NULL,
	[IsAdhoc] BIT						  CONSTRAINT [DF_SubscriberDemographicArchive_IsAdhoc] DEFAULT (0) NOT NULL,
	[ResponseOther] VARCHAR(256) NULL,
    CONSTRAINT [PK_SubscriberDemographicArchive] PRIMARY KEY CLUSTERED ([SDArchiveID] ASC)
);



GO


