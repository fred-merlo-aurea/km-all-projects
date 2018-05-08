CREATE TABLE [dbo].[SubscriberDemographicTransformed] (
    [SubscriberDemographicTransformedID] INT              IDENTITY (1, 1) NOT NULL,
    [PubID]                              INT              NULL,
    [SORecordIdentifier]                 UNIQUEIDENTIFIER NOT NULL,
    [STRecordIdentifier]                 UNIQUEIDENTIFIER NOT NULL,
    [MAFField]                           VARCHAR (255)    NULL,
    [Value]                              VARCHAR (MAX)    NULL,
    [NotExists]                          BIT              NULL,
    [NotExistReason]                     VARCHAR (50)     NULL,
    [DateCreated]                        DATETIME          NULL,
    [DateUpdated]                        DATETIME         NULL,
    [CreatedByUserID]                    INT              NOT NULL,
    [UpdatedByUserID]                    INT              NULL,
    [DemographicUpdateCodeId]            INT              CONSTRAINT [DF_SubscriberDemographicTransformed_DemographicUpdateCodeId] DEFAULT ((1928)) NOT NULL,
    [IsAdhoc]                            BIT              CONSTRAINT [DF_SubscriberDemographicTransformed_IsAdhoc] DEFAULT ((0)) NOT NULL,
    [ResponseOther]                      VARCHAR (256)    NULL,
    [IsDemoDate] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_SubscriberDemographicTransformed] PRIMARY KEY CLUSTERED ([SubscriberDemographicTransformedID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberDemographicTransformed_MAFField]
    ON [dbo].[SubscriberDemographicTransformed]([MAFField] ASC)
    INCLUDE([SubscriberDemographicTransformedID], [SORecordIdentifier], [Value]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberDemographicTransformed_MAFField_PubID_SubscriberDemographicTransformedID_STRecordIdentifier]
    ON [dbo].[SubscriberDemographicTransformed]([MAFField] ASC, [PubID] ASC, [SubscriberDemographicTransformedID] ASC, [STRecordIdentifier] ASC)
    INCLUDE([Value]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberDemographicTransformed_SORecordIdentifier]
    ON [dbo].[SubscriberDemographicTransformed]([SORecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberDemographicTransformed_STRecordIdentifier]
    ON [dbo].[SubscriberDemographicTransformed]([STRecordIdentifier] ASC) WITH (FILLFACTOR = 90);
GO

CREATE STATISTICS [STAT_SubscriberDemographicTransformed_SubscriberDemographicTransformedID_STRecordIdentifier]
    ON [dbo].[SubscriberDemographicTransformed]([SubscriberDemographicTransformedID], [STRecordIdentifier]);
GO

CREATE STATISTICS [STAT_SubscriberDemographicTransformed_SubscriberDemographicTransformedID_STRecordIdentifier_MAFField]
    ON [dbo].[SubscriberDemographicTransformed]([PubID], [SubscriberDemographicTransformedID], [STRecordIdentifier], [MAFField]);
GO

