CREATE TABLE [dbo].[PenetrationReports] (
    [ReportID]       INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]     VARCHAR (100) NOT NULL,
    [CreatedBy]      INT           NOT NULL,
    [CreateddtStamp] DATETIME      NOT NULL,
    [UpdatedBy]      INT           NOT NULL,
    [UpdateddtStamp] DATETIME      NOT NULL,
    [BrandID]        INT           NULL,
    CONSTRAINT [PK_PenetrationReports] PRIMARY KEY CLUSTERED ([ReportID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PenetrationReports_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);

