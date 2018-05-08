CREATE TABLE [dbo].[ActionBackUp]
(
	[ActionBackUpID] INT IDENTITY (1, 1) NOT NULL, 
    [ProductID] INT NOT NULL, 
    [PubSubscriptionID] INT NOT NULL, 
    [PubCategoryID] INT NOT NULL, 
    [PubTransactionID] INT NOT NULL
)
