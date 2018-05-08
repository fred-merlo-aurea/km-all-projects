

CREATE PROCEDURE [dbo].[o_BatchHistoryDetail_Select_BatchID_Name_Sequence]
@BatchID int,
@Name varchar(500),
@SequenceID int
AS

	DECLARE @Sqlstmt VARCHAR(max)
	DECLARE @WhereStmt VARCHAR(max)
	
	-- Dynamically build the WHERE statement for the query below
	IF (@BatchID) > 0
	BEGIN
		SET @WhereStmt = ' b.BatchID = ' + CAST(@BatchID AS VARCHAR(25)) + ''
	END

	IF LEN(@Name) > 0
	BEGIN
		IF LEN(@WhereStmt) > 0
		BEGIN
			SET @WhereStmt += ' AND (ISNULL(s.FirstName, '''') + '' '' + ISNULL(s.LastName, '''') LIKE ''%'' + '''+ NULLIF(@Name, '') +''' +''%'')'
		END
		ELSE
		BEGIN
			SET @WhereStmt = ' ISNULL(s.FirstName, '''') + '' '' + ISNULL(s.LastName, '''') LIKE ''%'' + '''+ NULLIF(@Name, '') +''' +''%'' '
		END
	END

	IF (@SequenceID) > 0
	BEGIN
		IF LEN(@WhereStmt) > 0
		BEGIN
			SET @WhereStmt += ' AND sub.SequenceID = ' + CAST(@SequenceID AS VARCHAR(25)) + ' '
		END
		ELSE
		BEGIN
			SET @WhereStmt = ' sub.SequenceID = ' + CAST(@SequenceID AS VARCHAR(25)) + ' '
		END
	END
	-- Execute query for results, add @WhereStmt if the LEN is longer than zero
	IF LEN(@WhereStmt) > 0
	BEGIN
	SET @Sqlstmt = 'SELECT DISTINCT b.BatchID,b.UserID,b.BatchCount,b.IsActive,b.DateCreated as ''BatchDateCreated'',
					   b.DateFinalized as ''BatchDateFinalized'',
					   p.PublicationID,p.PublicationName,p.PublicationCode,
					   pub.PublisherID,pub.PublisherName,
					   h.BatchCountItem,h.SubscriberID,h.SubscriptionID,
					   h.HistorySubscriptionID,
					   h.DateCreated as ''HistoryDateCreated'',
					   s.FirstName, s.LastName, s.FirstName + '' '' + s.LastName as ''FullName'',
					   ul.Object,ul.FromObjectValues,ul.ToObjectValues,ul.DateCreated ''UserLogDateCreated'',
					   sub.SequenceID
					FROM Batch b With(NoLock)
					JOIN Publication p With(NoLock) ON b.PublicationID = p.PublicationID
					JOIN Publisher pub With(NoLock) ON p.PublisherID = pub.PublisherID
					JOIN History h With(NoLock) ON b.BatchID = h.BatchID
					JOIN Subscriber s With(NoLock) ON h.SubscriberID = s.SubscriberID
					LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
					LEFT JOIN UAS..UserLog ul With(NoLock) ON htul.UserLogID = ul.UserLogID
					LEFT JOIN Subscription sub With(NoLock) ON s.SubscriberID = sub.SubscriberID
					WHERE ' + @WhereStmt
	END

	EXEC(@Sqlstmt)

