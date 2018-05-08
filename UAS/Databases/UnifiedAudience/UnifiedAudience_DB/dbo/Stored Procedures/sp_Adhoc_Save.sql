CREATE proc [dbo].[sp_Adhoc_Save](
@AdhocName varchar(255), 
@CategoryID int,
@SortOrder int
)
as
BEGIN
	
	SET NOCOUNT ON

	insert  into Adhoc (AdhocName, CategoryID, SortOrder) 
	values (@AdhocName, @CategoryID, @SortOrder)

End