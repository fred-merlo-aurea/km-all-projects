CREATE TABLE [dbo].[Emails] (
    [EmailID]         INT            IDENTITY (1, 1) NOT NULL,
    [EmailAddress]    VARCHAR (255)  NOT NULL,
    [CustomerID]      INT            NOT NULL,
    [Title]           VARCHAR (50)   NULL,
    [FirstName]       VARCHAR (50)   NULL,
    [LastName]        VARCHAR (50)   NULL,
    [FullName]        VARCHAR (50)   NULL,
    [Company]         VARCHAR (100)  NULL,
    [Occupation]      VARCHAR (50)   NULL,
    [Address]         VARCHAR (255)  NULL,
    [Address2]        VARCHAR (255)  NULL,
    [City]            VARCHAR (50)   NULL,
    [State]           VARCHAR (50)   NULL,
    [Zip]             VARCHAR (50)   NULL,
    [Country]         VARCHAR (50)   NULL,
    [Voice]           VARCHAR (50)   NULL,
    [Mobile]          VARCHAR (50)   NULL,
    [Fax]             VARCHAR (50)   NULL,
    [Website]         VARCHAR (50)   NULL,
    [Age]             VARCHAR (50)   NULL,
    [Income]          VARCHAR (50)   NULL,
    [Gender]          VARCHAR (50)   NULL,
    [User1]           VARCHAR (255)  NULL,
    [User2]           VARCHAR (255)  NULL,
    [User3]           VARCHAR (255)  NULL,
    [User4]           VARCHAR (255)  NULL,
    [User5]           VARCHAR (255)  NULL,
    [User6]           VARCHAR (255)  NULL,
    [Birthdate]       DATETIME       NULL,
    [UserEvent1]      VARCHAR (50)   NULL,
    [UserEvent1Date]  DATETIME       NULL,
    [UserEvent2]      VARCHAR (50)   NULL,
    [UserEvent2Date]  DATETIME       NULL,
    [Notes]           VARCHAR (1000) NULL,
    [DateAdded]       DATETIME       CONSTRAINT [DF_Emails_DateAdded] DEFAULT (getdate()) NULL,
    [DateUpdated]     DATETIME       NULL,
    [Password]        VARCHAR (25)   NULL,
    [BounceScore]     INT            CONSTRAINT [DF_Emails_BounceScore] DEFAULT ((0)) NULL,
    [CarrierCode]     VARCHAR (10)   NULL,
    [SMSOptIn]        VARCHAR (10)   NULL,
    [SoftBounceScore] INT            NULL,
    CONSTRAINT [PK_Emails] PRIMARY KEY CLUSTERED ([EmailID] ASC) WITH (FILLFACTOR = 80)
);

GO
CREATE NONCLUSTERED INDEX [IX_EmailAddress_CustomerID]
    ON [dbo].[Emails]([EmailAddress] ASC, [CustomerID] ASC) WITH (FILLFACTOR = 80);


GO


CREATE INDEX [IX_Emails_CustomerID_includes_EmailID] 
	ON [Emails] ([CustomerID])  INCLUDE ([EmailID]) WITH (FILLFACTOR=80, ONLINE=OFF, SORT_IN_TEMPDB=OFF);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Emails] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Emails] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Emails] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Emails] TO [ecn5writer]
    AS [dbo];


GO



GO



GO



GO
GRANT SELECT
    ON OBJECT::[dbo].[Emails] TO [reader]
    AS [dbo];

