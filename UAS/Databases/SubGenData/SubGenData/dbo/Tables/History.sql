CREATE TABLE [dbo].[History] (
    [history_id]    INT          NOT NULL,
    [account_id]    INT          NOT NULL,
    [user_id]       INT          NULL,
    [notes]         VARCHAR (50) NULL,
    [date]          DATETIME     NULL,
    [subscriber_id] INT          NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([history_id] ASC) WITH (FILLFACTOR = 90)
);

