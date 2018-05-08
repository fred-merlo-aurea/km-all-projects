CREATE TABLE [dbo].[Issue] (
    [issue_id]   INT          NOT NULL,
    [account_id] INT          NOT NULL,
    [name]       VARCHAR (50) NULL,
    [date]       DATETIME     NULL,
    CONSTRAINT [PK_Issue] PRIMARY KEY CLUSTERED ([issue_id] ASC) WITH (FILLFACTOR = 90)
);

