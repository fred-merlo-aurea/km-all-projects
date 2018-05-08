CREATE TABLE [dbo].[ApiLog] (
    [ApiLogId]         INT              IDENTITY (1, 1) NOT NULL,
    [ClientID]         INT              NOT NULL,
    [AccessKey]        UNIQUEIDENTIFIER NOT NULL,
    [RequestFromIP]    VARCHAR (50)     NOT NULL,
    [ApiId]            INT              NULL,
    [Entity]           VARCHAR (100)    NULL,
    [Method]           VARCHAR (100)    NULL,
    [ErrorMessage]     VARCHAR (MAX)    NULL,
    [RequestData]      VARCHAR (MAX)    NULL,
    [ResponseData]     VARCHAR (MAX)    NULL,
    [RequestStartDate] DATE             NOT NULL,
    [RequestStartTime] TIME (7)         NOT NULL,
    [RequestEndDate]   DATE             NULL,
    [RequestEndTime]   TIME (7)         NULL,
    PRIMARY KEY CLUSTERED ([ApiLogId] ASC)
);

