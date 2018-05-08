CREATE PROCEDURE [dbo].[e_Publication_Select_Client]
	@ClientID int
AS
	Select PUB.*
	from Publication PUB With(NoLock) join Publisher P With(NoLock) on PUB.PublisherID = P.PublisherID
	where P.ClientID = @ClientID
