CREATE TABLE [dbo].[ApplicationSetting] (
    [ApplicationSettingID] INT IDENTITY(1,1)     NOT NULL,
	[AppSettingDescription]  varchar(250) NOT NULL,
    [IsActive]             BIT      NOT NULL,
    [DateCreated]          DATETIME NOT NULL,
    [DateUpdated]          DATETIME NULL,
    [CreatedByUserID]      INT      NOT NULL,
    [UpdatedByUserID]      INT      NULL,
    CONSTRAINT [PK_ApplicationSetting] PRIMARY KEY CLUSTERED ([ApplicationSettingID] ASC)
);

