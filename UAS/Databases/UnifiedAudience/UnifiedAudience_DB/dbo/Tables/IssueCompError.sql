CREATE TABLE [dbo].[IssueCompError] (
    [IssueCompErrorID]   INT              IDENTITY (1, 1) NOT NULL,
    [CompName]           VARCHAR (200)    NOT NULL,
    [SFRecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [ProcessCode]        VARCHAR (50)     NOT NULL,
    [DateCreated]        DATETIME         NOT NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NOT NULL,
    [UpdatedByUserID]    INT              NULL,
    CONSTRAINT [PK_IssueCompError_IssueCompErrorID] PRIMARY KEY CLUSTERED ([IssueCompErrorID] ASC) WITH (FILLFACTOR = 90)
);