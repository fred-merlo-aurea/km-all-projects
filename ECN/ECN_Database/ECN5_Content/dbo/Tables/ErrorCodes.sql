CREATE TABLE [dbo].[ErrorCodes] (
    [ErrorCode]    INT           IDENTITY (1, 1) NOT NULL,
    [ErrorMessage] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ErrorCodes] PRIMARY KEY CLUSTERED ([ErrorCode] ASC)
);

