CREATE PROCEDURE e_SubscriptionResponseMap_Select_SubscriptionID
@SubscriptionID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT * 
	FROM SubscriptionResponseMap With(NoLock) 
	WHERE SubscriptionID = @SubscriptionID

END