CREATE PROCEDURE [dbo].[o_CircProduct_Select]
	@ClientID int
AS
	Select PublicationID as ProductID, PublicationName as ProductName, PublicationCode as ProductCode, P.ClientID, PUB.IsActive, CAST('false' AS BIT) as IsUAD, CAST('true' AS BIT) as IsCirc, AllowDataEntry 
	from Publication PUB With(NoLock) join Publisher P With(NoLock) on PUB.PublisherID = P.PublisherID
	where P.ClientID = @ClientID
