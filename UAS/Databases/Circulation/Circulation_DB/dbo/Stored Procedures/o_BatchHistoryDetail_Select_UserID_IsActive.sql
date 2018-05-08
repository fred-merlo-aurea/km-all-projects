
CREATE PROCEDURE [dbo].[o_BatchHistoryDetail_Select_UserID_IsActive]
@UserID int,
@IsActive bit
AS

	--SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',b.DateFinalized as 'BatchDateFinalized',
	--	   p.PublicationID,p.PublicationName,p.PublicationCode,
	--	   pub.PublisherID,pub.PublisherName,
	--	   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,h.SubscriptionHistoryID,h.DateCreated as 'HistoryDateCreated',
	--	   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
	--	   t.TaskID,t.TaskName,
	--	   ult.UserLogTypeID,ult.UserLogTypeName,
	--	   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	--FROM Batch b With(NoLock)
	--JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	--JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	--JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	--JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	--LEFT JOIN HistoryMap hm With(NoLock) ON h.HistoryID = hm.HistoryID
	--LEFT JOIN UserLog ul With(NoLock) ON hm.UserLogID = ul.UserLogID
	--LEFT JOIN UserLogType ult With(NoLock) ON ul.UserLogTypeID = ult.UserLogTypeID
	--LEFT JOIN Task t With(NoLock) ON ul.TaskID = t.TaskID 
	--WHERE b.UserID = @UserID AND b.IsActive = @IsActive

	SELECT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   p.PublicationID,p.PublicationName,p.PublicationCode,
		   pub.PublisherID,pub.PublisherName,
		   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   sub.SequenceID,
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated 'UserLogDateCreated'
	FROM Batch b With(NoLock)
	JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
	JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
	JOIN History h With(NoLock) ON b.BatchID = h.BatchID
	JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
	JOIN Subscription sub With(NoLock) On s.SubscriberID = sub.SubscriberID
	LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
	LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
	WHERE b.UserID = @UserID AND b.IsActive = @IsActive and ul.UserLogID != h.HistorySubscriptionID

