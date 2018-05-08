CREATE TABLE [dbo].[SmartFormActivityLog] (
    [SALID]         INT           IDENTITY (1, 1) NOT NULL,
    [SFID]          INT           NOT NULL,
    [CustomerID]    INT           NOT NULL,
    [GroupID]       INT           NOT NULL,
    [EmailID]       INT           NOT NULL,
    [EmailType]     VARCHAR (10)  NOT NULL,
    [EmailTo]       VARCHAR (500) NOT NULL,
    [EmailFrom]     VARCHAR (500) NOT NULL,
    [EmailSubject]  VARCHAR (500) NOT NULL,
    [SendTime]      DATETIME      CONSTRAINT [DF_SmartFormActivityLog_SendTime] DEFAULT (getdate()) NOT NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_SmartFormActivityLog_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_SmartFormActivityLog_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    CONSTRAINT [PK_SmartFormActivityLog] PRIMARY KEY CLUSTERED ([SALID] ASC) WITH (FILLFACTOR = 80)
);

