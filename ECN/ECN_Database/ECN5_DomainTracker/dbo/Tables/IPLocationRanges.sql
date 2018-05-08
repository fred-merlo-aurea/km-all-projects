CREATE TABLE [dbo].[IPLocationRanges] (
    [IPStart]  DECIMAL (18)  NOT NULL,
    [IPEnd]    DECIMAL (18)  NOT NULL,
    [Region]   VARCHAR (2)   NULL,
    [Country]  VARCHAR (128) NULL,
    [State]    VARCHAR (128) NULL,
    [City]     VARCHAR (128) NULL,
    [Lat]      DECIMAL (18)  NULL,
    [Long]     DECIMAL (18)  NULL,
    [Zip]      VARCHAR (50)  NULL,
    [TimeZone] VARCHAR (50)  NULL,
    CONSTRAINT [PK_IPLocationRanges] PRIMARY KEY CLUSTERED ([IPStart] ASC, [IPEnd] ASC)
);



