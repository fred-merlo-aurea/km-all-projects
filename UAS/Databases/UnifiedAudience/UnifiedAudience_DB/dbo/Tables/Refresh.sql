CREATE TABLE [dbo].[Refresh] (
    [RefreshID]   INT           IDENTITY (1, 1) NOT NULL,
    [EnterTime]   DATETIME      NULL,
    [StartTime]   DATETIME      NULL,
    [EndTime]     DATETIME      NULL,
    [Status]      VARCHAR (50)  NULL,
    [Action]      VARCHAR (50)  NULL,
    [NotifyEmail] VARCHAR (50)  NOT NULL,
    [Message]     VARCHAR (250) NULL,
    CONSTRAINT [PK_Refresh] PRIMARY KEY CLUSTERED ([RefreshID] ASC) WITH (FILLFACTOR = 90)
);

