CREATE TABLE [dbo].[MarketingAutomationHistory]
(
	[MarketingAutomationHistoryID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MarketingAutomationID] INT NOT NULL, 
    [UserID] INT NOT NULL, 
    [Action] VARCHAR(MAX) NOT NULL, 
    [HistoryDate] DATETIME NOT NULL
)
