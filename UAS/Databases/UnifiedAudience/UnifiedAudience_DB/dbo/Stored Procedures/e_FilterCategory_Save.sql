CREATE PROCEDURE [dbo].[e_FilterCategory_Save]
@FilterCategoryID int, 
@CategoryName varchar(50),
@ParentID int = 0,
@UserID int
as
BEGIN

	SET NOCOUNT ON

	if (@FilterCategoryID > 0)
		begin	
			Update FilterCategory set 
				CategoryName = @CategoryName, 
				ParentID = @ParentID,
				UpdatedUserID = @UserID, 
				UpdatedDate = GETDATE() 
			where FilterCategoryID = @FilterCategoryID

			select @FilterCategoryID
		End
	else
		Begin
			insert  into FilterCategory (CategoryName, ParentID, CreatedUserID, CreatedDate) 
			values (@CategoryName,  @ParentID, @UserID, GETDATE())

			select @@IDENTITY;
		End	
End