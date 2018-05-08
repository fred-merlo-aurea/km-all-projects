CREATE TABLE [dbo].[FileValidator_ImportError] (
    [FV_ImportErrorID]   INT            IDENTITY (1, 1) NOT NULL,
    [SourceFileID]       INT            NOT NULL,
    [RowNumber]          INT            NULL,
    [FormattedException] VARCHAR (MAX)  NULL,
    [ClientMessage]      VARCHAR (4000) NULL,
    [MAFField]           VARCHAR (255)  NULL,
    [BadDataRow]         VARCHAR (MAX)  NULL,
    [ThreadID]           INT            NULL,
    [DateCreated]        DATETIME       CONSTRAINT [DF_FileValidator_ImportError_DateCreated] DEFAULT (getdate()) NOT NULL,
    [ProcessCode]        VARCHAR (50)   NOT NULL,
    [IsDimensionError]   BIT            NOT NULL,
    CONSTRAINT [PK_DataImportExport_FV_ImportErrorID] PRIMARY KEY CLUSTERED ([FV_ImportErrorID] ASC) WITH (FILLFACTOR = 90)
);
