CREATE TABLE [dbo].[ApplicationServiceMap] (
    [ApplicationServiceMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ApplicationID]           INT      NOT NULL,
    [ServiceID]               INT      NOT NULL,
    [IsEnabled]               BIT      NOT NULL,
    [DateCreated]             DATETIME NOT NULL,
    [DateUpdated]             DATETIME NULL,
    [CreatedByUserID]         INT      NOT NULL,
    [UpdatedByUserID]         INT      NULL,
    CONSTRAINT [PK_ApplicationServiceMap] PRIMARY KEY CLUSTERED ([ApplicationID] ASC, [ServiceID] ASC),
    CONSTRAINT [FK_ApplicationServiceMap_Application] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Application] ([ApplicationID]),
    CONSTRAINT [FK_ApplicationServiceMap_Service] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Service] ([ServiceID])
);


