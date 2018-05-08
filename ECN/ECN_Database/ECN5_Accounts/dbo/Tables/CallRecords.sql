CREATE TABLE [dbo].[CallRecords] (
    [CallRecordID] INT      IDENTITY (1, 1) NOT NULL,
    [StaffID]      INT      NOT NULL,
    [CallDate]     DATETIME NOT NULL,
    [CallCount]    INT      CONSTRAINT [DF_CallRecords_CallCount] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_CallRecords] PRIMARY KEY CLUSTERED ([CallRecordID] ASC) WITH (FILLFACTOR = 80)
);

