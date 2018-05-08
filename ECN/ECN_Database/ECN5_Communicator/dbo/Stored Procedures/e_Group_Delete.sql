CREATE  PROC [dbo].[e_Group_Delete] 
(
	@UserID int = NULL,
    @GroupID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Delete from Groups WHERE GroupID = @GroupID AND CustomerID = @CustomerID
	--Update [Groups] SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE GroupID = @GroupID AND CustomerID = @CustomerID
END
