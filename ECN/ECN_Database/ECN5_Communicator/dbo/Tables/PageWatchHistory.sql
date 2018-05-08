CREATE TABLE [dbo].[PageWatchHistory] (
    [PageWatchHistoryID] INT          IDENTITY (1, 1) NOT NULL,
    [PageWatchTagID]     INT          NOT NULL,
    [PreviousHTML]       TEXT         NOT NULL,
    [CurrentHTML]        TEXT         NOT NULL,
    [StatusCode]         VARCHAR (50) NOT NULL,
    [AddedBy]            INT          NOT NULL,
    [DateAdded]          DATETIME     NOT NULL,
    [BlastID]            INT          NULL,
    CONSTRAINT [PK_PageWatchHistory] PRIMARY KEY CLUSTERED ([PageWatchHistoryID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_PageWatchHistory_PageWatchTag] FOREIGN KEY ([PageWatchTagID]) REFERENCES [dbo].[PageWatchTag] ([PageWatchTagID])
);

