CREATE TABLE [dbo].[ClientUADUsersMap] (
    [ClientID] INT NOT NULL,
    [UserID]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ClientID] ASC, [UserID] ASC)
);

