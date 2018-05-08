CREATE proc [dbo].[v_ListSizeOvertimeReport]
(
	@groupID int,  
	@startdate date,
	@enddate date
)
as
Begin


	with tmp_CTE (MonthCreated, YearCreated, subscribetypecode, Counts)
	as
	(
		select	month(createdon), year(createdon), subscribetypecode, count(EmailGroupID)
		from emailgroups eg
		where groupID = @groupID and (CAST(createdon as date) between @startdate and @enddate) and subscribetypecode in ('M','U','S')
		group by  month(createdon), year(createdon), subscribetypecode
	)
	select  	convert(date, convert(varchar(2),MonthCreated)+'/1'+'/'+convert(varchar(4),YearCreated))as RangeStart,
				DateAdd(dd, -1, DateAdd(mm, 1, convert(date, convert(varchar(2),MonthCreated)+'/1'+'/'+convert(varchar(4),YearCreated)))) as RangeEnd,
				sum(counts) as Added,  
				sum(case when  SubscribeTypeCode='M' then counts else 0 end) as Bounced,
				sum(case when  SubscribeTypeCode='U' then counts else 0 end) as UnSubscribed,  
				sum(case when  SubscribeTypeCode='S' then counts else 0 end) as Active
	 from  
	 tmp_CTE 
	 group by MonthCreated, YearCreated

End