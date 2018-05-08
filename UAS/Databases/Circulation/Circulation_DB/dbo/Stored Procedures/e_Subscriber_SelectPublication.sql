CREATE PROCEDURE [e_Subscriber_SelectPublication]
@PublicationID int
AS
SELECT s.*,
PublicationToolTip = ISNULL(substring((SELECT ( ', ' + p.PublicationName)
									   FROM Subscription sub With(NoLock)
									   JOIN Publication p With(NoLock) ON sub.PublicationID = p.PublicationID
									   WHERE  s.SubscriberID = sub.SubscriberID
										   FOR XML PATH( '' )
										  ), 3, 1000 ),-1)
FROM Subscriber s With(NoLock)
JOIN Subscription sub With(NoLock) ON s.SubscriberID = sub.SubscriberID
WHERE sub.PublicationID = @PublicationID
