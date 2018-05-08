CREATE PROCEDURE [e_Subscriber_Select_PublicationID_IsSubscribed_IsProspect]
@PublicationID int,
@IsSubscribed bit,
@IsProspect bit
AS
	SELECT s.*,
	PublicationToolTip = ISNULL(substring((SELECT ( ', ' + p.PublicationName)
									   FROM Subscription sub With(NoLock)
									   JOIN Publication p With(NoLock) ON sub.PublicationID = p.PublicationID
									   WHERE  s.SubscriberID = sub.SubscriberID
										   FOR XML PATH( '' )
										  ), 3, 1000 ),-1)
	FROM Subscriber s With(NoLock)
	LEFT JOIN Subscription sub With(NoLock) ON s.SubscriberID = sub.SubscriberID
	LEFT JOIN Prospect p With(NoLock) ON s.SubscriberID = p.SubscriberID
	WHERE sub.IsSubscribed = @IsSubscribed AND p.IsProspect = @IsProspect AND p.PublicationID = @PublicationID
