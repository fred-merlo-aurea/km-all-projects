CREATE PROCEDURE [dbo].[e_IssueSplitFilterDetails_Select_FilterID]
	@FilterID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM IssueSplitFilterDetails i With(NoLock)
	WHERE i.FilterID = @FilterID

END
GO