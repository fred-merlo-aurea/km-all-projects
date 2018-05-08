CREATE  PROC [dbo].[e_ConversionLinks_Delete_Single] 
(
	@LayoutID int = NULL,
    @CustomerID int = NULL,
    @LinkID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE cl SET cl.IsDeleted = 1, cl.UpdatedDate = GETDATE(), cl.UpdatedUserID = @UserID
	FROM ConversionLinks cl
		JOIN Layout l WITH (NOLOCK) ON cl.LayoutID = l.LayoutID
	WHERE l.CustomerID = @CustomerID AND cl.LayoutID = @LayoutID and cl.LinkID = @LinkID
END
