CREATE PROCEDURE [dbo].[e_FilterCategory_Delete]
@FilterCategoryID int,
@UserID int
as
BEGIN

	SET NOCOUNT ON

	Update FilterCategory 
	set IsDeleted = 1, 
		UpdatedDate = GETDATE(), 
		UpdatedUserID = @UserID  
	where FilterCategoryID = @FilterCategoryID
End
