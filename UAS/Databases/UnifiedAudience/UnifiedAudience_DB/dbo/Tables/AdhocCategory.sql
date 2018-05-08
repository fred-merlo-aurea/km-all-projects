CREATE TABLE [dbo].[AdhocCategory] (
    [CategoryID]      INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName]    VARCHAR (50) NULL,
    [SortOrder]       INT          NULL,
    [DateCreated]     DATETIME     CONSTRAINT [DF_AdhocCategory_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]     DATETIME     NULL,
    [CreatedByUserID] INT          NULL,
    [UpdatedByUserID] INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [IDX_AdhocCategory_CategoryName]
    ON [dbo].[AdhocCategory]([CategoryName] ASC);