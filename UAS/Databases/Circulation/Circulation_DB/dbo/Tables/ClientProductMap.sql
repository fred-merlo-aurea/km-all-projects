CREATE TABLE [dbo].[ClientProductMap]
(
	ProductID INT NOT NULL,
	ClientID INT NOT NULL,
	ProductCode VARCHAR(50) NULL
	PRIMARY KEY(ProductID, ClientID)
)
