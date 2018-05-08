CREATE TABLE [dbo].[Publication] (
    [publication_id]  INT          NOT NULL,
    [account_id]      INT          NOT NULL,
    [name]            VARCHAR (50) NULL,
    [issues_per_year] INT          NULL,
	[KMPubCode]       VARCHAR (50) NULL,
    [KMPubID]         INT          NULL,
    CONSTRAINT [PK_Publication] PRIMARY KEY CLUSTERED ([publication_id] ASC) WITH (FILLFACTOR = 90)
);

