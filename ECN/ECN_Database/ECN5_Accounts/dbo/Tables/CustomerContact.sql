CREATE TABLE [dbo].[CustomerContact] (
    [ContactID]     INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT           NOT NULL,
    [FirstName]     VARCHAR (50)  NOT NULL,
    [LastName]      VARCHAR (50)  NULL,
    [Address]       VARCHAR (100) NULL,
    [Address2]      VARCHAR (50)  NULL,
    [City]          VARCHAR (50)  NULL,
    [State]         VARCHAR (2)   NULL,
    [Zip]           VARCHAR (10)  NULL,
    [Phone]         VARCHAR (20)  NULL,
    [Mobile]        VARCHAR (20)  NULL,
    [Email]         VARCHAR (50)  NULL,
    [Createdby]     VARCHAR (50)  NULL,
    [Updatedby]     VARCHAR (50)  NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_CustomerContacts_DateAdded] DEFAULT (getdate()) NULL,
    [UpdatedDate]   DATETIME      NULL,
    [CreatedUserID] INT           NULL,
    [UpdatedUserID] INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_CustomerContact_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CustomerContact] PRIMARY KEY CLUSTERED ([ContactID] ASC) WITH (FILLFACTOR = 80)
);

