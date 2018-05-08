CREATE PROCEDURE dbo.e_LandingPageAssignContent_Delete_LPAID   
@LPAID int,
@UserID int

AS
	Update LandingPageAssignContent SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  LPAID    = @LPAID