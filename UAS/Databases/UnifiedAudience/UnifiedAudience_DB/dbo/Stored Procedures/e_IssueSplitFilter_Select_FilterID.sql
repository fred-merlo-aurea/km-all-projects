CREATE PROCEDURE [dbo].[e_IssueSplitFilter_Select_FilterID]
	@FilterID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM IssueSplitFilter i With(NoLock)
	WHERE i.FilterID = @FilterID

END