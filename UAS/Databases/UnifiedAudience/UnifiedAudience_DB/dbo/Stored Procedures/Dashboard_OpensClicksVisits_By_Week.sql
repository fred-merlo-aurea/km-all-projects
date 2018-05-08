CREATE PROCEDURE [dbo].[Dashboard_OpensClicksVisits_By_Week]
(
	@month int = 0,
	@year int = 0,
	@brandID int = 0
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @startdate date,
			@enddate date
	
	if @month = 0
	Begin
		set @month = Month(getdate())
	End
	
	if @year = 0
	Begin
		set @year = Year(getdate())
	End
	
	set @startdate = convert(date,  convert(varchar(10),@month) + '/01/' +  convert(varchar(10),@year))
	set @enddate = dateadd(Day, -1, dateadd(Month, 1, convert(date, convert(varchar(10),@month) + '/01/' +  convert(varchar(10),@year))))
		
	select  
		DateMonth as Month,
		DateYear as Year,
		WeekPart,
		sum(case when type='open' then Counts end) as totalOpenCounts,
		sum(case when type='click' then Counts end) as totalClickCounts,
		sum(case when type='visit' then Counts end) as totalVisitCounts
	from 
	(
		select 'click' as type, 
			s.DateMonth, 
			s.DateYear,
			DATEPART(WEEK, [Date]) as WeekPart,
			SUM([Counts]) as Counts  
		from 
				dbo.[Summary_Data] s with (NOLOCK)  
		Where
				entityname = 'SubscriberClickActivity' and [Type] = 'New' and
				s.[Date] between @startdate and @enddate  and 
				isnull(brandID, 0) = @brandID
		group by 
				s.DateMonth, 
				s.DateYear,
				DATEPART(WEEK, [Date])
		union 
		select 'open' as type, 
			s.DateMonth, 
			s.DateYear,
			DATEPART(WEEK, [Date]) as WeekPart,
			SUM([Counts]) as Counts  
		from 
				dbo.[Summary_Data] s with (NOLOCK) 
		where
				entityname = 'SubscriberOpenActivity' and [Type] = 'New' and
				s.[Date] between @startdate and @enddate  and 
				isnull(brandID, 0) = @brandID
		group by 
				s.DateMonth, 
				s.DateYear,
				DATEPART(WEEK, [Date])
		union 
		select 'Visit' as type, 
			s.DateMonth, 
			s.DateYear,
			DATEPART(WEEK, [Date]) as WeekPart,
			SUM([Counts]) as Counts  
		from 
				dbo.[Summary_Data] s with (NOLOCK) 
		where
				entityname = 'SubscriberVisitActivity' and [Type] = 'New' and
				s.[Date] between @startdate and @enddate  and 
				isnull(brandID, 0) = @brandID
		group by 
				s.DateMonth, 
				s.DateYear,
				DATEPART(WEEK, [Date]) 		          
		
	) inn
	group by                               
		DateMonth ,
		DateYear,
		WeekPart
	order by 2,1,3
END
GO