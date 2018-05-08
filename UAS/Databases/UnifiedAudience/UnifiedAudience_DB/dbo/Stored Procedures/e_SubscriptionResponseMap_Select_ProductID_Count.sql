CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select_ProductID_Count]
@ProductID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	Select COUNT(*) 
	FROM SubscriptionResponseMap srm with(nolock)
		JOIN PubSubscriptions s  with(nolock) ON srm.SubscriptionID = s.SubscriptionID
	WHERE s.PubID = @ProductID 

END