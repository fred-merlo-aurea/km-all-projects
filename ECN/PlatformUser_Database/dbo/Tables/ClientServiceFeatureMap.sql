CREATE TABLE [dbo].[ClientServiceFeatureMap] (
    [ClientServiceFeatureMapID] INT             IDENTITY (1, 1) NOT NULL,
    [ClientID]                  INT             NOT NULL,
    [ServiceFeatureID]          INT             NOT NULL,
    [IsEnabled]                 BIT             NOT NULL,
    [Rate]                      DECIMAL (14, 2) NOT NULL,
    [RateDurationInMonths]      INT             NOT NULL,
    [RateStartDate]             DATE            NULL,
    [RateExpireDate]            DATE            NULL,
    [DateCreated]               DATETIME        NOT NULL,
    [DateUpdated]               DATETIME        NULL,
    [CreatedByUserID]           INT             NOT NULL,
    [UpdatedByUserID]           INT             NULL,
    CONSTRAINT [PK_ClientServiceFeatureMap] PRIMARY KEY CLUSTERED ([ClientServiceFeatureMapID] ASC),
    CONSTRAINT [FK_ClientServiceFeatureMap_Client] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK_ClientServiceFeatureMap_ServiceFeature] FOREIGN KEY ([ServiceFeatureID]) REFERENCES [dbo].[ServiceFeature] ([ServiceFeatureID])
);


GO
CREATE NONCLUSTERED INDEX [IDX_ClientServiceFeatureMap_ClientID_ServiceFeatureID_IsEnabled]
    ON [dbo].[ClientServiceFeatureMap]([ClientID] ASC, [ServiceFeatureID] ASC, [IsEnabled] ASC) WITH (FILLFACTOR = 90);

