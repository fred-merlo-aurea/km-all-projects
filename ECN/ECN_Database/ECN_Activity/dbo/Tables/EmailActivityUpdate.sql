CREATE TABLE [dbo].[EmailActivityUpdate]
(
    [UpdateID]  INT            IDENTITY (1, 1) NOT NULL,
    [OldEmailID] INT NULL, 
    [OldEmailAddress] VARCHAR(255) NULL, 
    [NewEmailID] INT NULL, 
    [NewEmailAddress] VARCHAR(255) NULL, 
    [UpdateTime] DATETIME NOT NULL, 
    [ApplicationSourceID] INT NOT NULL, 
    [SourceID] INT NOT NULL, 
    [Comments] VARCHAR(355) NULL, 
    CONSTRAINT [PK_EmailActivityUpdate] PRIMARY KEY ([UpdateID])
)