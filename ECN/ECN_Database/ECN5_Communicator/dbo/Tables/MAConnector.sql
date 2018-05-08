CREATE TABLE [dbo].[MAConnector]
(
	[ConnectorID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [From] VARCHAR(255) NULL, 
    [To] VARCHAR(255) NULL, 
    [MarketingAutomationID] INT NULL, 
    [ControlID] VARCHAR(255) NULL
)
