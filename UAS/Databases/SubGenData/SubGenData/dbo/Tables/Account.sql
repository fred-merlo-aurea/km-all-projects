CREATE TABLE [dbo].[Account] (
    [account_id]      INT           NOT NULL,
    [company_name]    VARCHAR (50)  NULL,
    [email]           VARCHAR (100) NULL,
    [website]         VARCHAR (100) NULL,
    [active]          BIT           NULL,
    [api_active]      BIT           NULL,
    [api_key]         VARCHAR (100) NULL,
    [api_login]       VARCHAR (50)  NULL,
    [plan]            VARCHAR (50)  NULL,
    [audit_type]      VARCHAR (50)  NULL,
    [created]         DATETIME      NULL,
    [master_checkout] VARCHAR (255) NULL,
	[KMClientId]      INT           NULL,
	[KMFtpFolder]	  VARCHAR(50)   NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([account_id] ASC) WITH (FILLFACTOR = 90)
);

