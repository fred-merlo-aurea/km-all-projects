CREATE TABLE [dbo].[ValueOption] (
    [option_id]    INT          NOT NULL,
    [field_id]     INT          NULL,
    [account_id]   INT          NOT NULL,
    [value]        VARCHAR (255) NULL,
    [display_as]   VARCHAR (255) NULL,
    [disqualifier] BIT          NULL,
    [active]       BIT          NULL,
    [order]        INT          NULL,
	[KMCodeSheetID] INT			NULL,
	[KMProductCode]	VARCHAR(50) NULL,
	[KMProductId] INT NULL,
    CONSTRAINT [PK_ValueOption] PRIMARY KEY CLUSTERED ([option_id] ASC) WITH (FILLFACTOR = 90)
);

