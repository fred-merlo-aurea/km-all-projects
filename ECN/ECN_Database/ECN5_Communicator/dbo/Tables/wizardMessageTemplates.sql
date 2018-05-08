CREATE TABLE [dbo].[wizardMessageTemplates] (
    [WizMsgTemplateID]    INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]          INT            NULL,
    [TemplateName]        VARCHAR (50)   CONSTRAINT [DF_wizardMessageTemplates_TemplateName] DEFAULT ('') NULL,
    [TemplateDescription] VARCHAR (255)  CONSTRAINT [DF_wizardMessageTemplates_TemplateDescription] DEFAULT ('') NULL,
    [AdHocEmailAddresses] VARCHAR (1000) CONSTRAINT [DF_wizardMessageTemplates_AdHocEmailAddresses] DEFAULT ('') NULL,
    [CustReference]       VARCHAR (50)   CONSTRAINT [DF_wizardMessageTemplates_CustReference] DEFAULT ('') NULL,
    [BillingCode]         VARCHAR (50)   CONSTRAINT [DF_wizardMessageTemplates_BillingCode] DEFAULT ('') NULL,
    [FromName]            VARCHAR (50)   CONSTRAINT [DF_wizardMessageTemplates_FromName] DEFAULT ('') NULL,
    [FromEmail]           VARCHAR (50)   CONSTRAINT [DF_wizardMessageTemplates_FromEmail] DEFAULT ('tradeshowam@kmpsgroup.com') NULL,
    [ReplyToEmail]        VARCHAR (50)   CONSTRAINT [DF_wizardMessageTemplates_ReplyToEmail] DEFAULT ('tradeshowam@kmpsgroup.com') NULL,
    [EmailSubject]        VARCHAR (50)   CONSTRAINT [DF_wizardMessageTemplates_EmailSubject] DEFAULT ('') NULL,
    [DateAdded]           DATETIME       CONSTRAINT [DF_wizardMessageTemplates_DateAdded] DEFAULT (getdate()) NULL,
    [DateUpdated]         DATETIME       CONSTRAINT [DF_wizardMessageTemplates_DateUpdated] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_wizardMessageTemplates] PRIMARY KEY CLUSTERED ([WizMsgTemplateID] ASC) WITH (FILLFACTOR = 100)
);

