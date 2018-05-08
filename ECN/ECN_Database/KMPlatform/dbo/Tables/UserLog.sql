CREATE TABLE [dbo].[UserLog] (
    [UserLogID]        INT           IDENTITY (1, 1) NOT NULL,
    [ApplicationID]    INT           NOT NULL,
    [UserLogTypeID]    INT           NOT NULL,
    [UserID]           INT           NOT NULL,
    [Object]           NVARCHAR (50) NOT NULL,
    [FromObjectValues] TEXT          NOT NULL,
    [ToObjectValues]   TEXT          NOT NULL,
    [DateCreated]      DATETIME      NOT NULL,
	[ClientID]		   INT				 NULL,
	[GroupTransactionCode] VARCHAR(50)   NULL,
    CONSTRAINT [PK_UserLog] PRIMARY KEY CLUSTERED ([UserLogID] ASC)
);

