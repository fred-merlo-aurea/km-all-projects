CREATE TABLE [dbo].[SFSettings] (
    [SFSettingsID]                 INT            IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]                INT            NULL,
    [CustomerID]                   INT            NULL,
    [CustomerCanOverride]          BIT            NULL,
    [CustomerDoesOverride]         BIT            NULL,
    [PushChannelMasterSuppression] BIT            NULL,
    [CreatedUserID]                INT            NULL,
    [UpdatedUserID]                INT            NULL,
    [CreatedDate]                  DATETIME       NULL,
    [UpdatedDate]                  DATETIME       NULL,
    [RefreshToken]                 VARCHAR (1000) NULL,
    [ConsumerKey]                  VARCHAR (500)  NULL,
    [ConsumerSecret]               VARCHAR (500)  NULL,
    [SandboxMode]                  BIT            NULL,
    CONSTRAINT [PK_SFSettings] PRIMARY KEY CLUSTERED ([SFSettingsID] ASC)
);

