CREATE PROCEDURE [dbo].[Dashboard_getDimensionCounts_By_PubTypeID]
(
	@PubTypeID int,
	@mastergroupID int,
	@brandID int = 0
)
AS
Begin

	declare @totalcounts bigint

	if @brandID = 0
	Begin
		select @totalcounts = ISNULL(SUM([Counts]), 0)
		from	
				Summary_ProductTypeDimension_Data with (NOLOCK)
		where  
				PubTypeID = @PubTypeID and  
				MasterGroupID = @mastergroupID
	
		select MasterValue, MasterDesc, Counts,
			  convert(decimal(18,2),(convert(decimal(18,2),ISNULL([Counts], 0)) * 100) / convert(decimal(18,2),@totalcounts)) as Percentage
		from	
				Summary_ProductTypeDimension_Data with (NOLOCK)
		where  
				PubTypeID = @PubTypeID and  
				MasterGroupID = @mastergroupID
	End
	Else
	Begin
		select @totalcounts = ISNULL(SUM([Counts]), 0)
		from	
				Summary_PubTypeBrandDimension_Data with (NOLOCK)
		where  
				PubTypeID = @PubTypeID and  
				MasterGroupID = @mastergroupID and
				BrandID = @brandID
	
		select MasterValue, MasterDesc, Counts,
			  convert(decimal(18,2),(convert(decimal(18,2),ISNULL([Counts], 0)) * 100) / convert(decimal(18,2),@totalcounts)) as Percentage
		from	
				Summary_PubTypeBrandDimension_Data with (NOLOCK)
		where  
				PubTypeID = @PubTypeID and  
				MasterGroupID = @mastergroupID and
				BrandID = @brandID

	End
End
GO