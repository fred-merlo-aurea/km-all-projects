CREATE TABLE [dbo].[Users] (
    [UserID]              INT              IDENTITY (1, 1) NOT NULL,
    [CustomerID]          INT              NULL,
    [UserName]            VARCHAR (100)    NULL,
    [Password]            VARCHAR (50)     NULL,
    [CommunicatorOptions] VARCHAR (50)     NULL,
    [CollectorOptions]    VARCHAR (50)     NULL,
    [CreatorOptions]      VARCHAR (50)     NULL,
    [AccountsOptions]     VARCHAR (50)     NULL,
    [ActiveFlag]          VARCHAR (1)      NULL,
    [CreatedDate]         DATETIME         CONSTRAINT [DF_Users_CreatedDate] DEFAULT (getdate()) NULL,
    [AcceptTermsDate]     DATETIME         NULL,
    [RoleID]              INT              NULL,
    [AccessKey]           UNIQUEIDENTIFIER CONSTRAINT [DF_Users_AccessKey] DEFAULT (newid()) NULL,
    [CreatedUserID]       INT              NULL,
    [IsDeleted]           BIT              CONSTRAINT [DF_Users_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]         DATETIME         NULL,
    [UpdatedUserID]       INT              NULL,
    CONSTRAINT [Users_PK] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_Users_AccessKey]
    ON [dbo].[Users]([AccessKey] ASC) WITH (FILLFACTOR = 80);

