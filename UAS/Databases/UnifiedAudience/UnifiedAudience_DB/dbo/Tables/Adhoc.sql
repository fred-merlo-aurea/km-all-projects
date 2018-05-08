CREATE TABLE [dbo].[Adhoc] (
    [AdhocID]         INT          IDENTITY (1, 1) NOT NULL,
    [AdhocName]       VARCHAR (255) NULL,
    [CategoryID]      INT          NULL,
    [SortOrder]       INT          NULL,
    [DateCreated]     DATETIME     CONSTRAINT [DF_Adhoc_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]     DATETIME     NULL,
    [CreatedByUserID] INT          NULL,
    [UpdatedByUserID] INT          NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_Adhoc_AdhocName]
    ON [dbo].[Adhoc]([AdhocName] ASC);

