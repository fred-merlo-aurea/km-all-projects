CREATE proc [dbo].[sp_Pharmalive_substatusreport]
(
	@groupID varchar(1000),	
	@month int,
	@year int
)
as

Begin
	
	set NOCOUNT ON

	declare @startdate varchar(20),
			@enddate varchar(20)

	set @startdate =  convert(varchar(10),convert(datetime,convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year)),101) + ' 00:00:00'
	set @enddate = convert(varchar(10),dateadd(dd, -1, dateadd(mm,1, convert(datetime,convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year)))),101) + ' 23:59:59'

	--select @startdate, @enddate
			
	select e.emailID, LOWER(e.emailaddress) as emailaddress, g.groupID, g.groupname, guid, Transactiontype as subscriptiontype, case when transactiontype='new' then 1 else 2 end as sortorder, Transactiontype, amount, convert(datetime,inn.DateAdded) as DateAdded, datediff(week,(convert(datetime,inn.DateAdded)-datepart(dd,convert(datetime,inn.DateAdded))+1),convert(datetime,inn.DateAdded))+1 as weekofthemonth from 
		(
			select	distinct emailID, 
					gdf.groupID, 
					entryID,
					entryID as Guid,
					max(case when shortname='subtype' then datavalue end) as Transactiontype,
					max(case when shortname='amountpaid' then datavalue end) as Amount,
					max(case when shortname='subtype' then edv.modifieddate end) as DateAdded,
					(select datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = gdf.groupID and shortname ='paidorfree') as PaidOrFree
			from	
					emaildatavalues edv join 
					groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
			where 
					gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
					entryID is not null and 
					shortname in ('subtype','amountpaid')
			group by 
					emailID, gdf.groupID, entryID
			having 
					max(case when shortname='subtype' then edv.modifieddate end) between @startdate and @enddate
		)	
		inn join 
		emails e on inn.emailID = e.emailID join
		groups g on g.groupID = inn.groupID
	where 
		isnull(convert(decimal,Amount),0) > 0 
	union all
	select e.emailID, LOWER(e.emailaddress) as emailaddress, g.groupID, g.groupname, guid, 'Adjustment' as subscriptiontype, 3, Transactiontype, amount, convert(datetime,inn.DateAdded) as DateAdded, datediff(week,(convert(datetime,inn.DateAdded)-datepart(dd,convert(datetime,inn.DateAdded))+1),convert(datetime,inn.DateAdded))+1 as weekofthemonth from 
		(
			select	distinct emailID, gdf.groupID, entryID,
					max(case when shortname='TransEntryID' then datavalue end) as Guid,
					max(case when shortname='AdjType' then datavalue end) as Transactiontype,
					-1 * max(case when shortname='AdjAmount' then convert(decimal(18,2),isnull(datavalue,0)) end) as Amount,
					max(case when shortname='AdjDate' then datavalue end) as DateAdded
			from	
					emaildatavalues edv join 
					groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
			where 
					gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
					entryID is not null and 
					shortname in ('TransEntryID','AdjType','AdjAmount','AdjDate')
			group by 
					emailID, gdf.groupID, entryID
			having 
					max(case when shortname='AdjDate' then datavalue end) between @startdate and @enddate
		) 
		inn join emails e  on inn.emailID = e.emailID join
		groups g on g.groupID = inn.groupID
	where isnull(convert(decimal,Amount),0) < 0 
	order by groupname, sortorder, dateadded, weekofthemonth

end
