CREATE TABLE [dbo].[SubGenUserMap]
(
	[UserID] INT NOT NULL,
	[ClientID] INT NOT NULL,
	[SubGenUserId] INT NOT NULL,
	[SubGenAccountId] INT NOT NULL,
	[SubGenAccountName] VARCHAR(50) NULL, 
    CONSTRAINT [PK_SubGenUserMap] PRIMARY KEY ([UserID], [SubGenUserId], [ClientID])
)
