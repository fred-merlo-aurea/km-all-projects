﻿CREATE TABLE [dbo].[Emails_HIMSS_12302013] (
    [EmailID]        INT            IDENTITY (1, 1) NOT NULL,
    [EmailAddress]   VARCHAR (255)  NOT NULL,
    [CustomerID]     INT            NOT NULL,
    [Title]          VARCHAR (50)   NULL,
    [FirstName]      VARCHAR (50)   NULL,
    [LastName]       VARCHAR (50)   NULL,
    [FullName]       VARCHAR (50)   NULL,
    [Company]        VARCHAR (100)  NULL,
    [Occupation]     VARCHAR (50)   NULL,
    [Address]        VARCHAR (255)  NULL,
    [Address2]       VARCHAR (255)  NULL,
    [City]           VARCHAR (50)   NULL,
    [State]          VARCHAR (50)   NULL,
    [Zip]            VARCHAR (50)   NULL,
    [Country]        VARCHAR (50)   NULL,
    [Voice]          VARCHAR (50)   NULL,
    [Mobile]         VARCHAR (50)   NULL,
    [Fax]            VARCHAR (50)   NULL,
    [Website]        VARCHAR (50)   NULL,
    [Age]            VARCHAR (50)   NULL,
    [Income]         VARCHAR (50)   NULL,
    [Gender]         VARCHAR (50)   NULL,
    [User1]          VARCHAR (255)  NULL,
    [User2]          VARCHAR (255)  NULL,
    [User3]          VARCHAR (255)  NULL,
    [User4]          VARCHAR (255)  NULL,
    [User5]          VARCHAR (255)  NULL,
    [User6]          VARCHAR (255)  NULL,
    [Birthdate]      DATETIME       NULL,
    [UserEvent1]     VARCHAR (50)   NULL,
    [UserEvent1Date] DATETIME       NULL,
    [UserEvent2]     VARCHAR (50)   NULL,
    [UserEvent2Date] DATETIME       NULL,
    [Notes]          VARCHAR (1000) NULL,
    [DateAdded]      DATETIME       NULL,
    [DateUpdated]    DATETIME       NULL,
    [Password]       VARCHAR (25)   NULL,
    [BounceScore]    INT            NULL,
    [CarrierCode]    VARCHAR (10)   NULL,
    [SMSOptIn]       VARCHAR (10)   NULL
);

