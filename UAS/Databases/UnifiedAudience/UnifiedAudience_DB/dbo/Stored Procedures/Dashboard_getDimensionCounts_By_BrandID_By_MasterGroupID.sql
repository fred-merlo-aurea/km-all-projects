CREATE PROCEDURE Dashboard_getDimensionCounts_By_BrandID_By_MasterGroupID
(
	@brandID int,
	@mastergroupID int
)
AS
Begin

	declare @totalcounts bigint

	select @totalcounts = ISNULL(SUM([Counts]), 0)
	from	
			Summary_BrandDimension_Data with (NOLOCK)
	where  
			BrandID = @brandID and  
			MasterGroupID = @mastergroupID
	
	select MasterValue, MasterDesc, Counts,
		  convert(decimal(18,2),(convert(decimal(18,2),ISNULL([Counts], 0)) * 100) / convert(decimal(18,2),@totalcounts)) as Percentage
	from	
			Summary_BrandDimension_Data with (NOLOCK)
	where  
			BrandID = @brandID and  
			MasterGroupID = @mastergroupID

End