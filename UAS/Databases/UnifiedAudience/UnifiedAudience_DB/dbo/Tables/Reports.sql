CREATE TABLE [dbo].[Reports] (
    [ReportID]         INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]       VARCHAR (200) NOT NULL,
    [IsActive]         BIT           NOT NULL,
    [DateCreated]      DATETIME      CONSTRAINT [DF_Reports_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT           NULL,
    [ProvideID]        BIT           DEFAULT ((1)) NOT NULL,
    [ProductID]        INT           NULL,
    [URL]              VARCHAR (250) NULL,
    [IsCrossTabReport] BIT           NULL,
    [Row]              VARCHAR (50)  NULL,
    [Column]           VARCHAR (50)  NULL,
    [SuppressTotal]    BIT           NULL,
    [Status]           BIT           NULL,
    [ReportTypeID]     INT           NULL,
    CONSTRAINT [PK_Reports_ReportID] PRIMARY KEY CLUSTERED ([ReportID] ASC) WITH (FILLFACTOR = 90)
);

