CREATE TABLE [dbo].[EmailPreview] (
    [EmailTestID] INT            NOT NULL,
    [BlastID]     INT            NOT NULL,
    [CustomerID]  INT            NOT NULL,
    [ZipFile]     NVARCHAR (500) NULL,
    [CreatedByID] INT            NOT NULL,
    [DateCreated] DATE           NOT NULL,
    [TimeCreated] TIME (7)       NOT NULL,
    [LinkTestID]  INT NULL, 
    [BaseChannelGUID] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_EmailPreview] PRIMARY KEY CLUSTERED ([EmailTestID] ASC) WITH (FILLFACTOR = 80)
);

