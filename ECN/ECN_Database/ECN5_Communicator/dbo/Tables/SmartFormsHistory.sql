CREATE TABLE [dbo].[SmartFormsHistory] (
    [SmartFormID]              INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]                  INT            NULL,
    [SubscriptionGroupIDs]     VARCHAR (500)  NULL,
    [SmartFormName]            VARCHAR (500)  NULL,
    [SmartFormHtml]            TEXT           NULL,
    [SmartFormFields]          VARCHAR (8000) NULL,
    [Response_UserMsgSubject]  VARCHAR (500)  NULL,
    [Response_UserMsgBody]     TEXT           NULL,
    [Response_UserScreen]      TEXT           NULL,
    [Response_FromEmail]       VARCHAR (500)  NULL,
    [Response_AdminEmail]      VARCHAR (500)  NULL,
    [Response_AdminMsgSubject] VARCHAR (500)  NULL,
    [Response_AdminMsgBody]    TEXT           NULL,
    [CreatedDate]              DATETIME       CONSTRAINT [DF_SmartFormsHistory_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedDate]              DATETIME       NULL,
    [Type]                     VARCHAR (10)   CONSTRAINT [DF_SmartFormsHistory_Type] DEFAULT ('SO') NULL,
    [DoubleOptIn]              BIT            CONSTRAINT [DF_SmartFormsHistory_DoubleOptIn] DEFAULT (0) NULL,
    [CreatedUserID]            INT            NULL,
    [IsDeleted]                BIT            CONSTRAINT [DF_SmartFormsHistory_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID]            INT            NULL,
    CONSTRAINT [PK_SmartFormsHistory] PRIMARY KEY CLUSTERED ([SmartFormID] ASC) WITH (FILLFACTOR = 80)
);

