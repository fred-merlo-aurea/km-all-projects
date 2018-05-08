CREATE PROCEDURE e_Subscription_Select_SubscriberID_PublicationID
@SubscriberID int,
@PublicationID int
AS
	SELECT * FROM Subscription With(NoLock) WHERE SubscriberID = @SubscriberID AND PublicationID = @PublicationID
