
CREATE PROCEDURE [dbo].[o_BatchHistoryDetail_Select]
AS
	SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   p.PublicationID,p.PublicationName,p.PublicationCode,
		   pub.PublisherID,pub.PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   --ult.UserLogTypeID,ult.UserLogTypeName,
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	--LEFT JOIN UAS..UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID

