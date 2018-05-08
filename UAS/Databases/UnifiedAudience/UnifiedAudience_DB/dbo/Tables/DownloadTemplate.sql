CREATE TABLE [dbo].[DownloadTemplate](
	[DownloadTemplateID] INT IDENTITY(1,1) NOT NULL,
	[DownloadTemplateName] VARCHAR(50) NULL,
	[PubID] INT NULL,
	[CreatedUserID] INT NULL,
	[CreatedDate] DATETIME NULL,
	[UpdatedUserID] INT NULL,
	[UpdatedDate] DATETIME NULL,
	[IsDeleted] BIT NULL,
	[BrandID] INT NULL,
CONSTRAINT [PK_DownloadTemplate] PRIMARY KEY CLUSTERED ([DownloadTemplateID] ASC)  WITH (FILLFACTOR = 90)
);
