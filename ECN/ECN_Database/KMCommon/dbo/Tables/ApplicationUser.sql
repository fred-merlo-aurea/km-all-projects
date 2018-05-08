CREATE TABLE [dbo].[ApplicationUser] (
    [ApplicationID] INT NOT NULL,
    [UserID]        INT NOT NULL,
    CONSTRAINT [FK_ApplicationUser_Application] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Application] ([ApplicationID]),
    CONSTRAINT [FK_ApplicationUser_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);

