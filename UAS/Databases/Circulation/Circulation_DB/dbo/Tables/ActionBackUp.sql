CREATE TABLE [dbo].[ActionBackUp]
(
	[ActionBackUpID] INT IDENTITY (1, 1) NOT NULL, 
    [ProductID] INT NOT NULL, 
    [SubscriptionID] INT NOT NULL, 
    [ActionID_Current] INT NOT NULL, 
    [ActionID_Previous] INT NULL
)
