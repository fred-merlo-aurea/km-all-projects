CREATE TABLE [dbo].[PageWatchTag] (
    [PageWatchTagID] INT           IDENTITY (1, 1) NOT NULL,
    [PageWatchID]    INT           NOT NULL,
    [Name]           VARCHAR (100) NULL,
    [WatchTag]       VARCHAR (300) NOT NULL,
    [PreviousHTML]   TEXT          NULL,
    [AddedBy]        INT           NOT NULL,
    [DateAdded]      DATETIME      NOT NULL,
    [UpdatedBy]      INT           NULL,
    [DateUpdated]    DATETIME      NULL,
    [IsChanged]      BIT           CONSTRAINT [DF_PageWatchTag_IsChanged] DEFAULT ((0)) NOT NULL,
    [IsActive]       BIT           CONSTRAINT [DF_PageWatchTag_IsActive] DEFAULT ((1)) NOT NULL,
    [DateChanged]    DATETIME      NULL,
    CONSTRAINT [PK_PageWatchTag] PRIMARY KEY CLUSTERED ([PageWatchTagID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_PageWatchTag_PageWatch] FOREIGN KEY ([PageWatchID]) REFERENCES [dbo].[PageWatch] ([PageWatchID])
);

