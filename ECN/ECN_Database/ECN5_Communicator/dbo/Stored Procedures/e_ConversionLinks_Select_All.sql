CREATE  PROC [dbo].[e_ConversionLinks_Select_All] 
(
	@LayoutID int = NULL
)
AS 
BEGIN
	select cl.*, l.CustomerID
	from 
		ConversionLinks cl with (nolock) 
		join Layout l with (nolock) on cl.LayoutID = l.LayoutID
	where 
		cl.LayoutID = @LayoutID AND cl.IsDeleted = 0 and l.IsDeleted = 0
END
