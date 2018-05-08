CREATE TABLE [dbo].[DomainTrackerGroupDataFields] (
    [DomainTrackerID]              INT          NULL,
    [GroupDatafieldsID]            INT          NULL,
    [Source]                       VARCHAR (50) NULL,
    [SourceID]                     VARCHAR (50) NULL,
    [DomainTrackerGroupDataFields] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_DomainTrackerGroupDataFields] PRIMARY KEY CLUSTERED ([DomainTrackerGroupDataFields] ASC) WITH (FILLFACTOR = 80)
);

