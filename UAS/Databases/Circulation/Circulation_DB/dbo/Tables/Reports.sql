CREATE TABLE [dbo].[Reports] (
    [ReportID]        INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]      VARCHAR (200) NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    [ProvideID]       BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ReportID] ASC) WITH (FILLFACTOR = 80)
);

