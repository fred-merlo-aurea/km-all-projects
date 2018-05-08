CREATE PROCEDURE e_Subscription_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM Subscription With(NoLock) WHERE PublicationID = @PublicationID
