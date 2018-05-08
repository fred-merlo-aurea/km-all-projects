CREATE PROCEDURE [dbo].[e_QuestionCategory_Save]
@QuestionCategoryID int, 
@CategoryName varchar(50),
@ParentID int = 0,
@UserID int
as
BEGIN

	SET NOCOUNT ON

	if (@QuestionCategoryID > 0)
		begin	
			Update QuestionCategory 
			set CategoryName = @CategoryName, 
				ParentID = @ParentID, 
				UpdatedUserID = @UserID, 
				UpdatedDate = GETDATE() 
			where QuestionCategoryID = @QuestionCategoryID

			select @QuestionCategoryID
		End
	else
		Begin
			insert  into QuestionCategory (CategoryName, ParentID, CreatedUserID, CreatedDate)
			values (@CategoryName, @ParentID, @UserID, GETDATE())

			select @@IDENTITY;
		End	

End