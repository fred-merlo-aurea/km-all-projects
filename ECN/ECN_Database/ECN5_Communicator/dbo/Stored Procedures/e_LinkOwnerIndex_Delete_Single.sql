CREATE  PROC [dbo].[e_LinkOwnerIndex_Delete_Single] 
(
	@UserID int = NULL,
    @CustomerID int = NULL,
    @LinkOwnerIndexID int = NULL
)
AS 
BEGIN
	Update LinkOwnerIndex SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE CustomerID = @CustomerID AND LinkOwnerIndexID = @LinkOwnerIndexID
END
