CREATE TABLE [dbo].[CustomerTemplate] (
    [CTID]             INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT          NULL,
    [TemplateTypeCode] VARCHAR (50) NULL,
    [HeaderSource]     TEXT         NULL,
    [FooterSource]     TEXT         NULL,
    [IsActive]         BIT          CONSTRAINT [DF_CustomerTemplate_IsActive] DEFAULT ((0)) NULL,
    [CCNotifyEmail]    CHAR (1)     CONSTRAINT [DF_CustomerTemplates_CCNotifyEmail] DEFAULT ('N') NULL,
    [UpdatedDate]      DATETIME     NULL,
    [CreatedDate]      DATETIME     CONSTRAINT [DF_CustomerTemplate_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedUserID]    INT          NULL,
    [IsDeleted]        BIT          CONSTRAINT [DF_CustomerTemplate_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID]    INT          NULL,
    CONSTRAINT [PK_CustomerTemplates] PRIMARY KEY CLUSTERED ([CTID] ASC) WITH (FILLFACTOR = 80)
);

