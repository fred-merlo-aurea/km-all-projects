CREATE proc [dbo].[rpt_PharmaLive_EffortKey_Report]
(
	@groupID int,
	@startdate varchar(10),
	@enddate varchar(10)
)
as
Begin
	
	declare @gdfID int

	select @gdfID = groupdatafieldsID from groupdatafields where groupID = @groupID and shortname = 'Effort_Code'

	select	DataValue as  EffortKey, convert(varchar,month(modifieddate)) + '/' + Convert(varchar,Year(ModifiedDate)) as MonthYear , count(emaildatavaluesID) as counts
	from	emaildatavalues 
	where	groupdatafieldsID = @gdfID and
			Modifieddate between @startdate and @enddate + ' 23:59:59'
	group by Datavalue, convert(varchar,month(modifieddate)) + '/' + Convert(varchar,Year(ModifiedDate))
	order by Datavalue

End
