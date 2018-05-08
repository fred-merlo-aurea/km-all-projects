CREATE PROCEDURE [e_Subscriber_Select_PublicationID_IsProspect]
@PublicationID int,
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
	JOIN Prospect p With(NoLock) ON s.SubscriberID = p.SubscriberID
	WHERE p.IsProspect = @IsProspect AND p.PublicationID = @PublicationID
