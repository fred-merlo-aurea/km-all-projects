CREATE PROCEDURE [dbo].[o_BatchHistoryDetail_Select_SubscriptionID]
@SubscriptionID int,
@ClientName varchar(100)
AS
BEGIN

	SET NOCOUNT ON
	----Start Test Data--------------------------------------
	--DECLARE @ClientName varchar(100) = 'Name', @SubscriptionID int = 1
	----End Test Data----------------------------------------

	/** Grab values from KMPlatform UserLog into Temp Table to prevent joining across linked server **/
	create table #UserLog (UserLogID int, UserLogTypeID int, Object varchar(max), FromObjectValues varchar(max), ToObjectValues varchar(max), DateCreated datetime)	
	CREATE CLUSTERED INDEX IDX_C_UserLog_UserLogID ON #UserLog(UserLogID)
	IF (NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'KMPlatform'))
	BEGIN
		insert into #UserLog
		Select UserLogID, UserLogTypeID, [Object], FromObjectValues, ToObjectValues, DateCreated
			from [10.10.41.198].KMPlatform.dbo.UserLog ul with(nolock)	
			where UserLogID	in 
			(
				Select htul.UserLogID 
				from Batch b With(NoLock)
					JOIN History h With(NoLock) ON b.BatchID = h.BatchID
					LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID			
					where h.SubscriptionID = @SubscriptionID and htul.UserLogID is not null		
			)
	END
	else
	BEGIN
		insert into #UserLog
		Select UserLogID, UserLogTypeID, [Object], FromObjectValues, ToObjectValues, DateCreated
			from KMPlatform.dbo.UserLog ul with(nolock)	
			where UserLogID	in 
			(
				Select htul.UserLogID 
				from Batch b With(NoLock)
					JOIN History h With(NoLock) ON b.BatchID = h.BatchID
					LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID	
					where h.SubscriptionID = @SubscriptionID and htul.UserLogID is not null					
			)
	END

	SELECT b.BatchID,b.UserID,b.BatchCount,b.BatchNumber,
		   b.IsActive,b.DateCreated as 'BatchDateCreated',
		   b.DateFinalized as 'BatchDateFinalized',
		   p.PubID,p.PubName,p.PubCode,
		   @ClientName as 'ClientName',
		   h.BatchCountItem,h.PubSubscriptionID,h.SubscriptionID,
		   h.HistorySubscriptionID,
		   h.DateCreated as 'HistoryDateCreated',
		   s.FirstName, s.LastName, s.FirstName + ' ' + s.LastName as 'FullName',
		   isnull(ul.UserLogTypeID,0) as 'UserLogTypeID', 
		   case when ul.UserLogTypeID is not null then (select codeName from UAD_LOOKUP..Code with(nolock) where codeid = ul.UserLogTypeID) else '' end as 'UserLogTypeName',
		   isnull(ul.Object,'') as 'Object',
		   isnull(ul.FromObjectValues,'') as 'FromObjectValues',
		   isnull(ul.ToObjectValues,'') as 'ToObjectValues',
		   isnull(ul.DateCreated,'') as 'UserLogDateCreated',
		   sub.Sequence as 'SequenceID',
		   htul.UserLogID
	FROM Batch b With(NoLock)
		JOIN Pubs p With(NoLock) ON b.PublicationID = p.PubID
		JOIN History h With(NoLock) ON b.BatchID = h.BatchID
		JOIN PubSubscriptions s With(NoLock) ON h.PubSubscriptionID = s.PubSubscriptionID
		JOIN Subscriptions sub With(NoLock) On s.SubscriptionID = sub.SubscriptionID
		LEFT JOIN HistoryToUserLog htul With(NoLock) ON h.HistoryID = htul.HistoryID
		left join #UserLog ul on htul.UserLogID = ul.UserLogID
	WHERE h.SubscriptionID = @SubscriptionID
	ORDER BY h.DateCreated DESC

	If(OBJECT_ID('tempdb..#UserLog') Is Not Null)
	Begin
		Drop Table #UserLog
	End

END