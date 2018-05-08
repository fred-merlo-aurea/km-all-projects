create procedure o_FinalizeBatch_Select_StartDate_EndDate
@startDate date,
@endDate date
as
begin

	set nocount on
	----Start Test Data--------------------------------------
	--DECLARE @startDate date = '2017-05-01', @endDate date = '2017-05-30'
	----End Test Data----------------------------------------

	/** Grab values from KMPlatform UserLog into Temp Table to prevent joining across linked server **/
	create table #FinalizeBatchUser (UserID int, UserName varchar(100))	
	CREATE CLUSTERED INDEX IDX_C_FinalizeBatchUser_UserID ON #FinalizeBatchUser(UserID)
	create table #FinalizeBatchClient (ClientID int, DisplayName varchar(100))	
	CREATE CLUSTERED INDEX IDX_C_FinalizeBatchClient_ClientID ON #FinalizeBatchClient(ClientID)
	IF (NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'KMPlatform'))
	BEGIN
		insert into #FinalizeBatchUser (UserID, UserName)
		Select UserID, UserName
			from [10.10.41.198].KMPlatform.dbo.[User] u with(nolock)	
			where UserID in
			(
				Select distinct UserID 
				from Batch b with(Nolock)
				where CAST(b.DateCreated as date) between @startDate and @endDate
			)

		insert into #FinalizeBatchClient (ClientID, DisplayName)
		Select ClientID, DisplayName
			from [10.10.41.198].KMPlatform.dbo.Client c with(nolock)	
			where ClientID in 
			(
				Select distinct p.ClientID 
				from Batch b with(nolock)
				join Pubs p with(nolock) ON b.PublicationID = p.PubID				
				where CAST(b.DateCreated as date) between @startDate and @endDate
			)
	END
	else
	BEGIN
		insert into #FinalizeBatchUser (UserID, UserName)
		Select UserID, UserName
			from KMPlatform.dbo.[User] u with(nolock)	
			where UserID in
			(
				Select distinct UserID 
				from Batch b with(Nolock)
				where CAST(b.DateCreated as date) between @startDate and @endDate
			)

		insert into #FinalizeBatchClient (ClientID, DisplayName)
		Select ClientID, DisplayName
			from KMPlatform.dbo.Client c with(nolock)	
			where ClientID in 
			(
				Select distinct p.ClientID 
				from Batch b with(nolock)
				join Pubs p with(nolock) ON b.PublicationID = p.PubID				
				where CAST(b.DateCreated as date) between @startDate and @endDate
			)
	END

	begin
		select b.BatchID,
				c.ClientID as 'ClientId',
				c.DisplayName as 'ClientName',
				b.PublicationID as 'ProductID',
				p.PubName as 'PublicationName', 
				p.PubCode as 'PublicationCode', 
				Max(h.BatchCountItem) as 'LastCount', 
				u.UserName as 'UserName', 
				b.DateCreated, 
				b.DateFinalized,
				b.BatchNumber, 
				b.UserID
		from Batch b with(nolock)
		join Pubs p with(nolock) on b.PublicationID = p.PubID
		join History h with(nolock) on b.BatchID = h.BatchID
		join #FinalizeBatchUser u ON u.UserID = b.UserID
		join #FinalizeBatchClient c on p.ClientId = c.ClientId
		where CAST(b.DateCreated as date) between @startDate and @endDate
		group by b.BatchID, c.ClientID, c.DisplayName, b.PublicationID, p.PubName, p.PubCode, u.UserName, b.DateCreated, b.DateFinalized,b.BatchNumber, b.UserID
	end

	If(OBJECT_ID('tempdb..#FinalizeBatchUser') Is Not Null)
	Begin
		Drop Table #FinalizeBatchUser
	End
	If(OBJECT_ID('tempdb..#FinalizeBatchClient') Is Not Null)
	Begin
		Drop Table #FinalizeBatchClient
	End

end
go
