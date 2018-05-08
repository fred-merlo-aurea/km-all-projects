CREATE TABLE [dbo].[Reports] (
    [ReportID]    INT           IDENTITY (1, 1) NOT NULL,
    [ReportName]  VARCHAR (500) NOT NULL,
    [ControlName] VARCHAR (500) NULL,
    [ShowInSetup] BIT           NULL,
    [IsExport] BIT NULL, 
    CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);



