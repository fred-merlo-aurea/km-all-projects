CREATE TABLE [dbo].[PublicationSequence] (
    [PublicationID]   INT      NOT NULL,
    [SequenceID]      INT      NOT NULL,
    [DateCreated]     DATETIME NOT NULL,
    [CreatedByUserID] INT      NOT NULL,
    CONSTRAINT [PK_PublicationSequence] PRIMARY KEY CLUSTERED ([PublicationID] ASC, [SequenceID] ASC) WITH (FILLFACTOR = 80)
);

