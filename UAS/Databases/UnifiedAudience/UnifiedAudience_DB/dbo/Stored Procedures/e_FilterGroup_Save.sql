CREATE proc [dbo].[e_FilterGroup_Save](
@FilterID int,
@SortOrder int
)
as
BEGIN

	SET NOCOUNT ON

	insert into FilterGroup (FilterID, SortOrder) 
	values (@FilterID, @SortOrder)
	select @@IDENTITY

End