CREATE TABLE [dbo].[User] (
    [UserID]                  INT              IDENTITY (1, 1) NOT NULL,
    [DefaultClientGroupID]    INT              CONSTRAINT [DF_User_DefaultClientGroupID] DEFAULT ((-1)) NOT NULL,
    [DefaultClientID]         INT              CONSTRAINT [DF_User_DefaultClientID] DEFAULT ((-1)) NOT NULL,
    [Status]                  VARCHAR (50)     CONSTRAINT [DF_User_Status] DEFAULT ('DISABLED') NOT NULL,
    [FirstName]               VARCHAR (50)     NOT NULL,
    [LastName]                VARCHAR (50)     NOT NULL,
    [UserName]                VARCHAR (50)     NOT NULL,
    [Password]                VARCHAR (250)    NOT NULL,
    [Salt]                    VARCHAR (250)    NOT NULL,
    [EmailAddress]            VARCHAR (250)    NOT NULL,
    [IsActive]                BIT              NOT NULL,
    [AccessKey]               UNIQUEIDENTIFIER CONSTRAINT [DF__User__AccessKey__08B54D69] DEFAULT (newid()) NOT NULL,
    [IsAccessKeyValid]        BIT              CONSTRAINT [DF__User__IsAccessKe__09A971A2] DEFAULT ('false') NOT NULL,
    [IsKMStaff]               BIT              CONSTRAINT [DF_User_IsKMStaff] DEFAULT ((0)) NULL,
    [IsPlatformAdministrator] BIT              CONSTRAINT [DF_User_IsPlatformAdministrator] DEFAULT ((0)) NULL,
    [DateCreated]             DATETIME         CONSTRAINT [DF_User_DateAdded] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]             DATETIME         NULL,
    [CreatedByUserID]         INT              NOT NULL,
    [UpdatedByUserID]         INT              NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IDX_UserName]
    ON [dbo].[User]([UserName] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_EmailAddress_UNIQUE_KEY]
    ON [dbo].[User]([EmailAddress] ASC);

