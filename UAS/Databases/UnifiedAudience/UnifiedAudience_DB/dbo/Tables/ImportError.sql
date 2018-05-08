CREATE TABLE [dbo].[ImportError] (
    [ImportErrorID]      INT            IDENTITY (1, 1) NOT NULL,
    [SourceFileID]       INT            NOT NULL,
    [RowNumber]          INT            NULL,
    [FormattedException] VARCHAR (MAX)  NULL,
    [ClientMessage]      VARCHAR (4000) NULL,
    [MAFField]           VARCHAR (255)  NULL,
    [BadDataRow]         VARCHAR (MAX)  NULL,
    [ThreadID]           INT            NULL,
    [DateCreated]        DATETIME       CONSTRAINT [DF_ImportError_DateCreated] DEFAULT (getdate()) NOT NULL,
    [ProcessCode]        VARCHAR (50)   NOT NULL,
    [IsDimensionError]   BIT            NOT NULL,
    CONSTRAINT [PK_ImportError] PRIMARY KEY CLUSTERED ([ImportErrorID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IDX_ImportError_ProcessCode]
    ON [dbo].[ImportError]([ProcessCode] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_ImportError_RowNumber]
    ON [dbo].[ImportError]([RowNumber] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_ImportError_SourceFileID]
    ON [dbo].[ImportError]([SourceFileID] ASC) WITH (FILLFACTOR = 70);
GO