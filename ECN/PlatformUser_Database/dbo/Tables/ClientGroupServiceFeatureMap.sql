CREATE TABLE [dbo].[ClientGroupServiceFeatureMap] (
    [ClientGroupServiceFeatureMapID] INT             IDENTITY (1, 1) NOT NULL,
    [ClientGroupID]                  INT             NOT NULL,
    [ServiceID]                      INT             NOT NULL,
    [ServiceFeatureID]               INT             NOT NULL,
    [IsEnabled]                      BIT             NOT NULL,
    [Rate]                           DECIMAL (14, 2) NOT NULL,
    [RateDurationInMonths]           INT             NOT NULL,
    [RateStartDate]                  DATE            NULL,
    [RateExpireDate]                 DATE            NULL,
    [DateCreated]                    DATETIME        NOT NULL,
    [DateUpdated]                    DATETIME        NULL,
    [CreatedByUserID]                INT             NOT NULL,
    [UpdatedByUserID]                INT             NULL,
    CONSTRAINT [PK_ClientGroupServiceFeatureMap] PRIMARY KEY CLUSTERED ([ClientGroupID] ASC, [ServiceID] ASC, [ServiceFeatureID] ASC),
    CONSTRAINT [FK_ClientGroupServiceFeatureMap_ClientGroup] FOREIGN KEY ([ClientGroupID]) REFERENCES [dbo].[ClientGroup] ([ClientGroupID])
);





GO
