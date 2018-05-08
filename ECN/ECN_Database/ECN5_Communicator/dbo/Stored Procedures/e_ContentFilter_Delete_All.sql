CREATE  PROC [dbo].[e_ContentFilter_Delete_All] 
(
	@ContentID int,
    @CustomerID int,
    @UserID int
)
AS 
BEGIN
	UPDATE cf SET cf.IsDeleted = 1, cf.UpdatedDate = GETDATE(), cf.UpdatedUserID = @UserID
	FROM ContentFilter cf
		JOIN [Groups] g WITH (NOLOCK) ON cf.GroupID = g.GroupID
	WHERE g.CustomerID = @CustomerID AND cf.ContentID=@ContentID
END