CREATE TABLE [dbo].[ActivityLog] (
    [ActivityLogID] UNIQUEIDENTIFIER CONSTRAINT [DF_ActivityLog_ActivityLogID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UserID]        UNIQUEIDENTIFIER NULL,
    [Activity]      VARCHAR (150)    NULL,
    [PageUrl]       VARCHAR (150)    NULL,
    [ActivityDate]  DATETIME         NULL,
    [SessionID]     VARCHAR (50)     NULL,
    CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED ([ActivityLogID] ASC)
);

