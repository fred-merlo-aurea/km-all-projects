CREATE PROCEDURE [dbo].[e_FilterGroup_Delete_ByFilterID]
@FilterID int
as
BEGIN

	SET NOCOUNT ON

	Delete 
	from FilterGroup 
	where FilterID = @FilterID

End