CREATE  PROC [dbo].[e_Layout_Delete] 
(
	@UserID int = NULL,
    @LayoutID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update Layout SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE LayoutID = @LayoutID AND CustomerID = @CustomerID
END
