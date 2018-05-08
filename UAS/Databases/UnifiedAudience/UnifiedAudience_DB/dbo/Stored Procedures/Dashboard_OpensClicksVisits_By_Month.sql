-- Procedure
CREATE PROCEDURE [dbo].[Dashboard_OpensClicksVisits_By_Month]
(
	@startmonth int = 0,
	@startyear int = 0,
	@endmonth int = 0,
	@endyear int = 0,
	@brandID int = 0
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @startdate datetime,
			@enddate datetime
			
	declare @YearMonths table([Year] int,[Month] int)
	
	if @startmonth = 0
	Begin
		set @startmonth = Month(DateAdd(MONTH, -12, getdate()) )
	End
	
	if @startyear = 0
	Begin
		set @startyear = Year(DateAdd(MONTH, -12, getdate()) )
	End
	
	if @endmonth = 0
	Begin
		set @endmonth = Month(getdate())
	End
	
	if @endyear = 0
	Begin
		set @endyear = Year(getdate())
	End
	
	set @startdate = convert(date,  convert(varchar(10),@startmonth) + '/01/' +  convert(varchar(10),@startyear))
	set @enddate = dateadd(Day, -1, dateadd(Month, 1, convert(date, convert(varchar(10),@endmonth) + '/01/' +  convert(varchar(10),@endyear))))
	
	while (@StartDate < @EndDate)
    begin
		insert into @YearMonths
		select YEAR(@StartDate), MONTH(@StartDate)  
		
		set @StartDate = DATEADD(month, 1, @StartDate)
    end
	
	 select  
		 Month, Year,
		sum(case when type='open' then Counts end) as totalOpenCounts,
		sum(case when type='click' then Counts end) as totalClickCounts,
		sum(case when type='visit' then Counts end) as totalVisitCounts
	from 
	(
		select 'click' as type, 
			y.Month, 
			y.Year,
			SUM([Counts]) as Counts  
		from 
				@YearMonths y left outer join 
				dbo.[Summary_Data] s with (NOLOCK) on s.[Datemonth]  = y.Month and s.DateYear = y.Year and entityname = 'SubscriberClickActivity' and [Type] = 'New' and isnull(brandID, 0) = @brandID
		group by 
				y.Month, 
				y.Year 
		union 
		select 'open' as type, 
			y.Month, 
			y.Year,
			SUM([Counts]) as Counts  
		from 
				@YearMonths y left outer join 
				dbo.[Summary_Data] s with (NOLOCK) on s.[Datemonth]  = y.Month and s.DateYear = y.Year and  entityname = 'SubscriberOpenActivity' and [Type] = 'New' and isnull(brandID, 0) = @brandID
		group by 
				y.Month, 
				y.Year  
		union 
		select 'Visit' as type, 
			y.Month, 
			y.Year,
			SUM([Counts]) as Counts  
		from 
				@YearMonths y left outer join 
				dbo.[Summary_Data] s with (NOLOCK) on s.[Datemonth]  = y.Month and s.DateYear = y.Year and entityname = 'SubscriberVisitActivity' and [Type] = 'New' and isnull(brandID, 0) = @brandID
		group by 
				y.Month, 
				y.Year 		          
		
	) inn
	group by                               
			   Month, Year
	order by 2,1,3
END
GO