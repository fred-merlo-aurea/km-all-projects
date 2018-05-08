CREATE TABLE [dbo].[MarketingAutomation]
(
	[MarketingAutomationID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(500) NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedDate] DATETIME NULL, 
    [CreatedUserID] INT NULL, 
    [UpdatedUserID] INT NULL, 
    [IsDeleted] BIT NULL, 
    [Goal] VARCHAR(MAX) NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL, 
    [JSONDiagram] VARCHAR(MAX) NULL, 
    [CustomerID] INT NULL, 
    [State] VARCHAR(100) NULL
)
