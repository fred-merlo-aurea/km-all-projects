CREATE PROCEDURE e_Publication_Select_Subscriber
@SubscriberID int
AS
	SELECT p.* 
	FROM Publication p With(NoLock) 
	JOIN Subscription s With(NoLock) ON s.PublicationID = p.PublicationID
	WHERE s.SubscriberID = @SubscriberID AND s.IsSubscribed = 'true'
