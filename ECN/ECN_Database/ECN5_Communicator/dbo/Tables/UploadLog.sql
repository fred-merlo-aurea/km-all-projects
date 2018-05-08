CREATE TABLE [dbo].[UploadLog] (
    [UploadID]   INT           IDENTITY (1, 1) NOT NULL,
    [UserID]     INT           NOT NULL,
    [CustomerID] INT           NOT NULL,
    [Path]       VARCHAR (500) NOT NULL,
    [FileName]   VARCHAR (250) NOT NULL,
    [uploaddate] DATETIME      NOT NULL,
    [PageSource] VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_UploadLog] PRIMARY KEY CLUSTERED ([UploadID] ASC) WITH (FILLFACTOR = 80)
);

