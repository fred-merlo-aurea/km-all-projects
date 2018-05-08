CREATE TABLE [dbo].[SubscriptionManagementGroup]
(
	[SubscriptionManagementGroupID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SubscriptionManagementPageID] INT NULL, 
    [GroupID] INT NULL, 
    [Label] VARCHAR(200) NULL, 
    [IsDeleted] BIT NULL, 
    [CreatedUserID] INT NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL
)
