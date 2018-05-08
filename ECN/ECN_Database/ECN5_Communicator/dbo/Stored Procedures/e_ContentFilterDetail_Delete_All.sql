CREATE  PROC [dbo].[e_ContentFilterDetail_Delete_All] 
(
	@FilterID int,
    @CustomerID int,
    @UserID int
)
AS 
BEGIN
	UPDATE cfd SET cfd.IsDeleted = 1, cfd.UpdatedDate = GETDATE(), cfd.UpdatedUserID = @UserID
	FROM ContentFilterDetail cfd
		JOIN ContentFilter cf WITH (NOLOCK) ON cfd.FilterID = cf.FilterID
		JOIN [Groups] g WITH (NOLOCK) ON cf.GroupID = g.GroupID
	WHERE g.CustomerID = @CustomerID AND cfd.FilterID=@FilterID
END