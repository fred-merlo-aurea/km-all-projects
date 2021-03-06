﻿CREATE TABLE [dbo].[ApplicationLog] (
    [ApplicationLogId] INT           IDENTITY (1, 1) NOT NULL,
    [ApplicationId]    INT           NOT NULL,
    [SeverityCodeId]   INT           NOT NULL,
    [SourceMethod]     VARCHAR (250) NULL,
    [Exception]        VARCHAR (MAX) NOT NULL,
    [LogNote]          VARCHAR (MAX) NULL,
    [IsBug]            BIT           NULL,
    [IsUserSubmitted]  BIT           NULL,
    [ClientId]         INT           NULL,
    [SubmittedBy]      VARCHAR (250) NULL,
    [SubmittedByEmail] VARCHAR (100) NULL,
    [IsFixed]          BIT           NULL,
    [FixedDate]        DATE          NULL,
    [FixedTime]        TIME (7)      NULL,
    [FixedBy]          VARCHAR (50)  NULL,
    [FixedNote]        VARCHAR (750) NULL,
    [LogAddedDate]     DATE          DEFAULT (getdate()) NOT NULL,
    [LogAddedTime]     TIME (7)      DEFAULT (getdate()) NOT NULL,
    [LogUpdatedDate]   DATE          NULL,
    [LogUpdatedTime]   TIME (7)      NULL,
    [NotificationSent] BIT           DEFAULT ('false') NOT NULL,
    CONSTRAINT [PK_ApplicationLog] PRIMARY KEY CLUSTERED ([ApplicationLogId] ASC) WITH (FILLFACTOR = 80)
);

