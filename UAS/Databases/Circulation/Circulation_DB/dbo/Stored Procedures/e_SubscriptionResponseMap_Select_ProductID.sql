CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select_ProductID]
@ProductID int
AS
	Select s.* FROM SubscriptionResponseMap s With(NoLock)
	JOIN Subscription sp With(NoLock) ON sp.SubscriptionID = s.SubscriptionID
	WHERE sp.PublicationID = @ProductID