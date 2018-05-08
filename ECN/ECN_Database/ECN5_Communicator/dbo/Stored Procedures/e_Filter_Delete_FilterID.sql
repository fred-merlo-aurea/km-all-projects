CREATE  PROC [dbo].[e_Filter_Delete_FilterID] 
(
	@UserID int = NULL,
    @FilterID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update Filter SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE FilterID = @FilterID AND CustomerID = @CustomerID
END
