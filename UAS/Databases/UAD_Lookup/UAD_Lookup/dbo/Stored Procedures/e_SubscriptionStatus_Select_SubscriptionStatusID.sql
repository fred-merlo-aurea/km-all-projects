CREATE PROCEDURE e_SubscriptionStatus_Select_SubscriptionStatusID
@SubscriptionStatusID int
AS    
BEGIN

	set nocount on

	SELECT ss.*
	FROM SubscriptionStatus ss With(NoLock)
	WHERE ss.SubscriptionStatusID = @SubscriptionStatusID

END