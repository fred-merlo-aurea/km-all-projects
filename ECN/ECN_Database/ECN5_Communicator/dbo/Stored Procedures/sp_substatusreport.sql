CREATE proc [dbo].[sp_substatusreport]
(
	@groupID int,	
	@month int,
	@year int
)
as/*

declare @groupID int,
		@month int,
		@year int

set @groupID = 14092
set @month = 5
set @year = 2009
*/
Begin
	
	set NOCOUNT ON

	declare @enddate varchar(20)

	set @enddate = convert(varchar(10),dateadd(dd, -1, dateadd(mm,1, convert(datetime,convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year)))),101) + ' 23:59:59'

			
	create table #tmpreport (emailID int, groupID int, entryID varchar(40), SubType varchar(20), ModifiedDate datetime)

	insert into #tmpreport
	select emailID, groupID, entryID, subtype, Modifieddate  from 
	(
		select	distinct emailID, gdf.groupID, entryID, 
				max(case when shortname='amountpaid' then datavalue end) as amountpaid,
				max(case when shortname='subtype' then datavalue end) as subtype,
				max(case when shortname='subtype' then edv.modifieddate end) as Modifieddate
		from	emaildatavalues edv join 
				groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID 
		where 
				gdf.groupID = @groupID 
				and entryID is not null and 
				shortname in ('amountpaid','subtype') and
				edv.ModifiedDate <= @enddate
		group by emailID, gdf.groupID, entryID
	) inn
	where isnull(convert(decimal,amountpaid),0) > 0

	select subtype, convert(datetime, convert(varchar,month(modifieddate))  +'/01/' +  convert(varchar,year(modifieddate))) as subdate,  count(emailID) as counts 
	from #tmpreport 
	group by subtype, convert(datetime,convert(varchar,month(modifieddate)) + '/01/' +  convert(varchar,year(modifieddate))) order by 3, 2 

	drop table #tmpreport

end
