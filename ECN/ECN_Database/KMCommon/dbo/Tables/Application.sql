CREATE TABLE [dbo].[Application] (
    [ApplicationName]  VARCHAR (255) NOT NULL,
    [IsActive]         BIT           NOT NULL,
    [FromEmailAddress] VARCHAR (100) NOT NULL,
    [ApplicationID]    INT           NOT NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY NONCLUSTERED ([ApplicationID] ASC)
);

