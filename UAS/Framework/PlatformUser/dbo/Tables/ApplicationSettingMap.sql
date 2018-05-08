CREATE TABLE [dbo].[ApplicationSettingMap] (
    [ApplicationSettingMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ApplicationID]           INT      NOT NULL,
    [ApplicationSettingID]    INT      NOT NULL,
    [IsActive]                BIT      NOT NULL,
    [DateCreated]             DATETIME NOT NULL,
    [DateUpdated]             DATETIME NULL,
    [CreatedByUserID]         INT      NOT NULL,
    [UpdatedByUserID]         INT      NULL,
    CONSTRAINT [PK_ApplicationSettingMap] PRIMARY KEY CLUSTERED ([ApplicationID] ASC, [ApplicationSettingID] ASC),
    CONSTRAINT [FK_ApplicationSettingMap_Application] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Application] ([ApplicationID]),
    CONSTRAINT [FK_ApplicationSettingMap_ApplicationSetting] FOREIGN KEY ([ApplicationSettingID]) REFERENCES [dbo].[ApplicationSetting] ([ApplicationSettingID])
);

