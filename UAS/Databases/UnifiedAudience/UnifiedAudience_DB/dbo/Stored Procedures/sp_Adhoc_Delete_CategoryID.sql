create proc [dbo].[sp_Adhoc_Delete_CategoryID](
@CategoryID int)
as
BEGIN
	
	SET NOCOUNT ON

	delete 
	from adhoc 
	where categoryID = @CategoryID

End