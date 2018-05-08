CREATE TABLE [dbo].[IPLocationDetailed] (
    [IPLocationDetailedId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [IPaddress]            VARCHAR (50)  NULL,
    [IPRangeValue]         BIGINT        NULL,
    [Region]               VARCHAR (2)   NULL,
    [Country]              VARCHAR (128) NULL,
    [State]                VARCHAR (128) NULL,
    [City]                 VARCHAR (128) NULL,
    [Lat]                  DECIMAL (18)  NULL,
    [Long]                 DECIMAL (18)  NULL,
    [Zip]                  VARCHAR (50)  NULL,
    [TimeZone]             VARCHAR (50)  NULL,
    CONSTRAINT [PK_IPLocationDetailed] PRIMARY KEY CLUSTERED ([IPLocationDetailedId] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_IPLocationDetailed_IPAddress]
    ON [dbo].[IPLocationDetailed]([IPaddress] ASC);

