CREATE TABLE [dbo].[FileValidator_DemographicTransformed] (
    [FV_DemographicTransformedID] INT              IDENTITY (1, 1) NOT NULL,
    [PubID]                       INT              NULL,
    [STRecordIdentifier]          UNIQUEIDENTIFIER NOT NULL,
    [MAFField]                    VARCHAR (255)    NULL,
    [Value]                       VARCHAR (MAX)    NULL,
    [NotExists]                   BIT              NULL,
    [NotExistReason]              VARCHAR (50)     NULL,
    [DateCreated]                 DATETIME         CONSTRAINT [DF_FileValidator_DemographicTransformed_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]                 DATETIME         NULL,
    [CreatedByUserID]             INT              NOT NULL,
    [UpdatedByUserID]             INT              NULL,
    CONSTRAINT [PK_FV_DemographicTransformed] PRIMARY KEY CLUSTERED ([FV_DemographicTransformedID] ASC) WITH (FILLFACTOR = 70)
);


