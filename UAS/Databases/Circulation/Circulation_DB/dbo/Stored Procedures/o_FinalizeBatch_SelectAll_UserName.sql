CREATE PROCEDURE [dbo].[o_FinalizeBatch_SelectAll_UserName]
@UserID int
AS

	SELECT b.BatchID,pub.ClientID,pub.ClientName,b.PublicationID as ProductID,p.PublicationName, p.PublicationCode, Max(h.BatchCountItem) as LastCount, u.FirstName + ' ' + u.LastName as UserName, b.DateCreated, b.DateFinalized,b.BatchNumber
		FROM Batch b With(NoLock)
		JOIN Publication p ON b.PublicationID = p.PublicationID
		JOIN [Publisher] pp With(NoLock) ON p.PublisherID = pp.PublisherID
        JOIN UAS..Client pub with(nolock) on pp.ClientID = pub.ClientID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
		JOIN UAS..[User] u With(NoLock) ON u.UserID = b.UserID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		LEFT JOIN UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
		LEFT JOIN UAS..CodeType ct ON ct.CodeTypeName = 'User Log'
		LEFT JOIN UAS..Code c ON c.CodeTypeId = ct.CodeTypeDescription 
		WHERE b.UserID = @UserID
	Group By b.BatchID, pub.ClientID, pub.ClientName, b.PublicationID, p.PublicationName, p.PublicationCode, u.FirstName, u.LastName, b.DateCreated, b.DateFinalized,b.BatchNumber
