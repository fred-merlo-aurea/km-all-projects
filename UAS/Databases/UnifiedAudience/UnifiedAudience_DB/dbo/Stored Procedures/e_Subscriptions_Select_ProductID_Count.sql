CREATE PROCEDURE [dbo].[e_Subscriptions_Select_ProductID_Count]
@ProductID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	Select COUNT(*) 
	FROM Subscriptions s with(nolock)
		JOIN PubSubscriptions sp with(nolock) ON s.SubscriptionID = sp.SubscriptionID
	WHERE sp.PubID = @ProductID
	
END