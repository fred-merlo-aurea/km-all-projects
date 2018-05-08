CREATE TABLE [dbo].[BillingContact] (
    [BillingContactID] INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NOT NULL,
    [Salutation]       VARCHAR (10)  NOT NULL,
    [ContactName]      VARCHAR (50)  NOT NULL,
    [FirstName]        VARCHAR (50)  NULL,
    [LastName]         VARCHAR (50)  NULL,
    [ContactTitle]     VARCHAR (50)  NOT NULL,
    [Phone]            VARCHAR (50)  NOT NULL,
    [Fax]              VARCHAR (50)  NOT NULL,
    [Email]            VARCHAR (50)  NOT NULL,
    [StreetAddress]    VARCHAR (200) NOT NULL,
    [City]             VARCHAR (50)  NOT NULL,
    [State]            VARCHAR (50)  NOT NULL,
    [Country]          VARCHAR (20)  NOT NULL,
    [Zip]              VARCHAR (20)  NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_BillingContacts_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedUserID]    INT           NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_BillingContact_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]    INT           NULL,
    [UpdatedDate]      DATETIME      NULL,
    CONSTRAINT [PK_BillingContacts] PRIMARY KEY CLUSTERED ([BillingContactID] ASC) WITH (FILLFACTOR = 80)
);

