CREATE TABLE [dbo].[User] (
    [FirstName]    VARCHAR (50)  NOT NULL,
    [LastName]     VARCHAR (50)  NOT NULL,
    [EmailAddress] VARCHAR (100) NOT NULL,
    [UserID]       INT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (FILLFACTOR = 80)
);

