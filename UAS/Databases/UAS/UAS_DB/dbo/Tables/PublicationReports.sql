CREATE TABLE [dbo].[PublicationReports] (
    [ReportID]         INT           IDENTITY (1, 1) NOT NULL,
    [PublicationID]    INT           NOT NULL,
    [ReportName]       VARCHAR (100) NOT NULL,
    [url]              VARCHAR (250) NOT NULL,
    [IsCrossTabReport] BIT           NULL,
    [Row]              VARCHAR (50)  NULL,
    [Column]           VARCHAR (50)  NULL,
    [SuppressTotal]    BIT           NULL,
    [Status]           BIT           NULL,
    CONSTRAINT [PK_PublicationReports] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);

