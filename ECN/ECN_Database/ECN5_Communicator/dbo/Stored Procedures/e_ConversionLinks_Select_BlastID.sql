CREATE  PROC [dbo].[e_ConversionLinks_Select_BlastID] 
(
	@BlastID int = NULL
)
AS 
BEGIN
	select cl.*, l.CustomerID
	from 
		ConversionLinks cl with (nolock) 
		join Layout l with (nolock) on cl.LayoutID = l.LayoutID
		JOIN Blast b ON l.LayoutID = b.LayoutID AND b.BlastID = @BlastID
	where 
		cl.IsActive = 'Y' AND 
		cl.IsDeleted = 0 and 
		l.IsDeleted = 0 and 
		b.StatusCode <> 'Deleted'
	ORDER BY SortOrder 
END