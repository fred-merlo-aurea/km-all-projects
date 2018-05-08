CREATE TABLE [dbo].[UserLog] (
    [UserLogID]        INT          IDENTITY (1, 1) NOT NULL,
    [ApplicationID]    INT          NOT NULL,
    [UserLogTypeID]    INT          NOT NULL,
    [UserID]           INT          NOT NULL,
    [Object]           VARCHAR (50) NOT NULL,
    [FromObjectValues] VARCHAR(MAX) NULL,
    [ToObjectValues]   VARCHAR(MAX) NULL,
    [DateCreated]      DATETIME     NOT NULL,
    CONSTRAINT [PK_UserLog] PRIMARY KEY CLUSTERED ([UserLogID] ASC) WITH (FILLFACTOR = 80)
);

