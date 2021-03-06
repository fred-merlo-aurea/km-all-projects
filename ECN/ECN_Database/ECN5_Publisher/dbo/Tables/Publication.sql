﻿CREATE TABLE [dbo].[Publication] (
    [PublicationID]        INT           IDENTITY (1, 1) NOT NULL,
    [PublicationName]      VARCHAR (100) NOT NULL,
    [PublicationType]      VARCHAR (50)  NULL,
    [PublicationCode]      VARCHAR (25)  NULL,
    [CustomerID]           INT           NOT NULL,
    [GroupID]              INT           NULL,
    [Active]               BIT           NOT NULL,
    [CreatedDate]          DATETIME      CONSTRAINT [DF__Magazine__DateCr__76CBA758] DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]          DATETIME      CONSTRAINT [DF__Magazine__DateMo__77BFCB91] DEFAULT (getdate()) NOT NULL,
    [ContactAddress1]      VARCHAR (100) NULL,
    [ContactAddress2]      VARCHAR (100) NULL,
    [ContactEmail]         VARCHAR (100) NULL,
    [ContactPhone]         VARCHAR (25)  NULL,
    [EnableSubscription]   BIT           NULL,
    [LogoLink]             VARCHAR (255) NULL,
    [LogoURL]              VARCHAR (255) NULL,
    [SubscriptionOption]   INT           NULL,
    [CreatedUserID]        INT           NULL,
    [ContactFormLink]      VARCHAR (255) NULL,
    [SubscriptionFormLink] VARCHAR (255) NULL,
    [CategoryID]           INT           NULL,
    [Circulation]          INT           NULL,
    [FrequencyID]          INT           NULL,
    [IsDeleted]            BIT           CONSTRAINT [DF_Publications_IsDeleted] DEFAULT ((0)) NOT NULL,
    [UpdatedUserID]        INT           NULL,
    CONSTRAINT [PK_Magazines] PRIMARY KEY CLUSTERED ([PublicationID] ASC) WITH (FILLFACTOR = 80)
);

