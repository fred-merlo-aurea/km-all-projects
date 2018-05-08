CREATE TABLE [dbo].[ClientConfigurationValueCData] (
    [ClientConfigurationValueCDataID] INT           IDENTITY (1, 1) NOT NULL,
    [ValueHash]                       NCHAR (32)    NOT NULL,
    [ValueCData]                      VARCHAR (MAX) NOT NULL,
    [DateCreated]                     DATETIME      NOT NULL,
    [CreatedByUserID]                 INT           NOT NULL,
    [DateUpdated]                     DATETIME      NULL,
    [UpdatedByUserID]                 INT           NULL,
    PRIMARY KEY CLUSTERED ([ClientConfigurationValueCDataID] ASC),
    UNIQUE NONCLUSTERED ([ValueHash] ASC)
);

