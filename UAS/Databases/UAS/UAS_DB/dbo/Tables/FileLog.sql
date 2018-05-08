CREATE TABLE [dbo].[FileLog] (
    [SourceFileID]     INT           NOT NULL,
    [FileStatusTypeID] INT           NOT NULL,
    [Message]          VARCHAR (MAX) NOT NULL,
    [LogDate]          DATE          NOT NULL,
    [LogTime]          TIME (7)      NOT NULL, 
    [ProcessCode] VARCHAR(50) NULL
);



GO
CREATE NONCLUSTERED INDEX [IDX_SourceFileID]
    ON [dbo].[FileLog]([SourceFileID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IDX_FileStatusTypeID]
    ON [dbo].[FileLog]([FileStatusTypeID] ASC) WITH (FILLFACTOR = 90);

GO

CREATE NONCLUSTERED INDEX [IDX_LogDate]
    ON [dbo].[FileLog]([LogDate] ASC) WITH (FILLFACTOR = 90);
GO