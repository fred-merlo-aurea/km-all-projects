CREATE PROCEDURE [dbo].[Dashboard_BrandNewSubscriberTrend_By_Range]
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
	
	Select 
		'b' + convert(varchar(100), x.BrandID) as brandID , x.Month, x.Year, x.MonthYearLabel, isnull(y.Counts,0) as Counts
	from 
		(select BrandID, y.Month, 
				y.Year, 
				Upper(Substring(DateName( month , DateAdd( month , y.Month , 0 ) - 1 ),1,3)) + ', ' + Substring(convert(varchar(4),y.Year),3,2) as MonthYearLabel from @YearMonths y 
				cross join Brand b  with (NOLOCK)  where b.IsDeleted = 0  and (@brandID = 0 or b.brandID = @brandID)
		)x left join 
		(select	b.BrandID, 
				s.[Datemonth], 
				s.DateYear , 
				SUM([Counts]) as Counts  
		from 
			dbo.[Summary_Data] s with (NOLOCK) 
			join BrandDetails bd  with (NOLOCK) on s.PubID = bd.PubID  
			join	Brand b  with (NOLOCK)  on b.BrandID = bd.BrandID and b.IsDeleted = 0 
			where  entityName='pubsubscriptions' and [Type]='new' and (@brandID = 0 or b.brandID = @brandID)
		group by 
				b.BrandID, 
				s.[Datemonth], 
				s.DateYear)y on x.BrandID = y.BrandID and x.Month = y.DateMonth and x.Year = y.DateYear
	order by
		x.BrandID, x.Year, x.Month				
END
GO