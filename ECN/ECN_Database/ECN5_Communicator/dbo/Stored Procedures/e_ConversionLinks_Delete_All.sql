CREATE  PROC [dbo].[e_ConversionLinks_Delete_All] 
(
	@LayoutID int = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE cl SET cl.IsDeleted = 1, cl.UpdatedDate = GETDATE(), cl.UpdatedUserID = @UserID
	FROM ConversionLinks cl
		JOIN Layout l WITH (NOLOCK) ON cl.LayoutID = l.LayoutID
	WHERE l.CustomerID = @CustomerID AND cl.LayoutID = @LayoutID
END
