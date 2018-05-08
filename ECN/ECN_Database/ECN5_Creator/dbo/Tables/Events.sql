CREATE TABLE [dbo].[Events] (
    [EventID]       INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT          NULL,
    [EventTypeCode] VARCHAR (50) NULL,
    [EventName]     VARCHAR (50) NULL,
    [StartDate]     DATETIME     NULL,
    [EndDate]       DATETIME     NULL,
    [Times]         VARCHAR (50) NULL,
    [Location]      VARCHAR (50) NULL,
    [Description]   TEXT         NULL,
    [DisplayFlag]   VARCHAR (1)  NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([EventID] ASC) WITH (FILLFACTOR = 80)
);

