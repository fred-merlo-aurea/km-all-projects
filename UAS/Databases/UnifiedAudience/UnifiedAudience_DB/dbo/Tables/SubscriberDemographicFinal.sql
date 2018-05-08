CREATE TABLE [dbo].[SubscriberDemographicFinal] (
    [SDFinalID]               INT              IDENTITY (1, 1) NOT NULL,
    [PubID]                   INT              NULL,
    [SFRecordIdentifier]      UNIQUEIDENTIFIER NOT NULL,
    [MAFField]                VARCHAR (255)    NULL,
    [Value]                   VARCHAR (MAX)    NULL,
    [NotExists]               BIT              NULL,
    [DateCreated]             DATETIME          NULL,
    [DateUpdated]             DATETIME         NULL,
    [CreatedByUserID]         INT              NOT NULL,
    [UpdatedByUserID]         INT              NULL,
    [DemographicUpdateCodeId] INT              CONSTRAINT [DF_SubscriberDemographicFinal_DemographicUpdateCodeId] DEFAULT ((1928)) NOT NULL,
    [IsAdhoc]                 BIT              CONSTRAINT [DF_SubscriberDemographicFinal_IsAdhoc] DEFAULT ((0)) NOT NULL,
    [ResponseOther]           VARCHAR (256)    NULL,
    [IsDemoDate] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_SubscriberDemographicFinal] PRIMARY KEY CLUSTERED ([SDFinalID] ASC) WITH (FILLFACTOR = 90)
);

GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberDemographicFinal_SFRecordIdentifier]
    ON [dbo].[SubscriberDemographicFinal]([SFRecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO
