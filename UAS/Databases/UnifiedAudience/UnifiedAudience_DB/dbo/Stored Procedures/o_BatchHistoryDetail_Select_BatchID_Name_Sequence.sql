CREATE PROCEDURE [dbo].[o_BatchHistoryDetail_Select_BatchID_Name_Sequence]
@BatchID int,
@Name varchar(500),
@SequenceID int,
@ClientName varchar(100)
AS
BEGIN

	SET NOCOUNT ON

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
					SET @WhereStmt += ' AND sub.SEQUENCE = ' + CAST(@SequenceID AS VARCHAR(25)) + ' '
				END
			ELSE
				BEGIN
					SET @WhereStmt = ' sub.SEQUENCE = ' + CAST(@SequenceID AS VARCHAR(25)) + ' '
				END
		END
	-- Execute query for results, add @WhereStmt if the LEN is longer than zero
	IF LEN(@WhereStmt) > 0
		BEGIN
			DECLARE @LinkedServer varchar(20) = ''
			IF (NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'KMPlatform'))
			BEGIN
				set @LinkedServer = '[10.10.41.198].'
			END

			SET @Sqlstmt = 'SELECT DISTINCT b.BatchID,b.UserID,b.BatchCount,b.BatchNumber,
						    b.IsActive,b.DateCreated as ''BatchDateCreated'',
							b.DateFinalized as ''BatchDateFinalized'',
							p.PubID,p.PubName,p.PubCode,
							''' + @ClientName + ''' as ''ClientName'',
							h.BatchCountItem,h.PubSubscriptionID,h.SubscriptionID,
							h.HistorySubscriptionID,
							h.DateCreated as ''HistoryDateCreated'',
							s.FirstName, s.LastName, s.FirstName + '' '' + s.LastName as ''FullName'',
							isnull(ul.UserLogTypeID,0) as ''UserLogTypeID'','''' 
						    case when ul.UserLogTypeID is not null then (select codeName from UAD_LOOKUP..Code with(nolock) where codeid = ul.UserLogTypeID) else '''' end as ''UserLogTypeName'',''''
						    isnull(ul.Object,'''') as ''Object'',''''
						    isnull(ul.FromObjectValues,'''') as ''FromObjectValues'',''''
						    isnull(ul.ToObjectValues,'''') as ''ToObjectValues'',''''
						    isnull(ul.DateCreated,'''') as ''UserLogDateCreated'',''''
						    sub.Sequence as ''SequenceID'',''''
						    htul.UserLogID

							FROM Batch b with(nolock)
							JOIN Pubs p with(nolock) ON b.PublicationID = p.PubID
							JOIN History h with(nolock) ON b.BatchID = h.BatchID
							JOIN PubSubscriptions s with(nolock) ON h.PubSubscriptionID = s.PubSubscriptionID
							JOIN Subscriptions sub with(nolock) On s.SubscriptionID = sub.SubscriptionID
							LEFT JOIN HistoryToUserLog htul with(nolock) ON h.HistoryID = htul.HistoryID
							left join ' + @LinkedServer + 'KMPlatform..UserLog ul with(nolock) on htul.UserLogID = ul.UserLogID
							ORDER BY h.DateCreated DESC
							WHERE ' + @WhereStmt
		END

	EXEC(@Sqlstmt)

END