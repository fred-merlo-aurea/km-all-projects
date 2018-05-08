CREATE TABLE [dbo].[PubGroups] (
    [PubID]   INT NOT NULL,
    [GroupID] INT NOT NULL,
	[Name] varchar(100) NULL
    CONSTRAINT [PK_PubGroups] PRIMARY KEY CLUSTERED ([PubID] ASC, [GroupID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PubGroups_Pubs] FOREIGN KEY ([PubID]) REFERENCES [dbo].[Pubs] ([PubID])
);

