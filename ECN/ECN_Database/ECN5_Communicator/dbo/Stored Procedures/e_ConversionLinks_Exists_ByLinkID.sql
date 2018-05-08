CREATE  PROC [dbo].[e_ConversionLinks_Exists_ByLinkID] 
(
	@LayoutID int = NULL,
	@LinkID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	IF EXISTS (
		SELECT TOP 1 cl.LinkID
		from 
			ConversionLinks cl with (nolock) 
			join Layout l with (nolock) on cl.LayoutID = l.LayoutID
		where 
			l.CustomerID = @CustomerID AND cl.LayoutID = @LayoutID AND cl.LinkID = @LinkID AND cl.IsDeleted = 0 and l.IsDeleted = 0
	) SELECT 1 ELSE SELECT 0
END
