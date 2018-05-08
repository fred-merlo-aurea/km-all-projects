

CREATE PROCEDURE [dbo].[o_BatchHistoryDetail_Select_SubscriberID]
@SubscriberID int
AS
	SELECT b.BatchID,b.UserID,b.BatchCount,b.BatchNumber,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   h.PublicationID,'' as PublicationName, '' as PublicationCode,
		   h.PublisherID,'' as PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   c.CodeID,c.DisplayName AS UserLogTypeName,
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	--JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	--JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	LEFT JOIN UAS..Code c on c.CodeId = ul.UserLogTypeID
	LEFT JOIN UAS..CodeType ct on c.CodeTypeId = ct.CodeTypeId
	WHERE s.SubscriberID = @SubscriberID and ul.UserLogID != h.HistorySubscriptionID
