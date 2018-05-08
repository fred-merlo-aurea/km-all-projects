CREATE PROCEDURE dbo.Dashboard_CumulativeGrowth
(
	@entityName varchar(50),
	@Type varchar(20),
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
    
	if @brandID = 0
	Begin
		select	y.Month, 
				y.Year,
				isnull([Counts]  ,0) as Counts
			from 
					@YearMonths y left outer join 
					dbo.[Summary_Data] s with (NOLOCK) on s.[Datemonth]  = y.Month and s.DateYear = y.Year
					and  entityName= @entityName and Type= @Type and isnull(BrandID,0) = 0
			order by YEAR asc, MONTH asc
	End
	Else
	Begin
		select	y.Month, 
				y.Year,
				isnull([Counts]  ,0) as Counts
			from 
					@YearMonths y left outer join 
					dbo.[Summary_Data] s with (NOLOCK) on s.[Datemonth]  = y.Month and s.DateYear = y.Year
					and  entityName= @entityName and Type= @Type and BrandID = @brandID
			order by YEAR asc, MONTH asc
	End

END
GO