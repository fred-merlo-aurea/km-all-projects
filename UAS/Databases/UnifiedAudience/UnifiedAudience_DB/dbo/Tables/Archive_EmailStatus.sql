CREATE TABLE [dbo].[Archive_EmailStatus] (
    [Archive_EmailStatusId] INT           IDENTITY (1, 1) NOT NULL,
    [SubscriptionId]        INT           NULL,
    [Email]                 VARCHAR (100) NULL,
    [EmailExists]           BIT           NULL,
    [ArchivedDate]          DATETIME      NULL,
    CONSTRAINT [PK_Archive_EmailStatus_Archive_EmailStatusId] PRIMARY KEY CLUSTERED ([Archive_EmailStatusId] ASC) WITH (FILLFACTOR = 90)
);
