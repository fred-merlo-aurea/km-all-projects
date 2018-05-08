CREATE TABLE [dbo].[ClientApplicationMap] (
    [ClientApplicationMapID] INT IDENTITY (1, 1) NOT NULL,
    [ApplicationID]          INT NOT NULL,
    [ClientID]               INT NOT NULL,
    CONSTRAINT [PK_ClientApplicationMap] PRIMARY KEY CLUSTERED ([ClientApplicationMapID] ASC)
);

