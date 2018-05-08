CREATE  PROC [dbo].[e_ConversionLinks_Select_Single] 
(
	@LinkID int = NULL
)
AS 
BEGIN
	select cl.*, l.CustomerID
	from 
		ConversionLinks cl with (nolock) 
		join Layout l with (nolock) on cl.LayoutID = l.LayoutID
	where 
		cl.LinkID = @LinkID AND cl.IsDeleted = 0 and l.IsDeleted = 0
END
