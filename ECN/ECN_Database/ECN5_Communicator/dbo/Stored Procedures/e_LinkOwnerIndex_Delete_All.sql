CREATE  PROC [dbo].[e_LinkOwnerIndex_Delete_All] 
(
	@UserID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update LinkOwnerIndex SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE CustomerID = @CustomerID
END
