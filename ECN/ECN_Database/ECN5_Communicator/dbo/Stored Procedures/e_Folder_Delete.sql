CREATE  PROC [dbo].[e_Folder_Delete] 
(
	@UserID int = NULL,
    @FolderID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	Update Folder SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE FolderID = @FolderID AND CustomerID = @CustomerID
END
