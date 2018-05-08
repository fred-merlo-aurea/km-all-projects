CREATE TABLE [dbo].[IP2Location] (
    [IPStart]  DECIMAL(18)        NULL,
    [IPEnd]    DECIMAL(18)        NULL,
    [Region]   VARCHAR (2) NULL,
    [Country]  VARCHAR (128) NULL,
    [State]    VARCHAR (128) NULL,
    [City]     VARCHAR (128) NULL,
    [Lat]      DECIMAL  NULL,
    [Long]     DECIMAL  NULL,
    [Zip]      VARCHAR (50)  NULL,
    [TimeZone] VARCHAR (50)  NULL
);


GO
CREATE NONCLUSTERED INDEX [IpEnd]
    ON [dbo].[IP2Location]([IpEnd] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IpStart]
    ON [dbo].[IP2Location]([IpStart] ASC) WITH (FILLFACTOR = 80);

