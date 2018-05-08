CREATE TABLE [dbo].[ClientGroupApplicationMap] (
    [ClientGroupApplicationMapID] INT IDENTITY (1, 1) NOT NULL,
    [ClientgroupID]               INT NOT NULL,
    [ApplicationID]               INT NOT NULL,
    CONSTRAINT [PK_ClientGroupApplicationMap] PRIMARY KEY CLUSTERED ([ClientGroupApplicationMapID] ASC)
);

