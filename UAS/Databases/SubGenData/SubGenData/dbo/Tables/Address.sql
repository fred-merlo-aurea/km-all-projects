CREATE TABLE [dbo].[Address] (
    [address_id]           INT              NOT NULL,
    [account_id]           INT              NOT NULL,
    [first_name]           VARCHAR (50)     NULL,
    [last_name]            VARCHAR (50)     NULL,
    [address]              VARCHAR (50)     NULL,
    [address_line_2]       VARCHAR (50)     NULL,
    [company]              VARCHAR (50)     NULL,
    [city]                 VARCHAR (50)     NULL,
    [state]                VARCHAR (60)     NULL,
    [subscriber_id]        INT              NULL,
    [country]              VARCHAR (50)     NULL,
    [country_name]         VARCHAR (50)     NULL,
    [country_abbreviation] VARCHAR (50)     NULL,
    [latitude]             DECIMAL (18, 10) NULL,
    [longitude]            DECIMAL (18, 10) NULL,
    [verified]             BIT              NULL,
    [zip_code]             VARCHAR (12)     NULL, 
    CONSTRAINT [PK_Address] PRIMARY KEY ([address_id])
);

