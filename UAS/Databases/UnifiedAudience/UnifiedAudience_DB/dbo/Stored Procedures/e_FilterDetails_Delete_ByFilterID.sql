CREATE PROCEDURE [dbo].[e_FilterDetails_Delete_ByFilterID]
@FilterID int
as
BEGIN

	SET NOCOUNT ON

	Delete 
	from FilterDetails 
	where FilterGroupID in (Select fg.FilterGroupID from FilterGroup fg where FilterID = @FilterID)

End