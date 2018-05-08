CREATE TABLE [dbo].[MAControl]
(
	[ControlID] VARCHAR(255) NOT NULL, 
    [ECNID] INT NULL, 
    [xPosition] DECIMAL(15, 2) NULL, 
    [yPosition] DECIMAL(15, 2) NULL, 
    [MarketingAutomationID] INT NULL, 
    [Text] VARCHAR(255) NULL, 
    [ExtraText] VARCHAR(255) NULL, 
    [ControlType] VARCHAR(255) NULL, 
    [MAControlID] INT NOT NULL IDENTITY, 
    CONSTRAINT [PK_MAControl] PRIMARY KEY ([MAControlID])
)
