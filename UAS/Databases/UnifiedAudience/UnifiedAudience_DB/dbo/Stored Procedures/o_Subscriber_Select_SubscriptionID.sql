CREATE PROCEDURE [dbo].[o_Subscriber_Select_SubscriptionID]
@SubscriptionID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM Subscriptions With(NoLock)
	WHERE SubscriptionID = @SubscriptionID

END
GO