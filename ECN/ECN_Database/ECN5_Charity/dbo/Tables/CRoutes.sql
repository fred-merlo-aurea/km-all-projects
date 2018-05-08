CREATE TABLE [dbo].[CRoutes] (
    [CRouteID]   INT          NOT NULL,
    [ClientID]   INT          NULL,
    [Address]    VARCHAR (50) NULL,
    [ZipCode]    VARCHAR (10) NULL,
    [CRoute]     VARCHAR (5)  NULL,
    [PickupDate] DATETIME     NULL
);

