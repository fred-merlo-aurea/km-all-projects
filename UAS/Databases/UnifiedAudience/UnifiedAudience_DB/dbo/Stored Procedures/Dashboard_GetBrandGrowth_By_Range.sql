
CREATE PROC Dashboard_GetBrandGrowth_By_Range
(
	@startdate date = '',
	@endDate date = ''
	
)
as
Begin
	SET NOCOUNT ON;
	
	if @startdate = null or @startdate = ''
	Begin
		set @startdate = DateAdd(YEAR, -1, getdate()) 
	End
	
	if @endDate = null or @endDate = ''
	Begin
		set @endDate = GETDATE()
	End
	
	declare @totalcounts bigint

	select @totalcounts = ISNULL(SUM([Counts]), 0)
	FROM Brand b  with (NOLOCK)  join
			BrandDetails bd  with (NOLOCK)  on b.BrandID = bd.BrandID  join
			dbo.[Summary_Data] s with (NOLOCK)on 
			s.PubID = bd.PubID and entityName='subscriptions' and [Type]='new' and s.Date between @startdate and @endDate

	select  B.BrandID,B.BrandName, isnull(SUM([Counts]),0)  as [Counts],
			convert(decimal(18,2),(convert(decimal(18,2),ISNULL(SUM([Counts]), 0)) * 100) / convert(decimal(18,2),@totalcounts)) as CountsPercentage
	from 
			Brand b  with (NOLOCK)  join
			BrandDetails bd  with (NOLOCK)  on b.BrandID = bd.BrandID  join
			dbo.[Summary_Data] s with (NOLOCK)on 
			s.PubID = bd.PubID and entityName='subscriptions' and [Type]='new' and s.Date between @startdate and @endDate
	group by B.BrandID,BrandName
	order by BrandName
	
End