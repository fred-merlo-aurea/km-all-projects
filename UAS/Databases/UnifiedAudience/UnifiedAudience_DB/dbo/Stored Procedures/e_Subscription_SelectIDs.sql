CREATE PROCEDURE [dbo].[e_Subscription_SelectIDs]
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT SubscriptionID
	FROM Subscriptions With(NoLock)

END