CREATE TABLE [dbo].[User] (
    [user_id]      INT           NOT NULL,
    [account_id]   INT           NOT NULL,
    [first_name]   VARCHAR (50)  NULL,
    [last_name]    VARCHAR (50)  NULL,
    [password]     VARCHAR (25)  NULL,
    [password_md5] VARCHAR (32)  NULL,
    [email]        VARCHAR (100) NULL,
    [is_admin]     BIT           NULL,
    [KMUserID]     INT           NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY ([user_id])
);



