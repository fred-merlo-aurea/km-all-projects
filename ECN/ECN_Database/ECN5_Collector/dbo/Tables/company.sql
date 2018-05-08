CREATE TABLE [dbo].[company] (
    [company_id] INT           IDENTITY (1, 1) NOT NULL,
    [name]       VARCHAR (75)  NULL,
    [address]    VARCHAR (50)  NULL,
    [city]       VARCHAR (50)  NULL,
    [state]      VARCHAR (50)  NULL,
    [zip]        VARCHAR (10)  NULL,
    [country]    VARCHAR (50)  NULL,
    [phone]      VARCHAR (15)  NULL,
    [fax]        VARCHAR (15)  NULL,
    [url]        VARCHAR (255) NULL,
    CONSTRAINT [PK_company] PRIMARY KEY CLUSTERED ([company_id] ASC) WITH (FILLFACTOR = 80)
);

