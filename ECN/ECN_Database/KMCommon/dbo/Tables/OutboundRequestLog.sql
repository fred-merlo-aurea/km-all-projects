CREATE TABLE [dbo].[OutboundRequestLog] (
    [OutboundRequestLogId] INT           IDENTITY (1, 1) NOT NULL,
    [InboundRequestLogId]  INT           NOT NULL,
    [RequestStartDateTime] DATETIME2 (7) NOT NULL,
    [RequestEndDateTime]   DATETIME2 (7) NULL,
    [RequestFromIp]        VARCHAR (15)  CONSTRAINT [DF__OutboundR__Reque__656C112C] DEFAULT ('') NOT NULL,
    [TargetAddress]        VARCHAR (255) NOT NULL,
    [ServiceMethod]        INT           NOT NULL,
    [ErrorMessage]         VARCHAR (MAX) NULL,
    [RequestData]          VARCHAR (MAX) NOT NULL,
    [ResponseData]         VARCHAR (MAX) NULL,
    CONSTRAINT [PK__tmp_ms_x__4D62053D60A75C0F] PRIMARY KEY CLUSTERED ([OutboundRequestLogId] ASC),
    CONSTRAINT [FK_OutboundRequestLog_InboundRequestLog] FOREIGN KEY ([InboundRequestLogId]) REFERENCES [dbo].[InboundRequestLog] ([InboundRequestLogID])
);

