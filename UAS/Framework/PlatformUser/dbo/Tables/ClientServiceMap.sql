CREATE TABLE [dbo].[ClientServiceMap] (
    [ClientServiceMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ClientID]           INT      NOT NULL,
    [ServiceID]          INT      NOT NULL,
    [IsEnabled]          BIT      CONSTRAINT [DF_ClientServiceMap_IsEnabled] DEFAULT ((1)) NOT NULL,
    [DateCreated]        DATETIME CONSTRAINT [DF_ClientServiceMap_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]        DATETIME NULL,
    [CreatedByUserID]    INT      NOT NULL,
    [UpdatedByUserID]    INT      NULL,
    CONSTRAINT [PK_ClientServiceMap] PRIMARY KEY CLUSTERED ([ClientServiceMapID] ASC),
    CONSTRAINT [FK_ClientServiceMap_Client] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK_ClientServiceMap_Service] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Service] ([ServiceID])
);

