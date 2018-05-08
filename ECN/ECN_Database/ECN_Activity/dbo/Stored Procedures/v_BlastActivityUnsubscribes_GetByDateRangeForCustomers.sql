
CREATE PROCEDURE [dbo].[v_BlastActivityUnsubscribes_GetByDateRangeForCustomers]  
(
	@StartDate varchar(25),-- = '2/13/2013',
	@EndDate varchar(25),-- = '2/13/2013',
	@CustomerIDs varchar(255),-- = '1',
	@UnsubscribeCode varchar(1) = '0'
) AS 
BEGIN
	set @StartDate = @StartDate + ' 00:00:00'
	set @EndDate = @EndDate + ' 23:59:59'
	
	create table #SuppRecords (BlastID int, EmailID int, UnsubscribeTime datetime, GroupID int, EmailSubject varchar(255), EmailFrom varchar(255), CustomerID int)
	
	declare @SQLQuery varchar(max)
	set @SQLQuery = '
	INSERT INTO #SuppRecords
	SELECT 
		bau.BlastID, bau.EmailID, bau.UnsubscribeTime, b.GroupID, b.EmailSubject, b.EmailFrom, b.CustomerID
	FROM 
		BlastActivityUnSubscribes bau WITH (NOLOCK) 
		join ecn5_communicator..Blast b with (nolock) on bau.BlastID = b.BlastID
	WHERE 
		b.CustomerID in (' + @CustomerIDs + ') and
		bau.UnsubscribeTime between ''' + @StartDate + ''' and ''' + @EndDate + ''''
		if @UnsubscribeCode <> '0'
		begin
			set @SQLQuery = @SQLQuery + ' and bau.UnsubscribeCodeID = ' + @UnsubscribeCode
		end
	exec (@SQLQuery)
	
	select '"' + e.EmailAddress + '"' as EmailAddress, '"' + sup.EmailSubject + '"' as EmailSubject, sup.UnsubscribeTime, '"' + g.GroupName + '"' as GroupName, '"' + sup.EmailFrom + '"' as EmailFrom, '"' + c.CustomerName + ' (' + CONVERT(varchar(20),sup.CustomerID) + ')"' as Customer, '"' + IsNull(edv.DataValue, '') + '"' as AccountID, '"' + IsNull(f.FolderName, '') + '"' as FolderName
	from
		#SuppRecords sup
		join ecn5_accounts..Customer c with (nolock) on sup.CustomerID = c.CustomerID
		join ecn5_communicator..Emails e with (nolock) on sup.EmailID = e.EmailID
		join ecn5_communicator..Groups g with (nolock) on sup.GroupID = g.GroupID
		left outer join ECN5_COMMUNICATOR..Folder f with (nolock) on IsNull(g.FolderID, 0) = f.FolderID
		left outer join ecn5_communicator..GroupDatafields gdf with (nolock) on sup.GroupID = gdf.GroupID and gdf.ShortName = 'AccountID'
		left outer join ecn5_communicator..EmailDataValues edv with (nolock) on sup.EmailID = edv.EmailID and gdf.GroupDatafieldsID = edv.GroupDatafieldsID and edv.DataValue is not null
	drop table #SuppRecords
END

--select top 10 * from BlastActivityUnSubscribes where BlastID <> 0 order by UnsubscribeID desc
--select * from ecn5_communicator..Blast where BlastID = 1568907
--select * from ecn5_communicator..GroupDatafields where GroupID = 49195
--select * from ecn5_communicator..EmailDataValues where GroupDatafieldsID in (150596,150930)
