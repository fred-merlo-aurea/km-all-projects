CREATE proc [dbo].[sp_OnOffsbyField]
(
	@groupID int,
	@field varchar(100),
	@startdate date,
	@enddate date,
	@reporttype varchar(10)
)
as
Begin

	declare 
		@groupdatafieldsID int

	create table #emails (emailID int, subscribetypecode varchar(1), dt datetime, Field varchar(255))

	

	insert into #emails 
	
	
	--select	emailID, 
	--		subscribetypecode, 
	--		isnull(lastchanged,createdon), '' 
	--from emailgroups 
	--where	groupID = @groupID and (subscribetypecode='S' or subscribetypecode='U') and
	--	isnull(lastchanged,createdon) between @startdate and @enddate


	select	emailID, 
			subscribetypecode, 
			Case when SubscribeTypeCode = 'S' then CreatedOn else isnull(LastChanged,CreatedOn) end, ''
	from emailgroups 
	where groupID = @groupID and --(subscribetypecode='S' or subscribetypecode='U') and
		(
			( CAST(createdon as date) between @startdate and @enddate AND SubscribeTypeCode = 'S') OR 
			(cast(isnull(lastchanged ,createdon ) as date) between @startdate and @enddate AND SubscribeTypeCode = 'U')
		)

	if len(@field) > 0
	Begin
		if substring(@field,1,3) = 'UDF'
			select @groupdatafieldsID = groupdatafieldsID from groupdatafields where groupID = @groupID and shortname = Replace(@field,'UDF-','')

		if @groupdatafieldsID > 0
		Begin
			update #emails
			set field = datavalue
			from #emails e join emaildatavalues edv on e.emailID = edv.emailID and groupdatafieldsID = @groupdatafieldsID
		end
		else
		Begin
			exec ('update #emails set field = isnull(' + @field + ','''') from #emails es join emails e on es.emailID = e.emailID')
		End
	End
	
	select Field, convert(datetime,convert(varchar,month(dt)) + '/01' + '/' + + convert(varchar,year(dt))) as Months, 
	case when subscribetypecode ='S' then 'on' else 'off' end as subscribetypecode,
			count(emailID) as counts
	from #emails
	where subscribetypecode = (case when @reporttype = 'on' then 'S' when @reporttype = 'off' then 'U' else subscribetypecode end)
	group by Field, convert(datetime,convert(varchar,month(dt)) + '/01' + '/' + + convert(varchar,year(dt))), subscribetypecode

	drop table #emails

End
