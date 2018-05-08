CREATE TABLE [dbo].[Subscriber] (
    [subscriber_id] INT           NOT NULL,
    [account_id]    INT           NOT NULL,
    [renewal_code]  VARCHAR (5)   NULL,
    [email]         VARCHAR (100) NULL,
    [password]      VARCHAR (25)  NULL,
    [password_md5]  VARCHAR (32)  NULL,
    [first_name]    VARCHAR (25)  NULL,
    [last_name]     VARCHAR (25)  NULL,
    [source]        VARCHAR (100) NULL,
    [create_date]   DATETIME      NULL,
    [delete_date]   DATETIME      NULL, 
    CONSTRAINT [PK_Subscriber] PRIMARY KEY ([subscriber_id])
);

