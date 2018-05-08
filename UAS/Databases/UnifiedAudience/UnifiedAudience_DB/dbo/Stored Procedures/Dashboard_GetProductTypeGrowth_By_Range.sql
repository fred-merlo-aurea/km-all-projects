CREATE PROC [dbo].[Dashboard_GetProductTypeGrowth_By_Range]
(
	@startdate date = '',
	@endDate date = '',
	@BrandID int = 0
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

	
	if (@brandID = 0)
	Begin
		select  PubTypeDisplayName, isnull(SUM([Counts]),0)  as Counts
		from 
				pubs p with (NOLOCK) join 
				PubTypes pt on p.PubTypeID = pt.PubTypeID join 
				dbo.[Summary_Data] s with (NOLOCK) on 
				s.PubID = p.PubID and entityName='subscriptions' and [Type]='new' and s.Date between @startdate and @endDate
		group by PubTypeDisplayName
		order by PubTypeDisplayName
	end
	else
	Begin
		select  PubTypeDisplayName, isnull(SUM([Counts]),0)  as Counts
		from 
				dbo.[Summary_Data] s with (NOLOCK) join
				pubs p with (NOLOCK) on s.PubID = p.PubID  join 
				PubTypes pt on p.PubTypeID = pt.PubTypeID join 
				BrandDetails bd with (NOLOCK) on bd.PubID = p.pubID
		Where
				entityName='subscriptions' and [Type]='new' and s.Date between @startdate and @endDate and isnull(bd.BrandID,0) = @BrandID
		group by PubTypeDisplayName
		order by PubTypeDisplayName	
	End	
End
GO