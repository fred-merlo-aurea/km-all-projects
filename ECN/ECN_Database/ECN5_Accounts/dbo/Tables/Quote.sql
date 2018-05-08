﻿CREATE TABLE [dbo].[Quote] (
    [QuoteID]          INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NOT NULL,
    [ChannelID]        INT           CONSTRAINT [DF_Quotes_ChannelID] DEFAULT ((-1)) NOT NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_Quote_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]      DATETIME      NOT NULL,
    [ApproveDate]      DATETIME      CONSTRAINT [DF_Quotes_ApprovedDate] DEFAULT ('1/1/2318') NOT NULL,
    [StartDate]        DATETIME      NOT NULL,
    [Salutation]       VARCHAR (50)  NOT NULL,
    [FirstName]        VARCHAR (50)  NOT NULL,
    [LastName]         VARCHAR (50)  NOT NULL,
    [Email]            VARCHAR (100) NOT NULL,
    [Phone]            VARCHAR (50)  CONSTRAINT [DF_Quotes_Phone] DEFAULT ('') NOT NULL,
    [Fax]              VARCHAR (50)  NULL,
    [Company]          VARCHAR (50)  CONSTRAINT [DF_Quotes_Company] DEFAULT ('') NOT NULL,
    [BillType]         VARCHAR (20)  NOT NULL,
    [CreatedUserID]    INT           NOT NULL,
    [UpdatedUserID]    INT           NULL,
    [AccountManagerID] INT           NULL,
    [NBDIDs]           VARCHAR (50)  NULL,
    [Status]           SMALLINT      NOT NULL,
    [Notes]            TEXT          NULL,
    [TestUserName]     VARCHAR (100) CONSTRAINT [DF_Quotes_TestUserName] DEFAULT ('') NULL,
    [TestPassword]     VARCHAR (20)  CONSTRAINT [DF_Quotes_TestPassword] DEFAULT ('') NULL,
    [InternalNotes]    TEXT          CONSTRAINT [DF_Quotes_InternalNotes] DEFAULT ('') NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_Quotes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Quotes] PRIMARY KEY CLUSTERED ([QuoteID] ASC) WITH (FILLFACTOR = 80)
);
