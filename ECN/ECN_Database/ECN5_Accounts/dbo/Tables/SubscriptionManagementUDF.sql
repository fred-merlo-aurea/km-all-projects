CREATE TABLE [dbo].[SubscriptionManagementUDF]
(
	[SubscriptionManagementUDFID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SubscriptionManagementGroupID] INT NULL, 
    [GroupDataFieldsID] INT NULL, 
    [StaticValue] VARCHAR(100) NULL, 
    [IsDeleted] BIT NULL, 
    [CreatedUserID] INT NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL
)
