CREATE PROCEDURE [e_Subscription_Select_Email]
@Email varchar(100)
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT DISTINCT s.*
    FROM Subscriptions s With(NoLock) 
		JOIN PubSubscriptions ps With(NoLock) ON s.SubscriptionID = ps.SubscriptionID
	WHERE ps.Email = @Email

END