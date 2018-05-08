CREATE TABLE [dbo].[GenerateMailingList] (
    [mailing_list_id] INT          NOT NULL,
    [publication_id]  INT          NOT NULL,
    [account_id]      INT          NOT NULL,
    [IsNCOA]          BIT          CONSTRAINT [DF__GenerateM__IsNCO__276EDEB3] DEFAULT ('false') NOT NULL,
    [grace_date]      DATE         NULL,
    [grace_issues]    INT          CONSTRAINT [DF__GenerateM__grace__286302EC] DEFAULT ((0)) NULL,
    [name]            VARCHAR (50) NULL
);

