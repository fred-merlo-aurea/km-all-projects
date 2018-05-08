CREATE TABLE [dbo].[SubscriptionManagementReason]
(
	[SubscriptionManagementReasonID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SubscriptionManagementID] INT NULL, 
    [Reason] VARCHAR(500) NULL, 
    [IsDeleted] BIT NULL, 
    [CreatedUserID] INT NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [SortOrder] INT NULL
)
