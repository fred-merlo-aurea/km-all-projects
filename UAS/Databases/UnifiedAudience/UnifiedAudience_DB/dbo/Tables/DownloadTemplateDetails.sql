CREATE TABLE [dbo].[DownloadTemplateDetails] (
    [DownloadTemplateDetailsID] INT           IDENTITY (1, 1) NOT NULL,
    [DownloadTemplateID]        INT           NULL,
    [ExportColumn]              VARCHAR (100) NULL,
    [IsDescription]             BIT           CONSTRAINT [DF_DownloadTemplateDetails_IsDescription] DEFAULT ((0)) NULL,
    [FieldCase] VARCHAR(100) NULL, 
    CONSTRAINT [PK_DownloadTemplateDetails] PRIMARY KEY CLUSTERED ([DownloadTemplateDetailsID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DownloadTemplateDetails_DownloadTemplate] FOREIGN KEY ([DownloadTemplateID]) REFERENCES [dbo].[DownloadTemplate] ([DownloadTemplateID])
);
GO

