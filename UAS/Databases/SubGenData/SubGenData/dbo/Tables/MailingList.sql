CREATE TABLE [dbo].[MailingList] (
    [mailing_list_id]        INT          NOT NULL,
    [account_id]             INT          NOT NULL,
    [parent_mailing_list_id] INT          NULL,
    [count]                  INT          NULL,
    [date_created]           DATETIME     NULL,
    [grace_from_date]        DATETIME     NULL,
    [grace_issues]           INT          NULL,
    [is_gap]                 BIT          NULL,
    [name]                   VARCHAR (50) NULL,
    [status]                 VARCHAR (50) NULL,
    CONSTRAINT [PK_MailingList] PRIMARY KEY CLUSTERED ([mailing_list_id] ASC) WITH (FILLFACTOR = 90)
);

