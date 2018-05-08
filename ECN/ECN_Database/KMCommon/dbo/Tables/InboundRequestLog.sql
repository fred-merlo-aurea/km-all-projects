CREATE TABLE [dbo].[InboundRequestLog] (
    [InboundRequestLogID]  INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]             INT           NOT NULL,
    [RequestStartDateTime] DATETIME2 (7) NOT NULL,
    [RequestEndDateTime]   DATETIME2 (7) NULL,
    [RequestFromIP]        VARCHAR (15)  CONSTRAINT [DF__InboundRe__Reque__6383C8BA] DEFAULT ('') NOT NULL,
    [ServiceMethodID]      INT           NOT NULL,
    [ErrorMessage]         VARCHAR (MAX) NULL,
    [RequestData]          VARCHAR (MAX) NOT NULL,
    [ResponseData]         VARCHAR (MAX) CONSTRAINT [DF__InboundRe__Respo__66603565] DEFAULT ('') NULL,
    CONSTRAINT [PK__tmp_ms_x__D269204A5BE2A6F2] PRIMARY KEY CLUSTERED ([InboundRequestLogID] ASC)
);

