
CREATE PROCEDURE [dbo].[v_Group_GetMSByDateRangeForCustomers]  
(
	@StartDate varchar(25),-- = '6/16/2013',
	@EndDate varchar(25),-- = '6/22/2013',
	@CustomerIDs varchar(255)-- = '3549,3550,3551'
) AS 
BEGIN
	set @StartDate = @StartDate + ' 00:00:00'
	set @EndDate = @EndDate + ' 23:59:59'
	
	create table #SuppRecords (EmailID int, UnsubscribeTime datetime, GroupID int, CustomerID int)
	declare @SQLQuery varchar(max)
	set @SQLQuery = '
	INSERT INTO #SuppRecords
    select eg.EmailID as EmailID, eg.LastChanged as UnsubscribeTime, g.GroupID as GroupID, g.CustomerID as CustomerID
    from 
          ECN5_COMMUNICATOR..EmailGroups eg with (nolock)
          join ECN5_COMMUNICATOR..Groups g with (nolock) on eg.GroupID = g.GroupID
          left outer join ECN_ACTIVITY..BlastActivityUnSubscribes bau with (nolock) on eg.EmailID = bau.EmailID and bau.UnsubscribeCodeID in (1,2,4)
    where 
          g.CustomerID in (' + @CustomerIDs + ') and
          g.MasterSupression = 1 and
          bau.UnsubscribeID is null and
          (eg.LastChanged between ''' + @StartDate + ''' and ''' + @EndDate + ''' or eg.CreatedOn between ''' + @StartDate + ''' and ''' + @EndDate + ''')'
	exec (@SQLQuery)	
	
	select '"' + e.EmailAddress + '"' as EmailAddress, sup.UnsubscribeTime, '"' + g.GroupName + '"' as GroupName, '"' + c.CustomerName + ' (' + CONVERT(varchar(20),sup.CustomerID) + ')"' as Customer
	from
		#SuppRecords sup
		join ecn5_accounts..Customer c with (nolock) on sup.CustomerID = c.CustomerID
		join ecn5_communicator..Emails e with (nolock) on sup.EmailID = e.EmailID
		join ecn5_communicator..Groups g with (nolock) on sup.GroupID = g.GroupID
	drop table #SuppRecords
END

--select top 10 * from BlastActivityUnSubscribes where BlastID <> 0 order by UnsubscribeID desc
--select * from ecn5_communicator..Blast where BlastID = 1568907
--select * from ecn5_communicator..GroupDatafields where GroupID = 49195
--select * from ecn5_communicator..EmailDataValues where GroupDatafieldsID in (150596,150930)

