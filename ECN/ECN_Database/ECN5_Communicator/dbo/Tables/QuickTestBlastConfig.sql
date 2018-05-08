CREATE TABLE [dbo].[QuickTestBlastConfig]
(
	[QTBCID] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [IsDefault] BIT NULL, 
    [BaseChannelID] INT NULL, 
	[BaseChannelDoesOverride] BIT NULL,
    [CustomerCanOverride] BIT NULL, 
    [CustomerID] INT NULL, 
    [CustomerDoesOverride] BIT NULL, 
    [AllowAdhocEmails] BIT NULL, 
    [AutoCreateGroup] BIT NULL, 
    [AutoArchiveGroup] BIT NULL, 
    [CreatedUserID] INT NULL, 
    [CreatedDate] DATETIME NULL,
	[UpdatedUserID] INT NULL,
    [UpdatedDate] DATETIME NULL
)
