CREATE TABLE [dbo].[ClientGroupServiceMap] (
    [ClientGroupServiceMapID] INT             IDENTITY (1, 1) NOT NULL,
    [ClientGroupID]           INT             NOT NULL,
    [ServiceID]               INT             NOT NULL,
    [IsEnabled]               BIT             NOT NULL,
    [Rate]                    DECIMAL (14, 2) NOT NULL,
    [RateDurationInMonths]    INT             NOT NULL,
    [RateStartDate]           DATE            NULL,
    [RateExpireDate]          DATE            NULL,
    [DateCreated]             DATETIME        NOT NULL,
    [DateUpdated]             DATETIME        NULL,
    [CreatedByUserID]         INT             NOT NULL,
    [UpdatedByUserID]         INT             NULL,
    CONSTRAINT [PK_ClientGroupServiceMap] PRIMARY KEY CLUSTERED ([ClientGroupID] ASC, [ServiceID] ASC),
    CONSTRAINT [FK_ClientGroupServiceMap_ClientGroup] FOREIGN KEY ([ClientGroupID]) REFERENCES [dbo].[ClientGroup] ([ClientGroupID]),
    CONSTRAINT [FK_ClientGroupServiceMap_Service] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Service] ([ServiceID])
);

