CREATE TABLE [dbo].[CustomField] (
    [field_id]    INT           NOT NULL,
    [account_id]  INT           NOT NULL,
    [name]        VARCHAR (255) NULL,
    [display_as]  VARCHAR (255) NULL,
    [type]        VARCHAR (50)  NULL,
    [allow_other] BIT           NULL,
    [text_value]  VARCHAR (255) NULL,
	[KMResponseGroupID] INT		NULL,
	[KMProductCode]	VARCHAR(50) NULL,
	[KMProductId] INT NULL,
    CONSTRAINT [PK_CustomField] PRIMARY KEY CLUSTERED ([field_id] ASC) WITH (FILLFACTOR = 90)
);

