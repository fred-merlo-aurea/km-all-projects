CREATE TABLE [dbo].[UserAuthorizationLog] (
    [UserAuthLogID]   INT              IDENTITY (1, 1) NOT NULL,
    [AuthSource]      VARCHAR (50)     NOT NULL,
    [AuthMode]        VARCHAR (50)     NOT NULL,
    [AuthAttemptDate] DATE             NOT NULL,
    [AuthAttemptTime] TIME (7)         NOT NULL,
    [IsAuthenticated] BIT              DEFAULT ('false') NOT NULL,
    [IpAddress]       VARCHAR (15)     NULL,
    [AuthUserName]    VARCHAR (50)     NULL,
    [AuthAccessKey]   UNIQUEIDENTIFIER NULL,
    [ServerVariables] VARCHAR (MAX)    NULL,
    [AppVersion]      VARCHAR (50)     NULL,
    [UserID]          INT              NULL,
    [LogOutDate]      DATE             NULL,
    [LogOutTime]      TIME (7)         NULL,
    PRIMARY KEY CLUSTERED ([UserAuthLogID] ASC)
);

