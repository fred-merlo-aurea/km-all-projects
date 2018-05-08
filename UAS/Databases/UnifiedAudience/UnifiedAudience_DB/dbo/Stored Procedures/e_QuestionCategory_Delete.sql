CREATE PROCEDURE [dbo].[e_QuestionCategory_Delete]
@QuestionCategoryID int,
@UserID int
as
BEGIN

	SET NOCOUNT ON

	Update QuestionCategory 
	set IsDeleted = 1,
		UpdatedDate = GETDATE(),
		UpdatedUserID = @UserID  
	where QuestionCategoryID = @QuestionCategoryID

End