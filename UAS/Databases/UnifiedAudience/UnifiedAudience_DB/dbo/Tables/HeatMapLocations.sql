CREATE TABLE [dbo].[HeatMapLocations] (
    [LocationID]   INT          IDENTITY (1, 1) NOT NULL,
    [CreatedBy]    INT          NOT NULL,
    [LocationName] VARCHAR (50) NOT NULL,
    [Address]      VARCHAR (50) NOT NULL,
    [City]         VARCHAR (20) NOT NULL,
    [State]        VARCHAR (20) NOT NULL,
    [Zip]          VARCHAR (20) NOT NULL,
    [Radius]       VARCHAR (10) NOT NULL,
    [Country]      VARCHAR (50) NOT NULL,
    [BrandID]      INT          NULL,
    CONSTRAINT [PK_HeatMapLocations] PRIMARY KEY CLUSTERED ([LocationID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_HeatMapLocations_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);
GO
CREATE NONCLUSTERED INDEX [IDX_HeatMapLocations_CreatedBy]
    ON [dbo].[HeatMapLocations]([CreatedBy] ASC) WITH (FILLFACTOR = 70);
GO

