CREATE TABLE [dbo].[Bundle] (
    [bundle_id]   INT           NOT NULL,
    [account_id]  INT           NOT NULL,
    [name]        VARCHAR (250) NULL,
    [price]       FLOAT (53)    NULL,
    [active]      BIT           NULL,
    [promo_code]  VARCHAR (25)  NULL,
    [description] VARCHAR (250) NULL,
    [type]        VARCHAR (50)  NULL,
	[publication_id] int null,
	[issues] int default(0) not null
    CONSTRAINT [PK_Bundle] PRIMARY KEY CLUSTERED ([bundle_id] ASC) WITH (FILLFACTOR = 90)
);

