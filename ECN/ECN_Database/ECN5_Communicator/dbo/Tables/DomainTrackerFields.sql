CREATE TABLE [dbo].[DomainTrackerFields] (
    [DomainTrackerFieldsID] INT          IDENTITY (1, 1) NOT NULL,
    [DomainTrackerID]       INT          NULL,
    [GroupDatafieldsID]     INT          NULL,
    [Source]                VARCHAR (50) NULL,
    [SourceID]              VARCHAR (50) NULL,
    [CreatedUserID]         INT          NULL,
    [CreatedDate]           DATETIME     NULL,
    [UpdatedUserID]         INT          NULL,
    [UpdatedDate]           DATETIME     NULL,
    [IsDeleted]             BIT          NULL,
    CONSTRAINT [PK_DomainTrackerFields] PRIMARY KEY CLUSTERED ([DomainTrackerFieldsID] ASC) WITH (FILLFACTOR = 80)
);

