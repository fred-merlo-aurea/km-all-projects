CREATE  PROC [dbo].[e_Content_Delete] 
(
	@UserID int,
    @ContentID int,
    @CustomerID int
)
AS 
BEGIN
	Update Content SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE ContentID = @ContentID AND CustomerID = @CustomerID
END