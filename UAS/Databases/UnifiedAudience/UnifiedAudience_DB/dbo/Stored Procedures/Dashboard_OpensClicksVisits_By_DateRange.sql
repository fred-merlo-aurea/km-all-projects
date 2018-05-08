CREATE PROCEDURE [dbo].[Dashboard_OpensClicksVisits_By_DateRange]
(
	@startdate date = '',
	@enddate date  = '',
	@brandID int = 0
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	if @startdate = null or @startdate = ''
	Begin
		set @startdate = GETDATE() - 7
	End
	
	if @enddate = null or @enddate = ''
	Begin
		set @enddate = GETDATE()
	End
		
	select  
		Date,
		isnull(sum(case when type='open' then Counts end),0) as totalOpenCounts,
		isnull(sum(case when type='click' then Counts end),0) as totalClickCounts,
		isnull(sum(case when type='visit' then Counts end),0) as totalVisitCounts
	from 
	(
		select 'click' as type, 
				s.Date,
				Counts  
		from 
				dbo.[Summary_Data] s with (NOLOCK)  
		Where
				entityname = 'SubscriberClickActivity' and [Type] = 'New' and
				s.[Date] between @startdate and @enddate  and 
				isnull(brandID, 0) = @brandID
		
		union 
		select 'open' as type, 
				s.Date,
				Counts  
		from 
				dbo.[Summary_Data] s with (NOLOCK) 
		where
				entityname = 'SubscriberOpenActivity' and [Type] = 'New' and
				s.[Date] between @startdate and @enddate  and 
				isnull(brandID, 0) = @brandID
		union 
		select 'Visit' as type, 
				s.Date,
				Counts 
		from 
				dbo.[Summary_Data] s with (NOLOCK) 
		where
				entityname = 'SubscriberVisitActivity' and [Type] = 'New' and
				s.[Date] between @startdate and @enddate  and 
				isnull(brandID, 0) = @brandID
		
	) inn
	group by                               
		Date
	order by Date
END
GO