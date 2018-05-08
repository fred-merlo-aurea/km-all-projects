CREATE TABLE [dbo].[tmp_Migration_AMSUsers] (
    [AMSUserID]    INT            NOT NULL,
    [FirstName]    VARCHAR (50)   NOT NULL,
    [LastName]     VARCHAR (50)   NOT NULL,
    [username]     VARCHAR (50)   NOT NULL,
    [emailaddress] VARCHAR (250)  NOT NULL,
    [clientID]     INT            NOT NULL,
    [DBname]       VARCHAR (8000) NULL,
    [IsAmsUser]    INT            NOT NULL
);

