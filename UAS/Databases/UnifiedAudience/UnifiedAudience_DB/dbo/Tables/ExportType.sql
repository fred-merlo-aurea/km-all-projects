CREATE TABLE [dbo].[ExportType] (
    [ExportTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [Type]         VARCHAR (50) NULL,
    CONSTRAINT [PK_ExportType] PRIMARY KEY CLUSTERED ([ExportTypeID] ASC) WITH (FILLFACTOR = 90)
);

